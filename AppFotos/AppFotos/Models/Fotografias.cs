﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppFotos.Models
{
    /// <summary>
    /// Fotografias disponíveis na aplicação
    /// </summary>
    public class Fotografias
    {
        /// <summary>
        /// identificador da fotografia
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Título da fotografia
        /// </summary>
        public string Titulo { get; set; }
        /// <summary>
        /// Descrição da fotografia
        /// </summary>
        public string Descricao { get; set; }
        /// <summary>
        /// Nome do ficheiro da fotografia no disco rígido do servidor
        /// </summary>
        public string Ficheiro { get; set; }
        /// <summary>
        /// Data em que a fotografia foi tirada
        /// </summary>
        [Display(Name = "Data")]
        [DataType(DataType.Date)] // transforma o attributo na BD em "Date"
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}",ApplyFormatInEditMode =true)]
        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório.")]
        public DateTime Data { get; set; }
        /// <summary>
        /// Preço de venda da fotografia
        /// </summary>
        [Display(Name = "Preço")]
        public decimal Preco { get; set; }

        /******** Definição dos relacionamentos *********/
        
        // Relacionamentos 1-N
        
        /// <summary>
        /// FK para a tabela de categorias
        /// </summary>
        [ForeignKey(nameof(Categoria))]
        public int CategoriaFK { get; set; }
        /// <summary>
        /// FK para as Categorias
        /// </summary>
        public Categorias Categoria { get; set; }

        /// <summary>
        /// FK para referenciar o Dono da fotografia
        /// </summary>
        [ForeignKey(nameof(Dono))]
        public int DonoFK { get; set; }
        /// <summary>
        /// FK para referenciar o Dono da fotografia
        /// </summary>
        public Utilizadores Dono { get; set; }


        // Relacionamentos M-N

        /// <summary>
        /// Lista de gostos de uma fotografia
        /// </summary>
        public ICollection<Gostos> ListaGostos { get; set; }
        /// <summary>
        /// Lista de compras que compraram uma fotografia
        /// </summary>
        public ICollection<Compras> ListaCompras { get; set; }

    }
}
