using System;
using System.Data;
using SGAC.Almacen.DA;

namespace SGAC.Almacen.BL
{
    public class PedidoHistoricoConsultaBL
    {
        public DataTable Consultar(int pehi_iPedidoId)
        {
            DataTable dtlResult = new DataTable();
            PedidoHistoricoConsultaDA xFun = new PedidoHistoricoConsultaDA();

            try
            {
                dtlResult = xFun.PedidoHistoricoConsultar(pehi_iPedidoId);
                return dtlResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (xFun != null)
                    xFun = null;
            }
        }
    }
}