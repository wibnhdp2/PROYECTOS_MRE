using System;
using System.Data;
using SGAC.Accesorios;
using SGAC.Registro.Actuacion.DA;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActuacionAnotacionConsultaBL
    {
        public DataTable Obtener(long LonPersonaId, int intCurrentPage, int IntPageSize, ref int IntTotalCount, ref int IntTotalPages)
        {
            ActuacionAnotacionConsultaDA objDA = new ActuacionAnotacionConsultaDA();
            try
            {
                return objDA.Obtener(LonPersonaId, intCurrentPage, IntPageSize, ref IntTotalCount, ref IntTotalPages);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                    objDA = null;
            }
        }
    }
}