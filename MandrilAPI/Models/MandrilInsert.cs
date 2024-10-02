using System.ComponentModel.DataAnnotations;

namespace MandrilAPI;


public class MandrilInsert
{
    [Required]
    [MaxLength(50)]
    public string Nombre { get; set; } = "";
    [Required]
    [MaxLength(50)]
    public string Apellido { get; set; } = "";
}

