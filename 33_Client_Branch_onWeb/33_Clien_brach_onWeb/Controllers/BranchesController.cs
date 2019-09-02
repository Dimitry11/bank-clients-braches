using _33_Clien_brach_onWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _33_Clien_brach_onWeb.Controllers
{
    public class BranchesController : Controller
    {
        string MSG = string.Empty;
        public ActionResult ErrorAction { get; private set; }
        Model1 db = new Model1();
        Branch branch;
        MSSQL sql;

        public BranchesController()
        {
            sql = new MSSQL();
        }
        // GET: Branches
        public async Task<ActionResult> List_Branch()
        {
            ViewBag.Message = MSG;
            return View(await db.Branch.ToListAsync());
        }

        [HttpGet]
        public ActionResult Add_Branch()
        {
            branch = new Branch();
            branch.name = "";
            branch.postcode = 0;
            branch.address = null;
            branch.phone_number = 0;
            branch.suport_email = null;
            branch.schedual = null;
            branch.id_bank = 0;
            if (IsError())
                return ErrorAction;
            return View(branch);
        }

        [HttpPost]
        public ActionResult Add_Branch(Branch branch)
        {
            if (!ModelState.IsValid)  // Если поля не заполненые, то оставляем форму без изменений
                return View(branch);
            this.branch = branch;
            MSG = Add(branch);
            if (IsError())
                return ErrorAction;
            return RedirectToAction("Branches", "Pagination");
        }

        // Method for Insert one entity on the table(ADO .NET):
        string Add(Branch branch)
        {
            const string connectionString = @"Data Source=DESKTOP-6NBVMFM\MSSQLSERVER1;Initial Catalog=32_new_test;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
            string MSG = string.Empty;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Branches(name, postcode, address, phone_number, suport_email, schedual, id_bank) " +
                "Values(@name, @postcode, @address, @phone_number, @suport_email, @schedual, @id_bank);", conn);
            cmd.Parameters.AddWithValue("@name", branch.name);
            cmd.Parameters.AddWithValue("@postcode", branch.postcode);
            cmd.Parameters.AddWithValue("@address", branch.address);
            cmd.Parameters.AddWithValue("@phone_number", branch.phone_number);
            cmd.Parameters.AddWithValue("@suport_email", branch.suport_email);
            cmd.Parameters.AddWithValue("@schedual", branch.schedual);
            cmd.Parameters.AddWithValue("@id_bank", branch.id_bank);

            int result = cmd.ExecuteNonQuery();
            if (result == 1)
                MSG = branch.name + " Inserted Successfully!";
            else
                MSG = branch.name + " NOT Inserted! Try to Find this Error";
            conn.Close();
            return MSG;
        }

        public bool IsError()
        {
            branch = new Branch();
            if (sql.IsError())
            {
                ViewBag.error = sql.error;
                ViewBag.query = sql.query;
                ErrorAction = View("~/Views/Error.cshtml");
                return true;
            }
            if (sql.IsError())
            {
                ViewBag.error = sql.error;
                ViewBag.query = sql.query;
                ErrorAction = View("~/Views/Error.cshtml");
                return true;
            }

            return false;
        }

        [HttpGet]
        public ActionResult Delete(int id, bool? SaveChangeError)
        {
            if (SaveChangeError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete Failed...";
            }
            return View(db.Branch.Find(id));
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            try
            {
                Branch branch = db.Branch.Find(id);
                db.Branch.Remove(branch);
                db.SaveChanges();
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new System.Web.Routing.RouteValueDictionary { { "id", id }, { "SaveChangeError", true } });
            }

            return RedirectToAction("List_Branch");
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = await db.Branch.FindAsync(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }

        // Edit Branch:
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = await db.Branch.FindAsync(id);

            if (branch == null)
            {
                return HttpNotFound();
            }

            return View(branch);
        }

        [HttpPost]
        public ActionResult Edit(Branch branch)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(branch).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("List_Branch");
                }
            }
            catch (DataException de)
            {
                ModelState.AddModelError($"{de.Message}", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(branch);
        }

        // Delete Branch:
        public async Task<ActionResult> DeleteBranch()
        {
            return View(await db.Branch.ToListAsync());
        }
    }
}