using System;
using System.Data;
using SGAC.Cliente.Colas.DA;

namespace SGAC.Cliente.Colas.BL
{
    public class VentanillaDetalleConsultaBL
    {
        public DataTable Consultar(int idoficinaconsular, int iVentanillaId)
        {
            VentanillaDetalleConsultaDA xFun = new VentanillaDetalleConsultaDA();

            try
            {
                return xFun.Consultar(idoficinaconsular, iVentanillaId);
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
