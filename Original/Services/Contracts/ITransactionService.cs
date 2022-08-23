using Entities.Responses;
using Entities.Responses.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface ITransactionService
    {
        Task<LineGraphData> GetLast12MonthBalances(string? userId);
        Task<LineGraphDataAverage> GetLast12MonthBalancesWithAverage(string? userId);
    }
}