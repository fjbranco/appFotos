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
        public int Id { get; set; }
        // Nome do utilizador
        public string Nome { get; set; }
        /// <summary>
        /// Morada do utilizador
        /// </summary>
        public string Morada { get; set; }
        /// <summary>
        /// Código postal da morada do utilizador
        /// </summary>
        public string CodPostal { get; set; }
        /// <summary>
        /// País da morada do utilizador
        /// </summary>
        public string Pais { get; set; }
        // Número de NIF do utilizador
        public string NIF { get; set; }
        /// <summary>
        /// Número de telemóvel do utilizador
        /// </summary>
        public string Telemovel { get; set; }

    }
}
