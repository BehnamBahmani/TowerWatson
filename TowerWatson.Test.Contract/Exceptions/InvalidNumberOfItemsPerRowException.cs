namespace TowerWatson.Test.Contract.Exceptions
{
    public class InvalidNumberOfItemsPerRowException : ClaimImporterException
    {
        public InvalidNumberOfItemsPerRowException(string line)
            : base(line)
        {
            
        }
    }
}