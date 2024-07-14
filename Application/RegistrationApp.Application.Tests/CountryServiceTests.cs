using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RegistrationApp.Core.Interfaces;
using RegistrationApp.Entities;
using ResistrationApp.Application.CountryService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RegistrationApp.Application.Tests
{
    [TestFixture]
    public class CountryServiceTests
    {
        private Mock<ICountriesRepository> _countriesRepositoryMock;
        private Mock<ILogger<CountryService>> _loggerMock;
        private CountryService _countryService;

        [SetUp]
        public void SetUp()
        {
            _countriesRepositoryMock = new Mock<ICountriesRepository>();
            _loggerMock = new Mock<ILogger<CountryService>>();
            _countryService = new CountryService(_countriesRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetCountries_ReturnsListOfCountryDTOs()
        {
            // Arrange
            var countries = new List<Country>
        {
            new Country { Id = 1, Name = "Country1" },
            new Country { Id = 2, Name = "Country2" }
        };
            _countriesRepositoryMock.Setup(repo => repo.GetAllCountries()).ReturnsAsync(countries);

            // Act
            var result = await _countryService.GetCountries();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("CountryDTO", result[0].GetType().Name);
            Assert.AreEqual("Country1", result[0].Name);
            Assert.AreEqual("Country2", result[1].Name);
        }

        [Test]
        public async Task GetProvinces_ReturnsListOfProvinceDTOs()
        {
            // Arrange
            int countryId = 1;
            var provinces = new List<Province>
        {
            new Province { Id = 1, Name = "Province1", CountryId = countryId },
            new Province { Id = 2, Name = "Province2", CountryId = countryId }
        };
            _countriesRepositoryMock.Setup(repo => repo.GetProvincesByCountryId(countryId)).ReturnsAsync(provinces);

            // Act
            var result = await _countryService.GetProvinces(countryId);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("ProvinceDTO", result[0].GetType().Name);
            Assert.AreEqual("Province1", result[0].Name);
            Assert.AreEqual("Province2", result[1].Name);
            Assert.AreEqual(countryId, result[0].CountryId);
            Assert.AreEqual(countryId, result[1].CountryId);
        }
    }
}
