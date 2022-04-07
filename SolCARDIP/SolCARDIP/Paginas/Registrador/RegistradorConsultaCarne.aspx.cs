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
// PDF --------------------------------------
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.xml;
using iTextSharp.text.html.simpleparser;
// ------------------------------------------
using System.Web.Routing;
using System.Collections.Specialized;
using System.Text;
using System.Data;

namespace SolCARDIP.Paginas.Registrador
{
    public partial class RegistradorConsultaCarne : System.Web.UI.Page
    {
        brGeneral obrGeneral = new brGeneral();
        CodigoUsuario oCodigoUsuario = new CodigoUsuario();
        UIEncriptador oUIEncriptador = new UIEncriptador();
        public static List<beEstado> ListaEstados = new List<beEstado>();
        public static List<beParametro> ListaObservaciones = new List<beParametro>();
        public static beCarneIdentidad obeCarneIdentidadDerivar = new beCarneIdentidad();
        string controlId = "";
        public static string claveEstado = "";
        public static bool claveNuevo = false;
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
                        String redirecturl = oCodigoUsuario.crearSTR(Request.Url.ToString(), currurl, objUsuarioBE.Alias, srtAl,0);
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
                    divDerivar.Style.Add("display", "none");
                    divDerivarControlCalidad.Style.Add("display", "none");
                    divObservar.Style.Add("display", "none");
                    obtenerNacionalidadPais(sender, e);
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
        private void buscarRegLinea()
        {
            try
            {
                
                string regLineaNumero = "";
                beRegistroLinea parametros = new beRegistroLinea();
                parametros.NumeroRegLinea = regLineaNumero;
                parametros.FechaCreacion_inicio = Convert.ToDateTime(txtFechaIni.Text);
                parametros.FechaCreacion_fin = Convert.ToDateTime(txtFechaFin.Text);
                brRegistroLinea obrRegistroLinea = new brRegistroLinea();
                DataTable dt = obrRegistroLinea.consultarRegistroDT(parametros);

                if (dt.Rows.Count>0)
                {
                        gdvSolicitudesLinea.DataSource = dt;
                        gdvSolicitudesLinea.DataBind();
                        lblTotalRegistros.Text = "Resultado de la busqueda: " + dt.Rows.Count.ToString() + " Registros.";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('NO SE ENCONTRÓ NINGUN REGISTRO EN LINEA');", true);
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
            }
        }
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
                gvCarne.DataSource = null;
                gvCarne.DataBind();
                if (chkSolOnline.Checked)
                {
                    if (txtFechaIni.Text.Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('Ingrese los datos de busqueda');", true);
                        return;
                    }
                    if (txtFechaFin.Text.Length == 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('Ingrese los datos de busqueda');", true);
                        return;
                    }
                    buscarRegLinea();
                    return;
                }


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
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
            }
        }

