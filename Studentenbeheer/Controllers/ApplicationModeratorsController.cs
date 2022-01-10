#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Studentenbeheer.Areas.Identity.Data;
using Studentenbeheer.Data;

namespace Studentenbeheer.Controllers
{
    [Authorize(Roles = "Beheerder")]

    public class ApplicationModeratorsController : ApplicationController
    {

        public ApplicationModeratorsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<ApplicationController> logger) : base(context, httpContextAccessor, logger)
        {
        }
        public IActionResult Index(string userName, string name, string email)
        {
            if (userName == null) userName = "";
            if (name == null) name = "";
            if (email == null) email = "";
            List<ApplicationUser> users =
                _context.Users.ToList()
                .Where(u => (userName == "" || u.UserName.Contains(userName))
                         && (name == "" || (u.FirstName.Contains(name) || u.LastName.Contains(name)))
                         && (email == "" || u.Email.Contains(email)))
                .OrderBy(u => u.FirstName + " " + u.LastName)
                .ToList();
            List<ApplicationModerator> applicationModerator = new List<ApplicationModerator>();
            foreach (var user in users)
            {
                applicationModerator.Add(new ApplicationModerator
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Lockout = user.LockoutEnd != null,
                    PhoneNumber = user.PhoneNumber,
                    Student = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Student").Count() > 0,
                    Docent = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Docent").Count() > 0,
                    Beheerder = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Beheerder").Count() > 0
                });

            }
            ViewData["userName"] = userName;
            ViewData["name"] = name;
            ViewData["email"] = email;
            return View(applicationModerator);
        }

        public async Task<ActionResult> Locking(string id)
        {
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user.LockoutEnd != null)
                user.LockoutEnd = null;
            else
                user.LockoutEnd = new DateTimeOffset(DateTime.Now + new TimeSpan(7, 0, 0, 0));
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult Roles(string id)
        {
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.Id == id);
            ApplicationModerator model = new ApplicationModerator
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Lockout = user.LockoutEnd != null,
                PhoneNumber = user.PhoneNumber,
                Student = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Student").Count() > 0,
                Docent = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Docent").Count() > 0,
                Beheerder = _context.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == "Beheerder").Count() > 0
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Roles([Bind("Id, UserName, Voornaam, AchterNaam, Student, Docent, Beheerder")] ApplicationModerator model)
        {
            List<IdentityUserRole<string>> roles = _context.UserRoles.Where(ur => ur.UserId == model.Id).ToList();
            foreach (IdentityUserRole<string> role in roles)
            {
                _context.Remove(role);
            }
            if (model.Student) _context.Add(new IdentityUserRole<string> { RoleId = "Student", UserId = model.Id });
            if (model.Docent) _context.Add(new IdentityUserRole<string> { RoleId = "Docent", UserId = model.Id });
            if (model.Beheerder) _context.Add(new IdentityUserRole<string> { RoleId = "Beheerder", UserId = model.Id });
            await _context.SaveChangesAsync();
            ; return RedirectToAction("Index");
        }
    }

}

