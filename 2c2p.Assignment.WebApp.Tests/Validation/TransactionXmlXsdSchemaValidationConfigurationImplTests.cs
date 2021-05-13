using _2c2p.Assignment.Tools.Abstraction.Validation;
using _2c2p.Assignment.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace _2c2p.Assignment.WebApp.Tests.Bussiness
{
    [TestClass]

    public class TransactionXmlXsdSchemaValidationConfigurationImplTests
    {
        private readonly string wrongxml = @"<Transactions>
                        <Transaction id=""Inv000010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"">
                            <TransactionDate>T H I S  N O T  A  D A T E - T I M E  V A L U E</TransactionDate>
                            <PaymentDetails>
                                <Amount>A M O U N T  F I E L D  I S  A L S O  W R O N G!</Amount>
                                <CurrencyCode> W R O N G  F I E L D </CurrencyCode>
                            </PaymentDetails>
                            <Status>D O N A  D O N A  D O N A  D O N A  D O!</Status>
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
        public void ShouldReturnValidationErrors()
        {

            var validationServiceMockConfig = new Mock<IValidationServiceProvider>();
            var validationConfiguration = new TransactionXmlXsdSchemaValidationConfigurationImpl();

            Func<string, IValidationResult> validator = null;

            validationServiceMockConfig.Setup(mockSetup => mockSetup.RegisterByKey(It.IsAny<string>(), It.IsAny<Func<string, IValidationResult>>()))
               .Callback<string, Func<string, IValidationResult>>((name, func) => validator = func);

            validationConfiguration.Configure(validationServiceMockConfig.Object);

            var result = validator.DynamicInvoke(wrongxml) as IValidationResult;
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void ShouldPassTheValidation()
        {

            var validationServiceMockConfig = new Mock<IValidationServiceProvider>();
            var validationConfiguration = new TransactionXmlXsdSchemaValidationConfigurationImpl();

            Func<string, IValidationResult> validator = null;

            validationServiceMockConfig.Setup(mockSetup => mockSetup.RegisterByKey(It.IsAny<string>(), It.IsAny<Func<string, IValidationResult>>()))
               .Callback<string, Func<string, IValidationResult>>((name, func) => validator = func);

            validationConfiguration.Configure(validationServiceMockConfig.Object);

            var result = validator.DynamicInvoke(xml) as IValidationResult;
            Assert.IsTrue(result.IsValid);
        }
    }
}
