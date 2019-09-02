using _33_Clien_brach_onWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data;
using System.Net;

namespace _33_Clien_brach_onWeb.Controllers
{
    public class BanksController : Controller
    {
        string MSG = string.Empty;
        Model1 db = new Model1();
        Bank bank;
        public ActionResult ErrorAction { get; private set; }
        MSSQL sql;

        public BanksController()
        {
            sql = new MSSQL();
        }
        // GET: Banks
        public async Task<ActionResult> List_Banks()
        {
            return View(await db.Banks.ToListAsync());
        }

        [HttpGet]
        public ActionResult Add_Bank()
        {
            bank = new Bank();
            bank.name = "";
            bank.bank_acc = Guid.NewGuid();
            bank.branch_number = 0;
            if (IsError())
                return ErrorAction;
            return View(bank);
        }

        [HttpPost]
        public ActionResult Add_Bank(Bank bank)
        {
            if (!ModelState.IsValid)  // Если поля не заполненые, то оставляем форму без изменений
                return View(bank);
            this.bank = bank;
            MSG = Add(bank);
            if (IsError())
                return ErrorAction;
            return Redirect("/Pagination/Banks/" + bank.id);
        }

        // Method for Insert one entity on the table(ADO .NET):
        string Add(Bank bank)
        {
            const string connectionString = @"Data Source=DESKTOP-6NBVMFM\MSSQLSERVER1;Initial Catalog=32_new_test;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
            MSG = string.Empty;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Banks(name, bank_acc, branch_number) " +
                "Values(@name, @bank_acc, @branch_number);", conn);
            cmd.Parameters.AddWithValue("@name", bank.name);
            cmd.Parameters.AddWithValue("@bank_acc", bank.bank_acc);
            cmd.Parameters.AddWithValue("@branch_number", bank.branch_number);

            int result = cmd.ExecuteNonQuery();
            if (result == 1)
                MSG = bank.name + " Inserted Successfully!";
            else
                MSG = bank.name + " NOT Inserted! Try to Find this Error";
            conn.Close();
            return MSG;
        }

        [HttpGet]
        public ActionResult Delete(int id, bool? SaveChangeError)
        {
            if (SaveChangeError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete Failed...";
            }
            return View(db.Banks.Find(id));
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            try
            {
                Bank bank = db.Banks.Find(id);
                db.Banks.Remove(bank);
                db.SaveChanges();
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new System.Web.Routing.RouteValueDictionary { { "id", id }, { "SaveChangeError", true } });
            }

            return RedirectToAction("List_Banks");
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bank b = await db.Banks.FindAsync(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            return View(b);
        }

        // Edit Bank:
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bank b = await db.Banks.FindAsync(id);

            if (b == null)
            {
                return HttpNotFound();
            }

            return View(b);
        }

        [HttpPost]
        public ActionResult Edit(Bank b)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(b).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("List_Banks");
                }
            }
            catch (DataException de)
            {
                ModelState.AddModelError($"{de.Message}", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(b);
        }

        public bool IsError()
        {
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

        // Delete Bank:
        public async Task<ActionResult> DeleteBank()
        {
            return View(await db.Banks.ToListAsync());
        }
    }
}