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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace _2c2p.Assignment.WebApp.Tests.Bussiness
{
    [TestClass]

    public class TransactionManagerGetAllTransactionsTests
    {
        private readonly List<Transaction> _transactions = new List<Transaction>() {
            new Transaction{ Id = 1 , TransactionId = "Inv00001" , Amount = 100, CurrencyCode = ISO4217CurrencyCode.Values.USD,  TransactionDate =  new DateTime(2021,5,8) , Status = TransactionStatus.Values.APPROVED  },
            new Transaction{ Id = 2 , TransactionId = "Inv00002" , Amount = 150, CurrencyCode = ISO4217CurrencyCode.Values.EUR,  TransactionDate =  new DateTime(2021,5,8) , Status = TransactionStatus.Values.DONE },
            new Transaction{ Id = 3 , TransactionId = "Inv00003" , Amount = 1000, CurrencyCode = ISO4217CurrencyCode.Values.USD,  TransactionDate =  new DateTime(2021,5,8) , Status = TransactionStatus.Values.FINISHED},
            new Transaction{ Id = 4 , TransactionId = "Inv00004" , Amount = 2500, CurrencyCode = ISO4217CurrencyCode.Values.USD,  TransactionDate =  new DateTime(2021,5,8) , Status = TransactionStatus.Values.REJECTED  },
            new Transaction{ Id = 5 , TransactionId = "Inv00005" , Amount = 11400, CurrencyCode = ISO4217CurrencyCode.Values.EUR,  TransactionDate =  new DateTime(2021,5,8) , Status = TransactionStatus.Values.APPROVED  },
            new Transaction{ Id = 6 , TransactionId = "Inv00006" , Amount = 700, CurrencyCode = ISO4217CurrencyCode.Values.USD,  TransactionDate =  new DateTime(2021,5,8) , Status = TransactionStatus.Values.APPROVED  },
            new Transaction{ Id = 7 , TransactionId = "Inv00007" , Amount = 17600, CurrencyCode = ISO4217CurrencyCode.Values.USD,  TransactionDate =  new DateTime(2021,5,8) , Status = TransactionStatus.Values.FAILED },
            new Transaction{ Id = 8 , TransactionId = "Inv00008" , Amount = 9700, CurrencyCode = ISO4217CurrencyCode.Values.EUR,  TransactionDate =  new DateTime(2021,5,9) , Status = TransactionStatus.Values.APPROVED  },
            new Transaction{ Id = 9 , TransactionId = "Inv00009" , Amount = 10, CurrencyCode = ISO4217CurrencyCode.Values.EUR,  TransactionDate =  new DateTime(2021,5,9) , Status = TransactionStatus.Values.DONE  },
            new Transaction{ Id = 10 , TransactionId = "Inv00010" , Amount = 500, CurrencyCode = ISO4217CurrencyCode.Values.USD,  TransactionDate =  new DateTime(2021,5,9) , Status = TransactionStatus.Values.DONE  },
            new Transaction{ Id = 11 , TransactionId = "Inv00011" , Amount = 600, CurrencyCode = ISO4217CurrencyCode.Values.USD,  TransactionDate =  new DateTime(2021,5,9) , Status = TransactionStatus.Values.APPROVED  },
            new Transaction{ Id = 12 , TransactionId = "Inv00012" , Amount = 9800, CurrencyCode = ISO4217CurrencyCode.Values.EUR,  TransactionDate =  new DateTime(2021,5,9) , Status = TransactionStatus.Values.REJECTED  },
            new Transaction{ Id = 13 , TransactionId = "Inv00013" , Amount = 4300, CurrencyCode = ISO4217CurrencyCode.Values.USD,  TransactionDate =  new DateTime(2021,5,9) , Status = TransactionStatus.Values.APPROVED  },
            new Transaction{ Id = 14 , TransactionId = "Inv00014" , Amount = 400, CurrencyCode = ISO4217CurrencyCode.Values.EUR,  TransactionDate =  new DateTime(2021,5,10) , Status = TransactionStatus.Values.REJECTED  },
            new Transaction{ Id = 15 , TransactionId = "Inv00015" , Amount = 670, CurrencyCode = ISO4217CurrencyCode.Values.USD,  TransactionDate =  new DateTime(2021,5,11) , Status = TransactionStatus.Values.APPROVED  },
            new Transaction{ Id = 16 , TransactionId = "Inv00016" , Amount = 16500, CurrencyCode = ISO4217CurrencyCode.Values.EUR,  TransactionDate =  new DateTime(2021,5,11) , Status = TransactionStatus.Values.APPROVED  },
            new Transaction{ Id = 17 , TransactionId = "Inv00017" , Amount = 90, CurrencyCode = ISO4217CurrencyCode.Values.USD,  TransactionDate =  new DateTime(2021,5,11) , Status = TransactionStatus.Values.FAILED  },
            new Transaction{ Id = 18 , TransactionId = "Inv00018" , Amount = 10400, CurrencyCode = ISO4217CurrencyCode.Values.EUR,  TransactionDate =  new DateTime(2021,5,11) , Status = TransactionStatus.Values.APPROVED  },
            new Transaction{ Id = 19 , TransactionId = "Inv00019" , Amount = 12300, CurrencyCode = ISO4217CurrencyCode.Values.USD,  TransactionDate =  new DateTime(2021,5,11) , Status = TransactionStatus.Values.FAILED},
            new Transaction{ Id = 20 , TransactionId = "Inv00020" , Amount = 4100, CurrencyCode = ISO4217CurrencyCode.Values.EUR,  TransactionDate =  new DateTime(2021,5,11) , Status = TransactionStatus.Values.APPROVED  },
            new Transaction{ Id = 21 , TransactionId = "Inv00021" , Amount = 1050, CurrencyCode = ISO4217CurrencyCode.Values.USD,  TransactionDate =  new DateTime(2021,5,11) , Status = TransactionStatus.Values.FINISHED  },
            new Transaction{ Id = 22 , TransactionId = "Inv00022" , Amount = 1600, CurrencyCode = ISO4217CurrencyCode.Values.EUR,  TransactionDate =  new DateTime(2021,5,11) , Status = TransactionStatus.Values.APPROVED  },
            new Transaction{ Id = 23 , TransactionId = "Inv00023" , Amount = 1400, CurrencyCode = ISO4217CurrencyCode.Values.EUR,  TransactionDate =  new DateTime(2021,5,12) , Status = TransactionStatus.Values.DONE  },
            new Transaction{ Id = 24 , TransactionId = "Inv00024" , Amount = 10660, CurrencyCode = ISO4217CurrencyCode.Values.EUR,  TransactionDate =  new DateTime(2021,5,12) , Status = TransactionStatus.Values.APPROVED },
            new Transaction{ Id = 25 , TransactionId = "Inv00025" , Amount = 10740, CurrencyCode = ISO4217CurrencyCode.Values.USD,  TransactionDate =  new DateTime(2021,5,12) , Status = TransactionStatus.Values.FINISHED  },
            new Transaction{ Id = 26 , TransactionId = "Inv00026" , Amount = 1040, CurrencyCode = ISO4217CurrencyCode.Values.USD,  TransactionDate =  new DateTime(2021,5,12) , Status = TransactionStatus.Values.REJECTED  },
            new Transaction{ Id = 27 , TransactionId = "Inv00027" , Amount = 1300, CurrencyCode = ISO4217CurrencyCode.Values.EUR,  TransactionDate =  new DateTime(2021,5,12) , Status = TransactionStatus.Values.DONE  },
            new Transaction{ Id = 28 , TransactionId = "Inv00028" , Amount = 6090, CurrencyCode = ISO4217CurrencyCode.Values.USD,  TransactionDate =  new DateTime(2021,5,12) , Status = TransactionStatus.Values.FINISHED }
        };

        [TestMethod]
        public void ShouldReturnTransactionsBetweenTheGivenDateRange()
        {
            var dbQueryResult = new List<Transaction>();

            var dataContextConfig = new Mock<DataContext>();

            var mappingConfigs = new IMappingConfiguration[] { new TransactionToGetAllTransactionsResponseModelMappingConfiguration() };
            var mappingService = new MappingService(mappingConfigs);
            var validationService = new ValidationService();

            var transactionsQueryable = _transactions.AsQueryable();
            var transactionsDbSetMockConfig = new Mock<DbSet<Transaction>>();

            transactionsDbSetMockConfig.As<IQueryable<Transaction>>().Setup(mockSetup => mockSetup.Provider).Returns(new CustomizedQueryProvider(transactionsQueryable.Provider, new Action<string, Expression, EnumerableQuery<Transaction>>((methodName, expression, result) =>
            {
                dbQueryResult = result.ToList();
            })));
            transactionsDbSetMockConfig.As<IQueryable<Transaction>>().Setup(mockSetup => mockSetup.Expression).Returns(transactionsQueryable.Expression);
            transactionsDbSetMockConfig.As<IQueryable<Transaction>>().Setup(mockSetup => mockSetup.ElementType).Returns(transactionsQueryable.ElementType);
            transactionsDbSetMockConfig.As<IQueryable<Transaction>>().Setup(mockSetup => mockSetup.GetEnumerator()).Returns(() => transactionsQueryable.GetEnumerator());

            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "Test")
            .Options;

            var dataContextMockConfig = new Mock<DataContext>(dbContextOptions);
            dataContextMockConfig.SetupGet(mockSetup => mockSetup.Transactions).Returns(() => transactionsDbSetMockConfig.Object);

            var transactionManager = new TransactionManager(dataContextMockConfig.Object, validationService, mappingService);

            var begin = new DateTime(2021, 5, 9);
            var end = new DateTime(2021, 5, 11);

            transactionManager.GetAllTransactionsByDateRange(begin, end);
            Assert.IsTrue(dbQueryResult.All(transaction => transaction.TransactionDate >= begin && transaction.TransactionDate <= end));
        }

        [TestMethod]
        public void ShouldReturnTransactionsWithGivenCurrency()
        {
            var dataContextConfig = new Mock<DataContext>();

            var mappingConfigs = new IMappingConfiguration[] { new TransactionToGetAllTransactionsResponseModelMappingConfiguration() };
            var mappingService = new MappingService(mappingConfigs);
            var validationService = new ValidationService();

            var transactionsQueryable = _transactions.AsQueryable();
            var transactionsDbSetMockConfig = new Mock<DbSet<Transaction>>();

            transactionsDbSetMockConfig.As<IQueryable<Transaction>>().Setup(mockSetup => mockSetup.Provider).Returns(transactionsQueryable.Provider);
            transactionsDbSetMockConfig.As<IQueryable<Transaction>>().Setup(mockSetup => mockSetup.Expression).Returns(transactionsQueryable.Expression);
            transactionsDbSetMockConfig.As<IQueryable<Transaction>>().Setup(mockSetup => mockSetup.ElementType).Returns(transactionsQueryable.ElementType);
            transactionsDbSetMockConfig.As<IQueryable<Transaction>>().Setup(mockSetup => mockSetup.GetEnumerator()).Returns(() => transactionsQueryable.GetEnumerator());

            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "Test")
            .Options;

            var dataContextMockConfig = new Mock<DataContext>(dbContextOptions);
            dataContextMockConfig.SetupGet(mockSetup => mockSetup.Transactions).Returns(() => transactionsDbSetMockConfig.Object);

            var transactionManager = new TransactionManager(dataContextMockConfig.Object, validationService, mappingService);

            var currency = "USD";

            var result = transactionManager.GetAllTransGetAllTransactionsByCurrency(currency);
            Assert.IsTrue(result.All(transaction => transaction.Payment.EndsWith(currency)));
        }

        [TestMethod]
        public void ShouldReturnTransactionsWithGivenStatusCode()
        {
            var dataContextConfig = new Mock<DataContext>();

            var mappingConfigs = new IMappingConfiguration[] { new TransactionToGetAllTransactionsResponseModelMappingConfiguration() };
            var mappingService = new MappingService(mappingConfigs);
            var validationService = new ValidationService();

            var transactionsQueryable = _transactions.AsQueryable();
            var transactionsDbSetMockConfig = new Mock<DbSet<Transaction>>();

            transactionsDbSetMockConfig.As<IQueryable<Transaction>>().Setup(mockSetup => mockSetup.Provider).Returns(transactionsQueryable.Provider);
            transactionsDbSetMockConfig.As<IQueryable<Transaction>>().Setup(mockSetup => mockSetup.Expression).Returns(transactionsQueryable.Expression);
            transactionsDbSetMockConfig.As<IQueryable<Transaction>>().Setup(mockSetup => mockSetup.ElementType).Returns(transactionsQueryable.ElementType);
            transactionsDbSetMockConfig.As<IQueryable<Transaction>>().Setup(mockSetup => mockSetup.GetEnumerator()).Returns(() => transactionsQueryable.GetEnumerator());

            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "Test")
            .Options;

            var dataContextMockConfig = new Mock<DataContext>(dbContextOptions);
            dataContextMockConfig.SetupGet(mockSetup => mockSetup.Transactions).Returns(() => transactionsDbSetMockConfig.Object);

            var transactionManager = new TransactionManager(dataContextMockConfig.Object, validationService, mappingService);

            var status = "D";

            var result = transactionManager.GetAllTransactionsByStatus(status);
            Assert.IsTrue(result.All(transaction => transaction.Status == status));
        }
    }
}
