using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.BE.MRE;
using SGAC.Configuracion.Maestro.BL;
using SGAC.Accesorios;
using SGAC.Controlador;
using System.Data;

namespace SGAC.WebApp.Configuracion
{
    public partial class FrmTablasMaestras : MyBasePage
	{
        #region CAMPOS
        private string strNombreEntidad = "TABLA MAESTRA";
        private string strVariableAccion = "TablaMaestra_Accion";             
        #endregion

        #region Eventos
        private void Page_Init(object sender, EventArgs e)
        {            
            ctrlPaginador.Visible = false;
        }

		protected void Page_Load(object sender, EventArgs e)
		{
            string StrScript = string.Empty;

            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            lblUserName.Text = Util.GetSessionVariableValue(Session, Constantes.CONST_SESION_USUARIO);

            ctrlToolBarConsulta.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);
            ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);

            ctrlToolBarConsulta.VisibleIButtonBuscar = true;
            ctrlToolBarConsulta.VisibleIButtonCancelar = true;
            ctrlToolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarConsulta.btnCancelar.Text = "    Limpiar";

            ctrlToolBarMantenimiento.btnNuevoHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonNuevoClick(ctrlToolBarMantenimiento_btnNuevoHandler);
            ctrlToolBarMantenimiento.btnEditarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEditarClick(ctrlToolBarMantenimiento_btnEditarHandler);
            ctrlToolBarMantenimiento.btnEliminarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEliminarClick(ctrlToolBarMantenimiento_btnEliminarHandler);
            ctrlToolBarMantenimiento.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBarMantenimiento_btnGrabarHandler);
            ctrlToolBarMantenimiento.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarMantenimiento_btnCancelarHandler);

            ctrlToolBarMantenimiento.VisibleIButtonNuevo = true;
            ctrlToolBarMantenimiento.VisibleIButtonEditar = true;
            ctrlToolBarMantenimiento.VisibleIButtonEliminar = true;
            ctrlToolBarMantenimiento.VisibleIButtonGrabar = true;
            ctrlToolBarMantenimiento.VisibleIButtonCancelar = true;

            ctrlToolBarMantenimiento.btnGrabar.OnClientClick = "return Validar();";

            string eventTarget = Request["__EVENTTARGET"] ?? string.Empty;
            if (eventTarget == "GrabarHandler")
            {
                if (Session["Grabo"].ToString().Equals("NO"))
                {
                    if (ddl_TablaRegistro.SelectedItem.Text == "MA_ETIQUETA")
                    {
                        GrabarEtiqueta();
                    }
                    else
                    {
                        GrabarHandler();
                    }
                }
                else
                {
                    if (ddl_TablaBusqueda.SelectedIndex > 0)
                    {
                        if (ddl_TablaBusqueda.SelectedItem.Text == "MA_ETIQUETA")
                        {
                            BindGridEtiquetas();
                        }
                        else
                        {
                            BindGridTablasMaestras(Convert.ToInt32(ddl_TablaBusqueda.SelectedValue));
                        }
                    }
                }
            }

            if (!Page.IsPostBack)
            {
                Session["Grabo"] = "NO";
                Util.CargarDropDownList(ddl_TablaBusqueda, comun_Part1.ObtenerTablasMaestras(Session), "descripcion", "id", true, " - SELECCIONAR - ");
                Util.CargarDropDownList(ddl_TablaRegistro, comun_Part1.ObtenerTablasMaestras(Session), "descripcion", "id", true, " - SELECCIONAR - ");                
                CargarDatosIniciales();

            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo, ctrlToolBarMantenimiento.btnEditar, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar };
                GridView[] arrGridView = { Grd_Tablas };
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }
        }

        protected void ddl_TablaBusqueda_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_TablaBusqueda.SelectedValue == "0")
            {
                ctrlToolBarConsulta_btnCancelarHandler();
            }
            if (ddl_TablaBusqueda.SelectedItem.Text == "MA_ETIQUETA")
            {
                DataTable dtPlantilla = new DataTable();
                dtPlantilla = comun_Part1.ObtenerParametrosPorGrupo(Session, "ACTUACIÓN-TIPO PLANTILLA");
                Util.CargarDropDownList(ddl_ConsultaPlantilla, dtPlantilla, "descripcion", "id", true, " - TODOS - ");
                Util.CargarDropDownList(ddl_Plantilla, dtPlantilla, "descripcion", "id", true, " - SELECCIONAR - ");

                BusquedaPlantilla.Visible = true;
            }
            else
            {
                BusquedaPlantilla.Visible = false;
            }
            updConsulta.Update();
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            if (ddl_TablaBusqueda.SelectedValue == "0")
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_NO_SELECCION_FILTROS, true, Enumerador.enmTipoMensaje.WARNING);
            }
            else
            {
                ctrlPaginador.InicializarPaginador();

                if (ddl_TablaBusqueda.SelectedItem.Text == "MA_ETIQUETA")
                {
                    BindGridEtiquetas();
                }
                else
                {
                    BindGridTablasMaestras(Convert.ToInt32(ddl_TablaBusqueda.SelectedValue));
                }
            }
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            chkActivo.Checked = true;
            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();
            Comun.EjecutarScript(Page, Util.ActivarTab(0,Constantes.CONST_TAB_INICIAL) + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }

        protected void ddl_TablaRegistro_SelectedIndexChanged(object sender, EventArgs e)
        {
            SeteaCamposTablaMaestra(Convert.ToInt32(ddl_TablaRegistro.SelectedValue));
            //------------------------------------------------
            if (ddl_TablaRegistro.SelectedItem.Text == "MA_ETIQUETA")
            {
                DataTable dtPlantilla = new DataTable();
                dtPlantilla = comun_Part1.ObtenerParametrosPorGrupo(Session, "ACTUACIÓN-TIPO PLANTILLA");                
                Util.CargarDropDownList(ddl_Plantilla, dtPlantilla, "descripcion", "id", true, " - SELECCIONAR - ");

                CampoPlantilla.Visible = true;
                CampoCodigo.Visible = false;
                CampoGrupo.Visible = false;
                CampoNombre.Visible = false;
                CampoDescripcion.Visible = false;
                CampoSimbolo.Visible = false;
            }
            else
            {
                CampoPlantilla.Visible = false;
            }
            //------------------------------------------------
            hidTabla.Value = ddl_TablaRegistro.SelectedValue;
            HF_Tabla.Value = ddl_TablaRegistro.SelectedItem.Text;
            updMantenimiento.Update();
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
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;            
            HabilitarMantenimiento();
            ddl_TablaRegistro.Enabled = false;

            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_EDITAR));
        }

        void ctrlToolBarMantenimiento_btnEliminarHandler()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.ELIMINAR;
            ctrlToolBarMantenimiento_btnGrabarHandler();
        }       

        void ctrlToolBarMantenimiento_btnGrabarHandler()
        {                       
            string StrScript = string.Empty;

            int IntRpta = 0;

            TablaMaestraConsultaBL BL = new TablaMaestraConsultaBL();
            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

            if (enmAccion == Enumerador.enmAccion.INSERTAR)
            {
                if (ddl_TablaRegistro.SelectedItem.Text != "MA_ETIQUETA")
                {
                    IntRpta = BL.Existe(Convert.ToInt32(ddl_TablaRegistro.SelectedValue),
                                        1,
                                        0,
                                        txtCodigo.Text.Trim());

                    if (Convert.ToInt32(ddl_TablaRegistro.SelectedValue) == (int)Enumerador.enmTabla.MA_OCUPACION)
                    {
                        if (IntRpta == 1)
                        {
                            StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", "Ya existe el codigo de la ocupación esta consignando.", false, 190, 250);
                            Comun.EjecutarScript(Page, StrScript);
                            return;
                        }
                    }
                    else if (Convert.ToInt32(ddl_TablaRegistro.SelectedValue) == (int)Enumerador.enmTabla.MA_PROFESION)
                    {
                        if (IntRpta == 1)
                        {
                            StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", "Ya existe el codigo de la profesión esta consignando.", false, 190, 250);
                            Comun.EjecutarScript(Page, StrScript);
                            return;
                        }
                    }
                }
                else
                {
                    //---------------------------------------------------
                    //Verificar duplicidad en la tabla: MA_ETIQUETA
                    //---------------------------------------------------
                    EtiquetaConsultaBL EtiquetaBL = new EtiquetaConsultaBL();
                    Int16 intPlantillaId = Convert.ToInt16(ddl_Plantilla.SelectedValue);
                    DataTable dtEtiquetas = new DataTable();
                    int IntTotalPages = 0;
                    int IntTotalCount = 0;

                    dtEtiquetas = EtiquetaBL.Consultar(0, intPlantillaId, txtEtiqueta.Text.Trim(), "A", 1, 1, "N", ref IntTotalPages, ref IntTotalCount);
                    if (dtEtiquetas.Rows.Count > 0)
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Tablas Maestras", "Ya existe la Etiqueta registrada en esta Plantilla.", false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);
                        return;
                    }
                    //---------------------------------------------------
                }
            }
            else if (enmAccion == Enumerador.enmAccion.MODIFICAR)
            {
                if (ddl_TablaRegistro.SelectedItem.Text != "MA_ETIQUETA")
                {
                    IntRpta = BL.Existe(Convert.ToInt32(ddl_TablaRegistro.SelectedValue),
                                        2,
                                        Convert.ToInt16(Session["IntRegistroId"]),
                                        txtCodigo.Text.Trim());

                    if (Convert.ToInt32(ddl_TablaRegistro.SelectedValue) == (int)Enumerador.enmTabla.MA_OCUPACION)
                    {
                        if (IntRpta == 1)
                        {
                            StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Tablas Maestras", "Ya existe el codigo de la ocupación esta consignando.", false, 190, 250);
                            Comun.EjecutarScript(Page, StrScript);
                            return;
                        }
                    }
                    else if (Convert.ToInt32(ddl_TablaRegistro.SelectedValue) == (int)Enumerador.enmTabla.MA_PROFESION)
                    {
                        if (IntRpta == 1)
                        {
                            StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Tablas Maestras", "Ya existe el codigo de la profesión esta consignando.", false, 190, 250);
                            Comun.EjecutarScript(Page, StrScript);
                            return;
                        }
                    }
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
            Comun.EjecutarScript(Page, Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR) + Util.HabilitarTab(0));
        }        

        protected void Grd_Tablas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strScript = string.Empty;

            int IntIndex = Convert.ToInt32(e.CommandArgument);

            Session["IntTablaId"] = Grd_Tablas.Rows[IntIndex].Cells[0].Text;
            ddl_TablaRegistro.SelectedValue = Grd_Tablas.Rows[IntIndex].Cells[0].Text;
            Session["IntRegistroId"] = Grd_Tablas.Rows[IntIndex].Cells[2].Text;

            if (e.CommandName == "Consultar")
            {
                Session[strVariableAccion] = Enumerador.enmAccion.CONSULTAR;

                ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
                ctrlToolBarMantenimiento.btnEditar.Enabled = true;
                ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
                
                if (Convert.ToInt32(Session["IntTablaId"]) == (int)Enumerador.enmTabla.MA_ESTADO)
                {
                    ddl_Grupo.Items.Clear();
                    FillWebCombo(SGAC.WebApp.Accesorios.SysTables.TraerSysTable("", "GrupoEstado"), ddl_Grupo, CultureDescription(), "Valor");
                    foreach (ListItem item in this.ddl_Grupo.Items)
                    {
                        if (item.Text == Convert.ToString(Page.Server.HtmlDecode(Grd_Tablas.Rows[IntIndex].Cells[1].Text)))
                        {
                            ddl_Grupo.SelectedValue = item.Value;
                        }
                    }          
                }
                if (Convert.ToInt32(Session["IntTablaId"]) == (int)Enumerador.enmTabla.MA_SERVICIO)
                {
                    ddl_Grupo.Items.Clear();
                    FillWebCombo(SGAC.WebApp.Accesorios.SysTables.TraerSysTable("", "GrupoServicio"), ddl_Grupo, CultureDescription(), "Valor");
                    foreach (ListItem item in this.ddl_Grupo.Items)
                    {
                        if (item.Text == Convert.ToString(Page.Server.HtmlDecode(Grd_Tablas.Rows[IntIndex].Cells[1].Text)))
                        {
                            ddl_Grupo.SelectedValue = item.Value;
                        }
                    }          
                }

                if (ddl_TablaBusqueda.SelectedItem.Text == "MA_ETIQUETA")
                {
                    ddl_Plantilla.SelectedValue = Convert.ToString(Page.Server.HtmlDecode(Grd_Tablas.Rows[IntIndex].Cells[6].Text));
                    txtNro.Text = Convert.ToString(Page.Server.HtmlDecode(Grd_Tablas.Rows[IntIndex].Cells[5].Text));
                    txtEtiqueta.Text = Convert.ToString(Page.Server.HtmlDecode(Grd_Tablas.Rows[IntIndex].Cells[4].Text));
                }
                else
                {
                    txtDescripcion.Text = Convert.ToString(Page.Server.HtmlDecode(Grd_Tablas.Rows[IntIndex].Cells[3].Text));
                    txtNombre.Text = Convert.ToString(Page.Server.HtmlDecode(Grd_Tablas.Rows[IntIndex].Cells[4].Text));
                    txtSimbolo.Text = Convert.ToString(Page.Server.HtmlDecode(Grd_Tablas.Rows[IntIndex].Cells[5].Text));
                    txtCodigo.Text = Convert.ToString(Page.Server.HtmlDecode(Grd_Tablas.Rows[IntIndex].Cells[6].Text));
                }               

                string strEstado = Convert.ToString(Page.Server.HtmlDecode(Grd_Tablas.Rows[IntIndex].Cells[7].Text)).Trim();

                if (strEstado == ((char)Enumerador.enmEstado.ACTIVO).ToString())
                {
                    chkActivoMant.Checked = true;
                }
                else
                {
                    chkActivoMant.Checked = false;
                }

                SeteaCamposTablaMaestra(Convert.ToInt32(Grd_Tablas.Rows[IntIndex].Cells[0].Text));
                //------------------------------------------------
                if (ddl_TablaBusqueda.SelectedItem.Text == "MA_ETIQUETA")
                {
                    CampoPlantilla.Visible = true;
                    CampoCodigo.Visible = false;
                    CampoGrupo.Visible = false;
                    CampoNombre.Visible = false;
                    CampoDescripcion.Visible = false;
                    CampoSimbolo.Visible = false;
                }
                //------------------------------------------------
                HabilitarMantenimiento(false);
                updMantenimiento.Update();
                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
            }
            else if (e.CommandName == "Editar")
            {
                Session[strVariableAccion] = Enumerador.enmAccion.MODIFICAR;

                ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
                ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                ctrlToolBarMantenimiento.btnEliminar.Enabled = true;
                
                if (Convert.ToInt32(Session["IntTablaId"]) == (int)Enumerador.enmTabla.MA_ESTADO)
                {
                    ddl_Grupo.Items.Clear();
                    FillWebCombo(SGAC.WebApp.Accesorios.SysTables.TraerSysTable("", "GrupoEstado"), ddl_Grupo, CultureDescription(), "Valor");
                    foreach (ListItem item in this.ddl_Grupo.Items)
                    {
                        if (item.Text == Convert.ToString(Page.Server.HtmlDecode(Grd_Tablas.Rows[IntIndex].Cells[1].Text)))
                        { 
                            ddl_Grupo.SelectedValue = item.Value;
                        }
                    }                    
                }
                if (Convert.ToInt32(Session["IntTablaId"]) == (int)Enumerador.enmTabla.MA_SERVICIO)
                {
                    ddl_Grupo.Items.Clear();                    
                    FillWebCombo(SGAC.WebApp.Accesorios.SysTables.TraerSysTable("", "GrupoServicio"), ddl_Grupo, CultureDescription(), "Valor");
                    foreach (ListItem item in this.ddl_Grupo.Items)
                    {
                        if (item.Text == Convert.ToString(Page.Server.HtmlDecode(Grd_Tablas.Rows[IntIndex].Cells[1].Text)))
                        {
                            ddl_Grupo.SelectedValue = item.Value;
                        }
                    }    
                }

                if (ddl_TablaBusqueda.SelectedItem.Text == "MA_ETIQUETA")
                {
                    ddl_Plantilla.SelectedValue = Convert.ToString(Page.Server.HtmlDecode(Grd_Tablas.Rows[IntIndex].Cells[6].Text));
                    txtNro.Text = Convert.ToString(Page.Server.HtmlDecode(Grd_Tablas.Rows[IntIndex].Cells[5].Text));
                    txtEtiqueta.Text = Convert.ToString(Page.Server.HtmlDecode(Grd_Tablas.Rows[IntIndex].Cells[4].Text));
                }
                else
                {
                    txtDescripcion.Text = Convert.ToString(Page.Server.HtmlDecode(Grd_Tablas.Rows[IntIndex].Cells[3].Text));
                    txtNombre.Text = Convert.ToString(Page.Server.HtmlDecode(Grd_Tablas.Rows[IntIndex].Cells[4].Text));
                    txtSimbolo.Text = Convert.ToString(Page.Server.HtmlDecode(Grd_Tablas.Rows[IntIndex].Cells[5].Text));
                    txtCodigo.Text = Convert.ToString(Page.Server.HtmlDecode(Grd_Tablas.Rows[IntIndex].Cells[6].Text));
                }
                string strEstado = Convert.ToString(Page.Server.HtmlDecode(Grd_Tablas.Rows[IntIndex].Cells[7].Text)).Trim();

                if (strEstado == ((char)Enumerador.enmEstado.ACTIVO).ToString())
                {
                    chkActivoMant.Checked = true;
                }
                else
                {
                    chkActivoMant.Checked = false;
                }

                HabilitarMantenimiento(true);
                ddl_TablaRegistro.Enabled = false;
                SeteaCamposTablaMaestra(Convert.ToInt32(Grd_Tablas.Rows[IntIndex].Cells[0].Text));
                //------------------------------------------------
                if (ddl_TablaBusqueda.SelectedItem.Text == "MA_ETIQUETA")
                {
                    CampoPlantilla.Visible = true;
                    CampoCodigo.Visible = false;
                    CampoGrupo.Visible = false;
                    CampoNombre.Visible = false;
                    CampoDescripcion.Visible = false;
                    CampoSimbolo.Visible = false;
                }
                //------------------------------------------------
                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);

                updMantenimiento.Update();                
            }

            Comun.EjecutarScript(Page, strScript);
        }        

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            if (ddl_TablaBusqueda.SelectedItem.Text == "MA_ETIQUETA")
            {
                BindGridEtiquetas();
            }
            else
            {
                BindGridTablasMaestras(Convert.ToInt32(ddl_TablaBusqueda.SelectedValue));
            }
        }


        protected void ddl_ConsultaPlantilla_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridEtiquetas();
        }

        #endregion

        #region Métodos
        private void CargarDatosIniciales()
        {            
            Session.Add(strVariableAccion, Enumerador.enmAccion.INSERTAR);
            
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
            ctrlToolBarMantenimiento.btnConfiguration.Enabled = false;

            CampoCodigo.Visible = false;
            CampoGrupo.Visible = false;
            CampoNombre.Visible = false;
            CampoDescripcion.Visible = false;
            CampoSimbolo.Visible = false;
            CampoPlantilla.Visible = false;
            BusquedaPlantilla.Visible = false;
            updConsulta.Update();
            updMantenimiento.Update();
        }              

        private void HabilitarMantenimiento(bool bolHabilitar = true)
        {
            ddl_Grupo.Enabled = bolHabilitar;
            ddl_TablaRegistro.Enabled = bolHabilitar; 
            txtNombre.Enabled = bolHabilitar;           
            txtDescripcion.Enabled = bolHabilitar;
            txtSimbolo.Enabled = bolHabilitar;
            //------------------------------------
            ddl_Plantilla.Enabled = bolHabilitar;
            txtNro.Enabled = bolHabilitar;
            txtEtiqueta.Enabled = bolHabilitar;
            //------------------------------------

            chkActivoMant.Enabled = bolHabilitar;
            txtCodigo.Enabled = false;  
        }

        private void LimpiarDatosMantenimiento()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;

            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            if ((Convert.ToInt32(Session["IntTablaId"]) == (int)Enumerador.enmTabla.MA_ESTADO) || (Convert.ToInt32(Session["IntTablaId"]) == (int)Enumerador.enmTabla.MA_SERVICIO))
            {
                ddl_Grupo.SelectedItem.Text = "";
            }

            ddl_TablaRegistro.SelectedValue = "0";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtSimbolo.Text = "";
            txtCodigo.Text = "";
            //---------------------------
            if (ddl_Plantilla.Items.Count > 0)
            {
                ddl_Plantilla.SelectedIndex = 0;
            }
            txtNro.Text = "";
            txtEtiqueta.Text = "";
            //----------------------------
            chkActivoMant.Checked = true;

            CampoCodigo.Visible = false;
            CampoGrupo.Visible = false;
            CampoNombre.Visible = false;
            CampoDescripcion.Visible = false;
            CampoSimbolo.Visible = false;
            CampoPlantilla.Visible = false;

            ddl_TablaBusqueda.SelectedValue = "0";
            Grd_Tablas.DataSource = null;
            Grd_Tablas.DataBind();
            ctrlPaginador.InicializarPaginador();
            ctrlPaginador.Visible = false;

            updConsulta.Update();
            updMantenimiento.Update();
        }

        private void SeteaCamposTablaMaestra(int IntTablaId)
        {
            if (IntTablaId == (int)Enumerador.enmTabla.MA_CONTINENTE)
            {
                CampoCodigo.Visible = false;
                CampoGrupo.Visible = false;
                CampoNombre.Visible = true;
                CampoDescripcion.Visible = false;
                CampoSimbolo.Visible = false;
            }
            else if (IntTablaId == (int)Enumerador.enmTabla.MA_REQUISITO_ACTUACION)
            {
                CampoCodigo.Visible = false;
                CampoGrupo.Visible = false;
                CampoNombre.Visible = false;
                CampoDescripcion.Visible = true;               
                CampoSimbolo.Visible = false;       
            }
            else if (IntTablaId == (int)Enumerador.enmTabla.MA_ESTADO)
            {
                CampoCodigo.Visible = false;
                CampoGrupo.Visible = true;
                CampoNombre.Visible = true;
                CampoDescripcion.Visible = true;
                CampoSimbolo.Visible = false;
                FillWebCombo(SGAC.WebApp.Accesorios.SysTables.TraerSysTable("", "GrupoEstado"), ddl_Grupo, CultureDescription(), "Valor");
            }
            else if (IntTablaId == (int)Enumerador.enmTabla.MA_SERVICIO)
            {
                CampoCodigo.Visible = false;
                CampoGrupo.Visible = true;
                CampoNombre.Visible = true;
                CampoDescripcion.Visible = true;
                CampoSimbolo.Visible = false;
                FillWebCombo(SGAC.WebApp.Accesorios.SysTables.TraerSysTable("", "GrupoServicio"), ddl_Grupo, CultureDescription(), "Valor");
            }
            else if (IntTablaId == (int)Enumerador.enmTabla.MA_MONEDA)
            {
                CampoCodigo.Visible = false;
                CampoGrupo.Visible = false;
                CampoNombre.Visible = true;
                CampoDescripcion.Visible = true;
                CampoSimbolo.Visible = true;
            }
            else if (IntTablaId == (int)Enumerador.enmTabla.MA_OCUPACION)
            {
                CampoCodigo.Visible = true;
                CampoGrupo.Visible = false;
                CampoNombre.Visible = true;
                CampoDescripcion.Visible = true;
                CampoSimbolo.Visible = false;
            }
            else if (IntTablaId == (int)Enumerador.enmTabla.MA_PROFESION)
            {
                CampoCodigo.Visible = true;
                CampoGrupo.Visible = false;
                CampoNombre.Visible = true;
                CampoDescripcion.Visible = true;
                CampoSimbolo.Visible = false;
            }
            else
            {
                CampoCodigo.Visible = false;
                CampoGrupo.Visible = false;
                CampoNombre.Visible = true;
                CampoDescripcion.Visible = true;
                CampoSimbolo.Visible = false;
            }
        }

        private void MostrarCambiosDatosMantenimiento()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;

            if ((Convert.ToInt32(Session["IntTablaId"]) == (int)Enumerador.enmTabla.MA_ESTADO) || (Convert.ToInt32(Session["IntTablaId"]) == (int)Enumerador.enmTabla.MA_SERVICIO))
            {
                ddl_Grupo.SelectedItem.Text = "";
            }           

            ddl_TablaBusqueda.SelectedValue = ddl_TablaRegistro.SelectedValue;
            BindGridTablasMaestras(Convert.ToInt32(ddl_TablaBusqueda.SelectedValue));

            ddl_TablaRegistro.SelectedValue = "0";
            ddl_TablaRegistro.Enabled = true;
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtSimbolo.Text = "";
            txtCodigo.Text = "";

            CampoCodigo.Visible = false;
            CampoGrupo.Visible = false;
            CampoNombre.Visible = false;
            CampoDescripcion.Visible = false;
            CampoSimbolo.Visible = false;

            string strScript = string.Empty;
            strScript = Util.ActivarTab(0, Constantes.CONST_TAB_INICIAL);
            Comun.EjecutarScript(Page, strScript + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));           
        }

        private void MostrarCambiosDatosMantenimientoEtiquetas()
        {
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            

            ddl_TablaBusqueda.SelectedValue = ddl_TablaRegistro.SelectedValue;
            BindGridEtiquetas();

            ddl_TablaRegistro.SelectedValue = "0";
            ddl_TablaRegistro.Enabled = true;
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtSimbolo.Text = "";
            txtCodigo.Text = "";
            //---------------------------
            ddl_Plantilla.SelectedIndex = 0;
            txtNro.Text = "";
            txtEtiqueta.Text = "";
            //----------------------------
            CampoCodigo.Visible = false;
            CampoGrupo.Visible = false;
            CampoNombre.Visible = false;
            CampoDescripcion.Visible = false;
            CampoSimbolo.Visible = false;
            CampoPlantilla.Visible = false;

            string strScript = string.Empty;
            strScript = Util.ActivarTab(0, Constantes.CONST_TAB_INICIAL);
            Comun.EjecutarScript(Page, strScript + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }

        private void BindGridTablasMaestras(int IntTabla)
        {
            DataTable DtTabla = new DataTable();
            TablaMaestraConsultaBL BL=new TablaMaestraConsultaBL();
            Proceso MiProc = new Proceso();

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_CANT_REGISTRO;

            string strEstado = "";

            if (chkActivo.Checked)
            {
                strEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
            }
            else
            {
                strEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
            }

            Object[] miArray = new Object[6] { IntTabla,                                       
                                               ctrlPaginador.PaginaActual.ToString(),
                                               intPaginaCantidad,
                                               IntTotalCount, 
                                               IntTotalPages,
                                               strEstado};

            DtTabla = BL.Consultar(IntTabla,
                                   ctrlPaginador.PaginaActual.ToString(),
                                   intPaginaCantidad,
                                   ref IntTotalCount,
                                   ref IntTotalPages,
                                   strEstado);

            if (DtTabla.Rows.Count > 0)
            {
                Grd_Tablas.DataSource = DtTabla;
                Grd_Tablas.DataBind();

                ctrlPaginador.TotalResgistros = IntTotalCount;
                ctrlPaginador.TotalPaginas = IntTotalPages;

                ctrlPaginador.Visible = false;
                if (ctrlPaginador.TotalResgistros > intPaginaCantidad)
                {
                    ctrlPaginador.Visible = true;
                }

                ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + IntTotalCount, true, Enumerador.enmTipoMensaje.INFORMATION);
            }
            else
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                Grd_Tablas.DataSource = null;
                Grd_Tablas.DataBind();
            }

            updConsulta.Update();
        }

        private void BindGridEtiquetas()
        {
            DataTable DtTabla = new DataTable();
            EtiquetaConsultaBL BL = new EtiquetaConsultaBL();

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_CANT_REGISTRO;

            string strEstado = "";

            if (chkActivo.Checked)
            {
                strEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
            }
            else
            {
                strEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
            }

            if (ddl_TablaBusqueda.SelectedItem.Text == "MA_ETIQUETA")
            {
                if (ddl_ConsultaPlantilla.Items.Count == 0)
                {
                    DataTable dtPlantilla = new DataTable();
                    dtPlantilla = comun_Part1.ObtenerParametrosPorGrupo(Session, "ACTUACIÓN-TIPO PLANTILLA");
                    Util.CargarDropDownList(ddl_ConsultaPlantilla, dtPlantilla, "descripcion", "id", true, " - TODOS - ");
                }
            }
            else
            {
                return;
            }

            Int16 intPlantillaId = Convert.ToInt16(ddl_ConsultaPlantilla.SelectedValue);

            DtTabla = BL.Consultar(0, intPlantillaId, "", strEstado, intPaginaCantidad,
                                    ctrlPaginador.PaginaActual, "S",
                                   ref IntTotalPages,
                                   ref IntTotalCount);

            if (DtTabla.Rows.Count > 0)
            {
                Grd_Tablas.DataSource = DtTabla;
                Grd_Tablas.DataBind();

                ctrlPaginador.TotalResgistros = IntTotalCount;
                ctrlPaginador.TotalPaginas = IntTotalPages;

                ctrlPaginador.Visible = false;
                if (ctrlPaginador.TotalResgistros > intPaginaCantidad)
                {
                    ctrlPaginador.Visible = true;
                }

                ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + IntTotalCount, true, Enumerador.enmTipoMensaje.INFORMATION);
            }
            else
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                Grd_Tablas.DataSource = null;
                Grd_Tablas.DataBind();
            }

            updConsulta.Update();
        }

        private void GrabarHandler()
        {
            SGAC.BE.MRE.MA_TABLA_MAESTRA ObjBE = new BE.MRE.MA_TABLA_MAESTRA();
            Proceso ObjProc = new Proceso();           
            string StrScript = string.Empty;

            TablaMaestraMantenimientoBL BL = new TablaMaestraMantenimientoBL();

            int IntRpta = 0;

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

            ObjBE.tama_cCodigo = txtCodigo.Text.Trim().ToUpper();

            if ((Convert.ToInt32(ddl_TablaRegistro.SelectedValue) == (int)Enumerador.enmTabla.MA_ESTADO) || (Convert.ToInt32(ddl_TablaRegistro.SelectedValue) == (int)Enumerador.enmTabla.MA_SERVICIO))
            {
                ObjBE.tama_vGrupo = ddl_Grupo.SelectedItem.Text.Trim();
            }
            else
            {
                ObjBE.tama_vGrupo = "";
            }

            ObjBE.tama_vNombre = txtNombre.Text.Trim().ToUpper();
            ObjBE.tama_vDescripcionCorta = txtNombre.Text.Trim().ToUpper();
            ObjBE.tama_vDescripcion = txtDescripcion.Text.Trim().ToUpper();
            ObjBE.tama_vDescripcionLarga = txtDescripcion.Text.Trim().ToUpper();
            ObjBE.tama_vSimbolo = txtSimbolo.Text.Trim().ToUpper();
            ObjBE.tama_sOficinaConsularId = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            if (chkActivoMant.Checked)
            {
                ObjBE.tama_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
            }
            else
            {
                ObjBE.tama_cEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
            }


            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:
                    ObjBE.tama_sTablaId = Convert.ToInt16(ddl_TablaRegistro.SelectedValue);
                    ObjBE.tama_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    ObjBE.tama_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                    IntRpta = BL.Insertar(ObjBE);
                  
                    break;

                case Enumerador.enmAccion.MODIFICAR:
                    ObjBE.tama_sTablaId = Convert.ToInt16(Session["IntTablaId"]);
                    ObjBE.tama_sRegistroId = Convert.ToInt16(Session["IntRegistroId"]);
                    ObjBE.tama_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    ObjBE.tama_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                    IntRpta = BL.Actualizar(ObjBE);  
                 
                    break;

                case Enumerador.enmAccion.ELIMINAR:
                    ObjBE.tama_sTablaId = Convert.ToInt16(Session["IntTablaId"]);
                    ObjBE.tama_sRegistroId = Convert.ToInt16(Session["IntRegistroId"]);
                    ObjBE.tama_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    ObjBE.tama_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                    IntRpta = BL.Eliminar(ObjBE);
                   
                    break;
            }
            //------------------------------------------------
            //DataTable DtMaestraConsulta = new DataTable();
            //int MaestroId = 0;
            //int IntTotalCount = 0;
            //int IntTotalPages = 0;
            //TablaMaestraConsultaBL BLc = new TablaMaestraConsultaBL();

            //DtMaestraConsulta=BLc.Consultar(MaestroId,
            //                                "1",
            //                                Constantes.CONST_CANT_REGISTRO,
            //                                ref IntTotalCount,
            //                                ref IntTotalPages);

            //Session[Constantes.CONST_SESION_DT_MAESTRA] = DtMaestraConsulta;
            //------------------------------------------------

            string strScript = string.Empty;

            if (ObjProc.IErrorNumero == 0)
            {
                if (IntRpta == (int)Enumerador.enmResultadoQuery.OK)
                {
                    Session["Grabo"] = "SI";

                    if (enmAccion != Enumerador.enmAccion.ELIMINAR)
                    {
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO);
                    }
                    else
                    {
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_ELIMINADO);
                    }

                    MostrarCambiosDatosMantenimiento();
                }
                else
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
                }
            }
            else
            {
                Session["Grabo"] = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, ObjProc.vErrorMensaje);
            }

            Comun.EjecutarScript(Page, strScript);
        }


        private void GrabarEtiqueta()
        {
            MA_ETIQUETA objBE = new MA_ETIQUETA();
            EtiquetaMantenimientoBL BL = new EtiquetaMantenimientoBL();
            int IntRpta = 0;

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

            objBE.etiq_sPlantillaId = Convert.ToInt16(ddl_Plantilla.SelectedValue);
            objBE.etiq_tOrden = Convert.ToInt16(txtNro.Text);
            objBE.etiq_vEtiqueta = txtEtiqueta.Text.Trim();
            objBE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:
                    objBE.etiq_iEtiquetaId = 0;
                    objBE.etiq_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objBE.etiq_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                    IntRpta = BL.Insertar(objBE);

                    break;
                case Enumerador.enmAccion.MODIFICAR:
                    objBE.etiq_iEtiquetaId = Convert.ToInt64(Session["IntRegistroId"]);
                    if (chkActivoMant.Checked)
                    {
                        objBE.etiq_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString(); 
                    }
                    else
                    {
                        objBE.etiq_cEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString(); 
                    }
                    objBE.etiq_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objBE.etiq_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                    IntRpta = BL.Actualizar(objBE);

                    break;
                case Enumerador.enmAccion.ELIMINAR:
                    objBE.etiq_iEtiquetaId = Convert.ToInt64(Session["IntRegistroId"]);
                    objBE.etiq_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    objBE.etiq_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                    IntRpta = BL.Anular(objBE);

                    break;
            }
            //--------------------------------------
            //DataTable DtMaestraConsulta = new DataTable();
            
            //int IntTotalCount = 0;
            //int IntTotalPages = 0;

            //EtiquetaConsultaBL BLC = new EtiquetaConsultaBL();

            //Int16 intPlantillaId = Convert.ToInt16(ddl_Plantilla.SelectedValue);
            //string strEstado = "";

            //if (chkActivo.Checked)
            //{
            //    strEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
            //}
            //else
            //{
            //    strEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
            //}
            //DtMaestraConsulta = BLC.Consultar(0, intPlantillaId, "", strEstado,
            //    Constantes.CONST_CANT_REGISTRO, 1, "S",
            //    ref IntTotalPages,
            //    ref IntTotalCount);
            //Session[Constantes.CONST_SESION_DT_MAESTRA] = DtMaestraConsulta;
            //--------------------------------------
            string strScript = string.Empty;
            if (IntRpta == (int)Enumerador.enmResultadoQuery.OK)
            {
                Session["Grabo"] = "SI";

                if (enmAccion != Enumerador.enmAccion.ELIMINAR)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO);
                }
                else
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_ELIMINADO);
                }
                MostrarCambiosDatosMantenimientoEtiquetas();
            }
            else
            {
                Session["Grabo"] = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_MENSAJE_ERROR_MANTENIMIENTO);
            }
            Comun.EjecutarScript(Page, strScript);
        }

        private void FillWebCombo(DataTable pDataTable,
                                 DropDownList pWebCombo,
                                 String str_pDescripcion,
                                 String str_pValor)
        {
            pWebCombo.DataSource = pDataTable;
            pWebCombo.DataTextField = str_pDescripcion;
            pWebCombo.DataValueField = str_pValor;
            pWebCombo.DataBind();
            pWebCombo.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
        }
      


        #endregion      
    }

    public static class StringExtension
    {
        public static string EncodeString(this string cadena)
        {
            return cadena.Replace("&nbsp;", "");
        }

        public static string EncodeNumeric(this string cadena)
        {
            return cadena.Replace("&nbsp;", "0");
        }

        public static string EncodeNumeric2(this string cadena)
        {
            return cadena.Replace("&nbsp;", "00");
        }

        public static string EncodeBooleano(this string cadena)
        {
            return cadena.Replace("&nbsp;", "0");
        }
    }
}