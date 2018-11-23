using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoneModular.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            var sampleDoc = SvgDocument.Open(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\sample.svg"));


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

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}