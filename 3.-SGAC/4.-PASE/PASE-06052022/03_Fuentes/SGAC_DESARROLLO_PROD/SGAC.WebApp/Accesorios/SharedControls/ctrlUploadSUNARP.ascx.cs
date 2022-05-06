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
using SGAC.BE;
using SGAC.Accesorios;
using System.Text;
using System.IO;
using System.Configuration;
using System.Web.Configuration;
using System.Net;
using AjaxControlToolkit;
using iTextSharp.text.pdf;

namespace SGAC.WebApp.Accesorios.SharedControls
{
    public partial class ctrlUploadSUNARP : System.Web.UI.UserControl
    {

        public event EventHandler Click = null;
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

        
        public bool isGeneral { get; set; }
        public bool isConsultar { get; set; }

        public string GUID
        {
            set { HFGUID.Value = value; }
            get { return HFGUID.Value; }
        }
        public string CUO 
        {
            set { HF_CUO.Value = value; }
            get { return HF_CUO.Value; }
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {

            BtnGrabActAdj.OnClientClick = "return ValidarRegistroAdjunto()";


            cmb_TipoArchivo.AutoPostBack = true;
            if (!Page.IsPostBack)
            {
                int intPesoMaximoArchivo = Convert.ToInt32(ConfigurationManager.AppSettings["SUNARP.PesoMaximoArchivo"].ToString());
                double decFile = Math.Round((intPesoMaximoArchivo / 1024f) / 1024f, 0);

                lblTamanioMax.Text = decFile.ToString();

                msjeWarning.Visible = false;
                msjeError.Visible = false;
                msjeSucess.Visible = false;
                CargarTipoArchivo();

                ViewState["iOperAdj"] = true;

                if (Convert.ToBoolean(ViewState["ACT_DIGITALIZA"]) == true)
                {
                    HFAutodhesivo.Value = "1";
                }
                else
                {
                    HFAutodhesivo.Value = "0";
                }                
            }
                       

            VerificaExistenciaDigitalizacion();


            if (isConsultar)
            {
                cmb_TipoArchivo.Enabled = false;
                txtDescAdj.Enabled = false;
                Grd_Archivos.Enabled = false;
                ckHabilitarDigitalizacion.Enabled = false;
                FileUploader.Enabled = false;
                BtnGrabActAdj.Enabled = false;
                BtnLimpiaAdj.Enabled = false;

                HabilitarControlesAdjunto(true);
            }
            updActuacionAdjuntar.Update();
        }

        protected void BtnGrabActAdj_Click(object sender, EventArgs e)
        {
            string StrScript = string.Empty;

            if (Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + GUID]) == 0)
            {

                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuación", "Debe crear la actuación antes de adjuntar.");
                Comun.EjecutarScript(Page, StrScript);
                if (Click != null)
                {
                    Click(this, EventArgs.Empty);
                }
                return;
            }

            String scriptMover = String.Empty;

            scriptMover = @"$(function(){{ MoveTabIndex(2);}});";
            scriptMover = string.Format(scriptMover);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);

