using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolCARDIP_REGLINEA.UserControl
{
    public partial class cuUploadFile : System.Web.UI.UserControl
    {
        private string _fileName;
        public string FileName
        {
            get { return fileupload.FileName; }
            set { _fileName = value; }
        }

        public bool HasFile
        {
            get { return fileupload.HasFile; }
        }
        public string ContentType
        {
            get { return fileupload.PostedFile.ContentType; }
        }

        public double ContentLength
        {
            get { return fileupload.PostedFile.ContentLength; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void SaveAs(string ruta)
        {
            fileupload.SaveAs(ruta);
        }
    }
}