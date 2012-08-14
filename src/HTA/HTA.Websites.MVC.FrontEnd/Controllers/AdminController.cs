using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTA.Adventures.Models;
using HTA.Websites.MVC.FrontEnd.API;

namespace HTA.Websites.MVC.FrontEnd.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

    }
}
