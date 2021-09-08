using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base.Identity;
using Microsoft.AspNetCore.Identity;

namespace Domain.App.Identity
{
    public class AppUser : BaseAppUser<Guid>
    {
        public ICollection<UserAnswer>? UserAnswers { get; set; }
    }
    
}