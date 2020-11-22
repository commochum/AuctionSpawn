using AuctionSpawn.DAO;
using AuctionSpawn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AuctionSpawn.Controllers
{
    public class ItemController : Controller
    {
        AuctionDAO auctionDAO = new AuctionDAO();
        ItemDAO itemDAO = new ItemDAO();
        
        [HttpGet]
        [Route("AddItem/{id}")]
        public ActionResult AddItem(int id)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = "View Item Page";
                ViewBag.AuctionID = id;
                Item item = new Item();
                return View(item);
            }
            return View();
        }

        [HttpGet]
        public ActionResult AddItem()
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = "View Item";
                Item item = new Item();
                return View();
            }
            return View();
        }

        [HttpGet]
        [Route("DeleteItem/{itemId}")]
        public ActionResult DeleteItem(int auctionId, int itemId)
        {
            return View(itemDAO.GetById(itemId));
        }

        [HttpPost]
        [Route("DeleteItem/{itemId}")]
        public ActionResult DeleteItem(int itemId)
        {
            itemDAO.DeleteById(itemId);
            var urlBuilder = new System.UriBuilder(Request.Url.AbsoluteUri)
                        {
                            Path = Url.Action("ViewAuctionDetails", "Auction"),
                            Query = Request["auctionId"]
                        };

            Uri uri = urlBuilder.Uri;
            uri = new Uri(uri.ToString().Replace("?", "/"));
            return Redirect(uri.AbsoluteUri);
        }


        [HttpPost]
        [Route("AddItem/{id}")]
        public ActionResult AddItem(FormCollection formObj)
        {
            if (ModelState.IsValid)
            {
                //int auctionID = Convert.ToInt32(Request["id"]);
                string ItemTitle = formObj["txtItemTitle"];
                string ItemDescription = formObj["txtItemDescription"];
                int ItemStartPrice = Convert.ToInt32(formObj["StartPrice"]);

                int auctionID = Convert.ToInt32(Url.RequestContext.RouteData.Values["id"]);

                Item item = new Item(ItemTitle, ItemDescription, ItemStartPrice);

                var urlBuilder = new System.UriBuilder();

                if (auctionID > 0)
                {
                    try
                    {
                        //create from View Auction Details
                        auctionDAO.AddItemToAuctionDB(auctionID, item);
                        urlBuilder = new System.UriBuilder(Request.Url.AbsoluteUri)
                        {
                            Path = Url.Action("ViewAuctionDetails", "Auction"),
                            Query = "id=" + auctionID,
                        };
                    }
                    catch (Exception ex)
                    {
                        return View("Add Item Error: Maybe cause when Item Title: " + ItemTitle + " already exists(DB Unique Constraint Error) in Auction with ID " + auctionID + " or other unknown exception! ", new HandleErrorInfo(ex, "Item", "AddItem"));
                    }
                }
                Uri uri = urlBuilder.Uri;
                return Redirect(uri.AbsoluteUri);
            }
            return View();
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
    }
}