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

namespace SGAC.WebApp.Accesorios.SharedControls
{
    public partial class ctrlAdjunto_ : System.Web.UI.UserControl
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

        public bool IsCivil { get; set; }
        public bool IsMilitar { get; set; }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            

            BtnGrabActAdj.OnClientClick = "return ValidarRegistroAdjunto()";



            if (!Page.IsPostBack)
            {
                lblTamanioMax.Text = Constantes.CONST_TAMANIO_MAX_ADJUNTO_KB.ToString();

                msjeWarning.Visible = false;
                msjeError.Visible = false;
                msjeSucess.Visible = false;
                CargarListadosDesplegables();
                Session["iOperAdj"] = true;
                ValidarSession();

               
                if (Convert.ToBoolean(Session["ACT_DIGITALIZA"]) == true)
                {
                    HFAutodhesivo.Value = "1";
                }
                else
                {
                    HFAutodhesivo.Value = "0";
                }
                if (IsMigratorio)
                {
                    lblTamanioMax.Text = "";
                    lblLeyendaAdj.Text = "";
                    lblUnidad.Text = "";

                }

                

            }
           
            if (IsCivil)
            {
                
                cmb_TipoArchivo.Items[2].Enabled = false;
                cmb_TipoArchivo.Items[3].Enabled = false;

                if (Session["COD_AUTOADHESIVO"].ToString() != string.Empty)
                    cmb_TipoArchivo.Items[5].Enabled = false;
                else
                {
                    cmb_TipoArchivo.Items[4].Enabled = false;
                }
            }

            if (IsMilitar) {
                cmb_TipoArchivo.Items[2].Enabled = false;
                cmb_TipoArchivo.Items[3].Enabled = false;

                if (Session["COD_AUTOADHESIVO"].ToString() != string.Empty)
                    cmb_TipoArchivo.Items[5].Enabled = false;
                else
                {
                    cmb_TipoArchivo.Items[4].Enabled = false;
                }
                
            }
            VerificaExistenciaDigitalizacion();
            if (IsMigratorio)
            {

                if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                    HabilitarControlesAdjunto(false);
            }

