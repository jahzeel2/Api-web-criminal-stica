using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiCriminalistica.Models;

namespace WebApiCriminalistica.Data
{
    public class WebApiCriminalisticaContext : DbContext
    {
        public WebApiCriminalisticaContext (DbContextOptions<WebApiCriminalisticaContext> options)
            : base(options)
        {
        }

        //convencion para que los datos datetime sean todos de tipo date
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTime>().HaveColumnType("date");
        }

        public DbSet<WebApiCriminalistica.Models.Estados> Estados { get; set; } = default!;

        public DbSet<WebApiCriminalistica.Models.Rol> Rol { get; set; }

        public DbSet<WebApiCriminalistica.Models.UnidadSistema> UnidadSistema { get; set; }

        public DbSet<WebApiCriminalistica.Models.UsuarioCriminalistica> UsuarioCriminalistica { get; set; }

        public DbSet<WebApiCriminalistica.Models.Expediente> Expediente { get; set; }

        public DbSet<WebApiCriminalistica.Models.MovimientoExpte> MovimientoExpte { get; set; }

        public DbSet<WebApiCriminalistica.Models.Peritos> Peritos { get; set; }
    }
}
