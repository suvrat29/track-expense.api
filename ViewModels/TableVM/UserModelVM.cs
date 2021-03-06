using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace track_expense.api.ViewModels.TableVM
{
    [Table("logindata")]
    public class UserModelVM
    {
        [Key]
        public long id { get; set; }
        public string email { get; set; }
        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }
        public string avatar { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public bool invited { get; set; }
        public DateTime dateinvited { get; set; }
        public bool verified { get; set; }
        public DateTime dateverified { get; set; }
        public long createdby { get; set; }
        public DateTime datecreated { get; set; }
        public long modifiedby { get; set; }
        public DateTime datemodified { get; set; }
        public string resetkey { get; set; }
        public bool disabled { get; set; }
        public DateTime datedisabled { get; set; }
        public bool deleted { get; set; }
        public DateTime datedeleted { get; set; }
        public long? region { get; set; }
        public long? currency { get; set; }
    }
}
