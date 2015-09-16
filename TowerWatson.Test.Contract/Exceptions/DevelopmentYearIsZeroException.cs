namespace TowerWatson.Test.Contract.Exceptions
{
    public class InvalidDevelopmentYearException : ClaimImporterException
    {
        public InvalidDevelopmentYearException(string line)
            : base(line)
        {

        }
    }
}