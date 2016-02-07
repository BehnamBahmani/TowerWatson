using System;
using System.Windows;
using System.Windows.Threading;
using TowerWatson.Test.Init;

namespace TowerWatson.Test.GUI.Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            StructureMapInit.BootstrapStructureMap();
            DispatcherUnhandledException += ApplicationDispatcherUnhandledException;
        }

        private static void ApplicationDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {

            //todo: its better to log the exception 
            //ExceptionLogger.LogExceptionToFile(e);
            var err = e.Exception;

            while (err.InnerException != null)
                err = err.InnerException;

            MessageBox.Show(err.StackTrace, err.Message);

            e.Handled = true;

        }
    }
}