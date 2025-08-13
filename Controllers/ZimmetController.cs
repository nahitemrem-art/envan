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

        

        public async Task<IActionResult> Create()
        {
            var zimmetliCihazIdler = await _context.Zimmetler.Select(z => z.CihazID).ToListAsync();
            ViewBag.Cihazlar = await _context.Cihazlar
                .Where(c => !zimmetliCihazIdler.Contains(c.CihazID))
                .Select(c => new { c.CihazID, MarkaModelSeriNo = $"{c.Marka} {c.Model} (SN:{c.SeriNo})" })
                .ToListAsync();

            ViewBag.Personeller = await _context.Personeller
                .Select(p => new { p.PersonelID, DisplayText = $"{p.Ad} {p.Soyad} - {p.BirimAdi}" })
                .ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Zimmet zimmet) // Bind attribute kaldırıldı
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Otomatik tarih ataması
                    zimmet.ZimmetTarihi = zimmet.ZimmetTarihi == default ? DateTime.Now : zimmet.ZimmetTarihi;
                    
                    // Cihaz durum güncelleme
                    var cihaz = await _context.Cihazlar.FindAsync(zimmet.CihazID);
                    if (cihaz == null)
                    {
                        ModelState.AddModelError("CihazID", "Seçilen cihaz bulunamadı");
                        return await ReloadCreateView(zimmet);
                    }
                    
                    cihaz.Durum = "Zimmetli";
                    _context.Update(cihaz);
                    
                    _context.Add(zimmet);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Zimmet işlemi başarıyla oluşturuldu";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu: " + ex.Message);
            }
            
            return await ReloadCreateView(zimmet);
        }

private async Task<IActionResult> ReloadCreateView(Zimmet zimmet)
{
    ViewBag.Cihazlar = await _context.Cihazlar
        .Where(c => c.Durum == "Depoda")
        .ToListAsync();
        
    ViewBag.Personeller = await _context.Personeller
        .Where(p => p.AktifMi)
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
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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