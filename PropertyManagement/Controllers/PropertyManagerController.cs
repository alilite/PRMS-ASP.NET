using System.Data.Entity;
using System;
using System.Linq;
using System.Web.Mvc;
using PropertyManagement.Models;

namespace PropertyManagement.Controllers
{
    public class PropertyManagerController : Controller
    {
        private readonly PropertyManagementDbContext _db = new PropertyManagementDbContext();

        // Manage Apartments
        public ActionResult ManageApartments(string searchTerm)
        {
            int managerId = Convert.ToInt32(Session["UserId"]); // Assuming the manager is logged in and their ID is in the session
            var apartments = _db.Apartments
                                .Where(a => a.ManagerId == managerId)
                                .Include(a => a.Property)
                                .ToList();

            // Apply search filter
            if (!string.IsNullOrEmpty(searchTerm))
            {
                apartments = apartments
                             .Where(a => a.Number.Contains(searchTerm) ||
                                         a.Property.Name.Contains(searchTerm) ||
                                         a.AvailabilityStatus.Contains(searchTerm))
                             .ToList();
            }

            ViewBag.SearchTerm = searchTerm; // Pass the search term back to the view
            return View(apartments);
        }

        // Create Apartment (GET)
        public ActionResult CreateApartment()
        {
            ViewBag.Properties = new SelectList(_db.Properties, "PropertyId", "Name");
            return View();
        }

        // Create Apartment (POST)
        [HttpPost]
        public ActionResult CreateApartment(Apartment apartment)
        {
            if (ModelState.IsValid)
            {
                apartment.ManagerId = Convert.ToInt32(Session["UserId"]); // Automatically assign the logged-in manager
                _db.Apartments.Add(apartment);
                _db.SaveChanges();
                return RedirectToAction("ManageApartments");
            }
            ViewBag.Properties = new SelectList(_db.Properties, "PropertyId", "Name", apartment.PropertyId);
            return View(apartment);
        }

        // Edit Apartment (GET)
        // Edit Apartment (GET)
        public ActionResult EditApartment(int id)
        {
            int managerId = Convert.ToInt32(Session["UserId"]); // Retrieve the logged-in manager's ID

            var apartment = _db.Apartments.Include("Property").Include("Property.Owner")
                                           .FirstOrDefault(a => a.ApartmentId == id && a.ManagerId == managerId);

            if (apartment == null) // Ensure the apartment belongs to the manager
            {
                return HttpNotFound();
            }

            return View(apartment); // Pass the apartment to the view
        }

        // Edit Apartment (POST)
        [HttpPost]
        public ActionResult EditApartment(Apartment apartment)
        {
            if (ModelState.IsValid)
            {
                // Fetch the existing apartment record
                var existingApartment = _db.Apartments.Include("Property").FirstOrDefault(a => a.ApartmentId == apartment.ApartmentId);

                if (existingApartment != null)
                {
                    // Update only the editable fields
                    existingApartment.Number = apartment.Number;
                    existingApartment.RentAmount = apartment.RentAmount;
                    existingApartment.AvailabilityStatus = apartment.AvailabilityStatus;
                    existingApartment.Bathroom = apartment.Bathroom;
                    existingApartment.Bedroom = apartment.Bedroom;

                    _db.SaveChanges();
                    return RedirectToAction("ManageApartments");
                }
            }

            return View(apartment);
        }



        // Delete Apartment
        public ActionResult DeleteApartment(int id)
        {
            var apartment = _db.Apartments.Find(id);
            if (apartment != null)
            {
                _db.Apartments.Remove(apartment);
                _db.SaveChanges();
            }
            return RedirectToAction("ManageApartments");
        }

        public ActionResult ManageProperties(string searchTerm)
        {
            int managerId = Convert.ToInt32(Session["UserId"]); // Get the logged-in manager's ID

            // Fetch properties managed by this manager
            var properties = _db.Properties.Where(p => p.ManagerId == managerId).ToList();

            // Apply search filter
            if (!string.IsNullOrEmpty(searchTerm))
            {
                properties = properties
                             .Where(p => p.Name.Contains(searchTerm) ||
                                         p.Location.Contains(searchTerm) ||
                                         p.Description.Contains(searchTerm))
                             .ToList();
            }

            ViewBag.SearchTerm = searchTerm; // Pass the search term back to the view
            return View(properties);
        }


        // Create Property (GET)
        public ActionResult CreateProperty()
        {
            int managerId = Convert.ToInt32(Session["UserId"]); // Get the logged-in manager's ID

            // Populate dropdown for Owner selection
            ViewBag.Owners = new SelectList(_db.Owners, "OwnerId", "Name");

            return View();
        }

        // Create Property (POST)
        [HttpPost]
        public ActionResult CreateProperty(Property property)
        {
            if (ModelState.IsValid)
            {
                property.ManagerId = Convert.ToInt32(Session["UserId"]); // Assign logged-in manager
                _db.Properties.Add(property);
                _db.SaveChanges();
                return RedirectToAction("ManageProperties");
            }

            ViewBag.Owners = new SelectList(_db.Owners, "OwnerId", "Name", property.OwnerId);
            return View(property);
        }


