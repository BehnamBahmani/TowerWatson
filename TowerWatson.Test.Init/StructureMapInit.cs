using StructureMap;
using TowerWatson.Test.Contract.Interfaces;
using TowerWatson.Test.Service;

namespace TowerWatson.Test.Init
{
    public class StructureMapInit
    {
        public static void BootstrapStructureMap()
        {
            ObjectFactory.Initialize(x => x.Scan(scanner =>
            {
                scanner.TheCallingAssembly();

               
                scanner.Assembly("TowerWatson.Test.Service");
                scanner.Assembly("TowerWatson.Test.Contract");
                scanner.WithDefaultConventions();
                x.For(typeof(IClaimFileImporterService)).Use(typeof(ClaimFileImporterService));
                x.For(typeof(ITextFileServices)).Use(typeof(TextFileServices));
            }));

            ObjectFactory.AssertConfigurationIsValid();
        }
    }
}