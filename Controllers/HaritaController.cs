using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnvanterTakip.Data;
using EnvanterTakip.Models;

namespace EnvanterTakip.Controllers
{
    public class HaritaController : Controller
    {
        private readonly EnvanterContext _context;

        public HaritaController(EnvanterContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? kategoriId, string? arama)
        {
            ViewBag.Kategoriler = await _context.Kategoriler.ToListAsync();
            
            var konumlar = _context.Konumlar
                .Include(k => k.Kategori)
                .Where(k => k.Aktif);

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

            var konumlarList = await konumlar.ToListAsync();
            return View(konumlarList);
        }

        public async Task<IActionResult> Kent(int? kategoriId, string? arama)
        {
            return await Index(kategoriId, arama);
        }

        [HttpGet]
        public async Task<IActionResult> GetKonumlar(int? kategoriId)
        {
            var konumlar = await _context.Konumlar
                .Include(k => k.Kategori)
                .Where(k => k.Aktif)
                .Where(k => !kategoriId.HasValue || k.KategoriId == kategoriId.Value)
                .Select(k => new
                {
                    id = k.Id,
                    ad = k.Ad,
                    aciklama = k.Aciklama,
                    adres = k.Adres,
                    telefon = k.Telefon,
                    email = k.Email,
                    website = k.Website,
                    enlem = k.Enlem,
                    boylam = k.Boylam,
                    kategori = k.Kategori.Ad,
                    kategoriRenk = k.Kategori.Renk,
                    kategoriIkon = k.Kategori.Ikon,
                    resimUrl = k.ResimUrl,
                    calismaSaatleri = k.CalismaSaatleri
                })
                .ToListAsync();

            return Json(konumlar);
        }

        public async Task<IActionResult> Detay(int id)
        {
            var konum = await _context.Konumlar
                .Include(k => k.Kategori)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (konum == null)
            {
                return NotFound();
            }

            return View(konum);
        }
    }
}
