// Controllers/CihazController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnvanterTakip.Data;
using EnvanterTakip.Models;

namespace EnvanterTakip.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CihazController : Controller
    {
        private readonly EnvanterContext _context;

        public CihazController(EnvanterContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var cihazlarQuery = _context.Cihazlar.Include(c => c.Zimmetler).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                var searchStringLower = searchString.ToLower();
                cihazlarQuery = cihazlarQuery.Where(c =>
                    c.CihazTuru.ToLower().Contains(searchStringLower) ||
                    c.Marka.ToLower().Contains(searchStringLower) ||
                    c.Model.ToLower().Contains(searchStringLower) ||
                    c.SeriNo.ToLower().Contains(searchStringLower) ||
                    c.Durum.ToLower().Contains(searchStringLower));
            }

            var cihazlar = await cihazlarQuery.ToListAsync();

            // Her cihazın Durumunu güncelle
            foreach (var cihaz in cihazlar)
            {
                // Eğer cihazın en az bir aktif zimmeti varsa (ör: IadeTarihi null ise)
                if (cihaz.Zimmetler != null && cihaz.Zimmetler.Any(z => z.IadeTarihi == null))
                    cihaz.Durum = "Zimmetli";
                else
                    cihaz.Durum = "Depoda";
            }

            return View(cihazlar);
        }
        // Controllers/CihazController.cs
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cihaz cihaz)
        {
            if (ModelState.IsValid)
            {
                cihaz.Durum = "Depoda"; // Varsayılan durum
                _context.Add(cihaz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cihaz);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cihaz = await _context.Cihazlar.FindAsync(id);
            if (cihaz == null)
                return NotFound();
            return View(cihaz);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cihaz cihaz)
        {
            if (id != cihaz.CihazID)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(cihaz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cihaz);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var cihaz = await _context.Cihazlar.FindAsync(id);
            if (cihaz == null)
                return NotFound();
            return View(cihaz);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cihaz = await _context.Cihazlar.FindAsync(id);
            if (cihaz != null)
            {
                _context.Cihazlar.Remove(cihaz);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
