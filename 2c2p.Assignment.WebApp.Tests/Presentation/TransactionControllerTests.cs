using _2c2p.Assignment.Bussiness;
using _2c2p.Assignment.Data.Context;
using _2c2p.Assignment.Data.Model;
using _2c2p.Assignment.Model.Mapping;
using _2c2p.Assignment.Tools.Abstraction.Mapping;
using _2c2p.Assignment.Tools.Abstraction.Validation;
using _2c2p.Assignment.Tools.Impl.Mapping;
using _2c2p.Assignment.Tools.Impl.Validation;
using _2c2p.Assignment.Validation;
using _2c2p.Assignment.WebApp.Controllers;
using _2c2p.Assignment.WebApp.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace _2c2p.Assignment.WebApp.Tests.Bussiness
{
    [TestClass]

    public class TransactionControllerTests
    {
        private readonly string wrongxml = @"<Transactions>
                        <Transaction id=""Inv00001"">
                            <TransactionDate>2019-01-23T13:45:10</TransactionDate>
                            <PaymentDetails>
                                <Amount>200.00</Amount>
                                <CurrencyCode> W R O N G  F I E L D </CurrencyCode>
                            </PaymentDetails>
                            <Status>Done</Status>
                        </Transaction>
                        <Transaction id=""Inv00002"">
                        <TransactionDate>2019-01-24T16:09:15</TransactionDate>
                        <PaymentDetails>
                            <Amount>10000.00</Amount>
                            <CurrencyCode>EUR</CurrencyCode>
                        </PaymentDetails>
                        <Status> A N O T H E R  W R O N G  F I E L D </Status>
                        </Transaction>
                    </Transactions>";


        private readonly string xml = @"<Transactions>
                        <Transaction id=""Inv00001"">
                            <TransactionDate>2019-01-23T13:45:10</TransactionDate>
                            <PaymentDetails>
                                <Amount>200.00</Amount>
                                <CurrencyCode>USD</CurrencyCode>
                            </PaymentDetails>
                            <Status>Done</Status>
                        </Transaction>
                        <Transaction id=""Inv00002"">
                        <TransactionDate>2019-01-24T16:09:15</TransactionDate>
                        <PaymentDetails>
                            <Amount>10000.00</Amount>
                            <CurrencyCode>EUR</CurrencyCode>
                        </PaymentDetails>
                        <Status>Rejected</Status>
                        </Transaction>
                    </Transactions>";

        [TestMethod]
        public  void ShouldReturnBadrequestWhenFileSizeExceeded()
        {
            var formFileConfig = new Mock<IFormFile>();
            var dataContextConfig = new Mock<DataContext>();
            var validationServiceProviderConfig = new Mock<IValidationServiceProvider>();
            var mappingServiceProviderConfig = new Mock<IMappingServiceProvider>();

            var transactionManager = new TransactionManager(null, validationServiceProviderConfig.Object, mappingServiceProviderConfig.Object);
            var controller = new TransactionController( transactionManager); 

            formFileConfig.SetupGet(mockSetup => mockSetup.Length).Returns(1024 * 1024 + 1);
            var result =  controller.Upload(formFileConfig.Object);
            var isExpected = result.GetType() == typeof(BadRequestObjectResult);
            Assert.IsTrue(isExpected);
        }

        [TestMethod]
        public void ShouldReturnBadrequestWhenValidationErrorsOccur()
        {
            var formFileConfig = new Mock<IFormFile>();
            var dataContextConfig = new Mock<DataContext>();
            var mappingServiceProviderConfig = new Mock<IMappingServiceProvider>();

            var validationConfigs = new IValidationConfiguration[] { new TransactionXmlValidationConfiguration(), new TransactionCsvValidationConfiguration() };
            var validationService = new ValidationService(validationConfigs);

            var transactionManager = new TransactionManager(null, validationService, mappingServiceProviderConfig.Object);

            var buffer = Encoding.UTF8.GetBytes(wrongxml);
            var memoryStream = new MemoryStream(buffer);

            formFileConfig.SetupGet(mockSetup => mockSetup.Length).Returns(1024 * 1024);
            formFileConfig.SetupGet(mockSetup => mockSetup.ContentType).Returns("text/xml");
            formFileConfig.SetupGet(mockSetup => mockSetup.FileName).Returns("file.xml");
            formFileConfig.Setup(mockSetup => mockSetup.OpenReadStream()).Returns(memoryStream);

            var controller = new TransactionController( transactionManager);

            var result = controller.Upload(formFileConfig.Object);

            var isExpected = result.GetType() == typeof(BadRequestObjectResult);
            Assert.IsTrue(isExpected);
        }

        [TestMethod]
        public void ShouldReturnBadrequestWhenUploadedFileFormatIsWrong()
        {
            var formFileConfig = new Mock<IFormFile>();
            var dataContextConfig = new Mock<DataContext>();
            var mappingServiceProviderConfig = new Mock<IMappingServiceProvider>();

            var validationConfigs = new IValidationConfiguration[] { new TransactionXmlValidationConfiguration(), new TransactionCsvValidationConfiguration() };
            var validationService = new ValidationService(validationConfigs);

            var transactionManager = new TransactionManager(null, validationService, mappingServiceProviderConfig.Object);

            var buffer = Encoding.UTF8.GetBytes(wrongxml);
            var memoryStream = new MemoryStream(buffer);

            formFileConfig.SetupGet(mockSetup => mockSetup.Length).Returns(1024 * 1024);
            formFileConfig.SetupGet(mockSetup => mockSetup.ContentType).Returns("wrong file content");
            formFileConfig.SetupGet(mockSetup => mockSetup.FileName).Returns("file.xml");
            formFileConfig.Setup(mockSetup => mockSetup.OpenReadStream()).Returns(memoryStream);

            var controller = new TransactionController(transactionManager);

            var result = controller.Upload(formFileConfig.Object) as BadRequestObjectResult;

            var isExpected = result != null && ((List<string>)result.Value).Contains("Unknown format");
            Assert.IsTrue(isExpected);
        }

        [TestMethod]
        public void ShouldReturnBadRequestWhenUploadedFileExtensionIsWrong()
        {
            var formFileConfig = new Mock<IFormFile>();
            var dataContextConfig = new Mock<DataContext>();
            var mappingServiceProviderConfig = new Mock<IMappingServiceProvider>();

            var validationConfigs = new IValidationConfiguration[] { new TransactionXmlValidationConfiguration(), new TransactionCsvValidationConfiguration() };
            var validationService = new ValidationService(validationConfigs);

            var transactionManager = new TransactionManager(null, validationService, mappingServiceProviderConfig.Object);

            var buffer = Encoding.UTF8.GetBytes(wrongxml);
            var memoryStream = new MemoryStream(buffer);

            formFileConfig.SetupGet(mockSetup => mockSetup.Length).Returns(1024 * 1024);
            formFileConfig.SetupGet(mockSetup => mockSetup.ContentType).Returns("text/xml");
            formFileConfig.SetupGet(mockSetup => mockSetup.FileName).Returns("file.with a wrong extension");
            formFileConfig.Setup(mockSetup => mockSetup.OpenReadStream()).Returns(memoryStream);
            
            var controller = new TransactionController(transactionManager);

            var result = controller.Upload(formFileConfig.Object) as BadRequestObjectResult;

            var isExpected = result != null && ((List<string>)result.Value).Contains("Unknown format");
            Assert.IsTrue(isExpected);
        }

        [TestMethod]
        public void ShouldReturnStatusCodeResultWith500WhenExceptionOccurs()
        {
            var formFileConfig = new Mock<IFormFile>();
            var dataContextConfig = new Mock<DataContext>();
            var validationServiceProviderConfig = new Mock<IValidationServiceProvider>();
            var mappingServiceProviderConfig = new Mock<IMappingServiceProvider>();

            formFileConfig.SetupGet(mockSetup => mockSetup.Length).Returns(1024 * 1024);
            formFileConfig.SetupGet(mockSetup => mockSetup.ContentType).Returns("text/xml");
            formFileConfig.SetupGet(mockSetup => mockSetup.FileName).Returns("file.csv");

            var transactionManagerConfig = new Mock<TransactionManager>(null,null,null);
            transactionManagerConfig.Setup(mockSetup => mockSetup.SaveTransactions(It.IsAny<IFormFile>())).Callback(() => throw new Exception("Surprise!"));
            var controller = new TransactionController(transactionManagerConfig.Object);

            var result = controller.Upload(formFileConfig.Object) as StatusCodeResult;
            var isExpected = result != null && result.StatusCode == 500;
            Assert.IsTrue(isExpected);

        }

        [TestMethod]
        public void ShouldReturnOkResultWhenThereIsNoError()
        {
            var formFileConfig = new Mock<IFormFile>();
            var dataContextConfig = new Mock<DataContext>();

            var mappingConfigs = new IMappingConfiguration[] { new CsvToTransactionMappingConfiguration(), new XmlToTransactionMappingConfiguration() };
            var mappingService = new MappingService(mappingConfigs);

            var validationConfigs = new IValidationConfiguration[] { new TransactionXmlValidationConfiguration(), new TransactionCsvValidationConfiguration() };
            var validationService = new ValidationService(validationConfigs);

            var transactions = new List<Transaction>();
            var transactionsQueryable = transactions.AsQueryable();

            var transactionsDbSetMockConfig = new Mock<DbSet<Transaction>>();
            transactionsDbSetMockConfig.As<IQueryable<Transaction>>().Setup(mockSetup => mockSetup.Provider).Returns(transactionsQueryable.Provider);
            transactionsDbSetMockConfig.As<IQueryable<Transaction>>().Setup(mockSetup => mockSetup.Expression).Returns(transactionsQueryable.Expression);
            transactionsDbSetMockConfig.As<IQueryable<Transaction>>().Setup(mockSetup => mockSetup.ElementType).Returns(transactionsQueryable.ElementType);
            transactionsDbSetMockConfig.As<IQueryable<Transaction>>().Setup(mockSetup => mockSetup.GetEnumerator()).Returns(() => transactionsQueryable.GetEnumerator());
            transactionsDbSetMockConfig.Setup(mockSetup => mockSetup.Add(It.IsAny<Transaction>())).Callback<Transaction>(record => transactions.Add(record));
            transactionsDbSetMockConfig.Setup(mockSetup => mockSetup.AddRange(It.IsAny<IEnumerable<Transaction>>())).Callback<IEnumerable<Transaction>>(record => transactions.AddRange(record));

            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "Test")
            .Options;

            var dataContextMockConfig = new Mock<DataContext>(dbContextOptions);
            dataContextMockConfig.SetupGet(mockSetup => mockSetup.Transactions).Returns(() => transactionsDbSetMockConfig.Object);


            var transactionManager = new TransactionManager(dataContextMockConfig.Object, validationService, mappingService);

            var buffer = Encoding.UTF8.GetBytes(xml);
            var memoryStream = new MemoryStream(buffer);

            formFileConfig.SetupGet(mockSetup => mockSetup.Length).Returns(1024 * 1024);
            formFileConfig.SetupGet(mockSetup => mockSetup.ContentType).Returns("text/xml");
            formFileConfig.SetupGet(mockSetup => mockSetup.FileName).Returns("file.xml");
            formFileConfig.Setup(mockSetup => mockSetup.OpenReadStream()).Returns(memoryStream);

            var controller = new TransactionController(transactionManager);

            var result = controller.Upload(formFileConfig.Object);

            var isExpected = result.GetType() == typeof(OkResult);
            Assert.IsTrue(isExpected);
        }
    }
}
