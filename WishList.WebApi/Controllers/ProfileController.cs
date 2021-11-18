using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WishList.BusinessLogic.Interfaces;
using WishList.DataTransferObjects.Profile;

namespace WishList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService userService;

        public ProfileController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Route("ListProfile")]
        [Authorize]
        public async Task<List<ProfileView>> GetListProfiles()
        {
            return await userService.GetListProfilesAsync();
        }

        [HttpGet]
        [Route("Get")]
        [Authorize]
        public async Task<ProfileView> Get(Guid id)
        {
            return await userService.GetProfileAsync(id);
        }

        [HttpGet]
        [Route("GetByAccountId")]
        [Authorize]
        public async Task<ProfileView> GetByAccountId(Guid id)
        {
            return await userService.GetProfileByAccountIdAsync(id);
        }

        [HttpGet]
        [Route("GetMyProfile")]
        [Authorize()]

        public async Task<ProfileView> GetMyProfile()
        {
            return await userService.GetMyProfileAsync();
        }
    }
}
