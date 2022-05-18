using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;

namespace SGAC.Registro.Actuacion.DA
{
    public class TramiteConsultaDA 
    {
        private string StrConnectionName = string.Empty;

        public TramiteConsultaDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~TramiteConsultaDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        //----------------------------------------------------------
        // Modificado por: Miguel Márquez Beltrán
        // Fecha: 11/11/2019
        // Motivo: Método sin uso.
        //----------------------------------------------------------
        #region Comentada
        //public DataTable ActuacionConsultaConsular(long intPersonaId,
        //                                           int intSeccionId,
        //                                           DateTime FechaInicio,
        //                                           DateTime FechaFinal,
        //                                           int IntCurrentPage,
        //                                           int IntPageSize,
        //                                           ref int IntTotalCount,
        //                                           ref int IntTotalPages)
        //{
        //    DataSet DsResult = new DataSet();
        //    DataTable DtResult = new DataTable();

        //    try
        //    {
        //        SqlParameter[] prmParameter = new SqlParameter[8];

        //        prmParameter[0] = new SqlParameter("@sPersonaId", SqlDbType.Int);
        //        prmParameter[0].Value = intPersonaId;

        //        prmParameter[1] = new SqlParameter("@sSeccionId", SqlDbType.Int);
        //        prmParameter[1].Value = intSeccionId;

        //        prmParameter[2] = new SqlParameter("@FechaInicio", SqlDbType.VarChar, 20);
        //        prmParameter[2].Value = FechaInicio;

        //        prmParameter[3] = new SqlParameter("@FechaFinal", SqlDbType.VarChar, 100);
        //        prmParameter[3].Value = FechaFinal;

        //        prmParameter[4] = new SqlParameter("@ICurrentPage", SqlDbType.VarChar, 100);
        //        prmParameter[4].Value = IntCurrentPage;

        //        prmParameter[5] = new SqlParameter("@IPageSize", SqlDbType.Int);
        //        prmParameter[5].Value = IntPageSize;

        //        prmParameter[6] = new SqlParameter("@ITotalRecords", SqlDbType.Int);
        //        prmParameter[6].Value = ParameterDirection.Output;

        //        prmParameter[7] = new SqlParameter("@ITotalPages", SqlDbType.Int);
        //        prmParameter[7].Direction = ParameterDirection.Output;

        //        DsResult = SqlHelper.ExecuteDataset(StrConnectionName,
        //                                            CommandType.StoredProcedure,
        //                                            "PN_REGISTRO_RE_ACTUACION_CONSULTA",
        //                                            prmParameter);

        //        DtResult = DsResult.Tables[0];

        //        IntTotalCount = Convert.ToInt32(((SqlParameter)prmParameter[6]).Value);
        //        IntTotalPages = Convert.ToInt32(((SqlParameter)prmParameter[7]).Value);

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
        #endregion
        // ***********************************
        // MODIFICADO : 19-03-2015 - A COMMAND
        public DataTable ActuacionConsultaJudicial(long intPersonaId,
                                            DateTime FechaInicio,
                                            DateTime FechaFinal,
                                            int IntCurrentPage,
                                            int IntPageSize,
                                            ref int IntTotalCount,
                                            ref int IntTotalPages)
        {
            DataTable DtResult = new DataTable();
            try
            {

                using (SqlConnection cnx = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO_USP_RE_ACTOJUDICIAL_CONSULTAR", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.Add(new SqlParameter("@iPersonaId", intPersonaId));
                        cmd.Parameters.Add(new SqlParameter("@dFechaInicio", FechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@dFechaFin", FechaFinal));
                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", IntCurrentPage));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", IntPageSize));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@ITotalRecords", SqlDbType.SmallInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@ITotalPages", SqlDbType.SmallInt);
                        lReturn2.Direction = ParameterDirection.Output;
                        
                        cmd.Connection.Open();

                        DataSet ds_Objeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(ds_Objeto);
                        }

                        DtResult = ds_Objeto.Tables[0];

                        IntTotalCount = Convert.ToInt32(lReturn1.Value);
                        IntTotalPages = Convert.ToInt32(lReturn2.Value);

                        return DtResult;
                    }
                }       
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DtResult = null;
            }
        }

        public DataTable ActuacionConsultaNotarial(long intPersonaId,
                                                   DateTime FechaInicio,
                                                   DateTime FechaFinal,
                                                   int IntCurrentPage,
                                                   int IntPageSize,
                                                   ref int IntTotalCount,
                                                   ref int IntTotalPages)
        {
            DataTable objResultado = new DataTable();

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTONOTARIAL_CONSULTAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 600;

                        cmd.Parameters.Add(new SqlParameter("@iPersonaId", intPersonaId));
                        cmd.Parameters.Add(new SqlParameter("@dFechaInicio", FechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@dFechaFin", FechaFinal));

                        cmd.Parameters.Add(new SqlParameter("@ICurrentPage", IntCurrentPage));
                        cmd.Parameters.Add(new SqlParameter("@IPageSize", IntPageSize));

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


    }
}
