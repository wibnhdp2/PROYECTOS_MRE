using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.BE.MRE;
using System.Data;
using SGAC.Cliente.Colas.BL;
using SGAC.Accesorios;

namespace SGAC.WebApp.Colas
{
    public partial class FrmVideRegistro : MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SeteraControles2();

                Button btnGrabarE = (Button)ctrlToolBarMantenimiento.FindControl("btnGrabar");
                btnGrabarE.OnClientClick = "return ValidarTextBox(this)";

                ctrlOficinaConsular.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsular_SelectedIndexChanged);
                if (!Page.IsPostBack)
                {
                    ctrlOficinaConsular.Cargar(false);
                    ctrlOficinaConsular1.Cargar(false);
                    ctrlToolBarMantenimiento_btnNuevoHandler();
                }
                if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
                {                    
                    Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo, ctrlToolBarMantenimiento.btnEditar, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar };
                    GridView[] arrGridView = { grdVideos };
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
            ctrlOficinaConsular1.SelectedValue = ctrlOficinaConsular.SelectedValue;
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

        void ctrlToolBar1_btnBuscarHandler()
        {
            if (ctrlOficinaConsular.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "Ingrese una oficina consular" + "');", true);
                return;
            }
            CargarGrilla();
        }

        void ctrlToolBar1_btnCancelarHandler()
        {
            ctrlOficinaConsular.SelectedIndex = 0;
            if (Session["dt"] != null)
            {
                DataTable dt = ((DataTable)Session["dt"]).Clone();
                grdVideos.DataSource = dt;
                grdVideos.DataBind();
            }
            else
            {
                grdVideos.DataSource = null;
                grdVideos.DataBind();
            }
        }

        void ctrlToolBarMantenimiento_btnNuevoHandler()
        {
            EstadoTool(true, true, false, true, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));

            Session["IQueHace"] = 1;
            limpiar();

            ActivarControles(true);
            limpiar();

            ViewState["CodigoVideo"] = 0;
            txtNombre.Focus();
        }

        void ctrlToolBarMantenimiento_btnCancelarHandler()
        {
            EstadoTool(true, true, false, false, false, false);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/


            Comun.EjecutarScript(Page, Util.NombrarTab(0, Constantes.CONST_TAB_INICIAL) + Util.HabilitarTab(0));

            limpiar();
            ActivarControles(false);
            CargarGrilla();
        }

        void ctrlToolBarMantenimiento_btnEditarHandler()
        {
            Session["IQueHace"] = 2;
            ActivarControles(true);
            txtDescripcion.Focus();
            EstadoTool(true, true, false, true, true, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_EDITAR) + Util.DeshabilitarTab(0));
        }

        void ctrlToolBarMantenimiento_btnGrabarHandler()
        {
            if (ctrlOficinaConsular1.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "Ingrese una oficina consular" + "');", true);
                return;
            }
            
            TicketeraMantenimientoBL ObjBL = new TicketeraMantenimientoBL();
            VideoMantenimientoBL BL = new VideoMantenimientoBL();
            CL_VIDEO ObjTicteraBE = new CL_VIDEO();

            int intRpta = 0;

            updMantenimiento.Update();

            ObjTicteraBE.vide_sOficinaConsularId = Convert.ToInt16(this.ctrlOficinaConsular1.SelectedValue);
            ObjTicteraBE.vide_vDescripcion = txtDescripcion.Text;
            ObjTicteraBE.vide_IOrden = Convert.ToInt32(TXTORDEN.Text);
            ObjTicteraBE.vide_vUrl = txtNombre.Text;

            if (Convert.ToInt32(Session["IQueHace"]) == 1)
            {
                foreach (GridViewRow row in grdVideos.Rows)
                {
                    if (Convert.ToInt32(row.Cells[3].Text) == Convert.ToInt32(TXTORDEN.Text))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "El número de orden ya existe" + "');", true);
                        return;
                    }
                }
                ObjTicteraBE.vide_dFechaCreacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                ObjTicteraBE.vide_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                ObjTicteraBE.vide_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                intRpta = BL.Insertar(ObjTicteraBE);
            }
            else
            {
                foreach (GridViewRow row in grdVideos.Rows)
                {
                    if (Convert.ToInt32(row.Cells[0].Text) != Convert.ToInt32(Session["IdTicketera"]))
                    {
                        if (Convert.ToInt32(row.Cells[3].Text) == Convert.ToInt32(TXTORDEN.Text))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "El número de orden ya existe" + "');", true);
                            return;
                        }
                    }
                }

                ObjTicteraBE.vide_sVideoId = Convert.ToInt16(ViewState["CodigoVideo"]);
                ObjTicteraBE.vide_dFechaModificacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                ObjTicteraBE.vide_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                ObjTicteraBE.vide_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

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
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Video", Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
            }

            Comun.EjecutarScript(Page, strScript);
        }

        void ctrlToolBarMantenimiento_btnEliminarHandler()
        {
            object[] parObjetos = new object[1];
            VideoMantenimientoBL BL = new VideoMantenimientoBL();

            try
            {
                CL_VIDEO srvServicio = new CL_VIDEO();
                srvServicio.vide_dFechaModificacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                srvServicio.vide_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                srvServicio.vide_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                srvServicio.vide_sVideoId = Convert.ToInt16(Session["IdTicketera"]);
                srvServicio.vide_cEstado = Enumerador.enmEstado.DESACTIVO.ToString();
                parObjetos[0] = srvServicio;
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
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Video", Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
                }
                
                Comun.EjecutarScript(Page, strScript);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ctrlToolBarMantenimiento_btnCancelarHnadler()
        {
            limpiar();
            CargarGrilla();
            Comun.EjecutarScript(Page, Util.HabilitarTab(0));
        }

        protected void grdVideos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Comun.EjecutarScript(Page, Util.HabilitarTab(1));
        }

        protected void grdVideos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            string strScript = string.Empty;
            Session["IdTicketera"] = grdVideos.Rows[index].Cells[0].Text;

            try
            {
                ctrlOficinaConsular1.SelectedValue = grdVideos.DataKeys[index].Values["vide_sOficinaConsularId"].ToString();
                ViewState.Add("CodigoVideo", HttpUtility.HtmlDecode(grdVideos.Rows[index].Cells[0].Text));
                this.txtDescripcion.Text = HttpUtility.HtmlDecode(grdVideos.Rows[index].Cells[1].Text);
                this.txtNombre.Text = HttpUtility.HtmlDecode(grdVideos.Rows[index].Cells[2].Text);
                this.TXTORDEN.Text = HttpUtility.HtmlDecode(grdVideos.Rows[index].Cells[3].Text);

                updMantenimiento.Update();

                if (e.CommandName == "Consultar")
                {
                    ActivarControles(false);
                    EstadoTool(true, true, true, false, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
                }

                if (e.CommandName == "Editar")
                {
                    EstadoTool(true, true, false, true, true, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
                    Session["IQueHace"] = 2;
                    ActivarControles(true);
                    txtDescripcion.Focus();
                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
                }

                Comun.EjecutarScript(Page, strScript);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            VideoConsultaBL BL = new VideoConsultaBL();

            try
            {
                int intTotalRegistros = 0;
                int intTotalPaginas = 0;

                DataTable dt = BL.Consultar(Convert.ToInt32(this.ctrlOficinaConsular.SelectedValue),
                                            ctrlPaginador.PaginaActual, Constantes.CONST_CANT_REGISTRO,
                                            ref intTotalRegistros,
                                            ref intTotalPaginas);
                
                Session.Add("dt", dt);
                grdVideos.SelectedIndex = -1;
                grdVideos.DataSource = dt;
                grdVideos.DataBind();

                // Paginador
                ctrlPaginador.TotalResgistros = intTotalRegistros;
                ctrlPaginador.TotalPaginas = intTotalPaginas;

                ctrlValidacionVideo.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);

                if (grdVideos.Rows.Count > 0)
                {
                    ctrlValidacionVideo.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + ctrlPaginador.TotalResgistros, 
                                                          true, 
                                                          Enumerador.enmTipoMensaje.INFORMATION);
                }

                ctrlPaginador.Visible = false;
                if (ctrlPaginador.TotalPaginas > 1)
                {
                    ctrlPaginador.Visible = true;
                }

                updConsulta.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ActivarControles(bool valor)
        {
            this.txtDescripcion.Enabled = valor;
            this.txtNombre.Enabled = valor;
            this.TXTORDEN.Enabled = valor;
            ctrlOficinaConsular1.Enabled = valor;
        }

        void limpiar()
        {
            ViewState["CodigoVideo"] = 0;
            this.txtDescripcion.Text = string.Empty;
            this.txtNombre.Text = string.Empty;
            this.TXTORDEN.Text = string.Empty;
        }

        protected void updGrillaConsulta_Init(object sender, EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
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
    }
}