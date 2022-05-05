using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Seguridad.Logica.BussinessEntity;

namespace Seguridad.Logica.BussinessADO
{
    class csSistemaADO
    {
        csConexionADO MiConexion = new csConexionADO();

        public csTablaBE Consultar(string strSistemaID, string strNombre, string strEstado, int intPageSize, int intPageNumber, string strcontar)
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
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_SISTEMA_CONSULTAR_MRE";
            cmd.Parameters.Add(new SqlParameter("@P_SIST_SSISTEMAID", strSistemaID));
            cmd.Parameters.Add(new SqlParameter("@P_SIST_VNOMBRE", strNombre));
            cmd.Parameters.Add(new SqlParameter("@P_SIST_CESTADO", strEstado));
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

        public byte[] LeerImagen(string strSistemaID)
        {
            byte[] arr = null;
            String sCadena = MiConexion.GetCadenaConexion();
            SqlConnection cnx = new SqlConnection(sCadena);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            cmd.Connection = cnx;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_SISTEMA_CONSULTAR_IMAGEN_MRE";
            cmd.Parameters.Add(new SqlParameter("@P_SIST_SSISTEMAID", strSistemaID));

            try
            {
                cnx.Open();

                dr = cmd.ExecuteReader();
                dr.Read();
                if (dr["SIST_VEXTENSION"] != null)
                {
                    if (dr["SIST_VEXTENSION"].ToString().Trim().Length > 0)
                    { arr = (byte[])dr["SIST_GIMAGEN"]; }
                }
                cnx.Close();
                return arr;
            }
            catch (SqlException ex)
            { throw ex; }

            finally
            { cnx.Close(); }
        }

        public string Adicionar(csSistemaBE BE)
        {
            String sCadena = MiConexion.GetCadenaConexion();
            SqlConnection cnx = new SqlConnection(sCadena);
            SqlCommand cmd = new SqlCommand();


            //Se configura el comando: Store Procedure
            cnx.Open();
            cmd.Connection = cnx;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_SISTEMA_ADICIONAR_MRE";
            cmd.CommandTimeout = 0;

            string sError = "N";
            try
            {
                cmd.Parameters.Add(new SqlParameter("@P_SIST_VNOMBRE", BE.nombre));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_VDESCRIPCION", BE.descripcion));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_VABREVIATURA", BE.abreviatura));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_VURLDESARROLLO", BE.urlDesarrollo));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_VURLPRUEBAS", BE.urlTest));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_VURLPRODUCCION", BE.urlProduccion));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_VGUID", BE.guid));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_SORDEN", BE.orden));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_SUSUARIOCREACION", BE.UsuarioCreacion));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_VIPCREACION", BE.IPCreacion));

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

        public string Modificar(csSistemaBE BE)
        {
            String sCadena = MiConexion.GetCadenaConexion();
            SqlConnection cnx = new SqlConnection(sCadena);
            SqlCommand cmd = new SqlCommand();


            //Se configura el comando: Store Procedure
            cnx.Open();
            cmd.Connection = cnx;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_SISTEMA_MODIFICAR_MRE";
            cmd.CommandTimeout = 0;

            string sError = "N";
            try
            {
                cmd.Parameters.Add(new SqlParameter("@P_SIST_SSISTEMAID", BE.SistemaId));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_VNOMBRE", BE.nombre));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_VDESCRIPCION", BE.descripcion));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_VABREVIATURA", BE.abreviatura));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_VURLDESARROLLO", BE.urlDesarrollo));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_VURLPRUEBAS", BE.urlTest));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_VURLPRODUCCION", BE.urlProduccion));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_VGUID", BE.guid));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_SORDEN", BE.orden));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_CESTADO", BE.Estado));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_SUSUARIOMODIFICACION", BE.UsuarioModificacion));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_VIPMODIFICACION", BE.IPModificacion));

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

        public string ModificarImagen(csSistemaBE BE)
        {
            String sCadena = MiConexion.GetCadenaConexion();
            SqlConnection cnx = new SqlConnection(sCadena);
            SqlCommand cmd = new SqlCommand();


            //Se configura el comando: Store Procedure
            cnx.Open();
            cmd.Connection = cnx;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_SISTEMA_MODIFICAR__IMAGEN_MRE";
            cmd.CommandTimeout = 0;

            string sError = "N";
            try
            {
                cmd.Parameters.Add(new SqlParameter("@P_SIST_SSISTEMAID", BE.SistemaId));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_GIMAGEN", SqlDbType.Image)).Value = BE.imagen;
                cmd.Parameters.Add(new SqlParameter("@P_SIST_VEXTENSION", BE.extension));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_SUSUARIOMODIFICACION", BE.UsuarioModificacion));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_VIPMODIFICACION", BE.IPModificacion));

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

        public string Anular(string strSistemaID, string strUsuarioModificacion, string strIPModificacion)
        {
            String sCadena = MiConexion.GetCadenaConexion();
            SqlConnection cnx = new SqlConnection(sCadena);
            SqlCommand cmd = new SqlCommand();


            //Se configura el comando: Store Procedure
            cnx.Open();
            cmd.Connection = cnx;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PS_SEGURIDAD.USP_SE_SISTEMA_ANULAR_MRE";
            cmd.CommandTimeout = 0;

            string sError = "N";
            try
            {
                cmd.Parameters.Add(new SqlParameter("@P_SIST_SSISTEMAID", strSistemaID));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_SUSUARIOMODIFICACION", strUsuarioModificacion));
                cmd.Parameters.Add(new SqlParameter("@P_SIST_VIPMODIFICACION", strIPModificacion));

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
