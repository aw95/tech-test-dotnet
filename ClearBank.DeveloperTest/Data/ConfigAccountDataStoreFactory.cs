using ClearBank.DeveloperTest.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Data
{
    public class ConfigAccountDataStoreFactory : IAccountDataStoreFactory
    {
        private readonly IAccountDataStore _primary;
        private readonly IAccountDataStore _backup;
        private readonly IConfiguration _configuration;

        public ConfigAccountDataStoreFactory(
            IAccountDataStore primary, 
            IAccountDataStore backup, 
            IConfiguration configuration)
        {
            _primary = primary ?? throw new ArgumentNullException(nameof(primary));
            _backup = backup ?? throw new ArgumentNullException(nameof(backup));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IAccountDataStore GetDataStore()
        {
            var dataStoreType = _configuration["DataStoreType"];
            if (string.Equals(dataStoreType, "Backup", StringComparison.OrdinalIgnoreCase))
            {
                return _backup;
            }

            return _primary;
        }
    }
}
