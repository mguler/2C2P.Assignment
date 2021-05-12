using _2c2p.Assignment.Tools.Abstraction.Validation;
using _2c2p.Assignment.Tools.Impl.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace _2c2p.Assignment.Tools.Tests
{
    [TestClass]
    public class ValidationTest
    {
        [TestMethod]
        public void RegisterValidator()
        {
            var isSuccessfull = false;

            var validationService = new ValidationService();
            validationService.Register<string>(new Func<string, IValidationResult>((input) => {

                isSuccessfull = true;
                return null;

            }));

            validationService.Validate("");
            Assert.IsTrue(isSuccessfull);

        }

        [TestMethod]
        public void ShouldThrowExceptionWhenRegisterValidatorForTypeTwice()
        {
            var validationService = new ValidationService();
            validationService.Register<string>(new Func<string, IValidationResult>((input) => {
                return null;

            }));

            validationService.Validate("");
            Assert.ThrowsException<Exception>(() => {
                validationService.Register<string>(new Func<string, IValidationResult>((input) => {
                    return null;
                }));
            });

        }

    }
}
