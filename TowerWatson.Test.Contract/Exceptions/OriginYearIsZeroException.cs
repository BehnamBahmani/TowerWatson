namespace TowerWatson.Test.Contract.Exceptions
{
    public class InvalidOriginYearException : ClaimImporterException
    {
        public InvalidOriginYearException(string line)
            : base(line)
        {

        }
    }
}