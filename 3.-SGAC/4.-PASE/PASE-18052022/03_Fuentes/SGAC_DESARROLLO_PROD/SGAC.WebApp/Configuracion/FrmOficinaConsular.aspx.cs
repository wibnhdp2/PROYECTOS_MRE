using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.Accesorios;
using System.Data;
using SGAC.Controlador;
using SGAC.Configuracion.Sistema.BL;
using SGAC.BE.MRE;
using System.Web.Script.Serialization;

namespace SGAC.WebApp.Configuracion
{
    public partial class OficinaConsular : MyBasePage
    {
        #region CAMPOS
        private string strNombreEntidad = "OFICINA CONSULAR";
        private string strVariableAccion = "OficinaConsular_Accion";
        private string strVariableDt = "OficinaConsular_Tabla";
        private string strVariableIndice = "OficinaConsular_Indice";

        private string strVariableAccionOFCH = "OficinaConsularHonoraria_Accion";
        private string strVariableDtOFCH = "OficinaConsularHonoraria_Tabla";
        private string strVariableIndiceOFCH = "OficinaConsularHonoraria_Indice";

        private string strVariableAccionFunc = "Funcionario_Accion";
        private string strVariableDtFunc = "Funcionario_Tabla";
        private string strVariableDtFunc_tmp = "Funcionario_Tabla_tmp";
        private string strVariableIndiceFunc = "Funcionario_Indice";

        private string strIdFuncionario = "Funcionario_Indice";
        private string strNombFuncionario = "ocfu_vNombreFuncionario"; //"Funcionario_Nombre";
        #endregion

        #region Eventos
        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;

            ctrlPaginadorDep.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginadorDep.Visible = false;
            ctrlPaginadorDep.PaginaActual = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblUserName.Text = Util.GetSessionVariableValue(Session, Constantes.CONST_SESION_USUARIO);
            ctrFechaInicio.AllowFutureDate = true;
            ctrFechaFin.AllowFutureDate = true;
            ctrlToolBarConsulta.VisibleIButtonBuscar = true;
            ctrlToolBarConsulta.VisibleIButtonCancelar = true;

