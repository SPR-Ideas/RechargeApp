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
    public class RechargeTablesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RechargeTablesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RechargeTables
        public async Task<IActionResult> Index()
        {
              return _context.rechargeTables != null ? 
                          View(await _context.rechargeTables.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.rechargeTables'  is null.");
        }

        // GET: RechargeTables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.rechargeTables == null)
            {
                return NotFound();
            }

            var rechargeTable = await _context.rechargeTables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rechargeTable == null)
            {
                return NotFound();
            }

            return View(rechargeTable);
        }

        // GET: RechargeTables/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RechargeTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,userId,planId,timestamp")] RechargeTable rechargeTable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rechargeTable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rechargeTable);
        }

        // GET: RechargeTables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.rechargeTables == null)
            {
                return NotFound();
            }

            var rechargeTable = await _context.rechargeTables.FindAsync(id);
            if (rechargeTable == null)
            {
                return NotFound();
            }
            return View(rechargeTable);
        }

        // POST: RechargeTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,userId,planId,timestamp")] RechargeTable rechargeTable)
        {
            if (id != rechargeTable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rechargeTable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RechargeTableExists(rechargeTable.Id))
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
            return View(rechargeTable);
        }

        // GET: RechargeTables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.rechargeTables == null)
            {
                return NotFound();
            }

            var rechargeTable = await _context.rechargeTables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rechargeTable == null)
            {
                return NotFound();
            }

            return View(rechargeTable);
        }

        // POST: RechargeTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.rechargeTables == null)
            {
                return Problem("Entity set 'ApplicationDbContext.rechargeTables'  is null.");
            }
            var rechargeTable = await _context.rechargeTables.FindAsync(id);
            if (rechargeTable != null)
            {
                _context.rechargeTables.Remove(rechargeTable);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RechargeTableExists(int id)
        {
          return (_context.rechargeTables?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
