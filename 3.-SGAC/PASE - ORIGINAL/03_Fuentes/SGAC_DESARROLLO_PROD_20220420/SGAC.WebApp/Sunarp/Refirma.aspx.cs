using SGAC.Accesorios;
using SGAC.WebApp.Refirma;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGAC.WebApp.Sunarp
{
    public partial class Refirma : System.Web.UI.Page
    {
        static string CLIENTID = System.Web.Configuration.WebConfigurationManager.AppSettings["CLIENTID"];
        static string CLIENTSECRET = System.Web.Configuration.WebConfigurationManager.AppSettings["CLIENTSECRET"];

        static string PROTOCOL = "";
        static string SERVER_PATH = "";

        static string DIR_IMAGE = "";
        static string FILEUPLOADURL = "";
        static string FILEDOWNLOADLOGOURL = "";
        static string FILEDOWNLOADSTAMPURL = "";
        static string VALOR_NOMBRE = "";


        private static void ActualizarValores()
        {
            bool valor = HttpContext.Current.Request.IsSecureConnection;
            if (valor)
            {
                SERVER_PATH = FullyQualifiedApplicationPath();
                DIR_IMAGE = SERVER_PATH + "/Refirma/img";
                //FILEUPLOADURL = SERVER_PATH + "/Sunarp/Refirma.aspx";
                FILEUPLOADURL = SERVER_PATH + "/Sunarp/RefirmaService.asmx/GuardarDocumento";
                FILEDOWNLOADLOGOURL = DIR_IMAGE + "/escudo.png";
                FILEDOWNLOADSTAMPURL = DIR_IMAGE + "/escudo.png";
                PROTOCOL = "S";
            }
            else
            {
                SERVER_PATH = FullyQualifiedApplicationPath();
                DIR_IMAGE = SERVER_PATH + "/Refirma/img";
                //FILEUPLOADURL = SERVER_PATH + "/Sunarp/Refirma.aspx";
                FILEUPLOADURL = SERVER_PATH + "/Sunarp/RefirmaService.asmx/GuardarDocumento";
                FILEDOWNLOADLOGOURL = DIR_IMAGE + "/escudo.png";
                FILEDOWNLOADSTAMPURL = DIR_IMAGE + "/escudo.png";
                PROTOCOL = "T";
            }
        }
        private static string FullyQualifiedApplicationPath()
        {
            string appPath = string.Empty; //Getting the current context of HTTP request 
            var context = HttpContext.Current; //Checking the current context content 
            if (context != null)
            { //Formatting the fully qualified website url/name 
                appPath = string.Format(@"{0}://{1}{2}{3}",
                    context.Request.Url.Scheme,
                    context.Request.Url.Host,
                    context.Request.Url.Port == 80 ? string.Empty : ":" + context.Request.Url.Port,
                    context.Request.ApplicationPath);
            }
            if (!appPath.EndsWith("/")) appPath += "/";
            return appPath;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        [WebMethod]
        public static string retornarArgumentosJson(string type, string documentName)
        {
            documentName = HttpContext.Current.Session["NombreArchivoConFirma"].ToString(); 
            string archivoDescargar = HttpContext.Current.Session["NombreArchivoSinFirma"].ToString();
            int cantidadHojas = Convert.ToInt16(HttpContext.Current.Session["CantHojasPDFOriginal"]);
            ActualizarValores();
            clsParametros obj = new clsParametros();
            if (type == "W")
            {
                obj.app = "pdf";
                obj.fileUploadUrl = FILEUPLOADURL;
                obj.reason = "Soy_el_autor del_documento";
                obj.pageNumber = "0";
                obj.maxFileSize = "10242880";
                obj.type = type;
                obj.clientId = CLIENTID;
                obj.clientSecret = CLIENTSECRET;
                obj.dcfilter = ".*FIR.*|.*FAU.*";
                obj.fileDownloadUrl = SERVER_PATH + "/documents/" + archivoDescargar;
                obj.posx = "5";
                obj.posy = "750";
                obj.outputFile = documentName;
                obj.protocol = PROTOCOL;
                obj.contentFile = archivoDescargar;
                obj.stampAppearanceId = "0";
                obj.isSignatureVisible = "true";
                obj.idFile = "MyForm";
                obj.fileDownloadLogoUrl = FILEDOWNLOADLOGOURL;
                obj.fileDownloadStampUrl = FILEDOWNLOADSTAMPURL;
                obj.pageNumber = cantidadHojas.ToString();
                obj.maxFileSize = "10242880";
                obj.fontSize = "7";
                obj.timestamp = "false";
            }
            else
            {
                obj.app = "pdf";
                obj.fileUploadUrl = FILEUPLOADURL;
                obj.reason = "Soy_el_autor del_documento";
                obj.pageNumber = "0";
                obj.maxFileSize = "10242880";
                obj.type = type;
                obj.clientId = CLIENTID;
                obj.clientSecret = CLIENTSECRET;
                obj.dcfilter = ".*FIR.*|.*FAU.*";
                obj.fileDownloadUrl = "";
                obj.posx = "5";
                obj.posy = "750";
                obj.outputFile = documentName;
                obj.protocol = PROTOCOL;
                obj.contentFile = "";
                obj.stampAppearanceId = "0";
                obj.isSignatureVisible = "true";
                obj.idFile = "MyForm";
                obj.fileDownloadLogoUrl = FILEDOWNLOADLOGOURL;
                obj.fileDownloadStampUrl = FILEDOWNLOADSTAMPURL;
                obj.pageNumber = "0";
                obj.maxFileSize = "10242880";
                obj.fontSize = "7";
                obj.timestamp = "false";
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonString = serializer.Serialize(obj);
            string jsonStringEncode = Base64Encode(jsonString);
            return jsonStringEncode;
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}