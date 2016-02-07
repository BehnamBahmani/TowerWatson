using System;
using GalaSoft.MvvmLight.Command;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TowerWatson.Test.Contract.Interfaces;
using TowerWatson.Test.Contract.Model;

namespace TowerWatson.Test.GUI.Infrastructure.ViewModel
{
    public class MainViewModel : ToweWatsonBaseViewModel
    {
        private string _claimFileName;
        private string _resultMessage;
        private FileInfo ClaimFileinfo { get { return new FileInfo(ClaimFileName); } }

        public ICommand ImportCommand { get; set; }
        public ICommand SelectFileCommand { get; set; }
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
        public string ResultMessage
        {
            get { return _resultMessage; }
            set
            {
                if (value == _resultMessage) return;
                _resultMessage = value;
                RaisePropertyChanged(() => ResultMessage);
            }
        }

        private readonly IClaimFileImporterService _iClaimFileImporterService;
        private readonly IClaimFileProcessService _iClaimFileProcessService;
        private readonly ITextFileServices _iTextFileService;
        private readonly IDialogService _iDialogService;

        public MainViewModel(IClaimFileImporterService claimFileImporterService, IClaimFileProcessService claimFileProcessService, ITextFileServices textFileService, IDialogService dialogService)
        {
            ImportCommand = new RelayCommand(ImportExecute, ImportCanExecute);
            SelectFileCommand = new RelayCommand(SelectFileExecute);

            _iClaimFileImporterService = claimFileImporterService;
            _iClaimFileProcessService = claimFileProcessService;
            _iTextFileService = textFileService;
            _iDialogService = dialogService;

#if DEBUG
            ClaimFileName = Path.GetFullPath(@"..\..\..\SampleData\ProvidedFile.csv");
#endif
        }

        private void ImportExecute()
        {

            var claimFileRows = _iClaimFileImporterService.Import(ClaimFileName);
            if (_iClaimFileImporterService.Exceptions.Any())
            {
                //todo: Exception handeling
                ResultMessage = _iClaimFileImporterService.Exceptions.First().Message;
            }
            else
            {
                var processResult = _iClaimFileProcessService.Process(claimFileRows);
                if (processResult.Exception != null)
                {
                    //todo: Exception handeling
                    ResultMessage = processResult.Exception.Message;
                }
                else
                    WriteToFile(processResult);
            }
        }
        private bool ImportCanExecute()
        {
            return !string.IsNullOrEmpty(ClaimFileName) && File.Exists(ClaimFileName) && ClaimFileinfo.Extension.ToLower() == ".csv";
        }
        private void SelectFileExecute()
        {
            ClaimFileName = _iDialogService.GetFile("Comma Seperated Files|*.csv");
        }

        private void WriteToFile(ProcessClaimFileResult processResult)
        {
            var sb = new StringBuilder();
            sb.Append(string.Format("{0}, {1}", processResult.EarliestOriginYear, processResult.NumberOfDevelopment));
            sb.AppendLine();
            if (processResult.AccumulatedTriangle != null)
                foreach (var row in processResult.AccumulatedTriangle)
                {
                    sb.Append(string.Format("{0}, {1}", row.Key, string.Join(",", row.Value)));
                    sb.AppendLine();
                }
            if (sb.Length == 0)
                ResultMessage = "Could not extract any data from the selected file.";
            if (ClaimFileinfo.DirectoryName != null)
            {
                var saveFileName = Path.Combine(ClaimFileinfo.DirectoryName, "Result.csv");
                _iTextFileService.Write(saveFileName, sb.ToString());
                ResultMessage = string.Format("{0} {1} {2}", saveFileName, Environment.NewLine, "contains the result.");
            }
        }
    }
}