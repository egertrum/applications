using System;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Areas.Identity.IdentityErrorDescriber
{
    public class LocalizedIdentityErrorDescriber : Microsoft.AspNetCore.Identity.IdentityErrorDescriber
{
    public override IdentityError DefaultError() { return new IdentityError { Code = nameof(DefaultError), Description = Resources.Areas.Identity.IdentityErrorDescriber.LocalizedIdentityErrorDescriber.DefaultError }; }
    public override IdentityError ConcurrencyFailure() { return new IdentityError { Code = nameof(ConcurrencyFailure), Description = Resources.Areas.Identity.IdentityErrorDescriber.LocalizedIdentityErrorDescriber.ConcurrencyFailure }; }
    public override IdentityError PasswordMismatch() { return new IdentityError { Code = nameof(PasswordMismatch), Description = Resources.Areas.Identity.IdentityErrorDescriber.LocalizedIdentityErrorDescriber.PasswordMismatch }; }
    public override IdentityError InvalidToken() { return new IdentityError { Code = nameof(InvalidToken), Description = Resources.Areas.Identity.IdentityErrorDescriber.LocalizedIdentityErrorDescriber.InvalidToken }; }
    public override IdentityError LoginAlreadyAssociated() { return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = Resources.Areas.Identity.IdentityErrorDescriber.LocalizedIdentityErrorDescriber.LoginAlreadyAssociated }; }
    public override IdentityError InvalidUserName(string userName) { return new IdentityError { Code = nameof(InvalidUserName), Description = string.Format(Resources.Areas.Identity.IdentityErrorDescriber.LocalizedIdentityErrorDescriber.InvalidUserName, userName) }; }
    public override IdentityError InvalidEmail(string email) { return new IdentityError { Code = nameof(InvalidEmail), Description = string.Format(Resources.Areas.Identity.IdentityErrorDescriber.LocalizedIdentityErrorDescriber.InvalidEmail, email)  }; }
    public override IdentityError DuplicateUserName(string userName) { return new IdentityError { Code = nameof(DuplicateUserName), Description = string.Format(Resources.Areas.Identity.IdentityErrorDescriber.LocalizedIdentityErrorDescriber.DuplicateUserName, userName)  }; }
    public override IdentityError DuplicateEmail(string email) { return new IdentityError { Code = nameof(DuplicateEmail), Description = string.Format(Resources.Areas.Identity.IdentityErrorDescriber.LocalizedIdentityErrorDescriber.DuplicateEmail, email)  }; }
    public override IdentityError InvalidRoleName(string role) { return new IdentityError { Code = nameof(InvalidRoleName), Description = string.Format(Resources.Areas.Identity.IdentityErrorDescriber.LocalizedIdentityErrorDescriber.InvalidRoleName, role)  }; }
    public override IdentityError DuplicateRoleName(string role) { return new IdentityError { Code = nameof(DuplicateRoleName), Description = string.Format(Resources.Areas.Identity.IdentityErrorDescriber.LocalizedIdentityErrorDescriber.DuplicateRoleName, role)  }; }
    public override IdentityError UserAlreadyHasPassword() { return new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = Resources.Areas.Identity.IdentityErrorDescriber.LocalizedIdentityErrorDescriber.UserAlreadyHasPassword }; }
    public override IdentityError UserLockoutNotEnabled() { return new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = Resources.Areas.Identity.IdentityErrorDescriber.LocalizedIdentityErrorDescriber.UserLockoutNotEnabled }; }
    public override IdentityError UserAlreadyInRole(string role) { return new IdentityError { Code = nameof(UserAlreadyInRole), Description = string.Format(Resources.Areas.Identity.IdentityErrorDescriber.LocalizedIdentityErrorDescriber.UserAlreadyInRole, role)  }; }
    public override IdentityError UserNotInRole(string role) { return new IdentityError { Code = nameof(UserNotInRole), Description = string.Format(Resources.Areas.Identity.IdentityErrorDescriber.LocalizedIdentityErrorDescriber.UserNotInRole, role)  }; }
    public override IdentityError PasswordTooShort(int length) { return new IdentityError { Code = nameof(PasswordTooShort), Description = string.Format(Resources.Areas.Identity.IdentityErrorDescriber.LocalizedIdentityErrorDescriber.PasswordTooShort, length)  }; }
    public override IdentityError PasswordRequiresNonAlphanumeric() { return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = Resources.Areas.Identity.IdentityErrorDescriber.LocalizedIdentityErrorDescriber.PasswordRequiresNonAlphanumeric }; }
    public override IdentityError PasswordRequiresDigit() { return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = Resources.Areas.Identity.IdentityErrorDescriber.LocalizedIdentityErrorDescriber.PasswordRequiresDigit }; }
    public override IdentityError PasswordRequiresLower() { return new IdentityError { Code = nameof(PasswordRequiresLower), Description = Resources.Areas.Identity.IdentityErrorDescriber.LocalizedIdentityErrorDescriber.PasswordRequiresLower }; }
    public override IdentityError PasswordRequiresUpper() { return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = Resources.Areas.Identity.IdentityErrorDescriber.LocalizedIdentityErrorDescriber.PasswordRequiresUpper }; }
}
}