using _2c2p.Assignment.Data.Model;
using _2c2p.Assignment.Tools.Abstraction.Mapping;
using _2c2p.Assignment.Tools.Abstraction.Validation;
using _2c2p.Assignment.Tools.Impl.Extensions;
using _2c2p.Assignment.Tools.Impl.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace _2c2p.Assignment.Validation
{
    public class TransactionCsvValidationConfiguration : IValidationConfiguration
    {
        public void Configure(IValidationServiceProvider validationServiceProvider)
        {
            validationServiceProvider.RegisterByKey("application/vnd.ms-excel", new Func<string, IValidationResult>((input) =>
            {
            var stringReader = new StringReader(input);
            var validationResult = new ValidationResult();

            var transactionLine = "";
            while ((transactionLine = stringReader.ReadLine()) != null)
            {
                var messages = new StringBuilder();
                var transaction = transactionLine.Split(new char[] { ',' });

                if (transaction.Length != 5)
                {
                    messages.Append("| transaction record has missing fields");
                    continue;
                }

                if (transaction[0].Length > 50)
                {
                    messages.Append("| id should not be longer than 50 characters");
                    validationResult.IsValid = false;
                }

                if (!transaction[1].IsMatch("^([1-9][0-9]{0,2})(,[0-9]{3})*\\.[0-9]{2}$"))
                {
                    messages.Append("|Amount should be a decimal value");
                    validationResult.IsValid = false;
                }

                if (!Enum.IsDefined(typeof(ISO4217CurrencyCode.Values), transaction[2]))
                {
                    messages.Append("|CurrencyCode field should be one of ISO 4217 currency codes");
                    validationResult.IsValid = false;
                }


                if (transaction[3].IsMatch("^(0[1-9]|[1-2][0-9]|3[0-1])/(0[1-9]|1[0-2])/20[1-9]{2} ([0-1][0-9]|2[0-4]):([0-5][0-9]|60):([0-5][0-9]|60)$"))
                {
                    messages.Append("| TransactionDate should be in yyyy-MM-ddThh:mm:ss format");
                    validationResult.IsValid = false;
                }

                    var status = new List<string> { "Approved", "Failed", "Finished" };

                    if (!status.Contains(transaction[4]))
                    {
                        messages.Append("|Status field should be one of Approved,Failed,Finished values");
                        validationResult.IsValid = false;
                    }

                    validationResult.Messages.Add($"{transaction[0]} , {transaction[1]} , {transaction[2]} , {transaction[3]} , {transaction[4]} {messages}");
                }
                return validationResult;
            }));
        }
    }
}
