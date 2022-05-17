using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.IO;
using SGAC.Accesorios;

namespace SGAC.WebApp.Registro.Services
{
    /// <summary>
    /// Descripción breve de Services
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]

    public class Services : System.Web.Services.WebService
    {

        [WebMethod]
        public String GetCapturedImage(string str_Base64, string str_Carpeta, string str_Name, string str_ext)
        {
            String s_RutaImagen = ConfigurationManager.AppSettings["UploadPath"].ToString();
            String _fileName = String.Empty;
            String imagePath = String.Empty;
            try
            {

                byte[] bytes = Convert.FromBase64String(str_Base64.Replace("data:image/" + str_ext.Replace("jpg", "jpeg") + ";base64,", "").Trim());
                string imageName = Documento.GetUniqueUploadFileName(Convert.ToInt64("0"), s_RutaImagen, ref _fileName);

                imagePath = string.Format("{0}.jpg", _fileName);
                if (str_Carpeta.Equals(""))
                {
                    File.WriteAllBytes(Path.Combine(s_RutaImagen, imagePath), bytes);
                }
                else
                {
                    File.WriteAllBytes(Path.Combine(s_RutaImagen, str_Carpeta, imagePath), bytes);
                }
                

            }
            catch
            {
            }
            String url = imagePath;
            return url;
        }
    }
}
