using Final.Models;
using Health__.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows.Media.Imaging;

namespace Health__.Controllers
{
    public class AppointmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Appointments
        public ActionResult Index(string status, DateTime? date)
        {
            var appointments = db.Appointments
                .Include(a => a.Doctor)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                // Assuming status is an enum like AppointmentStatus
                if (Enum.TryParse(status, out AppointmentStatus parsedStatus))
                {
                    appointments = appointments.Where(a => a.Status == parsedStatus);
                }
            }

            if (date.HasValue)
            {
                appointments = appointments.Where(a => DbFunctions.TruncateTime(a.AppointmentDate) == date.Value.Date);
            }

            return View(appointments.ToList());
        }


        // GET: Appointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: Appointments/Create
        public ActionResult Create()
        {
            var doctors = db.Doctors
                .Select(d => new
                {
                    DoctorId = d.DoctorId,
                    DisplayName = "Dr " + d.Surname + " (" + d.Specialty + ") - " + (d.IsAvailable ? "Available" : "Unavailable")
                }).ToList();
            ViewBag.PatientId = new SelectList(doctors, "PatientId", "DisplayName");
            ViewBag.DoctorId = new SelectList(doctors, "DoctorId", "DisplayName");
            return View();
        }



        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DoctorId,PatientId,AppointmentDate,Status,Notes,CreatedOn,BookingDate")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var doctors = db.Doctors
                .Select(d => new
                {
                    DoctorId = d.DoctorId,
                    DisplayName = "Dr " + d.Surname + " (" + d.Specialty + ") - " + (d.IsAvailable ? "Available" : "Unavailable")
                }).ToList();
          
            ViewBag.DoctorId = new SelectList(doctors, "DoctorId", "DisplayName", appointment.DoctorId);
            return View(appointment);
        }


        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            var doctors = db.Doctors
                .Select(d => new
                {
                    DoctorId = d.DoctorId,
                    DisplayName = "Dr " + d.Surname + " (" + d.Specialty + ") - " + (d.IsAvailable ? "Available" : "Unavailable")
                }).ToList();
            
            ViewBag.DoctorId = new SelectList(doctors, "DoctorId", "DisplayName", appointment.DoctorId);
            return View(appointment);
        }
     
        [Authorize(Roles = "Admin,Doctor")]
        public ActionResult ChangeStatus(int id)
        {
            var appointment = db.Appointments.Find(id);
            if (appointment == null)
                return HttpNotFound();

            // Example: cycle through statuses
            switch (appointment.Status)
            {
                case AppointmentStatus.Pending:
                    appointment.Status = AppointmentStatus.Confirmed;
                    break;
                case AppointmentStatus.Confirmed:
                    appointment.Status = AppointmentStatus.Completed;
                    break;
                default:
                    appointment.Status = AppointmentStatus.Pending;
                    break;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [Authorize]
        public ActionResult CancelAppointment(int id)
        {
            var appointment = db.Appointments.Find(id);
            if (appointment == null)
                return HttpNotFound();

            appointment.Status = AppointmentStatus.Cancelled;
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        [Authorize]
        public ActionResult RescheduleAppointment(int id)
        {
            var appointment = db.Appointments.Find(id);
            if (appointment == null)
                return HttpNotFound();

            return View(appointment); // View name: RescheduleAppointment.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RescheduleAppointment(Appointment updated)
        {
            var appointment = db.Appointments.Find(updated.Id);
            if (appointment == null)
                return HttpNotFound();

            appointment.AppointmentDate = updated.AppointmentDate;
            appointment.Status = AppointmentStatus.Rescheduled;
            db.SaveChanges();

            return RedirectToAction("Index");
        }




        // GET: Appointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RescheduleForm(int id)
        {
            var appointment = db.Appointments.Find(id);
            if (appointment == null)
                return HttpNotFound();

            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RescheduleForm(Appointment updated)
        {
            var appointment = db.Appointments.Find(updated.Id);
            if (appointment == null)
                return HttpNotFound();

            appointment.AppointmentDate = updated.AppointmentDate;
            appointment.Status = AppointmentStatus.Rescheduled;
            db.SaveChanges();

            return RedirectToAction("Index");
        }


       


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
