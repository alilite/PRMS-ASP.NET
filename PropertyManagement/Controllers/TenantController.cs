using PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PropertyManagement.Controllers
{
    public class TenantController : Controller
    {
        private readonly PropertyManagementDbContext _db = new PropertyManagementDbContext();

        // View Apartments
        public ActionResult ViewApartments(string searchCriteria, decimal? minRent, decimal? maxRent, string availability)
        {
            // Fetch apartments based on search criteria
            var apartments = _db.Apartments.Include("PropertyManager").Include("Property").AsQueryable();

            if (!string.IsNullOrEmpty(searchCriteria))
            {
                apartments = apartments.Where(a => a.Property.Location.Contains(searchCriteria) || a.Property.Name.Contains(searchCriteria));
            }
            if (minRent.HasValue)
            {
                apartments = apartments.Where(a => a.RentAmount >= minRent);
            }
            if (maxRent.HasValue)
            {
                apartments = apartments.Where(a => a.RentAmount <= maxRent);
            }
            if (!string.IsNullOrEmpty(availability))
            {
                apartments = apartments.Where(a => a.AvailabilityStatus == availability);
            }

            return View(apartments.ToList());
        }

        // Make Appointment (GET)
        public ActionResult MakeAppointment(int id)
        {
            var apartment = _db.Apartments.Include("PropertyManager").FirstOrDefault(a => a.ApartmentId == id);
            if (apartment == null) return HttpNotFound();

            ViewBag.ApartmentDetails = apartment;
            return View(new Appointment { ApartmentId = id });
        }

        // Make Appointment (POST)
        [HttpPost]
        public ActionResult MakeAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.TenantId = Convert.ToInt32(Session["UserId"]); // Assign logged-in tenant
                appointment.ManagerId = _db.Apartments.Find(appointment.ApartmentId)?.ManagerId ?? 0;
                appointment.Status = "Scheduled";
                appointment.AppointmentTime = DateTime.Now.TimeOfDay; // Extract only the time portion
                _db.Appointments.Add(appointment);
                _db.SaveChanges();
                return RedirectToAction("ViewApartments");
            }

            return View(appointment);
        }

        public ActionResult SendMessage(int id)
        {
            // Automatically set the receiver as a manager
            var model = new Message
            {
                ReceiverId = id,
                ReceiverRole = "Manager" // Fixed to "Manager"
            };

            ViewBag.ReceiverId = id;
            ViewBag.ReceiverRole = "Manager";

            return View(model);
        }


        [HttpPost]
        public ActionResult SendMessage(Message message)
        {
            if (ModelState.IsValid)
            {
                message.SenderId = Convert.ToInt32(Session["UserId"]); // Logged-in tenant's ID
                message.SenderRole = "Tenant"; // Fixed to "Tenant"
                message.Timestamp = DateTime.Now;

                message.ReceiverRole = "Manager";

                _db.Messages.Add(message);
                _db.SaveChanges();

                return RedirectToAction("ViewMessages");
            }

            ViewBag.ReceiverId = message.ReceiverId;
            ViewBag.ReceiverRole = "Manager";
            ViewBag.SenderId = Convert.ToInt32(Session["UserId"]);
            ViewBag.SenderRole = "Tenant";

            return View(message);
        }




        public ActionResult ViewMessages()
        {
            int tenantId = Convert.ToInt32(Session["UserId"]);
            string role = Session["Role"].ToString();

            var messages = _db.Messages
                .Where(m =>
                    (m.SenderId == tenantId && m.SenderRole == role) ||
                    (m.ReceiverId == tenantId && m.ReceiverRole == role))
                .OrderByDescending(m => m.Timestamp)
                .ToList();

            return View(messages);
        }


    }
}
