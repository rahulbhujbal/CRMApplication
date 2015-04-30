using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRMApplication.Models;
using CRMApplication.Repositories;
using StructureMap;

namespace CRMApplication.Controllers
{
    [Authorize]
    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    public class CustomerController : Controller
    {
        private IDataRepository dataRepository;

        public CustomerController()
        {
            dataRepository = (IDataRepository)ObjectFactory.GetInstance(typeof(IDataRepository));
        }

        public CustomerController(IDataRepository dataRepo)
        {
            dataRepository = dataRepo;
        }

        // GET: Customers
        public ActionResult Index()
        {
            return View(dataRepository.GetAllCustomers());
        }

        // GET: Customers/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerModel customer = dataRepository.GetCustomer(id);
            
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Select/5
        public ActionResult Select(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CustomerModel customer = dataRepository.GetCustomerWithNotes(id);
            
            ViewBag.Customer = (CustomerModel)customer;
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer.Notes);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerId,FirstName,LastName,CompanyName,Email,City,State,StreetAddress1,StreetAddress2,Zip,County,Country,PhoneNumber")] CustomerModel customer)
        {
            if (ModelState.IsValid)
            {
                dataRepository.CreateCustomer(customer);
                
                return RedirectToAction("Index");
            }

            return View(customer);
        }


        // GET: Customers/CreateNote
        [HttpGet]
        public ActionResult CreateNote(decimal id)
        {
            Note note = dataRepository.GetNoteForCustomer(id);
            return View("CreateNote", note);
        }

        // POST: Customers/CreateNote
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNote([Bind(Include = "NoteId,NoteBody,CreatedDate,CreatedBy,CustomerId")] Note note)
        {
            if (ModelState.IsValid)
            {
                dataRepository.CreateNoteForCustomer(note, User.Identity.Name);
                return RedirectToAction("Select", new { id = note.CustomerId });
            }

            return View(note);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Customer customer = dataRepository.GetCustomerObject(id);
            
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerId,FirstName,LastName,CompanyName,Email,City,State,StreetAddress1,StreetAddress2,Zip,County,Country,PhoneNumber")] CustomerModel customer)
        {
            if (ModelState.IsValid)
            {
                dataRepository.UpdateCustomer(ref customer);
            
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerModel customer = dataRepository.GetCustomer(id);
            
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            dataRepository.DeleteCustomer(id);
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
