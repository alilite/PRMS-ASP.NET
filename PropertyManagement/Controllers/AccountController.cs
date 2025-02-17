using PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PropertyManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly PropertyManagementDbContext _db = new PropertyManagementDbContext();


        // Owner Dashboard
        public ActionResult OwnerDashboard()
        {
            int ownerId = Convert.ToInt32(Session["UserId"]); // Retrieve the logged-in tenant's ID

            // Fetch tenant details
            var owner = _db.Owners.FirstOrDefault(o => o.OwnerId == ownerId);
            if (owner == null)
            {
                return HttpNotFound(); // Return an error if no tenant is found
            }

            return View(owner); // Pass the tenant object to the view
        }

        // Tenant Dashboard
        public ActionResult TenantDashboard()
        {
            int tenantId = Convert.ToInt32(Session["UserId"]); // Retrieve the logged-in tenant's ID

            // Fetch tenant details
            var tenant = _db.Tenants.FirstOrDefault(t => t.TenantId == tenantId);
            if (tenant == null)
            {
                return HttpNotFound(); // Return an error if no tenant is found
            }

            return View(tenant); // Pass the tenant object to the view
        }

        // Manager Dashboard
        public ActionResult ManagerDashboard()
        {
            int managerId = Convert.ToInt32(Session["UserId"]); // Retrieve the logged-in manager's ID from session

            // Fetch manager details from the database
            var manager = _db.PropertyManagers.Find(managerId);

            return View(manager); // Pass the manager details to the view
        }



        // Sign Up (For Owners and Tenants only)
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(string name, string email, string password, string confirmPassword, string role)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                ModelState.AddModelError("", "All fields are required.");
                return View();
            }

            if (password != confirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return View();
            }

            if (role == "Owner")
            {
                if (_db.Owners.Any(o => o.Email == email))
                {
                    ModelState.AddModelError("", "This email is already registered.");
                    return View();
                }

                var owner = new Owner
                {
                    Name = name,
                    Email = email,
                    Phone = "N/A", // Can be updated later
                    Password = password // Hash later
                };
                _db.Owners.Add(owner);
            }
            else if (role == "Tenant")
            {
                if (_db.Tenants.Any(t => t.Email == email))
                {
                    ModelState.AddModelError("", "This email is already registered.");
                    return View();
                }

                var tenant = new Tenant
                {
                    Name = name,
                    Email = email,
                    Phone = "N/A", // Can be updated later
                    Password = password // Hash later
                };
                _db.Tenants.Add(tenant);
            }
            else
            {
                ModelState.AddModelError("", "Invalid role selected.");
                return View();
            }

            _db.SaveChanges();
            return RedirectToAction("Login");
        }

        // Login (For Owners, Managers, and Tenants)
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email and password are required.");
                return View();
            }

            // Check Owners
            var owner = _db.Owners.FirstOrDefault(o => o.Email == email && o.Password == password);
            if (owner != null)
            {
                Session["UserId"] = owner.OwnerId;
                Session["Role"] = "Owner";
                return RedirectToAction("OwnerDashboard");
            }

            // Check Managers
            var manager = _db.PropertyManagers.FirstOrDefault(pm => pm.Email == email && pm.Password == password);
            if (manager != null)
            {
                Session["UserId"] = manager.ManagerId;
                Session["Role"] = "Manager";
                return RedirectToAction("ManagerDashboard");
            }

            // Check Tenants
            var tenant = _db.Tenants.FirstOrDefault(t => t.Email == email && t.Password == password);
            if (tenant != null)
            {
                Session["UserId"] = tenant.TenantId;
                Session["Role"] = "Tenant";
                return RedirectToAction("TenantDashboard");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View();
        }

    }
}