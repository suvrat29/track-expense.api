using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using track_expense.api.DatabaseAccess;
using track_expense.api.Extensions;
using track_expense.api.TableOps.Interfaces;
using track_expense.api.ViewModels.ControllerVM;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.TableOps.TableClasses
{
    public class SubcategorydataProvider : ISubcategorydataProvider
    {
        #region Variables
        private readonly PostgreSQLContext _dbcontext;
        #endregion

        #region Constructor
        public SubcategorydataProvider(PostgreSQLContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        #endregion

        #region CUD Operations
        public void CreateNewSubcategory(SubcategorydataVM subcategoryDataModel)
        {
            _dbcontext.subcategorydata.Add(subcategoryDataModel);
            _dbcontext.SaveChanges();
        }

        public async Task CreateNewSubcategoryAsync(SubcategorydataVM subcategoryDataModel)
        {
            await _dbcontext.subcategorydata.AddAsync(subcategoryDataModel);
            await _dbcontext.SaveChangesAsync();
        }

        public bool UpdateSubcategoryDetails(UpdateSubcategoryVM subcategoryData, long userId)
        {
            SubcategorydataVM subcategory = GetSubcategoryById(subcategoryData.id, userId);

            if (subcategory != null)
            {
                subcategory.name = string.IsNullOrWhiteSpace(subcategoryData.name) ? throw new ApplicationException("Subcategory name cannot be empty") : subcategoryData.name;
                subcategory.icon = subcategoryData.icon;
                subcategory.modifiedby = userId;
                subcategory.datemodified = DateTime.Now.ToUniversalTime();

                _dbcontext.subcategorydata.Update(subcategory);
                _dbcontext.SaveChanges();

                return true;
            }
            else
            {
                throw new ApiErrorResponse($"No record found for subcategoryId: {subcategoryData.id} for this user");
            }
        }

        public async Task<bool> UpdateSubcategoryDetailsAsync(UpdateSubcategoryVM subcategoryData, long userId)
        {
            SubcategorydataVM subcategory = await GetSubcategoryByIdAsync(subcategoryData.id, userId).ConfigureAwait(false);

            if (subcategory != null)
            {
                subcategory.name = string.IsNullOrWhiteSpace(subcategoryData.name) ? throw new ApplicationException("Subcategory name cannot be empty") : subcategoryData.name;
                subcategory.icon = subcategoryData.icon;
                subcategory.modifiedby = userId;
                subcategory.datemodified = DateTime.Now.ToUniversalTime();

                _dbcontext.subcategorydata.Update(subcategory);
                await _dbcontext.SaveChangesAsync();

                return true;
            }
            else
            {
                throw new ApiErrorResponse($"No record found for subcategoryId: {subcategoryData.id} for this user");
            }
        }

        public void InactiveSubcategory(long subcategoryId, long userId)
        {
            SubcategorydataVM subcategory = GetSubcategoryById(subcategoryId, userId);

            if (subcategory != null)
            {
                subcategory.inactive = true;
                subcategory.modifiedby = userId;
                subcategory.datemodified = DateTime.Now.ToUniversalTime();

                _dbcontext.subcategorydata.Update(subcategory);
                _dbcontext.SaveChanges();
            }
            else
            {
                throw new ApiErrorResponse($"No record found for subcategoryId: {subcategoryId} for this user");
            }
        }

        public async Task InactiveSubcategoryAsync(long subcategoryId, long userId)
        {
            SubcategorydataVM subcategory = await GetSubcategoryByIdAsync(subcategoryId, userId).ConfigureAwait(false);

            if (subcategory != null)
            {
                subcategory.inactive = true;
                subcategory.modifiedby = userId;
                subcategory.datemodified = DateTime.Now.ToUniversalTime();

                _dbcontext.subcategorydata.Update(subcategory);
                await _dbcontext.SaveChangesAsync();
            }
            else
            {
                throw new ApiErrorResponse($"No record found for subcategoryId: {subcategoryId} for this user");
            }
        }

        public void DeleteSubcategory(long subcategoryId, long userId)
        {
            SubcategorydataVM subcategory = GetSubcategoryById(subcategoryId, userId);

            if (subcategory != null)
            {
                subcategory.deleted = true;
                subcategory.modifiedby = userId;
                subcategory.datemodified = DateTime.Now.ToUniversalTime();

                _dbcontext.subcategorydata.Update(subcategory);
                _dbcontext.SaveChanges();
            }
            else
            {
                throw new ApiErrorResponse($"No record found for subcategoryId: {subcategoryId} for this user");
            }
        }

        public async Task DeleteSubcategoryAsync(long subcategoryId, long userId)
        {
            SubcategorydataVM subcategory = await GetSubcategoryByIdAsync(subcategoryId, userId).ConfigureAwait(false);

            if (subcategory != null)
            {
                subcategory.deleted = true;
                subcategory.modifiedby = userId;
                subcategory.datemodified = DateTime.Now.ToUniversalTime();

                _dbcontext.subcategorydata.Update(subcategory);
                await _dbcontext.SaveChangesAsync();
            }
            else
            {
                throw new ApiErrorResponse($"No record found for subcategoryId: {subcategoryId} for this user");
            }
        }
        #endregion

        #region Fetch Operations
        public SubcategorydataVM GetSubcategoryById(long subcategoryId, long userId)
        {
            return _dbcontext.subcategorydata.FromSqlRaw(GetSubcategoryByIdSQL(userId, subcategoryId)).OrderBy(x => x.id).LastOrDefault();
        }

        public async Task<SubcategorydataVM> GetSubcategoryByIdAsync(long subcategoryId, long userId)
        {
            return await _dbcontext.subcategorydata.FromSqlRaw(GetSubcategoryByIdSQL(userId, subcategoryId)).OrderBy(x => x.id).LastOrDefaultAsync();
        }

        public List<SubcategorydataVM> GetSubcategoryList(long categoryId, long userId)
        {
            return _dbcontext.subcategorydata.FromSqlRaw(GetSubcategoryListSQL(userId, categoryId)).ToList();
        }

        public async Task<List<SubcategorydataVM>> GetSubcategoryListAsync(long categoryId, long userId)
        {
            return await _dbcontext.subcategorydata.FromSqlRaw(GetSubcategoryListSQL(userId, categoryId)).ToListAsync();
        }
        #endregion

        #region SQL Queries
        private string GetSubcategoryByIdSQL(long userId, long subcategoryId)
        {
            StringBuilder _sql = new StringBuilder();
            _sql.Append(GetSubcategorySQL(userId));
            _sql.Append(GetSubcategoryByIdWhereSQL(subcategoryId));

            return _sql.ToString();
        }

        private string GetSubcategoryListSQL(long userId, long categoryId)
        {
            StringBuilder _sql = new StringBuilder();
            _sql.Append(GetSubcategorySQL(userId));
            _sql.Append(GetSubcategoryListWhereSQL(categoryId));

            return _sql.ToString();
        }

        private string GetSubcategorySQL(long userId)
        {
            StringBuilder _sql = new StringBuilder();
            _sql.Append("SELECT ");
            _sql.Append("id, ");
            _sql.Append("categoryid, ");
            _sql.Append("name, ");
            _sql.Append("COALESCE(icon, '') icon, ");
            _sql.Append("COALESCE(inactive, 'false') inactive, ");
            _sql.Append("COALESCE(deleted, 'false') deleted, ");
            _sql.Append("createdby, ");
            _sql.Append("datecreated, ");
            _sql.Append("COALESCE(modifiedby, 0) modifiedby, ");
            _sql.Append("datemodified ");
            _sql.Append("FROM subcategorydata ");
            _sql.Append($"WHERE createdby = {userId} ");
            _sql.Append("AND COALESCE(inactive, 'false') <> 'true' ");
            _sql.Append("AND COALESCE(deleted, 'false') <> 'true' ");

            return _sql.ToString();
        }

        private string GetSubcategoryByIdWhereSQL(long subcategoryId)
        {
            return $"AND id = {subcategoryId} ";
        }

        private string GetSubcategoryListWhereSQL(long categoryId)
        {
            return $"AND categoryid = {categoryId} ";
        }
        #endregion
    }
}
