using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace track_expense.api.ViewModels.TableVM
{
    [Table("emaildata")]
    public class EmaildataVM
    {
        [Key]
        public long id {  get; set; }
        public string name {  get; set; }
        public string subject {  get; set; }
        public string body {  get; set; }
    }
}
