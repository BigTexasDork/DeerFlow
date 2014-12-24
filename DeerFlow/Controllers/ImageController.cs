﻿using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeerFlow.Models;
using ExifLib;

namespace DeerFlow.Controllers
{
    [Authorize]
    public class ImageController : Controller
    {
        private DeerFlowContext db = new DeerFlowContext();

        // GET: Image
        public ActionResult Index()
        {
            var imageInfo = db.ImageInfo.Include(i => i.Image);
            return View(imageInfo.ToList());
        }

        // GET: Image/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImageInfo imageInfo = db.ImageInfo.Find(id);
            if (imageInfo == null)
            {
                return HttpNotFound();
            }
            return View(imageInfo);
        }

        // GET: Image/Create
        public ActionResult Create()
        {
            ViewBag.ImageId = new SelectList(db.Image, "Id", "Id");
            return View();
        }

        // POST: Image/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ContentType,ExifDate,ExifLattitude,ExifLongitude,StorageType,ImageId")] ImageInfo imageInfo)
        {
            if (ModelState.IsValid)
            {
                db.ImageInfo.Add(imageInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ImageId = new SelectList(db.Image, "Id", "Id", imageInfo.ImageId);
            return View(imageInfo);
        }

        // GET: Image/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImageInfo imageInfo = db.ImageInfo.Find(id);
            if (imageInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.ImageId = new SelectList(db.Image, "Id", "Id", imageInfo.ImageId);
            return View(imageInfo);
        }

        // POST: Image/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ContentType,ExifDate,ExifLattitude,ExifLongitude,StorageType,ImageId")] ImageInfo imageInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(imageInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ImageId = new SelectList(db.Image, "Id", "Id", imageInfo.ImageId);
            return View(imageInfo);
        }

        // GET: Image/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ImageInfo imageInfo = db.ImageInfo.Find(id);
            if (imageInfo == null)
            {
                return HttpNotFound();
            }
            return View(imageInfo);
        }

        // POST: Image/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ImageInfo imageInfo = db.ImageInfo.Find(id);
            db.ImageInfo.Remove(imageInfo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ImageDetail(int id)
        {
            //var ii = db.ImageInfo.Where(i => i.Id == id).Include(b => b.Image).FirstOrDefault();
            var ii = db.Image.Find(id);
            if (ii != null)
            {
                return File(ii.Data, "image/jpeg");
            }
            return HttpNotFound();
        }

        public ActionResult Upload()
        {
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
                    if (file == null)
                    {
                        return Json(new { Message = "No files uploaded" });
                    }

                    fName = file.FileName;
                    if (file.ContentLength > 0)
                    {
                        // save the stream, need to read it twice
                        //   once for exif
                        //   once for db
                        MemoryStream memoryStream;
                        using (var inputStream = file.InputStream)
                        {
                            memoryStream = inputStream as MemoryStream;
                            if (memoryStream == null)
                            {
                                memoryStream = new MemoryStream();
                                inputStream.CopyTo(memoryStream);
                            }
                        }

                        memoryStream.Seek(0, SeekOrigin.Begin);

                        // get exif info
                        var exif = new ExifReader(memoryStream);
                        var exifDate = new DateTime();
                        exif.GetTagValue(ExifTags.DateTimeDigitized, out exifDate);

                        // reset the stream
                        memoryStream.Seek(0, SeekOrigin.Begin);

                        // get info for db
                        var buffer = memoryStream.ToArray();

                        var valid = IsValidImage(buffer);
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

                        //var valid = IsValidImage(buffer);
                        //var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                        //var pathString = originalDirectory.ToString();
                        //if (!Directory.Exists(originalDirectory.ToString()))
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
        
        private static bool IsValidImage(byte[] bytes)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                    System.Drawing.Image.FromStream(ms);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}