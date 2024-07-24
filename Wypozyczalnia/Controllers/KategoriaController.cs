using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wypozyczalnia.Data;
using Wypozyczalnia.Models;

namespace Wypozyczalnia.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KategoriaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KategoriaController(ApplicationDbContext context)
        {
            _context = context;
        }



        // GET: Kategoria
        public async Task<IActionResult> Index()
        {
            return View(await _context.Kategorie.ToListAsync());
        }



        //////////////////////////////////////////////////////////////////
        //Dodawanie Kategorii


        // GET: Kategoria/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kategoria/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KategoriaId,KategoriaNazwa")] KategoriaModel kategoriaModel)
        {

            _context.Add(kategoriaModel);
            await _context.SaveChangesAsync();

            //return View(kategoriaModel);
            return RedirectToAction(nameof(Index));
        }


        //////////////////////////////////////////////////////////////////
        //Edytowanie Kategorii

        // GET: Kategoria/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategoriaModel = await _context.Kategorie.FindAsync(id);
            if (kategoriaModel == null)
            {
                return NotFound();
            }
            return View(kategoriaModel);
        }

        // POST: Kategoria/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KategoriaId,KategoriaNazwa")] KategoriaModel kategoriaModel)
        {
            if (id != kategoriaModel.KategoriaId)
            {
                return NotFound();
            }



            _context.Update(kategoriaModel);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));


           
        }



        // GET: Kategoria/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategoriaModel = await _context.Kategorie
                .FirstOrDefaultAsync(m => m.KategoriaId == id);
            if (kategoriaModel == null)
            {
                return NotFound();
            }

            return View(kategoriaModel);
        }

        // POST: Kategoria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kategoriaModel = await _context.Kategorie.FindAsync(id);
            if (kategoriaModel != null)
            {
                _context.Kategorie.Remove(kategoriaModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KategoriaModelExists(int id)
        {
            return _context.Kategorie.Any(e => e.KategoriaId == id);
        }
    }
}
