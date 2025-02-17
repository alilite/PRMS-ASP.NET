using PropertyManagement.Models;
using System.Linq;
using System.Web.Mvc;

namespace PropertyManagement.Controllers
{
    public class OwnerController : Controller
    {
        private readonly PropertyManagementDbContext _db = new PropertyManagementDbContext();

        // Manage Tenants
        public ActionResult ManageTenants(string search)
        {
            var tenants = string.IsNullOrEmpty(search)
                ? _db.Tenants.ToList()
                : _db.Tenants.Where(t => t.Name.Contains(search) || t.Email.Contains(search)).ToList();

            ViewBag.Search = search; // Retain search term in the view
            return View(tenants);
        }

        // Manage Property Managers
        public ActionResult ManagePropertyManagers(string search)
        {
            var managers = string.IsNullOrEmpty(search)
                ? _db.PropertyManagers.ToList()
                : _db.PropertyManagers.Where(pm => pm.Name.Contains(search) || pm.Email.Contains(search)).ToList();

            ViewBag.Search = search; // Retain search term in the view
            return View(managers);
        }

        // Delete Tenant
        public ActionResult DeleteTenant(int id)
        {
            var tenant = _db.Tenants.Find(id);
            if (tenant != null)
            {
                _db.Tenants.Remove(tenant);
                _db.SaveChanges();
            }
            return RedirectToAction("ManageTenants");
        }

        // Delete Property Manager
        public ActionResult DeletePropertyManager(int id)
        {
            var manager = _db.PropertyManagers.Find(id);
            if (manager != null)
            {
                _db.PropertyManagers.Remove(manager);
                _db.SaveChanges();
            }
            return RedirectToAction("ManagePropertyManagers");
        }

        // Create Tenant (GET)
        public ActionResult CreateTenant()
        {
            return View();
        }

        // Create Tenant (POST)
        [HttpPost]
        public ActionResult CreateTenant(Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                _db.Tenants.Add(tenant);
                _db.SaveChanges();
                return RedirectToAction("ManageTenants");
            }
            return View(tenant);
        }

        // Update Tenant (GET)
        public ActionResult UpdateTenant(int id)
        {
            var tenant = _db.Tenants.Find(id);
            if (tenant == null) return HttpNotFound();

            return View(tenant);
        }

        // Update Tenant (POST)
        [HttpPost]
        public ActionResult UpdateTenant(Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(tenant).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("ManageTenants");
            }
            return View(tenant);
        }

        // Create Property Manager (GET)
        public ActionResult CreatePropertyManager()
        {
            ViewBag.Owners = _db.Owners.ToList();
            return View();
        }

        // Create Property Manager (POST)
        [HttpPost]
        public ActionResult CreatePropertyManager(PropertyManager manager, int OwnerId)
        {
            if (ModelState.IsValid)
            {
                manager.OwnerId = OwnerId; // Set the selected OwnerId
                _db.PropertyManagers.Add(manager);
                _db.SaveChanges();
                return RedirectToAction("ManagePropertyManagers");
            }
            ViewBag.Owners = _db.Owners.ToList(); // Reload owners in case of an error
            return View(manager);
        }

        // Update Property Manager (GET)
        public ActionResult UpdatePropertyManager(int id)
        {
            var manager = _db.PropertyManagers.Find(id);
            if (manager == null) return HttpNotFound();

            return View(manager);
        }

        // Update Property Manager (POST)
        [HttpPost]
        public ActionResult UpdatePropertyManager(PropertyManager manager)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(manager).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("ManagePropertyManagers");
            }
            return View(manager);
        }
    }
}
