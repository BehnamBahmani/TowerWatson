namespace TowerWatson.Test.Contract.Exceptions
{
    public class ProductNameIsEmptyException : ClaimImporterException
    {
        public ProductNameIsEmptyException(string line):base(line)
        {
            
        }
    }
}
