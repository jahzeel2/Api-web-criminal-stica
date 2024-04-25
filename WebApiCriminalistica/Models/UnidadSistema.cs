using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace WebApiCriminalistica.Models
{
    [Table("UNIDADSISTEMA")]
    public class UnidadSistema
    {
        [Key]
        public int id { get; set; }
        public string nombre { get; set; }
    }
}
