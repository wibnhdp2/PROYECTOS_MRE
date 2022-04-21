using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace SGAC.Accesorios
{
    public class Descarga
    {
        public void Download(string pstrFileSource, string pstrFileTarget, bool bolIndicador)
        {
            try
            {
                string ext = Path.GetExtension(pstrFileSource);
                string type = "";
                // set known types based on file extension  
                if (ext != null)
                {
                    switch (ext.ToLower())
                    {
                        case ".htm":
                        case ".html":
                            type = "text/HTML";
                            break;

                        case ".txt":
                            type = "text/plain";
                            break;

                        case ".doc":
                        case ".rtf":
                            type = "Application/msword";
                            break;

                        case ".pdf":
                            type = "application/pdf";
                            break;
                        case ".xls":
                            type = "application/vnd.ms-excel";
                            break;
                    }
                }

                HttpContext.Current.Response.AppendHeader("Pragma", "no-cache");
                HttpContext.Current.Response.AppendHeader("Cache-Control", "no-cache");
                HttpContext.Current.Response.AppendHeader("max-age", "0");
                /*************************/
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = type;
                HttpContext.Current.Response.BufferOutput = true;
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + pstrFileTarget);

                FileStream fs = new FileStream(pstrFileSource, FileMode.Open, FileAccess.Read);
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                fs.Close();
                fs = null;

                HttpContext.Current.Response.OutputStream.Write(data, 0, data.Length);
                HttpContext.Current.Response.OutputStream.Flush();
                HttpContext.Current.Response.OutputStream.Close();
                HttpContext.Current.Response.Flush();

                if (bolIndicador)
                {
                    HttpContext.Current.Response.Close();
                    HttpContext.Current.Response.End();
                }
            }
            catch (Exception ex)
            {
                throw new SGACExcepcion(ex.Message);
            }
        }
    }
}
