namespace TP_FINAL_GRUPO_C.Herramientas.Filtros
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    public class AdminAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!EsAdmin(context.HttpContext.User))
            {
                // Si el usuario no es un administrador, redirigir al controlador de errores con un mensaje
                context.Result = new RedirectToActionResult("Error", "Error", new { statusCode = 404 , errorMessage = "Página no encontrada" });
            }
        }

        private bool EsAdmin(System.Security.Claims.ClaimsPrincipal user)
        {
            if (user.HasClaim(c => c.Type == "EsAdmin" && c.Value == "True"))
            {
                return true;
            }
            return false;

        }
    }
}
