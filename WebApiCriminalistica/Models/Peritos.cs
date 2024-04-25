using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCriminalistica.Models
{
    [Table("PERITOS")]
    public class Peritos
    {
        [Key]
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public int dni { get; set; }
        public string tipoPersona { get; set; } // POLICIA O CIVIL
        public int? idPersonalPolicial { get; set; } // SE AGREGA SI EL PERITO ES PERSONAL POLICIAL
        public int? idPersonalCivil { get; set; }
        public string tipoPerito { get; set; }
        public DateTime fechaAlta { get; set; }
        public int usuarioAlta {get; set; }
        public DateTime? fechaBaja { get; set; }
        public int? usuarioBaja { get; set; }
        public int unidadCreacion { get; set; }
        public Boolean activo { get; set; }


        [ForeignKey("usuarioAlta")]
        public virtual UsuarioCriminalistica? usuarioAltaNavegacion { get; set; }
        
        [ForeignKey("usuarioBaja")]
        public virtual UsuarioCriminalistica? usuarioBajaNavegacion { get; set; }

    }

}
