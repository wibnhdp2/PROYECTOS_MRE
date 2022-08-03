using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Seguridad.Logica.BussinessEntity;

namespace Seguridad.Logica.BussinessADO
{
    class csUsuarioRolADO
    {
        csConexionADO MiConexion = new csConexionADO();

        public csTablaBE Consultar(string strUsuarioRolID, string strUsuarioID, string strGrupoID, string strRolConfiguracionID, string strOficinaConsularID, string strEstado, int intPageSize, int intPageNumber, string strcontar)
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
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_USUARIOROL_CONSULTAR_MRE";
            cmd.Parameters.Add(new SqlParameter("@P_USRO_SUSUARIOROLID", strUsuarioRolID));
            cmd.Parameters.Add(new SqlParameter("@P_USRO_SUSUARIOID", strUsuarioID));
            cmd.Parameters.Add(new SqlParameter("@P_USRO_SGRUPOID", strGrupoID));
            cmd.Parameters.Add(new SqlParameter("@P_USRO_SROLCONFIGURACIONID", strRolConfiguracionID));
            cmd.Parameters.Add(new SqlParameter("@P_USRO_SOFICINACONSULARID", strOficinaConsularID));
            cmd.Parameters.Add(new SqlParameter("@P_USRO_CESTADO", strEstado));
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

        public string Adicionar(csUsuarioRolBE BE, SqlTransaction transaction, SqlConnection cnx)
        {
            SqlCommand cmd = new SqlCommand();

            //Se configura el comando: Store Procedure
            cmd.Connection = cnx;
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_USUARIOROL_ADICIONAR_MRE";
            cmd.CommandTimeout = 0;

            string sError = "N";
            try
            {
                cmd.Parameters.Add(new SqlParameter("@P_USRO_SUSUARIOID", BE.UsuarioId));
                cmd.Parameters.Add(new SqlParameter("@P_USRO_SGRUPOID", BE.GrupoId));
                cmd.Parameters.Add(new SqlParameter("@P_USRO_SROLCONFIGURACIONID", BE.RolConfiguracionId));
                cmd.Parameters.Add(new SqlParameter("@P_USRO_SOFICINACONSULARID", BE.OficinaConsularId));
                cmd.Parameters.Add(new SqlParameter("@P_USRO_SACCESO", BE.Acceso));
                cmd.Parameters.Add(new SqlParameter("@P_USRO_SUSUARIOCREACION", BE.UsuarioCreacion));
                cmd.Parameters.Add(new SqlParameter("@P_USRO_VIPCREACION", BE.IPCreacion));
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

        public string Modificar(csUsuarioRolBE BE, SqlTransaction transaction, SqlConnection cnx)
        {
            SqlCommand cmd = new SqlCommand();

            //Se configura el comando: Store Procedure
            cmd.Connection = cnx;
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_USUARIOROL_MODIFICAR_MRE";
            cmd.CommandTimeout = 0;

            string sError = "N";
            try
            {
                cmd.Parameters.Add(new SqlParameter("@P_USRO_SUSUARIOROLID", BE.UsuarioRolId));
                cmd.Parameters.Add(new SqlParameter("@P_USRO_SUSUARIOID", BE.UsuarioId));
                cmd.Parameters.Add(new SqlParameter("@P_USRO_SGRUPOID", BE.GrupoId));
                cmd.Parameters.Add(new SqlParameter("@P_USRO_SROLCONFIGURACIONID", BE.RolConfiguracionId));
                cmd.Parameters.Add(new SqlParameter("@P_USRO_SOFICINACONSULARID", BE.OficinaConsularId));
                cmd.Parameters.Add(new SqlParameter("@P_USRO_SACCESO", BE.Acceso));
                cmd.Parameters.Add(new SqlParameter("@P_USRO_CESTADO", BE.Estado));
                cmd.Parameters.Add(new SqlParameter("@P_USRO_SUSUARIOMODIFICACION", BE.UsuarioModificacion));
                cmd.Parameters.Add(new SqlParameter("@P_USRO_VIPMODIFICACION", BE.IPModificacion));
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

        public string Anular(string strUsuarioRolID, string strUsuarioModificacion, string strIPModificacion, SqlTransaction transaction, SqlConnection cnx)
        {
            SqlCommand cmd = new SqlCommand();

            //Se configura el comando: Store Procedure
            cnx.Open();
            cmd.Connection = cnx;
            cmd.Transaction = transaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_USUARIOROL_ANULAR_MRE";
            cmd.CommandTimeout = 0;

            string sError = "N";
            try
            {
                cmd.Parameters.Add(new SqlParameter("@P_USRO_SUSUARIOROLID", strUsuarioRolID));
                cmd.Parameters.Add(new SqlParameter("@P_USRO_SUSUARIOMODIFICACION", strUsuarioModificacion));
                cmd.Parameters.Add(new SqlParameter("@P_USRO_VIPMODIFICACION", strIPModificacion));
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
