using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using KeyboardAndMouse.Models;
using Microsoft.AspNetCore.Authorization;
using KeyboardAndMouse.Data;

namespace KeyboardAndMouse.Controllers
{
    public class MouseController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MouseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var mice = await _context.Mice.Include(m => m.Category).ToListAsync();
            return View(mice);
        }

        [Authorize]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mouse = _context.Mice.Include(m => m.Category).FirstOrDefault(m => m.Id == id);

            if (mouse == null)
            {
                return NotFound();
            }

            return View(mouse);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mouse = _context.Mice.Find(id);

            if (mouse == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(mouse);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CategoryId,Description,Rating")] Mouse mouse)
        {
            if (id != mouse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MouseExists(mouse.Id))
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
            ViewBag.Categories = _context.Categories.ToList();
            return View(mouse);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,CategoryId,Description,Rating")] Mouse mouse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _context.Categories.ToList();
            return View(mouse);
        }

        private bool MouseExists(int id)
        {
            return _context.Mice.Any(m => m.Id == id);
        }
    }
}