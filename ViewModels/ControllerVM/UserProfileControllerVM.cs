using System.Collections.Generic;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.ViewModels.ControllerVM
{
    public class UserProfileControllerVM
    {
    }

    public class UserProfileDataVM
    {
        public long id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string avatar { get; set; } = "";
        public long region { get; set; }
        public long currency { get; set; }
        public List<LocaleRegionList> regionlist { get; set; }
        public List<LocaleCurrencyList> currencylist { get; set; }
    }
}