            updActuacionAdjuntar.Update();
        }

        public static String GuardarAdjunto()
        {

            try
            {


            }
            catch (Exception ex)
            {

            }

            return "";
        }




        protected void BtnGrabActAdj_Click(object sender, EventArgs e)
        {
            string StrScript = string.Empty;


            if (Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]) == 0)
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

            if (Convert.ToBoolean(Session["bCivil"]) != true)
            {
                scriptMover = @"$(function(){{ MoveTabIndex(2);}});";
                scriptMover = string.Format(scriptMover);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);
            }

            BtnGrabActAdj.Enabled = false;
            if (CargarArchivo())
            {
                int IntRpta = 0;

                RE_ACTUACIONADJUNTO ObjAdjActBE = new RE_ACTUACIONADJUNTO();
                Proceso MiProc = new Proceso();
                Enumerador.enmAccion enmAccion = Enumerador.enmAccion.INSERTAR;

                ObjAdjActBE.acad_iActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                ObjAdjActBE.acad_sAdjuntoTipoId = Convert.ToInt16(cmb_TipoArchivo.SelectedValue);
                ObjAdjActBE.acad_vNombreArchivo = lblNombreArchivo.Text;
                ObjAdjActBE.acad_vDescripcion = Util.ReemplazarCaracter(txtDescAdj.Text.Trim().ToUpper());




                #region - Solo para migratorio -
                if (IsMigratorio)
                {
                    int IntTotalCount = 0;
                    int IntTotalPages = 0;
                    int intPaginaCantidad = Constantes.CONST_PAGE_SIZE_ADJUNTOS;

                    Object[] mi_Migratorio = new Object[5] { ObjAdjActBE.acad_iActuacionDetalleId,  
                                               "1", // ctrlPagActuacionAdjuntos_Click.PaginaActual.ToString(), 
                                               intPaginaCantidad, 
                                               IntTotalCount, 
                                               IntTotalPages };

                    var dtAdjuntos = (DataTable)MiProc.Invocar(ref mi_Migratorio,
                                                           "SGAC.BE.RE_ACTUACIONADJUNTO",
                                                           Enumerador.enmAccion.OBTENER);

                    if (dtAdjuntos.Rows.Count > 0)
                    {
                        /*Buscamos si existe algun dato ya registrado para el tipo de adjunto*/
                        try
                        {
                            var obj_Existe = (from dr in dtAdjuntos.AsEnumerable()
                                              where Convert.ToInt32(dr["sAdjuntoTipoId"]) == Convert.ToInt32(cmb_TipoArchivo.SelectedItem.Value)
                                              select dr).CopyToDataTable();

                            if (obj_Existe.Rows.Count > 0)
                            {
                                if (Convert.ToBoolean(Session["iOperAdj"]))
                                {
                                    cmb_TipoArchivo_SelectedIndexChanged(sender, EventArgs.Empty);
                                    lblMsjeWarnig.Text = "Tipo de archivo seleccionado ya se encuentra registrado";
                                    msjeWarning.Visible = true;
                                    msjeError.Visible = false;
                                    msjeSucess.Visible = false;
                                    if (Click != null)
                                    {
                                        Click(this, EventArgs.Empty);
                                    }
                                    return;
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                #endregion




                if (Convert.ToBoolean(Session["iOperAdj"]))
                {

                    ObjAdjActBE.acad_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    ObjAdjActBE.acad_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                    enmAccion = Enumerador.enmAccion.INSERTAR;
                }
                else
                {
                    ObjAdjActBE.acad_iActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);
                    ObjAdjActBE.acad_sAdjuntoTipoId = Convert.ToInt16(cmb_TipoArchivo.SelectedValue);
                    ObjAdjActBE.acad_vNombreArchivo = lblNombreArchivo.Text;
                    ObjAdjActBE.acad_vDescripcion = Util.ReemplazarCaracter(txtDescAdj.Text.Trim().ToUpper());

                    ObjAdjActBE.acad_iActuacionAdjuntoId = Convert.ToInt32(Session["IActuacionAdjuntoId"]);
                    ObjAdjActBE.acad_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    ObjAdjActBE.acad_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    ObjAdjActBE.acad_vNombreArchivo = lblNombreArchivo.Text;
                    enmAccion = Enumerador.enmAccion.MODIFICAR;
                }

                Object[] miArray = new Object[2] { ObjAdjActBE, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) };
                IntRpta = (int)MiProc.Invocar(ref miArray, "SGAC.BE.RE_ACTUACIONADJUNTO", enmAccion);
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
                        //Grd_Archivos.Enabled = false;
                        BtnGrabActAdj.Enabled = false;

                    }

                }


                Session["iOperAdj"] = true;

                cmb_TipoArchivo.SelectedIndex = 0;
                txtDescAdj.Text = "";

                hidNomAdjFile.Value = "";

                CargarGrillaAdjuntos(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]));

                lblNombreArchivo.Text = String.Empty;
                ObjAdjActBE = null;
            }

            BtnGrabActAdj.Enabled = true;

            VerificaExistenciaDigitalizacion();

            ckHabilitarDigitalizacion.Visible = false;
            cmb_funcionario.Visible = false;
            lblfuncionario.Visible = false;
            if (IsMigratorio)
            {
                string s_valor = string.Empty;
                switch (Convert.ToInt32(cmb_TipoArchivo.SelectedValue))
                {
                    case (Int32)Enumerador.enmTipoAdjunto.FIRMA:
                        s_valor = "5 KB a 10";
                        lblLeyendaAdj.Text = "Solo se permiten guardar archivos de un tamaño Max. de ";
                        lblUnidad.Text = "KB";
                        break;
                    case (Int32)Enumerador.enmTipoAdjunto.FOTO:
                        lblLeyendaAdj.Text = "Solo se permiten guardar archivos de un tamaño Max. de ";
                        lblUnidad.Text = "KB";
                        s_valor = "10 KB a 18";
                        break;
                    case (Int32)Enumerador.enmTipoAdjunto.HUELLA:
                        s_valor = "10 KB a 20";
                        lblLeyendaAdj.Text = "Solo se permiten guardar archivos de un tamaño Max. de ";
                        lblUnidad.Text = "KB";
                        break;
                    default:
                        lblTamanioMax.Text = "";
                        s_valor = "";
                        lblUnidad.Text = "";
                        lblLeyendaAdj.Text = "";
                        break;

                }
                
                lblTamanioMax.Text = s_valor;
                
            }

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
            String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];
            String uploadFileName = String.Empty;

            Session["iOperAdj"] = true;

            String strMensaje = String.Empty;

            HabilitarCajasDigitalizacion(true);
            //HabilitarCajas(true);

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

                    updActuacionAdjuntar.Update();
                }
            }

            if (Convert.ToInt32(Session["Actuacion_Accion"]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                BtnGrabActAdj.Enabled = false;
            else
                BtnGrabActAdj.Enabled = true;


            if (Click != null)
            {
                Click(this, EventArgs.Empty);
            }
        }

        protected void cmb_TipoArchivo_prueba_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmb_TipoArchivo.SelectedValue) == (int)Enumerador.enmTipoAdjunto.DOCUMENTO_DIGITALIZA)
            {
                //this.btnDigitalizar.Visible = true;
            }

            string x = cmb_TipoArchivo.SelectedItem.Text;

            hidNomAdjFile.Value = x;
            String[] Ext = cmb_TipoArchivo.SelectedItem.Text.Split('|');
            lblNombreArchivo.Text = x;
            hd_Extension.Value = Ext[1].ToString();

        }

        protected void ckHabilitarDigitalizacion_CheckedChanged(object sender, EventArgs e)
        {
            ValidarDigitalizacion();

            

        }

        Descarga objDescarga;
        protected void Grd_Archivos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex > -1)
                {
                    Int32 iTipoAdjunto = Convert.ToInt32(Grd_Archivos.Rows[e.Row.RowIndex].Cells[2].Text);
                    //e.Row.Cells[9].Enabled = true;
                    if (iTipoAdjunto == (int)Enumerador.enmTipoAdjunto.DOCUMENTO_DIGITALIZA)
                    {
                        e.Row.Cells[10].Enabled = false;
                        e.Row.Cells[11].Enabled = false;

                        e.Row.Cells[11].Visible = false;
                        //e.Row.colum[11].Visible = false;
                    }
                }
                return;
            }

            ImageButton imgImprimir = e.Row.FindControl("btnPrint") as ImageButton;

            //ScriptManager.GetCurrent(this).RegisterPostBackControl(imgImprimir);
        }
        protected void Grd_Archivos_RowCreated(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[8].ColumnSpan = 3;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
            }
        }
        protected void Grd_Archivos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            string StrScript = string.Empty;
            int Index = Convert.ToInt32(e.CommandArgument);

            String scriptMover = String.Empty;

            if (Convert.ToBoolean(Session["bCivil"]) != true)
            {
                scriptMover = @"$(function(){{ MoveTabIndex(2);}});";
                scriptMover = string.Format(scriptMover);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoverTab", scriptMover, true);
            }
            Session["iOperAdj"] = false;
            if (e.CommandName == "Imprimir")
            {
                //string FilePath = Server.MapPath(@"~\Images\adjunto_copia.pdf");
                //WebClient User = new WebClient();
                //Byte[] FileBuffer = User.DownloadData(FilePath);
                //if (FileBuffer != null)
                //{
                //    Response.ContentType = "application/pdf";
                //    Response.AddHeader("content-length", FileBuffer.Length.ToString());
                //    Response.BinaryWrite(FileBuffer);
                //}


                LimpiarMensaje();

                string strNombreArchivo = Convert.ToString(Grd_Archivos.Rows[Index].Cells[6].Text);
                string strRuta = ConfigurationManager.AppSettings["UploadPath"].ToString() + strNombreArchivo;

                objDescarga = new Descarga();
                objDescarga.Download(strRuta, strNombreArchivo, false);
                objDescarga = null;
            }
            else if (e.CommandName == "Editar")
            {
                if (IsMigratorio)
                {
                    if (Convert.ToInt32(Session["Actuacion_Accion"]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                        return;
                }
                //if (Convert.ToInt32(Session["ACT_DIGITALIZA"]) == 1)
                //{
                //    if (Click != null)
                //    {
                //        Click(this, EventArgs.Empty);
                //    }
                //    return;
                //}

                LimpiarMensaje();
                Session["IActuacionAdjuntoId"] = Convert.ToString(Grd_Archivos.Rows[Index].Cells[0].Text);
                cmb_TipoArchivo.SelectedValue = Convert.ToString(Grd_Archivos.Rows[Index].Cells[2].Text);
                cmb_TipoArchivo_SelectedIndexChanged(sender, EventArgs.Empty);
                //cmb_TipoArchivo.SelectedItem.Text = Convert.ToString(Grd_Archivos.Rows[Index].Cells[3].Text);
                hidNomAdjFile.Value = Convert.ToString(Grd_Archivos.Rows[Index].Cells[4].Text);
                lblNombreArchivo.Text = Convert.ToString(Grd_Archivos.Rows[Index].Cells[6].Text);
                txtDescAdj.Text = Page.Server.HtmlDecode(Convert.ToString(Grd_Archivos.Rows[Index].Cells[7].Text));


            }
            else if (e.CommandName == "Eliminar")
            {
                /*Migratorio*/
                if (IsMigratorio)
                {
                    if (Convert.ToInt32(Session["Actuacion_Accion"]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                        return;
                }
                //if (Convert.ToInt32(Session["ACT_DIGITALIZA"]) == 1) {
                //    if (Click != null)
                //    {
                //        Click(this, EventArgs.Empty);
                //    }
                //    return; }
                LimpiarMensaje();
                lblNombreArchivo.Text = String.Empty;

                #region Eliminar
                RE_ACTUACIONADJUNTO ObjAdjActBE = new RE_ACTUACIONADJUNTO();

                Proceso MiProc = new Proceso();

                int IntRpta = 0;

                ObjAdjActBE.acad_iActuacionAdjuntoId = Convert.ToInt64(Convert.ToString(Grd_Archivos.Rows[Index].Cells[0].Text));
                ObjAdjActBE.acad_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjAdjActBE.acad_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                Object[] miArray = new Object[2] { ObjAdjActBE,
                                                   Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) };

                IntRpta = (int)MiProc.Invocar(ref miArray,
                                              "SGAC.BE.RE_ACTUACIONADJUNTO",
                                              Enumerador.enmAccion.ELIMINAR);

                if (IntRpta > 0)
                {
                    Session["iOperAdj"] = true;
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "ADJUNTOS", "Se ha eliminado el adjunto.", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    CargarGrillaAdjuntos(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]));
                    // CargarGrillaAdjuntos(9);
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
            if (Click != null)
            {
                Click(this, EventArgs.Empty);
            }
            updActuacionAdjuntar.Update();
        }

        protected void ctrlPagActuacionAdjuntos_Click(object sender, EventArgs e)
        {
            LimpiarMensaje();
            CargarGrillaAdjuntos(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]));
            updActuacionAdjuntar.Update();

        }
        protected void cmb_TipoArchivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            String strScript = String.Empty;
            try
            {
                LimpiarMensaje();

                if (Convert.ToInt32(cmb_TipoArchivo.SelectedValue) == (int)Enumerador.enmTipoAdjunto.DOCUMENTO_DIGITALIZA)
                {
                    string strCodigo = hifCodVinculado.Value;
                    if (strCodigo == string.Empty)
                        strCodigo = Session["COD_AUTOADHESIVO"].ToString();

                    if (strCodigo != string.Empty)
                    {
                        HabilitarCajasDigitalizacion(false);
                        LimpiarMensaje();
                        BtnGrabActAdj.Enabled = false;
                    }
                    else
                    {
                        BtnGrabActAdj.Enabled = false;
                        cmb_TipoArchivo.SelectedIndex = 0;
                        strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", "Debe Realizar la Vinculación antes de la Digitalización", false, 200, 400);
                        Comun.EjecutarScript(Page, strScript);
                    }

                   // HabilitarCajasDigitalizacion(true);

                }
                else
                {
                    if (IsMigratorio)
                    {
                        string s_valor = string.Empty;
                        switch (Convert.ToInt32(cmb_TipoArchivo.SelectedValue))
                        {
                            case (Int32)Enumerador.enmTipoAdjunto.FIRMA:
                                s_valor = "5 KB a 10";
                                lblLeyendaAdj.Text = "Solo se permiten guardar archivos de un tamaño Max. de ";
                                lblUnidad.Text = " KB";
                                break;
                            case (Int32)Enumerador.enmTipoAdjunto.FOTO:
                                s_valor = "10 KB a 18";
                                lblLeyendaAdj.Text = "Solo se permiten guardar archivos de un tamaño Max. de ";
                                lblUnidad.Text = " KB";
                                break;
                            case (Int32)Enumerador.enmTipoAdjunto.HUELLA:
                                s_valor = "10 KB a 20";
                                lblLeyendaAdj.Text = "Solo se permiten guardar archivos de un tamaño Max. de ";
                                lblUnidad.Text = " KB";
                                break;
                            default:
                                lblUnidad.Text = "";
                                lblLeyendaAdj.Text = "";
                                break;
                        }
                        lblTamanioMax.Text = s_valor;

                    }
                    BtnGrabActAdj.Enabled = true;
                    HabilitarCajasDigitalizacion(true);
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
        private void CargarListadosDesplegables()
        {
            // Tipo de Archivo
            CargarTipoArchivo();

            // Funcionario
            int intOficinaConsularId = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            DataTable dt = funcionario.dtFuncionario(intOficinaConsularId, 0);

            cmb_funcionario.Items.Clear();
            cmb_funcionario.DataTextField = "vFuncionario";
            cmb_funcionario.DataValueField = "IFuncionarioId";
            cmb_funcionario.DataSource = dt;
            cmb_funcionario.DataBind();
            cmb_funcionario.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
        }

        public void CargarGrillaAdjuntos(long LonActuacionDetalleId)
        {

            Grd_Archivos.DataSource = null;
            Grd_Archivos.DataBind();

            DataTable dtAdjuntos = new DataTable();
            Proceso MiProc = new Proceso();

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_PAGE_SIZE_ADJUNTOS;

            Object[] miArray = new Object[5] { LonActuacionDetalleId,  
                                               CtrlPageBarAdjunto.PaginaActual.ToString(), 
                                               intPaginaCantidad, 
                                               IntTotalCount, 
                                               IntTotalPages };

            dtAdjuntos = (DataTable)MiProc.Invocar(ref miArray,
                                                   "SGAC.BE.RE_ACTUACIONADJUNTO",
                                                   Enumerador.enmAccion.OBTENER);

            if (dtAdjuntos.Rows.Count > 0)
            {
                Grd_Archivos.DataSource = dtAdjuntos;
                Grd_Archivos.DataBind();

                CtrlPageBarAdjunto.TotalResgistros =  Convert.ToInt32(miArray[3]);
                CtrlPageBarAdjunto.TotalPaginas =  Convert.ToInt32(miArray[4]);

                CtrlPageBarAdjunto.Visible = false;
                if (CtrlPageBarAdjunto.TotalResgistros > intPaginaCantidad)
                {
                    CtrlPageBarAdjunto.Visible = true;
                }
            }
            updActuacionAdjuntar.Update();
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
            cmb_funcionario.SelectedIndex = 0;

            
            ckHabilitarDigitalizacion.Visible = !bEstado;
            lblfuncionario.Visible = !bEstado;
            cmb_funcionario.Visible = !bEstado;

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
                if (Convert.ToBoolean(Session["iOperAdj"]) != false)
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

                    //#region Validar PDF
                    //if (cmb_TipoArchivo.SelectedIndex > 0)
                    //{
                    //    if (Convert.ToInt32(cmb_TipoArchivo.SelectedValue) == (int)Enumerador.enmTipoAdjunto.PDF)
                    //    {
                           

                    //        if (!IsPDFHeader(FileUploader.na))
                    //        {
                    //            strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ADJUNTAR", "Documento inválido y/o corrupto.");
                    //            Comun.EjecutarScript(Page, strScript);
                    //            return false;
                    //        }
                    //    }
                    //}
                    //#endregion

                    String extension = System.IO.Path.GetExtension(_fileName);
                    int fileSizeInBytes = FileUploader.PostedFile.ContentLength;
                    int fileSizeInKB = fileSizeInBytes / Constantes.CONST_TAMANIO_MAX_ADJUNTO_KB;

                    int s_Maximio = 0;
                    int s_Minimo = 0;

                    if (IsMigratorio)
                    {
                        switch (Convert.ToInt32(cmb_TipoArchivo.SelectedItem.Value))
                        {
                            case (Int32)Enumerador.enmTipoAdjunto.FIRMA:
                                s_Maximio = Constantes.CONST_TAMANIO_MAX_ADJUNTO_FIRMA_KB;
                                s_Minimo = Constantes.CONST_TAMANIO_MIN_ADJUNTO_FIRMA_KB;
                                break;
                            case (Int32)Enumerador.enmTipoAdjunto.HUELLA:
                                s_Maximio = Constantes.CONST_TAMANIO_MAX_ADJUNTO_HUELLA_KB;
                                s_Minimo = Constantes.CONST_TAMANIO_MIN_ADJUNTO_HUELLA_KB;
                                break;
                            case (Int32)Enumerador.enmTipoAdjunto.FOTO:
                                s_Maximio = Constantes.CONST_TAMANIO_MAX_ADJUNTO_FOTO_KB;
                                s_Minimo = Constantes.CONST_TAMANIO_MIN_ADJUNTO_FOTO_KB;
                                break;
                        }

                        if (fileSizeInKB >= s_Minimo && fileSizeInKB <= s_Maximio)
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
                                //    uploadFileName = GetUniqueUploadFileName(uploadPath, _fileName);

                                uploadFileName = Documento.GetUniqueUploadFileName(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]), uploadPath, ref _fileName);
                                #region Adjuntar
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
                    }
                    else
                    {
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
                                //uploadFileName = GetUniqueUploadFileName(uploadPath, _fileName);
                                uploadFileName = Documento.GetUniqueUploadFileName(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]), uploadPath, ref _fileName);
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
                            lblMsjeWarnig.Text = System.Configuration.ConfigurationManager.AppSettings["UploadInvalidSizeMsje"];
                            msjeWarning.Visible = true;
                            msjeError.Visible = false;
                            msjeSucess.Visible = false;
                            Resultado = false;
                        }
                    }

                    
                    
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

     
        private void VerificaExistenciaDigitalizacion()
        {
            //CargarTipoArchivo();
            foreach (GridViewRow row in Grd_Archivos.Rows)
            {
                Int32 iTipoAdjunto = Convert.ToInt32(row.Cells[2].Text);

                if (iTipoAdjunto == (int)Enumerador.enmTipoAdjunto.DOCUMENTO_DIGITALIZA)
                {
                    //BtnGrabActAdj.Enabled = false;

                    //BtnLimpiaAdj.Enabled = false;
                    //HabilitarCajasDigitalizacion(true);
                    HabilitarCajas(true);
                    //cmb_TipoArchivo.Enabled = false;
                    //Grd_Archivos.Enabled = false;
                    Session["ACT_DIGITALIZA"] = 1;
                    HFAutodhesivo.Value = "1";

                    BtnGrabActAdj.Enabled = true;
                    break;
                }
                else
                {
                    BtnGrabActAdj.Enabled = true;
                    Session["ACT_DIGITALIZA"] = 0;
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

        public void ValidarSession()
        {
            if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
            {
                //Grd_Archivos.Enabled = false;
            }
        }

        public bool IsMigratorio { get; set; }

        public void CargarTipoArchivo()
        {
            // PENDIENTE: Los tipos de archivo se deben cargar según tarifa consular
            // Tarifas migratorias - foto, firma, huella
            // Tarifas Generales - archivo

            DataTable dt = new DataTable();
            dt = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTUACION_TIPO_ADJUNTO).Copy();

            if (IsMigratorio)
            {
                try
                {
                    //var doc_migra = (from dr in dt.AsEnumerable()
                    //                 where Convert.ToInt64(dr["id"]) != (Int64)Enumerador.enmTipoAdjunto.DOCUMENTO_DIGITALIZA && Convert.ToInt64(dr["id"]) != (Int64)Enumerador.enmTipoAdjunto.PDF
                    //                 select dr).CopyToDataTable();
                    var doc_migra = (from dr in dt.AsEnumerable()
                                     where Convert.ToInt64(dr["id"]) != (Int64)Enumerador.enmTipoAdjunto.DOCUMENTO_DIGITALIZA 
                                     select dr).CopyToDataTable();

                    Util.CargarParametroDropDownList(cmb_TipoArchivo, doc_migra, true);
                    return;
                }
                catch
                {

                }
                
                
            }

         
            object objCod = Session["COD_AUTOADHESIVO"];
            if (objCod != null)
            {
                if (objCod.ToString() == string.Empty || objCod.ToString() == "0")
                {
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = " id <> " + (int)Enumerador.enmTipoAdjunto.DOCUMENTO_DIGITALIZA;
                    Util.CargarParametroDropDownList(cmb_TipoArchivo, dv.ToTable(), true);                    
                    return;
                }
            }

           
            Util.CargarParametroDropDownList(cmb_TipoArchivo, dt, true);

            if (IsCivil)
            {
                cmb_TipoArchivo.Items[2].Enabled = false;
                cmb_TipoArchivo.Items[3].Enabled = false;

                if (Session["COD_AUTOADHESIVO"].ToString() != string.Empty)
                    cmb_TipoArchivo.Items[5].Enabled = false;
                else
                {
                    cmb_TipoArchivo.Items[4].Enabled = false;
                }
            }

            if (IsMilitar)
            {
                cmb_TipoArchivo.Items[2].Enabled = false;
                cmb_TipoArchivo.Items[3].Enabled = false;

                if (Session["COD_AUTOADHESIVO"].ToString() != string.Empty)
                    cmb_TipoArchivo.Items[5].Enabled = false;
                else
                {
                    cmb_TipoArchivo.Items[4].Enabled = false;
                }

            }

           
        }

        public void HabilitarAdjuntos(bool bolHabilitar = true)
        {
            cmb_TipoArchivo.Enabled = true;
            BtnLimpiaAdj.Enabled = bolHabilitar;
            HabilitarCajasDigitalizacion(bolHabilitar);
            Grd_Archivos.DataSource = null;
            Grd_Archivos.DataBind();
            CargarTipoArchivo();
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
            lblfuncionario.Visible = false;
            cmb_funcionario.Visible = false;

            //Se desabilita los item de la grilla
            if (IsMigratorio)
            {
                if (Convert.ToInt32(Session[strVariableAccion]) == (int)Enumerador.enmTipoOperacion.CONSULTA)
                {
                    Grd_Archivos.Columns[9].Visible = bolHabilitar;
                    Grd_Archivos.Columns[10].Visible = bolHabilitar;
                    lblLeyendaAdj.Text = "";
                    lblTamanioMax.Text = "";
                    lblUnidad.Text = "";
                    cmb_TipoArchivo.Enabled = bolHabilitar;
                    BtnGrabActAdj.Enabled = bolHabilitar;
                    txtDescAdj.Enabled = bolHabilitar;
                }
                
            }

            CargarTipoArchivo();
        }

        public void btn_visible(Button btn,Boolean estado)
        {
            btn.Visible = estado;
        }
        #endregion


        protected void CtrlPageBarActAdjunto_Click(object sender, EventArgs e)
        {
            CargarGrillaAdjuntos(Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]));
            updActuacionAdjuntar.Update();
        }


        public void AgregarAdjuntosDigitalizados(BE.MRE.RE_ACTUACIONADJUNTO oRE_ACTUACIONADJUNTO) {

            RE_ACTUACIONADJUNTO ObjAdjActBE = new RE_ACTUACIONADJUNTO();

            ObjAdjActBE.acad_iActuacionDetalleId = oRE_ACTUACIONADJUNTO.acad_iActuacionDetalleId;
            ObjAdjActBE.acad_sAdjuntoTipoId = oRE_ACTUACIONADJUNTO.acad_sAdjuntoTipoId;
            ObjAdjActBE.acad_vNombreArchivo = oRE_ACTUACIONADJUNTO.vNombreArchivo;
            ObjAdjActBE.acad_vDescripcion = Util.ReemplazarCaracter(oRE_ACTUACIONADJUNTO.vDescripcion);

            ObjAdjActBE.acad_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            ObjAdjActBE.acad_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

            ActuacionAdjuntoMantenimientoBL oActuacionAdjuntoMantenimientoBL = new ActuacionAdjuntoMantenimientoBL();
            Int32 IntRpta =  oActuacionAdjuntoMantenimientoBL.Insertar(ObjAdjActBE, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));



        }
    }
}
