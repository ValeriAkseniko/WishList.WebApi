using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WishList.DataTransferObjects.Accounts;
using WishList.DataTransferObjects.Profile;
using WishList.DataTransferObjects.Role;
using WishList.DataTransferObjects.Users;

namespace WishList.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task CreateAccountAsync(AccountCreateRequest accountCreateRequest);

        Task<List<UsersView>> GetUserListAsync();

        Task<UsersView> GetUserAsync();

        Task LoginAsync(string login, string password);

        Task LogoutAsync();

        Task UpdateMyProfileAsync(ProfileUpdateRequest profileUpdateRequest);

        Task<List<RoleView>> ListRolesAsync();
        Task CreateRoleAsync(RoleCreateRequest roleCreateRequest);
        Task<RoleView> GetRoleAsync(Guid id);
        Task<List<ProfileView>> GetListProfilesAsync();
        Task<ProfileView> GetProfileAsync(Guid id);
        Task<ProfileView> GetProfileByAccountIdAsync(Guid id);
        Task<ProfileView> GetMyProfileAsync();
    }
}
