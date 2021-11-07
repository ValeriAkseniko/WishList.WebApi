using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WishList.DataAccess;
using WishList.DataTransferObjects.Accounts;
using WishList.DataTransferObjects.Constants;
using WishList.Entities.Models;

namespace WishList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly WishListContext wishListContext;
        public AccountController(WishListContext wishListContext)
        {
            this.wishListContext = wishListContext;
        }

        [HttpPost]
        [Route("Create")]
        [AllowAnonymous]
        public async Task<bool> Create([FromBody] AccountCreateRequest accountCreateRequest)
        {            
            var existAccount = await wishListContext.Accounts.FirstOrDefaultAsync(x => x.Login == accountCreateRequest.Login);
            if (existAccount == null)
            {
                Account account = new Account()
                {
                    CreateDate = DateTime.Now,
                    Email = accountCreateRequest.Email,
                    HashPassword = accountCreateRequest.Password,
                    Id = Guid.NewGuid(),
                    Login = accountCreateRequest.Login,
                    RoleId = Permission.Id.DefaultUser
                };
                await wishListContext.Accounts.AddAsync(account);
                await wishListContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpGet]
        [Route("ListAccounts")]
        [Authorize(Roles="Admin")]
        public async Task<List<Account>> GetListAccount()
        {
            var user = HttpContext.User.Identity.Name;
            List<Account> accounts = await wishListContext.Accounts.ToListAsync();
            return accounts;
        }

        [HttpGet]
        [Route("Get")]
        [Authorize(Roles ="Admin")]
        public async Task<Account> GetAccounte(Guid id)
        {
            Account account = await wishListContext.Accounts.FirstOrDefaultAsync(x => x.Id == id);
            return account;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task Login(string Login, string Password)
        {
            Account account = await wishListContext.Accounts.Include(x=>x.Role).FirstOrDefaultAsync(u => u.Login == Login && u.HashPassword == Password);
            if (account != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, Login),
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
    }
}
