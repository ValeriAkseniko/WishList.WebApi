using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WishList.BusinessLogic.Interfaces;
using WishList.DataTransferObjects.Accounts;
using WishList.DataTransferObjects.Profile;
using WishList.DataTransferObjects.Users;

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
            await userService.CreateAccountAsync(accountCreateRequest);
        }

        [HttpGet]
        [Route("ListAccounts")]
        [Authorize(Roles = "Admin")]
        public async Task<List<UsersView>> GetListAccount()
        {
            return await userService.GetUserListAsync();
        }

        [HttpGet]
        [Route("Get")]
        [Authorize(Roles = "Admin")]
        public async Task<UsersView> GetAccount()
        {
            return await userService.GetUserAsync();
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task Login(string login, string password)
        {
            await userService.LoginAsync(login, password);
        }

        [HttpPost]
        [Route("Logout")]
        [Authorize]
        public async Task Logout()
        {
            await userService.LogoutAsync();
        }

        [HttpPost]
        [Route("UpdateProfile")]
        [Authorize]
        public async Task UpdateProfile([FromBody] ProfileUpdateRequest profileUpdateRequest)
        {
            await userService.UpdateMyProfileAsync(profileUpdateRequest);
        }
    }
}
