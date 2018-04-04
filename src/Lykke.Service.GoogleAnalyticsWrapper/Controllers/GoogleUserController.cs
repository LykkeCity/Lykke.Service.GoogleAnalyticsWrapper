using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.ApiLibrary.Extensions;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTraffic;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaUser;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Services;
using Lykke.Service.GoogleAnalyticsWrapper.Models;
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
        
        /// <summary>
        /// Gets gaUserId by client id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet("getGaUserId/{clientId}")]
        [SwaggerOperation("GetGaUserId")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetGaUserId(string clientId)
        {
            if (!clientId.IsGuid())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(clientId)} value"));
            
            var gaUser = await _gaUserService.GetGaUserAsync(clientId);

            if (gaUser == null)
                return NotFound();
            
            return Json(gaUser.TrackerUserId);
        }
        
        /// <summary>
        /// Gets information about client traffic
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet("getGaUserTraffic/{clientId}")]
        [SwaggerOperation("GetGaUserTraffic")]
        [ProducesResponseType(typeof(GaTraffic), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GaTraffic), (int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetGaUserTraffic(string clientId)
        {
            if (!clientId.IsGuid())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(clientId)} value"));
            
            var traffic = await _gaUserService.GetGaUserTrafficAsync(clientId);

            return Ok(traffic);
        }
        
        /// <summary>
        /// Adds client traffic
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("addGaUserTraffic")]
        [SwaggerOperation("AddGaUserTraffic")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddGaUserTraffic([FromBody]GaTrafficModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create(ModelState.GetErrorMessage()));

            var traffic = new GaTraffic
            {
                ClientId = model.ClientId,
                Source = model.Source,
                Medium = model.Medium,
                Campaign = model.Campaign,
                Keyword = model.Keyword,
                Content = model.Content
            };
            
            await _gaUserService.AddGaUserTrafficAsync(traffic);

            return Ok();
        }
        
        /// <summary>
        /// Adds client GA cid
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("addGaUserCid")]
        [SwaggerOperation("AddGaUserCid")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddGaUserCid([FromBody]GaUserModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create(ModelState.GetErrorMessage()));

            if (!model.ClientId.IsGuid())
                return BadRequest(ErrorResponse.Create($"{nameof(model.ClientId)} invalid"));

            if (string.IsNullOrEmpty(model.Cid))
                model.Cid = GaUser.GenerateNewCid();
            else if (!Regex.IsMatch(model.Cid, "\\d{9}\\.\\d{9}"))
                return BadRequest(ErrorResponse.Create($"{nameof(model.Cid)} invalid"));

            await _gaUserService.AddGaUserAsync(model.ClientId, model.Cid);

            return Ok();
        }
    }
}
