using AuctionSpawn.DAO;
using AuctionSpawn.Helper;
using AuctionSpawn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuctionSpawn.Controllers
{
    public class AuctionController : Controller
    {
        AuctionDAO auctionDAO = new AuctionDAO();

        [HttpGet]
        public ActionResult CreateAuction()
        {
            if(ModelState.IsValid)
            {
                ModelState.Clear();
                ViewBag.Message = "Create Auction";
                Auction auction = new Auction();
                auction.AuctionItems = Enumerable.Empty<Item>();
                ViewBag.Readonly = false;
                ViewBag.isCreate = true;
                return View(auction);
            }
            return View();
         
        }

        [HttpPost]
        public ActionResult CreateAuction(string Description)
        {
            ViewBag.Readonly = false;
            ViewBag.isCreate = true;
            auctionDAO.Save(Description);

            //Send acknowledgement receipt if successful
            bool isEmailSent = MailManager.SendHtmlEmail(Configuration.ToEmailAddress, Configuration.EmailSubject, MailMessageBody(Description));

            Console.WriteLine("Email Sent Successfully" +isEmailSent);

            var urlBuilder = new System.UriBuilder(Request.Url.AbsoluteUri)
            {
                Path = Url.Action("ViewAuction", "Auction"),
                Query = "",
            };

            Uri uri = urlBuilder.Uri;
            return Redirect(uri.AbsoluteUri);
        }

        [HttpPost]
        public ActionResult SaveAuction(string Description)
        {
            try
            {
                ViewBag.Readonly = false;
                ViewBag.isCreate = true;
                auctionDAO.Save(Description);

                var urlBuilder = new System.UriBuilder(Request.Url.AbsoluteUri)
                {
                    Path = Url.Action("ViewAuction", "Auction"),
                    Query = "",
                };


                Uri uri = urlBuilder.Uri;
                return Redirect(uri.AbsoluteUri);
            }
            catch (Exception ex)
            {
                return View("Error Saving Auction Description: \"Description\": Maybe cause when Auction Description already exists (DB Unique Constraint Error) or other unknown exception.", new HandleErrorInfo(ex, "Auction", "CreateAuction"));
            }
        }

        [HttpGet]
        public ActionResult ViewAuction()
        {
            if (ModelState.IsValid)
            {
                return View(auctionDAO.RetrieveAuctionsFromDB().ToList());
            }
            return View();
        }

        [HttpPost]
        public ActionResult ViewAuction(string auctionDescription)
        {
            if (ModelState.IsValid)
            {
                return View(auctionDAO.RetrieveAuctionsFromDB().ToList());
            }
            return View();
        }

        //being used
        [HttpGet]
        [Route("Auction/ViewAuctionDetails/{id}")]
        public ActionResult ViewAuctionDetails(int id)
        {
            ItemDAO itemDAO = new ItemDAO();
            if (ModelState.IsValid)
            {
                ViewBag.Readonly = true;
                ViewBag.isCreate = false;
                Auction auction = auctionDAO.GetById(id);
                auction.AuctionItems = itemDAO.RetrieveList(id);
                return View(auction);
            }
            return View();
        }

        [HttpPost]
        [Route("Auction/ViewAuctionDetails/{id}")]
        public ActionResult ViewAuctionDetails(int id, FormCollection formObj)
        {
            ItemDAO itemDAO = new ItemDAO();
            if (ModelState.IsValid)
            {
                int baseQuantity = Convert.ToInt32(formObj["baseItemQuantity"]);
                int newItemQuantity = Convert.ToInt32(formObj["newItemQuantity"]);
                int newTotalItems = baseQuantity + newItemQuantity;
                Auction auction = new Auction(id, newTotalItems);

                var urlBuilder = new System.UriBuilder();
                if (id > 0)
                {
                    try
                    {
                        //Update Item Quantity
                        auctionDAO.Update(auction);
                        urlBuilder = new System.UriBuilder(Request.Url.AbsoluteUri)
                        {
                            Path = Url.Action("ViewAuctionDetails", "Auction"),
                            Query = "id=" + id,
                        };
                    }
                    catch (Exception ex)
                    {
                        return View("Update Item Quantity Error! ", new HandleErrorInfo(ex, "Item", "AddItem"));
                    }
                }
                Uri uri = urlBuilder.Uri;
                return Redirect(uri.AbsoluteUri);
            }
            return View();
        }

        [HttpGet]
        public ActionResult AuctionItems(Item item)
        {
            IEnumerable<Item> items = Enumerable.Empty<Item>();
            items.ToList().Add(item);
            return View(items);
        }

        [HttpGet]
        public ActionResult DeleteAuction(int id, string description)
        {
            ViewBag.AuctionID = id;
            ViewBag.auctionDescription = description;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteAuction(int id)
        {
            auctionDAO.DeleteById(id);
            var urlBuilder = new System.UriBuilder(Request.Url.AbsoluteUri)
            {
                Path = Url.Action("ViewAuction", "Auction"),
                Query = "",
            };

            Uri uri = urlBuilder.Uri;
            return Redirect(uri.AbsoluteUri);
        }
        
        protected override void OnException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;

            filterContext.ExceptionHandled = true;

            var Result = this.View("Error", new HandleErrorInfo(exception,
                filterContext.RouteData.Values["controller"].ToString(),
                filterContext.RouteData.Values["action"].ToString()));

            filterContext.Result = Result;

        }

        private string MailMessageBody(string Description)
        {
            string htmlEmailBody = "<p>Hi there, </p>";

            htmlEmailBody += "<h3><b>Congratulations! <b><br/></h3>";
            htmlEmailBody += "<p>This is to acknowledge that you've successfully created and auction with us. </p>";
            htmlEmailBody += "<p>I am looking forward to a pleasant transaction and positive feedback for both of us.</p>";
            htmlEmailBody += "<p>You can now start adding items. I will delighted to be dealing with you and know you will enjoy your purchase.</p><br/>";


            // Add the account and order information to the email
            htmlEmailBody += "<table>";
            htmlEmailBody += "<tr><td>Auction Decsription:</td><td>" + Description + " </td></tr>";
            htmlEmailBody += "</table>";

            htmlEmailBody += "<br/><p>Please contact us at +63 111 1111 if you have concerns and issues.</p><br/>";

            htmlEmailBody += "<br/><p>Best Regards,</p><br/>";
            htmlEmailBody += "<p>AuctionSpawn by KTA</p><br/>";

            return htmlEmailBody;
        }

    }
}