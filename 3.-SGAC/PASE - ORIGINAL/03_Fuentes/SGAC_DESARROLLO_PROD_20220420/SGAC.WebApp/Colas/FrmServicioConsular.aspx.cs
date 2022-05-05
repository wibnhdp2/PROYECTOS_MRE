using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.Cliente.Colas.BL;
using SGAC.Controlador;
using SGAC.WebApp.Accesorios;

namespace SGAC.WebApp.Colas
{
    public partial class FrmServiciosConsulares : MyBasePage
    {
        Proceso p = new Proceso();
        #region CAMPOS
        private string strNombreEntidad = "SERVICIO CONSULAR";
        private string strVariableAccion = "ServicioConsular_Accion";
        private string strVariableDt = "ServicioConsular_Tabla";
        private string strVariableIndice = "ServicioConsular_Indice";

        #endregion

        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;

            ctrlPaginadorDetalle.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginadorDetalle.Visible = false;
            ctrlPaginadorDetalle.PaginaActual = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ctrlToolBarConsulta.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);
                ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);
                ctrlToolBarMantenimiento.btnNuevoHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonNuevoClick(ctrlToolBarMantenimiento_btnNuevoHandler);
                ctrlToolBarMantenimiento.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarMantenimiento_btnCancelarHandler);
                ctrlToolBarMantenimiento.btnEditarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEditarClick(ctrlToolBarMantenimiento_btnEditarHandler);
                ctrlToolBarMantenimiento.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBarMantenimiento_btnGrabarHandler);
                ctrlToolBarMantenimiento.btnEliminarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEliminarClick(ctrlToolBarMantenimiento_btnEliminarHandler);

                ctrlToolBarConsulta.btnImprimir.Visible = false;

                Comun.CargarPermisos(Session, ctrlToolBarConsulta, ctrlToolBarMantenimiento, grdVentanilla, HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);
                btnsubservicios.Enabled = true;
                ctrlToolBarMantenimiento.btnGrabar.OnClientClick = "return Validar();";

                ctrlToolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
                ctrlToolBarConsulta.btnCancelar.Text = "    Limpiar";
                this.ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);

                if (!Page.IsPostBack)
                {
                    EstadoTool(true, true, false, true, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
                    Session["DtRegistrosSubServicios"] = CrearDTRegistrosSubServicios();
                    ((DataTable)Session["DtRegistrosSubServicios"]).Clear();
                    CargarDatosIniciales();
                    ctrlOficinaConsular.Cargar(false);
                    ctrlOficinaConsular1.Cargar(false);

                    Session["IQueHace"] = 1;
                    ViewState.Add("AccionBoton", "NUEVO");
                }
                if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
                {
                    Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo, ctrlToolBarMantenimiento.btnEditar, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar};
                    GridView[] arrGridView = { grdVentanilla };
                    Comun.ModoLectura(ref arrButtons);
                    Comun.ModoLectura(ref arrGridView);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*[EVENTO QUE PERMITE REALIZAR LA BUSQUEDA O FUERZA A ACTUALIZAR LA GRILLA SEGUN EL CODIGO DE LA OFICINA DE CONSULADO]*/
        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            if (ctrlOficinaConsular.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "Ingrese una oficina consular" + "');", true);
                return;
            }

            CargarGrilla();
        }

        /*[EVENTO QUE PERMITE CANCELAR EL PROCESO]*/
        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            ctrlOficinaConsular.SelectedIndex = 0;
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

            ctrlPaginador.Visible = false;
        }

        /*[EVENTO QUE NOS PERMITE GENERAR UN NUEVO REGISTRO]*/
        void ctrlToolBarMantenimiento_btnNuevoHandler()
        {
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR) + Util.DeshabilitarTab(0));
            RegistrarEntidad();
            txtDesc.Focus();

            ViewState.Add("AccionBoton", "NUEVO");
            DtRegistros = null;
            ctrlOficinaConsular1.Enabled = true;
            txtDesc.Enabled = true;
            txtNumOrd.Enabled = true;
            //--------------------------------------------
            //Fecha: 23/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se adiciona la columna: serv_vServicioDireccion
            //--------------------------------------------

            txtDireccionServicio.Enabled = true;
        }

        /*[EVENTO QUE NOS PERMITE CANCELAR EL PROCESO DE OPERACION]*/
        void ctrlToolBarMantenimiento_btnCancelarHandler()
        {
            Comun.EjecutarScript(Page, Util.NombrarTab(0, Constantes.CONST_TAB_INICIAL) + Util.HabilitarTab(0));

            CargarGrilla();
            RegistrarEntidad();
            EstadoTool(true, true, false, false, false, false);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/

            txtDesc.Enabled = false;
            txtNumOrd.Enabled = false;
            //--------------------------------------------
            //Fecha: 23/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se adiciona la columna: serv_vServicioDireccion
            //--------------------------------------------

            txtDireccionServicio.Enabled = false;
            btnsubservicios.Enabled = false;
            ctrlOficinaConsular1.Enabled = false;
        }

        void ctrlToolBarMantenimiento_btnEditarHandler()
        {
            ModificarEntidad();
            ctrlOficinaConsular1.Enabled = true;
            ViewState.Add("AccionBoton", "MODIFICAR");
        }

        /*[DECLARAMOS UNA TABLA QUE VA ALMACENAR EL DETALLE DEL REGISTRO]*/
        DataTable DtRegistros = new DataTable();


        /*[EVENTO QUE SE VA ENCARGAR DE GRABAR O ACTUALIZAR EL REGISTRO]*/
        void ctrlToolBarMantenimiento_btnGrabarHandler()
        {
            if (Validar())
            {

                if (ctrlOficinaConsular1.SelectedIndex == 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "Ingrese una oficina consular" + "');", true);
                    return;
                }

                object[] parObjetos = new object[1];
                TicketeraMantenimientoBL ObjBL = new TicketeraMantenimientoBL();
                BE.MRE.CL_SERVICIO ObjTicteraBE = new BE.MRE.CL_SERVICIO();
                ServicioMantenimientoBL BL = new ServicioMantenimientoBL();
                long IntRpta = 0;

                ObjTicteraBE.serv_sOficinaConsularId = Convert.ToInt16(ctrlOficinaConsular1.SelectedValue);

                if ((txtDesc.Text).ToString() != "")
                {
                    ObjTicteraBE.serv_vDescripcion = txtDesc.Text;
                }

                if (txtDesc.Text == "")
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_TAB_REGISTRAR);
                }

                ObjTicteraBE.serv_IOrden = Convert.ToInt32(txtNumOrd.Text);
                ObjTicteraBE.serv_cEstado = "A";
                //--------------------------------------------
                //Fecha: 23/02/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Se adiciona la columna: serv_vServicioDireccion
                //--------------------------------------------
                ObjTicteraBE.serv_vServicioDireccion = "";
                if ((txtDireccionServicio.Text).ToString() != "")
                {
                    ObjTicteraBE.serv_vServicioDireccion = txtDireccionServicio.Text;
                }
                ObjTicteraBE.serv_sTipo = 1;
                parObjetos[0] = ObjTicteraBE;

                DataTable dtDetalle = new DataTable();
                dtDetalle = Llenar_detalle();

                //-----------------------------------------------------
                //Fecha: 08/02/2021
                //Autor: Miguel Márquez Beltrán.
                //Motivo: Se valida si es numérico. 
                //-----------------------------------------------------

                int intNroOrden = Convert.ToInt32(txtNumOrd.Text.Trim());
                int intValor = 0;

                if (ViewState["AccionBoton"].ToString() == "NUEVO")
                {

                    foreach (GridViewRow row in grdVentanilla.Rows)
                    {
                        if (IsInteger(row.Cells[2].Text))
                        {
                            intValor = Convert.ToInt32(row.Cells[2].Text);

                            if (intValor == intNroOrden)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "El número de orden ya existe" + "');", true);
                                return;
                            }
                        }
                    }

                    ObjTicteraBE.serv_dFechaCreacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                    ObjTicteraBE.serv_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                    ObjTicteraBE.serv_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    ObjTicteraBE.serv_sServicioIdCab = 0;

                    Object[] miArrayServicioCons = new Object[2] {
                                                          ObjTicteraBE,                                                          
                                                          dtDetalle
                                                         };

                    IntRpta = BL.InsertarDet(ObjTicteraBE, dtDetalle);
                }
                else
                {
                    intNroOrden = Convert.ToInt32(txtNumOrd.Text.Trim());
                    intValor = 0;

                    foreach (GridViewRow row in grdVentanilla.Rows)
                    {
                        if (Convert.ToInt32(row.Cells[0].Text) != Convert.ToInt32(Session["IdServConsular"]))
                        {
                            if (IsInteger(row.Cells[2].Text))
                            {
                                intValor = Convert.ToInt32(row.Cells[2].Text);

                                if (intValor == intNroOrden)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "El número de orden ya existe" + "');", true);
                                    return;
                                }
                            }
                        }
                    }


                    ObjTicteraBE.serv_sServicioIdCab = Convert.ToInt16(TxtId.Text);

                    ObjTicteraBE.serv_sServicioId = Convert.ToInt16(TxtId.Text);
                    ObjTicteraBE.serv_dFechaModificacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                    ObjTicteraBE.serv_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                    ObjTicteraBE.serv_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                    ObjTicteraBE.serv_dFechaCreacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                    ObjTicteraBE.serv_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                    ObjTicteraBE.serv_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                    Object[] miArrayServicioCons = new Object[2] {ObjTicteraBE, dtDetalle};

                    IntRpta = BL.Update(ObjTicteraBE, dtDetalle);
                }

                string strScript = string.Empty;
                if (p.IErrorNumero == 0)
                {
                    if (IntRpta ==(int)Enumerador.enmResultadoOperacion.OK)
                    {
                        EstadoTool(true, true, false, false, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/

                        CargarGrilla();
                        limpiar();

                        txtDesc.Enabled = false;
                        txtNumOrd.Enabled = false;
                        //--------------------------------------------
                        //Fecha: 23/02/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Se adiciona la columna: serv_vServicioDireccion
                        //--------------------------------------------
                        txtDireccionServicio.Enabled = false;
                        btnsubservicios.Enabled = false;
                        ctrlOficinaConsular1.Enabled = false;
                        GridDetalle.DataSource = null;
                        GridDetalle.DataBind();

                        strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                        strScript += Util.HabilitarTab(0);
                    }
                    else
                    {
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
                    }
                }
                else
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, p.vErrorMensaje);
                }

                Comun.EjecutarScript(Page, strScript);
            }
        }

        void ctrlToolBarMantenimiento_btnEliminarHandler()
        {
            BE.MRE.CL_SERVICIO ObjTicteraBE = new BE.MRE.CL_SERVICIO();
            ServicioMantenimientoBL BL = new ServicioMantenimientoBL();
            int IntRpta = 0;
            try
            {
                Object[] miArrayServicioCons = new Object[1] { ObjTicteraBE };

                ObjTicteraBE.serv_sServicioId = Convert.ToInt16(Session["IdServConsular"]);
                ObjTicteraBE.serv_sServicioIdCab = 0;
                ObjTicteraBE.serv_vDescripcion = "";
                ObjTicteraBE.serv_IOrden = 0;
                ObjTicteraBE.serv_cEstado = "";
                ObjTicteraBE.serv_sTipo = 0;
                //--------------------------------------------
                //Fecha: 23/02/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Se adiciona la columna: serv_vServicioDireccion
                //--------------------------------------------
                ObjTicteraBE.serv_vServicioDireccion = "";

                ObjTicteraBE.serv_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
                ObjTicteraBE.serv_dFechaModificacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                ObjTicteraBE.serv_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                ObjTicteraBE.serv_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                ObjTicteraBE.serv_dFechaCreacion = Comun.FormatearFecha(DateTime.Now.ToLongTimeString());
                ObjTicteraBE.serv_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID].ToString());
                ObjTicteraBE.serv_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                IntRpta = BL.Delete(ObjTicteraBE);

                string strScript = string.Empty;
                if (p.IErrorNumero == 0)
                {
                    if (IntRpta == (int)Enumerador.enmResultadoOperacion.OK)
                    {
                        ctrlToolBarMantenimiento_btnCancelarHandler();

                        strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                        strScript += Util.HabilitarTab(0);
                    }
                    else
                    {
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
                    }
                }
                else
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, p.vErrorMensaje);
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

        /*[FUNCIONES]*/

        /*[FUNCION QUE PERMITE PREPARAR EL FORMULARIO PARA GENERAR UN NUEVO REGISTRO]*/
        private void RegistrarEntidad()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;

            /*[DESACTIVAMOS LA PROPIEDAD READONLY DE LAS CAJAS DEL FORMULARIO]*/
            InputTool(false);

            /*[PREPARAMOS LOS BOTONES DEL MANTENIMIENTO]*/
            EstadoTool(true, true, false, true, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/


            /*[LIMPIAMOS LOS INPUTS DEL FORMULARIO]*/
            LimpiarDatosMantenimiento();

            /*[HABILITAMOS EL BOTON PARA PODER GRABAR SUB-SERVICIOS]*/
            this.btngrabarSubServicio.Enabled = true;
            this.btnsubservicios.Enabled = true;

            /*CargarGrillaDetalle();*/
            Session["IQueHace"] = 1;
        }

        private void HabilitarMantenimientoSub(bool bolHabilitar = true)
        {
            this.txtDescSub.Enabled = bolHabilitar;
            this.txtNumOrdSub.Enabled = bolHabilitar;
            //--------------------------------------------
            //Fecha: 23/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se adiciona la columna: serv_vServicioDireccion
            //--------------------------------------------
            this.txtDireccionSubServicio.Enabled = bolHabilitar;
        }

        /*[FUNCION QUE LIMPIA LA CAJAS DE TEXTO DEL MANTENIMIENTO DE CABECERA SERVICIO]*/
        private void LimpiarDatosMantenimiento()
        {
            txtDesc.Text = "";
            txtNumOrd.Text = "";
            txtDireccionServicio.Text = "";

            Session["DtRegistrosSubServicios"] = Cabecera_detalle();

            GridDetalle.DataSource = Cabecera_detalle();
            GridDetalle.DataBind();
        }

        private void LimpiarDatosMantenimientosub()
        {
            this.txtDescSub.Text = "";
            this.txtNumOrdSub.Text = "";
            //--------------------------------------------
            //Fecha: 23/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se adiciona la columna: serv_vServicioDireccion
            //--------------------------------------------
            this.txtDireccionSubServicio.Text = "";
        }

        private void ModificarEntidad()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;
            ctrlToolBarMantenimiento.btnNuevo.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnCancelar.Enabled = true;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = true;
            this.btnsubservicios.Enabled = true;

            InputTool(false);

            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_EDITAR));
        }

        private void EliminarEntidad()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.ELIMINAR;
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_ELIMINAR));
        }

        void ActivarControles(bool valor)
        {
            this.txtDesc.ReadOnly = valor;
            this.txtNumOrd.ReadOnly = valor;
            //--------------------------------------------
            //Fecha: 23/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se adiciona la columna: serv_vServicioDireccion
            //--------------------------------------------
            this.txtDireccionServicio.ReadOnly = valor;
        }

        private bool Validar()
        {
            bool bolValidado = true;

            if (this.txtDesc.Text == string.Empty)
            {
                bolValidado = false;
            }

            if ((this.txtNumOrd.Text) == "")
            {
                txtNumOrd.Text = "0";
                bolValidado = false;
            }

            if ((Convert.ToInt32(this.txtNumOrd.Text) == 0))
            {
                bolValidado = false;
                this.txtNumOrd.Text = "";
            }

            return bolValidado;

        }

        protected void grdVentanilla_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);

            Session["IdServConsular"] = grdVentanilla.Rows[index].Cells[0].Text;
            string strScript = string.Empty;

            try
            {
                //----------------------------------------------------------
                //Fecha: 22/04/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Se convierte codigo HTML en cadena decodificada.
                //----------------------------------------------------------

                ViewState.Add("sIdOfConsular", grdVentanilla.DataKeys[index].Values["serv_sOficinaConsularId"].ToString());
                ctrlOficinaConsular1.SelectedValue = grdVentanilla.DataKeys[index].Values["serv_sOficinaConsularId"].ToString();
                this.TxtId.Text = HttpUtility.HtmlDecode(grdVentanilla.Rows[index].Cells[0].Text).Trim();
                this.txtDesc.Text = HttpUtility.HtmlDecode(grdVentanilla.Rows[index].Cells[1].Text).Trim();
                this.txtNumOrd.Text = HttpUtility.HtmlDecode(grdVentanilla.Rows[index].Cells[2].Text).Trim();
                //--------------------------------------------
                //Fecha: 23/02/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Se adiciona la columna: serv_vServicioDireccion
                //--------------------------------------------
                this.txtDireccionServicio.Text = HttpUtility.HtmlDecode(grdVentanilla.Rows[index].Cells[4].Text).Trim();
                CargarGrillaDetalle();

                /*[PREGUNTAMOS POR EL BOTON QUE SE ESTA PRECIONANDO]*/
                if (e.CommandName == "Editar")
                {
                    Session["IQueHace"] = 1;
                    InputTool(false);
                    txtDesc.Focus();
                    EstadoTool(true, true, false, true, true, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
                    ctrlOficinaConsular1.Enabled = true;
                    ViewState.Add("AccionBoton", "MODIFICAR");
                }
                else if (e.CommandName == "Consultar")
                {
                    Session["IQueHace"] = 2;
                    InputTool(true);
                    EstadoTool(true, true, true, false, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
                    this.txtDesc.Enabled = false;
                    this.txtNumOrd.Enabled = false;
                    //--------------------------------------------
                    //Fecha: 23/02/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Se adiciona la columna: serv_vServicioDireccion
                    //--------------------------------------------
                    this.txtDireccionServicio.Enabled = false;
                    this.btngrabarSubServicio.Enabled = false;
                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
                    ctrlOficinaConsular1.Enabled = false;
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

        protected void ctrlPaginadorDetalle_Click(object sender, EventArgs e)
        {
            CargarGrillaDetalle();
        }

        protected void cboOficinaCons1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /*[FUNCION QUE RETORNA LOS DATOS DE TODOS LOS SERVICIOS]*/
        private void CargarGrilla()
        {
            try
            {
                int intTotalRegistros = 0, intTotalPaginas = 0;

                /*[ASIGNAMOS A UN AREGLO LOS PARAMETROS QUE VAMOS A UTILIZAR PARA LLENAR EL DATATABLE]*/
                //object[] arrParametros ={ Convert.ToInt32(this.ctrlOficinaConsular.SelectedValue),
                //                          ctrlPaginador.PaginaActual,
                //                          Constantes.CONST_CANT_REGISTRO,
                //                          intTotalRegistros,
                //                          intTotalPaginas
                //                        };
                //DataTable dt = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.CL_SERVICIO", Enumerador.enmAccion.CONSULTAR);

                ServicioConsultaBL objServicioConsultaBL = new ServicioConsultaBL();

                DataTable dt = new DataTable();

                dt = objServicioConsultaBL.Consultar(Convert.ToInt32(this.ctrlOficinaConsular.SelectedValue), ctrlPaginador.PaginaActual, Constantes.CONST_CANT_REGISTRO, ref intTotalRegistros, ref intTotalPaginas);


                //if (p.IErrorNumero == 0)
                //{
                    Session.Add("dt", dt);
                    grdVentanilla.SelectedIndex = -1;
                    grdVentanilla.DataSource = dt;
                    grdVentanilla.DataBind();

                    ctrlValidacion.MostrarValidacion(null);

                    /*[PAGINADOR]*/
                    //ctrlPaginador.TotalResgistros = Convert.ToInt32(arrParametros[3]);
                    ctrlPaginador.TotalResgistros = intTotalRegistros;
                    //ctrlPaginador.TotalPaginas = Convert.ToInt32(arrParametros[4]);
                    ctrlPaginador.TotalPaginas = intTotalPaginas;

                    ctrlPaginador.Visible = false;
                    if (ctrlPaginador.TotalPaginas > 1)
                        ctrlPaginador.Visible = true;

                    ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + ctrlPaginador.TotalResgistros, true, Enumerador.enmTipoMensaje.INFORMATION);
                    if (dt.Rows.Count < 1)
                    {
                        ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                    }
               // }

                UpdatePanel1.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CargarGrillaDetalle()
        {
            try
            {
                if (this.TxtId.Text != "")
                {
                    int intTotalRegistros = 0;
                    int intTotalPaginas = 0;

                    object[] arrParametros ={ Convert.ToInt32(this.ctrlOficinaConsular.SelectedValue),
                                              ctrlPaginadorDetalle.PaginaActual,
                                              Constantes.CONST_CANT_REGISTRO, 
                                              Convert.ToInt32(this.TxtId.Text),
                                              intTotalRegistros,
                                              intTotalPaginas
                                            };

                    DataTable dtDetalle = new DataTable();
                    ServicioConsultaBL ServicioConsultaBL = new ServicioConsultaBL();
                    dtDetalle = ServicioConsultaBL.ConsultarDetalle((Convert.ToInt32(this.ctrlOficinaConsular.SelectedValue)),
                                                                    ctrlPaginadorDetalle.PaginaActual,
                                                                    Constantes.CONST_CANT_REGISTRO,
                                                                    Convert.ToInt32(this.TxtId.Text),
                                                                    intTotalRegistros,
                                                                    intTotalPaginas);




                    if (p.IErrorNumero == 0)
                    {
                        Session["DtRegistrosSubServicios"] = dtDetalle;
                        this.GridDetalle.SelectedIndex = -1;
                        GridDetalle.DataSource = dtDetalle;
                        GridDetalle.DataBind();

                        if (GridDetalle.Rows.Count == 0)
                        {
                            ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                        }
                        ctrlPaginadorDetalle.TotalResgistros = Convert.ToInt32(arrParametros[4]);
                        ctrlPaginadorDetalle.TotalPaginas = Convert.ToInt32(arrParametros[5]);

                        ctrlPaginadorDetalle.Visible = false;
                        if (ctrlPaginadorDetalle.TotalPaginas > 1)
                        {
                            ctrlPaginadorDetalle.Visible = true;
                        }
                    }

                    if (GridDetalle.Rows.Count == 0)
                    {
                        ctrlValidacion.MostrarValidacion("No se encontro " + "registros" + ".");
                    }

                    if (this.txtDesc.Text == "")
                    {
                        ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                    }
                }
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

        /*[FUNCION QUE HABILITA O DESHABILITA LAS CAJAS DE TEXTO]*/
        void InputTool(bool estado)
        {
            this.txtDesc.ReadOnly = estado;
            this.txtNumOrd.ReadOnly = estado;
            this.txtDescSub.ReadOnly = estado;
            this.txtNumOrdSub.ReadOnly = estado;
            //--------------------------------------------
            //Fecha: 23/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se adiciona la columna: serv_vServicioDireccion
            //--------------------------------------------
            this.txtDireccionServicio.ReadOnly = estado;

            this.txtDesc.Enabled = !estado;
            this.txtNumOrd.Enabled = !estado;
            this.txtDescSub.Enabled = !estado;
            this.txtNumOrdSub.Enabled = !estado;
            //--------------------------------------------
            //Fecha: 23/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se adiciona la columna: serv_vServicioDireccion
            //--------------------------------------------
            this.txtDireccionServicio.Enabled = !estado;
            this.btnsubservicios.Enabled = !estado;
        }

        void ActivarToolSubServicios()
        {
            this.txtDescSub.Enabled = true;
            this.TxtIdDesc.Enabled = true;
            this.txtNumOrdSub.Enabled = true;
            this.txtDireccionSubServicio.Enabled = true;

            this.txtDescSub.ReadOnly = false;
            this.TxtIdDesc.ReadOnly = false;
            this.txtNumOrdSub.ReadOnly = false;
            //--------------------------------------------
            //Fecha: 23/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se adiciona la columna: serv_vServicioDireccion
            //--------------------------------------------
            this.txtDireccionSubServicio.ReadOnly = false;

            this.TxtIdDesc.Text = string.Empty;
            this.txtDescSub.Text = string.Empty;
            this.txtNumOrdSub.Text = string.Empty;
            //--------------------------------------------
            //Fecha: 23/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se adiciona la columna: serv_vServicioDireccion
            //--------------------------------------------
            this.txtDireccionSubServicio.Text = string.Empty;

            this.btngrabarSubServicio.Enabled = true;
            this.btnModificarSub.Enabled = false;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnCancelar.Enabled = true;
            this.txtDescSub.Focus();
        }

        void limpiar()
        {
            this.TxtId.Text = "";
            this.txtDesc.Text = "";
            this.txtNumOrd.Text = "";
            //--------------------------------------------
            //Fecha: 23/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se adiciona la columna: serv_vServicioDireccion
            //--------------------------------------------
            this.txtDireccionServicio.Text = "";
            ctrlOficinaConsular1.Enabled = false;
        }

        private void CargarDatosIniciales()
        {
            Session.Add(strVariableAccion, Enumerador.enmAccion.INSERTAR);
            Session.Add(strVariableIndice, -1);
            Session.Add(strVariableDt, new DataTable());

            // Registro
            ctrlToolBarMantenimiento.btnConfiguration.Enabled = false;

            this.txtDesc.Text = "";
            this.txtNumOrd.Text = "";
            this.txtDireccionServicio.Text = "";
            this.txtDescSub.Text = "";
            this.txtNumOrdSub.Text = "";
            //--------------------------------------------
            //Fecha: 23/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se adiciona la columna: serv_vServicioDireccion
            //--------------------------------------------
            this.txtDireccionSubServicio.Text = "";

            Comun.EjecutarScript(Page, Util.NombrarTab(0, "Consulta"));
        }

        protected void SubServicio_Click(object sender, EventArgs e)
        {
            ModalPanel_Cab.Show();
        }

        protected void GridDetalle_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            ViewState.Add("indexDetalle", index);
            Session["IdSubServicio"] = GridDetalle.Rows[index].Cells[0].Text;
            this.TxtIdDesc.Text = ((string)(Session["IdSubServicio"]));

            try
            {
                Comun.EjecutarScript(Page, Util.HabilitarTab(1));

                if (ctrlOficinaConsular1.Enabled == false)
                {
                    EstadoTool(true, true, true, false, false, true);/*[BUSCAR, NUEVO, EDITAR, GRABAR, ELIMINAR, CANCELAR]*/
                    return;
                }

                if (e.CommandName == "Editar")
                {
                    if (ctrlOficinaConsular1.Enabled == true)
                    {
                        ctrlToolBarMantenimiento.btnCancelar.Enabled = true;

                    }

                    Session["IQueHaceSubservicios"] = 2;
                    //----------------------------------------------------------
                    //Fecha: 22/04/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Se convierte codigo HTML en cadena decodificada.
                    //----------------------------------------------------------

                    this.TxtIdDesc.Text = HttpUtility.HtmlDecode(GridDetalle.Rows[index].Cells[1].Text).Trim();
                    this.txtDescSub.Text = HttpUtility.HtmlDecode(GridDetalle.Rows[index].Cells[3].Text).Trim();
                    this.txtNumOrdSub.Text = HttpUtility.HtmlDecode(GridDetalle.Rows[index].Cells[4].Text).Trim();
                    this.txtDireccionSubServicio.Text = HttpUtility.HtmlDecode(GridDetalle.Rows[index].Cells[7].Text).Trim();
                    ViewState.Add("IdSubServicio", GridDetalle.Rows[index].Cells[1].Text);

                    this.btngrabarSubServicio.Enabled = false;
                    this.btnModificarSub.Enabled = true;
                    this.btnCancelarSub.Enabled = true;

                    this.txtDescSub.ReadOnly = false;
                    this.txtNumOrdSub.ReadOnly = false;
                    this.txtDireccionSubServicio.ReadOnly = false;
                    this.txtDescSub.Enabled = true;
                    this.txtNumOrdSub.Enabled = true;
                    //--------------------------------------------
                    //Fecha: 23/02/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Se adiciona la columna: serv_vServicioDireccion
                    //--------------------------------------------
                    this.txtDireccionSubServicio.Enabled = true;

                    this.txtDescSub.Focus();

                    ModalPanel_Cab.Show();
                }

                if (e.CommandName == "Eliminar")
                {

                    DataTable dt = (DataTable)Session["DtRegistrosSubServicios"];
                    dt.Rows[index].Delete();
                    dt.AcceptChanges();

                    if (GridDetalle.Rows.Count == 1)
                    {
                        Session["DtRegistrosSubServicios"] = Cabecera_detalle();
                    }
                    else
                    {
                        Session["DtRegistrosSubServicios"] = dt;
                    }

                    GridDetalle.DataSource = dt;
                    GridDetalle.DataBind();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnsubservicios_Click(object sender, EventArgs e)
        {
            ActivarToolSubServicios();
            ModalPanel_Cab.Show();
        }

        protected void btngrabarSubServicio_Click(object sender, EventArgs e)
        {
            if (this.txtDescSub.Text == "")
            {
                ModalPanel_Cab.Show();
                return;
            }

            if (txtNumOrdSub.Text.Trim() == string.Empty)
            {
                ModalPanel_Cab.Show();
                return;
            }
            if (Convert.ToInt32(txtNumOrdSub.Text) == 0)
            {
                ModalPanel_Cab.Show();
                return;
            }

            DataTable dtDetalleServicio = new DataTable();
            dtDetalleServicio = (DataTable)Session["DtRegistrosSubServicios"];

            int intNroOrdenSub = Convert.ToInt32(txtNumOrdSub.Text.Trim());
            int intValorSub = 0;

            foreach (DataRow row in dtDetalleServicio.Rows)
            {
                if (IsInteger(row["serv_IOrden"].ToString()))
                {
                    intValorSub = Convert.ToInt32(row["serv_IOrden"].ToString().Trim());

                    if (intValorSub == intNroOrdenSub)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "Numero de Orden Existente" + "');", true);
                        ModalPanel_Cab.Show();
                        txtNumOrdSub.Focus();
                        return;
                    }
                }
            }

            DataRow newrow = dtDetalleServicio.NewRow();
            newrow["serv_sOficinaConsularId"] = Convert.ToInt32(ctrlOficinaConsular1.SelectedValue);
            newrow["serv_sServicioId"] = 0;
            newrow["serv_sServicioIdCab"] = TxtId.Text.Trim() == "" ? 0 : Convert.ToInt32(TxtId.Text.Trim());
            newrow["serv_vDescripcion"] = Page.Server.HtmlDecode(this.txtDescSub.Text.Trim()).Trim();
            newrow["serv_IOrden"] = Convert.ToInt32(this.txtNumOrdSub.Text);
            newrow["serv_cEstado"] = "A";
            newrow["serv_sTipo"] = 2;
            //--------------------------------------------
            //Fecha: 23/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se adiciona la columna: serv_vServicioDireccion
            //--------------------------------------------
            newrow["serv_vServicioDireccion"] = Page.Server.HtmlDecode(this.txtDireccionSubServicio.Text.Trim()).Trim();

            dtDetalleServicio.Rows.Add(newrow);

            Session["DtRegistrosSubServicios"] = dtDetalleServicio;

            this.txtDescSub.Text = string.Empty;
            this.txtNumOrdSub.Text = string.Empty;
            this.txtDireccionSubServicio.Text = string.Empty;
            this.txtDescSub.Focus();

            this.GridDetalle.DataSource = dtDetalleServicio;
            this.GridDetalle.DataBind();
            ctrlToolBarMantenimiento.btnCancelar.Enabled = true;

            ModalPanel_Cab.Show();
        }

        private DataTable CrearDTRegistrosSubServicios()
        {
            DataTable DtRegistrosSubServicios = new DataTable();
            DtRegistrosSubServicios.Columns.Clear();

            DataColumn workCol1 = DtRegistrosSubServicios.Columns.Add("serv_sOficinaConsularId", typeof(Int32));
            workCol1.AllowDBNull = true;
            workCol1.Unique = false;

            DataColumn workCol2 = DtRegistrosSubServicios.Columns.Add("serv_sServicioId", typeof(Int32));
            workCol2.AllowDBNull = true;
            workCol2.Unique = false;

            DataColumn workCo3 = DtRegistrosSubServicios.Columns.Add("serv_sServicioIdCab", typeof(Int32));
            workCo3.AllowDBNull = true;
            workCo3.Unique = false;

            DataColumn workCo4 = DtRegistrosSubServicios.Columns.Add("serv_vDescripcion", typeof(String));
            workCo4.AllowDBNull = true;
            workCo4.Unique = false;

            DataColumn workCo5 = DtRegistrosSubServicios.Columns.Add("serv_IOrden", typeof(Int32));
            workCo5.AllowDBNull = true;
            workCo5.Unique = false;

            DataColumn workCo6 = DtRegistrosSubServicios.Columns.Add("serv_cEstado", typeof(String));
            workCo6.AllowDBNull = true;
            workCo6.Unique = false;

            DataColumn workCo7 = DtRegistrosSubServicios.Columns.Add("serv_sTipo", typeof(Int32));
            workCo7.AllowDBNull = true;
            workCo7.Unique = false;

            //--------------------------------------------
            //Fecha: 23/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se adiciona la columna: serv_vServicioDireccion
            //--------------------------------------------
            DataColumn workCo8 = DtRegistrosSubServicios.Columns.Add("serv_vServicioDireccion", typeof(String));
            workCo8.AllowDBNull = true;
            workCo8.Unique = false;

            return DtRegistrosSubServicios;
        }

        protected void grdVentanilla_SelectedIndexChanged(object sender, EventArgs e)
        {
            Comun.EjecutarScript(Page, Util.HabilitarTab(1));
        }

        protected void btnCancelarSub_Click(object sender, EventArgs e)
        {
            limpiarpanelsubservicios();
            ctrlToolBarMantenimiento.btnCancelar.Enabled = true;
        }

        private void limpiarpanelsubservicios()
        {
            //--------------------------------------------
            //Fecha: 23/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se adiciona la columna: serv_vServicioDireccion
            //--------------------------------------------
            this.txtNumOrdSub.Text = string.Empty;
            this.txtDescSub.Text = string.Empty;
            this.TxtIdDesc.Text = string.Empty;
            this.txtDireccionSubServicio.Text = string.Empty;
            this.btngrabarSubServicio.Enabled = false;
            this.btnModificarSub.Enabled = false;
        }

        protected void btnModificarSub_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(ViewState["indexDetalle"]);

            DataTable dt = new DataTable();
            dt = (DataTable)Session["DtRegistrosSubServicios"];

            int n = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (index != n)
                {
                    if (row["serv_IOrden"].ToString() == txtNumOrdSub.Text.Trim())
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('" + "Numero de Orden Existente" + "');", true);
                        ModalPanel_Cab.Show();
                        txtNumOrdSub.Focus();
                        return;
                    }
                }
                n++;
            }

            dt.Rows[index]["serv_sOficinaConsularId"] = Convert.ToInt32(ctrlOficinaConsular1.SelectedValue);
            dt.Rows[index]["serv_sServicioId"] = Convert.ToInt16(ViewState["IdSubServicio"]);
            dt.Rows[index]["serv_sServicioIdCab"] = TxtId.Text.Trim() == "" ? 0 : Convert.ToInt32(TxtId.Text.Trim());
            dt.Rows[index]["serv_vDescripcion"] = Page.Server.HtmlDecode(this.txtDescSub.Text.Trim()).Trim();
            dt.Rows[index]["serv_IOrden"] = Convert.ToInt32(this.txtNumOrdSub.Text);
            dt.Rows[index]["serv_cEstado"] = "A";
            dt.Rows[index]["serv_sTipo"] = 2;
            //--------------------------------------------
            //Fecha: 23/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se adiciona la columna: serv_vServicioDireccion
            //--------------------------------------------
            dt.Rows[index]["serv_vServicioDireccion"] = Page.Server.HtmlDecode(this.txtDireccionSubServicio.Text.Trim()).Trim();

            dt.AcceptChanges();

            Session["DtRegistrosSubServicios"] = dt;

            this.GridDetalle.DataSource = dt;
            this.GridDetalle.DataBind();

            ctrlToolBarMantenimiento.btnCancelar.Enabled = true;
        }

        private DataTable Llenar_detalle()
        {
            DataTable dt = new DataTable();

            dt = Cabecera_detalle();

            foreach (GridViewRow row in GridDetalle.Rows)
            {
                DataRow newro = dt.NewRow();
                newro["serv_sOficinaConsularId"] = Convert.ToInt16(row.Cells[0].Text.Trim());
                newro["serv_sServicioId"] = Convert.ToInt16(row.Cells[1].Text.ToString());
                newro["serv_sServicioIdCab"] = Convert.ToInt16(row.Cells[2].Text.ToString());
                newro["serv_vDescripcion"] = Page.Server.HtmlDecode(row.Cells[3].Text.ToString()).Trim();
                newro["serv_IOrden"] = Convert.ToInt16(row.Cells[4].Text.ToString());
                newro["serv_cEstado"] = row.Cells[5].Text.ToString();
                newro["serv_sTipo"] = 2;
                //--------------------------------------------
                //Fecha: 23/02/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Se adiciona la columna: serv_vServicioDireccion
                //--------------------------------------------
                newro["serv_vServicioDireccion"] = Page.Server.HtmlDecode(row.Cells[7].Text.ToString()).Trim();

                dt.Rows.Add(newro);
            }

            return dt;

        }

        private DataTable Cabecera_detalle()
        {
            DataTable dt = new DataTable("detalle");

            dt.Columns.Add("serv_sOficinaConsularId", typeof(Int32));
            dt.Columns.Add("serv_sServicioId", typeof(Int32));
            dt.Columns.Add("serv_sServicioIdCab", typeof(Int32));

            dt.Columns.Add("serv_vDescripcion", typeof(string));
            dt.Columns.Add("serv_IOrden", typeof(Int32));
            dt.Columns.Add("serv_cEstado", typeof(string));
            dt.Columns.Add("serv_sTipo", typeof(Int32));
            //--------------------------------------------
            //Fecha: 23/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Se adiciona la columna: serv_vServicioDireccion
            //--------------------------------------------
            dt.Columns.Add("serv_vServicioDireccion", typeof(string));

            return dt;

        }

        protected void imgCerrar_Click(object sender, ImageClickEventArgs e)
        {
            ModalPanel_Cab.Hide();
            ctrlToolBarMantenimiento.btnCancelar.Enabled = true;
        }
        public static bool IsInteger(string theValue)
        {
            try
            {
                Convert.ToInt32(theValue);
                return true;
            }
            catch
            {
                return false;
            }
        } //IsInteger
    }
}
