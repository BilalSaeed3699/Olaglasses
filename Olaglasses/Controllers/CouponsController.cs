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
    public class CouponsController : Controller
    {
        // GET: Coupons
        OlaGlassesEntities dbEntity = new OlaGlassesEntities();
        public ActionResult Index(string message)
        {
            ViewBag.message = message;
            List<tblCoupon> coupons = new List<tblCoupon>();
            coupons = dbEntity.tblCoupons.ToList();
            ViewBag.couponsList = coupons;
            return View();
        }
        public ActionResult Create(int? id)
        {
            tblCoupon product = new tblCoupon();
            if (id > 0)
            {
                product = dbEntity.tblCoupons.Find(id);

            }
            else
            {
                product.CouponCode = "";
                product.CouponColor = "";
                product.CouponLink = "";
                product.CouponStatus = "";
                product.Discount = 0;
                product.Description = "";
                

            }
            return View(product);
        }
        [HttpPost]
        public ActionResult Create(tblCoupon product)
        {


            if (product.CouponID > 0)
            {
                dbEntity.Entry(product).State = EntityState.Modified;
                dbEntity.SaveChanges();
                return RedirectToAction("Index", new { message = "Record Updated Successfully" });

            }
            else
            {
                dbEntity.tblCoupons.Add(product);
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

                tblCoupon make = dbEntity.tblCoupons.Find(id);
                if (make != null)
                {
                    dbEntity.tblCoupons.Remove(make);
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