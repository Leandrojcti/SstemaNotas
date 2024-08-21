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

        //----------------------------------------------------------------------------------------------
        [HttpGet]
        public IActionResult RequererSenha(string returnUrl)
        {
            var model = new SenhaModel  //Instancia a ViewModel 
            {
                ReturnUrl = returnUrl // Preenche a ViewModel com a URL que o usuário tentou acessar
            };
            return View(model); // Retorna a view com o formulário de senha
        }

        //--------------------------------------------------------------------------------------------

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

        //---------------------------------------------------------------------------------------------------

        public IActionResult EncerrarSessao()
        {
            HttpContext.Session.Clear(); // Limpa todos os dados da sessão
            return RedirectToAction("Index", "Home"); // Redireciona para uma página após limpar a sessão
        }

        //----------------------------------------------------------------------------------------------------

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


        //--------------------------------------------------------------------------------------------------------------
        // GET: Notas/Edit/5

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
                   //script para proteger a action 
            //--------------------------------------------------------------------------------------------------------------
            // Verifica se a senha foi verificada anteriormente na sessão
            if (!HttpContext.Session.TryGetValue("PasswordVerified", out _))
            {
                // Redireciona o usuário para a página de solicitação de senha
                //new = preenche a propriedade ReturUrl com a url do edit 
                return RedirectToAction("RequererSenha", new { ReturnUrl = Url.Action("Edit","Notas",  new { id }) });
            }
            //---------------------------------------------------------------------------------------------------------------

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
        // Post vem da ação submeter no formulario
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
            // Verifica se a senha foi verificada anteriormente na sessão
            if (!HttpContext.Session.TryGetValue("PasswordVerified", out _))
            {
                // Redireciona o usuário para a página de solicitação de senha
                //new = preenche a propriedade ReturUrl com a url do edit 
                return RedirectToAction("RequererSenha", new { ReturnUrl = Url.Action("Delete", "Notas", new { id }) });
            }

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
