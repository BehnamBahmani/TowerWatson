using System.Collections.Generic;

namespace TowerWatson.Test.Contract.Interfaces
{
    public interface ITextFileServices
    {
        void Write(string filename, string text);
        IEnumerable<string> Read(string filename, bool hasHeader = true);
    }
}