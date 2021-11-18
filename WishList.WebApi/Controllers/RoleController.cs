using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WishList.BusinessLogic.Interfaces;
using WishList.BusinessLogicServices;
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
        private readonly IUserService userService;
        public RoleController(WishListContext wishListContext, IUserService userService)
        {
            this.wishListContext = wishListContext;
            this.userService = userService;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<List<RoleView>> GetListRoles()
        {
            return await userService.ListRoles();
        }

        [HttpGet]
        [Route("Get")]
        [Authorize(Roles = "admin")]
        public async Task<Role> GetRole(Guid id)
        {
            Role role = await wishListContext.Roles.FirstOrDefaultAsync(x => x.Id == id);
            return role;
        }
    }
}
