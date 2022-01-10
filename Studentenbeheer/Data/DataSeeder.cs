using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Studentenbeheer.Areas.Identity.Data;
using Studentenbeheer.Models;

namespace Studentenbeheer.Data
{
    public class DataSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService
                                                              <DbContextOptions<ApplicationDbContext>>()))
            {
                context.Database.EnsureCreated();

                ApplicationUser user = null;
                ApplicationUser user2 = null;
                ApplicationUser user3 = null;
                ApplicationUser user4 = null;
                ApplicationUser user5 = null;


                if (!context.Users.Any())
                {
                    user = new ApplicationUser
                    {
                        FirstName = "Julien",
                        LastName = "Lalleman",
                        UserName = "Julien",
                        Email = "julien.lalleman@student.be",
                        EmailConfirmed = true,
                    };

                    user2 = new ApplicationUser
                    {
                        FirstName = "Axel",
                        LastName = "Vankeerberghen",
                        UserName = "Axel",
                        Email = "axel.vankeerberghen@student.be",
                        EmailConfirmed = true,
                    };

                    user3 = new ApplicationUser
                    {
                        FirstName = "Zoë",
                        LastName = "Haller",
                        UserName = "Zoë",
                        Email = "zoë.haller@docent.be",
                        EmailConfirmed = true,
                    };

                    user4 = new ApplicationUser
                    {
                        FirstName = "Jozef",
                        LastName = "Kowalski",
                        UserName = "Jozef",
                        Email = "jozef.kowalski@docent.be",
                        EmailConfirmed = true,
                    };

                    user5 = new ApplicationUser
                    {
                        FirstName = "Admin",
                        LastName = "Admin",
                        UserName = "Admin",
                        Email = "admin.admin@beheerder.be",
                        EmailConfirmed = true,
                    };
                    userManager.CreateAsync(user, "Student@12345");
                    userManager.CreateAsync(user2, "Student@12345");
                    userManager.CreateAsync(user3, "Docent@12345");
                    userManager.CreateAsync(user4, "Docent@12345");
                    userManager.CreateAsync(user5, "Beheerder@12345");



                }


                if (!context.Roles.Any())
                    { 

                    context.Roles.AddRange(

                      new IdentityRole { Id = "Student", Name = "Student", NormalizedName = "STUDENT" },
                      new IdentityRole { Id = "Docent", Name = "Docent", NormalizedName = "DOCENT" },
                      new IdentityRole { Id = "Beheerder", Name = "Beheerder", NormalizedName = "BEHEERDER" }
                      );
                     context.SaveChanges();
                }

                if (!context.Gender.Any())
                {
                    context.Gender.AddRange(
                        new Gender { ID = 'M', Name = "Man" },
                        new Gender { ID = 'V', Name = "Vrouw" },
                        new Gender { ID = 'O', Name = "Onbekend" }
                        );
                    context.SaveChanges();
                }

                if (!context.Student.Any())
                {
                    context.Student.AddRange(
                        new Student { FirstName = user.FirstName, LastName = user.LastName, DateOfBirth = new DateTime(1999, 7, 15), GenderID = 'M', ApplicationUserId = user.Id, Deleted = DateTime.MaxValue },
                        new Student { FirstName = user2.FirstName, LastName = user2.LastName, DateOfBirth = new DateTime(1998, 1, 4), GenderID = 'M', ApplicationUserId = user2.Id, Deleted = DateTime.MaxValue }
                        );
                    context.SaveChanges();
                }
                if (!context.Docent.Any())
                {
                    context.Docent.AddRange(
                        new Docent { FirstName = user3.FirstName, LastName = user3.LastName, DateOfBirth = new DateTime(1998, 1, 4), GeslachtId = 'V', ApplicationUserId = user3.Id },
                        new Docent { FirstName = user4.FirstName, LastName = user4.LastName, DateOfBirth = new DateTime(1970, 5, 20), GeslachtId = 'O', ApplicationUserId = user4.Id }
                        );
                    context.SaveChanges();
                }

                if (!context.DocentModule.Any())
                {
                    context.DocentModule.AddRange(
                        new DocentModule { DocentId = 1, ModuleId = 1 },
                        new DocentModule { DocentId = 2, ModuleId = 2 }

                        );
                    context.SaveChanges();
                }
                if (!context.Module.Any())
                {
                    context.Module.AddRange(
                        new Module { Name = "Wiskunde", Description = "Wiskundige structuren worden met strikte logische redeneringen opgebouwd. Wiskundige beweringen waarvan de juistheid is aangetoond heten stellingen"/*, Deleted = DateTime.Now*/ },
                        new Module { Name = "Aardrijkskunde", Description = "Het aardoppervlak, het in kaart brengen van vormen van bijvoorbeeld cultuur, het plantenleven en de dierenwereld"/*, Deleted = DateTime.Now*/ }
                        );
                    context.SaveChanges();
                }
                if (!context.Inschrijvingen.Any())
                {
                    context.Inschrijvingen.AddRange(
                        new Inschrijvingen { ModuleIds = 1, StudentIds = 3, RegistrationDate = DateTime.Now, TakenOn = DateTime.Now, Result = 10 },
                        new Inschrijvingen { ModuleIds = 2, StudentIds = 4, RegistrationDate = DateTime.Now, TakenOn = DateTime.Now, Result = 10 }
                        );
                    context.SaveChanges();
                }
                if (user != null)
                {
                    context.UserRoles.AddRange(
                        new IdentityUserRole<string> { RoleId = "Beheerder", UserId = user5.Id },
                        new IdentityUserRole<string> { RoleId = "Docent", UserId = user3.Id },
                        new IdentityUserRole<string> { RoleId = "Docent", UserId = user4.Id },
                        new IdentityUserRole<string> { RoleId = "Student", UserId = user2.Id },
                        new IdentityUserRole<string> { RoleId = "Student", UserId = user.Id }
                        );
                    context.SaveChanges();
                }
            }
        }

    }
}
