using System;
using System.Collections.Generic;
using System.Data;
using SGAC.Accesorios;
using SGAC.Registro.Persona.DA;

namespace SGAC.Registro.Persona.BL
{
    public class PersonaFiliacionConsultaBL
    {
        public DataTable Obtener(long LonPersonaId)
        {
            PersonaFiliacionConsultaDA objDA = new PersonaFiliacionConsultaDA();

            try
            {
                return objDA.Obtener(LonPersonaId);
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

        public List<BE.MRE.RE_PERSONAFILIACION> ObtenerLista(long lngPersonaId)
        {
            PersonaFiliacionConsultaDA objDA = new PersonaFiliacionConsultaDA();
            try
            {
                return objDA.ObtenerLista(lngPersonaId);
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