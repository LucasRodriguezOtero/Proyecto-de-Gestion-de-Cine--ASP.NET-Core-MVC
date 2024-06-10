using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TP_FINAL_GRUPO_C.Models
{
    public class StatusCode
    {
        public static string ObtenerMensaje(int codigo)
        {
            switch (codigo)
            {
                case 200:
                    return "La peticion se realizo correctamente";
                case 204:
                    return "Eliminado correctamente";
                case 201:
                    return "Usuario Registrado con exito! Revise su email para validar cuenta";
                case 400:
                    return "Petición inválida, revise parametros";
                case 401:
                    return "Email o Password incorrecta";
                case 403:
                    return "Por favor, inicie session";
                case 404:
                    return "No se pudo encontrar la informacion solicitada";
                case 422:
                    return "Complete todos los campos";
                case 423:
                    return "No se puede acceder, el usuario se encuentra bloqueado";
                case 429:
                    return "Ha alcanzado la cantidad de intentos. Usuario bloqueado";
                case 500:
                    return "Error en la peticion, intentelo nuevamente";
                default:
                    return "Estado desconocido";
            }
        }
   
         
    }
}
