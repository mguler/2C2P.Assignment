using _2c2p.Assignment.Data.Model;
using _2c2p.Assignment.Tools.Abstraction.Mapping;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace _2c2p.Assignment.Model.Mapping
{
    public class XmlToTransactionMappingConfiguration : IMappingConfiguration
    {
        public void Configure(IMappingServiceProvider mappingServiceProvider)
        {
            mappingServiceProvider.RegisterByKey<string, List<Transaction>>("text/xml", new Func<string, List<Transaction>>((input) =>
            {
                var result = new List<Transaction>();
                var transactions = XElement.Parse(input);
                transactions.Descendants("Transaction").ToList().ForEach(element =>
                {
                    var transaction = new Transaction
                    {
                        TransactionId = element.Attribute("id").Value,
                        TransactionDate = DateTime.Parse(element.Element("TransactionDate").Value),
                        Status = (TransactionStatus.Values)Enum.Parse(typeof(TransactionStatus.Values), element.Element("Status").Value.ToUpper()),
                        Amount = decimal.Parse(element.Element("PaymentDetails").Element("Amount").Value),
                        CurrencyCode = (ISO4217CurrencyCode.Values)Enum.Parse(typeof(ISO4217CurrencyCode.Values), element.Element("PaymentDetails").Element("CurrencyCode").Value)
                    };
                    result.Add(transaction);
                });

                return result;
            }));
        }
    }
}