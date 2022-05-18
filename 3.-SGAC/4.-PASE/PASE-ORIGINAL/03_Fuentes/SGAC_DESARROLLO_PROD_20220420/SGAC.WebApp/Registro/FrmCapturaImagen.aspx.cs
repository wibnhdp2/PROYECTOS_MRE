using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Services;
using System.Drawing;
using SGAC.WebApp.Registro;

namespace SGAC.WebApp.Registro
{

    public partial class FrmCapturaImagen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (Request.InputStream.Length > 0)
                {
                    using (StreamReader reader = new StreamReader(Request.InputStream))
                    {
                        if ((string) (Session["Nombrearchivodevuelto"]) != null)
                        {
                            System.IO.File.Delete(
                                Server.MapPath(@"\Registro\" + (string) (Session["Nombrearchivodevuelto"])));
                            (Session["Nombrearchivodevuelto"]) = null;
                        }

                        if ((string) (Session["Nombrearchivodevuelto"]) == null)
                        {
                            string hexString = Server.UrlEncode(reader.ReadToEnd());
                            string imageName = DateTime.Now.ToString("dd-MM-yy hh-mm-ss");
                            string imagePath = string.Format("{0}.jpg", imageName);
                            File.WriteAllBytes(Server.MapPath(imagePath), ConvertHexToBytes(hexString));
                            Session["CapturedImage"] = ResolveUrl(imagePath);
                            Session["Nombrearchivo"] = imagePath;
                            Session["Nombrearchivodevuelto"] = imagePath;
                        }
                    }
                }
            }
        }

        private static byte[] ConvertHexToBytes(string hex)
        {
            byte[] bytes = new byte[hex.Length/2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i/2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        private string hex2binary(string hexvalue)
        {
            string binaryval = "";
            binaryval = Convert.ToString(Convert.ToInt32(hexvalue, 16), 2);
            return binaryval;
        }

        [WebMethod(EnableSession = true)]
        public static string GetCapturedImage()
        {
            string url = HttpContext.Current.Session["CapturedImage"].ToString();
            return url;
        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            SGAC.WebApp.Accesorios.Comun.EjecutarScript(Page, "window.close();");
            Session["CapturedImageDevuelta"] = (string) Session["Nombrearchivodevuelto"];


        }

        protected void btnCapture_Click(object sender, EventArgs e)
        {
            string campo = string.Empty;
            string campo2 = string.Empty;

            string script = @"$(function(){{
                                SupplierSelected('{0}','{1}');
                            }});";

            script = string.Format(script, campo, campo2);
            ScriptManager.RegisterStartupScript(Page, typeof (Page), "popupclose", script, true);
        }
    }
}