using Core.IRepositories;
using Core.Repository;
using Infrastructure.FileHelper;
using Infrastructure.Repositories.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModels;

namespace Infrastructure.Repositories
{
    public class FileHelper
    {
        IBlobStorageRepository blobStorage;
        IMaterialRepository _material;
        public FileHelper(IBlobStorageRepository ibs, IMaterialRepository material)
        {
            this.blobStorage = ibs;
            this._material = material;
        }
        public FilesReturnModel UploadMaterial(HttpContextBase ContentBase, List<ViewDataUploadFilesResult> resultList, string ContainerName)
        {
            var model = new FilesReturnModel();
            var httpRequest = ContentBase.Request;
            for (int i = 0; i < httpRequest.Files.Count; i++)
            {
                var file = httpRequest.Files[i];
                model = blobStorage._PostDesignFilesandGetUrl(file, ContainerName);
                var fileType = MimeMapping.GetMimeMapping(file.FileName);
                resultList.Add(UploadResult(model.FileName, file.ContentLength, fileType, model.url, model.uploadOn, ContainerName));

            }
          return model;
        }
        public FilesReturnModel UploadMaterialThumb(HttpContextBase ContentBase, List<ViewDataUploadFilesResult> resultList, string ContainerName)
        {
            var thumbModel = new FilesReturnModel();
            var httpRequest = ContentBase.Request;
            for (int i = 0; i < httpRequest.Files.Count; i++)
            {
                var file = httpRequest.Files[i];

                var resize = ImageHandler.ImageScale(file, 400, 200);
               thumbModel = blobStorage._PostMaterialImageResizeAndGetUrl(file, resize, ContainerName);
             
                //  thumbModel = blobStorage._PostDesignFilesandGetUrl(file, ContainerName);
                var fileType = MimeMapping.GetMimeMapping(file.FileName);
                resultList.Add(UploadResult(thumbModel.FileName, file.ContentLength, fileType, thumbModel.url, thumbModel.uploadOn, ContainerName));

            }
            return thumbModel;
        }
        


        public ViewDataUploadFilesResult UploadResult(String FileName, int fileSize, String FileType, string FullImageUrl, string ThumbUrl, string ContainerName)
        {
            //String getType = System.Web.MimeMapping.GetMimeMapping(FileFullPath);
            var result = new ViewDataUploadFilesResult()
            {
                name = FileName,
                size = fileSize,
                type = FileType,
                url = FullImageUrl,
                deleteUrl = "/Material/DeleteMaterial/?file=" + FileName + "&container=" + ContainerName,
                thumbnailUrl = ThumbUrl,
                deleteType = "GET",
            };

            return result;
        }

        public List<ViewDataUploadFilesResult> GetProjectFilesByContainer(string container)
        {
            var r = new List<ViewDataUploadFilesResult>();
            var result = blobStorage.GetBlobList(container);
            foreach(CloudBlockBlob blobItem in result)
            {
                var fileName = blobItem.Name;

                var fileSize = (int)blobItem.Properties.Length;

                var fileType = blobItem.GetType().ToString();
                var url = Convert.ToString(blobItem.Uri);
                var sas = blobStorage._GetBlobImageSASToken(container);
                var FullUrl = url + sas;
                var Thumburl = GetThumbBlobUrl(fileName, container);
                r.Add(UploadResult(fileName, fileSize, fileType, FullUrl, Thumburl, container));
            }
            return r;
        }
        public string GetThumbBlobUrl(string fileName, string ContainerName)
        {

            CloudBlobContainer Thumbcontainer = blobStorage.GetProjectCloudBlobContainer(ContainerName);
            CloudBlockBlob blob = Thumbcontainer.GetBlockBlobReference(fileName);
            var url = Convert.ToString(blob.Uri);
            var sas = blobStorage._GetBlobImageSASToken(ContainerName);

            return url + sas;
        }
    }

    public class ViewDataUploadFilesResult
    {
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string deleteUrl { get; set; }
        public string thumbnailUrl { get; set; }
        public string deleteType { get; set; }
    }
    public class JsonFiles
    {
        public ViewDataUploadFilesResult[] files;
        public string TempFolder { get; set; }
        public JsonFiles(List<ViewDataUploadFilesResult> filesList)
        {
            files = new ViewDataUploadFilesResult[filesList.Count];
            for (int i = 0; i < filesList.Count; i++)
            {
                files[i] = filesList.ElementAt(i);
            }

        }
    }

}
