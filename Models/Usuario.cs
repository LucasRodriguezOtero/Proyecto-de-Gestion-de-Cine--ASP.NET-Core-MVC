using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_FINAL_GRUPO_C.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TP_FINAL_GRUPO_C.Models
{
    public class Usuario
    {
        public int ID { get; set; }
        public int DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        [EmailAddress(ErrorMessage = "El campo Correo no es una dirección de correo electrónico válida")]
        public string Mail { get; set; }
        public string Password { get; set; }
        public int IntentosFallidos { get; set; }
        public bool Bloqueado { get; set; }
        public double Credito { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public bool EsAdmin { get; set; }

        public ICollection<Funcion> MisFunciones { get; } = new List<Funcion>();
        public List<UsuarioFuncion> UserFuncion { get; set; }



        public Usuario() { }

        public Usuario(int DNI, string Nombre, string Apellido,string Mail, string Password, DateTime FechaNacimiento, bool EsAdmin)
        {
          
            this.DNI = DNI;
            this.Nombre = Nombre;
            this.Apellido = Apellido;
            this.Mail = Mail;
            this.Password = Password;
            IntentosFallidos = 0;
            Bloqueado = false;
            Credito = 0;
            this.FechaNacimiento = FechaNacimiento;
            this.EsAdmin = EsAdmin;

        }              

        public List<Funcion> ObtenerMisFunciones()
        {
            return MisFunciones.ToList();
        }

        public List<Funcion> MostrarFuncionesProximas()
        {
            List<Funcion> proximasFunciones = new List<Funcion>();

            DateTime fechaActual = DateTime.Now;

            foreach (Funcion funcion in MisFunciones)
            {
                if (funcion.Fecha > fechaActual)
                {
                    proximasFunciones.Add(funcion);
                }
            }

            return proximasFunciones;
        }

        public List<Funcion> MostrarFuncionesPasadas()
        {
            List<Funcion> pasadasFunciones = new List<Funcion>();

            DateTime fechaActual = DateTime.Now;

            foreach (Funcion funcion in MisFunciones)
            {
                if (funcion.Fecha < fechaActual)
                {
                    pasadasFunciones.Add(funcion);
                }
            }
            return pasadasFunciones;
        }

        public string[] ToString()
        {
            return new string[] { ID.ToString(), DNI.ToString(), Nombre, Apellido, Mail, Password, IntentosFallidos.ToString(), Bloqueado.ToString(), Credito.ToString(), FechaNacimiento.ToString("dd/MM/yyyy"), EsAdmin.ToString() };
        }
    }
}