        // Edit Property (GET)
        public ActionResult EditProperty(int id)
        {
            int managerId = Convert.ToInt32(Session["UserId"]);

            // Fetch property and ensure it belongs to the manager
            var property = _db.Properties.FirstOrDefault(p => p.PropertyId == id && p.ManagerId == managerId);
            if (property == null) return HttpNotFound();

            ViewBag.Owners = new SelectList(_db.Owners, "OwnerId", "Name", property.OwnerId);
            return View(property);
        }

        // Edit Property (POST)
        [HttpPost]
        public ActionResult EditProperty(Property property)
        {
            if (ModelState.IsValid)
            {
                var existingProperty = _db.Properties.Find(property.PropertyId);
                if (existingProperty != null)
                {
                    // Update only allowed fields
                    existingProperty.Name = property.Name;
                    existingProperty.Location = property.Location;
                    existingProperty.Description = property.Description;

                    _db.SaveChanges();
                    return RedirectToAction("ManageProperties");
                }
            }

            ViewBag.Owners = new SelectList(_db.Owners, "OwnerId", "Name", property.OwnerId);
            return View(property);
        }


        public ActionResult DeleteProperty(int id)
        {
            int managerId = Convert.ToInt32(Session["UserId"]);

            // Fetch property and ensure it belongs to the manager
            var property = _db.Properties.FirstOrDefault(p => p.PropertyId == id && p.ManagerId == managerId);
            if (property != null)
            {
                _db.Properties.Remove(property);
                _db.SaveChanges();
            }

            return RedirectToAction("ManageProperties");
        }

        public ActionResult ManageApartmentStatuses(string searchTerm)
        {
            int managerId = Convert.ToInt32(Session["UserId"]); // Retrieve the logged-in manager's ID

            // Fetch apartments managed by this manager
            var apartments = _db.Apartments.Where(a => a.ManagerId == managerId).Include(a => a.Property).ToList();

            // Apply search filter
            if (!string.IsNullOrEmpty(searchTerm))
            {
                apartments = apartments
                             .Where(a => a.Number.Contains(searchTerm) ||
                                         a.Property.Name.Contains(searchTerm) ||
                                         a.AvailabilityStatus.Contains(searchTerm))
                             .ToList();
            }

            ViewBag.SearchTerm = searchTerm; // Pass the search term back to the view
            return View(apartments);
        }

        // Update Apartment Status (GET)
        public ActionResult UpdateApartmentStatus(int id)
        {
            int managerId = Convert.ToInt32(Session["UserId"]); // Retrieve the logged-in manager's ID

            // Fetch the apartment and ensure it belongs to the manager
            var apartment = _db.Apartments.FirstOrDefault(a => a.ApartmentId == id && a.ManagerId == managerId);
            if (apartment == null) return HttpNotFound();

            return View(apartment);
        }

        // Update Apartment Status (POST)
        [HttpPost]
        public ActionResult UpdateApartmentStatus(Apartment apartment)
        {
            if (ModelState.IsValid)
            {
                // Update the status field only
                var existingApartment = _db.Apartments.Find(apartment.ApartmentId);
                if (existingApartment != null)
                {
                    existingApartment.AvailabilityStatus = apartment.AvailabilityStatus;
                    _db.SaveChanges();
                    return RedirectToAction("ManageApartmentStatuses");
                }
            }

            return View(apartment);
        }




        public ActionResult ManageAppointments()
        {
            int managerId = Convert.ToInt32(Session["UserId"]); // Retrieve the logged-in manager's ID

            // Fetch appointments managed by this manager
            var appointments = _db.Appointments
                                  .Where(a => a.ManagerId == managerId)
                                  .Include(a => a.Tenant)
                                  .Include(a => a.Apartment)
                                  .ToList();

            return View(appointments);
        }


        // Create Appointment (GET)
        public ActionResult CreateAppointment()
        {
            int managerId = Convert.ToInt32(Session["UserId"]); // Retrieve the logged-in manager's ID

            // Populate dropdowns for tenants and apartments
            ViewBag.Tenants = new SelectList(_db.Tenants, "TenantId", "Name");
            ViewBag.Apartments = new SelectList(
                _db.Apartments.Where(a => a.ManagerId == managerId), "ApartmentId", "Number"
            );

            return View();
        }

        // Create Appointment (POST)
        [HttpPost]
        public ActionResult CreateAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.ManagerId = Convert.ToInt32(Session["UserId"]); // Assign logged-in manager
                appointment.Status = "Scheduled"; // Default status
                _db.Appointments.Add(appointment);
                _db.SaveChanges();
                return RedirectToAction("ManageAppointments");
            }

            // Repopulate dropdowns in case of validation errors
            int managerId = Convert.ToInt32(Session["UserId"]);
            ViewBag.Tenants = new SelectList(_db.Tenants, "TenantId", "Name", appointment.TenantId);
            ViewBag.Apartments = new SelectList(
                _db.Apartments.Where(a => a.ManagerId == managerId), "ApartmentId", "Number", appointment.ApartmentId
            );

