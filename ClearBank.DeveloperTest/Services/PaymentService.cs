using ClearBank.DeveloperTest.Common;
using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Services.Interfaces;
using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.Logging;
using System;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStoreFactory _dataStoreFactory;
        private readonly IPaymentValidatorService _validator;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(
            IAccountDataStoreFactory dataStoreFactory,
            IPaymentValidatorService validator,
            ILogger<PaymentService> logger)
        {
            _dataStoreFactory = dataStoreFactory ?? throw new ArgumentNullException(nameof(dataStoreFactory));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _logger = logger;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var result = new MakePaymentResult { Success = false };

            if (request == null)
            {
                _logger?.LogWarning("MakePayment called with null request.");
                return result;
            }

            var maskedAccountNumber = AccountNumberMasker.Mask(request.DebtorAccountNumber);

            try
            {
                var dataStore = _dataStoreFactory.GetDataStore();
                if (dataStore == null)
                {
                    _logger?.LogError("No data store available.");
                    return result;
                }

                var account = dataStore.GetAccount(request.DebtorAccountNumber);

                // Use validator for all payment types
                var valid = _validator.Validate(account, request);
                if (!valid)
                {
                    _logger?.LogInformation(
                        "Validation failed for account ending in {LastFourDigits} and scheme {PaymentScheme}.",
                        maskedAccountNumber,
                        request.PaymentScheme);
                    return result;
                }

                // All validation passed -> perform debit and update
                account.Balance -= request.Amount;
                dataStore.UpdateAccount(account);

                result.Success = true;
                _logger?.LogInformation(
                    "Payment of {Amount} from account ending in {LastFourDigits} succeeded.",
                    request.Amount,
                    maskedAccountNumber);
            }
            catch (Exception ex)
            {
                _logger?.LogError(
                    ex,
                    "Unexpected error while trying to make a payment for debtor account ending in {LastFourDigits}.",
                    maskedAccountNumber);
                result.Success = false;
            }

            return result;
        }
    }
}