using _33_Clien_brach_onWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _33_Clien_brach_onWeb.Controllers
{
    public class aboutController : Controller
    {
        public ActionResult about()
        {
            Info info = new Info();
            return View(info);
        }
    }
}