using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class UsuarioAplicacion: IdentityUser
    {
        [Required(ErrorMessage ="Nombre es requerido")]
        [MaxLength(80)]
        public string Nombres { get; set; }
        [Required(ErrorMessage = "Apellido es requerido")]
        [MaxLength(80)]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "Direccion es requerida")]
        [MaxLength(200)]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "Ciudad es requerida")]
        [MaxLength(60)]
        public string Ciudad { get; set; }
        [Required(ErrorMessage = "Pais es requerido")]
        [MaxLength(60)]
        public string Pais { get; set; }


        //Propiedad para referencia y que no se una a la base de datos
        [NotMapped]
        public string Role { get; set; }

    }
}
