using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WishList.DataAccess;
using WishList.DataTransferObjects.Role;
using WishList.Entities.Models;

namespace WishList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly WishListContext wishListContext;
        public RoleController(WishListContext wishListContext)
        {
            this.wishListContext = wishListContext;
        }

        [HttpPost]
        [Route("Create")]
        public async Task Create([FromBody] RoleCreateRequest roleCreateRequest)
        {
            Role role = new Role()
            {
                Id = Guid.NewGuid(),
                Name = roleCreateRequest.Name,
                Description = roleCreateRequest.Description
            };
            await wishListContext.Roles.AddAsync(role);
            await wishListContext.SaveChangesAsync();

        }

        [HttpGet]
        [Route("ListRoles")]
        public async Task<List<Role>> GetListRoles()
        {
            List<Role> roles = await wishListContext.Roles.ToListAsync();
            return roles;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<Role> GetRole(Guid id)
        {
            Role role = await wishListContext.Roles.FirstOrDefaultAsync(x => x.Id == id);
            return role;
        }
    }
}
