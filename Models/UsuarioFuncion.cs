using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP_FINAL_GRUPO_C.Models;

namespace TP_FINAL_GRUPO_C.Models
{
    public class UsuarioFuncion
    {
        public int ID { get; set; }
        public int idUsuario { get; set; }
        public Usuario usuario { get; set; }
        public int idFuncion { get; set; }
        public Funcion funcion { get; set; }

        public int CantidadEntradasCompradas { get; set; }

        public UsuarioFuncion() { }

        public UsuarioFuncion(int idUsuario, int idFuncion, int cantidadEntradas)
        {
            this.idUsuario = idUsuario;
            this.idFuncion = idFuncion;
            this.CantidadEntradasCompradas = cantidadEntradas;
        }

        public UsuarioFuncion(int ID, int idUsuario, int idFuncion, int cantidadEntradas)
        {
            this.ID = ID;
            this.idUsuario = idUsuario;
            this.idFuncion = idFuncion;
            this.CantidadEntradasCompradas = cantidadEntradas;
        }
    }
}
