using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;

namespace SGAC.WebApp.Accesorios
{
    public partial class VisPDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
              if (Session["rutaPDF"] != null)
                {
                    
                    string srutaPDF = Session["rutaPDF"].ToString();

                    //------------------------------
                    MemoryStream msPDF = new MemoryStream();
                    msPDF = ObtieneMemoryStreamDeFile(srutaPDF);
                    //------------------------------
                    byte[] byteImage = ObtieneBytesDeStream(msPDF);

                    Session["rutaPDF"] = null;
                   
                  /*****************************/
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.Clear();                    
                    Response.ClearHeaders();
                    Response.AppendHeader("Pragma", "no-cache");
                    Response.AppendHeader("Cache-Control", "no-cache");
                    Response.AppendHeader("max-age", "0");
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment;filename=Documento.pdf");
                    

                    Response.BinaryWrite(byteImage);
                    Response.End();
                    
                    if (File.Exists(srutaPDF))
                    {
                        File.Delete(srutaPDF);
                    }
                    
                }           
        }

        public MemoryStream ObtieneMemoryStreamDeFile(string strFile)
        {
            MemoryStream msPDFFirmado = new MemoryStream();

            using (FileStream source = File.Open(@strFile, FileMode.Open))
            {
                source.CopyTo(msPDFFirmado);
            }
            return msPDFFirmado;
        }
        public byte[] ObtieneBytesDeStream(Stream input)
        {
            input.Position = 0;
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}