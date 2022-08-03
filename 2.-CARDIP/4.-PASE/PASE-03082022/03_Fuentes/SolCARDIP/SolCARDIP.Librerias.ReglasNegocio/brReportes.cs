using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SolCARDIP.Librerias.EntidadesNegocio;
using SolCARDIP.Librerias.AccesoDatos;

namespace SolCARDIP.Librerias.ReglasNegocio
{
    public class brReportes:brGeneral
    {
        public beReportesListas traerDatos(beReporteResumenxCalidad parametros)
        {
            beReportesListas obeReportesListas = new beReportesListas();
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daReportes odaReportes = new daReportes();
                    obeReportesListas = odaReportes.traerDatos(con, parametros);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (obeReportesListas);
        }

        public beReportesListas traerDatosDetalle(beReporteDetallexCalidad parametros)
        {
            beReportesListas obeReportesListas = new beReportesListas();
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daReportes odaReportes = new daReportes();
                    obeReportesListas = odaReportes.traerDatosDetalle(con, parametros);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (obeReportesListas);
        }
        public beReportesListas consularCarnet(beReporteDetallexCalidad parametros)
        {
            beReportesListas obeReportesListas = new beReportesListas();
            using (SqlConnection con = new SqlConnection(CadenaConexion))
            {
                try
                {
                    con.Open();
                    daReportes odaReportes = new daReportes();
                    obeReportesListas = odaReportes.consultarCarnet(con, parametros);
                }
                catch (Exception ex)
                {
                    grabarLog(ex);
                }
            }
            return (obeReportesListas);
        }
    }
}
