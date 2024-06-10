using Microsoft.AspNetCore.Mvc;
using TP_FINAL_GRUPO_C.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using TP_FINAL_GRUPO_C.Herramientas.Filtros;

namespace TP_FINAL_GRUPO_C.Controllers
{
    
    public class MiPerfilController : Controller
    {
        private readonly ILogger<MiPerfilController> _logger;
        private readonly MyContext _context;

        public MiPerfilController(ILogger<MiPerfilController> logger, MyContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst("ID")?.Value;

                if (!string.IsNullOrEmpty(userId))
                {

                    var usuario = await _context.Usuarios
                      .Include(u => u.MisFunciones)
                      .Where(u => u.ID == Convert.ToInt32(userId))
                      .FirstOrDefaultAsync();



                    if (usuario != null)
                    {


                        var funcionesIds = usuario.MisFunciones.Select(f => f.ID).ToList();
                      

                        var funcionesProximas = await _context.Funciones
                            .Include(f => f.MiPelicula)
                            .Include(f => f.MiSala)
                            .Where(f => funcionesIds.Contains(f.ID) && f.Fecha.Date >= DateTime.Today)
                            .ToListAsync();

                        var funcionesPasadas = await _context.Funciones
                            .Include(f => f.MiPelicula)
                            .Include(f => f.MiSala)
                            .Where(f => funcionesIds.Contains(f.ID) && f.Fecha.Date < DateTime.Today)
                            .ToListAsync();

                        ViewData["entradasPorFuncion"] = _context.UF.ToList();


                        ViewData["funcionesProximas"] = funcionesProximas;
                        ViewData["funcionesPasadas"] = funcionesPasadas;

                        return View(usuario);
                    }
                }
            }

            return RedirectToAction("Login", "Usuarios");
        }


      
        public async Task<IActionResult> Edit()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst("ID")?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.ID == Convert.ToInt32(userId));

                    if (usuario != null)
                    {
                        return View(usuario);
                    }
                }
            }

            return RedirectToAction("Login", "Usuarios");
        }

        [HttpPost]
        public async Task<IActionResult> Update(Usuario usuarioActualizado)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userId = User.FindFirst("ID")?.Value;

                    if (!string.IsNullOrEmpty(userId))
                    {
                        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.ID == Convert.ToInt32(userId));

                        if (usuario != null)
                        {
                            usuario.Nombre = usuarioActualizado.Nombre;
                            usuario.Apellido = usuarioActualizado.Apellido;
                            usuario.DNI = usuarioActualizado.DNI;
                            usuario.FechaNacimiento = usuarioActualizado.FechaNacimiento;

                            _context.Update(usuario);
                            await _context.SaveChangesAsync();

                            return RedirectToAction("Index");
                        }
                    }
                }
            }

            return View("Editar", usuarioActualizado);
        }


        public IActionResult BuyCredit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcesarCargarCredito(double monto)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst("ID")?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.ID == Convert.ToInt32(userId));

                    if (usuario != null)
                    {
                        usuario.Credito += monto;
                        _context.Update(usuario);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("Index");
                    }
                }
            }

            return RedirectToAction("Login", "Usuarios");
        }


    }
}
