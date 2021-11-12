using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishList.DataAccess.Interfaces.Repositories;
using WishList.Entities.Models;

namespace WishList.DataAccess.Repositories
{
    public class WishListItemRepository : IWishListItemRepository
    {
        private readonly WishListContext wishListContext;

        public WishListItemRepository(WishListContext wishListContext)
        {
            this.wishListContext = wishListContext;
        }
        public void Dispose()
        {
            wishListContext.Dispose();
        }

        public async Task CreateAsync(List<ListItem> items)
        {
            foreach (var item in items)
            {
                await wishListContext.ListItems.AddAsync(item);
                await wishListContext.SaveChangesAsync();
            }
        }

        public async Task<ListItem> GetAsync(Guid id)
        {
            return await wishListContext.ListItems
                .Include(x => x.WishList)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(ListItem newItem, Guid id)
        {
            var entity = await GetAsync(id);
            entity.Description = newItem.Description;
            entity.Name = newItem.Name;
            entity.Price = newItem.Price;
            entity.Received = newItem.Received;
            entity.Reference = newItem.Reference;
            entity.WishList = newItem.WishList;
            entity.CreateDate = newItem.CreateDate;
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

        public async Task<List<ListItem>> ListAsync()
        {
            return await wishListContext.ListItems.ToListAsync();
        }

        public async Task<List<ListItem>> ListAsync(Guid wishlistId)
        {
            return await wishListContext.ListItems
                .Where(x => x.WishListId == wishlistId)
                .ToListAsync();
        }

        public async Task<List<ListItem>> ListAsync(Guid wishlistId, int page, int pageSize)
        {
            return await wishListContext.ListItems
                .Where(x => x.WishListId == wishlistId)
                .OrderBy(x => x.Name)
                .Skip(page * pageSize).Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<ListItem>> LiastAsync(int page, int pageSize)
        {
            return await wishListContext.ListItems
                .OrderBy(x => x.Name)
                .Skip(page * pageSize).Take(pageSize)
                .ToListAsync();
        }
    }
}
