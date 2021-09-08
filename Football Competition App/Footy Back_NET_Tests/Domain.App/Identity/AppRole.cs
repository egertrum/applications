using System;
using System.ComponentModel.DataAnnotations;
using Domain.Base.Identity;
using Microsoft.AspNetCore.Identity;

namespace Domain.App.Identity
{
    public class AppRole : BaseAppRole<Guid>
    {
    }
}