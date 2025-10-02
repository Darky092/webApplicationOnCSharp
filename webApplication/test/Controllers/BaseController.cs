using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApplication.Authorization;

namespace webApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        // returns the current authenticated account (null if not logged in)
        public user user => (user)HttpContext.Items ["user"];
    }
}
