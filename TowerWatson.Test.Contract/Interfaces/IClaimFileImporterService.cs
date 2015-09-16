using System;
using System.Collections.Generic;
using TowerWatson.Test.Contract.Model;

namespace TowerWatson.Test.Contract.Interfaces
{
    public interface IClaimFileImporterService
    {
        List<Exception> Exceptions {  get; }
        Dictionary<string, Dictionary<int, List<BaseClaimRowMetaData>>> Import(string filename);
    }
}