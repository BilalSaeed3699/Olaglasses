using Olaglasses.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Olaglasses.Controllers
{
    public class ProductsController : Controller
    {

        private OlaGlassesEntities dbEntities = new OlaGlassesEntities();
        // GET: Products

        //Old Backup 2022-01-01
        //public ActionResult Product_Details(int? Productid)
        //{
        //    try
        //    {
        //        //Get userid from session
        //        int Userid = 1;
        //        ViewBag.Reviewslist = dbEntities.tblReviews.Where(a => a.GlassID == Productid).ToList();
        //        ViewBag.Reviewscount = dbEntities.tblReviews.Where(a => a.GlassID == Productid).Count();
        //        ViewBag.Reviewsimagescount = dbEntities.tblReviews.Where(a => a.GlassID == Productid && a.ReviewImage != null).Count();

        //        var RatingSum = dbEntities.tblReviews.Where(a => a.GlassID == Productid).Select(a => a.Rating).Sum();
        //        var RatingCount = dbEntities.tblReviews.Where(a => a.GlassID == Productid).Count();
        //        RatingCount = RatingCount * 5;

        //        int AverageRating = 0;
        //        AverageRating = Convert.ToInt32((RatingSum * 5) / RatingCount);
        //        ViewBag.AverageRating = AverageRating;

        //        ViewBag.ReviewsLike = dbEntities.tblReviewLikes.Where(a => a.UserID == Userid && a.Productid == Productid).ToList();
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return View();
        //}
        public ActionResult Product_Details(int? Productid)
        {
            try
            {
                //Get userid from session
                int Userid = 0;


                Userid = Convert.ToInt32(Session["UserID"]);
                ViewBag.Isfav = dbEntities.tblFavourites.Where(x => x.ProductID == Productid && x.UserID == Userid).FirstOrDefault();
                ViewBag.Reviewslist = dbEntities.tblReviews.Where(a => a.GlassID == Productid).ToList();
                ViewBag.Reviewscount = dbEntities.tblReviews.Where(a => a.GlassID == Productid).Count();
                ViewBag.Reviewsimagescount = dbEntities.tblReviews.Where(a => a.GlassID == Productid && a.ReviewImage != null).Count();

                var RatingSum = dbEntities.tblReviews.Where(a => a.GlassID == Productid).Select(a => a.Rating).Sum();
                var RatingCount = dbEntities.tblReviews.Where(a => a.GlassID == Productid).Count();
                RatingCount = RatingCount * 5;

                int AverageRating = 0;
                AverageRating = Convert.ToInt32((RatingSum * 5) / RatingCount);
                ViewBag.AverageRating = AverageRating;

                ViewBag.ReviewsLike = dbEntities.tblReviewLikes.Where(a => a.UserID == Userid && a.Productid == Productid).ToList();



                tblProduct product = dbEntities.tblProducts.Find(Productid);
                if (product.ProductCategory == "Accessory")
                {
                    ViewBag.Variation = dbEntities.tblglassPictures.Where(x => x.glassID == Productid).ToList();
                }
                else
                {
                    ViewBag.Variation = dbEntities.tblVariations.Where(x => x.ProductID == Productid).ToList();
                }

                Userid = Convert.ToInt32(Session["UserID"]);
                ViewBag.Userid = Userid;
                ViewBag.ProductList = dbEntities.Sp_Get_Product_List(Userid, "").ToList();

                ViewBag.ClientReviews = dbEntities.Sp_Get_Five_Reviews().ToList();
                return View(product);
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult ProductFavourite(int? id = 0)
        {
            try
            {
                int UserID = 0;
                UserID = Convert.ToInt32(Session["UserID"]);
                if (UserID != 0)
                {
                    var ReviewLikeExist = dbEntities.tblFavourites.Where(a => a.ProductID == id && a.UserID == UserID);

                    if (ReviewLikeExist.Count() > 0)
                    {
                        var ReviewLike = dbEntities.tblFavourites.Where(a => a.ProductID == id && a.UserID == UserID);
                        dbEntities.tblFavourites.RemoveRange(ReviewLike);
                        dbEntities.SaveChanges();
                    }
                    else
                    {
                        tblFavourite tbl = new tblFavourite();
                        tbl.UserID = UserID;
                        tbl.ProductID = id;
                        tbl.CreateDate = DateTime.Now;
                        dbEntities.tblFavourites.Add(tbl);
                        dbEntities.SaveChanges();

                    }

                }
                return RedirectToAction("Product_Details", "Products", new { Productid = id });
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Product_Details", "Products", new { Productid = id });
        }

        public ActionResult UpdateLikes(int? ReviewID = 0, int? GlassID = 0)
        {
            try
            {
                int UserID = 0;
                UserID = Convert.ToInt32(Session["UserID"]);
                var ReviewLikeExist = dbEntities.tblReviewLikes.Where(a => a.Productid == GlassID && a.ReviewID == ReviewID);

                if (ReviewLikeExist.Count() > 0)
                {
                    var ReviewLike = dbEntities.tblReviewLikes.Where(a => a.Productid == GlassID && a.ReviewID == ReviewID);
                    dbEntities.tblReviewLikes.RemoveRange(ReviewLike);
                    dbEntities.SaveChanges();
                }
                else if (ReviewLikeExist.Count() == 0)
                {

                    tblReviewLike tbl = new tblReviewLike();
                    tbl.UserID = UserID;
                    tbl.ReviewID = ReviewID;
                    tbl.Productid = GlassID;
                    dbEntities.tblReviewLikes.Add(tbl);
                    dbEntities.SaveChanges();
                }

                return RedirectToAction("Product_Details", "Products", new { Productid = GlassID });
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Product_Details", "Products", new { Productid = GlassID });
        }

        [HttpPost]
        public ActionResult SaveReview(string UserID, string GlassID, string Rating, HttpPostedFileBase[] ReviewImage, string Review = "")
        {
            try
            {
                tblReview review = new tblReview();
                string MainRoot = "";
                review.UserID = Convert.ToInt32(UserID);
                review.GlassID = Convert.ToInt32(GlassID);
                review.Rating = Convert.ToInt32(Rating);
                review.UserName = Convert.ToString(Session["userName"]);
                review.Review = Review;
                review.CreatedDate = DateTime.Now;


                review.UserImage = Convert.ToString(Session["UserImage"]);
                //review.ReviewImage = "/ProjectImages/Variations/placeholder.jpg";
                foreach (HttpPostedFileBase image in ReviewImage)
                {

                    if (image != null)
                    {


                        var ReviewFileName = Path.GetFileName(image.FileName);
                        MainRoot = "/ProjectImages/Uploads/Reviews/" + review.UserID;

                        bool exists = System.IO.Directory.Exists(Server.MapPath(MainRoot));
                        if (!exists)
                            System.IO.Directory.CreateDirectory(Server.MapPath(MainRoot));



                        var ReviewServerFilePath = Path.Combine(Server.MapPath(MainRoot) + "\\" + ReviewFileName);

                        MainRoot = "/ProjectImages/Uploads/Reviews/" + review.UserID + "/" + ReviewFileName;

                        review.ReviewImage = MainRoot;
                        //Save Files to path
                        image.SaveAs(ReviewServerFilePath);

                    }
                }


                dbEntities.tblReviews.Add(review);
                dbEntities.SaveChanges();
                return RedirectToAction("Product_Details", "Products", new { Productid = Convert.ToInt32(GlassID) });
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Exception Occur While Saving Broker " + ex.Message;
            }

            return RedirectToAction("Product_Details", "Products", new { Productid = Convert.ToInt32(GlassID) });
        }
        [HttpGet]
        public ActionResult Deletecart(int? Cartid)
        {
            try
            {
                tblcart c = dbEntities.tblcart.Find(Cartid);
                int userID = c.Userid;
                dbEntities.tblcart.Remove(c);
                dbEntities.SaveChanges();
                return RedirectToAction("MovetoOrdercart", "Products", new { id = userID });
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("MovetoOrdercart", "Products", new { Userid = 1 });
        }




        [HttpGet]
        public ActionResult Orderprocessing(int? Productid,int? VariationId,string Image,string size,int? USID,string Productcategory="")
        {
            try
            {
                ViewBag.SPH = dbEntities.tblSizes.Where(x => x.Type == "SPH").ToList();
                ViewBag.CYL = dbEntities.tblSizes.Where(x => x.Type == "CYL").ToList();
                ViewBag.ADD = dbEntities.tblSizes.Where(x => x.Type == "ADD").ToList();
                ViewBag.PD = dbEntities.tblSizes.Where(x => x.Type == "PD").ToList();
                tblProduct pd = dbEntities.tblProducts.Find(Productid);
                ViewBag.Image = Image;
                ViewBag.size = size;
                string colors = dbEntities.tblVariations.Find(VariationId).Color1;
                
                ViewBag.colorname = dbEntities.tblColors.Where(a=> a.hexacode== colors).Select(a=>a.colorname).FirstOrDefault();
                ViewBag.UserPrescription = dbEntities.tblUserPrescription.Where(a => a.UserID == USID).ToList();
                ViewBag.VariationId = VariationId;
                ViewBag.UserID = USID;
                
                if(pd.shape==null)
                {
                    pd.shape = "";

                }
                ViewBag.Shape = pd.shape;
                ViewBag.Productcategory = Productcategory;

                double FrameAWidth = Convert.ToDouble(dbEntities.tblVariations.Find(VariationId).FrameAWidth);
                string FrameDBBridger = dbEntities.tblVariations.Find(VariationId).FrameDBBridger;
                string FrameED = dbEntities.tblVariations.Find(VariationId).FrameED;

                ViewBag.FrameAWidth = FrameAWidth;
                ViewBag.FrameDBBridger = FrameDBBridger;
                ViewBag.FrameED = FrameED;
                return View(pd);
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [HttpPost]
        public ActionResult MovetoOrderprocessing(int? Productid, string color, string size)
        {
            try
            {

                return Json(new { url = Url.Action("Orderprocessing", "Products", new { Productid = Productid, color = color, size = size }) });
            }
            catch (Exception ex)
            {

            }
            return Json(new { url = Url.Action("Orderprocessing", "Products", new { Productid = Productid, color = color, size = size }) });
        }

        [HttpGet]
        public ActionResult MovetoOrdercart(int? id)
        {
            try
            {
                string cwhere = "";
                ViewBag.Usercartdetails = dbEntities.tblcart.Where(a => a.Userid == id).ToList();
                cwhere = "and ProductCategory = 'Accessory'";
                ViewBag.ProductList = dbEntities.Sp_Get_Product_List_Filter("Accessory", id, cwhere).ToList();
                ViewBag.Userid = id;
                return View();
            }
            catch (Exception ex)
            {

            }
            return View();
        }

       

        public ActionResult AddtoCart(int ProductID, float price, string Name, string Image, string color, string size, int? type = 0, int UserID = 0)
        {
            try
            {
                tblProduct product = dbEntities.tblProducts.Find(ProductID);
                tblcart cart = new tblcart();
                cart.Userid = UserID;
                cart.GlassID = ProductID;
                cart.ProductImagePath = Image;
                cart.Price = price;
                cart.Title = Name;
                cart.UVProtection = "";
                cart.LensType = "";
                cart.VisionType = "";
                cart.Quantity = 1;
                cart.TotalAmount = price;
                cart.Productcategory = "Accessory";
                cart.Shippingamount = 9;

                cart.Colour = color;
                cart.Size = size;
                dbEntities.tblcart.Add(cart);
                dbEntities.SaveChanges();


                return RedirectToAction("MovetoOrdercart", "Products", new { id = UserID });
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("MovetoOrdercart", "Products", new { id = UserID });
        }


        [HttpPost]
        public ActionResult OrderprocessingAddtoCart(tblcart cartdata, int? isusedsaveprescription = 0, int? Actioncalltype = 0)
        {
            try
            {

                tblcart cart = new tblcart();
                dbEntities.tblcart.Add(cartdata);
                dbEntities.SaveChanges();
                


                return Json(new { url = Url.Action("MovetoOrdercart", "Products", new { id = cartdata.Userid }) });
            }
            catch (Exception ex)
            {

            }
            return Json(new { url = Url.Action("MovetoOrdercart", "Products", new { Userid = cartdata.Userid }) });
        }


        [HttpPost]
        public ActionResult MoveprocessingtoOrdercart(tblcart cart)
        {
            try
            {
                dbEntities.tblcart.Add(cart);
                dbEntities.SaveChanges();
                int  Userid = cart.Userid;

                return Json(new { url = Url.Action("MovetoOrdercart", "Products", new { Userid = Userid }) });
            }
            catch (Exception ex)
            {

            }
            return Json(new { url = Url.Action("Orderprocessing", "Products", new { Usercartid = 1 }) });
        }


        [HttpPost]
        public ActionResult Applycoupon(string Couponcode)
        {
            tblCoupon cp = new tblCoupon();
            try
            {

                Couponcode = Couponcode.Trim();
                cp = dbEntities.tblCoupons.Where(a => a.CouponCode == Couponcode && a.CouponStatus == "Active").FirstOrDefault();

                if (cp == null)
                {
                    return Json("-1", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(cp.Discount, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception ex)
            {
                return Json(new { cp, data = ex.Message });
            }

        }


        [HttpPost]
        public ActionResult Updatecart(string updatetype, int? cartid, int? quantity, int? subtotal, double TaxAmount, double Discount, double Shippingamount)
        {
            tblcart cp = new tblcart();
            try
            {


                cp = dbEntities.tblcart.Find(cartid);

                if (updatetype == "EL")
                {
                    cp.LensType = "Enhanced";
                }
                if (updatetype == "SL")
                {
                    cp.LensType = "Standard";
                }
                if (updatetype == "RE")
                {
                    cp.VisionType = "Reading";
                }
                if (updatetype == "DI")
                {
                    cp.VisionType = "Distance";
                }
                if (updatetype == "UV")
                {
                    if (cp.UVProtection == "on")
                    {
                        cp.UVProtection = "off";
                    }
                    else if (cp.UVProtection == "off")
                    {
                        cp.UVProtection = "on";
                    }

                }

                cp.Quantity = Convert.ToInt32(quantity);
                cp.TotalAmount = Convert.ToDouble(subtotal);

                cp.Shippingamount = Shippingamount;
                cp.TaxAmount = TaxAmount;
                cp.Discount = Discount;
                dbEntities.SaveChanges();
                return Json("1", JsonRequestBehavior.AllowGet);



            }
            catch (Exception ex)
            {
                return Json(new { cp, data = ex.Message });
            }

        }




        //comment backup bilal
        //public ActionResult UpdateLikes(int? ReviewID = 0, int? GlassID = 0)
        //{
        //    try
        //    {
        //        var ReviewLikeExist = dbEntities.tblReviewLikes.Where(a => a.Productid == GlassID && a.ReviewID == ReviewID);

        //        if (ReviewLikeExist.Count() > 0)
        //        {
        //            var ReviewLike = dbEntities.tblReviewLikes.Where(a => a.Productid == GlassID && a.ReviewID == ReviewID);
        //            dbEntities.tblReviewLikes.RemoveRange(ReviewLike);
        //            dbEntities.SaveChanges();
        //        }
        //        else if (ReviewLikeExist.Count() == 0)
        //        {
        //            tblReviewLike tbl = new tblReviewLike();
        //            tbl.UserID = 1;
        //            tbl.ReviewID = ReviewID;
        //            tbl.Productid = GlassID;
        //            dbEntities.tblReviewLikes.Add(tbl);
        //            dbEntities.SaveChanges();
        //        }

        //        return RedirectToAction("Product_Details", "Products", new { Productid = GlassID });
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return RedirectToAction("Product_Details", "Products", new { Productid = GlassID });
        //}

        //[HttpPost]
        //public ActionResult SaveReview(string UserID, string GlassID, string Rating, HttpPostedFileBase[] ReviewImage, string Review = "")
        //{
        //    try
        //    {
        //        tblReview review = new tblReview();
        //        string MainRoot = "";
        //        review.UserID = 1;
        //        review.GlassID = 2;
        //        review.Rating = Convert.ToInt32(Rating);
        //        review.Review = Review;
        //        review.CreatedDate = DateTime.Now;




        //        foreach (HttpPostedFileBase image in ReviewImage)
        //        {

        //            if (image != null)
        //            {


        //                var ReviewFileName = Path.GetFileName(image.FileName);
        //                MainRoot = "/Uploads/Reviews/" + review.UserID;

        //                bool exists = System.IO.Directory.Exists(Server.MapPath(MainRoot));
        //                if (!exists)
        //                    System.IO.Directory.CreateDirectory(Server.MapPath(MainRoot));



        //                var ReviewServerFilePath = Path.Combine(Server.MapPath(MainRoot) + "\\" + ReviewFileName);

        //                MainRoot = "/Uploads/Reviews/" + review.UserID + "/" + ReviewFileName;

        //                review.ReviewImage = MainRoot;
        //                //Save Files to path
        //                image.SaveAs(ReviewServerFilePath);

        //            }
        //        }


        //        dbEntities.tblReviews.Add(review);
        //        dbEntities.SaveChanges();
        //        return RedirectToAction("Product_Details", "Products", new { Productid = 1 });
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Error = "Exception Occur While Saving Broker " + ex.Message;
        //    }

        //    return RedirectToAction("Product_Details", "Products", new { Productid = 1 });
        //}

    }
}