using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BirKadınBirErkekTasarımMerkezi.Data;
using BirKadınBirErkekTasarımMerkezi.Models;
using Microsoft.AspNetCore.Authorization;

namespace BirKadınBirErkekTasarımMerkezi.Controllers
{
    [Authorize(Roles = "PATRON")]

    public class KisimsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KisimsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Kisims
        public async Task<IActionResult> Index()
        {
              return _context.kisims != null ? 
                          View(await _context.kisims.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.kisims'  is null.");
        }

        // GET: Kisims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.kisims == null)
            {
                return NotFound();
            }

            var kisim = await _context.kisims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kisim == null)
            {
                return NotFound();
            }

            return View(kisim);
        }

        // GET: Kisims/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kisims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KisimAdi,Kapasitesi")] Kisim kisim)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kisim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kisim);
        }

        // GET: Kisims/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.kisims == null)
            {
                return NotFound();
            }

            var kisim = await _context.kisims.FindAsync(id);
            if (kisim == null)
            {
                return NotFound();
            }
            return View(kisim);
        }

        // POST: Kisims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KisimAdi,Kapasitesi")] Kisim kisim)
        {
            if (id != kisim.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kisim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KisimExists(kisim.Id))
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
            return View(kisim);
        }

        // GET: Kisims/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.kisims == null)
            {
                return NotFound();
            }

            var kisim = await _context.kisims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kisim == null)
            {
                return NotFound();
            }

            return View(kisim);
        }

        // POST: Kisims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.kisims == null)
            {
                return Problem("Entity set 'ApplicationDbContext.kisims'  is null.");
            }
            var kisim = await _context.kisims.FindAsync(id);
            if (kisim != null)
            {
                _context.kisims.Remove(kisim);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KisimExists(int id)
        {
          return (_context.kisims?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
