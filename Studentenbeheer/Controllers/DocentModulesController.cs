#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Studentenbeheer.Data;
using Studentenbeheer.Models;

namespace Studentenbeheer.Controllers
{
    public class DocentModulesController : ApplicationController
    {

        public DocentModulesController(ApplicationDbContext context, HttpContextAccessor httpContextAccessor, ILogger<ApplicationController> logger) : base(context, httpContextAccessor, logger)
        {
           
        }

        // GET: DocentModules
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DocentModule.Include(d => d.Docent).Include(d => d.Module);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DocentModules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docentModule = await _context.DocentModule
                .Include(d => d.Docent)
                .Include(d => d.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docentModule == null)
            {
                return NotFound();
            }

            return View(docentModule);
        }

        // GET: DocentModules/Create
        public IActionResult Create()
        {
            ViewData["DocentId"] = new SelectList(_context.Docent, "Id", "FirstName");
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Description");
            return View();
        }

        // POST: DocentModules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ModuleId,DocentId")] DocentModule docentModule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(docentModule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DocentId"] = new SelectList(_context.Docent, "Id", "FirstName", docentModule.DocentId);
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Description", docentModule.ModuleId);
            return View(docentModule);
        }

        // GET: DocentModules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docentModule = await _context.DocentModule.FindAsync(id);
            if (docentModule == null)
            {
                return NotFound();
            }
            ViewData["DocentId"] = new SelectList(_context.Docent, "Id", "FirstName", docentModule.DocentId);
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Description", docentModule.ModuleId);
            return View(docentModule);
        }

        // POST: DocentModules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModuleId,DocentId")] DocentModule docentModule)
        {
            if (id != docentModule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(docentModule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocentModuleExists(docentModule.Id))
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
            ViewData["DocentId"] = new SelectList(_context.Docent, "Id", "FirstName", docentModule.DocentId);
            ViewData["ModuleId"] = new SelectList(_context.Module, "Id", "Description", docentModule.ModuleId);
            return View(docentModule);
        }

        // GET: DocentModules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docentModule = await _context.DocentModule
                .Include(d => d.Docent)
                .Include(d => d.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docentModule == null)
            {
                return NotFound();
            }

            return View(docentModule);
        }

        // POST: DocentModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var docentModule = await _context.DocentModule.FindAsync(id);
            _context.DocentModule.Remove(docentModule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocentModuleExists(int id)
        {
            return _context.DocentModule.Any(e => e.Id == id);
        }
    }
}
