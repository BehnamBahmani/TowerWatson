namespace TowerWatson.Test.GUI.Common.Messenger
{
    public class AppMessenger
    {
        public static string OpenNewProject = "OpenNewProject";

        public static string SetDialogResultValue = "SetDialogResultValue";
        public static string CloseWindowsBoundToMe = "CloseWindowsBoundToMe";
        public static string SendDialogResult = "SendDialogResult";

        public static readonly GalaSoft.MvvmLight.Messaging.Messenger Messenger =
            new GalaSoft.MvvmLight.Messaging.Messenger();
    }
}