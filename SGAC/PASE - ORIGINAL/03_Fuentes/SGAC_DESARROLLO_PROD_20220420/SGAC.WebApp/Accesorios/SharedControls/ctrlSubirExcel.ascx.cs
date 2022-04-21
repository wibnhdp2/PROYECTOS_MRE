using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;

namespace SGAC.WebApp.Accesorios.SharedControls
{
    public partial class ctrlSubirExcel : System.Web.UI.UserControl
    {
        public delegate void OnUploadButtonClick();
        public event OnUploadButtonClick btnUploadButtonHandler;

        public delegate void OnUploadButtonInicioClick();
        public event OnUploadButtonInicioClick btnUploadButtonInicioHandler;

        string Texto = String.Empty;
        public string NombreArchivo
        {
            get { return Path.GetFileName(FileUploadControl.PostedFile.FileName); }
        }
        public string RutaArchivo
        {
            get { return hRutaArchivo.Value; }
        }
        public string Extension
        {
            get { return Path.GetExtension(FileUploadControl.PostedFile.FileName); }
        }
        public string TextoResultado
        {
            get { return StatusLabel.Text; }
            set { StatusLabel.Text = Texto; }
        }
        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (btnUploadButtonInicioHandler != null)
            {
                btnUploadButtonInicioHandler();
            }
           
                if (FileUploadControl.HasFile)
                {
                    try
                    {
                        if (FileUploadControl.PostedFile.ContentType == "application/vnd.ms-excel" || FileUploadControl.PostedFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        {
                            if (FileUploadControl.PostedFile.ContentLength < 5120000)
                            {
                                string filename = GenerarCodigo();
                                filename = filename + Path.GetExtension(FileUploadControl.FileName);
                                string FolderPath = ConfigurationManager.AppSettings["UploadPath"];
                                string FilePath;
                                FilePath = FolderPath + @"\" + filename;
                                hRutaArchivo.Value = FilePath;
                                FileUploadControl.SaveAs(FilePath);
                                StatusLabel.Text = "El archivo fue cargado correctamente!";
                            }
                            else
                                StatusLabel.Text = "El archivo debe pesar máximo 5 Mb!";
                        }
                        else
                            StatusLabel.Text = "Solo se aceptan archivos EXCEL!";
                    }
                    catch (Exception ex)
                    {
                        StatusLabel.Text = "Ha ocurrido un error inesperado: " + ex.Message;
                    }

                    
                    if (btnUploadButtonHandler != null)
                    {
                        btnUploadButtonHandler();
                    }
                }
            
        }
        public void EliminarExcel(string FilePath)
        {
            File.Delete(FilePath);
        }

        private string GenerarCodigo()
        {
            System.Random obj = new System.Random();
            string sCadena = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            int longitud = sCadena.Length;
            char cletra;
            int nlongitud = 10;
            string sNuevacadena = string.Empty;
            for (int i = 0; i < nlongitud; i++)
            {
                cletra = sCadena[obj.Next(longitud)];
                sNuevacadena += cletra.ToString();
            }
            return sNuevacadena;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            StatusLabel.Visible = false;
        }

    }
}