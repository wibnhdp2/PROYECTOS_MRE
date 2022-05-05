using System;
using SGAC.Accesorios;
using SGAC.BE;
using SGAC.Registro.Actuacion.DA;

namespace SGAC.Registro.Actuacion.BL
{
    public class ActuacionAnotacionMantenimientoBL
    {
        public int Insertar(RE_ACTUACIONANOTACION ObjAnotBE, int IntOficinaConsular)
        {
            ActuacionAnotacionMantenimientoDA objDA = new ActuacionAnotacionMantenimientoDA();
            try
            {
                return objDA.Insertar(ObjAnotBE, IntOficinaConsular);
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

        public int Actualizar(RE_ACTUACIONANOTACION ObjAnotBE, int IntOficinaConsular)
        {
            ActuacionAnotacionMantenimientoDA objDA = new ActuacionAnotacionMantenimientoDA();
            try
            {
                return objDA.Actualizar(ObjAnotBE, IntOficinaConsular);
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

        public int Eliminar(RE_ACTUACIONANOTACION ObjAnotBE, int IntOficinaConsular)
        {
            ActuacionAnotacionMantenimientoDA objDA = new ActuacionAnotacionMantenimientoDA();
            try
            {
                return objDA.Eliminar(ObjAnotBE, IntOficinaConsular);
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