namespace TowerWatson.Test.Contract.Exceptions
{
    public class DevelopmentYearIsLessThanOrignException : ClaimImporterException
    {
        public DevelopmentYearIsLessThanOrignException(string line)
            : base(line)
        {

        }
    }
}