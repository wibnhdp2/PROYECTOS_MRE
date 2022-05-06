using System;
using System.Data;
using SGAC.Cliente.Colas.DA;

namespace SGAC.Cliente.Colas.BL
{
    public class TelevisorConsultaBL
    {
        public DataTable Consultar(int iOficinaconsularId,
                                   int intPaginaActual,
                                   int intPaginaCantidad,
                                   ref int intTotalRegistros,
                                   ref int intTotalPaginas)
        {
            TelevisorConsultaDA xFun = new TelevisorConsultaDA();

            try
            {
                return xFun.Consultar(iOficinaconsularId, 
                                      intPaginaActual, 
                                      intPaginaCantidad, 
                                      ref intTotalRegistros, 
                                      ref intTotalPaginas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xFun = null;
            }

        }
    }
}
