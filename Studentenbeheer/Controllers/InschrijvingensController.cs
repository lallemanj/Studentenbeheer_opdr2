using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Studentenbeheer.Data;
using Studentenbeheer.Models;

namespace Studentenbeheer.Controllers
{
    [Authorize(Roles = "Student,Beheerder,Docent")]

    public class InschrijvingensController : ApplicationController
    {

        public InschrijvingensController(ApplicationDbContext context, HttpContextAccessor httpContextAccessor, ILogger<ApplicationController> logger) : base(context, httpContextAccessor, logger)
        {
        }

        // GET: Inschrijvingens
        public async Task<IActionResult> Index()
        {
            var Inschrijvingens = _context.Inschrijvingen.Include(l => l.Module).Include(l => l.Student);
            if (User.IsInRole("Beheerder"))
            {
                return View(await Inschrijvingens.ToListAsync());
            }
            if (User.IsInRole("Docent"))
            {
                var ModuleId = _context.DocentModule.Include(dm => dm.Module).Include(dm => dm.Docent)
                    .Where(dm => dm.Docent.ApplicationUserId == _user.Id)
                    .Select(dm => dm.Module.Id).ToList();
                var Inschrijvingens2 = _context.Inschrijvingen.Include(i => i.Module).Include(i => i.Student)
                    .Where(i => ModuleId.Contains(i.Module.Id));
                return View(await Inschrijvingens2.ToListAsync());
            }
            if (User.IsInRole("Student"))
            {
                var Inschrijvingens2 = _context.Inschrijvingen.Include(i => i.Module).Include(i => i.Student)
                    .Where(i => i.Student.ApplicationUserId == _user.Id);
                return View(await Inschrijvingens2.ToListAsync());

            }
            return View(await Inschrijvingens.ToListAsync());
        }

        // GET: Inschrijvingens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inschrijvingen = await _context.Inschrijvingen
                .Include(e => e.Module)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inschrijvingen == null)
            {
                return NotFound();
            }

