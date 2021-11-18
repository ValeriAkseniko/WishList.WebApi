using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WishList.BusinessLogic.Interfaces;
using WishList.DataTransferObjects.WishListItems;

namespace WishList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListItemController : ControllerBase
    {
        private readonly IWishListService wishListService;
        private readonly IUserService userService;

        public WishListItemController(IWishListService wishListService, IUserService userService)
        {
            this.wishListService = wishListService;
            this.userService = userService;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize]
        public async Task Create([FromBody] WishListItemsCreateRequest wishListItemsCreateRequest)
        {
            var user = await userService.GetUserAsync();
            await wishListService.CreateWishListItemAsync(wishListItemsCreateRequest, user.ProfileId);
        }

        [HttpGet]
        [Route("GetListItems")]
        [Authorize]
        public async Task<List<WishListItemView>> GetListItems(Guid wishListId)
        {
            var user = await userService.GetUserAsync();
            return await wishListService.GetListItemsAsync(wishListId, user.ProfileId, user.RoleId);
        }

        [HttpGet]
        [Route("Get")]
        [AllowAnonymous]
        public async Task<WishListItemView> Get(Guid id)
        {
            var user = await userService.GetUserAsync();
            return await wishListService.GetWishListItemAsync(id, user.ProfileId, user.RoleId);
        }

        [HttpDelete]
        [Route("Delete")]
        [Authorize]
        public async Task Delete(Guid id)
        {
            var user = await userService.GetUserAsync();
            await wishListService.DeleteItemAsync(id, user.ProfileId, user.RoleId);
        }
    }
}