            return View(appointment);
        }


        // Edit Appointment (GET)
        public ActionResult EditAppointment(int id)
        {
            int managerId = Convert.ToInt32(Session["UserId"]); // Retrieve the logged-in manager's ID

            var appointment = _db.Appointments.Include("Tenant").Include("Apartment")
                                               .FirstOrDefault(a => a.AppointmentId == id && a.ManagerId == managerId);
            if (appointment == null) return HttpNotFound();

            ViewBag.Tenants = new SelectList(_db.Tenants, "TenantId", "Name", appointment.TenantId);
            ViewBag.Apartments = new SelectList(
                _db.Apartments.Where(a => a.ManagerId == managerId), "ApartmentId", "Number", appointment.ApartmentId
            );

            return View(appointment);
        }

        // Edit Appointment (POST)
        [HttpPost]
        public ActionResult EditAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                var existingAppointment = _db.Appointments.Find(appointment.AppointmentId);
                if (existingAppointment != null)
                {
                    existingAppointment.AppointmentDate = appointment.AppointmentDate;
                    existingAppointment.AppointmentTime = appointment.AppointmentTime;
                    existingAppointment.Status = appointment.Status;
                    _db.SaveChanges();
                    return RedirectToAction("ManageAppointments");
                }
            }

            int managerId = Convert.ToInt32(Session["UserId"]);
            ViewBag.Tenants = new SelectList(_db.Tenants, "TenantId", "Name", appointment.TenantId);
            ViewBag.Apartments = new SelectList(
                _db.Apartments.Where(a => a.ManagerId == managerId), "ApartmentId", "Number", appointment.ApartmentId
            );

            return View(appointment);
        }

        public ActionResult DeleteAppointment(int id)
        {
            int managerId = Convert.ToInt32(Session["UserId"]); // Retrieve the logged-in manager's ID

            var appointment = _db.Appointments.FirstOrDefault(a => a.AppointmentId == id && a.ManagerId == managerId);
            if (appointment != null)
            {
                _db.Appointments.Remove(appointment);
                _db.SaveChanges();
            }

            return RedirectToAction("ManageAppointments");
        }

        public ActionResult ViewMessages()
        {
            int managerId = Convert.ToInt32(Session["UserId"]); // Logged-in manager's ID
            string role = Session["Role"].ToString(); // Should be "Manager"

            var messages = _db.Messages
                .Where(m => m.ReceiverId == managerId && m.ReceiverRole == role)
                .OrderByDescending(m => m.Timestamp)
                .ToList();

            return View(messages);
        }



        // Reply to a Message (GET)
        public ActionResult ReplyMessage(int id)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            string userRole = Session["Role"].ToString();

            // Fetch the message to reply to, ensuring the logged-in user is the receiver
            var originalMessage = _db.Messages.FirstOrDefault(m => m.MessageId == id && m.ReceiverId == userId && m.ReceiverRole == userRole);
            if (originalMessage == null)
            {
                return HttpNotFound(); // Message not found or the user is not authorized to reply
            }

            // Pass the original sender's details as the receiver for the reply
            var replyMessage = new Message
            {
                ReceiverId = originalMessage.SenderId,
                ReceiverRole = originalMessage.SenderRole
            };

            ViewBag.ReceiverName = originalMessage.SenderRole == "Tenant"
                ? _db.Tenants.FirstOrDefault(t => t.TenantId == originalMessage.SenderId)?.Name
                : _db.PropertyManagers.FirstOrDefault(pm => pm.ManagerId == originalMessage.SenderId)?.Name;

            return View(replyMessage);
        }



        [HttpPost]
        public ActionResult ReplyMessage(Message message)
        {
            if (ModelState.IsValid)
            {
                message.SenderId = Convert.ToInt32(Session["UserId"]); // Logged-in user's ID
                message.SenderRole = Session["Role"].ToString(); // Logged-in user's role
                message.Timestamp = DateTime.Now; // Current timestamp

                _db.Messages.Add(message);
                _db.SaveChanges();
                return RedirectToAction("ViewMessages");
            }

            ViewBag.Error = "An error occurred while sending the message. Please try again.";
            return View(message);
        }



        public ActionResult ViewConversation(int tenantId)
        {
            int managerId = Convert.ToInt32(Session["UserId"]);

            // Fetch all messages exchanged between the tenant and manager
            var messages = _db.Messages
                .Where(m =>
                    (m.SenderId == managerId && m.SenderRole == "Manager" && m.ReceiverId == tenantId && m.ReceiverRole == "Tenant") ||
                    (m.SenderId == tenantId && m.SenderRole == "Tenant" && m.ReceiverId == managerId && m.ReceiverRole == "Manager"))
                .OrderBy(m => m.Timestamp)
                .ToList();

            ViewBag.TenantName = _db.Tenants.FirstOrDefault(t => t.TenantId == tenantId)?.Name;
            return View(messages);
        }



    }
}
