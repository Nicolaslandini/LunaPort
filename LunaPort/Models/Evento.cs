using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LunaPort.Models
{
    public class Evento
    {
        [Key]
        public int IdEvento { get; set; }
        [Required]
        public String? Nombre { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayName("Fecha de evento")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}")]
        public DateTime Fecha { get; set; }
        [Required]
        [DisplayName("Estadio")]
        public int IdEstadio { get; set; }
        public int Participantes { get; set; }
    }
}
