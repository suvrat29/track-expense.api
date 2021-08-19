using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace track_expense.api.ViewModels.TableVM
{
    [Table("subcategorydata")]
    public class SubcategorydataVM
    {
        [Key]
        public long id { get; set; }
        public long categoryid { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public bool inactive { get; set; }
        public bool deleted { get; set; }
        public long createdby { get; set; }
        public DateTime datecreated { get; set; }
        public long modifiedby { get; set; }
        public DateTime datemodified { get; set; }
    }
}
