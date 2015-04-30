using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMApplication
{
    [MetadataType(typeof(Customer_Metadata))]
    public partial class Customer
    {
        [Display(Name="Customer Name")]
        public string FullName 
        { 
            get 
            { 
                return this.LastName.Trim() + ", " + this.FirstName.Trim();
            }
        }
    }

    public class Customer_Metadata
    {

        public decimal CustomerId { get; set; }

        [Required, DisplayName("First Name")]
        [RegularExpression(RegularExpressions.Name, ErrorMessage = ValidationConstants.InvalidFirstName)]
        public string FirstName { get; set; }

        [Required, DisplayName("Last Name")]
        [RegularExpression(RegularExpressions.Name, ErrorMessage = ValidationConstants.InvalidLastName)]
        public string LastName { get; set; }

        [DisplayName("Company")]
        public string CompanyName { get; set; }

        [Required, DisplayName("Email")]
        [RegularExpression(RegularExpressions.Email, ErrorMessage = ValidationConstants.InvalidEmailAddress)]
        public string Email { get; set; }


        public string City { get; set; }

        public string State { get; set; }

        [DisplayName("Street Address 1")]
        public string StreetAddress1 { get; set; }

        [DisplayName("Street Address 2")]
        public string StreetAddress2 { get; set; }

        public string Zip { get; set; }

        public string County { get; set; }

        public string Country { get; set; }

        [Required]
        [DisplayName("Phone Number")]
        [RegularExpression(RegularExpressions.PhoneNumber, ErrorMessage = ValidationConstants.InvalidPhoneMessage)]
        public string PhoneNumber { get; set; }
    }
}
