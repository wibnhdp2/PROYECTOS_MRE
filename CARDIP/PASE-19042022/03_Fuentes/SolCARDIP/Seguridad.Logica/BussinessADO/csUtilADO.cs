using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Seguridad.Logica.BussinessEntity;

namespace Seguridad.Logica.BussinessADO
{
    class csUtilADO
    {
        csConexionADO MiConexion = new csConexionADO();

        public DateTime obtenerDateTime(string stryyyymmdd)
        {
            string strAnio = stryyyymmdd.Substring(0, 4);
            string strMes = stryyyymmdd.Substring(4, 2);
            string strDia = stryyyymmdd.Substring(6, 2);
            int intanio = Convert.ToInt32(strAnio);
            int intMes = Convert.ToInt32(strMes);
            int intDia = Convert.ToInt32(strDia);
            DateTime dt = new DateTime(intanio, intMes, intDia);
            return dt;
        }

        public DateTime obtenerFechaHora(string strAnioMesDiaHoraMin)
        {
            //2015-05-07 11:32

            string strAnio = strAnioMesDiaHoraMin.Substring(0, 4);
            string strMes = strAnioMesDiaHoraMin.Substring(5, 2);
            string strDia = strAnioMesDiaHoraMin.Substring(8, 2);
            string strHor = strAnioMesDiaHoraMin.Substring(11, 2);
            string strMin = strAnioMesDiaHoraMin.Substring(14, 2);

            int intanio = Convert.ToInt32(strAnio);
            int intMes = Convert.ToInt32(strMes);
            int intDia = Convert.ToInt32(strDia);
            int intHor = Convert.ToInt32(strHor);
            int intMin = Convert.ToInt32(strMin);

            DateTime dt = new DateTime(intanio, intMes, intDia, intHor, intMin,0);
            return dt;
        }

        public string obtenerFechaHoraMinutoAdicional()
        {
            string strFechaHora = "";
            String sCadena = MiConexion.GetCadenaConexion();
            SqlConnection cnx = new SqlConnection(sCadena);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            cmd.Connection = cnx;

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select FechaHora = convert(char(16),DATEADD(MINUTE,1,GETDATE()),121)";
            cnx.Open();
            try
            {
                dr = cmd.ExecuteReader();
                dr.Read();
                strFechaHora = dr["FechaHora"].ToString();
            }
            catch (SqlException ex)
            { throw ex; }

            finally
            { cnx.Close(); }
            return strFechaHora;
        }

        public string obtenerFechaHoraActual()
        {
            string strFechaHoraActual = "";
            String sCadena = MiConexion.GetCadenaConexion();
            SqlConnection cnx = new SqlConnection(sCadena);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;

            cmd.Connection = cnx;

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select FechaHora = convert(char(16),GETDATE(),121)";
            cnx.Open();
            try
            {
                dr = cmd.ExecuteReader();
                dr.Read();
                strFechaHoraActual = dr["FechaHora"].ToString();
            }
            catch (SqlException ex)
            { throw ex; }

            finally
            { cnx.Close(); }
            return strFechaHoraActual;
        }

        public string obtenerFechaHoraMenorIgual(string strFechaHoraMinAdicional)
        {
            string strMenorIgual = "N";
            string strFechaHoraActual = obtenerFechaHoraActual();
            DateTime dtFechaHoraActual = obtenerFechaHora(strFechaHoraActual);
            DateTime dtFechaHoraMinAdicional = obtenerFechaHora(strFechaHoraMinAdicional);

            if (dtFechaHoraActual <= dtFechaHoraMinAdicional)
            {
                strMenorIgual = "S";
            }
            return strMenorIgual;
        }
    }
}
