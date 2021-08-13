using Microsoft.EntityFrameworkCore;
using track_expense.api.ViewModels.TableVM;

namespace track_expense.api.DatabaseAccess
{
    public class PostgreSQLContext : DbContext
    {
        #region TableProperties
        public DbSet<UserModelVM> user {get; set;}
        public DbSet<ApplicationlogVM> applog { get; set; }
        public DbSet<LocalesVM> locale { get; set; }
        public DbSet<EmaildataVM> emaildata { get; set; }
        #endregion

        #region Constructor
        public PostgreSQLContext(DbContextOptions<PostgreSQLContext> options) : base(options)
        {
        }
        #endregion

        #region Methods

        #region Protected Methods
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        #endregion

        #region Public Methods
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }
        #endregion

        #endregion
    }
}
