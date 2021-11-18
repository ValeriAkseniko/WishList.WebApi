using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WishList.BusinessLogic.Interfaces;
using WishList.DataAccess;
using WishList.DataAccess.Interfaces.Repositories;
using WishList.DataTransferObjects.Accounts;
using WishList.DataTransferObjects.Constants;
using WishList.DataTransferObjects.Profile;
using WishList.DataTransferObjects.Role;
using WishList.DataTransferObjects.Users;
using WishList.Entities.Models;

namespace WishList.BusinessLogicServices
{
    public class UserService : IUserService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IProfileRepository profileRepository;
        private readonly WishListContext wishListContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IAccountRepository accountRepository, IProfileRepository profileRepository, IHttpContextAccessor httpContextAccessor, WishListContext wishListContext)
        {
            this.accountRepository = accountRepository;
            this.profileRepository = profileRepository;
            this.wishListContext = wishListContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CreateAccount(AccountCreateRequest accountCreateRequest)
        {
            var existAccount = await accountRepository.GetAsync(accountCreateRequest.Login);
            if (existAccount == null)
            {
                Account account = new Account()
                {
                    CreateDate = DateTime.Now,
                    Email = accountCreateRequest.Email,
                    HashPassword = GetHash(accountCreateRequest.Password),
                    Id = Guid.NewGuid(),
                    Login = accountCreateRequest.Login,
                    RoleId = Permission.Id.DefaultUser
                };
                await accountRepository.Create(account);
                Profile profile = new Profile()
                {
                    AccountId = account.Id,
                    Id = Guid.NewGuid()
                };
                await profileRepository.Create(profile);
                await accountRepository.UpdateProfileIdAsync(profile.Id, account.Id);
            }
        }

        public async Task<List<UsersView>> GetUserList()
        {
            var entitys = await accountRepository.ListAsync();
            List<UsersView> usersView = new List<UsersView>();
            foreach (var item in entitys)
            {
                UsersView userView = new UsersView()
                {
                    Login = item.Login,
                    Email = item.Email,
                    Gender = item.Profile.Gender,
                    Role = item.Role,
                    Nickname = item.Profile.Nickname
                };
                usersView.Add(userView);
            }
            return usersView;
        }

        public async Task<UsersView> GetUser()
        {
            var user = _httpContextAccessor.HttpContext.User.Identity.Name;
            var entity = await accountRepository.GetAsync(user);
            UsersView userView = new UsersView()
            {
                Login = entity.Login,
                Email = entity.Email,
                Gender = entity.Profile.Gender,
                Role = entity.Role,
                Nickname = entity.Profile.Nickname
            };
            return userView;
        }


        public async Task Login(string login, string password)
        {
            Account account = await wishListContext.Accounts
                .Include(x => x.Role)
                .Include(x => x.Profile)
                .FirstOrDefaultAsync(x => x.Login == login);
            if (account.HashPassword == GetHash(password) && account != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, account.Role.Name)
                };
                // создаем объект ClaimsIdentity
                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                // установка аутентификационных куки
                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            }
        }
        public async Task Logout()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }


        public async Task UpdateProfile(ProfileUpdateRequest profileUpdateRequest)
        {
            var user = _httpContextAccessor.HttpContext.User.Identity.Name;
            var account = await accountRepository.GetAsync(user);
            var profile = await profileRepository.GetAsyncByAccountId(account.Id);
            var entity = await profileRepository.GetAsync(profile.Id);
            entity.Nickname = profileUpdateRequest.Nickname;
            entity.Gender = (Gender)profileUpdateRequest.Gender;
            entity.Birthday = profileUpdateRequest.Birthday;
            wishListContext.Entry(entity).State = EntityState.Modified;
            await wishListContext.SaveChangesAsync();
        }

        public async Task<List<RoleView>> ListRoles()
        {
            var roles = await wishListContext.Roles.ToListAsync();
            List<RoleView> rolesView = new List<RoleView>();
            foreach (var item in roles)
            {
                RoleView roleView = new RoleView()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description
                };
                rolesView.Add(roleView);
            }
            return rolesView;
        }

        private string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hash);
        }
    }
}
