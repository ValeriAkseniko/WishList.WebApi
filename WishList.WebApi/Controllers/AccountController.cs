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
        public async Task Create([FromBody] AccountCreateRequest accountCreateRequest)
        {
            Account account = new Account()
            {
                CreateDate = DateTime.Now,
                Email = accountCreateRequest.Email,
                HashPassword = accountCreateRequest.Password,
                Id = Guid.NewGuid(),
                Login = accountCreateRequest.Login,
                RoleId = Guid.Parse("375af7cc-a281-4432-a3f5-14af10bf73f6")
            };
            await wishListContext.Accounts.AddAsync(account);
            await wishListContext.SaveChangesAsync();
        }

        [HttpGet]
        [Route("ListAccounts")]
        [Authorize]
        public async Task<List<Account>> GetListAccount()
        {
            var user = HttpContext.User.Identity.Name;
            List<Account> accounts = await wishListContext.Accounts.ToListAsync();
            return accounts;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<Account> GetAccounte(Guid id)
        {
            Account account = await wishListContext.Accounts.FirstOrDefaultAsync(x => x.Id == id);
            return account;
        }

        [HttpPost]
        [Route("Login")]
        public async Task Login(string Login, string Password)
        {
            Account account = await wishListContext.Accounts.FirstOrDefaultAsync(u => u.Login == Login && u.HashPassword == Password);
            if (account != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, Login)
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
