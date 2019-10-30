using System;
using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;

namespace WebExpenses.Controllers
{
    public class MenuController : Controller
    {
        public MenuController()
        {
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}