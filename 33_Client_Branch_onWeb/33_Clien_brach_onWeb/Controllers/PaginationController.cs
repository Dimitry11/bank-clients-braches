using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _33_Clien_brach_onWeb.Models;
using PagedList;

namespace _33_Clien_brach_onWeb.Controllers
{
    public class PaginationController : Controller
    {
        Model1 db;
        public PaginationController()
        {
            db = new Model1();
        }
        public ActionResult Clients(int? page)
        {
            var list = db.Clients.ToList();
            int pageSize = 1;
            int pageNumber = (page ?? 1);  // if that page dosen't exists then we show 1st page
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Branches(int? page)
        {
            var list = db.Branch.ToList();
            int pageSize = 1;
            int pageNumber = (page ?? 1);  // if that page dosen't exists then we show 1st page
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Banks(int? page)
        {
            var list = db.Banks.ToList();
            int pageSize = 1;
            int pageNumber = (page ?? 1);  // if that page dosen't exists then we show 1st page
            return View(list.ToPagedList(pageNumber, pageSize));
        }
    }
}