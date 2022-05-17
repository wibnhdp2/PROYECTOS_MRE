using System;
using System.Data;
using SGAC.Configuracion.Maestro.DA;

namespace SGAC.Configuracion.Maestro.BL
{
    public class MonedaConsultaBL
    {

        public DataTable Consultar_Moneda(int intMonedaId, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            DA.MonedaConsultaDA objDA = new MonedaConsultaDA();

            try
            {
                return objDA.Consultar_Moneda(intMonedaId, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }
    }
}
