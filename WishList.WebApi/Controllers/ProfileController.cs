using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WishList.DataAccess;
using WishList.DataTransferObjects.Profile;
using WishList.Entities.Models;

namespace WishList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly WishListContext wishListContext;

        public ProfileController(WishListContext wishListContext)
        {
            this.wishListContext = wishListContext;
        }

        [HttpPost]
        [Route("ProfileCreate")]
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
            await wishListContext.Profiles.AddAsync(profile);
            await wishListContext.SaveChangesAsync();
        }

        [HttpGet]
        [Route("ListProfile")]
        public async Task<List<Profile>> GetListProfile()
        {
            List<Profile> profiles = await wishListContext.Profiles.ToListAsync();
            return profiles;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<Profile> Get(Guid id)
        {
            Profile profile = await wishListContext.Profiles.Include(x=>x.Account).FirstOrDefaultAsync(x => x.Id == id);
            return profile;
        }

        [HttpGet]
        [Route("GetByAccountId")]
        public async Task<Profile> GetByAccountId(Guid id)
        {
            Profile profile = await wishListContext.Profiles.FirstOrDefaultAsync(x => x.AccountId == id);
            return profile;
        }
    }
}
