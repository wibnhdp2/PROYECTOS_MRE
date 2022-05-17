using System;
using SGAC.Accesorios;
using SGAC.Registro.Persona.DA;

namespace SGAC.Registro.Persona.BL
{
    public class PersonaIdentificacionMantenimientoBL
    {
        public int Insertar(BE.RE_PERSONAIDENTIFICACION ObjPerIdentBE,
                             int IntOficinaConsularId)
        {
            DA.PersonaIdentificacionMantenimientoDA objDA = new PersonaIdentificacionMantenimientoDA();

            try
            {
                return objDA.Insertar(ObjPerIdentBE,
                                   IntOficinaConsularId);
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

        public int Actualizar(BE.RE_PERSONAIDENTIFICACION ObjPerIdentBE,
                               int IntOficinaConsularId)
        {
            DA.PersonaIdentificacionMantenimientoDA objDA = new PersonaIdentificacionMantenimientoDA();

            try
            {
                return objDA.Actualizar(ObjPerIdentBE,
                                        IntOficinaConsularId);
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

        public int Eliminar(BE.RE_PERSONAIDENTIFICACION ObjPerIdentBE,
                             int IntOficinaConsularId)
        {
            DA.PersonaIdentificacionMantenimientoDA objDA = new PersonaIdentificacionMantenimientoDA();

            try
            {
                return objDA.Eliminar(ObjPerIdentBE,
                                   IntOficinaConsularId);
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