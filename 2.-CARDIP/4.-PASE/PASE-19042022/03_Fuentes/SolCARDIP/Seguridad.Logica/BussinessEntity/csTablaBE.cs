using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Seguridad.Logica.BussinessEntity
{
    [Serializable]
    public class csTablaBE
    {
        public DataTable dtRegistros { get; set; }
        public int CantidadPaginas { get; set; }
    }
}
