using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BLL.App.DTO.Identity;
using Microsoft.AspNetCore.Identity;

namespace PublicApi.DTO.v1
{
    public class AppUser : IdentityUser<Guid>
    {
        [StringLength(128, MinimumLength = 1)]
        public string Firstname { get; set; } = default!;
        [StringLength(128, MinimumLength = 1)]
        public string Lastname { get; set; } = default!;

        [StringLength(128, MinimumLength = 1)]
        [EmailAddress]
        public override string Email { get; set; } = default!;

        [StringLength(128, MinimumLength = 1)]

        public override string UserName { get; set; } = default!;

        public string IdentificationCode { get; set; } = default!;
        
        public ICollection<Report>? Reports { get; set; }
        
        public ICollection<string>? AppRoles { get; set; }

        public string FullName => Firstname + " " + Lastname;
        public string FullNameEmail => FullName + " (" + Email + ")";
    }
}