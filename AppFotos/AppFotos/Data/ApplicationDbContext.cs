using AppFotos.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppFotos.Data;

/// <summary>
/// Esta classe representa a Base de Dados associada ao projecto
/// Se houver mais bases de dados, irão haver tantas classes deste tipo quanto as necessárias
/// 
/// Esta classe é equivalente a Create Schema/Create Database
/// </summary>
public class ApplicationDbContext : IdentityDbContext
{
    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="options"></param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    // especificar as tableas associadas à BD
    /// <summary>
    /// Tabela Categorias na DB
    /// </summary>
    public DbSet<Categorias> Categorias { get; set; }
    /// <summary>
    /// Tabela Utilizadores na DB
    /// </summary>
    public DbSet<Utilizadores> Utilizadores { get; set; }
    /// <summary>
    /// Tabela Fotografias na DB
    /// </summary>
    public DbSet<Fotografias> Fotografias { get; set; }
    /// <summary>
    /// Tabela Compras na DB
    /// </summary>
    public DbSet<Compras> Compras { get; set; }
    /// <summary>
    /// Tabela Gostos na DB
    /// </summary>
    public DbSet<Gostos> Gostos { get; set; }
}
