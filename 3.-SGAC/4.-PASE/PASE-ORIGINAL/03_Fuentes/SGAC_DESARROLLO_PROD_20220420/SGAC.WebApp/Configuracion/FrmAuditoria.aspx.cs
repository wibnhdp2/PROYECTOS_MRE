using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.Accesorios;
using System.Data;
using SGAC.Controlador;
using SGAC.Configuracion.Sistema.BL;
using SGAC.Configuracion.Seguridad.BL;

namespace SGAC.WebApp.Configuracion
{
    public partial class Auditoria : MyBasePage
    {
        #region CAMPOS
        private string strNombreEntidad = "AUDITORIA";
        private string strVariableDt = "Auditoria_Tabla";
        private string strVariableIndice = "Auditoria_Indice";
        #endregion

        #region Eventos
        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblUserName.Text = Util.GetSessionVariableValue(Session, Constantes.CONST_SESION_USUARIO);

            ctrlToolBarConsulta.VisibleIButtonBuscar = true;
            ctrlToolBarConsulta.VisibleIButtonCancelar = true;
            ctrlToolBarConsulta.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);
            ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);

            ctrlToolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarConsulta.btnCancelar.Text = "    Limpiar";

            ctrlOficinaConsular.AutoPostBack = true;

            ctrlOficinaConsular.ddlOficinaConsular.SelectedIndexChanged += new EventHandler(ddlOficinaConsular_SelectedIndexChanged);        

            if (!Page.IsPostBack)
            {
                CargarListadosDesplegables();
                CargarDatosIniciales();

                string StrScript = string.Empty;
                StrScript = @"$(function(){{
                                    DisableTabIndex(1);
                                }});";
                StrScript = string.Format(StrScript);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "DisableTabIndex1", StrScript, true);
            }
        }

        protected void gdvAuditoria_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strScript = string.Empty;
            int intSeleccionado = Convert.ToInt32(e.CommandArgument);

            Session[strVariableIndice] = intSeleccionado;

            if (e.CommandName == "Consultar")
            {
                HabilitarMantenimiento(false);
                PintarSeleccionado(intSeleccionado);
                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
                Comun.EjecutarScript(Page, strScript);
            }
        }

        void ddlOficinaConsular_SelectedIndexChanged(object sender, EventArgs e)
        {
            int intOficinaSeleccionada = 0;
            if (ctrlOficinaConsular.SelectedIndex > 0)
            {
                intOficinaSeleccionada = Convert.ToInt32(ctrlOficinaConsular.SelectedValue);
            }

            //Proceso p = new Proceso();
            //object[] arrParametros = new object[1];
            //arrParametros[0] = intOficinaSeleccionada;
            //DataTable dtUsuarios = (DataTable)p.Invocar(ref arrParametros, "SGAC.BE.SE_USUARIO", "LISTAR");

            UsuarioConsultasBL objUsuarioConsultaBL = new UsuarioConsultasBL();
            DataTable dtUsuarios = new DataTable();

            dtUsuarios = objUsuarioConsultaBL.ObtenerLista(intOficinaSeleccionada);

            Util.CargarDropDownList(ddlUsuario, dtUsuarios, "usua_vAlias", "usua_sUsuarioId", true, " - TODOS - ");
        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            CargarGrilla();
            updConsulta.Update();
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            if (txtFecInicio.Text.Trim().Length == 0 || txtFecFin.Text.Trim().Length == 0)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_VACIA, true, Enumerador.enmTipoMensaje.WARNING);
                updConsulta.Update();
                return;
            }
            //CONST_VALIDACION_FECHA_NO_VALIDA

            if (Comun.EsFecha(txtFecInicio.Text.Trim()) == false)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_INICIAL, true, Enumerador.enmTipoMensaje.WARNING);
                updConsulta.Update();
                return;
            }
            if (Comun.EsFecha(txtFecFin.Text.Trim()) == false)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_FECHA_FINAL, true, Enumerador.enmTipoMensaje.WARNING);
                updConsulta.Update();
                return;
            }


            DateTime datFechaInicio = new DateTime();
            DateTime datFechaFin = new DateTime();

            if (!DateTime.TryParse(txtFecInicio.Text, out datFechaInicio))
            {
                datFechaInicio = Comun.FormatearFecha(txtFecInicio.Text);
            }
            if (!DateTime.TryParse(txtFecFin.Text, out datFechaFin))
            {
                datFechaFin = Comun.FormatearFecha(txtFecFin.Text);
            }

            if (datFechaInicio > datFechaFin)
            {
                ctrlPaginador.InicializarPaginador();
                ctrlPaginador.Visible = false;
                gdvAuditoria.DataSource = null;
                gdvAuditoria.DataBind();

                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);                
            }
            else
            {
                ctrlPaginador.InicializarPaginador();
                CargarGrilla();                
            }

            updConsulta.Update();
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            LimpiarDatosConsulta();
        }

        protected void gdvAuditoria_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[0].Text.Trim() != "&nbsp;")
                {
                    e.Row.Cells[0].Text = (Comun.FormatearFecha(e.Row.Cells[0].Text)).ToString(ConfigurationManager.AppSettings["FormatoFechaLarga"]);
                }
            }
        }

        #endregion

        #region Métodos

        private void CargarListadosDesplegables()
        {
            object[] arrParametros = new object[1];

            /* Filtro por atributos del usuario */
            if ((int)Session[Constantes.CONST_SESION_ACCESO_ID] == (int)Enumerador.enmAccesoUsuario.INDIVIDUAL ||
                (int)Session[Constantes.CONST_SESION_OFICINACONSULAR_ID] != Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                /* Consulta */
                ctrlOficinaConsular.Cargar(false, false);
            }
            else
            {
                ctrlOficinaConsular.Cargar(false, true, "- TODAS -");
            }

            ctrlOficinaConsular.SelectedValue = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]).ToString();

            UsuarioConsultasBL objBL = new UsuarioConsultasBL();
            DataTable dtUsuarios = objBL.ObtenerLista(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));

            if (dtUsuarios.Rows.Count > 0)
            {
                Util.CargarDropDownList(ddlUsuario, dtUsuarios, "usua_vAlias", "usua_sUsuarioId", true, " - TODOS - ");
            }
            else
            {
                Util.CargarParametroDropDownList(ddlUsuario, new DataTable(), true);
            }

            // PARAMETROS            
            Util.CargarParametroDropDownList(ddlTipoResultado, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.SEGURIDAD_RESULTADO_OPERACION), true, " - TODAS - ");

            // FORMULARIOS
            FormularioConsultasBL objConsulta = new FormularioConsultasBL();
            DataTable dt = objConsulta.ListarPorAplicacion((int)Enumerador.enmAplicacion.WEB, (int)Enumerador.enmVisibilidad.VISIBLE);
            Util.CargarDropDownList(ddlFormulario, dt, "form_vDescripcion", "form_sFormularioId", true, " - TODOS - ");

            // Mantenimiento
            Util.CargarParametroDropDownList(ddlResultado, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.SEGURIDAD_RESULTADO_OPERACION), true, "");
            Util.CargarParametroDropDownList(ddlOperacionTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.SEGURIDAD_TIPO_OPERACION), true, "");
            Util.CargarDropDownList(ddlFormularioDetalle, dt, "form_vDescripcion", "form_sFormularioId", true, "");
            Util.CargarParametroDropDownList(ddlTabla, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONFIG_TABLAS), true, "");
        }

        private void CargarDatosIniciales()
        {
            LimpiarDatosConsulta();
            HabilitarMantenimiento(false);
        }

        private void CargarGrilla()
        {
            DataTable dt = new DataTable();

            int intTotalRegistros = 0, intTotalPaginas = 0;
            int intOficinaSeleccionada = 0;

            DropDownList ComboConsulta = (DropDownList)ctrlOficinaConsular.FindControl("ddlOficinaConsular");
            int IntCuentaOFc = Convert.ToInt32(ComboConsulta.Items.Count.ToString());
            if (IntCuentaOFc > 0)
                intOficinaSeleccionada = Convert.ToInt32(ctrlOficinaConsular.SelectedValue);

            int intUsuarioId = 0;
            if (ddlUsuario.SelectedIndex > 0)
                intUsuarioId = Convert.ToInt32(ddlUsuario.SelectedValue);

            int intOperacionId = 0;
            if (ddlOperacionCon.SelectedIndex > 0)
               intOperacionId  = Convert.ToInt32(ddlOperacionCon.SelectedValue);

            int intResultadoId = 0;
            if (ddlTipoResultado.SelectedIndex > 0)
                intResultadoId = Convert.ToInt32(ddlTipoResultado.SelectedValue);
            
            int intFormularioId = 0;
            if (ddlFormulario.SelectedIndex > 0)
                intFormularioId = Convert.ToInt32(ddlFormulario.SelectedValue);

            DateTime datFechaInicio = Comun.FormatearFecha(txtFecInicio.Text);           
            DateTime datFechaFin = Comun.FormatearFecha(txtFecFin.Text);

            try
            {
                AuditoriaConsultasBL objAuditoriaBL = new AuditoriaConsultasBL();
                dt = objAuditoriaBL.Consultar(
                    ctrlPaginador.PaginaActual, Constantes.CONST_CANT_REGISTRO,
                    ref intTotalRegistros, ref intTotalPaginas, intOficinaSeleccionada,
                    intUsuarioId, intOperacionId, intResultadoId, intFormularioId,
                    datFechaInicio, datFechaFin);

                if (dt.Rows.Count > 0)
                {
                    Session[strVariableDt] = dt;

                    gdvAuditoria.SelectedIndex = -1;
                    gdvAuditoria.DataSource = dt;
                    gdvAuditoria.DataBind();

                    ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + intTotalRegistros, true, Enumerador.enmTipoMensaje.INFORMATION);

                    ctrlPaginador.TotalResgistros = intTotalRegistros;
                    ctrlPaginador.TotalPaginas = intTotalPaginas;

                    ctrlPaginador.Visible = false;
                    if (ctrlPaginador.TotalPaginas > 1)
                    {
                        ctrlPaginador.Visible = true;
                    }
                }
                else
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                    gdvAuditoria.SelectedIndex = -1;
                    gdvAuditoria.DataSource = null;
                    gdvAuditoria.DataBind();
                }
            }
            catch
            {
                string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_MENSAJE_OPERACION_FALLIDA);
                Comun.EjecutarScript(Page, strScript);
            }
        }

        private object[] ObtenerFiltro()
        {
            int intTotalRegistros = 0, intTotalPaginas = 0;
            int intOficinaSeleccionada = 0;
            object[] arrParametros = new object[11];

            //Evaluar el numero de registros que tiene el combo
            DropDownList ComboConsulta = (DropDownList)ctrlOficinaConsular.FindControl("ddlOficinaConsular");
            int IntCuentaOFc = Convert.ToInt32(ComboConsulta.Items.Count.ToString());            

            if (IntCuentaOFc > 0)
            {
                intOficinaSeleccionada = Convert.ToInt32(ctrlOficinaConsular.SelectedValue);
            }
            else
            {
                arrParametros[4] = 0;
            }
            
            arrParametros[0] = ctrlPaginador.PaginaActual;
            arrParametros[1] = Constantes.CONST_CANT_REGISTRO;
            arrParametros[2] = intTotalRegistros;
            arrParametros[3] = intTotalPaginas;
            arrParametros[4] = intOficinaSeleccionada;

            if (ddlUsuario.SelectedIndex > 0)
            {
                arrParametros[5] = Convert.ToInt32(ddlUsuario.SelectedValue);
            }
            else
            {
                arrParametros[5] = 0;
            }

            if (ddlOperacionCon.SelectedIndex > 0)
            {
                arrParametros[6] = Convert.ToInt32(ddlOperacionCon.SelectedValue);
            }
            else
            {
                arrParametros[6] = 0;
            }

            if (ddlTipoResultado.SelectedIndex > 0)
                arrParametros[7] = Convert.ToInt32(ddlTipoResultado.SelectedValue);
            else
                arrParametros[7] = 0;

            if (ddlFormulario.SelectedIndex > 0)
            {
                arrParametros[8] = Convert.ToInt32(ddlFormulario.SelectedValue);
            }
            else
            {
                arrParametros[8] = 0;
            }

            DateTime datFechaInicio = new DateTime();
            DateTime datFechaFin = new DateTime();

            if (!DateTime.TryParse(txtFecInicio.Text, out datFechaInicio))
            {
                datFechaInicio = Comun.FormatearFecha(txtFecInicio.Text);
            }
            arrParametros[9] = datFechaInicio;

            if (!DateTime.TryParse(txtFecFin.Text, out datFechaFin))
            {
                datFechaFin = Comun.FormatearFecha(txtFecFin.Text);
            }
            arrParametros[10] = datFechaFin;

            return arrParametros;
        }

        private void HabilitarMantenimiento(bool bolHabilitar = true)
        {
            txtUsuarioDetalle.Enabled = false;
            txtFechaRegistro.Enabled = false;
            ddlOperacionTipo.Enabled = false;            
            ddlFormularioDetalle.Enabled = false;
            ddlTabla.Enabled = false;
            txtCampos.Enabled = false;
            txtValores.Enabled = false;
            txtHostName.Enabled = false;
            txtIP.Enabled = false;
        }

        private void PintarSeleccionado(int intSeleccionado)
        {
            DataRow drSeleccionado = ObtenerFilaSeleccionada();
            if (drSeleccionado != null)
            {
                txtUsuarioDetalle.Text = drSeleccionado["audi_vUsuario"].ToString();
                if (drSeleccionado["audi_dCreaFecha"] != null)
                {
                    //txtFechaRegistro.Text = Convert.ToDateTime(drSeleccionado["audi_dCreaFecha"]).ToString(ConfigurationManager.AppSettings["FormatoFechas"] + " HH:mm:ss");
                    txtFechaRegistro.Text = Comun.FormatearFecha(drSeleccionado["audi_dCreaFecha"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechaLarga"]);
                }

                int intResultado = Convert.ToInt32(drSeleccionado["audi_sOperacionResultadoId"]);
                if (intResultado == (int)Enumerador.enmResultadoAuditoria.OK)
                {
                    Util.CargarParametroDropDownList(ddlOperacionTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.SEGURIDAD_TIPO_OPERACION), true, "");
                }
                else
                {
                    Util.CargarParametroDropDownList(ddlOperacionTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.SEGURIDAD_RESULTADO_INCIDENCIA), true, "");
                }

                ddlResultado.SelectedValue = intResultado.ToString();

                if (drSeleccionado["audi_sOperacionTipoId"] != null)
                    ddlOperacionTipo.SelectedValue = drSeleccionado["audi_sOperacionTipoId"].ToString();                

                ddlTabla.SelectedIndex = 0;
                if (drSeleccionado["audi_sTabla"] != null)
                {
                    if (drSeleccionado["audi_sTabla"].ToString() != "0")
                    {
                        ddlTabla.SelectedValue = drSeleccionado["audi_sTabla"].ToString();
                    }
                }

                ddlFormularioDetalle.SelectedIndex = 0;
                if (drSeleccionado["audi_sFormularioId"] != null)
                {
                    if (drSeleccionado["audi_sFormularioId"].ToString() != string.Empty)
                    {
                        if (drSeleccionado["audi_sFormularioId"].ToString() != "0")
                        {
                            ddlFormularioDetalle.SelectedValue = drSeleccionado["audi_sFormularioId"].ToString();
                        }
                    }
                }

                txtComentario.Text = drSeleccionado["audi_vComentario"].ToString();
                txtCampos.Text = drSeleccionado["audi_vCamposNombre"].ToString();
                txtValores.Text = drSeleccionado["audi_vCamposValor"].ToString();
                txtHostName.Text = drSeleccionado["audi_vHostName"].ToString();
                txtIP.Text = drSeleccionado["audi_vDireccionIP"].ToString();                

                updMantenimiento.Update();
            }
        }

        private DataRow ObtenerFilaSeleccionada()
        {
            if (Session != null)
            {
                if (Session[strVariableDt] != null)
                {
                    DataTable dt = (DataTable)Session[strVariableDt];
                    if (Session[strVariableIndice] != null)
                    {
                        int intSeleccionado = (int)Session[strVariableIndice];
                        DataRow dr = dt.Rows[intSeleccionado];
                        return dr;
                    }
                }
            }

            return null;
        }

        private void LimpiarDatosConsulta()
        {
            txtFecInicio.Text = DateTime.Today.ToString("MMM") + "-" + "01" + "-" + DateTime.Today.ToString("yyyy");
            txtFecFin.Text = DateTime.Today.ToString("MMM") + "-" + DateTime.Today.ToString("dd") + "-" + DateTime.Today.ToString("yyyy"); 

            ddlUsuario.SelectedIndex = 0;
            ddlFormulario.SelectedIndex = 0;            
            ddlOperacionCon.SelectedIndex = 0;           
           
            gdvAuditoria.DataSource = null;
            gdvAuditoria.DataBind();

            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
            ctrlPaginador.InicializarPaginador();

            updConsulta.Update();

            txtUsuarioDetalle.Text = "";
            txtFechaRegistro.Text = "";
            ddlOperacionTipo.SelectedIndex = 0;
            ddlFormularioDetalle.SelectedIndex = 0;
            ddlTabla.SelectedIndex = 0;
            txtCampos.Text = "";
            txtValores.Text = "";
            txtHostName.Text = "";
            txtIP.Text = "";

            updMantenimiento.Update();
        }        
        #endregion

        protected void ddlTipoResultado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Comun.ToNullInt32(ddlTipoResultado.SelectedItem.Value) == (int)Enumerador.enmResultadoAuditoria.OK)
            {
                Util.CargarParametroDropDownList(ddlOperacionCon, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.SEGURIDAD_TIPO_OPERACION), true, " - TODAS - ");
            }
            else
            {
                Util.CargarParametroDropDownList(ddlOperacionCon, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.SEGURIDAD_RESULTADO_INCIDENCIA), true, " - TODAS - ");
            }
        }
    }
}