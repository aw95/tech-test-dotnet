using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Types
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AllowedSchemeAttribute : Attribute
    {
        public AllowedPaymentSchemes AllowedScheme { get; }

        public AllowedSchemeAttribute(AllowedPaymentSchemes allowedScheme)
        {
            AllowedScheme = allowedScheme;
        }
    }

}
