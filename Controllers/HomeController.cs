using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TP_FINAL_GRUPO_C.Models;
using Microsoft.AspNetCore.Authorization;

namespace TP_FINAL_GRUPO_C.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyContext _context;
        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        public async Task<IActionResult> Index(string pelicula, string ubicacion, DateTime? fecha, int precioMin, int precioMax)
        {
            if(User.Identity.IsAuthenticated) { 
            // Establecer la fecha de referencia, por defecto es la fecha actual
            DateTime fechaReferencia = fecha ?? DateTime.Now;

            // Obtener todas las funciones con fecha mayor o igual a la fecha de referencia
            var funcionesQuery = _context.Funciones
                .Include(f => f.MiPelicula)
                .Include(f => f.MiSala)
                .Where(f => f.Fecha.Date >= fechaReferencia.Date);

            // Aplicar otros filtros
            funcionesQuery = funcionesQuery
                .Where(f => string.IsNullOrWhiteSpace(pelicula) || f.MiPelicula.Nombre.Contains(pelicula))
                .Where(f => string.IsNullOrWhiteSpace(ubicacion) || f.MiSala.Ubicacion.Equals(ubicacion))
                .Where(f => precioMin == 0 || f.Costo >= precioMin)
                .Where(f => precioMax == 0 || f.Costo <= precioMax);

            // Obtener todas las películas y todas las salas
            var funciones = await funcionesQuery.ToListAsync();
            var peliculas = await _context.Peliculas.ToListAsync();
            var salas = await _context.Salas.ToListAsync();

            // Pasar datos a la vista
            ViewData["Funciones"] = funciones;
            ViewData["Peliculas"] = peliculas;
            ViewData["Salas"] = salas;

            return View();
            }
            else
            {
            return RedirectToAction("Login", "Usuarios");
            }
        }
        
        public async Task<IActionResult> VerFuncionesPelicula(int peliculaId)
        {
            var funciones = await _context.Funciones
                .Include(f => f.MiPelicula)
                .Include(f => f.MiSala)
                .Where(f => f.MiPelicula.ID == peliculaId && f.Fecha.Date >= DateTime.Now)
                .ToListAsync();

            ViewData["FuncionesFiltradas"] = funciones;
            return View("FiltroFunciones");
        }
        
        public async Task<IActionResult> VerFuncionesSala(int salaId)
        {
            var funciones = await _context.Funciones
                .Include(f => f.MiPelicula)
                .Include(f => f.MiSala)
                .Where(f => f.MiSala.ID == salaId && f.Fecha.Date >= DateTime.Now)
                .ToListAsync();

            ViewData["FuncionesFiltradas"] = funciones;
            return View("FiltroFunciones");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
