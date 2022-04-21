using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SGAC.Registro.Actuacion.DA;
using SGAC.Accesorios;

namespace SGAC.Registro.Actuacion.BL
{
    using SGAC.BE.MRE;
    using SGAC.DA.MRE.ACTONOTARIAL;

    public class ActoNotarialConsultaBL
    {
        public RE_ACTONOTARIAL obtener(RE_ACTONOTARIAL actonotarial) {
            RE_ACTONOTARIAL_DA lACTONOTARIAL_DA = new RE_ACTONOTARIAL_DA();
            return lACTONOTARIAL_DA.obtener(actonotarial);
        }

        public DataTable ListarActuacionDetalle(long lngActuacionId)
        {
            ActoNotarialConsultaDA objDA = new ActoNotarialConsultaDA();
            return objDA.ListarActuacionDetalle(lngActuacionId);
        }

        public DataTable ObtenerUsoSubTipo(long sTipoActoNotarialId)
        {
            ActoNotarialConsultaDA objDA = new ActoNotarialConsultaDA();
            return objDA.ObtenerUsoSubTipo(sTipoActoNotarialId);
        }

        public DataTable ReporteSupervivencia(int iActoNotarialId, int sOficinaConsularId)
        {
            ActoNotarialConsultaDA objDA = new ActoNotarialConsultaDA();
            return objDA.ReporteSupervivencia(iActoNotarialId, sOficinaConsularId);
        }

        public DataTable ReporteAutorizacionViaje(int iActoNotarialId, int sOficinaConsularId, Int64? ianpa_iReferenciaId = null)
        {
            ActoNotarialConsultaDA objDA = new ActoNotarialConsultaDA();
            return objDA.ReporteAutorizacionViaje(iActoNotarialId, sOficinaConsularId, ianpa_iReferenciaId);
        }

        public DataTable ReportePoderFueraRegistro(int iActoNotarialId, int sOficinaConsularId, Int64? ianpa_iReferenciaId = null)
        {
            ActoNotarialConsultaDA objDA = new ActoNotarialConsultaDA();
            return objDA.ReportePoderFueraRegistro(iActoNotarialId, sOficinaConsularId,ianpa_iReferenciaId);
        }
        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 19/09/2016
        // Objetivo: Se adiciono el parametro Sub tipo de acto notarial
        //------------------------------------------------------------------------

