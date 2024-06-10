using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP_FINAL_GRUPO_C.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using TP_FINAL_GRUPO_C.Herramientas.Filtros;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;

namespace TP_FINAL_GRUPO_C.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly MyContext _context;

        public UsuariosController(MyContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        [Authorize]
        [TypeFilter(typeof(AdminAuthorizationFilter))]
        public async Task<IActionResult> Index()
        {
            return _context.Usuarios != null ?
                        View(await _context.Usuarios.ToListAsync()) :
                        Problem("Entity set 'MyContext.Usuarios'  is null.");
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.ID == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,DNI,Nombre,Apellido,Mail,Password,IntentosFallidos,Bloqueado,Credito,FechaNacimiento,EsAdmin")] Usuario usuario)
        {
            var mailCheck = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Mail == usuario.Mail);

            if (mailCheck != null)
            {
                ModelState.AddModelError(string.Empty, "Mail already in use");
                return View();
            }


            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Usuarios/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("ID,DNI,Nombre,Apellido,Mail,Password,IntentosFallidos,Bloqueado,Credito,FechaNacimiento,EsAdmin")] Usuario usuario)
        {

            var mailCheck = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Mail == usuario.Mail);

            if (mailCheck != null)
            {
                ModelState.AddModelError(string.Empty, "Mail already in use");
                return View();
            }


            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(usuario);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string mail, string password)
        {
            if (string.IsNullOrEmpty(mail) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Mail and password are required.");
                return View();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Mail == mail);

            
            if (usuario != null && usuario.Password == password)
            {
                // Check if the user is blocked
                if (usuario.Bloqueado || usuario.IntentosFallidos > 3)
                {
                    ModelState.AddModelError(string.Empty, "This account is blocked. Please contact support.");
                    return View();
                }

                // En el caso de que el usuario entre correctamente se le reestablecen los intentos
                usuario.IntentosFallidos = 0;
                await _context.SaveChangesAsync();

                // Redirect to a logged-in page or perform other actions
                AuthenticationAsync(usuario);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Increment failed login attempts and block if necessary
                ModelState.AddModelError(string.Empty, "Invalid mail or password.");

                if (usuario != null && usuario.Bloqueado == false)
                {
                    usuario.IntentosFallidos++;
                    if (usuario.IntentosFallidos > 3)
                    {
                        // Block the user if failed attempts reach 5
                        ModelState.AddModelError(string.Empty, "Intentos fallidos > 3.Se bloqueo el usuario");
                        BloquearUsuario(usuario);
                    }
                }
  
                _context.SaveChanges();             
                return View();
            }
        }

        private async Task<IActionResult> AuthenticationAsync(Usuario usuario)
        {

            //PasswordHasher passwordHasher = new PasswordHasher();
            //var checking = passwordHasher.Check("hash-pwd-bd", "password-user");

            var claims = new List<Claim>() { new Claim("ID", usuario.ID.ToString()), new Claim("Nombre", usuario.Nombre), new Claim("Mail", usuario.Mail), new Claim("EsAdmin", usuario.EsAdmin.ToString()) };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties()
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(10),
                    AllowRefresh = true,
                    IsPersistent = false
                });


            await Task.Delay(TimeSpan.FromMinutes(10));

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        private void BloquearUsuario(Usuario usuario)
        {
            usuario.Bloqueado = true;
            _context.SaveChanges();
        }


        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,DNI,Nombre,Apellido,Mail,Password,IntentosFallidos,Bloqueado,Credito,FechaNacimiento,EsAdmin")] Usuario usuario)
        {
            if (id != usuario.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.ID))
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.ID == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'MyContext.Usuarios'  is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return (_context.Usuarios?.Any(e => e.ID == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> ComprarEntrada(int idFuncion, int cantidadEntradas)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Usuarios");
            }

            string userId = User.FindFirst("ID")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Usuarios");
            }

            var usuario = await _context.Usuarios.FindAsync(Convert.ToInt32(userId));
            if (usuario == null)
            {
                return NotFound();
            }

            var funcion = await _context.Funciones.FirstOrDefaultAsync(f => f.ID == idFuncion);
            if (funcion == null)
            {
                return NotFound();
            }

            string ErrorMessage = " ";
            int StatusCode = 200;

            if (funcion.Fecha <= DateTime.Now)
            {
                ErrorMessage = "No se pueden comprar entradas para funciones pasadas.";
                StatusCode = 405;
                ModelState.AddModelError(string.Empty, "No se pueden comprar entradas para funciones pasadas.");
            }

            if (funcion.AsientosDisponibles < cantidadEntradas)
            {
                ErrorMessage = "No hay suficientes asientos disponibles para esta función.";
                StatusCode = 405;
                ModelState.AddModelError(string.Empty, "No hay suficientes asientos disponibles para esta función.");
            }

            double costoTotal = funcion.Costo * cantidadEntradas;

            if (usuario.Credito < costoTotal)
            {
                ErrorMessage = "No tienes suficiente crédito para comprar estas entradas.";
                StatusCode = 402;
                ModelState.AddModelError(string.Empty, "No tienes suficiente crédito para comprar estas entradas.");
            }

            if (!ModelState.IsValid)
            {
                // return Json(new { success = false, redirectUrl = Url.Action("Error", "Error", new { errorMessage = ErrorMessage, statusCode = StatusCode }) });
                return Json(new { statusCode = StatusCode, errorMessage = ErrorMessage });
            }

            var usuarioFuncionExistente = _context.UF.Where(f => f.idUsuario == usuario.ID && f.idFuncion == funcion.ID).FirstOrDefault();

            if (usuarioFuncionExistente != null)
            {
                // If an entry exists, update the quantity
                usuarioFuncionExistente.CantidadEntradasCompradas += cantidadEntradas;
                _context.Update(usuarioFuncionExistente);
            }
            else
            {
                var ultimoIdUsuarioFuncion = _context.UF
               .OrderByDescending(uf => uf.ID)
               .FirstOrDefault()?.ID;

                usuario.MisFunciones.Add(funcion);

                //Create([Bind("ID,idUsuario,idFuncion,CantidadEntradasCompradas")] UsuarioFuncion usuarioFuncion)
                UsuarioFuncion entrada = new UsuarioFuncion((Convert.ToInt32(ultimoIdUsuarioFuncion) + 1), usuario.ID, funcion.ID, cantidadEntradas);

                // Agregar la nueva entrada UsuarioFuncion al contexto
                _context.UF.Add(entrada);
            } 

            funcion.AsientosDisponibles -= cantidadEntradas;
            usuario.Credito -= costoTotal;

            // Actualizar el estado del usuario y la función
            _context.Update(usuario);
            _context.Update(funcion);

            try
            {
                // Guardar todos los cambios en el contexto
                await _context.SaveChangesAsync();
                return Json(new { statusCode = StatusCode });
            }
            catch (DbUpdateException ex)
            {
                // Manejar la excepción DbUpdateException
                return Json(new { statusCode = 500, errorMessage = "Error al guardar los cambios en la base de datos." });
            }



        }

        public async Task<IActionResult> DevolverEntrada(int idfuncion, int idUsuario)
        {
            Console.WriteLine("estoy en devolver entrada");

            var usuarioFuncion = _context.UF.Where(uf => uf.idUsuario == idUsuario && uf.idFuncion == idfuncion).FirstOrDefault();

            //var usuarioFuncion = await _context.UF.FindAsync(idUsuarioFuncion);

            if (usuarioFuncion == null)
            {
                return NotFound();
            }

            var usuario = _context.Usuarios.Where(u => u.ID == idUsuario).FirstOrDefault();
            var funcion = _context.Funciones.Where(f => f.ID == idfuncion).FirstOrDefault();


            if (usuario == null || funcion == null)
            {
                return NotFound();
            }

            // Incrementar los asientos disponibles
            funcion.AsientosDisponibles += usuarioFuncion.CantidadEntradasCompradas;

            // Incrementar el crédito del usuario
            double costoTotal = funcion.Costo * usuarioFuncion.CantidadEntradasCompradas;
            usuario.Credito += costoTotal;

            // Remover la entrada de UsuarioFuncion
            _context.UF.Remove(usuarioFuncion); // para MI ACA EXPLOTA

            // Actualizar el estado del usuario y la función
            _context.Update(usuario);
            _context.Update(funcion);

            try
            {
                // Guardar todos los cambios en el contexto
                await _context.SaveChangesAsync();
                return Json(new { statusCode = 200 });
            }
            catch (DbUpdateException ex)
            {
                // Manejar la excepción DbUpdateException
                return Json(new { statusCode = 500, errorMessage = "Error al guardar los cambios en la base de datos." });
            }
        }

        // RedirectToAction("Index");
    }



}