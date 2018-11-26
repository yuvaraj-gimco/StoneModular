using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Infrastructure.Storage
{
    public sealed class StorageAccConnection
    {
        static StorageAccConnection _connection;

        public CloudBlobClient storageClient;

        public string ProfileContainer;

        public StorageAccConnection()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StoneModular_AzureStorageConnectionString"));
            storageClient = storageAccount.CreateCloudBlobClient();
            ProfileContainer = System.Configuration.ConfigurationManager.AppSettings["PhotoContainer"];
        }
        public static StorageAccConnection GetObject()
        {
            if (_connection == null)
            {
                _connection = new StorageAccConnection();


            }
            return _connection;
        }
    }
}
