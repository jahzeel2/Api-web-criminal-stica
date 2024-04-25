using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCriminalistica.Models
{
    [Table("USUARIOCRIMINALISTICA")]
    public class UsuarioCriminalistica
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int userCreaRepo { get; set; }
        [Required]
        public int usuarioRepo { get; set; }
        public DateTime fechaAlta { get; set; }
        public int? persona { get; set; }
        public int? civil { get; set; }
        public int norDni { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public Boolean tipoPersona { get; set; }
        public DateTime? fechaBaja { get; set; }
        public int? usuarioBaja { get; set; }
        public Boolean baja { get; set; }
        public int sistema { get; set; } //unidadsistema
        public string? cifrado { get; set; }
        public DateTime? fechaVinculacion { get; set; }
        public int rol { get; set; }
        public Boolean activo { get; set; }


        [ForeignKey("sistema")]
        public virtual UnidadSistema? unidadSistemaNavigation { get; set; }
        [ForeignKey("rol")]
        public virtual Rol? rolNavigation { get; set; }
    }
}
