using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRMApplication.Models
{
    public class NoteModel
    {
        [Required, DisplayName("Note")]
        public string NoteBody { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public System.DateTime CreatedDate { get; set; }

        public decimal NoteId { get; set; }
        
        public string CreatedBy { get; set; }
        public Nullable<decimal> CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}