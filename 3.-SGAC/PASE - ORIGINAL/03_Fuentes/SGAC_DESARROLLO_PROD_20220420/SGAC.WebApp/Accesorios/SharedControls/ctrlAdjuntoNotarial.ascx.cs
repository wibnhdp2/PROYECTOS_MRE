using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.Globalization;
using System.Threading;
using SGAC.WebApp.Accesorios;
using SGAC.Registro.Actuacion.BL;
using SGAC.Registro.Persona.BL;
using SGAC.Configuracion.Sistema.BL;
using SGAC.Controlador;
using SGAC.BE.MRE;
using SGAC.Accesorios;
using System.Text;
using System.IO;
using System.Configuration;
using System.Web.Configuration;
using System.Net;
using AjaxControlToolkit;


namespace SGAC.WebApp.Accesorios.SharedControls
{
    public partial class ctrlAdjuntoNotarial : System.Web.UI.UserControl
    {

        public event EventHandler Click;
        private string strVariableAccion = "Actuacion_Accion";

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

        private string _fileExtension = ".pdf";
        public string FileExtension
        {
            get { return _fileExtension; }
            set { _fileExtension = value; hd_Extension.Value = _fileExtension; }
        }

        private bool _bMoverTabIndex = true;
        public bool bMoverTabIndex
        {
            get { return _bMoverTabIndex; }
            set { _bMoverTabIndex = value; }
        }
       
        #endregion



        protected void Page_Load(object sender, EventArgs e)
        {

            lblTamanioMax.Text = Constantes.CONST_TAMANIO_MAX_ADJUNTO_KB.ToString();

            BtnGrabActAdj.OnClientClick = "return ValidarRegistroAdjunto()";            
            if (!Page.IsPostBack)
            {
                msjeWarning.Visible = false;
                msjeError.Visible = false;
                msjeSucess.Visible = false;

                Session["iOperAdj"] = true;
                ValidarSession();
                CargarTipoArchivo();
            }
        }

        #region Eventos
        protected void BtnGrabActAdj_Click(object sender, EventArgs e)
        {
            string StrScript = string.Empty;
            if (Convert.ToInt64(Session[Constantes.CONST_SESION_ACTONOTARIAL_ID]) == 0)
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuación", "Debe crear la actuación antes de adjuntar.");
                Comun.EjecutarScript(Page, StrScript);
                return;
            }

        
            BtnGrabActAdj.Enabled = false;
            if (CargarArchivo())
            {

                //RE_ACTUACIONADJUNTO ObjAdjActBE = new RE_ACTUACIONADJUNTO();
                RE_ACTONOTARIALDOCUMENTO ObjAdjActBE = new RE_ACTONOTARIALDOCUMENTO();
                Proceso MiProc = new Proceso();
                Enumerador.enmAccion enmAccion = Enumerador.enmAccion.INSERTAR;

                int IntRpta = 0;

                ObjAdjActBE.ando_iActoNotarialId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTONOTARIAL_ID]);
                ObjAdjActBE.ando_iActoNotarialDocumentoId = 0; 
                ObjAdjActBE.ando_sTipoDocumentoId = Convert.ToInt16(cmb_TipoArchivo.SelectedValue);

                ObjAdjActBE.ando_sTipoInformacionId = 8101;
                ObjAdjActBE.ando_sSubTipoInformacionId = 8151;
                ObjAdjActBE.ando_vRutaArchivo = lblNombreArchivo.Text;

                //ObjAdjActBE.acad_vNombreArchivo = lblNombreArchivo.Text;
                ObjAdjActBE.ando_vDescripcion = Util.ReemplazarCaracter(txtDescAdj.Text.Trim().ToUpper());
                if (Convert.ToBoolean(Session["iOperAdj"]))
                {
                    ObjAdjActBE.ando_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    ObjAdjActBE.ando_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    ObjAdjActBE.ando_dFechaCreacion = DateTime.Now;
                    ObjAdjActBE.ando_cEstado = "A";
                    enmAccion = Enumerador.enmAccion.INSERTAR;
                }
                else
                {
                    //este falta
                    //ObjAdjActBE.ando_iActoNotarialDocumentoId acad_iActuacionAdjuntoId = Convert.ToInt32(Session["IActuacionAdjuntoId"]);
                    ObjAdjActBE.ando_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    ObjAdjActBE.ando_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    ObjAdjActBE.ando_dFechaModificacion = DateTime.Now;
                    ObjAdjActBE.ando_vDescripcion = lblNombreArchivo.Text;
                    enmAccion = Enumerador.enmAccion.MODIFICAR;
                }

