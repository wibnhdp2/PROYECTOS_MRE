using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Seguridad.Logica.BussinessEntity;

namespace Seguridad.Logica.BussinessADO
{
    class csFormularioADO
    {
        csConexionADO MiConexion = new csConexionADO();

        public csTablaBE Consultar(string strFormularioID, string strAplicacionID, string strNombre, string strVisible, string strEstado, int intPageSize, int intPageNumber, string strcontar)
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
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_FORMULARIO_CONSULTAR_MRE";
            cmd.Parameters.Add(new SqlParameter("@P_FORM_SFORMULARIOID", strFormularioID));
            cmd.Parameters.Add(new SqlParameter("@P_FORM_SAPLICACIONID", strAplicacionID));
            cmd.Parameters.Add(new SqlParameter("@P_FORM_VNOMBRE", strNombre));
            cmd.Parameters.Add(new SqlParameter("@P_FORM_BVISIBLE", strVisible));
            cmd.Parameters.Add(new SqlParameter("@P_FORM_CESTADO", strEstado));
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
        public DataTable ConsultarMenu(string strAplicacionID, string strRolOpcion, string strRutaPagina, string strVisible)
        {
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
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_FORMULARIO_CONSULTAR_MENU_MRE";
            cmd.Parameters.Add(new SqlParameter("@P_SAPLICACIONID", strAplicacionID));
            cmd.Parameters.Add(new SqlParameter("@P_VROLOPCION", strRolOpcion));
            cmd.Parameters.Add(new SqlParameter("@P_VRUTAPAGINA", strRutaPagina));
            cmd.Parameters.Add(new SqlParameter("@P_SVISIBLE", strVisible));
            
            da.SelectCommand = cmd;
            try
            {
                da.Fill(dt);
            }
            catch (SqlException ex)
            { throw ex; }

            finally
            { cnx.Close(); }

            return dt;
        }
        public string Adicionar(csFormularioBE BE)
        {
            String sCadena = MiConexion.GetCadenaConexion();
            SqlConnection cnx = new SqlConnection(sCadena);
            SqlCommand cmd = new SqlCommand();


            //Se configura el comando: Store Procedure
            cnx.Open();
            cmd.Connection = cnx;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_FORMULARIO_ADICIONAR_MRE";
            cmd.CommandTimeout = 0;

            string sError = "N";
            try
            {
                cmd.Parameters.Add(new SqlParameter("@P_FORM_SAPLICACIONID", BE.AplicacionId));
                cmd.Parameters.Add(new SqlParameter("@P_FORM_VNOMBRE", BE.Nombre));
                cmd.Parameters.Add(new SqlParameter("@P_FORM_SREFERENCIAID", BE.ReferenciaId));
                cmd.Parameters.Add(new SqlParameter("@P_FORM_VRUTA", BE.Ruta));
                cmd.Parameters.Add(new SqlParameter("@P_FORM_SORDEN ", BE.Orden));
                cmd.Parameters.Add(new SqlParameter("@P_FORM_BVISIBLE", BE.Visible));
                cmd.Parameters.Add(new SqlParameter("@P_FORM_SUSUARIOCREACION", BE.UsuarioCreacion));
                cmd.Parameters.Add(new SqlParameter("@P_FORM_VIPCREACION", BE.IPCreacion));

                cmd.Parameters.Add("@P_CERROR", SqlDbType.Char, 1).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                sError = cmd.Parameters["@P_CERROR"].Value.ToString();
            }
            catch (SqlException ex)
            {
                return "S." + ex.Message;
            }
            finally
            { cnx.Close(); }
            return sError;
        }

        public string Modificar(csFormularioBE BE)
        {
            String sCadena = MiConexion.GetCadenaConexion();
            SqlConnection cnx = new SqlConnection(sCadena);
            SqlCommand cmd = new SqlCommand();


            //Se configura el comando: Store Procedure
            cnx.Open();
            cmd.Connection = cnx;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_FORMULARIO_MODIFICAR_MRE";
            cmd.CommandTimeout = 0;

            string sError = "N";
            try
            {
                cmd.Parameters.Add(new SqlParameter("@P_FORM_SFORMULARIOID", BE.FormularioId));
                cmd.Parameters.Add(new SqlParameter("@P_FORM_SAPLICACIONID", BE.AplicacionId));
                cmd.Parameters.Add(new SqlParameter("@P_FORM_SCOMPONENTEID", BE.ComponenteId));
                cmd.Parameters.Add(new SqlParameter("@P_FORM_VNOMBRE", BE.Nombre));
                cmd.Parameters.Add(new SqlParameter("@P_FORM_SREFERENCIAID", BE.ReferenciaId));
                cmd.Parameters.Add(new SqlParameter("@P_FORM_VRUTA", BE.Ruta));
                cmd.Parameters.Add(new SqlParameter("@P_FORM_VIMAGEN", BE.Imagen));
                cmd.Parameters.Add(new SqlParameter("@P_FORM_SORDEN ", BE.Orden));
                cmd.Parameters.Add(new SqlParameter("@P_FORM_BVISIBLE", BE.Visible));
                cmd.Parameters.Add(new SqlParameter("@P_FORM_CESTADO", BE.Estado));
                cmd.Parameters.Add(new SqlParameter("@P_FORM_SUSUARIOMODIFICACION", BE.UsuarioModificacion));
                cmd.Parameters.Add(new SqlParameter("@P_FORM_VIPMODIFICACION", BE.IPModificacion));

                cmd.Parameters.Add("@P_CERROR", SqlDbType.Char, 1).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                sError = cmd.Parameters["@P_CERROR"].Value.ToString();
            }
            catch (SqlException ex)
            {
                return "S." + ex.Message;
            }
            finally
            { cnx.Close(); }
            return sError;
        }

        public string Anular(string strFormularioID, string strUsuarioModificacion, string strIPModificacion)
        {
            String sCadena = MiConexion.GetCadenaConexion();
            SqlConnection cnx = new SqlConnection(sCadena);
            SqlCommand cmd = new SqlCommand();


            //Se configura el comando: Store Procedure
            cnx.Open();
            cmd.Connection = cnx;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_FORMULARIO_ANULAR_MRE";
            cmd.CommandTimeout = 0;

            string sError = "N";
            try
            {
                cmd.Parameters.Add(new SqlParameter("@P_FORM_SFORMULARIOID", strFormularioID));
                cmd.Parameters.Add(new SqlParameter("@P_FORM_SUSUARIOMODIFICACION", strUsuarioModificacion));
                cmd.Parameters.Add(new SqlParameter("@P_FORM_VIPMODIFICACION", strIPModificacion));

                cmd.Parameters.Add("@P_CERROR", SqlDbType.Char, 1).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                sError = cmd.Parameters["@P_CERROR"].Value.ToString();
            }
            catch (SqlException ex)
            {
                return "S." + ex.Message;
            }
            finally
            { cnx.Close(); }
            return sError;
        }


    }
}
