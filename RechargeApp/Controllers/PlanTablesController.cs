using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RechargeApp.Data;
using RechargeApp.Models;

namespace RechargeApp.Controllers
{
    public class PlanTablesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlanTablesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PlanTables
        public async Task<IActionResult> Index()
        {
              return _context.planTables != null ? 
                          View(await _context.planTables.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.planTables'  is null.");
        }

        // GET: PlanTables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.planTables == null)
            {
                return NotFound();
            }

            var planTable = await _context.planTables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (planTable == null)
            {
                return NotFound();
            }

            return View(planTable);
        }

        // GET: PlanTables/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlanTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,amount,validity,description")] PlanTable planTable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(planTable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(planTable);
        }

        // GET: PlanTables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.planTables == null)
            {
                return NotFound();
            }

            var planTable = await _context.planTables.FindAsync(id);
            if (planTable == null)
            {
                return NotFound();
            }
            return View(planTable);
        }

        // POST: PlanTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,amount,validity,description")] PlanTable planTable)
        {
            if (id != planTable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(planTable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanTableExists(planTable.Id))
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
            return View(planTable);
        }

        // GET: PlanTables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.planTables == null)
            {
                return NotFound();
            }

            var planTable = await _context.planTables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (planTable == null)
            {
                return NotFound();
            }

            return View(planTable);
        }

        // POST: PlanTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.planTables == null)
            {
                return Problem("Entity set 'ApplicationDbContext.planTables'  is null.");
            }
            var planTable = await _context.planTables.FindAsync(id);
            if (planTable != null)
            {
                _context.planTables.Remove(planTable);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanTableExists(int id)
        {
          return (_context.planTables?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
