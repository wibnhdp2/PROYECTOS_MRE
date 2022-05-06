using System.Configuration;
using System.Data;
using SGAC.Accesorios;
using System;
using System.Data.SqlClient;
using DL.DAC;
using System.Collections.Generic;

namespace SGAC.Contabilidad.Reportes.DA
{
    public class ReporteConsultasDA
    {
        private string strConnectionName = string.Empty;

        public ReporteConsultasDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ReporteConsultasDA()
        {
            GC.Collect();
        }

        public DataSet ObtenerReporteRegistroGeneralEntradas(int intOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            string strHostName, int intUsuarioId, string strDireccionIP, int intOficinaConsularIdLogeo)
        {
            DataSet dsResult = null;

            SqlParameter[] prmParameter = new SqlParameter[7];

            prmParameter[0] = new SqlParameter("@sOficinaConsularId", SqlDbType.BigInt);
            prmParameter[0].Value = intOficinaConsularId;
            prmParameter[1] = new SqlParameter("@dFechaInicio", SqlDbType.DateTime);
            prmParameter[1].Value = dFechaInicio.ToString("yyyy-MM-dd 00:00:00");
            prmParameter[2] = new SqlParameter("@dFechaFin", SqlDbType.DateTime);
            prmParameter[2].Value = dFechaFin;
            prmParameter[3] = new SqlParameter("@vHostName", SqlDbType.VarChar);
            prmParameter[3].Value = strHostName;
            prmParameter[4] = new SqlParameter("@sUsuarioCreacion", SqlDbType.SmallInt);
            prmParameter[4].Value = intUsuarioId;
            prmParameter[5] = new SqlParameter("@vDireccionIP", SqlDbType.VarChar);
            prmParameter[5].Value = strDireccionIP;
            prmParameter[6] = new SqlParameter("@sOficinaConsularIdCreacion", SqlDbType.SmallInt);
            prmParameter[6].Value = intOficinaConsularIdLogeo;


            try
            {
                dsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_CONTABILIDAD.USP_CO_REPORTE_REGISTRO_GENERAL_ENTRADAS",
                                                    prmParameter);

              
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                prmParameter = null;
                dsResult = null;
            }
        }


