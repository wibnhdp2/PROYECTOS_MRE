using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActoNotarialConsultaDA
    {
        private string StrConnectionName = string.Empty;

        public ActoNotarialConsultaDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataTable ObtenerUsoSubTipo(long sTipoActoNotarialId)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIAL_OBTENER_USO_SUBTIPO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@acno_sTipoActoNotarialId", sTipoActoNotarialId));
                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }

                        dt = ds_Objeto.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public DataTable ListarActuacionDetalle(long lngActuacionId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_OBTENER_NOTARIAL", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@acde_iActuacionId", lngActuacionId));
                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }

                        dt = ds_Objeto.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public DataTable ReportePoderFueraRegistro(int iActoNotarialId, int sOficinaConsularId, Int64? ianpa_iReferenciaId = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_FORMATO_PODER_FUERA_DE_REGISTRO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@acno_sOficinaConsularId", sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@acno_iActoNotarialId", iActoNotarialId));
                        cmd.Parameters.Add(new SqlParameter("@anpa_iReferenciaParticipanteId", ianpa_iReferenciaId));
                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }
                        dt = ds_Objeto.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public DataTable ReporteSupervivencia(int iActoNotarialId, int sOficinaConsularId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_FORMATO_SUPERVIVENCIA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@acno_sOficinaConsularId",sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@acno_iActoNotarialId", iActoNotarialId));
                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }
                        dt = ds_Objeto.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public DataTable ReporteAutorizacionViaje(int iActoNotarialId, int sOficinaConsularId, Int64? ianpa_iReferenciaId = null)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_FORMATO_AUTORIZACION_VIAJE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@acno_sOficinaConsularId", sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@acno_iActoNotarialId", iActoNotarialId));
                        cmd.Parameters.Add(new SqlParameter("@anpa_iReferenciaParticipanteId", ianpa_iReferenciaId));

                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }
                        dt = ds_Objeto.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
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
                                                string strUbigeoDestino="")
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIAL_PROTOCOLAR_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (intOficinaConsularId != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sOficinaConsularId", intOficinaConsularId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sOficinaConsularId", DBNull.Value));
                        }

                        if (strNumProyecto.Length != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@vNumeroEscrituraPublica", strNumProyecto));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@vNumeroEscrituraPublica", DBNull.Value));
                        }

                        if (intEstadoId != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sEstadoId", intEstadoId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sEstadoId", DBNull.Value));
                        }

                        if (dateFechaInicio != DateTime.MinValue)
                        {
                            cmd.Parameters.Add(new SqlParameter("@dFechaInicio", dateFechaInicio));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@dFechaInicio", DBNull.Value));
                        }

                        if (dateFechaFinal != DateTime.MinValue)
                        {
                            cmd.Parameters.Add(new SqlParameter("@dFechaFin", dateFechaFinal));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@dFechaFin", DBNull.Value));
                        }

                        if (intFuncionarioAutorizadorId != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_IFuncionarioAutorizadorId", intFuncionarioAutorizadorId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_IFuncionarioAutorizadorId", DBNull.Value));
                        }

                        if (intTipoActoNotarialId != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sTipoActoNotarialId", intTipoActoNotarialId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sTipoActoNotarialId", DBNull.Value));
                        }

                        if (intSubTipoActoNotarialId != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sSubTipoActoNotarialId", intSubTipoActoNotarialId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sSubTipoActoNotarialId", DBNull.Value));
                        }


                        if (strApellidoPaterno.Length != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vApellidoPaterno", strApellidoPaterno));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vApellidoPaterno", DBNull.Value));
                        }

                        if (strApellidoMaterno.Length != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vApellidoMaterno", strApellidoMaterno));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vApellidoMaterno", DBNull.Value));
                        }

                        if (strNombres.Length != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vNombres", strNombres));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vNombres", DBNull.Value));
                        }

                        if (strNumeroDocumento.Length != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@peid_vDocumentoNumero", strNumeroDocumento));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@peid_vDocumentoNumero", DBNull.Value));
                        }

                        if (sTipoDocumento!=0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@peid_sDocumentoTipoId", sTipoDocumento));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@peid_sDocumentoTipoId", DBNull.Value));
                        }

                        if (sTipoParticipante != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@anpa_sTipoParticipanteId", sTipoParticipante));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@anpa_sTipoParticipanteId", DBNull.Value));
                        }

                        if (sAnio != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@sAnio", sAnio));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@sAnio", DBNull.Value));
                        }



                        if (FechIniPago != DateTime.MinValue)
                        {
                            cmd.Parameters.Add(new SqlParameter("@dFechaInicioPago", FechIniPago));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@dFechaInicioPago", DBNull.Value));
                        }

                        if (FechFinPago != DateTime.MinValue)
                        {
                            cmd.Parameters.Add(new SqlParameter("@dFechaFinPago", FechFinPago));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@dFechaFinPago", DBNull.Value));
                        }


                        //
                        if (ICorrelativoActuacion != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@ICorrelativoActuacion", ICorrelativoActuacion));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@ICorrelativoActuacion", DBNull.Value));
                        }

                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", intCurrentPage));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", intPageSize));
                        cmd.Parameters.Add(new SqlParameter("@cUbigeoDestino", strUbigeoDestino));


                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }

                        IntTotalCount = Convert.ToInt32(lReturn1.Value);
                        IntTotalPages = Convert.ToInt32(lReturn2.Value);
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        //------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 20/09/2016
        // Objetivo: Consultar las autorizaciones de viaje de menor 
        //------------------------------------------------------------

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
                                                string strUbigeoDestino="")
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIAL_AUT_VIAJE_MENOR_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (intOficinaConsularId != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sOficinaConsularId", intOficinaConsularId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sOficinaConsularId", DBNull.Value));
                        }

                        if (strNumProyecto.Length != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@vNumeroEscrituraPublica", strNumProyecto));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@vNumeroEscrituraPublica", DBNull.Value));
                        }

                        if (intEstadoId != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sEstadoId", intEstadoId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sEstadoId", DBNull.Value));
                        }

                        if (dateFechaInicio != DateTime.MinValue)
                        {
                            cmd.Parameters.Add(new SqlParameter("@dFechaInicio", dateFechaInicio));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@dFechaInicio", DBNull.Value));
                        }

                        if (dateFechaFinal != DateTime.MinValue)
                        {
                            cmd.Parameters.Add(new SqlParameter("@dFechaFin", dateFechaFinal));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@dFechaFin", DBNull.Value));
                        }

                        if (intFuncionarioAutorizadorId != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_IFuncionarioAutorizadorId", intFuncionarioAutorizadorId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_IFuncionarioAutorizadorId", DBNull.Value));
                        }

                        if (intTipoActoNotarialId != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sTipoActoNotarialId", intTipoActoNotarialId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sTipoActoNotarialId", DBNull.Value));
                        }

                        if (intSubTipoActoNotarialId != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sSubTipoActoNotarialId", intSubTipoActoNotarialId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sSubTipoActoNotarialId", DBNull.Value));
                        }

                        if (sTipoParticipante != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@anpa_sTipoParticipanteId", sTipoParticipante));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@anpa_sTipoParticipanteId", DBNull.Value));
                        }

                        if (strApellidoPaterno.Length != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vApellidoPaterno", strApellidoPaterno));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vApellidoPaterno", DBNull.Value));
                        }

                        if (strApellidoMaterno.Length != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vApellidoMaterno", strApellidoMaterno));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vApellidoMaterno", DBNull.Value));
                        }

                        if (strNombres.Length != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vNombres", strNombres));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vNombres", DBNull.Value));
                        }

                        if (sTipoDocumento != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@peid_sDocumentoTipoId", sTipoDocumento));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@peid_sDocumentoTipoId", DBNull.Value));
                        }

                        if (strNumeroDocumento.Length != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@peid_vDocumentoNumero", strNumeroDocumento));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@peid_vDocumentoNumero", DBNull.Value));
                        }


                        if (dateFechaInicioPago != DateTime.MinValue)
                        {
                            cmd.Parameters.Add(new SqlParameter("@dFechaInicioPago", dateFechaInicioPago));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@dFechaInicioPago", DBNull.Value));
                        }

                        if (dateFechaFinalPago != DateTime.MinValue)
                        {
                            cmd.Parameters.Add(new SqlParameter("@dFechaFinPago", dateFechaFinalPago));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@dFechaFinPago", DBNull.Value));
                        }
                      
                        //

                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", intCurrentPage));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", intPageSize));
                        cmd.Parameters.Add(new SqlParameter("@cUbigeoDestino", strUbigeoDestino));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }

                        IntTotalCount = Convert.ToInt32(lReturn1.Value);
                        IntTotalPages = Convert.ToInt32(lReturn2.Value);
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
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
                                               short sAnio
            )
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_ACTONOTARIAL_PROTOCOLAR_REPORTE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        if (intOficinaConsularId != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sOficinaConsularId", intOficinaConsularId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sOficinaConsularId", DBNull.Value));
                        }

                        if (strNumProyecto.Length != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@vNumeroEscrituraPublica", strNumProyecto));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@vNumeroEscrituraPublica", DBNull.Value));
                        }

                        if (intEstadoId != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sEstadoId", intEstadoId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sEstadoId", DBNull.Value));
                        }

                        if (dateFechaInicio != DateTime.MinValue)
                        {
                            cmd.Parameters.Add(new SqlParameter("@dFechaInicio", dateFechaInicio));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@dFechaInicio", DBNull.Value));
                        }


                        if (dateFechaFinal != DateTime.MinValue)
                        {
                            cmd.Parameters.Add(new SqlParameter("@dFechaFin", dateFechaFinal));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@dFechaFin", DBNull.Value));
                        }

                        if (intFuncionarioAutorizadorId != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_IFuncionarioAutorizadorId", intFuncionarioAutorizadorId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_IFuncionarioAutorizadorId", DBNull.Value));
                        }

                        if (intTipoActoNotarialId != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sTipoActoNotarialId", intTipoActoNotarialId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sTipoActoNotarialId", DBNull.Value));
                        }
                        if (intSubTipoActoNotarialId != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sSubTipoActoNotarialId", intSubTipoActoNotarialId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@acno_sSubTipoActoNotarialId", DBNull.Value));
                        }
                        if (strApellidoPaterno.Length != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vApellidoPaterno", strApellidoPaterno));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vApellidoPaterno", DBNull.Value));
                        }

                        if (strApellidoMaterno.Length != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vApellidoMaterno", strApellidoMaterno));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vApellidoMaterno", DBNull.Value));
                        }

                        if (strNombres.Length != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vNombres", strNombres));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@pers_vNombres", DBNull.Value));
                        }

                        if (strNumeroDocumento.Length != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@peid_vDocumentoNumero", strNumeroDocumento));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@peid_vDocumentoNumero", DBNull.Value));
                        }

                        if (sTipoDocumento != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@peid_sDocumentoTipoId", sTipoDocumento));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@peid_sDocumentoTipoId", DBNull.Value));
                        }

                        if (sTipoParticipante != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@anpa_sTipoParticipanteId", sTipoParticipante));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@anpa_sTipoParticipanteId", DBNull.Value));
                        }

                        if (sAnio != 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@sAnio", sAnio));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@sAnio", DBNull.Value));
                        }

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException exec)
            {
                objResultado = null;
                throw exec;
            }

            return objResultado;
        }

        public DataTable ObtenerCuerpo(long lonvCuerpo)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_FORMATO_MINUTA_TESTIMONIO_ESCRITURA", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@ancu_iActoNotarialId", lonvCuerpo));
                        #endregion

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }

                        dt = ds_Objeto.Tables[0];
                    }
                }
            }
            catch (SqlException exec)
            {
                dt = null;
                throw exec;
            }

            return dt;
        }

        public DataTable ObtenerFormatoParte(long lonActoNotarialId, int intOficinaConsularId)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RP_FORMATO_PARTE", cnx))
                    {
                        cnx.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        #region Creando Parametros
                        cmd.Parameters.Add(new SqlParameter("@acno_iActoNotarialId", lonActoNotarialId));
                        cmd.Parameters.Add(new SqlParameter("@acno_sOficinaConsularId", intOficinaConsularId));
                        #endregion

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }

                        dt = ds_Objeto.Tables[0];
                    }
                }
            }
            catch (SqlException exec)
            {
                dt = null;
                throw exec;
            }

            return dt;
        }

        public DataTable ActonotarialObtenerDatosPrincipales(long lngActoNotarialId, int IntOficinaConsular)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALPROTOCOLAR_OBTENER_PARA_CUERPO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@acno_iActoNotarialId", lngActoNotarialId));
                        cmd.Parameters.Add(new SqlParameter("@acno_sOficinaConsularId", IntOficinaConsular));
                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }

                        dt = ds_Objeto.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public DataTable ActonotarialObtenerParticipantes(long lngActoNotarialId)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALPARTICIPANTE_OBTENER_PARA_CUERPO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@anpa_iActoNotarialId", lngActoNotarialId));
                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }

                        dt = ds_Objeto.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        /// <summary>
        /// Obtener Lista de formatos por Acto Notarial desde el Registro
        /// </summary>
        /// <param name="intActuacionId">Identificador de la Actuación</param>
        /// <param name="intActuacionDetalleId">Identificador de la Actuación Detalle</param>
        /// <param name="intTipoFormatoId">Identificador del Tipo de Formato</param>
        /// <returns></returns>
        public DataTable ObtenerActoNotarialDetalle(Int64 intActuacionId, Int64? intActuacionDetalleId, Int16 intTipoFormatoId, 
            bool bolSoloNoVinculados = false, string strActuacionDetalleIds = "")
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALDETALLE_CONSULTAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ande_iActuacionId", intActuacionId));
                        if (intActuacionDetalleId != 0)
                            cmd.Parameters.Add(new SqlParameter("@ande_iActuacionDetalleId", intActuacionDetalleId));
                        if (intTipoFormatoId != 0)
                            cmd.Parameters.Add(new SqlParameter("@ande_sTipoFormatoId", intTipoFormatoId));

                        cmd.Parameters.Add(new SqlParameter("@bNoVinculados", bolSoloNoVinculados));
                        cmd.Parameters.Add(new SqlParameter("@vActuacionesDetalleIds", strActuacionDetalleIds));

                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }

                        dt = ds_Objeto.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        /// <summary>
        /// Obtener Lista de formatos por tipo en Consulta de Actos Notariales
        /// </summary>
        /// <param name="intActuacionId">Identificador de la Actuación</param>
        /// <param name="intTipoFormatoId">Identificador del Tipo de Formato</param>
        /// <returns></returns>
        public DataTable ObtenerActoNotarialDetalle(Int64 intActuacionId, Int16 intTipoFormatoId = 0)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIALDETALLE_SEGUIMIENTO", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@acno_iActuacionId", intActuacionId));
                        if (intTipoFormatoId != 0)
                            cmd.Parameters.Add(new SqlParameter("@ande_sTipoFormatoId", intTipoFormatoId));
                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }

                        dt = ds_Objeto.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        
    }
}
