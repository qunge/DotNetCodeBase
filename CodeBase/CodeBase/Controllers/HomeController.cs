using Newtonsoft.Json;
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
        public ActionResult UploadFile()
        {
            string name = Request.Form["name"];
            string age = Request.Form["age"];
            //文件大小限制10M
            const int maxSize = 10485760;
            //定义允许上传的文件扩展名
            
            var ext = new[] { "rar", "zip", "gif", "jpg", "jpeg", "png", "bmp", "xls", "xlsx", "doc", "docx", "et", "wps" };
            // 拼接后缀名
            string strExt = "";
            foreach (var a in ext)
            {
                strExt += a + "/";
            }
            strExt = strExt.TrimEnd('/');
            //文件保存路径
            string savePath = "/upLoads/";                

            var resp = new { status=false,message="", ImgUrl ="", FName ="", OName =""};
            var files = System.Web.HttpContext.Current.Request.Files;
            if (files.Count > 0)
            {
                // 文件后缀名
                string fileExt = Path.GetExtension(files[0].FileName);
                // 判断上传文件是否符合规则
                if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(ext, fileExt.Substring(1).ToLower()) < 0)
                {
                    resp = new { status = false, message = string.Format("扩展名为{0}的文件不允许上传!只允许上传{1}格式的文件。", fileExt, strExt), ImgUrl = "", FName = "", OName = "" };
                }
                else
                {
                    if (files[0].InputStream.Length > maxSize)
                    {
                        resp = new { status = false, message = "上传文件大小超过限制！", ImgUrl = "", FName = "", OName = "" };
                    }
                    else
                    {
                        try
                        {
                            // 新文件名
                            string fileNewName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + fileExt;
                            // 保存文件
                            SaveFile(files[0], HttpContext.Server.MapPath(savePath), fileNewName);

                            resp = new { 
                                status = true, 
                                message = "上传成功! 文件大小为:" + files[0].ContentLength, 
                                ImgUrl = HttpContext.Server.MapPath(savePath) + fileNewName, 
                                FName = fileNewName, 
                                OName = files[0].FileName
                            };
                        }
                        catch (Exception ex)
                        {
                            resp = new { status = false, message = ex.ToString(), ImgUrl = "", FName = "", OName = "" };
                        }
                    }
                }
            }
            else
            {
                resp = new { status = false, message = "请选择文件!", ImgUrl = "", FName = "", OName = "" };
            }
            return Json(resp);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="imgFile">要保存的文件</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="fileName">文件名称</param>
        private void SaveFile(HttpPostedFile imgFile, string savePath, string fileName)
        {
            if (!Directory.Exists(savePath))    //判断文件存放路径是否存在
            {
                Directory.CreateDirectory(savePath);
            }

            imgFile.SaveAs(Path.Combine(savePath, fileName));
        }

    }

}