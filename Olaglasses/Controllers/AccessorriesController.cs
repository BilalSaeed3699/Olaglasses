using Olaglasses.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Olaglasses.Controllers
{
    [FilterConfig.NoDirectAccess]
    [FilterConfig.AuthorizeActionFilter]
    public class AccessorriesController : Controller
    {
        // GET: Accessorries
        OlaGlassesEntities dbEntity = new OlaGlassesEntities();
        public ActionResult Index(string message)
        {
            ViewBag.message = message;
            List<tblProduct> products = new List<tblProduct>();
            products = dbEntity.tblProducts.Where(x => x.ProductCategory == "Accessory").ToList();
            dbEntity.Database.ExecuteSqlCommand("Delete from tblglassPicture where glassID=0");       
            dbEntity.SaveChanges();
            ViewBag.ProductList = products;
            return View();
        }
        public ActionResult Create(int? id)
        {
            tblProduct product = new tblProduct();
            if (id > 0)
            {
                product = dbEntity.tblProducts.Find(id);

                List<tblglassPicture> tblglassPictures = new List<tblglassPicture>();
                tblglassPictures = dbEntity.tblglassPictures.Where(a => a.glassID == id).ToList();
                ViewBag.AccessoryImages = tblglassPictures;

            }
            else
            {
                product.GlassID = 0;
                product.Title = "";
                product.shape = "";
                product.Colour = "";
                product.Material = "";
                product.Brand = "";
                product.Gender = "";
                product.AdditionalInfo = "";
                product.AvailableSize = "";
                product.Price = 0;
                product.Cost = 0;
                product.Manufacturer = "";
                product.Model = "";
                product.OrignalCode = "";
                product.Mgf_Code = "";
                product.Rim = "";
                product.Sticker = "";
                product.Feature = "";
                product.ProductCategory = "";
                product.ReleatedTo = 0;
                product.SellinClinic = "";

            }
            return View(product);
        }
        [HttpPost]
        public ActionResult Create(tblProduct product)
        {
            if (product.Title != null)
            {
                
                product.ProductCategory = "Accessory";
                if (product.GlassID > 0)
                {
                    dbEntity.Entry(product).State = EntityState.Modified;
                    dbEntity.SaveChanges();
                    
                    return RedirectToAction("Index", new { message = "Record Updated Successfully" });

                }
                else
                {
                    dbEntity.tblProducts.Add(product);
                    dbEntity.SaveChanges();
                    int Glassid = product.GlassID;
                    List<tblglassPicture> tblglassPictureList = dbEntity.tblglassPictures.Where(x => x.glassID == 0).ToList();
                    foreach (var tblglassPicture in tblglassPictureList)
                    {
                        tblglassPicture.glassID = Glassid;
                        dbEntity.Entry(tblglassPicture).State = EntityState.Modified;
                        dbEntity.SaveChanges();
                    }
                    return RedirectToAction("Index", new { message = "Record Added Successfully" });
                }
            }
            return RedirectToAction("Create", new { id = 0 }) ;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteData(int? id)
        {
            try
            {

                tblProduct make = dbEntity.tblProducts.Find(id);
                if (make != null)
                {
                    dbEntity.tblProducts.Remove(make);
                    dbEntity.SaveChanges();
                }


                return RedirectToAction("Index", new { message = "Record Deleted Successfully" });
            }
            catch (Exception)
            {

                throw;
            }

        }


        [HttpPost]

        //Upload Files 
        public ActionResult UploadImages(int? ProductId = 0)
        {
            if (Request.Files.Count > 0)
            {
                try
                {


                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                            fname = file.FileName.Replace(" ","_");
                        }
                        else
                        {
                            fname = file.FileName.Replace(" ", "_");

                        }


                        var FolderPath = Server.MapPath("/ProjectImages/AccessoryImage/");

                        if (!Directory.Exists(FolderPath))
                        {
                            // Try to create the directory.
                            DirectoryInfo di = Directory.CreateDirectory(FolderPath);
                        }
                        FolderPath = "/ProjectImages/AccessoryImage/" + fname;
                        

                        tblglassPicture temp = new tblglassPicture();
                        temp.glassID = ProductId;
                        temp.ImagePath = FolderPath;
                        dbEntity.tblglassPictures.Add(temp);
                        dbEntity.SaveChanges();

                        FolderPath = "/ProjectImages/AccessoryImage/";
                        fname = Path.Combine(Server.MapPath(FolderPath), fname);

                        file.SaveAs(fname);

                    }
                    List<tblglassPicture> TemporaryUploadedFiles = new List<tblglassPicture>();

                    TemporaryUploadedFiles = dbEntity.tblglassPictures.Where(p => p.glassID == ProductId).ToList().ToList();
                    return Json(TemporaryUploadedFiles, JsonRequestBehavior.AllowGet);






                }
                catch (Exception ex)
                {
                    return Json("Error occurred.Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("1");
            }
        }

        public ActionResult DeleteImage(int? VCardid = 0, int? ImageId = 0)
        {

            if (VCardid > 0)
            {
                dbEntity.Database.ExecuteSqlCommand("Delete from tblglassPicture where PictureID=" + ImageId);
                //var ProductImage = db.tblTemplatesImages.Where(a => a.ID == ImageId);
                //db.tblTemplatesImages.Remove(ProductImage);
                dbEntity.SaveChanges();

                List<tblglassPicture> ProductUploadedFiles = new List<tblglassPicture>();

                ProductUploadedFiles = dbEntity.tblglassPictures.Where(p => p.glassID == VCardid).ToList();
                return Json(ProductUploadedFiles, JsonRequestBehavior.AllowGet);
            }


            return Json("", JsonRequestBehavior.AllowGet);


        }
    }
}