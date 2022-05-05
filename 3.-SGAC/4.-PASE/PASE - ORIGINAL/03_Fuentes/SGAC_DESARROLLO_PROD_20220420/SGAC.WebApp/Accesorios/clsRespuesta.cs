using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGAC.WebApp.Accesorios
{
    public class clsRespuesta
    {
        public Boolean bResultado { get; set; }
        public String vMensaje { get; set; }
        public Int32 NumeroPagina { get; set; }

        public Int32 iResultado { get; set; }
        public Int32 FojaInicial { get; set; }
        public Int32 FojaFinal { get; set; }

    }
}