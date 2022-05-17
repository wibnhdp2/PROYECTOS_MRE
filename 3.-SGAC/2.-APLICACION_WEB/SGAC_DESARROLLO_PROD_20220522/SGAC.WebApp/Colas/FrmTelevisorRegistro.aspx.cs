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
    public partial class FrmTelevisorRegistro : MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SeteraControles2();

                Button btnGrabarE = (Button)ctrlToolBar2.FindControl("btnGrabar");
                btnGrabarE.OnClientClick = "return ValidarTextBox(this)";

                ctrlToolBar2.btnGrabar.OnClientClick = "return Validar();";

                ctrlOficinaConsular.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsular_SelectedIndexChanged);

                if (!Page.IsPostBack)
                {
                    ctrlOficinaConsular.Cargar(false);
                    ctrlOficinaConsular1.Cargar(false);
                    ctrlToolBar2_btnNuevoHandler();
                }
                if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
                {
                    Button[] arrButtons = { ctrlToolBar2.btnNuevo, ctrlToolBar2.btnEditar, ctrlToolBar2.btnGrabar, ctrlToolBar2.btnEliminar };
                    GridView[] arrGridView = { grdTelevisor };
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
            ctrlToolBar2.btnNuevoHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonNuevoClick(ctrlToolBar2_btnNuevoHandler);
            ctrlToolBar2.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBar2_btnCancelarHandler);
            ctrlToolBar2.btnEditarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEditarClick(ctrlToolBar2_btnEditarHandler);
            ctrlToolBar2.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBar2_btnGrabarHandler);
            ctrlToolBar2.btnEliminarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEliminarClick(ctrlToolBar2_btnEliminarHandler);

            ctrlToolBar1.VisibleIButtonCancelar = true;
            ctrlToolBar1.VisibleIButtonBuscar = true;

            ctrlToolBar2.VisibleIButtonNuevo = true;
            ctrlToolBar2.VisibleIButtonEditar = true;
            ctrlToolBar2.VisibleIButtonGrabar = true;
            ctrlToolBar2.VisibleIButtonCancelar = true;

            ctrlToolBar2.VisibleIButtonEliminar = true;

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
                grdTelevisor.DataSource = dt;
                grdTelevisor.DataBind();
            }
            else
            {
                grdTelevisor.DataSource = null;
                grdTelevisor.DataBind();
            }
        }

        void ctrlToolBar2_btnNuevoHandler()
        {
            Session["IQueHace"] = 1;
            limpiar();
            ActivarControles(true);
            EstadoTool(true, true, false, true, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
            TxtId.Text = "0";
            txtDescripcion.Focus();
        }

        void ctrlToolBar2_btnCancelarHandler()
        {
            string strScript = "";
            limpiar();
            ActivarControles(false);
            CargarGrilla();

            strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
            strScript += Util.NombrarTab(0, Constantes.CONST_TAB_CONSULTAR);
            strScript += Util.HabilitarTab(0);
            Comun.EjecutarScript(Page, Util.HabilitarTab(0));
            EstadoTool(true, true, false, false, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
        }

        void ctrlToolBar2_btnEditarHandler()
        {
            Session["IQueHace"] = 2;
            ActivarControles(true);
            txtDescripcion.Focus();
            EstadoTool(true, true, false, true, true, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
        }

        void ctrlToolBar2_btnGrabarHandler()
        {
            if (ctrlOficinaConsular1.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "Ingrese una oficina consular" + "');", true);
                return;
            }
            
            TicketeraMantenimientoBL ObjBL = new TicketeraMantenimientoBL();
            TelevisorMantenimientoBL BL=new TelevisorMantenimientoBL();
            CL_TELEVISOR ObjTicteraBE = new CL_TELEVISOR();
            int intRpta = 0;

            ObjTicteraBE.telv_sOficinaConsularId = Convert.ToInt16(this.ctrlOficinaConsular1.SelectedValue);
            ObjTicteraBE.telv_vDescripcion = txtDescripcion.Text;
            ObjTicteraBE.telv_vMarca = txtMarca.Text;
            ObjTicteraBE.telv_vModelo = txtModelo.Text;
            ObjTicteraBE.telv_vSerie = txtSerie.Text;
            ObjTicteraBE.telv_vCaracteristicas = TxtObservacion.Text;
            ObjTicteraBE.telv_cEstado = "A";

            if (Convert.ToInt32(Session["IQueHace"]) == 1)
            {
                ObjTicteraBE.telv_dFechaCreacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                ObjTicteraBE.telv_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                ObjTicteraBE.telv_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                intRpta = BL.Insertar(ObjTicteraBE);
            }
            else
            {
                ObjTicteraBE.telv_sTelevisorId = Convert.ToInt16(TxtId.Text);
                ObjTicteraBE.telv_dFechaModificacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                ObjTicteraBE.telv_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                ObjTicteraBE.telv_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                intRpta = BL.Actualizar(ObjTicteraBE);
            }

            string strScript = string.Empty;

            if (intRpta == (int)Enumerador.enmResultadoQuery.OK)
            {
                ctrlToolBar2_btnCancelarHandler();

            }
            else
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Televisores", Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
            }

            Comun.EjecutarScript(Page, strScript);
        }

        void ctrlToolBar2_btnEliminarHandler()
        {
            TelevisorMantenimientoBL BL=new TelevisorMantenimientoBL();

            try
            {
                CL_TELEVISOR srvServicio = new CL_TELEVISOR();
                srvServicio.telv_dFechaModificacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                srvServicio.telv_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                srvServicio.telv_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                srvServicio.telv_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                srvServicio.telv_sTelevisorId = Convert.ToInt16(Session["IdTicketera"]);
                srvServicio.telv_cEstado = Enumerador.enmEstado.DESACTIVO.ToString();

                int intResultado = BL.Eliminar(srvServicio);
                
                string strScript = string.Empty;
                
                if (intResultado == (int)Enumerador.enmResultadoQuery.OK)
                {
                    limpiar();
                    strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                    strScript += Util.NombrarTab(0, Constantes.CONST_TAB_CONSULTAR);
                    strScript += Util.HabilitarTab(0);
                    CargarGrilla();
                }
                else
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Televisores", Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
                }
                
                Comun.EjecutarScript(Page, strScript);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ctrlToolBar2_btnCancelarHnadler()
        {
            limpiar();
            CargarGrilla();
            Comun.EjecutarScript(Page, Util.HabilitarTab(0));
        }

        protected void grdTelevisor_SelectedIndexChanged(object sender, EventArgs e)
        {
            Comun.EjecutarScript(Page, Util.HabilitarTab(1));
        }

        protected void grdTelevisor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            Session["IdTicketera"] = grdTelevisor.Rows[index].Cells[0].Text;
            string strScript = string.Empty;
            try
            {
                ctrlOficinaConsular1.SelectedValue = grdTelevisor.DataKeys[index].Values["telv_sOficinaConsularId"].ToString();

                this.TxtId.Text = grdTelevisor.Rows[index].Cells[0].Text;
                this.txtDescripcion.Text = HttpUtility.HtmlDecode(grdTelevisor.Rows[index].Cells[1].Text);
                this.txtMarca.Text = HttpUtility.HtmlDecode(grdTelevisor.Rows[index].Cells[2].Text);
                this.txtModelo.Text = HttpUtility.HtmlDecode(grdTelevisor.Rows[index].Cells[3].Text);
                this.txtSerie.Text = HttpUtility.HtmlDecode(grdTelevisor.Rows[index].Cells[4].Text);
                this.TxtObservacion.Text = HttpUtility.HtmlDecode(grdTelevisor.Rows[index].Cells[5].Text);


                if (e.CommandName == "Consultar")
                {
                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
                    EstadoTool(true, true, true, false, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/

                    ActivarControles(false);
                }

                if (e.CommandName == "Editar")
                {
                    EstadoTool(true, true, false, true, true, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/

                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
                    Session["IQueHace"] = 2;
                    txtDescripcion.Focus();
                    ActivarControles(true);
                }

                updMantenimiento.Update();

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
            TelevisorConsultaBL BL = new TelevisorConsultaBL();

            try
            {
                int intTotalRegistros = 0;
                int intTotalPaginas = 0;

                DataTable dt = BL.Consultar(Convert.ToInt32(ctrlOficinaConsular.SelectedValue),
                                            ctrlPaginador.PaginaActual,
                                            Constantes.CONST_CANT_REGISTRO,
                                            ref intTotalRegistros, 
                                            ref intTotalPaginas);
                
                Session.Add("dt", dt);
                grdTelevisor.SelectedIndex = -1;
                grdTelevisor.DataSource = dt;
                grdTelevisor.DataBind();

                // Paginador
                ctrlPaginador.TotalResgistros = intTotalRegistros;
                ctrlPaginador.TotalPaginas = intTotalPaginas;

                ctrlPaginador.Visible = false;
                if (ctrlPaginador.TotalPaginas > 1)
                {
                    ctrlPaginador.Visible = true;
                }

                ctrlValidacionTelevisor.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                if (grdTelevisor.Rows.Count > 0)
                {
                    ctrlValidacionTelevisor.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + ctrlPaginador.TotalResgistros, 
                                                              true, 
                                                              Enumerador.enmTipoMensaje.INFORMATION);
                }

                updConsulta.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ActivarTool()
        {
            ctrlToolBar2.btnNuevo.Enabled = !ctrlToolBar2.btnNuevo.Enabled;
            ctrlToolBar2.btnEditar.Enabled = !ctrlToolBar2.btnEditar.Enabled;
            ctrlToolBar2.btnGrabar.Enabled = !ctrlToolBar2.btnGrabar.Enabled;
            ctrlToolBar2.btnEliminar.Enabled = !ctrlToolBar2.btnEliminar.Enabled;
        }

        void ActivarControles(bool valor)
        {
            this.txtMarca.Enabled = valor;
            this.txtDescripcion.Enabled = valor;
            this.txtModelo.Enabled = valor;
            this.TxtObservacion.Enabled = valor;
            this.txtSerie.Enabled = valor;
            ctrlOficinaConsular1.Enabled = valor;
        }

        void limpiar()
        {
            this.TxtId.Text = "";
            this.txtDescripcion.Text = "";
            this.txtMarca.Text = "";
            this.txtModelo.Text = "";
            this.txtSerie.Text = "";
            this.TxtObservacion.Text = "";
            ctrlOficinaConsular1.Enabled = false;
        }

        /*[FUNCION QUE HABILITA O DESHABILITA LOS BOTONES]*/
        void EstadoTool(bool b, bool n, bool e, bool g, bool el, bool c)
        {
            ctrlToolBar2.btnNuevo.Enabled = n;
            ctrlToolBar2.btnEditar.Enabled = e;
            ctrlToolBar2.btnGrabar.Enabled = g;
            ctrlToolBar2.btnEliminar.Enabled = el;
            ctrlToolBar2.btnCancelar.Enabled = c;
        }

    }
}