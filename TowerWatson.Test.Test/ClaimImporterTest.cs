
using System.IO;
using System.Linq;
using NUnit.Framework;
using TowerWatson.Test.Contract.Interfaces;

using TowerWatson.Test.Service;

namespace TowerWatson.Test.Test
{
    [TestFixture]
    class ClaimImporterTest
    {

        private IClaimFileImporterService _iClaimFileImporterService;
        private IClaimFileProcessService _iClaimFileProcessService;
        private string _claimFileName;

        [SetUp]
        public void Setup()
        {
            _iClaimFileImporterService = new ClaimFileImporterService();
            _iClaimFileProcessService = new ClaimFileProcessService();
        }

        [Test]
        public void BaseSample()
        {
            _claimFileName = Path.GetFullPath(@"..\..\..\SampleData\ProvidedFile.csv");
            var claimFileRows = _iClaimFileImporterService.Import(_claimFileName);
            var processResult = _iClaimFileProcessService.Process(claimFileRows);

            Assert.AreEqual(processResult.EarliestOriginYear, 1990);
            Assert.AreEqual(processResult.NumberOfDevelopment, 4);
            Assert.AreEqual(processResult.AccumulatedTriangle.Count, 2);
            Assert.AreEqual(processResult.AccumulatedTriangle.First().Key, "Comp");
            Assert.AreEqual(processResult.AccumulatedTriangle.First().Value, new[] { 0, 0, 0, 0, 0, 0, 0, 110, 280, 200 });

            Assert.AreEqual(processResult.AccumulatedTriangle.Last().Key, "Non-Comp");
            Assert.AreEqual(processResult.AccumulatedTriangle.Last().Value, new[] { 45.2, 110, 110, 147, 50, 125, 150, 55, 140, 100 });

        }

        [Test]
        public void MissedOriginYear()
        {
            //Remove 'Comp, 1992, 1992, 110.0' line from Sample file
            _claimFileName = Path.GetFullPath(@"..\..\..\SampleData\MissedOriginYear.csv");
            var claimFileRows = _iClaimFileImporterService.Import(_claimFileName);
            var processResult = _iClaimFileProcessService.Process(claimFileRows);

            Assert.AreEqual(processResult.EarliestOriginYear, 1990);
            Assert.AreEqual(processResult.NumberOfDevelopment, 4);
            Assert.AreEqual(processResult.AccumulatedTriangle.Count, 2);
            Assert.AreEqual(processResult.AccumulatedTriangle.First().Key, "Comp");
            Assert.AreEqual(processResult.AccumulatedTriangle.First().Value, new[] { 0, 0, 0, 0, 0, 0, 0, 0, 170, 200 });

            Assert.AreEqual(processResult.AccumulatedTriangle.Last().Key, "Non-Comp");
            Assert.AreEqual(processResult.AccumulatedTriangle.Last().Value, new[] { 45.2, 110, 110, 147, 50, 125, 150, 55, 140, 100 });

        }

        [Test]
        public void EmptyFile()
        {
            _claimFileName = Path.GetFullPath(@"..\..\..\SampleData\EmptyFile.csv");
            var claimFileRows = _iClaimFileImporterService.Import(_claimFileName);
            var processResult = _iClaimFileProcessService.Process(claimFileRows);

            Assert.AreEqual(processResult.EarliestOriginYear, 0);
            Assert.AreEqual(processResult.NumberOfDevelopment, 0);
            Assert.AreEqual(processResult.AccumulatedTriangle, null);
            Assert.IsNotNull(processResult.Exception);


        }
        [Test]
        public void WrongContentFile()
        {
            _claimFileName = Path.GetFullPath(@"..\..\..\SampleData\WrongContentFile.csv");
            var claimFileRows = _iClaimFileImporterService.Import(_claimFileName);
            var processResult = _iClaimFileProcessService.Process(claimFileRows);

            Assert.AreEqual(processResult.EarliestOriginYear, 0);
            Assert.AreEqual(processResult.NumberOfDevelopment, 0);
            Assert.AreEqual(processResult.AccumulatedTriangle, null);
        }
        [Test]
        public void RowsWithExceptionFile()
        {
            _claimFileName = Path.GetFullPath(@"..\..\..\SampleData\RowsWithExceptionFile.csv");
            _iClaimFileImporterService.Import(_claimFileName);
            Assert.AreEqual(_iClaimFileImporterService.Exceptions.Count, 5);
        }


        
        [ExpectedException(typeof(FileNotFoundException))]
        [Test]
        public void FileNotFoundException()
        {
            _claimFileName = Path.GetFullPath(@"..\..\..\SampleData\FileNotExists.csv");
            _iClaimFileImporterService.Import(_claimFileName);
        }
    }
}
