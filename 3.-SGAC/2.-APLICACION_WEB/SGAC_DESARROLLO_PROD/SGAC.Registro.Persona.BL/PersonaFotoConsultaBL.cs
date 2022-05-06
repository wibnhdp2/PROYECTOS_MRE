using System;
using System.Data;
using SGAC.Accesorios;

namespace SGAC.Registro.Persona.BL
{
    public class PersonaFotoConsultaBL
    {
        public DataTable PersonaFotoGetFotoFirma(long LonPersonaId, int IntImagenTipo)
        {
            DA.PersonaFotoConsultaDA objDA = new DA.PersonaFotoConsultaDA();

            try
            {
                return objDA.PersonaFotoGetFotoFirma(LonPersonaId, IntImagenTipo);
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