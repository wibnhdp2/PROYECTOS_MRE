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
    public partial class VistaPreviaPDF : System.Web.UI.Page
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
                //Response.Clear();
                //Response.ClearContent();
                //Response.ClearHeaders();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "inline;filename=Documento.pdf");
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.BinaryWrite(byteImage);
                //Response.TransmitFile(srutaPDF);
                //Response.WriteFile(srutaPDF);
                //Response.Flush();

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