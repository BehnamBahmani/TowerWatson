using System;

namespace TowerWatson.Test.Contract.Exceptions
{
    public class ClaimImporterException : Exception
    {
        public string Line { private get; set; }
        public ClaimImporterException(string line)
        {
            Line = line;
        }
    }
}