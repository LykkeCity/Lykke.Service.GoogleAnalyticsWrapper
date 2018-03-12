using System.Net;
using System.Threading.Tasks;
using Common;
using Lykke.Common.ApiLibrary.Extensions;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Services;
using Lykke.Service.GoogleAnalyticsWrapper.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using ErrorResponse = Lykke.Common.Api.Contract.Responses.ErrorResponse;

namespace Lykke.Service.GoogleAnalyticsWrapper.Controllers
{
    [Route("api/[controller]")]
    public class TrackerController : Controller
    {
        private readonly IGaTrackerService _gaTrackerService;

        public TrackerController(
            IGaTrackerService gaTrackerService
        )
        {
            _gaTrackerService = gaTrackerService;
        }
        
        [HttpPost("userRegisteredEvent")]
        [SwaggerOperation("UserRegisteredEvent")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UserRegisteredEvent([FromBody]TrackEventModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create(ModelState.GetErrorMessage()));

            if (!model.UserId.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.UserId)} value"));

            await _gaTrackerService.SendEvent(new TrackerInfo
                {
                    UserId = model.UserId,
                    UserAgent = model.UserAgent,
                    ClientInfo = model.ClientInfo,
                    Ip = model.Ip
                },
                TrackerCategories.Users, TrackerEvents.UserRegistered);
            
            return Ok();
        }
        
        [HttpPost("kycCompletedEvent")]
        [SwaggerOperation("KycCompletedEvent")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> KycCompletedEvent([FromBody]TrackEventModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessage());

            if (!model.UserId.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.UserId)} value"));

            await _gaTrackerService.SendEvent(new TrackerInfo
                {
                    UserId = model.UserId,
                    UserAgent = model.UserAgent,
                    ClientInfo = model.ClientInfo,
                    Ip = model.Ip
                },
                TrackerCategories.Users, TrackerEvents.KycCompleted);
            
            return Ok();
        }
        
        [HttpPost("mtOrderCreatedEvent")]
        [SwaggerOperation("MtOrderCreatedEvent")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> MtOrderCreatedEvent([FromBody]TrackEventModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessage());

            if (!model.UserId.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.UserId)} value"));

            await _gaTrackerService.SendEvent(new TrackerInfo
                {
                    UserId = model.UserId,
                    UserAgent = model.UserAgent,
                    ClientInfo = model.ClientInfo,
                    Ip = model.Ip
                },
                TrackerCategories.MarginTrading, TrackerEvents.OrderCreated);
            
            return Ok();
        }
        
        [HttpPost("withdrawDepositEvent")]
        [SwaggerOperation("WithdrawDepositEvent")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> WithdrawDepositEvent([FromBody]WithdrawDepositEventModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessage());

            if (!model.UserId.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.UserId)} value"));

            await _gaTrackerService.SendWithdrawDepositEvent(new WithdrawDepositInfo{
                Amount = model.Amount, 
                AssetId = model.AssetId, 
                UserId = model.UserId, 
                UserAgent = model.UserAgent,
                ClientInfo = model.ClientInfo,
                Ip = model.Ip
            });
            
            return Ok();
        }
        
        [HttpPost("trackTransaction")]
        [SwaggerOperation("TrackTransaction")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> TrackTransaction([FromBody]TransactionModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessage());

            if (!model.UserId.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.UserId)} value"));

            await _gaTrackerService.SendTransaction(new TransactionInfo{
                Id = model.Id,
                UserId = model.UserId, 
                Amount = model.Amount, 
                AssetId = model.AssetId, 
                Fee = model.Fee,
                FeeAssetId = model.FeeAssetId
            });
            
            return Ok();
        }
    }
}
