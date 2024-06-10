using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_FINAL_GRUPO_C.Models
{
    public class Pelicula
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Sinopsis { get; set; }
        public string Poster { get; set; }
        public int Duracion { get; set; }

        public List<Funcion> MisFunciones { get;} = new List<Funcion> { };

        public Pelicula() { }

        public Pelicula(string Nombre, string Descripcion, string Sinopsis, string Poster, int Duracion)

        {
            this.Nombre = Nombre;
            this.Descripcion = Descripcion;
            this.Sinopsis = Sinopsis;
            this.Poster = Poster;
            this.Duracion = Duracion;
        }

        public List<Funcion> ObtenerMisFunciones()
        {
            return MisFunciones.ToList();
        }

        public string[] PeliculaToString()
        {
            return new string[] { ID.ToString(), Nombre.ToString(), Descripcion.ToString(), Sinopsis.ToString(), Poster.ToString(), Duracion.ToString() };
        }
    }
}
