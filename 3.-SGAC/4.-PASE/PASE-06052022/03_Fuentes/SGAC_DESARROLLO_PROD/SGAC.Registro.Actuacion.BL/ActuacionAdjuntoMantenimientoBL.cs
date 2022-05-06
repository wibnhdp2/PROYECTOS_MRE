using System;
using SGAC.Accesorios;
using SGAC.BE;
using SGAC.Registro.Actuacion.DA;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActuacionAdjuntoMantenimientoBL
    {
        public int Insertar(RE_ACTUACIONADJUNTO ObjActAdjBE,
                            int IntOficinaConsularId)
        {
            ActuacionAdjuntoMantenimientoDA objDA = new ActuacionAdjuntoMantenimientoDA();
            try
            {
                return objDA.Insertar(ObjActAdjBE,
                                   IntOficinaConsularId);
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

        public int Actualizar(RE_ACTUACIONADJUNTO ObjActAdjBE,
                              int IntOficinaConsularId)
        {
            ActuacionAdjuntoMantenimientoDA objDA = new ActuacionAdjuntoMantenimientoDA();
            try
            {
                return objDA.Actualizar(ObjActAdjBE,
                                     IntOficinaConsularId);
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

        public int Eliminar(RE_ACTUACIONADJUNTO ObjActAdjBE,
                            int IntOficinaConsularId)
        {
            ActuacionAdjuntoMantenimientoDA objDA = new DA.ActuacionAdjuntoMantenimientoDA();
            try
            {
                return objDA.Eliminar(ObjActAdjBE,
                                   IntOficinaConsularId);
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