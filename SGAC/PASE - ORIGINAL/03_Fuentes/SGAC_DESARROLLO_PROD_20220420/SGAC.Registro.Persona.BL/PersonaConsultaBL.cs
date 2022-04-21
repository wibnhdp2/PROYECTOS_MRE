using System;
using System.Data;
using SGAC.Accesorios;
using SGAC.Registro.Persona.DA;
using SGAC.BE;
using SGAC.BE.MRE;
using SGAC.DA.MRE;

namespace SGAC.Registro.Persona.BL
{
    public class PersonaConsultaBL
    {

        public BE.MRE.RE_PERSONA USP_RE_PERSONA_OBTENERXIDPERSONA(Int64 iPersonaID) {
                
            PersonaConsultaDA objPersonaDA = new PersonaConsultaDA();
            return objPersonaDA.USP_RE_PERSONA_OBTENERXIDPERSONA(iPersonaID);
        }

        public BE.MRE.RE_PERSONA ObtenerRuneRapido(BE.MRE.RE_PERSONA objPersona)
        {
            BE.MRE.RE_PERSONA RUNERAPIDO = new BE.MRE.RE_PERSONA();

            PersonaConsultaDA objPersonaDA = new PersonaConsultaDA();
            PersonaIdentificacionConsultaBL objIdentificacionBL = new PersonaIdentificacionConsultaBL();

            if (objPersona.pers_iPersonaId != 0)
            {
                RUNERAPIDO = objPersonaDA.obtener(objPersona);
                if (RUNERAPIDO.pers_iPersonaId != 0)
                {
                    RUNERAPIDO.Identificacion = objIdentificacionBL.Obtener(objPersona);
                }
            }
            else if (objPersona.Identificacion.peid_sDocumentoTipoId != 0 && objPersona.Identificacion.peid_vDocumentoNumero != string.Empty)
            {
                RUNERAPIDO.Identificacion = objIdentificacionBL.Obtener(objPersona);

                objPersona.pers_iPersonaId = RUNERAPIDO.Identificacion.peid_iPersonaId;

                if (objPersona.pers_iPersonaId != 0)
                {
                    RUNERAPIDO = objPersonaDA.obtener(objPersona);
                }
            }

            if (RUNERAPIDO.pers_iPersonaId != 0)
            {
                RegistroUnicoConsultaBL objRegistroUnicoBL = new RegistroUnicoConsultaBL();
                RUNERAPIDO.REGISTROUNICO.reun_iPersonaId = RUNERAPIDO.pers_iPersonaId;
                RUNERAPIDO.REGISTROUNICO = objRegistroUnicoBL.Obtener(RUNERAPIDO.REGISTROUNICO);

                PersonaResidenciaConsultaBL objResidenciaBL = new PersonaResidenciaConsultaBL();
                RUNERAPIDO.Residencias = objResidenciaBL.ObtenerLista(RUNERAPIDO.pers_iPersonaId);

                PersonaFiliacionConsultaBL objFiliacionBL = new PersonaFiliacionConsultaBL();
                RUNERAPIDO.FILIACIONES = objFiliacionBL.ObtenerLista(RUNERAPIDO.pers_iPersonaId);
            }
            return RUNERAPIDO;
        }

        //
        public SGAC.BE.MRE.RE_PERSONA Obtener(SGAC.BE.MRE.RE_PERSONAIDENTIFICACION identificacion)
        {
            SGAC.BE.MRE.RE_PERSONA lRE_PERSONA = new BE.MRE.RE_PERSONA();
            PersonaIdentificacionConsultaBL lPersonaIdentificacionConsultaBL = new PersonaIdentificacionConsultaBL();
            lRE_PERSONA.pers_iPersonaId = (lPersonaIdentificacionConsultaBL.Obtener(identificacion)).peid_iPersonaId;

            RE_PERSONA_DA lPERSONA_DA = new RE_PERSONA_DA();

            if (lRE_PERSONA.pers_iPersonaId !=0) 
                lRE_PERSONA = lPERSONA_DA.obtener(lRE_PERSONA);
            else 
                lRE_PERSONA = new BE.MRE.RE_PERSONA();

            return lRE_PERSONA;
        }

        public SGAC.BE.MRE.RE_PERSONA Obtener(SGAC.BE.MRE.RE_PERSONA persona)
        {
            RE_PERSONA_DA lPERSONA_DA = new RE_PERSONA_DA();
            return lPERSONA_DA.obtener(persona);
        }
        
