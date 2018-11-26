using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Storage
{
    public sealed class StorageAccSasKey
    {
        static StorageAccSasKey _Sas;

        public SharedAccessBlobPolicy sharekey;
        public StorageAccSasKey()
        {
            sharekey = new SharedAccessBlobPolicy();
            sharekey.SharedAccessStartTime = DateTime.UtcNow.AddDays(-2);
            sharekey.SharedAccessExpiryTime = DateTime.UtcNow.AddDays(365);
            sharekey.Permissions = SharedAccessBlobPermissions.Read;
        }
        public static StorageAccSasKey GetObject()
        {
            if (_Sas == null)
            {
                _Sas = new StorageAccSasKey();

            }
            return _Sas;
        }
    }
}
