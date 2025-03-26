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
        private readonly ApplicationDbContext _context;

        public FotografiasController(ApplicationDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> Create([Bind("Titulo,Descricao,Ficheiro,Data,PrecoAux,CategoriaFK,DonoFK")] Fotografias fotografia)
        {
            // variáveis auxiliares
            bool haErro = false;
            
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
            // avalia se os dados estão de acordo com o Model
            if (ModelState.IsValid && !haErro)
            {
                // Transferir o valor do preço aux para o  Preco
                fotografia.Preco = Convert.ToDecimal(fotografia.PrecoAux.Replace('.',',')/*,new CultureInfo("pt-PT")*/);

                // Adicionar os dados à nova fotografia
                _context.Add(fotografia);
                await _context.SaveChangesAsync();
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
