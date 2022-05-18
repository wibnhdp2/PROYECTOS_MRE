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

namespace SGAC.WebApp.Configuracion
{
    public partial class FrmPais : System.Web.UI.Page
    {
        #region CAMPOS
        private string strNombreEntidad = "PAIS";
        private string strVariableAccion = "Pais_Accion";
        private string strVariableDt = "Pais_Tabla";
        private string strVariableIndice = "Pais_Indice";
        #endregion


        #region Eventos

        private void Page_Init(object sender, System.EventArgs e)
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
                GridView[] arrGridView = { gdvPais };
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }
        }

        protected void gdvPais_RowCommand(object sender, GridViewCommandEventArgs e)
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

                strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
            }

            Comun.EjecutarScript(Page, strScript);
        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            Session[strVariableDt] = new DataTable();

            BindGrid(txtConsultaNombrePais.Text, Convert.ToInt16(ddlConsultaContinente.SelectedValue), Convert.ToInt16(ddlConsultaIdioma.SelectedValue));
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            Session[strVariableDt] = new DataTable();
            ctrlPaginador.InicializarPaginador();
            BindGrid(txtConsultaNombrePais.Text, Convert.ToInt16(ddlConsultaContinente.SelectedValue), Convert.ToInt16(ddlConsultaIdioma.SelectedValue));
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            chkActivo.Checked = true;
            txtConsultaNombrePais.Text = "";
            ddlConsultaContinente.SelectedIndex = 0;
            ddlConsultaIdioma.SelectedIndex = 0;
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

            PaisConsultasBL BL = new PaisConsultasBL();

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];
            if (enmAccion == Enumerador.enmAccion.INSERTAR)
            {
                DataTable dtPais = new DataTable();
                int IntTotalCount = 0;
                int IntTotalPages = 0;

                string strNombrePais = txtregNombrePais.Text.Trim();

                dtPais = BL.Consultar_Pais(0, "A", "1", 1, "N", ref IntTotalCount, ref IntTotalPages, 0, strNombrePais);

                if (IntTotalCount > 0)
                {
                    strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "País", "Ya existe el nombre del país.", false, 190, 250);
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
            ctrlToolBarMantenimiento.btnGrabar.Enabled = false;
            Comun.EjecutarScript(Page, Util.ActivarTab(0, "Consulta") + Util.NombrarTab(0, Constantes.CONST_TAB_INICIAL) + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }

        protected void ddlregMonedaOficial_SelectedIndexChanged(object sender, EventArgs e)
        {
            VisualizarCodigoSimboloMoneda();
        }

        #endregion

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
            DataTable dtContinente = new DataTable();

            dtContinente = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.CONTINENTE);

            Util.CargarParametroDropDownList(ddlConsultaContinente, dtContinente, true);
            Util.CargarParametroDropDownList(ddlregContinente, dtContinente, true);
            Util.CargarParametroDropDownList(ddlregMonedaOficial, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.MONEDA), true);
            //--------------------------------
            DataTable dtIdioma = new DataTable();

            dtIdioma = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.TRADUCCION_IDIOMA);
            dtIdioma.DefaultView.Sort = "descripcion";

            //DataView dv = dtIdioma.DefaultView;
            //DataTable dtOrdenado = dv.ToTable();
            //dtOrdenado.DefaultView.Sort = "descripcion";

            Util.CargarParametroDropDownList(this.ddlConsultaIdioma, dtIdioma, true);
            Util.CargarParametroDropDownList(this.ddlregIdioma, dtIdioma, true);
        }

        private void HabilitarMantenimiento(bool bolHabilitar = true)
        {
            txtregNombrePais.Enabled = bolHabilitar;
            txtregCapital.Enabled = bolHabilitar;
            txtregZonaHoraria.Enabled = bolHabilitar;
            ddlregMonedaOficial.Enabled = bolHabilitar;
            txtregNacionalidad.Enabled = bolHabilitar;
            ddlregContinente.Enabled = bolHabilitar;
            txtregGentilicioFemenino.Enabled = bolHabilitar;
            txtregGentilicioMasculino.Enabled = bolHabilitar;
            txtregISOLetra.Enabled = bolHabilitar;
            txtregISONumero.Enabled = bolHabilitar;
            chkActivoMant.Enabled = bolHabilitar;
            ddlregIdioma.Enabled = bolHabilitar;
        }

        private void LimpiarDatosMantenimiento()
        {
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

            HF_sPaisId.Value = "0";
            txtregNombrePais.Text = "";
            txtregCapital.Text = "";
            txtregZonaHoraria.Text = "";
            ddlregMonedaOficial.SelectedIndex = 0;
            txtregNacionalidad.Text = "";
            ddlregContinente.SelectedIndex = 0;
            txtregGentilicioFemenino.Text = "";
            txtregGentilicioMasculino.Text = "";
            txtregISOLetra.Text = "";
            txtregISONumero.Text = "";
            txtregCodigoMoneda.Text = "";
            txtregSimboloMoneda.Text = "";
            chkActivoMant.Checked = true;
            ddlregIdioma.SelectedIndex = 0;

            gdvPais.DataSource = null;
            gdvPais.DataBind();
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

        private void PintarSeleccionado()
        {
            bool bExisteMoneda = false;

            if (Session != null)
            {
                DataRow drSeleccionado = ObtenerFilaSeleccionada();
                if (drSeleccionado != null)
                {
                    HF_sPaisId.Value = drSeleccionado["PAIS_SPAISID"].ToString();
                    txtregNombrePais.Text = drSeleccionado["PAIS_VNOMBRE"].ToString();
                    txtregCapital.Text = drSeleccionado["PAIS_VCAPITAL"].ToString();
                    txtregZonaHoraria.Text = drSeleccionado["PAIS_VZONAHORARIA"].ToString();

                    if (drSeleccionado["PAIS_SMONEDAID"].ToString() != "0")
                    {
                        ddlregMonedaOficial.SelectedValue = drSeleccionado["PAIS_SMONEDAID"].ToString();
                        bExisteMoneda = true;
                    }
                    else
                    {
                        ddlregMonedaOficial.SelectedIndex = 0;
                    }


                    txtregNacionalidad.Text = drSeleccionado["PAIS_VNACIONALIDAD"].ToString();

                    if (drSeleccionado["PAIS_SCONTINENTEID"].ToString() != "0")
                    {
                        ddlregContinente.Text = drSeleccionado["PAIS_SCONTINENTEID"].ToString();
                    }
                    else
                    {
                        ddlregContinente.SelectedIndex = 0;
                    }

                    txtregGentilicioFemenino.Text = drSeleccionado["PAIS_VGENTILICIO_FEM"].ToString();
                    txtregGentilicioMasculino.Text = drSeleccionado["PAIS_VGENTILICIO_MAS"].ToString();
                    txtregISOLetra.Text = drSeleccionado["PAIS_CLETRA_ISO_3166"].ToString();
                    txtregISONumero.Text = drSeleccionado["PAIS_SNUMERO_ISO_3166"].ToString();

                    if (drSeleccionado["PAIS_CESTADO"].ToString() == ((char)Enumerador.enmEstado.ACTIVO).ToString())
                    {
                        chkActivoMant.Checked = true;
                    }
                    else
                    {
                        chkActivoMant.Checked = false;
                    }


                    //--------------------------------------------
                    if (bExisteMoneda)
                    {
                        VisualizarCodigoSimboloMoneda();
                    }
                    //--------------------------------------------
                    if (drSeleccionado["PAIS_SIDIOMA"].ToString() != "0")
                    {
                        ddlregIdioma.SelectedValue = drSeleccionado["PAIS_SIDIOMA"].ToString();
                    }
                    else
                    {
                        ddlregIdioma.SelectedIndex = 0;
                    }
                    //--------------------------------------------
                    updMantenimiento.Update();
                }
            }
        }

        private SGAC.BE.MRE.SI_PAIS ObtenerEntidadMantenimiento()
        {
            if (Session != null)
            {
                SGAC.BE.MRE.SI_PAIS objParametro = new BE.MRE.SI_PAIS();

                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.INSERTAR)
                {
                    objParametro.pais_sPaisId = Convert.ToInt16(ObtenerFilaSeleccionada()["PAIS_SPAISID"]);
                }
                else
                {
                    objParametro.pais_sPaisId = Convert.ToInt16(HF_sPaisId.Value);
                }

                if ((Enumerador.enmAccion)Session[strVariableAccion] != Enumerador.enmAccion.ELIMINAR)
                {
                    objParametro.pais_vNombre = txtregNombrePais.Text;
                    objParametro.pais_vZonaHoraria = txtregZonaHoraria.Text.ToUpper();
                    objParametro.pais_vCapital = txtregCapital.Text.ToUpper();
                    objParametro.pais_vNacionalidad = txtregNacionalidad.Text.ToUpper();
                    objParametro.pais_vReferenciaMapa = "";
                    objParametro.pais_sMonedaId = Convert.ToInt16(ddlregMonedaOficial.SelectedValue);
                    objParametro.pais_sContinenteId = Convert.ToInt16(ddlregContinente.SelectedValue);
                    objParametro.pais_cLetra_ISO_3166 = txtregISOLetra.Text.ToUpper();
                    objParametro.pais_sNumero_ISO_3166 = Convert.ToInt16(txtregISONumero.Text);
                    objParametro.pais_vGentilicio_Mas = txtregGentilicioMasculino.Text.ToUpper();
                    objParametro.pais_vGentilicio_Fem = txtregGentilicioFemenino.Text.ToUpper();
                    objParametro.pais_sIdiomaId = Convert.ToInt16(ddlregIdioma.SelectedValue);
                }

                objParametro.pais_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParametro.pais_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objParametro.pais_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objParametro.pais_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();

                if (chkActivoMant.Checked)
                {
                    objParametro.pais_cEstado = ((char)Enumerador.enmEstado.ACTIVO).ToString();
                }
                else
                {
                    objParametro.pais_cEstado = ((char)Enumerador.enmEstado.DESACTIVO).ToString();
                }

                return objParametro;
            }

            return null;
        }

        private void BindGrid(string strNombrePais, short sContinenteId, short sIdiomaId)
        {
            DataTable dtPais = new DataTable();

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

            PaisConsultasBL BL = new PaisConsultasBL();

            dtPais = BL.Consultar_Pais(0, strEstado, ctrlPaginador.PaginaActual.ToString(), intPaginaCantidad, "S",
                                        ref IntTotalCount, ref IntTotalPages, sContinenteId, strNombrePais, sIdiomaId);

            Session[strVariableDt] = dtPais;

            if (dtPais.Rows.Count > 0)
            {
                gdvPais.SelectedIndex = -1;
                gdvPais.DataSource = dtPais;
                gdvPais.DataBind();

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

                gdvPais.DataSource = null;
                gdvPais.DataBind();
            }
            updConsulta.Update();
        }

        private void GrabarHandler()
        {
            Int64 intResultado = 0;
            string strScript = string.Empty;

            PaisMantenimientoBL BL = new PaisMantenimientoBL();

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVariableAccion];

            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:
                    intResultado = BL.Insertar(ObtenerEntidadMantenimiento(), Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                    break;
                case Enumerador.enmAccion.MODIFICAR:
                    intResultado = BL.Actualizar(ObtenerEntidadMantenimiento(), Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                    break;
                case Enumerador.enmAccion.ELIMINAR:
                    intResultado = BL.Anular(ObtenerEntidadMantenimiento(), Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                    break;
                case Enumerador.enmAccion.CONSULTAR:
                    break;
            }

            if (intResultado == (int)Enumerador.enmResultadoOperacion.OK)
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
                BindGrid(txtConsultaNombrePais.Text, Convert.ToInt16(ddlConsultaContinente.SelectedValue), Convert.ToInt16(ddlConsultaIdioma.SelectedValue));

                updConsulta.Update();
                updMantenimiento.Update();
            }
            else if (intResultado == (int)Enumerador.enmResultadoOperacion.ERROR)
            {
                Session["Grabo"] = string.Empty;
                strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Error del Sistema. Consulte con el area de soporte técnico");
            }

            Session[strVariableAccion] = Enumerador.enmAccion.INSERTAR;

            Comun.EjecutarScript(Page, strScript);
        }

        private void VisualizarCodigoSimboloMoneda()
        {
            SGAC.Configuracion.Maestro.BL.MonedaConsultaBL BL = new SGAC.Configuracion.Maestro.BL.MonedaConsultaBL();
            DataTable dtMoneda = new DataTable();

            txtregCodigoMoneda.Text = "";
            txtregSimboloMoneda.Text = "";

            if (ddlregMonedaOficial.SelectedIndex > 0)
            {
                int intMonedaId = Convert.ToInt32(ddlregMonedaOficial.SelectedValue);
                int IntTotalPages = 0;

                dtMoneda = BL.Consultar_Moneda(intMonedaId, "A", "1", 1, "N", ref IntTotalPages);
                if (dtMoneda.Rows.Count > 0)
                {
                    txtregCodigoMoneda.Text = dtMoneda.Rows[0]["MONE_VCODIGO"].ToString();
                    txtregSimboloMoneda.Text = dtMoneda.Rows[0]["MONE_VSIMBOLO"].ToString();
                }
            }
            dtMoneda.Dispose();
        }

        #endregion

    }
}