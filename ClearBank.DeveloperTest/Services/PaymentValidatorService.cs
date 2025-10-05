using ClearBank.DeveloperTest.Common;
using ClearBank.DeveloperTest.Services.Interfaces;
using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.Logging;
using System;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentValidatorService : IPaymentValidatorService
    {
        private readonly ILogger<PaymentValidatorService> _logger;

        public PaymentValidatorService(ILogger<PaymentValidatorService> logger)
        {
            _logger = logger;
        }

        public bool Validate(Account account, MakePaymentRequest request)
        {
            // Check 1: Account must exist
            if (account == null)
            {
                _logger?.LogWarning("Payment validation failed: account not found.");
                return false;
            }

            // Check 2: Payment scheme must be allowed for this account
            var requiredScheme = request.PaymentScheme.GetAllowedScheme();

            if (!account.AllowedPaymentSchemes.HasFlag(requiredScheme))
            {
                _logger?.LogWarning(
                    "Payment validation failed: {Scheme} not allowed for account ending in {AccountNumber}.",
                    request.PaymentScheme,
                    AccountNumberMasker.Mask(account.AccountNumber));
                return false;
            }

            // Check 3: Account must have sufficient balance
            if (account.Balance < request.Amount)
            {
                _logger?.LogWarning(
                    "Payment validation failed: insufficient funds for account ending in {AccountNumber}. Balance={Balance}, Amount={Amount}",
                    AccountNumberMasker.Mask(account.AccountNumber),
                    account.Balance,
                    request.Amount);
                return false;
            }

            // Check 4: Account must be Live
            if (account.Status != AccountStatus.Live)
            {
                _logger?.LogWarning(
                    "Payment validation failed: invalid account status for account ending in {AccountNumber}. Status={Status}",
                    AccountNumberMasker.Mask(account.AccountNumber),
                    account.Status);
                return false;
            }

            return true;
        }
    }
}