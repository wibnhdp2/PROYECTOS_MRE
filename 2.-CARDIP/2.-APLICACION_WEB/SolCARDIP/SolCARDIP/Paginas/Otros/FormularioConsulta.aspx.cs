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

namespace SolCARDIP.Paginas.Otros
{
    public partial class FormularioConsulta : System.Web.UI.Page
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
                        List<beCalidadMigratoria> listaFiltroCalMig = new List<beCalidadMigratoria>();
                        listaFiltroCalMig = obeGenerales.ListaCalidadMigratoriaNivelPrincipal.FindAll(x => x.Nombre.Equals("DIPLOMÁTICO") | x.Nombre.Equals("OFICIAL (C / PRIVILEGIOS)") | x.Nombre.Equals("OFICIAL (S / PRIVILEGIOS)"));
                        listaFiltroCalMig.Insert(0, new beCalidadMigratoria { CalidadMigratoriaid = 0, Nombre = "<Seleccione>" });
                        ddlCalidadMigratoriaPri.DataSource = listaFiltroCalMig;
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
                        List<beEstado> listaFiltroEstado = new List<beEstado>();
                        listaFiltroEstado = obeGenerales.ListaEstados.FindAll(x => x.DescripcionCorta.Equals("EMITIDO (IMPRESO)") | x.DescripcionCorta.Equals("VENCIDO"));
                        listaFiltroEstado.Insert(0, new beEstado { Estadoid = 0, DescripcionCorta = "<Seleccione>" });
                        ddlEstado.DataSource = listaFiltroEstado;
                        ddlEstado.DataValueField = "Estadoid";
                        ddlEstado.DataTextField = "DescripcionCorta";
                        ddlEstado.DataBind();
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
                    valueTKSEG = oCodigoUsuario.generateValTK(Path.GetFileNameWithoutExtension(Request.Url.LocalPath));
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
                    divModal2.Style.Add("display", "none");
                    obtenerNacionalidadPais(sender, e);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "preloader", "preloader();", true);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR AL CARGAR LA PÁGINA');", true);
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
                obeCarneIdentidadPrincipal = obrCarneIdentidadPrincipal.consultarPrivilegios(parametros);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
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
                gvCarne.DataSource = (obeCarneIdentidadPrincipal.ListaConsulta != null ? obeCarneIdentidadPrincipal.ListaConsulta : null);
                gvCarne.DataBind();
                Session["ResultadoConsulta"] = obeCarneIdentidadPrincipal.ListaConsulta;
                Session["ParametrosConsulta"] = parametros;
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
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
                        case "pdfInformacion":
                            byte[] pdfByte = oCodigoUsuario.crearPDF1(obeCarneIdentidad, ViewState["UserNomComp"].ToString()); //ms.ToArray();
                            Session["bytePDF"] = pdfByte;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirPDF", "window.open('../PDF.aspx', '_blank')", true);
                            break;
                        //case "verMovimientos":
                        //    beMovimientoCarneIdentidad parametros = new beMovimientoCarneIdentidad();
                        //    parametros.CarneIdentidadid = obeCarneIdentidad.CarneIdentidadid;
                        //    brMovimientoCarneIdentidad obrMovimientoCarneIdentidad = new brMovimientoCarneIdentidad();
                        //    List<beMovimientoCarneIdentidad> lbeMovimientoCarneIdentidad = new List<beMovimientoCarneIdentidad>();
                        //    lbeMovimientoCarneIdentidad = obrMovimientoCarneIdentidad.consultarMovimientos(parametros);
                        //    if (lbeMovimientoCarneIdentidad != null)
                        //    {
                        //        gvMovimientos.DataSource = lbeMovimientoCarneIdentidad;
                        //        gvMovimientos.DataBind();
                        //        divMovimientos.Style.Add("display", "block");
                        //        divModal.Style.Add("display", "block");
                        //        controlId = "btnCerrarMovimientos";
                        //        ejecucionePostBack();
                        //    }
                        //    else
                        //    {
                        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR AL PROCESAR LA INFORMACIÓN');", true);
                        //    }
                        //    break;
                    }
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
            }
        }
        #endregion
        #region Funciones
        protected void paginacion(long totalPag, long residuo)// TRABAJA EN CONJUNTO CON EL CONTROL cambiarPagina()
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

        protected beCarneIdentidad obtenerRegistro(object sender, EventArgs e) // PERMITE OBTENER TODA LA INFORMACIÓN DE UN REGISTRO SELECCIONADO
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
        #endregion

    }
}
