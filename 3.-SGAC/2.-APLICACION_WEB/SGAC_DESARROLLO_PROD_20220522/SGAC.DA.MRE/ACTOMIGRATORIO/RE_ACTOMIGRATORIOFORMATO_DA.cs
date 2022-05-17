using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGAC.BE.MRE;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace SGAC.DA.MRE.ACTOMIGRATORIO
{
    public class RE_ACTOMIGRATORIOFORMATO_DA
    {
        public RE_ACTOMIGRATORIOFORMATO insertar(RE_ACTOMIGRATORIOFORMATO ActoMigratorioFormato)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOMIGRATORIOFORMATO_ADICIONAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@amfr_iActoMigratorioId", ActoMigratorioFormato.amfr_iActoMigratorioId));
                        if (ActoMigratorioFormato.amfr_sTitularFamiliaId == 0)
                            cmd.Parameters.Add(new SqlParameter("@amfr_sTitularFamiliaId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@amfr_sTitularFamiliaId", ActoMigratorioFormato.amfr_sTitularFamiliaId));

                        cmd.Parameters.Add(new SqlParameter("@amfr_bAcuerdoProgramaFlag", ActoMigratorioFormato.amfr_bAcuerdoProgramaFlag));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vVisaCodificacion", ActoMigratorioFormato.amfr_vVisaCodificacion));
                        cmd.Parameters.Add(new SqlParameter("@amfr_sDiasPermanencia", ActoMigratorioFormato.amfr_sDiasPermanencia));
                        cmd.Parameters.Add(new SqlParameter("@amfr_sPasaporteRevalidarOficinaConsularId", ActoMigratorioFormato.amfr_sPasaporteRevalidarOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@amfr_sPasaporteRevalidarOficinaMigracionId", ActoMigratorioFormato.amfr_sPasaporteRevalidarOficinaMigracionId));
                        cmd.Parameters.Add(new SqlParameter("@amfr_dPasaporteRevalidarFechaExpedicion", ActoMigratorioFormato.amfr_dPasaporteRevalidarFechaExpedicion));
                        cmd.Parameters.Add(new SqlParameter("@amfr_dPasaporteRevalidarFechaExpiracion", ActoMigratorioFormato.amfr_dPasaporteRevalidarFechaExpiracion));
                        cmd.Parameters.Add(new SqlParameter("@amfr_sTipoAutorizacionId", ActoMigratorioFormato.amfr_sTipoAutorizacionId));

                        if (ActoMigratorioFormato.amfr_sTipoDocumentoRREEId == 0)
                            cmd.Parameters.Add(new SqlParameter("@amfr_sTipoDocumentoRREEId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@amfr_sTipoDocumentoRREEId", ActoMigratorioFormato.amfr_sTipoDocumentoRREEId));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vNumDocumentoRREE", ActoMigratorioFormato.amfr_vNumDocumentoRREE));
                        cmd.Parameters.Add(new SqlParameter("@amfr_dFechaRREE", ActoMigratorioFormato.amfr_dFechaRREE));

                        if (ActoMigratorioFormato.amfr_sTipoDocumentoDIGEMINId == 0)
                            cmd.Parameters.Add(new SqlParameter("@amfr_sTipoDocumentoDIGEMINId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@amfr_sTipoDocumentoDIGEMINId", ActoMigratorioFormato.amfr_sTipoDocumentoDIGEMINId));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vNumDocumentoDIGEMIN", ActoMigratorioFormato.amfr_vNumDocumentoDIGEMIN));
                        cmd.Parameters.Add(new SqlParameter("@amfr_dFechaDIGEMIN", ActoMigratorioFormato.amfr_dFechaDIGEMIN));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vCargoFuncionario", ActoMigratorioFormato.amfr_vCargoFuncionario));
                        cmd.Parameters.Add(new SqlParameter("@amfr_bFlagVisaDiplomatica", ActoMigratorioFormato.amfr_bFlagVisaDiplomatica));
                        cmd.Parameters.Add(new SqlParameter("@amfr_sCargoDiplomaticoId", ActoMigratorioFormato.amfr_sCargoDiplomaticoId));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vMotivoVisaDiplomatica", ActoMigratorioFormato.amfr_vMotivoVisaDiplomatica));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vInstitucionSolicitaVisaDiplomatica", ActoMigratorioFormato.amfr_vInstitucionSolicitaVisaDiplomatica));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vInstitucionRealizaVisaDiplomatica", ActoMigratorioFormato.amfr_vInstitucionRealizaVisaDiplomatica));

                        if (ActoMigratorioFormato.amfr_sCancilleriaSolicitaAutorizacionId == 0)
                            cmd.Parameters.Add(new SqlParameter("@amfr_sCancilleriaSolicitaAutorizacionId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@amfr_sCancilleriaSolicitaAutorizacionId", ActoMigratorioFormato.amfr_sCancilleriaSolicitaAutorizacionId));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vDocumentoAutoriza", ActoMigratorioFormato.amfr_vDocumentoAutoriza));
                        cmd.Parameters.Add(new SqlParameter("@amfr_bFlagVisaPrensa", ActoMigratorioFormato.amfr_bFlagVisaPrensa));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vMedioComunicacionPrensa", ActoMigratorioFormato.amfr_vMedioComunicacionPrensa));
                        cmd.Parameters.Add(new SqlParameter("@amfr_sCargoPrensaId", ActoMigratorioFormato.amfr_sCargoPrensaId));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vMotivoPrensa", ActoMigratorioFormato.amfr_vMotivoPrensa));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vObservaciones", ActoMigratorioFormato.amfr_vObservaciones));

                        cmd.Parameters.Add(new SqlParameter("@amfr_cEstado", ActoMigratorioFormato.amfr_cEstado));
                        cmd.Parameters.Add(new SqlParameter("@amfr_sUsuarioCreacion", ActoMigratorioFormato.amfr_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vIPCreacion", ActoMigratorioFormato.amfr_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@amfr_dFechaCreacion", ActoMigratorioFormato.amfr_dFechaCreacion));
                        cmd.Parameters.Add(new SqlParameter("@amfr_sUsuarioModificacion", ActoMigratorioFormato.amfr_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vIPModificacion", ActoMigratorioFormato.amfr_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@amfr_dFechaModificacion", ActoMigratorioFormato.amfr_dFechaModificacion));

                        cmd.Parameters.Add(new SqlParameter("@amfr_sTipoNumeroPasaporteId", ActoMigratorioFormato.amfr_sTipoNumeroPasaporteId));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vNumeroPasaporte", ActoMigratorioFormato.amfr_vNumeroPasaporte));

                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", ActoMigratorioFormato.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", ActoMigratorioFormato.HostName));

                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@amfr_iActoMigratorioFormatoId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActoMigratorioFormato.amfr_iActoMigratorioFormatoId = Convert.ToInt64(lReturn.Value);
                        ActoMigratorioFormato.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                ActoMigratorioFormato.Error = true;
                ActoMigratorioFormato.Message = exec.Message.ToString();
            }
            return ActoMigratorioFormato;
        }

        public RE_ACTOMIGRATORIOFORMATO actualizar(RE_ACTOMIGRATORIOFORMATO ActoMigratorioFormato)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTOMIGRATORIOFORMATO_ACTUALIZAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@amfr_iActoMigratorioFormatoId", ActoMigratorioFormato.amfr_iActoMigratorioFormatoId));
                        cmd.Parameters.Add(new SqlParameter("@amfr_iActoMigratorioId", ActoMigratorioFormato.amfr_iActoMigratorioId));
                        if (ActoMigratorioFormato.amfr_sTitularFamiliaId == 0)
                            cmd.Parameters.Add(new SqlParameter("@amfr_sTitularFamiliaId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@amfr_sTitularFamiliaId", ActoMigratorioFormato.amfr_sTitularFamiliaId));

                        cmd.Parameters.Add(new SqlParameter("@amfr_bAcuerdoProgramaFlag", ActoMigratorioFormato.amfr_bAcuerdoProgramaFlag));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vVisaCodificacion", ActoMigratorioFormato.amfr_vVisaCodificacion));
                        cmd.Parameters.Add(new SqlParameter("@amfr_sDiasPermanencia", ActoMigratorioFormato.amfr_sDiasPermanencia));
                        cmd.Parameters.Add(new SqlParameter("@amfr_sPasaporteRevalidarOficinaConsularId", ActoMigratorioFormato.amfr_sPasaporteRevalidarOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@amfr_sPasaporteRevalidarOficinaMigracionId", ActoMigratorioFormato.amfr_sPasaporteRevalidarOficinaMigracionId));
                        cmd.Parameters.Add(new SqlParameter("@amfr_dPasaporteRevalidarFechaExpedicion", ActoMigratorioFormato.amfr_dPasaporteRevalidarFechaExpedicion));
                        cmd.Parameters.Add(new SqlParameter("@amfr_dPasaporteRevalidarFechaExpiracion", ActoMigratorioFormato.amfr_dPasaporteRevalidarFechaExpiracion));
                        cmd.Parameters.Add(new SqlParameter("@amfr_sTipoAutorizacionId", ActoMigratorioFormato.amfr_sTipoAutorizacionId));

                        if (ActoMigratorioFormato.amfr_sTipoDocumentoRREEId == 0)
                            cmd.Parameters.Add(new SqlParameter("@amfr_sTipoDocumentoRREEId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@amfr_sTipoDocumentoRREEId", ActoMigratorioFormato.amfr_sTipoDocumentoRREEId));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vNumDocumentoRREE", ActoMigratorioFormato.amfr_vNumDocumentoRREE));
                        cmd.Parameters.Add(new SqlParameter("@amfr_dFechaRREE", ActoMigratorioFormato.amfr_dFechaRREE));

                        if (ActoMigratorioFormato.amfr_sTipoDocumentoDIGEMINId == 0)
                            cmd.Parameters.Add(new SqlParameter("@amfr_sTipoDocumentoDIGEMINId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@amfr_sTipoDocumentoDIGEMINId", ActoMigratorioFormato.amfr_sTipoDocumentoDIGEMINId));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vNumDocumentoDIGEMIN", ActoMigratorioFormato.amfr_vNumDocumentoDIGEMIN));
                        cmd.Parameters.Add(new SqlParameter("@amfr_dFechaDIGEMIN", ActoMigratorioFormato.amfr_dFechaDIGEMIN));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vCargoFuncionario", ActoMigratorioFormato.amfr_vCargoFuncionario));
                        cmd.Parameters.Add(new SqlParameter("@amfr_bFlagVisaDiplomatica", ActoMigratorioFormato.amfr_bFlagVisaDiplomatica));
                        cmd.Parameters.Add(new SqlParameter("@amfr_sCargoDiplomaticoId", ActoMigratorioFormato.amfr_sCargoDiplomaticoId));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vMotivoVisaDiplomatica", ActoMigratorioFormato.amfr_vMotivoVisaDiplomatica));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vInstitucionSolicitaVisaDiplomatica", ActoMigratorioFormato.amfr_vInstitucionSolicitaVisaDiplomatica));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vInstitucionRealizaVisaDiplomatica", ActoMigratorioFormato.amfr_vInstitucionRealizaVisaDiplomatica));
                        if (ActoMigratorioFormato.amfr_sCancilleriaSolicitaAutorizacionId == 0)
                            cmd.Parameters.Add(new SqlParameter("@amfr_sCancilleriaSolicitaAutorizacionId", DBNull.Value));
                        else
                            cmd.Parameters.Add(new SqlParameter("@amfr_sCancilleriaSolicitaAutorizacionId", ActoMigratorioFormato.amfr_sCancilleriaSolicitaAutorizacionId));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vDocumentoAutoriza", ActoMigratorioFormato.amfr_vDocumentoAutoriza));
                        cmd.Parameters.Add(new SqlParameter("@amfr_bFlagVisaPrensa", ActoMigratorioFormato.amfr_bFlagVisaPrensa));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vMedioComunicacionPrensa", ActoMigratorioFormato.amfr_vMedioComunicacionPrensa));
                        cmd.Parameters.Add(new SqlParameter("@amfr_sCargoPrensaId", ActoMigratorioFormato.amfr_sCargoPrensaId));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vMotivoPrensa", ActoMigratorioFormato.amfr_vMotivoPrensa));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vObservaciones", ActoMigratorioFormato.amfr_vObservaciones));

                        cmd.Parameters.Add(new SqlParameter("@amfr_sUsuarioModificacion", ActoMigratorioFormato.amfr_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vIPModificacion", ActoMigratorioFormato.amfr_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@amfr_sOficinaConsularId", ActoMigratorioFormato.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vHostName", ActoMigratorioFormato.HostName));
                        cmd.Parameters.Add(new SqlParameter("@amfr_sTipoNumeroPasaporteId", ActoMigratorioFormato.amfr_sTipoNumeroPasaporteId));
                        cmd.Parameters.Add(new SqlParameter("@amfr_vNumeroPasaporte", ActoMigratorioFormato.amfr_vNumeroPasaporte));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@ReturnValue", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;
                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        ActoMigratorioFormato.amfr_iActoMigratorioFormatoId = Convert.ToInt64(lReturn.Value);
                        ActoMigratorioFormato.Error = false;
                    }
                }
            }
            catch (SqlException exec)
            {
                ActoMigratorioFormato.Error = true;
                ActoMigratorioFormato.Message = exec.Message.ToString();
            }
            return ActoMigratorioFormato;
        }

        public RE_ACTOMIGRATORIOFORMATO obtener(RE_ACTOMIGRATORIOFORMATO ActoMigratorio)
        {
            return null;
        }

        public List<RE_ACTOMIGRATORIOFORMATO> paginado(RE_ACTOMIGRATORIOFORMATO ActoMigratorio)
        {
            return null;
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }
    }
}
