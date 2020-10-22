using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ScrumPokerAPI.Controllers
{
    [Authorize]
    [ApiController]
    public class APIBaseController : ControllerBase
    {
     
    }
}
