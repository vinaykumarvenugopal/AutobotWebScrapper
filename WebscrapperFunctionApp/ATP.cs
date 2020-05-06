using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AutobotWebScrapper.Application.ATP.Queries.GetATPInfoBySymbol;
using MediatR;
using System.Web.Http;
using AutobotWebScrapper.Application.Common.Exceptions;
using System.Linq;

namespace WebscrapperFunctionApp
{
    public class ATP
    {
        private readonly IMediator _mediator;
        public ATP(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [FunctionName("ATP")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "atp")] HttpRequest req,
            ILogger log)
        {
            try
            {
                GetATPBySymbolQuery request = new GetATPBySymbolQuery
                {

                    SymbolName = req.Query["SymbolName"],
                    PreviousContractFromDate = ConvertToDate(req.Query["PreviousContractFromDate"]),
                    PreviousContractToDate = ConvertToDate(req.Query["PreviousContractToDate"]),
                    PreviousContractExpiryDate = ConvertToDate(req.Query["PreviousContractExpiryDate"]),
                    NewContractExpiryDate = ConvertToDate(req.Query["NewContractExpiryDate"]),
                    FirstDayOfNewContract = ConvertToDate(req.Query["FirstDayOfNewContract"])
                };
                log.LogInformation($"Received request to get ATP information. {JsonConvert.SerializeObject(request)}");

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
                    return new BadRequestObjectResult(new { Error = $"No records found for the given input request. " +
                        $"{JsonConvert.SerializeObject(request)}" });
                }
                catch ( ValidationException ve)
                {
                    return new BadRequestObjectResult(ve.Failures.Select(e => new {
                        Field = e.Key,
                        Message = e.Value
                    }));
                }
            }
            catch (Exception e)
            {
                log.LogError(e, "Some internal/unknown error occurred!");
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
