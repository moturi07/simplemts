using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtsDAL.Services.IServices;
using mtsDAL.ViewModels;
using System;
using System.Threading.Tasks;

namespace mtsapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class HomeController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public HomeController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        ///  This endpoint is used to create an account
        /// </summary>
        [HttpPost, Route("accounts")]
        public async Task<ActionResult<string>> SaveAccountDataAsync([FromBody] CreateAccountView model)
        {
            try
            {
                return Ok(await _accountService.SaveAccountDataAsync(model));
            }
            catch (Exception ex)
            {
                return BadRequest("Error saving Account. "+ex.Message);
            }
        }

        /// <summary>
        ///  This endpoint is used to get all accounts that are both active and dormant
        /// </summary>
        [Obsolete]
        [HttpGet, Route("accounts")]
        public async Task<ActionResult<ListResult<AccountView>>> GetAccountDataAsync()
        {
            try
            {
                return Ok(await _accountService.GetAccountDataAsync());
            }
            catch (Exception ex)
            {
                return BadRequest("Error finding Account's. " + ex.Message);
            }
        }

        /// <summary>
        ///  This endpoint is used to get a specific account that might be active or dormant
        /// </summary>
        [HttpGet, Route("accounts/{id}")]
        public async Task<ActionResult<ListResult<AccountView>>> GetAccountDataAsync([FromRoute] int id)
        {
            try
            {
                return Ok(await _accountService.GetAccountDataAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest("Error finding Account's. "+ex.Message);
            }
        }

        /// <summary>
        ///  This endpoint is used transfer amounts between active accounts
        /// </summary>
        [HttpPost, Route("transfers")]
        public async Task<ActionResult<int>> CreateTransactionAsync([FromBody] CreateTransaction model)
        {
            try
            {
                return Ok(await _accountService.CreateTransactionAsync(model));
            }
            catch (Exception ex)
            {
                return BadRequest("Error Creating Transaction. "+ex.Message);
            }
        }
    }
}
