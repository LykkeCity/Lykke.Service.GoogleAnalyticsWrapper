using System.Net;
using System.Threading.Tasks;
using Common;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.GoogleAnalyticsWrapper.Controllers
{
    [Route("api/[controller]")]
    public class GoogleUserController : Controller
    {
        private readonly IGaUserService _gaUserService;

        public GoogleUserController(
            IGaUserService gaUserService
            )
        {
            _gaUserService = gaUserService;
        }
        
        [HttpGet("getGaUserId")]
        [SwaggerOperation("GetGaUserId/{clientId}")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetGaUserId(string clientId)
        {
            if (!clientId.IsGuid())
                return BadRequest($"Invalid {nameof(clientId)} value");
            
            var gaUserId = await _gaUserService.GetGaUserIdAsync(clientId);

            if (gaUserId == null)
                return NotFound();
            
            return Ok(gaUserId);
        }
    }
}
