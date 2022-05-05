using System;
using System.Collections.Generic;

namespace SolCARDIP.Librerias.EntidadesNegocio
{
    public class beReporteResumenxCalidad
    {
        // PARAMETROS
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        // RESULTADO
        public string CalidadMigratoria { get; set; }
        public int Registrados { get; set; }
        public int Emitidos { get; set; }
        public int Vigentes { get; set; }
        public int Vencidos { get; set; }
    }
}
