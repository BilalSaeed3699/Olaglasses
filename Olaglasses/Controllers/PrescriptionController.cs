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
    public class PrescriptionController : Controller
    {
        // GET: Prescription
        OlaGlassesEntities dbEntity = new OlaGlassesEntities();
        public ActionResult Index(int? id, string message)
        {
            ViewBag.message = message;
            ViewBag.UserID = id;
            List<tblUserPrescription> prescriptions = new List<tblUserPrescription>();
            prescriptions = dbEntity.tblUserPrescription.Where(x => x.UserID == id).ToList();
            ViewBag.PrescriptionList = prescriptions;
            return View();
        }

        public ActionResult Create(int? id,int UserID)
        {
            ViewBag.SPH = dbEntity.tblSizes.Where(x => x.Type == "SPH").ToList();
            ViewBag.CYL = dbEntity.tblSizes.Where(x => x.Type == "CYL").ToList();
            ViewBag.ADD = dbEntity.tblSizes.Where(x => x.Type == "ADD").ToList();
            ViewBag.PD = dbEntity.tblSizes.Where(x => x.Type == "PD").ToList();

            tblUserPrescription product = new tblUserPrescription();
            if (id > 0)
            {
                product = dbEntity.tblUserPrescription.Find(id);

            }
            else
            {
                var path = "/ProjectImages/Variations/placeholder.jpg";
                product.r_sph = 0;
                product.r_cyl = 0;
                product.r_axis = 0;
                product.r_add = 0;
                product.l_sph = 0;
                product.l_cyl = 0;
                product.l_axis = 0;
                product.l_add = 0;
                product.PD = 0;
                product.Fname = "";
                product.Lname = "";
                product.prescriptionDate = DateTime.Now;
                product.PrescriptionImage = path;
                product.UserID = UserID;

            }
            return View(product);
        }
        [HttpPost]
        public ActionResult Create(tblUserPrescription product, HttpPostedFileBase Image)
        {
            if (Image != null)
            {
                //var ext = Path.GetExtension(Image.FileName);

                var path = Path.Combine(Server.MapPath("~/ProjectImages/Prescription"), Image.FileName.Replace(" ", "_"));
                var path1 = Path.Combine(("\\ProjectImages\\Prescription"), Image.FileName.Replace(" ", "_"));
                product.PDImage = path1;
                Image.SaveAs(path);
            }

            if (product.PrescriptionID > 0)
            {
                dbEntity.Entry(product).State = EntityState.Modified;
                dbEntity.SaveChanges();
                return RedirectToAction("Index", new { id = product.UserID, message = "Record Updated Successfully" });

            }
            else
            {
                dbEntity.tblUserPrescription.Add(product);
                dbEntity.SaveChanges();
                return RedirectToAction("Index", new { id=product.UserID, message = "Record Added Successfully" });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteData(int? id,int UserID)
        {
            try
            {

                tblUserPrescription make = dbEntity.tblUserPrescription.Find(id);
                if (make != null)
                {
                    dbEntity.tblUserPrescription.Remove(make);
                    dbEntity.SaveChanges();
                }


                return RedirectToAction("Index", new {id= UserID, message = "Record Deleted Successfully" });
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}