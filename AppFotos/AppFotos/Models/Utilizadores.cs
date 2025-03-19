using System.ComponentModel.DataAnnotations;

namespace AppFotos.Models
{
    /// <summary>
    /// Utilizadores não anónimos da aplicação
    /// </summary>
    public class Utilizadores
    {
        /// <summary>
        /// Identificador do utilizador
        /// </summary>
        [Key]
        public int Id { get; set; }
        // Nome do utilizador
        [Display(Name ="Nome")]
        [StringLength(50)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        public string Nome { get; set; }
        /// <summary>
        /// Morada do utilizador
        /// </summary>
        [Display(Name = "Morada")]
        [StringLength(50)]
        public string Morada { get; set; }
        /// <summary>
        /// Código postal da morada do utilizador
        /// </summary>
        [Display(Name ="Código Postal")]
        [StringLength(50)]
        [RegularExpression("[1-9][0-9]{3}-[0-9]{3} [A-Za-z][A-Za-z ]+", ErrorMessage ="No {0} só são aceites algarismos e letras inglesas.")]
        public string CodPostal { get; set; }
        //[RegularExpression("[1-9][0-9]{3,4}-[0-9]{3,4}( [A-Za-z][A-Za-z ]*)?")]
        /// <summary>
        /// País da morada do utilizador
        /// </summary>
        [Display(Name="País")]
        [StringLength(50)]
        public string Pais { get; set; }
        // Número de NIF do utilizador
        /// </summary>
        [Display(Name = "NIF")]
        [StringLength(9)]
        [RegularExpression("[1-9][0-9]{8}",ErrorMessage="Deve escrever um {0} válido.")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        public string NIF { get; set; }
        /// <summary>
        /// Número de telemóvel do utilizador
        /// </summary>
        /// </summary>
        [Display(Name = "Telemóvel")]
        [StringLength(16)]
        [RegularExpression("(([+]|00)[0-9]{1,5})?[1-9][0-9]{5,8}",ErrorMessage="Deve escrever um {0} válido.")]
        public string Telemovel { get; set; }

        /******** Definição dos relacionamentos *********/

        /// <summary>
        /// Lista das fotografias que são propriedade do utilizador
        /// </summary>
        public ICollection<Fotografias> ListaFotos { get; set; }

        /// <summary>
        /// Lista de gostos das fotografias do utilizador
        /// </summary>
        public ICollection<Gostos> ListaGostos { get; set; }
        /// <summary>
        /// Lista de compras de fotografias do utilizador
        /// </summary>
        public ICollection<Compras> ListaCompras { get; set; }

    }
}
