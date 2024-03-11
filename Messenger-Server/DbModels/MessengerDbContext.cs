using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.DbModels
{
    public class MessengerDbContext : DbContext
    {
        public MessengerDbContext(DbContextOptions options) : base(options) { }
        public MessengerDbContext() : base() { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DbMessage>()
                .HasOne(m => m.FromClient)
                .WithMany(c => c.SentMessages)
                .HasForeignKey(m => m.FromClientId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DbMessage>()
                .HasOne(m => m.ToClient)
                .WithMany(c => c.ReceivedMessages)
                .HasForeignKey(m => m.ToClientId)
                .OnDelete(DeleteBehavior.Restrict);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            builder.UseSqlite("Data Source=D:\\Mein progectos\\Messenger-Server\\Messenger-Server\\MessageDb.db");
        }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<DbMessage> Messages { get; set; }
    }
}
