using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TP_FINAL_GRUPO_C.Models;

namespace TP_FINAL_GRUPO_C.Models
{
    public class Funcion
    {
        public int ID { get; set; }
        public Sala MiSala { get; set; }
        public Pelicula MiPelicula { get; set; }
        public int idSala { get; set; }
        public int idPelicula { get; set; }
        public DateTime Fecha { get; set; }
        public int AsientosDisponibles { get; set; }
        
        // Cantidad de entradas compradas, Asientos Disponibles depende de la capacidad - este valor
        public int CantidadClientes { get; set; }
        public double Costo { get; set; }

        public ICollection<Usuario> Clientes { get;} = new List<Usuario>();
        public List<UsuarioFuncion> UserFuncion { get; set; }


        public Funcion() { }

        public Funcion( DateTime fecha, double costo, int idSala, int idPelicula, int AsientosDisponibles)
        {
            this.idSala = idSala;
            this.idPelicula = idPelicula;
            Fecha = fecha;
            this.CantidadClientes = 0;
            Costo = costo;
            this.AsientosDisponibles = AsientosDisponibles;
        }     

        public string[] ToString()
        {
            return new string[] { ID.ToString(), Fecha.ToString("dd/MM/yyyy"), AsientosDisponibles.ToString(), Costo.ToString(), idSala.ToString(), idPelicula.ToString() };

            #region To Strings no utilizados 
            /*
            if (MiSala != null && MiPelicula != null)
            {
                return new string[] { ID.ToString(), Fecha.ToString("dd/MM/yyyy"), AsientosDisponibles.ToString(), Costo.ToString(), MiSala.ID.ToString(), MiPelicula.ID.ToString(), MiPelicula.Nombre.ToString() };

            }
            else if (MiSala == null && MiPelicula != null)
            {
                return new string[] { ID.ToString(), Fecha.ToString("dd/MM/yyyy"), AsientosDisponibles.ToString(), Costo.ToString(), "", "", MiPelicula.ID.ToString(), MiPelicula.Nombre.ToString() };
            }
            else if (MiSala != null && MiPelicula == null)
            {
                return new string[] { ID.ToString(), Fecha.ToString("dd/MM/yyyy"), AsientosDisponibles.ToString(), Costo.ToString(), MiSala.ID.ToString(), MiSala.Capacidad.ToString(), "", "", AsientosDisponibles.ToString() };
            }
            else
            {
                return new string[] { ID.ToString(), Fecha.ToString("dd/MM/yyyy"), AsientosDisponibles.ToString(), Costo.ToString(), "", "", "", "", AsientosDisponibles.ToString() };

            }
            */
            #endregion

        }


    }

}

