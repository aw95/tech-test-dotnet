using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Common
{
    public static class PaymentSchemeExtensions
    {
        /// <summary>
        /// Static extension method to get the AllowedPaymentSchemes flag associated with a PaymentScheme enum value.
        /// </summary>
        
        public static AllowedPaymentSchemes GetAllowedScheme(this PaymentScheme scheme)
        {
            var type = typeof(PaymentScheme);
            var member = type.GetMember(scheme.ToString()).FirstOrDefault();
            var attribute = member?.GetCustomAttributes(typeof(AllowedSchemeAttribute), false)
                                   .Cast<AllowedSchemeAttribute>()
                                   .FirstOrDefault();

            if (attribute == null)
                throw new ArgumentException($"No AllowedSchemeAttribute defined for {scheme}");

            return attribute.AllowedScheme;
        }
    }
}
