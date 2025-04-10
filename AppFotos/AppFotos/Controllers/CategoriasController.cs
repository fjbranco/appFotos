﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppFotos.Data;
using AppFotos.Models;
using Microsoft.IdentityModel.Tokens;

namespace AppFotos.Controllers
{
    public class CategoriasController : Controller
    {
        /// <summary>
        /// Referência a base de dados
        /// </summary>
        private readonly ApplicationDbContext _context;

        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categorias.ToListAsync());
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorias = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categorias == null)
            {
                return NotFound();
            }

            return View(categorias);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            // mostra a View de nome "Create" que está na pasta "Categorias"
            return View(); 
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] // Responde a uma resposta do browser feita em POST
        [ValidateAntiForgeryToken] // Protecção contra ataques
        public async Task<IActionResult> Create([Bind("Categoria")] Categorias novaCategoria)
        {
            // Tarefas
            // - Ajustar o nome das variáveis
            // - Ajustar os anotadores, neste caso em concreto eliminar o Id do 'Bind'

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(novaCategoria);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    // throw;
                    ModelState.AddModelError("", "Aconteceu um erro a guardar os dados.");
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(novaCategoria);
        }

        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            // se chego aqui, há Categoria para editar

            // guardar os dados do objecto que vai ser  enviado paera o browser do utlizador
            HttpContext.Session.SetInt32("CategoriaID",categoria.Id);
            // mostrar a view
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, [Bind("Id,Categoria")] Categorias categoriaAlterada)
        {
            // a anotação FromRoute lê o valor do ID da rota
            // se houver alterações à rota, há alterações indevidas
            if (id != categoriaAlterada.Id)
            {
                //return NotFound();
                return RedirectToAction("Index");
            }

            // verificar se dados recebidos são correspondentes ao objecto enviado para o browser
            var categoriaID = HttpContext.Session.GetInt32("CategoriaID");
            var acao = HttpContext.Session.GetString("Acao");
            
            // demorou muito tempo
            if (categoriaID == null || acao.IsNullOrEmpty())
            {
                ModelState.AddModelError("", "Demorou muito tempo");
              /*  // guardar os dados do objecto que vai ser  enviado paera o browser do utlizador
                HttpContext.Session.SetInt32("CategoriaID", categoriaAlterada.Id);*/
                return View(categoriaAlterada);
            }
            // houve adulteração dos dados
            if(categoriaID != categoriaAlterada.Id)
            {
                // O utilizador está a tentar alterar outro objecto diferente do que receber
                return RedirectToAction("Index");
            }
            

            if (ModelState.IsValid)
            {
                try
                {
                    //guardar os dados aterados
                    _context.Update(categoriaAlterada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriasExists(categoriaAlterada.Id))
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
            return View(categoriaAlterada);
        }

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorias = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categorias == null)
            {
                return NotFound();
            }

            return View(categorias);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categorias = await _context.Categorias.FindAsync(id);
            if (categorias != null)
            {
                _context.Categorias.Remove(categorias);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriasExists(int id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }
    }
}
