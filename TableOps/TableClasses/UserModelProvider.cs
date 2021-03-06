using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using track_expense.api.DatabaseAccess;
using track_expense.api.TableOps.Interfaces;
using track_expense.api.ViewModels.TableVM;
using track_expense.api.ViewModels.ControllerVM;
using track_expense.api.Extensions;

namespace track_expense.api.TableOps.TableClasses
{
    public class UserModelProvider : IUserModelProvider
    {
        #region Variables
        private readonly PostgreSQLContext _dbcontext;
        #endregion

        #region Constructor
        public UserModelProvider(PostgreSQLContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        #endregion

        #region CUD Operations
        public void CreateUserAccount(UserModelVM userModel)
        {
            _dbcontext.user.Add(userModel);
            _dbcontext.SaveChanges();

            UserModelVM _user = _dbcontext.user.FromSqlRaw($"SELECT * FROM logindata WHERE email = '{userModel.email}'").OrderBy(x => x.id).LastOrDefault();

            if (_user == null)
            {
                throw new ApiErrorResponse("Failed to create user account");
            }
            else
            {
                _user.createdby = _user.id;

                _dbcontext.user.Update(_user);
                _dbcontext.SaveChanges();
            }
        }

        public async Task CreateUserAccountAsync(UserModelVM userModel)
        {
            await _dbcontext.user.AddAsync(userModel);
            await _dbcontext.SaveChangesAsync();

            UserModelVM _user = await _dbcontext.user.FromSqlRaw($"SELECT * FROM logindata WHERE email = '{userModel.email}'").OrderBy(x => x.id).LastOrDefaultAsync();

            if (_user == null)
            {
                throw new ApiErrorResponse("Failed to create user account");
            }
            else
            {
                _user.createdby = _user.id;

                _dbcontext.user.Update(_user);
                await _dbcontext.SaveChangesAsync();
            }
        }

        public void UpdateUserDetails(UserModelVM userModel)
        {
            userModel.modifiedby = userModel.id;
            userModel.datemodified = DateTime.Now.ToUniversalTime();

            _dbcontext.user.Update(userModel);
            _dbcontext.SaveChanges();
        }

        public async Task UpdateUserDetailsAsync(UserModelVM userModel)
        {
            userModel.modifiedby = userModel.id;
            userModel.datemodified = DateTime.Now.ToUniversalTime();

            _dbcontext.user.Update(userModel);
            await _dbcontext.SaveChangesAsync();
        }

        public void DisableUserAccount(long userId)
        {
            UserModelVM _user = _dbcontext.user.Find(userId);

            if (_user != null)
            {
                _user.disabled = true;
                _user.datedisabled = DateTime.Now.ToUniversalTime();
                _user.modifiedby = userId;
                _user.datemodified = DateTime.Now.ToUniversalTime();

                _dbcontext.user.Update(_user);
                _dbcontext.SaveChanges();
            }
            else
            {
                throw new ApiErrorResponse($"No record found for userId: {userId}");
            }
        }

        public async Task DisableUserAccountAsync(long userId)
        {
            UserModelVM _user = await _dbcontext.user.FindAsync(userId);

            if (_user != null)
            {
                _user.disabled = true;
                _user.datedisabled = DateTime.Now.ToUniversalTime();
                _user.modifiedby = userId;
                _user.datemodified = DateTime.Now.ToUniversalTime();

                _dbcontext.user.Update(_user);
                await _dbcontext.SaveChangesAsync();
            }
            else
            {
                throw new ApiErrorResponse($"No record found for userId: {userId}");
            }
        }

        public void DeleteUserAccount(long userId)
        {
            UserModelVM _user = _dbcontext.user.Find(userId);

            if (_user != null)
            {
                _user.deleted = true;
                _user.datedeleted = DateTime.Now.ToUniversalTime();
                _user.modifiedby = userId;
                _user.datemodified = DateTime.Now.ToUniversalTime();

                _dbcontext.user.Update(_user);
                _dbcontext.SaveChanges();
            }
            else
            {
                throw new ApiErrorResponse($"No record found for userId: {userId}");
            }
        }

        public async Task DeleteUserAccountAsync(long userId)
        {
            UserModelVM _user = await _dbcontext.user.FindAsync(userId);

            if (_user != null)
            {
                _user.deleted = true;
                _user.datedeleted = DateTime.Now.ToUniversalTime();
                _user.modifiedby = userId;
                _user.datemodified = DateTime.Now.ToUniversalTime();

                _dbcontext.user.Update(_user);
                await _dbcontext.SaveChangesAsync();
            }
            else
            {
                throw new ApiErrorResponse($"No record found for userId: {userId}");
            }
        }

        public bool UpdateUserProfile(UserProfileUpdateVM userProfileData, long userId)
        {
            UserModelVM userData = GetUserAccountById(userId);

            userData.firstname = userProfileData.firstname;
            userData.lastname = userProfileData.lastname;
            userData.avatar = userProfileData.avatar;
            userData.region = userProfileData.region;
            userData.currency = userProfileData.currency;
            userData.modifiedby = userId;
            userData.datemodified = DateTime.Now.ToUniversalTime();

            _dbcontext.user.Update(userData);
            _dbcontext.SaveChanges();

            return true;
        }

        public async Task<bool> UpdateUserProfileAsync(UserProfileUpdateVM userProfileData, long userId)
        {
            UserModelVM userData = await GetUserAccountByIdAsync(userId).ConfigureAwait(false);

            if (userData != null)
            {
                userData.firstname = userProfileData.firstname;
                userData.lastname = userProfileData.lastname;
                userData.avatar = userProfileData.avatar;
                userData.region = userProfileData.region;
                userData.currency = userProfileData.currency;
                userData.modifiedby = userId;
                userData.datemodified = DateTime.Now.ToUniversalTime();

                _dbcontext.user.Update(userData);
                await _dbcontext.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Fetch Operations
        public UserModelVM GetUserAccountById(long userId)
        {
            return _dbcontext.user.Find(userId);
        }
        
        public async Task<UserModelVM> GetUserAccountByIdAsync(long userId)
        {
            return await _dbcontext.user.FindAsync(userId);
        }

        public async Task<bool> ResetKeyExistsAsync(string resetKey)
        {
            UserModelVM _user = await _dbcontext.user.FromSqlRaw($"SELECT * FROM logindata WHERE resetkey = '{resetKey}'").OrderBy(x => x.id).LastOrDefaultAsync();

            if (_user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> UserAlreadyExistsAsync(string email)
        {
            UserModelVM _user = await _dbcontext.user.FromSqlRaw($"SELECT * FROM logindata WHERE email = '{email}'").OrderBy(x => x.id).LastOrDefaultAsync();

            if (_user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public UserModelVM GetUserAccountByEmail(string email)
        {
            return _dbcontext.user.FromSqlRaw($"SELECT * FROM logindata WHERE email = '{email}'").OrderBy(x => x.id).LastOrDefault();
        }

        public async Task<UserModelVM> GetUserAccountByEmailAsync(string email)
        {
            return await _dbcontext.user.FromSqlRaw($"SELECT * FROM logindata WHERE email = '{email}'").OrderBy(x => x.id).LastOrDefaultAsync();
        }
        #endregion
    }
}
