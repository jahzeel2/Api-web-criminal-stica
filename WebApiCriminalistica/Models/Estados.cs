using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCriminalistica.Models
{
    [Table("ESTADO")]
    public class Estados
    {
        [Key]
        public int id { get; set; }
        public string nombre { get; set; }
        public int? unidadCreacion { get; set; }
        public Boolean activo { get; set; }

        //cambios
        [ForeignKey("unidadCreacion")]
        public virtual UnidadSistema? unidad { get; set; }
    }
    
}
