using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WishList.DataAccess;
using WishList.DataTransferObjects.WishListItems;
using WishList.Entities.Models;

namespace WishList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListItemController : ControllerBase
    {
        private readonly WishListContext wishListContext;
        public WishListItemController(WishListContext wishListContext)
        {
            this.wishListContext = wishListContext;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize]
        public async Task<bool> Create([FromBody] WishListItemCreateRequest wishListItemCreateRequest)
        {
            var user = HttpContext.User.Identity.Name;
            var account = await wishListContext.Accounts.FirstOrDefaultAsync(x => x.Login == user);
            var wishlist = await wishListContext.WishLists.FirstOrDefaultAsync(x => x.Id == wishListItemCreateRequest.WishListId);
            if (account.ProfileId == wishlist.OwnerId)
            {
                ListItem listItem = new ListItem()
                {
                    Id = Guid.NewGuid(),
                    Description = wishListItemCreateRequest.Description,
                    CreateDate = DateTime.Now,
                    WishListId = wishListItemCreateRequest.WishListId,
                    Name = wishListItemCreateRequest.Name,
                    Price = wishListItemCreateRequest.Price,
                    Reference = wishListItemCreateRequest.Reference,
                    Received = false
                };
                await wishListContext.ListItems.AddAsync(listItem);
                await wishListContext.SaveChangesAsync();
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
        public async Task<List<ListItem>> GetListItems(Guid wishListId)
        {
            var user = HttpContext.User.Identity.Name;
            var account = await wishListContext.Accounts
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Login == user);
            var wishlist = await wishListContext.WishLists.FirstOrDefaultAsync(x => x.Id == wishListId);
            if (account.Role.Name == "Admin" || account.ProfileId == wishlist.OwnerId)
            {
                List<ListItem> listItems = await wishListContext.ListItems.Where(x => x.WishListId == wishListId).ToListAsync();
                return listItems;
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        [Route("Get")]
        [AllowAnonymous]
        public async Task<ListItem> Get(Guid id)
        {
            var user = HttpContext.User.Identity.Name;
            var account = await wishListContext.Accounts
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Login == user);
            var wishlist = await wishListContext.ListItems
                .Include(x => x.WishListId)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (account.ProfileId == wishlist.WishList.OwnerId || account.Role.Name == "Admin")
            {
                ListItem listitem = await wishListContext.ListItems.FirstOrDefaultAsync(x => x.Id == id);
                return listitem;
            }
            else
            {
                return null;
            }

        }

        [HttpDelete]
        [Route("Delete")]
        [Authorize]
        public async Task<bool> Delete(Guid itemId)
        {
            var user = HttpContext.User.Identity.Name;
            var account = await wishListContext.Accounts
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Login == user);
            var item = await wishListContext.ListItems.Include(x=>x.WishList).FirstOrDefaultAsync(x => x.Id == itemId);
            if (account.Role.Name == "Admin" || item.WishList.OwnerId == account.ProfileId)
            {
                ListItem listItem = await Get(itemId);
                wishListContext.Entry(listItem).State = EntityState.Deleted;
                wishListContext.ListItems.Remove(listItem);
                await wishListContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
