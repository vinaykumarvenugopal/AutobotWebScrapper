using AutobotWebScrapper.Application.ATP.Queries.GetATPInfoBySymbol;
using AutobotWebScrapper.Application.Common.Exceptions;
using AutobotWebScrapper.Application.Interfaces.Infrastructure;
using AutobotWebScrapper.Application.Models.Infrastructure;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests
{
    public class GetATPBySymbolQueryHandlerShould
    {
        Mock<IFetchFuturesDataService> mockFuturesDataService;
        public GetATPBySymbolQueryHandlerShould()
        {
            mockFuturesDataService = new Mock<IFetchFuturesDataService>();
        }

        [Fact]
        public async Task ReturnValidResultIfCorrectInputPassed()
        {
            //Arrange
            mockFuturesDataService
                .Setup(s => s.GetHistoricalFuturesByDateAsync(It.IsAny<string>(), It.IsAny<DateTime>(),
                            It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new HistoricalFuturesResponse
                            {
                                SymbolName = "NIFTY",
                                LotSize = 75,
                                Data = new List<HistoricalFutureOHLC>
                                {
                                    new HistoricalFutureOHLC {NoOfcontracts = 136575, TurnOverInLacs= 1092692.52},
                                    new HistoricalFutureOHLC {NoOfcontracts = 122927, TurnOverInLacs= 981194.11},
                                    new HistoricalFutureOHLC {NoOfcontracts = 117896, TurnOverInLacs= 945971.09}
                                }
                            });

            mockFuturesDataService
                .Setup(s => s.GetFuturesDayCloseByDateAsync(It.IsAny<string>(), It.IsAny<DateTime>(),
                            It.IsAny<DateTime>())).ReturnsAsync(11306.7);

            //Act
            var sut = new GetATPBySymbolQueryHandler(mockFuturesDataService.Object);
            var output = await sut.Handle(new GetATPBySymbolQuery(), new CancellationToken());

            //Assert
            using (new AssertionScope())
            {

                output.ATP.Should().Be(10666.64);
                output.Difference.Should().Be(640.06);
                output.UpsideStrikePrice.Should().Be(11946.76);
                output.DownsideStrikePrice.Should().Be(10666.64);
            }

        }
        [Fact]
        public async Task ReturnNullResultIfInvalidPreviousContractDatesPassed()
        {
            //Arrange
            mockFuturesDataService
                .Setup(s => s.GetHistoricalFuturesByDateAsync(It.IsAny<string>(), It.IsAny<DateTime>(),
                            It.IsAny<DateTime>(), It.IsAny<DateTime>())).Throws(new NoRecordsFoundException("No Records"));

            mockFuturesDataService
                .Setup(s => s.GetFuturesDayCloseByDateAsync(It.IsAny<string>(), It.IsAny<DateTime>(),
                            It.IsAny<DateTime>())).ReturnsAsync(11306.7);


            //Act
            var sut = new GetATPBySymbolQueryHandler(mockFuturesDataService.Object);
            var output = await sut.Handle(new GetATPBySymbolQuery(), new CancellationToken());

            //Assert
            output.Should().BeNull();
        }
        [Fact]
        public async Task ReturnNullResultIfInvalidCurrentContractDatesPassed()
        {
            //Arrange
            mockFuturesDataService
                .Setup(s => s.GetHistoricalFuturesByDateAsync(It.IsAny<string>(), It.IsAny<DateTime>(),
                            It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new HistoricalFuturesResponse
                            {
                                SymbolName = "NIFTY",
                                LotSize = 75,
                                Data = new List<HistoricalFutureOHLC>
                                {
                                    new HistoricalFutureOHLC {NoOfcontracts = 136575, TurnOverInLacs= 1092692.52},
                                    new HistoricalFutureOHLC {NoOfcontracts = 122927, TurnOverInLacs= 981194.11},
                                    new HistoricalFutureOHLC {NoOfcontracts = 117896, TurnOverInLacs= 945971.09}
                                }
                            });

            mockFuturesDataService
                .Setup(s => s.GetFuturesDayCloseByDateAsync(It.IsAny<string>(), It.IsAny<DateTime>(),
                            It.IsAny<DateTime>())).Throws(new NoRecordsFoundException("No Records"));


            //Act
            var sut = new GetATPBySymbolQueryHandler(mockFuturesDataService.Object);
            var output = await sut.Handle(new GetATPBySymbolQuery(), new CancellationToken());

            //Assert
            output.Should().BeNull();
        }
    }
}
