using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCriminalistica.Models
{
    [Table("EXPEDIENTE")]
    public class Expediente
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int unidadCreacion { get; set; } // se usa para filtrar los datos de las distintas unidades
        public DateTime fechaExpte { get; set; }
        public string nroNota { get; set; }
        public string origenExpte { get; set; }
        public string extracto { get; set; }
        public string nroIntervencion { get; set; }
        public string informeTecnico { get; set; }
        public int peritoInterviniente { get; set; }
        public string tipoPericia { get; set; }
        public int estadoExpte { get; set; }
        public string? observacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioCrea { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int? usuarioModifica { get; set; }
        public DateTime? fechaBaja { get; set; }
        public DateTime? fechaBaja2 { get; set; }
        public int? usuarioBaja { get; set; }
        public Boolean activo { get; set; }
        public string numerointerno { get; set; }

        public string? personalInterviniente { get; set; }


        //cambios
        [ForeignKey("peritoInterviniente")]
        public virtual Peritos? perito { get; set; }

        [ForeignKey("estadoExpte")]
        public virtual Estados? estadoNavegacion { get; set; }

        [ForeignKey("usuarioCrea")]
        public virtual UsuarioCriminalistica? usuarioCreaNavegacion { get; set; }

        [ForeignKey("usuarioModifica")]
        public virtual UsuarioCriminalistica? usuarioModificaNavegacion { get; set; }

        [ForeignKey("usuarioBaja")]
        public virtual UsuarioCriminalistica? usuarioBajaNavegacion { get; set; }

    }
}
