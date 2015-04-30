using CRMApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMApplication.Repositories
{
    public interface IDataRepository
    {
        List<CustomerModel> GetAllCustomers();
        
        CustomerModel GetCustomer(decimal id);

        CustomerModel GetCustomerWithNotes(decimal id);

        void CreateCustomer(CustomerModel customer);
        
        Note GetNoteForCustomer(decimal id);
        
        void CreateNoteForCustomer(Note note, string createdBy);

        Customer GetCustomerObject(decimal id);

        void UpdateCustomer(ref CustomerModel customer);
        
        void DeleteCustomer(decimal id);

        bool IsValidUser(LoginModel loginModel);

        void CreateUser(User user);

        void Dispose();
        
    }
}
