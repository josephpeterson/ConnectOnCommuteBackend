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
        public virtual DbSet<AccountConnection> TblConnection { get; set; }
        public virtual DbSet<AccountNotification> TblNotification { get; set; }


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
            modelBuilder.Entity<AccountConnection>(entity =>
            {
                entity.ToTable("coctblConnection");

            });
            modelBuilder.Entity<AccountNotification>(entity =>
            {
                entity.ToTable("coctblNotifications");

            });
        }
    }
}
