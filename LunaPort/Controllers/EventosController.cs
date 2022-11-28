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
    public class EventosController : Controller
    {
        private readonly LunaDbContext _context;

        public EventosController(LunaDbContext context)
        {
            _context = context;
        }

        // GET: Eventoes
        public async Task<IActionResult> Index()
        {
              return View(await _context.Eventos.ToListAsync());
        }

        // GET: Eventoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Eventos == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .FirstOrDefaultAsync(m => m.IdEvento == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // GET: Eventoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Eventoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEvento,Nombre,Fecha,IdEstadio,0")] Evento evento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(evento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(evento);
        }

        // GET: Eventoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Eventos == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }
            return View(evento);
        }

        // POST: Eventoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEvento,Nombre,Fecha,IdEstadio,Participantes")] Evento evento)
        {
            if (id != evento.IdEvento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventoExists(evento.IdEvento))
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
            return View(evento);
        }

        // GET: Eventoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Eventos == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .FirstOrDefaultAsync(m => m.IdEvento == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // POST: Eventoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Eventos == null)
            {
                return Problem("Entity set 'LunaDbContext.Eventos'  is null.");
            }
            var evento = await _context.Eventos.FindAsync(id);
            if (evento != null)
            {
                _context.Eventos.Remove(evento);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventoExists(int id)
        {
          return _context.Eventos.Any(e => e.IdEvento == id);
     
        }

        // Obtiene la capacidad actual del evento para saber cuantos tickets hay disponibles para la compra

        [HttpGet]
        public async Task<IActionResult> CantidadDisponible(int id)
        {
            var evento = await _context.Eventos
         .FirstOrDefaultAsync(m => m.IdEvento == id);
            if (evento == null)
            {
                return NotFound();
            }

            var estadio = await _context.Estadios.FindAsync(evento.IdEstadio);
            if (estadio == null)
            {
                return NotFound();
            }
            int capacidadEstadio = estadio.CapacidadMax;

            int capacidadActual = capacidadEstadio - evento.Participantes;

            ViewBag.capacidadDisponible = capacidadActual;
            
            return View("Details",evento);
        }


        /* Reservar - realiza descuento de entradas y envia datos para realizar boleto */

        [HttpPost]
        public async Task<IActionResult> EfectuarReserva(int? id, int cantidadEntradas)
        {
            var evento = await _context.Eventos
                .FirstOrDefaultAsync(m => m.IdEvento == id);
            if (evento == null)
            {
                return NotFound();
            }

            int participantes = evento.Participantes;
            String[] meses = new string[] { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };

            var estadio = await _context.Estadios.FindAsync(evento.IdEstadio);
            if (estadio == null)
            {
                return NotFound();
            }
            int capacidadEstadio = estadio.CapacidadMax;
            if (participantes + cantidadEntradas <= capacidadEstadio)
            {
                participantes += cantidadEntradas;
                evento.Participantes = participantes;
                await _context.SaveChangesAsync();
              
                
                
                ViewBag.cantidadEntradas = cantidadEntradas;
                ViewBag.dia = Convert.ToDateTime(evento.Fecha).Day;
                ViewBag.mes = meses[Convert.ToDateTime(evento.Fecha).Month - 1];
                ViewBag.hora = Convert.ToDateTime(evento.Fecha).ToShortTimeString();
                ViewBag.nombreEvento = evento.Nombre;
                ViewBag.nombreEstadio = estadio.Nombre;
                return View("CompraRealizada", evento);
            }
            else
            {
                ViewBag.mensajeError = "No se puede reservar esa cantidad de entradas";
                return View("CompraDefectuosa", evento);
            }

        }
        
    

    }
}
