using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Services.Interfaces;
using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace TestProject1.Services
{
    public class PaymentServiceTests
    {
        private readonly Mock<IAccountDataStoreFactory> _mockFactory;
        private readonly Mock<IAccountDataStore> _mockDataStore;
        private readonly Mock<ILogger<PaymentService>> _mockLogger;
        private readonly Mock<IPaymentValidatorService> _mockValidator;
        private readonly PaymentService _sut;

        public PaymentServiceTests()
        {
            _mockFactory = new Mock<IAccountDataStoreFactory>();
            _mockDataStore = new Mock<IAccountDataStore>();
            _mockLogger = new Mock<ILogger<PaymentService>>();
            _mockValidator = new Mock<IPaymentValidatorService>();

            _mockFactory.Setup(f => f.GetDataStore()).Returns(_mockDataStore.Object);

            _sut = new PaymentService(_mockFactory.Object, _mockValidator.Object, _mockLogger.Object);
        }

        [Fact]
        public void MakePayment_WithValidBacsPayment_ReturnsSuccess()
        {
            // Arrange
            var account = CreateValidAccount(AllowedPaymentSchemes.Bacs);
            var request = CreateValidRequest(PaymentScheme.Bacs, 100m);

            _mockDataStore.Setup(ds => ds.GetAccount(request.DebtorAccountNumber))
                .Returns(account);
            _mockValidator.Setup(v => v.Validate(account, request))
                .Returns(true);

            // Act
            var result = _sut.MakePayment(request);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public void MakePayment_WithValidFasterPayment_ReturnsSuccess()
        {
            // Arrange
            var account = CreateValidAccount(AllowedPaymentSchemes.FasterPayments);
            var request = CreateValidRequest(PaymentScheme.FasterPayments, 250m);

            _mockDataStore.Setup(ds => ds.GetAccount(request.DebtorAccountNumber))
                .Returns(account);
            _mockValidator.Setup(v => v.Validate(account, request))
                .Returns(true);

            // Act
            var result = _sut.MakePayment(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(750m, account.Balance);
        }

        [Fact]
        public void MakePayment_WithValidChapsPayment_ReturnsSuccess()
        {
            // Arrange
            var account = CreateValidAccount(AllowedPaymentSchemes.Chaps);
            var request = CreateValidRequest(PaymentScheme.Chaps, 500m);

            _mockDataStore.Setup(ds => ds.GetAccount(request.DebtorAccountNumber))
                .Returns(account);
            _mockValidator.Setup(v => v.Validate(account, request))
                .Returns(true);

            // Act
            var result = _sut.MakePayment(request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(500m, account.Balance);
        }

        [Fact]
        public void MakePayment_UpdatesAccountBalance_WhenPaymentSucceeds()
        {
            // Arrange
            var account = CreateValidAccount(AllowedPaymentSchemes.Bacs);
            account.Balance = 1000m;
            var request = CreateValidRequest(PaymentScheme.Bacs, 350m);

            _mockDataStore.Setup(ds => ds.GetAccount(request.DebtorAccountNumber))
                .Returns(account);
            _mockValidator.Setup(v => v.Validate(account, request))
                .Returns(true);

            // Act
            _sut.MakePayment(request);

            // Assert
            Assert.Equal(650m, account.Balance);
        }

        [Fact]
        public void MakePayment_WhenValidationFails_ReturnsFalse()
        {
            // Arrange
            var account = CreateValidAccount(AllowedPaymentSchemes.Bacs);
            var request = CreateValidRequest(PaymentScheme.Bacs, 100m);

            _mockDataStore.Setup(ds => ds.GetAccount(request.DebtorAccountNumber))
                .Returns(account);
            _mockValidator.Setup(v => v.Validate(account, request))
                .Returns(false);

            // Act
            var result = _sut.MakePayment(request);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public void MakePayment_WhenGetAccountThrows_ReturnsFalse()
        {
            // Arrange
            var request = CreateValidRequest(PaymentScheme.Bacs, 100m);
            _mockDataStore.Setup(ds => ds.GetAccount(request.DebtorAccountNumber))
                .Throws(new Exception("Database error"));

            // Act
            var result = _sut.MakePayment(request);

            // Assert
            Assert.False(result.Success);
        }

        private Account CreateValidAccount(AllowedPaymentSchemes allowedSchemes)
        {
            return new Account
            {
                AccountNumber = "12345678",
                Balance = 1000m,
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = allowedSchemes
            };
        }

        private MakePaymentRequest CreateValidRequest(PaymentScheme scheme, decimal amount)
        {
            return new MakePaymentRequest
            {
                DebtorAccountNumber = "12345678",
                CreditorAccountNumber = "87654321",
                Amount = amount,
                PaymentDate = DateTime.Now,
                PaymentScheme = scheme
            };
        }
    }
}
