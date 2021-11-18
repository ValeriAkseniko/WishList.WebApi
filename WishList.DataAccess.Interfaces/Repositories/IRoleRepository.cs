using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WishList.Entities.Models;

namespace WishList.DataAccess.Interfaces.Repositories
{
    public interface IRoleRepository : IDisposable
    {
        Task<List<Role>> ListAsync();

        Task<Role> GetAsync(Guid id);
        Task CreateAsync(Role role);
    }
}
