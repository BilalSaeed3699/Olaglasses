using Olaglasses.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Olaglasses.Controllers
{
    public class PaymentProcessController : Controller
    {
        OlaGlassesEntities dbEntity = new OlaGlassesEntities();


        [HttpPost]
        public ActionResult Index1(int? id=0,double txtordertotal = 0,double txtdiscountamount=0, double txtsubtotal=0)
        {
           
            List<tblOrder> order = dbEntity.tblOrder.Where(x => x.userID == id).ToList();
            List<tblcart> cart = dbEntity.tblcart.Where(x => x.Userid == id).ToList();
            ViewBag.UserID = cart.FirstOrDefault().Userid;


            double tax = txtsubtotal / 100 * 5;
            ViewBag.GrandAmount = txtordertotal;
            ViewBag.txtdiscountamount = txtdiscountamount;
            ViewBag.txtsubtotal = txtsubtotal;
            ViewBag.OrderList = order;
            ViewBag.tax = tax;
            ViewBag.cart = cart;
            tblcart tblcart;
            foreach (var item in cart)
            {
                tblcart = new tblcart();
                tblcart = dbEntity.tblcart.Find(item.Cartid);
                tblcart.GranTotal = txtordertotal;
                tblcart.Discount = txtdiscountamount;
                tblcart.TaxAmount = tax;

                dbEntity.Entry(tblcart).State = EntityState.Modified;
                dbEntity.SaveChanges();
            }
            return View();
        }
        public ActionResult Checkout(int? id = 0, int OlderID=0)
        {
            tblOrder order = new tblOrder();
            if(OlderID>0)
            {
                 order = dbEntity.tblOrder.Where(x => x.OrderID == OlderID).FirstOrDefault();
            }

            List<tblcart> cart = dbEntity.tblcart.Where(x => x.Userid == id).ToList();
            ViewBag.UserID = cart.FirstOrDefault().Userid;
            ViewBag.GrandAmount = cart.FirstOrDefault().GranTotal;
            
            ViewBag.cart = cart;
            return View(order);
        }
        [HttpPost]
        public ActionResult Checkout(tblOrder order, string IsSame , int? id)
        {
            tblcart cart = dbEntity.tblcart.Where(x => x.Userid == id).FirstOrDefault();
            ViewBag.UserID = cart.Userid;
            ViewBag.GrandAmount = cart.GranTotal;
            return View();
        }
        [HttpPost]
        [Obsolete]
        public ActionResult Index(string cardname, string cardNumber, string expirydate, string CVCNumber, int userID, tblOrder order, string IsSame, string amount = "")
        {

            try
            {
                tblUser user = dbEntity.tblUsers.Find(userID);
                tblSetting sa = dbEntity.tblSettings.Find(3);
                DateTime date = Convert.ToDateTime(expirydate);
                int month = date.Month;
                int year = date.Year;
                Stripe.StripeConfiguration.SetApiKey(sa.StripeID);
                var options1 = new Stripe.TokenCreateOptions
                {
                    Card = new Stripe.TokenCardOptions
                    {

                        Number = cardNumber,
                        ExpMonth = month,
                        ExpYear = year,
                        Cvc = CVCNumber,
                    },
                };



                Stripe.TokenService tokenService = new Stripe.TokenService();

                Stripe.Token token = tokenService.Create(options1);

                Stripe.CustomerCreateOptions customer = new Stripe.CustomerCreateOptions();

                customer.Email = user.UserEmail;

                customer.Source = token.Id;

                var custService = new Stripe.CustomerService();

                Stripe.Customer stpCustomer = custService.Create(customer);
                long amounts = Convert.ToInt64(amount) * 100;

                var options = new Stripe.ChargeCreateOptions
                {

                    Amount = amounts,

                    Currency = "USD",

                    ReceiptEmail = sa.ReceipentEmail,

                    Customer = stpCustomer.Id,

                    Description = Convert.ToString("Payment to Ola Glasses"), //Optional  

                };

                //and Create Method of this object is doing the payment execution.  

                var service = new Stripe.ChargeService();

                Stripe.Charge charge = service.Create(options); // This will do the Payment

                string s = charge.Status;
                if (s != "succeeded")
                {
                    ViewBag.message = "There is a problem in transaction please retry.";
                }

                else
                {
                    tblUserPrescription userPrescription;
                    tblOrderDetails orderDetails;
                    tblOrderPrescription orderPrescription;
                    tblOrder tblOrder = new tblOrder();
                    tblOrder = order;
                    tblOrder.userID = userID;
                    tblOrder.OrderDate = DateTime.Now;
                    tblOrder.UserName = user.Firstname + " " + user.Lastname;
                    tblOrder.UserEmail = user.UserEmail;
                    tblOrder.Status = "Not Started";
                    tblOrder.Total = Convert.ToInt64(amount);
                    tblOrder.UserAddress = user.Address;
                    dbEntity.tblOrder.Add(tblOrder);
                    dbEntity.SaveChanges();

                    int orderID = tblOrder.OrderID;

                    List<tblcart> cart = dbEntity.tblcart.Where(x => x.Userid == userID).ToList();

                    foreach (var item in cart)
                    {
                        double subTotal = 0;
                        orderDetails = new tblOrderDetails();
                        orderDetails.OrderID = orderID;
                        orderDetails.ProductID = item.GlassID;
                        orderDetails.Productname = item.Title;
                        orderDetails.ProductPrice = item.Price.ToString();
                        orderDetails.OrderDate = DateTime.Now;
                        orderDetails.Vision = item.VisionType;
                        orderDetails.LensType = item.LensType;
                        orderDetails.UV_Protection = item.UVProtection;
                        orderDetails.Total = item.TotalAmount.ToString();
                        orderDetails.ProductVariation = item.Variationid;
                        orderDetails.quantity = item.Quantity;
                        orderDetails.TaxAmount = item.TaxAmount;
                        orderDetails.Discount = item.Discount;
                        orderDetails.FrameColor = item.Colour;
                        orderDetails.ProductImage = item.ProductImagePath;
                        orderDetails.ShippingAmount = item.Shippingamount;
                        subTotal = item.TotalAmount;
                        if (item.LensType == "Standard")
                        {
                            subTotal = subTotal + 39;
                        }
                        else if (item.LensType == "Enhanced")
                        {
                            subTotal = subTotal + 49;
                        }
                        if (item.UVProtection == "on")
                        {
                            subTotal = subTotal + 20;
                        }
                        orderDetails.Subtotal = subTotal;

                        dbEntity.tblOrderDetails.Add(orderDetails);
                        dbEntity.SaveChanges();

                        if (item.VisionType != "")
                        {
                            orderPrescription = new tblOrderPrescription();
                            orderPrescription.ProductID = item.GlassID;
                            orderPrescription.OrderID = orderID;
                            orderPrescription.r_sph = item.r_sph;
                            orderPrescription.r_cyl = item.r_cyl;
                            orderPrescription.r_axis = item.r_axis;
                            orderPrescription.r_add = item.r_add;
                            orderPrescription.l_sph = item.l_sph;
                            orderPrescription.l_cyl = item.l_cyl;
                            orderPrescription.l_axis = item.l_axis;
                            orderPrescription.l_add = item.l_add;
                            orderPrescription.lensType = item.VisionType;
                            orderPrescription.quantity = item.Quantity;
                            orderPrescription.Fname = item.Fname;
                            orderPrescription.Lname = item.Lname;
                            orderPrescription.prescription = item.prescription;
                            orderPrescription.prescriptionDate = item.prescriptionDate;
                            orderPrescription.PD = item.PD;
                            orderPrescription.UserID = item.Userid;
                            orderPrescription.PDImage = item.PDImage;
                            orderPrescription.PrescriptionImage = item.PrescriptionImage;
                            dbEntity.tblOrderPrescriptions.Add(orderPrescription);
                            dbEntity.SaveChanges();
                            if (item.prescription == "Yes")
                            {
                                userPrescription = new tblUserPrescription();
                                userPrescription.ProductID = item.GlassID;
                                userPrescription.OrderID = orderID;
                                userPrescription.r_sph = item.r_sph;
                                userPrescription.r_cyl = item.r_cyl;
                                userPrescription.r_axis = item.r_axis;
                                userPrescription.r_add = item.r_add;
                                userPrescription.l_sph = item.l_sph;
                                userPrescription.l_cyl = item.l_cyl;
                                userPrescription.l_axis = item.l_axis;
                                userPrescription.l_add = item.l_add;
                                userPrescription.lensType = item.VisionType;
                                userPrescription.quantity = item.Quantity;
                                userPrescription.Fname = item.Fname;
                                userPrescription.Lname = item.Lname;
                                userPrescription.prescription = item.prescription;
                                userPrescription.prescriptionDate = item.prescriptionDate;
                                userPrescription.PD = item.PD;
                                userPrescription.UserID = item.Userid;
                                userPrescription.PDImage = item.PDImage;
                                userPrescription.PrescriptionImage = item.PrescriptionImage;
                                dbEntity.tblUserPrescription.Add(userPrescription);
                                dbEntity.SaveChanges();
                            }
                        }
                    }



                    return RedirectToAction("OrderDetails", "PaymentProcess", new { id = orderID });
                }
                return View();
            }
            catch (Exception)
            {
                ViewBag.message = "There is a problem in transaction please retry.";
                return View();
                throw;
            }
          
        }

        public ActionResult OrderDetails(int? id = 1)
        {
            List<spgetorderDetails_Result4> spgetorderDetails = dbEntity.spgetorderDetails(id).ToList();
            ViewBag.OrderDetails = spgetorderDetails;
            return View();

        }
    }
}