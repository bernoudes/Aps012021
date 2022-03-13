using APS_01_2021.Models;
using Microsoft.EntityFrameworkCore;

namespace APS_01_2021.Data
{
    public class MyDbContext : DbContext
    {
        public DbSet<UserModel> User { get; set; }
        public DbSet<InviteContactModel> InviteContact { get; set; }
        public DbSet<InviteMeetModel> InviteMeeting { get; set; }
        public DbSet<ContactModel> Contact { get; set; }
        public DbSet<ChatMessageModel> ChatMessage { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
