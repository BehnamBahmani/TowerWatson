using System.Collections.Generic;

namespace TowerWatson.Test.Contract.Model
{
    public class ProcessClaimFileResult
    {
        public int EarliestOriginYear { get; set; }
        public double NumberOfDevelopment { get; set; }
        public Dictionary<string, double[]> AccumulatedTriangle { get; set; }
    }
}