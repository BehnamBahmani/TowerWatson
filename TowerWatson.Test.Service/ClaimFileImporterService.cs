using System;
using System.Collections.Generic;
using System.Linq;
using TowerWatson.Test.Contract.Exceptions;
using TowerWatson.Test.Contract.Interfaces;
using TowerWatson.Test.Contract.Model;

namespace TowerWatson.Test.Service
{
    public class ClaimFileImporterService : IClaimFileImporterService
    {
        public List<Exception> Exceptions { get; private set; }
        public Dictionary<string, Dictionary<int, List<BaseClaimRowMetaData>>> Import(string filename)
        {
            Exceptions = new List<Exception>();
            var textFileServices = new TextFileServices();
            var text = textFileServices.Read(filename);

            return Process(text);
        }
        

        private Dictionary<string, Dictionary<int, List<BaseClaimRowMetaData>>> Process(IEnumerable<string> text)
        {
            var rows = new Dictionary<string, Dictionary<int, List<BaseClaimRowMetaData>>>();
            //  var lines = text.Select(a => a.Split(',')).Where(l => l.Count() == 4);
            foreach (var convertedLine in text.Select(Convert).Where(convertedLine => convertedLine != null))
            {
                var claimMetaData = new BaseClaimRowMetaData { DevelopmentYear = convertedLine.DevelopmentYear, IncrementalValue = convertedLine.IncrementalValue };
                if (!rows.ContainsKey(convertedLine.ProductName))
                {
                    rows.Add(convertedLine.ProductName, new Dictionary<int, List<BaseClaimRowMetaData>> { { convertedLine.OriginYear, new List<BaseClaimRowMetaData> { claimMetaData } } });
                }
                else
                {
                    var product = rows[convertedLine.ProductName];
                    if (!product.ContainsKey(convertedLine.OriginYear))
                        product.Add(convertedLine.OriginYear, new List<BaseClaimRowMetaData> { claimMetaData });
                    else product[convertedLine.OriginYear].Add(claimMetaData);
                }
            }

            return rows;
        }

        private ClaimRow Convert(string line)
        {
            var parts = line.Split(',');
            if (parts.Count() != 4)
            {
                Exceptions.Add(new InvalidNumberOfItemsPerRowException(line));
                return null;
            }

            int originYear;
            int developmentYear;
            double incrementalValue;

            var productName = parts[0];
            int.TryParse(parts[1], out originYear);
            int.TryParse(parts[2], out developmentYear);
            double.TryParse(parts[3], out incrementalValue);

            return CheckValues(line, productName, originYear, developmentYear, incrementalValue) ?
                new ClaimRow { ProductName = productName, OriginYear = originYear, DevelopmentYear = developmentYear, IncrementalValue = incrementalValue } : null;
        }

        private bool CheckValues(string line, string productName, int originYear, int developmentYear, double incrementalValue)
        {
            var exceptions = new List<Exception>();

            if (string.IsNullOrEmpty(productName)) exceptions.Add(new ProductNameIsEmptyException(line));
            if (originYear < 1900) exceptions.Add(new InvalidOriginYearException(line));
            if (developmentYear <1900) exceptions.Add(new InvalidDevelopmentYearException(line));
            if (incrementalValue < 0) exceptions.Add(new IncrementalValueIsLessThanZeroException(line));
            if (developmentYear < originYear) exceptions.Add(new DevelopmentYearIsLessThanOrignException(line));

            Exceptions.AddRange(exceptions);
            return !exceptions.Any();
        }
    }
}