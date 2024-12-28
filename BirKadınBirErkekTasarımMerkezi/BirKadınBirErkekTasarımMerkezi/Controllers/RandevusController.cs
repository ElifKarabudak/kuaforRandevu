using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BirKadınBirErkekTasarımMerkezi.Data;
using BirKadınBirErkekTasarımMerkezi.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BirKadınBirErkekTasarımMerkezi.Controllers
{
    [Authorize]
    public class RandevusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RandevusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Randevus
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.randevus.Include(r => r.Islemler).Include(r => r.Ustalar);
            return View(await applicationDbContext.ToListAsync());
        }
        [Authorize(Roles = "PATRON")]

        public async Task<IActionResult> Onayla(int id)
        {
            var randevu = await _context.randevus.FindAsync(id);
            if (randevu == null)
            {
                return NotFound();
            }

            randevu.Onay = true; 
            _context.Update(randevu);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "PATRON")]
        public async Task<IActionResult> RedEt(int id)
        {
            var randevu = await _context.randevus.FindAsync(id);
            if (randevu == null)
            {
                return NotFound();
            }

            randevu.Onay = false; 
            _context.Update(randevu);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "PATRON")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.randevus == null)
            {
                return NotFound();
            }

            var randevu = await _context.randevus
                .Include(r => r.Islemler)
                .Include(r => r.Ustalar)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (randevu == null)
            {
                return NotFound();
            }

            return View(randevu);
        }

        public IActionResult Create()
        {
            ViewData.Clear();
            ViewData["IslemlerID"] = new SelectList(_context.islemlers, "ID", "Ad");
            ViewData["UstalarID"] = new SelectList(_context.ustalars, "Id", "Adi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,iletisimNumaraniz,IslemlerID,UstalarID,Zaman")] Randevu randevu)
        {
            ModelState.Remove("Islemler");
            ModelState.Remove("Ustalar");
            var ustaIslemKontrol = await _context.ustalarinYapabildigiIslemlers
                                           .AnyAsync(ui => ui.UstalarID == randevu.UstalarID && ui.IslemlerID == randevu.IslemlerID);

            if (!ustaIslemKontrol)
            {
                ModelState.AddModelError("", "Seçilen usta, bu işlemi yapamıyor.");
            }
            if (randevu.Zaman.DayOfWeek == DayOfWeek.Tuesday)
            {
                ModelState.AddModelError("", "Salı günleri salon tatilidir.");
            }
            else if (randevu.Zaman.TimeOfDay < new TimeSpan(9, 0, 0) || randevu.Zaman.TimeOfDay > new TimeSpan(18, 0, 0))
            {
                ModelState.AddModelError("", "Seçilen zaman salonun çalışma saatleri dışında. Salon 09:00 - 18:00 arasında açıktır.");
            }
            TimeSpan randevuSuresi = new TimeSpan(1, 0, 0);  

           /* var ustaRandevuKontrol = await _context.randevus
                .Where(r => r.UstalarID == randevu.UstalarID)
                .AnyAsync(r =>
                    (r.Zaman < randevu.Zaman.Add(randevuSuresi) && r.Zaman.Add(randevuSuresi) > randevu.Zaman)
                );
            if (ustaRandevuKontrol)
            {
                ModelState.AddModelError("", "Seçilen zamanda usta meşgul, lütfen başka bir zaman dilimi seçin.");
            }*/
            var girisyapan = User.FindFirstValue(ClaimTypes.NameIdentifier);
            randevu.kullaniciID = girisyapan;
            if (ModelState.IsValid)
            {
                _context.Add(randevu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IslemlerID"] = new SelectList(_context.islemlers, "ID", "Ad", randevu.IslemlerID);
            ViewData["UstalarID"] = new SelectList(_context.ustalars, "Id", "Adi", randevu.UstalarID);
            return View(randevu);
        }

        [Authorize(Roles = "PATRON")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.randevus == null)
            {
                return NotFound();
            }

            var randevu = await _context.randevus.FindAsync(id);
            if (randevu == null)
            {
                return NotFound();
            }
            ViewData["IslemlerID"] = new SelectList(_context.islemlers, "ID", "ID", randevu.IslemlerID);
            ViewData["UstalarID"] = new SelectList(_context.ustalars, "Id", "Id", randevu.UstalarID);
            return View(randevu);
        }
        [Authorize(Roles = "PATRON")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,iletisimNumaraniz,kullaniciID,IslemlerID,UstalarID,Zaman")] Randevu randevu)
        {
            if (id != randevu.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Islemler");
            ModelState.Remove("Ustalar");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(randevu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RandevuExists(randevu.Id))
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
            ViewData["IslemlerID"] = new SelectList(_context.islemlers, "ID", "ID", randevu.IslemlerID);
            ViewData["UstalarID"] = new SelectList(_context.ustalars, "Id", "Id", randevu.UstalarID);
            return View(randevu);
        }

        [Authorize(Roles = "PATRON")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.randevus == null)
            {
                return NotFound();
            }

            var randevu = await _context.randevus
                .Include(r => r.Islemler)
                .Include(r => r.Ustalar)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (randevu == null)
            {
                return NotFound();
            }

            return View(randevu);
        }

        // POST: Randevus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.randevus == null)
            {
                return Problem("Entity set 'ApplicationDbContext.randevus'  is null.");
            }
            var randevu = await _context.randevus.FindAsync(id);
            if (randevu != null)
            {
                _context.randevus.Remove(randevu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RandevuExists(int id)
        {
          return (_context.randevus?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
