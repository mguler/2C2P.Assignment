using _2c2p.Assignment.Data.Model;
using _2c2p.Assignment.Tools.Abstraction.Mapping;
using _2c2p.Assignment.Tools.Impl.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace _2c2p.Assignment.Model.Mapping
{
    public class CsvToTransactionMappingConfiguration : IMappingConfiguration
    {
        public void Configure(IMappingServiceProvider mappingServiceProvider)
        {
            mappingServiceProvider.RegisterByKey<string, List<Transaction>>("application/vnd.ms-excel", new Func<string, List<Transaction>>((input) =>
            {
                var result = new List<Transaction>();
                var stringReader = new StringReader(input);

            var transactionLine = "";
                while ((transactionLine = stringReader.ReadLine()) != null)
                {
                    var splitted = transactionLine.Matches("(?<=\")(.*?)(?=\")").Where(value => value != ",").ToArray(); 

                    var transaction = new Transaction
                    {
                        TransactionId = splitted[0],
                        Amount = decimal.Parse(splitted[1]),
                        CurrencyCode = (ISO4217CurrencyCode.Values)Enum.Parse(typeof(ISO4217CurrencyCode.Values), splitted[2]),
                        TransactionDate = DateTime.ParseExact(splitted[3], "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture),
                        Status = (TransactionStatus.Values)Enum.Parse(typeof(TransactionStatus.Values), splitted[4].ToUpper()),
                    };
                    result.Add(transaction);
                }

                return result;
            }));
        }
    }
}