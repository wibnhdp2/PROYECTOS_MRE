using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.Controlador;
using SGAC.Registro.Actuacion.BL;

namespace SGAC.WebApp.Accesorios.SharedControls
{
    public partial class ctrlBusquedaAvanzada : System.Web.UI.UserControl
    {
        string strbackgroundColorFila = "#fbf3bb";
        string strFondoColorPaginaSeleccionada = "#4e102e";
        const string C_MensajeValidacion = "Ingrese un apellido y nombres/ambos apellidos/número de documento de identidad.";

        readonly PagedDataSource _pgsource = new PagedDataSource();
        int _firstIndex, _lastIndex;

        #region Campos
        private int _pageSize = Constantes.CONST_CANT_REGISTRO;
        private string strMensajeValidacionVacio = Constantes.CONST_MENSAJE_NO_SELECCION_FILTROS;
        #endregion

        #region Propiedades
        public Enumerador.enmBusquedaDirecciona Direcciona { get; set; }
        public Enumerador.enmTipoPersona TipoPersona { get; set; }

        //------------------------------------------
        //Fecha: 27/08/2018
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Crear GUID 
        //------------------------------------------        
        private string _pageUniqueId = Guid.NewGuid().ToString();

        public string PageUniqueId
        {
            get { return _pageUniqueId; }
        }

        #endregion

