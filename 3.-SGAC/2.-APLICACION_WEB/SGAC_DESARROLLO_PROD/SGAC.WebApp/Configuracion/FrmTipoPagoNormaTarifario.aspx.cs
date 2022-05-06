using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.Accesorios;
using System.Data;
using SGAC.Controlador;
using SGAC.Configuracion.Sistema.BL;

namespace SGAC.WebApp.Configuracion
{
    public partial class FrmTipoPagoNormaTarifario : System.Web.UI.Page
    {
        #region CAMPOS
            private string strNombreEntidad = "NORMA_TARIFARIO";
            private string strVariableAccion = "NormaTarifario_Accion";
            private string strVariableDt = "NormaTarifario_Tabla";
            private string strVariableIndice = "NormaTarifario_Indice";
        #endregion

        private void Page_Init(object sender, System.EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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
                Session["Grabo"] = "NO";
                CargarDatosIniciales();
                CargarListadosDesplegables();
                ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo, ctrlToolBarMantenimiento.btnEditar, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar };
                GridView[] arrGridView = { gdvTipoPagoNorma };
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }
        }


        protected void chkSeleccionarTodasNormas_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkSeleccionarTodasNormas = (CheckBox)gdvNorma.HeaderRow.FindControl("chkSeleccionarTodasNormas");
            foreach (GridViewRow row in gdvNorma.Rows)
            {
                CheckBox chkSeleccionarNorma = (CheckBox)row.FindControl("chkSeleccionarNorma");
                if (chkSeleccionarTodasNormas.Checked == true)
                {
                    chkSeleccionarNorma.Checked = true;
                }
                else
                {
                    chkSeleccionarNorma.Checked = false;
                }
            }
        }

             
        protected void chkSeleccionarTodasTarifas_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkSeleccionarTodasTarifas = (CheckBox)gdvTarifario.HeaderRow.FindControl("chkSeleccionarTodasTarifas");
            foreach (GridViewRow row in gdvTarifario.Rows)
            {
                CheckBox chkSeleccionarTarifa = (CheckBox)row.FindControl("chkSeleccionarTarifa");
                if (chkSeleccionarTodasTarifas.Checked == true)
                {
                    chkSeleccionarTarifa.Checked = true;
                }
                else
                {
                    chkSeleccionarTarifa.Checked = false;
                }
            }
        }

        protected void gdvTipoPagoNorma_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strScript = string.Empty;
            int intSeleccionado = Convert.ToInt32(e.CommandArgument);

            Session[strVariableIndice] = intSeleccionado;

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

                ctrlToolBarMantenimiento.btnNuevo.Enabled = true;
                ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
                ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                ctrlToolBarMantenimiento.btnEliminar.Enabled = true;

                HabilitarMantenimiento(true);
                PintarSeleccionado();
                ddlregTipoPago.Enabled = false;
                

                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
                
            }

            Comun.EjecutarScript(Page, strScript);
            
        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            Session[strVariableDt] = new DataTable();

            BindGrid();
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            Session[strVariableDt] = new DataTable();
            ctrlPaginador.InicializarPaginador();

            BindGrid();
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            ddlConsultaTipoPago.SelectedIndex = 0;
            ddlConsultaNorma.SelectedIndex = 0;
            ddlConsultaTarifa.SelectedIndex = 0;

            LimpiarDatosMantenimiento();
            HabilitarMantenimiento();
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            Comun.EjecutarScript(Page, Util.ActivarTab(0, "Consulta") + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));

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

        void ctrlToolBarMantenimiento_btnGrabarHandler()
        {
            string strScript = string.Empty;

            if (ddlregTipoPago.SelectedIndex == 0)
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Debe seleccionar un Tipo de Pago.", false, 190, 250);
                Comun.EjecutarScript(Page, strScript);
                return;
            }

            //-----------------------------------------------------------
            ActualizarSesionTarifario();
            DataTable dtListaTarifas = new DataTable();
            dtListaTarifas = (DataTable)Session["listaTarifas"];
            DataView dvTarifas = dtListaTarifas.DefaultView;
            dvTarifas.RowFilter = "seleccion = 1";
            DataTable dtTarifasSel = dvTarifas.ToTable();

            //-----------------------------------------------------------
           
            if (dtTarifasSel.Rows.Count == 0)
            {
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Debe seleccionar por lo menos una Tarifa Consular.", false, 190, 250);
                Comun.EjecutarScript(Page, strScript);
                return;
            }

            //-------------------------------------
            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

            if (enmAccion == Enumerador.enmAccion.INSERTAR)
            {

                ActualizarSesionNorma();
                DataTable dtListaNormas = new DataTable();
                dtListaNormas = (DataTable)Session["listaNormas"];
                DataView dvNormas = dtListaNormas.DefaultView;
                dvNormas.RowFilter = "seleccion = 1";
                DataTable dtNormasSel = dvNormas.ToTable();
                //-------------------------------------
                DataTable dtNormaTarifario = new DataTable();
                NormaTarifarioDL objNormaTarifarioBL;
                int IntTotalCount = 0;
                int IntTotalPages = 0;
                int intPaginaCantidad = Constantes.CONST_CANT_REGISTRO;
                Int16 intTipoPagoId = 0;
                string strTipoPago = "";

                if (ddlregTipoPago.SelectedIndex > 0)
                {
                    intTipoPagoId = Convert.ToInt16(ddlregTipoPago.SelectedValue);
                    strTipoPago = ddlregTipoPago.SelectedItem.Text;
                }

                if (dtNormasSel.Rows.Count == 0)
                {
                    objNormaTarifarioBL = new NormaTarifarioDL();
                    dtNormaTarifario = objNormaTarifarioBL.Consultar(intTipoPagoId, 0, "", "", false, intPaginaCantidad, ctrlPaginador.PaginaActual, "S", ref IntTotalCount, ref IntTotalPages);

                    if (dtNormaTarifario.Rows.Count > 0)
                    {
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Ya existe registrado el Tipo de Pago: " + strTipoPago, false, 190, 250);
                        Comun.EjecutarScript(Page, strScript);
                        return;
                    }
                }
                else
                {
                    Int16 intNormaId = 0;
                    string strNorma = "";
                    bool bExiste = false;

                    for (int i = 0; i < dtNormasSel.Rows.Count; i++)
                    {
                        intNormaId = Convert.ToInt16(dtNormasSel.Rows[i]["norm_sNormaId"].ToString());
                        strNorma = dtNormasSel.Rows[i]["norm_vDescripcionCorta"].ToString();
                        objNormaTarifarioBL = new NormaTarifarioDL();

                        dtNormaTarifario = objNormaTarifarioBL.Consultar(intTipoPagoId, intNormaId, "", "", false, intPaginaCantidad, ctrlPaginador.PaginaActual, "S", ref IntTotalCount, ref IntTotalPages);

                        if (dtNormaTarifario.Rows.Count > 0)
                        {
                            bExiste = true;
                            break;
                        }
                    }

                    if (bExiste)
                    {
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, "Ya existe registrado el Tipo de Pago: " + strTipoPago + " y la Norma: " + strNorma, false, 190, 250);
                        Comun.EjecutarScript(Page, strScript);
                        return;
                    }
                }
            }
            //-------------------------------------

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
            ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
            Comun.EjecutarScript(Page, Util.ActivarTab(0, "Consulta") + Util.NombrarTab(0, Constantes.CONST_TAB_INICIAL) + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }

        protected void gdvNorma_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblID = (Label)e.Row.FindControl("lblID");
                CheckBox chkSeleccionarNorma = (CheckBox)e.Row.FindControl("chkSeleccionarNorma");

                DataTable dt = new DataTable();
                dt = (DataTable)Session["listaNormas"];

                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["norm_sNormaId"].ToString().Equals(lblID.Text.Trim()))
                        {
                            if (dt.Rows[i]["seleccion"].ToString().Equals("1"))
                            {
                                chkSeleccionarNorma.Checked = true;
                            }
                        }
                    }
                }
            }
        }
        
        

        protected void gdvTarifario_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblID = (Label)e.Row.FindControl("lblID");
                CheckBox chkSeleccionarTarifa = (CheckBox)e.Row.FindControl("chkSeleccionarTarifa");

                DataTable dt = new DataTable();
                dt = (DataTable)Session["listaTarifas"];

                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["tari_sTarifarioId"].ToString().Equals(lblID.Text.Trim()))
                        {
                            if (dt.Rows[i]["seleccion"].ToString().Equals("1"))
                            {
                                chkSeleccionarTarifa.Checked = true;
                            }
                        }
                    }
                }
            }
        }

        protected void chkSoloNormasSeleccionadas_CheckedChanged(object sender, EventArgs e)
        {
            FiltroExcepcionNormas();
        }

       
        protected void chkSoloTarifasSeleccionadas_CheckedChanged(object sender, EventArgs e)
        {
            FiltroExcepcionTarifaCosto();
        }

        #region Métodos

        private void CargarDatosIniciales()
        {
            Session.Add(strVariableAccion, Enumerador.enmAccion.INSERTAR);
            Session.Add(strVariableIndice, -1);
            Session.Add(strVariableDt, new DataTable());

            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            Comun.EjecutarScript(Page, Util.NombrarTab(0, "Consulta"));
            updMantenimiento.Update();
        }

        private void CargarListadosDesplegables()
        {                     
            DataTable dtTipoPago = new DataTable();
            DataTable dtNorma = new DataTable();
            DataTable dtTarifario = new DataTable();
           
            //-------------------------------------------------------
            dtTipoPago = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACREDITACION_TIPO_COBRO);

            DataView dvTipoPago = dtTipoPago.DefaultView;
            dvTipoPago.Sort = "descripcion";
            DataTable dtTipoPagoOrdenada = dvTipoPago.ToTable();

            Util.CargarParametroDropDownList(ddlConsultaTipoPago, dtTipoPagoOrdenada, true, "- TODOS -");
            Util.CargarParametroDropDownList(ddlregTipoPago, dtTipoPagoOrdenada, true, "- SELECCIONAR -");
           
            //-------------------------------------------------------
            NormaTarifarioDL objNormaBL = new NormaTarifarioDL();
            int IntTotalCount = 0;
            int IntTotalPages = 0;
            Int16 intVigenteId = 0;

            intVigenteId = ObtenerEstadoNormaEstado();

            dtNorma = objNormaBL.ConsultarNorma(0, 0, "", "", "", intVigenteId, 5000, 1, "N", ref IntTotalCount, ref IntTotalPages);

            DataView dvNorma = dtNorma.DefaultView;
            dvNorma.Sort = "norm_vDescripcionCorta";
            DataTable dtNormaOrdenada = dvNorma.ToTable();

            Util.CargarDropDownList(ddlConsultaNorma, dtNormaOrdenada, "norm_vDescripcionCorta", "norm_sNormaId", true, "- TODOS -");

            DataTable dtListaNormas = new DataTable();

            dtListaNormas = CrearListaNormas(dtNormaOrdenada);
            Session["listaNormas"] = dtListaNormas;
            gdvNorma.DataSource = dtListaNormas;
            gdvNorma.DataBind();            

            //------------------------------------------------
            //Fecha: 03/04/2019
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Mostrar todas las tarifas consulares
            //------------------------------------------------
            object[] arrParametros = { 0, "", 
                                       "", 
                                       ((char)Enumerador.enmEstado.ACTIVO).ToString(),
                                       1, 500, 0, 0 };


            dtTarifario = comun_Part2.ObtenerTarifarioConsulta(Session, ref arrParametros);

            Util.CargarDropDownList(ddlConsultaTarifa, dtTarifario, "tari_vdescripcioncorta", "tari_starifarioId", true, "- TODOS -");

            DataTable dtListaTarifas = new DataTable();

            dtListaTarifas = CrearListaTarifas(dtTarifario);
           
            Session["listaTarifas"] = dtListaTarifas;
            FiltroExcepcionTarifaCosto(true);
        }

        private void HabilitarMantenimiento(bool bolHabilitar = true)
        {
            ddlregTipoPago.Enabled = bolHabilitar;
        }

        private void LimpiarDatosMantenimiento()
        {
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            HF_TipoPagoId.Value = "";
            HF_NormaId.Value = "";

            ddlregTipoPago.SelectedIndex = 0;
            

            gdvTipoPagoNorma.DataSource = null;
            gdvTipoPagoNorma.DataBind();
            ctrlPaginador.InicializarPaginador();
            ctrlPaginador.Visible = false;

            chkSoloTarifasSeleccionadas.Checked = false;
            chkSoloTarifasSeleccionadas.Text = "Mostrar solo las seleccionadas";
            chkSoloNormasSeleccionadas.Checked = false;
            chkSoloNormasSeleccionadas.Text = "Mostrar solo las seleccionadas";

            chkExcepcionTarifa.Checked = false;
            chkTarifaConCosto.Checked = false;
            chkTarifaSinCosto.Checked = false;

            //------------------------------------------------
            DataTable dtTarifario = new DataTable();
            //dtTarifario = (DataTable)Session[Constantes.CONST_SESION_DT_TARIFARIO];
            dtTarifario = Comun.ObtenerTarifarioCargaInicial(Session);


            DataTable dtListaTarifas = new DataTable();

            dtListaTarifas = CrearListaTarifas(dtTarifario);

            Session["listaTarifas"] = dtListaTarifas;
            FiltroExcepcionTarifaCosto(true);
            //------------------------------------------------
            DataTable dtNorma = new DataTable();
            NormaTarifarioDL objNormaBL = new NormaTarifarioDL();
            int IntTotalCount = 0;
            int IntTotalPages = 0;
            Int16 intVigenteId = 0;

            intVigenteId = ObtenerEstadoNormaEstado();

            dtNorma = objNormaBL.ConsultarNorma(0, 0, "", "", "", intVigenteId, 5000, 1, "N", ref IntTotalCount, ref IntTotalPages);

            DataView dvNorma = dtNorma.DefaultView;
            dvNorma.Sort = "norm_vDescripcionCorta";
            DataTable dtNormaOrdenada = dvNorma.ToTable();
            DataTable dtListaNormas = new DataTable();

            dtListaNormas = CrearListaNormas(dtNormaOrdenada);
            Session["listaNormas"] = dtListaNormas;
            FiltroExcepcionNormas(true);
            //------------------------------------------------
                                 
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

        private void PintarSeleccionado()
        {
            if (Session != null)
            {
                 DataRow drSeleccionado = ObtenerFilaSeleccionada();
                 if (drSeleccionado != null)
                 {
                     ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
                     ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                     ctrlToolBarMantenimiento.btnEliminar.Enabled = true;
                     chkSoloNormasSeleccionadas.Checked = false;                     
                     chkSoloTarifasSeleccionadas.Checked = false;

                     //---------------------------------------------------------------
                     HF_TipoPagoId.Value = drSeleccionado["nota_sPagoTipoId"].ToString();

                     Int16 intTipoPagoId = Convert.ToInt16(HF_TipoPagoId.Value);

                     ddlregTipoPago.SelectedValue = HF_TipoPagoId.Value;

                     HF_NormaId.Value = drSeleccionado["nota_sNormaId"].ToString();

                     Int16 intNormaId = Convert.ToInt16(HF_NormaId.Value);
                     //---------------------------------------------------------------

                     DataTable dtNorma = new DataTable();
                     NormaTarifarioDL objNormaBL = new NormaTarifarioDL();
                     int IntTotalCount = 0;
                     int IntTotalPages = 0;
                     Int16 intVigenteId = 0;

                     intVigenteId = ObtenerEstadoNormaEstado();

                     dtNorma = objNormaBL.ConsultarNorma(0, 0, "", "", "", intVigenteId, 5000, 1, "N", ref IntTotalCount, ref IntTotalPages);

                     DataTable dtListaNormas = new DataTable();

                     dtListaNormas = CrearListaNormas(dtNorma);
                     //---------------------------------------------------------------
                     DataTable dtTarifario = new DataTable();
                     //dtTarifario = (DataTable)Session[Constantes.CONST_SESION_DT_TARIFARIO];
                     dtTarifario = Comun.ObtenerTarifarioCargaInicial(Session);

                     DataTable dtListaTarifas = new DataTable();

                     dtListaTarifas = CrearListaTarifas(dtTarifario);
                     //---------------------------------------------------------------
                     DataTable dtNormaTarifario = new DataTable();

                     NormaTarifarioDL objNormaTarifarioBL = new NormaTarifarioDL();
                     
                     IntTotalCount = 0;
                     IntTotalPages = 0;

                     dtNormaTarifario = objNormaTarifarioBL.Consultar(intTipoPagoId, intNormaId, "", "",false, 20000, 1, "S", ref IntTotalCount, ref IntTotalPages);

                     DataTable dtNormaSel = dtNormaTarifario.DefaultView.ToTable(true, "norm_sNormaId");
                     DataTable dtTarifarioSel = dtNormaTarifario.DefaultView.ToTable(true, "nota_sTarifarioId");

                     if (dtNormaTarifario.Rows.Count > 0)
                     {
                         #region ExisteNormaTarifario

                         Int16 intTarifaId = 0;
                         intNormaId = 0;

                        //-----------------------------------------------------------------
                         for (int i = 0; i < dtNormaSel.Rows.Count; i++)
                         {
                             intNormaId = Convert.ToInt16(dtNormaSel.Rows[i]["norm_sNormaId"].ToString());

                             for (int x = 0; x < dtListaNormas.Rows.Count; x++)
                             {
                                 if (intNormaId == Convert.ToInt16(dtListaNormas.Rows[x]["norm_sNormaId"].ToString()))
                                 {
                                     dtListaNormas.Rows[x]["seleccion"] = "1";
                                     break;
                                 }
                             }
                         }
                         //-----------------------------------------------------------------
                         for (int i = 0; i < dtTarifarioSel.Rows.Count; i++)
                         {
                             intTarifaId = Convert.ToInt16(dtTarifarioSel.Rows[i]["nota_sTarifarioId"].ToString());

                             for (int x = 0; x < dtListaTarifas.Rows.Count; x++)
                             {
                                 if (intTarifaId == Convert.ToInt16(dtListaTarifas.Rows[x]["tari_sTarifarioId"].ToString()))
                                 {
                                     dtListaTarifas.Rows[x]["seleccion"] = "1";
                                     break;
                                 }
                             }
                         }
                         
                         
                         //-----------------------------------------------------------------
                         #endregion
                     }
                     //-----------------------------------------------------
                     chkSoloNormasSeleccionadas.Checked = true;
                     //-----------------------------------------------------
                     Session["listaNormas"] = dtListaNormas;
                     //-----------------------------------------------------
                     chkSoloNormasSeleccionadas.Text = "Todas las Normas";
                     DataTable dtNormas = new DataTable();
                     dtNormas = (DataTable)Session["listaNormas"];
                     DataView dvNormas = dtNormas.DefaultView;

                     dvNormas.RowFilter = "seleccion = 1";
                     DataTable dtListaNormasFiltradas = dvNormas.ToTable();

                     //-----------------------------------------------------
                     gdvNorma.DataSource = dtListaNormasFiltradas;
                     gdvNorma.DataBind();
                     //-----------------------------------------------------

                     Session["listaTarifas"] = dtListaTarifas;
                     //-----------------------------------------------------
                     chkSoloTarifasSeleccionadas.Checked = true;
                     chkSoloTarifasSeleccionadas.Text = "Todas las tarifas";
                     DataTable dtTarifas = new DataTable();
                     dtTarifas = (DataTable)Session["listaTarifas"];
                     DataView dvTarifas = dtTarifas.DefaultView;

                     dvTarifas.RowFilter = "seleccion = 1";

                     DataTable dtListaTarifasFiltrada = dvTarifas.ToTable();
                     //-----------------------------------------------------

                     gdvTarifario.DataSource = dtListaTarifasFiltrada;
                     gdvTarifario.DataBind();
                                          
                     //-----------------------------------------------------

                     if (Convert.ToBoolean(drSeleccionado["FlagExcepcionTarifa"].ToString()) == true)
                     {
                         chkExcepcionTarifa.Checked = true;
                         FiltroExcepcionTarifaCosto();
                     }
                     updMantenimiento.Update();
                 }
            }
        }

        private SGAC.BE.MRE.SI_NORMA_TARIFARIO ObtenerEntidadMantenimiento()
        {
            if (Session != null)
            {
                SGAC.BE.MRE.SI_NORMA_TARIFARIO objParametro = new BE.MRE.SI_NORMA_TARIFARIO();

                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
                {
                    //Editar o Anular
                    objParametro.nota_iNormaTarifarioId = 0;
                    objParametro.nota_sPagoTipoId = Convert.ToInt16(HF_TipoPagoId.Value);
                    objParametro.nota_sNormaId = Convert.ToInt16(HF_NormaId.Value);
                    objParametro.nota_sTarifarioId = 0;
                    objParametro.nota_sOficinaConsularId = 0;
                }
                objParametro.nota_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParametro.nota_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objParametro.nota_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParametro.nota_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objParametro.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                
                return objParametro;
            }

            return null;
        }

        private void BindGrid()
        {
            DataTable dtNormaTarifario = new DataTable();
            NormaTarifarioDL objNormaTarifarioBL = new NormaTarifarioDL();
            
            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaCantidad = Constantes.CONST_CANT_REGISTRO;
            
            Int16 intTipoPagoId = 0;
            Int16 intNormaId = -1;
            string strTarifaLetra = "";
            bool bExcepcion = false;
            //-----------------------------------------------------
           
            if (ddlConsultaTipoPago.SelectedIndex > 0)
            {
                intTipoPagoId = Convert.ToInt16(ddlConsultaTipoPago.SelectedValue);
            }
            if (ddlConsultaNorma.SelectedIndex > 0)
            {
                intNormaId = Convert.ToInt16(ddlConsultaNorma.SelectedValue);
            }

            if (ddlConsultaTarifa.SelectedIndex > 0) 
            {
                string strDescripcion = ddlConsultaTarifa.SelectedItem.Text;

                strTarifaLetra = strDescripcion.Substring(0, strDescripcion.IndexOf(" ")).Trim();
            }

            bExcepcion = chkconsultaExcepcion.Checked;

            dtNormaTarifario = objNormaTarifarioBL.Consultar(intTipoPagoId, intNormaId, strTarifaLetra, "", bExcepcion, intPaginaCantidad, ctrlPaginador.PaginaActual, "S", ref IntTotalCount, ref IntTotalPages);

            Session[strVariableDt] = dtNormaTarifario;

            if (dtNormaTarifario.Rows.Count > 0)
            {
                gdvTipoPagoNorma.SelectedIndex = -1;
                gdvTipoPagoNorma.DataSource = dtNormaTarifario;
                gdvTipoPagoNorma.DataBind();

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

                gdvTipoPagoNorma.DataSource = null;
                gdvTipoPagoNorma.DataBind();
            }
            updConsulta.Update();
        }

        private void GrabarHandler()
        {
            BE.MRE.SI_NORMA_TARIFARIO objNormaTarifarioBE = new BE.MRE.SI_NORMA_TARIFARIO();

            string strScript = string.Empty;

            NormaTarifarioDL objNormaTarifarioBL = new NormaTarifarioDL();
            
            List<SGAC.BE.MRE.SI_NORMA_TARIFARIO> listaNormaTarifario = new List<BE.MRE.SI_NORMA_TARIFARIO>();

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];
                        
            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:

                    ActualizarSesionNorma();
                    ActualizarSesionTarifario();
                    listaNormaTarifario = ObtenerListaNormaTarifario();

                    objNormaTarifarioBE = objNormaTarifarioBL.InsertarNormaTarifario(listaNormaTarifario);

                    break;
                case Enumerador.enmAccion.MODIFICAR:
                    
                        ActualizarSesionNorma();
                        ActualizarSesionTarifario();
                        listaNormaTarifario = ObtenerListaNormaTarifario();
                        
                        //------------------------------------------------------------------------------------
                        //Fecha: 06/06/2019
                        //Autor: Miguel Márquez
                        //Objetivo: Verificar si los registros de la Tabla: SI_NORMA_TARIFARIO deben anularse.
                        //------------------------------------------------------------------------------------
        
                        BE.MRE.SI_NORMA_TARIFARIO objNormaTarifario = new BE.MRE.SI_NORMA_TARIFARIO();

                        objNormaTarifario = ObtenerEntidadMantenimiento();

                        int IntTotalCount = 0;
                        int IntTotalPages = 0;
                        Int16 intTipoPagoId = Convert.ToInt16(objNormaTarifario.nota_sPagoTipoId);
                        Int16 intNormaId = Convert.ToInt16(objNormaTarifario.nota_sNormaId);
                        Int16 intTarifaId = 0;
                        bool bExiste = false;
                        bool bExisteError = false;    

                        DataTable dtNormaTarifario = objNormaTarifarioBL.Consultar(intTipoPagoId, intNormaId, "", "", false, 10000, 1, "N", ref IntTotalCount, ref IntTotalPages);

                        for (int i = 0; i < dtNormaTarifario.Rows.Count; i++)
                        {
                            intTarifaId = Convert.ToInt16(dtNormaTarifario.Rows[i]["nota_sTarifarioId"].ToString());
                            bExiste = false;
                            //---------------------------------------------------
                            for (int x = 0; x < listaNormaTarifario.Count; x++)
                            {
                                if (intTipoPagoId == listaNormaTarifario[x].nota_sPagoTipoId &&
                                    intNormaId == listaNormaTarifario[x].nota_sNormaId &&
                                    intTarifaId == listaNormaTarifario[x].nota_sTarifarioId)
                                {
                                    bExiste = true;
                                    break;
                                }
                            }
                            //---------------------------------------------------
                            if (bExiste==false)
                            {
                                bool bExistenRegistro = false;

                                bExistenRegistro = objNormaTarifarioBL.ConsultarCantidadPagoNormaTarifario(intNormaId, intTarifaId, intTipoPagoId);
                                
                                if (bExistenRegistro == false)
                                {                                                                     
                                    objNormaTarifario.nota_sTarifarioId = intTarifaId;
                                    objNormaTarifarioBE = objNormaTarifarioBL.AnularNormaTarifario(objNormaTarifario);
                                }
                                else
                                {
                                    bExisteError = true;
                                    objNormaTarifarioBE.Error = true;
                                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Existen registros vinculados a la Norma y el Tipo de Pago.");
                                    break;
                                }
                            }
                            //---------------------------------------------------
                        }
                        //************************************
                        if (bExisteError == false)
                        {
                            //-------------------------------------------------------------------------------------------
                            //Fecha: 06/06/2019
                            //Autor: Miguel Márquez
                            //Objetivo: Verificar si los registros de la lista: listaNormaTarifario deben adicionarse
                            //-------------------------------------------------------------------------------------------
                            SGAC.BE.MRE.SI_NORMA_TARIFARIO BE;
                            List<SGAC.BE.MRE.SI_NORMA_TARIFARIO> listaNormaTarifarioNuevos = new List<BE.MRE.SI_NORMA_TARIFARIO>();
                            //------------------------------------------------------
                            for (int x = 0; x < listaNormaTarifario.Count; x++)
                            {
                                intTarifaId = listaNormaTarifario[x].nota_sTarifarioId;

                                bool bExistenRegistro = false;
                                //---------------------------------------------------
                                for (int i = 0; i < dtNormaTarifario.Rows.Count; i++)
                                {
                                    if (Convert.ToInt16(dtNormaTarifario.Rows[i]["nota_sTarifarioId"].ToString()) == intTarifaId)
                                    {
                                        bExistenRegistro = true;
                                        break;
                                    }
                                }
                                //---------------------------------------------------
                                if (bExistenRegistro == false)
                                {
                                    BE = new BE.MRE.SI_NORMA_TARIFARIO();
                                    BE.nota_sNormaId = intNormaId;
                                    BE.nota_sTarifarioId = intTarifaId;
                                    BE.nota_cEstado = "A";
                                    BE.nota_sPagoTipoId = intTipoPagoId;
                                    BE.nota_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                                    BE.nota_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                                    BE.nota_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                                    BE.nota_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                                    BE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                                    listaNormaTarifarioNuevos.Add(BE);
                                }
                            }
                            //------------------------------------------------------
                            if (listaNormaTarifarioNuevos.Count > 0)
                            {
                                objNormaTarifarioBE = objNormaTarifarioBL.InsertarNormaTarifario(listaNormaTarifarioNuevos);
                            }
                        }
                        //************************************
                    break;
                case Enumerador.enmAccion.ELIMINAR:
                    //----------------------------------------------------------------------------------------
                    //Fecha: 06/06/2019
                    //Autor: Miguel Márquez
                    //Objetivo: Validar que no existan registro asociados a la tabla: SI_NORMA_TARIFARIO
                    //----------------------------------------------------------------------------------------
                    BE.MRE.SI_NORMA_TARIFARIO objNormaTarifariox = new BE.MRE.SI_NORMA_TARIFARIO();

                    objNormaTarifariox = ObtenerEntidadMantenimiento();                    

                    Int16 intTipoPagoIdx = Convert.ToInt16(objNormaTarifariox.nota_sPagoTipoId);
                    Int16 intNormaIdx = Convert.ToInt16(objNormaTarifariox.nota_sNormaId);

                    bool bExistenRegistros = false;

                    bExistenRegistros = objNormaTarifarioBL.ConsultarCantidadPagoNormaTarifario(intNormaIdx, 0, intTipoPagoIdx);

                    if (bExistenRegistros == false)
                    {
                        objNormaTarifarioBE = objNormaTarifarioBL.AnularNormaTarifario(ObtenerEntidadMantenimiento());
                    }
                    else
                    {
                        objNormaTarifarioBE.Error = true;
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Existen registros vinculados a la Norma y el Tipo de Pago.");
                    }
                    break;
                case Enumerador.enmAccion.CONSULTAR:
                    break;
            }
            if (objNormaTarifarioBE.Error == false)
            {
                if (enmAccion == Enumerador.enmAccion.ELIMINAR)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO_ANULAR);
                }
                else
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO);
                }

                Session["Grabo"] = "SI";

                HabilitarMantenimiento();
                LimpiarDatosMantenimiento();

                strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                strScript += Util.HabilitarTab(0);

                Session[strVariableDt] = new DataTable();
                BindGrid();

                updConsulta.Update();
                updMantenimiento.Update();
            }
            else
            {
                Session["Grabo"] = string.Empty;
                if (strScript == string.Empty)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Error del Sistema. Consulte con el area de soporte técnico");
                }
            }
            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;
            Comun.EjecutarScript(Page, strScript);
        }

       
        private DataTable CrearTablaNorma()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("norm_sNormaId", typeof(Int16));
            dt.Columns.Add("norm_vDescripcionCorta", typeof(String));
            dt.Columns.Add("seleccion", typeof(Int16));

            return dt;
        }
        private DataTable CrearTablaTarifario()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("tari_sTarifarioId", typeof(Int16));
            dt.Columns.Add("tarifa", typeof(String));
            dt.Columns.Add("tari_FCosto", typeof(float));
            dt.Columns.Add("FlagExcepcion", typeof(Int16));
            dt.Columns.Add("seleccion", typeof(Int16));

            return dt;
        }
        
        private Int16 ObtenerEstadoNormaEstado()
        {
            DataTable dtEstadoNorma = new DataTable();
            //dtEstadoNorma = Comun.ObtenerEstadosPorGrupo(Session, "NORMA-ESTADO");
            dtEstadoNorma = comun_Part1.ObtenerParametrosPorGrupoMRE("NORMA-ESTADO");
            Int16 intVigenteId = 0;

            for (int i = 0; i < dtEstadoNorma.Rows.Count; i++)
            {
                if (dtEstadoNorma.Rows[i]["Valor"].ToString().Equals("VIGENTE"))
                {
                    intVigenteId = Convert.ToInt16(dtEstadoNorma.Rows[i]["id"].ToString());
                    break;
                }
            }
            return intVigenteId;
        }

      
        private DataTable CrearListaTarifas(DataTable dtTarifario)
        {
            DataTable dtListaTarifas = new DataTable();

            dtListaTarifas = CrearTablaTarifario();

            DataRow dr;
            string strTarifa = "";
            string strDescripcion = "";
            float intCosto = 0;
            bool bExcepcion = false;
            string strLetra = "";

            for (int i = 0; i < dtTarifario.Rows.Count; i++)
            {
                dr = dtListaTarifas.NewRow();

                dr["tari_sTarifarioId"] = dtTarifario.Rows[i]["tari_sTarifarioId"].ToString();

                strDescripcion = dtTarifario.Rows[i]["tari_vdescripcioncorta"].ToString();
                intCosto = Convert.ToInt64(dtTarifario.Rows[i]["tari_FCosto"].ToString());
                strLetra = dtTarifario.Rows[i]["tari_vLetra"].ToString().Trim();

                if (strLetra.Length == 0)
                {
                    strTarifa = dtTarifario.Rows[i]["tari_sNumero"].ToString().Trim();
                }
                else
                {
                    strTarifa = dtTarifario.Rows[i]["tari_sNumero"].ToString().Trim() + "-" + strLetra;
                }

                bExcepcion = Convert.ToBoolean(dtTarifario.Rows[i]["tari_bFlagExcepcion"].ToString());

                dr["tarifa"] = strTarifa;
                dr["tari_FCosto"] = intCosto;
                if (bExcepcion)
                {
                    dr["FlagExcepcion"] = "1";
                }
                else
                {
                    dr["FlagExcepcion"] = "0";
                }
                dr["seleccion"] = "0";

                dtListaTarifas.Rows.Add(dr);
            }
            return dtListaTarifas;
        }
        private DataTable CrearListaNormas(DataTable dtNorma)
        {
            DataTable dtListaNormas = new DataTable();

            dtListaNormas = CrearTablaNorma();

            DataRow dr;

            for (int i = 0; i < dtNorma.Rows.Count; i++)
            {
                dr = dtListaNormas.NewRow();

                dr["norm_sNormaId"] = dtNorma.Rows[i]["norm_sNormaId"].ToString();

                dr["norm_vDescripcionCorta"] = dtNorma.Rows[i]["norm_vDescripcionCorta"].ToString();
                dr["seleccion"] = "0";

                dtListaNormas.Rows.Add(dr);
            }
            return dtListaNormas;
        }
        private void ActualizarSesionNorma()
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["listaNormas"];

            for (int x = 0; x < gdvNorma.Rows.Count; x++)
            {
                GridViewRow row = gdvNorma.Rows[x];
                CheckBox chkSeleccionarNorma = (CheckBox)row.FindControl("chkSeleccionarNorma");
                Int16 intNormaId = Convert.ToInt16(gdvNorma.DataKeys[x].Value);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (intNormaId == Convert.ToInt16(dt.Rows[i]["norm_sNormaId"].ToString()))
                    {
                        if (chkSeleccionarNorma.Checked)
                        {
                            dt.Rows[i]["seleccion"] = "1";
                        }
                        else
                        {
                            dt.Rows[i]["seleccion"] = "0";
                        }
                        break;
                    }
                }
            }
            Session["listaNormas"] = dt;
        }


        private void ActualizarSesionTarifario()
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["listaTarifas"];

            for (int x = 0; x < gdvTarifario.Rows.Count; x++)
            {
                GridViewRow row = gdvTarifario.Rows[x];
                CheckBox chkSeleccionarTarifa = (CheckBox)row.FindControl("chkSeleccionarTarifa");
                Int16 intTarifarioId = Convert.ToInt16(gdvTarifario.DataKeys[x].Value);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (intTarifarioId == Convert.ToInt16(dt.Rows[i]["tari_sTarifarioId"].ToString()))
                    {
                        if (chkSeleccionarTarifa.Checked)
                        {
                            dt.Rows[i]["seleccion"] = "1";
                        }
                        else
                        {
                            dt.Rows[i]["seleccion"] = "0";
                        }
                        break;
                    }
                }
            }
            Session["listaTarifas"] = dt;
        }

        private List<SGAC.BE.MRE.SI_NORMA_TARIFARIO> ObtenerListaNormaTarifario()
        {
            SGAC.BE.MRE.SI_NORMA_TARIFARIO BE;
            List<SGAC.BE.MRE.SI_NORMA_TARIFARIO> listaNormaTarifario = new List<BE.MRE.SI_NORMA_TARIFARIO>();

            //----------------------------------------------------
            DataTable dtListaNormas = new DataTable();
            dtListaNormas = (DataTable)Session["listaNormas"];
            DataView dvNormas = dtListaNormas.DefaultView;
            dvNormas.RowFilter = "seleccion = 1";
            DataTable dtNormasSel = dvNormas.ToTable();
            //----------------------------------------------------

            DataTable dtListaTarifas = new DataTable();
            dtListaTarifas = (DataTable)Session["listaTarifas"];
            DataView dvTarifas = dtListaTarifas.DefaultView;
            dvTarifas.RowFilter = "seleccion = 1";
            DataTable dtTarifasSel = dvTarifas.ToTable();
            //----------------------------------------------------
            
            Int16 intTarifaId = 0;
            Int16 intTipoPagoId = 0;
            Int16 intNormaId = 0;

            if (ddlregTipoPago.SelectedIndex > 0)
            {
                intTipoPagoId = Convert.ToInt16(ddlregTipoPago.SelectedValue);
            }

            if (dtNormasSel.Rows.Count > 0)
            {
                for (int i = 0; i < dtNormasSel.Rows.Count; i++)
                {

                    intNormaId = Convert.ToInt16(dtNormasSel.Rows[i]["norm_sNormaId"].ToString());

                    for (int x = 0; x < dtTarifasSel.Rows.Count; x++)
                    {
                        intTarifaId = Convert.ToInt16(dtTarifasSel.Rows[x]["tari_sTarifarioId"].ToString());

                        BE = new BE.MRE.SI_NORMA_TARIFARIO();

                        BE.nota_sNormaId = intNormaId;
                        BE.nota_sTarifarioId = intTarifaId;
                        BE.nota_cEstado = "A";
                        BE.nota_sPagoTipoId = intTipoPagoId;
                        BE.nota_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        BE.nota_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                        BE.nota_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        BE.nota_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                        BE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                        listaNormaTarifario.Add(BE);
                    }                   
                }
            }
            else
            {
                for (int x = 0; x < dtTarifasSel.Rows.Count; x++)
                {
                    intTarifaId = Convert.ToInt16(dtTarifasSel.Rows[x]["tari_sTarifarioId"].ToString());

                    BE = new BE.MRE.SI_NORMA_TARIFARIO();

                    BE.nota_sNormaId = intNormaId;
                    BE.nota_sTarifarioId = intTarifaId;
                    BE.nota_cEstado = "A";
                    BE.nota_sPagoTipoId = intTipoPagoId;
                    BE.nota_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    BE.nota_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                    BE.nota_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    BE.nota_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                    BE.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    listaNormaTarifario.Add(BE);
                    
                }
            }

            return listaNormaTarifario;
        }


        #endregion

        protected void chkExcepcionTarifa_CheckedChanged(object sender, EventArgs e)
        {
            FiltroExcepcionTarifaCosto();
        }

        protected void chkTarifaConCosto_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTarifaConCosto.Checked)
            {
                chkTarifaSinCosto.Checked = !(chkTarifaConCosto.Checked);
            }
            FiltroExcepcionTarifaCosto();
        }

        protected void chkTarifaSinCosto_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTarifaSinCosto.Checked)
            {
                chkTarifaConCosto.Checked = !(chkTarifaSinCosto.Checked);
            }
            FiltroExcepcionTarifaCosto();
        }

        private void FiltroExcepcionTarifaCosto(bool bEsNuevo = false)
        {
            bool bExcepcion = chkExcepcionTarifa.Checked;
            bool bTarifaConCosto = chkTarifaConCosto.Checked;
            bool bTarifaSinCosto = chkTarifaSinCosto.Checked;
            bool bTarifasSeleccionadas = chkSoloTarifasSeleccionadas.Checked;
            string strTarifasSeleccionadas = "";

            if (bTarifasSeleccionadas)
            {
                strTarifasSeleccionadas = " and seleccion = 1";
                chkSoloTarifasSeleccionadas.Text = "Todas las tarifas";
            }
            else
            {
                chkSoloTarifasSeleccionadas.Text = "Mostrar solo las seleccionadas";
            }

            if (bEsNuevo == false)
            {
                ActualizarSesionTarifario();
            }
            DataTable dt = new DataTable();
            dt = (DataTable)Session["listaTarifas"];
            DataView dv = dt.DefaultView;
                                               
            if (bExcepcion)
            {
                dv.RowFilter = "FlagExcepcion = 1" + strTarifasSeleccionadas;

                if (bTarifaConCosto)
                {
                    dv.RowFilter = "FlagExcepcion = 1 and tari_FCosto > 0" + strTarifasSeleccionadas;
                }
                if (bTarifaSinCosto)
                {
                    dv.RowFilter = "FlagExcepcion = 1 and tari_FCosto = 0" + strTarifasSeleccionadas;
                }
            }
            else
            {
                dv.RowFilter = "FlagExcepcion = 0" + strTarifasSeleccionadas;
                if (bTarifaConCosto)
                {
                    dv.RowFilter = "FlagExcepcion = 0 and tari_FCosto > 0" + strTarifasSeleccionadas;
                }
                if (bTarifaSinCosto)
                {
                    dv.RowFilter = "FlagExcepcion = 0 and tari_FCosto = 0" + strTarifasSeleccionadas;
                }
            }
            DataTable dtListaTarifasFiltradas = dv.ToTable();
            gdvTarifario.DataSource = dtListaTarifasFiltradas;
            gdvTarifario.DataBind();
        }

        private void FiltroExcepcionNormas(bool bEsNuevo = false)
        {
            if (bEsNuevo == false)
            {
                ActualizarSesionNorma();
            }

            if (chkSoloNormasSeleccionadas.Checked)
            {
                chkSoloNormasSeleccionadas.Text = "Todas las Normas";

                DataTable dt = new DataTable();
                dt = (DataTable)Session["listaNormas"];
                DataView dv = dt.DefaultView;

                dv.RowFilter = "seleccion = 1";
                DataTable dtListaNormas = dv.ToTable();

                gdvNorma.DataSource = dtListaNormas;
                gdvNorma.DataBind();
            }
            else
            {
                chkSoloNormasSeleccionadas.Text = "Mostrar solo las seleccionadas";

                DataTable dt = new DataTable();
                dt = (DataTable)Session["listaNormas"];
                DataView dv = dt.DefaultView;

                dv.RowFilter = "";
                DataTable dtListaNormas = dv.ToTable();

                gdvNorma.DataSource = dtListaNormas;
                gdvNorma.DataBind();
            }
        }
    }
}