using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Olaglasses.Models;
using Stripe.Infrastructure;
namespace Olaglasses.Controllers
{
    public class PaymentController : Controller
    {


        OlaGlassesEntities dbEntity = new OlaGlassesEntities();
        public ActionResult OrderProcess(int? productid, int? VariavtionID)
        {
            tblProduct product = dbEntity.tblProducts.Where(x => x.GlassID == productid).FirstOrDefault();
            ViewBag.VariavtionID = VariavtionID;
            return View(product);
        }
        // GET: Payment
        [Obsolete]
        public ActionResult Index()
        {
            Stripe.StripeConfiguration.SetApiKey("sk_test_51KCNvMAAmezOUSgHzvzlmVPJbRdOjcsDpuFRpvurc3vR5OlfBlWSVcmDMfaY0s00s9BjUVCjteBTbZIs9WLjpuP600fhBe83Gy");
            //Stripe.card card = new Stripe.CreditCardOptions();

            //card.Name = "";

            //card.Number ="";

            //card.ExpYear = "";

            //card.ExpMonth ="";

            //card.Cvc = "";

            // set card to token object and create token  

            var options1 = new Stripe.TokenCreateOptions
            {
                Card = new Stripe.TokenCardOptions
                {
                   
                    Number = "4242424242424242",
                    ExpMonth = 12,
                    ExpYear = 2022,
                    Cvc = "314",
                },
            };

            //Stripe.TokenCreateOptions tokenCreateOption = new Stripe.TokenCreateOptions();

            //tokenCreateOption.Card = card;

            Stripe.TokenService tokenService = new Stripe.TokenService();

            Stripe.Token token = tokenService.Create(options1);

            Stripe.CustomerCreateOptions customer = new Stripe.CustomerCreateOptions();

            customer.Email = "abdulqadeerkhan070@gmail.com";

            customer.Source = token.Id;

            var custService = new Stripe.CustomerService();

            Stripe.Customer stpCustomer = custService.Create(customer);

            var options = new Stripe.ChargeCreateOptions
            {

                Amount = Convert.ToInt32(100.0),

                Currency = "USD",

                ReceiptEmail = "abdulqadeerkhan070@gmail.com",

                Customer = stpCustomer.Id,

                Description = Convert.ToString("abdulqadeerkhan070@gmail.com"), //Optional  

            };

            //and Create Method of this object is doing the payment execution.  

            var service = new Stripe.ChargeService();
            
            Stripe.Charge charge = service.Create(options); // This will do the Payment

            string s= charge.Status;
            return View();
        }
    }
}