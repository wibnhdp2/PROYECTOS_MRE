 using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.BE.MRE;
using SGAC.Cliente.Colas.BL;
using SGAC.Controlador;
using SGAC.WebApp.Accesorios;

namespace SGAC.WebApp.Colas
{
    public partial class FrmConfiguracionVentanilla : MyBasePage
    {
        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
            SeteraControles2();                    
        }

        string LastServicio = string.Empty;
        int CurrentRow = -1;

        protected void Page_Load(object sender, EventArgs e)
        {            
            ctrlToolBarMantenimiento.btnGrabar.OnClientClick = "return Validar();";

            if (!Page.IsPostBack)
            {
                SeteraControles2();
                ddlOficinaConsularConsulta.Cargar(false);
                ddlOficinaConsularConsulta2.Cargar(false);

                Util.CargarParametroDropDownList(ddlSector, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.COLAS_SECTOR_CONSULAR));
                EstadoTool(true, true, false, true, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
                Session["AccionBoton"] = "NUEVO";
                ddlOficinaConsularConsulta2.Enabled = true;
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo,ctrlToolBarMantenimiento.btnEditar, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar };
                GridView[] arrGridView = { grdVentanilla };
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }

        }

        void ddlOficinaConsular_SelectedIndexChanged(object sender, EventArgs e)
        {
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

            ctrlToolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarConsulta.btnCancelar.Text = "    Limpiar";

            Comun.CargarPermisos(Session, ctrlToolBarConsulta, ctrlToolBarMantenimiento, grdVentanilla, HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            if (ddlOficinaConsularConsulta.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "Ingrese una oficina consular" + "');", true);
                return;
            }

            CargarGrilla();
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            ddlOficinaConsularConsulta.SelectedIndex = 0;
            if (Session["dtVentanilla"] != null)
            {
                DataTable dt = ((DataTable)Session["dtVentanilla"]).Clone();
                grdVentanilla.DataSource = dt;
                grdVentanilla.DataBind();
            }
            else
            {
                grdVentanilla.DataSource = null;
                grdVentanilla.DataBind();
            }
        }

        void ctrlToolBarMantenimiento_btnNuevoHandler()
        {
            try
            {
                EstadoTool(true, true, false, true, false, true);
                Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR) + Util.DeshabilitarTab(0));

                Session["AccionBoton"] = "NUEVO";

                ddlSector.SelectedIndex = -1;
                ddlSector.Enabled = true;

                ddlOficinaConsularConsulta2.SelectedIndex = -1;
                ddlOficinaConsularConsulta2.Enabled = true;
                txtDescripcion.Text = string.Empty;
                txtDescripcion.Enabled = true;
                txtNumOrden.Text = string.Empty;
                txtNumOrden.Enabled = true;
                grvVentanillaServicio.DataSource = Cabecera_detalle();
                grvVentanillaServicio.DataBind();
                btnAdicionarServicios.Enabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ctrlToolBarMantenimiento_btnCancelarHandler()
        {
            try
            {
                EstadoTool(true, true, false, false, false, false);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
                Comun.EjecutarScript(Page, Util.NombrarTab(0, Constantes.CONST_TAB_INICIAL) + Util.HabilitarTab(0));
                txtDescripcion.Text = string.Empty;
                txtDescripcion.Enabled = false;
                txtNumOrden.Text = string.Empty;
                txtNumOrden.Enabled = false;
                ddlSector.Enabled = false;
                btnAdicionarServicios.Enabled = false;
                ddlOficinaConsularConsulta2.Enabled = false;

                grvVentanillaServicio.DataSource = Cabecera_detalle();
                grvVentanillaServicio.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ctrlToolBarMantenimiento_btnEditarHandler()
        {
            try
            {
                EstadoTool(true, true, false, true, true, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
                Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_EDITAR) + Util.DeshabilitarTab(0));
                Session["AccionBoton"] = "MODIFICAR";
                ddlOficinaConsularConsulta2.Enabled = true;
                txtNumOrden.Enabled = true;
                txtDescripcion.Enabled = true;
                btnAdicionarServicios.Enabled = true;
                ddlSector.Enabled = true;
                ddlOficinaConsularConsulta2.Enabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ctrlToolBarMantenimiento_btnGrabarHandler()
        {
            CL_VENTANILLA objEnVentanilla = new CL_VENTANILLA();
            List<CL_VENTANILLASERVICIO> objListVenServicio = new List<CL_VENTANILLASERVICIO>();

            VentanillaMantenimientoBL BL = new VentanillaMantenimientoBL();
            VentanillaServicioConsultaBL objLo = new VentanillaServicioConsultaBL();

            if (ddlOficinaConsularConsulta2.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "Ingrese una oficina consular" + "');", true);
                return;
            }

            if ((String)Session["AccionBoton"] == "NUEVO")
            {
                foreach (GridViewRow row in grdVentanilla.Rows)
                {
                    if (Convert.ToInt32(row.Cells[3].Text) == Convert.ToInt32(txtNumOrden.Text.Trim()))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "El número de orden ya existe" + "');", true);
                        return;
                    }
                }
            }

            if ((String)Session["AccionBoton"] == "MODIFICAR")
            {
                foreach (GridViewRow row in grdVentanilla.Rows)
                {
                    if (Convert.ToInt32(row.Cells[1].Text) != Convert.ToInt32(ViewState["CodigoVentanilla"]))
                    {
                        if (Convert.ToInt32(row.Cells[3].Text) == Convert.ToInt32(txtNumOrden.Text))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "El número de orden ya existe" + "');", true);
                            return;
                        }
                    }
                }
            }

            int intRpta = 0;

            try
            {
                object[] parObjetos = new object[1];

                objEnVentanilla.vent_sVentanillaId = Convert.ToInt16(ViewState["CodigoVentanilla"]);
                objEnVentanilla.vent_sOficinaConsularId = Convert.ToInt16(ddlOficinaConsularConsulta2.SelectedValue);
                objEnVentanilla.vent_vDescripcion = txtDescripcion.Text.Trim();
                objEnVentanilla.vent_INumeroOrden = Convert.ToInt32(txtNumOrden.Text.Trim());
                objEnVentanilla.vent_sSectorId = Convert.ToInt16(ddlSector.SelectedValue);
                objEnVentanilla.vent_cEstado = "A";
                objEnVentanilla.vent_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                objEnVentanilla.vent_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                objEnVentanilla.vent_dFechaCreacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());

                objEnVentanilla.vent_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                objEnVentanilla.vent_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                objEnVentanilla.vent_dFechaModificacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());

                CL_VENTANILLASERVICIO objEnVenServicio;
                foreach (GridViewRow row in grvVentanillaServicio.Rows)
                {
                    objEnVenServicio = new CL_VENTANILLASERVICIO();
                    CheckBox chkObligatorio = (CheckBox)row.FindControl("chkObligatorio");
                    Label LblDato = (Label)row.FindControl("lblserv_vDescripcion");
                    string ssss = LblDato.Text;

                    objEnVenServicio.vede_sVentanillaId = Convert.ToInt16(ViewState["CodigoVentanilla"]);
                    objEnVenServicio.vede_sServicioId = Convert.ToInt16(row.Cells[0].Text.Trim());
                    objEnVenServicio.vede_IObligatorio = chkObligatorio.Checked ? 1 : 2;
                    objEnVenServicio.vede_cEstado = "A";
                    objEnVenServicio.vede_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                    objEnVenServicio.vede_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    objEnVenServicio.vede_dFechaCreacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());

                    objEnVenServicio.vede_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                    objEnVenServicio.vede_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    objEnVenServicio.vede_dFechaModificacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());

                    objListVenServicio.Add(objEnVenServicio);
                }

                if ((String)Session["AccionBoton"] == "NUEVO")
                {
                    intRpta = BL.Insertar(objEnVentanilla, objListVenServicio);
                }

                if ((String)Session["AccionBoton"] == "MODIFICAR")
                {
                    intRpta = BL.Actualizar(objEnVentanilla, objListVenServicio);
                }

                string strScript = string.Empty;

                if (intRpta == (int)Enumerador.enmResultadoOperacion.OK)
                {
                    EstadoTool(true, true, false, false, false, true); /*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/

                    txtDescripcion.Text = string.Empty;
                    txtDescripcion.Enabled = false;
                    txtNumOrden.Text = string.Empty;
                    txtNumOrden.Enabled = false;
                    btnAdicionarServicios.Enabled = false;

                    grvVentanillaServicio.DataSource = Cabecera_detalle();
                    grvVentanillaServicio.DataBind();

                    strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                    strScript += Util.NombrarTab(0, Constantes.CONST_TAB_CONSULTAR);
                    strScript += Util.HabilitarTab(0);

                    CargarGrilla();
                }
                else
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Ventanilla", Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
                }

                ddlOficinaConsularConsulta2.Enabled = false;

                Comun.EjecutarScript(Page, strScript);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ctrlToolBarMantenimiento_btnEliminarHandler()
        {
            int intRpta = 0;

            VentanillaMantenimientoBL BL = new VentanillaMantenimientoBL();
            VentanillaServicioConsultaBL objLo = new VentanillaServicioConsultaBL();
            CL_VENTANILLA objEnVentanilla = new CL_VENTANILLA();

            try
            {
                objEnVentanilla.vent_sVentanillaId = Convert.ToInt16(ViewState["CodigoVentanilla"]);
                objEnVentanilla.vent_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                objEnVentanilla.vent_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                objEnVentanilla.vent_dFechaModificacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                objEnVentanilla.vent_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());

                Object[] miArrayServicioCons = new Object[1] { objEnVentanilla };

                intRpta = BL.Eliminar(objEnVentanilla);

                string strScript = string.Empty;

                if (intRpta == (int)Enumerador.enmResultadoOperacion.OK)
                {
                    EstadoTool(true, true, false, true, false, true); /*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/

                    txtDescripcion.Text = string.Empty;
                    txtDescripcion.Enabled = false;
                    txtNumOrden.Text = string.Empty;
                    txtNumOrden.Enabled = false;
                    btnAdicionarServicios.Enabled = false;

                    grvVentanillaServicio.DataSource = Cabecera_detalle();
                    grvVentanillaServicio.DataBind();

                    strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                    strScript += Util.NombrarTab(0, Constantes.CONST_TAB_CONSULTAR);
                    strScript += Util.HabilitarTab(0);

                    CargarGrilla();
                }
                else
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Ventanilla", Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
                }

                ddlOficinaConsularConsulta2.Enabled = false;

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
            VentanillaConsultaBL BL = new VentanillaConsultaBL();

            try
            {
                DataTable dt = new DataTable();

                int intTotalRegistros = 0;
                int intTotalPaginas = 0;

                dt = BL.Consultar(Convert.ToInt32(ddlOficinaConsularConsulta.SelectedValue),
                                  ctrlPaginador.PaginaActual,
                                  Constantes.CONST_CANT_REGISTRO,
                                  ref intTotalRegistros,
                                  ref intTotalPaginas);

                Session.Add("dt", dt);
                Session["dtVentanilla"] = dt;
                grdVentanilla.SelectedIndex = -1;
                grdVentanilla.DataSource = dt;
                grdVentanilla.DataBind();

                // Paginador
                ctrlPaginador.TotalResgistros = intTotalRegistros;
                ctrlPaginador.TotalPaginas = intTotalPaginas;

                ctrlValidacionVentanilla.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                if (dt.Rows.Count > 0)
                {
                    ctrlValidacionVentanilla.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + ctrlPaginador.TotalResgistros, true, Enumerador.enmTipoMensaje.INFORMATION);
                }
                UpdatePanel1.Update();

                ctrlPaginador.Visible = false;
                if (ctrlPaginador.TotalPaginas > 1)
                {
                    ctrlPaginador.Visible = true;
                }

                updGrillaConsulta.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grdVentanilla_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string strScript = string.Empty;
                int index = Convert.ToInt32(e.CommandArgument);

                ddlOficinaConsularConsulta2.SelectedValue = grdVentanilla.Rows[index].Cells[0].Text;
                ViewState.Add("sOficinaConsular", grdVentanilla.Rows[index].Cells[0].Text);
                ViewState.Add("CodigoVentanilla", grdVentanilla.Rows[index].Cells[1].Text);
                txtDescripcion.Text = HttpUtility.HtmlDecode(grdVentanilla.Rows[index].Cells[2].Text);
                txtNumOrden.Text = grdVentanilla.Rows[index].Cells[3].Text;
                ddlSector.SelectedValue = grdVentanilla.Rows[index].Cells[4].Text;

                mostrar_ServiciosVentanilla();

                if (e.CommandName == "Consultar")
                {
                    EstadoTool(true, true, true, false, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
                    btnAdicionarServicios.Enabled = false;
                    ddlSector.Enabled = false;
                    ddlOficinaConsularConsulta2.Enabled = false;
                    txtDescripcion.Enabled = false;
                    txtNumOrden.Enabled = false;

                }
                else if (e.CommandName == "Editar")
                {
                    EstadoTool(true, true, false, true, true, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
                    ddlOficinaConsularConsulta2.Enabled = true;
                    btnAdicionarServicios.Enabled = true;
                    txtDescripcion.Enabled = true;
                    txtNumOrden.Enabled = true;
                    ddlSector.Enabled = true;
                    Session["AccionBoton"] = "MODIFICAR";
                }

                Comun.EjecutarScript(Page, strScript);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        private void mostrar_ServiciosVentanilla()
        {
            VentanillaServicioConsultaBL BL = new VentanillaServicioConsultaBL();

            try
            {
                DataTable dt = new DataTable();

                int intTotalRegistros = 0;
                int intTotalPaginas = 0;

                intTotalRegistros = 10;
                intTotalPaginas = 2;

                dt = BL.Consultar(Convert.ToInt32(ViewState["CodigoVentanilla"]),
                                  ctrlPaginador.PaginaActual,
                                  Constantes.CONST_CANT_REGISTRO,
                                  ref intTotalRegistros,
                                  ref intTotalPaginas);

                Session["dtServiciosVentanilla"] = dt;
                grvVentanillaServicio.DataSource = dt;
                grvVentanillaServicio.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void mostrar_Servicios()
        {
            try
            {
                //Proceso p = new Proceso();
                DataTable dt = new DataTable();
                object[] arrParametros = ObtenerFiltroServicios();

                //dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CL_SERVICIO", "LISTARREGISTRO");


                ServicioConsultaBL objServicioConsultaBL = new ServicioConsultaBL();

                dt = objServicioConsultaBL.GetAll(Convert.ToInt32(arrParametros[0].ToString()), arrParametros[1].ToString());

                //if (p.IErrorNumero == 0)
                if (dt.Rows.Count > 0)
                {
                    Session["dtServicios"] = dt;

                    grvServicios.SelectedIndex = -1;
                    grvServicios.DataSource = dt;
                    grvServicios.DataBind();
                }
                else
                {
                    //string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Ventanilla", p.vErrorMensaje);
                    string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Ventanilla", "No existe ningún Servicio registrado.");
                    Comun.EjecutarScript(Page, strScript);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private object[] ObtenerFiltroServicios()
        {
            string serv_sServicioId = string.Empty;
            int n = 0;

            foreach (GridViewRow row in grvVentanillaServicio.Rows)
            {
                serv_sServicioId += n == 0 ? row.Cells[0].Text.ToString() : "-" + row.Cells[0].Text.ToString();
                n++;
            }

            object[] arrParametros = new object[2];
            arrParametros[0] = Convert.ToInt32(ddlOficinaConsularConsulta2.SelectedValue);
            arrParametros[1] = serv_sServicioId;

            return arrParametros;
        }

        protected void btnAdicionarServicios_Click(object sender, EventArgs e)
        {
            try
            {
                ModalPanel_Servicio.Show();
                mostrar_Servicios();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grvVentanillaServicio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(hd_rowIndex.Value);


                if (!btnAdicionarServicios.Enabled) return;

                if (e.CommandName == "Eliminar")
                {
                    DataTable dt = (DataTable)Session["dtServiciosVentanilla"];
                    dt.Rows[index].Delete();
                    dt.AcceptChanges();

                    if (grvVentanillaServicio.Rows.Count == 1)
                    {
                        dt = Cabecera_detalle();
                    }

                    Session["dtServiciosVentanilla"] = dt;

                    grvVentanillaServicio.DataSource = dt;
                    grvVentanillaServicio.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable Cabecera_detalle()
        {
            DataTable dt = new DataTable("detalle");

            dt.Columns.Add("vede_sVentanillaId", typeof(Int32));
            dt.Columns.Add("vent_sOficinaConsularId", typeof(Int32));
            dt.Columns.Add("vede_sServicioId", typeof(Int32));
            dt.Columns.Add("serv_vDescripcion", typeof(string));
            dt.Columns.Add("subServicio", typeof(string));
            dt.Columns.Add("vede_IObligatorio", typeof(Int32));
            dt.Columns.Add("vede_cEstado", typeof(string));
            dt.Columns.Add("vede_sUsuarioCreacion", typeof(Int32));
            dt.Columns.Add("vede_vIPCreacion", typeof(string));
            dt.Columns.Add("serv_IOrden", typeof(Int32));
            dt.Columns.Add("serv_sTipo", typeof(Int32));

            return dt;
        }

        protected void grvServicios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {

                DataRowView row = ((DataRowView)(e.Row.DataItem));

                if (LastServicio == row["SERVICIO"].ToString())
                {
                    if ((grvServicios.Rows[CurrentRow].Cells[2].RowSpan == 0))
                    {
                        grvServicios.Rows[CurrentRow].Cells[2].RowSpan = 2;
                    }
                    else
                    {
                        grvServicios.Rows[CurrentRow].Cells[2].RowSpan++;
                    }
                    e.Row.Cells.RemoveAt(2);

                }
                else
                {
                    e.Row.VerticalAlign = VerticalAlign.Middle;
                    LastServicio = row["SERVICIO"].ToString();
                    CurrentRow = e.Row.RowIndex;
                }
            }


        }

        protected void btnAdicionarServicio_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();

                dt = Cabecera_detalle();
                int obligatorio = 0;

                foreach (GridViewRow row in grvServicios.Rows)
                {
                    CheckBox chkRow = (CheckBox)row.FindControl("chkRow");

                    if (chkRow.Checked)
                    {
                        DataRow newrow = dt.NewRow();

                        int fila = row.RowIndex;
                        string lblOfConsular = grvServicios.DataKeys[fila].Values["serv_sOficinaConsularId"].ToString();
                        string lblcodServicio = grvServicios.DataKeys[fila].Values["serv_sServicioId"].ToString();
                        string lblServicio = grvServicios.DataKeys[fila].Values["SERVICIO"].ToString();
                        string lblSubServicio = grvServicios.DataKeys[fila].Values["SUBSERVICIO"].ToString();
                        string lblserv_IOrden = grvServicios.DataKeys[fila].Values["serv_IOrden"].ToString();

                        newrow["serv_sTipo"] = row.Cells[0].Text.Trim();
                        newrow["vede_sVentanillaId"] = Convert.ToInt32(ViewState["CodigoVentanilla"]);
                        newrow["vent_sOficinaConsularId"] = lblOfConsular;
                        newrow["vede_sServicioId"] = lblcodServicio;
                        newrow["serv_vDescripcion"] = lblServicio;
                        newrow["subServicio"] = lblSubServicio;

                        obligatorio = 2;

                        foreach (GridViewRow row1 in grvVentanillaServicio.Rows)
                        {
                            if (row1.Cells[0].Text.Trim() == lblcodServicio.Trim())
                            {
                                CheckBox chkObligatorio = (CheckBox)row1.FindControl("chkObligatorio");

                                if (chkObligatorio.Checked)
                                {
                                    obligatorio = 1;
                                }
                            }
                        }

                        newrow["vede_IObligatorio"] = obligatorio;
                        newrow["serv_IOrden"] = Convert.ToInt32(lblserv_IOrden.Trim());
                        dt.Rows.Add(newrow);
                    }
                }

                Session["dtServiciosVentanilla"] = dt;

                grvVentanillaServicio.DataSource = dt;
                grvVentanillaServicio.DataBind();

                grvVentanillaServicio.SelectedIndex = -1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void grvVentanillaServicio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowType == DataControlRowType.DataRow))
            {

                DataRowView row = ((DataRowView)(e.Row.DataItem));

                if (LastServicio == row["serv_vDescripcion"].ToString())
                {
                    if ((grvVentanillaServicio.Rows[CurrentRow].Cells[2].RowSpan == 0))
                    {
                        grvVentanillaServicio.Rows[CurrentRow].Cells[2].RowSpan = 2;
                    }
                    else
                    {
                        grvVentanillaServicio.Rows[CurrentRow].Cells[2].RowSpan++;
                    }
                    e.Row.Cells.RemoveAt(2);
                }
                else
                {
                    e.Row.VerticalAlign = VerticalAlign.Middle;
                    LastServicio = row["serv_vDescripcion"].ToString();
                    CurrentRow = e.Row.RowIndex;
                }
            }
        }

        protected void imgCerrar_Click(object sender, ImageClickEventArgs e)
        {

        }

    }
}
