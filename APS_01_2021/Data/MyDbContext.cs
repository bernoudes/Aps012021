﻿using APS_01_2021.Models;
using Microsoft.EntityFrameworkCore;

namespace APS_01_2021.Data
{
    public class MyDbContext : DbContext
    {
        public DbSet<UserModel> User { get; set; }
        public DbSet<InviteListContactModel> InviteListContact { get; set; }
        public DbSet<InviteListMeetingModel> InviteListMeeting { get; set; }
        public DbSet<ContactModel> Contact { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
