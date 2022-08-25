using Entities;
using Entities.Responses;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Services.Contracts;
using Microsoft.AspNetCore.Routing;

namespace Services
{
    public class TransactionService : ITransactionService
    {
        private readonly BBBankContext _bbBankContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TransactionService(BBBankContext BBBankContext, IHttpContextAccessor httpContextAccessor)
        {
            _bbBankContext = BBBankContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LineGraphData> GetLast12MonthBalances(string? userId)
        {
            // Object to contain the line graph data
            var lineGraphData = new LineGraphData();

            // Object to contain the transactions data
            var allTransactions = new List<Transaction>();
            if (userId == null)
            {
                // if account id is NULL then fetch all transactions
                allTransactions = _bbBankContext.Transactions.ToList();
            }
            else
            {
                // if account id is not NULL then fetch all transactions for specific account id
                allTransactions = _bbBankContext.Transactions.Where(x => x.Account.User.Id == userId).ToList();
            }
            if (allTransactions.Count() > 0)
            {
                // Calculate the total balance till now
                var totalBalance = allTransactions.Sum(x => x.TransactionAmount);
                lineGraphData.TotalBalance = totalBalance;

                decimal lastMonthTotal = 0;

                // looping through last three months starting from the current
                for (int i = 12; i > 0; i--)
                {
                    // Calculate the running total balance
                    var runningTotal = allTransactions.Where(x => x.TransactionDate >= DateTime.Now.AddMonths(-i) &&
                       x.TransactionDate < DateTime.Now.AddMonths(-i + 1)).Sum(y => y.TransactionAmount) + lastMonthTotal;

                    // adding labels to line graph data for current month and year
                    lineGraphData.Labels.Add(DateTime.Now.AddMonths(-i + 1).ToString("MMM yyyy"));

                    // adding data to line graph data for current month and year
                    lineGraphData.Figures.Add(runningTotal);

                    // saving the running total for this month
                    lastMonthTotal = runningTotal;
                }


                var routeValues = _httpContextAccessor.HttpContext.GetRouteData().Values;
                if (routeValues["version"] != null && routeValues["version"].ToString() == "2")
                {
                    lineGraphData.Average = Math.Round(lineGraphData.Figures.Sum() / lineGraphData.Figures.Count(),2);
                }
            }
            // returning the line graph data object
            return lineGraphData;
        }

    }
}