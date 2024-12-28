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
    [Authorize]
    public class IslemlersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IslemlersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.islemlers.Include(i => i.Kisim);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> ErkeklereOzelSalon()
        {
            var applicationDbContext = _context.islemlers.Include(i => i.Kisim).Where(x=>x.KisimID==1);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> KadinlaraOzelSalon()
        {
            var applicationDbContext = _context.islemlers.Include(i => i.Kisim).Where(x => x.KisimID == 2);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.islemlers == null)
            {
                return NotFound();
            }

            var islemler = await _context.islemlers
                .Include(i => i.Kisim)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (islemler == null)
            {
                return NotFound();
            }

            return View(islemler);
        }
        [Authorize(Roles = "PATRON")]

        public IActionResult Create()
        {
            ViewData["KisimID"] = new SelectList(_context.kisims, "Id", "Id");
            return View();
        }
        [Authorize(Roles = "PATRON")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Ad,Sure,Ucret,KisimID")] Islemler islemler)
        {
            ModelState.Remove("Kisim");
            if (ModelState.IsValid)
            {
                _context.Add(islemler);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KisimID"] = new SelectList(_context.kisims, "Id", "Id", islemler.KisimID);
            return View(islemler);
        }
        [Authorize(Roles = "PATRON")]
        [Authorize(Roles = "PATRON")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.islemlers == null)
            {
                return NotFound();
            }

            var islemler = await _context.islemlers.FindAsync(id);
            if (islemler == null)
            {
                return NotFound();
            }
            ViewData["KisimID"] = new SelectList(_context.kisims, "Id", "Id", islemler.KisimID);
            return View(islemler);
        }
        [Authorize(Roles = "PATRON")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Ad,Sure,Ucret,KisimID")] Islemler islemler)
        {
            if (id != islemler.ID)
            {
                return NotFound();
            }
            ModelState.Remove("Kisim");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(islemler);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IslemlerExists(islemler.ID))
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
            ViewData["KisimID"] = new SelectList(_context.kisims, "Id", "Id", islemler.KisimID);
            return View(islemler);
        }

        [Authorize(Roles = "PATRON")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.islemlers == null)
            {
                return NotFound();
            }

            var islemler = await _context.islemlers
                .Include(i => i.Kisim)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (islemler == null)
            {
                return NotFound();
            }

            return View(islemler);
        }

        [Authorize(Roles = "PATRON")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.islemlers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.islemlers'  is null.");
            }
            var islemler = await _context.islemlers.FindAsync(id);
            if (islemler != null)
            {
                _context.islemlers.Remove(islemler);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IslemlerExists(int id)
        {
          return (_context.islemlers?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
