using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using casasamaritanonorte.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace casasamaritanonorte.DAL
{
    public class CasaContext : DbContext
    {
        public CasaContext() : base("casasamaritanonorte")
        {
        }

        public DbSet<Captador> Captador { get; set; }
        public DbSet<Campanha> Campanha { get; set; }
        public DbSet<Evento> Evento { get; set; }
        public DbSet<Album> Album { get; set; }
        public DbSet<Foto> Foto { get; set; }
        public DbSet<Contato> Contato { get; set; }
        public DbSet<Pagamento> Pagamento { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}