using System;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

using SGAC.BE.MRE;
//-----------------------------------------------------------
// Autor: Miguel Márquez Beltrán
// Fecha: 11/01/2017
// Objetivo: clase de acceso a datos de la ficha registral
//-----------------------------------------------------------

namespace SGAC.Registro.Actuacion.DA
{
    public class FichaRegistralDA
    {
        public RE_FICHAREGISTRAL insertar(RE_FICHAREGISTRAL fichaRegistral)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHAREGISTRAL_ADICIONAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@fire_iActuacionDetalleId", fichaRegistral.fire_iActuacionDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@fire_vNumeroFicha", fichaRegistral.fire_vNumeroFicha));
                        cmd.Parameters.Add(new SqlParameter("@fire_vCodigoLocal", fichaRegistral.fire_vCodigoLocal));
                        cmd.Parameters.Add(new SqlParameter("@fire_vCodigoLocalDestino", fichaRegistral.fire_vCodigoLocalDestino));
                        cmd.Parameters.Add(new SqlParameter("@fire_sEstadoId", fichaRegistral.fire_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@fire_dFechaEstado", fichaRegistral.fire_dFechaEstado));
                        cmd.Parameters.Add(new SqlParameter("@fire_vObservacion", fichaRegistral.fire_vObservacion));
                        cmd.Parameters.Add(new SqlParameter("@fire_vNumeroGuia", fichaRegistral.fire_vNumeroGuia));
                        cmd.Parameters.Add(new SqlParameter("@fire_sUsuarioCreacion", fichaRegistral.fire_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@fire_vIPCreacion", fichaRegistral.fire_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", fichaRegistral.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@fire_vTipoRegistro", fichaRegistral.fire_vTipoRegistro));
                        cmd.Parameters.Add(new SqlParameter("@fire_dFechaEnvio", fichaRegistral.fire_dFechaEnvio));
                        cmd.Parameters.Add(new SqlParameter("@fire_vNumeroOficio", fichaRegistral.fire_vNumeroOficio));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@fire_iFichaRegistralId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@fihi_iFichaHistoricoId", SqlDbType.BigInt);
                        lReturn2.Direction = ParameterDirection.Output;

                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        fichaRegistral.fire_iFichaRegistralId = Convert.ToInt64(lReturn.Value);
                        fichaRegistral.fihi_iFichaHistoricoId = Convert.ToInt64(lReturn2.Value);
                        fichaRegistral.Error = false;
                    }
                }

            }
            catch (SqlException exec)
            {
                fichaRegistral.Error = true;
                fichaRegistral.Message = exec.Message.ToString();
            }
            return fichaRegistral;
        }

        public RE_FICHAREGISTRAL actualizar(RE_FICHAREGISTRAL fichaRegistral)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHAREGISTRAL_ACTUALIZAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@fire_iFichaRegistralId", fichaRegistral.fire_iFichaRegistralId));
                        cmd.Parameters.Add(new SqlParameter("@fire_vNumeroFicha", fichaRegistral.fire_vNumeroFicha));
                        cmd.Parameters.Add(new SqlParameter("@fire_vCodigoLocal", fichaRegistral.fire_vCodigoLocal));
                        cmd.Parameters.Add(new SqlParameter("@fire_vCodigoLocalDestino", fichaRegistral.fire_vCodigoLocalDestino));
                        cmd.Parameters.Add(new SqlParameter("@fire_dFechaEstado", fichaRegistral.fire_dFechaEstado));
                        cmd.Parameters.Add(new SqlParameter("@fire_vObservacion", fichaRegistral.fire_vObservacion));
                        cmd.Parameters.Add(new SqlParameter("@fire_vNumeroGuia", fichaRegistral.fire_vNumeroGuia));
                        cmd.Parameters.Add(new SqlParameter("@fire_sEstadoId", fichaRegistral.fire_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@fire_sUsuarioModificacion", fichaRegistral.fire_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@fire_vIPModificacion", fichaRegistral.fire_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", fichaRegistral.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@fire_vTipoRegistro", fichaRegistral.fire_vTipoRegistro));
                        cmd.Parameters.Add(new SqlParameter("@fire_dFechaEnvio", fichaRegistral.fire_dFechaEnvio));
                        cmd.Parameters.Add(new SqlParameter("@fire_vNumeroOficio", fichaRegistral.fire_vNumeroOficio));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@fihi_iFichaHistoricoId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;

                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        fichaRegistral.fihi_iFichaHistoricoId = Convert.ToInt64(lReturn.Value);                        
                        fichaRegistral.Error = false;
                    }
                }

            }
            catch (SqlException exec)
            {
                fichaRegistral.Error = true;
                fichaRegistral.Message = exec.Message.ToString();
            }
            return fichaRegistral;
        }

        public RE_FICHAREGISTRAL anular(RE_FICHAREGISTRAL fichaRegistral)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHAREGISTRAL_ANULAR_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@fire_iFichaRegistralId", fichaRegistral.fire_iFichaRegistralId));
                        cmd.Parameters.Add(new SqlParameter("@fire_sEstadoId", fichaRegistral.fire_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@fire_sUsuarioModificacion", fichaRegistral.fire_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@fire_vIPModificacion", fichaRegistral.fire_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", fichaRegistral.OficinaConsultar));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@fihi_iFichaHistoricoId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;

                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        fichaRegistral.fihi_iFichaHistoricoId = Convert.ToInt64(lReturn.Value);
                        fichaRegistral.Error = false;
                    }
                }

            }
            catch (SqlException exec)
            {
                fichaRegistral.Error = true;
                fichaRegistral.Message = exec.Message.ToString();
            }
            return fichaRegistral;
        }

        public DataTable Consultar(long intFichaRegistralId, long intActuacionDetalleId, string strNumeroFicha, string strFechaInicio, string strFechaFin,
                                   int intEstadoId, int ICurrentPag, int IPageSize, ref int ITotalRecords, ref int ITotalPages)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHA_REGISTRAL_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_FIRE_IFICHAREGISTRALID", intFichaRegistralId));
                        cmd.Parameters.Add(new SqlParameter("@P_FIRE_IACTUACIONDETALLEID", intActuacionDetalleId));
                        cmd.Parameters.Add(new SqlParameter("@P_FIRE_VNUMEROFICHA", strNumeroFicha));
                        cmd.Parameters.Add(new SqlParameter("@P_CFECHA_INICIAL", strFechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@P_CFECHA_FINAL", strFechaFin));
                        cmd.Parameters.Add(new SqlParameter("@P_FIRE_SESTADOID", intEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", ICurrentPag));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", IPageSize));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }
                        ITotalRecords = Convert.ToInt32(lReturn1.Value);
                        ITotalPages = Convert.ToInt32(lReturn2.Value);
                    }
                }
            }

            catch (SqlException exec)
            {
                dtResultado = null;
                throw exec;
            }

            return dtResultado;
        }

        public DataTable ConsultarTitular(int intOficinaConsularId, long intFichaRegistralId, string strNumeroFicha, int intEstadoId, string strFechaInicio, string strFechaFin,
                                          int intCorrelativoActuacion, string strNumeroGuia, string strDocumentoNumero,
                                          string strApPaterno, string strApMaterno, string strNombres, string strNumeroHoja, bool bSIO,
                                          int ICurrentPag, int IPageSize, ref int ITotalRecords, ref int ITotalPages)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHA_REGISTRAL_CONSULTAR_TITULAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_SOFICINACONSULARID", intOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@P_FIRE_IFICHAREGISTRALID", intFichaRegistralId));
                        cmd.Parameters.Add(new SqlParameter("@P_FIRE_VNUMEROFICHA", strNumeroFicha));
                        cmd.Parameters.Add(new SqlParameter("@P_FIRE_SESTADOID", intEstadoId));                        
                        cmd.Parameters.Add(new SqlParameter("@P_CFECHA_INICIAL", strFechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@P_CFECHA_FINAL", strFechaFin));
                        cmd.Parameters.Add(new SqlParameter("@P_ICORRELATIVOACTUACION", intCorrelativoActuacion));
                        cmd.Parameters.Add(new SqlParameter("@P_VNUMEROGUIA", strNumeroGuia));
                        cmd.Parameters.Add(new SqlParameter("@P_VDOCUMENTONUMERO", strDocumentoNumero));
                        cmd.Parameters.Add(new SqlParameter("@P_VAPELLIDOPATERNO", strApPaterno));
                        cmd.Parameters.Add(new SqlParameter("@P_VAPELLIDOMATERNO", strApMaterno));
                        cmd.Parameters.Add(new SqlParameter("@P_VNOMBRES", strNombres));
                        cmd.Parameters.Add(new SqlParameter("@P_VNUMEROHOJA", strNumeroHoja));
                        cmd.Parameters.Add(new SqlParameter("@P_BSIO", bSIO));
                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", ICurrentPag));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", IPageSize));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }
                        ITotalRecords = Convert.ToInt32(lReturn1.Value);
                        ITotalPages = Convert.ToInt32(lReturn2.Value);
                    }
                }
            }

            catch (SqlException exec)
            {
                dtResultado = null;
                throw exec;
            }

            return dtResultado;
        }


        public DataTable ObtenerDocumentosFichaRegistral(long intFichaRegistralId)
            
        {
            DataTable dtResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHA_REGISTRAL_CONSULTAR_DOCUMENTOS", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@FIRE_IFICHAREGISTRALID", intFichaRegistralId));
                        
                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }

            catch (SqlException exec)
            {
                dtResultado = null;
                throw exec;
            }

            return dtResultado;
        }

        public DataTable Reporte(long intFichaRegistralId)
            
        {
            DataTable dtResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHA_REGISTRAL_REPORTE_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@FIRE_IFICHAREGISTRALID", intFichaRegistralId));
                        
                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }

            catch (SqlException exec)
            {
                dtResultado = null;
                throw exec;
            }

            return dtResultado;
        }

        public DataTable ConsultarFichasPorEnviar(int intOficinaConsularId, long intFichaRegistralId, int intTarifarioId, string strNumeroFicha, int intEstadoId, 
                                                string strFechaInicio, string strFechaFin, int ICurrentPag, int IPageSize, ref int ITotalRecords, ref int ITotalPages)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHA_REGISTRAL_PORENVIAR_CONSULTAR_TITULAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_SOFICINACONSULARID", intOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@P_FIRE_IFICHAREGISTRALID", intFichaRegistralId));
                        cmd.Parameters.Add(new SqlParameter("@P_ACDE_STARIFARIOID", intTarifarioId));
                        cmd.Parameters.Add(new SqlParameter("@P_FIRE_VNUMEROFICHA", strNumeroFicha));
                        cmd.Parameters.Add(new SqlParameter("@P_FIRE_SESTADOID", intEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@P_CFECHA_INICIAL", strFechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@P_CFECHA_FINAL", strFechaFin));                        
                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", ICurrentPag));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", IPageSize));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }
                        ITotalRecords = Convert.ToInt32(lReturn1.Value);
                        ITotalPages = Convert.ToInt32(lReturn2.Value);
                    }
                }
            }

            catch (SqlException exec)
            {
                dtResultado = null;
                throw exec;
            }

            return dtResultado;
        }

        public RE_FICHAREGISTRAL ActualizarEnvioSIO(RE_FICHAREGISTRAL fichaRegistral)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHAREGISTRAL_ACTUALIZAR_ENVIO_SIO_MRE", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@fire_iFichaRegistralId", fichaRegistral.fire_iFichaRegistralId));
                        cmd.Parameters.Add(new SqlParameter("@fire_sUsuarioModificacion", fichaRegistral.fire_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@fire_vIPModificacion", fichaRegistral.fire_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@fire_dFechaEnvio", fichaRegistral.fire_dFechaEnvio));
                        cmd.Parameters.Add(new SqlParameter("@fire_vNumeroOficio", fichaRegistral.fire_vNumeroOficio));
                        cmd.Parameters.Add(new SqlParameter("@ofco_sOficinaConsularId", fichaRegistral.OficinaConsultar));
                        #endregion

                        #region Output
                        SqlParameter lReturn = cmd.Parameters.Add("@fihi_iFichaHistoricoId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;

                        #endregion

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        fichaRegistral.fihi_iFichaHistoricoId = Convert.ToInt64(lReturn.Value);
                        fichaRegistral.Error = false;
                    }
                }

            }
            catch (SqlException exec)
            {
                fichaRegistral.Error = true;
                fichaRegistral.Message = exec.Message.ToString();
            }
            return fichaRegistral;
        }


        public DataTable ConsultarTitularReporte(int intOficinaConsularId, long intFichaRegistralId, string strNumeroFicha, int intEstadoId, string strFechaInicio, string strFechaFin,
                                          int intCorrelativoActuacion, string strNumeroGuia, string strDocumentoNumero,
                                          string strApPaterno, string strApMaterno, string strNombres, string strNumeroHoja, bool bSio)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_FICHA_REGISTRAL_CONSULTAR_TITULAR_REPORTE_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_SOFICINACONSULARID", intOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@P_FIRE_IFICHAREGISTRALID", intFichaRegistralId));
                        cmd.Parameters.Add(new SqlParameter("@P_FIRE_VNUMEROFICHA", strNumeroFicha));
                        cmd.Parameters.Add(new SqlParameter("@P_FIRE_SESTADOID", intEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@P_CFECHA_INICIAL", strFechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@P_CFECHA_FINAL", strFechaFin));
                        cmd.Parameters.Add(new SqlParameter("@P_ICORRELATIVOACTUACION", intCorrelativoActuacion));
                        cmd.Parameters.Add(new SqlParameter("@P_VNUMEROGUIA", strNumeroGuia));
                        cmd.Parameters.Add(new SqlParameter("@P_VDOCUMENTONUMERO", strDocumentoNumero));
                        cmd.Parameters.Add(new SqlParameter("@P_VAPELLIDOPATERNO", strApPaterno));
                        cmd.Parameters.Add(new SqlParameter("@P_VAPELLIDOMATERNO", strApMaterno));
                        cmd.Parameters.Add(new SqlParameter("@P_VNOMBRES", strNombres));
                        cmd.Parameters.Add(new SqlParameter("@P_VNUMEROHOJA", strNumeroHoja));
                        cmd.Parameters.Add(new SqlParameter("@P_BSIO", bSio));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }

            catch (SqlException exec)
            {
                dtResultado = null;
                throw exec;
            }

            return dtResultado;
        }


        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }    
    }
}
