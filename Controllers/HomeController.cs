using Microsoft.AspNetCore.Mvc;
using EnvanterTakip.Models;
using EnvanterTakip.Data;

namespace EnvanterTakip.Controllers
{
    public class HomeController : Controller
    {
        private readonly EnvanterContext _context;

        public HomeController(EnvanterContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}