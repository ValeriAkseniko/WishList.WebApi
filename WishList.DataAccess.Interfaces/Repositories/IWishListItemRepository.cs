using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WishList.Entities.Models;

namespace WishList.DataAccess.Interfaces.Repositories
{
    public interface IWishListItemRepository : IDisposable
    {
        Task CreateAsync(List<ListItem> items);

        Task<ListItem> GetAsync(Guid id);

        Task UpdateAsync(ListItem newItem, Guid id);

        Task DeleteAsync(Guid id);

        Task<List<ListItem>> ListAsync();

        Task<List<ListItem>> ListAsync(Guid wishlistId);

        Task<List<ListItem>> ListAsync(Guid wishlistId, int page, int pageSize);

        Task<List<ListItem>> ListAsync(int page, int pageSize);

        Task<bool> WishListItemExistAsync(Guid id);
    }
}
