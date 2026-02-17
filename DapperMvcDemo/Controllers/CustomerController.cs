using Microsoft.AspNetCore.Mvc;
using System.Data;
using Dapper;
using DapperMvcDemo.Models;
using Microsoft.Data.SqlClient;
using Z.Dapper.Plus;

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


        public IActionResult BulkInsertDemo()
        {
            DapperPlusMapping.EnsureMapped();

            var list = Enumerable.Range(1, 50)
                .Select(i => new Customer
                {
                    FirstName = "FN_" + i,
                    LastName = "LN_" + i,
                    Email = $"user{i}@demo.com"
                })
                .ToList();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.BulkInsert(list);
            }

            return RedirectToAction("Index");
        }
        public IActionResult BulkUpdateDemo()
        {
            DapperPlusMapping.EnsureMapped();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var customersToUpdate = connection.Query<Customer>(
                    "SELECT TOP 5 * FROM dbo.Customers ORDER BY CustomerID DESC"
                ).ToList();

                customersToUpdate.ForEach(c =>
                    c.Email = "updated_" + c.Email
                );

                connection.BulkUpdate(customersToUpdate);
            }

            return RedirectToAction("Index");
        }
        // GET: Customer/BulkDeleteDemo
        public IActionResult BulkDeleteDemo()
        {
            DapperPlusMapping.EnsureMapped();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                // Παίρνουμε τους 3 τελευταίους
                var customersToRemove = connection.Query<Customer>(
                    "SELECT  * FROM dbo.Customers WHERE CustomerID >50 AND CustomerID < 58 ORDER BY CustomerID DESC"
                ).ToList();

                connection.BulkDelete(customersToRemove);
            }

            return RedirectToAction("Index");
        }
        // GET: Customer/BulkMergeDemo
        public IActionResult BulkMergeDemo()
        {
            DapperPlusMapping.EnsureMapped();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                // 1) Παίρνουμε 1 υπάρχοντα (τελευταίο)
                var existing = connection.Query<Customer>(
                    "SELECT TOP 1 * FROM dbo.Customers ORDER BY CustomerID DESC"
                ).SingleOrDefault();

                var list = new List<Customer>();

                // Αν υπάρχει, τον πειράζουμε ώστε να δούμε UPDATE
                if (existing != null)
                {
                    existing.Email = "merged_" + existing.Email;
                    list.Add(existing);
                }

                // 2) Φτιάχνουμε 1 καινούργιο (CustomerID=0 -> INSERT)
                var newOne = new Customer
                {
                    FirstName = "Merge",
                    LastName = "Insert",
                    Email = "merge_insert@demo.com"
                };
                list.Add(newOne);

                // 3) Ένα call που κάνει και UPDATE και INSERT
                connection.BulkMerge(list);
            }

            return RedirectToAction("Index");
        }

    }
}
