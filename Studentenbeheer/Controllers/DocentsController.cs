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
using Studentenbeheer.Models;

namespace Studentenbeheer.Controllers
{
    [Authorize(Roles = "Beheerder")]

    public class DocentsController : ApplicationController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DocentsController(UserManager<ApplicationUser> userManager,
                                    ApplicationDbContext context,
                                    IHttpContextAccessor httpContextAccessor,
                                    ILogger<ApplicationController> logger) : base(context, httpContextAccessor, logger)
        {
            _userManager = userManager;
        }

        // GET: Docents
        public async Task<IActionResult> Index()
        {
            ViewData["genderId"] = new SelectList(_context.Gender.ToList(), "ID", "Name");
            return View(await _context.Docent.ToListAsync());
        }

        // GET: Docents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docent = await _context.Docent
                //.Include(d => d.ApplicationUser)
                //.Include(d => d.Geslacht)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docent == null)
            {
                return NotFound();
            }

            return View(docent);
        }

        // GET: Docents/Create
        public IActionResult Create()
        {
            //ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["GeslachtId"] = new SelectList(_context.Gender, "ID", "Name");
            return View();
        }

        // POST: Docents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,DateOfBirth,Deleted,GeslachtId")] Docent docent)
        {
            if (ModelState.IsValid)
            {
                var user = Activator.CreateInstance<ApplicationUser>();
                user.FirstName = docent.FirstName;
                user.LastName = docent.LastName;
                user.UserName = docent.FirstName;
                user.Email = docent.FirstName + "." + docent.LastName + "@docent.be";
                user.EmailConfirmed = true;
                await _userManager.CreateAsync(user, "Student@12345");


                docent.ApplicationUserId = user.Id;
                _context.Add(docent);

                await _context.SaveChangesAsync();

                await _userManager.AddToRoleAsync(user, "Docent");
                return RedirectToAction(nameof(Index));
            }
            ViewData["GeslachtId"] = new SelectList(_context.Gender, "ID", "Name", docent.GeslachtId);
            return View(docent);
        }
            // GET: Docents/Edit/5
            public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docent = await _context.Docent.FindAsync(id);
            if (docent == null)
            {
                return NotFound();
            }
            //ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", docent.ApplicationUserId);
            //ViewData["GeslachtId"] = new SelectList(_context.Gender, "ID", "Name", docent.GeslachtId);
            return View(docent);
        }

        // POST: Docents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,DateOfBirth,GeslachtId")] Docent docent)
        {
            if (id != docent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(docent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocentExists(docent.Id))
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
            //ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", docent.ApplicationUserId);
            ViewData["GeslachtId"] = new SelectList(_context.Gender, "ID", "Name", docent.GeslachtId);
            return View(docent);
        }

        // GET: Docents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docent = await _context.Docent
                //.Include(d => d.ApplicationUser)
                //.Include(d => d.Geslacht)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docent == null)
            {
                return NotFound();
            }

            return View(docent);
        }

        // POST: Docents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var docent = await _context.Docent.FindAsync(id);
            docent.Deleted = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocentExists(int id)
        {
            //return _context.Docent.Any(e => e.Id == id);
            throw new NotImplementedException();

        }
    }
}
