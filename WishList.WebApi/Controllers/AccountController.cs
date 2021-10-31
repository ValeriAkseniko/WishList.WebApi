using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task Create([FromBody] AccountCreateRequest accountCreateRequest)
        {
            Account account = new Account()
            {
                CreateDate = DateTime.Now,
                Email = accountCreateRequest.Email,
                HashPassword = accountCreateRequest.Password,
                Id = Guid.NewGuid(),
                Login = accountCreateRequest.Login,
                RoleId = Guid.Parse("3868C13D-8D12-46A6-B709-52BCF010BDFF")
            };
            await wishListContext.Accounts.AddAsync(account);
            await wishListContext.SaveChangesAsync();
        }

        [HttpGet]
        [Route("ListAccounts")]
        public async Task<List<Account>> GetListAccount()
        {
            List<Account> accounts = await wishListContext.Accounts.ToListAsync();
            return accounts;
        }
    }
}
