using _2c2p.Assignment.Data.Context;
using _2c2p.Assignment.Data.Model;
using _2c2p.Assignment.Dto;
using _2c2p.Assignment.Tools.Abstraction.Mapping;
using _2c2p.Assignment.Tools.Abstraction.Validation;
using _2c2p.Assignment.WebApp.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2c2p.Assignment.Bussiness
{
    public class TransactionManager
    {
        private readonly DataContext _dataContext;
        private readonly IValidationServiceProvider _validationService;
        private readonly IMappingServiceProvider _mappingServiceProvider;

        public TransactionManager(DataContext dataContext, IValidationServiceProvider validationService, IMappingServiceProvider mappingServiceProvider)
        {
            _validationService = validationService;
            _mappingServiceProvider = mappingServiceProvider;
            _dataContext = dataContext;
        }

        public virtual TransactionUploadResponseModel SaveTransactions(IFormFile formFile)
        {
            var result = new TransactionUploadResponseModel();

            if ((formFile.Length / 1024f) / 1024f > 1)
            {
                result.Messages.Add("file size exceeded");
                result.IsSuccessful = false;
                return result;
            }

            if (formFile.ContentType != "application/vnd.ms-excel" && formFile.ContentType != "text/xml")
            {
                result.Messages.Add("Unknown format");
                result.IsSuccessful = false;
                return result;
            }

            if (formFile.ContentType == "application/vnd.ms-excel" && !formFile.FileName.ToLower().EndsWith("csv")
                || formFile.ContentType == "text/xml" && !formFile.FileName.ToLower().EndsWith("xml"))
            {
                result.Messages.Add("Unknown format");
                result.IsSuccessful = false;
                return result;
            }

            var stream = formFile.OpenReadStream();
            using (var streamReader = new StreamReader(stream))
            {
                string text = streamReader.ReadToEnd();
                var validationResult = _validationService.Validate(formFile.ContentType, text);

                if (!validationResult.IsValid)
                {
                    result.Messages = validationResult.Messages.ToList();
                    result.IsSuccessful = false;
                    result.HasValiationErrors = true;
                    return result;
                }
                var transactions = _mappingServiceProvider.Map<List<Transaction>>(formFile.ContentType, text);

                _dataContext.Transactions.AddRange(transactions);
                _dataContext.SaveChanges();
            }
            return result;
        }
        public virtual List<GetAllTransactionsResponseModel> GetAllTransGetAllTransactionsByCurrency(string currency)
        {
            var currencyEnumValue = (ISO4217CurrencyCode.Values)Enum.Parse(typeof(ISO4217CurrencyCode.Values),currency);
            var transactions = _dataContext.Transactions.Where(transaction => transaction.CurrencyCode== currencyEnumValue).ToList();
            var result = _mappingServiceProvider.Map<List<GetAllTransactionsResponseModel>>(transactions);
            return result;
        }
        public virtual List<GetAllTransactionsResponseModel> GetAllTransactionsByDateRange(DateTime begin, DateTime end)
        {
            var transactions = _dataContext.Transactions.Where(transaction => transaction.TransactionDate >= begin && transaction.TransactionDate <= end).ToList();
            var result = _mappingServiceProvider.Map<List<GetAllTransactionsResponseModel>>(transactions);
            return result;
        }
        public virtual List<GetAllTransactionsResponseModel> GetAllTransactionsByStatus(string status)
        {
            var transactions = _dataContext.Transactions.Where(transaction => 
            status == "A" && transaction.Status == TransactionStatus.Values.APPROVED
            || status == "R" && (transaction.Status == TransactionStatus.Values.REJECTED || transaction.Status == TransactionStatus.Values.FAILED)
            || status == "D" && (transaction.Status == TransactionStatus.Values.DONE || transaction.Status == TransactionStatus.Values.FINISHED)).ToList();
            var result = _mappingServiceProvider.Map<List<GetAllTransactionsResponseModel>>(transactions);
            return result;
        }

    }
}