        public DataTable ActoProtocolarConsulta(int intOficinaConsularId,
                                                string strNumProyecto,
                                                int intEstadoId,
                                                DateTime dateFechaInicio,
                                                DateTime dateFechaFinal,
                                                int intFuncionarioAutorizadorId,
                                                int intTipoActoNotarialId,
                                                int intSubTipoActoNotarialId,
                                                string strApellidoPaterno,
                                                string strApellidoMaterno,
                                                string strNombres,
                                                short sTipoDocumento,
                                                string strNumeroDocumento,
                                                short sTipoParticipante,
                                                short sAnio,
                                                DateTime? FechIniPago,
                                                DateTime? FechFinPago,
                                                int ICorrelativoActuacion,
                                                int intCurrentPage,
                                                int intPageSize,
                                                ref int IntTotalCount,
                                                ref int IntTotalPages,
                                                string strUbigeoDestino = "")
       {

            ActoNotarialConsultaDA objDA = new ActoNotarialConsultaDA();

            try
            {
                return objDA.ActoProtocolarConsulta(intOficinaConsularId,
                                                    strNumProyecto,
                                                    intEstadoId,
                                                    dateFechaInicio,
                                                    dateFechaFinal,
                                                    intFuncionarioAutorizadorId,
                                                    intTipoActoNotarialId,
                                                    intSubTipoActoNotarialId,
                                                    strApellidoPaterno,
                                                    strApellidoMaterno,
                                                    strNombres,
                                                    sTipoDocumento,
                                                    strNumeroDocumento,
                                                    sTipoParticipante,
                                                    sAnio,
                                                    FechIniPago,
                                                    FechFinPago,
                                                    ICorrelativoActuacion,
                                                    intCurrentPage,
                                                    intPageSize,
                                                    ref IntTotalCount,
                                                    ref IntTotalPages, 
                                                    strUbigeoDestino);
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }
        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 20/09/2016
        // Objetivo: Adiciona la consulta de registros de Viaje de Menor
        //------------------------------------------------------------------------

        public DataTable AutorizacionViajeMenorConsulta(int intOficinaConsularId,
                                                string strNumProyecto,
                                                int intEstadoId,
                                                DateTime dateFechaInicio,
                                                DateTime dateFechaFinal,
                                                int intFuncionarioAutorizadorId,
                                                int intTipoActoNotarialId,
                                                int intSubTipoActoNotarialId,
                                                short sTipoParticipante, 
                                                string strApellidoPaterno,
                                                string strApellidoMaterno,
                                                string strNombres,
                                                short sTipoDocumento,
                                                string strNumeroDocumento,
                                                DateTime? dateFechaInicioPago,
                                                DateTime? dateFechaFinalPago,
                                                int intCurrentPage,
                                                int intPageSize,
                                                ref int IntTotalCount,
                                                ref int IntTotalPages,
                                                string strUbigeoDestino = "")
        {

            ActoNotarialConsultaDA objDA = new ActoNotarialConsultaDA();

            try
            {
                return objDA.AutorizacionViajeMenorConsulta(intOficinaConsularId,
                                                    strNumProyecto,
                                                    intEstadoId,
                                                    dateFechaInicio,
                                                    dateFechaFinal,
                                                    intFuncionarioAutorizadorId,
                                                    intTipoActoNotarialId,
                                                    intSubTipoActoNotarialId,
                                                    sTipoParticipante,
                                                    strApellidoPaterno,
                                                    strApellidoMaterno,
                                                    strNombres,
                                                    sTipoDocumento,
                                                    strNumeroDocumento,
                                                    dateFechaInicioPago,
                                                    dateFechaFinalPago,
                                                    intCurrentPage,
                                                    intPageSize,
                                                    ref IntTotalCount,
                                                    ref IntTotalPages,
                                                    strUbigeoDestino);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }

        public DataTable ActoProtocolarReporte(int intOficinaConsularId,
                                               string strNumProyecto,
                                               int intEstadoId,
                                               DateTime dateFechaInicio,
                                               DateTime dateFechaFinal,
                                               int intFuncionarioAutorizadorId,
                                               int intTipoActoNotarialId,
                                               int intSubTipoActoNotarialId,
                                               string strApellidoPaterno,
                                               string strApellidoMaterno,
                                               string strNombres,
                                               short sTipoDocumento,
                                               string strNumeroDocumento,
                                               short sTipoParticipante,
                                               short sAnio)
        {

            ActoNotarialConsultaDA objDA = new ActoNotarialConsultaDA();

            try
            {
                return objDA.ActoProtocolarReporte(intOficinaConsularId,
                                                   strNumProyecto,
                                                   intEstadoId,
                                                   dateFechaInicio,
                                                   dateFechaFinal,
                                                   intFuncionarioAutorizadorId,
                                                   intTipoActoNotarialId,
                                                   intSubTipoActoNotarialId,
                                                   strApellidoPaterno,
                                                   strApellidoMaterno,
                                                   strNombres,
                                                   sTipoDocumento,
                                                   strNumeroDocumento,
                                                   sTipoParticipante,
                                                   sAnio
                                                   );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }

        public DataTable ObtenerCuerpo(long lonvCuerpo)
        {
            ActoNotarialConsultaDA objDA = new ActoNotarialConsultaDA();

            try
            {
                return objDA.ObtenerCuerpo(lonvCuerpo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }

        public DataTable ActonotarialObtenerDatosPrincipales(long lngActoNotarialId, int IntOficinaConsular)
        {
            ActoNotarialConsultaDA objDA = new ActoNotarialConsultaDA();

            try
            {
                return objDA.ActonotarialObtenerDatosPrincipales(lngActoNotarialId, IntOficinaConsular);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }

        public DataTable ActonotarialObtenerParticipantes(long lngActoNotarialId)
        {
            ActoNotarialConsultaDA objDA = new ActoNotarialConsultaDA();

            try
            {
                return objDA.ActonotarialObtenerParticipantes(lngActoNotarialId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                objDA = null;
            }
        }

        public string ExisteNumeroDocumento(BE.MRE.SI_LIBRO objLibro)
        {
            RE_ACTONOTARIAL_DA oRE_ACTONOTARIAL_DA = new RE_ACTONOTARIAL_DA();
            return oRE_ACTONOTARIAL_DA.ExisteNumeroEscritura(objLibro);
        }

        public Boolean ExisteNumeroLibro(Int32 libr_INumeroLibro, Int16 libr_sOficinaConsularId)
        {
            RE_ACTONOTARIAL_DA oRE_ACTONOTARIAL_DA = new RE_ACTONOTARIAL_DA();
            return oRE_ACTONOTARIAL_DA.ExisteNumeroLibro(libr_INumeroLibro, libr_sOficinaConsularId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acno_iActoNotarialID"></param>
        /// <param name="libr_sTipoLibroId"></param>
        /// <param name="libr_sOficinaConsularId"></param>
        /// <param name="acno_sNumeroHojas"></param>
        /// <param name="operacion">0. Consulta y Valida, 1. Consulta, Valida y Actualiza</param>
        /// <param name="oMensaje"></param>
        /// <param name="FojaInicial"></param>
        /// <param name="FojaFinal"></param>
        /// <param name="acno_INumeroEscritura"></param>
        /// <param name="acno_INumeroLibro"></param>
        /// <returns></returns>
        public Int32 ValidarFojas(Int64 acno_iActoNotarialID, Int16 libr_sTipoLibroId, Int16 libr_sOficinaConsularId, Int32 acno_sNumeroHojas, Int32 operacion, ref String oMensaje, ref Int32 FojaInicial, ref Int32 FojaFinal, ref Int32 acno_INumeroEscritura, ref String acno_INumeroLibro)
        {
            RE_ACTONOTARIAL_DA oRE_ACTONOTARIAL_DA = new RE_ACTONOTARIAL_DA();
            return oRE_ACTONOTARIAL_DA.ValidarFojas(acno_iActoNotarialID, libr_sTipoLibroId, libr_sOficinaConsularId, acno_sNumeroHojas, operacion, ref oMensaje, ref FojaInicial, ref FojaFinal, ref acno_INumeroEscritura, ref acno_INumeroLibro);
        }
        
        public DataTable ObtenerTarifaTipoActo(Int16 sTipoActoId)
        {
            RE_ACTONOTARIAL_DA oRE_ACTONOTARIAL_DA = new RE_ACTONOTARIAL_DA();
            return oRE_ACTONOTARIAL_DA.ObtenerTarifaTipoActo(sTipoActoId);
        }

        public DataTable ObtenerActoNotarialDetalle(Int64 intActuacionId, Int64? intActuacionDetalleId, Int16 intTipoFormatoId, bool bolSoloNoVinculados = false, string strActuacionesDetalleIds = "")
        {
            ActoNotarialConsultaDA objDA = new ActoNotarialConsultaDA();
            return objDA.ObtenerActoNotarialDetalle(intActuacionId, intActuacionDetalleId, intTipoFormatoId, bolSoloNoVinculados, strActuacionesDetalleIds);
        }


        public DataTable ObtenerActoNotarialDetalle(Int64 intActuacionId, short intTipoFormato)
        {
            ActoNotarialConsultaDA objDA = new ActoNotarialConsultaDA();
            return objDA.ObtenerActoNotarialDetalle(intActuacionId, intTipoFormato);
        }
        //-------------------------------------------------------
        //Fecha: 10/11/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Búsqueda de la Escritura Pública
        //--------------------------------------------------------
        public BE.MRE.RE_ACTONOTARIAL BusquedaEscrituraPublica(string strAnioEscritura, string strNumeroEscrituraPublica, Int16 iOficinaConsularId, string strPG_PE)
        {
            RE_ACTONOTARIAL_DA ActoNotarial_DA = new RE_ACTONOTARIAL_DA();
            return ActoNotarial_DA.BusquedaEscrituraPublica(strAnioEscritura, strNumeroEscrituraPublica, iOficinaConsularId, strPG_PE);
        }

        //------------------------------------------------
        //Fecha: 03/12/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Consultar el idioma diferente al castellano
        //          del primer participante.
        //------------------------------------------------
        public BE.MRE.RE_ACTONOTARIALPARTICIPANTE ObtenerIdiomaPrimerParticipanteOtorgante(Int64 iActoNotarialId)
        {
            RE_ACTONOTARIALPARTICIPANTE_DA Participante_DA = new RE_ACTONOTARIALPARTICIPANTE_DA();
            return Participante_DA.ObtenerIdiomaPrimerParticipanteOtorgante(iActoNotarialId);
        }
    }
}
