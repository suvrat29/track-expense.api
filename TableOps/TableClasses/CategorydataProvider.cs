using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                category.name = string.IsNullOrWhiteSpace(categoryData.name) ? throw new ApplicationException("Category name cannot be blank") : categoryData.name;
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
            CategorydataVM category = await GetCategoryByIdAsync(categoryData.id, userId).ConfigureAwait(false);

            if (category != null)
            {
                category.name = string.IsNullOrWhiteSpace(categoryData.name) ? throw new ApplicationException("Category name cannot be blank") : categoryData.name;
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
            CategorydataVM category = await GetCategoryByIdAsync(categoryId, userId).ConfigureAwait(false);

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
            CategorydataVM category = await GetCategoryByIdAsync(categoryId, userId).ConfigureAwait(false);

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
            return _dbcontext.categorydata.FromSqlRaw(GetCategoryListSQL(userId, categoryId)).OrderBy(x => x.id).LastOrDefault();
        }

        public async Task<CategorydataVM> GetCategoryByIdAsync(long categoryId, long userId)
        {
            return await _dbcontext.categorydata.FromSqlRaw(GetCategoryListSQL(userId, categoryId)).OrderBy(x => x.id).LastOrDefaultAsync();
        }

        public List<CategorydataVM> GetCategoryList(long userId, bool withSubcategoryCount)
        {
            ConcurrentBag<CategorydataVM> listData = new ConcurrentBag<CategorydataVM>();
            List<CategorydataVM> categories = new List<CategorydataVM>();
            ConcurrentBag<long> categoryBag = new ConcurrentBag<long>();

            categories = _dbcontext.categorydata.FromSqlRaw(GetCategoryListSQL(userId, 0)).ToList();

            if (withSubcategoryCount)
            {
                Parallel.ForEach(categories, category => { categoryBag.Add(category.id); });

                List<SubcategoryCountVM> subcategoryCountList = _dbcontext.getDataFromQueryAsList<SubcategoryCountVM>(GetSubcategoryCountSQL(categoryBag.ToList()));

                if (subcategoryCountList != null && subcategoryCountList.Count > 0)
                {
                    Parallel.ForEach(categories, category =>
                    {
                        SubcategoryCountVM subCategoryData = subcategoryCountList.Where(x => x.categoryId == category.id).FirstOrDefault();

                        if (subCategoryData != null)
                        {
                            category.subcategorycount = subCategoryData.subcategoryCount;
                        }

                        listData.Add(category);
                    });
                }

                return listData.ToList();
            }
            else
            {
                return categories;
            }
        }

        public async Task<List<CategorydataVM>> GetCategoryListAsync(long userId, bool withSubcategoryCount)
        {
            ConcurrentBag<CategorydataVM> listData = new ConcurrentBag<CategorydataVM>();
            List<CategorydataVM> categories = new List<CategorydataVM>();
            ConcurrentBag<long> categoryBag = new ConcurrentBag<long>();

            categories = await _dbcontext.categorydata.FromSqlRaw(GetCategoryListSQL(userId, 0)).ToListAsync();

            if (withSubcategoryCount)
            {
                Parallel.ForEach(categories, category => { categoryBag.Add(category.id); });

                List<SubcategoryCountVM> subcategoryCountList = await _dbcontext.getDataFromQueryAsListAsync<SubcategoryCountVM>(GetSubcategoryCountSQL(categoryBag.ToList()));

                if (subcategoryCountList != null && subcategoryCountList.Count > 0)
                {
                    Parallel.ForEach(categories, category =>
                    {
                        SubcategoryCountVM subCategoryData = subcategoryCountList.Where(x => x.categoryId == category.id).FirstOrDefault();

                        if (subCategoryData != null)
                        {
                            category.subcategorycount = subCategoryData.subcategoryCount;
                        }

                        listData.Add(category);
                    });
                }

                return listData.ToList();
            }
            else
            {
                return categories;
            }
        }
        #endregion

        #region SQL Queries
        private string GetCategoryListSQL(long userId, long categoryId)
        {
            StringBuilder _sql = new StringBuilder();
            _sql.Append("SELECT ");
            _sql.Append("id, ");
            _sql.Append("name, ");
            _sql.Append("type, ");
            _sql.Append("COALESCE(icon, '') icon, ");
            _sql.Append("COALESCE(description, '') description, ");
            _sql.Append("COALESCE(inactive, 'false') inactive, ");
            _sql.Append("COALESCE(deleted, 'false') deleted, ");
            _sql.Append("createdby, ");
            _sql.Append("datecreated, ");
            _sql.Append("COALESCE(modifiedby, 0) modifiedby, ");
            _sql.Append("datemodified ");
            _sql.Append("FROM categorydata ");
            _sql.Append($"WHERE createdby = {userId} ");
            _sql.Append("AND COALESCE(inactive, 'false') <> 'true' ");
            _sql.Append("AND COALESCE(deleted, 'false') <> 'true' ");
            if (categoryId > 0)
            {
                _sql.Append($"AND id = {categoryId}");
            }

            return _sql.ToString();
        }

        private string GetSubcategoryCountSQL(List<long> categoryIds)
        {
            StringBuilder _sql = new StringBuilder();
            string listStr = "";
            _sql.Append("SELECT COUNT(id) subcategoryCount, ");
            _sql.Append("categoryid categoryId ");
            _sql.Append("FROM subcategorydata ");
            _sql.Append("WHERE categoryid IN ( ");
            foreach (long id in categoryIds)
            {
                listStr += id.ToString() + ", ";
            }
            _sql.Append($"{listStr.Substring(0, listStr.Length - 2)}");
            _sql.Append(") ");
            _sql.Append("GROUP BY (categoryid)");

            return _sql.ToString();
        }

        #endregion
    }
}
