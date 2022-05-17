using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.Registro.Actuacion.BL;
using System.Data;

namespace SGAC.WebApp.Accesorios.SharedControls
{
    public partial class ctrlBusquedaPopup : UserControl
    {
        #region Campos
        private int PAGE_SIZE = Constantes.CONST_CANT_REGISTRO;
        private string strMensajeValidacionVacio = Constantes.CONST_MENSAJE_NO_SELECCION_FILTROS;

        #endregion

        #region Propiedades
        public Enumerador.enmTipoPersona TipoPersona { get; set; }
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
            try
            {
                if (!Page.IsPostBack)
                {
                    HFGUID.Value = PageUniqueId.Replace("-", "");
                    
                    Session["iPersonaId"] = 0;
                    Session["iTipoId"] = 0;

                    Session["iPersonaId" + HFGUID.Value] = 0;
                    Session["iTipoId" + HFGUID.Value] = 0;

                    CargarListadosDesplegables();
                    CargarDatosIniciales();
                    
                    // MDIAZ - Modificar
                    ddlPersonaTipo.SelectedValue = ((int)Enumerador.enmTipoPersona.NATURAL).ToString();
                    ddlPersonaTipo.Enabled = false;
                    // --
                }

                if (hEnter.Value != null)
                {
                    if (this.hEnter.Value == "1")
                        btn_Buscar_Click(null, EventArgs.Empty);
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

        protected void txtNroDocumento_TextChanged(object sender, EventArgs e)
        {
            ValidarBusqueda();
        }

        protected void txtPriApellido_TextChanged(object sender, EventArgs e)
        {
            ValidarBusqueda();
        }

        protected void txtSegApellido_TextChanged(object sender, EventArgs e)
        {
            ValidarBusqueda();
        }

        protected void txtNombres_TextChanged(object sender, EventArgs e)
        {
            ValidarBusqueda();
        }

        protected void btn_Buscar_Click(object sender, EventArgs e)
        {
            InicializarBusqueda();
            if (txtNroDocumento.Text.Trim() == string.Empty &&
                txtPriApellido.Text.Trim() == string.Empty &&
                txtSegApellido.Text.Trim() == string.Empty &&
                txtRazonSocial.Text.Trim() == string.Empty &&
                txtNombres.Text.Trim() == string.Empty)
            {
                ctrlValidacion.MostrarValidacion(strMensajeValidacionVacio);
            }
            else
            {
                if (ddlPersonaTipo.SelectedValue == Convert.ToInt32(Enumerador.enmTipoPersona.JURIDICA).ToString())
                {
                    if ((txtNroDocumento.Text.Trim().Length > 0) || (txtRazonSocial.Text.Trim().Length >= 3))
                    {
                        CargarGrillaEmpresa(txtNroDocumento.Text.Trim(), txtRazonSocial.Text.Trim());
                        MuestraGrid();
                    }
                    else
                    {
                        ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_MIN_3_CARACTERES, true, Enumerador.enmTipoMensaje.WARNING);
                        Grd_Empresa.DataSource = null;
                        Grd_Empresa.DataBind();
                    }
                }
                else
                {
                    if ((txtNroDocumento.Text.Trim().Length > 0) ||
                        (txtPriApellido.Text.Trim().Length >= 3) || 
                        (txtSegApellido.Text.Trim().Length >= 3) || 
                        (txtNombres.Text.Trim().Length >= 3))
                    {
                        CargarGrillaSolicitante(txtNroDocumento.Text.Trim(), txtPriApellido.Text.Trim(), txtSegApellido.Text.Trim(), txtNombres.Text.Trim());
                        MuestraGrid();
                    }
                    else
                    {
                        ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_MIN_3_CARACTERES, true, Enumerador.enmTipoMensaje.WARNING);
                        Grd_Solicitante.DataSource = null;
                        Grd_Solicitante.DataBind();
                    }
                }
            }

            this.hEnter.Value = "0";
            UpdGrvPaginada.Update();
        }

