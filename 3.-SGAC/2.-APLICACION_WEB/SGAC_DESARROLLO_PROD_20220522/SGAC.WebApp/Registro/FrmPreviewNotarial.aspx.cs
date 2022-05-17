using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using SGAC.Accesorios;

namespace SGAC.WebApp.Registro
{
    public partial class FrmPreviewNotarial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string s_Ruta = Convert.ToString(Request.QueryString["Ruta"]);
            string s_RutaCompleta = Convert.ToString(Request.QueryString["RutaNombre"]);
            
            if (s_RutaCompleta != null && s_RutaCompleta != "")
            {
                s_RutaCompleta = s_RutaCompleta.Replace("@", @"\");
                s_Ruta = s_RutaCompleta;
            }

            String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
            imgPicture.Visible = false;

            if (Session["strTipoArchivo"].Equals(".pdf"))
            {
                if (s_Ruta.Length > 0)
                {
                    //-------------------------------------------------------------------------
                    //Fecha: 24/01/2017
                    //Autor: Miguel Angel Márquez Beltrán
                    //Objetivo: Obtener el nombre de archivo PDF para leer del Disco
                    //-------------------------------------------------------------------------                    
                    string strAnio = s_Ruta.Substring(0, 4);
                    string strMes = s_Ruta.Substring(4, 2);
                    string strDia = s_Ruta.Substring(6, 2);
                    String[] strArchivo = s_Ruta.Split('_');
                    Int64 iActuacionDetalleId = Convert.ToInt64(strArchivo[2].ToString());
                    //--------------------------------------------
                    //Ejemplo: 20150506_10029_10048_798989092.pdf
                    //iActuacionDetalleId sería: 10048
                    //Referencia: ctrlUploader.ascx.cs linea 128
                    //          y Documento.cs linea: 141 
                    //          metodo: GetUniqueUploadFileNamePDF.
                    //--------------------------------------------
                    string strMision = Documento.GetMisionActuacionDetalle(iActuacionDetalleId);
                    string strFilePath;
                    if (s_RutaCompleta != null)
                    {
                        strFilePath = Path.Combine(uploadPath, s_Ruta);
                    }
                    else {
                         strFilePath = Path.Combine(uploadPath, strMision, strAnio, strMes, strDia, s_Ruta);
                    }
                    

                    if (!File.Exists(strFilePath))
                    {
                        strFilePath = uploadPath + "\\" + s_Ruta;
                    }

                    Response.Clear();
                    Response.ContentType = "application/" + s_Ruta.Substring(s_Ruta.Length - 3, 3);
                    Response.WriteFile(strFilePath);
                    Response.End();
                    //-------------------------------------------------------------------------                    
                }
            }
            else if (Session["strTipoArchivo"].Equals(".jpg"))
            {
                imgPicture.Visible = true;
                s_Ruta = s_Ruta.Replace(".jpg?", ".jpg");
                imgPicture.ImageUrl = string.Format("~/LoadImagen.ashx?vClass={0}", s_Ruta);
            }
        }
    }
}
