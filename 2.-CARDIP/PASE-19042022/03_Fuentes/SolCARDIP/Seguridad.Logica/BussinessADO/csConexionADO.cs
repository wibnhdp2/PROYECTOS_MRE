using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using SAE.UInterfaces;

namespace Seguridad.Logica.BussinessADO
{
    public class csConexionADO
    {
        private static UIEncriptador UIEncripto = new UIEncriptador();

        public String GetCadenaConexion()
        {
            string strCnx = ConfigurationManager.AppSettings["conexionSeguridad"].ToString();
            string strCadenaEnc = ConfigurationManager.ConnectionStrings[strCnx].ToString();
            string strCadena = UIEncripto.DesEncriptarCadena(strCadenaEnc);

            if (strCadena == null)
            { return null; }
            else
            { return strCadena; }
        }

        public string ExecuteSP(string sNameProcedure, SqlCommand cmd)
        {
            string sCadena = GetCadenaConexion();
            SqlConnection cnx = new SqlConnection(sCadena);

            //Se configura el comando: Store Procedure
            cmd.Connection = cnx;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = sNameProcedure;
            cmd.CommandTimeout = 0;

            try
            {
                cnx.Open();
                cmd.ExecuteNonQuery();
                cnx.Close();
                string sMensaje = cmd.Parameters["@chrError"].Value.ToString();
                return sMensaje;
            }
            catch (SqlException ex)
            {
                return "1." + ex.Message;
            }
            finally
            { cnx.Close(); }
        }

        public string getFechaActual()
        {
            string strFechaActual = "";
            string sCadena = GetCadenaConexion();
            //Se inicia la Conexion
            SqlConnection cnx = new SqlConnection(sCadena);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            cnx.Open();
            cmd.Connection = cnx;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select convert(char(8),getdate(),112) as FechaActual";

            try
            {
                dr = cmd.ExecuteReader();

                dr.Read();

                strFechaActual = dr["FechaActual"].ToString();
                dr.Close();
            }                        
            catch (SqlException ex)
            {
                return ex.Message;
            }
            finally
            { cnx.Close(); }            

            return strFechaActual;
        }
    }
}
