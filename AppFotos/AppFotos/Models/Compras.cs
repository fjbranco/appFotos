using System.ComponentModel.DataAnnotations.Schema;

namespace AppFotos.Models
{
    /// <summary>
    /// Compras efectuadas por um utilizador
    /// </summary>
    public class Compras
    {
        /// <summary>
        /// Identificador da compra
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Data da compra
        /// </summary>
        public DateTime Data { get; set; }
        /// <summary>
        /// Estado da compra. Representa um conjunto de valores pré-determinados que representam a evolução da compra.
        /// </summary>
        public Estados Estado { get; set; }

        /******** Definição dos relacionamentos *********/

        // Relacionamentos 1-N

        /// <summary>
        /// FK para referenciar o comprador
        /// </summary>
        [ForeignKey(nameof(Comprador))]

        public int CompradorFK { get; set; }
        /// <summary>
        /// FK para referenciar o comprador
        /// </summary>
        public Utilizadores Comprador { get; set; }

        public ICollection<Fotografias> ListaFotografiasCompradas { get; set; }
    }
    /// <summary>
    /// Estados associados a uma 'compra'
    /// </summary>
    public enum Estados{ 
        Pendente, 
        Pago, 
        Enviada, 
        Entregue, 
        Terminada
    }

    
}
