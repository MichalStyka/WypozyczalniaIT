using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wypozyczalnia.Models;

namespace Wypozyczalnia.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<KategoriaModel> Kategorie { get; set; }
        public DbSet<SprzetModel> Sprzety { get; set; }

        public DbSet<WypozyczenieModel> Wypozycenia { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //kategoria

            modelBuilder.Entity<SprzetModel>()
                .HasOne(s => s.Kategoria)
                .WithMany(s => s.Sprzety)
                .HasForeignKey(s => s.KategoriaId)
                .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Entity<WypozyczenieModel>()
                .HasOne(w => w.Sprzet)
                .WithMany(s => s.Wypozyczenia)
                .HasForeignKey(w => w.SprzetId)
                 .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<WypozyczenieModel>()
           .HasOne(w => w.User)
           .WithMany()
           .HasForeignKey(w => w.UserId)
           .OnDelete(DeleteBehavior.SetNull);


        }

    }
}
