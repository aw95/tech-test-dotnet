using System;
using System.ComponentModel.DataAnnotations;

namespace ClearBank.DeveloperTest.Types
{
    public class MakePaymentRequest
    {
        [Required(ErrorMessage = "Creditor account number is required.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Creditor account number must be between 4 and 20 characters.")]
        public string CreditorAccountNumber { get; set; }

        [Required(ErrorMessage = "Debtor account number is required.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Debtor account number must be between 4 and 20 characters.")]
        public string DebtorAccountNumber { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment date is required.")]
        public DateTime PaymentDate { get; set; }

        [EnumDataType(typeof(PaymentScheme), ErrorMessage = "Invalid payment scheme.")]
        public PaymentScheme PaymentScheme { get; set; }
    }
}
