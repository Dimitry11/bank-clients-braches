using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _33_Clien_brach_onWeb.Models;

namespace _33_Clien_brach_onWeb.Controllers
{
    public class pageController : Controller
    {
        private MSSQL sql;
        // GET: page
        public ActionResult Index()
        {
            sql = new MSSQL();
            Client client = new Client();
            //story.GenerateList("5");
            return View(client);
        }
    }
}