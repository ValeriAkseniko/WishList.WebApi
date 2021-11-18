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
using WishList.BusinessLogic.Interfaces;
using WishList.DataAccess.Interfaces.Repositories;
using WishList.DataTransferObjects.Accounts;
using WishList.DataTransferObjects.Constants;
using WishList.DataTransferObjects.Profile;
using WishList.DataTransferObjects.Users;
using WishList.Entities.Models;

namespace WishList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Route("Create")]
        [AllowAnonymous]
        public async Task Create([FromBody] AccountCreateRequest accountCreateRequest)
        {
            await userService.CreateAccount(accountCreateRequest);
        }

        [HttpGet]
        [Route("ListAccounts")]
        [Authorize(Roles = "Admin")]
        public async Task<List<UsersView>> GetListAccount()
        {
            return await userService.GetUserList();
        }

        [HttpGet]
        [Route("Get")]
        [Authorize(Roles = "Admin")]
        public async Task<UsersView> GetAccount()
        {
            return await userService.GetUser();
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task Login(string login, string password)
        {
            await userService.Login(login, password);
        }

        [HttpPost]
        [Route("Logout")]
        [Authorize]
        public async Task Logout()
        {
            await userService.Logout();
        }

        [HttpPost]
        [Route("UpdateProfile")]
        [Authorize]
        public async Task UpdateProfile([FromBody] ProfileUpdateRequest profileUpdateRequest)
        {
            await userService.UpdateProfile(profileUpdateRequest);
        }
    }
}
