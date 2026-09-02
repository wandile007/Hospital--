using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Final.Models;
using Health__.Models;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using static Final.Models.Booking;

namespace Health__.Controllers
{
    public class BookingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Bookings
        public ActionResult Index(int? doctorId, DateTime? date)
        {
            var bookings = db.Bookings.AsQueryable();

            if (doctorId.HasValue)
                bookings = bookings.Where(b => b.DoctorId == doctorId.Value);

            if (date.HasValue)
                bookings = bookings.Where(b => DbFunctions.TruncateTime(b.BookingDate) == date.Value.Date);

            return View(bookings.ToList());
        }

        public ActionResult ExportToPdf()
        {
            var bookings = db.Bookings.ToList();

            using (var ms = new MemoryStream())
            {
                var doc = new iTextSharp.text.Document();
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                doc.Add(new iTextSharp.text.Paragraph("Booking List"));
                var table = new PdfPTable(5); // Adjust column count as needed

                table.AddCell("Patient Name");
                table.AddCell("Contact");
                table.AddCell("Email");
                table.AddCell("Date");
                table.AddCell("Doctor");

                foreach (var b in bookings)
                {
                    table.AddCell(b.PatientName);
                    table.AddCell(b.ContactNumber);
                    table.AddCell(b.Email);
                    table.AddCell(b.BookingDate.ToString("dd MMM yyyy"));
                    table.AddCell(b.DoctorId.ToString());
                }

                doc.Add(table);
                doc.Close();

                return File(ms.ToArray(), "application/pdf", "Bookings.pdf");
            }
        }


        public ActionResult ExportToExcel()
        {
            var bookings = db.Bookings.ToList();

            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Bookings");

                // Headers
                ws.Cells[1, 1].Value = "Patient Name";
                ws.Cells[1, 2].Value = "Contact";
                ws.Cells[1, 3].Value = "Email";
                ws.Cells[1, 4].Value = "Date";
                ws.Cells[1, 5].Value = "Doctor";

                // Data
                for (int i = 0; i < bookings.Count; i++)
                {
                    var b = bookings[i];
                    ws.Cells[i + 2, 1].Value = b.PatientName;
                    ws.Cells[i + 2, 2].Value = b.ContactNumber;
                    ws.Cells[i + 2, 3].Value = b.Email;
                    ws.Cells[i + 2, 4].Value = b.BookingDate.ToString("dd MMM yyyy");
                    ws.Cells[i + 2, 5].Value = b.DoctorId;
                }

                var stream = new MemoryStream(package.GetAsByteArray());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Bookings.xlsx");
            }
        }



        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Booking booking = db.Bookings.Find(id);
            if (booking == null)
                return HttpNotFound();

            return View(booking);
        }

        // GET: Bookings/Create
        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.DoctorList = new SelectList(db.Doctors, "Id", "Name");
            return View();
        }
        [Authorize(Roles = "Patient")]
        public ActionResult MyBookings(string searchDoctor, string statusFilter, string dateFilter)
        {
            string currentUserId = User.Identity.GetUserId();
            var patient = db.Patients.FirstOrDefault(p => p.IdentityUserId == currentUserId);
            if (patient == null)
            {
                return HttpNotFound("Patient profile not found.");
            }

            var bookings = db.Bookings
                .Include(b => b.Doctor)
                .Where(b => b.PatientId == patient.PatientId);

            if (!string.IsNullOrEmpty(searchDoctor))
            {
                bookings = bookings.Where(b => b.Doctor.Name.Contains(searchDoctor));
            }

            if (!string.IsNullOrEmpty(statusFilter) && Enum.TryParse(statusFilter, out status parsedStatus))
            {
                bookings = bookings.Where(b => b.Status == parsedStatus);
            }

            if (!string.IsNullOrEmpty(dateFilter) && DateTime.TryParse(dateFilter, out DateTime parsedDate))
            {
                bookings = bookings.Where(b => DbFunctions.TruncateTime(b.BookingDate) == parsedDate.Date);
            }

            return View(bookings.OrderByDescending(b => b.BookingDate).ToList());
        }


        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.CreatedOn = DateTime.Now;
                booking.Status = Booking.status.Pending;

                db.Bookings.Add(booking);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            // Repopulate dropdown if model validation fails
            ViewBag.DoctorList = new SelectList(db.Doctors, "Id", "Name", booking.DoctorId);
            return View(booking);
        }




        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Booking booking = db.Bookings.Find(id);
            if (booking == null)
                return HttpNotFound();

            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PatientName,ContactNumber,Email,BookingDate,TimeSlot,DoctorId,Notes,CreatedOn")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "Booking updated successfully.";
                return RedirectToAction("Index");
            }

            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Booking booking = db.Bookings.Find(id);
            if (booking == null)
                return HttpNotFound();

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            TempData["Message"] = "Booking deleted successfully.";
            return RedirectToAction("Index");
        }

        // GET: Bookings/Reschedule/5
        public ActionResult Reschedule(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Booking booking = db.Bookings.Find(id);
            if (booking == null)
                return HttpNotFound();

            // Optional: restrict patient access to their own booking
            if (User.IsInRole("Patient") && booking.Email != User.Identity.GetUserName())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(booking);
        }

        // POST: Bookings/Reschedule/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reschedule([Bind(Include = "Id,BookingDate,TimeSlot")] Booking updated)
        {
            var booking = db.Bookings.Find(updated.Id);
            if (booking == null)
                return HttpNotFound();

            booking.BookingDate = updated.BookingDate;
            booking.TimeSlot = updated.TimeSlot;

            db.Entry(booking).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Message"] = "Booking rescheduled successfully.";
            return RedirectToAction("Index");
        }

        // POST: Bookings/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(int id)
        {
            var booking = db.Bookings.Find(id);
            if (booking == null)
                return HttpNotFound();

            // Optional: restrict patient access to their own booking
            if (User.IsInRole("Patient") && booking.Email != User.Identity.GetUserName())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            booking.Notes += "\n[Cancelled on " + DateTime.Now.ToShortDateString() + "]";
            db.Entry(booking).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Message"] = "Booking cancelled.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
