using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppFotos.Models
{
    /// <summary>
    /// Regista os 'gostos' que um utilizador da app tem
    /// pelas fotografias existentes
    /// </summary>
    [PrimaryKey(nameof(UtilizadorFK),nameof(FotografiaFK))]
    public class Gostos
    {
        /// <summary>
        /// Data em que o utilizador marcou gosto de uma fotografia
        /// </summary>
        public DateTime Data { get; set; }


        /******** Definição dos relacionamentos *********/

        // Relacionamentos 1-N

        /// <summary>
        /// FK para referenciar o uitlizador que gosta da fotografia
        /// </summary>
        [ForeignKey(nameof(Utilizador))]
        public int UtilizadorFK { get; set; }
        /// <summary>
        /// FK para referenciar o uitlizador que gosta da fotografia
        /// </summary>
        public Utilizadores Utilizador { get; set; }

        // Relacionamentos 1-N
        /// <summary>
        /// FK para referenciar a fotografia
        /// </summary>
        [ForeignKey(nameof(Fotografia))]
        public int FotografiaFK { get; set; }
        /// <summary>
        /// FK para referenciar a fotografia
        /// </summary>
        public Fotografias Fotografia { get; set; }

    }
}
