using System.ComponentModel.DataAnnotations;

namespace AppFotos.Models
{
    /// <summary>
    /// Categorias a que as fotografias podem ser associadas
    /// </summary>
    public class Categorias
    {
        /// <summary>
        /// Identificador da categoria
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Nome da categoria
        /// </summary>
        [Required(ErrorMessage ="A {0} é de preenchimento obrigatório.")]
        [StringLength(20)]
        [Display(Name = "Categoria")]
        public string Categoria { get; set; }

        /******** Definição dos relacionamentos *********/

        // Relacionamentos -

        /// <summary>
        /// Lista das fotografias associadas a uma categoria
        /// </summary>
        public ICollection<Fotografias> ListaFotografias { get; set; }
    }
}
