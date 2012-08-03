using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServerSideTimeR.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Exploring Connect, Disconnect and Reconnect in SignalR";

            return View();
        }

        public JsonResult GetPageInfo(int page)
        {
            return Json(new { name = "Page Number", number = page });
        }
    }
}
