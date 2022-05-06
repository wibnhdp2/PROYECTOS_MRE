using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using SGAC.Accesorios;

namespace SGAC.DA.MRE.ACTONOTARIAL
{
    using SGAC.BE.MRE;
    using System.Data;
    using System.Data.SqlClient;
    using System.Reflection;

    public class RE_ACTONOTARIAL_DA
    {

        public string strError = string.Empty;

        public RE_ACTONOTARIAL insertar(RE_ACTONOTARIAL ActoNotarial)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIAL_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@acno_iActuacionId", ActoNotarial.acno_iActuacionId));
                        cmd.Parameters.Add(new SqlParameter("@acno_sTipoActoNotarialId", ActoNotarial.acno_sTipoActoNotarialId));
                        cmd.Parameters.Add(new SqlParameter("@acno_sSubTipoActoNotarialId", ActoNotarial.acno_sSubTipoActoNotarialId));
                        cmd.Parameters.Add(new SqlParameter("@acno_sOficinaConsularId", ActoNotarial.acno_sOficinaConsularId));

                        if (ActoNotarial.acno_iActoNotarialReferenciaId != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_iActoNotarialReferenciaId", ActoNotarial.acno_iActoNotarialReferenciaId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_iActoNotarialReferenciaId", DBNull.Value));
                        }

                        cmd.Parameters.Add(new SqlParameter("@acno_IFuncionarioAutorizadorId", ActoNotarial.acno_IFuncionarioAutorizadorId));
                        cmd.Parameters.Add(new SqlParameter("@acno_IFuncionarioCertificadorId", ActoNotarial.acno_IFuncionarioCertificadorId));

                        if (ActoNotarial.acno_vAutorizacionTipo != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_vAutorizacionTipo", ActoNotarial.acno_vAutorizacionTipo));
                        }
                        cmd.Parameters.Add(new SqlParameter("@acno_vDenominacion", ActoNotarial.acno_vDenominacion));
                        cmd.Parameters.Add(new SqlParameter("@acno_bDigitalizadoFlag", ActoNotarial.acno_bDigitalizadoFlag));
                        cmd.Parameters.Add(new SqlParameter("@acno_bFlagMinuta", ActoNotarial.acno_bFlagMinuta));
                        cmd.Parameters.Add(new SqlParameter("@acno_vNumeroColegiatura", ActoNotarial.acno_vNumeroColegiatura));
                        cmd.Parameters.Add(new SqlParameter("@acno_iNumeroActoNotarial", ActoNotarial.acno_iNumeroActoNotarial));

                        if (ActoNotarial.acno_vNumeroEscrituraPublica != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_vNumeroEscrituraPublica", ActoNotarial.acno_vNumeroEscrituraPublica));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_vNumeroEscrituraPublica", DBNull.Value));
                        }

                        cmd.Parameters.Add(new SqlParameter("@acno_vItinerario", ActoNotarial.acno_vItinerario));

                        if (ActoNotarial.acno_dFechaExtension != DateTime.MinValue)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_dFechaExtension", ActoNotarial.acno_dFechaExtension));
                        }

                        cmd.Parameters.Add(new SqlParameter("@acno_vObservaciones", ActoNotarial.acno_vObservaciones));

                        if (ActoNotarial.acno_cUbigeoDestino.Length != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_cUbigeoDestino", ActoNotarial.acno_cUbigeoDestino));
                        }

                        cmd.Parameters.Add(new SqlParameter("@acno_IFuncionarioAnulaId", ActoNotarial.acno_IFuncionarioAnulaId));

                        if (ActoNotarial.acno_dFechaAnulacion != DateTime.MinValue)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_dFechaAnulacion", ActoNotarial.acno_dFechaAnulacion));
                        }

                        cmd.Parameters.Add(new SqlParameter("@acno_vMotivoAnulacion", ActoNotarial.acno_vMotivoAnulacion));
                        
                        cmd.Parameters.Add(new SqlParameter("@acno_sEstadoId", ActoNotarial.acno_sEstadoId));                        
                        cmd.Parameters.Add(new SqlParameter("@acno_sUsuarioCreacion", ActoNotarial.acno_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@acno_vIPCreacion", ActoNotarial.acno_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        if (ActoNotarial.acno_sAccionSubTipoActoNotarialId != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sAccionSubTipoActoNotarialId", ActoNotarial.acno_sAccionSubTipoActoNotarialId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sAccionSubTipoActoNotarialId", DBNull.Value));
                        }

                        if (ActoNotarial.acno_sSubTipoActoNotarialExtraProtocolarId != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sSubTipoActoNotarialExtraProtocolarId", ActoNotarial.acno_sSubTipoActoNotarialExtraProtocolarId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sSubTipoActoNotarialExtraProtocolarId", DBNull.Value));
                        }


                        if (ActoNotarial.acno_sCondicionTipoActoNotarialId != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sCondicionTipoActoNotarialId", ActoNotarial.acno_sCondicionTipoActoNotarialId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sCondicionTipoActoNotarialId", 0));
                        }

                        //----------------------------------------------------------------
                        // Fecha: 16/03/2022
                        // Autor: Miguel Márquez
                        // Motivo: Adicionar las columnas acno_sAutorizacionDocumentoTipoId
                        //         acno_vAutorizacionDocumentoNumero
                        //----------------------------------------------------------------
                        if (ActoNotarial.acno_sAutorizacionDocumentoTipoId != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sAutorizacionDocumentoTipoId", ActoNotarial.acno_sAutorizacionDocumentoTipoId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sAutorizacionDocumentoTipoId", DBNull.Value));
                        }

                        if (ActoNotarial.acno_vAutorizacionDocumentoNumero != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_vAutorizacionDocumentoNumero", ActoNotarial.acno_vAutorizacionDocumentoNumero));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_vAutorizacionDocumentoNumero", string.Empty));
                        }
                        //----------------------------------------------------------------
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@acno_iActoNotarialId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;                        
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActoNotarial.acno_iActoNotarialId = Convert.ToInt64(lReturn.Value);
                        ActoNotarial.Error = false;
                    }
                }
            }
            catch (SqlException exec){
                strError = exec.Message;
                ActoNotarial.Error = true;
                ActoNotarial.Message = exec.Message.ToString();
            }
            catch (Exception exec)
            {
                strError = exec.Message;
                ActoNotarial.Error = true;
                ActoNotarial.Message = exec.Message.ToString();
            }
            return ActoNotarial;
        }

        public RE_ACTONOTARIAL actualizarFechaConclusionFirma(RE_ACTONOTARIAL ActoNotarial)
        {
            try
            {

                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {

                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIAL_ACTUALIZAR_DFECHACONCLUSIONFIRMA_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cnx.Open();

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@acno_iActoNotarialId", ActoNotarial.acno_iActoNotarialId));
                        
                        if (ActoNotarial.acno_sOficinaConsularId != 0) cmd.Parameters.Add(new SqlParameter("@acno_sOficinaConsularId", ActoNotarial.acno_sOficinaConsularId));

                        if (ActoNotarial.acno_dFechaConclusionFirma != DateTime.MinValue)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_dFechaConclusionFirma", ActoNotarial.acno_dFechaConclusionFirma));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_dFechaConclusionFirma", ActoNotarial.acno_dFechaExtension));
                        }
                        

                        cmd.Parameters.Add(new SqlParameter("@acno_sUsuarioModificacion", ActoNotarial.acno_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@acno_vIPModificacion", ActoNotarial.acno_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));


                        #endregion

                        cmd.ExecuteNonQuery();
                        ActoNotarial.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                ActoNotarial.Error = true;
                ActoNotarial.Message = exec.Message.ToString();
            }
            catch (Exception exec)
            {
                strError = exec.Message;
                ActoNotarial.Error = true;
                ActoNotarial.Message = exec.Message.ToString();
            }
            return ActoNotarial;
        }


        public RE_ACTONOTARIAL actualizar(RE_ACTONOTARIAL ActoNotarial)
        {
            try {

                using (SqlConnection cnx = new SqlConnection(this.conexion())) {

                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIAL_ACTUALIZAR", cnx)) {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cnx.Open();
                        
                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@acno_iActoNotarialId", ActoNotarial.acno_iActoNotarialId));
                        cmd.Parameters.Add(new SqlParameter("@acno_iActuacionId", ActoNotarial.acno_iActuacionId));

                        if (ActoNotarial.acno_sTipoActoNotarialId != 0)             cmd.Parameters.Add(new SqlParameter("@acno_sTipoActoNotarialId", ActoNotarial.acno_sTipoActoNotarialId));
                        if (ActoNotarial.acno_sSubTipoActoNotarialId !=0)           cmd.Parameters.Add(new SqlParameter("@acno_sSubTipoActoNotarialId", ActoNotarial.acno_sSubTipoActoNotarialId));
                        if (ActoNotarial.acno_sOficinaConsularId !=0)               cmd.Parameters.Add(new SqlParameter("@acno_sOficinaConsularId", ActoNotarial.acno_sOficinaConsularId));                        
                        if (ActoNotarial.acno_iActoNotarialReferenciaId != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_iActoNotarialReferenciaId", ActoNotarial.acno_iActoNotarialReferenciaId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_iActoNotarialReferenciaId", DBNull.Value));
                        }

                        if (ActoNotarial.acno_IFuncionarioAutorizadorId != 0)       cmd.Parameters.Add(new SqlParameter("@acno_IFuncionarioAutorizadorId", ActoNotarial.acno_IFuncionarioAutorizadorId));
                        if (ActoNotarial.acno_IFuncionarioCertificadorId !=0)       cmd.Parameters.Add(new SqlParameter("@acno_IFuncionarioCertificadorId", ActoNotarial.acno_IFuncionarioCertificadorId));
                        
                        if (ActoNotarial.acno_vAutorizacionTipo != null)            cmd.Parameters.Add(new SqlParameter("@acno_vAutorizacionTipo", ActoNotarial.acno_vAutorizacionTipo));
                        
                        if (ActoNotarial.acno_vDenominacion != null)                cmd.Parameters.Add(new SqlParameter("@acno_vDenominacion", ActoNotarial.acno_vDenominacion));
                        if (ActoNotarial.acno_bDigitalizadoFlag != null)            cmd.Parameters.Add(new SqlParameter("@acno_bDigitalizadoFlag", ActoNotarial.acno_bDigitalizadoFlag));
                        if (ActoNotarial.acno_bFlagMinuta != null)                  cmd.Parameters.Add(new SqlParameter("@acno_bFlagMinuta", ActoNotarial.acno_bFlagMinuta));
                        
                        if (ActoNotarial.acno_vNumeroColegiatura != null)           cmd.Parameters.Add(new SqlParameter("@acno_vNumeroColegiatura", ActoNotarial.acno_vNumeroColegiatura));

                        if (ActoNotarial.acno_iNumeroActoNotarial != null)          cmd.Parameters.Add(new SqlParameter("@acno_iNumeroActoNotarial", ActoNotarial.acno_iNumeroActoNotarial));
                        
                        if (ActoNotarial.acno_vNumeroEscrituraPublica != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_vNumeroEscrituraPublica", ActoNotarial.acno_vNumeroEscrituraPublica));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_vNumeroEscrituraPublica", DBNull.Value));
                        }

                        if (ActoNotarial.acno_vItinerario != null)                  cmd.Parameters.Add(new SqlParameter("@acno_vItinerario", ActoNotarial.acno_vItinerario));
                        if (ActoNotarial.acno_dFechaExtension != DateTime.MinValue) cmd.Parameters.Add(new SqlParameter("@acno_dFechaExtension", ActoNotarial.acno_dFechaExtension));
                        if (ActoNotarial.acno_vObservaciones != null)               cmd.Parameters.Add(new SqlParameter("@acno_vObservaciones", ActoNotarial.acno_vObservaciones));
                        
                        if (ActoNotarial.acno_cUbigeoDestino != null) 
                        {
                            if (ActoNotarial.acno_cUbigeoDestino != "" && ActoNotarial.acno_cUbigeoDestino.Length == 6)
                            {
                                cmd.Parameters.Add(new SqlParameter("@acno_cUbigeoDestino", ActoNotarial.acno_cUbigeoDestino));
                            }
                        } 

                        if (ActoNotarial.acno_IFuncionarioAnulaId != 0)             cmd.Parameters.Add(new SqlParameter("@acno_IFuncionarioAnulaId", ActoNotarial.acno_IFuncionarioAnulaId));
                        if (ActoNotarial.acno_dFechaAnulacion != DateTime.MinValue) cmd.Parameters.Add(new SqlParameter("@acno_dFechaAnulacion", ActoNotarial.acno_dFechaAnulacion));
                        if (ActoNotarial.acno_vMotivoAnulacion != null)             cmd.Parameters.Add(new SqlParameter("@acno_vMotivoAnulacion", ActoNotarial.acno_vMotivoAnulacion));

                        if (ActoNotarial.acno_sEstadoId == Convert.ToInt16(Enumerador.enmNotarialProtocolarEstado.ANULADA))
                            cmd.Parameters.Add(new SqlParameter("@acno_sEstadoId", ActoNotarial.acno_sEstadoId));
                        
                        cmd.Parameters.Add(new SqlParameter("@acno_sUsuarioModificacion", ActoNotarial.acno_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@acno_vIPModificacion", ActoNotarial.acno_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        if (ActoNotarial.acno_sAccionSubTipoActoNotarialId != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sAccionSubTipoActoNotarialId", ActoNotarial.acno_sAccionSubTipoActoNotarialId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sAccionSubTipoActoNotarialId", DBNull.Value));
                        }


                        if (ActoNotarial.acno_vNumeroOficio != null)
                        {
                            if (ActoNotarial.acno_vNumeroOficio != String.Empty)
                                cmd.Parameters.Add(new SqlParameter("@acno_vNumeroOficio", ActoNotarial.acno_vNumeroOficio));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_vNumeroOficio", DBNull.Value));
                        }


                        if (ActoNotarial.acno_vRegistradorNombre != null)
                        {
                            if (ActoNotarial.acno_vRegistradorNombre != String.Empty)
                                cmd.Parameters.Add(new SqlParameter("@acno_vRegistradorNombre", ActoNotarial.acno_vRegistradorNombre));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_vRegistradorNombre", DBNull.Value));
                        }

                        if (ActoNotarial.acno_cRegistradorUbigeo != null)
                        {
                            if (ActoNotarial.acno_cRegistradorUbigeo != String.Empty)
                                if (ActoNotarial.acno_cRegistradorUbigeo != "000000")
                                    if (ActoNotarial.acno_cRegistradorUbigeo.Trim().Length == 6)
                                        cmd.Parameters.Add(new SqlParameter("@acno_cRegistradorUbigeo", ActoNotarial.acno_cRegistradorUbigeo));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_cRegistradorUbigeo", DBNull.Value));
                        }


                        if (ActoNotarial.acno_vPresentanteNombre != null)
                        {
                            if (ActoNotarial.acno_vPresentanteNombre != String.Empty)
                                cmd.Parameters.Add(new SqlParameter("@acno_vRepresentanteNombre", ActoNotarial.acno_vPresentanteNombre));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_vRepresentanteNombre", DBNull.Value));
                        }

                        if (ActoNotarial.acno_sPresentanteTipoDocumento != null)
                            if (ActoNotarial.acno_sPresentanteTipoDocumento != 0)
                                cmd.Parameters.Add(new SqlParameter("@acno_sRepresentanteTipoDoc", ActoNotarial.acno_sPresentanteTipoDocumento));
                        else
                                cmd.Parameters.Add(new SqlParameter("@acno_sRepresentanteTipoDoc", DBNull.Value));


                        if (ActoNotarial.acno_vPresentanteNumeroDocumento != null)
                            if (ActoNotarial.acno_vPresentanteNumeroDocumento != String.Empty)
                                cmd.Parameters.Add(new SqlParameter("@acno_vRepresentanteNumeroDoc", ActoNotarial.acno_vPresentanteNumeroDocumento));
                            else
                                cmd.Parameters.Add(new SqlParameter("@acno_vRepresentanteNumeroDoc", DBNull.Value));


                        if (ActoNotarial.acno_sPresentanteGenero != null)
                            if (ActoNotarial.acno_sPresentanteGenero != 0)
                                cmd.Parameters.Add(new SqlParameter("@acno_sPresentanteGenero", ActoNotarial.acno_sPresentanteGenero));
                            else
                                cmd.Parameters.Add(new SqlParameter("@acno_sPresentanteGenero", DBNull.Value));

                        if (ActoNotarial.acno_vNombreColegiatura != null) cmd.Parameters.Add(new SqlParameter("@acno_vNombreColegiatura", ActoNotarial.acno_vNombreColegiatura));

                        if (ActoNotarial.acno_sSubTipoActoNotarialExtraProtocolarId != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sSubTipoActoNotarialExtraProtocolarId", ActoNotarial.acno_sSubTipoActoNotarialExtraProtocolarId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sSubTipoActoNotarialExtraProtocolarId", DBNull.Value));
                        }


                        if (ActoNotarial.acno_sCondicionTipoActoNotarialId != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sCondicionTipoActoNotarialId", ActoNotarial.acno_sCondicionTipoActoNotarialId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sCondicionTipoActoNotarialId", 0));
                        }
                        if (ActoNotarial.acno_vPartidaRegistral != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_vPartidaRegistral", ActoNotarial.acno_vPartidaRegistral));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_vPartidaRegistral", null));
                        }
                        if (ActoNotarial.acno_iOficinaRegistralId != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_iOficinaRegistralId", ActoNotarial.acno_iOficinaRegistralId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_iOficinaRegistralId", null));
                        }
                        //----------------------------------------------------------------
                        // Fecha: 16/03/2022
                        // Autor: Miguel Márquez
                        // Motivo: Adicionar las columnas acno_sAutorizacionDocumentoTipoId
                        //         acno_vAutorizacionDocumentoNumero
                        //----------------------------------------------------------------
                        if (ActoNotarial.acno_sAutorizacionDocumentoTipoId != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sAutorizacionDocumentoTipoId", ActoNotarial.acno_sAutorizacionDocumentoTipoId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sAutorizacionDocumentoTipoId", DBNull.Value));
                        }

                        if (ActoNotarial.acno_vAutorizacionDocumentoNumero != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_vAutorizacionDocumentoNumero", ActoNotarial.acno_vAutorizacionDocumentoNumero));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_vAutorizacionDocumentoNumero", string.Empty));
                        }
                        //----------------------------------------------------------------

                        #endregion

                        cmd.ExecuteNonQuery();
                        ActoNotarial.Error = false;
                    }
                }
            }
            catch (SqlException exec) {
                strError = exec.Message;
                ActoNotarial.Error = true;
                ActoNotarial.Message = exec.Message.ToString();
            }
            catch (Exception exec)
            {
                strError = exec.Message;
                ActoNotarial.Error = true;
                ActoNotarial.Message = exec.Message.ToString();
            }
            return ActoNotarial;
        }

        public RE_ACTONOTARIAL actualizar_Ep_Foj_Vin(RE_ACTONOTARIAL ActoNotarial)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIAL_ACTUALIZAR_NEP_NF_VIN", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cnx.Open();

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@acno_iActoNotarialId", ActoNotarial.acno_iActoNotarialId));

                        if (ActoNotarial.acno_iActoNotarialReferenciaId != 0)
                        {

                            cmd.Parameters.Add(new SqlParameter("@acno_iActoNotarialReferenciaId", ActoNotarial.acno_iActoNotarialReferenciaId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_iActoNotarialReferenciaId", null));
                        }
                        
                        if (ActoNotarial.acno_vNumeroLibro != null) cmd.Parameters.Add(new SqlParameter("@acno_vNumeroLibro", ActoNotarial.acno_vNumeroLibro));
                        if (ActoNotarial.acno_vNumeroFojaInicial != null) cmd.Parameters.Add(new SqlParameter("@acno_vNumeroFojaInicial", ActoNotarial.acno_vNumeroFojaInicial));
                        if (ActoNotarial.acno_vNumeroFojaFinal != null) cmd.Parameters.Add(new SqlParameter("@acno_vNumeroFojaFinal", ActoNotarial.acno_vNumeroFojaFinal));
                        if (ActoNotarial.acno_vNumeroEscrituraPublica != null) cmd.Parameters.Add(new SqlParameter("@acno_vNumeroEscrituraPublica", ActoNotarial.acno_vNumeroEscrituraPublica));
                        if (ActoNotarial.acno_vNumeroEscrituraPublica != null) cmd.Parameters.Add(new SqlParameter("@acno_nCostoEP", ActoNotarial.acno_nCostoEP));
                        if (ActoNotarial.acno_vNumeroEscrituraPublica != null) cmd.Parameters.Add(new SqlParameter("@acno_nCostoParte2", ActoNotarial.acno_nCostoParte2));
                        if (ActoNotarial.acno_vNumeroEscrituraPublica != null) cmd.Parameters.Add(new SqlParameter("@acno_nCostoTestimonio", ActoNotarial.acno_nCostoTestimonio));
                        
                        cmd.Parameters.Add(new SqlParameter("@acno_sOficinaConsularId", ActoNotarial.acno_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@acno_sUsuarioModificacion", ActoNotarial.acno_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@acno_vIPModificacion", ActoNotarial.acno_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@acno_vHostName", Util.ObtenerHostName()));
                        #endregion

                        cmd.ExecuteNonQuery();

                        ActoNotarial.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                ActoNotarial.Error = true;
                ActoNotarial.Message = exec.Message.ToString();
            }
            catch (Exception exec)
            {
                strError = exec.Message;
                ActoNotarial.Error = true;
                ActoNotarial.Message = exec.Message.ToString();
            }
            return ActoNotarial;
        }

        public RE_ACTONOTARIAL actualizar_Estado(RE_ACTONOTARIAL ActoNotarial)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIAL_PROTOCOLAR_ACTUALIZAR_ESTADO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cnx.Open();

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@acno_iActoNotarialId", ActoNotarial.acno_iActoNotarialId));

                        cmd.Parameters.Add(new SqlParameter("@acno_sEstadoId", ActoNotarial.acno_sEstadoId));

                        cmd.Parameters.Add(new SqlParameter("@acno_sOficinaConsularId", ActoNotarial.acno_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@acno_sUsuarioModificacion", ActoNotarial.acno_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@acno_vIPModificacion", ActoNotarial.acno_vIPCreacion));

                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@acno_sTamanoLetra", ActoNotarial.acno_sTamanoLetra));
                        cmd.Parameters.Add(new SqlParameter("@libr_sTipoLibroId", ActoNotarial.sTipoLibroId));
                        cmd.Parameters.Add(new SqlParameter("@acno_sNumeroHojas", ActoNotarial.acno_sNumeroHojas));

                        if (ActoNotarial.iNumeroEscrituraPublica != null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ano_iNumeroEscrituraPublica", ActoNotarial.iNumeroEscrituraPublica));
                        }
                        
                        #endregion

                        cmd.ExecuteNonQuery();

                        ActoNotarial.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                ActoNotarial.Error = true;
                ActoNotarial.Message = exec.Message.ToString();
            }
            catch (Exception exec)
            {
                strError = exec.Message;
                ActoNotarial.Error = true;
                ActoNotarial.Message = exec.Message.ToString();
            }
            return ActoNotarial;
        }

        public RE_ACTONOTARIAL obtener(RE_ACTONOTARIAL ActoNotarial) {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIAL_OBTENER", cnx))                    
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        if (ActoNotarial.acno_iActoNotarialId !=0 )       cmd.Parameters.Add(new SqlParameter("@acno_iActoNotarialId", ActoNotarial.acno_iActoNotarialId));
                        if (ActoNotarial.acno_iActuacionId !=0)           cmd.Parameters.Add(new SqlParameter("@acno_iActuacionId", ActoNotarial.acno_iActuacionId));
                        if (ActoNotarial.acno_sTipoActoNotarialId != 0)   cmd.Parameters.Add(new SqlParameter("@acno_sTipoActoNotarialId", ActoNotarial.acno_sTipoActoNotarialId));
                        if (ActoNotarial.acno_sSubTipoActoNotarialId !=0) cmd.Parameters.Add(new SqlParameter("@acno_sSubTipoActoNotarialId", ActoNotarial.acno_sSubTipoActoNotarialId));
                        if (ActoNotarial.acno_sOficinaConsularId!=0)      cmd.Parameters.Add(new SqlParameter("@acno_sOficinaConsularId", ActoNotarial.acno_sOficinaConsularId));
                        if (ActoNotarial.acno_iActoNotarialReferenciaId != 0) cmd.Parameters.Add(new SqlParameter("@acno_iActoNotarialReferenciaId", ActoNotarial.acno_iActoNotarialReferenciaId));
                        #endregion

                        using (SqlDataReader loReader = cmd.ExecuteReader()){
                            while (loReader.Read()){
                                for (int col = 0; col <= loReader.FieldCount - 1; col++){
                                    if (loReader[col].GetType().ToString() != "System.DBNull"){
                                        PropertyInfo pInfo = (PropertyInfo)ActoNotarial.GetType().GetProperty(loReader.GetName(col).ToString(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                        if (pInfo != null)
                                        {
                                            pInfo.SetValue(ActoNotarial, Convert.ChangeType(loReader[col], pInfo.PropertyType), null);
                                        }
                                    }
                                }
                            }
                        }

                        ActoNotarial.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                ActoNotarial.Error = true;
                ActoNotarial.Message = exec.Message.ToString();
            }
            catch (Exception exec)
            {
                strError = exec.Message;
                ActoNotarial.Error = true;
                ActoNotarial.Message = exec.Message.ToString();
            }
            return ActoNotarial;            
        }

        public List<RE_ACTONOTARIAL> paginado(RE_ACTONOTARIAL ActoNotarial) {
            return null;
        }

        string conexion() {
            return ConfigurationManager.AppSettings["ConexionSGAC"];

        }


        public DataTable ObtenerTarifaTipoActo(Int16 sTipoActoId)
        {
            try{
                    using(SqlConnection cnn = new SqlConnection(this.conexion())){
                        using (SqlCommand cmd = new SqlCommand("[PS_SISTEMA].[USP_SI_PARAMETRO_OBTENER_POR_ACTOPROTOCOLAR]", cnn)) {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@acno_sSubTipoNotarialId", SqlDbType.SmallInt).Value = sTipoActoId;
                            DataSet ds = new DataSet();
                            DataTable dt = new DataTable();
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd)) {
                                da.Fill(dt);
                                return dt;
                            }
                        }
                    }
            }
            catch(SqlException){
                return null;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
     
        public string ExisteNumeroEscritura(BE.MRE.SI_LIBRO objLibro)
        {
            string strMensaje = string.Empty;
            try
            {
                using (SqlConnection cnn = new SqlConnection(this.conexion())) {
                    using (SqlCommand cmd = new SqlCommand("[PN_REGISTRO].[USP_RE_EXISTE_NUMEROESCRITURA]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@libr_sTipoLibroId", SqlDbType.SmallInt).Value = objLibro.libr_sTipoLibroId;
                        cmd.Parameters.Add("@libr_INumeroEscritura", SqlDbType.VarChar).Value = objLibro.libr_INumeroEscritura;
                        cmd.Parameters.Add("@libr_sOficinaConsularId", SqlDbType.SmallInt).Value = objLibro.libr_sOficinaConsularId;

                        cmd.Parameters.Add("@libr_sPeriodo", SqlDbType.SmallInt).Value = objLibro.libr_sPeriodo;
                        cmd.Parameters.Add("@libr_INumeroLibro", SqlDbType.Int).Value = objLibro.libr_INumeroLibro;
                        cmd.Parameters.Add("@libr_INumeroFolioInicial", SqlDbType.Int).Value = objLibro.libr_INumeroFolioInicial;
                        cmd.Parameters.Add("@libr_INumeroFolioActual", SqlDbType.Int).Value = objLibro.libr_INumeroFolioActual;
                        cmd.Parameters.Add("@libr_INumeroFolioTotal", SqlDbType.Int).Value = objLibro.libr_INumeroFolioTotal;

                        cmd.Parameters.Add("@libr_sLibroId", SqlDbType.SmallInt).Value = objLibro.libr_sLibroId;

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@vMensaje", SqlDbType.VarChar, 200);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        strMensaje = Convert.ToString(lReturn.Value);

                    }
                }
            }
            catch (Exception ex) {
                strError = ex.Message;
                strMensaje = ex.Message;
            }
            return strMensaje;
        }


        public Boolean ExisteNumeroLibro(Int32 libr_INumeroLibro, Int16 libr_sOficinaConsularId)
        {
            Boolean Resultado = false;
            try
            {
                using (SqlConnection cnn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REGISTRO].[USP_RE_EXISTE_NUMERO_LIBRO]", cnn))
                    {
                        Int32 TotalNro = 0;

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@libr_INumeroLibro", SqlDbType.BigInt).Value = libr_INumeroLibro;
                        cmd.Parameters.Add("@libr_sOficinaConsularId", SqlDbType.VarChar).Value = libr_sOficinaConsularId;

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@Rspta", SqlDbType.Int);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        TotalNro = Convert.ToInt32(lReturn.Value);

                        if (TotalNro == 0)
                        {
                            Resultado = false;
                        }
                        else
                        {
                            Resultado = true;
                        }

                        return Resultado;

                    }
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return Resultado;
            }
        }


        public Int32 ValidarFojas(Int64 acno_iActoNotarialID, Int16 libr_sTipoLibroId, Int16 libr_sOficinaConsularId, Int32 acno_sNumeroHojas, Int32 operacion, ref String oMensaje, ref Int32 FojaInicial, ref Int32 FojaFinal, ref Int32 acno_INumeroEscritura,ref String acno_INumeroLibro)
        {
            Int32 Resultado = 0;
            try
            {
                using (SqlConnection cnn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REGISTRO].[USP_RE_ACTO_NOTARIAL_VALIDAR_FOJAS]", cnn))
                    {
                        Int32 TotalNro = 0;

                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@acno_iActoNotarialID", SqlDbType.BigInt).Value = acno_iActoNotarialID;
                        cmd.Parameters.Add("@libr_sTipoLibroId", SqlDbType.SmallInt).Value = libr_sTipoLibroId;
                        cmd.Parameters.Add("@acno_sOficinaConsularId", SqlDbType.SmallInt).Value = libr_sOficinaConsularId;
                        cmd.Parameters.Add("@acno_sNumeroHojas", SqlDbType.SmallInt).Value = acno_sNumeroHojas;
                        cmd.Parameters.Add("@operacion", SqlDbType.Int).Value = operacion;

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@Rspta", SqlDbType.Int);
                        lReturn.Direction = ParameterDirection.Output;

                         SqlParameter Mensjajes = cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar,200);
                        Mensjajes.Direction = ParameterDirection.Output;

                        SqlParameter sFojaInicio = cmd.Parameters.Add("@FojaInicial", SqlDbType.Int,5);
                        sFojaInicio.Direction = ParameterDirection.Output;

                        SqlParameter sFojaFinal = cmd.Parameters.Add("@FojaFinal", SqlDbType.Int,5);
                        sFojaFinal.Direction = ParameterDirection.Output;

                        SqlParameter INumeroEscritura = cmd.Parameters.Add("@acno_vNumeroEscrituraPublica", SqlDbType.Int, 5);
                        INumeroEscritura.Direction = ParameterDirection.Output;

                        SqlParameter vNumeroLibro = cmd.Parameters.Add("@acno_vNumeroLibro", SqlDbType.VarChar, 50);
                        vNumeroLibro.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        Resultado = Convert.ToInt32(lReturn.Value);
                        oMensaje =  Convert.ToString(Mensjajes.Value);
                        FojaInicial = Convert.ToInt32(sFojaInicio.Value);
                        FojaFinal = Convert.ToInt32(sFojaFinal.Value);
                        acno_INumeroEscritura = Convert.ToInt32(INumeroEscritura.Value);
                        acno_INumeroLibro = Convert.ToString(vNumeroLibro.Value);
                         
                        return Resultado;
                    }
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return Resultado;
            }
        }

        //-----------------------------------------
        //Fecha: 10/11/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Búsqueda de la Escritura Pública.
        //-----------------------------------------
        public RE_ACTONOTARIAL BusquedaEscrituraPublica(string strAnioEscritura, string strNumeroEscrituraPublica, Int16 iOficinaConsularId, string strPG_PE)
        {
            RE_ACTONOTARIAL ActoNotarial = new RE_ACTONOTARIAL();
            
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIAL_BUSQUEDA_ESCRITURA_PUBLICA_MRE", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@P_CANIOESCRITURA", strAnioEscritura));
                        cmd.Parameters.Add(new SqlParameter("@P_VNUMEROESCRITURAPUBLICA", strNumeroEscrituraPublica));
                        cmd.Parameters.Add(new SqlParameter("@P_SOFICINACONSULARID", iOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@P_CPG_PE", strPG_PE));
                        #endregion

                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                ActoNotarial.acno_iActoNotarialId = Convert.ToInt64(loReader["acno_iActoNotarialId"].ToString());
                                ActoNotarial.FechaExtension = (Util.FormatearFecha(loReader["acno_dFechaExtension"].ToString())).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                                ActoNotarial.acno_sTipoActoNotarialId = Convert.ToInt16(loReader["acno_sTipoActoNotarialId"].ToString());
                                ActoNotarial.acno_sSubTipoActoNotarialId = Convert.ToInt16(loReader["acno_sSubTipoActoNotarialId"].ToString());
                                ActoNotarial.Tipo_Acto = loReader["TIPO_ACTO"].ToString();
                                ActoNotarial.Sub_Tipo = loReader["SUB_TIPO"].ToString();
                                
                            }
                        }

                        ActoNotarial.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                strError = exec.Message;
                ActoNotarial.Error = true;
                ActoNotarial.Message = exec.Message.ToString();
            }
            catch (Exception exec)
            {
                strError = exec.Message;
                ActoNotarial.Error = true;
                ActoNotarial.Message = exec.Message.ToString();
            }
            return ActoNotarial;
        }
        //-----------------------------------------
    }
}
