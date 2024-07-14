using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResistrationApp.Application.CountryService;
using ResistrationApp.Application.CountryService.DTOs;

namespace RegistrationApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<CountryService> _logger;

        public CountriesController(ICountryService countryService, ILogger<CountryService> logger)
        {
            _countryService = countryService;
            _logger = logger;
        }

        [HttpGet("countries")]
        public async Task<ActionResult<CountryDTO>> GetCountries()
        {
            try
            {
                _logger.LogInformation("Attempting to retrieve all countries");

                var countries = await _countryService.GetCountries();

                _logger.LogInformation("Countries retrieved successfully. Count = {count}", countries.Count);

                return Ok(countries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving countries");
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("provinces")]
        public async Task<ActionResult<ProvinceDTO>> GetProvinces(int countryId)
        {
            try
            {
                _logger.LogInformation("Attempting to retrieve provinces by countryId = {countryId}", countryId);

                var provinces = await _countryService.GetProvinces(countryId);

                _logger.LogInformation("Provinces retrieved successfully by countryId = {countryId}. Count = {count}", countryId, provinces.Count);

                return Ok(provinces);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving provinces by countryId = {countryId}", countryId);
                return BadRequest(ex.Message);
            }

        }
    }
}
