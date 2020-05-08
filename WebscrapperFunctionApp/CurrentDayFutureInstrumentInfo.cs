using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MediatR;
using AutobotWebScrapper.Application.Instrument.Queries.GetTodaysInstrumentInfo;
using AutobotWebScrapper.Application.Common.Exceptions;
using System.Linq;
using System.Web.Http;

namespace WebscrapperFunctionApp
{
    public class CurrentDayFutureInstrumentInfo
    {
        private readonly IMediator _mediator;
        public CurrentDayFutureInstrumentInfo(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [FunctionName("CurrentDayFutureInstrumentInfo")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "todaysfuturesymbolinfo")] HttpRequest req,
            ILogger log)
        {
            try
            {
                GetTodaysInstrumentInfoQuery request = new GetTodaysInstrumentInfoQuery
                {
                    SymbolName = req.Query["SymbolName"],
                    ContractExpiryDate = ConvertToDate(req.Query["ContractExpiryDate"])
                };

                log.LogInformation($"Received request to get current day " +
                    $"future instrument information. {JsonConvert.SerializeObject(request)}");

                try
                {
                    var response = await _mediator.Send(request);
                    if (response == null)
                    {
                        throw new NoRecordsFoundException("No information found for the given request.");
                    }

                    return new OkObjectResult(response);
                }
                catch (NoRecordsFoundException)
                {
                    return new BadRequestObjectResult(new
                    {
                        Error = $"No records found for the given input request. " +
                        $"{JsonConvert.SerializeObject(request)}"
                    });
                }
                catch (ValidationException ve)
                {
                    return new BadRequestObjectResult(ve.Failures.Select(e => new {
                        Field = e.Key,
                        Message = e.Value
                    }));
                }
            }
            catch (Exception e)
            {

                log.LogError(e, $"Some internal/unknown error occurred! {e.Message}");
                return new InternalServerErrorResult();
            }
            
            
        }
        private DateTime ConvertToDate(string DateInStringFormat)
        {
            if (string.IsNullOrEmpty(DateInStringFormat)) return DateTime.MinValue;

            return DateTime.ParseExact(DateInStringFormat, "yyyy-MM-dd", null);

        }
    }
}
