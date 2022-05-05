using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.ComponentModel;
using SGAC.Accesorios;

namespace SGAC.WebApp.Accesorios.SharedControls
{
    public partial class ctrlUploader : System.Web.UI.UserControl
    {
        public event EventHandler Click;

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

        private int _fileSize = Constantes.CONST_TAMANIO_MAX_ADJUNTO_KB;
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

        private string _RutafileName;
        public string RUTAFileName
        {
            get { return hRutaArchivoNuevo.Value; }
            set { hRutaArchivoNuevo.Value = value; }
        }
        

        private string _fileExtension = ".pdf";
        public string FileExtension
        {
            get { return _fileExtension; }
            set { _fileExtension = value; hd_Extension.Value = _fileExtension; }
        }
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                msjeWarning.Visible = false;
                msjeError.Visible = false;
                msjeSucess.Visible = false;
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                subirArchivo(hd_Extension.Value.ToString());
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The operation is not valid for the state") || ex.ToString().Contains("La operación no es válida para el estado"))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", "HTTP 404 - intentelo de nuevo", true);
                }
                else
                {
                    Session["_LastException"] = ex;
                    Response.Redirect("../PageError/GenericErrorPage.aspx");
                }
            }

        }

        private void subirArchivo(string tipoArchivo)
        {
            try
            {
                string localfilepath = "";
                string uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
                string uploadFileName = "";
                lblNombreArchivo.Text = "";

                if (FileUploader.HasFile)
                {
                    localfilepath = FileUploader.FileName;
                    _fileName = Path.GetFileName(localfilepath);
                    String extension = System.IO.Path.GetExtension(_fileName);
                    int fileSizeInBytes = FileUploader.PostedFile.ContentLength;
                    int fileSizeInKB = fileSizeInBytes / Constantes.CONST_TAMANIO_MAX_ADJUNTO_KB;

                    if (Constantes.CONST_TAMANIO_MAX_ADJUNTO_KB >= fileSizeInKB)
                    {
                        if (tipoArchivo.ToUpper() == extension.ToUpper())
                        {
                            string strMensaje = string.Empty;
                            if (Directory.Exists(uploadPath))
                            {
                            }
                            else
                            {
                                try
                                {
                                    Directory.CreateDirectory(uploadPath);
                                }
                                catch
                                {
                                    strMensaje = "No se encuentra o no existe el directorio de Adjuntos.";
                                }
                            }

                            //-------------------------------------------------------------------------
                            //Fecha: 24/01/2017
                            //Autor: Miguel Angel Márquez Beltrán
                            //Objetivo: Obtener el nombre de archivo PDF para guardar en el Disco
                            //-------------------------------------------------------------------------

                            if (tipoArchivo.ToUpper().Equals(".PDF"))
                            {
                                uploadFileName = Documento.GetUniqueUploadFileNamePDF(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]), uploadPath, ref _fileName, ref _RutafileName);
                                hRutaArchivoNuevo.Value = _RutafileName;
                                //-------------------------------------------------------------------------
                            }
                            else
                            {
                                uploadFileName = Documento.GetUniqueUploadFileName(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]), uploadPath, ref _fileName);
                                hRutaArchivoNuevo.Value = "";
                            }

                                

                            try
                            {
                                FileUploader.SaveAs(uploadFileName);
                                if (hRutaArchivoNuevo.Value != null && hRutaArchivoNuevo.Value != "")
                                {
                                    lblNombreArchivo.Text = hRutaArchivoNuevo.Value.ToString();
                                }
                                else{
                                    lblNombreArchivo.Text = _fileName;
                                }
                                
                                lblMsjeSucess.Text = System.Configuration.ConfigurationManager.AppSettings["UploadSucessMsje"];
                                msjeSucess.Visible = true;
                                msjeError.Visible = false;
                                msjeWarning.Visible = false;
                            }
                            catch
                            {
                                lblMsjeError.Text = strMensaje;
                                msjeError.Visible = true;
                                msjeSucess.Visible = false;
                                msjeWarning.Visible = false;
                            }
                        }
                        else
                        {
                            lblMsjeWarnig.Text = System.Configuration.ConfigurationManager.AppSettings["UploadInvalidFileMsje"];
                            msjeWarning.Visible = true;
                            msjeError.Visible = false;
                            msjeSucess.Visible = false;
                        }
                    }
                    else
                    {
                        lblMsjeWarnig.Text = System.Configuration.ConfigurationManager.AppSettings["UploadInvalidSizeMsje"];
                        msjeWarning.Visible = true;
                        msjeError.Visible = false;
                        msjeSucess.Visible = false;
                    }
                }
                else
                {
                    lblMsjeWarnig.Text = System.Configuration.ConfigurationManager.AppSettings["UploadWarningMsje"];
                    msjeWarning.Visible = true;
                    msjeError.Visible = false;
                    msjeSucess.Visible = false;
                }

                if (Click != null)
                {
                    Click(this, EventArgs.Empty);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Metodos
        public void LimpiarMensaje()
        {
            lblMsjeError.Text = string.Empty;
            lblMsjeWarnig.Text = string.Empty;
            lblMsjeSucess.Text = string.Empty;

            msjeError.Visible = false;
            msjeSucess.Visible = false;
            msjeWarning.Visible = false;

            lblNombreArchivo.Text = "";
        }

        public void LimpiarControl()
        {
            lblNombreArchivo.Text = "";
            lblNombreArchivo.Visible = false;

            System.Web.UI.HtmlControls.HtmlTableCell msjeSucess = (System.Web.UI.HtmlControls.HtmlTableCell)this.FindControl("msjeSucess");
            msjeSucess.Visible = false;

            System.Web.UI.HtmlControls.HtmlTableCell msjeWarning = (System.Web.UI.HtmlControls.HtmlTableCell)this.FindControl("msjeWarning");
            msjeWarning.Visible = false;

            System.Web.UI.HtmlControls.HtmlTableCell msjeError = (System.Web.UI.HtmlControls.HtmlTableCell)this.FindControl("msjeError");
            msjeError.Visible = false;
        }

        private string GetUniqueUploadFileName(string uploadPath, string fileName)
        {
            try
            {
                string filepath = uploadPath + "/" + fileName;
                string fileext = Path.GetExtension(filepath);
                string filenamewithoutext = Path.GetFileNameWithoutExtension(filepath);

                do
                {
                    Random rnd = new Random();
                    int temp = rnd.Next(1000, 1000000);
                    filenamewithoutext += "_" + temp;
                    fileName = filenamewithoutext + fileext;
                    filepath = uploadPath + "/" + fileName;

                } while (File.Exists(filepath));

                _fileName = fileName;

                return filepath;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion


        

     
    }
}
