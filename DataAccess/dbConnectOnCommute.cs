using System;
namespace ConnectOnCommuteBackend.DataAccess
{
    using ConnectOnCommuteBackend.Models;
    using Microsoft.EntityFrameworkCore;

    public class DbConnectOnCommute : DbContext
    {

        public DbConnectOnCommute(DbContextOptions<DbConnectOnCommute> options) : base(options)
        {
            //if (Database.IsSqlServer()) Database.SetCommandTimeout(280);
        }

        public virtual DbSet<Account> TblAccount { get; set; }
        public virtual DbSet<UserPosition> TblPosition { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("coctblAccount");
            });
            modelBuilder.Entity<UserPosition>(entity =>
            {
                entity.ToTable("coctblPosition");
            
            });
        }
    }
}
