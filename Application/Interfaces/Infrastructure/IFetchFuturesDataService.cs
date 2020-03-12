using AutobotWebScrapper.Application.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutobotWebScrapper.Application.Interfaces.Infrastructure
{
    public interface IFetchFuturesDataService
    {
        public Task<HistoricalFuturesResponse> GetHistoricalFuturesByDateAsync(string symbolName, DateTime startDate,
                    DateTime endDate, DateTime expiryDate);

        public Task<double> GetFuturesDayCloseByDateAsync(string symbolName, DateTime date, DateTime expiryDate);
    }
}

