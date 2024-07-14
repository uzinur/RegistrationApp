using ResistrationApp.Application.CountryService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResistrationApp.Application.CountryService
{
    public interface ICountryService
    {
        Task<List<CountryDTO>> GetCountries();
        Task<List<ProvinceDTO>> GetProvinces(int countryId);
    }
}
