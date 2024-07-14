using Microsoft.Extensions.Logging;
using RegistrationApp.Core.Interfaces;
using ResistrationApp.Application.CountryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ResistrationApp.Application.CountryService
{
    public class CountryService : ICountryService
    {
        private readonly ICountriesRepository _countriesRepository;
        private readonly ILogger<CountryService> _logger;

        public CountryService(ICountriesRepository countriesRepository, ILogger<CountryService> logger)
        {
            _countriesRepository = countriesRepository;
            _logger = logger;
        }

        public async Task<List<CountryDTO>> GetCountries()
        {
            var countries = await _countriesRepository.GetAllCountries();

            var countryDTOs = countries.Select(c => new CountryDTO { Name = c.Name, Id = c.Id }).ToList();

            return countryDTOs;
        }

        public async Task<List<ProvinceDTO>> GetProvinces(int countryId)
        {
            var provinces = await _countriesRepository.GetProvincesByCountryId(countryId);

            var provinceDTOs = provinces.Select(p => new ProvinceDTO { Name = p.Name, Id = p.Id, CountryId = p.CountryId }).ToList();

            return provinceDTOs;
        }
    }
}
