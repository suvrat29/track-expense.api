using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace track_expense.api.ViewModels.TableVM
{
    [Table("useractivitylog")]
    public class UseractivitylogVM
    {
        [Key]
        public long id { get; set; }
        public int activitytype { get; set; }
        public int actiontype { get; set; }
        public long userid { get; set; }
        public DateTime actiondate { get; set; }
        public string actionfield1 { get; set; }
        public string actionfield2 { get; set; }
    }
}
