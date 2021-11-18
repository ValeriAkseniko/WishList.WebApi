using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WishList.DataTransferObjects.Accounts;
using WishList.DataTransferObjects.Profile;
using WishList.DataTransferObjects.Role;
using WishList.DataTransferObjects.Users;

namespace WishList.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task CreateAccount(AccountCreateRequest accountCreateRequest);

        Task<List<UsersView>> GetUserList();

        Task<UsersView> GetUser();

        Task Login(string login, string password);

        Task Logout();

        Task UpdateProfile(ProfileUpdateRequest profileUpdateRequest);

        Task<List<RoleView>> ListRoles();
    }
}
