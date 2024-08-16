using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaNote.BancoDados;
using SistemaNote.Models;


namespace SistemaNote.Controllers
{
    public class NotasController : Controller
    {
        private readonly ClaseContext _context;

        public NotasController(ClaseContext context)
        {
            _context = context;
        }

        //Metodo chama a view para inserir a senha 
        //------------------------------------------------------

        [HttpGet]
        public IActionResult RequererSenha()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RequererSenha(string RetornaUrl)
        {
            var model = new SenhaModel  //Instancia a ViewModel 
            {
                ReturnUrl = RetornaUrl // Preenche a ViewModel com a URL que o usuário tentou acessar
            };
            return View(model); // Retorna a view com o formulário de senha
        }

        //-------------------------------------------------------------

        //-------------------------------------------------------------
        [HttpPost]
        public IActionResult ValidarSenha(SenhaModel model)
        {
            const string SenhaCorreta = "841626"; // Defina sua senha aqui

            if (model.senha == SenhaCorreta)
            {
                HttpContext.Session.SetString("PasswordVerified", "true"); // Armazena na sessão que a senha foi verificada com sucesso
                return Redirect(model.ReturnUrl);                          // Redireciona o usuário para a URL original
            }

            ModelState.AddModelError(string.Empty, "Senha incorreta.");  
            return View("RequererSenha", model);  
        }

        //-------------------------------------------------------------

        // GET: Notas
        public async Task<IActionResult> Index()  
        {
            return View(await _context.notas.ToListAsync());
        }

      
        // GET: Notas/Create
        public IActionResult Create() //senha
        {
            return View();
        }

        // POST: Notas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Titulo,Conteudo")] Notas notas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notas);
        }

        // GET: Notas/Edit/5

        [HttpPost]
        public async Task<IActionResult> Edit(int? id)
        {
            // Verifica se a senha foi verificada anteriormente na sessão
            if (!HttpContext.Session.TryGetValue("PasswordVerified", out _))
            {
                // Redireciona o usuário para a página de solicitação de senha
                return RedirectToAction("RequererSenha", new { returnUrl = Url.Action("Edit", new { id }) });
            }


            if (id == null)
            {
                return NotFound();
            }

            var notas = await _context.notas.FindAsync(id);
            if (notas == null)
            {
                return NotFound();
            }


            return View(notas);
        }

        // POST: Notas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Titulo,Conteudo")] Notas notas)
        {
            if (id != notas.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotasExists(notas.id))
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
            return View(notas);
        }

        // GET: Notas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notas = await _context.notas
                .FirstOrDefaultAsync(m => m.id == id);
            if (notas == null)
            {
                return NotFound();
            }

            return View(notas);
        }

        // POST: Notas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notas = await _context.notas.FindAsync(id);
            if (notas != null)
            {
                _context.notas.Remove(notas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotasExists(int id)
        {
            return _context.notas.Any(e => e.id == id);
        }
    }
}
