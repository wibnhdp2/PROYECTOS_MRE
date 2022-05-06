using System;
using SGAC.Accesorios;

namespace SGAC.Registro.Persona.BL
{
    public class PersonaFotoMantenimientoBL
    {
        public int Insertar(BE.RE_PERSONAFOTO ObjPersFotoBE,
                            int IntOficinaConsularId)
        {
            DA.PersonaFotoMantenimientoDA objDA = new DA.PersonaFotoMantenimientoDA();

            try
            {
                return objDA.Insertar(ObjPersFotoBE, IntOficinaConsularId);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }

        public int Actualizar(BE.RE_PERSONAFOTO ObjPersFotoBE,
                              int IntOficinaConsularId)
        {
            DA.PersonaFotoMantenimientoDA objDA = new DA.PersonaFotoMantenimientoDA();

            try
            {
                return objDA.Actualizar(ObjPersFotoBE, IntOficinaConsularId);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
            }
            finally
            {
                if (objDA != null)
                {
                    objDA = null;
                }
            }
        }

        public int Eliminar(BE.RE_PERSONAFOTO ObjPersFotoBE,
                            int IntOficinaConsularId)
        {
            DA.PersonaFotoMantenimientoDA objDA = new DA.PersonaFotoMantenimientoDA();

            try
            {
                return objDA.Eliminar(ObjPersFotoBE, IntOficinaConsularId);
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message, ex.InnerException);
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