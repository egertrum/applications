using System.Collections.Generic;

namespace WebApp.ViewModels.Persons
{
    public class PersonIndexViewModel
    {
        public IEnumerable<PublicApi.DTO.v1.Person> Persons { get; set; } = default!;
        
        public string? errorMessage { get; set; } 
    }
}