        private int CurrentPage
        {
            get
            {
                if (ViewState["CurrentPage"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["CurrentPage"]);
            }
            set
            {
                ViewState["CurrentPage"] = value;
            }
        }

        #region Clases

        [Serializable]
        class persona
        {
            public string iPersonaId { get; set; }
            public string sPersonaTipoId { get; set; }
            public string sDocumentoTipoId { get; set; }
            public string vDescTipDoc { get; set; }
            public string vApellidoPaterno { get; set; }
            public string vApellidoMaterno { get; set; }
            public string vNombres { get; set; }
            public string vNroDocumento { get; set; }
            public string sNacionalidadId { get; set; }
            public string sGeneroId { get; set; }
            public string pers_vApellidoCasada { get; set; }
            public string vTipoDocumento { get; set; }
            public string vNombre { get; set; }
            public string vNroTipoDocumento { get; set; }
            public string vFecNac { get; set; }
            public string pers_vNacionalidad { get; set; }
        }

        [Serializable]
        class empresa
        {
            public string empr_iEmpresaId { get; set; }
            public string empr_sTipoEmpresaId { get; set; }
            public string empr_sTipoDocumentoId { get; set; }
            public string vDescTipDoc { get; set; }
            public string empr_vActividadComercial { get; set; }
            public string empr_cEstado { get; set; }
            public string empr_vRazonSocial { get; set; }
            public string empr_vNumeroDocumento { get; set; }
            public string empr_vTelefono { get; set; }
            public string empr_vCorreo { get; set; }
            public string empr_vTipoEmpresa { get; set; }
        }

        #endregion

        private List<persona> arrListaPersona
        {
            get
            {
                if (ViewState["ListaPersona"] == null)
                {
                    return null;
                }
                return ((List<persona>)ViewState["ListaPersona"]);
            }
            set
            {
                ViewState["ListaPersona"] = value;
            }

        }

        private List<empresa> arrListaEmpresa
        {
            get
            {
                if (ViewState["ListaEmpresa"] == null)
                {
                    return null;
                }
                return ((List<empresa>)ViewState["ListaEmpresa"]);
            }
            set
            {
                ViewState["ListaEmpresa"] = value;
            }

        }

        private void Page_Init(object sender, System.EventArgs e)
        {
            CurrentPage = 0;
            tabpaginado.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
             try
            {
                if (!Page.IsPostBack)
                {

                    //HFGUID.Value = PageUniqueId.Replace("-", "");
                    CargarListadosDesplegables();
                    CargarDatosIniciales();
                    hdn_tipo_direccion.Value = ((int)Direcciona).ToString();
                    hdn_tipo_persona.Value = ((int)TipoPersona).ToString();

                    if (Direcciona == Enumerador.enmBusquedaDirecciona.RUNE)
                    {
                        ddlPersonaTipo.SelectedValue = hdn_tipo_persona.Value;
                        //ddlPersonaTipo.Enabled = false;
                        MostrarDatosPorTipoPersona(Convert.ToInt32(hdn_tipo_persona.Value));
                    }
                    txtPriApellido.Focus();
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


        protected void btn_Buscar_Click(object sender, EventArgs e)
        {
            try
            {
                InicializarBusqueda();
                if (txtNroDocumento.Text.Trim() == string.Empty &&
                    (txtPriApellido.Text.Trim() == string.Empty &&
                    txtRazonSocial.Text.Trim() == string.Empty) &&
                    txtSegApellido.Text.Trim() == string.Empty &&
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
                            arrListaEmpresa = obtenerArregloEmpresa(txtNroDocumento.Text.Trim(), txtRazonSocial.Text.Trim());
                            CurrentPage = 0;
                            BindDataIntoRepeater();
                            MuestraGrid();

                            if (arrListaEmpresa.Count == 0)
                            {
                                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA, true, Enumerador.enmTipoMensaje.INFORMATION);
                                
                            }
                        }
                        else
                        {
                            ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_MIN_3_CARACTERES, true, Enumerador.enmTipoMensaje.WARNING);
                            limpiarListas();
                        }
                    }
                    else
                    {
                        //*****************************************************************************
                        // Fecha: 15/04/2021
                        // Autor: Miguel Márquez Beltrán
                        // Requerimiento: OBSERVACIONES_SGAC_15042021.doc
                        // Motivo: Filtrar Apellido Paterno y Nombres, mínimo 2 caracteres.
                        //         Filtrar Apellido Paterno y Apellido Materno, mínimo 2 caracteres
                        //         Filtrar Apellido Materno y Nombres, mínimo 2 caracteres.
                        //*****************************************************************************
                        if ((txtNroDocumento.Text.Trim().Length > 0) ||
                            (txtPriApellido.Text.Trim().Length >= 2 && txtNombres.Text.Trim().Length >= 2) ||
                            (txtPriApellido.Text.Trim().Length >= 2 && txtSegApellido.Text.Trim().Length >= 2) ||
                            (txtSegApellido.Text.Trim().Length >= 2 && txtNombres.Text.Trim().Length >= 2))
                        {
                            arrListaPersona = obtenerArregloPersona(txtNroDocumento.Text.Trim(), txtPriApellido.Text.Trim(), txtSegApellido.Text.Trim(), txtNombres.Text.Trim());
                            CurrentPage = 0;
                            BindDataIntoRepeater();
                            MuestraGrid();
                            if (arrListaPersona.Count == 0)
                            {
                                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA, true, Enumerador.enmTipoMensaje.INFORMATION);

                            }
                        }
                        else
                        {
                            ctrlValidacion.MostrarValidacion(C_MensajeValidacion, true, Enumerador.enmTipoMensaje.WARNING);
                            limpiarListas();
                        }
                    }
                }

                this.hEnter.Value = "0";
                UpdGrvPaginada.Update();
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

        protected void btn_NewRUNE_Click(object sender, EventArgs e)
        {
            try
            {
                Session["Nombre"] = null;
                Session["flgModoBusquedaAct"] = null;
                Session["ApePat"] = null;
                Session["ApeMat"] = null;
                Session["ApeCasada"] = null;
                Session["Nombres"] = null;

                Session["DescTipDoc"] = null;
                Session["NroDoc"] = null;
                Session["PER_NACIONALIDAD"] = null;
                Session["iPersonaId"] = null;
                Session["iTipoId"] = null;
                Session["iDocumentoTipoId"] = null;
                Session["iPersonaTipoId"] = null;
                Session["FecNac"] = null;
                Session["PER_GENERO"] = null;
                Session["iCodPersonaId"] = null;
                Session["DescTipDoc_OTRO"] = null;
                Session["DtPersonaAct"] = null;

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

        protected void ddlPersonaTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CurrentPage = 0;
                tabpaginado.Visible = false;

                MuestraGrid();
                int intPersonaTipoId = Convert.ToInt32(ddlPersonaTipo.SelectedValue);
                MostrarDatosPorTipoPersona(intPersonaTipoId);

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

        protected void lbFirst_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            BindDataIntoRepeater();
        }
        protected void lbLast_Click(object sender, EventArgs e)
        {
            CurrentPage = (Convert.ToInt32(ViewState["TotalPages"]) - 1);
            BindDataIntoRepeater();
        }
        protected void lbPrevious_Click(object sender, EventArgs e)
        {
            CurrentPage -= 1;
            BindDataIntoRepeater();
        }
        protected void lbNext_Click(object sender, EventArgs e)
        {
            CurrentPage += 1;
            BindDataIntoRepeater();
        }
        protected void rptPaging_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("newPage")) return;
            CurrentPage = Convert.ToInt32(e.CommandArgument.ToString());
            BindDataIntoRepeater();
        }

