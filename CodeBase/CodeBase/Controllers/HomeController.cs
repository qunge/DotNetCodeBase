using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeBase.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void UploadFile()
        {
            var files = System.Web.HttpContext.Current.Request.Files;
            if (files.Count > 0)
            {
                // 文件后缀名
                string fileExt = Path.GetExtension(files[0].FileName);
            }
        }


    }
}