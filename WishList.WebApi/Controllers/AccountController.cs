using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WishList.DataAccess.Interfaces.Repositories;
using WishList.DataTransferObjects.Accounts;
using WishList.DataTransferObjects.Constants;
using WishList.Entities.Models;

namespace WishList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository accountRepository;
        private readonly IProfileRepository profileRepository;

        public AccountController(IAccountRepository accountRepository, IProfileRepository profileRepository)
        {
            this.accountRepository = accountRepository;
            this.profileRepository = profileRepository;
        }

        [HttpPost]
        [Route("Create")]
        [AllowAnonymous]
        public async Task<bool> Create([FromBody] AccountCreateRequest accountCreateRequest)
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
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpGet]
        [Route("ListAccounts")]
        [Authorize(Roles = "Admin")]
        public async Task<List<Account>> GetListAccount()
        {
            List<Account> accounts = await accountRepository.ListAsync();
            return accounts;
        }

        [HttpGet]
        [Route("Get")]
        [Authorize(Roles = "Admin")]
        public async Task<Account> GetAccounte(Guid id)
        {
            Account account = await accountRepository.GetAsync(id);
            return account;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task Login(string login, string password)
        {
            Account account = await accountRepository.GetAsync(login);
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
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            }
        }

        [HttpPost]
        [Route("Logout")]
        [Authorize]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hash);
        }
    }
}
