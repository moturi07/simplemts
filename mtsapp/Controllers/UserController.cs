using Microsoft.AspNetCore.Mvc;

namespace mtsapp.Controllers
{
    public class UserController : ControllerBase
    {
        /// <summary>
        ///  This Endpoint does not work
        /// </summary>
        public IActionResult Login()
        {
            return Ok();
        }
    }
}