        protected void cambiarPagina(object sender, EventArgs e) // DROPDOWLIST DE PAGINACION DE LA GRILL
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
            catch(Exception ex)
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
                    switch (comando)
                    {
                        case "editarRegistro":
                            string carneIdEditar = oUIEncriptador.EncriptarCadena(obeCarneIdentidad.CarneIdentidadid.ToString());
                            string identificadorEditar = oUIEncriptador.EncriptarCadena(obeCarneIdentidad.ConIdent);
                            string pagEditar = oUIEncriptador.EncriptarCadena(ddlPaginas.SelectedValue);
                            string regPrevId = oUIEncriptador.EncriptarCadena(obeCarneIdentidad.ConRegistroPrevioId.ToString());
                            string regComp = oUIEncriptador.EncriptarCadena(obeCarneIdentidad.FlagRegistroCompleto.ToString());
                            Response.Redirect(String.Format("RegistradorCarneRegistro.aspx?carneId={0}&identificador={1}&pag={2}&regPrevId={3}&regComp={4}", carneIdEditar, identificadorEditar, pagEditar, regPrevId, regComp),false);
                            break;
                        case "derivarRegistro":
                            claveEstado = "DERIVADO";
                            obeCarneIdentidadDerivar = obeCarneIdentidad;
                            lblDerivarIdentificador.Text = obeCarneIdentidad.ConIdent;
                            lblDerivarFuncionario.Text = obeCarneIdentidad.ConFuncionario;
                            lblDerivarPais.Text = obeCarneIdentidad.ConPaisNacionalidad;
                            lblDerivarMision.Text = obeCarneIdentidad.ConOficinaConsularEx;
                            lblDerivarCalMig.Text = obeCarneIdentidad.ConCalidadMigratoria;
                            if (obeCarneIdentidadDerivar.ConEstado.Equals("VENCIDO") | obeCarneIdentidadDerivar.Renovar == true) { obeCarneIdentidadDerivar.Renovar = true; }
                            divModal.Style.Add("display", "block");
                            divDerivar.Style.Add("display", "block");
                            break;
                        case "renovarRegistro":
                            string carneIdRenovar = oUIEncriptador.EncriptarCadena(obeCarneIdentidad.CarneIdentidadid.ToString());
                            string identificadorRenovar = oUIEncriptador.EncriptarCadena(obeCarneIdentidad.ConIdent);
                            string pagRenovar = oUIEncriptador.EncriptarCadena(ddlPaginas.SelectedValue);
                            string renovar = oUIEncriptador.EncriptarCadena("1");
                            string regComp1 = oUIEncriptador.EncriptarCadena(obeCarneIdentidad.FlagRegistroCompleto.ToString());
                            string regPrevRenId = oUIEncriptador.EncriptarCadena(obeCarneIdentidad.ConRegistroPrevioId.ToString());
                            Response.Redirect(String.Format("RegistradorCarneRegistro.aspx?carneId={0}&identificador={1}&pag={2}&renovar={3}&regComp={4}&regPrevId={5}", carneIdRenovar, identificadorRenovar, pagRenovar, renovar, regComp1, regPrevRenId));
                            break;
                        case "controlCalidad":
                            claveEstado = "CONTROL DE CALIDAD";
                            obeCarneIdentidadDerivar = obeCarneIdentidad;
                            lblContCalIdentificador.Text = obeCarneIdentidad.ConIdent;
                            lblContCalFuncionario.Text = obeCarneIdentidad.ConFuncionario;
                            lblContCalPais.Text = obeCarneIdentidad.ConPaisNacionalidad;
                            lblContCalMision.Text = obeCarneIdentidad.ConOficinaConsularEx;
                            lblContCalCalMig.Text = obeCarneIdentidad.ConCalidadMigratoria;
                            divModal.Style.Add("display", "block");
                            divDerivarControlCalidad.Style.Add("display", "block");
                            break;
                        case "pdfInformacion":
                            byte[] pdfByte = oCodigoUsuario.crearPDF1(obeCarneIdentidad, ViewState["UserNomComp"].ToString()); //ms.ToArray();
                            Session["bytePDF"] = pdfByte;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirPDF", "window.open('../PDF.aspx', '_blank');", true);
                            break;
                        case "derivarRegistroCompleto":
                            claveEstado = "REGISTRADO";
                            obeCarneIdentidadDerivar = obeCarneIdentidad;
                            if (obeCarneIdentidadDerivar.ConEstado.Equals("VENCIDO") | obeCarneIdentidadDerivar.Renovar == true) { obeCarneIdentidadDerivar.Renovar = true; }
                            AutoDerivarse();
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
        protected void actualizarEstadoObservado(object sender, EventArgs e)
        {
            string claveEstado = "OBSERVADO";

            short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
            short oficinaId = short.Parse(ViewState["Oficinaid"].ToString());
            string ipCreacion = ViewState["IP"].ToString();
            beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
            // CARDIP
            short estadoId = obtenerIdEstado(claveEstado);
            beCarneIdentidad paramCarnet = new beCarneIdentidad();
            paramCarnet.ConSolicitudId = int.Parse(lblRegIdentificador.Value);
            paramCarnet.Estadoid = estadoId;
            paramCarnet.Usuariomodificacion = usuarioId;
            paramCarnet.Oservacion = txtMotivoObs.Text.ToUpper();
            obeCarneIdentidadPrincipal.CarneIdentidad = paramCarnet;

            brCarneIdentidadPrincipal obrCarneIdentidadPrincipal = new brCarneIdentidadPrincipal();
            Boolean exito = obrCarneIdentidadPrincipal.actualizarEstadoObservado(obeCarneIdentidadPrincipal);
            if (exito)
            {
                //cambiarPagina(sender, e);
                buscarRegistros(sender, e);
                divModal.Style.Add("display", "none");
                divObservar.Style.Add("display", "none");
                /*===========================ENVIO DE CORREO==================*/
           
                beCarneIdentidad beReg = obrCarneIdentidadPrincipal.obtenerCorreoCiudadano(obeCarneIdentidadPrincipal);
                if (beReg != null) {


                    DataTable _dtDatos = new DataTable();
                    _dtDatos = crearTabla("", ViewState["UserNomComp"].ToString(), txtMotivoObs.Text, beReg.Numero_reg_linea);
                    bool bEnvio = false;

                    if (beReg.Correo == "")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('No se ha registrado el correo del ciudadano(a)');", true);
                    }
                    else{
                        try
                        {
                            bEnvio = EnviarCorreo(_dtDatos, beReg.Correo, "ESTADO DE SOLICITUD NRO. " + beReg.Numero_reg_linea, "/Plantillas/CorreoSolicitudRechazada.html");
                            if (bEnvio)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('Se registro correctamente la observación');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('Se registro correctamente la observación, Ocurrio un error al enviar el correo al ciudadano');", true);
                            }
                        }
                        catch (Exception ex)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('Se registro correctamente la observación, Ocurrio un error al enviar el correo al ciudadano(a)');", true); ;
                        }
                    }                  
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('Se registro correctamente la observación, PERO NO SE HA PODIDO ENVIAR EL CORREO AL CIUDADANO SOBRE LA OBSERVACION, POR LO QUE NO SE HA ENCONTRADO EL CORREO REGISTRADO');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL PROCESAR LA INFORMACION');", true);
            }
        }

        //--------------------------------------------                    
        // Creador por: Jonatan Silva Cachay
        // Fecha: 02/02/2017
        // Objetivo: Envio de Correo
        //--------------------------------------------
        private bool EnviarCorreo(DataTable _dtReemplazo, string CorreoElectronico, string strASUNTO, string strPlantilla)
        {
            #region Envío Correo
            string strScript = string.Empty;

            string strSMTPServer = string.Empty;
            string strSMTPPuerto = string.Empty;
            string strEmailFrom = string.Empty;
            string strEmailPassword = string.Empty;
            string strEmailTo = string.Empty;
            string sMensaje = string.Empty;
            strSMTPServer = "VICUS.RREE.GOB.PE";
            strSMTPPuerto = "25";
            strEmailFrom = "ALERTAS_SIGC@RREE.GOB.PE"; //ConfigurationManager.AppSettings["ConexionSGAC"];
            strEmailPassword = "";

            strEmailTo = CorreoElectronico;
            string strTitulo = strASUNTO;

            // ENVIAR CORREO

            bool bEnviado = false;
     
            string strRutaCorreo = string.Empty;
            strRutaCorreo = Server.MapPath("~") + strPlantilla;
            try
            {
                CodigoUsuario co = new CodigoUsuario();
                bEnviado = co.EnviarCorreoPlantillaHTML(strRutaCorreo, _dtReemplazo, strSMTPServer, strSMTPPuerto,
                                               strEmailFrom, strEmailPassword,
                                               strEmailTo, strTitulo, System.Net.Mail.MailPriority.High, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            #endregion

            string strScriptCorreo = string.Empty;
            if (bEnviado)
            {
                sMensaje = "*** CORREO ENVIADO***" + "\\r\\n";
                sMensaje = sMensaje + "Se verificó la solicitud \\t" + "\\r\\n";
                sMensaje = sMensaje + "Se ha enviado un correo al solicitante con la información del estado de la solicitud \\t" + "\\r\\n";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('" + sMensaje + "');", true);
            }
            return bEnviado;
        }
        //--------------------------------------------                    
        // Creador por: Jonatan Silva Cachay
        // Fecha: 02/02/2017
        // Objetivo: Crea un datatable que se utiliza para reemplazar datos de la plantilla HTML del formato del correo
        //--------------------------------------------
        private DataTable crearTabla(string solicitante, string certificadorAtendio, string Observacion,string solicitud)
        {
            DataTable dtReemplazaCorreo = new DataTable();
            DataColumn dc1 = new DataColumn();
            DataColumn dc2 = new DataColumn();
            dc1.ColumnName = "valor";
            dc2.ColumnName = "reemplazo";
            dtReemplazaCorreo.Columns.Add(dc1);
            dtReemplazaCorreo.Columns.Add(dc2);


            DataRow dr = default(DataRow);
            dr = dtReemplazaCorreo.NewRow();
            dr[0] = "{SOLICITANTE}";
            dr[1] = solicitante;
            dtReemplazaCorreo.Rows.Add(dr);

            DataRow dr1 = default(DataRow);
            dr1 = dtReemplazaCorreo.NewRow();
            dr1[0] = "{MOTIVO}";
            dr1[1] = Observacion;
            dtReemplazaCorreo.Rows.Add(dr1);

            DataRow dr2 = default(DataRow);
            dr2 = dtReemplazaCorreo.NewRow();
            dr2[0] = "{ATENDIO}";
            dr2[1] = certificadorAtendio;
            dtReemplazaCorreo.Rows.Add(dr2);
           
            DataRow dr3 = default(DataRow);
            dr3 = dtReemplazaCorreo.NewRow();
            dr3[0] = "{fechaActual}";
            dr3[1] = DateTime.Now.ToShortDateString();
            dtReemplazaCorreo.Rows.Add(dr3);

            DataRow dr4 = default(DataRow);
            dr4 = dtReemplazaCorreo.NewRow();
            dr4[0] = "{SOLICITUD}";
            dr4[1] = solicitud;
            dtReemplazaCorreo.Rows.Add(dr4);
            return dtReemplazaCorreo;
        }


        private void AutoDerivarse() // ACTUALIZA EL ESTADO DEL REGISTRO
        {
            try
            {
                string NombreUsuario = ViewState["UserNomComp"].ToString();
                short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
                short oficinaId = short.Parse(ViewState["Oficinaid"].ToString());
                string ip = ViewState["IP"].ToString();
                short estado = obtenerIdEstado(claveEstado);
                short tipoObs = obtenerTipoObs("DERIVADO PARA REGISTRO COMPLETO");
                beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
                // CARNE DE IDENTIDAD
                beCarneIdentidad obeCarneIdentidad = new beCarneIdentidad();
                obeCarneIdentidad.CarneIdentidadid = obeCarneIdentidadDerivar.CarneIdentidadid;
                obeCarneIdentidad.Estadoid = estado;
                obeCarneIdentidad.UsuarioDeriva = usuarioId;
                obeCarneIdentidad.Usuariomodificacion = usuarioId;
                obeCarneIdentidad.Ipmodificacion = ip;
                obeCarneIdentidadPrincipal.CarneIdentidad = obeCarneIdentidad;
                // MOVIMIENTO CARNÉ
                beMovimientoCarneIdentidad obeMovimientoCarneIdentidad = new beMovimientoCarneIdentidad();
                obeMovimientoCarneIdentidad.CarneIdentidadid = obeCarneIdentidadDerivar.CarneIdentidadid;
                obeMovimientoCarneIdentidad.Estadoid = estado;
                obeMovimientoCarneIdentidad.Oficinaconsularid = oficinaId;
                obeMovimientoCarneIdentidad.ObservacionTipo = tipoObs;
                obeMovimientoCarneIdentidad.ObservacionDetalle = "DERIVADO A " + NombreUsuario + "PARA RENOVACIÓN";
                obeMovimientoCarneIdentidad.Usuariocreacion = usuarioId;
                obeMovimientoCarneIdentidad.Ipcreacion = ip;
                obeCarneIdentidadPrincipal.MovimientoCarne = obeMovimientoCarneIdentidad;
                brCarneIdentidadPrincipal obrCarneIdentidadPrincipal = new brCarneIdentidadPrincipal();
                bool exito = obrCarneIdentidadPrincipal.derivarRenovacion(obeCarneIdentidadPrincipal);
                if (exito)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE DERIVÓ EL REGISTRO CON EXITO');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL DERIVAR EL REGISTRO.');", true);
                }
            cambiarPagina(null, null);
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                cambiarPagina(null, null);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
            }
        }

        private void AutoDerivarsePopup(string NumSolicitud,String estadoObservacion) // ACTUALIZA EL ESTADO DEL REGISTRO
        {
            try
            {
                string NombreUsuario = ViewState["UserNomComp"].ToString();
                short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
                short oficinaId = short.Parse(ViewState["Oficinaid"].ToString());
                string ip = ViewState["IP"].ToString();
                short estado = obtenerIdEstado(claveEstado);
                short tipoObs = obtenerTipoObs("DERIVADO PARA REGISTRO COMPLETO");
                beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
                // CARNE DE IDENTIDAD
                beCarneIdentidad obeCarneIdentidad = new beCarneIdentidad();
                obeCarneIdentidad.CarneIdentidadid = obeCarneIdentidadDerivar.CarneIdentidadid;
                obeCarneIdentidad.Estadoid = estado;
                obeCarneIdentidad.UsuarioDeriva = usuarioId;
                obeCarneIdentidad.Usuariomodificacion = usuarioId;
                obeCarneIdentidad.Ipmodificacion = ip;
                obeCarneIdentidadPrincipal.CarneIdentidad = obeCarneIdentidad;
                // MOVIMIENTO CARNÉ
                beMovimientoCarneIdentidad obeMovimientoCarneIdentidad = new beMovimientoCarneIdentidad();
                obeMovimientoCarneIdentidad.CarneIdentidadid = obeCarneIdentidadDerivar.CarneIdentidadid;
                obeMovimientoCarneIdentidad.Estadoid = estado;
                obeMovimientoCarneIdentidad.Oficinaconsularid = oficinaId;
                obeMovimientoCarneIdentidad.ObservacionTipo = tipoObs;
                obeMovimientoCarneIdentidad.ObservacionDetalle = "DERIVADO A " + NombreUsuario + "PARA RENOVACIÓN";
                obeMovimientoCarneIdentidad.Usuariocreacion = usuarioId;
                obeMovimientoCarneIdentidad.Ipcreacion = ip;
                obeCarneIdentidadPrincipal.MovimientoCarne = obeMovimientoCarneIdentidad;
                brCarneIdentidadPrincipal obrCarneIdentidadPrincipal = new brCarneIdentidadPrincipal();
                bool exito = obrCarneIdentidadPrincipal.derivarRenovacion(obeCarneIdentidadPrincipal);
                if (exito)
                {
                    string carneIdEditar = oUIEncriptador.EncriptarCadena(obeCarneIdentidad.CarneIdentidadid.ToString());
                    string identificadorEditar = oUIEncriptador.EncriptarCadena("1");
                    string pagEditar = oUIEncriptador.EncriptarCadena("1");
                    string regPrevId = oUIEncriptador.EncriptarCadena(obeCarneIdentidad.ConRegistroPrevioId.ToString());
                    string regComp = oUIEncriptador.EncriptarCadena(obeCarneIdentidad.FlagRegistroCompleto.ToString());
                    Response.Redirect(String.Format("RegistradorCarneRegistro.aspx?carneId={0}&identificador={1}&pag={2}&regPrevId={3}&regComp={4}&Solicitud={5}&NumCarnet={6}&estado={7}", carneIdEditar, identificadorEditar, pagEditar, regPrevId, regComp,"RENOVACION", NumSolicitud, estadoObservacion), false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL DERIVAR EL REGISTRO.');", true);
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
            }
        }
        protected short obtenerTipoObs(string claveObs) // OBTIENE EL ID DE UNA LISTA DE ACUERDO A UNA CLAVE
        {
            int pos = ListaObservaciones.FindIndex(x => x.Descripcion.Equals(claveObs));
            short estadoObs = (pos != -1 ? ListaObservaciones[pos].Parametroid : short.Parse("0"));
            return (estadoObs);
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
                short estadoId = obtenerIdEstado(claveEstado);
                if (estadoId > 0)
                {
                    bool exito = false;
                    short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
                    short oficinaId = short.Parse(ViewState["Oficinaid"].ToString());
                    string ipCreacion = ViewState["IP"].ToString();
                    beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
                    // CARDIP
                    beCarneIdentidad parametrosDerivar = new beCarneIdentidad();
                    parametrosDerivar.CarneIdentidadid = obeCarneIdentidadDerivar.CarneIdentidadid;
                    parametrosDerivar.Estadoid = estadoId;
                    parametrosDerivar.NuevoCarne = false;
                    parametrosDerivar.Renovar = obeCarneIdentidadDerivar.Renovar;
                    parametrosDerivar.FlagControlCalidad = (claveEstado.Equals("CONTROL DE CALIDAD") ? false : obeCarneIdentidadDerivar.FlagControlCalidad);
                    parametrosDerivar.ControlCalidad = obeCarneIdentidadDerivar.ControlCalidad;
                    parametrosDerivar.ControlCalidadDetalle = obeCarneIdentidadDerivar.ControlCalidadDetalle;
                    obeCarneIdentidadPrincipal.CarneIdentidad = parametrosDerivar;
                    // MOVIMIENTO CARNE IDENTIDAD - NUEVO
                    beMovimientoCarneIdentidad obeMovimientoCarneIdentidad = new beMovimientoCarneIdentidad();
                    obeMovimientoCarneIdentidad.CarneIdentidadid = obeCarneIdentidadDerivar.CarneIdentidadid;
                    obeMovimientoCarneIdentidad.Estadoid = estadoId;
                    obeMovimientoCarneIdentidad.ObservacionDetalle = (obeCarneIdentidadDerivar.FlagControlCalidad ? "LEVANTAMIENTO DE OBSERVACIONES" : null);
                    obeMovimientoCarneIdentidad.Oficinaconsularid = oficinaId;
                    obeMovimientoCarneIdentidad.Usuariocreacion = usuarioId;
                    obeMovimientoCarneIdentidad.Ipcreacion = ipCreacion;
                    if (obeCarneIdentidadDerivar.Renovar == true)
                    {
                        int pos = ListaObservaciones.FindIndex(x => x.Descripcion.Equals("RENOVACION DE CARNÉ"));
                        short tipoObs = ListaObservaciones[pos].Parametroid;
                        obeMovimientoCarneIdentidad.ObservacionTipo = tipoObs;
                        obeMovimientoCarneIdentidad.ObservacionDetalle = "SOLICITA RENOVACIÓN";
                    }
                    obeCarneIdentidadPrincipal.MovimientoCarne = obeMovimientoCarneIdentidad;
                    brCarneIdentidadPrincipal obrCarneIdentidadPrincipal = new brCarneIdentidadPrincipal();
                    exito = obrCarneIdentidadPrincipal.actualizarEstado(obeCarneIdentidadPrincipal);
                    if (exito)
                    {
                        cambiarPagina(sender, e);
                        if (claveEstado.Equals("DERIVADO"))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE DERIVO EL REGISTRO AL ADMINISTRADOR');", true);
                        }
                        if (claveEstado.Equals("CONTROL DE CALIDAD"))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE DERIVO EL REGISTRO PARA CONTROL DE CALIDAD');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL PROCESAR LA INFORMACION');", true);
                    }
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
                    short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
                    List<beCarneIdentidad> lbeCarneIdentidad = new List<beCarneIdentidad>();
                    lbeCarneIdentidad = (List<beCarneIdentidad>)Session["ResultadoConsulta"];
                    //int pos = lbeCarneIdentidad.FindIndex(x => x.Renovar == true & x.ConIdent.Equals(e.Row.Cells[0].Text));
                    ImageButton ibtDuplicado = e.Row.FindControl("ibtDuplicado") as ImageButton;
                    ImageButton ibtDerivarRegCom = e.Row.FindControl("ibtDerivarRegCom") as ImageButton;
                    //ImageButton ibtObservar = e.Row.FindControl("ibtObservar") as ImageButton;
                    //ImageButton ibtSubsanar = e.Row.FindControl("ibtSubsanar") as ImageButton;
                    int pos = lbeCarneIdentidad.FindIndex(x => x.ConIdent.Equals(e.Row.Cells[0].Text));
                    if (pos != -1)
                    {
                        #region
                        //if (lbeCarneIdentidad[pos].UsuarioDeriva == usuarioId)
                        //{
                        //    if (e.Row.Cells[3].Text.Equals("DERIVADO PARA COMPLETAR"))
                        //    {
                        //        e.Row.Cells[12].Controls[1].Visible = true;
                        //        if (lbeCarneIdentidad[pos].FlagRegistroCompleto) { e.Row.Cells[12].Controls[3].Visible = true; }
                        //        oCodigoUsuario.colorCeldas("#6D98E4", "#000000", sender, e);
                        //    }
                        //    if (e.Row.Cells[3].Text.Equals("REGISTRADO"))
                        //    {
                        //        e.Row.Cells[12].Controls[1].Visible = true;
                        //        e.Row.Cells[12].Controls[3].Visible = true;
                        //        if (lbeCarneIdentidad[pos].FlagControlCalidad)
                        //        {
                        //            oCodigoUsuario.colorCeldas("#A51212", "#FFFFFF", sender, e);
                        //        }
                        //        else
                        //        {
                        //            oCodigoUsuario.colorCeldas("#6D98E4", "#000000", sender, e);
                        //        }
                        //    }
                        //    if (e.Row.Cells[3].Text.Equals("APROBADO"))
                        //    {
                        //        e.Row.Cells[12].Controls[7].Visible = true;
                        //        oCodigoUsuario.colorCeldas("#6D98E4", "#000000", sender, e);
                        //    }
                        //    if (e.Row.Cells[3].Text.Equals("VENCIDO"))
                        //    {
                        //        e.Row.Cells[12].Controls[3].Visible = true;
                        //        e.Row.Cells[12].Controls[5].Visible = true;
                        //    }
                        //    if (e.Row.Cells[3].Text.Equals("CONTROL DE CALIDAD"))
                        //    {
                        //        if (lbeCarneIdentidad[pos].FlagControlCalidad)
                        //        {
                        //            e.Row.Cells[12].Controls[1].Visible = true;
                        //            e.Row.Cells[12].Controls[3].Visible = true;
                        //            oCodigoUsuario.colorCeldas("#A51212", "#FFFFFF", sender, e);
                        //        }
                        //    }
                        //    if (lbeCarneIdentidad[pos].Renovar == true)
                        //    {
                        //        e.Row.Cells[3].Text = e.Row.Cells[3].Text + " (RENOVACIÓN)";
                        //        oCodigoUsuario.colorCeldas("#6D98E4", "#000000", sender, e);
                        //    }
                        //    if (lbeCarneIdentidad[pos].FlagControlCalidad)
                        //    {
                        //        e.Row.Cells[3].Text = e.Row.Cells[3].Text + " (OBSERVADO)";
                        //        oCodigoUsuario.colorCeldas("#A51212", "#FFFFFF", sender, e);
                        //    }
                        //}
                        #endregion
                        if (e.Row.Cells[3].Text.Equals("EMITIDO (IMPRESO)"))
                        {
                            ibtDuplicado.Visible = true;
                        }
                        if (e.Row.Cells[3].Text.Equals("REGISTRADO"))
                        {
                            //ibtObservar.Visible = true;
                            //ibtSubsanar.Visible = false;
                        }
                        if (e.Row.Cells[3].Text.Equals("OBSERVADO"))
                        {
                            //ibtObservar.Visible = false;
                            //ibtSubsanar.Visible = true;
                        }
                        


                        if (e.Row.Cells[3].Text.Equals("DERIVADO PARA COMPLETAR"))
                        {
                            e.Row.Cells[12].Controls[1].Visible = true;
                            if (lbeCarneIdentidad[pos].FlagRegistroCompleto) { e.Row.Cells[12].Controls[3].Visible = true; }
                            oCodigoUsuario.colorCeldas("#6D98E4", "#000000", sender, e);
                        }
                        if (e.Row.Cells[3].Text.Equals("REGISTRADO"))
                        {
                            e.Row.Cells[12].Controls[1].Visible = true;
                            e.Row.Cells[12].Controls[3].Visible = true;
                            if (lbeCarneIdentidad[pos].FlagControlCalidad)
                            {
                                oCodigoUsuario.colorCeldas("#A51212", "#FFFFFF", sender, e);
                            }
                            else
                            {
                                oCodigoUsuario.colorCeldas("#6D98E4", "#000000", sender, e);
                            }
                        }
                        if (e.Row.Cells[3].Text.Equals("APROBADO"))
                        {
                            e.Row.Cells[12].Controls[7].Visible = true;
                            oCodigoUsuario.colorCeldas("#6D98E4", "#000000", sender, e);
                        }
                        if (e.Row.Cells[3].Text.Equals("VENCIDO"))
                        {
                            if (lbeCarneIdentidad[pos].UsuarioDeriva == 0)
                            {
                                ibtDerivarRegCom.Visible = true;
                            }
                            if (lbeCarneIdentidad[pos].UsuarioDeriva == usuarioId)
                            {
                                e.Row.Cells[12].Controls[3].Visible = true;
                                e.Row.Cells[12].Controls[5].Visible = true;
                            }
                        }
                        if (e.Row.Cells[3].Text.Equals("CONTROL DE CALIDAD"))
                        {
                            if (lbeCarneIdentidad[pos].FlagControlCalidad)
                            {
                                e.Row.Cells[12].Controls[1].Visible = true;
                                e.Row.Cells[12].Controls[3].Visible = true;
                                oCodigoUsuario.colorCeldas("#A51212", "#FFFFFF", sender, e);
                            }
                        }
                        if (lbeCarneIdentidad[pos].Renovar == true)
                        {
                            e.Row.Cells[3].Text = e.Row.Cells[3].Text + " (RENOVACIÓN)";
                            oCodigoUsuario.colorCeldas("#6D98E4", "#000000", sender, e);
                        }
                        if (lbeCarneIdentidad[pos].Duplicado)
                        {
                            e.Row.Cells[3].Text = e.Row.Cells[3].Text + " (DUPLICADO)";
                            if (e.Row.Cells[3].Text.Equals("REGISTRADO"))
                            {
                                oCodigoUsuario.colorCeldas("#6D98E4", "#000000", sender, e);
                            }
                        }
                        if (lbeCarneIdentidad[pos].FlagControlCalidad)
                        {
                            e.Row.Cells[3].Text = e.Row.Cells[3].Text + " (OBSERVADO)";
                            oCodigoUsuario.colorCeldas("#A51212", "#FFFFFF", sender, e);
                        }
                    }
                }
            }
            catch(Exception ex)
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
            catch(Exception ex)
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
            catch(Exception ex)
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

        protected void AbrirPopup(object sender, EventArgs e) // SELECCION DE REGISTROS EN LAS DISTINTAS HERRAMIENTAS QUE OBEDECEN AL COMMANDNAME
        {
            try
            {
                Int16 _iEstado;
                _iEstado = Int16.Parse(((ImageButton)sender).Attributes["estadoId"]);
                Int64 _iCarnetCodigoID;
                _iCarnetCodigoID = Int64.Parse(((ImageButton)sender).Attributes["CarnetCodigoID"]);
                hEstadoId.Value = _iEstado.ToString();
                hCanetId.Value = _iCarnetCodigoID.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "AbrirPopup();", true);
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
            }
        }
        #endregion
        protected void btnGenerarDuplicadorPopup_Click(object sender, EventArgs e)
        {
            try
            {
                GenerarDuplicado(hNumSolLinea.Value);
                buscarRegistros(sender, e);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('Se Generó el duplicado de carnet - Se derivó al administrador');", true);
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
            }
        }
        protected void btnGenerarDuplicador_Click(object sender, EventArgs e)
        {
            try
            {
                GenerarDuplicado();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('Se Generó el duplicado de carnet - Se derivó al administrador');", true);
                cambiarPagina(sender, e);
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
            }
        }

        private void GenerarDuplicado(string SolicitudRegistroLinea = "")
        {
            try
            {
                beCarneIdentidad parametrosCarneIdentidad = new beCarneIdentidad();
                beRegistroLinea parametros = new beRegistroLinea();
                short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
                string ipCreacion = ViewState["IP"].ToString();
                short oficinaId = short.Parse(ViewState["Oficinaid"].ToString());
                if (SolicitudRegistroLinea == "")
                {
                    parametros.NumeroRegLinea = txtNroRegistroLinea.Text;
                }
                else {
                    parametros.NumeroRegLinea = SolicitudRegistroLinea;
                }
                
                parametrosCarneIdentidad.Estadoid = obtenerIdEstado("DERIVADO");
                if (SolicitudRegistroLinea == "")
                {
                    parametrosCarneIdentidad.CarneIdentidadid = Convert.ToInt16(hCanetId.Value);
                }
                else
                {
                    parametrosCarneIdentidad.CarneIdentidadid = Convert.ToInt16(hCanetIdPupop.Value);
                }
                parametrosCarneIdentidad.Ipmodificacion = ipCreacion;
                parametrosCarneIdentidad.Usuariomodificacion = usuarioId;

                beMovimientoCarneIdentidad obeMovimientoCarneIdentidad = new beMovimientoCarneIdentidad();
                if (SolicitudRegistroLinea == "")
                {
                    obeMovimientoCarneIdentidad.CarneIdentidadid = Convert.ToInt16(hCanetId.Value);
                }
                else
                {
                    obeMovimientoCarneIdentidad.CarneIdentidadid = Convert.ToInt16(hCanetIdPupop.Value);
                }
                
                obeMovimientoCarneIdentidad.Estadoid = obtenerIdEstado("DERIVADO");
                if (SolicitudRegistroLinea == "")
                {
                    obeMovimientoCarneIdentidad.ObservacionDetalle = "SE DERIVA PARA GENERAR DUPLICADO DE CARNET";
                }
                else {
                    obeMovimientoCarneIdentidad.ObservacionDetalle = "SE DERIVA PARA GENERAR DUPLICADO DE CARNET - BANDEJA SOLICITUD";
                }
                obeMovimientoCarneIdentidad.Oficinaconsularid = oficinaId;
                obeMovimientoCarneIdentidad.Usuariocreacion = usuarioId;
                obeMovimientoCarneIdentidad.Ipcreacion = ipCreacion;

                brCarneIdentidadPrincipal obj = new brCarneIdentidadPrincipal();
                obj.GenerarDuplicadoCarnet(parametrosCarneIdentidad, parametros, obeMovimientoCarneIdentidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void chkSolOnline_CheckedChanged(object sender, EventArgs e)
        {
            txtFechaIni.Text = "";
            txtFechaFin.Text = "";
            if (chkSolOnline.Checked)
            {
                DatosCarnet.Disabled = true;
                DatosFuncionario.Disabled = true;
                DatosMision.Disabled = true;
                btnFechaIni.Enabled = true;
                btnFechaFin.Enabled = true;
                txtFechaIni.Enabled = true;
                txtFechaFin.Enabled = true;
                cabeceraGrilla.Visible = false;
                gvCarne.DataSource = null;
                gvCarne.DataBind();
                gvCarne.Visible = false;
                divGridView.Visible = false;
                lblTotalRegistros.Text = "Resultado de la busqueda:";
            }
            else {
                DatosCarnet.Disabled = false;
                DatosFuncionario.Disabled = false;
                DatosMision.Disabled = false;
                btnFechaIni.Enabled = false;
                btnFechaFin.Enabled = false;
                txtFechaIni.Enabled = false;
                txtFechaFin.Enabled = false;
                cabeceraGrilla.Visible = true;
                gvCarne.Visible = true;
                divGridView.Visible = true;
                gdvSolicitudesLinea.DataSource = null;
                gdvSolicitudesLinea.DataBind();
                lblTotalRegistros.Text = "Resultado de la busqueda:";
            }
            DateTime primerDia = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            txtFechaIni.Text = primerDia.ToShortDateString();
            txtFechaFin.Text = DateTime.Now.ToShortDateString();
        }
        protected void descargarResumen(object sender, ImageClickEventArgs e)
        {
            try
            {
                 string _ruta;
                _ruta = ((ImageButton)sender).Attributes["resumen"].ToString();
                

                if (_ruta.Length > 0)
                {
                    if (File.Exists(_ruta))
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(_ruta);
                        Session["bytePDF"] = bytes;

                        string _open = "window.open('../PDF.aspx', '_newtab');";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                        return;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(string), "RESUMEN", "alert('No se encuentra el archivo');", false);
                    }
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }
        protected void Atender(object sender, ImageClickEventArgs e)
        {
            try
            {
                string _regLinea;
                _regLinea = ((ImageButton)sender).Attributes["numRegLinea"].ToString();
                string _CodregLinea;
                _CodregLinea = ((ImageButton)sender).Attributes["idRegLinea"].ToString();
                string tipEmision;
                tipEmision = ((ImageButton)sender).Attributes["tipEmision"].ToString();
                string numeroCarne;
                numeroCarne = ((ImageButton)sender).Attributes["numeroCarne"].ToString();
                string estado = ((ImageButton)sender).Attributes["estado"].ToString();

                if (tipEmision == "NUEVO")
                {
                    Response.Redirect(String.Format("RegistradorCarneRegistro.aspx?solicitudId={0}&NumCarnet={1}&Solicitud={2}&estado={3}", _CodregLinea, _regLinea, "NUEVO", estado), false);
                    return;
                }
                if (tipEmision == "RENOVACIÓN")
                {
                    beCarneIdentidad parametros = new beCarneIdentidad();
                    parametros.Periodo = 0;
                    parametros.IdentMesaPartes = "";
                    parametros.IdentNumero = 0;
                    parametros.CarneNumero = numeroCarne;
                    parametros.Estadoid = 0;
                    parametros.CalidadMigratoriaid = 0;
                    parametros.CalidadMigratoriaSecId = 0;
                    parametros.CatMision = 0;
                    parametros.OficinaConsularExid = 0;
                    parametros.FechaEmision = new DateTime(1900, 1, 1);
                    parametros.FechaVencimiento = new DateTime(1900, 1, 1);
                    parametros.ApePatPersona = "";
                    parametros.ApeMatPersona = "";
                    parametros.NombresPersona = "";
                    parametros.PaisPersona = 0;
                    parametros.NumPag = 1;
                    parametros.CantReg = long.Parse(obrGeneral.cantRegxPag);
                    brCarneIdentidadPrincipal obrCarneIdentidadPrincipal = new brCarneIdentidadPrincipal();
                    beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
                    obeCarneIdentidadPrincipal = obrCarneIdentidadPrincipal.consultar(parametros);
                    claveEstado = "REGISTRADO";
                    obeCarneIdentidadDerivar = obeCarneIdentidadPrincipal.ListaConsulta[0];
                    if (obeCarneIdentidadDerivar.ConEstado.Equals("VENCIDO") | obeCarneIdentidadDerivar.Renovar == true) 
                    { 
                        obeCarneIdentidadDerivar.Renovar = true; 
                    }
                    AutoDerivarsePopup(_regLinea, estado);
                    return;
                }
                if (tipEmision == "DUPLICADO")
                {
                    beCarneIdentidad parametros = new beCarneIdentidad();
                    parametros.Periodo = 0;
                    parametros.IdentMesaPartes = "";
                    parametros.IdentNumero = 0;
                    parametros.CarneNumero = numeroCarne;
                    parametros.Estadoid = 0;
                    parametros.CalidadMigratoriaid = 0;
                    parametros.CalidadMigratoriaSecId = 0;
                    parametros.CatMision = 0;
                    parametros.OficinaConsularExid = 0;
                    parametros.FechaEmision = new DateTime(1900, 1, 1);
                    parametros.FechaVencimiento = new DateTime(1900, 1, 1);
                    parametros.ApePatPersona = "";
                    parametros.ApeMatPersona = "";
                    parametros.NombresPersona = "";
                    parametros.PaisPersona = 0;
                    parametros.NumPag = 1;
                    parametros.CantReg = long.Parse(obrGeneral.cantRegxPag);
                    brCarneIdentidadPrincipal obrCarneIdentidadPrincipal = new brCarneIdentidadPrincipal();
                    beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
                    obeCarneIdentidadPrincipal = obrCarneIdentidadPrincipal.consultar(parametros);
                    //obeCarneIdentidadPrincipal.ListaConsulta;
                    lblNroCarne.Text = "NRO.CARNET: " + numeroCarne;
                    lblPersona.Text = "NOMBRE: " + obeCarneIdentidadPrincipal.ListaConsulta[0].NombresPersona + obeCarneIdentidadPrincipal.ListaConsulta[0].ApePatPersona + obeCarneIdentidadPrincipal.ListaConsulta[0].ApeMatPersona;
                    hEstadoIdPopup.Value = obeCarneIdentidadPrincipal.ListaConsulta[0].Estadoid.ToString();
                    hCanetIdPupop.Value = obeCarneIdentidadPrincipal.ListaConsulta[0].CarneIdentidadid.ToString();
                    lblEstado.Text = "ESTADO: " + obeCarneIdentidadPrincipal.ListaConsulta[0].ConEstado.ToString() ;
                    lblFechaRegistro.Text = "FECHA REGISTRO: " + Convert.ToDateTime(obeCarneIdentidadPrincipal.ListaConsulta[0].ConFechaInscripcion).ToShortDateString();
                    lblFechaEmision.Text = "FECHA EMISIÓN: " + Convert.ToDateTime(obeCarneIdentidadPrincipal.ListaConsulta[0].ConFechaEmision.ToString()).ToShortDateString();
                    lblFechaVencimiento.Text = "FECHA VENCIMIENTO: " + Convert.ToDateTime(obeCarneIdentidadPrincipal.ListaConsulta[0].ConFechaVen.ToString()).ToShortDateString();
                    hNumSolLinea.Value = _regLinea;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "AbrirPopupRenovacion();", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }
        protected void ObservarRegistro(object sender, ImageClickEventArgs e)
        {
            string regLinea  = ((ImageButton)sender).Attributes["numRegLinea"].ToString();
            string CodregLinea  = ((ImageButton)sender).Attributes["idRegLinea"].ToString(); 
            string tipEmision  = ((ImageButton)sender).Attributes["tipEmision"].ToString();
            string numeroCarne  = ((ImageButton)sender).Attributes["numeroCarne"].ToString();
            System.Diagnostics.Debug.Write("\n__________regLinea:" + regLinea+ "\n__CodregLinea:" + CodregLinea+ "\n__tipEmision:" + tipEmision + "\n___numeroCarne:" + numeroCarne);
            // case "observarRegistro":
            //claveEstado = "OBSERVADO";
            //obeCarneIdentidadDerivar = obeCarneIdentidad;
            lblRegIdentificador.Value = CodregLinea;// "" + obeCarneIdentidadDerivar.CarneIdentidadid;
            txtMotivoObs.Text = "";
            divModal.Style.Add("display", "block");
            divObservar.Style.Add("display", "block");
            
        }
        protected void SubsanarRegistro(object sender, ImageClickEventArgs e)
        {
            string _regLinea = ((ImageButton)sender).Attributes["numRegLinea"].ToString();
            string _CodregLinea = ((ImageButton)sender).Attributes["idRegLinea"].ToString();
            string tipEmision = ((ImageButton)sender).Attributes["tipEmision"].ToString();
            string numeroCarne = ((ImageButton)sender).Attributes["numeroCarne"].ToString();
           
            Response.Redirect(String.Format("RegistradorCarneRegistro.aspx?solicitudId={0}&NumCarnet={1}&Solicitud={2}&estado={3}", _CodregLinea, _regLinea, "NUEVO", "OBSERVADO"), false);
        }

        protected void gdvSolicitudesLinea_rowDataBound(object sender, GridViewRowEventArgs e) 
        {
            if (e.Row != null)
            {
                ImageButton imgAtender = e.Row.FindControl("imgAtender") as ImageButton;
                ImageButton imgObservar = e.Row.FindControl("imgObservar") as ImageButton;
                ImageButton imgSubsanar = e.Row.FindControl("imgSubsanar") as ImageButton;
                string estado;
                if (imgAtender == null)
                {
                    return;
                }
                estado = e.Row.Cells[2].Text.Replace("&nbsp;", "");

                if (estado == "ENVIADO")
                {
                    imgAtender.Visible = true;
                    imgObservar.Visible = true;
                }
                if (estado.Equals("OBSERVADO"))
                {
                    //ibtObservar.Visible = false;
                    //imgSubsanar.Visible = true;
                    imgAtender.Visible = true;
                }


            }
        }

    }
}