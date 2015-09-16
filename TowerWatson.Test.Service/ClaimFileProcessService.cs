using System.Collections.Generic;
using System.Linq;
using TowerWatson.Test.Contract.Interfaces;
using TowerWatson.Test.Contract.Model;

namespace TowerWatson.Test.Service
{
    public class ClaimFileProcessService : IClaimFileProcessService
    {
        private int _numberOfDevelopment;
        private int _earliestOriginYear;
        private int _lastDevelopmentYear;

        /// <summary>
        /// Process the content of Claim files and change to structure we want for Result file
        /// </summary>
        /// <param name="rows">content of claim file</param>
        /// <returns></returns>
        public ProcessClaimFileResult Process(Dictionary<string, Dictionary<int, List<BaseClaimRowMetaData>>> rows)
        {
            //We can throw Exception here too, depend on our policies
            if (!rows.Any()) return new ProcessClaimFileResult();

            var originYears = rows.SelectMany(r => r.Value.Select(v => v.Key)).Distinct().OrderBy(o => o).ToArray();
            _earliestOriginYear = originYears.First();
            _lastDevelopmentYear = originYears.Last();
            _numberOfDevelopment = _lastDevelopmentYear - _earliestOriginYear + 1;

            var accumulatedTriangle = CalculateAccumulatedTriangle(rows);

            return new ProcessClaimFileResult { EarliestOriginYear = _earliestOriginYear, NumberOfDevelopment = _numberOfDevelopment, AccumulatedTriangle = accumulatedTriangle };
        }

        private Dictionary<string, double[]> CalculateAccumulatedTriangle(Dictionary<string, Dictionary<int, List<BaseClaimRowMetaData>>> rows)
        {
            //On first look we may think that the order of this function is n^4 (number of products * origin years * development years * finding gap
            //<1900 because number of products, origin years and gaps are almost constant value, the real order is n

            var accumulatedTriangle = new Dictionary<string, double[]>();
            foreach (var prodcut in rows)
            {
                double[] accumulatedTrianglValues = new double[CalculateNumberOfTriangularNumbers(_numberOfDevelopment)];
                foreach (var originYear in prodcut.Value)
                {
                    int baseIndex = CalculateNumberOfTriangularNumbers(_numberOfDevelopment) - CalculateNumberOfTriangularNumbers(_numberOfDevelopment - (originYear.Key - _earliestOriginYear));
                    
                    int priYear = originYear.Key;
                    foreach (var baseClaimRowMetaData in originYear.Value)
                    {
                        FillInternalGaps(accumulatedTrianglValues, baseIndex, originYear.Key, ref priYear, baseClaimRowMetaData);

                        int offset = baseClaimRowMetaData.DevelopmentYear - originYear.Key;
                        accumulatedTrianglValues[baseIndex + offset] = 
                            offset == 0 ? baseClaimRowMetaData.IncrementalValue : 
                            accumulatedTrianglValues[baseIndex + offset - 1] + baseClaimRowMetaData.IncrementalValue;
                        priYear = baseClaimRowMetaData.DevelopmentYear;

                    }
                }
                accumulatedTriangle.Add(prodcut.Key, accumulatedTrianglValues);
            }
            return accumulatedTriangle;
        }


        private static void FillInternalGaps(IList<double> accumulatedTrianglValues, int baseIndex, int originYear, ref int priYear, BaseClaimRowMetaData baseClaimRowMetaData)
        {
            while (baseClaimRowMetaData.DevelopmentYear - priYear > 1)
            {
                priYear++;
                int offset = priYear - originYear;
                accumulatedTrianglValues[baseIndex + offset] = accumulatedTrianglValues[baseIndex + offset - 1];
               
            }
        }

        private static int CalculateNumberOfTriangularNumbers(int n)
        {
            return n * (n + 1) / 2;
        }
    }
}