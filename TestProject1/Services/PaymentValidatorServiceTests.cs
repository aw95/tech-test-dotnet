using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Services
{
    public class PaymentValidatorServiceTests
    {
        private readonly Mock<ILogger<PaymentValidatorService>> _mockLogger;
        private readonly PaymentValidatorService _sut;

        public PaymentValidatorServiceTests()
        {
            _mockLogger = new Mock<ILogger<PaymentValidatorService>>();
            _sut = new PaymentValidatorService(_mockLogger.Object);
        }

        #region FasterPayments Tests

        [Fact]
        public void Validate_FasterPayments_WithValidAccount_ReturnsTrue()
        {
            // Arrange
            var account = new Account
            {
                AccountNumber = "12345678",
                Balance = 1000m,
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments
            };
            var request = CreateRequest(PaymentScheme.FasterPayments, 500m);

            // Act
            var result = _sut.Validate(account, request);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Validate_FasterPayments_WithNullAccount_ReturnsFalse()
        {
            // Arrange
            var request = CreateRequest(PaymentScheme.FasterPayments, 100m);

            // Act
            var result = _sut.Validate(null, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_FasterPayments_WithSchemeNotAllowed_ReturnsFalse()
        {
            // Arrange
            var account = new Account
            {
                AccountNumber = "12345678",
                Balance = 1000m,
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
            };
            var request = CreateRequest(PaymentScheme.FasterPayments, 100m);

            // Act
            var result = _sut.Validate(account, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_FasterPayments_WithInsufficientBalance_ReturnsFalse()
        {
            // Arrange
            var account = new Account
            {
                AccountNumber = "12345678",
                Balance = 100m,
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments
            };
            var request = CreateRequest(PaymentScheme.FasterPayments, 500m);

            // Act
            var result = _sut.Validate(account, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_FasterPayments_WithDisabledAccount_ReturnsFalse()
        {
            // Arrange
            var account = new Account
            {
                AccountNumber = "12345678",
                Balance = 1000m,
                Status = AccountStatus.Disabled,
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments
            };
            var request = CreateRequest(PaymentScheme.FasterPayments, 100m);

            // Act
            var result = _sut.Validate(account, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_FasterPayments_WithInboundPaymentsOnlyAccount_ReturnsFalse()
        {
            // Arrange
            var account = new Account
            {
                AccountNumber = "12345678",
                Balance = 1000m,
                Status = AccountStatus.InboundPaymentsOnly,
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments
            };
            var request = CreateRequest(PaymentScheme.FasterPayments, 100m);

            // Act
            var result = _sut.Validate(account, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_FasterPayments_WithNegativeBalance_ReturnsFalse()
        {
            // Arrange
            var account = new Account
            {
                AccountNumber = "12345678",
                Balance = -100m,
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments
            };
            var request = CreateRequest(PaymentScheme.FasterPayments, 50m);

            // Act
            var result = _sut.Validate(account, request);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Bacs Tests

        [Fact]
        public void Validate_Bacs_WithValidAccount_ReturnsTrue()
        {
            // Arrange
            var account = new Account
            {
                AccountNumber = "12345678",
                Balance = 1000m,
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
            };
            var request = CreateRequest(PaymentScheme.Bacs, 100m);

            // Act
            var result = _sut.Validate(account, request);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Validate_Bacs_WithNullAccount_ReturnsFalse()
        {
            // Arrange
            var request = CreateRequest(PaymentScheme.Bacs, 100m);

            // Act
            var result = _sut.Validate(null, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_Bacs_WithSchemeNotAllowed_ReturnsFalse()
        {
            // Arrange
            var account = new Account
            {
                AccountNumber = "12345678",
                Balance = 1000m,
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments
            };
            var request = CreateRequest(PaymentScheme.Bacs, 100m);

            // Act
            var result = _sut.Validate(account, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_Bacs_WithInsufficientBalance_ReturnsFalse()
        {
            // Arrange
            var account = new Account
            {
                AccountNumber = "12345678",
                Balance = 50m,
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
            };
            var request = CreateRequest(PaymentScheme.Bacs, 100m);

            // Act
            var result = _sut.Validate(account, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_Bacs_WithDisabledAccount_ReturnsFalse()
        {
            // Arrange
            var account = new Account
            {
                AccountNumber = "12345678",
                Balance = 1000m,
                Status = AccountStatus.Disabled,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
            };
            var request = CreateRequest(PaymentScheme.Bacs, 100m);

            // Act
            var result = _sut.Validate(account, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_Bacs_WithInboundPaymentsOnlyAccount_ReturnsFalse()
        {
            // Arrange
            var account = new Account
            {
                AccountNumber = "12345678",
                Balance = 1000m,
                Status = AccountStatus.InboundPaymentsOnly,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
            };
            var request = CreateRequest(PaymentScheme.Bacs, 100m);

            // Act
            var result = _sut.Validate(account, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_Bacs_WithNegativeBalance_ReturnsFalse()
        {
            // Arrange
            var account = new Account
            {
                AccountNumber = "12345678",
                Balance = -50m,
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
            };
            var request = CreateRequest(PaymentScheme.Bacs, 100m);

            // Act
            var result = _sut.Validate(account, request);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Chaps Tests

        [Fact]
        public void Validate_Chaps_WithValidAccount_ReturnsTrue()
        {
            // Arrange
            var account = new Account
            {
                AccountNumber = "12345678",
                Balance = 1000m,
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps
            };
            var request = CreateRequest(PaymentScheme.Chaps, 100m);

            // Act
            var result = _sut.Validate(account, request);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Validate_Chaps_WithNullAccount_ReturnsFalse()
        {
            // Arrange
            var request = CreateRequest(PaymentScheme.Chaps, 100m);

            // Act
            var result = _sut.Validate(null, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_Chaps_WithSchemeNotAllowed_ReturnsFalse()
        {
            // Arrange
            var account = new Account
            {
                AccountNumber = "12345678",
                Balance = 1000m,
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
            };
            var request = CreateRequest(PaymentScheme.Chaps, 100m);

            // Act
            var result = _sut.Validate(account, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_Chaps_WithInsufficientBalance_ReturnsFalse()
        {
            // Arrange
            var account = new Account
            {
                AccountNumber = "12345678",
                Balance = 50m,
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps
            };
            var request = CreateRequest(PaymentScheme.Chaps, 100m);

            // Act
            var result = _sut.Validate(account, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_Chaps_WithDisabledAccount_ReturnsFalse()
        {
            // Arrange
            var account = new Account
            {
                AccountNumber = "12345678",
                Balance = 1000m,
                Status = AccountStatus.Disabled,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps
            };
            var request = CreateRequest(PaymentScheme.Chaps, 100m);

            // Act
            var result = _sut.Validate(account, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_Chaps_WithInboundPaymentsOnlyAccount_ReturnsFalse()
        {
            // Arrange
            var account = new Account
            {
                AccountNumber = "12345678",
                Balance = 1000m,
                Status = AccountStatus.InboundPaymentsOnly,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps
            };
            var request = CreateRequest(PaymentScheme.Chaps, 100m);

            // Act
            var result = _sut.Validate(account, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Validate_Chaps_WithNegativeBalance_ReturnsFalse()
        {
            // Arrange
            var account = new Account
            {
                AccountNumber = "12345678",
                Balance = -50m,
                Status = AccountStatus.Live,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps
            };
            var request = CreateRequest(PaymentScheme.Chaps, 100m);

            // Act
            var result = _sut.Validate(account, request);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Helper Methods

        private MakePaymentRequest CreateRequest(PaymentScheme scheme, decimal amount)
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

        #endregion
    }
}