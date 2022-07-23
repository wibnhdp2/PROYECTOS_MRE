using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Configuration;
using SolCARDIP.Librerias.EntidadesNegocio;
using SolCARDIP.Librerias.ReglasNegocio;
using Seguridad.Logica.BussinessEntity;
using SAE.UInterfaces;
using Microsoft.Security.Application;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web.Routing;
using System.Collections.Specialized;
namespace SolCARDIP.Paginas.Supervisor
{
    public partial class SupervisorConsulta : System.Web.UI.Page
    {
        brGeneral obrGeneral = new brGeneral();
        CodigoUsuario oCodigoUsuario = new CodigoUsuario();
        UIEncriptador oUIEncriptador = new UIEncriptador();
        public static string srtAl = "";
        public static List<beEstado> ListaEstados = new List<beEstado>();
        public static List<beParametro> ListaTipoObs = new List<beParametro>();
        public static List<beActaConformidadDetalle> listaConformidadDetalle;
        public static List<beActaRecepcionDetalle> listaRecepcionDetalle;
        public static string tipoMultiple = "";
        public static string claveEstado = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    #region Session Principal
                    csUsuarioBE objUsuarioBE = new csUsuarioBE();
                    objUsuarioBE = (csUsuarioBE)Session["usuario"];
                    ViewState["Usuarioid"] = objUsuarioBE.UsuarioId;
                    ViewState["Oficinaid"] = objUsuarioBE.codOficina;
                    ViewState["IP"] = oCodigoUsuario.obtenerIP();
                    ViewState["UserNomComp"] = objUsuarioBE.NombreCompleto;
                    Session["listaVariosEntrega"] = null;
                    Session["listaVariosRecepcion"] = null;
                    #endregion
                    #region Generales
                    //GENERALES
                    if (Session["Generales"] != null)
                    {
                        beGenerales obeGenerales = new beGenerales();
                        obeGenerales = (beGenerales)Session["Generales"];
                        // DOCUMENTO IDENTIDAD
                        obeGenerales.ListaDocumentoIdentidad.Insert(0, new beDocumentoIdentidad { Tipodocumentoidentidadid = 0, DescripcionCorta = "<Seleccione>" });
                        ddlDEDocumentoIdent.DataSource = obeGenerales.ListaDocumentoIdentidad;
                        ddlDEDocumentoIdent.DataValueField = "Tipodocumentoidentidadid";
                        ddlDEDocumentoIdent.DataTextField = "DescripcionCorta";
                        ddlDEDocumentoIdent.DataBind();

                        ddlVarDocumentoIdent.DataSource = obeGenerales.ListaDocumentoIdentidad;
                        ddlVarDocumentoIdent.DataValueField = "Tipodocumentoidentidadid";
                        ddlVarDocumentoIdent.DataTextField = "DescripcionCorta";
                        ddlVarDocumentoIdent.DataBind();
                        // GENERO
                        obeGenerales.ListaParametroGenero.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                        ddlSexo.DataSource = obeGenerales.ListaParametroGenero;
                        ddlSexo.DataValueField = "Parametroid";
                        ddlSexo.DataTextField = "Descripcion";
                        ddlSexo.DataBind();
                        // CALIDAD MIGRATORIA
                        obeGenerales.ListaCalidadMigratoriaNivelPrincipal.Insert(0, new beCalidadMigratoria { CalidadMigratoriaid = 0, Nombre = "<Seleccione>" });
                        ddlCalidadMigratoriaPri.DataSource = obeGenerales.ListaCalidadMigratoriaNivelPrincipal;
                        ddlCalidadMigratoriaPri.DataValueField = "CalidadMigratoriaid";
                        ddlCalidadMigratoriaPri.DataTextField = "Nombre";
                        ddlCalidadMigratoriaPri.DataBind();
                        // CATEGORIA OFICINA EXTRANJERA
                        obeGenerales.CategoriaOficinaExtranjera.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                        ddlCategoriaOfcoEx.DataSource = obeGenerales.CategoriaOficinaExtranjera;
                        ddlCategoriaOfcoEx.DataValueField = "Parametroid";
                        ddlCategoriaOfcoEx.DataTextField = "Descripcion";
                        ddlCategoriaOfcoEx.DataBind();
                        cargarComboNull(ddlMision);
                        // ESTADOS
                        obeGenerales.ListaEstados.Insert(0, new beEstado { Estadoid = 0, DescripcionCorta = "<Seleccione>" });
                        ddlEstado.DataSource = obeGenerales.ListaEstados;
                        ddlEstado.DataValueField = "Estadoid";
                        ddlEstado.DataTextField = "DescripcionCorta";
                        ddlEstado.DataBind();
                        ListaEstados = obeGenerales.ListaEstados;
                        // USUARIOS REGISTRADORES
                        List<beUsuario> listaFiltro = new List<beUsuario>();
                        listaFiltro = obeGenerales.ListaUsuarios.FindAll(x => x.Rol.Equals("REGISTRADOR"));
                        //listaFiltro.Insert(0, new beUsuario { Usuarioid = 0, NombreCompleto = "<Seleccione>" });
                        obeGenerales.ListaUsuarios.Insert(0, new beUsuario { Usuarioid = 0, NombreCompleto = "<Seleccione>" });
                        ddlRegistrador.DataSource = obeGenerales.ListaUsuarios;
                        ddlRegistrador.DataValueField = "Usuarioid";
                        ddlRegistrador.DataTextField = "NombreCompleto";
                        ddlRegistrador.DataBind();
                        // TIPO ENTRADA
                        obeGenerales.TipoEntrada.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                        ddlTipoEntrada.DataSource = obeGenerales.TipoEntrada;
                        ddlTipoEntrada.DataValueField = "Parametroid";
                        ddlTipoEntrada.DataTextField = "Descripcion";
                        ddlTipoEntrada.DataBind();

                        ListaTipoObs = obeGenerales.TipoObservacion;
                    }
                    #endregion
                    #region Otros Controles
                    System.Web.UI.WebControls.ListItem itemSel = new System.Web.UI.WebControls.ListItem();
                    itemSel.Text = "<Seleccione>";
                    itemSel.Value = "0";
                    ddlPeriodo.Items.Add(itemSel);
                    for (int i = 2017; i <= 2020; i++)
                    {
                        System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
                        item.Text = i.ToString();
                        item.Value = i.ToString();
                        ddlPeriodo.Items.Add(item);
                    }
                    //if (ddlPeriodo.Items.Count > 1) { ddlPeriodo.SelectedValue = "2017"; }
                    #endregion
                    #region Control Cancelar Edicion
                    if (Request.QueryString["pag"] != null)
                    {
                        if (Session["ResultadoConsulta"] != null)
                        {
                            ViewState["pag"] = Sanitizer.GetSafeHtmlFragment(oUIEncriptador.DesEncriptarCadena(Request.QueryString["pag"].ToString().Replace(" ", "+")));
                            cambiarPagina(sender, e);
                        }
                    }
                    #endregion
                    #region ModificarValorURL
                    String currurl = HttpContext.Current.Request.RawUrl;
                    int iqs = currurl.IndexOf('?');
                    int iqs1 = currurl.IndexOf("valS");
                    if (iqs1 != -1) { ViewState["srtAl"] = srtAl; }
                    if (iqs == -1 & iqs1 == -1)
                    {
                        srtAl = oCodigoUsuario.generarStrAleatorio();
                        ViewState["srtAl"] = srtAl;
                        String redirecturl = oCodigoUsuario.crearSTR(Request.Url.ToString(), currurl, objUsuarioBE.Alias, srtAl, 0);
                        Response.Redirect(redirecturl, false);
                    }
                    else if (iqs >= 0 & iqs1 == -1)
                    {
                        srtAl = oCodigoUsuario.generarStrAleatorio();
                        ViewState["srtAl"] = srtAl;
                        String redirecturl = oCodigoUsuario.crearSTR(Request.Url.ToString(), currurl, objUsuarioBE.Alias, srtAl, 1);
                        Response.Redirect(redirecturl, false);
                    }
                    #endregion
                    gvVariosEntrega.DataSource = null;
                    gvVariosEntrega.DataBind();
                }
                else
                {
                    #region PruebaValorURL
                    csUsuarioBE objUsuarioBE = new csUsuarioBE();
                    objUsuarioBE = (csUsuarioBE)Session["usuario"];
                    string qs = (Request.QueryString["valS"] != null & !Request.QueryString["valS"].ToString().Equals("") ? Request.QueryString["valS"].ToString() : "error");
                    bool exito = oCodigoUsuario.validarSTR(qs, objUsuarioBE.Alias, obrGeneral.host, ViewState["srtAl"].ToString());
                    if (!exito) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensajeError", "location.href='../../mensajes.aspx';", true); return; }
                    #endregion
                    #region Postback
                    divModal.Style.Add("display", "none");
                    divModal2.Style.Add("display", "none");
                    divDerivarRegCom.Style.Add("display", "none");
                    divDerivarEntrega.Style.Add("display", "none");
                    divMovimientos.Style.Add("display", "none");
                    divVarios.Style.Add("display", "none");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "preloader", "preloader();", true);
                    #endregion
                }
            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        #region Controles
        protected void buscarRegistros(object sender, EventArgs e) // BUSCA REGISTROS DE ACUERDO A FILTROS
        {
            try
            {
                beRegistroPrevio parametros = new beRegistroPrevio();
                parametros.Periodo = int.Parse(ddlPeriodo.SelectedValue);
                parametros.Ident = (txtNumeroIdent.Text.Equals("") ? 0 : int.Parse(txtNumeroIdent.Text));
                parametros.CarneNumero = txtNumeroCarne.Text.Trim();
                parametros.PrimerApellido = txtApePat.Text.Trim();
                parametros.SegundoApellido = txtApeMat.Text.Trim();
                parametros.Nombres = txtNombres.Text.Trim();
                parametros.FechaRegistroDesde = (txtFechaDesde.Text.Equals("") ? new DateTime(1900, 1, 1) : DateTime.Parse(txtFechaDesde.Text));
                parametros.FechaRegistroHasta = (txtFechaHasta.Text.Equals("") ? new DateTime(1900, 1, 1) : DateTime.Parse(txtFechaHasta.Text));
                parametros.TipoEntrada = short.Parse(ddlTipoEntrada.SelectedValue);
                parametros.CalidadMigratoria = short.Parse(ddlCalidadMigratoriaPri.SelectedValue);
                parametros.OficinaConsularExId = short.Parse(ddlMision.SelectedValue);
                parametros.EstadoId = short.Parse(ddlEstado.SelectedValue);
                parametros.CantReg = long.Parse(obrGeneral.cantRegxPag);
                parametros.NumPag = 1;
                brRegistroPrevio obrRegistroPrevio = new brRegistroPrevio();
                beRegistroPrevioListas obeRegistroPrevioListas = new beRegistroPrevioListas();
                obeRegistroPrevioListas = obrRegistroPrevio.consultar(parametros);
                Session["ResultadoConsulta"] = obeRegistroPrevioListas.listaRegistros;
                Session["ParametrosConsulta"] = parametros;
                gvRegPrev.DataSource = obeRegistroPrevioListas.listaRegistros;
                gvRegPrev.DataBind();
                // PAGINACION
                long _TotalPag = obeRegistroPrevioListas.Paginacion.Total;
                long _Residuo = obeRegistroPrevioListas.Paginacion.Residuo;
                long _TotalRegistros = obeRegistroPrevioListas.Paginacion.TotalRegistros;
                paginacion(_TotalPag, _Residuo);
                lblTotalRegistros.Text = (obeRegistroPrevioListas.listaRegistros != null ? "Resultado de la Busqueda: " + _TotalRegistros.ToString() + " registros encontrados" : "Resultado de la Busqueda: 0registros encontrados");

            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected void cambiarPagina(object sender, EventArgs e) // DROPDOWLIST DE PAGINACION DE LA GRILLA
        {
            try
            {
                beRegistroPrevio parametros = new beRegistroPrevio();
                parametros = (beRegistroPrevio)Session["ParametrosConsulta"];
                if (ddlPaginas.Items.Count > 0) { parametros.NumPag = long.Parse(ddlPaginas.SelectedValue); }
                brRegistroPrevio obrRegistroPrevio = new brRegistroPrevio();
                beRegistroPrevioListas obeRegistroPrevioListas = new beRegistroPrevioListas();
                obeRegistroPrevioListas = obrRegistroPrevio.consultar(parametros);
                Session["ResultadoConsulta"] = obeRegistroPrevioListas.listaRegistros;
                Session["ParametrosConsulta"] = parametros;
                gvRegPrev.DataSource = (obeRegistroPrevioListas.listaRegistros != null ? obeRegistroPrevioListas.listaRegistros : null);
                gvRegPrev.DataBind();
                // PAGINACION
                long _TotalPag = obeRegistroPrevioListas.Paginacion.Total;
                long _Residuo = obeRegistroPrevioListas.Paginacion.Residuo;
                long _TotalRegistros = obeRegistroPrevioListas.Paginacion.TotalRegistros;
                paginacion(_TotalPag, _Residuo);
                lblTotalRegistros.Text = (obeRegistroPrevioListas.listaRegistros != null ? "Resultado de la Busqueda: " + _TotalRegistros.ToString() + " registros encontrados" : "Resultado de la Busqueda: 0registros encontrados");
                ddlPaginas.SelectedValue = parametros.NumPag.ToString();
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected void seleccionarCategoriaOficina(object sender, EventArgs e) // DROPDOWLIST - A LA SELECCION DE UNA CATEGORIA MUESTRA UNA LISTA DE INTITUCIONES QUE CORRESPONDEN A ESA MISMA
        {
            try
            {
                if (!ddlCategoriaOfcoEx.SelectedValue.Equals("0"))
                {
                    if (Session["Generales"] != null)
                    {
                        beGenerales obeGenerales = new beGenerales();
                        obeGenerales = (beGenerales)Session["Generales"];
                        List<beOficinaconsularExtranjera> lbeOficinaconsularExtranjera = new List<beOficinaconsularExtranjera>();
                        lbeOficinaconsularExtranjera = oCodigoUsuario.obtenerOficinasConsularesExtranjeras(short.Parse(ddlCategoriaOfcoEx.SelectedValue), obeGenerales.ListaOficinasConsularesExtranjeras);
                        if (lbeOficinaconsularExtranjera.Count > 0)
                        {
                            lbeOficinaconsularExtranjera.Insert(0, new beOficinaconsularExtranjera { OficinaconsularExtranjeraid = 0, Nombre = "<Seleccione>" });
                            ddlMision.DataSource = lbeOficinaconsularExtranjera;
                            ddlMision.DataValueField = "OficinaconsularExtranjeraid";
                            ddlMision.DataTextField = "Nombre";
                            ddlMision.DataBind();
                            ddlMision.Enabled = true;
                        }
                        else
                        {
                            cargarComboNull(ddlMision);
                            ddlMision.Enabled = false;
                        }
                    }
                }
                else
                {
                    cargarComboNull(ddlMision);
                    ddlMision.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
                obrGeneral.grabarLog(ex);
            }
        }

        protected void seleccionarRegistro(object sender, EventArgs e) // SELECCION DE REGISTROS EN LAS DISTINTAS HERRAMIENTAS QUE OBEDECEN AL COMMANDNAME
        {
            try
            {
                ImageButton imageButton = (ImageButton)sender;
                TableCell tableCell = (TableCell)imageButton.Parent;
                GridViewRow row = (GridViewRow)tableCell.Parent;
                string comando = imageButton.CommandName;
                beRegistroPrevio obeRegistroPrevio = new beRegistroPrevio();
                obeRegistroPrevio = obtenerRegistro(sender, e);
                short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
                string ip = ViewState["IP"].ToString();
                if (obeRegistroPrevio != null)
                {
                    Session["Temp1"] = obeRegistroPrevio;
                    List<beRegistroPrevio> listaRegPrevio = new List<beRegistroPrevio>();
                    switch (comando)
                    {
                        case "agregarListaRecepcion":
                            #region ListaRecepcion
                            if (Session["listaVariosRecepcion"] != null)
                            {
                                listaRegPrevio = (List<beRegistroPrevio>)Session["listaVariosRecepcion"];
                            }
                            if (listaRegPrevio.Count > 0)
                            {
                                bool tipoEntrada = validarTipoEntrada(obeRegistroPrevio, 0);
                                if (tipoEntrada)
                                {
                                    int pos = listaRegPrevio.FindIndex(x => x.ConIdent.Equals(row.Cells[1].Text));
                                    if (pos != -1)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('REGISTRO YA SE AGREGO A LA LISTA');", true);
                                    }
                                    else
                                    {
                                        listaRegPrevio.Add(obeRegistroPrevio);
                                        List<beRegistroPrevio> listaRegPrevioOrden = listaRegPrevio.OrderByDescending(x => x.ConIdent).ToList();
                                        Session["listaVariosRecepcion"] = listaRegPrevio;
                                        agregarDetalle(0);
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('HAY REGISTROS CON OTRO TIPO DE ENTRADA EN LA LISTA. VERIFIQUE');", true);
                                }
                            }
                            else
                            {
                                listaRegPrevio.Add(obeRegistroPrevio);
                                Session["listaVariosRecepcion"] = listaRegPrevio;
                                agregarDetalle(0);
                            }
                            #endregion
                            break;
                        case "agregarListaEntrega":
                            #region ListaEntrega
                            string entradaValida = (obeRegistroPrevio.ConTipoEntrada != null & !obeRegistroPrevio.ConTipoEntrada.Equals("-") & !obeRegistroPrevio.ConTipoEntrada.Equals("") ? obeRegistroPrevio.ConTipoEntrada : "error");
                            if (!entradaValida.Equals("error"))
                            {
                                if (Session["listaVariosEntrega"] != null)
                                {
                                    listaRegPrevio = (List<beRegistroPrevio>)Session["listaVariosEntrega"];
                                }
                                if (listaRegPrevio.Count > 0)
                                {
                                    bool tipoEntrada = validarTipoEntrada(obeRegistroPrevio, 1);
                                    if (tipoEntrada)
                                    {
                                        int pos = listaRegPrevio.FindIndex(x => x.ConIdent.Equals(row.Cells[1].Text));
                                        if (pos != -1)
                                        {
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('REGISTRO YA SE AGREGO A LA LISTA');", true);
                                        }
                                        else
                                        {
                                            listaRegPrevio.Add(obeRegistroPrevio);
                                            List<beRegistroPrevio> listaRegPrevioOrden = listaRegPrevio.OrderByDescending(x => x.ConIdent).ToList();
                                            Session["listaVariosEntrega"] = listaRegPrevio;
                                            agregarDetalle(1);
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('HAY REGISTROS CON OTRO TIPO DE ENTRADA EN LA LISTA. VERIFIQUE');", true);
                                    }
                                }
                                else
                                {
                                    listaRegPrevio.Add(obeRegistroPrevio);
                                    Session["listaVariosEntrega"] = listaRegPrevio;
                                    agregarDetalle(1);
                                }
                                //lblInfoVarios.Text = "Registros Agregados para entregar: " + listaRegPrevio.Count.ToString();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('EL TIPO DE ENTRADA DE LA DOCUMENTACION NO ES VALIDA. VERIFIQUE');", true);
                            }
                            #endregion
                            break;
                        case "editarRegistroPrevio":
                            string regprevIdEditar = oUIEncriptador.EncriptarCadena(obeRegistroPrevio.RegistroPrevioId.ToString());
                            string identificadorEditar = oUIEncriptador.EncriptarCadena(obeRegistroPrevio.ConIdent);
                            string pagEditar = oUIEncriptador.EncriptarCadena(ddlPaginas.SelectedValue);
                            Response.Redirect(String.Format("SupervisorRegistroPrevio.aspx?regprevid={0}&identificador={1}&pag={2}", regprevIdEditar, identificadorEditar, pagEditar));
                            break;
                        case "derivarRegistroCompleto":
                            string estadoActual = oCodigoUsuario.detectarCaracterEspecial(row.Cells[4].Text);
                            if (estadoActual.Equals("REGISTRO PREVIO")) { claveEstado = "DERIVADO PARA COMPLETAR"; }
                            else
                            {
                                claveEstado = estadoActual;
                            }
                            lblDRCIdentificador.Text = obeRegistroPrevio.ConIdent;
                            lblDRCFuncionario.Text = obeRegistroPrevio.ConNombreCompleto;
                            lblDRCCalMig.Text = obeRegistroPrevio.ConCalidadMigratoriaDesc;
                            lblDRCMision.Text = obeRegistroPrevio.ConOficinaConsular;
                            ddlRegistrador.SelectedValue = obeRegistroPrevio.ConRegistradorId.ToString();
                            //ddlRegistrador.SelectedValue = "0";
                            divModal.Style.Add("display", "block");
                            divDerivarRegCom.Style.Add("display", "block");
                            break;
                        case "entregarRegistro":
                            lblDEIdentificador.Text = obeRegistroPrevio.ConIdent;
                            lblDETipoEntrada.Text = obeRegistroPrevio.ConTipoEntrada;
                            lblDEFuncionario.Text = obeRegistroPrevio.ConNombreCompleto;
                            lblDECalMig.Text = obeRegistroPrevio.ConCalidadMigratoriaDesc;
                            lblDEMision.Text = obeRegistroPrevio.ConOficinaConsular;
                            //lblDEInstitucion.Text = obeRegistroPrevio.ConOficinaConsular;
                            controlesBlanco(true);
                            controlesBlanco(false);
                            bloquearSolicitante(true, true);
                            // LLENA LISTA ACTA CONFORMIDAD DETALLE
                            listaConformidadDetalle = new List<beActaConformidadDetalle>();
                            beActaConformidadDetalle obeActaConformidadDetalle = new beActaConformidadDetalle();
                            obeActaConformidadDetalle.CarneIdentidadId = obeRegistroPrevio.ConCarneIdentidadId;
                            obeActaConformidadDetalle.UsuarioCreacion = usuarioId;
                            obeActaConformidadDetalle.IpCreacion = ip;
                            listaConformidadDetalle.Add(obeActaConformidadDetalle);
                            Session["Temp1"] = listaConformidadDetalle;
                            tipoMultiple = "entregaMultiple";
                            divModal.Style.Add("display", "block");
                            divDerivarEntrega.Style.Add("display", "block");
                            break;
                        case "verActaConformidad":
                            brActaConformidadCabecera obrActaConformidadCabecera = new brActaConformidadCabecera();
                            beActaConformidadCabecera parametrosConsulta = new beActaConformidadCabecera();
                            parametrosConsulta.ActaConformidadCabeceraId = obeRegistroPrevio.ConActaConformidad;
                            beActaConformidadPrincipal resultado = new beActaConformidadPrincipal();
                            resultado = obrActaConformidadCabecera.consultar(parametrosConsulta);
                            if (resultado != null)
                            {
                                byte[] pdfByte = oCodigoUsuario.crearActaConformidad(resultado, ViewState["UserNomComp"].ToString()); //ms.ToArray();
                                Session["bytePDF"] = pdfByte;
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirPDF", "window.open('../PDF.aspx', '_blank')", true);
                            }
                            break;
                        case "pdfInformacion":
                            beCarneIdentidad obeCarneIdentidadInfo = obtenerRegistroCI(obeRegistroPrevio.ConCarneIdentidadId);
                            byte[] pdfByteInfo = oCodigoUsuario.crearPDF1(obeCarneIdentidadInfo, ViewState["UserNomComp"].ToString()); //ms.ToArray();
                            Session["bytePDF"] = pdfByteInfo;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirPDF", "window.open('../PDF.aspx', '_blank')", true);
                            break;
                        case "verMovimientos":
                            beMovimientoCarneIdentidad parametros = new beMovimientoCarneIdentidad();
                            parametros.CarneIdentidadid = obeRegistroPrevio.ConCarneIdentidadId;
                            brMovimientoCarneIdentidad obrMovimientoCarneIdentidad = new brMovimientoCarneIdentidad();
                            List<beMovimientoCarneIdentidad> lbeMovimientoCarneIdentidad = new List<beMovimientoCarneIdentidad>();
                            lbeMovimientoCarneIdentidad = obrMovimientoCarneIdentidad.consultarMovimientos(parametros);
                            if (lbeMovimientoCarneIdentidad != null)
                            {
                                gvMovimientos.DataSource = lbeMovimientoCarneIdentidad;
                                gvMovimientos.DataBind();
                                divMovimientos.Style.Add("display", "block");
                                divModal.Style.Add("display", "block");
                                //controlId = "btnCerrarMovimientos";
                                //ejecucionePostBack();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL PROCESAR LA INFORMACION');", true);
                            }
                            break;
                        case "verActaRecepcion":
                            brActaRecepcionCabecera obrActaRecepcionCabecera = new brActaRecepcionCabecera();
                            beActaRecepcionCabecera parametrosConsultaRec = new beActaRecepcionCabecera();
                            parametrosConsultaRec.ActaRecepcionId = obeRegistroPrevio.ConActaRecepcion;
                            beActaRecepcionPrincipal resultadoRecConsulta = new beActaRecepcionPrincipal();
                            resultadoRecConsulta = obrActaRecepcionCabecera.consultar(parametrosConsultaRec);
                            if (resultadoRecConsulta != null)
                            {
                                byte[] pdfByte = oCodigoUsuario.crearActaRecepcion(resultadoRecConsulta, ViewState["UserNomComp"].ToString());
                                Session["bytePDF"] = pdfByte;
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirPDF", "window.open('../PDF.aspx', '_blank')", true);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected void actualizarEstado(object sender, EventArgs e) // ACTUALIZA EL ESTADO DEL REGISTRO
        {
            try
            {
                beRegistroPrevio obeRegistroPrevio = new beRegistroPrevio();
                if (Session["Temp1"] != null)
                {
                    obeRegistroPrevio = (beRegistroPrevio)Session["Temp1"];
                    short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
                    short oficinaId = short.Parse(ViewState["Oficinaid"].ToString());
                    string ip = ViewState["IP"].ToString();
                    short estado = obtenerIdEstado(claveEstado);
                    short tipoObs = obtenerTipoObs("DERIVADO PARA REGISTRO COMPLETO");
                    beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
                    // CARNE DE IDENTIDAD
                    beCarneIdentidad obeCarneIdentidad = new beCarneIdentidad();
                    obeCarneIdentidad.CarneIdentidadid = obeRegistroPrevio.ConCarneIdentidadId;
                    obeCarneIdentidad.Estadoid = estado;
                    obeCarneIdentidad.UsuarioDeriva = short.Parse(ddlRegistrador.SelectedValue);
                    obeCarneIdentidad.Usuariomodificacion = usuarioId;
                    obeCarneIdentidad.Ipmodificacion = ip;
                    obeCarneIdentidadPrincipal.CarneIdentidad = obeCarneIdentidad;
                    // MOVIMIENTO CARNÉ
                    beMovimientoCarneIdentidad obeMovimientoCarneIdentidad = new beMovimientoCarneIdentidad();
                    obeMovimientoCarneIdentidad.CarneIdentidadid = obeRegistroPrevio.ConCarneIdentidadId;
                    obeMovimientoCarneIdentidad.Estadoid = estado;
                    obeMovimientoCarneIdentidad.Oficinaconsularid = oficinaId;
                    obeMovimientoCarneIdentidad.ObservacionTipo = tipoObs;
                    obeMovimientoCarneIdentidad.ObservacionDetalle = "DERIVADO A " + ddlRegistrador.SelectedItem.Text;
                    obeMovimientoCarneIdentidad.Usuariocreacion = usuarioId;
                    obeMovimientoCarneIdentidad.Ipcreacion = ip;
                    obeCarneIdentidadPrincipal.MovimientoCarne = obeMovimientoCarneIdentidad;
                    brCarneIdentidadPrincipal obrCarneIdentidadPrincipal = new brCarneIdentidadPrincipal();
                    bool exito = obrCarneIdentidadPrincipal.derivar(obeCarneIdentidadPrincipal);
                    if (exito)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE DERIVÓ EL REGISTRO CON EXITO');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL DERIVAR EL REGISTRO.');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
                }
                cambiarPagina(sender, e);
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                cambiarPagina(sender, e);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
            }
        }

        protected void generarActaConformidad(object sender, EventArgs e) // GENERA EL ACTA DE RECEPCION O ENTREGA DE ACUERDO AL COMMANDNAME
        {
            try
            {
                short estadoId = -1;
                short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
                short oficinaId = short.Parse(ViewState["Oficinaid"].ToString());
                string ipCreacion = ViewState["IP"].ToString();
                string ip = ViewState["IP"].ToString();
                switch (tipoMultiple)
                {
                    case "recepcionMultiple":
                        #region Recepcion
                        short idActaRecepcionCabecera = -1;
                        List<beRegistroPrevio> listaRegPrevio = new List<beRegistroPrevio>();
                        listaRegPrevio = (List<beRegistroPrevio>)Session["listaVariosRecepcion"];
                        if (listaRegPrevio != null)
                        {
                            if (listaRegPrevio.Count > 0)
                            {
                                estadoId = obtenerIdEstado("REGISTRO PREVIO");
                                beActaRecepcionPrincipal obeActaRecepcionPrincipal = new beActaRecepcionPrincipal();
                                // ACTA RECEPCION CABECERA
                                beActaRecepcionCabecera obeActaRecepcionCabecera = new beActaRecepcionCabecera();
                                obeActaRecepcionCabecera.Observacion = (!txtVarObservacion.Text.Trim().Equals("") ? txtVarObservacion.Text.Trim().ToUpper() : null);
                                obeActaRecepcionCabecera.UsuarioCreacion = usuarioId;
                                obeActaRecepcionCabecera.IpCreacion = ip;
                                obeActaRecepcionPrincipal.ActaCabecera = obeActaRecepcionCabecera;
                                // SOLICITANTE
                                beSolicitante obeSolicitanteRecepcion = new beSolicitante();
                                obeSolicitanteRecepcion.PrimerApellido = (!txtDEApePat.Text.Equals("") ? txtDEApePat.Text.Trim().ToUpper() : txtVarApePat.Text.Trim().ToUpper());
                                obeSolicitanteRecepcion.SegundoApellido = (!txtDEApeMat.Text.Equals("") ? txtDEApeMat.Text.Trim().ToUpper() : txtVarApeMat.Text.Trim().ToUpper());
                                obeSolicitanteRecepcion.Nombres = (!txtDENombres.Text.Equals("") ? txtDENombres.Text.Trim().ToUpper() : txtVarNombres.Text.Trim().ToUpper());
                                obeSolicitanteRecepcion.TipoDocumentoIdentidadId = (!ddlDEDocumentoIdent.SelectedValue.Equals("0") ? short.Parse(ddlDEDocumentoIdent.SelectedValue) : short.Parse(ddlVarDocumentoIdent.SelectedValue));
                                obeSolicitanteRecepcion.NumeroDocumentoIdentidad = (!txtDENumeroDocIdent.Text.Equals("") ? txtDENumeroDocIdent.Text.Trim() : txtVarNumeroDocIdent.Text.Trim());
                                obeSolicitanteRecepcion.Telefono = (!txtDETelefono.Text.Equals("") ? txtDETelefono.Text.Trim() : txtVarTelefono.Text.Trim());
                                obeSolicitanteRecepcion.Usuariocreacion = usuarioId;
                                obeSolicitanteRecepcion.Ipcreacion = ip;
                                obeSolicitanteRecepcion.Usuariomodificacion = usuarioId;
                                obeSolicitanteRecepcion.Ipmodificacion = ip;
                                obeActaRecepcionPrincipal.Solicitante = obeSolicitanteRecepcion;
                                // REGISTRO PREVIO
                                obeActaRecepcionPrincipal.ActaDetalle = listaRecepcionDetalle;
                                // MOVIMIENTO CARNÉ
                                List<beMovimientoCarneIdentidad> listaMovimientosRecep = new List<beMovimientoCarneIdentidad>();
                                beMovimientoCarneIdentidad obeMovimientoCarneIdentidadRecep;
                                for (int i = 0; i <= listaRegPrevio.Count - 1; i++)
                                {
                                    obeMovimientoCarneIdentidadRecep = new beMovimientoCarneIdentidad();
                                    obeMovimientoCarneIdentidadRecep.CarneIdentidadid = listaRegPrevio[i].ConCarneIdentidadId;
                                    obeMovimientoCarneIdentidadRecep.Estadoid = estadoId;
                                    obeMovimientoCarneIdentidadRecep.ObservacionDetalle = "RECEPCIÓN DE CARNÉ";
                                    obeMovimientoCarneIdentidadRecep.Oficinaconsularid = oficinaId;
                                    obeMovimientoCarneIdentidadRecep.Usuariocreacion = usuarioId;
                                    obeMovimientoCarneIdentidadRecep.Ipcreacion = ipCreacion;
                                    listaMovimientosRecep.Add(obeMovimientoCarneIdentidadRecep);
                                }
                                obeActaRecepcionPrincipal.ListaMovimientos = listaMovimientosRecep;
                                brActaRecepcionCabecera obrActaRecepcionCabecera = new brActaRecepcionCabecera();
                                idActaRecepcionCabecera = obrActaRecepcionCabecera.adicionar(obeActaRecepcionPrincipal);
                                if (idActaRecepcionCabecera != -1)
                                {
                                    beActaRecepcionCabecera parametros = new beActaRecepcionCabecera();
                                    parametros.ActaRecepcionId = idActaRecepcionCabecera;
                                    beActaRecepcionPrincipal resultadoRecepcion = new beActaRecepcionPrincipal();
                                    resultadoRecepcion = obrActaRecepcionCabecera.consultar(parametros);
                                    if (resultadoRecepcion != null)
                                    {
                                        Session["listaVariosEntrega"] = null;
                                        gvVariosEntrega.DataSource = null;
                                        gvVariosEntrega.DataBind();
                                        controlesBlanco(true);
                                        controlesBlanco(false);
                                        //lblInfoVarios.Text = "Registros Agregados para entregar: ";
                                        byte[] pdfByte = oCodigoUsuario.crearActaRecepcion(resultadoRecepcion, ViewState["UserNomComp"].ToString()); //ms.ToArray();
                                        Session["bytePDF"] = pdfByte;
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirPDF", "window.open('../PDF.aspx', '_blank')", true);
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL GUARDAR LA INFORMACION.');", true);
                                    divModal.Style.Add("display", "block");
                                    divDerivarEntrega.Style.Add("display", "block");
                                }

                            }
                            else { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('NO HAY REGISTROS EN EL DETALLE. REVISE.');", true); }
                        }
                        else { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('NO HAY REGISTROS EN EL DETALLE. REVISE.');", true); }
                        #endregion
                        break;
                    case "entregaMultiple":
                        #region ActaConformidad
                        estadoId = obtenerIdEstado("EMITIDO (IMPRESO)");
                        short idActaConformidadCabecera = -1;
                        beActaConformidadPrincipal obeActaConformidadPrincipal = new beActaConformidadPrincipal();
                        // SOLICITANTE
                        beSolicitante obeSolicitanteEntrega = new beSolicitante();
                        obeSolicitanteEntrega.PrimerApellido = (!txtDEApePat.Text.Equals("") ? txtDEApePat.Text.Trim().ToUpper() : txtVarApePat.Text.Trim().ToUpper());
                        obeSolicitanteEntrega.SegundoApellido = (!txtDEApeMat.Text.Equals("") ? txtDEApeMat.Text.Trim().ToUpper() : txtVarApeMat.Text.Trim().ToUpper());
                        obeSolicitanteEntrega.Nombres = (!txtDENombres.Text.Equals("") ? txtDENombres.Text.Trim().ToUpper() : txtVarNombres.Text.Trim().ToUpper());
                        obeSolicitanteEntrega.TipoDocumentoIdentidadId = (!ddlDEDocumentoIdent.SelectedValue.Equals("0") ? short.Parse(ddlDEDocumentoIdent.SelectedValue) : short.Parse(ddlVarDocumentoIdent.SelectedValue));
                        obeSolicitanteEntrega.NumeroDocumentoIdentidad = (!txtDENumeroDocIdent.Text.Equals("") ? txtDENumeroDocIdent.Text.Trim() : txtVarNumeroDocIdent.Text.Trim());
                        obeSolicitanteEntrega.Telefono = (!txtDETelefono.Text.Equals("") ? txtDETelefono.Text.Trim() : txtVarTelefono.Text.Trim());
                        obeSolicitanteEntrega.Usuariocreacion = usuarioId;
                        obeSolicitanteEntrega.Ipcreacion = ip;
                        obeSolicitanteEntrega.Usuariomodificacion = usuarioId;
                        obeSolicitanteEntrega.Ipmodificacion = ip;
                        obeActaConformidadPrincipal.Solicitante = obeSolicitanteEntrega;
                        // ACTA CONFORMIDAD - CABECERA
                        beActaConformidadCabecera obeActaConformidadCabecera = new beActaConformidadCabecera();
                        if (!ddlDEDocumentoIdent.SelectedValue.Equals("0"))
                        {
                            obeActaConformidadCabecera.Observacion = (!txtDEObservacion.Text.Equals("") ? txtDEObservacion.Text.Trim().ToUpper() : null);
                        }
                        if (!ddlVarDocumentoIdent.SelectedValue.Equals("0"))
                        {
                            obeActaConformidadCabecera.Observacion = (!txtVarObservacion.Text.Equals("") ? txtVarObservacion.Text.Trim().ToUpper() : null);
                        }
                        obeActaConformidadCabecera.UsuarioCreacion = usuarioId;
                        obeActaConformidadCabecera.IpCreacion = ip;
                        obeActaConformidadPrincipal.ActaCabecera = obeActaConformidadCabecera;
                        // ACTA CONFORMIDAD - DETALLE
                        obeActaConformidadPrincipal.ActaDetalle = listaConformidadDetalle;
                        // MOVIMIENTO CARNE IDENTIDAD
                        List<beMovimientoCarneIdentidad> listaMovimientosEntrega = new List<beMovimientoCarneIdentidad>();
                        beMovimientoCarneIdentidad obeMovimientoCarneIdentidadEntrega;
                        for (int i = 0; i <= listaConformidadDetalle.Count - 1; i++)
                        {
                            obeMovimientoCarneIdentidadEntrega = new beMovimientoCarneIdentidad();
                            obeMovimientoCarneIdentidadEntrega.CarneIdentidadid = listaConformidadDetalle[i].CarneIdentidadId;
                            obeMovimientoCarneIdentidadEntrega.Estadoid = estadoId;
                            obeMovimientoCarneIdentidadEntrega.ObservacionDetalle = "ENTREGA DE CARNÉ";
                            obeMovimientoCarneIdentidadEntrega.Oficinaconsularid = oficinaId;
                            obeMovimientoCarneIdentidadEntrega.Usuariocreacion = usuarioId;
                            obeMovimientoCarneIdentidadEntrega.Ipcreacion = ipCreacion;
                            listaMovimientosEntrega.Add(obeMovimientoCarneIdentidadEntrega);
                        }
                        obeActaConformidadPrincipal.ListaMovimientos = listaMovimientosEntrega;
                        brActaConformidadCabecera obrActaConformidadCabecera = new brActaConformidadCabecera();
                        idActaConformidadCabecera = obrActaConformidadCabecera.adicionar(obeActaConformidadPrincipal);
                        if (idActaConformidadCabecera != -1)
                        {
                            beActaConformidadCabecera parametros = new beActaConformidadCabecera();
                            parametros.ActaConformidadCabeceraId = idActaConformidadCabecera;
                            beActaConformidadPrincipal resultado = new beActaConformidadPrincipal();
                            resultado = obrActaConformidadCabecera.consultar(parametros);
                            if (resultado != null)
                            {
                                Session["listaVariosEntrega"] = null;
                                gvVariosEntrega.DataSource = null;
                                gvVariosEntrega.DataBind();
                                controlesBlanco(true);
                                controlesBlanco(false);
                                //lblInfoVarios.Text = "Registros Agregados para entregar: ";
                                byte[] pdfByte = oCodigoUsuario.crearActaConformidad(resultado, ViewState["UserNomComp"].ToString()); //ms.ToArray();
                                Session["bytePDF"] = pdfByte;
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirPDF", "window.open('../PDF.aspx', '_blank')", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL GUARDAR LA INFORMACION.');", true);
                            divModal.Style.Add("display", "block");
                            divDerivarEntrega.Style.Add("display", "block");
                        }
                        #endregion
                        break;
                }
                cambiarPagina(sender, e);
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                divModal.Style.Add("display", "block");
                divVarios.Style.Add("display", "block");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR.');", true);
            }
        }

        protected void obtenerSolicitante(object sender, EventArgs e) // OBTIENE UN SOLICITANTE DE ACXUERDO AL TIPO Y NUMERO DE DOC DE IDENTIDAD Y MUESTRA LOS DATOS EN PANTALLA
        {
            try
            {
                ImageButton imageButton = (ImageButton)sender;
                string comando = imageButton.CommandName;

                beSolicitante parametros = new beSolicitante();
                parametros.TipoDocumentoIdentidadId = (!ddlDEDocumentoIdent.SelectedValue.Equals("0") ? short.Parse(ddlDEDocumentoIdent.SelectedValue) : short.Parse(ddlVarDocumentoIdent.SelectedValue));
                parametros.NumeroDocumentoIdentidad = (!txtDENumeroDocIdent.Text.Equals("") ? txtDENumeroDocIdent.Text.Trim() : txtVarNumeroDocIdent.Text.Trim());
                brSolicitante obrSolicitante = new brSolicitante();
                beSolicitante obeSolicitante = obrSolicitante.consultarxIdentificacion(parametros);
                if (obeSolicitante != null)
                {
                    switch (comando)
                    {
                        case "solicitanteDE":
                            txtDEApePat.Text = obeSolicitante.PrimerApellido;
                            txtDEApeMat.Text = obeSolicitante.SegundoApellido;
                            txtDENombres.Text = obeSolicitante.Nombres;
                            txtDETelefono.Text = obeSolicitante.Telefono;
                            divModal.Style.Add("display", "block");
                            divDerivarEntrega.Style.Add("display", "block");
                            bloquearSolicitante(false, true);
                            break;
                        case "solicitanteVar":
                            txtVarApePat.Text = obeSolicitante.PrimerApellido;
                            txtVarApeMat.Text = obeSolicitante.SegundoApellido;
                            txtVarNombres.Text = obeSolicitante.Nombres;
                            txtVarTelefono.Text = obeSolicitante.Telefono;
                            divModal.Style.Add("display", "block");
                            divVarios.Style.Add("display", "block");
                            bloquearSolicitante(false, false);
                            break;
                    }
                }
                else
                {
                    switch (comando)
                    {
                        case "solicitanteDE":
                            divModal.Style.Add("display", "block");
                            divDerivarEntrega.Style.Add("display", "block");
                            break;
                        case "solicitanteVar":
                            divModal.Style.Add("display", "block");
                            divVarios.Style.Add("display", "block");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR.');", true);
            }
        }

        protected void limpiarSolicitante(object sender, EventArgs e) // LIMPIA LOS CONTROLES DEL SOLICITANTE TANTO EN PANTALLA COMO EN MEMORIA
        {
            ImageButton imageButton = (ImageButton)sender;
            string comando = imageButton.CommandName;

            switch (comando)
            {
                case "solicitanteDE":
                    ddlVarDocumentoIdent.SelectedValue = "0";
                    txtDENumeroDocIdent.Text = string.Empty;
                    txtDEApePat.Text = string.Empty;
                    txtDEApeMat.Text = string.Empty;
                    txtDENombres.Text = string.Empty;
                    txtDETelefono.Text = string.Empty;
                    bloquearSolicitante(true, true);
                    divModal.Style.Add("display", "block");
                    divDerivarEntrega.Style.Add("display", "block");
                    break;
                case "solicitanteVar":
                    ddlVarDocumentoIdent.SelectedValue = "0";
                    txtVarNumeroDocIdent.Text = string.Empty;
                    txtVarApePat.Text = string.Empty;
                    txtVarApeMat.Text = string.Empty;
                    txtVarNombres.Text = string.Empty;
                    txtVarTelefono.Text = string.Empty;
                    bloquearSolicitante(true, false);
                    divModal.Style.Add("display", "block");
                    divVarios.Style.Add("display", "block");
                    break;
            }
        }

        protected void mostrarPanelVarios(object sender, EventArgs e) // MUESTRA LA LISTA DE LOS REGISTROS SELECCIONADOS PARA RECEPCION O ENTREGA
        {
            try
            {
                Button cButton = (Button)sender;
                string comando = cButton.CommandName;
                List<beRegistroPrevio> listaRegPrevio = new List<beRegistroPrevio>();
                switch (comando)
                {
                    case "recepcionMultiple":
                        listaRegPrevio = (List<beRegistroPrevio>)Session["listaVariosRecepcion"];
                        gvVariosEntrega.DataSource = listaRegPrevio;
                        gvVariosEntrega.DataBind();
                        break;
                    case "entregaMultiple":
                        listaRegPrevio = (List<beRegistroPrevio>)Session["listaVariosEntrega"];
                        gvVariosEntrega.DataSource = listaRegPrevio;
                        gvVariosEntrega.DataBind();
                        break;
                }

                tipoMultiple = comando;
                controlesBlanco(true);
                controlesBlanco(false);
                bloquearSolicitante(true, false);
                divModal.Style.Add("display", "block");
                divVarios.Style.Add("display", "block");
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected void quitarRegistroLista(object sender, EventArgs e) // LIMPIA LAS LISTAS DE REGISTROS SELECCIONADOS TANTO EN PANTALLA COMO EN MEMORIA
        {
            ImageButton imageButton = (ImageButton)sender;
            TableCell tableCell = (TableCell)imageButton.Parent;
            GridViewRow row = (GridViewRow)tableCell.Parent;
            try
            {
                if (tipoMultiple.Equals("recepcionMultiple"))
                {
                    List<beRegistroPrevio> listaRegPrevio = new List<beRegistroPrevio>();
                    listaRegPrevio = (List<beRegistroPrevio>)Session["listaVariosRecepcion"];
                    if (listaRegPrevio.Count > 0)
                    {
                        int pos = listaRegPrevio.FindIndex(x => x.ConIdent.Equals(row.Cells[0].Text));
                        if (pos != -1)
                        {
                            listaRegPrevio.Remove(listaRegPrevio[pos]);
                            List<beRegistroPrevio> listaRegPrevioOrden = listaRegPrevio.OrderByDescending(x => x.ConIdent).ToList();
                            gvVariosEntrega.DataSource = listaRegPrevioOrden;
                            gvVariosEntrega.DataBind();
                            Session["listaVariosRecepcion"] = listaRegPrevio;
                            agregarDetalle(0);
                        }
                    }
                }
                else
                {
                    List<beRegistroPrevio> listaRegPrevio = new List<beRegistroPrevio>();
                    listaRegPrevio = (List<beRegistroPrevio>)Session["listaVariosEntrega"];
                    if (listaRegPrevio.Count > 0)
                    {
                        int pos = listaRegPrevio.FindIndex(x => x.ConIdent.Equals(row.Cells[0].Text));
                        if (pos != -1)
                        {
                            listaRegPrevio.Remove(listaRegPrevio[pos]);
                            List<beRegistroPrevio> listaRegPrevioOrden = listaRegPrevio.OrderByDescending(x => x.ConIdent).ToList();
                            gvVariosEntrega.DataSource = listaRegPrevioOrden;
                            gvVariosEntrega.DataBind();
                            Session["listaVariosEntrega"] = listaRegPrevio;
                            agregarDetalle(1);
                        }
                    }
                }
                divModal.Style.Add("display", "block");
                divVarios.Style.Add("display", "block");
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }
        #endregion
        #region Funciones
        protected void verTemplate(object sender, GridViewRowEventArgs e) // PERMITE MOSTRAR LAS HERRAMIENTAS EN LOS REGISTROS DE LA GRILLA DE ACUERDO A SU ESTADO O CONDICION PARTICULAR
        {
            try
            {
                if (e.Row.DataItemIndex > -1)
                {
                    List<beRegistroPrevio> lbeRegistroPrevio = new List<beRegistroPrevio>();
                    lbeRegistroPrevio = (List<beRegistroPrevio>)Session["ResultadoConsulta"];
                    int pos = lbeRegistroPrevio.FindIndex(x => x.ConIdent.Equals(e.Row.Cells[1].Text));
                    if (pos != -1)
                    {
                        if (lbeRegistroPrevio[pos].ConActaRecepcion > 0)
                        {
                            e.Row.Cells[13].Controls[1].Visible = true;
                        }
                        if (e.Row.Cells[4].Text.Equals("REGISTRO PREVIO"))
                        {
                            if (lbeRegistroPrevio[pos].ConActaRecepcion > 0)
                            {
                                e.Row.Cells[12].Controls[3].Visible = true;
                            }
                            else
                            {
                                e.Row.Cells[12].Controls[1].Visible = true;
                                e.Row.Cells[0].Controls[3].Visible = true;
                            }
                            oCodigoUsuario.colorCeldas("#6D98E4", "#000000", sender, e);
                            
                        }
                        if (e.Row.Cells[4].Text.Equals("DERIVADO PARA COMPLETAR"))
                        {
                            e.Row.Cells[12].Controls[3].Visible = true;
                        }
                        if (e.Row.Cells[4].Text.Equals("REGISTRADO"))
                        {
                            e.Row.Cells[12].Controls[3].Visible = true;
                        }
                        if (e.Row.Cells[4].Text.Equals("DERIVADO"))
                        {
                            e.Row.Cells[12].Controls[3].Visible = true;
                        }
                        if (e.Row.Cells[4].Text.Equals("APROBADO"))
                        {
                            e.Row.Cells[12].Controls[3].Visible = true;
                        }
                        if (e.Row.Cells[4].Text.Equals("CONTROL DE CALIDAD"))
                        {
                            if (lbeRegistroPrevio[pos].FlagRegistroCompleto)
                            {
                                e.Row.Cells[4].Text = e.Row.Cells[4].Text + " (OBSERVADO)";
                            }
                        }
                        if (e.Row.Cells[4].Text.Equals("EMITIDO (IMPRESO)"))
                        {
                            if (!lbeRegistroPrevio[pos].ConFlagEntregado)
                            {
                                e.Row.Cells[12].Controls[5].Visible = true;
                                e.Row.Cells[0].Controls[1].Visible = true;
                                oCodigoUsuario.colorCeldas("#6D98E4", "#000000", sender, e);
                            }
                            else
                            {
                                e.Row.Cells[4].Text = e.Row.Cells[4].Text + " / ENTREGADO";
                                e.Row.Cells[12].Controls[7].Visible = true;
                            }
                        }
                        if (e.Row.Cells[4].Text.Equals("VENCIDO"))
                        {
                            e.Row.Cells[12].Controls[3].Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL MOSTRAR LA INFORMACION');", true);
            }
        }

        protected void paginacion(long totalPag, long residuo) // TRABAJA EN CONJUNTO CON EL CONTROL cambiarPagina()
        {
            try
            {
                int n = 1;
                ddlPaginas.Items.Clear();
                if (!totalPag.Equals(0))
                {
                    while (n <= totalPag)
                    {
                        ddlPaginas.Items.Add(n.ToString());
                        n++;
                    }
                }
                if (residuo > 0)
                {
                    ddlPaginas.Items.Add((totalPag + 1).ToString());
                    lblTotalPaginas.Text = "de " + (totalPag + 1).ToString();
                }
                else
                {
                    lblTotalPaginas.Text = "de " + totalPag.ToString();
                }
                if (ViewState["pag"] != null)
                {
                    ddlPaginas.SelectedValue = ViewState["pag"].ToString();
                    ViewState["pag"] = null;
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected beRegistroPrevio obtenerRegistro(object sender, EventArgs e)  // PERMITE OBTENER TODA LA INFORMACION DE UN REGISTRO SELECCIONADO
        {
            beRegistroPrevio obeRegistroPrevio = new beRegistroPrevio();
            try
            {
                ImageButton imageButton = (ImageButton)sender;
                TableCell tableCell = (TableCell)imageButton.Parent;
                GridViewRow row = (GridViewRow)tableCell.Parent;
                gvRegPrev.SelectedIndex = row.RowIndex;
                int fila = row.RowIndex;

                List<beRegistroPrevio> lbeRegistroPrevio = (List<beRegistroPrevio>)Session["ResultadoConsulta"];
                if (lbeRegistroPrevio != null)
                {
                    int pos = lbeRegistroPrevio.FindIndex(x => x.ConIdent.Equals(row.Cells[1].Text));
                    if (pos != -1) { obeRegistroPrevio = lbeRegistroPrevio[pos]; }
                    else { obeRegistroPrevio = null; }
                }
                else { obeRegistroPrevio = null; }
            }
            catch (Exception ex)
            {
                obeRegistroPrevio = null;
                obrGeneral.grabarLog(ex);
            }
            return (obeRegistroPrevio);
        }

        protected beCarneIdentidad obtenerRegistroCI(short carneID) // PERMITE OBTENER TODA LA INFORMACION DE UN REGISTRO SELECCIONADO
        {
            beCarneIdentidad obeCarneIdentidadCI = new beCarneIdentidad();
            brCarneIdentidadPrincipal obrCarneIdentidadPrincipal = new brCarneIdentidadPrincipal();
            obeCarneIdentidadCI = obrCarneIdentidadPrincipal.consultarxId(carneID);
            return (obeCarneIdentidadCI);
        }

        protected void cargarComboNull(DropDownList controlDropDown) // CARGA UN COMBO CON UNA LISTA VACIA Y LO BLOQUEA
        {
            try
            {
                List<beNull> lbeNull = new List<beNull>();
                beNull obeNull = new beNull();
                obeNull.Id = "0";
                obeNull.Seleccione = "<Seleccione>";
                lbeNull.Add(obeNull);
                controlDropDown.DataSource = lbeNull;
                controlDropDown.DataValueField = "Id";
                controlDropDown.DataTextField = "Seleccione";
                controlDropDown.DataBind();
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected short obtenerIdEstado(string claveEstado) // OBTIENE EL ID DE UNA LISTA DE ACUERDO A UNA CLAVE
        {
            int pos = ListaEstados.FindIndex(x => x.DescripcionCorta.Equals(claveEstado));
            short estadoReg = (pos != -1 ? ListaEstados[pos].Estadoid : short.Parse("0"));
            return (estadoReg);
        }

        protected short obtenerTipoObs(string claveObs) // OBTIENE EL ID DE UNA LISTA DE ACUERDO A UNA CLAVE
        {
            int pos = ListaTipoObs.FindIndex(x => x.Descripcion.Equals(claveObs));
            short estadoObs = (pos != -1 ? ListaTipoObs[pos].Parametroid : short.Parse("0"));
            return (estadoObs);
        } 

        protected void bloquearSolicitante(bool valor, bool controles) // BLOQUEA LOS CONTROLES DE LA INFORMACION DEL SOLICITANTE
        {
            if (controles)
            {
                ddlDEDocumentoIdent.Enabled = valor;
                txtDENumeroDocIdent.Enabled = valor;
                txtDEApePat.Enabled = valor;
                txtDEApeMat.Enabled = valor;
                txtDENombres.Enabled = valor;
                txtDETelefono.Enabled = valor;
            }
            else
            {
                ddlVarDocumentoIdent.Enabled = valor;
                txtVarNumeroDocIdent.Enabled = valor;
                txtVarApePat.Enabled = valor;
                txtVarApeMat.Enabled = valor;
                txtVarNombres.Enabled = valor;
                txtVarTelefono.Enabled = valor;
            }
        }

        protected bool validarTipoEntrada(beRegistroPrevio obeRegistroPrevio, int tipoLista) // VALIDA EL TIPO DE EMTRADA DE LA INFORMACION (CONSULARES / PRIVILEGIOS)
        {
            bool exito = false;
            try
            {
                string tipoEntrada = (obeRegistroPrevio.ConTipoEntrada != null & !obeRegistroPrevio.ConTipoEntrada.Equals("-") & !obeRegistroPrevio.ConTipoEntrada.Equals("") ? obeRegistroPrevio.ConTipoEntrada : "error");
                if (!tipoEntrada.Equals("error"))
                {
                    List<beRegistroPrevio> listaRegPrevio = new List<beRegistroPrevio>();
                    if (tipoLista == 0)
                    {
                        listaRegPrevio = (Session["listaVariosRecepcion"] != null ? (List<beRegistroPrevio>)Session["listaVariosRecepcion"] : null);
                    }
                    else
                    {
                        listaRegPrevio = (Session["listaVariosEntrega"] != null ? (List<beRegistroPrevio>)Session["listaVariosEntrega"] : null);
                    }
                    if (listaRegPrevio != null)
                    {
                        if (listaRegPrevio.Count > 0)
                        {
                            int pos = -1;
                            if (tipoEntrada.Equals("CONSULARES"))
                            {
                                pos = listaRegPrevio.FindIndex(x => x.ConTipoEntrada.Equals("PRIVILEGIOS"));
                            }
                            else
                            {
                                pos = listaRegPrevio.FindIndex(x => x.ConTipoEntrada.Equals("CONSULARES"));
                            }
                            if (pos == -1)
                            {
                                exito = true;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                exito = false;
                obrGeneral.grabarLog(ex);
            }
            return (exito);
        }

        protected void agregarDetalle(int tipoDetalle) // AGREGA DATOS COMPLEMENTARIOS PARA LA TRANSACCION DE DATOS
        {
            try
            {
                short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
                string ip = ViewState["IP"].ToString();
                if (tipoDetalle == 0)
                {
                    listaRecepcionDetalle = new List<beActaRecepcionDetalle>();
                    List<beRegistroPrevio> listaRegPrevio = new List<beRegistroPrevio>();
                    if (Session["listaVariosRecepcion"] != null)
                    {
                        listaRegPrevio = (List<beRegistroPrevio>)Session["listaVariosRecepcion"];
                        if (listaRegPrevio != null)
                        {
                            if (listaRegPrevio.Count > 0)
                            {
                                beActaRecepcionDetalle obeActaRecepcionDetalle;
                                foreach (beRegistroPrevio obeRegistroPrevio in listaRegPrevio)
                                {
                                    obeActaRecepcionDetalle = new beActaRecepcionDetalle();
                                    obeActaRecepcionDetalle.CarneIdentidadId = obeRegistroPrevio.ConCarneIdentidadId;
                                    obeActaRecepcionDetalle.UsuarioCreacion = usuarioId;
                                    obeActaRecepcionDetalle.IpCreacion = ip;
                                    listaRecepcionDetalle.Add(obeActaRecepcionDetalle);
                                }
                            }
                        }
                    }
                }
                else
                {
                    listaConformidadDetalle = new List<beActaConformidadDetalle>();
                    List<beRegistroPrevio> listaRegPrevio = new List<beRegistroPrevio>();
                    if (Session["listaVariosEntrega"] != null)
                    {
                        listaRegPrevio = (List<beRegistroPrevio>)Session["listaVariosEntrega"];
                        if (listaRegPrevio != null)
                        {
                            if (listaRegPrevio.Count > 0)
                            {
                                beActaConformidadDetalle obeActaConformidadDetalle;
                                foreach (beRegistroPrevio obeRegistroPrevio in listaRegPrevio)
                                {
                                    obeActaConformidadDetalle = new beActaConformidadDetalle();
                                    obeActaConformidadDetalle.CarneIdentidadId = obeRegistroPrevio.ConCarneIdentidadId;
                                    obeActaConformidadDetalle.UsuarioCreacion = usuarioId;
                                    obeActaConformidadDetalle.IpCreacion = ip;
                                    listaConformidadDetalle.Add(obeActaConformidadDetalle);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        protected void controlesBlanco(bool valor) // LIMPIA LOS CONTROLES DEL FORMULARIO
        {
            if (valor)
            {
                txtDEApePat.Text = string.Empty;
                txtDEApeMat.Text = string.Empty;
                txtDENombres.Text = string.Empty;
                ddlDEDocumentoIdent.SelectedValue = "0";
                txtDENumeroDocIdent.Text = string.Empty;
                txtDETelefono.Text = string.Empty;
                txtDEObservacion.Text = string.Empty;
            }
            else
            {
                txtVarApePat.Text = string.Empty;
                txtVarApeMat.Text = string.Empty;
                txtVarNombres.Text = string.Empty;
                ddlVarDocumentoIdent.SelectedValue = "0";
                txtVarNumeroDocIdent.Text = string.Empty;
                txtVarTelefono.Text = string.Empty;
                txtVarObservacion.Text = string.Empty;
            }
        }
        #endregion
        //protected void pruebaReporte(object sender, EventArgs e)
        //{
        //    byte[] pdfByte = oCodigoUsuario.crearReporte(ViewState["UserNomComp"].ToString()); //ms.ToArray();
        //    Session["bytePDF"] = pdfByte;
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirPDF", "window.open('../PDF.aspx', '_blank')", true);   
        //}
    }
}