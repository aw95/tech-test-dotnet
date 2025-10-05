namespace ClearBank.DeveloperTest.Types
{
    public enum PaymentScheme
    {

        [AllowedScheme(AllowedPaymentSchemes.FasterPayments)]
        FasterPayments,

        [AllowedScheme(AllowedPaymentSchemes.Bacs)]
        Bacs,

        [AllowedScheme(AllowedPaymentSchemes.Chaps)]
        Chaps

    }
}
