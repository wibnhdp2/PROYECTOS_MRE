using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SGAC.Reportes.BL
{
    public class ReportesMigratorioConsultaBL
    {
        public DataTable ReporteMigratorio(int i_TipoReporte, string sPasaporte_Inicial, string sPasaporte_Final,
            int iMision_IDV, int iEstado_Pasaporte_IDV, string sAnio, int iMision_PG, string sNumero_Pasaporte,
            bool bExpediente_IMG, int iTipo_Documento, string sNumero_Documento, int iEstado_Pasaporte_PG, string sNumero_Expediente,
            string sApellido_Paterno, string sApellido_Materno, string sNombres, DateTime? dFecInicio, DateTime? dFecFin)
        {
            return new SGAC.Reportes.DA.ReportesMigratorioConsultasDA().ObtenerReportesMigratorio(i_TipoReporte, sPasaporte_Inicial, sPasaporte_Final,
            iMision_IDV, iEstado_Pasaporte_IDV, sAnio, iMision_PG, sNumero_Pasaporte,
            bExpediente_IMG, iTipo_Documento, sNumero_Documento, iEstado_Pasaporte_PG, sNumero_Expediente,
            sApellido_Paterno, sApellido_Materno, sNombres, dFecInicio, dFecFin);
        }
    }
}
