using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WishList.DataAccess.Interfaces.Repositories;
using WishList.DataTransferObjects.WishListItems;
using WishList.DataTransferObjects.WishLists;

using WishListDb = WishList.Entities.Models.WishList;

namespace WishList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListRepositoty wishListRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IProfileRepository profileRepository;

        public WishListController(IWishListRepositoty wishListRepositoty, IAccountRepository accountRepository, IProfileRepository profileRepository)
        {
            this.wishListRepository = wishListRepositoty;
            this.accountRepository = accountRepository;
            this.profileRepository = profileRepository;
        }


        [HttpPost]
        [Route("Create")]
        [Authorize]
        public async Task Create(WishListCreateRequest wishListCreateRequest)
        {
            var user = HttpContext.User.Identity.Name;
            var account = await accountRepository.GetAsync(user);
            var profile = await profileRepository.GetAsyncByAccountId(account.Id);

            WishListDb wishList = new WishListDb()
            {
                CreateDate = DateTime.Now,
                Description = wishListCreateRequest.Description,
                Id = Guid.NewGuid(),
                Name = wishListCreateRequest.Name,
                OwnerId = profile.Id
            };
            await wishListRepository.CreateAsync(wishList);
        }

        [HttpGet]
        [Route("GetList")]
        [Authorize(Roles = "Admin")]
        public async Task<List<WishListView>> GetListWishlist()
        {
            var list = await wishListRepository.ListAsync();
            List<WishListView> listView = new List<WishListView>();
            for (int i = 0; i < list.Count; i++)
            {
                var item = await GetWishList(list[i].Id);
                listView.Add(item);
            }
            return listView;
        }

        [HttpGet]
        [Route("Get")]
        [AllowAnonymous]
        public async Task<WishListView> GetWishList(Guid id)
        {
            var item = await wishListRepository.GetAsync(id);
            WishListView newItem = new WishListView()
            {
                Description = item.Description,
                Id = item.Id,
                ListItems = item.ListItems.Select(x => new WishListItemView
                {
                    Description = x.Description,
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Received = x.Received,
                    Reference = x.Reference
                }).ToList(),
                Name = item.Name
            };
            return newItem;
        }

        [HttpGet]
        [Route("GetByOwner")]
        [Authorize(Roles = "Admin")]
        public async Task<List<WishListDb>> GetByOwner(Guid ownerId)
        {
            return await wishListRepository.ListAsync(ownerId);
        }
        [HttpGet]
        [Route("GetMyWishlists")]
        [Authorize]
        public async Task<List<WishListView>> GetMyWishlists()
        {
            var user = HttpContext.User.Identity.Name;
            var account = await accountRepository.GetAsync(user);
            var profile = await profileRepository.GetAsyncByAccountId(account.Id);
            var listWishlist = await wishListRepository.ListAsync(profile.Id);
            List<WishListView> result = new List<WishListView>();
            foreach (var item in listWishlist)
            {
                WishListView newItem = new WishListView()
                {
                    Description = item.Description,
                    Id = item.Id,
                    ListItems = item.ListItems.Select(x => new WishListItemView
                    {
                        Description = x.Description,
                        Id = x.Id,
                        Name = x.Name,
                        Price = x.Price,
                        Received = x.Received,
                        Reference = x.Reference
                    }).ToList(),
                    Name = item.Name
                };
                result.Add(newItem);
            }
            return result;
        }

        [HttpDelete]
        [Route("Delete")]
        [Authorize]
        public async Task<bool> Delete(Guid id)
        {
            var user = HttpContext.User.Identity.Name;
            var account = await accountRepository.GetAsync(user);
            var item = await wishListRepository.GetAsync(id);
            if (account.Role.Name == "Admin" || item.OwnerId == account.ProfileId)
            {
                await wishListRepository.DeleteAsync(id);
                return true;
            }
            else
            {
                return false;
            }
        }        
    }
}
