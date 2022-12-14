using Microsoft.AspNetCore.Mvc;

namespace NoKates.Common.Infrastructure.Controllers
{
    public class NoKatesControllerBase: ControllerBase
    {
        [HttpGet("NoKates/ControllerHealth")]
        public ActionResult GetControllerHealth()
        {
            return Ok();
        }
    }
}
