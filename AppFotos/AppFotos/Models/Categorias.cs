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
        public int Id { get; set; }
        /// <summary>
        /// Nome da categoria
        /// </summary>
        public string Categoria { get; set; }

        /******** Definição dos relacionamentos *********/

        // Relacionamentos -

        public ICollection<Fotografias>
    }
}
