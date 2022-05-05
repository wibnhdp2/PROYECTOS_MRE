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

namespace SolCARDIP.Paginas.Supervisor
{
    public partial class SupervisorConsultaControlCalidad : System.Web.UI.Page
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
                    divControlCalidad.Style.Add("display", "none");
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
        protected void buscarRegistros(object sender, EventArgs e)  // BUSCA REGISTROS DE ACUERDO A FILTROS
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
                        case "entrega":
                            claveEstado = "EMITIDO (IMPRESO)";
                            break;
                        case "controlCalidad":
                            claveEstado = "CONTROL DE CALIDAD";
                            obeCarneIdentidadDerivar = obeCarneIdentidad;
                            lblContCalIdentificador.Text = obeCarneIdentidad.ConIdent;
                            lblContCalNroCarne.Text = obeCarneIdentidad.CarneNumero;
                            divModal.Style.Add("display", "block");
                            divControlCalidad.Style.Add("display", "block");

                            string controlCalidad = obeCarneIdentidad.ControlCalidad;
                            string controlCalidadDetalle = obeCarneIdentidad.ControlCalidadDetalle;
                            string[] listaControl = oCodigoUsuario.obtenerControlCalidad(controlCalidad);
                            string[] listaControlDetalle = oCodigoUsuario.obtenerControlCalidadDetalle(controlCalidadDetalle);

