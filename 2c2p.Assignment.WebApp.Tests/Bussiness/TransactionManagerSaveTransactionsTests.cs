using _2c2p.Assignment.Bussiness;
using _2c2p.Assignment.Data.Context;
using _2c2p.Assignment.Data.Model;
using _2c2p.Assignment.Model.Mapping;
using _2c2p.Assignment.Tools.Abstraction.Mapping;
using _2c2p.Assignment.Tools.Abstraction.Validation;
using _2c2p.Assignment.Tools.Impl.Mapping;
using _2c2p.Assignment.Tools.Impl.Validation;
using _2c2p.Assignment.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace _2c2p.Assignment.WebApp.Tests.Bussiness
{
    [TestClass]

    public class TransactionManagerSaveTransactionsTests
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
        public void ShouldReturnFileSizeExceeded()
        {
            var formFileConfig = new Mock<IFormFile>();
            var dataContextConfig = new Mock<DataContext>();
            var validationServiceProviderConfig = new Mock<IValidationServiceProvider>();
            var mappingServiceProviderConfig = new Mock<IMappingServiceProvider>();
            var transactionManager = new TransactionManager(null, validationServiceProviderConfig.Object, mappingServiceProviderConfig.Object);


            formFileConfig.SetupGet(mockSetup => mockSetup.Length).Returns(1024 * 1024 + 1);

            var result = transactionManager.SaveTransactions(formFileConfig.Object);

            Assert.IsTrue(result.Messages.Contains("file size exceeded"));
        }
        [TestMethod]
        public void ShouldReturnFileUnknownFormatWhenUploadAFileWithWronContentType()
        {
            var formFileConfig = new Mock<IFormFile>();
            var dataContextConfig = new Mock<DataContext>();
            var validationServiceProviderConfig = new Mock<IValidationServiceProvider>();
            var mappingServiceProviderConfig = new Mock<IMappingServiceProvider>();
            var transactionManager = new TransactionManager(null, validationServiceProviderConfig.Object, mappingServiceProviderConfig.Object);


            formFileConfig.SetupGet(mockSetup => mockSetup.Length).Returns(1024 * 1024);
            formFileConfig.SetupGet(mockSetup => mockSetup.ContentType).Returns("unknown content type");
            formFileConfig.SetupGet(mockSetup => mockSetup.FileName).Returns("file.xml");

            var result = transactionManager.SaveTransactions(formFileConfig.Object);

            Assert.IsTrue(result.Messages.Contains("Unknown format"));
        }
        [TestMethod]
        public void ShouldReturnFileUnknownFormatWhenUploadAFileWithWronFilenameExtension()
        {
            var formFileConfig = new Mock<IFormFile>();
            var dataContextConfig = new Mock<DataContext>();
            var validationServiceProviderConfig = new Mock<IValidationServiceProvider>();
            var mappingServiceProviderConfig = new Mock<IMappingServiceProvider>();
            var transactionManager = new TransactionManager(null, validationServiceProviderConfig.Object, mappingServiceProviderConfig.Object);


            formFileConfig.SetupGet(mockSetup => mockSetup.Length).Returns(1024 * 1024);
            formFileConfig.SetupGet(mockSetup => mockSetup.ContentType).Returns("text/xml");
            formFileConfig.SetupGet(mockSetup => mockSetup.FileName).Returns("file.csv");

            var result = transactionManager.SaveTransactions(formFileConfig.Object);

            Assert.IsTrue(result.Messages.Contains("Unknown format"));
        }
        [TestMethod]
        public void ShouldReturnValidationErrorsWhenUploadTransactionsWithInvalidData()
        {
            var formFileConfig = new Mock<IFormFile>();
            var dataContextConfig = new Mock<DataContext>();
            var mappingServiceProviderConfig = new Mock<IMappingServiceProvider>();

            var validationConfigs = new IValidationConfiguration[] { new TransactionXmlValidationConfiguration(),new TransactionCsvValidationConfiguration()};
            var validationService = new ValidationService(validationConfigs);

            var transactionManager = new TransactionManager(null, validationService , mappingServiceProviderConfig.Object);

            var buffer = Encoding.UTF8.GetBytes(wrongxml);
            var memoryStream = new MemoryStream(buffer);

            formFileConfig.SetupGet(mockSetup => mockSetup.Length).Returns(1024 * 1024);
            formFileConfig.SetupGet(mockSetup => mockSetup.ContentType).Returns("text/xml");
            formFileConfig.SetupGet(mockSetup => mockSetup.FileName).Returns("file.xml");
            formFileConfig.Setup(mockSetup => mockSetup.OpenReadStream()).Returns(memoryStream);

            var result = transactionManager.SaveTransactions(formFileConfig.Object);
            Assert.IsTrue(result.HasValiationErrors);
        }
        [TestMethod]
        public void ShouldReturnSuccessfull()
        {
            var formFileConfig = new Mock<IFormFile>();
            var dataContextConfig = new Mock<DataContext>();

            var mappingConfigs = new IMappingConfiguration[] { new CsvToTransactionMappingConfiguration(), new XmlToTransactionMappingConfiguration() };
            var mappingService = new MappingService(mappingConfigs);

            var validationConfigs = new IValidationConfiguration[] { new TransactionXmlValidationConfiguration(), new TransactionCsvValidationConfiguration() };
            var validationService = new ValidationService(validationConfigs);

            var buffer = Encoding.UTF8.GetBytes(xml);
            var memoryStream = new MemoryStream(buffer);

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

            formFileConfig.SetupGet(mockSetup => mockSetup.Length).Returns(1024 * 1024);
            formFileConfig.SetupGet(mockSetup => mockSetup.ContentType).Returns("text/xml");
            formFileConfig.SetupGet(mockSetup => mockSetup.FileName).Returns("file.xml");
            formFileConfig.Setup(mockSetup => mockSetup.OpenReadStream()).Returns(memoryStream);

            var transactionManager = new TransactionManager(dataContextMockConfig.Object, validationService, mappingService);

            var result = transactionManager.SaveTransactions(formFileConfig.Object);
            Assert.IsTrue(result.IsSuccessful);
        }
    }
}
