namespace track_expense.api.ViewModels.ControllerVM
{
    public class SetupControllerVM
    {
    }

    public class AddNewCategoryVM
    {
        public string name { get; set; }
        public int type { get; set; }
        public string icon { get; set; } = "";
        public string description { get; set; } = "";
    }

    public class AddNewSubcategoryVM
    {
        public string name {  get; set; }
        public long categoryId { get; set; }
        public string icon { get; set; }
    }

    public class UpdateCategoryVM
    {
        public long id { get; set; }
        public string name { get; set; }
        public int type { get; set; }
        public string icon { get; set; } = "";
        public string description { get; set; } = "";
    }

    public class UpdateSubcategoryVM
    {
        public long id {  get; set; }
        public string name {  get; set; }
        public string icon { get; set; }
    }
}