        protected void rptPaging_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPage = (LinkButton)e.Item.FindControl("lbPaging");
            if (lnkPage.CommandArgument != CurrentPage.ToString()) return;
            lnkPage.Enabled = false;
            lnkPage.BackColor = Color.FromName(strFondoColorPaginaSeleccionada);
            lnkPage.ForeColor = Color.White;
        }

        protected void rptResultPersona_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {

                switch (e.CommandName)
                {
                    case "select":
                        RepeaterItem item = e.Item;

                        BE.RE_PERSONA objPersona = new BE.RE_PERSONA();
                        //------------------------------------------------------------------
                        LinkButton lnkvNombres = (LinkButton)item.FindControl("lnkvNombres");
                        string strNombres = lnkvNombres.Text;
                        string strPersonaId = lnkvNombres.CommandArgument;

                        LinkButton lnkPersonaTipoId = (LinkButton)item.FindControl("lnkPersonaTipoId");
                        string strPersonaTipoId = lnkPersonaTipoId.Text;

                        LinkButton lnkApePat = (LinkButton)item.FindControl("lnkApePat");
                        string strApePat = lnkApePat.Text;

                        LinkButton lnkApeMat = (LinkButton)item.FindControl("lnkApeMat");
                        string strApeMat = lnkApeMat.Text;

                        LinkButton lnkpers_vApellidoCasada = (LinkButton)item.FindControl("lnkpers_vApellidoCasada");
                        string strApellidoCasada = lnkpers_vApellidoCasada.Text;


                        LinkButton lnkFecNac = (LinkButton)item.FindControl("lnkFecNac");
                        string strFecNac = lnkFecNac.Text;

                        LinkButton lnkDocumentoTipoId = (LinkButton)item.FindControl("lnkDocumentoTipoId");
                        string strDocumentoTipoId = lnkDocumentoTipoId.Text;

                        LinkButton lnkvNroDocumento = (LinkButton)item.FindControl("lnkvNroDocumento");
                        string strNroDocumento = lnkvNroDocumento.Text;
                        Int16 intDocumentoId = Convert.ToInt16(strDocumentoTipoId);

                        LinkButton lnkNombre = (LinkButton)item.FindControl("lnkNombre");
                        string strNombre = lnkNombre.Text;

                        LinkButton lnkDescTipDoc = (LinkButton)item.FindControl("lnkDescTipDoc");
                        string DescTipDoc = lnkDescTipDoc.Text;

                        LinkButton lnksNacionalidadId = (LinkButton)item.FindControl("lnksNacionalidadId");
                        string strNacionalidadId = lnksNacionalidadId.Text;

                        LinkButton lnksGeneroId = (LinkButton)item.FindControl("lnksGeneroId");
                        string strGeneroId = lnksGeneroId.Text;

                        LinkButton lnkvTipoDocumento = (LinkButton)item.FindControl("lnkvTipoDocumento");
                        string strTipoDocumento = lnkvTipoDocumento.Text;
                        //------------------------------------------------------

                        objPersona.pers_iPersonaId = Convert.ToInt64(strPersonaId);
                        objPersona.pers_sPersonaTipoId = Convert.ToInt16(strPersonaTipoId);
                        objPersona.pers_vApellidoPaterno = strApePat;
                        objPersona.pers_vApellidoMaterno = strApeMat;
                        objPersona.pers_vNombres = strNombres;

                        strFecNac = strFecNac.Replace(".", "");

                        if (!(strFecNac == "&nbsp;" || strFecNac == string.Empty))
                        {
                            objPersona.pers_dNacimientoFecha = Comun.FormatearFecha(strFecNac);
                        }
                        //------------------------------------------------------                   
                    


                        //Session["OBJ_RE_PERSONA"] = objPersona;
                        BE.RE_PERSONAIDENTIFICACION objIdentificacion = new BE.RE_PERSONAIDENTIFICACION();
                        objIdentificacion.peid_iPersonaId = Convert.ToInt64(strPersonaId);
                        objIdentificacion.peid_sDocumentoTipoId = intDocumentoId;
                        objIdentificacion.peid_vDocumentoNumero = strNroDocumento;

                        //Session["OBJ_RE_PERSONAIDENTIFICACION"] = objPersona;


                        //Session["DtPersonaAct"] = GetDataPersona(Convert.ToInt64(strPersonaId), intDocumentoId, strNroDocumento);                        
                        Session["GrabarEditar"] = "EDITAR";
                        //Session["Nombre" + HFGUID.Value] = strNombre;
                        //Session["flgModoBusquedaAct"] = false;
                        //Session["ApePat" + HFGUID.Value] = strApePat;
                        //Session["ApeMat" + HFGUID.Value] = strApeMat;
                        //Session["ApeCasada" + HFGUID.Value] = strApellidoCasada;
                        //Session["Nombres" + HFGUID.Value] = strNombres;

                        //Session["DescTipDoc" + HFGUID.Value] = DescTipDoc;
                        //Session["NroDoc" + HFGUID.Value] = strNroDocumento;
                        //Session["PER_NACIONALIDAD" + HFGUID.Value] = strNacionalidadId;
                        //Session["iPersonaId" + HFGUID.Value] = strPersonaId;
                        //Session["iTipoId" + HFGUID.Value] = ddlPersonaTipo.SelectedValue;
                        //Session["iDocumentoTipoId" + HFGUID.Value] = strDocumentoTipoId;
                        //Session["iPersonaTipoId" + HFGUID.Value] = strPersonaTipoId;
                        //Session["FecNac" + HFGUID.Value] = strFecNac;
                        //Session["PER_GENERO" + HFGUID.Value] = strGeneroId;
                        //Session["iCodPersonaId" + HFGUID.Value] = strPersonaId;
                        //Session["DescTipDoc_OTRO" + HFGUID.Value] = strTipoDocumento;


                        //Session["Nombre"] = strNombre;
                        //Session["flgModoBusquedaAct"] = false;
                        //Session["ApePat"] = strApePat;
                        //Session["ApeMat"] = strApeMat;
                        //Session["ApeCasada"] = strApellidoCasada;
                        //Session["Nombres"] = strNombres;

                        //Session["DescTipDoc"] = DescTipDoc;
                        //Session["NroDoc"] = strNroDocumento;
                        //Session["PER_NACIONALIDAD"] = strNacionalidadId;
                        //Session["iPersonaId"] = strPersonaId;
                        //Session["iTipoId"] = ddlPersonaTipo.SelectedValue;
                        //Session["iDocumentoTipoId"] = strDocumentoTipoId;
                        //Session["iPersonaTipoId"] = strPersonaTipoId;
                        //Session["FecNac"] = strFecNac;
                        //Session["PER_GENERO"] = strGeneroId;
                        //Session["iCodPersonaId"] = strPersonaId;
                        //Session["DescTipDoc_OTRO"] = strTipoDocumento;

                        hid_iPersonaId.Value = strPersonaId;
                        int intDirecciona = Convert.ToInt32(hdn_tipo_direccion.Value);

                        //switch (intDirecciona)
                        //{
                        //    case (int)Enumerador.enmBusquedaDirecciona.RUNE:                                
                        //        Response.Redirect("~/Registro/FrmRegistroPersona.aspx?GUID=" + HFGUID.Value, false);
                        //        break;
                        //    case (int)Enumerador.enmBusquedaDirecciona.TRAMITE:                                
                        //        Response.Redirect("~/Registro/FrmTramite.aspx?GUID=" + HFGUID.Value, false);
                        //        break;
                        //    default:
                        //        break;
                        //}

                        string codPersonaEncriptada = Util.Encriptar(strPersonaId);
                        //------------------------------------------------------
                        //Fecha: 19/10/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Seleccionar el tipo y numero de documento
                        //------------------------------------------------------
                        string codTipoDocumentoEncriptada = Util.Encriptar(intDocumentoId.ToString());
                        string codNroDocumentoEncriptada = Util.Encriptar(strNroDocumento);
                        switch (intDirecciona)
                        {
                            case (int)Enumerador.enmBusquedaDirecciona.RUNE:
                                Response.Redirect("~/Registro/FrmRegistroPersona.aspx?CodPer=" + codPersonaEncriptada + "&CodTipoDoc=" + codTipoDocumentoEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                                break;
                            case (int)Enumerador.enmBusquedaDirecciona.TRAMITE:
                                Response.Redirect("~/Registro/FrmTramite.aspx?CodPer=" + codPersonaEncriptada + "&CodTipoDoc=" + codTipoDocumentoEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                                break;
                            default:
                                break;
                        }

                        break;
                    default: 
                        break;
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

        protected void rptResultPersona_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                RepeaterItem item = e.Item;


                HtmlTableRow fila = new HtmlTableRow();
                fila = (HtmlTableRow)item.FindControl("fila");
                if (fila != null)
                {

                    fila.Attributes.Add("onmouseover", "this.style.textDecoration='underline'; this.style.backgroundColor='" + strbackgroundColorFila + "'");
                    fila.Attributes.Add("onmouseout", "this.style.textDecoration='none';this.style.backgroundColor=''");
                    fila.Attributes.Add("style", "cursor:pointer;");                    
                }
            }
        }

        protected void rptResultEmpresa_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {

                switch (e.CommandName)
                {
                    case "select":
                        RepeaterItem item = e.Item;

                        LinkButton lnkTipoEmpresaId = (LinkButton)item.FindControl("lnkTipoEmpresaId");
                        string strTipoEmpresaId = lnkTipoEmpresaId.Text;
                        Int16 intTipoEmpresaId = Convert.ToInt16(strTipoEmpresaId);

                        string strEmpresaId = lnkTipoEmpresaId.CommandArgument;
                        Int64 intEmpresaId = Convert.ToInt64(strEmpresaId);

                        LinkButton lnkTipoDocumentoId = (LinkButton)item.FindControl("lnkTipoDocumentoId");
                        string strTipoDocumentoId = lnkTipoDocumentoId.Text;
                        Int16 intTipoDocumentoId = Convert.ToInt16(strTipoDocumentoId);

                        LinkButton lnkRazonSocial = (LinkButton)item.FindControl("lnkRazonSocial");
                        string strRazonSocial = lnkRazonSocial.Text;

                        LinkButton lnkNumeroDocumento = (LinkButton)item.FindControl("lnkNumeroDocumento");
                        string strNumeroDocumento = lnkNumeroDocumento.Text;

                        LinkButton lnkActividadComercial = (LinkButton)item.FindControl("lnkActividadComercial");
                        string strActividadComercial = lnkActividadComercial.Text;

                        LinkButton lnkTelefono = (LinkButton)item.FindControl("lnkTelefono");
                        string strTelefono = lnkTelefono.Text;

                        LinkButton lnkCorreo = (LinkButton)item.FindControl("lnkCorreo");
                        string strCorreo = lnkCorreo.Text;

                        LinkButton lnkEstado = (LinkButton)item.FindControl("lnkEstado");
                        string strEstado = lnkEstado.Text;

                        LinkButton lnkDescTipDoc = (LinkButton)item.FindControl("lnkDescTipDoc");
                        string strDescTipDoc = lnkDescTipDoc.Text;

                        //BE.RE_EMPRESA objEmpresa = new BE.RE_EMPRESA();
                        //objEmpresa.empr_iEmpresaId = intEmpresaId;
                        //objEmpresa.empr_sTipoEmpresaId = intTipoEmpresaId;
                        //objEmpresa.empr_sTipoDocumentoId = intTipoDocumentoId;
                        //objEmpresa.empr_vRazonSocial = strRazonSocial;
                        //objEmpresa.empr_vNumeroDocumento = strNumeroDocumento;
                        //objEmpresa.empr_vActividadComercial = strActividadComercial;
                        //objEmpresa.empr_vTelefono = strTelefono;
                        //objEmpresa.empr_vCorreo = strCorreo;
                        //objEmpresa.empr_cEstado = strEstado;
                        //Session["OBJ_RE_EMPRESA"] = objEmpresa;
                        //Session["bEsEdicion"] = "1";
                        //Session["flgModoBusquedaAct"] = false;
                        //Session["ApePat" + HFGUID.Value] = strRazonSocial;
                        //Session["ApeMat" + HFGUID.Value] = string.Empty;
                        //Session["NroDoc" + HFGUID.Value] = strNumeroDocumento;
                        //Session["Nombres" + HFGUID.Value] = string.Empty;
                        //Session["DescTipDoc" + HFGUID.Value] = strDescTipDoc;
                        //Session["NroDoc" + HFGUID.Value] = strNumeroDocumento;
                        //Session["iPersonaId" + HFGUID.Value] = strEmpresaId;
                        //Session["iTipoId" + HFGUID.Value] = ddlPersonaTipo.SelectedValue;
                        //Session["iDocumentoTipoId" + HFGUID.Value] =strTipoDocumentoId;
                        //Session["iPersonaTipoId" + HFGUID.Value] =strTipoEmpresaId;



                        hid_iPersonaId.Value = strEmpresaId;
                        string codPersonaEncriptada = Util.Encriptar(strEmpresaId);

                        int intDirecciona = Convert.ToInt32(hdn_tipo_direccion.Value);
                        //switch (intDirecciona)
                        //{
                        //    case (int)Enumerador.enmBusquedaDirecciona.RUNE:
                        //        //Response.Redirect("~/Registro/FrmRegistroEmpresa.aspx", false);
                        //        Response.Redirect("~/Registro/FrmRegistroEmpresa.aspx?GUID=" + HFGUID.Value, false);
                        //        break;
                        //    case (int)Enumerador.enmBusquedaDirecciona.TRAMITE:
                        //        //Response.Redirect("~/Registro/FrmTramite.aspx");
                        //        Response.Redirect("~/Registro/FrmTramite.aspx?GUID=" + HFGUID.Value, false);
                        //        break;
                        //    default:
                        //        break;
                        //}
                    
                        switch (intDirecciona)
                        {
                            case (int)Enumerador.enmBusquedaDirecciona.RUNE:
                                Response.Redirect("~/Registro/FrmRegistroEmpresa.aspx?CodPer=" + codPersonaEncriptada + "&Juridica=1", false);
                                break;
                            case (int)Enumerador.enmBusquedaDirecciona.TRAMITE:
                                Response.Redirect("~/Registro/FrmTramite.aspx?CodPer=" + codPersonaEncriptada + "&Juridica=1", false);
                                break;
                            default:
                                break;
                        }

                        break;
                    default:
                        break;
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

        protected void rptResultEmpresa_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                RepeaterItem item = e.Item;


                HtmlTableRow fila = new HtmlTableRow();
                fila = (HtmlTableRow)item.FindControl("fila");
                if (fila != null)
                {

                    fila.Attributes.Add("onmouseover", "this.style.textDecoration='underline'; this.style.backgroundColor='" + strbackgroundColorFila + "'");
                    fila.Attributes.Add("onmouseout", "this.style.textDecoration='none';this.style.backgroundColor=''");
                    fila.Attributes.Add("style", "cursor:pointer;");

                }
            }
        }     



        private void CargarDatosIniciales()
        {
            Session["iTipoId"] = 0;
            Session["iTipoId" + HFGUID.Value] = 0;

            Session["PER_NACIONALIDAD"] = 0;
            Session["PER_NACIONALIDAD" + HFGUID.Value] = 0;

            ctrlValidacion.MostrarValidacion("", false);
            ddlPersonaTipo.SelectedValue = ((int)Enumerador.enmTipoPersona.NATURAL).ToString();
        }

        private void CargarListadosDesplegables()
        {
            try
            {
                Util.CargarParametroDropDownList(ddlPersonaTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_TIPO));
            }
            catch (Exception ex)
            {             
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
       
             

        private void MuestraGrid()
        {
            if (ddlPersonaTipo.SelectedValue == Convert.ToInt32(Enumerador.enmTipoPersona.JURIDICA).ToString())
            {
                rptResultEmpresa.Visible = true;
                rptResultPersona.Visible = false;
                rptResultPersona.DataSource = null;
                rptResultPersona.DataBind();
            }
            else
            {
                rptResultEmpresa.Visible = false;
                rptResultPersona.Visible = true;
                rptResultEmpresa.DataSource = null;
                rptResultEmpresa.DataBind();
            }
        }

        private void InicializarBusqueda()
        {
            rptResultPersona.DataSource = null;
            rptResultPersona.DataBind();

            rptResultEmpresa.DataSource = null;
            rptResultEmpresa.DataBind();

            CurrentPage = 0;
            tabpaginado.Visible = false;
        }

        private void ValidarBusqueda()
        {
            if ((txtNroDocumento.Text.Length == 0) && (txtPriApellido.Text.Length == 0) && (txtSegApellido.Text.Length == 0))
            {
                ctrlValidacion.MostrarValidacion("", false);
                return;
            }
        }

        private DataTable GetDataPersona(long LonPersonaId, Int16 intDocumentoId = 0, string strDocumentoNumero = "")
        {
            try
            {
                SGAC.Registro.Persona.BL.PersonaConsultaBL objPersonaBL = new SGAC.Registro.Persona.BL.PersonaConsultaBL();
                DataTable dt = objPersonaBL.PersonaGetById(LonPersonaId, intDocumentoId, strDocumentoNumero);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
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

        private void BindDataIntoRepeater()
        {

            //*************************************
            if (ddlPersonaTipo.SelectedValue == Convert.ToInt32(Enumerador.enmTipoPersona.JURIDICA).ToString())
            {
                _pgsource.DataSource = arrListaEmpresa;
            }
            else
            {
                _pgsource.DataSource = arrListaPersona;
            }

            

            //*************************************

            //_pgsource.DataSource = dt.DefaultView;
            _pgsource.AllowPaging = true;
            // Number of items to be displayed in the Repeater
            _pgsource.PageSize = _pageSize;
            _pgsource.CurrentPageIndex = CurrentPage;
            // Keep the Total pages in View State
            ViewState["TotalPages"] = _pgsource.PageCount;
            // Example: "Page 1 of 10"
            lblpage.Text = "Página: " + (CurrentPage + 1) + " de " + _pgsource.PageCount;
            // Enable First, Last, Previous, Next buttons
            
            //lbPrevious.Enabled = !_pgsource.IsFirstPage;
            //lbNext.Enabled = !_pgsource.IsLastPage;
            //lbFirst.Enabled = !_pgsource.IsFirstPage;
            //lbLast.Enabled = !_pgsource.IsLastPage;

            btnPrimero.Enabled = !_pgsource.IsFirstPage;
            btnAnterior.Enabled = !_pgsource.IsFirstPage;
            btnSiguiente.Enabled = !_pgsource.IsLastPage;
            btnUltimo.Enabled = !_pgsource.IsLastPage;

            if (btnPrimero.Enabled)
            {
                btnPrimero.ImageUrl = "../../Images/NavFirstPage.gif";
            }
            else
            {
                btnPrimero.ImageUrl = "../../Images/NavFirstPageDisabled.gif";
            }
            if (btnAnterior.Enabled)
            {
                btnAnterior.ImageUrl = "../../Images/NavPreviousPage.gif";
            }
            else
            {
                btnAnterior.ImageUrl = "../../Images/NavPreviousPageDisabled.gif";
            }
            if (btnSiguiente.Enabled)
            {
                btnSiguiente.ImageUrl = "../../Images/NavNextPage.gif";
            }
            else
            {
                btnSiguiente.ImageUrl = "../../Images/NavNextPageDisabled.gif";
            }
            if (btnUltimo.Enabled)
            {
                btnUltimo.ImageUrl = "../../Images/NavLastPage.gif";
            }
            else
            {
                btnUltimo.ImageUrl = "../../Images/NavLastPageDisabled.gif";
            }

            // Bind data into repeater
            if (ddlPersonaTipo.SelectedValue == Convert.ToInt32(Enumerador.enmTipoPersona.JURIDICA).ToString())
            {
                rptResultEmpresa.DataSource = _pgsource;
                if (_pgsource.Count > 0)
                {
                    rptResultEmpresa.DataBind();
                }
            }
            else
            {
                rptResultPersona.DataSource = _pgsource;
                if (_pgsource.Count > 0)
                {
                    rptResultPersona.DataBind();
                }
            }
            
            // Call the function to do paging
            HandlePaging();
        }

        private void HandlePaging()
        {
            var dt = new DataTable();
            dt.Columns.Add("PageIndex"); //Start from 0
            dt.Columns.Add("PageText"); //Start from 1

            _firstIndex = CurrentPage - 5;
            if (CurrentPage > 5)
                _lastIndex = CurrentPage + 5;
            else
                _lastIndex = 10;

            // Check last page is greater than total page then reduced it to total no. of page is last index
            if (_lastIndex > Convert.ToInt32(ViewState["TotalPages"]))
            {
                _lastIndex = Convert.ToInt32(ViewState["TotalPages"]);
                _firstIndex = _lastIndex - 10;
            }

            if (_firstIndex < 0)
                _firstIndex = 0;

            // Now creating page number based on above first and last page index
            for (var i = _firstIndex; i < _lastIndex; i++)
            {
                var dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            rptPaging.DataSource = dt;
            rptPaging.DataBind();
        }

        private List<persona> obtenerArregloPersona(string StrNroDoc, string StrApePat, string StrApeMat, string strNombre)
        {                       
            List<persona> ListPersonas = new List<persona>();
            persona per;


            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaActual = 1;
            int intPagSize = 100000;

            ctrlValidacion.MostrarValidacion("", false);

            DataTable dt = new DataTable();

            ActuacionConsultaBL objBL = new ActuacionConsultaBL();

            try
            {


                dt = objBL.RecurrenteConsultar(
                                        (int)Enumerador.enmPersonaConsulta.ACTUACION,
                                        StrNroDoc, StrApePat, StrApeMat, strNombre,
                                        intPaginaActual,
                                        intPagSize,
                                        ref IntTotalCount, ref IntTotalPages);

                if (IntTotalCount > 0)
                {
                    tabpaginado.Visible = true;
                    ctrlValidacion.MostrarValidacion(
                        Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + " " + IntTotalCount,
                        true, Enumerador.enmTipoMensaje.INFORMATION);
                }



                string strFormatoFecha = ConfigurationManager.AppSettings["FormatoFechas"].ToString();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    per = new persona();
                    per.iPersonaId = dt.Rows[i]["iPersonaId"].ToString();
                    per.sPersonaTipoId = dt.Rows[i]["sPersonaTipoId"].ToString();
                    per.sDocumentoTipoId = dt.Rows[i]["sDocumentoTipoId"].ToString();
                    per.vDescTipDoc = dt.Rows[i]["vDescTipDoc"].ToString();
                    per.vApellidoPaterno = dt.Rows[i]["vApellidoPaterno"].ToString();
                    per.vApellidoMaterno = dt.Rows[i]["vApellidoMaterno"].ToString();
                    per.vNombres = dt.Rows[i]["vNombres"].ToString();
                    per.vNroDocumento = dt.Rows[i]["vNroDocumento"].ToString();
                    per.sNacionalidadId = dt.Rows[i]["sNacionalidadId"].ToString();
                    per.sGeneroId = dt.Rows[i]["sGeneroId"].ToString();
                    per.pers_vApellidoCasada = dt.Rows[i]["pers_vApellidoCasada"].ToString();
                    per.vTipoDocumento = dt.Rows[i]["vTipoDocumento"].ToString();
                    per.vNombre = dt.Rows[i]["vNombre"].ToString();
                    per.vNroTipoDocumento = dt.Rows[i]["vNroTipoDocumento"].ToString();

                    string strFechaNac = dt.Rows[i]["vFecNac"].ToString();
                    if (strFechaNac.Length > 0)
                    {
                        DateTime dFecNac = Comun.FormatearFecha(strFechaNac);

                        per.vFecNac = dFecNac.ToString(strFormatoFecha);
                    }
                    else
                    {
                        per.vFecNac = "";
                    }
                    per.pers_vNacionalidad = dt.Rows[i]["pers_vNacionalidad"].ToString();
                    ListPersonas.Add(per);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ListPersonas;
        }

        private List<empresa> obtenerArregloEmpresa(string StrNroDoc, string StrRazonSocial)
        {
            List<empresa> ListEmpresas = new List<empresa>();
            empresa emp;
            

            int IntTotalCount = 0;
            int IntTotalPages = 0;
            int intPaginaActual = 1;
            int intPagSize = 100000;

            ctrlValidacion.MostrarValidacion("", false);
            ActuacionConsultaBL objBL = new ActuacionConsultaBL();
            DataTable dt = new DataTable();

            try
            {

                dt = objBL.EmpresaConsultar(
                                       StrNroDoc.Trim(),
                                       StrRazonSocial.Trim(),
                                       intPaginaActual,
                                       intPagSize,
                                       ref IntTotalCount, ref IntTotalPages);
                if (IntTotalCount > 0)
                {
                    tabpaginado.Visible = true;
                    ctrlValidacion.MostrarValidacion(
                        Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + " " + IntTotalCount,
                        true, Enumerador.enmTipoMensaje.INFORMATION);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    emp = new empresa();
                    emp.empr_iEmpresaId = dt.Rows[i]["empr_iEmpresaId"].ToString();
                    emp.empr_sTipoEmpresaId = dt.Rows[i]["empr_sTipoEmpresaId"].ToString();
                    emp.empr_sTipoDocumentoId = dt.Rows[i]["empr_sTipoDocumentoId"].ToString();
                    emp.vDescTipDoc = dt.Rows[i]["vDescTipDoc"].ToString();
                    emp.empr_vActividadComercial = dt.Rows[i]["empr_vActividadComercial"].ToString();
                    emp.empr_cEstado = dt.Rows[i]["empr_cEstado"].ToString();
                    emp.empr_vRazonSocial = dt.Rows[i]["empr_vRazonSocial"].ToString();
                    emp.empr_vNumeroDocumento = dt.Rows[i]["empr_vNumeroDocumento"].ToString();
                    emp.empr_vTelefono = dt.Rows[i]["empr_vTelefono"].ToString();
                    emp.empr_vCorreo = dt.Rows[i]["empr_vCorreo"].ToString();
                    emp.empr_vTipoEmpresa = dt.Rows[i]["empr_vTipoEmpresa"].ToString();

                    ListEmpresas.Add(emp);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ListEmpresas;
        }


        private void limpiarListas()
        {
            tabpaginado.Visible = false;
            arrListaPersona = null;
            arrListaEmpresa = null;
            ViewState["TotalPages"] = 0;
            _pgsource.PageSize = 0;
            _pgsource.CurrentPageIndex = 0;
            rptResultEmpresa.DataSource = null;
            rptResultEmpresa.DataBind();
            rptResultPersona.DataSource = null;
            rptResultPersona.DataBind();
            HandlePaging();
        }
        //**********************************
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

        //--------------------------------------------
    
        //--------------------------------------------
    }
}