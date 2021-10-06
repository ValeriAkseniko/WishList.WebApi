﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishList.DataAccess.Interfaces.Repositories;
using WishList.Entities.Models;

namespace WishList.DataAccess.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly WishListContext wishListContext;
        public RoleRepository(WishListContext wishListContext)
        {
            this.wishListContext = wishListContext;
        }
        public void Dispose()
        {
            wishListContext.Dispose();
        }

        public async Task<Role> GetAsync(Guid id)
        {
            Role result = await wishListContext.Roles.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<List<Role>> ListAsync()
        {
            List<Role> ListRoles = await wishListContext.Roles.ToListAsync();
            return ListRoles;
        }
    }
}