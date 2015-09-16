using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TowerWatson.Test.GUI.Common.Messenger;

namespace TowerWatson.Test.GUI.Infrastructure.ViewModel
{
    public class ToweWatsonBaseViewModel : ViewModelBase
    {
        public void OpenWindow(string dialogName)
        {
            AppMessenger.Messenger.Send(new NotificationMessage(dialogName));
        }

        protected void TryThrowBackgroudWorkerError(RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
        {
            if (runWorkerCompletedEventArgs.Error != null)
            {
                throw runWorkerCompletedEventArgs.Error;
            }
        }

        public void SetDialogResultValue(bool value)
        {
            AppMessenger.Messenger.Send(new NotificationMessage<bool>(value, AppMessenger.SetDialogResultValue));
        }

        public void CloseWindowsBoundToMe()
        {
            AppMessenger.Messenger.Send(new NotificationMessage(AppMessenger.CloseWindowsBoundToMe));
        }
    }
}