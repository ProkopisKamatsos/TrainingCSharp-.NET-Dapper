using Microsoft.AspNetCore.Mvc;
using System.Data;
using Dapper;
using DapperMvcDemo.Models;
using Microsoft.Data.SqlClient;


namespace DapperMvcDemo.Controllers
{

    public class CustomerController : Controller
    {
        private readonly IConfiguration _configuration;

        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ActionResult Index()
        {
            List<Customer> customers = new List<Customer>();
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))

            {

                customers = db.Query<Customer>("Select * From Customers").ToList();
            }
            return View(customers);
        }
        // GET: Customer/Details/5
        public IActionResult Details(int id)
        {
            Customer customer;

            using (IDbConnection db = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")))
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                customer = db.QuerySingleOrDefault<Customer>(
                    "SELECT * FROM dbo.Customers WHERE CustomerID = @id",
                    new { id });
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            }

            return View(customer);
        }
        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer customer)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    string sqlQuery = "Insert Into Customers (FirstName, LastName, Email) Values(@FirstName, @LastName, @Email)";
                    int rowsAffected = db.Execute(sqlQuery, customer);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: Customer/Edit/5
        public IActionResult Edit(int id)
        {
            Customer customer;

            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                customer = db.QuerySingleOrDefault<Customer>(
                    "SELECT * FROM dbo.Customers WHERE CustomerID = @id",
                    new { id }
                );
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            }

            return View(customer);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Customer customer)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    string sqlQuery = @"UPDATE dbo.Customers
                                SET FirstName = @FirstName,
                                    LastName  = @LastName,
                                    Email     = @Email
                                WHERE CustomerID = @CustomerID";

                    db.Execute(sqlQuery, customer);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(customer);
            }
        }
        // GET: Customer/Delete/5
        public IActionResult Delete(int id)
        {
            Customer customer;

            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                customer = db.Query<Customer>(
                    "SELECT * FROM dbo.Customers WHERE CustomerID = @id",
                    new { id }
                ).SingleOrDefault();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Customer customer)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    db.Execute("DELETE FROM dbo.Customers WHERE CustomerID = @id", new { id });
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(customer);
            }
        }




    }
}
