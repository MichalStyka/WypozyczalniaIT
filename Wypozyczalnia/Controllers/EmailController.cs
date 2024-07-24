using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wypozyczalnia.Data;

namespace Wypozyczalnia.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailSender emailSender;
        private readonly ApplicationDbContext _context;

        public EmailController(ApplicationDbContext context,IEmailSender emailSender)
        {
            this.emailSender = emailSender;
            _context = context;
        }


        public async Task<IActionResult> Wyslij(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var wypozyczenieModel = await _context.Wypozycenia
           .Include(w => w.Sprzet) // Dołączeniee obiekt Sprzet
           .Include(u => u.User)
           .FirstOrDefaultAsync(m => m.WypozyczenieId == id);

            if (wypozyczenieModel == null)
            {
                return NotFound();
            }

            var Do = wypozyczenieModel.User.UserName;
            var subject = "Upomnienie";
            var message = "";
//generowanie wiadomosci
            DateTime DataDzisiejsza = DateTime.Now.Date;
            DateTime Zwrot = wypozyczenieModel.DataZwrotu.Date;
            string dataZwrotuFormatowana = wypozyczenieModel.DataZwrotu.ToString("dd-MM-yyyy");


            if (DataDzisiejsza < Zwrot) {
                message = $@"Witaj {wypozyczenieModel.User.UserName},

Uprzejmie przypominam,że czas wypozyczenia sprzetu:
- {wypozyczenieModel.Sprzet.SprzetNazwa} , dobiega końca w dniu {dataZwrotuFormatowana}.
Przy oddaniu należy uiścić opłate za wypozyczenie o wartości : {wypozyczenieModel.CenaCalkowita} zł.
dziękujemy!
"; }else
            {

                //var sprzet = await _context.Sprzety.FindAsync(wypozyczenieModel.SprzetId);


                // Obliczenie rzeczywistego kosztu

                DateTime DataWypozyczenia = wypozyczenieModel.DataWypozyczenia.Date;
                
                decimal rzeczywistyKoszt = 0;
               
                    if (DataDzisiejsza > DataWypozyczenia)
                    {
                        int dniWypozyczenia = (DataDzisiejsza - DataWypozyczenia).Days;
                    rzeczywistyKoszt = dniWypozyczenia * wypozyczenieModel.Sprzet.CenaZaDzien;
                    }
                

                message = $@"Witaj {wypozyczenieModel.User.UserName},

Uprzejmie przypominam,że czas wypozyczenia sprzetu:
- {wypozyczenieModel.Sprzet.SprzetNazwa} , dobiegł już końca w dniu {dataZwrotuFormatowana}
Początkowa przewidywalna opłata za wypozyczenie wynosiła : {wypozyczenieModel.CenaCalkowita} zł.
Za zwłokę o kazdy dzień zostania naliczona stawka jak za każdy kolejny dzień wypożyczenia sprzetu.
Ostateczna wartość opłaty wynosi : {rzeczywistyKoszt} zł.
dziękujemy!
";
            }



            
            await emailSender.SendEmailAsync(Do, subject, message);

            
            return RedirectToAction("Index", "Wypozyczenie");
        }







    }
}
