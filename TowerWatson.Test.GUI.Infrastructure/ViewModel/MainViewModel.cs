using GalaSoft.MvvmLight.Command;
using System.IO;
using System.Text;
using System.Windows.Input;
using TowerWatson.Test.Contract.Interfaces;
using TowerWatson.Test.Contract.Model;

namespace TowerWatson.Test.GUI.Infrastructure.ViewModel
{
    public class MainViewModel : ToweWatsonBaseViewModel
    {
        private string _claimFileName;
        private FileInfo ClaimFileinfo { get { return new FileInfo(ClaimFileName); } }

        public ICommand ImportCommand { get; set; }
        public string ClaimFileName
        {
            get { return _claimFileName; }
            set
            {
                if (value == _claimFileName) return;
                _claimFileName = value;
                RaisePropertyChanged(() => ClaimFileName);
            }
        }

        private readonly IClaimFileImporterService _iClaimFileImporterService;
        private readonly IClaimFileProcessService _iClaimFileProcessService;
        private readonly ITextFileServices _iTextFileService;

        public MainViewModel(IClaimFileImporterService claimFileImporterService, IClaimFileProcessService claimFileProcessService, ITextFileServices textFileService)
        {
            ImportCommand = new RelayCommand(ImportExecute, ImportCanExecute);

            _iClaimFileImporterService = claimFileImporterService;
            _iClaimFileProcessService = claimFileProcessService;
            _iTextFileService = textFileService;

#if DEBUG
            ClaimFileName = Path.GetFullPath(@"..\..\..\SampleData\ProvidedFile.csv");
#endif
        }
        private void ImportExecute()
        {
            var claimFileRows = _iClaimFileImporterService.Import(ClaimFileName);
            var processResult = _iClaimFileProcessService.Process(claimFileRows);
            WriteToFile(processResult);
        }

        private bool ImportCanExecute()
        {
            return !string.IsNullOrEmpty(ClaimFileName) && File.Exists(ClaimFileName) && ClaimFileinfo.Extension.ToLower() == ".csv";
        }

        private void WriteToFile(ProcessClaimFileResult processResult)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("{0}, {1}", processResult.EarliestOriginYear, processResult.NumberOfDevelopment));
            sb.AppendLine();
            foreach (var row in processResult.AccumulatedTriangle)
            {
                sb.Append(string.Format("{0}, {1}", row.Key, string.Join(",", row.Value)));
                sb.AppendLine();
            }
            var saveFileName = Path.Combine(ClaimFileinfo.DirectoryName, "Result.csv");
            _iTextFileService.Write(saveFileName, sb.ToString());
        }
    }
}