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
using Studentenbeheer.Models;

namespace Studentenbeheer.Controllers
{
    [Authorize(Roles = "Beheerder, Docent")]
    public class StudentsController : ApplicationController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<ApplicationController> logger)
            : base(context, httpContextAccessor, logger)
        {
            _userManager = userManager;
        }

        // GET: Students
        //[AllowAnonymous]
        public async Task<IActionResult> Index(string searchField = "", string orderBy = "", char selectItem = ' ')
        {
            var students = from s in _context.Student where s.Deleted > DateTime.Now orderby s.FirstName select s;
            var user = _context.Users.FirstOrDefault(e => e.UserName == User.Identity.Name);
            var roles = _context.UserRoles.Where(r => r.UserId == user.Id);
            var idertityroles = User.IsInRole("Guest");

            if (selectItem != 0)
            students = from g in _context.Student.Include(s => s.Gender)
                            where g.GenderID == selectItem
                           orderby g.FirstName, g.LastName
                           select g;

            if (!string.IsNullOrEmpty(searchField))
                students = from g in students
                           where g.FirstName.Contains(searchField) || g.LastName.Contains(searchField)
                           orderby g.FirstName, g.LastName
                           select g;
            //var studentsContext = _context.Student.Include(s => s.Gender);

            // Lijst alle message op.  We gebruiken Linq
            //var filteredMessages = from m in _context.Student select m;

            //if (!string.IsNullOrEmpty(voornaam))
            //    filteredMessages = from m in filteredMessages 
            //                       where m.FirstName.Contains(voornaam) 
            //                       select m;

            if (selectItem != ' ')
            {
                students = from g in students
                           where g.GenderID == selectItem
                           orderby g.ID
                           select g;
            }

            ViewData["VoornaamField"] = orderBy == "FirstName" ? "Vn_Desc" : "FirstName";
            ViewData["AchternaamField"] = orderBy == "LastName" ? "An_Desc" : "LastName";
            ViewData["GenderField"] = orderBy == "Gender" ? "Gender_Desc" : "Gender";
            ViewData["genderId"] = new SelectList(_context.Gender.ToList(), "ID", "Name");



            switch (orderBy)
            {
                case "FirstName":
                    students = students.OrderBy(m => m.FirstName);
                    break;
                case "Vn_Desc":
                    students = students.OrderByDescending(m => m.FirstName);
                    break;
                case "LastName":
                    students = students.OrderBy(m => m.LastName);
                    break;
                case "An_Desc":
                    students = students.OrderByDescending(m => m.LastName);
                    break;
                case "Gender_Desc":
                    students = students.OrderByDescending(m => m.Gender.ID);
                    break;
                default:
                    students = students.OrderBy(m => m.ID);
                    break;
            }

            //lijst van studenten
            IQueryable<Student> studentsToSelect = from s in _context.Student orderby s.ID select s;

            StudentIndexViewModel studentIndexViewModel = new StudentIndexViewModel()
            {
                SearchField = searchField,
                Students = await students.Include(s => s.Gender).ToListAsync(),
                SelectedItem = selectItem,
                GendersToSelect = new SelectList(await _context.Gender.ToListAsync(), "ID", "Name", selectItem)
            };

            return View(studentIndexViewModel);

            //return View(await students.ToListAsync());

        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Gender)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [Authorize(Roles = "Beheerder")]
        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["GenderID"] = new SelectList(_context.Gender, "ID", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,DateOfBirth,GenderID")] Student student)
        {
            if (ModelState.IsValid)
            {
                var user = Activator.CreateInstance<ApplicationUser>();
                user.FirstName = student.FirstName;
                user.LastName = student.LastName;
                user.UserName = student.FirstName;
                user.Email = student.FirstName + "." + student.LastName + "@docent.be";
                user.EmailConfirmed = true;
                await _userManager.CreateAsync(user, "Student@12345");


                student.ApplicationUserId = user.Id;
                _context.Add(student);

                await _context.SaveChangesAsync();

                await _userManager.AddToRoleAsync(user, "Student");
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenderID"] = new SelectList(_context.Gender, "ID", "Name", student.GenderID);
            return View(student);
        }


        [Authorize(Roles = "Beheerder")]
        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["GenderID"] = new SelectList(_context.Gender, "ID", "Name", student.GenderID);
            return View(student);
        }

        [Authorize(Roles = "Beheerder")]
        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,DateOfBirth,GenderID")] Student student)
        {
            if (id != student.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenderID"] = new SelectList(_context.Gender, "ID", "Name", student.GenderID);
            return View(student);
        }

        [Authorize(Roles = "Beheerder")]
        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Gender)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            student.Deleted = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.ID == id);
        }
    }
}