                ActoNotarialMantenimiento objActoNotarialBL = new ActoNotarialMantenimiento();
                BE.MRE.RE_ACTONOTARIALDOCUMENTO objDoumento = objActoNotarialBL.InsertarActoNotarialDocumento_2(ObjAdjActBE);

                if (objDoumento.ando_iActoNotarialDocumentoId == 0)
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuación", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }
                else
                {
                    
                    
                    if (ObjAdjActBE.ando_sTipoDocumentoId == (int)Enumerador.enmTipoAdjunto.DOCUMENTO_DIGITALIZA)
                    {
                        Grd_Archivos.Enabled = false;
                        BtnGrabActAdj.Enabled = false;
                    }
                }

                

                Session["iOperAdj"] = true;

                //cmb_TipoArchivo.SelectedIndex = 0;
                txtDescAdj.Text = "";

                hidNomAdjFile.Value = "";

                CargarGrillaAdjuntos();

                lblNombreArchivo.Text = String.Empty;
                //updActuacionAdjuntar.Update();
                if (ArchivoAdjuntado != null)
                    OnArchivoAdjuntado(null);
            }

         

            BtnGrabActAdj.Enabled = true;
        }

        protected void BtnLimpiaAdj_Click(object sender, EventArgs e)
        {

            LimpiarMensaje();
            String NombreArchivo = lblNombreArchivo.Text;
            lblNombreArchivo.Text = String.Empty;
            txtDescAdj.Text = String.Empty;
            //cmb_TipoArchivo.SelectedIndex = -1;
            String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
            String uploadFileName = String.Empty;

            Session["iOperAdj"] = true;

            String strMensaje = String.Empty;

            if (Directory.Exists(uploadPath))
            {

            }
            else
            {
                try
                {
                    // Crear carpeta
                    Directory.CreateDirectory(uploadPath);
                }
                catch
                {
                    strMensaje = "No se encuentra o no existe el directorio de Adjuntos.";
                }
            }
            uploadFileName = GetUniqueUploadFileName(uploadPath, NombreArchivo);

            if (System.IO.File.Exists(@uploadFileName))
            {
                try
                {
                    System.IO.File.Delete(@uploadFileName);
                }
                catch (System.IO.IOException ex)
                {
                    return;
                }
                finally
                {
                }
            }

            if (Convert.ToInt32(Session["Actuacion_Accion"]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                BtnGrabActAdj.Enabled = false;
            else
                BtnGrabActAdj.Enabled = true;
        }

        public event EventHandler ArchivoAdjuntado;

        // Invoke the Changed event; called whenever list changes
        protected virtual void OnArchivoAdjuntado(EventArgs e)
        {
            if (ArchivoAdjuntado != null)
                ArchivoAdjuntado(this, e);
        }


        protected void cmb_TipoArchivo_prueba_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmb_TipoArchivo.SelectedValue) == (int)Enumerador.enmTipoAdjunto.DOCUMENTO_DIGITALIZA)
            {
            }

            string x = cmb_TipoArchivo.SelectedItem.Text;

            hidNomAdjFile.Value = x;
            String[] Ext = cmb_TipoArchivo.SelectedItem.Text.Split('|');
            lblNombreArchivo.Text = x;
            hd_Extension.Value = Ext[1].ToString();

        }

        Descarga objDescarga;
        protected void Grd_Archivos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            string StrScript = string.Empty;
            int Index = Convert.ToInt32(e.CommandArgument);

            String scriptMover = String.Empty;

            if (_bMoverTabIndex)
            {
                // tab en el que se encuentra ADJUNTOS
                scriptMover = @"$(function(){{ MoveTabIndex(6);}});";
                scriptMover = string.Format(scriptMover);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);
            }

            Session["iOperAdj"] = false;
            if (e.CommandName == "Imprimir")
            {
                LimpiarMensaje();

                string strNombreArchivo = Convert.ToString(Grd_Archivos.Rows[Index].Cells[6].Text);
                string strRuta = "";
                //-------------------------------------------------------------------------
                //Fecha: 24/01/2017
                //Autor: Miguel Angel Márquez Beltrán
                //Objetivo: Obtener el nombre de archivo PDF para guardar en el Disco
                //-------------------------------------------------------------------------
                string ext = Path.GetExtension(strNombreArchivo).ToUpper();
                string struploadPath = ConfigurationManager.AppSettings["UploadPath"].ToString();
                if (ext.Equals(".PDF"))
                {
                    string strMision = Convert.ToString(Grd_Archivos.Rows[Index].Cells[8].Text);
                    string stryyyymmdd = Convert.ToString(Grd_Archivos.Rows[Index].Cells[9].Text);

                    string strAnio = stryyyymmdd.Substring(0, 4);
                    string strMes = stryyyymmdd.Substring(4, 2);
                    string strDia = stryyyymmdd.Substring(6, 2);
                    
                    string strpathMision = Path.Combine(struploadPath, strMision);
                    string strpathAnio = Path.Combine(strpathMision, strAnio);
                    string strpathAnioMes = Path.Combine(strpathAnio, strMes);
                    string strpathAnioMesDia = Path.Combine(strpathAnioMes, strDia);

                    if (!Directory.Exists(strpathMision))
                    {
                        Directory.CreateDirectory(strpathMision);
                    }
                    if (!Directory.Exists(strpathAnio))
                    {
                        Directory.CreateDirectory(strpathAnio);
                    }
                    if (!Directory.Exists(strpathAnioMes))
                    {
                        Directory.CreateDirectory(strpathAnioMes);
                    }
                    if (!Directory.Exists(strpathAnioMesDia))
                    {
                        Directory.CreateDirectory(strpathAnioMesDia);
                    }

                    string strfilePath = Path.Combine(strpathAnioMesDia, strNombreArchivo);

                    if (File.Exists(strfilePath))
                    {
                        strRuta = strfilePath;
                    }
                    else
                    {
                        strRuta = Path.Combine(struploadPath, strNombreArchivo);
                    }
                }
                else
                {
                    strRuta = Path.Combine(struploadPath, strNombreArchivo);
                }
                //-------------------------------------------------------------------------                
                string strScript = string.Empty;
                if (File.Exists(strRuta))
                {
                    try
                    {
                        objDescarga = new Descarga();
                        objDescarga.Download(strRuta, strNombreArchivo, false);
                        objDescarga = null;
                    }
                    catch (Exception ex)
                    {
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTO",
                            "El archivo no se pudo abrir. Vuelva a intentarlo." +
                            "\n(" + ex.Message + ")");
                        Comun.EjecutarScript(Page, strScript);
                    }
                }
               
            }
            else if (e.CommandName == "Editar")
            {
                LimpiarMensaje();
                Session["ando_iActoNotarialDocumentoId"] = Convert.ToString(Grd_Archivos.Rows[Index].Cells[0].Text);
                cmb_TipoArchivo.SelectedValue = Convert.ToString(Grd_Archivos.Rows[Index].Cells[2].Text);
                //cmb_TipoArchivo.SelectedItem.Text = Convert.ToString(Grd_Archivos.Rows[Index].Cells[3].Text);
                hidNomAdjFile.Value = Convert.ToString(Grd_Archivos.Rows[Index].Cells[4].Text);
                lblNombreArchivo.Text = Convert.ToString(Grd_Archivos.Rows[Index].Cells[6].Text);
                txtDescAdj.Text = Page.Server.HtmlDecode(Convert.ToString(Grd_Archivos.Rows[Index].Cells[7].Text));
            }
            else if (e.CommandName == "Eliminar")
            {
                LimpiarMensaje();
                lblNombreArchivo.Text = String.Empty;

                BE.MRE.RE_ACTONOTARIALDOCUMENTO actoNotarialDocumento = new RE_ACTONOTARIALDOCUMENTO();
                actoNotarialDocumento.ando_iActoNotarialDocumentoId = Convert.ToInt64(Convert.ToString(Grd_Archivos.Rows[Index].Cells[0].Text));
                actoNotarialDocumento.ando_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                actoNotarialDocumento.ando_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                actoNotarialDocumento.ando_dFechaModificacion = DateTime.Now;

                ActoNotarialMantenimiento objBL = new ActoNotarialMantenimiento();
                int intResultado = objBL.EliminarDocumento(actoNotarialDocumento);

                if (intResultado > 0)
                {
                    Session["iOperAdj"] = true;
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "ADJUNTOS", "Se ha eliminado el adjunto.", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    CargarGrillaAdjuntos();
                }
                else
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTOS", "Error. No se pudo realizar la operación", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }                
            }
        }

        protected void ctrlPagActuacionAdjuntos_Click(object sender, EventArgs e)
        {
            LimpiarMensaje();
            CargarGrillaAdjuntos();
        }
        protected void cmb_TipoArchivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            String strScript = String.Empty;
            try
            {
                if (Convert.ToInt32(cmb_TipoArchivo.SelectedValue) == (int)Enumerador.enmTipoAdjunto.DOCUMENTO_DIGITALIZA)
                {
                    string strCodigo = hifCodVinculado.Value;
                    if (strCodigo == string.Empty)
                        strCodigo = Session["COD_AUTOADHESIVO"].ToString();

                    if (strCodigo != string.Empty)
                    {
                        LimpiarMensaje();
                        BtnGrabActAdj.Enabled = false;
                    }
                    else
                    {
                        BtnGrabActAdj.Enabled = false;
                        //cmb_TipoArchivo.SelectedIndex = 0;
                        strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", "Debe Realizar la Vinculación antes de la Digitalización", false, 200, 400);
                        Comun.EjecutarScript(Page, strScript);
                    }
                }
                else
                {
                    BtnGrabActAdj.Enabled = true;
                    LimpiarMensaje();
                }

                if (cmb_TipoArchivo.SelectedIndex > 0)
                {
                    string x = cmb_TipoArchivo.SelectedItem.Text;
                    hidNomAdjFile.Value = x;
                    String[] Ext = cmb_TipoArchivo.SelectedItem.Text.Split('|');
                    lblNombreArchivo.Text = String.Empty;
                    hd_Extension.Value = Ext[1].ToString();
                }

                if (Convert.ToInt32(Session["Actuacion_Accion"]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                    BtnGrabActAdj.Enabled = false;
                else
                    BtnGrabActAdj.Enabled = true;
            }
            catch (Exception ex)
            {
                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", Constantes.CONST_MENSAJE_OPERACION_FALLIDA);
                Comun.EjecutarScript(Page, strScript);
            }
        }

        protected void btnDigitalizar_Click(object sender, EventArgs e)
        {

            //String path = @"~\Accesorios\FrmDigitaliza.aspx";
            //Response.Write("<script language='JavaScript'>window.open('" + path + "','_new','width=350,height=150')</script>");

            string strUrl = "../Accesorios/FrmDigitaliza.aspx";
            string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=750,height=550,left=100,top=10');";
            ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), "OpenPopup", strScript, true);

        }

        #endregion

        #region Metodos
        private string GetUniqueUploadFileName(string uploadPath, string fileName)
        {
            String strScript = String.Empty;
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

                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", ex.Message, false, 200, 400);
                Comun.EjecutarScript(Page, strScript);
                return ex.Message;
            }
        }

        private void CargarGrillaAdjuntos()
        {
            long lngActoNotarialId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTONOTARIAL_ID]);

            Grd_Archivos.DataSource = null;
            Grd_Archivos.DataBind();

            DataTable dtAdjuntos = new DataTable();
            Proceso MiProc = new Proceso();

            if (Session[Constantes.CONST_SESION_ACTONOTARIAL_ID] == null)
                return;

            BE.MRE.RE_ACTONOTARIALDOCUMENTO documento = new RE_ACTONOTARIALDOCUMENTO();
            documento.ando_iActoNotarialId = Convert.ToInt64(lngActoNotarialId);

            ActoNotarialMantenimiento objActoNotarialBL = new ActoNotarialMantenimiento();
            dtAdjuntos = objActoNotarialBL.ObtenerListaDocumentos(documento);
            dtAdjuntos.DefaultView.RowFilter = string.Format("ando_sTipoDocumentoId = '4204'");

            if (dtAdjuntos.Rows.Count > 0)
            {
                Grd_Archivos.DataSource = dtAdjuntos;
                Grd_Archivos.DataBind();
            }
        }

        static bool IsPDFHeader(string fileName)
        {
            byte[] buffer = null;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(fileName).Length;
            //buffer = br.ReadBytes((int)numBytes);
            buffer = br.ReadBytes(5);

            var enc = new ASCIIEncoding();
            var header = enc.GetString(buffer);

            if (header == "") { }
            else
            {
                //%PDF−1.0
                // If you are loading it into a long, this is (0x04034b50).
                if (buffer[0] == 0x25 && buffer[1] == 0x50
                    && buffer[2] == 0x44 && buffer[3] == 0x46)
                {
                    return header.StartsWith("%PDF-");
                }
            }
            return false;

        }

        private void HabilitarCajas(Boolean bEstado)
        {
            txtDescAdj.Enabled = bEstado;
            FileUploader.Enabled = bEstado;
            BtnGrabActAdj.Enabled = bEstado;
        }

        private Boolean CargarArchivo()
        {
            Boolean Resultado = false;
            String strScript = String.Empty;
            try
            {
                String localfilepath = String.Empty;
                String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
                String uploadFileName = String.Empty;

                if (FileUploader.FileBytes.Length == 0)
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", "Error al Adjuntar, el archivo es invalido y/o no tiene contenido.", false, 200, 400);
                    Comun.EjecutarScript(Page, strScript);
                    return false;
                }


                if (FileUploader.HasFile)
                {
                    lblNombreArchivo.Text = "";
                    localfilepath = FileUploader.FileName;
                    _fileName = Path.GetFileName(localfilepath);


                    String caracteres = ConfigurationManager.AppSettings["validarchars"].ToString();
                    Int32 SizeFile = localfilepath.Length;

                    string[] caract = caracteres.Split(',');

                    foreach (string onjcaract in caract)
                    {
                        for (int i = 0; i < SizeFile; i++)
                        {
                            String var = String.Empty;
                            var = localfilepath.Substring(i, 1);
                            if (var == onjcaract)
                            {
                                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", "Error al Adjunto, el archivo contiene caracteres Invalidos", false, 200, 400);
                                Comun.EjecutarScript(Page, strScript);
                                return false;
                            }
                        }
                    }

                    #region Validar PDF
                    if (cmb_TipoArchivo.SelectedIndex > 0)
                    {
                        if (Convert.ToInt32(cmb_TipoArchivo.SelectedValue) == (int)Enumerador.enmTipoAdjunto.PDF)
                        {
                            //if (!IsPDFHeader(FileUploader.PostedFile.FileName))
                            //{
                            //    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", "Documento inválido y/o corrupto.");
                            //    Comun.EjecutarScript(Page, strScript);
                            //    return false;
                            //}
                        }
                    }
                    #endregion

                    String extension = System.IO.Path.GetExtension(_fileName);
                    int fileSizeInBytes = FileUploader.PostedFile.ContentLength;
                    int fileSizeInKB = fileSizeInBytes / Constantes.CONST_TAMANIO_MAX_ADJUNTO_KB;

                    if (Constantes.CONST_TAMANIO_MAX_ADJUNTO_KB >= fileSizeInKB)
                    {
                        if (hd_Extension.Value.ToUpper() == extension.ToUpper())
                        {
                            String strMensaje = String.Empty;

                            if (Directory.Exists(uploadPath)) { }
                            else
                            {
                                try
                                {
                                    // Crear carpeta
                                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Ruta del Archivo", uploadPath, false, 200, 400);
                                    Comun.EjecutarScript(Page, strScript);
                                    Directory.CreateDirectory(uploadPath);
                                }
                                catch
                                {
                                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", "No se encuentra o no existe el directorio de Adjuntos.", false, 200, 400);
                                    Comun.EjecutarScript(Page, strScript);
                                    return false;
                                }
                            }

                            uploadFileName = GetUniqueUploadFileName(uploadPath, _fileName);

                            try
                            {
                                FileUploader.SaveAs(uploadFileName);

                                lblNombreArchivo.Text = _fileName;
                                lblMsjeSucess.Text = System.Configuration.ConfigurationManager.AppSettings["UploadSucessMsje"];
                                msjeSucess.Visible = true;
                                msjeError.Visible = false;
                                msjeWarning.Visible = false;
                                Resultado = true;
                            }
                            catch (Exception ex)
                            {
                                lblMsjeError.Text = strMensaje;
                                msjeError.Visible = true;
                                msjeSucess.Visible = false;
                                msjeWarning.Visible = false;

                                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", ex.Message, false, 200, 400);
                                Comun.EjecutarScript(Page, strScript);

                                return false;
                            }
                        }
                        else
                        {
                            lblMsjeWarnig.Text = System.Configuration.ConfigurationManager.AppSettings["UploadInvalidFileMsje"];
                            msjeWarning.Visible = true;
                            msjeError.Visible = false;
                            msjeSucess.Visible = false;
                            Resultado = false;
                        }
                    }
                    else
                    {
                        lblMsjeWarnig.Text = System.Configuration.ConfigurationManager.AppSettings["UploadWarningMsje"];
                        msjeWarning.Visible = true;
                        msjeError.Visible = false;
                        msjeSucess.Visible = false;
                        Resultado = false;
                    }

                    //if (Click != null)
                    //{
                    //    Click(this, EventArgs.Empty);
                    //}
                }
                else
                {
                    if (Convert.ToBoolean(Session["iOperAdj"]) == false)
                    {
                        Resultado = true;
                    }
                    else
                    {
                        lblMsjeWarnig.Text = System.Configuration.ConfigurationManager.AppSettings["UploadWarningMsje"];
                        msjeWarning.Visible = true;
                        msjeError.Visible = false;
                        msjeSucess.Visible = false;
                        Resultado = false;
                    }
                }
                return Resultado;
            }
            catch (Exception ex)
            {
                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", ex.Message, false, 200, 400);
                Comun.EjecutarScript(Page, strScript);

                return false;
            }
        }
        #endregion

        #region Métodos Públicos
        public void LimpiarMensaje()
        {
            lblMsjeError.Text = string.Empty;
            lblMsjeWarnig.Text = string.Empty;
            lblMsjeSucess.Text = string.Empty;

            msjeError.Visible = false;
            msjeSucess.Visible = false;
            msjeWarning.Visible = false;

        }

        public void SetCodigoVinculacion(string strCodigo)
        {
            hifCodVinculado.Value = strCodigo;
        }

        public void ValidarSession()
        {
            if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
            {
                Grd_Archivos.Enabled = false;
            }
        }
        public void SetearComboTipoArchivo(Enumerador.enmTipoAdjunto tipoAdjunto )
        {
            cmb_TipoArchivo.SelectedValue = ((int)tipoAdjunto).ToString();
        }

        public void CargarTipoArchivo()
        {
            DataTable dt = new DataTable();
            dt = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTUACION_TIPO_ADJUNTO).Copy();
            
            Util.CargarParametroDropDownList(cmb_TipoArchivo, dt, false);
            cmb_TipoArchivo.SelectedValue = ((int)Enumerador.enmTipoAdjunto.DOCUMENTO_DIGITALIZA).ToString();

            string x = cmb_TipoArchivo.SelectedItem.Text;
            hidNomAdjFile.Value = x;
            String[] Ext = cmb_TipoArchivo.SelectedItem.Text.Split('|');
            lblNombreArchivo.Text = String.Empty;
            hd_Extension.Value = Ext[1].ToString();
        }

        public void HabilitarAdjuntos(bool bolHabilitar = true)
        {
            cmb_TipoArchivo.Enabled = true;
            BtnLimpiaAdj.Enabled = bolHabilitar;
            Grd_Archivos.DataSource = null;
            Grd_Archivos.DataBind();
            CargarTipoArchivo();            
        }
        #endregion
    }
}
