using System.Collections.Generic;
using System.Threading.Tasks;
using track_expense.api.ViewModels.ControllerVM;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.Services.Interfaces
{
    public interface ISetupService
    {
        Task<List<CategorydataVM>> getAllCategoriesWithSubcategoriesAsync(string username);
        Task<List<CategorydataVM>> getAllCategoriesNoSubcategoryAsync(string username);
        Task<List<SubcategorydataVM>> getAllSubcategoriesByCategoryAsync(long categoryId, string username);
        Task<bool> addNewCategoryAsync(AddNewCategoryVM newCategoryData, string username);
        Task<bool> updateCategoryDetailsAsync(UpdateCategoryVM categoryData, string username);
        Task<bool> deleteCategoryAsync(long categoryId, string username);
        Task<bool> addNewSubcategoryAsync(AddNewSubcategoryVM newSubcategoryData, string username);
        Task<bool> updateSubcategoryDetailsAsync(UpdateSubcategoryVM subcategoryData, string username);
        Task<bool> deleteSubcategoryAsync(long subcategoryId, string username);
    }
}
