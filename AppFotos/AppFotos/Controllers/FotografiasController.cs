using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppFotos.Data;
using AppFotos.Models;

namespace AppFotos.Controllers
{
    public class FotografiasController : Controller
    {
        /// <summary>
        /// referência à base de dados
        /// </summary>
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public FotografiasController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Fotografias
        public async Task<IActionResult> Index()
        {
            /*  Interrogação à BD feita em LINQ <=> SQL
                
                Select *
                From Fotografias f inner join Categorias c on f.CategoriaFK=c.Id
                                   inner join Utilizadores u on f.DonoFK = u.Id
             */
            var listaFotografias = _context.Fotografias.Include(f => f.Categoria).Include(f => f.Dono);
            return View(await listaFotografias.ToListAsync());
        }

        // GET: Fotografias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            /*  Interrogação à BD feita em LINQ <=> SQL
                
                Select *
                From Fotografias f inner join Categorias c on f.CategoriaFK=c.Id
                                   inner join Utilizadores u on f.DonoFK = u.Id
                where f.Id = id
             */
            var fotografia = await _context.Fotografias
                .Include(f => f.Categoria)
                .Include(f => f.Dono)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fotografia == null)
            {
                return NotFound();
            }

            return View(fotografia);
        }

        // GET: Fotografias/Create
        public IActionResult Create()
        {
            // este é o primeiro método a ser chamado quando se pretende adicionar uma fotografia
            // estes contentores seervem para levar os dados das dropdowns para as Views
            // Select * from Categorias
            // Order by c.Categoria
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias.OrderBy(c=>c.Categoria), "Id", "Categoria");
            // Select * from Utilizadores
            // Order by c.Utilizador
            ViewData["DonoFK"] = new SelectList(_context.Utilizadores.OrderBy(u=>u.Nome), "Id", "Nome");
            return View();
        }

        // POST: Fotografias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo,Descricao,Ficheiro,Data,PrecoAux,CategoriaFK,DonoFK")] Fotografias fotografia, IFormFile imagemFoto)
        {
            // variáveis auxiliares
            bool haErro = false;
            string nomeImagem = "";

            // Avaliar se há Categoria
            if (fotografia.CategoriaFK <= 0)
             {
                haErro = true;
                // Erro. Não foi escolhida uma opção
                ModelState.AddModelError("", "Tem de escolher uma Categoria");
             }
            // Avaliar se há  Utilizador
            if (fotografia.DonoFK <= 0)
            {
                haErro = true;
                // Erro. Não foi escolhida uma opção
                ModelState.AddModelError("", "Tem de escolher um Dono");
             }
            /* Avaliar o ficheiro fornecido
             * - Há ficheiro?
             *  - Se não existir ficheiro, gerar mensagem de erro e devolver à view
             *  - Se existir,
             *   - será imagem?
             *    - se não for imagem, gerar mensagem de erro e devolver à view
             *    - se for imagem, 
             *     - determinar novo nome do ficheiro
             *     - guardar na BD o nome do ficheiro
             *     - guardar o ficheiro no disco rígido do servidor
             */



            // 
            if (imagemFoto == null)
            {
                // não há ficheiro
                haErro = true;
                // Erro. Não há ficheiro
                ModelState.AddModelError("", "Tem de submeter uma fotografia");
            }
            else
            {
                // há ficheiro, mas será imagem?
                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Guides/MIME_types
                if(imagemFoto.ContentType!="image/jpeg" && imagemFoto.ContentType!= "image/png")
                    
                {
                    // não há imagem
                    haErro = true;
                    // Erro. Não há imagem
                    ModelState.AddModelError("", "Tem de submeter uma fotografia");
                }
                else
                {
                    // há imagem
                    // novo nome para o ficheiro
                    Guid g = Guid.NewGuid();
                    nomeImagem = g.ToString();
                    string extensao = Path.GetExtension(imagemFoto.FileName).ToLowerInvariant();
                    nomeImagem += extensao;

                    // guardar este nome na BD
                    fotografia.Ficheiro = nomeImagem;


                }

            }


            // avalia se os dados estão de acordo com o Model
            if (ModelState.IsValid && !haErro)
            {
                // Transferir o valor do preço aux para o  Preco
                fotografia.Preco = Convert.ToDecimal(fotografia.PrecoAux.Replace('.', ',')/*,new CultureInfo("pt-PT")*/);

                // Adicionar os dados à nova fotografia
                _context.Add(fotografia);
                await _context.SaveChangesAsync();

                

                // guardar o ficheiro no disco rigido
                // determinar o local de armazenagem da imagem
                string localizacaoImagem = _webHostEnvironment.WebRootPath;
                localizacaoImagem = Path.Combine(localizacaoImagem,"imagens");

                if (!Directory.Exists(localizacaoImagem)){
                    Directory.CreateDirectory(localizacaoImagem);
                }

                    // gerar caminho completo para a imagem
                    nomeImagem = Path.Combine(localizacaoImagem, nomeImagem);
                // guardar a imagem
                using var stream = new FileStream(nomeImagem,FileMode.Create);
                await imagemFoto.CopyToAsync(stream);

                

                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias.OrderBy(c=>c.Categoria), "Id", "Categoria", fotografia.CategoriaFK);
            ViewData["DonoFK"] = new SelectList(_context.Utilizadores.OrderBy(u => u.Nome), "Id", "Nome", fotografia.DonoFK);
            // Ao chegar aqui é porque algo correu mal...
            return View(fotografia);
        }

        // GET: Fotografias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fotografia = await _context.Fotografias.FindAsync(id);
            if (fotografia == null)
            {
                return NotFound();
            }
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias.OrderBy(c => c.Categoria), "Id", "Categoria", fotografia.CategoriaFK);
            ViewData["DonoFK"] = new SelectList(_context.Utilizadores.OrderBy(c => c.Nome), "Id", "Nome", fotografia.DonoFK);
            return View(fotografia);
        }

        // POST: Fotografias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,Ficheiro,Data,Preco,CategoriaFK,DonoFK")] Fotografias fotografia)
        {
            if (id != fotografia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fotografia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FotografiasExists(fotografia.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaFK"] = new SelectList(_context.Categorias, "Id", "Categoria", fotografia.CategoriaFK);
            ViewData["DonoFK"] = new SelectList(_context.Utilizadores, "Id", "NIF", fotografia.DonoFK);
            return View(fotografia);
        }

        // GET: Fotografias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fotografia = await _context.Fotografias
                .Include(f => f.Categoria)
                .Include(f => f.Dono)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fotografia == null)
            {
                return NotFound();
            }

            return View(fotografia);
        }

        // POST: Fotografias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fotografia = await _context.Fotografias.FindAsync(id);
            if (fotografia != null)
            {
                _context.Fotografias.Remove(fotografia);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FotografiasExists(int id)
        {
            return _context.Fotografias.Any(e => e.Id == id);
        }
    }
}
