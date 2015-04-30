using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMApplication
{
    [MetadataType(typeof(User_Metadata))]
    public partial class User
    {
        private string _confirmPassword;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword
        {
            get
            { 
                return _confirmPassword; 
            }
            set 
            {
                _confirmPassword = value; 
            }
        }
    }

    public class User_Metadata
    {
        [Required, DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required, DisplayName("Last Name")]
        public string LastName { get; set; }
        
        [Required, DisplayName("Email")]
        [RegularExpression(RegularExpressions.Email, ErrorMessage = ValidationConstants.InvalidEmailAddress)]
        public string EmailAddress { get; set; }

        [Required, DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DisplayName("User Name")]
        public string UserName { get; set; }
    }
}
