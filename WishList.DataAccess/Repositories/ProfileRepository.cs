using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WishList.DataAccess.Interfaces.Repositories;
using WishList.Entities.Models;

namespace WishList.DataAccess.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly WishListContext wishListContext;
        public ProfileRepository(WishListContext wishListContext)
        {
            this.wishListContext = wishListContext;
        }
        public void Dispose()
        {
            wishListContext.Dispose();
        }

        public async Task CreateAsync(Profile profile)
        {
            await wishListContext.AddAsync(profile);
            await wishListContext.SaveChangesAsync();
        }

        public async Task<Profile> GetAsync(Guid id)
        {
            return await wishListContext.Profiles.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Profile> GetByAccountIdAsync(Guid accountId)
        {
            return await wishListContext.Profiles.FirstOrDefaultAsync(x => x.AccountId == accountId);
        }

        public async Task<List<Profile>> ListAsync()
        {
            return await wishListContext.Profiles
                .Include(x => x.Account)
                .ToListAsync();
        }

        public async Task<List<Profile>> ListAsync(int page, int pageSize)
        {
            return await wishListContext.Profiles
                .Include(x => x.Account)
                .OrderBy(x => x.Nickname)
                .Skip(page * pageSize).Take(pageSize)
                .ToListAsync();
        }

        public async Task UpdateAsync(Profile profile)
        {
            var entity = await GetAsync(profile.Id);
            entity.AccountId = profile.AccountId;
            entity.Birthday = profile.Birthday;
            entity.Gender = profile.Gender;
            entity.Nickname = profile.Nickname;
            wishListContext.Entry(entity).State = EntityState.Modified;
            await wishListContext.SaveChangesAsync();
        }

        public async Task<bool> ProfileExistAsync(Guid profileId)
        {
            return await wishListContext.Profiles
                .FirstOrDefaultAsync(x => x.Id == profileId) != null;
        }
    }
}