            return View(inschrijvingen);
        }

        [Authorize(Roles = "Beheerder")]

        // GET: Inschrijvingens/Create
        public IActionResult Create(int? id, int? num)
        {

            var student1 = _context.Student.Where(s => s.Deleted > DateTime.Now);
            var module = _context.Module.Where(s => s.Deleted > DateTime.Now);

            ViewData["ModulenIds"] = new MultiSelectList(module, "Id", "Name");
            ViewData["StudentenIds"] = new SelectList(student1, "ID", "FirstName");

            if (num == 1)
            {
                module = _context.Module.Where(s => s.Id == id);
                ViewData["ModulenIds"] = new SelectList(module, "Id", "Name");
                //ViewData["StudentenIds"] = new SelectList(_context.Student, "ID", "FirstName");
            }

            if (num == 2)
            {
                student1 = _context.Student.Where(s => s.ID == id);
                //ViewData["ModulenIds"] = new SelectList(_context.Set<Module>(), "Id", "Name");
                ViewData["StudentenIds"] = new SelectList(student1, "ID", "FirstName");
            }


            //Inschrijvingen inschrijvingen = new Inschrijvingen { RegistrationDate = DateTime.Now };

            return View();
        }

        [Authorize(Roles = "Beheerder")]

        // POST: Inschrijvingens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RegistrationDate,TakenOn,Result, ModuleIds, StudentIds")] Inschrijvingen inschrijvingen)
        {

            if (ModelState.IsValid)
            {
                //if (inschrijvingen.Student == null)
                //    inschrijvingen.Student = new List<Student>();
                //foreach (int id in inschrijvingen.StudentIds)
                //    inschrijvingen.Student.Add(_context.Student.FirstOrDefault(c => c.ID == id));
                //if (inschrijvingen.Module == null)
                //    inschrijvingen.Module = new List<Module>();
                //foreach (int id in inschrijvingen.ModuleIds)
                //    inschrijvingen.Module.Add(_context.Module.FirstOrDefault(c => c.Id == id));
                _context.Add(inschrijvingen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ModulenIds"] = new MultiSelectList(_context.Module.OrderBy(c => c.Name), "Id", "Name");
            ViewData["ModulenIds"] = new SelectList(_context.Module, "Id", "Name", inschrijvingen.ModuleIds);
            ViewData["StudentenIds"] = new SelectList(_context.Student, "ID", "FirstName", inschrijvingen.StudentIds);
            return View(inschrijvingen);
        }

        [Authorize(Roles = "Beheerder")]

        // GET: Inschrijvingens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inschrijvingen = await _context.Inschrijvingen.FindAsync(id);
            if (inschrijvingen == null)
            {
                return NotFound();
            }
            //inschrijvingen.StudentIds = new List<int>();
            //if (inschrijvingen.Student != null)
            //{
            //    foreach (Student std in inschrijvingen.Student)
            //        inschrijvingen.StudentIds.Add(std.ID);
            //}
            //inschrijvingen.ModuleIds = new List<int>();
            //if (inschrijvingen.Module != null)
            //{
            //    foreach (Module mdl in inschrijvingen.Module)
            //        inschrijvingen.ModuleIds.Add(mdl.Id);
            //}
            ViewData["ModulenIds"] = new SelectList(_context.Module, "Id", "Name", inschrijvingen.ModuleIds);
            ViewData["StudentenIds"] = new SelectList(_context.Student, "ID", "FirstName", inschrijvingen.StudentIds);


            return View(inschrijvingen);
        }

        [Authorize(Roles = "Beheerder")]

        // POST: Inschrijvingens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RegistrationDate,TakenOn,Result,ModuleIds,StudentIds")] Inschrijvingen inschrijvingen)
        {
            if (id != inschrijvingen.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //if (inschrijvingen.Student == null)
                    //{
                    //    inschrijvingen.Student = new List<Student>();
                    //    foreach (int r in inschrijvingen.StudentIds)
                    //        inschrijvingen.Student.Add(_context.Student.FirstOrDefault(c => c.ID == r));
                    //}
                    //if (inschrijvingen.Module == null)
                    //{
                    //    inschrijvingen.Module = new List<Module>();
                    //    foreach (int i in inschrijvingen.ModuleIds)
                    //        inschrijvingen.Module.Add(_context.Module.FirstOrDefault(c => c.Id == i));
                    //}
                    _context.Update(inschrijvingen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InschrijvingenExists(inschrijvingen.Id))
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
            //inschrijvingen.StudentIds = new List<int>();
            //if (inschrijvingen.Student != null)
            //    foreach (Student std in inschrijvingen.Student)
            //        inschrijvingen.StudentIds.Add(std.ID);
            //inschrijvingen.ModuleIds = new List<int>();
            //if (inschrijvingen.Module != null)
            //    foreach (Module mdl in inschrijvingen.Module)
            //        inschrijvingen.ModuleIds.Add(mdl.Id);
            ViewData["ModulenIds"] = new SelectList(_context.Module, "Id", "Name", inschrijvingen.ModuleIds);
            ViewData["StudentenIds"] = new SelectList(_context.Student, "ID", "FirstName", inschrijvingen.StudentIds);


            return View(inschrijvingen);
        }

        [Authorize(Roles = "Beheerder")]


        // GET: Inschrijvingens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inschrijvingen = await _context.Inschrijvingen
                .Include(l => l.Module)
                .Include(l => l.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inschrijvingen == null)
            {
                return NotFound();
            }

            return View(inschrijvingen);
        }

        [Authorize(Roles = "Beheerder")]


        // POST: Inschrijvingens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inschrijvingen = await _context.Inschrijvingen.FindAsync(id);
            _context.Inschrijvingen.Remove(inschrijvingen);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InschrijvingenExists(int id)
        {
            return _context.Inschrijvingen.Any(e => e.Id == id);
        }
    }
}
