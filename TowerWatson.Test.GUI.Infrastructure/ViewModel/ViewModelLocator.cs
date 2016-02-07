using StructureMap;

namespace TowerWatson.Test.GUI.Infrastructure.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private static MainViewModel _mainViewModel;

        public MainViewModel Main
        {
            get { return _mainViewModel ?? (_mainViewModel = ObjectFactory.GetInstance<MainViewModel>()); }
        }
    }
}