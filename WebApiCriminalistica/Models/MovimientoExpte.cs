using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCriminalistica.Models
{
    [Table("MOVIMIENTOEXPTE")]
    public class MovimientoExpte
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int expte { get; set; }
        public int? destinoPolicial { get; set; }
        public string? destinoNoPolicial { get; set; }
        public DateTime fechaEnvio { get; set; }
        public int? usuarioEnvia{ get; set; }
        public DateTime? fechaRecepcion { get; set; }
        public int? usuarioRecibe { get; set; }
        public string tipoMovimiento {get; set; }
        public string? observaciones { get; set; }
        //public string? estadoExpte { get; set; }
        public Boolean activo { get; set; }

        [ForeignKey("expte")]
        public virtual Expediente? expedienteNavegacion { get; set; }
        [ForeignKey("usuarioEnvia")]
        public virtual UsuarioCriminalistica? usuarioEnviaNavegacion { get; set; }
        [ForeignKey("usuarioRecibe")]
        public virtual UsuarioCriminalistica? usuarioRecibeNavegacion { get; set; }
    }
}
