using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WishList.BusinessLogic.Interfaces;
using WishList.DataTransferObjects.WishLists;

namespace WishList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IWishListService wishListService;

        public WishListController(IWishListService wishListService, IUserService userService)
        {
            this.wishListService = wishListService;
            this.userService = userService;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize]
        public async Task Create(WishListCreateRequest wishListCreateRequest)
        {
            var user = await userService.GetUserAsync();
            await wishListService.CreateWishListAsync(wishListCreateRequest, user.ProfileId);
        }

        [HttpGet]
        [Route("GetList")]
        [Authorize(Roles = "Admin")]
        public async Task<List<WishListView>> GetListWishlist()
        {
            return await wishListService.GetWishListsAsync();
        }

        [HttpGet]
        [Route("Get")]
        [AllowAnonymous]
        public async Task<WishListView> GetWishList(Guid id)
        {
            return await wishListService.GetWishListAsync(id);
        }

        [HttpGet]
        [Route("GetByOwner")]
        [Authorize(Roles = "Admin")]
        public async Task<List<WishListView>> GetByOwner(Guid ownerId)
        {
            return await wishListService.GetWishListByOwnerAsync(ownerId);
        }
        [HttpGet]
        [Route("GetMyWishlists")]
        [Authorize]
        public async Task<List<WishListView>> GetMyWishlists()
        {
            var user = await userService.GetUserAsync();
            return await wishListService.GetWishListByOwnerAsync(user.ProfileId);
        }

        [HttpDelete]
        [Route("Delete")]
        [Authorize]
        public async Task Delete(Guid id)
        {
            var user = await userService.GetUserAsync();
            await wishListService.DeleteAsync(id, user.ProfileId, user.RoleId);
        }
    }
}
