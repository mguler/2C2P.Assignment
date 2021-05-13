using _2c2p.Assignment.Data.Model;
using _2c2p.Assignment.Tools.Abstraction.Mapping;
using _2c2p.Assignment.Tools.Abstraction.Validation;
using _2c2p.Assignment.Tools.Impl.Validation;
using System;
using System.Xml.Linq;
using System.Xml.Schema;

namespace _2c2p.Assignment.Validation
{
    public class TransactionXmlXsdSchemaValidationConfigurationImpl : IValidationConfiguration
    {
        public void Configure(IValidationServiceProvider validationServiceProvider)
        {
            validationServiceProvider.RegisterByKey("text/xml", new Func<string, IValidationResult>((input) =>
            {
                var transactions = XDocument.Parse(input);
                var validationResult = new ValidationResult();

                XmlSchemaSet schema = new XmlSchemaSet();
                schema.Add("", "TransactionsXsd.xsd");
                transactions.Validate(schema, (sender, e) =>
                {
                    XmlSeverityType type = XmlSeverityType.Warning;
                    if (Enum.TryParse<XmlSeverityType>("Error", out type))
                    {
                        if (type == XmlSeverityType.Error) 
                        {
                            validationResult.IsValid = false;
                            validationResult.Messages.Add(e.Message);
                        }
                    }
                });
                return validationResult;
            }));
        }
    }
}