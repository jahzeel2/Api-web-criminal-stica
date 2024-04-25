using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCriminalistica.Models
{
    [Table("ROL")]
    public class Rol
    {
        [Key]
        public int id { get; set; }
        public string nombre { get; set; }
        public Boolean activo { get; set; }

    }
}
