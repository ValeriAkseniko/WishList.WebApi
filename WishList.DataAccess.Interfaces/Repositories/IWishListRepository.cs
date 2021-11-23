using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WishListDb = WishList.Entities.Models.WishList;

namespace WishList.DataAccess.Interfaces.Repositories
{
    public interface IWishListRepository : IDisposable
    {
        Task CreateAsync(WishListDb wishList);

        Task UpdateAsync(WishListDb wishList, Guid id);

        Task DeleteAsync(Guid id);

        Task<WishListDb> GetAsync(Guid id);

        Task<List<WishListDb>> ListAsync();

        Task<List<WishListDb>> ListAsync(Guid profileId);

        Task<List<WishListDb>> ListAsync(Guid profileId, int page, int pageSize);

        Task<List<WishListDb>> ListAsync(int page, int pageSize);

        Task<bool> WishListExistAsync(Guid wishListId);
    }
}
