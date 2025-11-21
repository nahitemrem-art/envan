using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnvanterTakip.Data;
using EnvanterTakip.Models;

namespace EnvanterTakip.Controllers
{
    [Authorize]
    public class KonumController : Controller
    {
        private readonly EnvanterContext _context;

        public KonumController(EnvanterContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? kategoriId, string? arama)
        {
            ViewBag.Kategoriler = await _context.Kategoriler.ToListAsync();
            
            var konumlar = _context.Konumlar.Include(k => k.Kategori).AsQueryable();

            if (kategoriId.HasValue)
            {
                konumlar = konumlar.Where(k => k.KategoriId == kategoriId.Value);
            }

            if (!string.IsNullOrEmpty(arama))
            {
                konumlar = konumlar.Where(k => 
                    k.Ad.Contains(arama) || 
                    (k.Aciklama != null && k.Aciklama.Contains(arama)) || 
                    (k.Adres != null && k.Adres.Contains(arama)));
            }

            return View(await konumlar.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Kategoriler = await _context.Kategoriler.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Konum konum)
        {
            if (ModelState.IsValid)
            {
                konum.EklenmeTarihi = DateTime.UtcNow;
                konum.EkleyenKullanici = User.Identity?.Name;
                _context.Add(konum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Kategoriler = await _context.Kategoriler.ToListAsync();
            return View(konum);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var konum = await _context.Konumlar.FindAsync(id);
            if (konum == null)
            {
                return NotFound();
            }

            var currentUser = User.Identity?.Name;
            var isAdmin = User.IsInRole("Admin");
            
            if (!isAdmin && konum.EkleyenKullanici != currentUser)
            {
                TempData["Error"] = "Bu konumu sadece ekleyen kullanıcı düzenleyebilir.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Kategoriler = await _context.Kategoriler.ToListAsync();
            return View(konum);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Konum konum)
        {
            if (id != konum.Id)
            {
                return NotFound();
            }

            var existingKonum = await _context.Konumlar.AsNoTracking().FirstOrDefaultAsync(k => k.Id == id);
            if (existingKonum == null)
            {
                return NotFound();
            }

            var currentUser = User.Identity?.Name;
            var isAdmin = User.IsInRole("Admin");
            
            if (!isAdmin && existingKonum.EkleyenKullanici != currentUser)
            {
                TempData["Error"] = "Bu konumu sadece ekleyen kullanıcı düzenleyebilir.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    konum.EkleyenKullanici = existingKonum.EkleyenKullanici;
                    _context.Update(konum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KonumExists(konum.Id))
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
            ViewBag.Kategoriler = await _context.Kategoriler.ToListAsync();
            return View(konum);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var konum = await _context.Konumlar
                .Include(k => k.Kategori)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (konum == null)
            {
                return NotFound();
            }

            var currentUser = User.Identity?.Name;
            var isAdmin = User.IsInRole("Admin");
            
            if (!isAdmin && konum.EkleyenKullanici != currentUser)
            {
                TempData["Error"] = "Bu konumu sadece ekleyen kullanıcı silebilir.";
                return RedirectToAction(nameof(Index));
            }

            return View(konum);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var konum = await _context.Konumlar.FindAsync(id);
            if (konum != null)
            {
                var currentUser = User.Identity?.Name;
                var isAdmin = User.IsInRole("Admin");
                
                if (!isAdmin && konum.EkleyenKullanici != currentUser)
                {
                    TempData["Error"] = "Bu konumu sadece ekleyen kullanıcı silebilir.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Konumlar.Remove(konum);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool KonumExists(int id)
        {
            return _context.Konumlar.Any(e => e.Id == id);
        }
    }
}
