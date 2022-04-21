using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SGAC.Configuracion.Sistema.DA;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.BL
{
    public class LibroConsultasBL
    {
        public DataTable obtener(int intOficinaConsularId, int intPeriodo,
            int intPaginaActual, int intPaginaCantidad, ref int intTotalRegistros, ref int intTotalPaginas)
        {
            DataTable dt = new DataTable();
            string strError = string.Empty;
            string strStackTrace = string.Empty;
            try
            {
                LibroConsultasDA objDA = new LibroConsultasDA();
                dt = objDA.obtener(intOficinaConsularId, intPeriodo, intPaginaActual, intPaginaCantidad,
                    ref intTotalRegistros, ref intTotalPaginas, ref strError, ref strStackTrace);

                if (strError != string.Empty)
                {
                    // incidencia base de datos                    
                }
            }
            catch (Exception ex)
            {
                // incidencia codigo
            }            

            return dt;
        }

        public BE.MRE.SI_LIBRO Consultar(BE.MRE.SI_LIBRO objLibro)
        {
            LibroConsultasDA objDA = new LibroConsultasDA();
            return objDA.consultar(objLibro);
        }

        public int ObtenerFojaActual(BE.MRE.SI_LIBRO libro)
        {

            int ret = 0;
            try
            {
                LibroConsultasDA objDA = new LibroConsultasDA();
                ret = objDA.ObtenerFojaActual(libro);
                return ret;

            }
            catch (Exception ex)
            {
                // incidencia codigo
            }


            return 0;
            
        }
    }
}
