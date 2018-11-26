using Core.Repository;
using Infrastructure.Storage;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure.Repositories.Storage
{
    public class BlobStorageRepository : IBlobStorageRepository
    {
        StorageAccConnection _connection;
        StorageAccSasKey _SasKey;

        public BlobStorageRepository()
        {
            _connection = StorageAccConnection.GetObject();
            _SasKey = StorageAccSasKey.GetObject();
        }

        public string _PostDesignFilesandGetUrl(HttpPostedFileBase httpPostedFile, string ContainerName)
        {

            CloudBlobClient blobClient = _connection.storageClient;
            var sasConstraints = _SasKey.sharekey;
            Stream stream = httpPostedFile.InputStream;
            var file = Path.GetFileNameWithoutExtension(httpPostedFile.FileName);
            var fileType = Path.GetExtension(httpPostedFile.FileName);
            var fileName = file + fileType;
            var contentType = httpPostedFile.ContentType;
            CloudBlobContainer containers = blobClient.GetContainerReference(ContainerName);
            if (!containers.Exists())
            {
                containers.CreateIfNotExists(BlobContainerPublicAccessType.Container);
            }
            CloudBlockBlob blockBlob = containers.GetBlockBlobReference(fileName);
            blockBlob.Properties.ContentType = contentType;
            blockBlob.UploadFromStream(stream);
            var Sas = containers.GetSharedAccessSignature(sasConstraints);
            var url = Convert.ToString(blockBlob.Uri);
            return url+Sas;
        }

        //public List<FilesReturnModel> GetProjectDesignFilesByContainer(string container)
        //{
        //    try
        //    {
        //        List<FilesReturnModel> d = new List<FilesReturnModel>();
        //        CloudBlobClient blobClient = _connection.storageClient;
        //        CloudBlobContainer blobcontainer = blobClient.GetContainerReference(container);
        //        var sasConstraints = _SasKey.sharekey;

        //        JArray urls = new JArray();
        //        var data = blobcontainer.ListBlobs();

        //        foreach (var blobItem in blobcontainer.ListBlobs())
        //        {
        //            var bloburl = blobItem.Uri.ToString();
        //            var Sas = blobcontainer.GetSharedAccessSignature(sasConstraints);
        //            urls.Add(bloburl + Sas);
        //            FilesReturnModel dat = new FilesReturnModel();
        //            dat.url = bloburl + Sas;
        //            dat.FileName = Path.GetFileName(blobItem.Uri.LocalPath);
        //            d.Add(dat);
        //        }

        //        return d;
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }

        //}


        //public IEnumerable<IListBlobItem> GetBlobList(string container)
        //{
        //    CloudBlobClient blobClient = _connection.storageClient;
        //    var sasConstraints = _SasKey.sharekey;
        //    CloudBlobContainer blobcontainer = blobClient.GetContainerReference(container);
        //    if (!blobcontainer.Exists())
        //    {
        //        blobcontainer.CreateIfNotExists();
        //    }
        //    return blobcontainer.ListBlobs();

        //}

        //public CloudBlobContainer GetProjectCloudBlobContainer(string container)
        //{
        //    var sasConstraints = new SharedAccessBlobPolicy();
        //    CloudBlobClient blobClient = _connection.storageClient;
        //    CloudBlobContainer blobcontainer = blobClient.GetContainerReference(container);
        //    if (!blobcontainer.Exists())
        //    {
        //        blobcontainer.CreateIfNotExists();
        //    }
        //    return blobcontainer;
        //}

        //public string _GetBlobImageSASToken(string container)
        //{

        //    CloudBlobClient blobClient = _connection.storageClient;
        //    CloudBlobContainer blobcontainer = blobClient.GetContainerReference(container);
        //    var sasConstraints = _SasKey.sharekey;
        //    return blobcontainer.GetSharedAccessSignature(sasConstraints);
        //}

        //public void DeleteContainer(string container)
        //{
        //    CloudBlobClient blobClient = _connection.storageClient;
        //    var sasConstraints = _SasKey.sharekey;
        //    CloudBlobContainer blobcontainer = blobClient.GetContainerReference(container);
        //    blobcontainer.DeleteIfExists(null, null, null);
        //}


        //public string DeleteSingleFile(String file, string container)
        //{
        //    CloudBlobClient blobClient = _connection.storageClient;
        //    var sasConstraints = _SasKey.sharekey;
        //    CloudBlobContainer blobcontainer = blobClient.GetContainerReference(container);
        //    if (!blobcontainer.Exists())
        //    {
        //        blobcontainer.CreateIfNotExists();
        //    }

        //    CloudBlockBlob blob = blobcontainer.GetBlockBlobReference(file);
        //    blob.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots, null, null, null);

        //    return "sucess";

        //}
        //public string _PostAdminPhotosWithRezie(HttpPostedFileBase httpPostedFile, byte[] data)
        //{
        //    CloudBlobClient blobClient = _connection.storageClient;
        //    var container = _connection.ProfileContainer;

        //    Stream stream = httpPostedFile.InputStream;

        //    var file = Path.GetFileNameWithoutExtension(httpPostedFile.FileName);
        //    var fileType = Path.GetExtension(httpPostedFile.FileName);
        //    var fileName = file + fileType;
        //    var contentType = httpPostedFile.ContentType;
        //    CloudBlobContainer containers = blobClient.GetContainerReference(container);
        //    if (!containers.Exists())
        //    {
        //        containers.CreateIfNotExists(BlobContainerPublicAccessType.Container);
        //    }
        //    CloudBlockBlob blockBlob = containers.GetBlockBlobReference(fileName);
        //    blockBlob.Properties.ContentType = contentType;
        //    blockBlob.UploadFromByteArray(data, 0, data.Length);
        //    var sasConstraints = _SasKey.sharekey;
        //    var Sas = containers.GetSharedAccessSignature(sasConstraints);
        //    var url = Convert.ToString(blockBlob.Uri);
        //    return url + Sas;
        //}

        //public FilesReturnModel _PostMaterialImageResizeAndGetUrl(HttpPostedFileBase httpPostedFile, byte[] data,string containerName)
        //{
        //    var sasConstraints = new SharedAccessBlobPolicy();
        //    CloudBlobClient blobClient = _connection.storageClient;
            
        //    var file = Path.GetFileNameWithoutExtension(httpPostedFile.FileName);
        //    var fileType = Path.GetExtension(httpPostedFile.FileName);
        //    var fileName = file + fileType;
        //    var contentType = httpPostedFile.ContentType;
        //    CloudBlobContainer containers = blobClient.GetContainerReference(containerName);
        //    if (!containers.Exists())
        //    {
        //        containers.CreateIfNotExists(BlobContainerPublicAccessType.Container);
        //    }
        //    CloudBlockBlob blockBlob = containers.GetBlockBlobReference(fileName);
        //    blockBlob.Properties.ContentType = contentType;
        //    blockBlob.UploadFromByteArray(data, 0, data.Length);
        //    sasConstraints.SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5);
        //    sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddDays(365);
        //    sasConstraints.Permissions = SharedAccessBlobPermissions.Read;
        //    var Sas = containers.GetSharedAccessSignature(sasConstraints);
        //    var url = Convert.ToString(blockBlob.Uri);

        //    var d = new FilesReturnModel
        //    {
        //        url = url + Sas,
        //        FileName = fileName,
        //        uploadOn = string.Format("{0:MMM dd, yyyy}", blockBlob.Properties.LastModified)
        //    };

        //    return d;

           
        //}



        //public virtual string PostScreenShotAndGetUrl(string fileName, byte[] data, string ContainerName)
        //{
        //    CloudBlobClient blobClient = _connection.storageClient;
        //    var sasConstraints = _SasKey.sharekey;
        //    CloudBlobContainer containers = blobClient.GetContainerReference(ContainerName);
        //    if (!containers.Exists())
        //    {
        //        containers.CreateIfNotExists(BlobContainerPublicAccessType.Container);
        //    }

        //    CloudBlockBlob blockBlob = containers.GetBlockBlobReference(fileName);
        //    if (blockBlob.Exists())
        //    {
        //        blockBlob.Delete();
        //        blockBlob.Properties.ContentType = "image/jpeg";
        //        blockBlob.UploadFromByteArray(data, 0, data.Length);
        //        var url = Convert.ToString(blockBlob.Uri);
        //        var Sas = containers.GetSharedAccessSignature(sasConstraints);
        //        return url + Sas;
        //    }
        //    else
        //    {
        //        blockBlob.Properties.ContentType = "image/jpeg";
        //        blockBlob.UploadFromByteArray(data, 0, data.Length);
        //        var url = Convert.ToString(blockBlob.Uri);
        //        var Sas = containers.GetSharedAccessSignature(sasConstraints);
        //        return url + Sas;
        //    }


        //}










    }
}
