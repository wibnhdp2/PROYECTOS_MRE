using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SGAC.Registro.Persona.DA;
using SGAC.BE;
using SGAC.Accesorios;

namespace SGAC.Registro.Persona.BL
{
    using SGAC.DA.MRE;
    public class PersonaResidenciaConsultaBL
    {

        public List<BE.MRE.RE_PERSONARESIDENCIA> PersonaResidencia_obtener(BE.MRE.RE_PERSONARESIDENCIA personaresidencia)
        {
            RE_PERSONARESIDENCIA_DA lPERSONARESIDENCIA_DA = new RE_PERSONARESIDENCIA_DA();
            return lPERSONARESIDENCIA_DA.listado(personaresidencia);
        }


        public DataTable Obtener(long LonPersonaId)
        {
            PersonaResidenciaConsultaDA objDA = new PersonaResidenciaConsultaDA();

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

        public List<BE.MRE.RE_PERSONARESIDENCIA> ObtenerLista(long lngPersonaId)
        {
            PersonaResidenciaConsultaDA objDA = new PersonaResidenciaConsultaDA();
            try
            {
                return objDA.ObtenerLista(lngPersonaId);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (objDA != null)
                    objDA = null;
            }
        }
    }
}
