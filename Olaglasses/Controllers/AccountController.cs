using Olaglasses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Olaglasses.Controllers
{
    [RequireHttps]
    public class AccountController : Controller
    {
        private OlaGlassesEntities dbEntities = new OlaGlassesEntities();
        Utility ut = new Utility();
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
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["UserID"] = null;
            Session["userName"] = null;
            Session["Email"] = null;
            Session["Role"] = null;
            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public ActionResult UserLogin(tblUser UserInformation)
        {
            tblUser us = new tblUser();
            var UserLoginReult = "";
            try
            {
                string encryptedpassword = Encrypt(UserInformation.UserPass, "sblw-3hn8-sqoy19");
                us = dbEntities.tblUsers.Where(a => a.UserEmail == UserInformation.UserEmail).FirstOrDefault();

                if (us == null)
                {
                    return Json(new { status = "-1", url = Url.Action("Index", "Home") });

                }
                us = dbEntities.tblUsers.Where(a => a.UserEmail == UserInformation.UserEmail && a.UserPass == encryptedpassword).FirstOrDefault();

                if (us == null)
                {
                    return Json(new { status = "-2", url = Url.Action("Index", "Home") });
                }
                else
                {
                    if (us.UserStatus == 0)
                    {
                        return Json(new { status = "-3", url = Url.Action("Index", "Home") });
                    }
                    else if (us.UserStatus == 1)
                    {
                        Session["UserID"] = us.UserID;
                        Session["userName"] = us.Firstname + " " + us.Lastname;
                        if (us.UserImage == null)
                        {
                            Session["UserImage"] = "/ProjectImages/UserImages/user.jpg";
                        }
                        else
                        {
                            Session["UserImage"] = us.UserImage;
                        }
                        Session["Email"] = us.UserEmail;
                        Session["Role"] = us.Role;
                        if (us.Role == "admin")
                        {
                            return Json(new { status = "1", url = Url.Action("Index", "Dashboard") });

                        }
                        else if (us.Role == "Candidate")
                        {
                            return Json(new { status = "2", url = Url.Action("Index", "Dashboard") });

                        }

                    }




                }
            }
            catch (Exception ex)
            {
                return Json(new { status = -1, data = ex.Message });
            }

            return Json(new { status = UserLoginReult, url = Url.Action("Index", "Home") });
        }


        [HttpPost]
        public ActionResult Login(string useremail, string userpassword)
        {

            tblUser us = new tblUser();
            try
            {
                string encryptedpassword = Encrypt(userpassword, "sblw-3hn8-sqoy19");
                us = dbEntities.tblUsers.Where(a => a.UserEmail == useremail).FirstOrDefault();

                if (us == null)
                {
                    ViewBag.Error = "User not exist";
                    return View();
                }
                us = dbEntities.tblUsers.Where(a => a.UserEmail == useremail && a.UserPass == encryptedpassword).FirstOrDefault();

                if (us == null)
                {
                    ViewBag.Error = "Invalid Credentials";
                    return View();
                }
                else
                {
                    if (us.UserStatus == 0)
                    {
                        ViewBag.Error = "User is not active";
                        return View();
                    }
                    else if (us.UserStatus >= 1)
                    {
                        Session["UserID"] = us.UserID;
                        Session["userName"] = us.Firstname + " " + us.Lastname;
                        if (us.UserImage == null)
                        {
                            Session["UserImage"] = "/ProjectImages/UserImages/user.jpg";
                        }
                        else
                        {
                            Session["UserImage"] = us.UserImage;
                        }
                        Session["Email"] = us.UserEmail;
                        Session["Role"] = us.Role;
                        Session["UserStatus"] = us.UserStatus;
                        if (us.Role == "admin")
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else if (us.Role == "Candidate")
                        {

                            return RedirectToAction("Index", "Home");
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Session["Error"] = ex.Message;
                ViewBag.error = "Exception Occur while Register User : " + Session["Error"];
                return RedirectToAction("ExceptionPage", "Error");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(tblUser UserInformation)
        {
            tblUser user = new tblUser();
            try
            {
                string encryptedpassword = Encrypt(UserInformation.UserPass, "sblw-3hn8-sqoy19");
                var userexists = dbEntities.tblUsers.Where(a => a.UserEmail == UserInformation.UserEmail);

                if (userexists.Count() > 0)
                {
                    return Json(new { status = "-1", url = Url.Action("Index", "Home") });
                }
                else
                {
                    user.UserPass = encryptedpassword;
                    user.UserEmail = UserInformation.UserEmail;
                    user.Firstname = UserInformation.Firstname;
                    user.Lastname = UserInformation.Lastname;
                    user.Role = "Candidate";
                    user.UserStatus = 1;
                    dbEntities.tblUsers.Add(user);
                    dbEntities.SaveChanges();

                    Session["UserID"] = user.UserID;
                    Session["userName"] = user.Firstname + " " + user.Lastname;
                    if (user.UserImage == null)
                    {
                        Session["UserImage"] = "/ProjectImages/UserImages/user.jpg";
                    }
                    else
                    {
                        Session["UserImage"] = user.UserImage;
                    }
                    Session["Email"] = user.UserEmail;
                    Session["Role"] = user.Role;
                    Session["UserStatus"] = user.UserStatus;
                    return Json(new { status = "1", url = Url.Action("Index", "Home") });
                }
                return View();
            }
            catch (Exception ex)
            {

            }
            return View();
        }



        [HttpPost]
        public ActionResult RegisterGoogle(tblUser UserInformation)
        {
            tblUser user = new tblUser();
            try
            {
                UserInformation.UserPass = "";
                string encryptedpassword = Encrypt(UserInformation.UserPass, "sblw-3hn8-sqoy19");
                tblUser userexists = dbEntities.tblUsers.Where(a => a.UserEmail == UserInformation.UserEmail).FirstOrDefault();

                if (userexists!=null)
                {
                    Session["UserID"] = userexists.UserID;
                    Session["userName"] = userexists.Firstname + " " + user.Lastname;
                    if (userexists.UserImage == null)
                    {
                        Session["UserImage"] = "/ProjectImages/UserImages/user.jpg";
                    }
                    else
                    {
                        Session["UserImage"] = userexists.UserImage;
                    }
                    Session["Email"] = userexists.UserEmail;
                    Session["Role"] = userexists.Role;
                    Session["UserStatus"] = userexists.UserStatus;
                    return Json(new { status = "1", url = Url.Action("Index", "Home") });
                }
                else
                {
                    user.UserImage = UserInformation.UserImage;
                    user.UserPass = encryptedpassword;
                    user.UserEmail = UserInformation.UserEmail;
                    user.Firstname = UserInformation.Firstname;
                    user.Lastname = UserInformation.Lastname;
                    user.Role = "Candidate";
                    user.UserStatus = 2;
                    dbEntities.tblUsers.Add(user);
                    dbEntities.SaveChanges();

                    Session["UserID"] = user.UserID;
                    Session["userName"] = user.Firstname + " " + user.Lastname;
                    if (user.UserImage == null)
                    {
                        Session["UserImage"] = "/ProjectImages/UserImages/user.jpg";
                    }
                    else
                    {
                        Session["UserImage"] = user.UserImage;
                    }
                    Session["Email"] = user.UserEmail;
                    Session["Role"] = user.Role;
                    Session["UserStatus"] = user.UserStatus;
                    return Json(new { status = "1", url = Url.Action("Index", "Home") });
                }
                return View();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [HttpGet]

        public ActionResult Forgetpassword()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            return View();
        }


        [HttpGet]
        public ActionResult Changewebpassword()
        {
            try
            {
                string Decryptemail = "";
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [HttpPost]
        public ActionResult Changewebpassword(string Email, string password)
        {
            string Decryptemail = "";
            try
            {
                Decryptemail = ut.Decrypt("1neC7n8zgA4dg6mzZY9g8oCcY+WzhQ+ODTZKW+s5+iloIiCt4X0txI/hp4KI/8B7");

                string encryptedpassword = Encrypt(password, "sblw-3hn8-sqoy19");
                tblUser us = dbEntities.tblUsers.Where(a => a.UserEmail == Decryptemail).FirstOrDefault();
                if (us != null)
                {
                    us.UserPass = encryptedpassword;
                    //dbEntities.tblUsers.Add(us);
                    dbEntities.SaveChanges();
                    return RedirectToAction("Login", "Account");
                }
                return View();



            }
            catch (Exception ex)
            {

            }
            return View();
        }


        [HttpPost]
        public ActionResult Forgetpassword(string Email)
        {
            string Encryptemail = "";
            Encryptemail = ut.Encrypt(Email);
            try
            {
                var userexists = dbEntities.tblUsers.Where(a => a.UserEmail == Email);

                if (userexists.Count() == 0)
                {
                    ViewBag.Color = "Red";
                    ViewBag.Error = "User Not Exists";
                    return View();
                }
                else
                {
                    using (MailMessage mm = new MailMessage("restock06@gmail.com", Email))
                    {
                        string link = Request.Url.ToString();
                        link = link.Replace("Changepassword", "Account");
                        mm.Subject = "Account Activation";
                        string body = "Hello " + Email + ",";
                        body += "<br /><br />Please click the following link to activate your account";
                        body += "<br /><a href = '" + link + "?E=" + Encryptemail + "'>Click here to activate your account.</a>";
                        body += "<br /><br />Thanks";
                        mm.Body = body;
                        mm.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential("restock06@gmail.com", "Developer@123");
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 465;
                        smtp.Send(mm);
                    }
                    ViewBag.Color = "green";
                    ViewBag.Error = "Link send to email";
                    return View();
                }
                return View();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
    }
}