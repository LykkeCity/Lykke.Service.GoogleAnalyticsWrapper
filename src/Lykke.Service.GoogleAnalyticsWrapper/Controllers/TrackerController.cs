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
        
        /// <summary>
        /// Tracks user registration
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

            await _gaTrackerService.SendEventAsync(new TrackerInfo
                {
                    UserId = model.UserId,
                    UserAgent = model.UserAgent,
                    ClientInfo = model.ClientInfo,
                    Ip = model.Ip
                },
                TrackerCategories.Users, TrackerEvents.UserRegistered);
            
            return Ok();
        }
        
        /// <summary>
        /// Tracks kyc completed
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

            await _gaTrackerService.SendEventAsync(new TrackerInfo
                {
                    UserId = model.UserId,
                    UserAgent = model.UserAgent,
                    ClientInfo = model.ClientInfo,
                    Ip = model.Ip
                },
                TrackerCategories.Users, TrackerEvents.KycCompleted);
            
            return Ok();
        }
        
        /// <summary>
        /// Tracks margin order change
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

            await _gaTrackerService.SendEventAsync(new TrackerInfo
                {
                    UserId = model.UserId,
                    UserAgent = model.UserAgent,
                    ClientInfo = model.ClientInfo,
                    Ip = model.Ip
                },
                TrackerCategories.MarginTrading, TrackerEvents.OrderCreated);
            
            return Ok();
        }
        
        /// <summary>
        /// Tracks withdraw / deposit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

            await _gaTrackerService.SendWithdrawDepositEventAsync(new WithdrawDepositInfo{
                Amount = model.Amount, 
                AssetId = model.AssetId, 
                UserId = model.UserId, 
                UserAgent = model.UserAgent,
                ClientInfo = model.ClientInfo,
                Ip = model.Ip
            });
            
            return Ok();
        }
        
        /// <summary>
        /// Tracks cashout fee as a transaction in GA
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("trackCashout")]
        [SwaggerOperation("TrackCashout")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> TrackCashout([FromBody]TransactionModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessage());

            if (!model.UserId.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.UserId)} value"));

            await _gaTrackerService.SendTransactionAsync(new TransactionInfo{
                Id = model.Id,
                UserId = model.UserId, 
                Amount = model.Amount, 
                AssetId = model.AssetId, 
                Name = GaTransactionType.Cashout,
                ClientInfo = model.ClientInfo,
                UserAgent = model.UserAgent,
                Ip = model.Ip
            });
            
            return Ok();
        }
        
        /// <summary>
        /// Tracks trade fee as a transaction in GA
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("trackTrade")]
        [SwaggerOperation("TrackTrade")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> TrackTrade([FromBody]TransactionModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessage());

            if (!model.UserId.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.UserId)} value"));

            await _gaTrackerService.SendTransactionAsync(new TransactionInfo{
                Id = model.Id,
                UserId = model.UserId, 
                Amount = model.Amount, 
                AssetId = model.AssetId, 
                Name = GaTransactionType.Trade,
                ClientInfo = model.ClientInfo,
                UserAgent = model.UserAgent,
                Ip = model.Ip
            });
            
            return Ok();
        }
    }
}
