using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Final.Models;
using Health__.Models;

namespace Health__.Controllers
{
    public class MedicalRecordsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MedicalRecords
        public ActionResult Index()
        {
            var medicalRecords = db.MedicalRecords.Include(m => m.Doctor).Include(m => m.Patient);
            return View(medicalRecords.ToList());
        }

        // GET: MedicalRecords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalRecord medicalRecord = db.MedicalRecords.Find(id);
            if (medicalRecord == null)
            {
                return HttpNotFound();
            }
            return View(medicalRecord);
        }

        // GET: MedicalRecords/Create
        public ActionResult Create()
        {
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "Name");
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FullName");
            return View();
        }

        // POST: MedicalRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RecordId,PatientId,DoctorId,Diagnosis,TreatmentPlan,PrescribedMedications,LabTestsOrdered,VisitDate,CreatedDate,Notes,IsFollowUpRequired,FollowUpDate")] MedicalRecord medicalRecord)
        {
            if (ModelState.IsValid)
            {
                db.MedicalRecords.Add(medicalRecord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "Name", medicalRecord.DoctorId);
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FullName", medicalRecord.PatientId);
            return View(medicalRecord);
        }

        // GET: MedicalRecords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalRecord medicalRecord = db.MedicalRecords.Find(id);
            if (medicalRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "Name", medicalRecord.DoctorId);
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FullName", medicalRecord.PatientId);
            return View(medicalRecord);
        }

        // POST: MedicalRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RecordId,PatientId,DoctorId,Diagnosis,TreatmentPlan,PrescribedMedications,LabTestsOrdered,VisitDate,CreatedDate,Notes,IsFollowUpRequired,FollowUpDate")] MedicalRecord medicalRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicalRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "Name", medicalRecord.DoctorId);
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "FullName", medicalRecord.PatientId);
            return View(medicalRecord);
        }

        // GET: MedicalRecords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalRecord medicalRecord = db.MedicalRecords.Find(id);
            if (medicalRecord == null)
            {
                return HttpNotFound();
            }
            return View(medicalRecord);
        }

        // POST: MedicalRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MedicalRecord medicalRecord = db.MedicalRecords.Find(id);
            db.MedicalRecords.Remove(medicalRecord);
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
