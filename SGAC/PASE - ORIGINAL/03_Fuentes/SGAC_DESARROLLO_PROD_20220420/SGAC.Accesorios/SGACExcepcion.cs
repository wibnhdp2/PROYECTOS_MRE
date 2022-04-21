using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.Accesorios
{
    public class SGACExcepcion : Exception
    {
        public SGACExcepcion() : base()
        {
        }

        public SGACExcepcion(string strMensaje)
            : base(strMensaje)
        {
        }

        public SGACExcepcion(string strMensaje, Exception e)
            : base(strMensaje, e)
        {
        }

        // Obtener codigo-mensaje usuario
        private int ObtenerCodigoExcepcion(Exception ex)
        {
            string strMensajeUsuario = string.Empty;
            if (ex is ArgumentNullException)
            {
                strMensajeUsuario = "Un Argumento es Nulo.";
            }
            else if (ex is FormatException)
            {
                strMensajeUsuario = "El formato no es el correcto.";
            }
            else if (ex is OverflowException)
            {
                strMensajeUsuario = "El dato ingresado es muy grande o pequeño para el tipo de dato.";
            }
            return 0;
        }
    }
}
