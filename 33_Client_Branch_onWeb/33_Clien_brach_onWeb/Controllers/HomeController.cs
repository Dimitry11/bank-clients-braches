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
using _33_Clien_brach_onWeb.Models;

namespace _33_Clien_brach_onWeb.Controllers
{
    public class HomeController : Controller
    {
        Client client;
        Bank bank;
        Branch branch;
        MSSQL sql;
        string MSG = string.Empty;
        public ActionResult ErrorAction { get; private set; }
        Model1 db = new Model1();

        public HomeController()
        {
            sql = new MSSQL();
            //client = new Client(sql);
        }
        // Clients:
        public async Task<ActionResult> Select()
        {
            return View(await db.Clients.ToListAsync());
        }

        // Delete Client:
        public async Task<ActionResult> DeleteClient()
        {
            return View(await db.Clients.ToListAsync());
        }

        // Edit Client:
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = await db.Clients.FindAsync(id);

            if(client == null)
            {
                return HttpNotFound();
            }

            return View(client);
        }

        [HttpPost]
        public ActionResult Edit(Client client)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(client).State = EntityState.Modified;
                    //db.Clients.Add(client);
                    db.SaveChanges();
                    return RedirectToAction("Select");
                }
            }
            catch (DataException de)
            {               
                ModelState.AddModelError($"{de.Message}", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(client);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = await db.Clients.FindAsync(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        public ActionResult Delete(int id, bool? SaveChangeError)
        {
            if (SaveChangeError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete Failed...";
            }
            return View(db.Clients.Find(id));
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            try
            {
                Client client = db.Clients.Find(id);
                db.Clients.Remove(client);
                db.SaveChanges();
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new System.Web.Routing.RouteValueDictionary { { "id", id }, { "SaveChangeError", true } });
            }
            return RedirectToAction("Select");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        ////public ActionResult random()
        ////{
        ////   //client.Random();
        ////    if (IsError())
        ////        return ErrorAction;
        ////    return View("number", client);
        ////}

        [HttpGet]
        public ActionResult Add_Client()
        {
            client = new Client();
            client.name = "";
            client.card_number = "";
            client.number_acc = Guid.NewGuid();
            client.date_register = DateTime.Now;
            if (IsError())
             return ErrorAction;
            return View(client);
        }
   
        [HttpPost]
        public ActionResult Add_Client(Client client)
        {
            if (!ModelState.IsValid)  // Если поля не заполненые, то оставляем форму без изменений
                return View(client);
            this.client = client;
            MSG = Add(client);
            ViewBag.Message = MSG;
             if (IsError())
                 return ErrorAction;
            return Redirect("/Home/Add_Client/" + client.id);
        }

        string Add(Client client)
        {
            const string connectionString = @"Data Source=DESKTOP-6NBVMFM\MSSQLSERVER1;Initial Catalog=32_new_test;Integrated Security=True";
            string MSG = string.Empty;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Clients(Name, Card_Number, Number_Acc, Date_Register) " +
                "Values(@Name, @Card_Number, @Number_Acc, @Date_Register);", conn);
            cmd.Parameters.AddWithValue("@Name", client.name);
            cmd.Parameters.AddWithValue("@Card_Number", client.card_number);
            cmd.Parameters.AddWithValue("@Number_Acc", client.number_acc = Guid.NewGuid());
            cmd.Parameters.AddWithValue("@Date_Register", client.date_register = DateTime.Now);

            int result = cmd.ExecuteNonQuery();
            if (result == 1)
                MSG = client.name + " Inserted Successfully!";
            else
                MSG = client.name + " NOT Inserted! Try to Find this Error";
            conn.Close();
            return MSG;
        }

        // client/number/1234    id = 1234
        public ActionResult number()
        {
            string id = (RouteData.Values["id"] ?? "").ToString();
            if (id == "")  // Если не ввели в url id - делаем Redirect
                return Redirect("/Home");
            //client.Number(Convert.ToInt32(id));
            if (IsError())
                return ErrorAction;
            return View(client);
        }

        public bool IsError()
        {
            client = new Client();
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
    }
}