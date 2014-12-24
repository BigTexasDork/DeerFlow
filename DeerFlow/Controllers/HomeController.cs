using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeerFlow.Models;
using ExifLib;

namespace DeerFlow.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload()
        {
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

        public ActionResult SaveUploadedFile()
        {
            var isSavedSuccessfully = true;
            var fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        using (var db = new DeerFlowContext())
                        {
                            var exif = new ExifReader(file.InputStream);
                            var exifDate = new DateTime();
                            exif.GetTagValue(ExifTags.DateTimeDigitized, out exifDate);

                            var buffer = new byte[file.ContentLength];
                            file.InputStream.Read(buffer, 0, file.ContentLength);
                            var image = new Image { Data = buffer };

                            db.Image.Add(image);
                            //db.SaveChanges();
                        
                            var imageData = new ImageInfo
                                {
                                    StorageType = "Database",
                                    ContentType = file.ContentType,
                                    Name = file.FileName,
                                    ExifDate = exifDate,
                                    Image = image
                                };

                            db.ImageInfo.Add(imageData);
                            db.SaveChanges();
                        }

                        //var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                        //var pathString = Path.Combine(originalDirectory.ToString(), "imagepath");
                        //var fileName1 = Path.GetFileName(file.FileName);
                        //var isExists = System.IO.Directory.Exists(pathString);
                        //if (!isExists)
                        //    Directory.CreateDirectory(pathString);
                        //var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        //file.SaveAs(path);

                    }

                }

            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }


            return Json(isSavedSuccessfully ? new { Message = fName } : new { Message = "Error in saving file" });
        }
    }
}