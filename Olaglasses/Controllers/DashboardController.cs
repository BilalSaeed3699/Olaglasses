using Olaglasses.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Olaglasses.Controllers
{
    [FilterConfig.NoDirectAccess]
    [FilterConfig.AuthorizeActionFilter]
    public class DashboardController : Controller
    {
        OlaGlassesEntities dbEntity = new OlaGlassesEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Users()
        {
            List<tblUser> users = new List<tblUser>();
            users = dbEntity.tblUsers.ToList();
            ViewBag.usersList = users;
            return View();
        }

        public ActionResult OrderDetails(int? id=1)
        {
            List<spgetorderDetails_Result4> spgetorderDetails = dbEntity.spgetorderDetails(id).ToList();
            ViewBag.OrderDetails = spgetorderDetails;
            return View();

        }

        [HttpPost]
        public ActionResult Orderstatusupdate(int? id,int? userid, string ostatus = "", string status = "")
        {
            tblOrder od = dbEntity.tblOrder.Where(o => o.OrderID == id).FirstOrDefault();
            od.Status = ostatus;

            dbEntity.SaveChanges();
            return Json(new {url = Url.Action("Orders", "Dashboard", new { id = userid,status= status }) });
        }

        public ActionResult Orders(int? id,string status="All")
        {
            ViewBag.userid = id;
            List<string> OrderStatus = new List<string>();
            OrderStatus.Add("Not Started");
            OrderStatus.Add("InProgress");
            OrderStatus.Add("Complete");
            OrderStatus.Add("Delivered");
            OrderStatus.Add("Remake");
            ViewBag.OrderStats = OrderStatus.ToList();
            ViewBag.Type = status;
            List<tblOrder> orders = new List<tblOrder>();
            if (id == 1)
            {
                if (status != "All")
                {
                    orders = dbEntity.tblOrder.Where(x=> x.Status == status).ToList();
                }
                else
                {
                    orders = dbEntity.tblOrder.ToList();
                }
            }
            else {
                if (status != "All")
                {
                    orders = dbEntity.tblOrder.Where(x => x.userID == id && x.Status == status).ToList();
                }
                else
                {
                    orders = dbEntity.tblOrder.Where(x => x.userID == id).ToList();
                }
            }
            ViewBag.ordersList = orders;
            ViewBag.UserID = id;
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult PersonalInfo(int? id,string Ismessage)
        {
            ViewBag.IsError = Ismessage;
            tblUser user = new tblUser();
            if(id==0)
            {
                var path = "/ProjectImages/UserImages/user.jpg";
                user.UserEmail = "";
                user.Address = "";
                user.Phone = "";
                user.Firstname = "";
                user.Lastname = "";
                user.UserImage = path;
                user.Gender = "";

            }
            else
            {
                
                user = dbEntity.tblUsers.Find(id);
                if(user.UserImage==null)
                {
                    var path = "/ProjectImages/UserImages/user.jpg";
                    user.UserImage = path;
                }
                
            }
            return View(user);
        }

        [HttpPost]
        public ActionResult PersonalInfo(tblUser user, HttpPostedFileBase Image)
        {
            var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", ".jpeg" };

            if (Image != null)
            {
                var ext = Path.GetExtension(Image.FileName);
                string myfile = user.Firstname + "_" + user.UserID + ext;
                var path = Path.Combine(Server.MapPath("~/ProjectImages/UserImages"), myfile);
                var path1 = Path.Combine(("\\ProjectImages\\UserImages"), myfile);
                user.UserImage = path1;
                Image.SaveAs(path);
            }
            using (var context = new OlaGlassesEntities())
            {
                user.UserStatus = 1;
                user.CreatedDate = DateTime.Now;
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
            Session["UserImage"] = user.UserImage;
            Session["userName"] = user.Firstname + " " + user.Lastname;
            ViewBag.IsError = "User Updated Successfully";
            return RedirectToAction("PersonalInfo", new { id=user.UserID, Ismessage= "Personal Info Updated Successfully" });
        }
        [HttpPost]
        public ActionResult ChangePassword(string oldPassword, string newPassword)
        {
            try
            {
                string encryptedpassword = Encrypt(oldPassword, "sblw-3hn8-sqoy19");
                string Email = Session["Email"].ToString();
                tblUser User = dbEntity.tblUsers.Where(x => x.UserPass == encryptedpassword && x.UserEmail == Email).FirstOrDefault();
                if (User == null)
                {
                    ViewBag.IsError = "Your old Password is incorrect.";
                    return View();
                }
                else
                {
                    string encryptedpassword1 = Encrypt(newPassword, "sblw-3hn8-sqoy19");
                    User.UserPass = encryptedpassword1;
                    dbEntity.Entry(User).State = EntityState.Modified;
                    dbEntity.SaveChanges();
                    ViewBag.IsSuccess = "Password change successfully.";
                    return View();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public static string Encrypt(string input, string key)
        {
            byte[] resultArray = { };
            try
            {
                byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateEncryptor();
                resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception ex)
            {

            }
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);

        }

        public ActionResult Favourites(int? id)
        {
            List<Sp_Get_Favourite_List_Result> favourites = dbEntity.Sp_Get_Favourite_List(id).ToList();
            ViewBag.favouritesList = favourites;
            ViewBag.UserID = id;
            return View();
        }

        public ActionResult Unfavourite(int? id,int userID)
        {
            dbEntity.Database.ExecuteSqlCommand("Delete from tblFavourite where FavouriteID="+id);
            dbEntity.SaveChanges();
            return RedirectToAction("Favourites", new { id= userID });
        }
    }
}