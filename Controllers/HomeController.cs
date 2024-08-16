using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaNote.BancoDados;
using SistemaNote.Models;
using System.Diagnostics;

namespace SistemaNote.Controllers
{
	public class HomeController(ClaseContext context) : Controller
	{
		
        private readonly ClaseContext _context = context;

        public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		public IActionResult Notas(string NomeNota)
		{
			ViewData["NomeDaNota"] = NomeNota;

           var Nota = _context.notas.FirstOrDefault(n =>n.Titulo == NomeNota);
			if (Nota == null)
			{
				return NotFound();  // Retorna 404 se não encontrar a nota
            }

			return View(Nota);
		}

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
