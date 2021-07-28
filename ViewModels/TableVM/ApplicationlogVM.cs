using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace track_expense.api.ViewModels.TableVM
{
    [Table("applicationlog")]
    public class ApplicationlogVM
    {
        [Key]
        public long id { get; set; }
        public string type { get; set; }
        public string page { get; set; }
        public string function { get; set; }
        public string message { get; set; }
        public string stacktrace { get; set; }
        public long userid { get; set; }
        public DateTime logdate { get; set; }
    }
}
