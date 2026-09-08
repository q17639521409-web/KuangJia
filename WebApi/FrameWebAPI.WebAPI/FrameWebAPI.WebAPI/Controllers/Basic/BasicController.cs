using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrameWebAPI.WebAPI.Controllers.Basic
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize("Permission")]
    public class BasicController : ControllerBase
    {
    } 
}
    