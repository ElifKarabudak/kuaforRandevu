using BirKadınBirErkekTasarımMerkezi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BirKadınBirErkekTasarımMerkezi.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Ustalar> ustalars { get; set; }
        public DbSet<Kisim> kisims { get; set; }
        public DbSet<Randevu> randevus { get; set; }
        public DbSet<UstalarinYapabildigiIslemler> ustalarinYapabildigiIslemlers { get; set; }
        public DbSet<Islemler> islemlers { get; set; }
    }
}
