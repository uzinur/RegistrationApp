using RegistrationApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationApp.Core.Interfaces
{
    public interface ICountriesRepository
    {
        Task<List<Country>> GetAllCountries();

        Task<List<Province>> GetProvincesByCountryId(int countryId);
    }
}
