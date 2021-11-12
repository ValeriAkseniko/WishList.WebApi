using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WishList.DataAccess;
using WishList.DataAccess.Interfaces.Repositories;
using WishList.DataAccess.Repositories;
using WishList.DataTransferObjects.WishListItems;
using WishList.DataTransferObjects.WishLists;
using WishList.Entities.Models;

namespace WishList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListItemController : ControllerBase
    {
        private readonly IWishListItemRepository wishListItemRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IProfileRepository profileRepository;
        private readonly IWishListRepositoty wishlistRepository;

        public WishListItemController(IWishListItemRepository wishListItemRepository, IWishListRepositoty wishListRepositoty, IAccountRepository accountRepository, IProfileRepository profileRepository)
        {
            this.wishListItemRepository = wishListItemRepository;
            this.accountRepository = accountRepository;
            this.profileRepository = profileRepository;
            this.wishlistRepository = wishListRepositoty;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize]
        public async Task<bool> Create([FromBody] List<WishListItemCreateRequest> wishListItemCreateRequest)
        {
            var user = HttpContext.User.Identity.Name;
            var account = await accountRepository.GetAsync(user);
            var profile = await profileRepository.GetAsyncByAccountId(account.Id);
            var wishlist = await wishlistRepository.GetAsync(wishListItemCreateRequest[0].WishListId);
            if (profile.Id == wishlist.OwnerId)
            {
                List<ListItem> listItems = new List<ListItem>();
                foreach (var item in wishListItemCreateRequest)
                {
                    ListItem createItem = new ListItem()
                    {
                        Id = Guid.NewGuid(),
                        Description = item.Description,
                        CreateDate = DateTime.Now,
                        WishListId = item.WishListId,
                        Name = item.Name,
                        Price = item.Price,
                        Reference = item.Reference,
                        Received = false
                    };
                    listItems.Add(createItem);
                }
                await wishListItemRepository.CreateAsync(listItems);
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpGet]
        [Route("GetListItems")]
        [Authorize]
        public async Task<List<WishListItemView>> GetListItems(Guid wishListId)
        {
            var user = HttpContext.User.Identity.Name;
            var account = await accountRepository.GetAsync(user);
            var wishlist = await wishlistRepository.GetAsync(wishListId);
            var profile = await profileRepository.GetAsyncByAccountId(account.Id);
            if (account.Role.Name == "Admin" || profile.Id == wishlist.OwnerId)
            {
                List<WishListItemView> listItemView = new List<WishListItemView>();
                foreach (var item in wishlist.ListItems)
                {
                    WishListItemView itemView = new WishListItemView
                    {
                        Description = item.Description,
                        Id = item.Id,
                        Name = item.Name,
                        Received = item.Received,
                        Reference = item.Reference
                    };
                    listItemView.Add(itemView);
                }
                return listItemView;
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        [Route("Get")]
        [AllowAnonymous]
        public async Task<WishListItemView> Get(Guid id)
        {
            var user = HttpContext.User.Identity.Name;
            var account = await accountRepository.GetAsync(user);
            var listItem = await wishListItemRepository.GetAsync(id);
            var wishlist = await wishlistRepository.GetAsync(listItem.WishListId);
            var profile = await profileRepository.GetAsyncByAccountId(account.Id);
            if (profile.Id == wishlist.OwnerId || account.Role.Name == "Admin")
            {
                WishListItemView itemView = new WishListItemView
                {
                    Description = listItem.Description,
                    Id = listItem.Id,
                    Name = listItem.Name,
                    Received = listItem.Received,
                    Reference = listItem.Reference
                };
                return itemView;
            }
            else
            {
                return null;
            }

        }

        [HttpDelete]
        [Route("Delete")]
        [Authorize]
        public async Task<bool> Delete(Guid id)
        {
            var user = HttpContext.User.Identity.Name;
            var account = await accountRepository.GetAsync(user);
            var item = await wishListItemRepository.GetAsync(id);
            var wishList = await wishlistRepository.GetAsync(item.WishListId);
            var profile = await profileRepository.GetAsyncByAccountId(account.Id);
            if (account.Role.Name == "Admin" || wishList.OwnerId == profile.Id)
            {
                await wishListItemRepository.DeleteAsync(id);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
