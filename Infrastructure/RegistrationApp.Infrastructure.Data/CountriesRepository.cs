using Microsoft.EntityFrameworkCore;
using RegistrationApp.Core.Interfaces;
using RegistrationApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationApp.Infrastructure.Data
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly RegistrationAppDbContext _registrationAppDbContext;

        public CountriesRepository(RegistrationAppDbContext registrationAppDbContext)
        {
            _registrationAppDbContext = registrationAppDbContext;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            return await _registrationAppDbContext.Countries.ToListAsync();
        }

        public async Task<List<Province>> GetProvincesByCountryId(int countryId)
        {
            return await _registrationAppDbContext.Provinces.Where(p => p.CountryId == countryId).ToListAsync();
        }
    }
}
