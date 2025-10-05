using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Common
{
    public static class AccountNumberMasker
    {
        /// <summary>
        /// Static method to mask an account number, showing only the last 4 digits.
        /// </summary>

        public static string Mask(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                return "UNKNOWN";

            var length = accountNumber.Length;
            return length <= 4 ? accountNumber : accountNumber.Substring(length - 4);
        }
    }
}
