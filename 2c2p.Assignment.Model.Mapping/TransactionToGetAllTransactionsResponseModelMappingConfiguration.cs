using _2c2p.Assignment.Data.Model;
using _2c2p.Assignment.Dto;
using _2c2p.Assignment.Tools.Abstraction.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2c2p.Assignment.Model.Mapping
{
    public class TransactionToGetAllTransactionsResponseModelMappingConfiguration : IMappingConfiguration
    {
        public void Configure(IMappingServiceProvider mappingServiceProvider)
        {
            mappingServiceProvider.Register(new Func<List<Transaction>, List<GetAllTransactionsResponseModel>>((input) =>
            {
                var result = input.Select(transaction => new GetAllTransactionsResponseModel
                {
                    Id = transaction.TransactionId,
                    Payment = $"{transaction.Amount} {transaction.CurrencyCode}",
                    Status = transaction.Status == TransactionStatus.Values.APPROVED ? "A"
                    : transaction.Status == TransactionStatus.Values.FAILED || transaction.Status == TransactionStatus.Values.REJECTED ? "R"
                    //FINISHED,DONE
                    : "D"
                }).ToList();
                return result;
            }));
        }
    }
}