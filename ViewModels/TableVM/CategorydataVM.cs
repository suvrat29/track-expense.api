using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace track_expense.api.ViewModels.TableVM
{
    [Table("categorydata")]
    public class CategorydataVM
    {
        [Key]
        public long id { get; set; }
        public string name { get; set; }
        public int type { get; set; }
        public string icon { get; set; }
        public string description { get; set; }
        public bool inactive { get; set; } = false;
        public bool deleted { get; set; } = false;
        public long createdby { get; set; }
        public DateTime datecreated { get; set; }
        public long modifiedby { get; set; }
        public DateTime datemodified { get; set; }
    }
}
