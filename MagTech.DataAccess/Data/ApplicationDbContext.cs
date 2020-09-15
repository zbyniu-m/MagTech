using System;
using System.Collections.Generic;
using System.Text;
using MagTech.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MagTech.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {            
        }
        public DbSet<BazaArtykulow> ZbiorArtykolow { get; set; }
        public DbSet<RaportOperacji> RaportyOperacji { get; set; }

        public DbSet<NaglowekKoszykZadania> NaglowekKoszykZadania { get; set; }
        public DbSet<KoszykZadanie> KoszykZadanie { get; set; }
        public DbSet<ZleceniaKosztowe> ZleceniaKosztowe { get; set; }


    }
}