            BtnGrabActAdj.Enabled = false;
            if (CargarArchivo())
            {
                int IntRpta = 0;

                RE_ACTUACIONADJUNTO ObjAdjActBE = new RE_ACTUACIONADJUNTO();
                                

                ObjAdjActBE.acad_iActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + GUID]);
                ObjAdjActBE.acad_sAdjuntoTipoId = Convert.ToInt16(cmb_TipoArchivo.SelectedValue);
                ObjAdjActBE.acad_vNombreArchivo = lblNombreArchivo.Text;
                ObjAdjActBE.acad_vDescripcion = Util.ReemplazarCaracter(txtDescAdj.Text.Trim().ToUpper());


                if (Convert.ToBoolean(ViewState["iOperAdj"]))
                {
                    ObjAdjActBE.acad_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    ObjAdjActBE.acad_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    //------------------------------------------------
                    //Insertar
                    //------------------------------------------------
                    ActuacionAdjuntoMantenimientoBL objAdjuntoBL = new ActuacionAdjuntoMantenimientoBL();
                    IntRpta = objAdjuntoBL.Insertar(ObjAdjActBE, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                }                
                               

                if (IntRpta <= 0)
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Actuación", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    if (Click != null)
                    {
                        Click(this, EventArgs.Empty);
                    }
                    return;
                }
                else
                {
                    if (ObjAdjActBE.acad_sAdjuntoTipoId == (int)Enumerador.enmTipoAdjunto.DOCUMENTO_DIGITALIZA)
                    {
                        BtnGrabActAdj.Enabled = false;
                    }
                }

                ViewState["iOperAdj"] = true;

                cmb_TipoArchivo.SelectedIndex = 0;
                BtnGrabActAdj.Enabled = false;

                txtDescAdj.Text = "";

                hidNomAdjFile.Value = "";

                CargarGrillaAdjuntos(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + GUID]));

                lblNombreArchivo.Text = String.Empty;
                ObjAdjActBE = null;
                BtnGrabActAdj.Enabled = false;
            }
            else
            {
                cmb_TipoArchivo.SelectedIndex = -1;
                BtnGrabActAdj.Enabled = false;
            }


            VerificaExistenciaDigitalizacion();

            ckHabilitarDigitalizacion.Visible = false;

            updActuacionAdjuntar.Update();
            if (Click != null)
            {
                BtnGrabActAdj = null;
                Click(this, EventArgs.Empty);
            }


        }

        protected void BtnLimpiaAdj_Click(object sender, EventArgs e)
        {

            LimpiarMensaje();
            String NombreArchivo = lblNombreArchivo.Text;
            lblNombreArchivo.Text = String.Empty;
            txtDescAdj.Text = String.Empty;
            cmb_TipoArchivo.SelectedIndex = -1;
            BtnGrabActAdj.Enabled = false;
            String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
            String uploadFileName = String.Empty;

            ViewState["iOperAdj"] = true;

            String strMensaje = String.Empty;

            HabilitarCajasDigitalizacion(true);
            

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
                catch (Exception ex)
                {
                    strMensaje = "No se encuentra o no existe el directorio de Adjuntos.";
                    #region Registro Incidencia
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_vValoresTabla = "CONTROL ADJUNTO - CREACIÓN DIRECTORIO",
                        audi_sOperacionTipoId = (int)Enumerador.enmTipoIncidencia.ERROR_APLICATION,
                        audi_sOperacionResultadoId = (int)Enumerador.enmResultadoAuditoria.ERR,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                        audi_vComentario = ex.Message,
                        audi_vMensaje = ex.StackTrace,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]),
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });
                    #endregion
                }
            }


            updActuacionAdjuntar.Update();


            if (Click != null)
            {
                Click(this, EventArgs.Empty);
            }
        }

    

        protected void ckHabilitarDigitalizacion_CheckedChanged(object sender, EventArgs e)
        {
            ValidarDigitalizacion();



        }

        Descarga objDescarga;
       
        
        protected void Grd_Archivos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            string StrScript = string.Empty;

            ViewState["iOperAdj"] = false;

            if (e.CommandName == "Descargar")
            {
                int Index = Convert.ToInt32(e.CommandArgument);

                #region Descargar

                LimpiarMensaje();
                //vNombreArchivo
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
                    //EP_R202000001_20200929172500.pdf
                    string strSigla = strNombreArchivo.Substring(0,2);
                    string strAnio = strNombreArchivo.Substring(14,4);
                    string strMes = strNombreArchivo.Substring(18,2);
                    string strDia = strNombreArchivo.Substring(20,2);

                    string strpathSigla = Path.Combine(struploadPath, strSigla);
                    string strpathAnio = Path.Combine(strpathSigla, strAnio);
                    string strpathAnioMes = Path.Combine(strpathAnio, strMes);
                    string strpathAnioMesDia = Path.Combine(strpathAnioMes, strDia);

                    if (!Directory.Exists(strpathSigla))
                    {
                        Directory.CreateDirectory(strpathSigla);
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

                #endregion
            }           
            else if (e.CommandName == "Eliminar")
            {
                int Index = Convert.ToInt32(e.CommandArgument);

                #region Eliminar

                if (Convert.ToInt32(Session["Actuacion_Accion"]) == (int)Enumerador.enmTipoOperacion.CONSULTA)                    
                    LimpiarMensaje();
                lblNombreArchivo.Text = String.Empty;
               
                RE_ACTUACIONADJUNTO ObjAdjActBE = new RE_ACTUACIONADJUNTO();
               
                int IntRpta = 0;
                //iActuacionAdjuntoId
                ObjAdjActBE.acad_iActuacionAdjuntoId = Convert.ToInt64(Convert.ToString(Grd_Archivos.Rows[Index].Cells[0].Text));
                
                ObjAdjActBE.acad_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjAdjActBE.acad_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);


                ActuacionAdjuntoMantenimientoBL objAdjuntoBL = new ActuacionAdjuntoMantenimientoBL();
                IntRpta = objAdjuntoBL.Eliminar(ObjAdjActBE, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

                if (IntRpta > 0)
                {
                    ViewState["iOperAdj"] = true;
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "ADJUNTOS", "Se ha eliminado el adjunto.", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    CargarGrillaAdjuntos(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + GUID]));
                }
                else
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTOS", "Error. No se pudo realizar la operación", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    if (Click != null)
                    {
                        Click(this, EventArgs.Empty);
                    }
                    return;
                }


                #endregion
            }
            else if (e.CommandName == "Firmar")
            {
                int Index = Convert.ToInt32(e.CommandArgument);
                #region FIRMAR

                bool bExisteArchivo = false;
                string strNombreArchivo = Convert.ToString(Grd_Archivos.Rows[Index].Cells[6].Text);   
                string rutaPDF = Server.MapPath("../documents/");
                string strRutaDestino = Path.Combine(rutaPDF, strNombreArchivo);
                string strRuta = string.Empty;
                string ext = Path.GetExtension(strNombreArchivo).ToUpper();
                string struploadPath = ConfigurationManager.AppSettings["UploadPath"].ToString();
                if (ext.Equals(".PDF"))
                {
                    //EP_R202000001_20200929172500.pdf
                    string strSigla = strNombreArchivo.Substring(0, 2);
                    string strAnio = strNombreArchivo.Substring(14, 4);
                    string strMes = strNombreArchivo.Substring(18, 2);
                    string strDia = strNombreArchivo.Substring(20, 2);

                    string strpathSigla = Path.Combine(struploadPath, strSigla);
                    string strpathAnio = Path.Combine(strpathSigla, strAnio);
                    string strpathAnioMes = Path.Combine(strpathAnio, strMes);
                    string strpathAnioMesDia = Path.Combine(strpathAnioMes, strDia);

                    if (!Directory.Exists(strpathSigla))
                    {
                        Directory.CreateDirectory(strpathSigla);
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
                    if (File.Exists(strRuta))
                    {
                        bExisteArchivo = true;
                        if (File.Exists(strRutaDestino))
                        {
                            File.Delete(strRutaDestino);
                        }
                        File.Copy(strRuta, strRutaDestino);

                    }
                }
                if (bExisteArchivo)
                {
                    Session["RutaArchivoConFirma"] = strNombreArchivo;

                    int intNumeroHojas = 0;
                    bool bError = false;
                    try
                    {
                        PdfReader reader = new PdfReader(strRuta);
                        intNumeroHojas = reader.NumberOfPages;
                    }
                    catch(Exception ex)
                    {
                        ex = null;
                        bError = true;
                        StrScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "FIRMAR", ex.Message, false, 200, 400);
                        Comun.EjecutarScript(Page, StrScript);
                    }
                    if (bError == false)
                    {
                        Session["CantHojasPDFOriginal"] = intNumeroHojas - 1;
                        //--------------------------------------------------------------------------------------------
                        // FIRMAR CON REFIRMA.
                        //--------------------------------------------------------------------------------------------                       
                        //Grd_Archivos.Rows[Index].Cells[7].Controls[4].Visible = false;
                        //--------------------------------------------------------------------------------------------
                    }
                }
                return;
                #endregion
            }
            if (Click != null)
            {
                Click(this, EventArgs.Empty);
            }
            updActuacionAdjuntar.Update();
        }

        protected void Grd_Archivos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
        }

        protected void ctrlPagActuacionAdjuntos_Click(object sender, EventArgs e)
        {
            LimpiarMensaje();
            CargarGrillaAdjuntos(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + GUID]));
            updActuacionAdjuntar.Update();

        }
        protected void cmb_TipoArchivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            String strScript = String.Empty;
            try
            {
                LimpiarMensaje();
                

                if (cmb_TipoArchivo.SelectedIndex > 0)
                {
                    string x = cmb_TipoArchivo.SelectedItem.Text;
                    hidNomAdjFile.Value = x;
                    String[] Ext = cmb_TipoArchivo.SelectedItem.Text.Split('|');
                    lblNombreArchivo.Text = String.Empty;
                    hd_Extension.Value = Ext[1].ToString();
                    BtnGrabActAdj.Enabled = true;
                }
                else
                {
                    BtnGrabActAdj.Enabled = false;
                }
               
            }
            catch
            {
                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", Constantes.CONST_MENSAJE_OPERACION_FALLIDA);
                Comun.EjecutarScript(Page, strScript);
            }
        }

        protected void btnDigitalizar_Click(object sender, EventArgs e)
        {            
            string strUrl = "../Accesorios/FrmDigitaliza.aspx";
            string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=750,height=550,left=100,top=10');";
            ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), "OpenPopup", strScript, true);
        }

        #endregion

        #region Metodos

        bool IsPDFHeader(string fileName)
        {
            byte[] data;
            try
            {
                using (MemoryStream memoryStream = new MemoryStream(FileUploader.FileBytes.ToArray()))
                {
                    //HttpPostedFileBase _file = (HttpPostedFileBase)FileUploader.HasFile;
                    //_file.InputStream.CopyTo(memoryStream);
                    data = memoryStream.ToArray();
                    PdfReader reader = new PdfReader(data);

                }
            }
            catch
            {
                data = null;
                return false;
            }
            return true;
        }

     
       
        public void CargarGrillaAdjuntos(long LonActuacionDetalleId)
        {

            Grd_Archivos.DataSource = null;
            Grd_Archivos.DataBind();

            DataTable dtAdjuntos = new DataTable();
           
            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_PAGE_SIZE_ADJUNTOS;

            ActuacionAdjuntoConsultaBL objAdjuntoBL = new ActuacionAdjuntoConsultaBL();
            dtAdjuntos = objAdjuntoBL.ActuacionAdjuntosObtener(LonActuacionDetalleId, CtrlPageBarAdjunto.PaginaActual.ToString(),
                intPaginaCantidad, ref IntTotalCount, ref IntTotalPages);

            if (dtAdjuntos.Rows.Count > 0)
            {
                Grd_Archivos.DataSource = dtAdjuntos;
                Grd_Archivos.DataBind();

                CtrlPageBarAdjunto.TotalResgistros = IntTotalCount;
                CtrlPageBarAdjunto.TotalPaginas = IntTotalPages;

                CtrlPageBarAdjunto.Visible = false;
                if (CtrlPageBarAdjunto.TotalResgistros > intPaginaCantidad)
                {
                    CtrlPageBarAdjunto.Visible = true;
                }
            }
            else
            {
                //---------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 14/09/2016
                // Objetivo: Deshabilitar el boton adjuntar cuando 
                //           no se ha seleccionado el tipo de archivo
                //---------------------------------------------
                if (cmb_TipoArchivo.SelectedIndex <= 0)
                {
                    BtnGrabActAdj.Enabled = false;
                }
            }
            updActuacionAdjuntar.Update();
        }

        private void ValidarDigitalizacion()
        {

            if (ckHabilitarDigitalizacion.Checked)
            {
                HabilitarCajas(true);
            }
            else
            {
                HabilitarCajas(false);
            }

            if (Convert.ToInt32(Session["Actuacion_Accion"]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                BtnGrabActAdj.Enabled = false;
            else
                BtnGrabActAdj.Enabled = true;
        }
        private void HabilitarCajasDigitalizacion(Boolean bEstado)
        {
            txtDescAdj.Enabled = bEstado;
            FileUploader.Enabled = bEstado;
            BtnGrabActAdj.Enabled = bEstado;

            ckHabilitarDigitalizacion.Checked = false;
            ckHabilitarDigitalizacion.Visible = !bEstado;
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

                Int32 TotalRegistro = Grd_Archivos.Rows.Count + 1;
                if (Convert.ToBoolean(ViewState["iOperAdj"]) != false)
                {
                    if (FileUploader.FileBytes.Length == 0)
                    {
                        strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", "Error al Adjuntar, el archivo es inválido y/o no tiene contenido.", false, 200, 400);
                        Comun.EjecutarScript(Page, strScript);
                        return false;
                    }
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

                    #region Validar PDF, Imagen
                    if (cmb_TipoArchivo.SelectedIndex > 0)
                    {
                        if (cmb_TipoArchivo.SelectedItem.Text.IndexOf("PDF") > 0)
                        {
                            if (CUO.Length > 0)
                            {
                                if (!IsPDFHeader(uploadFileName = Documento.GetUniqueUploadFileNameSUNARP(CUO, uploadPath, ref _fileName)))
                                {
                                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", "Documento inválido y/o corrupto.");
                                    Comun.EjecutarScript(Page, strScript);
                                    return false;
                                }
                            }
                            else
                            {
                                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", "Número de CUO es requerido.");
                                Comun.EjecutarScript(Page, strScript);
                                return false;
                            }
                        }
                        else
                        {
                            try
                            {
                                Stream stream = new MemoryStream(FileUploader.FileBytes);

                                stream.ReadByte();
                            }
                            catch
                            {
                                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", "Documento inválido y/o corrupto.");
                                Comun.EjecutarScript(Page, strScript);
                                return false;
                            }
                        }
                    }
                    #endregion

                    String extension = System.IO.Path.GetExtension(_fileName);
                    int fileSizeInBytes = FileUploader.PostedFile.ContentLength;
                    int fileSizeInKB = fileSizeInBytes / Constantes.CONST_TAMANIO_MAX_ADJUNTO_KB;

                    int intPesoMaximoArchivo = Convert.ToInt32(ConfigurationManager.AppSettings["SUNARP.PesoMaximoArchivo"].ToString());

                    bool bExito = validarPesoArchivo(intPesoMaximoArchivo, fileSizeInKB);

                    if (bExito)
                    {
                        if (hd_Extension.Value.ToUpper() == extension.ToUpper())
                        {
                            #region ObtenerNombreArchivo

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

                            if (CUO.Length > 0)
                            {
                                uploadFileName = Documento.GetUniqueUploadFileNameSUNARP(CUO, uploadPath, ref _fileName);
                            }
                            else
                            {
                                strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", "Número de CUO es requerido.", false, 200, 400);
                                Comun.EjecutarScript(Page, strScript);
                                return false;
                            }

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
                            #endregion
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
                        lblMsjeWarnig.Text = System.Configuration.ConfigurationManager.AppSettings["UploadInvalidSizeMsje"];
                        msjeWarning.Visible = true;
                        msjeError.Visible = false;
                        msjeSucess.Visible = false;
                        Resultado = false;
                    }
                   
                    //--------------------------------
                }
                else
                {
                    if (Convert.ToBoolean(ViewState["iOperAdj"]) == false)
                    {
                        Resultado = true;
                        //---------------------------------------------
                        // Autor: Miguel Márquez Beltrán
                        // Fecha: 14/09/2016
                        // Objetivo: Deshabilitar el boton adjuntar cuando 
                        //           no se ha seleccionado el tipo de archivo
                        //---------------------------------------------
                        if (cmb_TipoArchivo.SelectedIndex <= 0)
                        {
                            BtnGrabActAdj.Enabled = false;
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


        private void VerificaExistenciaDigitalizacion()
        {
           
            foreach (GridViewRow row in Grd_Archivos.Rows)
            {
                //sAdjuntoTipoId
                Int32 iTipoAdjunto = Convert.ToInt32(row.Cells[2].Text);

                if (iTipoAdjunto == (int)Enumerador.enmTipoAdjunto.DOCUMENTO_DIGITALIZA)
                {
                    HabilitarCajas(true);
                                        
                    ViewState["ACT_DIGITALIZA"] = 1;
                    HFAutodhesivo.Value = "1";

                    BtnGrabActAdj.Enabled = true;
                    break;
                }
                else
                {
                    BtnGrabActAdj.Enabled = true;
                    ViewState["ACT_DIGITALIZA"] = 0;
                    HFAutodhesivo.Value = "0";
                }
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


        public void CargarTipoArchivo()
        {            
            DataTable dt = new DataTable();
            dt = comun_Part1.ObtenerParametrosPorGrupo(Session, "SUNARP-TIPO ADJUNTO").Copy();

            Util.CargarParametroDropDownList(cmb_TipoArchivo, comun_Part1.ObtenerParametrosPorGrupo(Session, "SUNARP-TIPO ADJUNTO"), true);                       
        }

        public void HabilitarAdjuntos(bool bolHabilitar = true)
        {
            cmb_TipoArchivo.Enabled = true;
            BtnLimpiaAdj.Enabled = bolHabilitar;
            HabilitarCajasDigitalizacion(bolHabilitar);
            Grd_Archivos.DataSource = null;
            Grd_Archivos.DataBind();
        }

        // Sin limpiar la grilla
        public void HabilitarControlesAdjunto(bool bolHabilitar = true)
        {
            BtnGrabActAdj.Enabled = bolHabilitar;
            cmb_TipoArchivo.Enabled = true;
            BtnLimpiaAdj.Enabled = bolHabilitar;

            txtDescAdj.Enabled = bolHabilitar;
            FileUploader.Enabled = bolHabilitar;
            BtnGrabActAdj.Enabled = bolHabilitar;

            ckHabilitarDigitalizacion.Visible = false;
            
        }

        public void btn_visible(Button btn, Boolean estado)
        {
            btn.Visible = estado;
        }
        #endregion


        protected void CtrlPageBarActAdjunto_Click(object sender, EventArgs e)
        {
            CargarGrillaAdjuntos(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID + GUID]));
            updActuacionAdjuntar.Update();
        }


        public void AgregarAdjuntosDigitalizados(BE.MRE.RE_ACTUACIONADJUNTO oRE_ACTUACIONADJUNTO)
        {

            RE_ACTUACIONADJUNTO ObjAdjActBE = new RE_ACTUACIONADJUNTO();

            ObjAdjActBE.acad_iActuacionDetalleId = oRE_ACTUACIONADJUNTO.acad_iActuacionDetalleId;
            ObjAdjActBE.acad_sAdjuntoTipoId = oRE_ACTUACIONADJUNTO.acad_sAdjuntoTipoId;
            ObjAdjActBE.acad_vNombreArchivo = oRE_ACTUACIONADJUNTO.vNombreArchivo;
            ObjAdjActBE.acad_vDescripcion = Util.ReemplazarCaracter(oRE_ACTUACIONADJUNTO.vDescripcion);

            ObjAdjActBE.acad_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            ObjAdjActBE.acad_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

            ActuacionAdjuntoMantenimientoBL oActuacionAdjuntoMantenimientoBL = new ActuacionAdjuntoMantenimientoBL();
            Int32 IntRpta = oActuacionAdjuntoMantenimientoBL.Insertar(ObjAdjActBE, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

        }

        public bool validarPesoArchivo(int pesoValido, int PesoarchivoEvaluar)
        {
            double pesoValidoConvert = Convert.ToDouble(pesoValido);
            double decFile = Math.Round((PesoarchivoEvaluar / 1024f) / 1024f, 2);
            bool exito = (decFile <= pesoValidoConvert ? true : false);
            return exito;
        }
    //------------------------------------
    }
}