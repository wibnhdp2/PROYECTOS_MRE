using System.Data;
using SGAC.Contabilidad.Remesa.DA;
using System;

namespace SGAC.Contabilidad.Remesa.BL
{
    public class RemesaDetalleConsultasBL
    {
        private RemesaDetalleConsultasDA objDA;

        public DataTable ObtenerPorRemesa(int intPaginaActual, 
                                          int intPaginaCantidad, 
                                          ref int intTotalRegistros, 
                                          ref int intTotalPaginas, 
                                          int iRemesaId) 
        {
            try
            {
                objDA = new RemesaDetalleConsultasDA();
                return objDA.ObtenerPorRemesa(intPaginaActual, 
                                              intPaginaCantidad, 
                                              ref intTotalRegistros, 
                                              ref intTotalPaginas,
                                              iRemesaId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDA = null;
            }
        }
    }
}
