using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Text.Json;

namespace BBBankAPI.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }


        /// <summary>
        /// Get the last 12 month balances
        /// </summary>
        /// <returns>Returns last 12 months data</returns>
        [HttpGet]
        [Route("GetLast12MonthBalances")]
        public async Task<JsonResult> GetLast12MonthBalances()
        {
            try
            {
                var res = await _transactionService.GetLast12MonthBalances(null);

                return new JsonResult(JsonSerializer.Serialize(res));
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }
        }


        /// <summary>
        /// Get the last 12 month balances for specific user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns>Returns last 12 months data</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /GetLast12MonthBalances
        ///     {
        ///        "userId": "0ea07fd2-e971-4240-b280-2b1865f7cce8"
        ///   
        ///     }
        ///
        /// </remarks>
        [HttpGet]
        [Route("GetLast12MonthBalances/{userId}")]
        public async Task<ActionResult> GetLast12MonthBalances(string userId)
        {
            try
            {
                return new OkObjectResult(await _transactionService.GetLast12MonthBalances(userId));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }
    }
}