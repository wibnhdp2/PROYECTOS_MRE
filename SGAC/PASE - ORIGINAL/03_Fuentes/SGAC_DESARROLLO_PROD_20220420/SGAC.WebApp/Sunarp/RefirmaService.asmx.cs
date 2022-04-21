using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SGAC.WebApp.Sunarp
{
    /// <summary>
    /// Descripción breve de RefirmaService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class RefirmaService : System.Web.Services.WebService
    {
        [WebMethod]
        public void GuardarDocumento()
        {
            for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
            {
                var file = HttpContext.Current.Request.Files[i];
                // ELIMINAR ARCHIVO
                string rutaPDF = Server.MapPath("../documents/");
                string strPathFileEliminar = Path.Combine(rutaPDF, file.FileName);

                string rutacompletaEliminar = strPathFileEliminar;

                if (File.Exists(rutacompletaEliminar))
                {
                    File.Delete(rutacompletaEliminar);
                }

                // GRABAR ARCHIVO
                string sValParametroEnc = System.Web.Configuration.WebConfigurationManager.AppSettings["carpetaEscrituras"];
                string ruta = sValParametroEnc;

                string documentName =  file.FileName;

                string strAnio = documentName.Substring(1, 4);
                //strAnio -> "2020"

                string strPathFile = Path.Combine(ruta, strAnio);

                if (!Directory.Exists(strPathFile))
                {
                    DirectoryInfo di = Directory.CreateDirectory(strPathFile);
                }

                file.SaveAs(Path.Combine(strPathFile, documentName));
            }
        }
    }
}
