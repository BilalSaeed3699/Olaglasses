using Newtonsoft.Json;
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
    public class GlassessController : Controller
    {
        OlaGlassesEntities dbEntity = new OlaGlassesEntities();
        public ActionResult Index(string message)
        {
            ViewBag.message = message;
            List<tblProduct> products = new List<tblProduct>();
            products = dbEntity.tblProducts.Where(x => x.ProductCategory != "Accessory").ToList();
            ViewBag.ProductList = products;
            return View();
        }
        public ActionResult Create(int? id)
        {
            tblProduct product = new tblProduct();
            if (id>0)
            {
                 product = dbEntity.tblProducts.Find(id);

            }
            else
            {
                product.Title = "";
                product.shape = "";
                product.Colour = "";
                product.Material = "";
                product.Brand = "";
                product.Gender = "";
                product.AdditionalInfo = "";
                product.AvailableSize = "";
                product.Price =0;
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
            List<tblProduct> tblProducts = dbEntity.tblProducts.Where(x => x.ProductCategory != "Accessory").ToList();
            ViewBag.ProductList = tblProducts;
            return View(product);
        }
        [HttpPost]
        public ActionResult Create(tblProduct product)
        {

            
            if (product.GlassID>0)
            {
                dbEntity.Entry(product).State = EntityState.Modified;
                dbEntity.SaveChanges();
                return RedirectToAction("Index", new { message = "Record Updated Successfully" });

            }
            else
            {
                dbEntity.tblProducts.Add(product);
                dbEntity.SaveChanges();
                return RedirectToAction("Index", new { message = "Record Added Successfully" });
            }
            
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
                   
               
                return RedirectToAction("Index" , new { message="Record Deleted Successfully"});
            }
            catch (Exception)
            {

                throw;
            }

        }





        public ActionResult VariationIndex(int id,string message)
        {
            ViewBag.message = message;
            List<tblVariation> variations = new List<tblVariation>();
            variations = dbEntity.tblVariations.Where(x => x.ProductID == id).ToList();
            tblProduct p = dbEntity.tblProducts.Find(id);
            ViewBag.Productname = p.Title;
            ViewBag.GlassID = p.GlassID;
            ViewBag.variationstList = variations;
            return View();
        }
        public ActionResult VariationCreate(int? id, int productid)
        {
            ViewBag.color = dbEntity.tblColors.ToList();
            tblVariation product = new tblVariation();
            if (id > 0)
            {
                product = dbEntity.tblVariations.Find(id);

            }
            else
            {
                var path = "/ProjectImages/Variations/placeholder.jpg";
                product.VariationID = 0;
                product.ColorCode = "";
                product.Color1 = "";
                product.Color2 = "";
                product.size = "";
                product.FrameAWidth = 0;
                product.FrameBHeight = 0;
                product.FrameED = "";
                product.FrameDBBridger = "";
                product.FrameTempleLegs = "";
                product.FrameTotalWidth ="";
                product.MinPDNeg = "";
                product.MinPDPositive = "";
                product.ImagePath = path;
                product.ProductID = productid;


            }

            return View(product);
        }
        [HttpPost]
        public ActionResult VariationCreate(tblVariation product, HttpPostedFileBase Image)
        {

            if (Image != null)
            {
                //var ext = Path.GetExtension(Image.FileName);
               
                var path = Path.Combine(Server.MapPath("/ProjectImages/Variations/"), Image.FileName.Replace(" " ,"_"));
                var path1 = Path.Combine(("/ProjectImages/Variations/"), Image.FileName.Replace(" ", "_"));
                product.ImagePath = path1;
                Image.SaveAs(path);
            }
            if (product.VariationID > 0)
            {
                dbEntity.Entry(product).State = EntityState.Modified;
                dbEntity.SaveChanges();
                return RedirectToAction("VariationIndex", new { id = product.ProductID,  message = "Record Updated Successfully" });

            }
            else
            {
                dbEntity.tblVariations.Add(product);
                dbEntity.SaveChanges();
                return RedirectToAction("VariationIndex", new { id = product.ProductID, message = "Record Added Successfully" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VariationDeleteData(int? id,int Productid)
        {
            try
            {

                tblVariation make = dbEntity.tblVariations.Find(id);
                if (make != null)
                {
                    dbEntity.tblVariations.Remove(make);
                    dbEntity.SaveChanges();
                }


                return RedirectToAction("VariationIndex", new { id=Productid,message = "Record Deleted Successfully" });
            }
            catch (Exception)
            {

                throw;
            }

        }


        public  JsonResult GetCoode(string code,int glassID)
        {
            OlaGlassesEntities dbEntity1 = new OlaGlassesEntities();
            int ret = 0;

            List<tblVariation> variation1 = dbEntity1.tblVariations.ToList();
            tblVariation variation = dbEntity1.tblVariations.Where(x => x.ColorCode == code && x.ProductID == glassID).FirstOrDefault();
            if(variation!=null)
            {
                ret = 1;
            }
            
            return Json(ret,JsonRequestBehavior.AllowGet);
        }
    }
}