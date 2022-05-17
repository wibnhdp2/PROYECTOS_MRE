using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SGAC.Accesorios
{
    public class Fecha
    {
        #region FechaCadena
        //*********************************************************************************************
        //** NOMBRE      : FechaCadena
        //** TIPO        : Funcion
        //** DESCRIPCION : Convierte una fecha en formato de hora local a cadenatexto en formato de hora universal
        //** PARAMETROS  : 
        //**               TIPO       |  NOMBRE          | DESCRIPCION
        //**               ----------------------------------------------
        //**               DateTime   |  DatFecha        | Fecha y actual a tranformar
        //** 
        //** DEVUELVE    : string
        //*********************************************************************************************
        public string FechaCadena(DateTime DatFecha)
        {
            DatFecha = DatFecha.ToUniversalTime();
            string strFecha = DatFecha.Year.ToString("0000") + DatFecha.Month.ToString("00") + DatFecha.Day.ToString("00") + "T" + DatFecha.Hour.ToString("00") + DatFecha.Minute.ToString("00");

            return strFecha;
        }
        #endregion FechaCadena

        #region CadenaFecha
        /// <summary>
        /// Convierte una cadenatexto en formato de fecha y hora universal a datetime en fomato de fecha y hora local
        /// </summary>
        /// <param name="StrCadenaFecha">Fecha y actual a tranformar</param>
        /// <returns>DateTime</returns>
        public DateTime CadenaFecha(string StrCadenaFecha)
        {
            string strCadena = StrCadenaFecha.Substring(6, 2) + "/" + StrCadenaFecha.Substring(4, 2) + "/" + StrCadenaFecha.Substring(0, 4) + " " + StrCadenaFecha.Substring(9, 2) + ":" + StrCadenaFecha.Substring(11, 2);
            DateTime datFecha = Convert.ToDateTime(strCadena);
            return datFecha.ToLocalTime();
        }
        #endregion CadenaFecha

        public static DateTime? ConvertirCadena(string strFecha)
        {
            try
            {
                DateTime datFecha = new DateTime
                    (
                    Convert.ToInt16(strFecha.Substring(0, 4)),
                    Convert.ToInt16(strFecha.Substring(4, 2)),
                    Convert.ToInt16(strFecha.Substring(6, 2)),
                    Convert.ToInt16(strFecha.Substring(9, 2)),
                    Convert.ToInt16(strFecha.Substring(11, 2)),
                    0
                    );

                return datFecha;
            }
            catch
            {
                return null;
            }
        }
        //---------------------------------------------------------------
        //Fecha: 11/10/2017
        //Autor: Miguel Márquez beltrán
        //Objetivo: Se modifico el formato de la asignación de la fecha
        //---------------------------------------------------------------
        //---------------------------------------------------------------
        //Fecha: 11/10/2017
        //Autor: Miguel Márquez beltrán
        //Objetivo: Se modifico el formato de la asignación de la fecha
        //---------------------------------------------------------------

        public static string ConvertirFecha(DateTime? datFecha)
        {
            try
            {
                DateTime datActual = new DateTime().ToUniversalTime();
                string strFecha = "";

                if (datFecha != null)
                {
                    datActual = (DateTime)datFecha;                    
                }
                strFecha = datActual.Year.ToString("0000") + datActual.Month.ToString("00") + datActual.Day.ToString("00") + " " + datActual.Hour.ToString("00") + datActual.Minute.ToString("00");
                //strFecha = datActual.Year.ToString("0000") + datActual.Month.ToString("00") + datActual.Day.ToString("00") + "T" + datActual.Hour.ToString("00") + datActual.Minute.ToString("00");

                return strFecha;              

            }
            catch
            {
                return null;
            }
        }

        //----------------------------------------------------
        //Fecha: 17/10/2016
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Obtener la fecha actual del consulado
        //----------------------------------------------------

        public DateTime? ObtenerFechaActual(string strOficinaConsularId)
        {
            DateTime dFecha = new DateTime();

            try
            {               

                String StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
                using (SqlConnection cnn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT [PS_ACCESORIOS].FN_OBTENER_FECHAACTUAL(" + strOficinaConsularId + ")", cnn))
                    {

                        cnn.Open();
                        cmd.CommandType = CommandType.Text;

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    if (!dr.IsDBNull(0))
                                        dFecha = dr.GetDateTime(0);
                                }
                                return dFecha;
                            }
                            else return null;
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
