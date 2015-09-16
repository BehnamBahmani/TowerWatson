using System.Collections.Generic;
using TowerWatson.Test.Contract.Model;

namespace TowerWatson.Test.Contract.Interfaces
{
    public interface IClaimFileProcessService
    {
        ProcessClaimFileResult Process(Dictionary<string, Dictionary<int, List<BaseClaimRowMetaData>>> rows);
    }
}