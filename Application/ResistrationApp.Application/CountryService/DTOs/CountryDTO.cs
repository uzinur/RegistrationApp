using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResistrationApp.Application.CountryService.DTOs
{
    public class CountryDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
