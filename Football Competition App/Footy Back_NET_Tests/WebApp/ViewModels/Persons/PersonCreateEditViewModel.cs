using System;
using Domain.App;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.Persons
{
    public class PersonCreateEditViewModel
    {
        public PublicApi.DTO.v1.Person Person { get; set; } = default!;

        public SelectList? CountrySelectList { get; set; }
    }
}