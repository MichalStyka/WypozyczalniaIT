using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Wypozyczalnia.Data;
using Wypozyczalnia.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Wypozyczalnia.Controllers
{
    public class WypozyczenieController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // Dodane
        //private readonly SignInManager<IdentityUser> SignInManager;


        public WypozyczenieController(ApplicationDbContext context,UserManager<IdentityUser> userManager)// Zaktualizowane
        {
            _context = context;
            _userManager = userManager;// Przypisanie

        }

        [Authorize]
        // GET: Wypozyczenie 
        public async Task<IActionResult> Index(int? status)
        {


            if (User.IsInRole("Admin")) { 
            if (status == 1)
            {
                var applicationDbContext = _context.Wypozycenia.Include(w => w.Sprzet).Include(w => w.User).Where(s => s.Status == true);
                return View(await applicationDbContext.ToListAsync());

            }
            else if (status == 0)
            {
                var applicationDbContext = _context.Wypozycenia.Include(w => w.Sprzet).Include(w => w.User).Where(s => s.Status == false);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var applicationDbContext = _context.Wypozycenia.Include(w => w.Sprzet).Include(w => w.User);
                return View(await applicationDbContext.ToListAsync());
            }
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                if (status == 1)
                {
                    var applicationDbContext = _context.Wypozycenia.Include(w => w.Sprzet).Include(w => w.User).Where(s => s.Status == true).Where(u=>u.UserId==user.Id);
                    return View(await applicationDbContext.ToListAsync());

                }
                else if (status == 0)
                {
                    var applicationDbContext = _context.Wypozycenia.Include(w => w.Sprzet).Include(w => w.User).Where(s => s.Status == false).Where(u => u.UserId == user.Id);
                    return View(await applicationDbContext.ToListAsync());
                }
                else
                {
                    var applicationDbContext = _context.Wypozycenia.Include(w => w.Sprzet).Include(w => w.User).Where(u => u.UserId == user.Id);
                    return View(await applicationDbContext.ToListAsync());
                }
            }



        }







        // GET: Wypozyczenie/Create
        [Authorize]
        public IActionResult Create()
        {
            var DostepneSprzety = _context.Sprzety.Where(s => s.CzyDostepne == true).ToList();
            //ViewData["SprzetId"] = new SelectList(_context.Sprzety, "SprzetId", "SprzetNazwa");

            var cenySprzetu = DostepneSprzety.ToDictionary(s => s.SprzetId, s => s.CenaZaDzien);
            ViewBag.CenySprzetu = cenySprzetu;

            ViewData["SprzetId"] = new SelectList(DostepneSprzety, "SprzetId", "SprzetNazwa");
            return View();
        }

        // POST: Wypozyczenie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WypozyczenieId,SprzetId,DataWypozyczenia,DataZwrotu,UserId,CenaCalkowita,Status")] WypozyczenieModel wypozyczenieModel)
             
        {
            var user = await _userManager.GetUserAsync(User);
            wypozyczenieModel.UserId = user.Id; // Przypisanie UserId
            wypozyczenieModel.Status = true; // Ustawienie Status jako true
            

            if (ModelState.IsValid)
            {
                // Znajdowanie sprzętu powiązanego z wypożyczeniem
                var sprzet = await _context.Sprzety.FindAsync(wypozyczenieModel.SprzetId);
                //ustawienie jego wartości na false czyli zajete
                if (sprzet != null)
                {
                    sprzet.CzyDostepne = false; // Ustaw dostępność na false
                }

               


                _context.Add(wypozyczenieModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Logowanie błędów walidacji
    foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            ViewData["Errors"] = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();


             ViewData["SprzetId"] = new SelectList(_context.Sprzety, "SprzetId", "SprzetNazwa", wypozyczenieModel.SprzetId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", wypozyczenieModel.UserId);
            return View(wypozyczenieModel);
            
        }




        // GET: Wypozyczenie/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           
         var wypozyczenieModel = await _context.Wypozycenia
        .Include(w => w.Sprzet) // Dołączeniee obiekt Sprzet
        .FirstOrDefaultAsync(m => m.WypozyczenieId == id);

            if (wypozyczenieModel == null)
            {
                return NotFound();
            }

            // Pobieranie ceny sprzętu
            var cenySprzetu = _context.Sprzety.ToDictionary(s => s.SprzetId, s => s.CenaZaDzien);
            ViewBag.CenySprzetu = cenySprzetu;
           


            return View(wypozyczenieModel);
        }


        // POST: Wypozyczenie/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WypozyczenieId,SprzetId,DataWypozyczenia,DataZwrotu,UserId,CenaCalkowita,Status")] WypozyczenieModel wypozyczenieModel)
        {
            
            if (id != wypozyczenieModel.WypozyczenieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    _context.Update(wypozyczenieModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WypozyczenieModelExists(wypozyczenieModel.WypozyczenieId))
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
            //ViewData["SprzetId"] = new SelectList(_context.Sprzety, "SprzetId", "SprzetNazwa", wypozyczenieModel.SprzetId);
           // ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", wypozyczenieModel.UserId);
            return View(wypozyczenieModel);
        }

        // GET: Wypozyczenie/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var wypozyczenieModel = await _context.Wypozycenia
                .Include(w => w.Sprzet)
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.WypozyczenieId == id);

            if (wypozyczenieModel == null)
            {
                return NotFound();
            }


            // Obliczenie rzeczywistego kosztu
            DateTime currentDate = DateTime.Now.Date;
            DateTime rentalDate = wypozyczenieModel.DataWypozyczenia.Date;

            bool Status = wypozyczenieModel.Status;
            decimal rzeczywistyKoszt = 0;
            if (Status) {
                if (currentDate > rentalDate)
                {
                    int dniWypozyczenia = (currentDate - rentalDate).Days;
                    rzeczywistyKoszt = dniWypozyczenia * wypozyczenieModel.Sprzet.CenaZaDzien;
                }
            }
            
      

            ViewBag.RzeczywistyKoszt = rzeczywistyKoszt;


            return View(wypozyczenieModel);
        }

        // POST: Wypozyczenie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wypozyczenieModel = await _context.Wypozycenia.FindAsync(id);
            if (wypozyczenieModel != null)
            {
                // Zmiana statusu sprzętu na dostępny
                var sprzet = await _context.Sprzety.FindAsync(wypozyczenieModel.SprzetId);
                if (sprzet != null)
                {
                    sprzet.CzyDostepne = true;
                }
                _context.Wypozycenia.Remove(wypozyczenieModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool WypozyczenieModelExists(int id)
        {
            return _context.Wypozycenia.Any(e => e.WypozyczenieId == id);
        }



        //zakonczenie wypozyczenia
        // GET: Wypozyczenie/Zakoncz           
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Zakoncz(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var wypozyczenieModel = await _context.Wypozycenia
           .Include(w => w.Sprzet) // Dołączeniee obiekt Sprzet
           .FirstOrDefaultAsync(m => m.WypozyczenieId == id);

            if (wypozyczenieModel == null)
            {
                return NotFound();
            }


            // Obliczenie rzeczywistego kosztu
            DateTime currentDate = DateTime.Now.Date;
            DateTime rentalDate = wypozyczenieModel.DataWypozyczenia.Date;

            bool Status = wypozyczenieModel.Status;
            decimal rzeczywistyKoszt = 0;
            if (Status)
            {
                if (currentDate > rentalDate)
                {
                    int dniWypozyczenia = (currentDate - rentalDate).Days;
                    rzeczywistyKoszt = dniWypozyczenia * wypozyczenieModel.Sprzet.CenaZaDzien;
                }
            }

            ViewBag.RzeczywistyKoszt = rzeczywistyKoszt;
            return View(wypozyczenieModel);
        }


        // POST: Wypozyczenie/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Zakoncz(int id, [Bind("WypozyczenieId,SprzetId,DataWypozyczenia,DataZwrotu,UserId,CenaCalkowita,Status")] WypozyczenieModel wypozyczenieModel)
        {
            
            if (id != wypozyczenieModel.WypozyczenieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var sprzet = await _context.Sprzety.FindAsync(wypozyczenieModel.SprzetId);


                    // Obliczenie rzeczywistego kosztu
                    DateTime currentDate = DateTime.Now.Date;
                    DateTime rentalDate = wypozyczenieModel.DataWypozyczenia.Date;

                    bool Status = wypozyczenieModel.Status;
                    decimal rzeczywistyKoszt = 0;
                    if (Status)
                    {
                        if (currentDate > rentalDate)
                        {
                            int dniWypozyczenia = (currentDate - rentalDate).Days;
                            rzeczywistyKoszt = dniWypozyczenia * sprzet.CenaZaDzien;
                        }
                    }

                    // Aktualizacja CenaCalkowita, jeśli rzeczywisty koszt jest inny
                    if (rzeczywistyKoszt != wypozyczenieModel.CenaCalkowita)
                    {
                        wypozyczenieModel.CenaCalkowita = rzeczywistyKoszt;
                    }

                    wypozyczenieModel.Status = false; // Ustawienie Status jako false
                    // Znajdowanie sprzętu powiązanego z wypożyczeniem
                   
                    //ustawienie jego wartości na false czyli zajete
                    if (sprzet != null)
                    {
                        sprzet.CzyDostepne = true; // Ustaw dostępność na false
                    }
                    wypozyczenieModel.DataZwrotu = currentDate;
                    _context.Update(wypozyczenieModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WypozyczenieModelExists(wypozyczenieModel.WypozyczenieId))
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
           
            return View(wypozyczenieModel);
        }



    }
}
