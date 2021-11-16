using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WishList.Entities.Models;

namespace WishList.DataAccess.Interfaces.Repositories
{
    public interface IAccountRepository : IDisposable
    {
        Task Create(Account account);

        Task<Account> GetAsync(Guid id);

        Task<Account> GetAsync(string login);

        Task<List<Account>> ListAsync();

        Task<List<Account>> ListAsync(int page, int pageSize);

        Task UpdateProfileIdAsync(Guid profileId, Guid accountId);
    }
}
