using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Studentenbeheer.Areas.Identity.Data;

// Add profile data for application users by adding properties to the StudentenbeheerUser class
public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

public class ApplicationModerator
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool Lockout { get; set; }
    public bool Student { get; set; }
    public bool Docent { get; set; }
    public bool Beheerder { get; set; }

}



