using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_FINAL_GRUPO_C.Models
{
    public class Sala
    {

        public int ID { get; set; }
        public string Ubicacion { get; set; }
        public int Capacidad { get; set; }

        public List<Funcion> MisFunciones { get; } = new List<Funcion> { };

        public Sala() { }

        public Sala(string Ubicacion, int Capacidad)
        {
            this.Ubicacion = Ubicacion;
            this.Capacidad = Capacidad;
        }

        public List<Funcion> ObtenerMisFunciones()
        {
            return MisFunciones.ToList();
        }

        public string[] SalaToString()
        {

            return new string[] { ID.ToString(), Ubicacion.ToString(), Capacidad.ToString() };
        }

    }
}
