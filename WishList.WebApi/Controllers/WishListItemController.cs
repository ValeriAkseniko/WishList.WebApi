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
        public async Task Create([FromBody] WishListItemCreateRequest wishListItemCreateRequest)
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
        }

        [HttpGet]
        [Route("GetListItems")]
        public async Task<List<ListItem>> GetListItems(Guid wishListId)
        {
            List<ListItem> listItems = await wishListContext.ListItems.Where(x => x.WishListId == wishListId).ToListAsync();
            return listItems;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ListItem> Get(Guid id)
        {
            ListItem listitem = await wishListContext.ListItems.FirstOrDefaultAsync(x => x.Id == id);
            return listitem;
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task Delete(Guid itemId)
        {
            ListItem listItem = await Get(itemId);
            wishListContext.Entry(listItem).State = EntityState.Deleted;
            wishListContext.ListItems.Remove(listItem);
            await wishListContext.SaveChangesAsync();
        }
    }
}
