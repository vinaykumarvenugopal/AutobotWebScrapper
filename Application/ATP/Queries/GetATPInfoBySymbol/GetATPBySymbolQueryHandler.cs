using AutobotWebScrapper.Application.Common.Exceptions;
using AutobotWebScrapper.Application.Interfaces.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutobotWebScrapper.Application.ATP.Queries.GetATPInfoBySymbol
{
    public class GetATPBySymbolQueryHandler : IRequestHandler<GetATPBySymbolQuery, ATPInfoVM>
    {
        private readonly IFetchFuturesDataService _fetchFuturesDataService;

        public GetATPBySymbolQueryHandler(IFetchFuturesDataService fetchFuturesDataService)
        {
            _fetchFuturesDataService = fetchFuturesDataService;
        }
        public async Task<ATPInfoVM> Handle(GetATPBySymbolQuery request, CancellationToken cancellationToken)
        {
            ATPInfoVM result = null;

            double atp, firstDayClosePrice, difference;

            try
            {
                var futuresData = await _fetchFuturesDataService
                               .GetHistoricalFuturesByDateAsync(request.SymbolName, request.PreviousContractFromDate,
                               request.PreviousContractToDate, request.PreviousContractExpiryDate);

                atp = Math.Round(Math.Round(futuresData.Data.Average(r => r.TurnOverInLacs), 2) / Math.Round(futuresData.Data.Average(r => r.NoOfcontracts), 2), 2) *
                           (Math.Round(100000.00 / futuresData.LotSize, 2));
                firstDayClosePrice = await _fetchFuturesDataService.GetFuturesDayCloseByDateAsync(request.SymbolName,
                                    request.FirstDayOfNewContract, request.NewContractExpiryDate);
                atp = Math.Round(atp, 2);
                difference = Math.Round(Math.Abs(atp - firstDayClosePrice), 2);
            }
            catch (NoRecordsFoundException)
            {

                return result;
            }

            result = new ATPInfoVM
            {
                ATP = atp,
                Difference = difference,
                FirstDayClosePrice = firstDayClosePrice,
                UpsideStrikePrice = Math.Round(firstDayClosePrice + difference, 2),
                DownsideStrikePrice = Math.Round(firstDayClosePrice - difference, 2),
                SymbolName = request.SymbolName
            };

            return result;
        }
    }
}
