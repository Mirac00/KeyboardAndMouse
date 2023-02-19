using KeyboardAndMouse.Data;
using KeyboardAndMouse.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading.Tasks;

namespace KeyboardAndMouse.Controllers
{
    [Authorize]
    public class KeyboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public KeyboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Keyboard
        public async Task<IActionResult> Index()
        {
            var keyboards = await _context.Keyboards.Include(k => k.Category).ToListAsync();
            return View(keyboards);
        }

        // GET: Keyboard/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var keyboard = await _context.Keyboards.Include(k => k.Category).FirstOrDefaultAsync(m => m.Id == id);

            if (keyboard == null)
            {
                return NotFound();
            }

            return View(keyboard);
        }

        // GET: Keyboard/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Keyboard/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CategoryId,Description,Rating")] Keyboard keyboard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(keyboard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", keyboard.CategoryId);
            return View(keyboard);
        }

        // GET: Keyboard/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var keyboard = await _context.Keyboards.FindAsync(id);
            if (keyboard == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", keyboard.CategoryId);
            return View(keyboard);
        }

        // POST: Keyboard/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CategoryId,Description,Rating")] Keyboard keyboard)
        {
            if (id != keyboard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(keyboard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KeyboardExists(keyboard.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", keyboard.CategoryId);
            return View(keyboard);
        }

        private bool KeyboardExists(int id)
        {
            return _context.Keyboards.Any(e => e.Id == id);
        }
    }
}