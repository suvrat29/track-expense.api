using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using track_expense.api.DatabaseAccess;
using track_expense.api.Enums;
using track_expense.api.Extensions;
using track_expense.api.TableOps.Interfaces;
using track_expense.api.ViewModels.ControllerVM;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.TableOps.TableClasses
{
    public class CategorydataProvider : ICategorydataProvider
    {
        #region Variables
        private readonly PostgreSQLContext _dbcontext;
        #endregion

        #region Constructor
        public CategorydataProvider(PostgreSQLContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        #endregion

        #region CUD Operations
        public void CreateNewCategory(CategorydataVM categoryDataModel)
        {
            _dbcontext.categorydata.Add(categoryDataModel);
            _dbcontext.SaveChanges();
        }

        public async Task CreateNewCategoryAsync(CategorydataVM categoryDataModel)
        {
            await _dbcontext.categorydata.AddAsync(categoryDataModel);
            await _dbcontext.SaveChangesAsync();
        }

        public bool UpdateCategoryDetails(UpdateCategoryVM categoryData, long userId)
        {
            CategorydataVM category = GetCategoryById(categoryData.id, userId);

            if (category != null)
            {
                category.name = categoryData.name;
                category.type = categoryData.type == 0 ? CategoryDataEnum.TYPE_EXPENSE : categoryData.type == 1 ? CategoryDataEnum.TYPE_INCOME : throw new ApiErrorResponse("Unrecognized expense type");
                category.icon = categoryData.icon;
                category.description = categoryData.description;
                category.modifiedby = userId;
                category.datemodified = DateTime.Now.ToUniversalTime();

                _dbcontext.categorydata.Update(category);
                _dbcontext.SaveChanges();

                return true;
            }
            else
            {
                throw new ApiErrorResponse($"No record found for categoryId: {categoryData.id} for this user");
            }
        }

        public async Task<bool> UpdateCategoryDetailsAsync(UpdateCategoryVM categoryData, long userId)
        {
            CategorydataVM category = await GetCategoryByIdAsync(categoryData.id, userId);

            if (category != null)
            {
                category.name = categoryData.name;
                category.type = categoryData.type == 0 ? CategoryDataEnum.TYPE_EXPENSE : categoryData.type == 1 ? CategoryDataEnum.TYPE_INCOME : throw new ApiErrorResponse("Unrecognized expense type");
                category.icon = categoryData.icon;
                category.description = categoryData.description;
                category.modifiedby = userId;
                category.datemodified = DateTime.Now.ToUniversalTime();

                _dbcontext.categorydata.Update(category);
                await _dbcontext.SaveChangesAsync();

                return true;
            }
            else
            {
                throw new ApiErrorResponse($"No record found for categoryId: {categoryData.id} for this user");
            }
        }

        public void InactiveCategory(long categoryId, long userId)
        {
            CategorydataVM category = GetCategoryById(categoryId, userId);

            if (category != null)
            {
                category.inactive = true;
                category.modifiedby = userId;
                category.datemodified = DateTime.Now.ToUniversalTime();

                _dbcontext.categorydata.Update(category);
                _dbcontext.SaveChanges();
            }
            else
            {
                throw new ApiErrorResponse($"No record found for categoryId: {categoryId} for this user");
            }
        }

        public async Task InactiveCategoryAsync(long categoryId, long userId)
        {
            CategorydataVM category = await GetCategoryByIdAsync(categoryId, userId);

            if (category != null)
            {
                category.inactive = true;
                category.modifiedby = userId;
                category.datemodified = DateTime.Now.ToUniversalTime();

                _dbcontext.categorydata.Update(category);
                await _dbcontext.SaveChangesAsync();
            }
            else
            {
                throw new ApiErrorResponse($"No record found for categoryId: {categoryId} for this user");
            }
        }

        public void DeleteCategory(long categoryId, long userId)
        {
            CategorydataVM category = GetCategoryById(categoryId, userId);

            if (category != null)
            {
                category.deleted = true;
                category.modifiedby = userId;
                category.datemodified = DateTime.Now.ToUniversalTime();

                _dbcontext.categorydata.Update(category);
                _dbcontext.SaveChanges();
            }
            else
            {
                throw new ApiErrorResponse($"No record found for categoryId: {categoryId} for this user");
            }
        }

        public async Task DeleteCategoryAsync(long categoryId, long userId)
        {
            CategorydataVM category = await GetCategoryByIdAsync(categoryId, userId);

            if (category != null)
            {
                category.deleted = true;
                category.modifiedby = userId;
                category.datemodified = DateTime.Now.ToUniversalTime();

                _dbcontext.categorydata.Update(category);
                await _dbcontext.SaveChangesAsync();
            }
            else
            {
                throw new ApiErrorResponse($"No record found for categoryId: {categoryId} for this user");
            }
        }
        #endregion

        #region Fetch Operations
        public CategorydataVM GetCategoryById(long categoryId, long userId)
        {
            return _dbcontext.categorydata.FromSqlRaw($"SELECT * FROM categorydata WHERE createdby = {userId} AND COALESCE(inactive, 'false') <> 'true' AND COALESCE(deleted, 'false') <> 'true' AND id = {categoryId}").OrderBy(x => x.id).LastOrDefault();
        }

        public async Task<CategorydataVM> GetCategoryByIdAsync(long categoryId, long userId)
        {
            return await _dbcontext.categorydata.FromSqlRaw($"SELECT * FROM categorydata WHERE createdby = {userId} AND COALESCE(inactive, 'false') <> 'true' AND COALESCE(deleted, 'false') <> 'true' AND id = {categoryId}").OrderBy(x => x.id).LastOrDefaultAsync();
        }

        public List<CategorydataVM> GetCategoryList(long userId)
        {
            return _dbcontext.categorydata.FromSqlRaw($"SELECT * FROM categorydata WHERE createdby = {userId} AND COALESCE(inactive, 'false') <> 'true' AND COALESCE(deleted, 'false') <> 'true'").ToList();
        }

        public async Task<List<CategorydataVM>> GetCategoryListAsync(long userId)
        {
            return await _dbcontext.categorydata.FromSqlRaw($"SELECT * FROM categorydata WHERE createdby = {userId} AND COALESCE(inactive, 'false') <> 'true' AND COALESCE(deleted, 'false') <> 'true'").ToListAsync();
        }
        #endregion
    }
}
