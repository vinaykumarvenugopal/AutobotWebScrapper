using AutobotWebScrapper.Application.ATP.Queries.GetATPInfoBySymbol;
using AutobotWebScrapper.Application.Common.Exceptions;
using AutobotWebScrapper.Application.Interfaces.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace TestAPICalls
{
    public class EntryPointService : IEntryPointService
    {
        private readonly ILogger<EntryPointService> _logger;
        private readonly IMediator _mediator;


        public EntryPointService(IMediator mediator, ILogger<EntryPointService> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task RunAsync()
        {
            _logger.LogInformation("Preparing to initiate");

            try
            {
                var response = await _mediator.Send(new GetATPBySymbolQuery
                {
                    SymbolName = "TATASTEEL",
                    PreviousContractFromDate = new DateTime(2020, 1, 29),
                    PreviousContractToDate = new DateTime(2020, 2, 27),
                    PreviousContractExpiryDate = new DateTime(2020, 2, 27),
                    FirstDayOfNewContract = new DateTime(2020, 2, 28),
                    NewContractExpiryDate = new DateTime(2020, 3, 26)
                });
                _logger.LogInformation(JsonConvert.SerializeObject(response));
            }
            catch (ValidationException ve) { 
                _logger.LogError(ve, "Validation error occurred.");
                foreach (var error in ve.Failures) _logger.LogError(error.Value[0]);
            }
      
        }
    }
}