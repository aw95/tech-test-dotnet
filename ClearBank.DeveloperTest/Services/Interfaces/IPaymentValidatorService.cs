using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Services.Interfaces
{
    public interface IPaymentValidatorService
    {
        bool Validate(Account account, MakePaymentRequest request);
    }
}
