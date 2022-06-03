using Olaglasses.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Olaglasses.Controllers
{

    [FilterConfig.NoDirectAccess]
    [FilterConfig.AuthorizeActionFilter]
    public class ColorsController : Controller
    {
        OlaGlassesEntities dbEntity = new OlaGlassesEntities();
        public ActionResult Index(int? id,string message)
        {
            ViewBag.Color = dbEntity.tblColors.ToList();
            tblColors product = new tblColors();
            if (id > 0)
            {
                product = dbEntity.tblColors.Find(id);



            }
            else
            {
                product.ColorID = 0;
                product.hexacode = "";
                product.colorname = "";


            }
            ViewBag.message = message;
            return View(product);
           
          
        }
        public ActionResult Cancel()
        {
           
            return RedirectToAction("Index");


        }
        [HttpPost]
        public ActionResult Create(tblColors product)
        {
            if (product.hexacode != null)
            {
                if (product.ColorID > 0)
                {
                    dbEntity.Entry(product).State = EntityState.Modified;
                    dbEntity.SaveChanges();

                    return RedirectToAction("Index", new { message = "Record Updated Successfully" });

                }
                else
                {
                    dbEntity.tblColors.Add(product);
                    dbEntity.SaveChanges();          
                    return RedirectToAction("Index", new { message = "Record Added Successfully" });
                }
            }
            return RedirectToAction("Create", new { id = 0 });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteData(int? id)
        {
            try
            {

                tblColors make = dbEntity.tblColors.Find(id);
                if (make != null)
                {
                    dbEntity.tblColors.Remove(make);
                    dbEntity.SaveChanges();
                }


                return RedirectToAction("Index", new { message = "Record Deleted Successfully" });
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}