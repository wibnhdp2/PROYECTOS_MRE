using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SGAC.Configuracion.Sistema.DA;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.BL
{
    public class StockConsultaBL
    {
        public DataTable obtener(int intOficinaConsularId, int intTipoInsumo, int intPaginaActual, int intPaginaCantidad,
             ref int intTotalRegistros, ref int intTotalPaginas) 
        {
            DataTable dt = new DataTable();
            string strError = string.Empty;
            string strStackTrace = string.Empty;
            try
            {
                StockConsultaDA objDA = new StockConsultaDA();
                dt = objDA.obtener(intOficinaConsularId, intTipoInsumo, intPaginaActual, intPaginaCantidad,
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

        public BE.MRE.SI_STOCK_ALMACEN Consultar(BE.MRE.SI_STOCK_ALMACEN objStock)
        {
            StockConsultaDA objDA = new StockConsultaDA();
            return objDA.consultar(objStock);
        }
    }
}
