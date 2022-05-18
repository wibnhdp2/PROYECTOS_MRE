using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SGAC.Configuracion.Sistema.DA;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.BL
{
    public class ExpedienteConsultasBL
    {
        public DataTable obtener(int intOficinaConsularId, int intPeriodo, int intPaginaActual, int intPaginaCantidad, int intTipoDocMigratorio,
             ref int intTotalRegistros, ref int intTotalPaginas) 
        {
            DataTable dt = new DataTable();
            string strError = string.Empty;
            string strStackTrace = string.Empty;
            try
            {
                ExpedienteConsultasDA objDA = new ExpedienteConsultasDA();
                dt = objDA.obtener(intOficinaConsularId, intPeriodo, intPaginaActual, intPaginaCantidad, intTipoDocMigratorio,
                    ref intTotalRegistros, ref intTotalPaginas, ref strError, ref strStackTrace); 

                if (strError != string.Empty)
                {
                    
                }
            }
            catch (Exception ex)
            {
                
            }

            return dt;
        }

        public BE.MRE.SI_EXPEDIENTE Consultar(BE.MRE.SI_EXPEDIENTE objExpediente)
        {
            ExpedienteConsultasDA objDA = new ExpedienteConsultasDA();
            return objDA.consultar(objExpediente);
        }
    }
}
