using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGAC.BE
{
    public class RE_EmpresaFiltro
    {
        public Int64 empr_iEmpresaId { get; set; }
        public int empr_sTipoDocumentoId { get; set; }
        public string empr_vRazonSocial { get; set; }
        public string empr_vNumeroDocumento { get; set; }
        public int empr_sTipoEmpresaId { get; set; }
        
        //para el paginado
        public int iPaginaActual { get; set; }
        public int iPaginaCantidad { get; set; }
        public int iTotalRegistros { get; set; }
        public int iTotalPaginas { get; set; }
    }
}
