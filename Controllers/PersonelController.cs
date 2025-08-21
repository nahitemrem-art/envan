using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnvanterTakip.Data;
using EnvanterTakip.Models;

namespace EnvanterTakip.Controllers
{
    
    public class PersonelController : Controller
    {
        private readonly EnvanterContext _context;

        public PersonelController(EnvanterContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var personeller = from p in _context.Personeller
                           select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                personeller = personeller.Where(p => p.Ad.Contains(searchString)
                                       || p.Soyad.Contains(searchString));
            }
            return View(await personeller.AsNoTracking().ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Personel personel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(personel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personel = await _context.Personeller.FindAsync(id);
            if (personel == null)
            {
                return NotFound();
            }
            return View(personel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Personel personel)
        {
            if (id != personel.PersonelID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonelExists(personel.PersonelID))
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
            return View(personel);
        }

        private bool PersonelExists(int id)
        {
            return _context.Personeller.Any(e => e.PersonelID == id);
        }
    }
}