                            if (listaControl[0].Equals("0")) { txtObs1.Text = (listaControlDetalle[0] != null ? listaControlDetalle[0].ToUpper() : ""); } else { txtObs1.Text = string.Empty; }
                            if (listaControl[1].Equals("0")) { txtObs2.Text = (listaControlDetalle[1] != null ? listaControlDetalle[1].ToUpper() : ""); } else { txtObs2.Text = string.Empty; }
                            if (listaControl[2].Equals("0")) { txtObs3.Text = (listaControlDetalle[2] != null ? listaControlDetalle[2].ToUpper() : ""); } else { txtObs3.Text = string.Empty; }
                            if (listaControl[3].Equals("0")) { txtObs4.Text = (listaControlDetalle[3] != null ? listaControlDetalle[3].ToUpper() : ""); } else { txtObs4.Text = string.Empty; }
                            if (listaControl[4].Equals("0")) { txtObs5.Text = (listaControlDetalle[4] != null ? listaControlDetalle[4].ToUpper() : ""); } else { txtObs5.Text = string.Empty; }
                            if (listaControl[5].Equals("0")) { txtObs6.Text = (listaControlDetalle[5] != null ? listaControlDetalle[5].ToUpper() : ""); } else { txtObs6.Text = string.Empty; }
                            if (listaControl[6].Equals("0")) { txtObs7.Text = (listaControlDetalle[6] != null ? listaControlDetalle[6].ToUpper() : ""); } else { txtObs7.Text = string.Empty; }
                            if (listaControl[7].Equals("0")) { txtObs8.Text = (listaControlDetalle[7] != null ? listaControlDetalle[7].ToUpper() : ""); } else { txtObs8.Text = string.Empty; }
                            if (listaControl[8].Equals("0")) { txtObs9.Text = (listaControlDetalle[8] != null ? listaControlDetalle[8].ToUpper() : ""); } else { txtObs9.Text = string.Empty; }
                            if (listaControl[9].Equals("0")) { txtObs10.Text = (listaControlDetalle[9] != null ? listaControlDetalle[9].ToUpper() : ""); } else { txtObs10.Text = string.Empty; }
                            if (listaControl[10].Equals("0")) { txtObs11.Text = (listaControlDetalle[10] != null ? listaControlDetalle[10].ToUpper() : ""); } else { txtObs11.Text = string.Empty; }
                            if (listaControl[11].Equals("0")) { txtObs12.Text = (listaControlDetalle[11] != null ? listaControlDetalle[11].ToUpper() : ""); } else { txtObs12.Text = string.Empty; }
                            if (listaControl[12].Equals("0")) { txtObs13.Text = (listaControlDetalle[12] != null ? listaControlDetalle[12].ToUpper() : ""); } else { txtObs13.Text = string.Empty; }
                            if (listaControl[13].Equals("0")) { txtObs14.Text = (listaControlDetalle[13] != null ? listaControlDetalle[13].ToUpper() : ""); } else { txtObs14.Text = string.Empty; }
                            if (listaControl[14].Equals("0")) { txtObs15.Text = (listaControlDetalle[14] != null ? listaControlDetalle[14].ToUpper() : ""); } else { txtObs15.Text = string.Empty; }
                            txtObs16.Text = (listaControlDetalle[15] != null ? listaControlDetalle[15].ToUpper() : txtObs15.Text = string.Empty);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "recuperarValores", "recuperarValores('" + controlCalidad + "');", true);
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

                Button controlButton = (Button)sender;
                string comando = controlButton.CommandName;
                if (comando.Equals("emitido")) { claveEstado = "EMITIDO (IMPRESO)"; }
                short estadoId = obtenerIdEstado(claveEstado);
                if (estadoId > 0)
                {
                    bool exito = false;
                    short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
                    short oficinaId = short.Parse(ViewState["Oficinaid"].ToString());
                    string ipCreacion = ViewState["IP"].ToString();
                    string controlCalidad = (claveEstado.Equals("EMITIDO (IMPRESO)") ? "1|1|1|1|1|1|1|1|1|1|1|1|1|1|1|1" : hdfldControlCalidad.Value);
                    string controlCalidadDetalle = (claveEstado.Equals("EMITIDO (IMPRESO)") ? null : hdfldTextoConCal.Value); ;
                    int pos = ListaObservaciones.FindIndex(x => x.Descripcion.Equals("OBSERVACIÓN EN CONTROL DE CALIDAD"));
                    short tipoObs = ListaObservaciones[pos].Parametroid;
                    beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
                    // CARDIP
                    beCarneIdentidad parametrosDerivar = new beCarneIdentidad();
                    parametrosDerivar.CarneIdentidadid = obeCarneIdentidadDerivar.CarneIdentidadid;
                    parametrosDerivar.Estadoid = estadoId;
                    parametrosDerivar.NuevoCarne = false;
                    parametrosDerivar.Renovar = obeCarneIdentidadDerivar.Renovar;
                    parametrosDerivar.FlagControlCalidad = (claveEstado.Equals("CONTROL DE CALIDAD") ? true : false);
                    parametrosDerivar.ControlCalidad = controlCalidad;
                    parametrosDerivar.ControlCalidadDetalle = controlCalidadDetalle;
                    obeCarneIdentidadPrincipal.CarneIdentidad = parametrosDerivar;
                    // MOVIMIENTO CARNE IDENTIDAD - NUEVO
                    beMovimientoCarneIdentidad obeMovimientoCarneIdentidad = new beMovimientoCarneIdentidad();
                    obeMovimientoCarneIdentidad.CarneIdentidadid = obeCarneIdentidadDerivar.CarneIdentidadid;
                    obeMovimientoCarneIdentidad.Estadoid = estadoId;
                    obeMovimientoCarneIdentidad.Oficinaconsularid = oficinaId;
                    obeMovimientoCarneIdentidad.Usuariocreacion = usuarioId;
                    obeMovimientoCarneIdentidad.Ipcreacion = ipCreacion;
                    if (claveEstado.Equals("CONTROL DE CALIDAD"))
                    {
                        obeMovimientoCarneIdentidad.ObservacionTipo = tipoObs;
                    }
                    //obeMovimientoCarneIdentidad.ObservacionDetalle = (!txtDetalleObs.Text.Equals("") ? txtDetalleObs.Text.Trim().ToUpper() : null);
                    obeCarneIdentidadPrincipal.MovimientoCarne = obeMovimientoCarneIdentidad;
                    obeCarneIdentidadPrincipal.CarneIdentidad.ConSolicitudId = obeCarneIdentidadDerivar.ConSolicitudId;
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
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE OBSERVO EL REGISTRO');", true);
                        }
                        if (claveEstado.Equals("EMITIDO (IMPRESO)"))
                        {
                            /*################    adicionado para registro de estado de atendeido en tabla detalle - registro en linea  ################*/
                            string mensaje = obrCarneIdentidadPrincipal.obtenerMensajeEstado(obeCarneIdentidadPrincipal);
                            Boolean resulta = obrCarneIdentidadPrincipal.registrarDetalleRegistroLineaAtendido(obeCarneIdentidadPrincipal.CarneIdentidad.CarneIdentidadid, mensaje);
                            /*===========================ENVIO DE CORREO AL SOLICITANTE==================*/
           

                            beCarneIdentidad beReg = obrCarneIdentidadPrincipal.obtenerCorreoCiudadano(obeCarneIdentidadPrincipal);
                            
                            if (beReg != null && mensaje!="")
                            {

                                DataTable _dtDatos = new DataTable();
                                _dtDatos = crearTabla("", ViewState["UserNomComp"].ToString(), mensaje, beReg.Numero_reg_linea);
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
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('Se registro correctamente');", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('Se registro correctamente, Ocurrio un error al enviar el correo al ciudadano');", true);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('Se registro correctamente, Ocurrio un error al enviar el correo al ciudadano(a)');", true); ;
                                    }

                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('Se registro correctamente, PERO NO SE HA PODIDO ENVIAR EL CORREO AL CIUDADANO SOBRE LA OBSERVACION, POR LO QUE NO SE HA ENCONTRADO EL CORREO REGISTRADO');", true);
                            }
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
        private DataTable crearTabla(string solicitante, string certificadorAtendio, string Observacion, string solicitud)
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

        protected void verTemplate(object sender, GridViewRowEventArgs e) // PERMITE MOSTRAR LAS HERRAMIENTAS EN LOS REGISTROS DE LA GRILLA DE ACUERDO A SU ESTADO O CONDICION PARTICULAR
        {
            try
            {
                if (e.Row.DataItemIndex > -1)
                {
                    short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
                    List<beCarneIdentidad> lbeCarneIdentidad = new List<beCarneIdentidad>();
                    lbeCarneIdentidad = (List<beCarneIdentidad>)Session["ResultadoConsulta"];
                    int pos = lbeCarneIdentidad.FindIndex(x => x.ConIdent.Equals(e.Row.Cells[0].Text));
                    if (pos != -1)
                    {
                        if (e.Row.Cells[3].Text.Equals("CONTROL DE CALIDAD"))
                        {
                            if (!lbeCarneIdentidad[pos].FlagControlCalidad)
                            {
                                e.Row.Cells[12].Controls[1].Visible = true;
                                oCodigoUsuario.colorCeldas("#6D98E4", "#000000", sender, e);
                            }
                            else
                            {
                                e.Row.Cells[3].Text = e.Row.Cells[3].Text + " (OBSERVADO)";
                            }
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
        #endregion
    }
}