        public DataTable Consultar(int IntTipo,
                                    string StrNroDoc,
                                    string StrApePat,
                                    string StrApeMat,
                                    string StrCurrentPage,
                                    int IntPageSize,
                                    ref int IntTotalCount,
                                    ref int IntTotalPages)
        {
            DA.PersonaConsultaDA objDA = new DA.PersonaConsultaDA();

            try
            {
                return objDA.Consultar(IntTipo,
                                    StrNroDoc,
                                    StrApePat,
                                    StrApeMat,
                                    StrCurrentPage,
                                    IntPageSize,
                                    ref IntTotalCount,
                                    ref IntTotalPages);
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

        public DataTable RecurrenteConsultar(int IntTipo,
                                             string StrNroDoc,
                                             string StrApePat,
                                             string StrApeMat,
                                             int IntCurrentPage,
                                             int IntPageSize,
                                             ref int IntTotalCount,
                                             ref int IntTotalPages)
        {
            PersonaConsultaDA objDA = new PersonaConsultaDA();
            try
            {
                return objDA.RecurrenteConsultar(IntTipo,
                                              StrNroDoc,
                                              StrApePat,
                                              StrApeMat,
                                              IntCurrentPage,
                                              IntPageSize,
                                              ref IntTotalCount,
                                              ref IntTotalPages);
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

        public DataTable EmpresaConsultar(string StrNroDoc,
                                          string StrRazonSocial,
                                          int IntCurrentPage,
                                          int IntPageSize,
                                          ref int IntTotalCount,
                                          ref int IntTotalPages)
        {
            PersonaConsultaDA objDA = new PersonaConsultaDA();
            try
            {
                return objDA.EmpresaConsultar(StrNroDoc,
                                              StrRazonSocial,
                                              IntCurrentPage,
                                              IntPageSize,
                                              ref IntTotalCount,
                                              ref IntTotalPages);
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

        public DataTable PersonaGetById(long LonPersonaId, Int16 intDocumentoId = 0, string strDocumentoNumero = "")
        {
            DA.PersonaConsultaDA objDA = new DA.PersonaConsultaDA();

            try
            {
                return objDA.PersonaGetById(LonPersonaId, intDocumentoId, strDocumentoNumero);
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

        public DataTable PersonaGetById(string StrDNI)
        {
            DA.PersonaConsultaDA objDA = new DA.PersonaConsultaDA();

            try
            {
                return objDA.PersonaGetById(StrDNI);
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

        public DataSet Persona_Imprimir_Rune(BE.RE_PERSONA objBE)
        {
            DA.PersonaConsultaDA objDA = new DA.PersonaConsultaDA();

            try
            {
                return objDA.Persona_Imprimir_Rune(objBE);
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

        public int Tiene58A(long LonPersonaId)
        {
            DA.PersonaConsultaDA objDA = new DA.PersonaConsultaDA();

            try
            {
                return objDA.Tiene58A(LonPersonaId);
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
        public DataTable Tiene58B(long LonPersonaId)
        {
            DA.PersonaConsultaDA objDa = new DA.PersonaConsultaDA();
            try
            {
                return objDa.Tiene58B(LonPersonaId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objDa != null)
                {
                    objDa = null;
                }
            }
        }
        public DataTable Obtener_Persona(object[] arrParametros)
        {
            DA.PersonaConsultaDA objDa = new DA.PersonaConsultaDA();
            try
            {
                return objDa.Obtener_Persona(arrParametros);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objDa != null)
                {
                    objDa = null;
                }
            }
        }

        public DataTable Obtener_Empresa(object[] arrParametros)
        {
            DA.PersonaConsultaDA objDa = new DA.PersonaConsultaDA();
            try
            {
                return objDa.Obtener_Empresa(arrParametros);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (objDa != null)
                {
                    objDa = null;
                }
            }
        }

        public DataTable Obtener_Empresa_Por_Id(int intEmpresaId)
        {
            DA.PersonaConsultaDA objDA = new DA.PersonaConsultaDA();

            try
            {
                return objDA.Obtener_Empresa_Por_Id(intEmpresaId);
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

        public bool PersonaPoseeActuaciones(short iTipoDocumento, string vNumeroDocumento)
        {
            DA.PersonaConsultaDA objDA = new DA.PersonaConsultaDA();

            try
            {
                return objDA.PersonaPoseeActuaciones(iTipoDocumento, vNumeroDocumento);
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

        public DataTable ObtenerDatosPersona(short iTipoDocumento, string vNumeroDocumento)
        {
           
            DA.PersonaConsultaDA objDA = new DA.PersonaConsultaDA();
            DataTable _dt = new DataTable();
            try
            {
                 _dt = objDA.ObtenerDatosPersona(iTipoDocumento, vNumeroDocumento);
                 return _dt;
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

        public DataTable ObtenerDatosPersonaFiliacion(short iTipoDocumento, string vNumeroDocumento)
        {

            DA.PersonaConsultaDA objDA = new DA.PersonaConsultaDA();
            DataTable _dt = new DataTable();
            try
            {
                _dt = objDA.ObtenerDatosPersonaFiliacion(iTipoDocumento, vNumeroDocumento);
                return _dt;
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

        public DataTable ObtenerParticipacionesPersona(short iTipoDocumento, string vNumeroDocumento)
        {

            DA.PersonaConsultaDA objDA = new DA.PersonaConsultaDA();
            DataTable _dt = new DataTable();
            try
            {
                _dt = objDA.ObtenerParticipacionesPersona(iTipoDocumento, vNumeroDocumento);
                return _dt;
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

        public DataTable PersonaListarNacionalidades(Int64 iPersona)
        {
            DA.PersonaConsultaDA objDA = new DA.PersonaConsultaDA();

            try
            {
                return objDA.PersonaListarNacionalidades(iPersona);
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
        public DataTable PersonaListarUltimaNacionalidad(Int64 iPersona)
        {
            DA.PersonaConsultaDA objDA = new DA.PersonaConsultaDA();

            try
            {
                return objDA.PersonaListarUltimaNacionalidad(iPersona);
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
