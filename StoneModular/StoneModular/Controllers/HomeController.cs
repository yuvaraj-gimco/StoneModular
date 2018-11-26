using Infrastructure;
using Infrastructure.Repositories.Storage;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace StoneModular.Controllers
{
    public class HomeController : Controller
    {
        IBlobStorageRepository _blob;
        public HomeController(IBlobStorageRepository blob)
        {
            this._blob = blob;
        }
       



        public ActionResult Index()
        {

            var sampleDoc = SvgDocument.Open(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample.svg"));
            var image = sampleDoc.GetElementById<SvgImage>("image");
            image.Href = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQue5W5nc7lA1wdcz060Lc2cXACuv2Z--0CogCzOClOAbx6tODH";
            sampleDoc.Draw().Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\sample.png"));

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        
        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadFile(HttpPostedFileBase myfile)
        {


           var da= _blob._PostDesignFilesandGetUrl(myfile, "stonemodularimages");

            var fd = _blob._PotDesignFilesandGetUrl(myfile, "stonemodularimages");


            // extract only the fielname
            var fileName = Path.GetFileName(myfile.FileName);
            // store the file inside ~/App_Data/uploads folder
           // var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);

            var sampleDoc = SvgDocument.Open(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample.svg"));
            var image = sampleDoc.GetElementById<SvgImage>("image");
            image.Href = da;
            var rr= sampleDoc.Draw();


            byte[] result = null;

            if (rr != null)
            {
                MemoryStream stream = new MemoryStream();
                rr.Save(stream, ImageFormat.Jpeg);
                result = stream.ToArray();
            }

          var imageUrl=  _blob._PostAdminPhotosWithRezie(myfile, result, "stonemodularimages");
            ViewBag.image = imageUrl;

            

            //here i have got the files value is null.
            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}