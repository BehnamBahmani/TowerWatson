using System.Windows;
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
        }
    }
}