using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using System.IO;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace SGAC.Accesorios
{
    public class Documento
    {
        private String[] UNIDADES = {"", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve" };
        private String[] DECENAS = {"diez", "once", "doce", "trece", "catorce", "quince", "dieciseis","diecisiete", "dieciocho", "diecinueve", "veinti", "treinta", "cuarenta","cincuenta", "sesenta", "setenta", "ochenta", "noventa"};
        private String[] CENTENAS = {"", "ciento", "doscientos", "trecientos", "cuatrocientos", "quinientos", "seiscientos","setecientos", "ochocientos", "novecientos"};

        public static DataTable ObtenerDatosPorActuacionDetalle(Int64 lngActuacionDetalleId)
        {
            DataTable dtResult = new DataTable();
            try{

                String StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
                using (SqlConnection cnn = new SqlConnection(StrConnectionName)) {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONDETALLE_CONSULTAR_ID", cnn)){

                        cnn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@iActuacionDetalleId", SqlDbType.BigInt).Value = lngActuacionDetalleId;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds.Tables[0];
                        }
                    }
                }
               
            }
            catch (Exception ex) {

                return null;
            }
        }


        public  String USP_RE_ELIMINA_ETIQUETAS_HTML(String ParrafoConHTML)
        {
            try
            {
                String ParrafoSinHTML = String.Empty;

                String StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
                using (SqlConnection cnn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ELIMINA_ETIQUETAS_HTML", cnn))
                    {

                        cnn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@parrafo_con_html", SqlDbType.VarChar).Value = ParrafoConHTML;

                        using (SqlDataReader dr = cmd.ExecuteReader()) {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    if (!dr.IsDBNull(0))
                                        ParrafoSinHTML = dr.GetString(0);
                                }
                                return ParrafoSinHTML;
                            }
                            else return null;
                        }
                    }
                }

            }
            catch (Exception ex) {
                return null;
            }
        }

        public static string GetUniqueUploadFileName(Int64 iActuacionDetalleId, String uploadPath, ref String fileName)
        {
            try{

                DataTable dt = new DataTable();
                dt = ObtenerDatosPorActuacionDetalle(iActuacionDetalleId);
                String iActuacion = String.Empty;
                String FechaRegistro = String.Empty;
               

                foreach (DataRow row in dt.Rows) {

                    iActuacion = row["acde_iActuacionId"].ToString();
                    FechaRegistro = Convert.ToDateTime(row["acde_dFechaRegistrO"]).ToString("yyyyMMdd");
                  
                }

                if(string.IsNullOrEmpty(FechaRegistro))
                    FechaRegistro = DateTime.Now.ToString("yyyyMMdd");

                if (string.IsNullOrEmpty(iActuacion))
                    iActuacion = "0";

                String strScript = String.Empty;
                String FileName = String.Empty;

                string filepath = uploadPath + fileName;
                string fileext = Path.GetExtension(filepath);
                string filenamewithoutext = Path.GetFileNameWithoutExtension(filepath);

                String Hora = String.Empty;
                Random r = new Random();
                int aleatorio1 = r.Next();


                fileName = FechaRegistro + "_" + iActuacion + "_" + iActuacionDetalleId + "_" + aleatorio1 + fileext;

                filepath = uploadPath + fileName;

                return filepath;

            }
            catch (Exception ex) {
                return "";
            }                              
        }


        //-------------------------------------------------------------------------
        //Fecha: 24/01/2017
        //Autor: Miguel Angel Márquez Beltrán
        //Objetivo: Obtener el nombre de archivo PDF para guardar en el Disco
        //-------------------------------------------------------------------------
        public static string GetUniqueUploadFileNamePDF(Int64 iActuacionDetalleId, string struploadPath, ref string strFileName)
        {
            try
            {               
                DataTable dt = new DataTable();
                dt = ObtenerDatosPorActuacionDetalle(iActuacionDetalleId);
                String iActuacion = String.Empty;
                String FechaRegistro = String.Empty;
                string strMision = String.Empty;

                foreach (DataRow row in dt.Rows)
                {
                    iActuacion = row["acde_iActuacionId"].ToString();
                    FechaRegistro = Convert.ToDateTime(row["acde_dFechaRegistrO"]).ToString("yyyyMMdd");
                    strMision = row["ofco_vSiglas"].ToString();
                }
                if (string.IsNullOrEmpty(FechaRegistro))
                    FechaRegistro = DateTime.Now.ToString("yyyyMMdd");

                if (string.IsNullOrEmpty(iActuacion))
                    iActuacion = "0";

                String strScript = String.Empty;
                String FileName = String.Empty;

                string filepath = struploadPath + "/" + strFileName;
                string fileext = Path.GetExtension(filepath);
                string filenamewithoutext = Path.GetFileNameWithoutExtension(filepath);

                String Hora = String.Empty;
                Random r = new Random();
                int aleatorio1 = r.Next();

                strFileName = FechaRegistro + "_" + iActuacion + "_" + iActuacionDetalleId + "_" + aleatorio1 + fileext;

                string strAnio = FechaRegistro.Substring(0, 4);
                string strMes = FechaRegistro.Substring(4, 2);
                string strDia = FechaRegistro.Substring(6, 2);

                string strpathMision = Path.Combine(struploadPath, strMision);
                string strpathAnio = Path.Combine(strpathMision, strAnio);
                string strpathAnioMes = Path.Combine(strpathAnio, strMes);
                string strpathAnioMesDia = Path.Combine(strpathAnioMes, strDia);

                if (!Directory.Exists(strpathMision))
                {
                    Directory.CreateDirectory(strpathMision);
                }
                if (!Directory.Exists(strpathAnio))
                {
                    Directory.CreateDirectory(strpathAnio);
                }
                if (!Directory.Exists(strpathAnioMes))
                {
                    Directory.CreateDirectory(strpathAnioMes);
                }
                if (!Directory.Exists(strpathAnioMesDia))
                {
                    Directory.CreateDirectory(strpathAnioMesDia);
                }

                filepath = Path.Combine(strpathAnioMesDia, strFileName);

                return filepath;
            }
            catch (Exception ex)
            {
                return "";
            }           
        }

        //-------------------------------------------------------
        //Fecha: 25/01/2017
        //Autor: Miguel Angel Márquez Beltrán
        //Objetivo: Obtener el nombre de la Misión (Siglas)
        //-------------------------------------------------------

        public static string GetMisionActuacionDetalle(Int64 iActuacionDetalleId)
        {
            try
            {
                string strMision = String.Empty;

                DataTable dt = new DataTable();
                dt = ObtenerDatosPorActuacionDetalle(iActuacionDetalleId);
                foreach (DataRow row in dt.Rows)
                {
                    strMision = row["ofco_vSiglas"].ToString();
                }
                dt.Dispose();
                return strMision;
            }
            catch (Exception ex)
            {
                return "";
            } 
        }
        //----------------------------------------------------

        private Regex r;


 
        public  String ConvertirNumeroLetras(String numero, bool mayusculas) {
        String literal = "";
        String parte_decimal;    
        //si el numero utiliza (.) en lugar de (,) -> se reemplaza
        numero = numero.Replace(".", ",");
        //si el numero no tiene parte decimal, se le agrega ,00
        if (numero.IndexOf(",") == -1)
        {
            numero = numero + ",00";
        } 
        //se valida formato de entrada -> 0,00 y 999 999 999,00
        r = new Regex(@"\d{1,9},\d{1,2}");            
        MatchCollection mc = r.Matches(numero);
        if (mc.Count > 0) {
            //se divide el numero 0000000,00 -> entero y decimal
            String[] Num = numero.Split(',');                    
            
            //de da formato al numero decimal
            parte_decimal = Num[1];
            //se convierte el numero a literal
            if (int.Parse(Num[0]) == 0) {//si el valor es cero                
                literal = "cero ";
            }
            else if (int.Parse(Num[0]) > 999999)
            {//si es millon
                literal = getMillones(Num[0]);
            }
            else if (int.Parse(Num[0]) > 999)
            {//si es miles
                literal = getMiles(Num[0]);
            }
            else if (int.Parse(Num[0]) > 99)
            {//si es centena
                literal = getCentenas(Num[0]);
            }
            else if (int.Parse(Num[0]) > 9)
            {//si es decena
                literal = getDecenas(Num[0]);
            } else {//sino unidades -> 9
                literal = getUnidades(Num[0]);
            }
            //devuelve el resultado en mayusculas o minusculas
            if (mayusculas) {                
                return (literal).ToUpper();
            } else {
                return (literal);
            }
        } else {//error, no se puede convertir
            return literal = null;
        }
    }
    
        /* funciones para convertir los numeros a literales */
        private  String getUnidades(String numero)
        {   // 1 - 9            
            //si tuviera algun 0 antes se lo quita -> 09 = 9 o 009=9
            String num = numero.Substring(numero.Length - 1);            
            return UNIDADES[int.Parse(num)];
        }

        private String getDecenas(String num)
        {// 99                        
            int n = int.Parse(num);
            if (n < 10)
            {//para casos como -> 01 - 09
                return getUnidades(num);
            }
            else if (n > 19)
            {//para 20...99
                String u = getUnidades(num);
                if (u.Equals(""))
                { //para 20,30,40,50,60,70,80,90
                    if (n == 20)
                    {
                        return "veinte";
                    }
                    else
                    {
                        return DECENAS[int.Parse(num.Substring(0, 1)) + 8];
                    }
                }
                else
                {
                    if (n >= 31)
                    {
                        return DECENAS[int.Parse(num.Substring(0, 1)) + 8] + " y " + u;
                    }
                    else
                    {
                        if (n == 22)
                        {
                            return "veintidós";
                        }
                        else
                        {
                            return DECENAS[int.Parse(num.Substring(0, 1)) + 8] + "" + u;
                        }
                    }
                }
            }
            else
            {//numeros entre 11 y 19
                return DECENAS[n - 10];
            }
        }

        private String getCentenas(String num)
        {// 999 o 099
            if (int.Parse(num) > 99)
            {//es centena
                if (int.Parse(num) == 100)
                {//caso especial
                    return "cien";
                }
                else
                {
                    return CENTENAS[int.Parse(num.Substring(0, 1))] + " " + getDecenas(num.Substring(1));
                }
            }
            else
            {//por Ej. 099 
                //se quita el 0 antes de convertir a decenas
                return getDecenas(int.Parse(num) + "");
            }
        }

        private String getMiles(String numero)
        {// 999 999
            //obtiene las centenas
            String c = numero.Substring(numero.Length - 3);
            //obtiene los miles
            String m = numero.Substring(0, numero.Length - 3);
            String n = "";
            //se comprueba que miles tenga valor entero
            if (int.Parse(m) > 0)
            {
                if (int.Parse(m) == 1)
                {
                    return "mil " + getCentenas(c);
                }
                else
                {
                    n = getCentenas(m);
                    return n + " mil " + getCentenas(c);
                }
            }
            else
            {
                return "" + getCentenas(c);
            }

        }

        private String getMillones(String numero) 
        { //000 000 000        
            //se obtiene los miles
            String miles = numero.Substring(numero.Length - 6); 
            //se obtiene los millones
            String millon = numero.Substring(0, numero.Length - 6);
            String n = "";
            if (millon.Length > 1) 
            {
                n = getCentenas(millon) + " millones";
            }
            else
            {
                n = getUnidades(millon) + " millon";
            }
            return n + getMiles(miles);
        }

        public static string GetUniqueUploadFileNamePDFEscrituraPublica(string strMision, Int64 iActoNotarialId, string struploadPath, ref string strFileName, ref string strRutaFileName, string isEP="E")
        {
            try
            {                              
                String FechaRegistro = String.Empty;               
               
                if (string.IsNullOrEmpty(FechaRegistro))
                    FechaRegistro = DateTime.Now.ToString("yyyyMMdd");
                
                String strScript = String.Empty;
                String FileName = String.Empty;

                string filepath = struploadPath + "/" + strFileName;
                string fileext = Path.GetExtension(filepath);
                string filenamewithoutext = Path.GetFileNameWithoutExtension(filepath);

                String Hora = String.Empty;
                Random r = new Random();
                int aleatorio1 = r.Next();

                strFileName = FechaRegistro + "_" + iActoNotarialId + "_" + isEP + "_" + aleatorio1 + fileext;

                string strAnio = FechaRegistro.Substring(0, 4);
                string strMes = FechaRegistro.Substring(4, 2);
                string strDia = FechaRegistro.Substring(6, 2);

                string strpathMision = Path.Combine(struploadPath, strMision);
                string strpathAnio = Path.Combine(strpathMision, strAnio);
                string strpathAnioMes = Path.Combine(strpathAnio, strMes);
                string strpathAnioMesDia = Path.Combine(strpathAnioMes, strDia);

                strRutaFileName = strMision + "@" + strAnio + "@" + strMes + "@" + strDia + "@" + strFileName;

                if (!Directory.Exists(strpathMision))
                {
                    Directory.CreateDirectory(strpathMision);
                }
                if (!Directory.Exists(strpathAnio))
                {
                    Directory.CreateDirectory(strpathAnio);
                }
                if (!Directory.Exists(strpathAnioMes))
                {
                    Directory.CreateDirectory(strpathAnioMes);
                }
                if (!Directory.Exists(strpathAnioMesDia))
                {
                    Directory.CreateDirectory(strpathAnioMesDia);
                }

                filepath = Path.Combine(strpathAnioMesDia, strFileName);

                return filepath;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        //-------------------------------------------------------------------------
        //Fecha: 24/04/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: Obtener el nombre de archivo PDF para guardar en el Disco y Devulve la nueva ruta donde se guarda
        //-------------------------------------------------------------------------
        public static string GetUniqueUploadFileNamePDF(Int64 iActuacionDetalleId, string struploadPath, ref string strFileName, ref string strRutaFileName)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ObtenerDatosPorActuacionDetalle(iActuacionDetalleId);
                String iActuacion = String.Empty;
                String FechaRegistro = String.Empty;
                string strMision = String.Empty;

                foreach (DataRow row in dt.Rows)
                {
                    iActuacion = row["acde_iActuacionId"].ToString();
                    FechaRegistro = Convert.ToDateTime(row["acde_dFechaRegistrO"]).ToString("yyyyMMdd");
                    strMision = row["ofco_vSiglas"].ToString();
                }
                if (string.IsNullOrEmpty(FechaRegistro))
                    FechaRegistro = DateTime.Now.ToString("yyyyMMdd");

                if (string.IsNullOrEmpty(iActuacion))
                    iActuacion = "0";

                String strScript = String.Empty;
                String FileName = String.Empty;

                string filepath = struploadPath + "/" + strFileName;
                string fileext = Path.GetExtension(filepath);
                string filenamewithoutext = Path.GetFileNameWithoutExtension(filepath);

                String Hora = String.Empty;
                Random r = new Random();
                int aleatorio1 = r.Next();

                strFileName = FechaRegistro + "_" + iActuacion + "_" + iActuacionDetalleId + "_" + aleatorio1 + fileext;

                string strAnio = FechaRegistro.Substring(0, 4);
                string strMes = FechaRegistro.Substring(4, 2);
                string strDia = FechaRegistro.Substring(6, 2);

                string strpathMision = Path.Combine(struploadPath, strMision);
                string strpathAnio = Path.Combine(strpathMision, strAnio);
                string strpathAnioMes = Path.Combine(strpathAnio, strMes);
                string strpathAnioMesDia = Path.Combine(strpathAnioMes, strDia);

                strRutaFileName = strMision + "@" + strAnio + "@" + strMes + "@" + strDia + "@" + strFileName;

                if (!Directory.Exists(strpathMision))
                {
                    Directory.CreateDirectory(strpathMision);
                }
                if (!Directory.Exists(strpathAnio))
                {
                    Directory.CreateDirectory(strpathAnio);
                }
                if (!Directory.Exists(strpathAnioMes))
                {
                    Directory.CreateDirectory(strpathAnioMes);
                }
                if (!Directory.Exists(strpathAnioMesDia))
                {
                    Directory.CreateDirectory(strpathAnioMesDia);
                }

                filepath = Path.Combine(strpathAnioMesDia, strFileName);

                return filepath;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        //-------------------------------------------------------------------------
        //Fecha: 24/04/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: Obtener el nombre de archivo PDF para guardar en el Disco y Devulve la nueva ruta donde se guarda
        //-------------------------------------------------------------------------
        public static string GetUniqueUploadFileName(Int64 iActuacionDetalleId, String uploadPath, ref String fileName,ref string strRutaFileName)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ObtenerDatosPorActuacionDetalle(iActuacionDetalleId);
                String iActuacion = String.Empty;
                String FechaRegistro = String.Empty;


                foreach (DataRow row in dt.Rows)
                {

                    iActuacion = row["acde_iActuacionId"].ToString();
                    FechaRegistro = Convert.ToDateTime(row["acde_dFechaRegistrO"]).ToString("yyyyMMdd");

                }

                if (string.IsNullOrEmpty(FechaRegistro))
                    FechaRegistro = DateTime.Now.ToString("yyyyMMdd");

                if (string.IsNullOrEmpty(iActuacion))
                    iActuacion = "0";

                String strScript = String.Empty;
                String FileName = String.Empty;

                string filepath = uploadPath + "/" + fileName;
                string fileext = Path.GetExtension(filepath);
                string filenamewithoutext = Path.GetFileNameWithoutExtension(filepath);

                String Hora = String.Empty;
                Random r = new Random();
                int aleatorio1 = r.Next();


                fileName = FechaRegistro + "_" + iActuacion + "_" + iActuacionDetalleId + "_" + aleatorio1 + fileext;

                filepath = uploadPath + "/" + fileName;

                return filepath;

            }
            catch (Exception ex)
            {
                return "";
            }
        }

        //------------------------------------------------------------------------------
        //Fecha: 21/10/2020
        //Autor: Miguel Márquez Beltrán
        //Motivo: Obtener el nombre de archivo y la ruta para guardar en el disco.
        //------------------------------------------------------------------------------
        public static string GetUniqueUploadFileNameSUNARP(string strCUO, string struploadPath, ref string strFileName)
        {
            try
            {
                string strSigla = "EP";

                string strScript = string.Empty;
                string FileName = string.Empty;

                string filepath = struploadPath + "/" + strFileName;
                string fileext = Path.GetExtension(filepath);
                string filenamewithoutext = Path.GetFileNameWithoutExtension(filepath);
                string stryyyyMMdd_HHMMSS = DateTime.Now.ToString("yyyyMMddHHmmss");


                strFileName = strSigla + "_" + strCUO + "_" + stryyyyMMdd_HHMMSS + fileext;

                string strAnio = stryyyyMMdd_HHMMSS.Substring(0, 4);
                string strMes = stryyyyMMdd_HHMMSS.Substring(4, 2);
                string strDia = stryyyyMMdd_HHMMSS.Substring(6, 2);

                string strpathSigla = Path.Combine(struploadPath, strSigla);
                string strpathAnio = Path.Combine(strpathSigla, strAnio);
                string strpathAnioMes = Path.Combine(strpathAnio, strMes);
                string strpathAnioMesDia = Path.Combine(strpathAnioMes, strDia);

                //strRutaFileName = strSigla + "@" + strAnio + "@" + strMes + "@" + strDia + "@" + strFileName;

                if (!Directory.Exists(strpathSigla))
                {
                    Directory.CreateDirectory(strpathSigla);
                }
                if (!Directory.Exists(strpathAnio))
                {
                    Directory.CreateDirectory(strpathAnio);
                }
                if (!Directory.Exists(strpathAnioMes))
                {
                    Directory.CreateDirectory(strpathAnioMes);
                }
                if (!Directory.Exists(strpathAnioMesDia))
                {
                    Directory.CreateDirectory(strpathAnioMesDia);
                }

                filepath = Path.Combine(strpathAnioMesDia, strFileName);

                return filepath;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }
}
