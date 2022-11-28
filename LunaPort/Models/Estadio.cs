using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LunaPort.Models
{
    public class Estadio
    {



        [Key]
        
        public int Id{ get; set; }

        public string Nombre { get; set; }

        [Display(Name = "Capacidad Máxima")]
        public int CapacidadMax { get; set; }


    }

}