        // No se usa - MDIAZ
        protected void btn_NewRUNE_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlPersonaTipo.SelectedValue == ((int)Enumerador.enmTipoPersona.NATURAL).ToString())
                {
                    Response.Redirect("~/Registro/FrmRegistroPersona.aspx?GUID=" + HFGUID.Value, false);
                }
                else
                {
                    Response.Redirect("~/Registro/FrmRegistroEmpresa.aspx?GUID=" + HFGUID.Value, false);
                }
            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("~/PageError/GenericErrorPage.aspx");
            }
        }

        protected void ddlPersonaTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;

            MuestraGrid();
            int intPersonaTipoId = Convert.ToInt32(ddlPersonaTipo.SelectedValue);
            MostrarDatosPorTipoPersona(intPersonaTipoId);
        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            if (ddlPersonaTipo.SelectedValue == Convert.ToInt32(Enumerador.enmTipoPersona.JURIDICA).ToString())
            {
                CargarGrillaEmpresa(txtNroDocumento.Text.Trim(), txtRazonSocial.Text.Trim());
            }
            else
            {
                CargarGrillaSolicitante(txtNroDocumento.Text.Trim(), txtPriApellido.Text.Trim(), txtSegApellido.Text.Trim(), txtNombres.Text.Trim());
            }
            UpdGrvPaginada.Update();
        }

        protected void Grd_Solicitante_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Select")
            {
                BE.RE_PERSONA objPersona = new BE.RE_PERSONA();
                objPersona.pers_iPersonaId = Convert.ToInt64(Grd_Solicitante.Rows[index].Cells[0].Text);
                objPersona.pers_sPersonaTipoId = Convert.ToInt16(Grd_Solicitante.Rows[index].Cells[1].Text);
                objPersona.pers_vApellidoPaterno = Grd_Solicitante.Rows[index].Cells[4].Text;
                objPersona.pers_vApellidoMaterno = Grd_Solicitante.Rows[index].Cells[5].Text;
                objPersona.pers_vNombres = Grd_Solicitante.Rows[index].Cells[6].Text;

                if (!(Grd_Solicitante.Rows[index].Cells[9].Text.Trim() == "&nbsp;" || Grd_Solicitante.Rows[index].Cells[9].Text == string.Empty))
                {
                    objPersona.pers_dNacimientoFecha = Comun.FormatearFecha(Grd_Solicitante.Rows[index].Cells[9].Text);
                }

                Session["OBJ_RE_PERSONA"] = objPersona;

                BE.RE_PERSONAIDENTIFICACION objIdentificacion = new BE.RE_PERSONAIDENTIFICACION();
                objIdentificacion.peid_iPersonaId = Convert.ToInt64(Grd_Solicitante.Rows[index].Cells[0].Text);
                objIdentificacion.peid_sDocumentoTipoId = Convert.ToInt16(Grd_Solicitante.Rows[index].Cells[2].Text);
                objIdentificacion.peid_vDocumentoNumero = Grd_Solicitante.Rows[index].Cells[11].Text;
                Session["OBJ_RE_PERSONAIDENTIFICACION"] = objPersona;

                //Session["iPersonaId"] = Grd_Solicitante.Rows[index].Cells[0].Text;
                //Session["iTipoId"] = ddlPersonaTipo.SelectedValue;

                // -- MODIFICAR
                //Session["iPersonaTipoId"] = Grd_Solicitante.Rows[index].Cells[1].Text;
                //Session["iDocumentoTipoId"] = Grd_Solicitante.Rows[index].Cells[2].Text;
                //Session["DescTipDoc"] = Grd_Solicitante.Rows[index].Cells[3].Text;
                //Session["ApePat"] = Page.Server.HtmlDecode(Grd_Solicitante.Rows[index].Cells[4].Text);
                //Session["ApeMat"] = Page.Server.HtmlDecode(Grd_Solicitante.Rows[index].Cells[5].Text);
                //Session["Nombres"] = Page.Server.HtmlDecode(Grd_Solicitante.Rows[index].Cells[6].Text); // Cual es la diferencia?
                Session["Nombre"] = Page.Server.HtmlDecode(Grd_Solicitante.Rows[index].Cells[7].Text); // Cual es la diferencia?                
                //Session["FecNac"] = Grd_Solicitante.Rows[index].Cells[9].Text;
                //Session["NroDoc"] = Grd_Solicitante.Rows[index].Cells[12].Text;
                //Session["PER_NACIONALIDAD"] = Grd_Solicitante.Rows[index].Cells[13].Text;
                Session["flgModoBusquedaAct"] = false;

                Session["iPersonaId" + HFGUID.Value] = Grd_Solicitante.Rows[index].Cells[0].Text;
                Session["iTipoId" + HFGUID.Value] = ddlPersonaTipo.SelectedValue;

                Session["iPersonaTipoId" + HFGUID.Value] = Grd_Solicitante.Rows[index].Cells[1].Text;
                Session["iDocumentoTipoId" + HFGUID.Value] = Grd_Solicitante.Rows[index].Cells[2].Text;
                Session["DescTipDoc" + HFGUID.Value] = Grd_Solicitante.Rows[index].Cells[3].Text;
                Session["ApePat" + HFGUID.Value] = Page.Server.HtmlDecode(Grd_Solicitante.Rows[index].Cells[4].Text);
                Session["ApeMat" + HFGUID.Value] = Page.Server.HtmlDecode(Grd_Solicitante.Rows[index].Cells[5].Text);
                Session["Nombres" + HFGUID.Value] = Page.Server.HtmlDecode(Grd_Solicitante.Rows[index].Cells[6].Text); // Cual es la diferencia?
                Session["FecNac" + HFGUID.Value] = Grd_Solicitante.Rows[index].Cells[9].Text;
                Session["NroDoc" + HFGUID.Value] = Grd_Solicitante.Rows[index].Cells[12].Text;
                Session["PER_NACIONALIDAD" + HFGUID.Value] = Grd_Solicitante.Rows[index].Cells[13].Text;

                
                Comun.EjecutarScript(Page, "window.parent.close_ModalPopup('MainContent_btnRealizarBusqueda');");
            }
        }

        protected void Grd_Empresa_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Select")
            {
                BE.RE_EMPRESA objEmpresa = new BE.RE_EMPRESA();
                objEmpresa.empr_iEmpresaId = Convert.ToInt64(Grd_Empresa.Rows[index].Cells[0].Text);
                objEmpresa.empr_sTipoEmpresaId = Convert.ToInt16(Grd_Empresa.Rows[index].Cells[1].Text);
                objEmpresa.empr_sTipoDocumentoId = Convert.ToInt16(Grd_Empresa.Rows[index].Cells[2].Text);
                objEmpresa.empr_vRazonSocial = Grd_Empresa.Rows[index].Cells[4].Text;
                objEmpresa.empr_vNumeroDocumento = Grd_Empresa.Rows[index].Cells[5].Text;
                objEmpresa.empr_vActividadComercial = Grd_Empresa.Rows[index].Cells[6].Text;
                objEmpresa.empr_vTelefono = Grd_Empresa.Rows[index].Cells[7].Text;
                objEmpresa.empr_vCorreo = Grd_Empresa.Rows[index].Cells[8].Text;
                objEmpresa.empr_cEstado = Grd_Empresa.Rows[index].Cells[9].Text;
                Session["OBJ_RE_EMPRESA"] = objEmpresa;
                //Session["iPersonaId"] = Grd_Empresa.Rows[index].Cells[0].Text;
                //Session["iTipoId"] = ddlPersonaTipo.SelectedValue;

                Session["bEsEdicion"] = "1"; // Revisar

                // -- MODIFICAR
                //Session["iPersonaTipoId"] = Grd_Empresa.Rows[index].Cells[1].Text;
                //Session["iDocumentoTipoId"] = Grd_Empresa.Rows[index].Cells[2].Text;
                //Session["DescTipDoc"] = Grd_Empresa.Rows[index].Cells[3].Text;
                //Session["ApePat"] = Grd_Empresa.Rows[index].Cells[4].Text;
                //Session["ApeMat"] = string.Empty;
                //Session["NroDoc"] = Grd_Empresa.Rows[index].Cells[5].Text;
                Session["Nombres"] = string.Empty;
                Session["flgModoBusquedaAct"] = false;

                //-----------------------------------------
                Session["iPersonaTipoId" + HFGUID.Value] = Grd_Empresa.Rows[index].Cells[1].Text;
                Session["DescTipDoc" + HFGUID.Value] = Grd_Empresa.Rows[index].Cells[3].Text;
                Session["ApePat" + HFGUID.Value] = Grd_Empresa.Rows[index].Cells[4].Text;
                Session["ApeMat" + HFGUID.Value] = string.Empty;
                Session["NroDoc" + HFGUID.Value] = Grd_Empresa.Rows[index].Cells[5].Text;
                Session["Nombres" + HFGUID.Value] = string.Empty;
                Session["iPersonaId" + HFGUID.Value] = Grd_Empresa.Rows[index].Cells[0].Text;
                Session["iTipoId" + HFGUID.Value] = ddlPersonaTipo.SelectedValue;
                Session["iDocumentoTipoId" + HFGUID.Value] = Grd_Empresa.Rows[index].Cells[2].Text;


                Comun.EjecutarScript(Page, "window.parent.close_ModalPopup('MainContent_btnRealizarBusqueda');");
            }
        }

        #endregion

        #region Métodos
        private void CargarDatosIniciales()
        {
            Session["iTipoId"] = 0;
            Session["PER_NACIONALIDAD"] = 0;
            ctrlValidacion.MostrarValidacion("", false);
            ddlPersonaTipo.SelectedValue = ((int)Enumerador.enmTipoPersona.NATURAL).ToString();
        }
        private void CargarListadosDesplegables()
        {
            Util.CargarParametroDropDownList(ddlPersonaTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_TIPO));
        }
        private void CargarGrillaSolicitante(string StrNroDoc, string StrApePat, string StrApeMat, string strNombres)
        {
            int IntTotalCount = 0;
            int IntTotalPages = 0;

            ctrlValidacion.MostrarValidacion("", false);

            DataTable DtRecurrente = new DataTable();

            ActuacionConsultaBL objBL = new ActuacionConsultaBL();
            DtRecurrente = objBL.RecurrenteConsultar(
                                    (int)Enumerador.enmPersonaConsulta.ACTUACION,
                                    StrNroDoc, StrApePat, StrApeMat, strNombres,
                                    ctrlPaginador.PaginaActual,
                                    PAGE_SIZE,
                                    ref IntTotalCount, ref IntTotalPages);

            ctrlPaginador.Visible = false;
            if (DtRecurrente.Rows.Count > 0)
            {
                Grd_Solicitante.DataSource = DtRecurrente;
                Grd_Solicitante.DataBind();

                ctrlPaginador.TotalResgistros = IntTotalCount;
                ctrlPaginador.TotalPaginas = IntTotalPages;

                //Comun.ObtenerMensajeTotalRegistros(ctrlPaginador.PaginaActual, IntTotalCount, PAGE_SIZE)
                ctrlValidacion.MostrarValidacion(
                    Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + " " + IntTotalCount,
                    true, Enumerador.enmTipoMensaje.INFORMATION);

                if (ctrlPaginador.TotalPaginas > 1)
                {
                    ctrlPaginador.Visible = true;
                }

                TextBox txtPage = (TextBox)ctrlPaginador.FindControl("txtPagina");
            }
            else
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                Grd_Solicitante.DataSource = null;
                Grd_Solicitante.DataBind();
            }
        }
        private void CargarGrillaEmpresa(string StrNroDoc, string StrRazonSocial)
        {
            int IntTotalCount = 0;
            int IntTotalPages = 0;

            ctrlValidacion.MostrarValidacion("", false);

            ActuacionConsultaBL objBL = new ActuacionConsultaBL();
            DataTable DtEmpresa = new DataTable();

            DtEmpresa = objBL.EmpresaConsultar(
                                    StrNroDoc.Trim(),
                                    StrRazonSocial.Trim(),
                                    ctrlPaginador.PaginaActual,
                                    PAGE_SIZE,
                                    ref IntTotalCount, ref IntTotalPages);

            ctrlPaginador.Visible = false;
            if (DtEmpresa.Rows.Count > 0)
            {
                Grd_Empresa.DataSource = DtEmpresa;
                Grd_Empresa.DataBind();

                ctrlPaginador.TotalResgistros = IntTotalCount;
                ctrlPaginador.TotalPaginas = IntTotalPages;

                ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + ctrlPaginador.TotalResgistros, true, Enumerador.enmTipoMensaje.INFORMATION);

                if (ctrlPaginador.TotalPaginas > 1)
                {
                    ctrlPaginador.Visible = true;
                }
            }
            else
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                Grd_Empresa.DataSource = null;
                Grd_Empresa.DataBind();
            }
        }
        private void MuestraGrid()
        {
            if (ddlPersonaTipo.SelectedValue == Convert.ToInt32(Enumerador.enmTipoPersona.JURIDICA).ToString())
            {
                Grd_Empresa.Visible = true;
                Grd_Solicitante.Visible = false;
                Grd_Solicitante.DataSource = null;
                Grd_Solicitante.DataBind();
            }
            else
            {
                Grd_Empresa.Visible = false;
                Grd_Solicitante.Visible = true;
                Grd_Empresa.DataSource = null;
                Grd_Empresa.DataBind();
            }
        }
        private void InicializarBusqueda()
        {
            Grd_Solicitante.DataSource = null;
            Grd_Solicitante.DataBind();

            Grd_Empresa.DataSource = null;
            Grd_Empresa.DataBind();

            ctrlPaginador.InicializarPaginador();
        }
        private void ValidarBusqueda()
        {
            if ((txtNroDocumento.Text.Length == 0) && (txtPriApellido.Text.Length == 0) && (txtSegApellido.Text.Length == 0))
            {
                ctrlValidacion.MostrarValidacion("", false);
                return;
            }
        }

        private void MostrarDatosPorTipoPersona(int intTipoPersona)
        {
            if (intTipoPersona == (int)Enumerador.enmTipoPersona.NATURAL)
            {
                lblPriApellido.Text = "Primer Apellido: ";
                txtPriApellido.Text = string.Empty;
                lblSegApellido.Visible = true;
                txtSegApellido.Visible = true;
                lblNombres.Visible = true;
                txtNombres.Visible = true;
                btn_NewRUNE.Text = "Nuevo RUNE";
                btn_NewRUNE.Width = 110;

                pnlPersonaNatural.Visible = true;
                pnlPersonaJuridica.Visible = false;
            }
            else
            {
                lblPriApellido.Text = "Razón Social: ";

                txtRazonSocial.Text = string.Empty;

                btn_NewRUNE.Text = "Nuevo Persona Jurídica";
                btn_NewRUNE.Width = 200;

                pnlPersonaNatural.Visible = false;
                pnlPersonaJuridica.Visible = true;
            }
        }
        #endregion

        //------------------------------------------
        //Fecha: 28/08/2018
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Crear GUID 
        //------------------------------------------        
        private string _pageUniqueId = Guid.NewGuid().ToString();

        public string PageUniqueId
        {
            get { return _pageUniqueId; }
        }

        protected override object SaveControlState()
        {
            //Use an object array to store multiple values.
            //This can be expanded to store business objects instead
            //of just one value such as CustomerID.
            var controlState = new object[2];
            controlState[0] = base.SaveControlState();
            controlState[1] = _pageUniqueId;
            return controlState;
        }

        protected override void LoadControlState(object savedState)
        {
            var controlState = (object[])savedState;
            base.LoadControlState(controlState[0]);
            _pageUniqueId = (string)(controlState[1]);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.RegisterRequiresControlState(this);
        }

        //------------------------------------------        

    }
}