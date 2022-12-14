using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace BBBankAPI.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }


        /// <summary>
        /// Api Version is automatically dependency Injected
        /// This function returns Last 12 Months Balances
        /// </summary>
        /// <param name="apiVersion"></param>
        /// <returns>
        /// Returns LineGraphData with Average in v2 and without Average in v1
        /// </returns>
        // [Authorize(Roles = "bank-manager")]
        [HttpGet]
        [Route("GetLast12MonthBalances")]
        public async Task<ActionResult> GetLast12MonthBalances()
        {
            try
            {
                return new OkObjectResult(await _transactionService.GetLast12MonthBalances(null));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
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