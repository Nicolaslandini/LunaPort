using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LunaPort.Models;

namespace LunaPort.Controllers
{
    public class EstadiosController : Controller
    {
        private readonly LunaDbContext _context;

        public EstadiosController(LunaDbContext context)
        {
            _context = context;
        }

        // GET: Estadios
        public async Task<IActionResult> Index()
        {
              return View(await _context.Estadios.ToListAsync());
        }

        // GET: Estadios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Estadios == null)
            {
                return NotFound();
            }

            var estadio = await _context.Estadios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estadio == null)
            {
                return NotFound();
            }

            return View(estadio);
        }

        // GET: Estadios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Estadios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,CapacidadMax")] Estadio estadio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estadio);
        }

        // GET: Estadios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Estadios == null)
            {
                return NotFound();
            }

            var estadio = await _context.Estadios.FindAsync(id);
            if (estadio == null)
            {
                return NotFound();
            }
            return View(estadio);
        }

        // POST: Estadios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,CapacidadMax")] Estadio estadio)
        {
            if (id != estadio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadioExists(estadio.Id))
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
            return View(estadio);
        }

        // GET: Estadios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Estadios == null)
            {
                return NotFound();
            }

            var estadio = await _context.Estadios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estadio == null)
            {
                return NotFound();
            }

            return View(estadio);
        }

        // POST: Estadios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Estadios == null)
            {
                return Problem("Entity set 'LunaDbContext.Estadios'  is null.");
            }
            var estadio = await _context.Estadios.FindAsync(id);
            if (estadio != null)
            {
                _context.Estadios.Remove(estadio);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadioExists(int id)
        {
          return _context.Estadios.Any(e => e.Id == id);
        }
    }
}
