﻿using System;
using System.Collections.Generic;
using Domain.Base.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.App.Identity
{
    public class AppUser : BaseAppUser<Guid> 
    {
        [StringLength(128, MinimumLength = 1)]
        public string Firstname { get; set; } = default!;
        [StringLength(128, MinimumLength = 1)]
        public string Lastname { get; set; } = default!;
        
        public string IdentificationCode { get; set; } = default!;

        public string FullName => Firstname + " " + Lastname;
        public string FullNameEmail => FullName + " (" + Email + ")";
        
        public ICollection<Registration>? Registrations { get; set; }
        public ICollection<Person>? Persons { get; set; }
        public ICollection<Team>? Teams { get; set; }
        public ICollection<Report>? Reports { get; set; }
    }
    
}