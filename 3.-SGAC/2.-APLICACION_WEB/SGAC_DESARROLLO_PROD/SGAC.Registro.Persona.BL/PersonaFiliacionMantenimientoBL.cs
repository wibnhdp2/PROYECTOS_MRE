using System;
using SGAC.Accesorios;

namespace SGAC.Registro.Persona.BL
{
    public class PersonaFiliacionMantenimientoBL
    {
        public int Insertar(BE.RE_PERSONAFILIACION ObjPersFiliacionBE,
                            int IntOficinaConsularId)
        {
            DA.PersonaFiliacionMantenimientoDA objDA = new DA.PersonaFiliacionMantenimientoDA();

            try
            {
                return objDA.Insertar(ObjPersFiliacionBE, IntOficinaConsularId);
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

        public int Actualizar(BE.RE_PERSONAFILIACION ObjPersFiliacionBE,
                              int IntOficinaConsularId)
        {
            DA.PersonaFiliacionMantenimientoDA objDA = new DA.PersonaFiliacionMantenimientoDA();

            try
            {
                return objDA.Actualizar(ObjPersFiliacionBE, IntOficinaConsularId);
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

        public int Eliminar(BE.RE_PERSONAFILIACION ObjPersFiliacionBE,
                            int IntOficinaConsularId)
        {
            DA.PersonaFiliacionMantenimientoDA objDA = new DA.PersonaFiliacionMantenimientoDA();

            try
            {
                return objDA.Eliminar(ObjPersFiliacionBE, IntOficinaConsularId);
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

        public string InsertarPeronaFiliacion(BE.MRE.Custom.CBE_FILIACION cbeFiliacion)
        {
            // 1. Insertar Persona            
            //Int64 lngPersonaId = 0;
            if (cbeFiliacion.pefi_iPersonaId == 0)
            {
                // insertar Persona
                // personaidentificacion
                // registrounico
            }

            // 2. Insertar Filiación

            return string.Empty;
        }
    }
}
