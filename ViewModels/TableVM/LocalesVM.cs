using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace track_expense.api.ViewModels.TableVM
{
    [Table("locales")]
    public class LocalesVM
    {
        [Key]
        public long id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string callprefix { get; set; }
        public string currency { get; set; }
        public string currencycode { get; set; }
    }

    public class LocaleRegionList
    {
        public long id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
    }

    public class LocaleCurrencyList
    {
        public long id { get; set; }
        public string currency { get; set; }
        public string currencycode { get; set; }
    }
}
