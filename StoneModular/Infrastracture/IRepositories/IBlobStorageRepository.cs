
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

using System.Web;


namespace Core.Repository
{
    public interface IBlobStorageRepository
    {
        string _PostDesignFilesandGetUrl(HttpPostedFileBase httpPostedFile, string ContainerName);
        //JArray GetProjectDesignFilesByContainer(string container);

        //string DeleteSingleFile(string file, string container);
        ////List<FilesReturnModel> GetProjectDesignFilesByContainer(string container);
        //void DeleteContainer(string container);

        //IEnumerable<IListBlobItem> GetBlobList(string container);

        //string _GetBlobImageSASToken(string container);

        //CloudBlobContainer GetProjectCloudBlobContainer(string container);
        //string _PostAdminPhotosWithRezie(HttpPostedFileBase httpPostedFile, byte[] data);

        //FilesReturnModel _PostMaterialImageResizeAndGetUrl(HttpPostedFileBase httpPostedFile, byte[] data,string continerName);

        //string PostScreenShotAndGetUrl(string fileName, byte[] data, string ContainerName);
    }
}
