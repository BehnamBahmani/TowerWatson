namespace TowerWatson.Test.Contract.Exceptions
{
    public class IncrementalValueIsLessThanZeroException : ClaimImporterException
    {
        public IncrementalValueIsLessThanZeroException(string line)
            : base(line)
        {

        }
    }
   
}