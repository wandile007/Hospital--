using Final.Models;
using Health__.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Health__.Controllers
{
    public class DoctorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        // GET: Doctors
        public ActionResult Index()
        {
            return View(db.Doctors.ToList());
        }

        // GET: Doctors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null) return HttpNotFound();

            return View(doctor);
        }

        // GET: Doctors/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Doctors/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "DoctorId,Name,Surname,Specialty,OfficeLocation,PhoneNumber,Email,DateJoined,IsAvailable,Password,ConfirmPassword")] Doctor doctor)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Doctors.Add(doctor);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(doctor);
        //}

        // GET: Doctors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null) return HttpNotFound();

            return View(doctor);
        }

        // POST: Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DoctorId,Name,Surname,Specialty,OfficeLocation,PhoneNumber,Email,DateJoined,IsAvailable,IdentityUserId")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(doctor);
        }

        
        // GET: Doctor/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Doctor/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Doctor model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // Save doctor profile
                    model.IdentityUserId = user.Id;
                    db.Doctors.Add(model);
                    db.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            return View(model);
        }

        // GET: Doctors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null) return HttpNotFound();

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                TempData["Error"] = "Doctor not found.";
                return RedirectToAction("Index");
            }

            var hasAppointments = db.Appointments.Any(a => a.DoctorId == id);
            if (hasAppointments)
            {
                TempData["Error"] = "Cannot delete doctor with existing appointments.";
                return RedirectToAction("Index");
            }

            db.Doctors.Remove(doctor);
            db.SaveChanges();

            TempData["Message"] = "Doctor has been successfully deleted.";
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