        public DataSet ObtenerReporteTitulares(Int16 intOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            Int16 intEstadoCivilId=0,  Int16 intGeneroId=0, string strCodigoPostal="", Int16 intOcupacionId=0,
            Int16 intProfesionId=0, Int16 intGradoInstruccionId=0, Int16 intResidenciaTipoId=0, string strResidenciaUbigeo="",
            Int16 intNacionalidadId=0)            
        {
            DataSet dsResult = null;

            SqlParameter[] prmParameter = new SqlParameter[12];

            prmParameter[0] = new SqlParameter("@sOficinaConsularId", SqlDbType.SmallInt);
            prmParameter[0].Value = intOficinaConsularId;
            prmParameter[1] = new SqlParameter("@dFechaInicio", SqlDbType.DateTime);
            prmParameter[1].Value = dFechaInicio.ToString("yyyy-MM-dd 00:00:00");
            prmParameter[2] = new SqlParameter("@dFechaFin", SqlDbType.DateTime);
            prmParameter[2].Value = dFechaFin.ToString("yyyy-MM-dd 00:00:00");
            prmParameter[3] = new SqlParameter("@pers_sEstadoCivilId", SqlDbType.SmallInt);
            prmParameter[3].Value = intEstadoCivilId;
            prmParameter[4] = new SqlParameter("@pers_sGeneroId", SqlDbType.SmallInt);
            prmParameter[4].Value = intGeneroId;
            prmParameter[5] = new SqlParameter("@vCodigoPostal", SqlDbType.VarChar,20);
            prmParameter[5].Value = strCodigoPostal;
            prmParameter[6] = new SqlParameter("@pers_sOcupacionId", SqlDbType.SmallInt);
            prmParameter[6].Value = intOcupacionId;
            prmParameter[7] = new SqlParameter("@pers_sProfesionId", SqlDbType.SmallInt);
            prmParameter[7].Value = intProfesionId;
            prmParameter[8] = new SqlParameter("@pers_sGradoInstruccionId", SqlDbType.SmallInt);
            prmParameter[8].Value = intGradoInstruccionId;
            prmParameter[9] = new SqlParameter("@resi_sResidenciaTipoId", SqlDbType.SmallInt);
            prmParameter[9].Value = intResidenciaTipoId;
            prmParameter[10] = new SqlParameter("@resi_cResidenciaUbigeo", SqlDbType.VarChar, 20);
            prmParameter[10].Value = strResidenciaUbigeo;
            prmParameter[11] = new SqlParameter("@pers_sNacionalidadId", SqlDbType.SmallInt);
            prmParameter[11].Value = intNacionalidadId;


            try
            {
                dsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_CONTABILIDAD.USP_CO_REPORTE_TITULARES_MRE",
                                                    prmParameter);


                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                prmParameter = null;
                dsResult = null;
            }
        }


        public DataSet ObtenerReporteRegistroGeneralEntradasTimeOut(int intOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            string strHostName, int intUsuarioId, string strDireccionIP, int intOficinaConsularIdLogeo)
        {

            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REPORTE_REGISTRO_GENERAL_ENTRADAS", cnn))
                    {
                        //cmd.CommandTimeout = 30; //valor predeterminado
                        cmd.CommandTimeout = 600;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.SmallInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@dFechaInicio", SqlDbType.DateTime).Value = dFechaInicio;
                        cmd.Parameters.Add("@dFechaFin", SqlDbType.DateTime).Value = dFechaFin;
                        cmd.Parameters.Add("@vHostName", SqlDbType.VarChar).Value = strHostName;
                        cmd.Parameters.Add("@sUsuarioCreacion", SqlDbType.SmallInt).Value = intUsuarioId;
                        cmd.Parameters.Add("@vDireccionIP", SqlDbType.VarChar).Value = strDireccionIP;
                        cmd.Parameters.Add("@sOficinaConsularIdCreacion", SqlDbType.SmallInt).Value = intOficinaConsularIdLogeo;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ObtenerDTReporteRegistroGeneralEntradasTimeOut(int intOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            string strHostName, int intUsuarioId, string strDireccionIP, int intOficinaConsularIdLogeo)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REPORTE_REGISTRO_GENERAL_ENTRADAS", cnn))
                    {
                        //cmd.CommandTimeout = 30; //valor predeterminado
                        cmd.CommandTimeout = 600;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.SmallInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@dFechaInicio", SqlDbType.DateTime).Value = dFechaInicio;
                        cmd.Parameters.Add("@dFechaFin", SqlDbType.DateTime).Value = dFechaFin;
                        cmd.Parameters.Add("@vHostName", SqlDbType.VarChar).Value = strHostName;
                        cmd.Parameters.Add("@sUsuarioCreacion", SqlDbType.SmallInt).Value = intUsuarioId;
                        cmd.Parameters.Add("@vDireccionIP", SqlDbType.VarChar).Value = strDireccionIP;
                        cmd.Parameters.Add("@sOficinaConsularIdCreacion", SqlDbType.SmallInt).Value = intOficinaConsularIdLogeo;
                        cnn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader != null)
                            {
                                dt.Load(reader);
                            }
                            cmd.Parameters.Clear();
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<csReporteRGE> ObtenerReporteRegistroGeneralEntradasTimeOut_Reader(int intOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            string strHostName, int intUsuarioId, string strDireccionIP, int intOficinaConsularIdLogeo)
        {
            List<csReporteRGE> listRGE = new List<csReporteRGE>();
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REPORTE_REGISTRO_GENERAL_ENTRADAS", cnn))
                    {
                        //cmd.CommandTimeout = 30; //valor predeterminado
                        cmd.CommandTimeout = 600; // 10 minutos
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.SmallInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@dFechaInicio", SqlDbType.DateTime).Value = dFechaInicio;
                        cmd.Parameters.Add("@dFechaFin", SqlDbType.DateTime).Value = dFechaFin;
                        cmd.Parameters.Add("@vHostName", SqlDbType.VarChar).Value = strHostName;
                        cmd.Parameters.Add("@sUsuarioCreacion", SqlDbType.SmallInt).Value = intUsuarioId;
                        cmd.Parameters.Add("@vDireccionIP", SqlDbType.VarChar).Value = strDireccionIP;
                        cmd.Parameters.Add("@sOficinaConsularIdCreacion", SqlDbType.SmallInt).Value = intOficinaConsularIdLogeo;
                        cnn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            csReporteRGE objRGE;

                            while (reader.Read())
                            {
                                if (reader["Tipo"].ToString().Equals("D"))
                                {
                                    objRGE = new csReporteRGE();

                                    objRGE.Tipo = reader["Tipo"].ToString();
                                    objRGE.iNumero = Convert.ToInt64(reader["iNumero"]);
                                    objRGE.iNumeroOrden = Convert.ToInt64(reader["iNumeroOrden"]);
                                    objRGE.dFecha = Convert.ToDateTime(reader["dFecha"]);
                                    objRGE.vSolicitante = reader["vSolicitante"].ToString();
                                    objRGE.vApellidos = reader["vApellidos"].ToString();
                                    objRGE.vNombres = reader["vNombres"].ToString();
                                    objRGE.vAutoadhesivoCod = reader["vAutoadhesivoCod"].ToString();
                                    objRGE.vTarifaDesc = reader["vTarifaDesc"].ToString();
                                    objRGE.vTarifaNro = reader["vTarifaNro"].ToString();

                                    objRGE.FSolesConsular = Convert.ToDouble(reader["FSolesConsular"]);
                                    objRGE.FMonedaExtranjera = Convert.ToDouble(reader["FMonedaExtranjera"]);
                                    objRGE.FValorTCConsular = Convert.ToDouble(reader["FValorTCConsular"]);
                                    objRGE.vObservacion = reader["vObservacion"].ToString();
                                    objRGE.pago_sPagoTipoId = reader["pago_sPagoTipoId"].ToString();
                                    objRGE.iNumeroActuacion = reader["iNumeroActuacion"].ToString();
                                    objRGE.acde_ICorrelativoActuacion = reader["acde_ICorrelativoActuacion"].ToString();
                                    objRGE.iCantAutoadhesivo = reader["iCantAutoadhesivo"].ToString();
                                    objRGE.PagadoLimaME = Convert.ToDouble(reader["PagadoLimaME"]);
                                    objRGE.PagadoLimaSC = Convert.ToDouble(reader["PagadoLimaSC"]);
                                    objRGE.Moneda_S = reader["Moneda_S"].ToString();
                                    objRGE.TotalActEscPublicas = Convert.ToDouble(reader["TotalActEscPublicas"]);
                                    objRGE.PagoArubaSC = Convert.ToDouble(reader["PagoArubaSC"]);
                                    objRGE.PagadoOtrasIslasSC = Convert.ToDouble(reader["PagadoOtrasIslasSC"]);
                                    objRGE.Itinerante = reader["Itinerante"].ToString();

                                    listRGE.Add(objRGE);
                                }
                            }
                        }
                        return listRGE;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                listRGE = null;
            }   
        }

        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 09/08/2018
        // Objetivo: Consultar el RGE usando DataReader
        //------------------------------------------------------------------------

        public  List<csRGE>  ObtenerReporteRGE_Reader(int intOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            string strHostName, int intUsuarioId, string strDireccionIP, int intOficinaConsularIdLogeo)
        {
            List<csRGE> listRGE = new List<csRGE>();
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REPORTE_REGISTRO_GENERAL_ENTRADAS", cnn))
                    {
                        //cmd.CommandTimeout = 30; //valor predeterminado
                        cmd.CommandTimeout = 600;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.SmallInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@dFechaInicio", SqlDbType.DateTime).Value = dFechaInicio;
                        cmd.Parameters.Add("@dFechaFin", SqlDbType.DateTime).Value = dFechaFin;
                        cmd.Parameters.Add("@vHostName", SqlDbType.VarChar).Value = strHostName;
                        cmd.Parameters.Add("@sUsuarioCreacion", SqlDbType.SmallInt).Value = intUsuarioId;
                        cmd.Parameters.Add("@vDireccionIP", SqlDbType.VarChar).Value = strDireccionIP;
                        cmd.Parameters.Add("@sOficinaConsularIdCreacion", SqlDbType.SmallInt).Value = intOficinaConsularIdLogeo;
                        cnn.Open();
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            csRGE objRGE;
                            
                            while (reader.Read())
                            {
                                if (reader["Tipo"].ToString().Equals("D"))
                                {
                                    objRGE = new csRGE();
                                    objRGE.iCantAutoadhesivo = reader["iCantAutoadhesivo"].ToString();
                                    objRGE.iNumero = reader["iNumero"].ToString();
                                    objRGE.iNumeroOrden = reader["iNumeroOrden"].ToString();
                                    objRGE.dFecha = reader["dFecha"].ToString();
                                    objRGE.vSolicitante = reader["vSolicitante"].ToString();
                                    objRGE.vAutoadhesivoCod = reader["vAutoadhesivoCod"].ToString();
                                    objRGE.vTarifaDesc = reader["vTarifaDesc"].ToString();
                                    objRGE.vTarifaNro = reader["vTarifaNro"].ToString();
                                    objRGE.iNumeroActuacion = reader["iNumeroActuacion"].ToString();
                                    objRGE.FMonedaExtranjera = reader["FMonedaExtranjera"].ToString();
                                    objRGE.FSolesConsular = reader["FSolesConsular"].ToString();
                                    objRGE.FValorTCConsular = reader["FValorTCConsular"].ToString();
                                    objRGE.vObservacion = reader["vObservacion"].ToString();
                                    objRGE.Itinerante = reader["Itinerante"].ToString();
                                    objRGE.PagadoLimaME = reader["PagadoLimaME"].ToString();
                                    objRGE.PagadoLimaSC = reader["PagadoLimaSC"].ToString();

                                    listRGE.Add(objRGE);
                                }
                            }                            
                        }
                        return listRGE;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                listRGE = null;
            }            
        }


        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 05/09/2016
        // Objetivo: Consultar las actuaciones anuladas
        //------------------------------------------------------------------------

        public DataSet ObtenerReporteActuacionesAnuladas(int intOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
                    int intUsuarioId, string strDireccionIP, string strClaseFecha,Int16 intUsuarioElimina = 0)
        {
            DataSet dsResult = null;

            SqlParameter[] prmParameter = new SqlParameter[8];

            prmParameter[0] = new SqlParameter("@sOficinaConsularId", SqlDbType.BigInt);
            prmParameter[0].Value = intOficinaConsularId;
            prmParameter[1] = new SqlParameter("@dFechaInicio", SqlDbType.DateTime);
            prmParameter[1].Value = dFechaInicio.ToString("yyyy-MM-dd 00:00:00");
            prmParameter[2] = new SqlParameter("@dFechaFin", SqlDbType.DateTime);
            prmParameter[2].Value = dFechaFin;
            prmParameter[3] = new SqlParameter("@vHostName", SqlDbType.VarChar);
            prmParameter[3].Value = Util.ObtenerHostName(); 
            prmParameter[4] = new SqlParameter("@sUsuarioCreacion", SqlDbType.SmallInt);
            prmParameter[4].Value = intUsuarioId;
            prmParameter[5] = new SqlParameter("@vDireccionIP", SqlDbType.VarChar);
            prmParameter[5].Value = strDireccionIP;
            prmParameter[6] = new SqlParameter("@CCLASEFECHA", SqlDbType.Char);
            prmParameter[6].Value = strClaseFecha;
            prmParameter[7] = new SqlParameter("@SUSUARIOELIMINA", SqlDbType.SmallInt);
            prmParameter[7].Value = intUsuarioElimina;


            try
            {
                dsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_CONTABILIDAD.USP_CO_REPORTE_ACTUACIONES_ANULADAS_MRE",
                                                    prmParameter);


                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                prmParameter = null;
                dsResult = null;
            }
        }

        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 29/12/2016
        // Objetivo: Reporte de Guía de Despacho
        //------------------------------------------------------------------------
        public DataSet ObtenerReporteGuiaDespacho(Int16 intOficinaConsularId, string strNumeroGuia, string strAnioMes)
        {
            DataSet dsResult = null;

            SqlParameter[] prmParameter = new SqlParameter[3];

            prmParameter[0] = new SqlParameter("@P_SOFICINACONSULARID", SqlDbType.SmallInt);
            prmParameter[0].Value = intOficinaConsularId;
            prmParameter[1] = new SqlParameter("@P_VNUMEROGUIA", SqlDbType.VarChar);
            prmParameter[1].Value = strNumeroGuia;
            prmParameter[2] = new SqlParameter("@P_CANIOMES", SqlDbType.Char);
            prmParameter[2].Value = strAnioMes;       

            try
            {
                dsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REGISTRO.USP_RE_FICHA_REGISTRAL_REPORTE_GUIA_DESPACHO_MRE",
                                                    prmParameter);

                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                prmParameter = null;
                dsResult = null;
            }
        }


        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 09/01/2017
        // Objetivo: Reporte de Formato de Envio
        //------------------------------------------------------------------------
        public DataSet ObtenerReporteFormatoEnvio(Int16 intOficinaConsularId, string strNumeroGuia, string strAnioMes, 
                                                  Int16 intTarifarioId, Int16 intEstadoId)
        {
            DataSet dsResult = null;

            SqlParameter[] prmParameter = new SqlParameter[5];

            prmParameter[0] = new SqlParameter("@P_SOFICINACONSULARID", SqlDbType.SmallInt);
            prmParameter[0].Value = intOficinaConsularId;
            prmParameter[1] = new SqlParameter("@P_VNUMEROGUIA", SqlDbType.VarChar);
            prmParameter[1].Value = strNumeroGuia;
            prmParameter[2] = new SqlParameter("@P_CANIOMES", SqlDbType.Char);
            prmParameter[2].Value = strAnioMes;
            prmParameter[3] = new SqlParameter("@P_STARIFARIOID", SqlDbType.SmallInt);
            prmParameter[3].Value = intTarifarioId;
            prmParameter[4] = new SqlParameter("@P_SESTADOID", SqlDbType.SmallInt);
            prmParameter[4].Value = intEstadoId;


            try
            {
                dsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REGISTRO.USP_RE_FICHA_REGISTRAL_REPORTE_FORMATO_ENVIO_MRE",
                                                    prmParameter);

                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                prmParameter = null;
                dsResult = null;
            }
        }
        //------------------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 15/03/2017
        // Objetivo: Reporte de Formato de Envio Recuperados
        //------------------------------------------------------------------------
        public DataSet ObtenerReporteFormatoEnvioRecuparados(Int16 intOficinaConsularId, string strNumeroGuia, string strAnioMes, 
                                                  Int16 intTarifarioId, Int16 intEstadoId)
        {
            DataSet dsResult = null;

            SqlParameter[] prmParameter = new SqlParameter[5];

            prmParameter[0] = new SqlParameter("@P_SOFICINACONSULARID", SqlDbType.SmallInt);
            prmParameter[0].Value = intOficinaConsularId;
            prmParameter[1] = new SqlParameter("@P_VNUMEROGUIA", SqlDbType.VarChar);
            prmParameter[1].Value = strNumeroGuia;
            prmParameter[2] = new SqlParameter("@P_CANIOMES", SqlDbType.Char);
            prmParameter[2].Value = strAnioMes;
            prmParameter[3] = new SqlParameter("@P_STARIFARIOID", SqlDbType.SmallInt);
            prmParameter[3].Value = intTarifarioId;
            prmParameter[4] = new SqlParameter("@P_SESTADOID", SqlDbType.SmallInt);
            prmParameter[4].Value = intEstadoId;


            try
            {
                dsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REGISTRO.USP_RE_FICHA_REGISTRAL_REPORTE_FORMATO_ENVIO_RECUPERADO_MRE",
                                                    prmParameter);

                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                prmParameter = null;
                dsResult = null;
            }
        }
        

        //------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 21/11/2016
        // Objetivo: Reporte de Rendición de Cuentas por tarifa
        //------------------------------------------------------------------------
        public DataSet ObtenerReporteRendicionCuenta(long intFichaRegistralId, string strNumeroFicha, Int16 intEstadoId,
                                    string strAnioMes, string strNumeroGuia, Int16 intOficinaConsularId, Int16 OrdenadoPor)
        {
            DataSet dsResult = null;

            SqlParameter[] prmParameter = new SqlParameter[7];

            prmParameter[0] = new SqlParameter("@P_FIRE_IFICHAREGISTRALID", SqlDbType.BigInt);
            prmParameter[0].Value = intFichaRegistralId;
            prmParameter[1] = new SqlParameter("@P_FIRE_VNUMEROFICHA", SqlDbType.VarChar);
            prmParameter[1].Value = strNumeroFicha;
            prmParameter[2] = new SqlParameter("@P_FIRE_SESTADOID", SqlDbType.SmallInt);
            prmParameter[2].Value = intEstadoId;
            prmParameter[3] = new SqlParameter("@P_CANIOMES", SqlDbType.Char);
            prmParameter[3].Value = strAnioMes;
            prmParameter[4] = new SqlParameter("@P_VNUMEROGUIA", SqlDbType.VarChar);
            prmParameter[4].Value = strNumeroGuia;
            prmParameter[5] = new SqlParameter("@P_SOFICINACONSULARID", SqlDbType.SmallInt);
            prmParameter[5].Value = intOficinaConsularId;
            prmParameter[6] = new SqlParameter("@P_Ordenado", SqlDbType.SmallInt);
            prmParameter[6].Value = OrdenadoPor;


            try
            {
                dsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REGISTRO.USP_RE_FICHA_REGISTRAL_REPORTE_RENDICION_CUENTA_MRE",
                                                    prmParameter);

                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                prmParameter = null;
                dsResult = null;
            }
        }



        public DataSet ObtenerReporteSaldosConsulares(int intOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            string strHostName, int intUsuarioId, string strDireccionIP, int intOficinaConsularIdLogeo,int CuentaCorrienteId)
        {
            DataSet dsResult = null;
            SqlParameter[] prmParameter = new SqlParameter[8];

            prmParameter[0] = new SqlParameter("@sOficinaConsularId", SqlDbType.BigInt);
            prmParameter[0].Value = intOficinaConsularId;
            prmParameter[1] = new SqlParameter("@dFechaInicio", SqlDbType.DateTime);
            prmParameter[1].Value = dFechaInicio.ToString("yyyy-MM-dd 00:00:00");
            prmParameter[2] = new SqlParameter("@dFechaFin", SqlDbType.DateTime);
            prmParameter[2].Value = dFechaFin;
            prmParameter[3] = new SqlParameter("@vHostName", SqlDbType.VarChar);
            prmParameter[3].Value = strHostName;
            prmParameter[4] = new SqlParameter("@sUsuarioCreacion", SqlDbType.SmallInt);
            prmParameter[4].Value = intUsuarioId;
            prmParameter[5] = new SqlParameter("@vDireccionIP", SqlDbType.VarChar);
            prmParameter[5].Value = strDireccionIP;
            prmParameter[6] = new SqlParameter("@sOficinaConsularIdCreacion", SqlDbType.SmallInt);
            prmParameter[6].Value = intOficinaConsularIdLogeo;
            prmParameter[7] = new SqlParameter("@tran_sCuentaCorrienteId", SqlDbType.SmallInt);
            prmParameter[7].Value = CuentaCorrienteId;

            try
            {
                dsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_CONTABILIDAD.USP_CO_REPORTE_LIBRO_SALDOS_CONSULARES",
                                                    prmParameter);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                prmParameter = null;
                dsResult = null;
            }
        }

        public DataSet ObtenerReporteLibroAutoadhesivo(Int16 intOficinaConsularId, Int16 intPeriodo, Int16 intMes, ref Int16 intIDResultado, ref string strMensaje)
        {
            DataSet dsResult = null;
            SqlParameter[] prmParameter = new SqlParameter[5];

            prmParameter[0] = new SqlParameter("@P_OFCO_SOFICINACONSULARID", SqlDbType.SmallInt);
            prmParameter[0].Value = intOficinaConsularId;
            prmParameter[1] = new SqlParameter("@P_PERIODO", SqlDbType.SmallInt);
            prmParameter[1].Value = intPeriodo;
            prmParameter[2] = new SqlParameter("@P_MES", SqlDbType.SmallInt);
            prmParameter[2].Value = intMes;
            prmParameter[3] = new SqlParameter("@P_ID_RESULTADO", SqlDbType.SmallInt);
            prmParameter[3].Value = intIDResultado;
            prmParameter[4] = new SqlParameter("@P_MENSAJE", SqlDbType.VarChar);
            prmParameter[4].Value = strMensaje;


            try
            {
                dsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_CONTABILIDAD.USP_CO_LIBRO_AUTOADHESIVO",
                                                    prmParameter);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                prmParameter = null;
                dsResult = null;
            }
        }        

        public DataSet ObtenerReporteLibroCaja(int intOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            string strHostName, int intUsuarioId, string strDireccionIP, int intOficinaConsularIdLogeo)
        {
            DataSet dsResult = null;
            SqlParameter[] prmParameter = new SqlParameter[7];

            prmParameter[0] = new SqlParameter("@sOficinaConsularId", SqlDbType.BigInt);
            prmParameter[0].Value = intOficinaConsularId;
            prmParameter[1] = new SqlParameter("@dFechaInicio", SqlDbType.DateTime);
            prmParameter[1].Value = dFechaInicio.ToString("yyyy-MM-dd 00:00:00");
            prmParameter[2] = new SqlParameter("@dFechaFin", SqlDbType.DateTime);
            prmParameter[2].Value = dFechaFin;
            prmParameter[3] = new SqlParameter("@vHostName", SqlDbType.VarChar);
            prmParameter[3].Value = strHostName;
            prmParameter[4] = new SqlParameter("@sUsuarioCreacion", SqlDbType.SmallInt);
            prmParameter[4].Value = intUsuarioId;
            prmParameter[5] = new SqlParameter("@vDireccionIP", SqlDbType.VarChar);
            prmParameter[5].Value = strDireccionIP;
            prmParameter[6] = new SqlParameter("@sOficinaConsularIdCreacion", SqlDbType.SmallInt);
            prmParameter[6].Value = intOficinaConsularIdLogeo;

            try
            {
                dsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_CONTABILIDAD.USP_CO_REPORTE_LIBRO_CAJA",
                                                    prmParameter);

                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                prmParameter = null;
                dsResult = null;
            }
        }

        public DataSet ObtenerReporteDocumentoUnico(int intOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            string strHostName, int intUsuarioId, string strDireccionIP, int intOficinaConsularIdLogeo)
        {
            DataSet dsResult = null;
            SqlParameter[] prmParameter = new SqlParameter[7];

            prmParameter[0] = new SqlParameter("@sOficinaConsularId", SqlDbType.SmallInt);
            prmParameter[0].Value = intOficinaConsularId;
            prmParameter[1] = new SqlParameter("@dFechaInicio", SqlDbType.DateTime);
            prmParameter[1].Value = dFechaInicio;
            prmParameter[2] = new SqlParameter("@dFechaFin", SqlDbType.DateTime);
            prmParameter[2].Value =dFechaFin;
            prmParameter[3] = new SqlParameter("@vHostName", SqlDbType.VarChar, 50);
            prmParameter[3].Value = strHostName;
            prmParameter[4] = new SqlParameter("@sUsuarioCreacion", SqlDbType.SmallInt);
            prmParameter[4].Value = intUsuarioId;
            prmParameter[5] = new SqlParameter("@vDireccionIP", SqlDbType.VarChar, 50);
            prmParameter[5].Value = strDireccionIP;
            prmParameter[6] = new SqlParameter("@sOficinaConsularIdCreacion", SqlDbType.SmallInt);
            prmParameter[6].Value = intOficinaConsularIdLogeo;

            //82,'01-01-2014','01-01-2015','xxx',3,'xxx',82

            try
            {
                dsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_CONTABILIDAD.USP_CO_REPORTE_DOCUMENTO_UNICO",
                                                    prmParameter);
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                prmParameter = null;
                dsResult = null;
            }
        }

        // Otros
        public DataTable ObtenerRemesasConsulares(int intOficinaConsularOrigenId, int intOficinaConsularDestinoId,
            int intTipoid, int intEstadoId,
            DateTime datFechaInicio, DateTime datFechaFin, int iUsuario, string vIp)
        {
            DataSet dsResult = null;
            DataTable dtResult = null;

            SqlParameter[] prmParameter = new SqlParameter[9];
            prmParameter[0] = new SqlParameter("@reme_sOficinaConsularOrigenId", SqlDbType.SmallInt);
            prmParameter[0].Value = intOficinaConsularOrigenId;
            prmParameter[1] = new SqlParameter("@reme_sOficinaConsularDestinoId", SqlDbType.SmallInt);
            prmParameter[1].Value = intOficinaConsularDestinoId;
            prmParameter[2] = new SqlParameter("@reme_sTipoId", SqlDbType.SmallInt);
            prmParameter[2].Value = intTipoid;
            prmParameter[3] = new SqlParameter("@reme_sEstadoId", SqlDbType.SmallInt);
            prmParameter[3].Value = intEstadoId;
            prmParameter[4] = new SqlParameter("@reme_dFechaInicio", SqlDbType.DateTime);
            prmParameter[4].Value = datFechaInicio.ToString("yyyy-MM-dd 00:00:00");
            prmParameter[5] = new SqlParameter("@reme_dFechaFin", SqlDbType.DateTime);
            prmParameter[5].Value = datFechaFin;

            prmParameter[6] = new SqlParameter("@audi_sUsuarioCreacion", SqlDbType.Int);
            prmParameter[6].Value = iUsuario;

            prmParameter[7] = new SqlParameter("@audi_vIPCreacion", SqlDbType.VarChar, 20);
            prmParameter[7].Value = vIp;

            prmParameter[8] = new SqlParameter("@vHostName", SqlDbType.VarChar, 20);
            prmParameter[8].Value = Util.ObtenerHostName();

            try
            {
                dsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_CONTABILIDAD.USP_CO_REPORTE_REMESA_CONSULAR",
                                                    prmParameter);
                dtResult = dsResult.Tables[0];
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                prmParameter = null;
                dtResult = null;
                dsResult = null;
            }
        }

        public DataTable ObtenerEstadosBancarios(int intOficinaConsularid,
            int intBancoId, string strNumCuentaCorriente,
            DateTime datFechaInicio, DateTime datFechaFin, Int16 PeriodoAnio, Int16 PeriodoMes, string strTipoBusqueda, Int16 intCodCuentaCorriente)
        {
            DataSet dsResult = null;
            DataTable dtResult = null;

            SqlParameter[] prmParameter = new SqlParameter[9];

            prmParameter[0] = new SqlParameter("@sOficinaConsularId", SqlDbType.SmallInt);
            prmParameter[0].Value = intOficinaConsularid;
            prmParameter[1] = new SqlParameter("@sBancoId", SqlDbType.SmallInt);
            prmParameter[1].Value = intBancoId;
            prmParameter[2] = new SqlParameter("@vNumero", SqlDbType.VarChar);
            prmParameter[2].Value = strNumCuentaCorriente;
            prmParameter[3] = new SqlParameter("@dFechaInicio", SqlDbType.DateTime);
            prmParameter[3].Value = datFechaInicio.ToString("yyyy-MM-dd 00:00:00");
            prmParameter[4] = new SqlParameter("@dFechaFin", SqlDbType.DateTime);
            prmParameter[4].Value = datFechaFin;

            prmParameter[5] = new SqlParameter("@tran_sPeriodoAnio", SqlDbType.SmallInt);
            prmParameter[5].Value = PeriodoAnio;
            prmParameter[6] = new SqlParameter("@tran_sPeriodoMes", SqlDbType.SmallInt);
            prmParameter[6].Value = PeriodoMes;
            prmParameter[7] = new SqlParameter("@cBusqueda", SqlDbType.Char);
            prmParameter[7].Value = strTipoBusqueda;
            prmParameter[8] = new SqlParameter("@cuco_sCuentaCorrienteId", SqlDbType.SmallInt);
            prmParameter[8].Value = intCodCuentaCorriente;
            
            

            try
            {
                dsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_CONTABILIDAD.USP_CO_REPORTE_ESTADO_CUENTA",
                                                    prmParameter);
                dtResult = dsResult.Tables[0];
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                prmParameter = null;
                dtResult = null;
                dsResult = null;
            }
        }

        public DataSet ObtenerTransaccionResumen(short sCuentaCorrienteId, DateTime datFechaInicio, DateTime datFechaFin,
                                                    Int16 intPeriodoAnio, Int16 intPeriodoMes, string cBusqueda)
        {
            DataSet dsResult = null;
            //DataTable dtResult = null;

            SqlParameter[] prmParameter = new SqlParameter[6];

            prmParameter[0] = new SqlParameter("@tran_sCuentaCorrienteId", SqlDbType.SmallInt);
            prmParameter[0].Value = sCuentaCorrienteId;
            prmParameter[1] = new SqlParameter("@dFechaInicio", SqlDbType.DateTime);
            prmParameter[1].Value = datFechaInicio;
            prmParameter[2] = new SqlParameter("@dFechaFin", SqlDbType.DateTime);
            prmParameter[2].Value = datFechaFin;
            prmParameter[3] = new SqlParameter("@tran_sPeriodoAnio", SqlDbType.SmallInt);
            prmParameter[3].Value = intPeriodoAnio;
            prmParameter[4] = new SqlParameter("@tran_sPeriodoMes", SqlDbType.SmallInt);
            prmParameter[4].Value = intPeriodoMes;
            prmParameter[5] = new SqlParameter("@cBusqueda", SqlDbType.Char);
            prmParameter[5].Value = cBusqueda;
            
            try
            {
                dsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_CONTABILIDAD.USP_CO_REPORTE_TRANSACCION_RESUMEN",
                                                    prmParameter);
                //dtResult = dsResult.Tables[0];
                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                prmParameter = null;
                dsResult = null;
            }
        }


        public DataSet ObtenerReporteConciliacion(short intOficinaConsularId, short intBancoId, short intCuentaCorrienteId, DateTime datFechaInicio, DateTime datFechaFin)
        {
            DataSet objResultado = new DataSet();

            try
            {
                using (SqlConnection cn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REPORTE_CONCILIACIONBANCARIA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", intOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@sBancoId", intBancoId));
                        cmd.Parameters.Add(new SqlParameter("@sCuentaCorrienteId", intCuentaCorrienteId));
                        cmd.Parameters.Add(new SqlParameter("@dFechaInicio", datFechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@dFechaFin", datFechaFin));

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            objResultado = dsObjeto;
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

        //------------------------------------------------------------------------
        // Autor: Jonatan silva cachay
        // Fecha: 13/06/2017
        // Objetivo: Reporte Tramites Incompletos
        //------------------------------------------------------------------------
        public DataSet ObtenerReporteTramitesIncompletos(string strAnioMes,  Int16 intOficinaConsularId)
        {
            DataSet dsResult = null;

            SqlParameter[] prmParameter = new SqlParameter[2];

            prmParameter[0] = new SqlParameter("@P_CANIOMES", SqlDbType.Char);
            prmParameter[0].Value = strAnioMes;
            prmParameter[1] = new SqlParameter("@P_SOFICINACONSULARID", SqlDbType.SmallInt);
            prmParameter[1].Value = intOficinaConsularId;


            try
            {
                dsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REGISTRO.USP_RE_FICHA_REGISTRAL_REPORTE_TRAMITES_INCOMPLETOS",
                                                    prmParameter);

                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                prmParameter = null;
                dsResult = null;
            }
        }
        public DataSet ObtenerReporteConciliacionReniec(string strAnioMes)
        {
            DataSet dsResult = null;

            SqlParameter[] prmParameter = new SqlParameter[2];

            prmParameter[0] = new SqlParameter("@P_CANIOMES", SqlDbType.Char);
            prmParameter[0].Value = strAnioMes;

            try
            {
                dsResult = SqlHelper.ExecuteDataset(strConnectionName,
                                                    CommandType.StoredProcedure,
                                                    "PN_REGISTRO.USP_RE_FICHA_REGISTRAL_REPORTE_CONCILIACION_RENIEC",
                                                    prmParameter);

                return dsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                prmParameter = null;
                dsResult = null;
            }
        }
        public DataSet ObtenerReporteSaldosConsularesTimeuot(int intOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin,
            string strHostName, int intUsuarioId, string strDireccionIP, int intOficinaConsularIdLogeo, int CuentaCorrienteId)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REPORTE_LIBRO_SALDOS_CONSULARES", cnn))
                    {
                        //cmd.CommandTimeout = 30; //valor predeterminado
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.SmallInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@dFechaInicio", SqlDbType.DateTime).Value = dFechaInicio;
                        cmd.Parameters.Add("@dFechaFin", SqlDbType.DateTime).Value = dFechaFin;
                        cmd.Parameters.Add("@vHostName", SqlDbType.VarChar).Value = strHostName;
                        cmd.Parameters.Add("@sUsuarioCreacion", SqlDbType.SmallInt).Value = intUsuarioId;
                        cmd.Parameters.Add("@vDireccionIP", SqlDbType.VarChar).Value = strDireccionIP;
                        cmd.Parameters.Add("@sOficinaConsularIdCreacion", SqlDbType.VarChar).Value = intOficinaConsularIdLogeo;
                        cmd.Parameters.Add("@tran_sCuentaCorrienteId", SqlDbType.SmallInt).Value = CuentaCorrienteId;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataSet ObtenerReporteListadoRegCivil(DateTime dFechaInicio, DateTime dFechaFin, int intOficinaConsularId)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_REPORTE_ACTUACIONES_REGISTRO_CIVIL", cnn))
                    {
                        //cmd.CommandTimeout = 30; //valor predeterminado
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.SmallInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@dFechaInicio", SqlDbType.DateTime).Value = dFechaInicio;
                        cmd.Parameters.Add("@dFechaFin", SqlDbType.DateTime).Value = dFechaFin;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
