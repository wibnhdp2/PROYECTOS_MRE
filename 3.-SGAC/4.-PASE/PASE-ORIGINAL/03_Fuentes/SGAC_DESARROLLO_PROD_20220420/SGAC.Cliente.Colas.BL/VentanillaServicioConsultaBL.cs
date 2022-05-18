using System;
using System.Data;
using SGAC.Cliente.Colas.DA;

namespace SGAC.Cliente.Colas.BL
{

    public class VentanillaServicioConsultaBL
    {
        public DataTable Consultar(int vedeVentanillaId,
                                   int intPaginaActual,
                                   int intPaginaCantidad,
                                   ref int intTotalRegistros,
                                   ref int intTotalPaginas)
        {
            VentanillaServicioConsultaDA xFun = new VentanillaServicioConsultaDA();

            try
            {
                return xFun.Consultar(vedeVentanillaId, intPaginaActual, intPaginaCantidad, ref intTotalRegistros, ref intTotalPaginas);
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
