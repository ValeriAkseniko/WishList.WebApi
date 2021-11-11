using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishList.DataAccess.Interfaces.Repositories;
using WishList.Entities.Models;

using WishListDb = WishList.Entities.Models.WishList;

namespace WishList.DataAccess.Repositories
{
    public class WishlistRepository : IWishListRepositoty
    {
        private readonly WishListContext wishListContext;

        public WishlistRepository(WishListContext wishListContext)
        {
            this.wishListContext = wishListContext;
        }

        public void Dispose()
        {
            wishListContext.Dispose();
        }

        public async Task CreateAsync(WishListDb wishList)
        {
            await wishListContext.AddAsync(wishList);
            await wishListContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(WishListDb wishList, Guid id)
        {
            var entity = await GetAsync(id);
            entity.CreateDate = wishList.CreateDate;
            entity.Description = wishList.Description;
            entity.Name = wishList.Name;
            entity.OwnerId = wishList.OwnerId;
            entity.ListItems = wishList.ListItems;
            wishListContext.Entry(entity).State = EntityState.Modified;
            await wishListContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetAsync(id);
            wishListContext.Entry(entity).State = EntityState.Deleted;
            wishListContext.Remove(entity);
            await wishListContext.SaveChangesAsync();
        }

        public async Task<WishListDb> GetAsync(Guid id)
        {
            return await wishListContext.WishLists
                .Include(x => x.Owner)
                .Include(x => x.ListItems)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<WishListDb>> ListAsync()
        {
            return await wishListContext.WishLists
                .Include(x => x.Owner)
                .Include(x => x.ListItems)
                .ToListAsync();
        }

        public async Task<List<WishListDb>> ListAsync(Guid profileId)
        {
            return await wishListContext.WishLists
                .Where(x => x.OwnerId == profileId)
                .ToListAsync();
        }

        public async Task<List<WishListDb>> ListAsync(Guid profileId, int page, int pageSize)
        {
            return await wishListContext.WishLists
                .Where(x => x.OwnerId == profileId)
                .OrderBy(x => x.Name)
                .Skip(page * pageSize).Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<WishListDb>> ListAsync(int page, int pageSize)
        {
            return await wishListContext.WishLists
                .OrderBy(x => x.Name)
                .Skip(page * pageSize).Take(pageSize)
                .ToListAsync();
        }
    }
}
