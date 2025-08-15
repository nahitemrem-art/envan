using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnvanterTakip.Data;
using EnvanterTakip.Models;

namespace EnvanterTakip.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ZimmetController : Controller
    {
        private readonly EnvanterContext _context;

        public ZimmetController(EnvanterContext context)
        {
            _context = context;
        }

                public async Task<IActionResult> Index()
        {
            var zimmetler = await _context.Zimmetler
                .Include(z => z.Cihaz)
                .Include(z => z.Personel) // Değiştirildi
                .OrderByDescending(z => z.ZimmetTarihi)
                .ToListAsync();
            return View(zimmetler);
        }

        

        // GET: Zimmet/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Cihazlar = await _context.Cihazlar.ToListAsync();
            ViewBag.Personeller = await _context.Personeller
                .Select(p => new { p.PersonelID, DisplayText = p.Ad + " " + p.Soyad })
                .ToListAsync();
            return View();
        }

        // POST: Zimmet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Zimmet zimmet)
        {
            if (ModelState.IsValid)
            {
                zimmet.ZimmetTarihi = DateTime.SpecifyKind(zimmet.ZimmetTarihi, DateTimeKind.Utc);
                if (zimmet.IadeTarihi != null)
                    zimmet.IadeTarihi = DateTime.SpecifyKind(zimmet.IadeTarihi.Value, DateTimeKind.Utc);

                _context.Add(zimmet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // ModelState geçersizse ViewBag'leri tekrar doldur
            ViewBag.Cihazlar = await _context.Cihazlar.ToListAsync();
            ViewBag.Personeller = await _context.Personeller
                .Select(p => new { p.PersonelID, DisplayText = p.Ad + " " + p.Soyad })
                .ToListAsync();
            return View(zimmet);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var zimmet = await _context.Zimmetler.FindAsync(id);
            if (zimmet == null)
            {
                return NotFound();
            }
            ViewBag.Cihazlar = await _context.Cihazlar
                .Select(c => new { c.CihazID, MarkaModelSeriNo = $"{c.Marka} {c.Model} (SN:{c.SeriNo})" })
                .ToListAsync();

            ViewBag.Personeller = await _context.Personeller
                .Select(p => new { p.PersonelID, DisplayText = $"{p.Ad} {p.Soyad} - {p.BirimAdi}" })
                .ToListAsync();

            return View(zimmet);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Zimmet zimmet)
        {
            if (id != zimmet.ZimmetID)
                return NotFound();

            if (ModelState.IsValid)
            {
                zimmet.ZimmetTarihi = DateTime.SpecifyKind(zimmet.ZimmetTarihi, DateTimeKind.Utc);
                if (zimmet.IadeTarihi != null)
                    zimmet.IadeTarihi = DateTime.SpecifyKind(zimmet.IadeTarihi.Value, DateTimeKind.Utc);

                _context.Update(zimmet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Cihazlar = await _context.Cihazlar
                .Select(c => new { c.CihazID, MarkaModelSeriNo = $"{c.Marka} {c.Model} (SN:{c.SeriNo})" })
                .ToListAsync();

            ViewBag.Personeller = await _context.Personeller
                .Select(p => new { p.PersonelID, DisplayText = $"{p.Ad} {p.Soyad} - {p.BirimAdi}" })
                .ToListAsync();

            return View(zimmet);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var zimmet = await _context.Zimmetler
                .Include(z => z.Cihaz)
                .Include(z => z.Personel)
                .FirstOrDefaultAsync(z => z.ZimmetID == id);

            if (zimmet == null)
            {
                return NotFound();
            }
            return View(zimmet);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zimmet = await _context.Zimmetler.FindAsync(id);
            if (zimmet != null)
            {
                _context.Zimmetler.Remove(zimmet);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
