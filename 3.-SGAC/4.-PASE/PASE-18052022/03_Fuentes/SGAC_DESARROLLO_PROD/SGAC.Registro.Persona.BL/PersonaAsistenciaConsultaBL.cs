using System;
using System.Data;
using SGAC.Accesorios;

namespace SGAC.Registro.Persona.BL
{
    public class PersonaAsistenciaConsultaBL
    {
        public DataTable Obtener(long LonPersonaId,
                                 string StrCurrentPage,
                                 int IntPageSize,
                                 int intOficinaId,
                                 string strHotsname,
                                 int intUsuarioCreacion,
                                 string strIPCreacion,
                                 ref int IntTotalCount,
                                 ref int IntTotalPages)
        {
            DA.PersonaAsistenciaConsultaDA objDA = new DA.PersonaAsistenciaConsultaDA();

            try
            {
                return objDA.Obtener(LonPersonaId,
                                  StrCurrentPage,
                                  IntPageSize,
                                  intOficinaId,
                                  strHotsname,
                                  intUsuarioCreacion,
                                  strIPCreacion,
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

        public DataTable ListarAsistenciaPAH(DateTime strFchInicio,
                                             DateTime strFchFinal,
                                             string strCodContinente,
                                             Int16? sModalidadPahId,
                                             Int16 sOficinaConsularOrigenId,
                                             Int16? sOficinaConsularId,
                                             Int16? sUsuarioId, string titular)
        {
            DA.PersonaAsistenciaConsultaDA objDA = new DA.PersonaAsistenciaConsultaDA();

            try
            {
                return objDA.ListarAsistenciaPAH(strFchInicio,
                                              strFchFinal,
                                              strCodContinente,
                                              sModalidadPahId,
                                              sOficinaConsularOrigenId,
                                              sOficinaConsularId,
                                              sUsuarioId, titular);
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

        public DataTable ListarAsistenciaPALH(DateTime strFchInicio,
                                              DateTime strFchFinal,
                                              string strCodContinente,
                                              Int16? sModalidadPahlId,
                                              Int16 sOficinaConsularOrigenId,
                                              Int16? sOficinaConsularId,
                                              Int16? sUsuarioId, string titular)
        {
            DA.PersonaAsistenciaConsultaDA objDA = new DA.PersonaAsistenciaConsultaDA();

            try
            {
                return objDA.ListarAsistenciaPALH(strFchInicio,
                                               strFchFinal,
                                               strCodContinente,
                                               sModalidadPahlId,
                                               sOficinaConsularOrigenId,
                                               sOficinaConsularId,
                                               sUsuarioId, titular);
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

        public DataTable ListarAsistenciaBeneficiario(Int64 IdAsistencia, Int64 personaId)
        {
            DA.PersonaAsistenciaConsultaDA objDA = new DA.PersonaAsistenciaConsultaDA();
            try
            {
                return objDA.ListarAsistenciaBeneficiario(IdAsistencia, personaId);
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