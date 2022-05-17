using System;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.BE.MRE;
using System.Data;
using SGAC.Cliente.Colas.BL;
using SGAC.Accesorios;
using SGAC.Controlador;

namespace SGAC.WebApp.Colas
{
    public partial class FrmTicketeraRegister : MyBasePage
    {
        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            CargarGrilla();
            updGrillaConsulta.Update();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SeteraControles2();

                ctrlToolBarMantenimiento.btnGrabar.OnClientClick = "return Validar();";
                ddlOficinaConsularConsulta.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsular_SelectedIndexChanged);

                if (!Page.IsPostBack)
                {
                    ddlOficinaConsularConsulta.Cargar(false);
                    ddlOficinaConsularConsulta2.Cargar(false);
                    ctrlToolBarMantenimiento_btnNuevoHandler();
                }
                if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
                {
                    Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo, ctrlToolBarMantenimiento.btnEditar, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar };
                    GridView[] arrGridView = { grdTicketera };
                    Comun.ModoLectura(ref arrButtons);
                    Comun.ModoLectura(ref arrGridView);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ddlOficinaConsular_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlOficinaConsularConsulta2.SelectedValue = ddlOficinaConsularConsulta.SelectedValue;
            updMantenimiento.Update();
        }

        void SeteraControles2()
        {
            ctrlToolBar1.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBar1_btnBuscarHandler);
            ctrlToolBar1.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBar1_btnCancelarHandler);
            ctrlToolBarMantenimiento.btnNuevoHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonNuevoClick(ctrlToolBarMantenimiento_btnNuevoHandler);
            ctrlToolBarMantenimiento.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarMantenimiento_btnCancelarHandler);
            ctrlToolBarMantenimiento.btnEditarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEditarClick(ctrlToolBarMantenimiento_btnEditarHandler);
            ctrlToolBarMantenimiento.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBarMantenimiento_btnGrabarHandler);
            ctrlToolBarMantenimiento.btnEliminarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEliminarClick(ctrlToolBarMantenimiento_btnEliminarHandler);

            ctrlToolBar1.VisibleIButtonNuevo = false;
            ctrlToolBar1.VisibleIButtonEditar = false;
            ctrlToolBar1.VisibleIButtonGrabar = false;
            ctrlToolBar1.VisibleIButtonCancelar = true;
            ctrlToolBar1.VisibleIButtonBuscar = true;
            ctrlToolBar1.VisibleIButtonPrint = false;
            ctrlToolBar1.VisibleIButtonEliminar = false;
            ctrlToolBar1.VisibleIButtonConfiguration = false;
            ctrlToolBar1.VisibleIButtonSalir = false;

            ctrlToolBarMantenimiento.VisibleIButtonNuevo = true;
            ctrlToolBarMantenimiento.VisibleIButtonEditar = true;
            ctrlToolBarMantenimiento.VisibleIButtonGrabar = true;
            ctrlToolBarMantenimiento.VisibleIButtonCancelar = true;
            ctrlToolBarMantenimiento.VisibleIButtonBuscar = false;
            ctrlToolBarMantenimiento.VisibleIButtonPrint = false;
            ctrlToolBarMantenimiento.VisibleIButtonEliminar = true;
            ctrlToolBarMantenimiento.VisibleIButtonConfiguration = false;
            ctrlToolBarMantenimiento.VisibleIButtonSalir = false;

            ctrlToolBar1.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBar1.btnCancelar.Text = "    Limpiar";
        }

        void ctrlToolBar1_btnCancelarHandler()
        {
            ddlOficinaConsularConsulta.SelectedIndex = 0;
            if (Session["dtTicketera"] != null)
            {
                DataTable dt = ((DataTable)Session["dtTicketera"]).Clone();
                grdTicketera.DataSource = dt;
                grdTicketera.DataBind();
            }
            else
            {
                grdTicketera.DataSource = null;
                grdTicketera.DataBind();
            }
        }

        void ctrlToolBar1_btnBuscarHandler()
        {
            if (ddlOficinaConsularConsulta.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "Ingrese una oficina consular" + "');", true);
                return;
            }

            CargarGrilla();
        }

        void ctrlToolBarMantenimiento_btnEditarHandler()
        {
            EstadoTool(true, true, false, true, true, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_EDITAR) + Util.DeshabilitarTab(0));

            Session["IQueHace"] = 2;

            ActivarControles(false);
            txtDescripcion.Focus();
        }

        void ctrlToolBarMantenimiento_btnNuevoHandler()
        {
            EstadoTool(true, true, false, true, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR) );

            Session["IQueHace"] = 1;
            limpiar();

            ActivarControles(false);
            limpiar();
            ViewState.Add("IdTicketera", 0);
            txtDescripcion.Focus();
        }

        void ctrlToolBarMantenimiento_btnGrabarHandler()
        {
            if (ddlOficinaConsularConsulta2.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "Ingrese una oficina consular" + "');", true);
                return;
            }
            
            TicketeraMantenimientoBL BL = new TicketeraMantenimientoBL();
            SGAC.BE.MRE.CL_TICKETERA ObjTicteraBE = new BE.MRE.CL_TICKETERA();
            int intRpta = 0;

            updMantenimiento.Update();

            ObjTicteraBE.tira_sOficinaConsularId = Convert.ToInt16(this.ddlOficinaConsularConsulta2.SelectedValue);
            ObjTicteraBE.tira_vNombre = txtDescripcion.Text;
            ObjTicteraBE.tira_vMarca = TXTMARCA.Text;
            ObjTicteraBE.tira_vModelo = txtModelo.Text;
            ObjTicteraBE.tira_vSerie = TXTSERIE.Text;
            ObjTicteraBE.tira_vCaracteristicas = TXTOBSERVACION.Text;
            ObjTicteraBE.tira_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();

            if (Convert.ToInt32(Session["IQueHace"]) == 1)
            {
                ObjTicteraBE.tira_dFechaCreacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                ObjTicteraBE.tira_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString()); //   Session["VUsuarioId"]
                ObjTicteraBE.tira_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                intRpta = BL.Insertar(ref ObjTicteraBE);
            }
            else
            {
                ObjTicteraBE.tira_sTicketeraId = Convert.ToInt16(ViewState["IdTicketera"]);
                ObjTicteraBE.tira_dFechaModificacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                ObjTicteraBE.tira_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                ObjTicteraBE.tira_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                intRpta = BL.Actualizar(ObjTicteraBE);
            }

            string strScript = string.Empty;
           
            if (intRpta == (int)Enumerador.enmResultadoQuery.OK)
            {
                EstadoTool(true, true, false, false, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
                strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                strScript += Util.NombrarTab(0, Constantes.CONST_TAB_CONSULTAR);
                strScript += Util.HabilitarTab(0);

                ctrlToolBarMantenimiento_btnCancelarHandler();
                CargarGrilla();

            }
            else
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Ticketera", Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
            }

            Comun.EjecutarScript(Page, strScript);
        }

        void ctrlToolBarMantenimiento_btnCancelarHandler()
        {
            EstadoTool(true, true, false, false, false, false);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
            Comun.EjecutarScript(Page, Util.NombrarTab(0, Constantes.CONST_TAB_INICIAL) + Util.HabilitarTab(0));

            limpiar();
            ActivarControles(true);
            CargarGrilla();
        }

        void ctrlToolBarMantenimiento_btnEliminarHandler()
        {
            TicketeraMantenimientoBL BL = new TicketeraMantenimientoBL();
            try
            {
                CL_TICKETERA srvServicio = new CL_TICKETERA();

                srvServicio.tira_dFechaModificacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                srvServicio.tira_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                srvServicio.tira_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                srvServicio.tira_sTicketeraId = Convert.ToInt16(Session["IdTicketera"]);
                srvServicio.tira_cEstado = Enumerador.enmEstado.DESACTIVO.ToString();
                
                int intResultado = BL.Eliminar(srvServicio);

                string strScript = string.Empty;
               
                if (intResultado == (int)Enumerador.enmResultadoQuery.OK)
                {
                    EstadoTool(true, true, false, false, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
                    strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                    strScript += Util.NombrarTab(0, Constantes.CONST_TAB_CONSULTAR);
                    strScript += Util.HabilitarTab(0);

                    ctrlToolBarMantenimiento_btnCancelarHandler();
                    updGrillaConsulta.Update();
                }
                else
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Ticketera", Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
                }

                Comun.EjecutarScript(Page, strScript);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdTicketera_SelectedIndexChanged(object sender, EventArgs e)
        {
            Comun.EjecutarScript(Page, Util.HabilitarTab(1));
        }

        protected void grdTicketera_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            string strScript = string.Empty;
            Session["IdTicketera"] = grdTicketera.Rows[index].Cells[0].Text;
            try
            {
                ddlOficinaConsularConsulta2.SelectedValue = grdTicketera.DataKeys[index].Values["tira_sOficinaConsularId"].ToString();
                ViewState.Add("IdTicketera", HttpUtility.HtmlDecode(grdTicketera.Rows[index].Cells[0].Text));
                this.txtDescripcion.Text = HttpUtility.HtmlDecode(grdTicketera.Rows[index].Cells[1].Text);
                this.TXTMARCA.Text = HttpUtility.HtmlDecode(grdTicketera.Rows[index].Cells[2].Text);
                this.txtModelo.Text = HttpUtility.HtmlDecode(grdTicketera.Rows[index].Cells[3].Text);
                this.TXTSERIE.Text = HttpUtility.HtmlDecode(grdTicketera.Rows[index].Cells[4].Text);
                this.TXTOBSERVACION.Text = HttpUtility.HtmlDecode(grdTicketera.Rows[index].Cells[5].Text);
                updMantenimiento.Update();

                if (e.CommandName == "Consultar")
                {
                    ActivarControles(true);
                    EstadoTool(true, true, true, false, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
                }

                if (e.CommandName == "Editar")
                {
                    EstadoTool(true, true, false, true, true, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
                    Session["IQueHace"] = 2;
                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);

                    ActivarControles(false);
                    txtDescripcion.Focus();

                }

                Comun.EjecutarScript(Page, strScript);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CargarGrilla()
        {
            TicketeraConsultaBL BL = new TicketeraConsultaBL();

            try
            {
                int intTotalRegistros = 0;
                int intTotalPaginas = 0;

                DataTable dt = BL.Consultar(Convert.ToInt32(this.ddlOficinaConsularConsulta.SelectedValue),
                                          ctrlPaginador.PaginaActual,
                                          Constantes.CONST_CANT_REGISTRO,
                                          ref intTotalRegistros,
                                          ref intTotalPaginas);
                
                Session.Add("dt", dt);
                grdTicketera.SelectedIndex = -1;
                grdTicketera.DataSource = dt;
                grdTicketera.DataBind();

                // Paginador
                ctrlPaginador.TotalResgistros = intTotalRegistros;
                ctrlPaginador.TotalPaginas = intTotalPaginas;

                ctrlPaginador.Visible = false;
                if (ctrlPaginador.TotalPaginas > 1)
                    ctrlPaginador.Visible = true;

                ctrlValidacionTicketera.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                if (grdTicketera.Rows.Count > 0)
                {
                    ctrlValidacionTicketera.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + ctrlPaginador.TotalResgistros, 
                                                              true, 
                                                              Enumerador.enmTipoMensaje.INFORMATION);
                }

                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ActivarControles(bool valor)
        {
            this.TXTMARCA.Enabled = !valor;
            this.txtDescripcion.Enabled = !valor;
            this.txtModelo.Enabled = !valor;
            this.TXTOBSERVACION.Enabled = !valor;
            this.TXTSERIE.Enabled = !valor;
            ddlOficinaConsularConsulta2.Enabled = !valor;
        }

        void limpiar()
        {
            ViewState.Add("IdTicketera", 0);
            this.txtDescripcion.Text = "";
            this.TXTMARCA.Text = "";
            this.txtModelo.Text = "";
            this.TXTSERIE.Text = "";
            this.TXTOBSERVACION.Text = "";
        }

        /*[FUNCION QUE HABILITA O DESHABILITA LOS BOTONES]*/
        void EstadoTool(bool b, bool n, bool e, bool g, bool el, bool c)
        {
            ctrlToolBarMantenimiento.btnNuevo.Enabled = n;
            ctrlToolBarMantenimiento.btnEditar.Enabled = e;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = g;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = el;
            ctrlToolBarMantenimiento.btnCancelar.Enabled = c;
        }
        
        public static void EjecutarScript(Page Page, string strScript)
        {
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), "OpenPopup", strScript, true);
        }
    }
}