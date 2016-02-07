using System.Windows;
using Microsoft.Win32;
using TowerWatson.Test.Contract.Interfaces;

namespace TowerWatson.Test.Service
{
    public class DialogService : IDialogService
    {

        public bool DialogResult { get; set; }

        public DialogService()
        {
            DialogResult = false;
        }
        public string GetFile(string filter)
        {

            var openFileDialog = new OpenFileDialog
            {
                Filter = filter,
                Multiselect = false
            };

            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != null && dialogResult.Value)
            {
                DialogResult = true;
                return openFileDialog.FileName;
            }
            DialogResult = false;
            return string.Empty;
        }


    }
}