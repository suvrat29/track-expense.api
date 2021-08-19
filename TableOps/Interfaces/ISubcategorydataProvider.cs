using System.Collections.Generic;
using System.Threading.Tasks;
using track_expense.api.ViewModels.ControllerVM;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.TableOps.Interfaces
{
    public interface ISubcategorydataProvider
    {
        void CreateNewSubcategory(SubcategorydataVM subcategoryDataModel);
        Task CreateNewSubcategoryAsync(SubcategorydataVM subcategoryDataModel);
        bool UpdateSubcategoryDetails(UpdateSubcategoryVM subcategoryData, long userId);
        Task<bool> UpdateSubcategoryDetailsAsync(UpdateSubcategoryVM subcategoryData, long userId);
        void InactiveSubcategory(long subcategoryId, long userId);             //unused
        Task InactiveSubcategoryAsync(long subcategoryId, long userId);        //unused
        void DeleteSubcategory(long subcategoryId, long userId);
        Task DeleteSubcategoryAsync(long subcategoryId, long userId);
        List<SubcategorydataVM> GetSubcategoryList(long categoryId, long userId);
        Task<List<SubcategorydataVM>> GetSubcategoryListAsync(long categoryId, long userId);
        SubcategorydataVM GetSubcategoryById(long subcategoryId, long userId);
        Task<SubcategorydataVM> GetSubcategoryByIdAsync(long subcategoryId, long userId);
    }
}
