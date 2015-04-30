using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRMApplication;
using System.Web.Security;
using CRMApplication.Repositories;
using StructureMap;

namespace CRMApplication.Controllers
{
    public class UsersController : Controller
    {
        private IDataRepository dataRepository;
        private CRMDbEntities db = new CRMDbEntities();

        public UsersController()
        {
            dataRepository = (IDataRepository)ObjectFactory.GetInstance(typeof(IDataRepository));
        }

        public UsersController(IDataRepository dataRepo)
        {
            dataRepository = dataRepo;
        }

        // GET: Users/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if (dataRepository.IsValidUser(loginModel))
            {
                FormsAuthentication.SetAuthCookie(loginModel.UserName, false);
                return RedirectToAction("Index", "Customer");
            }

            loginModel.Password = string.Empty;
            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return View(loginModel);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,FirstName,LastName,EmailAddress,Password,ConfirmPassword,UserName")] User user)
        {
            if (user.Password != user.ConfirmPassword)
            {
                ModelState.AddModelError("PasswordMismatch", "Passwords does not match");
                return View(user);
            }

            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(user.UserName, false);
                dataRepository.CreateUser(user);
                return RedirectToAction("Index", "Customer");
            }

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,LastName,EmailAddress,Password,UserName")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dataRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