            ctrlToolBarConsulta.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);
            ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);

            ctrlToolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarConsulta.btnCancelar.Text = "    Limpiar";

            ctrlToolBarMantenimiento.VisibleIButtonNuevo = true;
            ctrlToolBarMantenimiento.VisibleIButtonEditar = true;
            ctrlToolBarMantenimiento.VisibleIButtonEliminar = true;
            ctrlToolBarMantenimiento.VisibleIButtonGrabar = true;
            ctrlToolBarMantenimiento.VisibleIButtonCancelar = true;

            ctrlToolBarMantenimiento.btnNuevoHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonNuevoClick(ctrlToolBarMantenimiento_btnNuevoHandler);
            ctrlToolBarMantenimiento.btnEditarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEditarClick(ctrlToolBarMantenimiento_btnEditarHandler);
            ctrlToolBarMantenimiento.btnEliminarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEliminarClick(ctrlToolBarMantenimiento_btnEliminarHandler);
            ctrlToolBarMantenimiento.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBarMantenimiento_btnGrabarHandler);
            ctrlToolBarMantenimiento.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarMantenimiento_btnCancelarHandler);

            ctrlToolBarMantenimiento.btnGrabar.OnClientClick = "return Validar();";

            string eventTarget = Request["__EVENTTARGET"] ?? string.Empty;
            if (eventTarget == "GrabarHandler")
            {
                if (Session["Grabo"].ToString().Equals("NO"))
                    GrabarHandler();
            }

            if (!Page.IsPostBack)
            {
                string strFechaTexto = Comun.ObtenerFechaActualTexto(Session);

                ctrFechaFin.Text = strFechaTexto;
                ctrFechaInicio.Text = strFechaTexto;

                Session["Grabo"] = "NO";
                CargarListadosDesplegables();
                CargarDatosIniciales();

                Session["iOperFuncionario"] = true;
                Session[strVariableDtFunc_tmp] = new DataTable();

                Util.CargarParametroDropDownList(ddlCargos, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.CARGO_FUNCIONARIO_LISTA), true);

                string script = "DisableBtnNuevaMoneda();";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", script, true);
            }

            if (Session["bCajaChicaSaldoSatisfactorio"] != null)
            {
                if (Convert.ToBoolean(Session["bCajaChicaSaldoSatisfactorio"].ToString()))
                {
                    string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Oficina Consular", "Saldo agregado satisfactoriamente.", false, 190, 250);
                    Comun.EjecutarScript(Page, strScript);
                    Session["bCajaChicaSaldoSatisfactorio"] = null;
                }
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo, ctrlToolBarMantenimiento.btnEditar, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar };
                GridView[] arrGridView = { gdvOficinaConsular, gdvDependientes, gdvFuncionario};
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }
        }

        protected void ddlDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlProvincia.Items.Clear();
            
            if (ddlDepartamento.SelectedIndex > 0)
            {
                
                //comun_Part3.CargarUbigeo(Session, ddlProvincia, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddlDepartamento.SelectedValue, "", true);
                if (ViewState["Ubigeo"] != null)
                {
                    beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                    obeUbigeoListas = (beUbigeoListas)ViewState["Ubigeo"];
                    List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                    lbeUbicaciongeografica = Comun.obtenerListaUbiGeo("02", ddlDepartamento.SelectedValue, "", obeUbigeoListas.Ubigeo02);
                    lbeUbicaciongeografica.Insert(0, new beUbicaciongeografica { Ubi02 = "00", Provincia = "-- SELECCIONE --" });

                    ddlProvincia.DataSource = lbeUbicaciongeografica;
                    ddlProvincia.DataValueField = "Ubi02";
                    ddlProvincia.DataTextField = "Provincia";
                    ddlProvincia.DataBind();
                    ddlProvincia.Enabled = true;
                }

                ddlDepartamento.Focus();
            }
            else
            {
                ddlProvincia.SelectedIndex = -1;
                ddlProvincia.Enabled = false;

                ddlDistrito.SelectedIndex = -1;
                ddlDistrito.Enabled = false;
            }
            updMantenimiento.Update();
        }

        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProvincia.SelectedIndex > 0)
            {                

                //comun_Part3.CargarUbigeo(Session, ddlDistrito, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddlDepartamento.SelectedValue, ddlProvincia.SelectedValue, true);

                if (ViewState["Ubigeo"] != null)
                {
                    beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                    obeUbigeoListas = (beUbigeoListas)ViewState["Ubigeo"];
                    List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                    lbeUbicaciongeografica = Comun.obtenerListaUbiGeo("03", ddlDepartamento.SelectedValue, ddlProvincia.SelectedValue, obeUbigeoListas.Ubigeo03);
                    lbeUbicaciongeografica.Insert(0, new beUbicaciongeografica { Ubi03 = "00", Distrito = "-- SELECCIONE --" });
                    ddlDistrito.DataSource = lbeUbicaciongeografica;
                    ddlDistrito.DataValueField = "Ubi03";
                    ddlDistrito.DataTextField = "Distrito";
                    ddlDistrito.DataBind();
                    ddlDistrito.Enabled = (ddlProvincia.SelectedValue.Equals("00") ? false : true);
                    ddlDistrito.Focus();
                    ddlDistrito.Enabled = true;
                }

                if (ddlDistrito.Enabled == true)
                    ddlProvincia.Focus();
                //---------------------------------------------------------------------------
                //Fecha: 05/09/2018
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Asignar la moneda por defecto cuando es nueva oficina consular
                //---------------------------------------------------------------------------
                Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

                if (enmAccion == Enumerador.enmAccion.INSERTAR)
                {
                    int intParentesis = 0;
                    string strNombrePais = "";                    

                    if (ddlProvincia.SelectedItem.Text.IndexOf("(") > -1) // Pais
                    {
                        intParentesis = ddlProvincia.SelectedItem.Text.IndexOf("(");
                        strNombrePais = ddlProvincia.SelectedItem.Text.Substring(0, intParentesis).Trim();
                    }
                    else
                    {
                        strNombrePais = ddlProvincia.SelectedItem.Text.Trim();
                    }

                    PaisConsultasBL objPaisBL = new PaisConsultasBL();
                    DataTable dtPais = new DataTable();

                    int IntTotalCount = 0;
                    int IntTotalPages = 0;

                    dtPais = objPaisBL.Consultar_Pais(0,"A","1",10,"N",ref IntTotalCount, ref IntTotalPages,0,strNombrePais);

                    string strMonedaId = "";                    

                    if (dtPais.Rows.Count > 0)
                    {
                        string strPais = "";
                        for (int i = 0; i < dtPais.Rows.Count; i++)
                        {
                            strPais = dtPais.Rows[i]["PAIS_VNOMBRE"].ToString().Trim();

                            if (strPais.Equals(strNombrePais))
                                {
                                    strMonedaId = dtPais.Rows[i]["PAIS_SMONEDAID"].ToString();
                                    break;
                                }                                                       
                        }

                    }
                    if (strMonedaId != "0" && strMonedaId.Length > 0)
                    {
                        ddlMoneda.SelectedValue = strMonedaId;
                    }
                }
               
                //---------------------------------------------------------------------------
            }
            else
            {
                ddlDistrito.SelectedIndex = -1;
                ddlDistrito.Enabled = false;
            }
            updMantenimiento.Update();
        }

        protected void gdvOficinaConsular_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strScript = string.Empty;
            int intSeleccionado = Convert.ToInt32(e.CommandArgument);

            Session[strVariableIndice] = intSeleccionado;

            Session["iOperFuncionario"] = false;

            if (e.CommandName == "Consultar")
            {
                Session[strVariableAccion] = Enumerador.enmAccion.CONSULTAR;

                ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
                ctrlToolBarMantenimiento.btnEditar.Enabled = true;
                ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

                HabilitarMantenimiento(false);
                PintarSeleccionado();

                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
            }
            else if (e.CommandName == "Editar")
            {
                Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;

                ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
                ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                ctrlToolBarMantenimiento.btnEliminar.Enabled = true;

                HabilitarMantenimiento(true);
                PintarSeleccionado();
                CargarMonedas();
                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
            }

            Comun.EjecutarScript(Page, strScript);
        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            Session[strVariableDt] = new DataTable();
            BindGrid(Convert.ToInt16(ddlCategoria.SelectedValue), txtDescripcionConsulta.Text, ddlContinente.SelectedItem.Value, ddlPais.SelectedItem.Value);
        }

        protected void ctrlPaginadorDep_Click(object sender, EventArgs e)
        {
            Session[strVariableDtOFCH] = new DataTable();
            BindGridOficinaDependiente(Convert.ToInt16(Session["Ofco_ConsularId"]));
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            Session[strVariableDt] = new DataTable();
            ctrlPaginador.InicializarPaginador();
            BindGrid(Convert.ToInt16(ddlCategoria.SelectedValue), txtDescripcionConsulta.Text,
                ddlContinente.SelectedItem.Value, ddlPais.SelectedItem.Value);
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();
            Comun.EjecutarScript(Page, Util.ActivarTab(0, Constantes.CONST_TAB_INICIAL) + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }

        void ctrlToolBarMantenimiento_btnNuevoHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();

            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }

        void ctrlToolBarMantenimiento_btnEditarHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = true;
            HabilitarMantenimiento();
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_EDITAR));
        }

        void ctrlToolBarMantenimiento_btnEliminarHandler()
        {
            HabilitarMantenimiento(false);
            Session[strVariableAccion] = Enumerador.enmAccion.ELIMINAR;
            ctrlToolBarMantenimiento_btnGrabarHandler();
            HabilitarMantenimiento();
            Comun.EjecutarScript(Page, Util.NombrarTab(1, "Elimina"));
        }

        protected void chkEsJefaturaMant_CheckedChanged(object sender, EventArgs e)
        {
            Session["ZonaHoraria"] = ddlZonaHoraria.SelectedValue;
            ddlZonaHoraria.SelectedValue = Convert.ToString(Session["ZonaHoraria"]);
        }

        void ctrlToolBarMantenimiento_btnGrabarHandler()
        {
            Proceso p = new Proceso();
            int IntRpta = 0;
            string strScript = string.Empty;

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];
            OficinaConsularConsultasBL BLc = new OficinaConsularConsultasBL();

            if (enmAccion == Enumerador.enmAccion.INSERTAR)
            {
                IntRpta = BLc.Existe(txtNombreMant.Text, 0, 1);

                if (IntRpta == 1)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Oficina Consular", "Ya existe una oficina consular con el nombre que esta consignando.", false, 190, 250);
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }
            }
            else if (enmAccion == Enumerador.enmAccion.MODIFICAR)
            {
                IntRpta = BLc.Existe(txtNombreMant.Text, Convert.ToInt16(Session["Ofco_ConsularId"]), 2);

                if (IntRpta == 1)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Oficina Consular", "Ya existe una oficina consular con el nombre que esta consignando.", false, 190, 250);
                    Comun.EjecutarScript(Page, strScript);
                    return;
                }
            }

            ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
            updMantenimiento.Update();

            Session["Grabo"] = "NO";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "var yes = confirm('¿Desea realizar la operación?'); if (yes) __doPostBack('GrabarHandler', 'yes');", true);
        }

        void ctrlToolBarMantenimiento_btnCancelarHandler()
        {
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            Comun.EjecutarScript(Page, Util.ActivarTab(0, Constantes.CONST_TAB_INICIAL) + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }

        #region OficinasDependientes
        protected void gdvDependientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int intSeleccionadoDet = Convert.ToInt32(e.CommandArgument);
            Session[strVariableIndiceOFCH] = intSeleccionadoDet;

            if (e.CommandName == "Eliminar")
            {
                DataTable dtDependientes = (DataTable)Session[strVariableDtOFCH];
                if (dtDependientes != null)
                {
                    if (dtDependientes.Rows.Count > 0)
                    {
                        dtDependientes.Rows.RemoveAt(intSeleccionadoDet);
                        dtDependientes.AcceptChanges();
                        gdvDependientes.DataSource = dtDependientes;
                        gdvDependientes.DataBind();
                        Session[strVariableDtOFCH] = dtDependientes;
                    }
                }
            }
        }

        #endregion

        #region Funcionarios

        protected void gdvFuncionario_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int intSeleccionadoDet = Convert.ToInt32(e.CommandArgument);
            Session[strVariableIndiceFunc] = intSeleccionadoDet;

            if (e.CommandName == "Eliminar")
            {
                DataTable dtFuncionario = ((DataTable)Session[strVariableDtFunc]).Copy();
                dtFuncionario.Rows[intSeleccionadoDet].Delete();
                dtFuncionario.AcceptChanges();
                Session[strVariableDtFunc] = dtFuncionario;

                this.gdvFuncionario.DataSource = dtFuncionario;
                this.gdvFuncionario.DataBind();

                //---
                DataTable dtFuncionarioTmp = ((DataTable)Session[strVariableDtFunc_tmp]).Copy();
                dtFuncionarioTmp.Rows[intSeleccionadoDet]["ocfu_cEstado"] = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
                dtFuncionarioTmp.AcceptChanges();
                Session[strVariableDtFunc_tmp] = dtFuncionarioTmp;
            }
            if (e.CommandName == "EditarCargo")
            {
                ocultarFuncionario.Visible = true;
                DataTable dtFuncionario = ((DataTable)Session[strVariableDtFunc]).Copy();
                string nombreFuncionario = string.Empty;
                string cargo = string.Empty;
                Int16 sGenero = 0;
                cargo = dtFuncionario.Rows[intSeleccionadoDet]["CARGO"].ToString();
                sGenero = Convert.ToInt16(dtFuncionario.Rows[intSeleccionadoDet]["sGenero"]);
                ddlCargos.SelectedIndex = 0;
                nombreFuncionario = dtFuncionario.Rows[intSeleccionadoDet]["ocfu_vApellidoPaternoFuncionario"].ToString() + " " +
                                    dtFuncionario.Rows[intSeleccionadoDet]["ocfu_vApellidoMaternoFuncionario"].ToString() + " ," +
                                    dtFuncionario.Rows[intSeleccionadoDet]["ocfu_vNombreFuncionario"].ToString();
                lblFuncionario.Text = nombreFuncionario;
                hCodID.Value = dtFuncionario.Rows[intSeleccionadoDet]["ocfu_sOfiConFuncionarioId"].ToString();

                if (ddlCargos.Items.FindByText(cargo) == null)
                {
                    ddlCargos.SelectedIndex = 0;
                }
                else
                {
                    ddlCargos.SelectedValue = ddlCargos.Items.FindByText(cargo).Value;
                }

                if (sGenero > 2)
                {
                    ddlSexo.SelectedValue = "0";
                }
                else
                {
                    ddlSexo.SelectedValue = sGenero.ToString();
                }


            }
            else { ocultarFuncionario.Visible = false; }
        }

        #endregion

        #endregion

        #region Metodos
        private void CargarDatosIniciales()
        {
            Session.Add(strVariableAccion, Enumerador.enmAccion.INSERTAR);
            Session.Add(strVariableIndice, -1);
            Session.Add(strVariableDt, new DataTable());

            Session.Add(strVariableAccionOFCH, Enumerador.enmAccion.INSERTAR);
            Session.Add(strVariableIndiceOFCH, -1);
            Session.Add(strVariableDtOFCH, new DataTable());

            Session.Add(strVariableAccionFunc, Enumerador.enmAccion.INSERTAR);
            Session.Add(strVariableIndiceFunc, -1);

            DataTable dt = FillEmptyDatagdvDependientes();
            gdvDependientes.DataSource = dt;
            gdvDependientes.DataBind();
            Session[strVariableDtOFCH] = dt;

            Session.Remove("DtRegFuncionario");
            Session["DtRegFuncionario"] = CrearDtRegFuncionario();
            ((DataTable)Session["DtRegFuncionario"]).Clear();

            gdvFuncionario.DataSource = CrearDtRegFuncionario();
            gdvFuncionario.DataBind();

            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            // Inicializar la variable de sesión (tabla funcionarios)
            OficinaConsularFuncConsultasBL objBL = new OficinaConsularFuncConsultasBL();
            DataTable dtFuncionarios = objBL.Obtener(Constantes.CONST_OFICINACONSULAR_LIMA);
            Session[strVariableDtFunc] = dtFuncionarios.Clone();
            // fin

            Comun.EjecutarScript(Page, Util.NombrarTab(0, "Consulta"));
            updMantenimiento.Update();
        }

        private void CargarListadosDesplegables()
        {
            Util.CargarParametroDropDownList(ddlCategoria, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONFIG_CATEGORIA_OFICINA_CONSULAR), true);


            //-----------------------------------------------
            beUbigeoListas obeUbigeoListas = new beUbigeoListas();
            UbigeoConsultasBL objUbigeoBL = new UbigeoConsultasBL();

            obeUbigeoListas = objUbigeoBL.obtenerUbiGeo();

            ViewState["Ubigeo"] = obeUbigeoListas;
            if (obeUbigeoListas != null)
            {
                if (obeUbigeoListas.Ubigeo01.Count > 0)
                {
                    obeUbigeoListas.Ubigeo01.Insert(0, new beUbicaciongeografica { Ubi01 = "00", Departamento = "-- SELECCIONE --" });
                    ddlContinente.DataSource = obeUbigeoListas.Ubigeo01;
                    ddlContinente.DataValueField = "Ubi01";
                    ddlContinente.DataTextField = "Departamento";
                    ddlContinente.DataBind();

                    ddlDepartamento.DataSource = obeUbigeoListas.Ubigeo01;
                    ddlDepartamento.DataValueField = "Ubi01";
                    ddlDepartamento.DataTextField = "Departamento";
                    ddlDepartamento.DataBind();

                }
            }
              //Util.CargarDropDownList(ddlContinente, comun_Part3.ObtenerContDepa(Session, Enumerador.enmNacionalidad.EXTRANJERA), "ubge_vDepartamento", "ubge_cUbi01", true);
            //-----------------------------------------------


            //comun_Part3.CargarUbigeo(Session, ddlDepartamento, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, "", "", true, Enumerador.enmNacionalidad.EXTRANJERA);
            this.ddlProvincia.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
            this.ddlDistrito.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));

          
            Util.CargarParametroDropDownList(ddlPais, new DataTable(), true);

            Util.CargarParametroDropDownList(ddlCategoriaMant, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONFIG_CATEGORIA_OFICINA_CONSULAR), true);

            //-------------------------------------------------------------
            //Fecha: 20/11/2019
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Enviar la tabla Parametros para optimizar la 
            //          carga de listas desplegables
            //-------------------------------------------------------------
            DataTable dtMoneda = new DataTable();
            dtMoneda = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.MONEDA);

            Util.CargarParametroDropDownList(ddlMoneda, dtMoneda, true);
            Util.CargarParametroDropDownList(ddlMonedaPopup, dtMoneda, true);
            dtMoneda.Dispose();
            //-------------------------------------------------------------


            Util.CargarComboAnios(ddlPorcentaje, 0, Constantes.CONST_PORCENTAJE_MAX_TC);

            comun_Part2.CargarOficinaConsular(Session, ddlOficinaConsularPedido, false);
            ddlOficinaConsularPedido.SelectedValue = Constantes.CONST_OFICINACONSULAR_LIMA.ToString();

            comun_Part2.CargarOficinaConsular(Session, ddlOficinaDependiente, false);
            ddlOficinaDependiente.SelectedValue = Constantes.CONST_OFICINACONSULAR_LIMA.ToString();

            Util.CargarDropDownList(ddlZonaHoraria, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONFIG_DIFERENCIA_HORARIA), "descripcion", "id", true);
            Util.CargarDropDownList(ddlDiferenciaHoraria, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONFIG_DIFERENCIA_HORARIA), "descripcion", "valor", true);

        }

        private void HabilitarMantenimiento(bool bolHabilitar = true)
        {
            ddlDepartamento.Enabled = bolHabilitar;

            ddlProvincia.Enabled = false;
            ddlDistrito.Enabled = false;

            txtCodigoLocal.Enabled = bolHabilitar;
            ddlCategoriaMant.Enabled = bolHabilitar;

            if (Convert.ToInt16(Session[strVariableAccion]) == Convert.ToInt16(Enumerador.enmAccion.MODIFICAR))
            {
                string script = "EnabledBtnNuevaMoneda();";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", script, true);
                ddlMoneda.Enabled = false;
            }
            else {
                string script = "DisableBtnNuevaMoneda();";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", script, true);
                ddlMoneda.Enabled = true;
            }

            //ddlMoneda.Enabled = bolHabilitar;
            //btnMonedaNueva.Enabled = bolHabilitar;
            txtNombreMant.Enabled = bolHabilitar;
            txtNombreAbrev.Enabled = bolHabilitar;
            txtDireccionMant.Enabled = bolHabilitar;
            txtTelefonoMant.Enabled = bolHabilitar;
            chkActivoMant.Enabled = bolHabilitar;

            chk01.Enabled = bolHabilitar;
            chk02.Enabled = bolHabilitar;
            chk03.Enabled = bolHabilitar;
            chk04.Enabled = bolHabilitar;
            chk05.Enabled = bolHabilitar;
            chk06.Enabled = bolHabilitar;
            chk07.Enabled = bolHabilitar;
            chk08.Enabled = bolHabilitar;
            chk09.Enabled = bolHabilitar;
            chk10.Enabled = bolHabilitar;
            chk11.Enabled = bolHabilitar;
            chk12.Enabled = bolHabilitar;
            chk13.Enabled = bolHabilitar;
            chk14.Enabled = bolHabilitar;
            chk15.Enabled = bolHabilitar;
            chk16.Enabled = bolHabilitar;
            chk17.Enabled = bolHabilitar;
            chk18.Enabled = bolHabilitar;
            chk19.Enabled = bolHabilitar;
            chk20.Enabled = bolHabilitar;
            chk21.Enabled = bolHabilitar;
            chk22.Enabled = bolHabilitar;
            chk23.Enabled = bolHabilitar;
            chk24.Enabled = bolHabilitar;

            ddlZonaHoraria.Enabled = bolHabilitar;

            txtSitioWeb.Enabled = bolHabilitar;

            txtRangoIPInicio.Enabled = bolHabilitar;
            txtRangoIPFin.Enabled = bolHabilitar;

            chkTieneASN.Enabled = bolHabilitar;
            chkEsJefaturaMant.Enabled = bolHabilitar;
            chkRemesaLimaMant.Enabled = bolHabilitar;
            //----------------------------------------------------
            //Fecha: 27/04/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Inicializar chkElecciones
            //Requerimiento solicitado por Rita Huambachano.
            //----------------------------------------------------
            chkElecciones.Enabled = bolHabilitar;
            //----------------------------------------------------

            ddlPorcentaje.Enabled = bolHabilitar;

            ddlOficinaConsularPedido.Enabled = bolHabilitar;


            ddlOficinaConsularPedido.Enabled = bolHabilitar;

            ddlOficinaDependiente.Enabled = bolHabilitar;

            gdvDependientes.Enabled = bolHabilitar;

            gdvFuncionario.Enabled = bolHabilitar;
        }

        private void LimpiarDatosMantenimiento()
        {
            chkRolActivo.Checked = true;
            chkActivoMant.Checked = true;
            ddlDepartamento.SelectedIndex = -1;
            ddlProvincia.SelectedIndex = -1;
            ddlDistrito.SelectedIndex = -1;
            ddlCategoriaMant.SelectedIndex = -1;
            ddlMoneda.SelectedIndex = -1;
            ddlMonedaPopup.SelectedIndex = -1;
            txtNombreMant.Text = "";
            txtNombreAbrev.Text = "";
            txtDireccionMant.Text = "";
            txtTelefonoMant.Text = "";
            txtLatitud.Text = "";
            txtLongitud.Text = "";
            //--------------------------------------
            // Autor: Miguel Márquez Beltrán
            // Fecha: 10/11/2016
            // Objetivo: Actualizar el código de local
            //--------------------------------------
            txtCodigoLocal.Text = "";
            //--------------------------------------
            chk01.Checked = false;
            chk02.Checked = false;
            chk03.Checked = false;
            chk04.Checked = false;
            chk05.Checked = false;
            chk06.Checked = false;
            chk07.Checked = false;
            chk08.Checked = false;
            chk09.Checked = false;
            chk10.Checked = false;
            chk11.Checked = false;
            chk12.Checked = false;
            chk13.Checked = false;
            chk14.Checked = false;
            chk15.Checked = false;
            chk16.Checked = false;
            chk17.Checked = false;
            chk18.Checked = false;
            chk19.Checked = false;
            chk20.Checked = false;
            chk21.Checked = false;
            chk22.Checked = false;
            chk23.Checked = false;
            chk24.Checked = false;

            ddlZonaHoraria.SelectedIndex = -1;
            ddlDiferenciaHoraria.SelectedIndex = -1;

            txtSitioWeb.Text = "";

            txtRangoIPInicio.Text = "";
            txtRangoIPFin.Text = "";

            chkTieneASN.Checked = false;
            chkEsJefaturaMant.Checked = false;
            chkRemesaLimaMant.Checked = false;
            //----------------------------------------------------
            //Fecha: 27/04/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Inicializar chkElecciones
            //Requerimiento solicitado por Rita Huambachano.
            //----------------------------------------------------
            chkElecciones.Checked = false;
            //----------------------------------------------------
            ddlPorcentaje.SelectedIndex = -1;
            string StrScript = string.Empty;
            StrScript = @"$(function(){{
                            HabilitaComboPorcentaje(0);
                        }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitaComboPorcentaje", StrScript, true);
            ddlPorcentaje.SelectedItem.Text = "0";

            
            //----------------------------------------------------------
            //Fecha: 15/12/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Comentar las siguientes líneas de código
            //      debido a que en la lista no existe Sede Central
            //      y como resultado se mostraba un error.
            //----------------------------------------------------------
            //ddlOficinaConsularPedido.SelectedValue = "1";
           // ddlOficinaDependiente.SelectedValue = "1";
            //----------------------------------------------------------
            gdvDependientes.DataSource = null;
            gdvDependientes.DataBind();

            DataTable dt = FillEmptyDatagdvDependientes();
            gdvDependientes.DataSource = dt;
            gdvDependientes.DataBind();
            Session[strVariableDtOFCH] = dt;




            gdvFuncionario.DataSource = null;
            gdvFuncionario.DataBind();


            Session.Remove("DtRegFuncionario");
            Session["DtRegFuncionario"] = CrearDtRegFuncionario();
            ((DataTable)Session["DtRegFuncionario"]).Clear();

            gdvFuncionario.DataSource = CrearDtRegFuncionario();
            gdvFuncionario.DataBind();
            ctrlPaginadorDep.InicializarPaginador();
            ctrlPaginadorDep.Visible = false;

            //-------------------------------------------
            // Limpiar objetos de la consulta.
            //-------------------------------------------
            ddlContinente.SelectedIndex = 0;
            this.ddlPais.Items.Clear();
            this.ddlPais.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));

            txtDescripcionConsulta.Text = "";
            ddlCategoria.SelectedIndex = -1;
            chkSelEsJefatura.Checked = false;
            chkSelElecciones.Checked = false;
            //-------------------------------------------

            gdvOficinaConsular.DataSource = null;
            gdvOficinaConsular.DataBind();
            ctrlPaginador.InicializarPaginador();
            ctrlPaginador.Visible = false;

            updMantenimiento.Update();
            updConsulta.Update();
        }

        private DataRow ObtenerFilaSeleccionada()
        {
            if (Session != null)
            {
                int intSeleccionado = (int)Session[strVariableIndice];
                return ((DataTable)Session[strVariableDt]).Rows[intSeleccionado];
            }

            return null;
        }

        private void CargarMonedas()
        {
            DataTable _dt = new DataTable();
            OficinaConsularConsultasBL BL = new OficinaConsularConsultasBL();

            _dt = BL.ConsultarMonedas(Convert.ToInt16(Session["Ofco_ConsularId"]));
            if (_dt.Rows.Count == 0)
            {
                ctrlValidacionRegistro.MostrarValidacion("No exsten registros para mostrar");
                gdvMonedaPop.DataSource = _dt;
                gdvMonedaPop.DataBind();
                LimpiarPopupMoneda();
            }
            else
            {
                LimpiarPopupMoneda();
                gdvMonedaPop.DataSource = _dt;
                gdvMonedaPop.DataBind();
            }
            updMantenimiento.Update();
            //updConsulta.Update();
        }
        private void LimpiarPopupMoneda()
        {
            ddlMonedaPopup.SelectedIndex = 0;

            string strFechaTexto = Comun.ObtenerFechaActualTexto(Session);

            ctrFechaFin.Text = strFechaTexto;
            ctrFechaInicio.Text = strFechaTexto;
        }
        private void PintarSeleccionado()
        {
            string StrScript = string.Empty;
            try
            {
                if (Session != null)
                {
                    DataRow drSeleccionado = ObtenerFilaSeleccionada();
                    if (drSeleccionado != null)
                    {
                        hdn_sOficinaConsularId.Value = drSeleccionado["ofco_sOficinaConsularId"].ToString();

                        Session["Ofco_ConsularId"] = drSeleccionado["ofco_sOficinaConsularId"].ToString();
                        Session["ReferenciaPadreId"] = drSeleccionado["ofco_iReferenciaPadreId"].ToString();

                        ddlDepartamento.SelectedValue = drSeleccionado["ofco_cUbigeoCodigo"].ToString().Substring(0, 2);

                        UbigeoConsultasBL BL = new UbigeoConsultasBL();
                        Session["dtUbigeo"] = BL.ObtenerUbigeo(drSeleccionado["ofco_cUbigeoCodigo"].ToString()).Copy();

                        if (((DataTable)Session["dtUbigeo"]).Rows.Count > 0)
                        {
                            this.ddlProvincia.Items.Clear();
                            this.ddlProvincia.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
                            this.ddlProvincia.Items.Insert(1, new ListItem((string)((DataTable)Session["dtUbigeo"]).Rows[0][5], (string)((DataTable)Session["dtUbigeo"]).Rows[0][2]));
                            ddlProvincia.SelectedValue = (string)((DataTable)Session["dtUbigeo"]).Rows[0][2];

                            this.ddlDistrito.Items.Clear();
                            this.ddlDistrito.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
                            this.ddlDistrito.Items.Insert(1, new ListItem((string)((DataTable)Session["dtUbigeo"]).Rows[0][6], (string)((DataTable)Session["dtUbigeo"]).Rows[0][3]));
                            ddlDistrito.SelectedValue = (string)((DataTable)Session["dtUbigeo"]).Rows[0][3];
                        }
                        if (drSeleccionado["ofco_sCategoriaId"].ToString() != "")
                        {
                            ddlCategoriaMant.SelectedValue = drSeleccionado["ofco_sCategoriaId"].ToString();
                        }

                        if (drSeleccionado["ofco_sMonedaId"].ToString() != "")
                        {
                            ddlMoneda.SelectedValue = drSeleccionado["ofco_sMonedaId"].ToString();
                        }
                        if (drSeleccionado["ofco_sMonedaId"].ToString() != "")
                        {
                            ddlMonedaPopup.SelectedValue = drSeleccionado["ofco_sMonedaId"].ToString();
                        }
                        
                        txtNombreMant.Text = drSeleccionado["ofco_vNombre"].ToString();
                        txtNombreAbrev.Text = drSeleccionado["ofco_vSiglas"].ToString();
                        txtDireccionMant.Text = drSeleccionado["ofco_vDireccion"].ToString();
                        txtTelefonoMant.Text = drSeleccionado["ofco_vTelefono"].ToString();

                        txtCodigoLocal.Text = drSeleccionado["ofco_vCodigoLocal"].ToString();
                        txtLongitud.Text = drSeleccionado["ofco_nvLongitud"].ToString();
                        txtLatitud.Text = drSeleccionado["ofco_nvLatitud"].ToString();
                        if (drSeleccionado["ofco_cEstado"].ToString() == ((char)Enumerador.enmEstado.ACTIVO).ToString())
                        {
                            chkActivoMant.Checked = true;
                        }
                        else
                        {
                            chkActivoMant.Checked = false;
                        }

                       
                        object objHorario = drSeleccionado["ofco_vHorarioAtencion"];

                        if (objHorario != null)
                        {
                            string strHorario = objHorario.ToString();

                            if (strHorario != string.Empty)
                            {
                                chk01.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(0, 1)));
                                chk02.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(1, 1)));
                                chk03.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(2, 1)));
                                chk04.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(3, 1)));
                                chk05.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(4, 1)));
                                chk06.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(5, 1)));
                                chk07.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(6, 1)));
                                chk08.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(7, 1)));
                                chk09.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(8, 1)));
                                chk10.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(9, 1)));
                                chk11.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(10, 1)));
                                chk12.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(11, 1)));
                                chk13.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(12, 1)));
                                chk14.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(13, 1)));
                                chk15.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(14, 1)));
                                chk16.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(15, 1)));
                                chk17.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(16, 1)));
                                chk18.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(17, 1)));
                                chk19.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(18, 1)));
                                chk20.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(19, 1)));
                                chk21.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(20, 1)));
                                chk22.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(21, 1)));
                                chk23.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(22, 1)));
                                chk24.Checked = Convert.ToBoolean(Convert.ToInt32(strHorario.Substring(23, 1)));
                            }
                        }

                        ddlZonaHoraria.SelectedValue = drSeleccionado["ofco_sZonaHoraria"].ToString();
                        ddlDiferenciaHoraria.SelectedIndex = ddlZonaHoraria.SelectedIndex;

                        txtSitioWeb.Text = drSeleccionado["ofco_vSitioWeb"].ToString();

                        txtRangoIPInicio.Text = drSeleccionado["ofco_vRangoInicialIP"].ToString();
                        txtRangoIPFin.Text = drSeleccionado["ofco_vRangoFinIP"].ToString();

                        if (drSeleccionado["ofco_bASNFlag"].ToString() == "True")
                        {
                            chkTieneASN.Checked = true;
                        }
                        else
                        {
                            chkTieneASN.Checked = false;
                        }

                        if (drSeleccionado["ofco_vJefatura"].ToString() == "SI")
                        {
                            chkEsJefaturaMant.Checked = true;

                            StrScript = string.Empty;
                            StrScript = @"$(function(){{
                                        HabilitaComboPorcentaje(1);
                                    }});";
                            StrScript = string.Format(StrScript);
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitaComboPorcentaje", StrScript, true);
                        }
                        else
                        {
                            chkEsJefaturaMant.Checked = false;

                            StrScript = string.Empty;
                            StrScript = @"$(function(){{
                                        HabilitaComboPorcentaje(0);
                                    }});";
                            StrScript = string.Format(StrScript);
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "HabilitaComboPorcentaje", StrScript, true);
                        }

                        if (drSeleccionado["ofco_IRemesaLimaFlag"].ToString() == "True")
                        {
                            chkRemesaLimaMant.Checked = true;
                        }
                        else
                        {
                            chkRemesaLimaMant.Checked = false;
                        }

                        //----------------------------------------------------
                        //Fecha: 27/04/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Asignar el valor a chkElecciones.
                        //Requerimiento solicitado por Rita Huambachano.
                        //----------------------------------------------------
                        if (drSeleccionado["ofco_bElecciones"].ToString() == "True")
                        {
                            chkElecciones.Checked = true;
                        }
                        else
                        {
                            chkElecciones.Checked = false;
                        }
                        //----------------------------------------------------


                        if (drSeleccionado["ofco_sPorcentajeTipoCambio"].ToString().Length == 0)
                        {
                            ddlPorcentaje.SelectedItem.Text = "0";
                        }
                        else
                        {
                            ddlPorcentaje.SelectedItem.Text = drSeleccionado["ofco_sPorcentajeTipoCambio"].ToString();
                        }

                        if (drSeleccionado["ofco_iReferenciaPadreId"].ToString() != "")
                        {
                            ddlOficinaConsularPedido.SelectedValue = drSeleccionado["ofco_iReferenciaPadreId"].ToString();
                        }
                        if (drSeleccionado["ofco_sOficinaDependeDe"].ToString() != "")
                        {
                            ddlOficinaDependiente.SelectedValue = drSeleccionado["ofco_sOficinaDependeDe"].ToString();
                        }
                        if (drSeleccionado["ofco_sOficinaConsularId"].ToString() != "")
                        {
                            BindGridOficinaDependiente(Convert.ToInt16(drSeleccionado["ofco_sOficinaConsularId"].ToString()));
                            BindGridFuncionario(Convert.ToInt16(drSeleccionado["ofco_sOficinaConsularId"].ToString()));
                        }

                        updMantenimiento.Update();
                    }
                }
            }
            catch
            {

            }
            
        }

        private SGAC.BE.MRE.SI_OFICINACONSULAR ObtenerEntidadMantenimiento()
        {
            if (Session != null)
            {
                SGAC.BE.MRE.SI_OFICINACONSULAR obj = new SGAC.BE.MRE.SI_OFICINACONSULAR();

                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
                {
                    obj.ofco_sOficinaConsularId = Convert.ToInt16(ObtenerFilaSeleccionada()["ofco_sOficinaConsularId"]);
                }

                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.ELIMINAR)
                {
                    obj.ofco_cUbigeoCodigo = ddlDepartamento.SelectedValue + ddlProvincia.SelectedValue + ddlDistrito.SelectedValue;
                    obj.ofco_sCategoriaId = Convert.ToInt16(ddlCategoriaMant.SelectedValue);
                    obj.ofco_sMonedaId = Convert.ToInt16(ddlMoneda.SelectedValue);
                    obj.ofco_vNombre = txtNombreMant.Text.ToUpper();

                    if (txtNombreAbrev.Text != string.Empty)
                    {
                        obj.ofco_vSiglas = txtNombreAbrev.Text.ToUpper();
                    }

                    //--------------------------------------
                    // Autor: Miguel Márquez Beltrán
                    // Fecha: 10/11/2016
                    // Objetivo: Actualizar el código de local
                    //--------------------------------------
                    if (txtCodigoLocal.Text != string.Empty)
                    {
                        obj.ofco_vCodigoLocal = txtCodigoLocal.Text.Trim();
                    }

                    //--------------------------------------
                    if (txtDireccionMant.Text != string.Empty)
                    {
                        obj.ofco_vDireccion = txtDireccionMant.Text.ToUpper();
                    }

                    if (txtTelefonoMant.Text != string.Empty)
                    {
                        obj.ofco_vTelefono = txtTelefonoMant.Text.Trim();
                    }

                    obj.ofco_cHorarioAtencion = ObtenerHorario();

                    if (ddlZonaHoraria.SelectedValue != "0")
                    {
                        obj.ofco_sZonaHoraria = Convert.ToInt16(ddlZonaHoraria.SelectedValue);
                    }

                    ddlDiferenciaHoraria.SelectedIndex = ddlZonaHoraria.SelectedIndex;
                    if (ddlDiferenciaHoraria.SelectedValue != "0")
                    {
                        obj.ofco_fDiferenciaHoraria = Convert.ToDouble(ddlDiferenciaHoraria.SelectedValue);
                    }

                    if (txtSitioWeb.Text != string.Empty)
                    {
                        obj.ofco_vSitioWeb = txtSitioWeb.Text;
                    }

                    if (txtRangoIPInicio.Text != string.Empty)
                    {
                        obj.ofco_vRangoInicialIP = txtRangoIPInicio.Text;
                    }

                    if (txtRangoIPFin.Text != string.Empty)
                    {
                        obj.ofco_vRangoFinIP = txtRangoIPFin.Text;
                    }

                    if (chkTieneASN.Checked)
                    {
                        obj.ofco_bASNFlag = true;
                    }
                    else
                    {
                        obj.ofco_bASNFlag = false;
                    }

                    if (chkEsJefaturaMant.Checked)
                    {
                        obj.ofco_bJefaturaFlag = true;
                    }
                    else
                    {
                        obj.ofco_bJefaturaFlag = false;
                    }

                    //----------------------------------------------------
                    //Fecha: 27/04/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Asignar valor al atributo: ofco_bElecciones
                    //Requerimiento solicitado por Rita Huambachano.
                    //----------------------------------------------------
                    if (chkElecciones.Checked)
                    {
                        obj.ofco_bElecciones = true;
                    }
                    else
                    {
                        obj.ofco_bElecciones = false;
                    }
                    //----------------------------------------------------

                    obj.ofco_nvLatitud = txtLatitud.Text;
                    obj.ofco_nvLongitud = txtLongitud.Text;

                    if (chkRemesaLimaMant.Checked)
                    {
                        obj.ofco_bRemesaLimaFlag = true;
                    }
                    else
                    {
                        obj.ofco_bRemesaLimaFlag = false;
                    }

                    obj.ofco_sPorcentajeTipoCambio = Convert.ToInt16(ddlPorcentaje.SelectedItem.Text);

                    obj.ofco_sReferenciaId = Convert.ToInt16(ddlOficinaConsularPedido.SelectedValue);

                    obj.ofco_sOficinaDependeDe = Convert.ToInt16(ddlOficinaDependiente.SelectedValue);

                    if (chkActivoMant.Checked)
                    {
                        obj.ofco_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                    }
                    else
                    {
                        obj.ofco_cEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
                    }

                }

                obj.ofco_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                obj.ofco_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                obj.ofco_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                obj.ofco_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();

                return obj;
            }

            return null;
        }

        private SGAC.BE.MRE.SI_OFICINACONSULAR ObtenerEntidadConsulta()
        {
            if (Session != null)
            {
                DataTable dt = (DataTable)Session[strVariableDt];
                DataRow drSeleccionado = dt.Rows[gdvOficinaConsular.SelectedIndex];

                SGAC.BE.MRE.SI_OFICINACONSULAR obj = new SGAC.BE.MRE.SI_OFICINACONSULAR();

                obj.ofco_vCodigo = drSeleccionado["ofco_vCodigo"].ToString();

                obj.ofco_sCategoriaId = Convert.ToInt16(drSeleccionado["ofco_sCategoriaId"]);
                obj.ofco_vNombre = drSeleccionado["ofco_vNombre"].ToString();
                obj.ofco_vDireccion = drSeleccionado["ofco_vDireccion"].ToString();
                obj.ofco_vTelefono = drSeleccionado["ofco_vTelefono"].ToString();
                obj.ofco_vSiglas = drSeleccionado["ofco_vSiglas"].ToString();
                obj.ofco_cUbigeoCodigo = drSeleccionado["ofco_cUbigeoCodigo"].ToString();
                obj.ofco_cHorarioAtencion = drSeleccionado["ofco_cHorarioAtencion"].ToString();
                obj.ofco_bASNFlag = Convert.ToBoolean(drSeleccionado["ofco_bASNFlag"]);
                obj.ofco_bJefaturaFlag = Convert.ToBoolean(drSeleccionado["ofco_bJefaturaFlag"]);
                obj.ofco_bRemesaLimaFlag = Convert.ToBoolean(drSeleccionado["ofco_bRemesaLimaFlag"]);
                //-----------------------------------------------------------
                //Fecha: 27/04/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Asignar el valor al atributo: ofco_bElecciones
                //Requerimiento solicitado por Rita Huambachano.
                //------------------------------------------------------------
                obj.ofco_bElecciones = Convert.ToBoolean(drSeleccionado["ofco_bElecciones"]);
                //----------------------------------------------------
                obj.ofco_cEstado = drSeleccionado["ofco_cEstado"].ToString();
                obj.ofco_sUsuarioCreacion = Convert.ToInt16(drSeleccionado["ofco_sUsuarioCreacion"]);
                obj.ofco_sUsuarioModificacion = Convert.ToInt16(drSeleccionado["ofco_sUsuarioModificacion"]);

                return obj;
            }

            return null;
        }

        private List<SGAC.BE.MRE.SI_OFICINACONSULAR> ObtenerOficinaDependiente()
        {
            DataTable dtDependienteOficinaConsular = (DataTable)Session[strVariableDtOFCH];
            if (dtDependienteOficinaConsular == null)
            {
                return null;
            }
            List<SGAC.BE.MRE.SI_OFICINACONSULAR> lsOficinaDependiente = new List<SI_OFICINACONSULAR>();
            foreach (DataRow dr in dtDependienteOficinaConsular.Rows)
            {
                SGAC.BE.MRE.SI_OFICINACONSULAR OficinaDependiente = new SGAC.BE.MRE.SI_OFICINACONSULAR();
                OficinaDependiente.ofco_sOficinaConsularId = Convert.ToInt16(dr["ofco_sOficinaConsularId"]);

                //---------------------------------------------
                //Fecha: 27/04/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Validar campo nulo.
                //---------------------------------------------
                if (!dr["ofco_sReferenciaId"].Equals(DBNull.Value))
                {
                    OficinaDependiente.ofco_sReferenciaId = Convert.ToInt16(dr["ofco_sReferenciaId"]);
                }
                //---------------------------------------------
                OficinaDependiente.ofco_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                OficinaDependiente.ofco_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();

                lsOficinaDependiente.Add(OficinaDependiente);
            }
            return lsOficinaDependiente;
        }


        private List<SGAC.BE.MRE.RE_OFICINACONSULARFUNCIONARIO> ObtenerListadoEntidadDetalle()
        {
            DataTable dtFuncionarioOficinaConsular = (DataTable)Session[strVariableDtFunc];
            DataTable dtFuncionarioTemp = ((DataTable)Session[strVariableDtFunc_tmp]).Copy();
            if (dtFuncionarioTemp == null)
            {
                return null;
            }

            List<SGAC.BE.MRE.RE_OFICINACONSULARFUNCIONARIO> lstEntidadDetalle = new List<SGAC.BE.MRE.RE_OFICINACONSULARFUNCIONARIO>();
            foreach (DataRow dr in dtFuncionarioTemp.Rows)
            {
                SGAC.BE.MRE.RE_OFICINACONSULARFUNCIONARIO objEntidadDetalle = new SGAC.BE.MRE.RE_OFICINACONSULARFUNCIONARIO();
                objEntidadDetalle.ocfu_sOfiConFuncionarioId = Convert.ToInt16(dr["ocfu_sOfiConFuncionarioId"]);
                objEntidadDetalle.ocfu_sOficinaConsularId = Convert.ToInt16(dr["ocfu_sOficinaConsularId"]);
                objEntidadDetalle.ocfu_vDocumentoNumero = Convert.ToString(dr["sDocumentoFuncionario"]);
                objEntidadDetalle.ocfu_vNombreFuncionario = Convert.ToString(dr["ocfu_vNombreFuncionario"]);
                objEntidadDetalle.ocfu_vApellidoPaternoFuncionario = Convert.ToString(dr["ocfu_vApellidoPaternoFuncionario"]);
                objEntidadDetalle.ocfu_vApellidoMaternoFuncionario = Convert.ToString(dr["ocfu_vApellidoMaternoFuncionario"]);
                objEntidadDetalle.ocfu_vCargo = Convert.ToString(dr["CARGO"]);
                objEntidadDetalle.ocfu_IFuncionarioId = Convert.ToInt32(dr["IFuncionarioId"]);
                objEntidadDetalle.ocfu_cEstado = Convert.ToString(dr["ocfu_cEstado"]);
                objEntidadDetalle.ocfu_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objEntidadDetalle.ocfu_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objEntidadDetalle.ocfu_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objEntidadDetalle.ocfu_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objEntidadDetalle.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                objEntidadDetalle.sGeneroId = Convert.ToInt16(dr["sGenero"]);

                lstEntidadDetalle.Add(objEntidadDetalle);
            }

            return lstEntidadDetalle;
        }

        private string ObtenerHorario()
        {
            string strHorario = string.Empty;

            strHorario += Convert.ToInt32(chk01.Checked).ToString();
            strHorario += Convert.ToInt32(chk02.Checked).ToString();
            strHorario += Convert.ToInt32(chk03.Checked).ToString();
            strHorario += Convert.ToInt32(chk04.Checked).ToString();
            strHorario += Convert.ToInt32(chk05.Checked).ToString();
            strHorario += Convert.ToInt32(chk06.Checked).ToString();
            strHorario += Convert.ToInt32(chk07.Checked).ToString();
            strHorario += Convert.ToInt32(chk08.Checked).ToString();
            strHorario += Convert.ToInt32(chk09.Checked).ToString();
            strHorario += Convert.ToInt32(chk10.Checked).ToString();
            strHorario += Convert.ToInt32(chk11.Checked).ToString();
            strHorario += Convert.ToInt32(chk12.Checked).ToString();
            strHorario += Convert.ToInt32(chk13.Checked).ToString();
            strHorario += Convert.ToInt32(chk14.Checked).ToString();
            strHorario += Convert.ToInt32(chk15.Checked).ToString();
            strHorario += Convert.ToInt32(chk16.Checked).ToString();
            strHorario += Convert.ToInt32(chk17.Checked).ToString();
            strHorario += Convert.ToInt32(chk18.Checked).ToString();
            strHorario += Convert.ToInt32(chk19.Checked).ToString();
            strHorario += Convert.ToInt32(chk20.Checked).ToString();
            strHorario += Convert.ToInt32(chk21.Checked).ToString();
            strHorario += Convert.ToInt32(chk22.Checked).ToString();
            strHorario += Convert.ToInt32(chk23.Checked).ToString();
            strHorario += Convert.ToInt32(chk24.Checked).ToString();

            return strHorario;
        }

        private DataRow ObtenerFilaSeleccionadaFuncinarioDet()
        {
            if (Session != null)
            {
                Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

                int intSeleccionado = (int)Session[strVariableIndiceFunc];

                if (enmAccion == Enumerador.enmAccion.INSERTAR)
                {
                    return ((DataTable)Session["DtRegFuncionario"]).Rows[intSeleccionado];
                }
                else
                {
                    return ((DataTable)Session[strVariableDtFunc]).Rows[intSeleccionado];
                }
            }

            return null;
        }

        private void PintarSeleccionadoFuncinarioDet()
        {
            if (Session != null)
            {
                Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

                DataRow drSeleccionado = ObtenerFilaSeleccionadaFuncinarioDet();
                if (drSeleccionado != null)
                {
                    if (enmAccion == Enumerador.enmAccion.INSERTAR)
                    {

                    }
                    else
                    {
                        Session["OfiConFuncionarioId"] = drSeleccionado["ocfu_sOfiConFuncionarioId"].ToString();
                    }

                    updMantenimiento.Update();
                }
            }
        }

        private void BindGrid(int IntCategoriaId, string StrvNombre, string IntContinente, string strPais)
        {
            DataTable DtOficinaConsular = new DataTable();

            Proceso MiProc = new Proceso();

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_CANT_REGISTRO;

            string strEstado = "";

            if (chkRolActivo.Checked)
            {
                strEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
            }
            else
            {
                strEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
            }
            //-------------------------------------------------
            //Fecha: 29/04/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Adicionar los parametros:
            //        bJefatura y bElecciones.
            //-------------------------------------------------
            bool bJefatura = false;
            bool bElecciones = false;

            if (chkSelEsJefatura.Checked)
                bJefatura = true;

            if (chkSelElecciones.Checked)
                bElecciones = true;

            OficinaConsularConsultasBL BL = new OficinaConsularConsultasBL();

            short iContinente = Convert.ToInt16(IntContinente);

            if (iContinente > 0 && iContinente < 90)
            {
                strPais = "";
            }


            DtOficinaConsular = BL.Consultar(IntCategoriaId,
                                             StrvNombre,
                                             IntContinente,
                                             strPais,
                                             ctrlPaginador.PaginaActual.ToString(),
                                             intPaginaCantidad,
                                             ref IntTotalCount,
                                             ref IntTotalPages,
                                             strEstado,
                                             bJefatura,
                                             bElecciones);

            Session[strVariableDt] = DtOficinaConsular;

            if (DtOficinaConsular.Rows.Count > 0)
            {
                gdvOficinaConsular.SelectedIndex = -1;
                gdvOficinaConsular.DataSource = DtOficinaConsular;
                gdvOficinaConsular.DataBind();

                ctrlPaginador.TotalResgistros = IntTotalCount;
                ctrlPaginador.TotalPaginas = IntTotalPages;

                ctrlPaginador.Visible = false;
                if (ctrlPaginador.TotalResgistros > intPaginaCantidad)
                {
                    ctrlPaginador.Visible = true;
                }

                ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + Convert.ToInt32(IntTotalCount), true, Enumerador.enmTipoMensaje.INFORMATION);
            }
            else
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                ctrlPaginador.Visible = false;
                ctrlPaginador.PaginaActual = 1;
                ctrlPaginador.InicializarPaginador();

                gdvOficinaConsular.DataSource = null;
                gdvOficinaConsular.DataBind();
            }

            updGrillaConsulta.Update();
        }

        private void BindGridOficinaDependiente(int IntOficinaConsularId)
        {
            DataTable DtOficCH = new DataTable();

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_CANT_REGISTRO;

            OficinaConsularConsultasBL BL = new OficinaConsularConsultasBL();

            DtOficCH = BL.ObtenerDependientes(Convert.ToInt16(IntOficinaConsularId),
                                             ctrlPaginadorDep.PaginaActual.ToString(),
                                             intPaginaCantidad,
                                             ref IntTotalCount,
                                             ref IntTotalPages);

            Session[strVariableDtOFCH] = DtOficCH;

            if (DtOficCH.Rows.Count > 0)
            {
                gdvDependientes.SelectedIndex = -1;
                gdvDependientes.DataSource = DtOficCH;
                gdvDependientes.DataBind();

                ctrlPaginadorDep.TotalResgistros = IntTotalCount;
                ctrlPaginadorDep.TotalPaginas = IntTotalPages;

                ctrlPaginadorDep.Visible = false;
                if (ctrlPaginadorDep.TotalResgistros > intPaginaCantidad)
                {
                    ctrlPaginadorDep.Visible = true;
                }
            }
            else
            {
                ctrlPaginadorDep.Visible = false;
                ctrlPaginadorDep.PaginaActual = 1;
                ctrlPaginadorDep.InicializarPaginador();

                gdvDependientes.DataSource = null;
                gdvDependientes.DataBind();


                DataTable dt = FillEmptyDatagdvDependientes();
                gdvDependientes.DataSource = dt;
                gdvDependientes.DataBind();
                Session[strVariableDtOFCH] = dt;
            }

            updMantenimiento.Update();
        }

        private void BindGridFuncionario(int IntOficinaConsularId)
        {
            DataTable DtOficinaConsulFunc = new DataTable();
            OficinaConsularFuncConsultasBL BL = new OficinaConsularFuncConsultasBL();

            DtOficinaConsulFunc = BL.Obtener(Convert.ToInt16(IntOficinaConsularId));

            Session[strVariableDtFunc] = DtOficinaConsulFunc;
            Session[strVariableDtFunc_tmp] = DtOficinaConsulFunc;

            if (DtOficinaConsulFunc.Rows.Count > 0)
            {
                gdvFuncionario.SelectedIndex = -1;
                gdvFuncionario.DataSource = DtOficinaConsulFunc;
                gdvFuncionario.DataBind();
            }
            else
            {
                gdvFuncionario.DataSource = null;
                gdvFuncionario.DataBind();

                Session.Remove("DtRegFuncionario");
                Session["DtRegFuncionario"] = CrearDtRegFuncionario();
                ((DataTable)Session["DtRegFuncionario"]).Clear();

                gdvFuncionario.DataSource = CrearDtRegFuncionario();
                gdvFuncionario.DataBind();
            }

            updMantenimiento.Update();
        }

        private void GrabarHandler()
        {
            string strScript = string.Empty;

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

            OficinaConsularMantenimientoBL BL = new OficinaConsularMantenimientoBL();
            bool Error = true;

            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:
                    BL.Insert(ObtenerEntidadMantenimiento(), ObtenerListadoEntidadDetalle(), ref Error);
                    break;
                case Enumerador.enmAccion.MODIFICAR:
                    BL.Update(ObtenerEntidadMantenimiento(), ObtenerListadoEntidadDetalle(), ObtenerOficinaDependiente(), ref Error);
                    break;
                case Enumerador.enmAccion.ELIMINAR:
                    BL.Delete(ObtenerEntidadMantenimiento(), ref Error);
                    break;
            }

            if (!Error)
            {
                if (enmAccion == Enumerador.enmAccion.ELIMINAR)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO_ANULAR);
                }
                else
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO);
                }

                HabilitarMantenimiento();
                LimpiarDatosMantenimiento();

                Session["Grabo"] = "SI";

                Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;

                strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                strScript += Util.HabilitarTab(0);

                int IntCategoria = Convert.ToInt16(ddlCategoriaMant.SelectedValue);
                Session[strVariableDt] = new DataTable();
                BindGrid(IntCategoria, null, ddlContinente.SelectedItem.Value, ddlPais.SelectedItem.Value);

                updConsulta.Update();
                updMantenimiento.Update();
            }
            else if (Error)
            {
                Session["Grabo"] = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Error de Sistema");
            }

            Comun.EjecutarScript(Page, strScript);
        }

        private DataTable CrearDtRegFuncionario()
        {
            DataTable DtRegFuncionario = new DataTable();
            DtRegFuncionario.Columns.Clear();

            DataColumn dcOfiConFuncionarioId = DtRegFuncionario.Columns.Add("ocfu_sOfiConFuncionarioId", typeof(int));
            dcOfiConFuncionarioId.AllowDBNull = true;
            dcOfiConFuncionarioId.Unique = false;

            DataColumn dcOficinaConsularId = DtRegFuncionario.Columns.Add("ocfu_sOficinaConsularId", typeof(int));
            dcOficinaConsularId.AllowDBNull = true;
            dcOficinaConsularId.Unique = false;

            DataColumn dcRequisitoId = DtRegFuncionario.Columns.Add("IFuncionarioId", typeof(int));
            dcRequisitoId.AllowDBNull = true;
            dcRequisitoId.Unique = false;

            DataColumn dcvFuncionario = DtRegFuncionario.Columns.Add("ocfu_vNombreFuncionario", typeof(string));
            DataColumn dcvApellidoPaterno = DtRegFuncionario.Columns.Add("ocfu_vApellidoPaternoFuncionario", typeof(string));
            DataColumn dcvApellidoMaterno = DtRegFuncionario.Columns.Add("ocfu_vApellidoMaternoFuncionario", typeof(string));

            DataColumn dcsGenero = DtRegFuncionario.Columns.Add("sGenero", typeof(int));

            return DtRegFuncionario;
        }

        public DataTable FillEmptyDatagdvDependientes()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Nombre", typeof(string));
            return dt;
        }

        #endregion

        protected void btnBuscarFunc_Click(object sender, EventArgs e)
        {
            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];
            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "FUNCIONARIO", "DEBE DE CREAR PRIMERO UNA OFICINA CONSULAR PARA PODER AGREGAR UN FUNCIONARIO"));

                    break;
                case Enumerador.enmAccion.MODIFICAR:
                case Enumerador.enmAccion.ELIMINAR:
                    Comun.EjecutarScript(this, "showModalPopup('../Configuracion/FrmBuscarFuncionario.aspx','BUSCAR FUNCIONARIO',550, 1000, '" + btnEjecutarFuncionario.ClientID + "');");
                    break;
                default:
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "FUNCIONARIO", "DEBE DE CREAR PRIMERO UNA OFICINA CONSULAR PARA PODER AGREGAR UN FUNCIONARIO"));
                    break;
            }


        }
        protected void btnEjecutarFuncionario_Click(object sender, EventArgs e)
        {
            object obj = Session[strIdFuncionario];
            object objNombre = Session[strNombFuncionario];
            if (obj != null)
            {
                if (Convert.ToInt32(obj) == -1 || objNombre == null)
                    return;

                int intFuncionarioId = Convert.ToInt32(Session[strIdFuncionario]);
                string NombresFuncionario = Session[strNombFuncionario].ToString();
                string strNombres = Session["ocfu_vNombreFuncionario"].ToString();
                string strPrimerApellido = Session["ocfu_vApellidoPaternoFuncionario"].ToString();
                string strSegundoApellido = Session["ocfu_vApellidoMaternoFuncionario"].ToString();
                string strFuncionarioDoc = Session["Funcionario_Documento"].ToString();
                string strFuncionarioCargo = Session["Funcionario_Cargo"].ToString();
                int intFuncionariosGenero = Convert.ToInt32(Session["Funcionario_sGenero"].ToString());

                Session.Remove(strIdFuncionario);
                Session.Remove("ocfu_vNombreFuncionario");
                Session.Remove("ocfu_vApellidoPaternoFuncionario");
                Session.Remove("ocfu_vApellidoMaternoFuncionario");
                Session.Remove("Funcionario_Documento");
                Session.Remove("Funcionario_Cargo");
                Session.Remove("Funcionario_sGenero");

                DataTable dt = (DataTable)Session[strVariableDtFunc];
                DataTable dtTemp = ((DataTable)Session[strVariableDtFunc_tmp]).Copy();
                DataRow dr;
                if (dt == null)
                {
                    dt = CrearDtRegFuncionario();
                }

                int intTarifarioRequisitoSel = Convert.ToInt32(Session[strVariableIndiceFunc]);
                if (intTarifarioRequisitoSel == -1)
                {
                    dr = dt.NewRow();
                    dr["ocfu_sOfiConFuncionarioId"] = 0;
                    dr["ocfu_sOficinaConsularId"] = hdn_sOficinaConsularId.Value;
                    dr["IFuncionarioId"] = intFuncionarioId;
                    dr["IDENTIFICADOR"] = intFuncionarioId;

                    dr["sDocumentoFuncionario"] = strFuncionarioDoc;

                    dr["NOMBRE_FUNCIONARIO"] = NombresFuncionario;
                    dr["ocfu_vNombreFuncionario"] = strNombres;
                    dr["ocfu_vApellidoPaternoFuncionario"] = strPrimerApellido;
                    dr["ocfu_vApellidoMaternoFuncionario"] = strSegundoApellido;

                    dr["CARGO"] = strFuncionarioCargo;
                    dr["ocfu_cEstado"] = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                    dr["sGenero"] = intFuncionariosGenero.ToString();

                }
                else
                {
                    dr = dt.NewRow();
                    dr["ocfu_sOfiConFuncionarioId"] = 0;
                    dr["ocfu_sOficinaConsularId"] = hdn_sOficinaConsularId.Value;
                    dr["IFuncionarioId"] = intFuncionarioId;
                    dr["IDENTIFICADOR"] = intFuncionarioId;

                    dr["sDocumentoFuncionario"] = strFuncionarioDoc;

                    dr["NOMBRE_FUNCIONARIO"] = NombresFuncionario;
                    dr["ocfu_vNombreFuncionario"] = strNombres;
                    dr["ocfu_vApellidoPaternoFuncionario"] = strPrimerApellido;
                    dr["ocfu_vApellidoMaternoFuncionario"] = strSegundoApellido;

                    dr["CARGO"] = strFuncionarioCargo;
                    dr["ocfu_cEstado"] = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                    dr["sGenero"] = intFuncionariosGenero.ToString();
                }
                dt.Rows.Add(dr);
                dtTemp.Rows.Add(dr.ItemArray);

                gdvFuncionario.DataSource = dt;
                gdvFuncionario.DataBind();
                Session[strVariableDtFunc] = dt;
                Session[strVariableIndiceFunc] = -1;

                Session[strVariableDtFunc_tmp] = dtTemp;
            }
        }

        protected void ddlContinente_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.ddlPais.Items.Clear();
                if (ddlContinente.SelectedIndex > 0)
                {
                    //Util.CargarDropDownList(ddlPais,
                    //    comun_Part3.ObtenerPaisProv(Session, ddlContinente.SelectedValue, true),
                    //        "ubge_vProvincia", "ubge_cUbi02", true);

                    if (ViewState["Ubigeo"] != null)
                    {
                        beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                        obeUbigeoListas = (beUbigeoListas)ViewState["Ubigeo"];
                        List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                        lbeUbicaciongeografica = Comun.obtenerListaUbiGeo("02", ddlContinente.SelectedValue, "", obeUbigeoListas.Ubigeo02);
                        lbeUbicaciongeografica.Insert(0, new beUbicaciongeografica { Ubi02 = "00", Provincia = "-- SELECCIONE --" });
                        ddlPais.DataSource = lbeUbicaciongeografica;
                        ddlPais.DataValueField = "Ubi02";
                        ddlPais.DataTextField = "Provincia";
                        ddlPais.DataBind();

                        ddlPais.Enabled = true;
                    }
                    this.ddlDistrito.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));

                    ScriptManager.GetCurrent(Page).SetFocus(ddlContinente.ClientID);
                }
                else
                {
                    Util.CargarParametroDropDownList(ddlPais, new DataTable(), true);
                }

            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The operation is not valid for the state") || ex.ToString().Contains("La operación no es válida para el estado"))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", "HTTP 404 - intentelo de nuevo", true);
                }
                else
                {
                    Session["_LastException"] = ex;
                    Response.Redirect("../PageError/GenericErrorPage.aspx");
                }
            }
        }

        protected void btnCajaChica_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(hdn_sOficinaConsularId.Value) == Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]))
            {
                if (Convert.ToInt16(Session[strVariableAccion]) == Convert.ToInt16(Enumerador.enmAccion.MODIFICAR))
                    Comun.EjecutarScript(this, "showModalPopup('../Configuracion/FrmCajaChica.aspx','CAJA CHICA',100, 600, '" + btnEjecutarFuncionario.ClientID + "');");
                else
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Caja Chica", "Solo puede acceder en modo edición."));
            }
            else
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Caja Chica", "La oficina consular seleccionada no es la Oficina Consular origen."));
            }
        }

        protected void btnActualizarCargo_Click(object sender, EventArgs e)
        {
            OficinaConsularFuncMantenimientoBL BLc = new OficinaConsularFuncMantenimientoBL();
            Int16 Cargo = 0;

            Cargo = Convert.ToInt16(ddlCargos.SelectedValue);
            bool Error = true;
            if (Cargo == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGOS", "Debe seleccionar un cargo."));
            }
            else
            {
                BLc.ActualizarCargo(Convert.ToInt16(hCodID.Value), Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]), Session[Constantes.CONST_SESION_DIRECCION_IP].ToString(), Cargo, Convert.ToInt16(ddlSexo.SelectedValue), ref Error);

                if (Error)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGOS", "Ha ocurrido un error."));
                }
                else
                {
                    BindGridFuncionario(Convert.ToInt16(Session["Ofco_ConsularId"]));
                }
                ocultarFuncionario.Visible = false;
            }

        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            registrarMoneda();
        }

        private void registrarMoneda()
        {
            OficinaConsularMantenimientoBL _obj = new OficinaConsularMantenimientoBL();
            Int16 oficinaMoneda = Convert.ToInt16(hidEditar.Value);
            Int16 oficinaConsular = Convert.ToInt16(Session["Ofco_ConsularId"]);
            Int16 Moneda = Convert.ToInt16(ddlMonedaPopup.SelectedValue);
            string resultado = "";
            bool error = false;

            // AQUI SE EVALUA LAS FECHAS PORQUE SON REQUERIDAS
            if (ctrFechaInicio.Text == "")
            {
                ctrlValidacionRegistro.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.ERROR);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "Popup();", true);
                updMantenimiento.Update(); return;
            }

            if (ctrFechaFin.Text == "")
            {
                ctrlValidacionRegistro.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.ERROR);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "Popup();", true);
                updMantenimiento.Update(); return;
            }

            if (Comun.FormatearFecha(ctrFechaFin.Text) < Comun.FormatearFecha(ctrFechaInicio.Text))
            {
                ctrlValidacionRegistro.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "Popup();", true);
                updMantenimiento.Update(); return;
            }
            try
            {
                _obj.RegistrarMoneda(oficinaMoneda, oficinaConsular, Convert.ToInt16(ddlMonedaPopup.SelectedValue), Comun.FormatearFecha(ctrFechaInicio.Text), Comun.FormatearFecha(ctrFechaFin.Text), Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]), Session[Constantes.CONST_SESION_DIRECCION_IP].ToString(), ref resultado, ref error);
                if (!error)
                {
                    if (resultado != "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "alert('" + resultado + "');", true);
                        LimpiarPopupMoneda();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "Popup();", true);
                    }
                    else
                    {
                        ddlMoneda.SelectedValue = ddlMonedaPopup.SelectedValue;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Mensaje", "alert('Se registro correctamente');", true);
                        CargarMonedas();
                        LimpiarPopupMoneda();
                        hidEditar.Value = "0";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "Popup();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
            }
        }
        protected void gdvMoneda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int intSeleccionadoDet = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EliminarMoneda")
            {
                try {
                    Int16 intIDOficinaMoneda;
                    bool error = false;
                    GridViewRow gdvMoneda;
                    gdvMoneda = (GridViewRow)((TableCell)((ImageButton)e.CommandSource).Parent).Parent;

                    Label lblIDOficinaMoneda = (Label)gdvMoneda.FindControl("lblIDOficinaMoneda");

                    string _vigente = Convert.ToString(gdvMonedaPop.Rows[intSeleccionadoDet].Cells[Util.ObtenerIndiceColumnaGrilla(gdvMonedaPop, "Vigente")].Text);
                    if (_vigente == "1")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Mensaje", "alert('No se puede eliminar la moneda vigente');", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "Popup();", true);
                        return;
                    }

                    intIDOficinaMoneda = Convert.ToInt16(lblIDOficinaMoneda.Text);
                    OficinaConsularMantenimientoBL _obj = new OficinaConsularMantenimientoBL();
                    _obj.EliminarMoneda(intIDOficinaMoneda, Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]), Session[Constantes.CONST_SESION_DIRECCION_IP].ToString(), ref error);
                    if (!error)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Mensaje", "alert('Se elimino correctamente');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Mensaje", "alert('Ha ocurrido un error inesperado');", true);
                    }
                    CargarMonedas();
                    LimpiarPopupMoneda();
                    hidEditar.Value = "0";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "Popup();", true);
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
                }
                
                //AnularFichaParticipante(strTipoParticipanteId);
            }
            if (e.CommandName == "EditarMoneda")
            {
                try
                {
                    GridViewRow gdvMoneda;
                    gdvMoneda = (GridViewRow)((TableCell)((ImageButton)e.CommandSource).Parent).Parent;

                    Label lblMoneda = (Label)gdvMoneda.FindControl("lblMoneda");
                    Label lblFechaInicio = (Label)gdvMoneda.FindControl("lblFechaInicio");
                    Label lblFechaFin = (Label)gdvMoneda.FindControl("lblFechaFin");
                    Label lblIDOficinaMoneda = (Label)gdvMoneda.FindControl("lblIDOficinaMoneda");
                    ddlMonedaPopup.SelectedValue = ddlMonedaPopup.Items.FindByText(lblMoneda.Text).Value;
                    //ctrFechaInicio.Text = lblFechaInicio.Text;
                    //ctrFechaFin.Text = lblFechaFin.Text;

                    DateTime datFechaInicio = new DateTime();
                    if (!DateTime.TryParse(lblFechaInicio.Text, out datFechaInicio))
                    {
                        datFechaInicio = Comun.FormatearFecha(lblFechaInicio.Text);
                    }
                    ctrFechaInicio.Text = datFechaInicio.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);

                    DateTime datFechaFin = new DateTime();
                    if (!DateTime.TryParse(lblFechaFin.Text, out datFechaFin))
                    {
                        datFechaFin = Comun.FormatearFecha(lblFechaFin.Text);
                    }
                    ctrFechaFin.Text = datFechaFin.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                    hidEditar.Value = lblIDOficinaMoneda.Text;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "Popup();", true);
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
                }
            }
        }

        protected void gdvMoneda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string _estado = DataBinder.Eval(e.Row.DataItem, "Vigente").ToString();

                if (_estado == "1")
                {
                    e.Row.BackColor = System.Drawing.Color.LightBlue;
                }
            }

        }
    }
}
