using System.Collections.Generic;
using System.IO;
using System.Linq;
using TowerWatson.Test.Contract.Interfaces;

namespace TowerWatson.Test.Service
{
    public class TextFileServices : ITextFileServices
    {
        public void Write(string filename, string text)
        {
            string directory = Path.GetDirectoryName(filename);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            File.WriteAllText(filename, text);
        }

        public IEnumerable<string> Read(string filename, bool hasHeader = true)
        {
            if (!File.Exists(filename)) throw new FileNotFoundException(filename);
            return File.ReadAllLines(filename).Skip(hasHeader ? 1 : 0);
        }
    }
}