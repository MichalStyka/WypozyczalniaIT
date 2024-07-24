using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wypozyczalnia.Data;
using Wypozyczalnia.Models;

namespace Wypozyczalnia.Controllers
{
    public class SprzetController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // Dodane
        private readonly SignInManager<IdentityUser> SignInManager;
        public SprzetController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        public async Task<IActionResult> Index(int? status)
        {
          
                if (status == 1)
                {
                    var applicationDbContext = _context.Sprzety.Include(s => s.Kategoria).Where(s => s.CzyDostepne == true);
                    return View(await applicationDbContext.ToListAsync());

                }
                else if (status == 0)
                {
                    var applicationDbContext = _context.Sprzety.Include(s => s.Kategoria).Where(s => s.CzyDostepne == false);
                    return View(await applicationDbContext.ToListAsync());
                }
                else
                {
                    var applicationDbContext = _context.Sprzety.Include(s => s.Kategoria);
                    return View(await applicationDbContext.ToListAsync());

                }
            

            

        }




        [Authorize(Roles = "Admin")]
        // GET: Sprzet/Create
        public IActionResult Create()
        {
            ViewData["KategoriaId"] = new SelectList(_context.Kategorie, "KategoriaId", "KategoriaNazwa");
            return View();
        }

        // POST: Sprzet/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SprzetId,SprzetNazwa,KategoriaId,CenaZaDzien,CzyDostepne")] SprzetModel sprzetModel)
        {

                _context.Add(sprzetModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        
        }

        /////
        //Edytowanie
        /////
        [Authorize(Roles = "Admin")]
        // GET: Sprzet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sprzetModel = await _context.Sprzety.FindAsync(id);
            if (sprzetModel == null)
            {
                return NotFound();
            }
            ViewData["KategoriaId"] = new SelectList(_context.Kategorie, "KategoriaId", "KategoriaNazwa", sprzetModel.KategoriaId);
            return View(sprzetModel);
        }

        // POST: Sprzet/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SprzetId,SprzetNazwa,KategoriaId,CenaZaDzien,CzyDostepne")] SprzetModel sprzetModel)
        {
            if (id != sprzetModel.SprzetId)
            {
                return NotFound();
            }
                    _context.Update(sprzetModel);
                    await _context.SaveChangesAsync();
              
                return RedirectToAction(nameof(Index));
            
        }


        /////
        //Usuwanie
        /////
        [Authorize(Roles = "Admin")]
        // GET: Sprzet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sprzetModel = await _context.Sprzety
                .Include(s => s.Kategoria)
                .FirstOrDefaultAsync(m => m.SprzetId == id);
            if (sprzetModel == null)
            {
                return NotFound();
            }

            return View(sprzetModel);
        }

        // POST: Sprzet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sprzetModel = await _context.Sprzety.FindAsync(id);
            if (sprzetModel != null)
            { 
                _context.Sprzety.Remove(sprzetModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SprzetModelExists(int id)
        {
            return _context.Sprzety.Any(e => e.SprzetId == id);
        }
    }
}
