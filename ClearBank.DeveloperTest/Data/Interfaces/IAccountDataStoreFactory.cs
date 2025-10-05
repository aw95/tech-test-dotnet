using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Data.Interfaces
{
    public interface IAccountDataStoreFactory
    {
        IAccountDataStore GetDataStore();
    }
}
