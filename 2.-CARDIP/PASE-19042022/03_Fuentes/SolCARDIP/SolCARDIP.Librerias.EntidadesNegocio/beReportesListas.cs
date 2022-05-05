using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolCARDIP.Librerias.EntidadesNegocio
{
    public class beReportesListas
    {
        // PARAMETROS RESUMEN
        public beReporteResumenxCalidad ParametrosResumen { get; set; }
        // PARAMETROS DETALLE
        public beReporteDetallexCalidad ParametrosDetalle { get; set; }
        // RESULTADO
        public List<beReporteResumenxCalidad> ListaResultado { get; set; }
        public List<beReporteDetallexCalidad> ListaResultadoDetalle { get; set; }
        public beReporteResumenxCalidad Totales { get; set; }
    }
}
