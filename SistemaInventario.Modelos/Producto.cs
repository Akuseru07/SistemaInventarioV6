using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Numero de Serie es Requerido")]
        [MaxLength(60)]
        public string NumeroSerie { get; set; }
        [Required(ErrorMessage = "Descripcion es Requerida")]
        [MaxLength(60)]
        public string Descripcion { get; set; }
        [Required(ErrorMessage ="Precio es requerido")]
        public double Precio { get; set; }
        [Required(ErrorMessage = "Costo es requerido")]
        public double Costo { get; set; }

        //La imagen no es requerida ya que se guarda cuando estamos grabando el producto
        public string ImagenUrl { get; set; }
        [Required(ErrorMessage ="Estado es requerido")]
        public bool Estado { get; set; }

        [Required(ErrorMessage ="Categoria es Requerido")]
        //Para poder hacer la foreign key, se hace esto para poder obtener las propiedades de categoria
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }

        [Required(ErrorMessage ="Marca es Requerido")]
        public int MarcaId { get; set; }
        [ForeignKey("MarcaId")]
        public Marca Marca { get; set; }

        //Relacion de recursividad, coy a tener una propiedad padre, la cual se va a relacionar con un mismo ID
        //de mi modelo
        //El int? es para que se grabe como nulo obligatoriamente

        public int? PadreId { get; set; }
        //Navegacion
        public virtual Producto Padre { get; set; } //un producto puede estar relacionado a un mismo producto y ese va a ser el padre

    }
}
