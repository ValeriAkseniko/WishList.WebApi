using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
using WishList.Infrastructure.Exceptions;

namespace WishList.BusinessLogicServices
{
    public class UserService : IUserService
    {
        private readonly IAccountRepository accountRepository;
        private readonly IProfileRepository profileRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IRoleRepository roleRepository;
        private readonly ILogger<UserService> logger;
        private readonly INotificationService notificationService;

        public UserService(IAccountRepository accountRepository, IProfileRepository profileRepository, IHttpContextAccessor httpContextAccessor, IRoleRepository roleRepository, ILogger<UserService> logger, INotificationService notificationService)
        {
            this.accountRepository = accountRepository;
            this.profileRepository = profileRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.roleRepository = roleRepository;
            this.logger = logger;
            this.notificationService = notificationService;
        }

        public async Task CreateAccountAsync(AccountCreateRequest accountCreateRequest)
        {
            if (await accountRepository.LoginExistAsync(accountCreateRequest.Login))
            {
                var exception = new LoginExistException(accountCreateRequest.Login);
                logger.LogError(exception.ToString());
                throw exception;
            }
            if (await accountRepository.EmailExistAsync(accountCreateRequest.Email))
            {
                var exception = new EmailExistException(accountCreateRequest.Email);
                logger.LogError(exception.ToString());
                throw exception;
            }
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

            await profileRepository.CreateAsync(profile);
            await accountRepository.UpdateProfileIdAsync(profile.Id, account.Id);
            await notificationService.RegistrationSuccessful(account.Email, account.Login);
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
            if (entity == null)
            {
                var exception = new UserNotFoundException(user);
                logger.LogError(exception.ToString());
                throw exception;
            }
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
            var account = await accountRepository.GetAsync(login.ToLower());
            if (account != null && account.HashPassword == GetHash(password.ToLower()))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, account.Role.Name)
                };
                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            }
            else
            {
                var exception = new WrongLoginOrPasswordException();
                logger.LogError(exception.ToString());
                throw exception;
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
            if (account == null)
            {
                var exception = new UserNotFoundException(user);
                logger.LogError(exception.ToString());
                throw exception;
            }
            var profile = await profileRepository.GetByAccountIdAsync(account.Id);
            if (profile == null)
            {
                var exception = new ProfileNotFoundByAccountException(account.Id);
                logger.LogError(exception.ToString());
                throw exception;
            }

            profile.Nickname = profileUpdateRequest.Nickname;
            profile.Gender = (Gender)profileUpdateRequest.Gender;
            profile.Birthday = profileUpdateRequest.Birthday;

            await profileRepository.UpdateAsync(profile);
        }

        public async Task<List<RoleView>> ListRolesAsync()
        {
            var entities = await roleRepository.ListAsync();
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
            var role = new Role()
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
            if (role == null)
            {
                var exception = new RoleNotFoundException(id);
                logger.LogError(exception.ToString());
                throw exception;
            }
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
            if (entity == null)
            {
                var exception = new ProfileNotFoundException(id);
                logger.LogError(exception.ToString());
                throw exception;
            }
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
            if (entity == null)
            {
                var exception = new ProfileNotFoundByAccountException(id);
                logger.LogError(exception.ToString());
                throw exception;
            }
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
            if (entity == null)
            {
                var exception = new UserNotFoundException(user);
                logger.LogError(exception.ToString());
                throw exception;
            }
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
