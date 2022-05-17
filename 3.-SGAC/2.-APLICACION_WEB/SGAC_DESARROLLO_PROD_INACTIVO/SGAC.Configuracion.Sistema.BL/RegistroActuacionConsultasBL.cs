using System;
using System.Data;
using SGAC.Configuracion.Sistema.DA;

namespace SGAC.Configuracion.Sistema.BL
{
    public class RegistroActuacionConsultasBL
    {
        public DataTable obtener(int intOficinaConsularId, int intPeriodo, int intSeccionId,
            int intPaginaActual, int intPaginaCantidad, ref int intTotalRegistros, ref int intTotalPaginas)
        {
            DataTable dt = new DataTable();            
            try
            {
                RegistroActuacionConsultaDA objDA = new RegistroActuacionConsultaDA();
                dt = objDA.obtener(intOficinaConsularId, intPeriodo, intSeccionId, intPaginaActual, intPaginaCantidad,
                    ref intTotalRegistros, ref intTotalPaginas);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        public BE.MRE.RE_CORRELATIVO obtener(BE.MRE.RE_CORRELATIVO objCorrelativo)
        {
            RegistroActuacionConsultaDA objDA = new RegistroActuacionConsultaDA();
            return objDA.obtener(objCorrelativo);
        }

        public DataTable obtenerCorrelativoTarifa(int intOficinaConsularId, int intPeriodo, int intSeccionId,
            int intPaginaActual, int intPaginaCantidad, ref int intTotalRegistros, ref int intTotalPaginas, ref int intCorrelativo)
        {
            DataTable dt = new DataTable();
            try
            {
                RegistroActuacionConsultaDA objDA = new RegistroActuacionConsultaDA();
                dt = objDA.obtenerCorrelativoTarifa(intOficinaConsularId, intPeriodo, intSeccionId, intPaginaActual, intPaginaCantidad,
                    ref intTotalRegistros, ref intTotalPaginas, ref intCorrelativo);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

    }
}
