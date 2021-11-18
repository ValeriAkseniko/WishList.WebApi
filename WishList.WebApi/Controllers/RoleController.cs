using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WishList.BusinessLogic.Interfaces;
using WishList.DataTransferObjects.Role;

namespace WishList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IUserService userService;
        public RoleController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "Admin")]
        public async Task Create([FromBody] RoleCreateRequest roleCreateRequest)
        {
            await userService.CreateRoleAsync(roleCreateRequest);
        }

        [HttpGet]
        [Route("ListRoles")]
        [Authorize(Roles = "Admin")]
        public async Task<List<RoleView>> GetListRoles()
        {
            return await userService.ListRolesAsync();
        }

        [HttpGet]
        [Route("Get")]
        [Authorize(Roles = "Admin")]
        public async Task<RoleView> GetRole(Guid id)
        {
            return await userService.GetRoleAsync(id);
        }
    }
}
