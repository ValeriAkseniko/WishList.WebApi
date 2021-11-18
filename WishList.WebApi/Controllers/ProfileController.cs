using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WishList.DataAccess.Interfaces.Repositories;
using WishList.DataTransferObjects.Profile;
using WishList.Entities.Models;

namespace WishList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileRepository profileRepository;
        private readonly IAccountRepository accountRepository;

        public ProfileController(IProfileRepository profileRepository,IAccountRepository accountRepository)
        {
            this.profileRepository = profileRepository;
            this.accountRepository = accountRepository;
        }

        [HttpGet]
        [Route("ListProfile")]
        [Authorize]
        public async Task<List<Profile>> GetListProfile()
        {
            return await profileRepository.ListAsync();
        }

        [HttpGet]
        [Route("Get")]
        [Authorize]
        public async Task<Profile> Get(Guid id)
        {
            return await profileRepository.GetAsync(id);
        }

        [HttpGet]
        [Route("GetByAccountId")]
        [Authorize]
        public async Task<Profile> GetByAccountId(Guid id)
        {
            return await profileRepository.GetByAccountIdAsync(id);
        }

        [HttpGet]
        [Route("GetMyProfile")]
        [Authorize()]

        public async Task<Profile> GetMyProfile()
        {
            var user = HttpContext.User.Identity.Name;
            var account = await accountRepository.GetAsync(user);
            var profile = await profileRepository.GetByAccountIdAsync(account.Id);
            return profile;
        }
    }
}
