using System;
using SGAC.Accesorios;
using SGAC.BE;
using SGAC.Registro.Actuacion.DA;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActuacionPagoMantenimientoBL
    {
        public int Actualizar(RE_PAGO ObjPagoBE,
                               int IntOficinaConsularId)
        {
            ActuacionPagoMantenimientoDA objDA = new ActuacionPagoMantenimientoDA();
            try
            {
                return objDA.Actualizar(ObjPagoBE,
                              IntOficinaConsularId);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }
    }
}