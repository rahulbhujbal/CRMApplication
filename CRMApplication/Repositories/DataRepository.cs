using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CRMApplication.Repositories;
using CRMApplication.Models;
using AutoMapper;

namespace CRMApplication
{
    public class DataRepository : IDataRepository
    {
        
        private static CRMDbEntities _db;
        public DataRepository()
        {
            Mapper.CreateMap<CustomerModel, Customer>();
            Mapper.CreateMap<Customer, CustomerModel>().ForMember(x=>x.Notes , opt=>opt.Ignore());

            Mapper.CreateMap<NoteModel, Note>();
            Mapper.CreateMap<Note, NoteModel>();

            //Mapper.CreateMap<List<CustomerModel>, List<Customer>>();
            //Mapper.CreateMap<List<Customer>, List<CustomerModel>>();
        }

        public CRMDbEntities db {
            get
            {
                if (_db == null)
                {
                    _db = new CRMDbEntities();
                }
                return _db;
            }
        }

        public List<CustomerModel> GetAllCustomers()
        {
            List<Customer> custList = db.Customers.ToList();

            List<CustomerModel> custModelList = new List<CustomerModel>();
            CustomerModel custModel;
            foreach (var customer in custList)
            {
                custModel = Mapper.Map<CustomerModel>(customer);
                custModelList.Add(custModel);
            }

            //List<CustomerModel> custModelList = Mapper.Map < List < Customer > ,List < CustomerModel >> (custList);
            return custModelList;
        }

        public CustomerModel GetCustomer(decimal id)
        {
            Customer customer = db.Customers.Find(id);
            return Mapper.Map<CustomerModel>(customer);
        }

        public CustomerModel GetCustomerWithNotes(decimal id)
        {
            Customer customer = db.Customers.Find(id);
            db.Entry(customer).Collection(n => n.Notes).Load();
            CustomerModel custModel = Mapper.Map<CustomerModel>(customer);
            custModel.Notes = new List<NoteModel>();
            NoteModel note;
            foreach (var noteitem in customer.Notes)
            {
                note = Mapper.Map<NoteModel>(noteitem);
                custModel.Notes.Add(note);
            }
            return custModel;
        }

        public void CreateCustomer(CustomerModel customer)
        {
            db.Customers.Add(Mapper.Map<Customer>(customer));
            db.SaveChanges();
        }

        public Note GetNoteForCustomer(decimal id)
        {
            Note note = new Note();
            note.CustomerId = id;
            note.Customer = db.Customers.Find(id);
            return note;
        }

        public Customer GetCustomerObject(decimal id)
        {
            Customer customer = db.Customers.Find(id);
            return customer;
        }

        public void CreateNoteForCustomer(Note note, string createdBy)
        {
            Customer customer = db.Customers.Find(note.CustomerId);
            note.CreatedDate = DateTime.Now;
            note.CreatedBy = createdBy;
            customer.Notes.Add(note);
            db.SaveChanges();
        }

        public void UpdateCustomer(ref CustomerModel customer)
        {
            Customer targetCustomer = db.Customers.Find(customer.CustomerId);
            targetCustomer = Mapper.Map<CustomerModel, Customer>(customer, targetCustomer);
            db.Entry(targetCustomer).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteCustomer(decimal id)
        {
            Customer customer = db.Customers.Find(id);
            db.Entry(customer).Collection(n => n.Notes).Load();
            customer.Notes.Clear();
            db.Customers.Remove(customer);
            db.SaveChanges();
        }

        public bool IsValidUser(LoginModel loginModel)
        {
            var users = from u in db.Users
                       where u.UserName == loginModel.UserName
                       && u.Password == loginModel.Password
                       select u;
            if (users.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreateUser(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
        }

        public void Dispose()
        {
            
        }

    }

}