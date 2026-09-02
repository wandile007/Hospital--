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
    public class PatientRegistrationViewModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PatientRegistrationViewModels
        public ActionResult Index()
        {
            return View(db.PatientRegistrationViewModels.ToList());
        }

        // GET: PatientRegistrationViewModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientRegistrationViewModel patientRegistrationViewModel = db.PatientRegistrationViewModels.Find(id);
            if (patientRegistrationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(patientRegistrationViewModel);
        }

        // GET: PatientRegistrationViewModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PatientRegistrationViewModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,FullName,DateOfBirth,Gender,PhoneNumber,Email,Address,Password,ConfirmPassword")] PatientRegistrationViewModel patientRegistrationViewModel)
        {
            if (ModelState.IsValid)
            {
                db.PatientRegistrationViewModels.Add(patientRegistrationViewModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patientRegistrationViewModel);
        }

        // GET: PatientRegistrationViewModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientRegistrationViewModel patientRegistrationViewModel = db.PatientRegistrationViewModels.Find(id);
            if (patientRegistrationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(patientRegistrationViewModel);
        }

        // POST: PatientRegistrationViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,FullName,DateOfBirth,Gender,PhoneNumber,Email,Address,Password,ConfirmPassword")] PatientRegistrationViewModel patientRegistrationViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patientRegistrationViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patientRegistrationViewModel);
        }

        // GET: PatientRegistrationViewModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientRegistrationViewModel patientRegistrationViewModel = db.PatientRegistrationViewModels.Find(id);
            if (patientRegistrationViewModel == null)
            {
                return HttpNotFound();
            }
            return View(patientRegistrationViewModel);
        }

        // POST: PatientRegistrationViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PatientRegistrationViewModel patientRegistrationViewModel = db.PatientRegistrationViewModels.Find(id);
            db.PatientRegistrationViewModels.Remove(patientRegistrationViewModel);
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
