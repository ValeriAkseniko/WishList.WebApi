﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WishList.DataAccess;
using WishList.DataTransferObjects.WishListItems;
using WishList.DataTransferObjects.WishLists;

using WishListDb = WishList.Entities.Models.WishList;

namespace WishList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly WishListContext wishListContext;

        public WishListController(WishListContext wishListContext)
        {
            this.wishListContext = wishListContext;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize]
        public async Task Create(WishListCreateRequest wishListCreateRequest)
        {
            var user = HttpContext.User.Identity.Name;
            var accountId = await wishListContext.Accounts.Where(x => x.Login == user).Select(x => x.Id).FirstOrDefaultAsync();
            var profileId = await wishListContext.Profiles.Where(x => x.AccountId == accountId).Select(x => x.Id).FirstOrDefaultAsync();

            WishListDb wishList = new WishListDb()
            {
                CreateDate = DateTime.Now,
                Description = wishListCreateRequest.Description,
                Id = Guid.NewGuid(),
                Name = wishListCreateRequest.Name,
                OwnerId = profileId
            };
            await wishListContext.WishLists.AddAsync(wishList);
            await wishListContext.SaveChangesAsync();
        }

        [HttpGet]
        [Route("GetList")]
        [Authorize]
        public async Task<List<WishListDb>> GetListWishlist()
        {
            List<WishListDb> list = await wishListContext.WishLists.ToListAsync();
            return list;
        }

        [HttpGet]
        [Route("Get")]
        [AllowAnonymous]
        public async Task<WishListView> GetWishList(Guid id)
        {
            WishListDb item = await wishListContext.WishLists.Include(x => x.ListItems).FirstOrDefaultAsync(x => x.Id == id);
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
        [Authorize]
        public async Task<List<WishListDb>> GetByOwner(Guid ownerId)
        {
            List<WishListDb> list = await wishListContext.WishLists.Where(x => x.OwnerId == ownerId).ToListAsync();
            return list;
        }

        [HttpDelete]
        [Route("Delete")]
        [Authorize]
        public async Task Delete(Guid id)
        {
            WishListDb item = await wishListContext.WishLists.FirstOrDefaultAsync(x => x.Id == id);
            wishListContext.Entry(item).State = EntityState.Deleted;
            wishListContext.WishLists.Remove(item);
            await wishListContext.SaveChangesAsync();
        }
    }
}
