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

    public class UstalarinYapabildigiIslemlersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UstalarinYapabildigiIslemlersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ustalarinYapabildigiIslemlers.Include(u => u.Islemler).Include(u => u.Ustalar);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ustalarinYapabildigiIslemlers == null)
            {
                return NotFound();
            }

            var ustalarinYapabildigiIslemler = await _context.ustalarinYapabildigiIslemlers
                .Include(u => u.Islemler)
                .Include(u => u.Ustalar)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ustalarinYapabildigiIslemler == null)
            {
                return NotFound();
            }

            return View(ustalarinYapabildigiIslemler);
        }

        // GET: UstalarinYapabildigiIslemlers/Create
        public IActionResult Create()
        {
            ViewData.Clear();
            ViewData["IslemlerID"] = new SelectList(_context.islemlers, "ID", "ID");
            ViewData["UstalarID"] = new SelectList(_context.ustalars, "Id", "Id");
            return View();
        }

        // POST: UstalarinYapabildigiIslemlers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UstalarID,IslemlerID")] UstalarinYapabildigiIslemler ustalarinYapabildigiIslemler)
        {
            ModelState.Remove("Islemler");
            ModelState.Remove("Ustalar");
            if (ModelState.IsValid)
            {
                _context.Add(ustalarinYapabildigiIslemler);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IslemlerID"] = new SelectList(_context.islemlers, "ID", "ID", ustalarinYapabildigiIslemler.IslemlerID);
            ViewData["UstalarID"] = new SelectList(_context.ustalars, "Id", "Id", ustalarinYapabildigiIslemler.UstalarID);
            return View(ustalarinYapabildigiIslemler);
        }

        // GET: UstalarinYapabildigiIslemlers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ustalarinYapabildigiIslemlers == null)
            {
                return NotFound();
            }

            var ustalarinYapabildigiIslemler = await _context.ustalarinYapabildigiIslemlers.FindAsync(id);
            if (ustalarinYapabildigiIslemler == null)
            {
                return NotFound();
            }
            ViewData["IslemlerID"] = new SelectList(_context.islemlers, "ID", "ID", ustalarinYapabildigiIslemler.IslemlerID);
            ViewData["UstalarID"] = new SelectList(_context.ustalars, "Id", "Id", ustalarinYapabildigiIslemler.UstalarID);
            return View(ustalarinYapabildigiIslemler);
        }

        // POST: UstalarinYapabildigiIslemlers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UstalarID,IslemlerID")] UstalarinYapabildigiIslemler ustalarinYapabildigiIslemler)
        {
            if (id != ustalarinYapabildigiIslemler.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Islemler");
            ModelState.Remove("Ustalar");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ustalarinYapabildigiIslemler);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UstalarinYapabildigiIslemlerExists(ustalarinYapabildigiIslemler.Id))
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
            ViewData["IslemlerID"] = new SelectList(_context.islemlers, "ID", "ID", ustalarinYapabildigiIslemler.IslemlerID);
            ViewData["UstalarID"] = new SelectList(_context.ustalars, "Id", "Id", ustalarinYapabildigiIslemler.UstalarID);
            return View(ustalarinYapabildigiIslemler);
        }

        // GET: UstalarinYapabildigiIslemlers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ustalarinYapabildigiIslemlers == null)
            {
                return NotFound();
            }

            var ustalarinYapabildigiIslemler = await _context.ustalarinYapabildigiIslemlers
                .Include(u => u.Islemler)
                .Include(u => u.Ustalar)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ustalarinYapabildigiIslemler == null)
            {
                return NotFound();
            }

            return View(ustalarinYapabildigiIslemler);
        }

        // POST: UstalarinYapabildigiIslemlers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ustalarinYapabildigiIslemlers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ustalarinYapabildigiIslemlers'  is null.");
            }
            var ustalarinYapabildigiIslemler = await _context.ustalarinYapabildigiIslemlers.FindAsync(id);
            if (ustalarinYapabildigiIslemler != null)
            {
                _context.ustalarinYapabildigiIslemlers.Remove(ustalarinYapabildigiIslemler);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UstalarinYapabildigiIslemlerExists(int id)
        {
          return (_context.ustalarinYapabildigiIslemlers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
