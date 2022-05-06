using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using System.Collections.Generic;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActuacionAdjuntoConsultaDA
    {
        private string StrConnectionName = string.Empty;

        public ActuacionAdjuntoConsultaDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ActuacionAdjuntoConsultaDA()
        {
            GC.Collect();
        }

        //public DataTable ActuacionAdjuntosObtener(long LonActuacionId, string StrCurrentPage, int IntPageSize, ref int IntTotalCount, ref int IntTotalPages)
        //{
        //    DataSet DsResult = new DataSet();
        //    DataTable DtResult = new DataTable();

        //    try
        //    {
        //        SqlParameter[] prmParameter = new SqlParameter[5];

        //        prmParameter[0] = new SqlParameter("@iActuacionDetalleId", SqlDbType.BigInt);
        //        prmParameter[0].Value = LonActuacionId;

        //        prmParameter[1] = new SqlParameter("@ICurrentPage", SqlDbType.Int);
        //        prmParameter[1].Value = StrCurrentPage;

        //        prmParameter[2] = new SqlParameter("@IPageSize", SqlDbType.Int);
        //        prmParameter[2].Value = IntPageSize;

        //        prmParameter[3] = new SqlParameter("@ITotalRecords", SqlDbType.Int);
        //        prmParameter[3].Direction = ParameterDirection.Output;

        //        prmParameter[4] = new SqlParameter("@ITotalPages", SqlDbType.Int);
        //        prmParameter[4].Direction = ParameterDirection.Output;

        //        DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
        //                                            CommandType.StoredProcedure,
        //                                            "PN_REGISTRO.USP_RE_ACTUACIONADJUNTO_OBTENER",
        //                                            prmParameter);

        //        DtResult = DsResult.Tables[0];

        //        IntTotalCount = Convert.ToInt32(((SqlParameter)prmParameter[3]).Value);
        //        IntTotalPages = Convert.ToInt32(((SqlParameter)prmParameter[4]).Value);

        //        return DtResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        DtResult = null;
        //        DsResult = null;
        //    }
        //}
        //------------------------------------------------------------------
        //Fecha: 19/12/2019
        //Autor: Miguel Márquez Beltrán
        //Motivo: Reemplazar el SQLHelper por SqlConnection y SqlCommand.
        //------------------------------------------------------------------

        public DataTable ActuacionAdjuntosObtener(long LonActuacionId, string StrCurrentPage, int IntPageSize, ref int IntTotalCount, ref int IntTotalPages)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONADJUNTO_OBTENER", cn))
                    {
                        
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@iActuacionDetalleId", SqlDbType.BigInt).Value = LonActuacionId;
                        cmd.Parameters.Add("@ICurrentPage", SqlDbType.Int).Value = StrCurrentPage;
                        cmd.Parameters.Add("@IPageSize", SqlDbType.Int).Value = IntPageSize;

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;
                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();

                        DsResult = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(DsResult);
                            DtResult = DsResult.Tables[0];
                        }
                        IntTotalCount = Convert.ToInt32(lReturn1.Value);
                        IntTotalPages = Convert.ToInt32(lReturn2.Value);
                    }
                }
                               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return DtResult;
        }

        //public DataTable ActuacionAdjuntosObtenerAdjuntos(long LonActuacionId, string strIdAdjunto, string StrCurrentPage, int IntPageSize, ref int IntTotalCount, ref int IntTotalPages)
        //{
        //    DataSet DsResult = new DataSet();
        //    DataTable DtResult = new DataTable();

        //    try
        //    {
        //        SqlParameter[] prmParameter = new SqlParameter[5];

        //        prmParameter[0] = new SqlParameter("@iActuacionDetalleId", SqlDbType.BigInt);
        //        prmParameter[0].Value = LonActuacionId;

        //        prmParameter[1] = new SqlParameter("@iIdAdjunto", SqlDbType.SmallInt);
        //        prmParameter[1].Value = strIdAdjunto;

        //        prmParameter[2] = new SqlParameter("@ICurrentPage", SqlDbType.Int);
        //        prmParameter[2].Value = StrCurrentPage;

        //        prmParameter[3] = new SqlParameter("@IPageSize", SqlDbType.Int);
        //        prmParameter[3].Value = IntPageSize;

        //        prmParameter[4] = new SqlParameter("@ITotalRecords", SqlDbType.Int);
        //        prmParameter[4].Direction = ParameterDirection.Output;

        //        prmParameter[5] = new SqlParameter("@ITotalPages", SqlDbType.Int);
        //        prmParameter[5].Direction = ParameterDirection.Output;

        //        DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
        //                                            CommandType.StoredProcedure,
        //                                            "PN_REGISTRO.USP_RE_ACTUACIONADJUNTO_OBTENER_ADJUNTO",
        //                                            prmParameter);

        //        DtResult = DsResult.Tables[0];

        //        IntTotalCount = Convert.ToInt32(((SqlParameter)prmParameter[4]).Value);
        //        IntTotalPages = Convert.ToInt32(((SqlParameter)prmParameter[5]).Value);

        //        return DtResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        DtResult = null;
        //        DsResult = null;
        //    }
        //}

        //------------------------------------------------------------------
        //Fecha: 19/12/2019
        //Autor: Miguel Márquez Beltrán
        //Motivo: Reemplazar el SQLHelper por SqlConnection y SqlCommand.
        //------------------------------------------------------------------

        public DataTable ActuacionAdjuntosObtenerAdjuntos(long LonActuacionId, string strIdAdjunto, string StrCurrentPage, int IntPageSize, ref int IntTotalCount, ref int IntTotalPages)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONADJUNTO_OBTENER_ADJUNTO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@iActuacionDetalleId", SqlDbType.BigInt).Value = LonActuacionId;
                        cmd.Parameters.Add("@iIdAdjunto", SqlDbType.SmallInt).Value = strIdAdjunto;
                        cmd.Parameters.Add("@ICurrentPage", SqlDbType.Int).Value = StrCurrentPage;
                        cmd.Parameters.Add("@IPageSize", SqlDbType.Int).Value = IntPageSize;

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;
                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();

                        DsResult = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(DsResult);
                            DtResult = DsResult.Tables[0];
                        }
                        IntTotalCount = Convert.ToInt32(lReturn1.Value);
                        IntTotalPages = Convert.ToInt32(lReturn2.Value);
                    }
                }
                            
            }
            catch (Exception ex)
            {
                throw new Exception("Se ha producido un error en el procedimiento: USP_RE_ACTUACIONADJUNTO_OBTENER_ADJUNTO", ex);
            }
            return DtResult;
        }

        //public DataTable ActuacionAdjuntosObtenerFoto(long LonActuacionId, string StrCurrentPage, int IntPageSize, ref int IntTotalCount, ref int IntTotalPages)
        //{
        //    DataSet DsResult = new DataSet();
        //    DataTable DtResult = new DataTable();

        //    try
        //    {
        //        SqlParameter[] prmParameter = new SqlParameter[5];

        //        prmParameter[0] = new SqlParameter("@iActuacionDetalleId", SqlDbType.BigInt);
        //        prmParameter[0].Value = LonActuacionId;

        //        prmParameter[1] = new SqlParameter("@ICurrentPage", SqlDbType.Int);
        //        prmParameter[1].Value = StrCurrentPage;

        //        prmParameter[2] = new SqlParameter("@IPageSize", SqlDbType.Int);
        //        prmParameter[2].Value = IntPageSize;

        //        prmParameter[3] = new SqlParameter("@ITotalRecords", SqlDbType.Int);
        //        prmParameter[3].Direction = ParameterDirection.Output;

        //        prmParameter[4] = new SqlParameter("@ITotalPages", SqlDbType.Int);
        //        prmParameter[4].Direction = ParameterDirection.Output;

        //        DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
        //                                            CommandType.StoredProcedure,
        //                                            "PN_REGISTRO.USP_RE_ACTUACIONADJUNTO_OBTENER_FOTO",
        //                                            prmParameter);

        //        DtResult = DsResult.Tables[0];

        //        IntTotalCount = Convert.ToInt32(((SqlParameter)prmParameter[3]).Value is DBNull ? 0 : Convert.ToInt32(((SqlParameter)prmParameter[3]).Value));
        //        IntTotalPages = Convert.ToInt32(((SqlParameter)prmParameter[4]).Value is DBNull ? 0 : Convert.ToInt32(((SqlParameter)prmParameter[4]).Value));

        //        return DtResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        DtResult = null;
        //        DsResult = null;
        //    }
        //}


        //------------------------------------------------------------------
        //Fecha: 19/12/2019
        //Autor: Miguel Márquez Beltrán
        //Motivo: Reemplazar el SQLHelper por SqlConnection y SqlCommand.
        //------------------------------------------------------------------

        public DataTable ActuacionAdjuntosObtenerFoto(long LonActuacionId, string StrCurrentPage, int IntPageSize, ref int IntTotalCount, ref int IntTotalPages)
        {
            DataSet DsResult = new DataSet();
            DataTable DtResult = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONADJUNTO_OBTENER_FOTO", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@iActuacionDetalleId", SqlDbType.BigInt).Value = LonActuacionId;
                        cmd.Parameters.Add("@ICurrentPage", SqlDbType.Int).Value = StrCurrentPage;
                        cmd.Parameters.Add("@IPageSize", SqlDbType.Int).Value = IntPageSize;

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;
                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();

                        DsResult = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(DsResult);
                            DtResult = DsResult.Tables[0];
                        }
                        IntTotalCount = Convert.ToInt32(lReturn1.Value);
                        IntTotalPages = Convert.ToInt32(lReturn2.Value);
                    }
                }
                              
            }
            catch (Exception ex)
            {
                throw new Exception("Se ha producido un error en el procedimiento: USP_RE_ACTUACIONADJUNTO_OBTENER_FOTO", ex);
            }


            return DtResult;
        }

        public List<BE.MRE.RE_ACTUACIONADJUNTO> Actuaciondetalle_Obtener_Digitalizados(Int64 iActuacionDetalleId) {

            try
            {
                using (SqlConnection cnn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REGISTRO].[USP_RE_ACTUACIONADJUNTO_OBTENER_DIGITALIZADO]", cnn))
                    {
                        cnn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@iActuacionDetalleId", SqlDbType.BigInt).Value = iActuacionDetalleId;

                        List<BE.MRE.RE_ACTUACIONADJUNTO> lActuacionAdjunto = new List<BE.MRE.RE_ACTUACIONADJUNTO>();
                        BE.MRE.RE_ACTUACIONADJUNTO oRE_ActuacionAdjunto = null;

                        using (SqlDataReader sdr = cmd.ExecuteReader()) {

                            if (sdr.HasRows)
                            {
                                while (sdr.Read()) {
                                    oRE_ActuacionAdjunto = new BE.MRE.RE_ACTUACIONADJUNTO();

                                    if (!sdr.IsDBNull(0))
                                        oRE_ActuacionAdjunto.acad_iActuacionAdjuntoId = sdr.GetInt64(0);
                                    if (!sdr.IsDBNull(1))
                                        oRE_ActuacionAdjunto.acad_iActuacionDetalleId = sdr.GetInt64(1);
                                    if (!sdr.IsDBNull(2))
                                        oRE_ActuacionAdjunto.acad_sAdjuntoTipoId = sdr.GetInt16(2);
                                    if (!sdr.IsDBNull(3))
                                        oRE_ActuacionAdjunto.acad_dFechaCreacion = sdr.GetDateTime(3);
                                    if (!sdr.IsDBNull(4))
                                        oRE_ActuacionAdjunto.usuario = sdr.GetString(4);
                                    if (!sdr.IsDBNull(5))
                                        oRE_ActuacionAdjunto.vAdjuntoTipo = sdr.GetString(5);
                                    if (!sdr.IsDBNull(6))
                                        oRE_ActuacionAdjunto.vNombreArchivo = sdr.GetString(6);
                                    if (!sdr.IsDBNull(7))
                                        oRE_ActuacionAdjunto.vDescripcion = sdr.GetString(7);
                                    lActuacionAdjunto.Add(oRE_ActuacionAdjunto);
                  
                                }
                                return lActuacionAdjunto;
                            }
                            else {
                                return null;
                            }
                        
                        }

                    }
                }
            }
            catch (Exception ex) {

                return null;
            }
            
        
        }

        }
}
