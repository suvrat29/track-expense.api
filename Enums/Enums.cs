namespace track_expense.api.Enums
{
    public static class Enums
    {
    }

    public static class CategoryDataEnum
    {
        public static readonly int TYPE_EXPENSE = 0;
        public static readonly int TYPE_INCOME = 1;
    }

    public enum ActivityTypeEnum
    {
        Profile = 0,
        Category = 1,
        Subcategory = 2,
        Income = 3,
        Expense = 4
    }

    public enum ActivityActionTypeEnum
    {
        Create = 0,
        Delete = 1,
        Modify = 2
    }
}
