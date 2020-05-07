using AutobotWebScrapper.Application.Common.Exceptions;
using AutobotWebScrapper.Application.Models.Infrastructure;
using AutobotWebScrapper.Infrastructure.NSEDataScrapper.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Infrastructure.IntegrationTests
{
    public class FetchFuturesDataServiceShould : IClassFixture<HttpClientFixture>
    {
        private readonly HttpClientFixture fixture;
        public FetchFuturesDataServiceShould(HttpClientFixture clientFixture)
        {
            fixture = clientFixture;
        }

        [Fact]
        public async Task GetDataSuccessfullyForADay()
        {
            //Arrange
            var sut = new FetchFuturesDataService(fixture.HTTPClientFactory);

            //Act
            var response = await sut.GetHistoricalFuturesByDateAsync("NIFTY", new DateTime(2018, 7, 10),
                                new DateTime(2018, 7, 10), new DateTime(2018, 7, 26));

            //Assert
            response.Data.Should().HaveCount(1, "because we are requesting data for only single day");

            response.LotSize.Should().Be(75, "because lot size of Nifty is 75 as of now..");


        }
        [Fact]
        public async Task GetDataSuccessfullyForAMonth()
        {
            //Arrange
            var sut = new FetchFuturesDataService(fixture.HTTPClientFactory);

            //Act
            var response = await sut.GetHistoricalFuturesByDateAsync("NIFTY", new DateTime(2018, 6, 29),
                                new DateTime(2018, 7, 26), new DateTime(2018, 7, 26));

            //Assert
            response.Data.Should().HaveCountGreaterThan(0);

            response.LotSize.Should().Be(75, "because lot size of Nifty is 75 as of now..");


        }

        [Fact]
        public async Task GetStockDataSuccessfullyForAMonth()
        {
            //Arrange
            var sut = new FetchFuturesDataService(fixture.HTTPClientFactory);

            //Act
            var response = await sut.GetHistoricalFuturesByDateAsync("INFY", new DateTime(2020, 1, 31),
                                new DateTime(2020, 2, 27), new DateTime(2020, 2, 27));

            //Assert
            response.Data.Should().HaveCountGreaterThan(0);

            response.LotSize.Should().Be(1200, "because lot size of Infy is 1200 as of now..");


        }
        [Fact]
        public async Task ThrowExceptionIfItsANoMarketDay()
        {
            //Arrange
            var sut = new FetchFuturesDataService(fixture.HTTPClientFactory);

            //Act
            Func<Task<HistoricalFuturesResponse>> func = async () =>
                await sut.GetHistoricalFuturesByDateAsync("INFY", new DateTime(2020, 3, 7),
                                new DateTime(2020, 3, 7), new DateTime(2020, 3, 26));

            //Assert
            await func.Should().ThrowAsync<NoRecordsFoundException>();


        }
        [Fact]
        public async Task ThrowExceptionIfInvalidDataIsPassed()
        {
            //Arrange
            var sut = new FetchFuturesDataService(fixture.HTTPClientFactory);

            //Act
            Func<Task<HistoricalFuturesResponse>> func = async () =>
                await sut.GetHistoricalFuturesByDateAsync("VINAY", new DateTime(2020, 3, 7),
                                new DateTime(2020, 3, 7), new DateTime(2020, 3, 26));

            //Assert
            await func.Should().ThrowAsync<NoRecordsFoundException>();


        }

        [Fact]
        public async Task HaveValidClosePriceWhenGetFuturesDayCloseByDateCalled()
        {
            //Arrange
            var sut = new FetchFuturesDataService(fixture.HTTPClientFactory);

            //Act
            var response = await sut.GetFuturesDayCloseByDateAsync("NIFTY", new DateTime(2018, 7, 10),
                                new DateTime(2018, 7, 26));

            //Assert
            response.Should().BePositive();
        }
        [Fact]
        public async Task HaveNoClosePriceWhenGetFuturesDayCloseByDateCalledWithInvalidInput()
        {
            //Arrange
            var sut = new FetchFuturesDataService(fixture.HTTPClientFactory);



            //Act
            Func<Task<double>> func = async () =>
               await sut.GetFuturesDayCloseByDateAsync("NIFTY", new DateTime(2018, 7, 1),
                                new DateTime(2018, 7, 26));

            //Assert
            await func.Should().ThrowAsync<NoRecordsFoundException>();


        }

        [Fact]
        public async Task GetSuccessfullyGetCurrentDayFutureInstrumentData()
        {
            //Arrange
            var sut = new FetchFuturesDataService(fixture.HTTPClientFactory);

            //Act
            var response = await sut.GetCurrentDayFutureInstrumentDataAsync("INFY");

            //Assert
            response.Details.Should().NotBeEmpty("There should be collection of stock information");

            response.SymbolName.Should().Be("INFY", "we are requesting data for INFY future instrument");
        }

        [Fact]
        public async Task ExceptionWhenInvalidSymbolNamePassedCallingGetCurrentDayFutureInstrumentData()
        {
            //Arrange
            var sut = new FetchFuturesDataService(fixture.HTTPClientFactory);

            //Act
            Func<Task<FutureInstrumentResponse>> func = async () =>
                await sut.GetCurrentDayFutureInstrumentDataAsync("VINAY");

            //Assert
            await func.Should().ThrowAsync<NoRecordsFoundException>();



        }
    }
}
