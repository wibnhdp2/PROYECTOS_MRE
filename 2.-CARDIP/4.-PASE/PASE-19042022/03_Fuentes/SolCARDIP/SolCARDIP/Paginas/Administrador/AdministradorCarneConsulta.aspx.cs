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

namespace SolCARDIP.Paginas.Administrador
{
    public partial class AdministradorCarneConsulta : System.Web.UI.Page
    {
        brGeneral obrGeneral = new brGeneral();
        CodigoUsuario oCodigoUsuario = new CodigoUsuario();
        UIEncriptador oUIEncriptador = new UIEncriptador();
        public static List<beEstado> ListaEstados = new List<beEstado>();
        public static List<beParametro> ListaObservaciones = new List<beParametro>();
        public static beCarneIdentidad obeCarneIdentidadDerivar = new beCarneIdentidad();
        public static string controlId = "";
        public static string claveEstado = "";
        public static bool claveNuevo = false;
        public static bool claveRenovar = false;
        public static bool claveEval = false;
        public static string valueTKSEG;
        public static string srtAl = "";
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
                    #endregion
                    #region Generales
                    //GENERALES
                    if (Session["Generales"] != null)
                    {
                        beGenerales obeGenerales = new beGenerales();
                        obeGenerales = (beGenerales)Session["Generales"];
                        // PAIS
                        obeGenerales.ListaPaises.Insert(0, new bePais { Paisid = 0, Nombre = "<Seleccione>" });
                        ddlNacionalidad.DataSource = obeGenerales.ListaPaises;
                        ddlNacionalidad.DataValueField = "Paisid";
                        ddlNacionalidad.DataTextField = "Nombre";
                        ddlNacionalidad.DataBind();
                        // CALIDAD MIGRATORIA
                        obeGenerales.ListaCalidadMigratoriaNivelPrincipal.Insert(0, new beCalidadMigratoria { CalidadMigratoriaid = 0, Nombre = "<Seleccione>" });
                        ddlCalidadMigratoriaPri.DataSource = obeGenerales.ListaCalidadMigratoriaNivelPrincipal;
                        ddlCalidadMigratoriaPri.DataValueField = "CalidadMigratoriaid";
                        ddlCalidadMigratoriaPri.DataTextField = "Nombre";
                        ddlCalidadMigratoriaPri.DataBind();
                        cargarComboNull(ddlCalidadMigratoriaSec);
                        // TITULAR - DEPENDIENTE (PARAMETRO)
                        obeGenerales.TitularDependienteParametros.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                        ddlTitDep.DataSource = obeGenerales.TitularDependienteParametros;
                        ddlTitDep.DataValueField = "Parametroid";
                        ddlTitDep.DataTextField = "Descripcion";
                        ddlTitDep.DataBind();
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
                        // TIPO OBSERVACION
                        obeGenerales.TipoObservacion.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                        ddlTipoObs.DataSource = obeGenerales.TipoObservacion;
                        ddlTipoObs.DataValueField = "Parametroid";
                        ddlTipoObs.DataTextField = "Descripcion";
                        ddlTipoObs.DataBind();
                        // TIPO DUPLICADO
                        obeGenerales.TipoDuplicado.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                        ddlTipoDuplicado.DataSource = obeGenerales.TipoDuplicado;
                        ddlTipoDuplicado.DataValueField = "Parametroid";
                        ddlTipoDuplicado.DataTextField = "Descripcion";
                        ddlTipoDuplicado.DataBind();
                        // GENERO
                        obeGenerales.ListaParametroGenero.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                        ddlSexo.DataSource = obeGenerales.ListaParametroGenero;
                        ddlSexo.DataValueField = "Parametroid";
                        ddlSexo.DataTextField = "Descripcion";
                        ddlSexo.DataBind();
                        // LISTAS
                        ListaEstados = obeGenerales.ListaEstados;
                        ListaObservaciones = obeGenerales.TipoObservacion;
                    }
                    #endregion
                    #region Otros Controles
                    // PERIODO ------------------------------------
                    ListItem itemSel = new ListItem();
                    itemSel.Text = "<Seleccione>";
                    itemSel.Value = "0";
                    ddlPeriodo.Items.Add(itemSel);
                    for (int i = 2017; i <= 2020; i++)
                    {
                        ListItem item = new ListItem();
                        item.Text = i.ToString();
                        item.Value = i.ToString();
                        ddlPeriodo.Items.Add(item);
                    }
                    //if (ddlPeriodo.Items.Count > 1) { ddlPeriodo.SelectedValue = "2017"; }
                    // TIEMPOS ------------------------------------
                    ListItem itemSel0 = new ListItem();
                    itemSel0.Text = "<->";
                    itemSel0.Value = "0";
                    ddlTiempos.Items.Add(itemSel0);
                    // DIAS
                    ListItem itemSel1 = new ListItem();
                    itemSel1.Text = "1 dia";
                    itemSel1.Value = "day 1";
                    ddlTiempos.Items.Add(itemSel1);
                    ListItem itemSel2 = new ListItem();
                    itemSel2.Text = "15 dias";
                    itemSel2.Value = "day 15";
                    ddlTiempos.Items.Add(itemSel2);
                    // MESES
                    ListItem itemSel3 = new ListItem();
                    itemSel3.Text = "1 Mes";
                    itemSel3.Value = "month 1";
                    ddlTiempos.Items.Add(itemSel3);
                    ListItem itemSel4 = new ListItem();
                    itemSel4.Text = "6 Meses";
                    itemSel4.Value = "month 6";
                    ddlTiempos.Items.Add(itemSel4);
                    // AÑOS
                    ListItem itemSel5 = new ListItem();
                    itemSel5.Text = "1 Año";
                    itemSel5.Value = "year 1";
                    ddlTiempos.Items.Add(itemSel5);
                    ListItem itemSel6 = new ListItem();
                    itemSel6.Text = "3 Años";
                    itemSel6.Value = "year 3";
                    ddlTiempos.Items.Add(itemSel6);
                    ListItem itemSel7 = new ListItem();
                    itemSel7.Text = "5 Años";
                    itemSel7.Value = "year 5";
                    ddlTiempos.Items.Add(itemSel7);
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
                    #region TKSEG
                    valueTKSEG = oCodigoUsuario.generateValTK(Path.GetFileNameWithoutExtension(Request.Url.LocalPath));//"aAsaCdV3mDbklEEX1ksi3ePwT/RmuRcj";
                    TKSEGENC.Value = valueTKSEG;
                    #endregion
                }
                else
                {
                    #region PruebaValorURL
                    csUsuarioBE objUsuarioBE = new csUsuarioBE();
                    objUsuarioBE = (csUsuarioBE)Session["usuario"];
                    string qs = Request.QueryString["valS"].ToString();
                    bool exito = oCodigoUsuario.validarSTR(qs, objUsuarioBE.Alias, obrGeneral.host, ViewState["srtAl"].ToString());
                    if (!exito) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensajeError", "location.href='../../mensajes.aspx';", true); return; }
                    #endregion
                    #region Postback
                    divModal.Style.Add("display", "none");
                    divModal2.Style.Add("display", "none");
                    divMovimientos.Style.Add("display", "none");
                    divAprobar.Style.Add("display", "none");
                    divDevolver.Style.Add("display", "none");
                    divEmitidoImpr.Style.Add("display", "none");
                    divReimpresion.Style.Add("display", "none");
                    obtenerNacionalidadPais(sender, e);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "preloader", "preloader();", true);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL CARGAR LA PAGINA');", true);
            }
        }

        #region Controles
        protected void buscarRegistros(object sender, EventArgs e) // BUSCA REGISTROS DE ACUERDO A FILTROS
        {
            try
            {
                #region TKSEG
                string TKSEG = TKSEGENC.Value;
                bool exitoTKSEG = oCodigoUsuario.compareValTK(TKSEG, valueTKSEG);
                if (!exitoTKSEG)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensajeError", "location.href='../../mensajes.aspx';", true);
                    return;
                }
                #endregion
                beCarneIdentidad parametros = new beCarneIdentidad();
                parametros.Periodo = int.Parse(ddlPeriodo.SelectedValue);
                parametros.IdentMesaPartes = txtMesaPartes.Text.ToUpper().Trim();
                parametros.IdentNumero = (txtNumeroIdent.Text.Equals("") ? 0 : int.Parse(txtNumeroIdent.Text));
                parametros.CarneNumero = txtNumeroCarne.Text;
                parametros.Estadoid = short.Parse(ddlEstado.SelectedValue);
                parametros.CalidadMigratoriaid = short.Parse(ddlCalidadMigratoriaPri.SelectedValue);
                parametros.CalidadMigratoriaSecId = short.Parse(ddlCalidadMigratoriaSec.SelectedValue);
                parametros.CatMision = short.Parse(ddlCategoriaOfcoEx.SelectedValue);
                parametros.OficinaConsularExid = short.Parse(ddlMision.SelectedValue);
                parametros.FechaEmision = (txtFechaEmision.Text.Equals("") ? new DateTime(1900, 1, 1) : DateTime.Parse(txtFechaEmision.Text));
                parametros.FechaVencimiento = (txtFechaVen.Text.Equals("") ? new DateTime(1900, 1, 1) : DateTime.Parse(txtFechaVen.Text));
                parametros.ApePatPersona = txtApePat.Text.Trim().ToUpper();
                parametros.ApeMatPersona = txtApeMat.Text.Trim().ToUpper();
                parametros.NombresPersona = txtNomrbes.Text.Trim().ToUpper();
                parametros.PaisPersona = short.Parse(ddlNacionalidad.SelectedValue);
                parametros.NumPag = 1;
                parametros.CantReg = long.Parse(obrGeneral.cantRegxPag);
                brCarneIdentidadPrincipal obrCarneIdentidadPrincipal = new brCarneIdentidadPrincipal();
                beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
                obeCarneIdentidadPrincipal = obrCarneIdentidadPrincipal.consultar(parametros);
                Session["ResultadoConsulta"] = obeCarneIdentidadPrincipal.ListaConsulta;
                Session["ParametrosConsulta"] = parametros;
                gvCarne.DataSource = (obeCarneIdentidadPrincipal.ListaConsulta != null ? obeCarneIdentidadPrincipal.ListaConsulta : null);
                gvCarne.DataBind();
                // PAGINACION
                long _TotalPag = obeCarneIdentidadPrincipal.Paginacion.Total;
                long _Residuo = obeCarneIdentidadPrincipal.Paginacion.Residuo;
                long _TotalRegistros = obeCarneIdentidadPrincipal.Paginacion.TotalRegistros;
                paginacion(_TotalPag, _Residuo);
                lblTotalRegistros.Text = (obeCarneIdentidadPrincipal.ListaConsulta != null ? "Resultado de la Busqueda: " + _TotalRegistros.ToString() + " registros encontrados" : "Resultado de la Busqueda: 0registros encontrados");
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
            }
        }

        protected void cambiarPagina(object sender, EventArgs e) // DROPDOWLIST DE PAGINACION DE LA GRILLA
        {
            try
            {
                beCarneIdentidad parametros = new beCarneIdentidad();
                parametros = (beCarneIdentidad)Session["ParametrosConsulta"];
                if (ddlPaginas.Items.Count > 0) { parametros.NumPag = long.Parse(ddlPaginas.SelectedValue); }
                brCarneIdentidadPrincipal obrCarneIdentidadPrincipal = new brCarneIdentidadPrincipal();
                beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
                obeCarneIdentidadPrincipal = obrCarneIdentidadPrincipal.consultar(parametros);
                Session["ResultadoConsulta"] = obeCarneIdentidadPrincipal.ListaConsulta;
                Session["ParametrosConsulta"] = parametros;
                gvCarne.DataSource = (obeCarneIdentidadPrincipal.ListaConsulta != null ? obeCarneIdentidadPrincipal.ListaConsulta : null);
                gvCarne.DataBind();
                // PAGINACION
                long _TotalPag = obeCarneIdentidadPrincipal.Paginacion.Total;
                long _Residuo = obeCarneIdentidadPrincipal.Paginacion.Residuo;
                long _TotalRegistros = obeCarneIdentidadPrincipal.Paginacion.TotalRegistros;
                paginacion(_TotalPag, _Residuo);
                lblTotalRegistros.Text = (obeCarneIdentidadPrincipal.ListaConsulta != null ? "Resultado de la Busqueda: " + _TotalRegistros.ToString() + " registros encontrados" : "Resultado de la Busqueda: 0registros encontrados");
                ddlPaginas.SelectedValue = parametros.NumPag.ToString();
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected void obtenerNacionalidadPais(object sender, EventArgs e) // DROPDOWLIST - A LA SELECCION DE UN PAIS MUESTRA LA NACIONALIDAD 
        {
            try
            {
                if (Session["Generales"] != null)
                {
                    beGenerales obeGenerales = new beGenerales();
                    obeGenerales = (beGenerales)Session["Generales"];
                    short PaisId = short.Parse(ddlNacionalidad.SelectedValue);
                    bePais obePais = oCodigoUsuario.obtenerDatosPais(PaisId, obeGenerales.ListaPaises);
                    if (obePais != null)
                    {
                        lblNacionalidad.Text = obePais.Nacionalidad;
                        lblNacionalidad.ForeColor = (obePais.Nacionalidad.Equals("[ NO DEFINIDO ]") ? Color.Red : Color.Green);
                    }
                    else
                    {
                        lblNacionalidad.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
            }
        }

        protected void seleccionarSexo(object sender, EventArgs e) // DROPDOWLIST - A LA SELECCION DE UN GENERO MODIFICA ALGUNOS CARGOS PARA DIFERENCIARLOS ENTRE ELLOS 
        {
            try
            {
                if (!ddlSexo.SelectedValue.Equals("0"))
                {
                    if (Session["Generales"] != null)
                    {
                        beGenerales obeGenerales = new beGenerales();
                        obeGenerales = (beGenerales)Session["Generales"];
                        int posGen = obeGenerales.ListaParametroGenero.FindIndex(x => x.Descripcion.Equals(ddlSexo.SelectedItem.Text));
                        if (posGen != -1)
                        {
                            string valorSexo = obeGenerales.ListaParametroGenero[posGen].Valor;
                            List<beParametro> listaEstadoCivil = new List<beParametro>();
                            listaEstadoCivil = obeGenerales.ListaParametroEstadoCivil.FindAll(x => x.Valor.Equals(valorSexo));
                            if (listaEstadoCivil != null & listaEstadoCivil.Count > 0)
                            {
                                //listaEstadoCivil.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                                //ddlEstadoCivil.DataSource = listaEstadoCivil;
                                //ddlEstadoCivil.DataValueField = "Parametroid";
                                //ddlEstadoCivil.DataTextField = "Descripcion";
                                //ddlEstadoCivil.DataBind();
                                //ddlEstadoCivil.Enabled = true;
                                //ddlEstadoCivil.Focus();
                                seleccionarCalidadMigratoria(sender, e);
                            }
                            else
                            {
                                //cargarComboNull(ddlEstadoCivil);
                                //ddlEstadoCivil.Enabled = false;
                            }
                        }
                    }
                }
                else
                {
                    //cargarComboNull(ddlEstadoCivil);
                    cargarComboNull(ddlCalidadMigratoriaSec);
                    seleccionarCalidadMigratoria(sender, e);
                    //ddlEstadoCivil.Enabled = false;
                    ddlCalidadMigratoriaSec.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected void seleccionarCalidadMigratoria(object sender, EventArgs e) // DROPDOWLIST - A LA SELECCION DE CALIDAD MUESTRA LOS CARGOS DISPONIBLES PARA ESA MISMA
        {
            try
            {
                if (Session["Generales"] != null)
                {
                    beGenerales obeGenerales = new beGenerales();
                    obeGenerales = (beGenerales)Session["Generales"];
                    obeGenerales.ListaCalidadMigratoriaNivelSecundario.Insert(0, new beCalidadMigratoria { CalidadMigratoriaid = 0, Nombre = "<Seleccione>" });
                    List<beCalidadMigratoria> lbeCalidadMigratoria = new List<beCalidadMigratoria>();
                    int pos = obeGenerales.TitularDependienteParametros.FindIndex(x => x.Valor.Equals(hdrbt.Value));
                    short Referencia = short.Parse(ddlCalidadMigratoriaPri.SelectedValue);
                    short TitularDependiente = short.Parse(ddlTitDep.SelectedValue);
                    short genero = short.Parse(ddlSexo.SelectedValue);
                    if (TitularDependiente > 0)
                    {
                        lbeCalidadMigratoria = oCodigoUsuario.obtenerListaTitularDependiente(Referencia, TitularDependiente, genero, obeGenerales.ListaCalidadMigratoriaNivelSecundario);
                    }
                    if (lbeCalidadMigratoria.Count > 0)
                    {
                        ddlCalidadMigratoriaSec.DataSource = lbeCalidadMigratoria;
                        ddlCalidadMigratoriaSec.DataValueField = "CalidadMigratoriaid";
                        ddlCalidadMigratoriaSec.DataTextField = "Nombre";
                        ddlCalidadMigratoriaSec.DataBind();
                        //lblMensajeCalidadMigratoria.Text = (lbeCalidadMigratoria.Count == 1 ? "No se encontraron opciones de cargo para esta calidad migratoria" : "");
                        ddlCalidadMigratoriaSec.Enabled = true;
                    }
                    else
                    {
                        cargarComboNull(ddlCalidadMigratoriaSec);
                        //lblMensajeCalidadMigratoria.Text = (!ddlCalidadMigratoriaPri.SelectedValue.Equals("0") & !ddlTitDep.SelectedValue.Equals("0") ? "No se encontraron opciones de cargo para esta calidad migratoria" : "");
                        ddlCalidadMigratoriaSec.Enabled = false;
                    }
                    // OBTIENE DEFINICION DE CALIDAD MIGRATORIA
                    //if (!ddlCalidadMigratoriaPri.SelectedValue.Equals("0"))
                    //{
                    //    int posTexto = obeGenerales.ListaCalidadMigratoriaNivelPrincipal.FindIndex(x => x.CalidadMigratoriaid == short.Parse(ddlCalidadMigratoriaPri.SelectedValue));
                    //    if (posTexto != -1)
                    //    {
                    //        txtCalidadMigratoria.Text = obeGenerales.ListaCalidadMigratoriaNivelPrincipal[posTexto].Definicion;
                    //    }
                    //}
                    //else
                    //{
                    //    txtCalidadMigratoria.Text = "";
                    //}
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
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
                string comando = imageButton.CommandName;
                beCarneIdentidad obeCarneIdentidad = new beCarneIdentidad();
                obeCarneIdentidad = obtenerRegistro(sender, e);
                if (obeCarneIdentidad != null)
                {
                    short obsId = -1;
                    switch (comando)
                    {
                        case "devolverRegistro":
                            claveEstado = "REGISTRADO";
                            claveEval = false;
                            List<beParametro> ListaTipoObs = new List<beParametro>();
                            ListaTipoObs = ListaObservaciones.FindAll(x => x.Descripcion.Equals("MODIFICACIÓN DE DATOS") | x.Descripcion.Equals("OTROS") | x.Descripcion.Equals("<Seleccione>"));
                            ddlTipoObs.DataSource = ListaTipoObs;
                            ddlTipoObs.DataBind();
                            claveRenovar = obeCarneIdentidad.Renovar;
                            obeCarneIdentidadDerivar = obeCarneIdentidad;
                            lblDevolverIdetn.Text = obeCarneIdentidad.ConIdent;
                            lblDevolverFuncionario.Text = obeCarneIdentidad.ConFuncionario;
                            lblDevolverPais.Text = obeCarneIdentidad.ConPaisNacionalidad;
                            lblDevolverMision.Text = obeCarneIdentidad.ConOficinaConsularEx;
                            lblDevolverCalMig.Text = obeCarneIdentidad.ConCalidadMigratoria;
                            ddlTipoObs.SelectedValue = "0";
                            txtDetalleObs.Text = string.Empty;
                            txtAprobarFechaEmision.Text = string.Empty;
                            txtAprobarFechaVencimiento.Text = string.Empty;
                            divModal.Style.Add("display", "block");
                            divDevolver.Style.Add("display", "block");
                            break;
                        case "aprobarRegistro":
                            claveEstado = "APROBADO";
                            claveEval = false;
                            if (obeCarneIdentidad.ConFechaEmision.Equals("-")) { claveNuevo = true; }
                            else { claveNuevo = false; }
                            claveRenovar = obeCarneIdentidad.Renovar;
                            obeCarneIdentidadDerivar = obeCarneIdentidad;
                            lblAprobarIdentificador.Text = obeCarneIdentidad.ConIdent;
                            lblAprobarFuncionario.Text = obeCarneIdentidad.ConFuncionario;
                            lblAprobarPais.Text = obeCarneIdentidad.ConPaisNacionalidad;
                            lblAprobarMision.Text = obeCarneIdentidad.ConOficinaConsularEx;
                            lblAprobarCalMig.Text = obeCarneIdentidad.ConCalidadMigratoria;
                            ddlTiempos.SelectedValue = "0";
                            ddlTipoObs.SelectedValue = "0";
                            txtDetalleObs.Text = string.Empty;
                            txtAprobarFechaEmision.Text = DateTime.Now.Date.ToShortDateString(); //(obeCarneIdentidad.ConFechaEmision.Equals("-") ? DateTime.Now.Date.ToShortDateString() : obeCarneIdentidad.ConFechaEmision);
                            hdfldFechaEmision.Value = obeCarneIdentidad.ConFechaEmision;
                            txtAprobarFechaVencimiento.Text = (!obeCarneIdentidad.FlagControlCalidad ? string.Empty : obeCarneIdentidad.ConFechaVen); //(obeCarneIdentidad.ConFechaVen.Equals("-") ? string.Empty : obeCarneIdentidad.ConFechaVen);
                            if (!obeCarneIdentidad.ConFechaVen.Equals("-") && obeCarneIdentidad.CarneNumero.Length > 0 && !obeCarneIdentidad.Renovar)
                            {
                                txtAprobarFechaVencimiento.Text = obeCarneIdentidad.ConFechaVen;
                            }
                            string controlCalidad = obeCarneIdentidad.ControlCalidad;
                            string controlCalidadDetalle = obeCarneIdentidad.ControlCalidadDetalle;
                            string[] listaControl = oCodigoUsuario.obtenerControlCalidad(controlCalidad);
                            string[] listaControlDetalle = oCodigoUsuario.obtenerControlCalidadDetalle(controlCalidadDetalle);
                            if (listaControl[13].Equals("0")) { txtAprobarFechaEmision.CssClass = "textboxObs"; txtAprobarFechaEmision.ToolTip = "DETALLE OBSERVACIÓN: " + (listaControlDetalle[13] != null ? listaControlDetalle[13].ToUpper() : ""); }
                            if (listaControl[8].Equals("0")) { txtAprobarFechaVencimiento.CssClass = "textboxObs"; txtAprobarFechaVencimiento.ToolTip = "DETALLE OBSERVACIÓN: " + (listaControlDetalle[8] != null ? listaControlDetalle[8].ToUpper() : ""); }
                            txtAprobarFechaVencimiento.CssClass = (listaControl[8].Equals("0") ? "textboxObs" : "");
                            divModal.Style.Add("display", "block");
                            divAprobar.Style.Add("display", "block");
                            break;
                        case "emitidoRegistro":
                            claveEval = false;
                            claveEstado = "EMITIDO (IMPRESO)";
                            claveRenovar = false;
                            obeCarneIdentidadDerivar = obeCarneIdentidad;
                            lblEmitirIdentificador.Text = obeCarneIdentidad.ConIdent;
                            lblEmitirFuncionario.Text = obeCarneIdentidad.ConFuncionario;
                            lblEmitirPais.Text = obeCarneIdentidad.ConPaisNacionalidad;
                            lblEmitirMision.Text = obeCarneIdentidad.ConOficinaConsularEx;
                            lblEmitirCalMig.Text = obeCarneIdentidad.ConCalidadMigratoria;
                            ddlTipoObs.SelectedValue = "0";
                            txtDetalleObs.Text = string.Empty;
                            txtAprobarFechaEmision.Text = string.Empty;
                            txtAprobarFechaVencimiento.Text = string.Empty;
                            divModal.Style.Add("display", "block");
                            divEmitidoImpr.Style.Add("display", "block");
                            break;
                        case "inhabilitar":
                            claveEval = false;
                            claveEstado = "INHABILITADO";
                            claveRenovar = false;
                            obsId = obtenerIdObs("INHABILITACIÓN DE REGISTRO");
                            obeCarneIdentidadDerivar = obeCarneIdentidad;
                            lblDevolverIdetn.Text = obeCarneIdentidad.ConIdent;
                            lblDevolverFuncionario.Text = obeCarneIdentidad.ConFuncionario;
                            lblDevolverPais.Text = obeCarneIdentidad.ConPaisNacionalidad;
                            lblDevolverMision.Text = obeCarneIdentidad.ConOficinaConsularEx;
                            lblDevolverCalMig.Text = obeCarneIdentidad.ConCalidadMigratoria;
                            ddlTipoObs.SelectedValue = obsId.ToString();
                            ddlTipoObs.Enabled = false;
                            txtDetalleObs.Text = string.Empty;
                            txtAprobarFechaEmision.Text = string.Empty;
                            txtAprobarFechaVencimiento.Text = string.Empty;
                            divModal.Style.Add("display", "block");
                            divDevolver.Style.Add("display", "block");
                            break;
                        case "habilitar":
                            claveEval = true;
                            claveEstado = "EMITIDO (IMPRESO)";
                            claveRenovar = false;
                            obsId = obtenerIdObs("HABILITACIÓN DE REGISTRO");
                            obeCarneIdentidadDerivar = obeCarneIdentidad;
                            lblDevolverIdetn.Text = obeCarneIdentidad.ConIdent;
                            lblDevolverFuncionario.Text = obeCarneIdentidad.ConFuncionario;
                            lblDevolverPais.Text = obeCarneIdentidad.ConPaisNacionalidad;
                            lblDevolverMision.Text = obeCarneIdentidad.ConOficinaConsularEx;
                            lblDevolverCalMig.Text = obeCarneIdentidad.ConCalidadMigratoria;
                            ddlTipoObs.SelectedValue = obsId.ToString();
                            ddlTipoObs.Enabled = false;
                            txtDetalleObs.Text = string.Empty;
                            txtAprobarFechaEmision.Text = string.Empty;
                            txtAprobarFechaVencimiento.Text = string.Empty;
                            divModal.Style.Add("display", "block");
                            divDevolver.Style.Add("display", "block");
                            break;
                        case "habilitarCorreccion":

                            break;
                        case "verFotografia":
                            string rutaArchivo = obrGeneral.rutaAdjuntos + obeCarneIdentidad.RutaArchivoFoto;
                            cargarImagenAlmacenada(rutaArchivo);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ibtVerFoto", "window.open('../VerImagenSave.aspx','_blank','width=600,height=750,toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1','_blank');", true);
                            break;
                        case "reimpresion":
                            claveEval = true;
                            claveEstado = "HABILITADO PARA REIMPRESIÓN";
                            claveRenovar = false;
                            //obsId = obtenerIdObs("HABILITACIÓN DE REGISTRO");
                            obeCarneIdentidadDerivar = obeCarneIdentidad;
                            lblIdentDupli.Text = obeCarneIdentidad.ConIdent;
                            lblFuncionarioDupli.Text = obeCarneIdentidad.ConFuncionario;
                            lblPaisDupli.Text = obeCarneIdentidad.ConPaisNacionalidad;
                            lblMisionDupli.Text = obeCarneIdentidad.ConOficinaConsularEx;
                            lblCalMigDupli.Text = obeCarneIdentidad.ConCalidadMigratoria;
                            ddlTipoDuplicado.SelectedValue = "0";
                            txtDetalleDuplicado.Text = string.Empty;
                            txtAprobarFechaEmision.Text = string.Empty;
                            txtAprobarFechaVencimiento.Text = string.Empty;
                            divModal.Style.Add("display", "block");
                            divReimpresion.Style.Add("display", "block");
                            break;
                        case "pdfInformacion":
                            byte[] pdfByte = oCodigoUsuario.crearPDF1(obeCarneIdentidad, ViewState["UserNomComp"].ToString()); //ms.ToArray();
                            Session["bytePDF"] = pdfByte;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirPDF", "window.open('../PDF.aspx', '_blank')", true);
                            break;
                        case "verMovimientos":
                            beMovimientoCarneIdentidad parametros = new beMovimientoCarneIdentidad();
                            parametros.CarneIdentidadid = obeCarneIdentidad.CarneIdentidadid;
                            brMovimientoCarneIdentidad obrMovimientoCarneIdentidad = new brMovimientoCarneIdentidad();
                            List<beMovimientoCarneIdentidad> lbeMovimientoCarneIdentidad = new List<beMovimientoCarneIdentidad>();
                            lbeMovimientoCarneIdentidad = obrMovimientoCarneIdentidad.consultarMovimientos(parametros);
                            if (lbeMovimientoCarneIdentidad != null)
                            {
                                gvMovimientos.DataSource = lbeMovimientoCarneIdentidad;
                                gvMovimientos.DataBind();
                                divMovimientos.Style.Add("display", "block");
                                divModal.Style.Add("display", "block");
                                controlId = "btnCerrarMovimientos";
                                ejecucionePostBack();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL PROCESAR LA INFORMACION');", true);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
            }
        }

        protected void actualizarEstado(object sender, EventArgs e) // ACTUALIZA EL ESTADO DEL REGISTRO
        {
            try
            {
                #region TKSEG
                string TKSEG = TKSEGENC.Value;
                bool exitoTKSEG = oCodigoUsuario.compareValTK(TKSEG, valueTKSEG);
                if (!exitoTKSEG)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensajeError", "location.href='../../mensajes.aspx';", true);
                    return;
                }
                #endregion
                bool exitoEvaluar = false;
                if (claveEstado.Equals("REGISTRADO") | claveEstado.Equals("INHABILITADO") | claveEval == true) { exitoEvaluar = evaluarControlesDevolver(); }
                if (claveEstado.Equals("APROBADO")) { exitoEvaluar = evaluarControlesAprobar(); }
                if (claveEstado.Equals("EMITIDO (IMPRESO)") & claveEval == false) { exitoEvaluar = true; }
                if (exitoEvaluar)
                {
                    short estadoId = obtenerIdEstado(claveEstado);
                    if (estadoId > 0)
                    {
                        bool exito = false;
                        short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
                        short oficinaId = short.Parse(ViewState["Oficinaid"].ToString());
                        string ipCreacion = ViewState["IP"].ToString();
                        beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
                        beConsultaExterna parametrosConsultaExterna = new beConsultaExterna();
                        parametrosConsultaExterna.CarneNumero = obeCarneIdentidadDerivar.CarneNumero;
                        // CARDIP
                        beCarneIdentidad parametrosDerivar = new beCarneIdentidad();
                        parametrosDerivar.CarneIdentidadid = obeCarneIdentidadDerivar.CarneIdentidadid;
                        if (!txtAprobarFechaEmision.Text.Equals("") & !txtAprobarFechaVencimiento.Text.Equals(""))
                        {
                            parametrosDerivar.FechaEmision = DateTime.Parse(txtAprobarFechaEmision.Text);
                            parametrosDerivar.FechaVencimiento = DateTime.Parse(txtAprobarFechaVencimiento.Text);
                        }
                        parametrosDerivar.Estadoid = estadoId;
                        parametrosDerivar.NuevoCarne = claveNuevo;
                        parametrosDerivar.Renovar = claveRenovar;
                        parametrosDerivar.FlagControlCalidad = obeCarneIdentidadDerivar.FlagControlCalidad;
                        obeCarneIdentidadPrincipal.CarneIdentidad = parametrosDerivar;
                        // HISTORICO CARNE IDENTIDAD
                        beCarneIdentidadHistorico obeCarneIdentidadHistorico = new beCarneIdentidadHistorico();
                        obeCarneIdentidadHistorico.CarneIdentidadid = obeCarneIdentidadDerivar.CarneIdentidadid;
                        obeCarneIdentidadHistorico.CalidadMigratoriaid = obeCarneIdentidadDerivar.CalidadMigratoriaid;
                        obeCarneIdentidadHistorico.CalidadMigratoriaSecId = obeCarneIdentidadDerivar.CalidadMigratoriaSecId;
                        if (!txtAprobarFechaEmision.Text.Equals("") & !txtAprobarFechaVencimiento.Text.Equals(""))
                        {
                            obeCarneIdentidadHistorico.FechaEmision = DateTime.Parse(txtAprobarFechaEmision.Text);
                            obeCarneIdentidadHistorico.FechaVencimiento = DateTime.Parse(txtAprobarFechaVencimiento.Text);
                        }
                        obeCarneIdentidadHistorico.RutaArchivoFoto = obeCarneIdentidadDerivar.RutaArchivoFoto;
                        obeCarneIdentidadHistorico.OficinaConsularExid = obeCarneIdentidadDerivar.OficinaConsularExid;
                        obeCarneIdentidadHistorico.EstadoId = estadoId;
                        obeCarneIdentidadHistorico.Usuariocreacion = usuarioId;
                        obeCarneIdentidadHistorico.Ipcreacion = ipCreacion;
                        obeCarneIdentidadHistorico.RutaArchivoFirma = obeCarneIdentidadDerivar.RutaArchivoFirma;
                        obeCarneIdentidadPrincipal.CarneIdentidadHistorico = obeCarneIdentidadHistorico;
                        // MOVIMIENTO CARNE IDENTIDAD - NUEVO
                        beMovimientoCarneIdentidad obeMovimientoCarneIdentidad = new beMovimientoCarneIdentidad();
                        obeMovimientoCarneIdentidad.CarneIdentidadid = obeCarneIdentidadDerivar.CarneIdentidadid;
                        obeMovimientoCarneIdentidad.Estadoid = estadoId;
                        obeMovimientoCarneIdentidad.Oficinaconsularid = oficinaId;
                        obeMovimientoCarneIdentidad.ObservacionDetalle = (obeCarneIdentidadDerivar.FlagControlCalidad ? "LEVANTAMIENTO DE OBSERVACIONES" : null);
                        if (!ddlTipoObs.SelectedValue.Equals("0"))
                        {
                            obeMovimientoCarneIdentidad.ObservacionTipo = short.Parse(ddlTipoObs.SelectedValue);
                            obeMovimientoCarneIdentidad.ObservacionDetalle = txtDetalleObs.Text.Trim().ToUpper();
                        }
                        obeMovimientoCarneIdentidad.Usuariocreacion = usuarioId;
                        obeMovimientoCarneIdentidad.Ipcreacion = ipCreacion;
                        obeCarneIdentidadPrincipal.MovimientoCarne = obeMovimientoCarneIdentidad;
                        if (claveEstado.Equals("EMITIDO (IMPRESO)") | claveEstado.Equals("INHABILITADO"))
                        {
                            if (claveEstado.Equals("EMITIDO (IMPRESO)"))
                            {
                                brCarneIdentidadPrincipal obrCarneIdentidadPrincipal = new brCarneIdentidadPrincipal();
                                exito = obrCarneIdentidadPrincipal.actualizarEstado(obeCarneIdentidadPrincipal);

                                // se desactiva por lo que en control de calidad se adiciona, en el boton conforminada.
                                /*################    adicionado para registro en linea  ################*/
                               /* Boolean resulta = obrCarneIdentidadPrincipal.registrarDetalleRegistroLineaAtendido( obeCarneIdentidadPrincipal.CarneIdentidad.CarneIdentidadid);
                                //===========================ENVIO DE CORREO AL SOLICITANTE==================
                                string strSMTPServer = "VICUS.RREE.GOB.PE";
                                string strSMTPPuerto = "25";
                                string strEmailFrom = "ALERTAS_SIGC@RREE.GOB.PE"; //ConfigurationManager.AppSettings["ConexionSGAC"];
                                string strEmailPassword = "";
                                beCarneIdentidad beReg = obrCarneIdentidadPrincipal.obtenerCorreoCiudadano(obeCarneIdentidadPrincipal);
                                string mensaje = obrCarneIdentidadPrincipal.obtenerMensajeEstado(obeCarneIdentidadPrincipal);
                                string correoCiudadano = beReg.Correo;
                                string numRegLinea = beReg.Numero_reg_linea;
                                string strEmailTo = "ingpipa@hotmail.com";
                                string strTitulo = "SOLICITUD DE CARNET - MINISTERIO DE RELACIONES EXTERIORES";
                                string htmlTexto = mensaje;
                                CodigoUsuario co = new CodigoUsuario();
                                bool bEnviado = co.EnviarCorreo(strSMTPServer, strSMTPPuerto, strEmailFrom, strEmailPassword, strEmailTo, strTitulo, System.Net.Mail.MailPriority.High, htmlTexto);
                               */
                            }
                            if (claveEstado.Equals("INHABILITADO"))
                            {
                                brCarneIdentidadPrincipal obrCarneIdentidadPrincipal = new brCarneIdentidadPrincipal();
                                exito = obrCarneIdentidadPrincipal.actualizarEstado(obeCarneIdentidadPrincipal);
                                //exito = obrCarneIdentidadPrincipal.inhabilitar(obeCarneIdentidadPrincipal, parametrosConsultaExterna);
                            }
                        }
                        else
                        {
                            brCarneIdentidadPrincipal obrCarneIdentidadPrincipal = new brCarneIdentidadPrincipal();
                            exito = obrCarneIdentidadPrincipal.actualizarEstado(obeCarneIdentidadPrincipal);
                        }

                        if (exito)
                        {
                            if (claveEstado.Equals("EMITIDO (IMPRESO)"))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE EMITIO EL REGISTRO CON EXITO');", true);
                            }
                            if (claveEstado.Equals("APROBADO"))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE APROBÓ EL REGISTRO');", true);
                            }
                            if (claveEstado.Equals("REGISTRADO"))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE DEVOLVIO EL REGISTRO');", true);
                            }
                            if (claveEstado.Equals("INHABILITADO"))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE INHABILITO EL REGISTRO');", true);
                            }

                            cambiarPagina(sender, e);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL PROCESAR LA INFORMACION');", true);
                        }
                    }
                }
                else
                {
                    Session["mensaje"] = "OCURRIO UN ERROR";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensajeError", "location.href='../../mensajes.aspx';", true);
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
            }
            cambiarPagina(sender, e);
        }
        #endregion
        #region Funciones
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

        protected void verTemplate(object sender, GridViewRowEventArgs e) // PERMITE MOSTRAR LAS HERRAMIENTAS EN LOS REGISTROS DE LA GRILLA DE ACUERDO A SU ESTADO O CONDICION PARTICULAR
        {
            try
            {
                if (e.Row.DataItemIndex > -1)
                {
                    List<beCarneIdentidad> lbeCarneIdentidad = new List<beCarneIdentidad>();
                    lbeCarneIdentidad = (List<beCarneIdentidad>)Session["ResultadoConsulta"];
                    //int pos = lbeCarneIdentidad.FindIndex(x => x.Renovar == true & x.ConIdent.Equals(e.Row.Cells[0].Text));
                    int pos = lbeCarneIdentidad.FindIndex(x => x.ConIdent.Equals(e.Row.Cells[0].Text));
                    if (pos != -1)
                    {
                        if (e.Row.Cells[3].Text.Equals("DERIVADO"))
                        {
                            e.Row.Cells[12].Controls[1].Visible = true;
                            e.Row.Cells[12].Controls[3].Visible = true;
                            e.Row.Cells[12].Controls[7].Visible = true;
                            if (lbeCarneIdentidad[pos].FlagControlCalidad)
                            {
                                oCodigoUsuario.colorCeldas("#A51212", "#FFFFFF", sender, e);
                            }
                            else
                            {
                                oCodigoUsuario.colorCeldas("#6D98E4", "#000000", sender, e);
                            }
                        }
                        //if (e.Row.Cells[3].Text.Equals("APROBADO"))
                        //{
                        //    e.Row.Cells[12].Controls[5].Visible = true;
                        //    oCodigoUsuario.colorCeldas("#6D98E4", "#000000", sender, e);
                        //}
                        if (e.Row.Cells[3].Text.Equals("EMITIDO (IMPRESO)"))
                        {
                            e.Row.Cells[12].Controls[9].Visible = true;
                            e.Row.Cells[12].Controls[13].Visible = true;
                            //e.Row.Cells[13].Controls[1].Visible = true;
                        }
                        if (e.Row.Cells[3].Text.Equals("INHABILITADO"))
                        {
                            e.Row.Cells[12].Controls[11].Visible = true;
                        }
                        if (lbeCarneIdentidad[pos].Renovar)
                        {
                            e.Row.Cells[3].Text = e.Row.Cells[3].Text + " (RENOVACIÓN)";
                        }
                        if (lbeCarneIdentidad[pos].Duplicado)
                        {
                            e.Row.Cells[3].Text = e.Row.Cells[3].Text + " (DUPLICADO)";
                            if (e.Row.Cells[3].Text.Equals("DERIVADO"))
                            {
                                oCodigoUsuario.colorCeldas("#6D98E4", "#000000", sender, e);
                            }
                        }
                        if (lbeCarneIdentidad[pos].FlagControlCalidad)
                        {
                            e.Row.Cells[3].Text = e.Row.Cells[3].Text + " (OBSERVADO)";
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

        protected beCarneIdentidad obtenerRegistro(object sender, EventArgs e) // PERMITE OBTENER TODA LA INFORMACION DE UN REGISTRO SELECCIONADO
        {
            beCarneIdentidad obeCarneIdentidad = new beCarneIdentidad();
            try
            {
                ImageButton imageButton = (ImageButton)sender;
                TableCell tableCell = (TableCell)imageButton.Parent;
                GridViewRow row = (GridViewRow)tableCell.Parent;
                gvCarne.SelectedIndex = row.RowIndex;
                int fila = row.RowIndex;

                List<beCarneIdentidad> lbeCarneIdentidad = (List<beCarneIdentidad>)Session["ResultadoConsulta"];
                if (lbeCarneIdentidad != null)
                {
                    int pos = lbeCarneIdentidad.FindIndex(x => x.ConIdent.Equals(row.Cells[0].Text));
                    if (pos != -1) { obeCarneIdentidad = lbeCarneIdentidad[pos]; }
                    else { obeCarneIdentidad = null; }
                }
                else { obeCarneIdentidad = null; }
            }
            catch (Exception ex)
            {
                obeCarneIdentidad = null;
                obrGeneral.grabarLog(ex);
            }
            return (obeCarneIdentidad);
        }

        protected void ejecucionePostBack() // PERMITE ENCAPSULAR TODAS LAS EJECUCIONES QUE DEBEN REALIZARSE AL HACER POSTBACK
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "focusControl", "focusControl('" + controlId + "');", true);
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

        protected short obtenerIdObs(string claveObs) // OBTIENE EL ID DE UNA LISTA DE ACUERDO A UNA CLAVE
        {
            int pos = ListaObservaciones.FindIndex(x => x.Descripcion.Equals(claveObs));
            short obsReg = (pos != -1 ? ListaObservaciones[pos].Parametroid : short.Parse("0"));
            return (obsReg);
        }

        protected void cargarImagenAlmacenada(string Path) // CARGA EN MEMORIA UNA IMAGEN ALMACENADA EN LOS SERVIDORES
        {
            try
            {
                if (File.Exists(Path))
                {
                    Byte[] imageByte = null;
                    imageByte = File.ReadAllBytes(Path);
                    string base64String = Convert.ToBase64String(imageByte, 0, imageByte.Length);
                    Session["tempImagenFotografiaSave"] = base64String;
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected bool evaluarControlesAprobar() // METODO DE SEGURIDAD PARA EVITAR LA INCLUSION DE CARACTERES NO VALIDOS
        {
            bool exitoEvaluar = false;
            // EVALUA ANTIXSS --------------------------------------------------
            // DROPDOWN
            string FechaEmi = oCodigoUsuario.evaluateAntiXSS(txtAprobarFechaEmision.Text);
            string FechaVen = oCodigoUsuario.evaluateAntiXSS(txtAprobarFechaVencimiento.Text);
            // EVALUA CARACTERES ------------------------------------------------
            // DROPDOWN
            exitoEvaluar = oCodigoUsuario.evaluarFecha(FechaEmi);
            if (exitoEvaluar) { exitoEvaluar = oCodigoUsuario.evaluarFecha(FechaVen); }

            return (exitoEvaluar);
        }

        protected bool evaluarControlesDevolver() // METODO DE SEGURIDAD PARA EVITAR LA INCLUSION DE CARACTERES NO VALIDOS
        {
            bool exitoEvaluar = false;
            // EVALUA ANTIXSS --------------------------------------------------
            // DROPDOWN
            string TipoObs = oCodigoUsuario.evaluateAntiXSS(ddlTipoObs.SelectedValue);
            string DetObs = oCodigoUsuario.evaluateAntiXSS(txtDetalleObs.Text.Trim().ToUpper());
            // EVALUA CARACTERES ------------------------------------------------
            // DROPDOWN
            exitoEvaluar = oCodigoUsuario.evaluarNumeros(TipoObs);
            if (exitoEvaluar) { exitoEvaluar = oCodigoUsuario.evaluarAlfaNum(DetObs); }

            return (exitoEvaluar);
        }
        #endregion
    }
}