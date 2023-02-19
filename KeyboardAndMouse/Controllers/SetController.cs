using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KeyboardAndMouse.Data;
using KeyboardAndMouse.Models;
using System.Linq;
using System.Threading.Tasks;

namespace KeyboardAndMouse.Controllers
{
    public class SetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SetController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Set
        public async Task<IActionResult> Index()
        {
            var sets = await _context.Sets
                .Include(s => s.Keyboard)
                .Include(s => s.Mouse)
                .Include(s => s.Category)
                .ToListAsync();

            return View(sets);
        }

        // GET: Set/Create
        public IActionResult Create()
        {
            ViewData["KeyboardId"] = new SelectList(_context.Keyboards, "Id", "Name");
            ViewData["MouseId"] = new SelectList(_context.Mice, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Set/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KeyboardId,MouseId,Name,CategoryId,Description,Rating")] Set set)
        {
            if (ModelState.IsValid)
            {
                set.Name = set.Name + " [" + _context.Keyboards.Find(set.KeyboardId).Name + " plus " + _context.Mice.Find(set.MouseId).Name + "]";
                _context.Add(set);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KeyboardId"] = new SelectList(_context.Keyboards, "Id", "Name", set.KeyboardId);
            ViewData["MouseId"] = new SelectList(_context.Mice, "Id", "Name", set.MouseId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", set.CategoryId);
            return View(set);
        }

        // GET: Set/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var set = await _context.Sets
                .Include(s => s.Keyboard)
                .Include(s => s.Mouse)
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (set == null)
            {
                return NotFound();
            }

            return View(set);
        }

        // GET: Set/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var set = await _context.Sets.FindAsync(id);
            if (set == null)
            {
                return NotFound();
            }
            ViewData["KeyboardId"] = new SelectList(_context.Keyboards, "Id", "Name", set.KeyboardId);
            ViewData["MouseId"] = new SelectList(_context.Mice, "Id", "Name", set.MouseId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", set.CategoryId);
            return View(set);
        }

        // POST: Set/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KeyboardId,MouseId,Name,CategoryId,Description,Rating")] Set set)
        {
            if (id != set.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(set);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SetExists(set.Id))
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
            return View(set);
        }

        private bool SetExists(int id)
        {
            return _context.Sets.Any(e => e.Id == id);
        }
    }
}

