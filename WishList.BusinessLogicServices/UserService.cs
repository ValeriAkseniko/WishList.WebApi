using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WishList.BusinessLogic.Interfaces;
using WishList.DataAccess.Interfaces.Repositories;
using WishList.DataTransferObjects.Accounts;
using WishList.DataTransferObjects.Profile;
using WishList.DataTransferObjects.Role;
using WishList.DataTransferObjects.Users;
using WishList.Entities.Models;
using WishList.Infrastructure.Constants;

namespace WishList.BusinessLogicServices
{
    public class UserService : IUserService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IProfileRepository profileRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IRoleRepository roleRepository;

        public UserService(IAccountRepository accountRepository, IProfileRepository profileRepository, IHttpContextAccessor httpContextAccessor, IRoleRepository roleRepository)
        {
            this.accountRepository = accountRepository;
            this.profileRepository = profileRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.roleRepository = roleRepository;
        }

        public async Task CreateAccountAsync(AccountCreateRequest accountCreateRequest)
        {
            var account = new Account()
            {
                CreateDate = DateTime.Now,
                Email = accountCreateRequest.Email.ToLower(),
                HashPassword = GetHash(accountCreateRequest.Password),
                Id = Guid.NewGuid(),
                Login = accountCreateRequest.Login.ToLower(),
                RoleId = Permissions.Id.DefaultUser
            };
            await accountRepository.CreateAsync(account);
            var profile = new Profile()
            {
                AccountId = account.Id,
                Id = Guid.NewGuid()
            };
            if (await accountRepository.LoginExistAsync(account))
            {

            }
            if (await accountRepository.EmailExistAsync(account))
            {

            }
            await profileRepository.CreateAsync(profile);
            await accountRepository.UpdateProfileIdAsync(profile.Id, account.Id);
        }

        public async Task<List<UsersView>> GetUserListAsync()
        {
            var entities = await accountRepository.ListAsync();
            return entities
                .Select(x => new UsersView
                {
                    Login = x.Login,
                    Email = x.Email,
                    Gender = x.Profile.Gender.ToString(),
                    ProfileId = x.ProfileId.Value,
                    RoleId = x.RoleId,
                    RoleName = x.Role.Name,
                    Nickname = x.Profile.Nickname
                })
                .ToList();
        }

        public async Task<UsersView> GetUserAsync()
        {
            var user = httpContextAccessor.HttpContext.User.Identity.Name;
            var entity = await accountRepository.GetAsync(user);
            return new UsersView()
            {
                Login = entity.Login,
                Email = entity.Email,
                Gender = entity.Profile.Gender.ToString(),
                RoleId = entity.RoleId,
                ProfileId = entity.ProfileId.Value,
                RoleName = entity.Role.Name,
                Nickname = entity.Profile.Nickname
            };
        }


        public async Task LoginAsync(string login, string password)
        {
            var account = await accountRepository.GetAsync(login);
            if (account.HashPassword == GetHash(password) && account != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, account.Role.Name)
                };
                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            }
        }
        public async Task LogoutAsync()
        {
            await httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }


        public async Task UpdateMyProfileAsync(ProfileUpdateRequest profileUpdateRequest)
        {
            var user = httpContextAccessor.HttpContext.User.Identity.Name;
            var account = await accountRepository.GetAsync(user);
            var profile = await profileRepository.GetByAccountIdAsync(account.Id);

            profile.Nickname = profileUpdateRequest.Nickname;
            profile.Gender = (Gender)profileUpdateRequest.Gender;
            profile.Birthday = profileUpdateRequest.Birthday;

            await profileRepository.UpdateAsync(profile);
        }

        public async Task<List<RoleView>> ListRolesAsync()
        {
            var entities = await roleRepository.ListAsync();
            List<RoleView> rolesView = new List<RoleView>();
            return entities
                .Select(x => new RoleView
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToList();
        }

        private string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hash);
        }

        public async Task CreateRoleAsync(RoleCreateRequest roleCreateRequest)
        {
            Role role = new Role()
            {
                Id = Guid.NewGuid(),
                Name = roleCreateRequest.Name,
                Description = roleCreateRequest.Description
            };
            await roleRepository.CreateAsync(role);
        }

        public async Task<RoleView> GetRoleAsync(Guid id)
        {
            var role = await roleRepository.GetAsync(id);
            return new RoleView
            {
                Description = role.Description,
                Id = role.Id,
                Name = role.Name
            };
        }

        public async Task<List<ProfileView>> GetListProfilesAsync()
        {
            var entities = await profileRepository.ListAsync();
            return entities
                .Select(x => new ProfileView
                {
                    AccountId = x.AccountId,
                    Birthday = x.Birthday,
                    Gender = x.Gender,
                    Id = x.Id,
                    Nickname = x.Nickname
                })
                .ToList();
        }

        public async Task<ProfileView> GetProfileAsync(Guid id)
        {
            var entity = await profileRepository.GetAsync(id);
            return new ProfileView
            {
                AccountId = entity.AccountId,
                Birthday = entity.Birthday,
                Gender = entity.Gender,
                Id = entity.Id,
                Nickname = entity.Nickname
            };
        }

        public async Task<ProfileView> GetProfileByAccountIdAsync(Guid id)
        {
            var entity = await profileRepository.GetByAccountIdAsync(id);
            return new ProfileView
            {
                AccountId = entity.AccountId,
                Birthday = entity.Birthday,
                Gender = entity.Gender,
                Id = entity.Id,
                Nickname = entity.Nickname
            };
        }

        public async Task<ProfileView> GetMyProfileAsync()
        {
            var user = httpContextAccessor.HttpContext.User.Identity.Name;
            var entity = await accountRepository.GetAsync(user);
            return new ProfileView
            {
                AccountId = entity.Id,
                Birthday = entity.Profile.Birthday,
                Gender = entity.Profile.Gender,
                Id = entity.ProfileId.Value,
                Nickname = entity.Profile.Nickname
            };
        }
    }
}
