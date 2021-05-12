using _2c2p.Assignment.Data.Model;
using _2c2p.Assignment.Tools.Abstraction.Mapping;
using _2c2p.Assignment.Tools.Abstraction.Validation;
using _2c2p.Assignment.Tools.Impl.Extensions;
using _2c2p.Assignment.Tools.Impl.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace _2c2p.Assignment.Validation
{
    public class TransactionXmlValidationConfiguration : IValidationConfiguration
    {
        public void Configure(IValidationServiceProvider validationServiceProvider)
        {
            validationServiceProvider.RegisterByKey("text/xml", new Func<string, IValidationResult>((input) =>
            {
                var transactions = XElement.Parse(input);
                var validationResult = new ValidationResult();
                transactions.Descendants("Transaction").ToList().ForEach(element=> {

                    var messages = new StringBuilder();

                    var id = element.Attribute("id").Value;
                    var transactionDate = element.Element("TransactionDate").Value;
                    var transactionStatus = element.Element("Status").Value;
                    var amount = "";
                    var currencyCode = "";

                    if (id.Length > 50) 
                    {
                        messages.Append("| id should not be longer than 50 characters");
                        validationResult.IsValid = false;
                    }

                    if (!transactionDate.IsMatch("^20[1-9]{2}\\-(0[1-9]|1[0-2])\\-(0[1-9]|[1-2][0-9]|3[0-1])T([0-1][0-9]|2[0-4]):([0-5][0-9]|60):([0-5][0-9]|60)$"))
                    {
                        messages.Append("| TransactionDate should be in yyyy-MM-ddThh:mm:ss format");
                        validationResult.IsValid = false;
                    }

                    var status = new List<string> { "Approved", "Rejected", "Done" };
                    if (!status.Contains(transactionStatus))
                    {
                        messages.Append("|Status field should be one of Approved,Rejected,Done values");
                        validationResult.IsValid = false;
                    }

                    var paymentDetails = element.Element("PaymentDetails");
                    if (paymentDetails == null)
                    {
                        messages.Append("|Transaction must contain PaymentDetails");
                        validationResult.IsValid = false;
                    }
                    else 
                    {
                        amount = paymentDetails.Element("Amount").Value;
                        currencyCode = paymentDetails.Element("CurrencyCode").Value;

                        if (!amount.IsMatch("^[1-9][0-9]+\\.[0-9]{2}$"))
                        {
                            messages.Append("|Amount should be a decimal value");
                            validationResult.IsValid = false;
                        }

                        if (!Enum.IsDefined(typeof(ISO4217CurrencyCode.Values), currencyCode))
                        {
                            messages.Append("|CurrencyCode field should be one of ISO 4217 currency codes");
                            validationResult.IsValid = false;
                        }                        
                    }
                    validationResult.Messages.Add($"{id} , {transactionDate} , {transactionStatus} , {amount} , {currencyCode} {messages}");
                });
                return validationResult;
            }));
        }
    }
}
