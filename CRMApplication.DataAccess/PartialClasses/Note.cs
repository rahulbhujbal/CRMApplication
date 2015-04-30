using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMApplication
{
    [MetadataType(typeof(Note_Metadata))]
    public partial class Note
    {
       
    }

    public class Note_Metadata
    {
        [Required, DisplayName("Note")]
        public string NoteBody { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public System.DateTime CreatedDate { get; set; }
    }
}
