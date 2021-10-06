using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WishList.Entities;
using WishList.Entities.Models;

using WishListDb = WishList.Entities.Models.WishList;
namespace WishList.DataAccess
{
    public class WishListContext : DbContext
    {
        public WishListContext(DbContextOptions<WishListContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<ListItem> ListItems { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<WishListDb> WishLists { get; set; }
    }
}
