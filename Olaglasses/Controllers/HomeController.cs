using Olaglasses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Olaglasses.Controllers
{
    public class HomeController : Controller
    {
        OlaGlassesEntities dbEntity = new OlaGlassesEntities();

        public ActionResult ProductFavourite(int? id = 0,string type="")
        {
            try
            {
                int UserID = 0;
                UserID = Convert.ToInt32(Session["UserID"]);
                if (UserID != 0)
                {
                    var ReviewLikeExist = dbEntity.tblFavourites.Where(a => a.ProductID == id && a.UserID == UserID);

                    if (ReviewLikeExist.Count() > 0)
                    {
                        var ReviewLike = dbEntity.tblFavourites.Where(a => a.ProductID == id && a.UserID == UserID);
                        dbEntity.tblFavourites.RemoveRange(ReviewLike);
                        dbEntity.SaveChanges();
                    }
                    else
                    {
                        tblFavourite tbl = new tblFavourite();
                        tbl.UserID = UserID;
                        tbl.ProductID = id;
                        tbl.CreateDate = DateTime.Now;
                        dbEntity.tblFavourites.Add(tbl);
                        dbEntity.SaveChanges();

                    }

                }
                if(type=="A")
                    return RedirectToAction("Accessories", "Home");
                if (type == "E")
                    return RedirectToAction("eyeglassess", "Home");
                if (type == "S")
                    return RedirectToAction("Sunglassess", "Home");
                if (type == "H")
                    return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Product_Details", "Products", new { Productid = id });
        }
        public ActionResult index(string sticker = "")
        {
            try
            {
                tblCoupon coupon = dbEntity.tblCoupons.Where(x => x.CouponStatus == "Active").FirstOrDefault();

                HttpCookie cookie = Request.Cookies["_CouponsColor"];
                if (cookie == null)
                {
                    cookie = new HttpCookie("_CouponsColor");
                    cookie.Value = Convert.ToString(coupon.CouponColor);
                    cookie.Expires = DateTime.Now.AddDays(30);
                }
                else { cookie.Value = Convert.ToString(coupon.CouponColor); }
              
                Response.Cookies.Add(cookie);
                HttpCookie cookie1 = Request.Cookies["_CouponsText"];
                if (cookie1 == null)
                {
                    cookie1 = new HttpCookie("_CouponsText");
                    cookie1.Value = Convert.ToString(coupon.Text);
                    cookie1.Expires = DateTime.Now.AddDays(30);
                }
                else { cookie1.Value = Convert.ToString(coupon.Text); }

                Response.Cookies.Add(cookie1);
                HttpCookie cookie2 = Request.Cookies["_CouponsLink"];
                if (cookie2 == null)
                {
                    cookie2 = new HttpCookie("_CouponsLink");
                    cookie2.Value = Convert.ToString(coupon.CouponColor);
                    cookie2.Expires = DateTime.Now.AddDays(30);
                }
                else{ cookie2.Value = Convert.ToString(coupon.CouponColor); }

                Response.Cookies.Add(cookie2);
                HttpCookie cookie3 = Request.Cookies["_CouponsDescription"];
                if (cookie3 == null)
                {
                    cookie3 = new HttpCookie("_CouponsDescription");
                    cookie3.Value = Convert.ToString(coupon.Description);
                    cookie3.Expires = DateTime.Now.AddDays(30);
                }
                else { cookie3.Value = Convert.ToString(coupon.Description); }
                Response.Cookies.Add(cookie3);
                Session["Coupon"] = dbEntity.tblCoupons.Where(x => x.CouponStatus == "Active").FirstOrDefault();
                //Replace session user here 
                int Userid = 0;
                Userid = Convert.ToInt32(Session["UserID"]);
                ViewBag.ProductList = dbEntity.Sp_Get_Product_List(Userid, sticker).ToList();

                ViewBag.ClientReviews = dbEntity.Sp_Get_Five_Reviews().ToList();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public ActionResult eyeglassess(string[] ddlgender, string[] ddlMaterial, string[] ddlAvailableSize, string[] ddlshape)
        {
            string cwhere = "";
            var genderfilters = "";
            var materialfilters = "";
            var availablesizesfilters = "";
            var shapesfilters = "";
            int Userid = 0;
            Userid = Convert.ToInt32(Session["UserID"]);
            try
            {
                //Dropdown List Values
                List<string> Gender = new List<string>();
                Gender.Add("Men");
                Gender.Add("Women");
                Gender.Add("Trans");
                ViewBag.Genderlist = Gender.ToList();

                List<string> AvailableSize = new List<string>();
                AvailableSize.Add("Medium");
                AvailableSize.Add("Large");
                AvailableSize.Add("Small");
                ViewBag.AvailableSize = AvailableSize.ToList();

                List<string> Material = new List<string>();
                Material.Add("Titanium");
                Material.Add("Plastic");
                Material.Add("Plastic Metal");
                ViewBag.Material = Material.ToList();

                List<string> shape = new List<string>();
                shape.Add("Round");
                shape.Add("Square");
                shape.Add("Aviator");
                shape.Add("Polygon");
                shape.Add("Cat Eye");
                ViewBag.shape = shape.ToList();

                ViewBag.Colors = dbEntity.tblVariations.Distinct().Select(x => x.ColorCode).ToList();

                //Maintaint state of all dropdown values
                ViewBag.StateGendervalue = ddlgender;
                ViewBag.StateMaterial = ddlMaterial;
                ViewBag.StateAvailableSize = ddlAvailableSize;
                ViewBag.Stateshape = ddlshape;


                //Split array in choma separate and quotes and pass to cwhere 
                //#Gender
                if (ddlgender != null)
                {
                    foreach (var item in ddlgender)
                    {
                        genderfilters += "'" + item + "'" + ",";
                    }
                    genderfilters = genderfilters.Remove(genderfilters.Length - 1, 1);
                    cwhere += " and   Gender in (" + genderfilters + ") ";
                }

                //#Material
                if (ddlMaterial != null)
                {
                    foreach (var item in ddlMaterial)
                    {
                        materialfilters += "'" + item + "'" + ",";
                    }
                    materialfilters = materialfilters.Remove(materialfilters.Length - 1, 1);
                    cwhere += " and   Material in (" + materialfilters + ") ";
                }

                //#Available Sizes
                if (ddlAvailableSize != null)
                {
                    foreach (var item in ddlAvailableSize)
                    {
                        availablesizesfilters += "'" + item + "'" + ",";
                    }
                    availablesizesfilters = availablesizesfilters.Remove(availablesizesfilters.Length - 1, 1);
                    cwhere += " and   AvailableSize in (" + availablesizesfilters + ") ";
                }

                //#Available Sizes
                if (ddlshape != null)
                {
                    foreach (var item in ddlshape)
                    {
                        shapesfilters += "'" + item + "'" + ",";
                    }
                    shapesfilters = shapesfilters.Remove(shapesfilters.Length - 1, 1);
                    cwhere += " and   shape in (" + shapesfilters + ") ";
                }



                //var materialfilters = "";
                //var availablesizesfilters = "";
                //var shapesfilters = "";
                cwhere += "and ProductCategory = 'Eyeglass' order by P.GlassID";
             
                ViewBag.ProductList = dbEntity.Sp_Get_Product_List_Filter("Eyeglasses", Userid,cwhere).ToList();
            }
            catch (Exception ex)
            {

            }
            return View();
        }



        public ActionResult Sunglassess(string[] ddlgender, string[] ddlMaterial, string[] ddlAvailableSize, string[] ddlshape)
        {
            try
            {
                string cwhere = "";
                var genderfilters = "";
                var materialfilters = "";
                var availablesizesfilters = "";
                var shapesfilters = "";
                int Userid = 0;
                Userid = Convert.ToInt32(Session["UserID"]);
                try
                {
                    //Dropdown List Values
                    List<string> Gender = new List<string>();
                    Gender.Add("Men");
                    Gender.Add("Women");
                    Gender.Add("Trans");
                    ViewBag.Genderlist = Gender.ToList();

                    List<string> AvailableSize = new List<string>();
                    AvailableSize.Add("Medium");
                    AvailableSize.Add("Large");
                    AvailableSize.Add("Small");
                    ViewBag.AvailableSize = AvailableSize.ToList();

                    List<string> Material = new List<string>();
                    Material.Add("Titanium");
                    Material.Add("Plastic");
                    Material.Add("Plastic Metal");
                    ViewBag.Material = Material.ToList();

                    List<string> shape = new List<string>();
                    shape.Add("Round");
                    shape.Add("Square");
                    shape.Add("Aviator");
                    shape.Add("Polygon");
                    shape.Add("Cat Eye");
                    ViewBag.shape = shape.ToList();

                    ViewBag.Colors = dbEntity.tblVariations.Distinct().Select(x => x.ColorCode).ToList();

                    //Maintaint state of all dropdown values
                    ViewBag.StateGendervalue = ddlgender;
                    ViewBag.StateMaterial = ddlMaterial;
                    ViewBag.StateAvailableSize = ddlAvailableSize;
                    ViewBag.Stateshape = ddlshape;


                    //Split array in choma separate and quotes and pass to cwhere 
                    //#Gender
                    if (ddlgender != null)
                    {
                        foreach (var item in ddlgender)
                        {
                            genderfilters += "'" + item + "'" + ",";
                        }
                        genderfilters = genderfilters.Remove(genderfilters.Length - 1, 1);
                        cwhere += " and   Gender in (" + genderfilters + ") ";
                    }

                    //#Material
                    if (ddlMaterial != null)
                    {
                        foreach (var item in ddlMaterial)
                        {
                            materialfilters += "'" + item + "'" + ",";
                        }
                        materialfilters = materialfilters.Remove(materialfilters.Length - 1, 1);
                        cwhere += " and   Material in (" + materialfilters + ") ";
                    }

                    //#Available Sizes
                    if (ddlAvailableSize != null)
                    {
                        foreach (var item in ddlAvailableSize)
                        {
                            availablesizesfilters += "'" + item + "'" + ",";
                        }
                        availablesizesfilters = availablesizesfilters.Remove(availablesizesfilters.Length - 1, 1);
                        cwhere += " and   AvailableSize in (" + availablesizesfilters + ") ";
                    }

                    //#Available Sizes
                    if (ddlshape != null)
                    {
                        foreach (var item in ddlshape)
                        {
                            shapesfilters += "'" + item + "'" + ",";
                        }
                        shapesfilters = shapesfilters.Remove(shapesfilters.Length - 1, 1);
                        cwhere += " and   shape in (" + shapesfilters + ") ";
                    }



                    //var materialfilters = "";
                    //var availablesizesfilters = "";
                    //var shapesfilters = "";
                    cwhere += "and ProductCategory = 'Sunglass' order by P.GlassID";
                   
                    ViewBag.ProductList = dbEntity.Sp_Get_Product_List_Filter("Sunglasses",Userid, cwhere).ToList();
                }
                catch (Exception ex)
                {

                }
                return View();
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult Accessories()
        {
            try
            {
                string cwhere = "";
                int Userid = 0;
                Userid = Convert.ToInt32(Session["UserID"]);
                try
                {


                    //var materialfilters = "";
                    //var availablesizesfilters = "";
                    //var shapesfilters = "";
                    cwhere = "and ProductCategory = 'Accessory' order by P.GlassID";
                   
                    ViewBag.ProductList = dbEntity.Sp_Get_Product_List_Filter("Accessory", Userid, cwhere).ToList();
                }
                catch (Exception ex)
                {

                }
                return View();
            }
            catch (Exception ex)
            {

            }
            return View();
        }


        public ActionResult OurStory()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult News()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult Contact()
        {
            try
            {
                ViewBag.message = "";
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [HttpPost]
        public ActionResult Contact(string email, string fname, string lname, string subject, string message, string phone)
        {
            try
            {

                //Guid activationCode = Guid.NewGuid();

                tblSetting sa = dbEntity.tblSettings.Find(3);
                using (MailMessage mm = new MailMessage(sa.Email, sa.Email))
                {
                    
                    mm.Subject = subject;
                    string body = "Hello,";
                    body += "<br /><br />You received a message from " + fname + " " + lname + "(" + email + ")";
                    body += "<br /><br />Phone Number: " + phone + "<br /><br />";
                    body += message;
                    mm.Body = body;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = sa.SMTP;
                    smtp.EnableSsl = Convert.ToBoolean(sa.IsActive);
                    NetworkCredential NetworkCred = new NetworkCredential(sa.Email, sa.Password);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = Convert.ToInt32(sa.Port);
                    smtp.Send(mm);
                }
                ViewBag.message = "We have received your message.Contact you soon";
                return View();
            }
            catch (Exception ex)
            {
                return View();
                throw;
            }

        }

    }
}