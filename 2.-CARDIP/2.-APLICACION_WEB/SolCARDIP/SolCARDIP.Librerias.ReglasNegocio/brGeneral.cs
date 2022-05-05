using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using SAE.UInterfaces;

namespace SolCARDIP.Librerias.ReglasNegocio
{
    public class brGeneral
    {
        public string CadenaConexion { get; set; }
        //public string CadenaConexionConsulta { get; set; }

        public string RutaLog1 { get; set; }

        public string RutaLog { get; set; }
        public string AbrevSistema { get; set; }
        public string rutaXML { get; set; }
        public string rutaFirmas { get; set; }
        public string urlSistema { get; set; }
        public string rutaAdjuntos { get; set; }
        public string fileExtFotografia { get; set; }
        public string filePesoFotografia { get; set; }
        public string cantRegxPag { get; set; }
        public string host { get; set; }
        public string rutaNotFound { get; set; }
        private static UIEncriptador UIEncripto = new UIEncriptador();

        public brGeneral()
        {
            //CONEXION DATOS
            string strCnx = ConfigurationManager.AppSettings["conexion"].ToString();
            string strCadenaEnc = ConfigurationManager.ConnectionStrings[strCnx].ToString();
            // CONEXION CONSULTA
            //string strCnxCon = ConfigurationManager.AppSettings["conexionConsulta"].ToString();
            //string strCadenaEncCon = ConfigurationManager.ConnectionStrings[strCnxCon].ToString();

            string rutaLogEnc1 = ConfigurationManager.AppSettings["rutaLog1"].ToString();
            string rutaLogEnc = ConfigurationManager.AppSettings["rutaLog"].ToString();
            string strAbrevSistemaEnc = ConfigurationManager.AppSettings["AbrevSistema"].ToString();
            string urlSistemaEnc = ConfigurationManager.AppSettings["urlSistema"].ToString();
            string rutaAdjuntosEnc = ConfigurationManager.AppSettings["rutaAdjuntos"].ToString();
            string fileExtFotografiaEnc = ConfigurationManager.AppSettings["fileExtFotografia"].ToString();
            string filePesoFotografiaEnc = ConfigurationManager.AppSettings["filePesoFotografia"].ToString();
            string cantRegxPagEnc = ConfigurationManager.AppSettings["cantRegxPag"].ToString();
            string hostEnc = ConfigurationManager.AppSettings["host"].ToString();

            CadenaConexion = UIEncripto.DesEncriptarCadena(strCadenaEnc);
            //CadenaConexionConsulta = UIEncripto.DesEncriptarCadena(strCadenaEncCon);

            RutaLog1 = UIEncripto.DesEncriptarCadena(rutaLogEnc1);

            RutaLog = UIEncripto.DesEncriptarCadena(rutaLogEnc);
            AbrevSistema = UIEncripto.DesEncriptarCadena(strAbrevSistemaEnc);
            urlSistema = UIEncripto.DesEncriptarCadena(urlSistemaEnc);
            rutaAdjuntos = UIEncripto.DesEncriptarCadena(rutaAdjuntosEnc);
            fileExtFotografia = UIEncripto.DesEncriptarCadena(fileExtFotografiaEnc);
            filePesoFotografia = UIEncripto.DesEncriptarCadena(filePesoFotografiaEnc);
            cantRegxPag = UIEncripto.DesEncriptarCadena(cantRegxPagEnc);
            host = UIEncripto.DesEncriptarCadena(hostEnc);
        }

        public void grabarLog(Exception ex)
        {
            using (StreamWriter sw = new StreamWriter(RutaLog, true))
            {
                System.Diagnostics.Debug.Write("\nError__________"+ex.Message+"\ntracert______"+ ex.StackTrace);
                sw.WriteLine("Fecha y Hora: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToString("hh:mm:ss.fff"));
                sw.WriteLine("Mensaje Error: " + ex.Message);
                sw.WriteLine("Detalle Error: " + ex.StackTrace);
                sw.WriteLine(new String('_', 50));
            }
        }

        public void grabarError(string mensaje)
        {
            using (StreamWriter sw = new StreamWriter(RutaLog1, true))
            {
                sw.WriteLine("Fecha y Hora: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToString("hh:mm:ss.fff"));
                sw.WriteLine("Mensaje Error: " + mensaje);
                sw.WriteLine(new String('_', 50));
            }
        }
    }
}
