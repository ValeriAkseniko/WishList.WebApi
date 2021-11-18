using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WishList.Entities.Models;

namespace WishList.DataAccess.Interfaces.Repositories
{
    public interface IProfileRepository
    {
        Task CreateAsync(Profile profile);

        Task<Profile> GetAsync(Guid id);

        Task<Profile> GetByAccountIdAsync(Guid accountId);

        Task<List<Profile>> ListAsync();

        Task<List<Profile>> ListAsync(int page, int pageSize);

        Task UpdateAsync(Profile profile);

    }
}
