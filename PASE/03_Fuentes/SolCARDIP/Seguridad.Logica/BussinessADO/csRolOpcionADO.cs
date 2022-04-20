using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Seguridad.Logica.BussinessEntity;

namespace Seguridad.Logica.BussinessADO
{
    class csRolOpcionADO
    {
        csConexionADO MiConexion = new csConexionADO();

        public csTablaBE Consultar(string strRolOpcionID, string strFormularioID, string strEstado, int intPageSize, int intPageNumber, string strcontar)
        {
            int intPageCount = 0;
            DataTable dt = new DataTable();
            String sCadena = MiConexion.GetCadenaConexion();
            //Se inicia la Conexion
            SqlConnection cnx = new SqlConnection(sCadena);
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();

            //Se configura el comando: store procedure
            cnx.Open();
            cmd.Connection = cnx;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_ROLOPCION_CONSULTAR_MRE";
            cmd.Parameters.Add(new SqlParameter("@P_ROOP_SROLOPCIONID", strRolOpcionID));
            cmd.Parameters.Add(new SqlParameter("@P_ROOP_SFORMULARIOID", strFormularioID));
            cmd.Parameters.Add(new SqlParameter("@P_ROOP_CESTADO", strEstado));
            cmd.Parameters.Add(new SqlParameter("@P_IPAGESIZE", intPageSize));
            cmd.Parameters.Add(new SqlParameter("@P_IPAGENUMBER", intPageNumber));
            cmd.Parameters.Add(new SqlParameter("@P_CCONTAR", strcontar));

            cmd.Parameters.Add("@P_IPAGECOUNT", SqlDbType.Int, 0).Direction = ParameterDirection.Output;

            da.SelectCommand = cmd;
            try
            {
                da.Fill(dt);
                intPageCount = int.Parse(cmd.Parameters["@P_IPAGECOUNT"].Value.ToString());
            }
            catch (SqlException ex)
            { throw ex; }

            finally
            { cnx.Close(); }

            csTablaBE dtRegistros = new csTablaBE();
            dtRegistros.dtRegistros = dt;
            dtRegistros.CantidadPaginas = intPageCount;
            return dtRegistros;
        }

        public string Adicionar(csRolOpcionBE BE, SqlTransaction transaction, SqlConnection cnx)
        {
            SqlCommand cmd = new SqlCommand();

            //Se configura el comando: Store Procedure
            cmd.Connection = cnx;
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_ROLOPCION_ADICIONAR_MRE";
            cmd.CommandTimeout = 0;

            string strError = "N";
            try
            {
                cmd.Parameters.Add(new SqlParameter("@P_ROOP_SFORMULARIOID", BE.FormularioId));
                cmd.Parameters.Add(new SqlParameter("@P_ROOP_VACCIONES", BE.Acciones));
                cmd.Parameters.Add(new SqlParameter("@P_ROOP_SUSUARIOCREACION", BE.UsuarioCreacion));
                cmd.Parameters.Add(new SqlParameter("@P_ROOP_VIPCREACION", BE.IPCreacion));

                cmd.Parameters.Add("@P_CERROR", SqlDbType.Char, 1).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@P_ROOP_SROLOPCIONID", SqlDbType.SmallInt).Direction = ParameterDirection.Output;                
                cmd.ExecuteNonQuery();

                strError = cmd.Parameters["@P_CERROR"].Value.ToString();
                string strRolOpcionID = cmd.Parameters["@P_ROOP_SROLOPCIONID"].Value.ToString();
                strError = strError + strRolOpcionID;
            }
            catch (SqlException ex)
            {
                return "S." + ex.Message;
            }
            return strError;
        }

        public string Modificar(csRolOpcionBE BE, SqlTransaction transaction, SqlConnection cnx)
        {
            SqlCommand cmd = new SqlCommand();


            //Se configura el comando: Store Procedure
            cmd.Connection = cnx;
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_ROLOPCION_MODIFICAR_MRE";
            cmd.CommandTimeout = 0;

            string strError = "N";
            try
            {
                cmd.Parameters.Add(new SqlParameter("@P_ROOP_SROLOPCIONID", BE.RolOpcionId));
                cmd.Parameters.Add(new SqlParameter("@P_ROOP_SFORMULARIOID", BE.FormularioId));
                cmd.Parameters.Add(new SqlParameter("@P_ROOP_VACCIONES", BE.Acciones));
                cmd.Parameters.Add(new SqlParameter("@P_ROOP_CESTADO", BE.Estado));
                cmd.Parameters.Add(new SqlParameter("@P_ROOP_SUSUARIOMODIFICACION", BE.UsuarioModificacion));
                cmd.Parameters.Add(new SqlParameter("@P_ROOP_VIPMODIFICACION", BE.IPModificacion));

                cmd.Parameters.Add("@P_CERROR", SqlDbType.Char, 1).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                strError = cmd.Parameters["@P_CERROR"].Value.ToString();
            }
            catch (SqlException ex)
            {
                return "S." + ex.Message;
            }
            return strError;
        }

        public string Anular(string strRolOpcionID, string strUsuarioModificacion, string strIPModificacion, SqlTransaction transaction, SqlConnection cnx)
        {
            SqlCommand cmd = new SqlCommand();

            //Se configura el comando: Store Procedure
            cmd.Connection = cnx;
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_ROLOPCION_ANULAR_MRE";
            cmd.CommandTimeout = 0;

            string sError = "N";
            try
            {
                cmd.Parameters.Add(new SqlParameter("@P_ROOP_SROLOPCIONID", strRolOpcionID));
                cmd.Parameters.Add(new SqlParameter("@P_ROOP_SUSUARIOMODIFICACION", strUsuarioModificacion));
                cmd.Parameters.Add(new SqlParameter("@P_ROOP_VIPMODIFICACION", strIPModificacion));

                cmd.Parameters.Add("@P_CERROR", SqlDbType.Char, 1).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                sError = cmd.Parameters["@P_CERROR"].Value.ToString();
            }
            catch (SqlException ex)
            {
                return "S." + ex.Message;
            }
            return sError;
        }

    }
}
