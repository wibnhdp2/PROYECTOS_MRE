using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.Configuracion.Maestro.BL;
using SGAC.Configuracion.Sistema.BL;
using SGAC.Controlador;
using SGAC.Registro.Persona.BL;
using SGAC.WebApp.Accesorios;
using SGAC.Registro.Actuacion.BL;
using SGAC.BE;
using SGAC.BE.MRE;
using SGAC.BE.MRE.Custom;

namespace SGAC.WebApp.Registro 
{
    public partial class FrmRegistroAsistencia : MyBasePage
    {
        #region Variable
        private string strMensajeDiferentesOC = "No puede editar una Asistencia que pertenece a otra Oficina Consular";
        private string srtBeneficiario = "BENEFICIARIO";
        private string strPersonaAsistenciaId = "IdPersonaAsistencia";
        #endregion

        #region Eventos

        private void Page_Init(object sender, System.EventArgs e)
        {
            ctrlPageBar1.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPageBar1.Visible = false;
            ctrlPageBar1.PaginaActual = 1;

            ctrlPageBar2.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPageBar2.Visible = false;
            ctrlPageBar2.PaginaActual = 1;
            ctrlToolBarMantenimiento.btnBuscar.Focus();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SeteaControles2();

            HiddenField TipoArchivo = (HiddenField)ctrlUploader1.FindControl("hd_Extension");
            TipoArchivo.Value = ".pdf";

            
            

            Button BtnEliminarE = (Button)ctrlToolBarMantenimiento.FindControl("btnEliminar");
            BtnEliminarE.OnClientClick = "return confirm('¿Desea eliminar el registro Seleccionado?');";

            ctrlToolBarConsulta.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);
            ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);

            this.ctrlToolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
            this.ctrlToolBarConsulta.btnCancelar.Text = "    Limpiar";

            ctrlToolBarMantenimiento.btnNuevoHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonNuevoClick(MyControl_btnNuevo);
            ctrlToolBarMantenimiento.btnEditarHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonEditarClick(MyControl_btnEditar);
            ctrlToolBarMantenimiento.btnEliminarHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonEliminarClick(MyControl_btnEliminar);
            ctrlToolBarMantenimiento.btnGrabarHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonGrabarClick(MyControl_btnGrabar);
            ctrlToolBarMantenimiento.btnCancelarHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonCancelarClick(MyControl_btnCancelar);

