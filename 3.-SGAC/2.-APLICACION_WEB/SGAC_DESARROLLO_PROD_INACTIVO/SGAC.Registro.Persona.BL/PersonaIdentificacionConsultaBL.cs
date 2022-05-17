using System;
using System.Data;
using SGAC.Accesorios;
using SGAC.DA.MRE;

namespace SGAC.Registro.Persona.BL
{
    public class PersonaIdentificacionConsultaBL
    {
        public DataTable Consultar(long LonPersonaId)
        {
            DA.PersonaIdentificacionConsultaDA objDA = new DA.PersonaIdentificacionConsultaDA();

            try
            {
                return objDA.Consultar(LonPersonaId);
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

        public BE.MRE.RE_PERSONAIDENTIFICACION Obtener(BE.MRE.RE_PERSONA persona)
        {
            BE.MRE.RE_PERSONAIDENTIFICACION lPERSONAIDENTIFICACION = new BE.MRE.RE_PERSONAIDENTIFICACION();
            lPERSONAIDENTIFICACION.peid_iPersonaId = persona.pers_iPersonaId;

            if (persona.pers_iPersonaId == 0)
            {
                if (persona.Identificacion.peid_sDocumentoTipoId != 0)
                    lPERSONAIDENTIFICACION.peid_sDocumentoTipoId = persona.Identificacion.peid_sDocumentoTipoId;
                if (persona.Identificacion.peid_vDocumentoNumero != string.Empty)
                    lPERSONAIDENTIFICACION.peid_vDocumentoNumero = persona.Identificacion.peid_vDocumentoNumero;
            }

            RE_PERSONAIDENTIFICACION_DA lPERSONAIDENTIFICACION_DA = new RE_PERSONAIDENTIFICACION_DA();
            return lPERSONAIDENTIFICACION_DA.obtener(lPERSONAIDENTIFICACION);
        }

        public BE.MRE.RE_PERSONAIDENTIFICACION Obtener(BE.MRE.RE_PERSONAIDENTIFICACION identificacion) {
            RE_PERSONAIDENTIFICACION_DA lPERSONAIDENTIFICACION_DA = new RE_PERSONAIDENTIFICACION_DA();
            return lPERSONAIDENTIFICACION_DA.obtener(identificacion);
        }

        public DataTable Obtener(long LonPersonaId, int IntDocumentoTipoId)
        {
            DA.PersonaIdentificacionConsultaDA objDA = new DA.PersonaIdentificacionConsultaDA();

            try
            {
                return objDA.Obtener(LonPersonaId, IntDocumentoTipoId);
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

        public int Existe(int IntTipoDocumentoId, string StrNroDocumento, long LonPersonaID, int IntOperacion)
        {
            DA.PersonaIdentificacionConsultaDA objDA = new DA.PersonaIdentificacionConsultaDA();

            try
            {
                return objDA.Existe(IntTipoDocumentoId, StrNroDocumento, LonPersonaID, IntOperacion);
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

        public int ActivoRune(long LonPersonaId)
        {
            DA.PersonaIdentificacionConsultaDA objDA = new DA.PersonaIdentificacionConsultaDA();

            try
            {
                return objDA.ActivoRune(LonPersonaId);
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
