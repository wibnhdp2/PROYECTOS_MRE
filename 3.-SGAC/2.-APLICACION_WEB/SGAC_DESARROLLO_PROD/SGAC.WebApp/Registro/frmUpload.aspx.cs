using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace SGAC.WebApp.Registro
{
    public partial class frmUpload : System.Web.UI.Page
    {
        #region Properties
        private int _height = 22;
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        private int _width = 287;
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private int _fileSize = 50024;
        public int FileSize
        {
            get { return _fileSize; }
            set { _fileSize = value; }
        }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        private string _fileExtension = ".pdf";
        public string FileExtension
        {
            get { return _fileExtension; }
            set { _fileExtension = value; }
        }
        #endregion
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

            }
        }

        protected void btnSubir_Click(object sender, EventArgs e)
        {
            string localfilepath = "";
            string uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
            string uploadFileName = "";

            if (FileUploader.HasFile)
            {
                localfilepath = FileUploader.FileName;
                _fileName = Path.GetFileName(localfilepath);
                Session["nombreArchivo"] = _fileName;

                String extension = System.IO.Path.GetExtension(_fileName);
                int fileSize = FileUploader.PostedFile.ContentLength;

                if (FileSize >= fileSize)
                {
                    if (FileExtension == extension)
                    {
                        uploadFileName = GetUniqueUploadFileName(uploadPath, _fileName);

                        try
                        {
                            FileUploader.SaveAs(uploadFileName);
                            lblMsjeSucess.Text = System.Configuration.ConfigurationManager.AppSettings["UploadSucessMsje"];
                            //msjeSucess.Visible = true;
                            //msjeError.Visible = false;
                            //msjeWarning.Visible = false;
                        }
                        catch
                        {
                            lblMsjeError.Text = System.Configuration.ConfigurationManager.AppSettings["UploadErrorMsje"];
                            //msjeError.Visible = true;
                            //msjeSucess.Visible = false;
                            //msjeWarning.Visible = false;
                        }
                    }
                    else
                    {
                        lblMsjeWarnig.Text = System.Configuration.ConfigurationManager.AppSettings["UploadInvalidFileMsje"];
                        //msjeWarning.Visible = true;
                        //msjeError.Visible = false;
                        //msjeSucess.Visible = false;
                    }
                }
                else
                {
                    lblMsjeWarnig.Text = System.Configuration.ConfigurationManager.AppSettings["UploadInvalidSizeMsje"];
                    //msjeWarning.Visible = true;
                    //msjeError.Visible = false;
                    //msjeSucess.Visible = false;
                }
            }
            else
            {
                lblMsjeWarnig.Text = System.Configuration.ConfigurationManager.AppSettings["UploadWarningMsje"];

                return;
                //msjeWarning.Visible = true;
                //msjeError.Visible = false;
                //msjeSucess.Visible = false;
            }





            string strScript = " window.close();";
            ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), "OpenPopup", strScript, true);

            //strScript = "window.opener = ~/Registro/FrmRegistroAsistencia.aspx; ";
            //ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), "OpenPopup", strScript, true);





        }

        public static void EjecutarScript(Page Page, string strScript)
        {
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), "OpenPopup", strScript, true);
        }

        #endregion

        #region Metodos
        private string GetUniqueUploadFileName(string uploadPath, string fileName)
        {
            string filepath = uploadPath + "/" + fileName;
            string fileext = Path.GetExtension(filepath);
            string filenamewithoutext = Path.GetFileNameWithoutExtension(filepath);

            do
            {
                Random rnd = new Random();
                int temp = rnd.Next(1000, 1000000);
                //filenamewithoutext += "_" + temp;
                fileName = filenamewithoutext + fileext;
                filepath = uploadPath + "/" + fileName;

            } while (File.Exists(filepath));

            return filepath;
        }
        #endregion
        /*
        protected void btnSubir_Click(object sender, EventArgs e)
        {
            if (fulDocumento.HasFile)
            {
                try
                {
                    //if (fileupload1.PostedFile.ContentType == "pdf")
                    //{
                    //fulDocumento2 = (Control)hd_file;

                    if (fulDocumento.PostedFile.ContentLength < 512000)
                    {
                        string filename = Path.GetFileName(fulDocumento.FileName);
                        fulDocumento.SaveAs(Server.MapPath("~/") + filename);
                        Label1.Text = "File uploaded successfully!";
                    }
                    else
                        Label1.Text = "File maximum size is 500 Kb";
                    //}
                    //else
                    //    Label1.Text = "Only JPEG files are accepted!";
                }
                catch (Exception exc)
                {
                    Label1.Text = "The file could not be uploaded. The following error occured: " + exc.Message;
                }
            }
        }

        */

    }
}