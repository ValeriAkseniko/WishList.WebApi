using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WishList.DataAccess.Interfaces.Repositories;
using WishList.Entities.Models;

namespace WishList.DataAccess.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly WishListContext wishListContext;
        public AccountRepository(WishListContext wishListContext)
        {
            this.wishListContext = wishListContext;
        }

        public void Dispose()
        {
            wishListContext.Dispose();
        }

        public async Task CreateAsync(Account account)
        {
            await wishListContext.Accounts.AddAsync(account);
            await wishListContext.SaveChangesAsync();
        }

        public async Task<Account> GetAsync(Guid id)
        {
            return await wishListContext.Accounts
                .Include(x => x.Role)
                .Include(x => x.Profile)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Account> GetAsync(string login)
        {
            return await wishListContext.Accounts
                .Include(x => x.Role)
                .Include(x => x.Profile)
                .FirstOrDefaultAsync(x => x.Login == login);
        }

        public async Task<List<Account>> ListAsync()
        {
            return await wishListContext.Accounts
                .Include(x => x.Role)
                .Include(x => x.Profile)
                .ToListAsync();
        }

        public async Task<List<Account>> ListAsync(int page, int pageSize)
        {
            return await wishListContext.Accounts
                .Include(x => x.Role)
                .Include(x => x.Profile)
                .OrderBy(x => x.Login)
                .Skip(page * pageSize).Take(pageSize)
                .ToListAsync();
        }

        public async Task UpdateProfileIdAsync(Guid profileId, Guid accountId)
        {
            var entity = await GetAsync(accountId);
            entity.ProfileId = profileId;
            wishListContext.Entry(entity).State = EntityState.Modified;
            await wishListContext.SaveChangesAsync();
        }

        public async Task<bool> LoginExistAsync(Account user)
        {
            return await wishListContext.Accounts
                .FirstOrDefaultAsync(x => x.Login.ToLower() == user.Login.ToLower()) != null;
        }

        public async Task<bool> EmailExistAsync(Account user)
        {
            return await wishListContext.Accounts
                .FirstOrDefaultAsync(x => x.Email.ToLower() == user.Email.ToLower()) != null;
        }

    }
}
