using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using SAE.UInterfaces;
using System.Data.SqlClient;
using System.Data;

namespace SolCARDIP_REGLINEA.Librerias.ReglasNegocio
{
    public class brGeneral
    {
        public string CadenaConexion { get; set; }
        public string RutaLog { get; set; }
        public string AbrevSistema { get; set; }
        public string rutaXML { get; set; }
        public string rutaFirmas { get; set; }
        public string urlSistema { get; set; }
        public string rutaAdjuntos { get; set; }
        public string fileExtFotografia { get; set; }
        public string filePesoFotografia { get; set; }
        public string fileExtPDF { get; set; }
        public string cantRegxPag { get; set; }
        public string host { get; set; }
        public string FechaUpdate { get; set; }
        public string PermitirFotografia { get; set; }
        private static UIEncriptador UIEncripto = new UIEncriptador();

        public brGeneral()
        {
            string strCnx = ConfigurationManager.AppSettings["conexion"].ToString();
            string strCadenaEnc = ConfigurationManager.ConnectionStrings[strCnx].ToString();
            string rutaLogEnc = ConfigurationManager.AppSettings["rutaLog"].ToString();
            string strAbrevSistemaEnc = ConfigurationManager.AppSettings["AbrevSistema"].ToString();
            string urlSistemaEnc = ConfigurationManager.AppSettings["urlSistema"].ToString();
            string rutaAdjuntosEnc = ConfigurationManager.AppSettings["rutaAdjuntos"].ToString();
            string fileExtFotografiaEnc = ConfigurationManager.AppSettings["fileExtFotografia"].ToString();
            string filePesoFotografiaEnc = ConfigurationManager.AppSettings["filePesoFotografia"].ToString();
            string cantRegxPagEnc = ConfigurationManager.AppSettings["cantRegxPag"].ToString();
            string hostEnc = ConfigurationManager.AppSettings["host"].ToString();
            string FechaUpdateEnc = ConfigurationManager.AppSettings["FechaUpdate"].ToString();
            string PermitirFotografiaEnc = ConfigurationManager.AppSettings["PermitirFotografia"].ToString();
            
            CadenaConexion = UIEncripto.DesEncriptarCadena(strCadenaEnc);
            RutaLog = UIEncripto.DesEncriptarCadena(rutaLogEnc);
            AbrevSistema = UIEncripto.DesEncriptarCadena(strAbrevSistemaEnc);
            urlSistema = UIEncripto.DesEncriptarCadena(urlSistemaEnc);
            rutaAdjuntos = UIEncripto.DesEncriptarCadena(rutaAdjuntosEnc);
            fileExtFotografia = UIEncripto.DesEncriptarCadena(fileExtFotografiaEnc);
            filePesoFotografia = UIEncripto.DesEncriptarCadena(filePesoFotografiaEnc);
            cantRegxPag = UIEncripto.DesEncriptarCadena(cantRegxPagEnc);
            host = UIEncripto.DesEncriptarCadena(hostEnc);
            FechaUpdate = FechaUpdateEnc;
            PermitirFotografia = UIEncripto.DesEncriptarCadena(PermitirFotografiaEnc);
            fileExtPDF = ".pdf";
        }

        public void grabarLog(Exception ex)
        {
            using (StreamWriter sw = new StreamWriter(RutaLog, true))
            {
                sw.WriteLine("Fecha y Hora: " + DateTime.Now.ToString("hh:mm:ss.fff"));
                sw.WriteLine("Mensaje Error: " + ex.Message);
                sw.WriteLine("Detalle Error: " + ex.StackTrace);
                sw.WriteLine(new String('_', 50));
            }


        }
        public void grabarLogEnBD(string formulario, string metodo,Exception ex)
        {
            try
            {

                using (SqlConnection cnx = new SqlConnection(CadenaConexion))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_REGLINEA.SC_LOG_ERRORES_REGISTRAR_LOG", cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        #region Creando Parametros

                        cmd.Parameters.Add("@LOER_VNOMFORMULARIO", SqlDbType.VarChar, 100).Value = formulario;
                        cmd.Parameters.Add("@LOER_VERROR", SqlDbType.VarChar, 500).Value = ex.Message;
                        cmd.Parameters.Add("@LOER_VDETALLEERROR", SqlDbType.VarChar, 1000).Value = ex.StackTrace;
                        cmd.Parameters.Add("@LOER_VMETODO", SqlDbType.VarChar, 100).Value = metodo;
                        cmd.Parameters.Add("@LOER_SUSUARIOCREACION", SqlDbType.SmallInt).Value = "1";
                        cmd.Parameters.Add("@LOER_VIPCREACION", SqlDbType.VarChar, 20).Value = "00:00:00:00";

                        #endregion Creando Parametros
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException exec)
            {
                throw exec;
            }
        }
    }
}