            ctrlOficinaConsular1.ddlOficinaConsular.AutoPostBack = true;
            ctrlOficinaConsular1.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsular_SelectedIndexChanged);

            string strFormatofecha = "";
            strFormatofecha = WebConfigurationManager.AppSettings["FormatoFechas"];
            Session["Formatofecha"] = strFormatofecha;

            /* Setea el rango ingreso permitido de fecha de Nacimiento del connacional del control calendario */
            GrdAnotaciones.Visible = true;

            if (!Page.IsPostBack)
            {
                try
                {
                    btnNuevaAsistencia.Visible = false;
                    lblDetalleAsistencia.Visible = false;

                    Session[srtBeneficiario] = Beneficiario_limpiar();

                    Session["StrArchivoSubido"] = "";
                    Session["NombreArchivoUpload"] = "";
                    ViewState.Add("fechaservicio", "");

                    Session["OficinaDelaAsistencia"] = Convert.ToString(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    CargarCambos();
                    MostrarTipoAyuda(Convert.ToInt32(Session["OficinaDelaAsistencia"]));

                    string StrScript = string.Empty;
                    StrScript = @"$(function(){{
                                    DisableTabIndex(1);
                                }});";
                    StrScript = string.Format(StrScript);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DisableTabIndex1", StrScript, true);
                    ctrlToolBarConsulta.btnBuscar.Focus();
                    lblTabConsulta.Focus();

                    lblCO_ddl_TipAyPAHL.Visible = false;
                    if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA)
                    {
                        ddl_TipAyPAHL.Enabled = false;
                        lblCO_ddl_TipAyPAHL.Visible = true;
                    }

                    MyControl_btnNuevo();


                }
                catch (Exception ex)
                {
                    Session["_LastException"] = ex;
                    Response.Redirect("../PageError/GenericErrorPage.aspx");
                }
            }

            Button BtnGrabarE = (Button)ctrlToolBarMantenimiento.FindControl("btnGrabar");
            BtnGrabarE.OnClientClick = "return Validar()";
            
            //if (ddl_TipAyPAH.Visible)
            //{
            //    BtnGrabarE.OnClientClick = "return ValidarPAH()";
            //}
            //else
            //{
            //    BtnGrabarE.OnClientClick = "return ValidarPAHL()";
            //}

            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar };
                GridView[] arrGridView = { GrdAnotaciones, gvBeneficiario };
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }
        }

        void ddlOficinaConsular_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int intOficinaConsularId = Convert.ToInt32(ctrlOficinaConsular1.SelectedValue);
                int intOficinaConsularOtrosId = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);


                
                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA && intOficinaConsularId == 0)
                {
                    ddl_Departamento.Enabled = true;
                    ddl_Provincia.Enabled = true;
                }

                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA && intOficinaConsularId == 0)
                {
                    ddl_Departamento.Enabled = false;
                    ddl_Provincia.Enabled = true;
                }

                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA && intOficinaConsularId == Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    ddl_Provincia.Enabled = true;
                }

                CargarUbicacionGeografica(intOficinaConsularId);

                if (intOficinaConsularId == 0) intOficinaConsularId = intOficinaConsularOtrosId;
                CargarFuncionarios(intOficinaConsularId, 0);
                CargarMoneda(intOficinaConsularId);

                Proceso MiProc = new Proceso();
                int intMonedaid = 0;
                Object[] miArrayPersIdentifBus = new Object[1] { intOficinaConsularId };
                intMonedaid = (Int32)MiProc.Invocar(ref miArrayPersIdentifBus,
                                  "SGAC.BE.MA_MONEDA",
                                  Enumerador.enmAccion.LEER);

                if (intMonedaid != 0 && intOficinaConsularId != 1)
                {
                    ddl_Moneda.SelectedValue = intMonedaid.ToString();
                }

            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            try
            {
                txtNroDocumento.Text = txtNroDocumento.Text.Trim();
                txtPriApellido.Text = txtPriApellido.Text.Trim();
                txtSegApellido.Text = txtSegApellido.Text.Trim();
                ctrlPageBar1.InicializarPaginador();

                ctrlValidacionVentanilla.MostrarValidacion(string.Empty, false);
                lblDetalleAsistencia.Visible = false;
                btnNuevaAsistencia.Visible = false;
                GrdAnatacion2.Visible = false;
                 
                if ((txtNroDocumento.Text.Length > 0) || ((txtPriApellido.Text.Length >= 3) || (txtSegApellido.Text.Length >= 3)))
                {
                    GrdAnotaciones.Visible = false;
                    BindGridPersona(txtNroDocumento.Text.Trim(), txtPriApellido.Text.Trim(), txtSegApellido.Text.Trim());
                }
                else
                {
                    ctrlValidacionVentanilla.MostrarValidacion(Constantes.CONST_VALIDACION_MIN_3_CARACTERES);

                    GrdAnotaciones.DataSource = null;
                    GrdAnotaciones.DataBind();

                    ctrlPageBar1.Visible = false;
                    ctrlPageBar1.PaginaActual = 1;
                    ctrlPageBar1.InicializarPaginador();

                    GrdAnatacion2.DataSource = null;
                    GrdAnatacion2.DataBind();

                    ctrlPageBar2.Visible = false;
                    ctrlPageBar2.PaginaActual = 1;
                    ctrlPageBar2.InicializarPaginador();

                    string strScript = string.Empty;
                    strScript = Util.ActivarTab(0, Constantes.CONST_TAB_INICIAL);
                    Comun.EjecutarScript(Page, strScript);

                }
                updConsulta.Update();
            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }

        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            txtNroDocumento.Text = "";
            txtPriApellido.Text = "";
            txtSegApellido.Text = "";

            GrdAnotaciones.DataSource = null;
            GrdAnotaciones.DataBind();

            ctrlPageBar1.Visible = false;
            ctrlPageBar1.PaginaActual = 1;
            ctrlPageBar1.InicializarPaginador();

            GrdAnatacion2.DataSource = null;
            GrdAnatacion2.DataBind();

            ctrlPageBar2.Visible = false;
            ctrlPageBar2.PaginaActual = 1;
            ctrlPageBar2.InicializarPaginador();

            lblDetalleAsistencia.Visible = false;
            btnNuevaAsistencia.Visible = false;
            MyControl_btnCancelar();

            updMantenimiento.Update();
        }

        void MyControl_btnNuevo()
        {
            Session["StrArchivoSubido"] = "";
            CmdVisualizar.Enabled = false;

            Session["IQueHace"] = 1;
            Session["IAsistenciaId"] = 0;

            ddl_Moneda.SelectedIndex = 0;
            ddl_Estado.SelectedIndex = -1;

            FileUpload botonUpload = (FileUpload)ctrlUploader1.FindControl("FileUploader");
            botonUpload.Enabled = true;

            Button botonsubir = (Button)ctrlUploader1.FindControl("btnUpload");
            botonsubir.Enabled = true;

            Label lblNombreArchivo = (Label)ctrlUploader1.FindControl("lblNombreArchivo");
            lblNombreArchivo.Visible = false;

            System.Web.UI.HtmlControls.HtmlTableCell msjeSucess = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader1.FindControl("msjeSucess");
            msjeSucess.Visible = false;

            System.Web.UI.HtmlControls.HtmlTableCell msjeWarning = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader1.FindControl("msjeWarning");
            msjeWarning.Visible = false;

            System.Web.UI.HtmlControls.HtmlTableCell msjeError = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader1.FindControl("msjeError");
            msjeError.Visible = false;

            BlanqueControles();

            ActivarTool(true, false, true, true, false);

            ActivarControles(true);

            //lblTabRegistro.Text = "Registro";

            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));

            pnlRtnOpcion.Enabled = true;

            MostrarTipoAyuda(Convert.ToInt32(Session["OficinaDelaAsistencia"]));

            ctrlOficinaConsular1.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();
            int intOficinaConsularId = Convert.ToInt32(Session["OficinaDelaAsistencia"]);  // OBTENEMOS EL ID DE LA OFICINA CONSULAR

            CargarUbicacionGeografica(intOficinaConsularId);

            if (intOficinaConsularId != 1)
            {

                Proceso MiProc = new Proceso();
                int intMonedaid = 0;                                                        // ALMACENA EL ID DE LA MONEDA A BUSCAR

                Object[] miArrayPersIdentifBus = new Object[1] { intOficinaConsularId };    //// TRAEMOS LOS DATOS DE LA OFICINA CONSULAR

                intMonedaid = (Int32)MiProc.Invocar(ref miArrayPersIdentifBus,              // TRAEMOS EL ID DE LA MONEDA A BUSCAR
                                                  "SGAC.BE.MA_MONEDA",
                                                  Enumerador.enmAccion.LEER);

                CargarMoneda(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

                if (intMonedaid != 0)
                {
                    ddl_Moneda.SelectedValue = intMonedaid.ToString();
                }

            }

            
            txtNroCaso.Enabled = true;

            CargarFuncionarios(Convert.ToInt32(ctrlOficinaConsular1.SelectedValue), 0);

            ViewState.Add("BeneficiarioAccion", Enumerador.enmAccion.INSERTAR);
            btnAgregarBene.Enabled = true;
            //btnAdicionar.Text = "Adicionar";
            //Limpiar_buscaBeneficiario();

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Convert.ToInt32(ctrlOficinaConsular1.SelectedValue))
            {
                ddl_Provincia.Enabled = false;
                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Convert.ToInt32(Constantes.CONST_OFICINACONSULAR_LIMA))
                {
                    ddl_Departamento.Enabled = true;
                    ddl_Provincia.Enabled = true;
                    ddl_Distrito.Enabled = true;
                }
                else
                {
                    ddl_Departamento.Enabled = false;
                    ddl_Provincia.Enabled = false;
                    ddl_Distrito.Enabled = false;
                }
            }

            txtFecServcio.Enabled = true;

            Int64 intAsistenciaId = 0;
            Int64 intPersonaId = Convert.ToInt64(ViewState["IPersonaAsisId"]);
            if (intPersonaId != 0)
            {
                gvBeneficiario.DataSource = Beneficiario(intAsistenciaId, intPersonaId);
                gvBeneficiario.DataBind();
            }

            ctrlOficinaConsular.SelectedIndex = 0;

            updMantenimiento.Update();
        }

        void MyControl_btnEliminar()
        {
            BE.MRE.RE_ASISTENCIA ObjAsisBE = new BE.MRE.RE_ASISTENCIA();
            string strMensaje = string.Empty;
            Proceso MiProc = new Proceso();

            int intPersonaId = Convert.ToInt32(ViewState["IPersonaAsisId"]);

            PersonaAsistenciaMantenimientoBL Asistencia = new PersonaAsistenciaMantenimientoBL();

            string StrScript = string.Empty;
            int intAsistenciaID;
            int IntRpta = 0;

            intAsistenciaID = Convert.ToInt32(Session["intAsistenciaID"]);

            ObjAsisBE.asis_iAsistenciaId = intAsistenciaID;
            ObjAsisBE.asis_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            ObjAsisBE.asis_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
            ObjAsisBE.asis_sOficinaConsularId = Convert.ToInt16(ctrlOficinaConsular1.SelectedValue);

            IntRpta = Asistencia.Eliminar(ObjAsisBE);

            Object[] miArrayPersIdentifBus = new Object[1] { ObjAsisBE };

            IntRpta = (int)MiProc.Invocar(ref miArrayPersIdentifBus,
                                          "SGAC.BE.RE_ASISTENCIA",
                                          Enumerador.enmAccion.ELIMINAR,
                                          Enumerador.enmAplicacion.WEB);

            if (IntRpta > 0)
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "ASISTENCIA", "La asistencia se eliminó con éxito.", false, 190, 250);
                Comun.EjecutarScript(Page, StrScript);

                //lblNombreArchivo.Text = "";

                Label lblNombreArchivo = (Label)ctrlUploader1.FindControl("lblNombreArchivo");
                lblNombreArchivo.Visible = true;

                CmdVisualizar.Enabled = false;

                Session["StrArchivoSubido"] = "";

                BlanqueControles();
                txtFecServcio.Enabled = false;

                updMantenimiento.Update();

                StrScript = string.Empty;
                StrScript = @"$(function(){{
                                    MoveTabIndex(0);
                                }});";
                StrScript = string.Format(StrScript);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex0", StrScript, true);

                StrScript = string.Empty;
                StrScript = @"$(function(){{
                                    DisableTabIndex(1);
                                }});";
                StrScript = string.Format(StrScript);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "DisableTabIndex1", StrScript, true);

                MostrarListaAsistencia(intPersonaId, "");

                updConsulta.Update();
            }
            else
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ASISTENCIA", "Error - No se realizó la operación.", false, 190, 250);
                Comun.EjecutarScript(Page, StrScript);
            }
        }

        private bool verificarMontoCero()
        {
            bool resultado = true;
            int OficinaDelregistro = 0;
            OficinaDelregistro = Convert.ToInt32(Session["OficinaDelaAsistencia"]);
            string StrScript = string.Empty;

            if (OficinaDelregistro == Constantes.CONST_OFICINACONSULAR_LIMA) // PAH
            {
                if (Convert.ToDouble(txtMonto.Text) == 0)
                {
                    resultado = false;
                    
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "ASISTENCIA", "No puede ingresar monto 0", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                }
                
            }
            if (txtFecServcio.Text.Trim() != "")
            {
                if (Comun.EsFecha(txtFecServcio.Text.Trim()) == false)
                {
                    resultado = false;
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "ASISTENCIA", Constantes.CONST_VALIDACION_FECHA_NO_VALIDA, false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                }
            }
            
            return resultado;
            
        }

        void MyControl_btnGrabar()
        {
            try
            {
                if (verificarMontoCero())
                {
                    string StrScript = string.Empty;
                    Int64 intPersonaId = Convert.ToInt64(ViewState["IPersonaAsisId"]);

                    TextBox txt = (TextBox)txtFecServcio.FindControl("TxtFecha");
                    txt.BorderColor = Color.Gray;

                    if (!txtFecServcio.ToDateTime())
                    {
                        txt.BorderWidth = 1;
                        txt.BorderStyle = BorderStyle.Solid;
                        txt.BorderColor = Color.Red;
                        updMantenimiento.Update();
                        return;
                    }

                    if (Grabar() == true)
                    {
                        CmdVisualizar.Enabled = false;

                        StrScript = string.Empty;
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "ASISTENCIA", Constantes.CONST_MENSAJE_EXITO, false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);

                        BlanqueControles();

                        txtFecServcio.Enabled = false;
                        updMantenimiento.Update();

                        StrScript = string.Empty;
                        StrScript = @"$(function(){{
                                    MoveTabIndex(0);
                                }});";
                        StrScript = string.Format(StrScript);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex0", StrScript, true);

                        StrScript = string.Empty;
                        StrScript = @"$(function(){{
                                    DisableTabIndex(1);
                                }});";
                        StrScript = string.Format(StrScript);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "DisableTabIndex1", StrScript, true);

                        MostrarListaAsistencia(intPersonaId, "");
                    }
                    else
                    {
                        StrScript = string.Empty;
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "ASISTENCIA", "No se pudo guardar la asistencia", false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);
                    }

                    updConsulta.Update();
                }
                
            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }

        }

        void MyControl_btnCancelar()
        {
            Session["StrArchivoSubido"] = "";

            txtFecServcio.Enabled = false;
            
            Label lblNombreArchivo = (Label)ctrlUploader1.FindControl("lblNombreArchivo");
            lblNombreArchivo.Visible = true;

            CmdVisualizar.Enabled = false;

            ActivarControles(false);

            ddl_Moneda.SelectedIndex = 0;
            ddl_Estado.SelectedIndex = -1;

            BlanqueControles();

            System.Web.UI.HtmlControls.HtmlTableCell msjeSucess = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader1.FindControl("msjeSucess");
            msjeSucess.Visible = false;

            System.Web.UI.HtmlControls.HtmlTableCell msjeWarning = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader1.FindControl("msjeWarning");
            msjeWarning.Visible = false;

            System.Web.UI.HtmlControls.HtmlTableCell msjeError = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader1.FindControl("msjeError");
            msjeError.Visible = false;

            Session["IQueHace"] = 3;

            string StrScript = string.Empty;
            StrScript = @"$(function(){{
                            DisableTabIndex(1);
                        }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "DisableTabIndex1", StrScript, true);

            StrScript = string.Empty;
            StrScript = @"$(function(){{
                            MoveTabIndex(0);
                        }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex0", StrScript, true);
            ctrlUploader1.LimpiarMensaje();
            

            updConsulta.Update();

            Comun.EjecutarScript(Page, Util.ActivarTab(0, Constantes.CONST_TAB_INICIAL) + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));

        }

        void MyControl_btnEditar()
        {
            if (ddl_Estado.SelectedValue == Constantes.CONST_ESTADO_ASISTENCIA_CONCLUIDO.ToString())
            {
                ActivarControles(false);
                string StrScript = string.Empty;
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ASISTENCIA", "No se puede editar asistencia, se encuentra " + ddl_Estado.SelectedItem.ToString(), false, 190, 250);
                Comun.EjecutarScript(Page, StrScript);
                return;
            }
            else
            {

                DataTable dtAsistencia = new DataTable();
                dtAsistencia = (DataTable)Session["dtAsistencia"];
                int intAsistenciaId = Convert.ToInt32(Session["intAsistenciaID"]);
                int intIdOficinaAsis = BuscarIdOficina(dtAsistencia, intAsistenciaId);

                string StrScript = string.Empty;
                if (intIdOficinaAsis != Convert.ToInt32(ctrlOficinaConsular1.SelectedValue))
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "ASISTENCIA", strMensajeDiferentesOC, false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);

                    updMantenimiento.Update();
                    return;
                }

                Session["IQueHace"] = 2;

                FileUpload fuUploader = (FileUpload)ctrlUploader1.FindControl("FileUploader");
                fuUploader.Enabled = true;

                Button BtnSubir = (Button)ctrlUploader1.FindControl("btnUpload");
                BtnSubir.Enabled = true;

                txtNroCaso.Enabled = true;

                ActivarControles(true);
                gvBeneficiario.Enabled = true;

                Int32 idFuncionario = Convert.ToInt32(ddlFuncionario.SelectedValue);
                
                
                int intOficinaConsularId = Convert.ToInt32(Session["OficinaDelaAsistencia"]);  // OBTENEMOS EL ID DE LA OFICINA CONSULAR
                if (intOficinaConsularId != Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    CargarFuncionarios(Convert.ToInt32(ctrlOficinaConsular1.SelectedValue), 0); 
                }


                bool bolbuscar = false;
                
                for (int i = 0; i < ddlFuncionario.Items.Count; i++)
                {
                    ddlFuncionario.SelectedIndex = i;
                    if (idFuncionario == Convert.ToInt32(ddlFuncionario.SelectedValue))
                    {
                        bolbuscar = true;
                        break;
                    }

                }
                if (!bolbuscar)
                {
                    ddlFuncionario.SelectedIndex = -1;
                    StrScript = string.Empty;
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "ASISTENCIA", "El Funcionario no esta Activo", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                }

                if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Convert.ToInt32(ctrlOficinaConsular1.SelectedValue))
                {
                    ddl_Provincia.Enabled = false;
                    if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Convert.ToInt32(Constantes.CONST_OFICINACONSULAR_LIMA))
                    {
                        ddl_Departamento.Enabled = true;
                        ddl_Provincia.Enabled = true;
                        ddl_Departamento.Enabled = true;
                    }
                    else
                    {
                        ddl_Departamento.Enabled = false;
                        ddl_Provincia.Enabled = false;
                        ddl_Departamento.Enabled = false;
                    }
                }

                ActivarTool(true, false, true, true, true);
                txtFecServcio.Enabled = true;
                ViewState.Add("BeneficiarioAccion", Enumerador.enmAccion.MODIFICAR);
                btnAgregarBene.Enabled = true;
                updMantenimiento.Update();
            }
        }

        protected void ddl_Pais_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillWebCombo(comun_Part3.ObtenerProvincias(Session, ddl_Departamento.SelectedValue.ToString()), ddl_Provincia, "ubge_vProvincia", "ubge_cUbi02");
            ddl_Ciudad_SelectedIndexChanged(sender, e);
            
        }

        protected void ddl_Ciudad_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillWebCombo(comun_Part3.ObtenerDistritos(Session, ddl_Departamento.SelectedValue.ToString(), ddl_Provincia.SelectedValue.ToString()), ddl_Distrito, "ubge_vDistrito", "ubge_cUbi03");
        }

        protected void GrdAnotaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {

                if (e.CommandName == "Seleccionar" || e.CommandName == "Select")
                {
                    int index = Convert.ToInt32(e.CommandArgument);
                    ViewState.Add("indexAnterior", index);

                    string BlancoHex = "#FFFFFF";
                    string grisHex = "#CCCCCC";
                    string SeleHex = "#656873"; /*"#D0DEF0";*/

                    int fila = 0;
                    foreach (GridViewRow row in GrdAnotaciones.Rows)
                    {
                        if (fila % 2 == 0) row.BackColor = System.Drawing.ColorTranslator.FromHtml(BlancoHex);
                        if (fila % 2 != 0) row.BackColor = System.Drawing.ColorTranslator.FromHtml(grisHex);
                        fila++;
                    }

                    GrdAnotaciones.Rows[index].BackColor = System.Drawing.ColorTranslator.FromHtml(SeleHex);

                    //---------------------
                    string strPersonaDocumento = HttpUtility.HtmlDecode(GrdAnotaciones.Rows[index].Cells[1].Text).Trim();

                    if (strPersonaDocumento == string.Empty)
                    {
                        string StrScript = string.Empty;
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ASISTENCIA", "Persona sin documento de identidad, por favor completar datos en RUNE", false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);

                        StrScript = @"$(function(){{
                                MoveTabIndex(1);
                            }});";
                        StrScript = string.Format(StrScript);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex2", StrScript, true);

                        return;

                    }
                    
                    else 
                    { 
                    
                    Label lblNombreArchivo = (Label)ctrlUploader1.FindControl("lblNombreArchivo");
                    lblNombreArchivo.Text = "";



                    Comun.EjecutarScript(Page, Util.HabilitarSoloTab(1));

                    int intPersonaId = Convert.ToInt32(GrdAnotaciones.Rows[index].Cells[0].Text);

                    string strPersonaNombre = HttpUtility.HtmlDecode(GrdAnotaciones.Rows[index].Cells[2].Text) + " " + 
                                              HttpUtility.HtmlDecode(GrdAnotaciones.Rows[index].Cells[3].Text) + ", " +
                                              HttpUtility.HtmlDecode(GrdAnotaciones.Rows[index].Cells[4].Text);

                    lblNombre.Text = "";

                    ViewState.Add("IPersonaAsisId", intPersonaId);

                    Session["StrPersonaNombre"] = strPersonaNombre;

                    MostrarListaAsistencia(Convert.ToInt64(ViewState["IPersonaAsisId"]), Convert.ToString(Session["StrPersonaNombre"]));
                    CargarFuncionarios(Convert.ToInt32(ctrlOficinaConsular1.SelectedValue), 0);

                    FillWebCombo(new SGAC.Configuracion.Sistema.BL.CircunscripcionConsultasBL().Consultar(Convert.ToInt32(ctrlOficinaConsular1.SelectedValue)),
                        ddlCircunscripcion, "circ_vNombre", "circ_sCircunscripcionId");

                   
                    //MyControl_btnNuevo();
                    updMantenimiento.Update();
                    updConsulta.Update();

                    }


                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + ex.Message + "');", true);
            }
        }

        protected void GrdAnatacion2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string strFormatofecha = "";
                strFormatofecha = Convert.ToString(Session["Formatofecha"]);

                string Date = e.Row.Cells[2].Text;  //Campo donde tienes tu fecha
                e.Row.Cells[2].Text = Comun.FormatearFecha(Date).ToString(strFormatofecha);
            }
        }

        private int ObtenerIndiceColumnaGrilla(GridView grid, string col)
        {
            string field = string.Empty;
            for (int i = 0; i < grid.Columns.Count; i++)
            {

                if (grid.Columns[i].GetType() == typeof(BoundField))
                {
                    field = ((BoundField)grid.Columns[i]).DataField.ToLower();
                }
                else if (grid.Columns[i].GetType() == typeof(TemplateField))
                {
                    field = ((TemplateField)grid.Columns[i]).HeaderText.ToLower();
                }

                if (field == col.ToLower())
                {
                    return i;
                }

                field = string.Empty;
            }
            return -1;
        }


        protected void GrdAnatacion2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dtAsistencia = new DataTable();
            int index = Convert.ToInt32(e.CommandArgument);
            int intAsistenciaId = Convert.ToInt32(GrdAnatacion2.Rows[index].Cells[0].Text);
            Session["IAsistenciaId"] = intAsistenciaId;
            dtAsistencia = (DataTable)Session["dtAsistencia"];
            Session["intAsistenciaID"] = intAsistenciaId;
            
            ddl_Departamento.Enabled = false;
            
            int intOficinaConsularLogin = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            int intOficinaConsularSelec = Convert.ToInt32(GrdAnatacion2.DataKeys[index].Values["sOficinaConsularId"]);

            ViewState.Add(strPersonaAsistenciaId, Convert.ToInt64(GrdAnatacion2.DataKeys[index].Values["iPersonaId"]));

            int IfuncionarioId = Convert.ToInt32(GrdAnatacion2.DataKeys[index].Values["asis_iFuncionarioId"]);
            CargarFuncionarios(intOficinaConsularSelec, IfuncionarioId);

            FillWebCombo(new SGAC.Configuracion.Sistema.BL.CircunscripcionConsultasBL().Consultar(Convert.ToInt32(GrdAnatacion2.DataKeys[index].Values["sOficinaConsularOrigenId"])),
                        ddlCircunscripcion, "circ_vNombre", "circ_sCircunscripcionId");


            

            try
            {
                ddlFuncionario.SelectedValue = IfuncionarioId.ToString();
            }
            catch
            {
                string strMensajeFuncionarioNoValido = "El funcionario asignado a la asistencia no se encuentra registrado.";
                string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ASISTENCIA DETALLE", strMensajeFuncionarioNoValido);
                Comun.EjecutarScript(Page, strScript);
                return;
            }

            Button BtnModificar = (Button)ctrlToolBarMantenimiento.FindControl("btnEditar");
            BtnModificar.Enabled = true;
            btnAgregarBene.Enabled = false;

            string habilitar = string.Empty;

            txtFecServcio.Enabled = false;
            if (e.CommandName == "Consultar")
            {
                
                ctrlOficinaConsular1.Cargar(true, true, " - OTROS - ");
                ctrlOficinaConsular.Cargar(true, true);

                MuestraSegundoTab(dtAsistencia, intAsistenciaId);
                
                consultarAsistencia(index);
                
                ActivarTool(true, false, false, true, false);

                if (intOficinaConsularSelec != intOficinaConsularLogin)
                {
                    BtnModificar.Enabled = false;
                }
                else
                {
                        btnAgregarBene.Enabled = false;
                        gvBeneficiario.Enabled = false;
                }

                //lblTabRegistro.Text = "Detalle";

                habilitar = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);


            }
            else if (e.CommandName == "Editar")
            {
                

                int intIdOficinaAsis = BuscarIdOficina(dtAsistencia, intAsistenciaId);

                if (intOficinaConsularSelec != intOficinaConsularLogin)
                {
                    ctrlValidacionVentanilla.MostrarValidacion(strMensajeDiferentesOC);
                    updMantenimiento.Update();
                    return;
                }

                MuestraSegundoTab(dtAsistencia, intAsistenciaId);
                ActivarTool(true, false, true, true, true);

                

                if (ddl_Estado.SelectedValue == Constantes.CONST_ESTADO_ASISTENCIA_CONCLUIDO.ToString())
                {
                    consultarAsistencia(index);
                    btnAgregarBene.Enabled = false;
                    gvBeneficiario.Enabled = false;
                    ActivarTool(true, false, false, true, false);
    
                    string StrScript = string.Empty;
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ASISTENCIA", "No se puede editar, se encuentra CONCLUIDO", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);

                    StrScript = @"$(function(){{
                                MoveTabIndex(1);
                            }});";
                    StrScript = string.Format(StrScript);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex2", StrScript, true);

                    //return;
                }
                else
                {
                    EditarAsistencia(index);
                    btnAgregarBene.Enabled = true;
                    gvBeneficiario.Enabled = true;
                }
                ViewState.Add("BeneficiarioAccion", Enumerador.enmAccion.MODIFICAR);
                //lblTabRegistro.Text = "Modifica";

                habilitar = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);

                ddl_Departamento.Enabled = false;
                ddl_Provincia.Enabled = false;
                ddl_Distrito.Enabled = false;

            }

            Comun.EjecutarScript(Page, habilitar);

            MostrarTipoAyuda(Convert.ToInt32(GrdAnatacion2.DataKeys[index].Values["sOficinaConsularOrigenId"]));

            updMantenimiento.Update();
        }

        private void consultarAsistencia(int index)
        {
            ctrlOficinaConsular1.SelectedValue = GrdAnatacion2.DataKeys[index].Values["sOficinaConsularId"].ToString().Trim();
            ctrlOficinaConsular1.Enabled = false;
            
            FileUpload fuUploader = (FileUpload)ctrlUploader1.FindControl("FileUploader");
            fuUploader.Enabled = false;
            
            Button BtnSubir = (Button)ctrlUploader1.FindControl("btnUpload");
            BtnSubir.Enabled = false;
            
            System.Web.UI.HtmlControls.HtmlTableCell msjeSucess = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader1.FindControl("msjeSucess");
            msjeSucess.Visible = false;
            
            System.Web.UI.HtmlControls.HtmlTableCell msjeWarning = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader1.FindControl("msjeWarning");
            msjeWarning.Visible = false;
            
            System.Web.UI.HtmlControls.HtmlTableCell msjeError = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader1.FindControl("msjeError");
            msjeError.Visible = false;
            
            txtNroCaso.Enabled = false;
            
            ActivarControles(false);

            Button BtnModificar = (Button)ctrlToolBarMantenimiento.FindControl("btnEditar");
            BtnModificar.Enabled = false;

            if (GrdAnatacion2.DataKeys[index].Values["sOficinaConsularId"].ToString() == Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString())
            {
                BtnModificar.Enabled = true;
            }

        }

        private void EditarAsistencia(int index)
        {
            ctrlOficinaConsular1.SelectedValue = GrdAnatacion2.DataKeys[index].Values["sOficinaConsularId"].ToString();
            Comun.EjecutarScript(Page, Util.HabilitarTab(1));

            FileUpload fuUploader = (FileUpload)ctrlUploader1.FindControl("FileUploader");
            fuUploader.Enabled = true;

            Button BtnSubir = (Button)ctrlUploader1.FindControl("btnUpload");
            BtnSubir.Enabled = true;
            
            System.Web.UI.HtmlControls.HtmlTableCell msjeSucess = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader1.FindControl("msjeSucess");
            msjeSucess.Visible = false;
            
            System.Web.UI.HtmlControls.HtmlTableCell msjeWarning = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader1.FindControl("msjeWarning");
            msjeWarning.Visible = false;
            
            System.Web.UI.HtmlControls.HtmlTableCell msjeError = (System.Web.UI.HtmlControls.HtmlTableCell)ctrlUploader1.FindControl("msjeError");
            msjeError.Visible = false;
            
            txtNroCaso.Enabled = true;
            
            MyControl_btnEditar();
            
            updConsulta.Update();
            
            ActivarControles(true);
        }

        protected void ddl_TipAyPAH_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_TipAyPAHL.SelectedIndex = 0;
            ddl_Otros.Focus();
        }

        protected void ddl_TipAyPAHL_SelectedIndexChanged(object sender, EventArgs e)
        {
            int intOficinaConsularId = Convert.ToInt32(ctrlOficinaConsular1.SelectedValue);
            if (intOficinaConsularId == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ddl_TipAyPAHL.SelectedIndex = 0;
                string strScript = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "ASISTENCIA", "La Cancillería debe registrar tipo de asistencia PAH.");
                Comun.EjecutarScript(Page, strScript);
            }
            else
            {
                ddl_TipAyPAH.SelectedIndex = 0;
            }
        }

        protected void MyUserControlUploader1Event_Click(object sender, EventArgs e)
        {
            string StrScript = string.Empty;

            ViewState.Add("fechaservicio", txtFecServcio.Value().ToString());

            CmdVisualizar.Enabled = false;
            Label lblNombreArchivo = (Label)ctrlUploader1.FindControl("lblNombreArchivo");
            lblNombreArchivo.Visible = true;
            if (lblNombreArchivo.Text != "") CmdVisualizar.Enabled = true;

            Session["NuevoRegistro"] = false;
            Session["StrArchivoSubido"] = ctrlUploader1.FileName;

            hidNomAdjFile.Value = ctrlUploader1.FileName;
            
            StrScript = @"$(function(){{
                                MoveTabIndex(1);
                            }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex2", StrScript, true);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {            
            Label strNombreArchivo1 = (Label)ctrlUploader1.FindControl("lblNombreArchivo");

            string strNombreArchivo = strNombreArchivo1.Text;

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
                Int64 iActuacionDetalleId = Convert.ToInt64(Session[Constantes.CONST_SESION_ACTUACIONDET_ID]);

                string strMision = Documento.GetMisionActuacionDetalle(iActuacionDetalleId);
                string strAnio = strNombreArchivo.Substring(1, 4);
                string strMes = strNombreArchivo.Substring(6, 2);
                string strDia = strNombreArchivo.Substring(9, 2);
                
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

                string strfilePath = Path.Combine(strpathAnioMesDia, strNombreArchivo.Substring(12, strNombreArchivo.Length - 12));
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
                    Descarga objDescarga = new Descarga();
                    objDescarga.Download(strRuta, strNombreArchivo, true);
                    objDescarga = null;

                }
                catch (Exception ex)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ASISTENCIA",
                        "El archivo no se pudo abrir. Vuelva a intentarlo." +
                        "\n(" + ex.Message + ")");
                    Comun.EjecutarScript(Page, strScript);
                }
            }
        }

        protected void txtNroDocumento_TextChanged(object sender, EventArgs e)
        {
            ctrlToolBarConsulta_btnBuscarHandler();
        }

        //protected void txtPriApellido_TextChanged(object sender, EventArgs e)
        //{
        //    //ctrlToolBarConsulta_btnBuscarHandler();
        //}

        //protected void txtSegApellido_TextChanged(object sender, EventArgs e)
        //{
        //    //ctrlToolBarConsulta_btnBuscarHandler();
        //}

        protected void ctrlPageBar1_Click(object sender, EventArgs e)
        {
            BindGridPersona(txtNroDocumento.Text.Trim(), txtPriApellido.Text.Trim(), txtSegApellido.Text.Trim());
            updConsulta.Update();
        }

        protected void ctrlPageBar2_Click(object sender, EventArgs e)
        {
            MostrarListaAsistencia(Convert.ToInt64(ViewState["IPersonaAsisId"]), Convert.ToString(Session["StrPersonaNombre"]));
            updConsulta.Update();
        }

        #endregion

        #region Metodos

        void MostrarTipoAyuda(Int32 sIdOficinaCOnsular)
        {
            int OficinaDelregistro = 0;
            OficinaDelregistro = sIdOficinaCOnsular;

            if (OficinaDelregistro == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                lblTituloOfConsular.Text = "Programa de Asistencia Humanitaria - Cancillería (PAH)";
                
                ddl_TipAyPAHL.Visible = false;
                ddl_TipAyPAH.Visible = true;
                ddl_Otros.Visible = false;

                lblTipoAyuda.Visible = false;
                lblTipPAH.Visible = true;
                LblTipoAnot.Visible = false;

                lblOficinaConsular.Visible=false;
                ctrlOficinaConsular1.Visible=false;
                lbl_ctrlOficinaConsular1.Visible=false;

                Label7.Visible = true;
                lblCO_ddl_TipAyPAHL.Visible = false;

                Label1.Text = "Departamento :";
                lblCiudad.Text = "Provincia :";
                lblDistrito.Text = "Distrito :";

                Label9.Visible = false;

                lblHoraInicio.Visible = false; 
                txtHoraInicio.Visible = false;
                lblFHoraInicio.Visible = false;
                lbl_txtHoraInicio.Visible = false;

                
                lblHoraFin.Visible = false; 
                txtHoraFin.Visible = false;
                lblFHoraFin.Visible = false;
                lbl_txtHoraFin.Visible = false;

                lblFuncionario.Visible = false;
                ddlFuncionario.Visible = false;
                lblFunc.Visible = false;

                lblNroCaso.Visible = true;
                txtNroCaso.Visible = true;

                TR_Oficina.Attributes.Remove("style");
                TR_Cir.Attributes.Add("style","display:none;");   
            }
            else
            {
                lblTituloOfConsular.Text = "Programa de Asistencia Legal Humanitaria y Servicios Consulares - Consulado (PALH)";

                TR_Oficina.Attributes.Add("style", "display:none;");
                TR_Cir.Attributes.Remove("style");
                ddl_TipAyPAHL.Visible = true;
                ddl_TipAyPAH.Visible = false;
                ddl_Otros.Visible = false;

                lblOficinaConsular.Visible = true;
                ctrlOficinaConsular1.Visible = true;
                lbl_ctrlOficinaConsular1.Visible = true;
                
                Label1.Text = "Continente :";
                lblCiudad.Text = "País :";
                lblDistrito.Text = "Ciudad :";


                lblTipoAyuda.Visible = true;
                lblTipPAH.Visible = false;
                LblTipoAnot.Visible = false;

                Label7.Visible = false;
                lblCO_ddl_TipAyPAHL.Visible = true;

                Label9.Visible = false;

                lblHoraInicio.Visible = true;
                txtHoraInicio.Visible = true;
                lblFHoraInicio.Visible = true;
                lbl_txtHoraInicio.Visible = true;

                lblHoraFin.Visible = true;
                txtHoraFin.Visible = true;
                lblFHoraFin.Visible = true;
                lbl_txtHoraFin.Visible = true;

                lblFuncionario.Visible = true;
                ddlFuncionario.Visible = true;
                lblFunc.Visible = true;
                lblNroCaso.Visible = false;
                txtNroCaso.Visible = false;
            }

            updMantenimiento.Update();
        }

        DataTable CrearDTTemporal()
        {
            DataTable Dt = new DataTable();
            Util MiFun = new Util();

            string[,] Columnas = new string[5, 2] { {"iPersonaId", "System.Int32"}, 
                                                    {"vNroDocumento", "System.String"},
                                                    {"vApellidoPaterno", "System.String"},
                                                    {"vApellidoMaterno", "System.String"},
                                                    {"vNombres", "System.String"}
                                                  };

            Dt = MiFun.DataTableCrear(Columnas, "MiTabla");

            //AGREGAMOS UNA FILA EN BLANCO PARA PODER MOSTRAR EL GRIDVIEW
            Dt.Rows.Add(Dt.NewRow());
            return Dt;
        }

        private void BindGridPersona(string StrNroDoc, string StrApePat, string StrApeMat)
        {
            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int IntTamañoPagina = Constantes.CONST_PAGE_SIZE_PERSONA_REASIGNACION;

            DataTable DtPersona = new DataTable();

            PersonaConsultaBL PersonaBL = new PersonaConsultaBL();

            Proceso MiProc = new Proceso();
            Object[] miArray = new Object[8] { 1,
                                                   StrNroDoc,
                                                   StrApePat,
                                                   StrApeMat,                                             
                                                   ctrlPageBar1.PaginaActual.ToString(),
                                                   IntTamañoPagina,
                                                   IntTotalCount,
                                                   IntTotalPages
            };

            DtPersona = (DataTable)MiProc.Invocar(ref miArray,
                                                  "SGAC.BE.RE_PERSONA",
                                                  Enumerador.enmAccion.CONSULTAR,
                                                  Enumerador.enmAplicacion.WEB);

            IntTotalCount = int.Parse(miArray[6].ToString());
            IntTotalPages = int.Parse(miArray[7].ToString());

            if (DtPersona.Rows.Count > 0)
            {
                ctrlValidacionVentanilla.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + IntTotalCount, true, Enumerador.enmTipoMensaje.INFORMATION);
                
                GrdAnotaciones.DataSource = DtPersona;
                GrdAnotaciones.DataBind();
                GrdAnotaciones.Visible = true;
                GrdAnatacion2.Visible = false;
                
                ctrlPageBar1.TotalResgistros = (int)miArray[6];
                ctrlPageBar1.TotalPaginas = (int)miArray[7];
                ctrlPageBar1.TotalResgistros = IntTotalCount;
                ctrlPageBar1.TotalPaginas = IntTotalPages;
                ctrlPageBar1.Visible = false;

                if (IntTotalPages > 1)
                {
                    ctrlPageBar1.Visible = true;
                }

                TextBox txtPage = (TextBox)ctrlPageBar1.FindControl("txtPagina");
            }
            else
            {
                ctrlPageBar1.Visible = false;
                ctrlValidacionVentanilla.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                
                GrdAnotaciones.Visible = false;
                GrdAnatacion2.Visible = false;

                txtNroDocumento.Text = "";
                txtPriApellido.Text = "";
                txtSegApellido.Text = "";
            }
        }

        int BuscarIdOficina(DataTable DtAsistencia, int intAsistenciaId)
        {
            int IntFila = 0;
            int IdOficina = 0;

            for (IntFila = 0; IntFila <= DtAsistencia.Rows.Count - 1; IntFila++)
            {
                if (Convert.ToInt32(DtAsistencia.Rows[IntFila]["iAsistenciaId"].ToString()) == (intAsistenciaId))
                {
                    IdOficina = Convert.ToInt32(DtAsistencia.Rows[IntFila]["sOficinaConsularId"].ToString());
                    break;
                }
            }
            return IdOficina;
        }

        void MuestraSegundoTab(DataTable DtAsistencia, int intAsistenciaId)
        {
            double Monto = 0;
            DateTime FechaServicio;
            int IntFila = 0;
            String FormatoFechas = ConfigurationManager.AppSettings["FormatoFechas"].ToString();

            gvBeneficiario.DataSource = Beneficiario(intAsistenciaId, Convert.ToInt64(ViewState[strPersonaAsistenciaId]));
            gvBeneficiario.DataBind();

            for (IntFila = 0; IntFila <= DtAsistencia.Rows.Count - 1; IntFila++)
            {
                if (Convert.ToInt32(DtAsistencia.Rows[IntFila]["iAsistenciaId"].ToString()) == (intAsistenciaId))
                {
                    FechaServicio = Comun.FormatearFecha(DtAsistencia.Rows[IntFila]["dFecServicio"].ToString().Trim());

                    this.ddl_TipAyPAHL.SelectedIndex = 0;
                    this.ddl_TipAyPAH.SelectedIndex = 0;
                    this.ddl_Otros.SelectedIndex = 0;

                    if (DtAsistencia.Rows[IntFila]["sTipAsistencia"].ToString() == Convert.ToInt16(Enumerador.enmServicio.PAHL).ToString()) // SI EL REGISTRO ES PALH
                    {
                        if (System.Convert.IsDBNull(DtAsistencia.Rows[IntFila]["sTipoServId"]) != true)
                        {
                            this.ddl_TipAyPAHL.SelectedValue = DtAsistencia.Rows[IntFila]["sTipoServId"].ToString();
                        }
                        if (System.Convert.IsDBNull(DtAsistencia.Rows[IntFila]["sOtrosServiciosId"]) != true)
                        {
                            this.ddl_Otros.SelectedValue = DtAsistencia.Rows[IntFila]["sOtrosServiciosId"].ToString(); 
                        }
                    }

                    if (DtAsistencia.Rows[IntFila]["sTipAsistencia"].ToString() == Convert.ToInt16(Enumerador.enmServicio.PAH).ToString()) // SI EL REGISTRO ES PAH
                    {
                        if (System.Convert.IsDBNull(DtAsistencia.Rows[IntFila]["sTipoServId"]) != true) 
                        { 
                            this.ddl_TipAyPAH.SelectedValue = DtAsistencia.Rows[IntFila]["sTipoServId"].ToString(); 
                        }
                        if (System.Convert.IsDBNull(DtAsistencia.Rows[IntFila]["sOtrosServiciosId"]) != true) 
                        { 
                            this.ddl_Otros.SelectedValue = DtAsistencia.Rows[IntFila]["sOtrosServiciosId"].ToString(); 
                        }
                    }

                    DateTime fecha = Comun.FormatearFecha(DtAsistencia.Rows[IntFila]["dFecServicio"].ToString());
                    this.txtFecServcio.Text = fecha.ToString(FormatoFechas);
                    this.txtNroCaso.Text = DtAsistencia.Rows[IntFila]["vNroCaso"].ToString();
                    this.txtHoraInicio.Text = DtAsistencia.Rows[IntFila]["vHoraInicio"].ToString();
                    this.txtHoraFin.Text = DtAsistencia.Rows[IntFila]["vHoraFin"].ToString();
                    this.txtDirURL.Text = DtAsistencia.Rows[IntFila]["vDirURL"].ToString();

                    if (Convert.ToString(DtAsistencia.Rows[IntFila]["asis_sCircunscripcionId"]).ToString() != "")
                        this.ddlCircunscripcion.SelectedValue = Convert.ToString(DtAsistencia.Rows[IntFila]["asis_sCircunscripcionId"]);

                    if (DtAsistencia.Rows[IntFila]["sTipAsistencia"].ToString() == Convert.ToInt16(Enumerador.enmServicio.PAH).ToString()) // SI EL REGISTRO ES PAH
                    {
                        if (Convert.ToInt32(DtAsistencia.Rows[IntFila]["sOficinaConsularId"].ToString()) == Convert.ToInt32(this.ctrlOficinaConsular1.SelectedValue))
                        {
                            this.ctrlOficinaConsular1.SelectedValue = DtAsistencia.Rows[IntFila]["sOficinaConsularId"].ToString();
                        }
                        this.ctrlOficinaConsular.SelectedValue = DtAsistencia.Rows[IntFila]["sOficinaConsularId"].ToString();

                        CargarMoneda(Convert.ToInt32(DtAsistencia.Rows[IntFila]["sOficinaConsularOrigenId"]));
                    }
                    else
                    {
                        this.ctrlOficinaConsular1.SelectedValue = DtAsistencia.Rows[IntFila]["sOficinaConsularOrigenId"].ToString();
                        this.ctrlOficinaConsular.SelectedValue = DtAsistencia.Rows[IntFila]["sOficinaConsularId"].ToString();
                        CargarMoneda(Convert.ToInt32(DtAsistencia.Rows[IntFila]["sOficinaConsularId"]));
                    }
                    Monto = Convert.ToDouble(DtAsistencia.Rows[IntFila]["FMontoServ"].ToString());
                    this.txtMonto.Text = Monto.ToString("#0.00");
                    this.txtDescAnot.Text = DtAsistencia.Rows[IntFila]["vObservaciones"].ToString();
                    
                    ddl_Moneda.SelectedValue = DtAsistencia.Rows[IntFila]["sMonedaId"].ToString();
                    ddl_Estado.SelectedValue = DtAsistencia.Rows[IntFila]["sEstado"].ToString();

                    CargarUbicacionGeografica(Convert.ToInt32(DtAsistencia.Rows[IntFila]["sOficinaConsularId"]));
                    string vdistrito = DtAsistencia.Rows[IntFila]["cUbigeo"].ToString();
                    ddl_Distrito.SelectedValue = vdistrito.Substring(4);

                    string nombreArchivo = string.Empty;
                    nombreArchivo = DtAsistencia.Rows[IntFila]["vNombre"].ToString();

                    CmdVisualizar.Enabled = false;
                    Label lblNombreArchivo = (Label)ctrlUploader1.FindControl("lblNombreArchivo");
                    lblNombreArchivo.Text = "";
                    if (nombreArchivo.Length > 0)
                    {
                        lblNombreArchivo.Text = nombreArchivo;
                        lblNombreArchivo.Visible = true;
                        CmdVisualizar.Enabled = true;
                    }

                    updMantenimiento.Update();
                }
            }
        }

        void ActivarControles(bool valor)
        {
            this.txtHoraInicio.Enabled = valor;
            this.txtHoraFin.Enabled = valor;
            this.txtDirURL.Enabled = valor;
            this.txtDescAnot.Enabled = valor;
            this.txtMonto.Enabled = valor;
            this.ctrlOficinaConsular1.Enabled = valor;

            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) != Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ddl_TipAyPAHL.Enabled = valor;
            }

            ddl_Departamento.Enabled = valor;
            ddl_TipAyPAH.Enabled = valor;
            ddl_Provincia.Enabled = valor;
            ddl_Otros.Enabled = valor;
            ddl_Estado.Enabled = valor;
            ddl_Moneda.Enabled = valor;
            ddlFuncionario.Enabled = valor;
            ddl_Distrito.Enabled = valor;
            ddlCircunscripcion.Enabled = valor;
            ctrlOficinaConsular.Enabled = valor;
        }

        void BlanqueControles()
        {
            this.txtFecServcio.Text = "";
            this.txtNroCaso.Text = "";
            this.txtHoraInicio.Text = "";
            this.txtHoraFin.Text = "";
            this.txtDirURL.Text = "";
            this.txtMonto.Text = "";
            this.txtDescAnot.Text = "";
            this.TxtNumBene.Text = "1";
            ddl_TipAyPAHL.SelectedIndex = 0;
            ddl_TipAyPAH.SelectedIndex = 0;
            ddl_Otros.SelectedIndex = 0;
            ddl_Departamento.SelectedIndex = -1;
            ddl_Provincia.SelectedIndex = -1;
            ddl_Distrito.SelectedIndex = -1;
            ddlCircunscripcion.SelectedIndex = -1;
        }

        private void CargarFuncionarios(int sOfConsularId, int IFuncionarioId)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = funcionario.dtFuncionario(sOfConsularId, IFuncionarioId);
                ddlFuncionario.Items.Clear();
                ddlFuncionario.DataTextField = "vFuncionario";
                ddlFuncionario.DataValueField = "IfuncionarioId";
                ddlFuncionario.DataSource = dt;
                ddlFuncionario.DataBind();
                ddlFuncionario.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void CargarCambos()
        {
            DataTable DtServicioPAH = new DataTable();
            DataTable DtServicioPAHL = new DataTable();
            DataTable DtServicioOtros = new DataTable();
            DataTable DtEstado = new DataTable();
            DataTable DtMoneda = new DataTable();
            Util FunUtil = new Util();

            UbigeoConsultasBL Ubigeos = new UbigeoConsultasBL();
            ServicioConsultaBL Servicio = new ServicioConsultaBL();
            EstadoConsultaBL Estado = new EstadoConsultaBL();

            DtServicioPAH = Servicio.Consulta(2);
            DtServicioPAHL = Servicio.Consulta(1);
            DtServicioOtros = Servicio.Consulta(3);
            DtEstado = Estado.ConsultaGrupo("PERSONA-ASISTENCIA");
            DtMoneda = Estado.ConsultaGrupo(Enumerador.enmEstadoGrupo.ASISTENCIA);

            DataTable dtResul = new DataTable();
            dtResul = comun_Part3.ObtenerContinente(Session);

            string strContDep = "";
            string strPaiPro = "";
            string strCiuDis = "";
            string strUbigeoConsulado = "";

            FillWebCombo(comun_Part3.ObtenerContinente(Session), ddl_Departamento, "ubge_vDepartamento", "ubge_cUbi01");

            int intOficinaConsularId = Convert.ToInt32(Session["OficinaDelaAsistencia"]);  // OBTENEMOS EL ID DE LA OFICINA CONSULAR


            if (intOficinaConsularId == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ctrlOficinaConsular1.Cargar(true, true, " - TODOS - ");
                ctrlOficinaConsular.CargarTodoSin(true, "- SELECCIONAR -", "Categoria", "CONSULADO HONORARIO");
            }
            else
            {
                // TRAEMOS LOS DATOS DE LA OFICINA CONSULAR
                DataTable dtResult = new DataTable();
                Controlador.Proceso MiProc = new Proceso();

                Object[] miArrayPersIdentifBus = new Object[1] { intOficinaConsularId };
                dtResult = (DataTable)MiProc.Invocar(ref miArrayPersIdentifBus,
                                                  "SGAC.BE.SI_OFICINACONSULAR",
                                                  "ObtenerPorId",
                                                  Enumerador.enmAplicacion.WEB);
                if (dtResult.Rows.Count != 0)
                {
                    strUbigeoConsulado = dtResult.Rows[0]["ofco_cUbigeoCodigo"].ToString();
                    strContDep = strUbigeoConsulado.Substring(0, 2);
                    strPaiPro = strUbigeoConsulado.Substring(2, 2);
                    strCiuDis = strUbigeoConsulado.Substring(4, 2);
                }

                ctrlOficinaConsular1.Cargar(true, true, " - OTROS - ", strContDep);
                ctrlOficinaConsular.Cargar(true, true, " - SELECCIONE - ", strContDep);

                FillWebCombo(comun_Part3.ObtenerProvincias(Session, strContDep), ddl_Provincia, "ubge_vProvincia", "ubge_cUbi02");
                FillWebCombo(comun_Part3.ObtenerDistritos(Session, strContDep, strPaiPro), ddl_Distrito, "ubge_vDistrito", "ubge_cUbi03");
            }

            ctrlOficinaConsular1.SelectedValue = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();

            FunUtil.DropDownListLLenar(ref ddl_TipAyPAHL, "serv_sServicioId", "serv_vDescripcionCorta", DtServicioPAHL);
            FunUtil.DropDownListLLenar(ref ddl_TipAyPAH, "serv_sServicioId", "serv_vDescripcionCorta", DtServicioPAH);
            FunUtil.DropDownListLLenar(ref ddl_Otros, "serv_sServicioId", "serv_vDescripcionCorta", DtServicioOtros);
            FunUtil.DropDownListLLenar(ref ddl_Estado, "esta_sEstadoId", "esta_vDescripcionCorta", DtEstado);

            ctrlOficinaConsular.SelectedIndex = 0;

            CargarMoneda(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

        }

        private void CargarMoneda(int sIdOficionaConsularOrigen)
        {
            DataTable dtMoneda = new DataTable();
            dtMoneda = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.MONEDA);

            int OficinaDelregistro = 0;
            OficinaDelregistro = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]); //Convert.ToInt32(Session["OficinaDelaAsistencia"]);

            object intMonedaIdSol = 0;
            if (ConfigurationManager.AppSettings["MonedaIdSol"] != null)
                intMonedaIdSol = ConfigurationManager.AppSettings["MonedaIdSol"];

            object intMonedaIdEuro = 0;
            if (ConfigurationManager.AppSettings["MonedaIdEuro"] != null)
                intMonedaIdEuro = ConfigurationManager.AppSettings["MonedaIdEuro"];

            object intMonedaIdDolar = 0;
            if (ConfigurationManager.AppSettings["MonedaIdDolar"] != null)
                intMonedaIdDolar = ConfigurationManager.AppSettings["MonedaIdDolar"];

            if (OficinaDelregistro == Constantes.CONST_OFICINACONSULAR_LIMA)    //OF. LOGUEADA
            {
                // FILTRAMOS LAS MONEDAS DOLARES AMERICANOS Y EUROS SOLO PARA CUANDO EL REGISTRO SEA DE TIPO PAH ESTO SE HACE A REQUERTIMIENTO DEL MINISTERIO
                if (sIdOficionaConsularOrigen == Constantes.CONST_OFICINACONSULAR_LIMA)//OF. ORIGEN
                {
                    DataView dvResult = new DataView(dtMoneda, "((id = " + intMonedaIdDolar + ") or (id = " + intMonedaIdEuro + ") or (id = " + intMonedaIdSol + " ))", "descripcion", DataViewRowState.CurrentRows);
                    dtMoneda = dvResult.ToTable();
                }
                else
                {
                    Proceso MiProc = new Proceso();
                    int intMonedaid = 0;
                    Object[] miArrayPersIdentifBus = new Object[1] { sIdOficionaConsularOrigen };
                    intMonedaid = (Int32)MiProc.Invocar(ref miArrayPersIdentifBus,
                                      "SGAC.BE.MA_MONEDA",
                                      Enumerador.enmAccion.LEER);

                    DataView dvResult = new DataView(dtMoneda, "((id = " + intMonedaIdDolar + ") or (id = " + intMonedaIdEuro + ") or (id = " + intMonedaid + "))", "descripcion", DataViewRowState.CurrentRows);
                    dtMoneda = dvResult.ToTable();
                }
            }
            else
            {
                Proceso MiProc = new Proceso();
                int intMonedaid = 0;
                Object[] miArrayPersIdentifBus = new Object[1] { sIdOficionaConsularOrigen };
                intMonedaid = (Int32)MiProc.Invocar(ref miArrayPersIdentifBus,
                                  "SGAC.BE.MA_MONEDA",
                                  Enumerador.enmAccion.LEER);

                //DataView dvResult = new DataView(dtMoneda, "((id = " + intMonedaIdDolar + ") or (id = " + intMonedaIdEuro + ") or (id = " + intMonedaid + "))", "descripcion", DataViewRowState.CurrentRows);
                DataView dvResult = new DataView(dtMoneda, "((id = " + intMonedaIdDolar + ") or (id = " + intMonedaid + "))", "descripcion", DataViewRowState.CurrentRows);
                dtMoneda = dvResult.ToTable();              
            }

            Util.CargarParametroDropDownList(ddl_Moneda, dtMoneda, true);
        }

        void FillWebCombo(DataTable pDataTable, DropDownList pWebCombo, String str_pDescripcion, String str_pValor)
        {
            pWebCombo.Items.Clear();
            pWebCombo.DataSource = pDataTable;
            pWebCombo.DataTextField = str_pDescripcion;
            pWebCombo.DataValueField = str_pValor;
            pWebCombo.DataBind();
            pWebCombo.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
        }

        void SeteaControles2()
        {
            ctrlToolBarConsulta.VisibleIButtonBuscar = true;
            ctrlToolBarConsulta.VisibleIButtonCancelar = true;
            ctrlToolBarMantenimiento.VisibleIButtonNuevo = true;
            ctrlToolBarMantenimiento.VisibleIButtonEditar = true;
            ctrlToolBarMantenimiento.VisibleIButtonGrabar = true;
            ctrlToolBarMantenimiento.VisibleIButtonCancelar = true;
            ctrlToolBarMantenimiento.VisibleIButtonBuscar = false;
            ctrlToolBarMantenimiento.VisibleIButtonPrint = false;
            ctrlToolBarMantenimiento.VisibleIButtonEliminar = true;
            ctrlToolBarMantenimiento.VisibleIButtonConfiguration = false;
            ctrlToolBarMantenimiento.VisibleIButtonSalir = false;
        }

        void ActivarTool(bool BtnNuevo, bool BtnEditar, bool BtnGrabar, bool BtnCancelar, bool BtnELiminar)
        {
            this.ctrlToolBarMantenimiento.btnNuevo.Enabled = BtnNuevo;
            this.ctrlToolBarMantenimiento.btnEditar.Enabled = BtnEditar;
            this.ctrlToolBarMantenimiento.btnGrabar.Enabled = BtnGrabar;
            this.ctrlToolBarMantenimiento.btnCancelar.Enabled = BtnCancelar;
            this.ctrlToolBarMantenimiento.btnEliminar.Enabled = BtnELiminar;
        }

        bool Grabar()
        {

            try
            {
                bool bolOk = false;
                string strNombreArchivo = string.Empty;

                Label lblNombreArchivo = (Label)ctrlUploader1.FindControl("lblNombreArchivo");
                lblNombreArchivo.Visible = true;

                strNombreArchivo = lblNombreArchivo.Text;

                PersonaAsistenciaMantenimientoBL Asistencia = new PersonaAsistenciaMantenimientoBL();
                BE.MRE.RE_ASISTENCIA BE_Asistencia = new BE.MRE.RE_ASISTENCIA();
                int intQueHace = Convert.ToInt32(Session["IQueHace"]);
                Int64 intPersonaId = Convert.ToInt64(ViewState["IPersonaAsisId"]);
                int intAsistenciaId = Convert.ToInt32(Session["IAsistenciaId"]);

                Int16 intMonedaId = 0;
                Int16 intTipoAsistencia = 0;
                Int16 intServicioPAHL = 0;
                Int16 intServicioPAH = 0;
                Int16? intServicioOtros = 0;
                string strUbigeoId = "";
                int OficinaDelregistro = 0;

                OficinaDelregistro = Convert.ToInt32(Session["OficinaDelaAsistencia"]);

                intServicioPAHL = 0;
                intServicioPAH = 0;
                intServicioOtros = null;

                if (OficinaDelregistro == Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    intTipoAsistencia = (int)Enumerador.enmServicio.PAH;

                    if (ddl_TipAyPAHL.SelectedValue != "")
                    {
                        intServicioPAHL = Convert.ToInt16(ddl_TipAyPAHL.SelectedValue);
                    }

                    if (ddl_TipAyPAH.SelectedValue != "")
                    {
                        intServicioPAH = Convert.ToInt16(ddl_TipAyPAH.SelectedValue);
                    }


                    ddl_Otros.SelectedIndex = 0;
                    intServicioOtros = Convert.ToInt16(ddl_Otros.SelectedIndex);
                }
                else
                {
                    if (ddl_TipAyPAHL.SelectedIndex > 0)
                    {
                        intTipoAsistencia = (int)Enumerador.enmServicio.PAHL;
                        intServicioPAHL = Convert.ToInt16(ddl_TipAyPAHL.SelectedValue);
                    }

                    if (ddl_TipAyPAH.SelectedIndex > 0)
                    {
                        intTipoAsistencia = (int)Enumerador.enmServicio.PAH;
                        intServicioPAH = Convert.ToInt16(ddl_TipAyPAH.SelectedValue);
                    }
                    intServicioOtros = Convert.ToInt16(ddl_Otros.SelectedValue);
                }

                strUbigeoId = ddl_Departamento.SelectedValue + ddl_Provincia.SelectedValue + ddl_Distrito.SelectedValue;
                intMonedaId = Convert.ToInt16(ddl_Moneda.SelectedValue);

                BE_Asistencia.asis_iAsistenciaId = intAsistenciaId;
                BE_Asistencia.asis_iPersonaId = intPersonaId;
                BE_Asistencia.asis_sTipAsistencia = intTipoAsistencia;
                BE_Asistencia.asis_dFecServicio = txtFecServcio.Value();
                BE_Asistencia.asis_vNroCaso = txtNroCaso.Text.ToUpper().Trim();
                BE_Asistencia.asis_vHoraInicio = txtHoraInicio.Text;
                BE_Asistencia.asis_vHoraFin = txtHoraFin.Text;
                BE_Asistencia.asis_sTipoServId = intServicioPAH > 0 ? intServicioPAH : intServicioPAHL;
                BE_Asistencia.asis_sOtrosServiciosId = Convert.ToInt16(intServicioOtros);
                BE_Asistencia.asis_cUbigeo = strUbigeoId;
                BE_Asistencia.asis_sOficinaConsularOrigenId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                BE_Asistencia.asis_sOficinaConsularId = (ctrlOficinaConsular1.Visible) ? Convert.ToInt16(ctrlOficinaConsular1.SelectedValue) : Comun.ToNullInt16(ctrlOficinaConsular.SelectedValue);
                BE_Asistencia.asis_IFuncionarioId = Convert.ToInt32(ddlFuncionario.SelectedValue);
                BE_Asistencia.asis_vDirURL = txtDirURL.Text;
                BE_Asistencia.asis_sMonedaId = intMonedaId;
                BE_Asistencia.asis_FMontoServ = Convert.ToDouble(txtMonto.Text);
                BE_Asistencia.asis_vObservaciones = txtDescAnot.Text;
                BE_Asistencia.asis_vNombreArchivo = strNombreArchivo;
                BE_Asistencia.asis_sEstado = Convert.ToInt16(ddl_Estado.SelectedValue);
                BE_Asistencia.asis_sNumeroBeneficiario = Convert.ToInt16(TxtNumBene.Text);
                BE_Asistencia.asis_sCircunscripcionId = Comun.ToNullInt16(ddlCircunscripcion.SelectedValue);


                if (intQueHace == 1)//Para registro nuevos
                {
                    BE_Asistencia.asis_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    BE_Asistencia.asis_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    BE_Asistencia.asis_sUsuarioModificacion = null;
                    BE_Asistencia.asis_vIPModificacion = null;
                    BE_Asistencia.asis_dFechaModificacion = null;
                }
                if (intQueHace == 2)//Para edición
                {
                    BE_Asistencia.asis_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    BE_Asistencia.asis_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                }

                //Controlador.Proceso MiProc = new Proceso();

                DataTable dt = new DataTable();
                dt = (DataTable)Session[srtBeneficiario];

                dt.Rows[0].Delete();
                dt.AcceptChanges();

                Object[] miArrayPersIdentifBus = new Object[2] { BE_Asistencia, dt };

                if (intQueHace == 1)
                {
                    PersonaConsultaBL objPersonaBL = new PersonaConsultaBL();

                    List<BE.MRE.RE_ASISTENCIABENEFICIARIO> lstBeneficiarios = new List<BE.MRE.RE_ASISTENCIABENEFICIARIO>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        BE.MRE.RE_ASISTENCIABENEFICIARIO objBeneficiario = new BE.MRE.RE_ASISTENCIABENEFICIARIO();
                        objBeneficiario.asbe_iAsistenciaBeneficiarioId = Convert.ToInt64(dr["asbe_iAsistenciaBeneficiarioId"]);
                        objBeneficiario.asbe_iAsistenciaId = Convert.ToInt64(dr["asbe_iAsistenciaId"]);
                        objBeneficiario.asbe_iPersonaId = Convert.ToInt64(dr["asbe_iPersonaId"]);
                        objBeneficiario.asbe_FMonto = Convert.ToDouble(dr["asbe_fMonto"]);
                        objBeneficiario.asbe_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        objBeneficiario.asbe_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        objBeneficiario.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                        objBeneficiario.PERSONA.Identificacion.peid_sDocumentoTipoId = Convert.ToInt16(dr["asbe_sDocumentoTipoId"]);
                        objBeneficiario.PERSONA.Identificacion.peid_vDocumentoNumero = Convert.ToString(dr["asbe_vDocumentoNumero"]);
                        
                        objBeneficiario.PERSONA.pers_iPersonaId = objBeneficiario.asbe_iPersonaId;
                        if (objBeneficiario.PERSONA.pers_iPersonaId != 0)
                            objBeneficiario.PERSONA = objPersonaBL.ObtenerRuneRapido(objBeneficiario.PERSONA);
                        else
                            objBeneficiario.PERSONA.Identificacion.peid_bActivoEnRune = true;

                        objBeneficiario.PERSONA.pers_vApellidoPaterno = Convert.ToString(dr["asbe_vApellidoPaterno"]);
                        objBeneficiario.PERSONA.pers_vApellidoMaterno = Convert.ToString(dr["asbe_vApellidoMaterno"]);
                        objBeneficiario.PERSONA.pers_vNombres = Convert.ToString(dr["asbe_vNombres"]);
                        objBeneficiario.PERSONA.pers_sGeneroId = Convert.ToInt16(dr["asbe_sGeneroId"]);
                        objBeneficiario.PERSONA.pers_sUsuarioCreacion = objBeneficiario.asbe_sUsuarioCreacion;
                        objBeneficiario.PERSONA.pers_sUsuarioModificacion = objBeneficiario.asbe_sUsuarioCreacion;
                        objBeneficiario.PERSONA.OficinaConsultar = objBeneficiario.OficinaConsultar;

                        lstBeneficiarios.Add(objBeneficiario);
                    }

                    PersonaAsistenciaMantenimientoBL objBL = new PersonaAsistenciaMantenimientoBL();
                    long intPrueba = objBL.Insertar(BE_Asistencia, lstBeneficiarios);

                    if (intPrueba > 0)
                    {
                        Session["StrArchivoSubido"] = "";
                        bolOk = true;
                    }
                }
                if (intQueHace == 2)
                {
                    PersonaConsultaBL objPersonaBL = new PersonaConsultaBL();

                    List<BE.MRE.RE_ASISTENCIABENEFICIARIO> lstBeneficiarios = new List<BE.MRE.RE_ASISTENCIABENEFICIARIO>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        BE.MRE.RE_ASISTENCIABENEFICIARIO objBeneficiario = new BE.MRE.RE_ASISTENCIABENEFICIARIO();
                        objBeneficiario.asbe_iAsistenciaBeneficiarioId = Convert.ToInt64(dr["asbe_iAsistenciaBeneficiarioId"]);
                        objBeneficiario.asbe_iAsistenciaId = Convert.ToInt64(dr["asbe_iAsistenciaId"]);
                        objBeneficiario.asbe_iPersonaId = Convert.ToInt64(dr["asbe_iPersonaId"]);
                        objBeneficiario.asbe_FMonto = Convert.ToDouble(dr["asbe_fMonto"]);
                        objBeneficiario.asbe_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        objBeneficiario.asbe_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        objBeneficiario.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                        objBeneficiario.PERSONA.Identificacion.peid_sDocumentoTipoId = Convert.ToInt16(dr["asbe_sDocumentoTipoId"]);
                        objBeneficiario.PERSONA.Identificacion.peid_vDocumentoNumero = Convert.ToString(dr["asbe_vDocumentoNumero"]);

                        objBeneficiario.PERSONA.pers_iPersonaId = objBeneficiario.asbe_iPersonaId;
                        if (objBeneficiario.PERSONA.pers_iPersonaId != 0)
                            objBeneficiario.PERSONA = objPersonaBL.ObtenerRuneRapido(objBeneficiario.PERSONA);
                        else
                            objBeneficiario.PERSONA.Identificacion.peid_bActivoEnRune = true;
                        objBeneficiario.PERSONA.pers_vApellidoPaterno = Convert.ToString(dr["asbe_vApellidoPaterno"]);
                        objBeneficiario.PERSONA.pers_vApellidoMaterno = Convert.ToString(dr["asbe_vApellidoMaterno"]);
                        objBeneficiario.PERSONA.pers_vNombres = Convert.ToString(dr["asbe_vNombres"]);
                        if (dr["asbe_sGeneroId"].ToString() != string.Empty)
                            objBeneficiario.PERSONA.pers_sGeneroId = Convert.ToInt16(dr["asbe_sGeneroId"]);
                        objBeneficiario.PERSONA.pers_sUsuarioCreacion = objBeneficiario.asbe_sUsuarioCreacion;
                        objBeneficiario.PERSONA.pers_sUsuarioModificacion = objBeneficiario.asbe_sUsuarioCreacion;
                        objBeneficiario.PERSONA.OficinaConsultar = objBeneficiario.OficinaConsultar;

                        lstBeneficiarios.Add(objBeneficiario);
                    }
                    
                    PersonaAsistenciaMantenimientoBL objBL = new PersonaAsistenciaMantenimientoBL();
                    long intPrueba = objBL.Actualizar(BE_Asistencia, lstBeneficiarios);

                    if (intPrueba > 0)
                    {
                        Session["StrArchivoSubido"] = "";
                        bolOk = true;
                    }
                }

                return bolOk;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void MostrarListaAsistencia(long LonPersonaId, string strPersonaNombre)
        {
            DataTable dtAsistencia = new DataTable();

            int intNumTotalRegistros = 0;
            int intNumTotalPaginas = 0;

            int intOficinaId = Convert.ToInt32(Session["OficinaDelaAsistencia"]);
            string strHostName = Convert.ToString(Session[Constantes.CONST_SESION_HOSTNAME]);
            int intUsuarioId = Convert.ToInt32(Session[Constantes.CONST_SESION_USUARIO_ID]);
            string strDireccionIP = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

            PersonaAsistenciaConsultaBL xFun = new PersonaAsistenciaConsultaBL();
            dtAsistencia = xFun.Obtener(LonPersonaId,
                                        ctrlPageBar2.PaginaActual.ToString(),
                                        ctrlPageBar2.PageSize,
                                        intOficinaId,
                                        strHostName,
                                        intUsuarioId,
                                        strDireccionIP,
                                        ref intNumTotalRegistros,
                                        ref intNumTotalPaginas);

            Session["dtAsistencia"] = dtAsistencia;

            if (dtAsistencia.Rows.Count != 0)
            {
                GrdAnatacion2.DataSource = dtAsistencia;
                GrdAnatacion2.DataBind();
                GrdAnatacion2.Visible = true;
                btnNuevaAsistencia.Visible = true;
                lblDetalleAsistencia.Visible = true;

                if (dtAsistencia.Rows.Count == 0)
                {
                    ctrlValidacionVentanilla.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                }
                else
                {
                    ctrlPageBar2.TotalResgistros = intNumTotalRegistros;
                    ctrlPageBar2.TotalPaginas = intNumTotalPaginas;

                    ctrlPageBar2.Visible = false;
                    if (ctrlPageBar2.TotalPaginas > 1)
                    {
                        ctrlPageBar2.Visible = true;
                    }
                    else
                    {
                        ctrlValidacionVentanilla.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + dtAsistencia.Rows.Count, true, Enumerador.enmTipoMensaje.INFORMATION);
                    }
                }
            }
            else
            {
                if (ctrlToolBarMantenimiento.btnNuevo.Enabled == true)
                {
                    MyControl_btnNuevo();
                }

                GrdAnatacion2.DataSource = dtAsistencia;
                GrdAnatacion2.DataBind();
                GrdAnatacion2.Visible = false;
                btnNuevaAsistencia.Visible = false;
                lblDetalleAsistencia.Visible = false;

                string StrScript = string.Empty;


                DataTable dt = (DataTable)Session[srtBeneficiario];
                gvBeneficiario.DataSource = dt;
                gvBeneficiario.DataBind();
                Session[srtBeneficiario] = dt;
                TxtNumBene.Text = dt.Rows.Count.ToString();	

                ctrlValidacionVentanilla.MostrarValidacion("No se ha encontrado asistencias para este Connacional");

                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "ASISTENCIA", "No se ha encontrado asistencias para: " + strPersonaNombre.ToUpper(), false, 190, 250);
                Comun.EjecutarScript(Page, StrScript);

                StrScript = @"$(function(){{
                                MoveTabIndex(1);
                            }});";
                StrScript = string.Format(StrScript);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex2", StrScript, true);

            }
        }

        private void CargarUbicacionGeografica(int intOficinaConsularId)
        {
            DataTable dtResult = new DataTable();
            Proceso MiProc = new Proceso();

            Object[] miArrayPersIdentifBus = { intOficinaConsularId };
            dtResult = (DataTable)MiProc.Invocar(ref miArrayPersIdentifBus,
                                              "SGAC.BE.SI_OFICINACONSULAR",
                                              "ObtenerPorId");
            if (dtResult.Rows.Count > 0)
            {
                string strUbigeoConsulado = dtResult.Rows[0]["ofco_cUbigeoCodigo"].ToString();
                string strContDep = strUbigeoConsulado.Substring(0, 2);
                string strPaiPro = strUbigeoConsulado.Substring(2, 2);
                string strCiuDis = strUbigeoConsulado.Substring(4, 2);

                ViewState.Add("Continente", strContDep);

                if (intOficinaConsularId == Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    string strDepaLima = Constantes.CONST_OFICINACONSULAR_LIMA_UBIGEO.Substring(0, 2); ;//"14";
                    string strProvLima = Constantes.CONST_OFICINACONSULAR_LIMA_UBIGEO.Substring(2, 2); ;//"01";
                    string strDistLima = Constantes.CONST_OFICINACONSULAR_LIMA_UBIGEO.Substring(4, 2); ;//"01";

                    DataTable dtDepartamentos = comun_Part3.ObtenerDepartamentos(Session);
                    ddl_Departamento.Items.Clear();
                    ddl_Departamento.SelectedIndex = -1;
                    ddl_Departamento.SelectedValue = null;
                    ddl_Departamento.ClearSelection();
                    ddl_Departamento.DataSource = dtDepartamentos;
                    ddl_Departamento.DataTextField = "ubge_vDepartamento";
                    ddl_Departamento.DataValueField = "ubge_cUbi01";
                    ddl_Departamento.DataBind();
                    ddl_Departamento.SelectedValue = strDepaLima;

                    ddl_Provincia.Items.Clear();
                    ddl_Provincia.SelectedIndex = -1;
                    ddl_Provincia.SelectedValue = null;
                    ddl_Provincia.ClearSelection();
                    ddl_Provincia.DataSource = comun_Part3.ObtenerProvincias(Session, strDepaLima);
                    ddl_Provincia.DataTextField = "ubge_vProvincia";
                    ddl_Provincia.DataValueField = "ubge_cUbi02";
                    ddl_Provincia.DataBind();
                    ddl_Provincia.SelectedValue = strProvLima;

                    ddl_Distrito.Items.Clear();
                    ddl_Distrito.SelectedIndex = -1;
                    ddl_Distrito.SelectedValue = null;
                    ddl_Distrito.ClearSelection();
                    ddl_Distrito.DataSource = comun_Part3.ObtenerDistritos(Session, strDepaLima, strProvLima);
                    ddl_Distrito.DataTextField = "ubge_vDistrito";
                    ddl_Distrito.DataValueField = "ubge_cUbi03";
                    ddl_Distrito.DataBind();
                    ddl_Distrito.Items.Insert(0, new ListItem(" - SELECCIONAR - ", "0"));
                    ddl_Distrito.SelectedValue = strDistLima;
                }
                else
                {

                    Util.CargarDropDownList(ddl_Departamento, comun_Part3.ObtenerContinente(Session), "ubge_vDepartamento", "ubge_cUbi01");
                    ddl_Departamento.SelectedValue = strContDep;

                    Util.CargarDropDownList(ddl_Provincia, comun_Part3.ObtenerProvincias(Session, strContDep), "ubge_vProvincia", "ubge_cUbi02");
                    ddl_Provincia.SelectedValue = strPaiPro;

                    Util.CargarDropDownList(ddl_Distrito, comun_Part3.ObtenerDistritos(Session, strContDep, strPaiPro), "ubge_vDistrito", "ubge_cUbi03", true);
                    ddl_Distrito.SelectedValue = strCiuDis;
                }
            }
            else
            {
                Util.CargarDropDownList(ddl_Departamento, comun_Part3.ObtenerContinente(Session), "ubge_vDepartamento", "ubge_cUbi01");
                ddl_Departamento.SelectedValue = ViewState["Continente"].ToString();

                Util.CargarDropDownList(ddl_Provincia, comun_Part3.ObtenerProvincias(Session, ddl_Departamento.SelectedValue), "ubge_vProvincia", "ubge_cUbi02");

                Util.CargarDropDownList(ddl_Distrito, comun_Part3.ObtenerDistritos(Session, ddl_Departamento.SelectedValue, ddl_Provincia.SelectedValue), "ubge_vDistrito", "ubge_cUbi03", true);
            }
        }

        public static void EjecutarScript(Page Page, string strScript)
        {
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), "OpenPopup", strScript, true);
        }

        #endregion

        protected void btnAgregarBene_Click(object sender, EventArgs e)
        {
            Session.Add("BeneficiarioAccion", Enumerador.enmAccion.INSERTAR);
            try
            {
                Comun.EjecutarScript(this, "showModalPopup('../Registro/FrmRegistroBeneficiario.aspx','ASISTENCIA BENEFICIARIO',200, 800, '" + btnRealizarBusqueda.ClientID + "');");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable Beneficiario(Int64 IdAsistencia, Int64 personaId)
        {
            try
            {
                DataTable dt = new DataTable();
                PersonaAsistenciaConsultaBL objBL = new PersonaAsistenciaConsultaBL();

                dt = objBL.ListarAsistenciaBeneficiario(IdAsistencia, personaId);
                Session[srtBeneficiario] = dt;

                TxtNumBene.Text = dt.Rows.Count.ToString();

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable Beneficiario_limpiar()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("asbe_iAsistenciaBeneficiarioId", typeof(string));
            dt.Columns.Add("asbe_iAsistenciaId", typeof(string));
            dt.Columns.Add("asbe_iPersonaId", typeof(string));
            dt.Columns.Add("asbe_sDocumentoTipoId", typeof(string));
            dt.Columns.Add("asbe_vNombreDocumento", typeof(string));
            dt.Columns.Add("asbe_vDocumentoNumero", typeof(string));
            dt.Columns.Add("asbe_vApellidoPaterno", typeof(string));
            dt.Columns.Add("asbe_vApellidoMaterno", typeof(string));
            dt.Columns.Add("asbe_vNombres", typeof(string));
            dt.Columns.Add("asbe_fMonto", typeof(string));
            dt.Columns.Add("asbe_iSolicitante", typeof(string));
            dt.Columns.Add("asbe_sGeneroId", typeof(string));
            dt.Columns.Add("Genero", typeof(string));
            return dt;
        }

        protected void imgCerrar_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void gvBeneficiario_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (!btnAgregarBene.Enabled) return;

                string StrScript = string.Empty;

                int index = Convert.ToInt32(e.CommandArgument);
                ViewState.Add("IndexBeneficiario", index);

                int ISolicitante = Convert.ToInt32(gvBeneficiario.DataKeys[index].Values["asbe_ISolicitante"].ToString());
                
                if (ISolicitante == 1 )
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "ASISTENCIA", "No se puede elimiar o editar Titular", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }

                if (btnAgregarBene.Enabled == false)
                {
                    return;
                }

                if (e.CommandName == "Editar")
                {
                    int IPersona = Convert.ToInt32(gvBeneficiario.DataKeys[index].Values["asbe_iPersonaId"].ToString());
                    if (IPersona != 0)
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR , "ASISTENCIA", "No se puede modificar datos de beneficiario. Por favor editar desde RUNE", false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);
                        return;
                    }

                    ViewState.Add("BeneficiarioAccion", Enumerador.enmAccion.MODIFICAR);

                    Comun.EjecutarScript(this, "showModalPopup('../Registro/FrmRegistroBeneficiario.aspx?vClass=" + index + "','ASISTENCIA BENEFICIARIO',400, 600, '" + btnRealizarBusqueda.ClientID + "');");
                }

                if (e.CommandName == "Eliminar")
                {

                    DataTable dt = ((DataTable)Session[srtBeneficiario]).Copy();

                    dt.Rows[index].Delete();
                    dt.AcceptChanges();

                    if (gvBeneficiario.Rows.Count == 1)
                    {
                        dt = Beneficiario_limpiar();
                    }

                    Session[srtBeneficiario] = dt;
                    gvBeneficiario.DataSource = dt;
                    gvBeneficiario.DataBind();

                    TxtNumBene.Text = dt.Rows.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }
        }

        protected void btnNuevaAsistencia_Click(object sender, EventArgs e)
        {
            Comun.EjecutarScript(Page, Util.ActivarTab(1, "Registro"));

            ActivarControles(true);
            MyControl_btnNuevo();            
        }

        protected void txtDocumento_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Comun.EjecutarScript(this, "showModalPopup('../Registro/FrmBusqueda.aspx','BÚSQUEDA',500, 1000, '" + btnRealizarBusqueda.ClientID + "');");
        }

        protected void btnRealizarBusqueda_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session[srtBeneficiario];

            gvBeneficiario.DataSource = dt;
            gvBeneficiario.DataBind();
            Session.Remove("iPersonaId");

            TxtNumBene.Text = dt.Rows.Count.ToString();

            GrdAnatacion2.Enabled = true;
        }

        protected void GrdAnotaciones_RowCreated(object sender, GridViewRowEventArgs e)
        {           
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                e.Row.ToolTip = "Haga Click para seleccionar la fila.";
                e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.GrdAnotaciones, "Select$" + e.Row.RowIndex);
            }

        }

        protected void ddl_Distrito_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_Distrito.SelectedValue.ToString() == "")
            {
                return;
            }

            string strCodContinente = string.Empty;
            string strCodPais = string.Empty;
            string strCodCiudad = string.Empty;

            strCodContinente = ddl_Departamento.SelectedValue.ToString();

            if (ddl_Distrito.SelectedValue == "00")
            {
                //ctrlOficinaConsular.Cargar(true, true, " - TODOS - ", strCodContinente);
                DataTable _dt = new DataTable();

                _dt = Comun.ObtenerOficinasActivas(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString()));
                //DataTable _dt = (DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULARACTIVAS];
                Util.CargarDropDownList(ctrlOficinaConsular.ddlOficinaConsular, _dt, "ofco_vNombre", "ofco_sOficinaConsularId", true, "- TODOS -");
            }
            else
            {
                strCodPais = ddl_Provincia.SelectedValue.ToString();
                strCodCiudad = ddl_Distrito.SelectedValue.ToString();
                ctrlOficinaConsular.CargarContinentePaisCuidad(true, true, " - SELECCIONAR - ", strCodContinente, strCodPais, strCodCiudad);
                if (ctrlOficinaConsular.ddlOficinaConsular.Items.Count > 1)
                { ctrlOficinaConsular.ddlOficinaConsular.SelectedIndex = 1; }
                
            }
        }
     }
}
