using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.BE.MRE;
using SGAC.Cliente.Colas.BL;
using SGAC.WebApp.Accesorios;

namespace SGAC.WebApp.Colas
{
    public partial class FrmPerfilesAtencion : MyBasePage
    {
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            SeteraControles2();

            Button btnGrabarE = (Button)ctrlToolBarMantenimiento.FindControl("btnGrabar");
            btnGrabarE.OnClientClick = "return ValidarTextBox(this)";

            ctrlOficinaConsular1.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsular_SelectedIndexChanged);

            if (!Page.IsPostBack)
            {
                ctrlOficinaConsular1.Cargar(false);
                ctrlOficinaConsular2.Cargar(false);
                ctrlOficinaConsular2.SelectedValue = ctrlOficinaConsular1.SelectedValue;          
                Session["IQueHace"] = 1;
                ViewState.Add("IdPerfil", 0);
                EstadoTool(true, true, false, true, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo, ctrlToolBarMantenimiento.btnEditar, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar };
                GridView[] arrGridView = { grdPerfilesAtencion };
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }

        }

        void ddlOficinaConsular_SelectedIndexChanged(object sender, EventArgs e)
        {
            ctrlOficinaConsular2.SelectedValue = ctrlOficinaConsular1.SelectedValue;
            updMantenimiento.Update();
        }

        void SeteraControles2()
        {
            ctrlToolBarConsulta.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);
            ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);

            ctrlToolBarMantenimiento.btnNuevoHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonNuevoClick(ctrlToolBarMantenimiento_btnNuevoHandler);
            ctrlToolBarMantenimiento.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarMantenimiento_btnCancelarHandler);
            ctrlToolBarMantenimiento.btnEditarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEditarClick(ctrlToolBarMantenimiento_btnEditarHandler);
            ctrlToolBarMantenimiento.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBarMantenimiento_btnGrabarHandler);
            ctrlToolBarMantenimiento.btnEliminarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEliminarClick(ctrlToolBarMantenimiento_btnEliminarHandler);

            ctrlToolBarConsulta.VisibleIButtonBuscar = true;
            ctrlToolBarConsulta.VisibleIButtonCancelar = true;

            ctrlToolBarMantenimiento.VisibleIButtonNuevo = true;
            ctrlToolBarMantenimiento.VisibleIButtonEditar = true;
            ctrlToolBarMantenimiento.VisibleIButtonGrabar = true;
            ctrlToolBarMantenimiento.VisibleIButtonCancelar = true;
            ctrlToolBarMantenimiento.VisibleIButtonEliminar = true;

            ctrlToolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarConsulta.btnCancelar.Text = "    Limpiar";
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            if (ctrlOficinaConsular1.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "Ingrese una oficina consular" + "');", true);
                return;
            }

            CargarGrilla();
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            ctrlPaginador.Visible = false;
            ctrlOficinaConsular1.SelectedIndex = 0;
            if (Session["dt"] != null)
            {
                DataTable dt = ((DataTable)Session["dt"]).Clone();
                grdPerfilesAtencion.DataSource = dt;
                grdPerfilesAtencion.DataBind();
            }
            else
            {
                grdPerfilesAtencion.DataSource = null;
                grdPerfilesAtencion.DataBind();
            }
        }

        void ctrlToolBarMantenimiento_btnNuevoHandler()
        {
            Session["IQueHace"] = 1;
            limpiar();
            
            EstadoTool(true, true, false, true, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
            ActivarControles(true);
            ViewState.Add("IdPerfil", 0);
            txtDesc.Focus();
        }

        void ctrlToolBarMantenimiento_btnCancelarHandler()
        {
            limpiar();
            ActivarControles(false);
            EstadoTool(true, true, false, false, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
            CargarGrilla();
            Comun.EjecutarScript(Page, Util.HabilitarTab(0));
        }

        void ctrlToolBarMantenimiento_btnEditarHandler()
        {
            Session["IQueHace"] = 2;
            EstadoTool(true, true, false, true, true, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
            ActivarControles(true);
            txtDesc.Focus();
        }

        void ctrlToolBarMantenimiento_btnGrabarHandler()
        {
            CL_PERFILATENCION ObjTicteraBE = new CL_PERFILATENCION();
            PerfilAtencionMantenimientoBL BL = new PerfilAtencionMantenimientoBL();

            int hora = 0;
            int minuto = 0;
            int segundo = 0;
            
            string horaTexto = txtTiemAtencion.Text + ":00";
            hora = Convert.ToInt32(horaTexto.Substring(0, 2));
            minuto = Convert.ToInt32(horaTexto.Substring(3, 2));
            segundo = Convert.ToInt32(horaTexto.Substring(6, 2));

            int intRpta = 0;

            if (ctrlOficinaConsular2.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "Ingrese una oficina consular" + "');", true);
                return;
            }

            if (txtTiemAtencion.Text.Trim().Length != 5)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "Formato de Hora Incorrecto" + "');", true);
                return;
            }
            if (Convert.ToInt32(Session["IQueHace"]) == 1)
            {
                EstadoTool(true, true, false, true, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
            }
            else
            {
                EstadoTool(true, true, true, true, true, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
            }

            if (hora > 24)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "Formato de Hora Incorrecto" + "');", true);
                return;
            }
            if (minuto > 59)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "Formato de Hora Incorrecto" + "');", true);
                return;
            }

            TimeSpan horaactual = new TimeSpan(hora, minuto, segundo);

            updMantenimiento.Update();

            ObjTicteraBE.peat_sOficinaConsularId = Convert.ToInt16(ctrlOficinaConsular2.SelectedValue);
            ObjTicteraBE.peat_vDescripcion = txtDesc.Text;
            ObjTicteraBE.peat_INumeroTicket = Convert.ToInt32(txtCanTick.Text);

            ObjTicteraBE.peat_ITiempoAtencion = horaactual;
            if (Convert.ToInt32(Session["IQueHace"]) == 1)
            {
                ObjTicteraBE.peat_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
                ObjTicteraBE.peat_dFechaCreacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                ObjTicteraBE.peat_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                ObjTicteraBE.peat_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                intRpta = BL.Insertar(ObjTicteraBE);
            }
            else
            {
                ObjTicteraBE.peat_IPerfilId = Convert.ToInt32(ViewState["IdPerfil"]);
                ObjTicteraBE.peat_dFechaModificacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                ObjTicteraBE.peat_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                ObjTicteraBE.peat_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                intRpta = BL.Actualizar(ObjTicteraBE);
            }

            string strScript = string.Empty;
            
            if (intRpta == (int)Enumerador.enmResultadoQuery.OK)
            {
                strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                strScript += Util.NombrarTab(0, Constantes.CONST_TAB_CONSULTAR);
                strScript += Util.HabilitarTab(0);

                CargarGrilla();
            }
            else
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Perfiles de Atencion", Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
            }

            Comun.EjecutarScript(Page, strScript);

            limpiar();
            ActivarControles(false);
            EstadoTool(true, true, false, false, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
        }

        void ctrlToolBarMantenimiento_btnEliminarHandler()
        {
            PerfilAtencionMantenimientoBL BL = new PerfilAtencionMantenimientoBL();
            
            CL_PERFILATENCION srvServicio = new CL_PERFILATENCION();
            srvServicio.peat_IPerfilId = Convert.ToInt32(Session["IdTicketera"]);
            srvServicio.peat_dFechaModificacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
            srvServicio.peat_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
            srvServicio.peat_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
            srvServicio.peat_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            srvServicio.peat_cEstado = Enumerador.enmEstado.DESACTIVO.ToString();

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
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Perfiles de Atencion", Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
            }

            Comun.EjecutarScript(Page, strScript);
        }

        void ctrlToolBarMantenimiento_btnCancelarHnadler()
        {
            limpiar();
            CargarGrilla();
            Comun.EjecutarScript(Page, Util.HabilitarTab(0));
        }

        protected void grdPerfilesAtencion_SelectedIndexChanged(object sender, EventArgs e)
        {
            Comun.EjecutarScript(Page, Util.HabilitarTab(1));
        }

        protected void grdPerfilesAtencion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            string strScript = string.Empty;
            Session["IdTicketera"] = grdPerfilesAtencion.Rows[index].Cells[0].Text;

            try
            {
                ctrlOficinaConsular2.SelectedValue = grdPerfilesAtencion.DataKeys[index].Values["peat_sOficinaConsularId"].ToString();
                ctrlOficinaConsular2.SelectedValue = ctrlOficinaConsular1.SelectedValue;

                ViewState.Add("IdPerfil", grdPerfilesAtencion.Rows[index].Cells[0].Text);

                this.txtDesc.Text = HttpUtility.HtmlDecode(grdPerfilesAtencion.Rows[index].Cells[1].Text);
                this.txtCanTick.Text = grdPerfilesAtencion.Rows[index].Cells[2].Text;
                this.txtTiemAtencion.Text = grdPerfilesAtencion.Rows[index].Cells[3].Text.Substring(0, 5);

                updMantenimiento.Update();

                ctrlOficinaConsular2.Enabled = false;
                this.txtDesc.Enabled = false;
                this.txtCanTick.Enabled = false;
                this.txtTiemAtencion.Enabled = false;

                if (e.CommandName == "Consultar")
                {
                    ctrlOficinaConsular2.Enabled = false;
                    EstadoTool(true, true, true, false, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
                }

                if (e.CommandName == "Editar")
                {
                    Session["IQueHace"] = 2;
                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
                    ctrlOficinaConsular2.Enabled = true;
                    this.txtDesc.Enabled = true;
                    this.txtCanTick.Enabled = true;
                    this.txtTiemAtencion.Enabled = true;

                    txtDesc.Focus();

                    EstadoTool(true, true, false, true, true, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
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
        #endregion

        #region Metodos
        private void CargarGrilla()
        {
            PerfilAtencionConsultaBL BL = new PerfilAtencionConsultaBL();
           
            int intTotalRegistros = 0;
            int intTotalPaginas = 0;

            DataTable dt = BL.Consultar(Convert.ToInt32(this.ctrlOficinaConsular1.SelectedValue), 
                                        ctrlPaginador.PaginaActual, 
                                        Constantes.CONST_CANT_REGISTRO,
                                        ref intTotalRegistros,
                                        ref intTotalPaginas);

            Session.Add("dt", dt);
            grdPerfilesAtencion.SelectedIndex = -1;
            grdPerfilesAtencion.DataSource = dt;
            grdPerfilesAtencion.DataBind();

            // Setea Paginador
            ctrlPaginador.TotalResgistros = intTotalRegistros;
            ctrlPaginador.TotalPaginas = intTotalPaginas;
            ctrlValidacionAtencion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);

            if (grdPerfilesAtencion.Rows.Count > 0)
            {
                ctrlValidacionAtencion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + ctrlPaginador.TotalResgistros, 
                                                            true, 
                                                            Enumerador.enmTipoMensaje.INFORMATION);
            }

            updConsulta.Update();

            ctrlPaginador.Visible = false;
            if (ctrlPaginador.TotalPaginas > 1)
            {
                ctrlPaginador.Visible = true;
            }
        }

        void ActivarControles(bool valor)
        {
            ctrlOficinaConsular2.Enabled = valor;
            this.txtDesc.Enabled = valor;
            this.txtCanTick.Enabled = valor;
            this.txtTiemAtencion.Enabled = valor;
        }

        void limpiar()
        {
            ViewState.Add("IdPerfil", 0);

            this.txtDesc.Text = "";
            this.txtCanTick.Text = "";
            this.txtTiemAtencion.Text = "";
            ctrlOficinaConsular2.Enabled = false;
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
        #endregion
    }
}