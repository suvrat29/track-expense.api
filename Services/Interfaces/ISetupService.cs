using System.Collections.Generic;
using System.Threading.Tasks;
using track_expense.api.ViewModels.ControllerVM;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.Services.Interfaces
{
    public interface ISetupService
    {
        Task<List<CategorydataVM>> getAllCategoriesAsync(string username);
        Task<bool> addNewCategoryAsync(AddNewCategoryVM newCategoryData, string username);
        Task<bool> updateCategoryDetailsAsync(UpdateCategoryVM categoryData, string username);
        Task<bool> deleteCategoryAsync(long categoryId, string username);
    }
}
