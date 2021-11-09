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
using WishList.DataTransferObjects.Profile;
using WishList.Entities.Models;

namespace WishList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileRepository profileRepository;

        public ProfileController(IProfileRepository profileRepository)
        {
            this.profileRepository = profileRepository;
        }

        [HttpPost]
        [Route("ProfileCreate")]
        [Authorize]
        public async Task Create([FromBody] ProfileCreateRequest profileCreateRequest)
        {
            Profile profile = new Profile()
            {
                AccountId = profileCreateRequest.AccountId,
                Birthday = profileCreateRequest.Birthday,
                Id = Guid.NewGuid(),
                Gender = (Gender)profileCreateRequest.Gender,
                Nickname = profileCreateRequest.Nickname
            };
            await profileRepository.Create(profile);
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
            return await profileRepository.GetAsyncByAccountId(id);
        }
    }
}
