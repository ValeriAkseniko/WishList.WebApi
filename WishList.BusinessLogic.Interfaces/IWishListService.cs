using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WishList.DataTransferObjects.WishListItems;
using WishList.DataTransferObjects.WishLists;

namespace WishList.BusinessLogic.Interfaces
{
    public interface IWishListService
    {
        Task CreateWishListAsync(WishListCreateRequest wishListCreateRequest, Guid profileId);
        Task<List<WishListView>> GetWishListsAsync();
        Task<WishListView> GetWishListAsync(Guid id);
        Task<List<WishListView>> GetWishListByOwnerAsync(Guid ownerId);
        Task DeleteAsync(Guid id, Guid profileId, Guid roleId);
        Task CreateWishListItemAsync(WishListItemsCreateRequest wishListItemsCreateRequest, Guid profileId);
        Task<List<WishListItemView>> GetListItemsAsync(Guid wishListId, Guid profileId, Guid roleId);
        Task<WishListItemView> GetWishListItemAsync(Guid id, Guid profileId, Guid roleId);
        Task DeleteItemAsync(Guid id, Guid profileId, Guid roleId);
    }
}
