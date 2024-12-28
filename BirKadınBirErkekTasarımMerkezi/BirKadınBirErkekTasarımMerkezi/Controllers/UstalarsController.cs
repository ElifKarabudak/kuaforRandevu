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
    public class UstalarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UstalarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ustalars
        public async Task<IActionResult> Index()
        {
              return _context.ustalars != null ? 
                          View(await _context.ustalars.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ustalars'  is null.");
        }

        [Authorize(Roles = "PATRON")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ustalars == null)
            {
                return NotFound();
            }

            var ustalar = await _context.ustalars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ustalar == null)
            {
                return NotFound();
            }

            return View(ustalar);
        }

        [Authorize(Roles = "PATRON")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "PATRON")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Adi,UstalikAlaniAciklama")] Ustalar ustalar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ustalar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ustalar);
        }

        [Authorize(Roles = "PATRON")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ustalars == null)
            {
                return NotFound();
            }

            var ustalar = await _context.ustalars.FindAsync(id);
            if (ustalar == null)
            {
                return NotFound();
            }
            return View(ustalar);
        }
        [Authorize(Roles = "PATRON")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Adi,UstalikAlaniAciklama")] Ustalar ustalar)
        {
            if (id != ustalar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ustalar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UstalarExists(ustalar.Id))
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
            return View(ustalar);
        }

        [Authorize(Roles = "PATRON")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ustalars == null)
            {
                return NotFound();
            }

            var ustalar = await _context.ustalars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ustalar == null)
            {
                return NotFound();
            }

            return View(ustalar);
        }

        [Authorize(Roles = "PATRON")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ustalars == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ustalars'  is null.");
            }
            var ustalar = await _context.ustalars.FindAsync(id);
            if (ustalar != null)
            {
                _context.ustalars.Remove(ustalar);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UstalarExists(int id)
        {
          return (_context.ustalars?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
