using System.Collections.Generic;
using System.Threading.Tasks;
using track_expense.api.ViewModels.ControllerVM;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.TableOps.Interfaces
{
    public interface ICategorydataProvider
    {
        void CreateNewCategory(CategorydataVM categoryDataModel);
        Task CreateNewCategoryAsync(CategorydataVM categoryDataModel);
        bool UpdateCategoryDetails(UpdateCategoryVM categoryData, long userId);
        Task<bool> UpdateCategoryDetailsAsync(UpdateCategoryVM categoryData, long userId);
        void InactiveCategory(long categoryId, long userId);        //unused
        Task InactiveCategoryAsync(long categoryId, long userId);   //unused
        void DeleteCategory(long categoryId, long userId);
        Task DeleteCategoryAsync(long categoryId, long userId);
        List<CategorydataVM> GetCategoryList(long userId, bool withSubcategoryCount);
        Task<List<CategorydataVM>> GetCategoryListAsync(long userId, bool withSubcategoryCount);
        CategorydataVM GetCategoryById(long categoryId, long userId);
        Task<CategorydataVM> GetCategoryByIdAsync(long categoryId, long userId);
    }
}
