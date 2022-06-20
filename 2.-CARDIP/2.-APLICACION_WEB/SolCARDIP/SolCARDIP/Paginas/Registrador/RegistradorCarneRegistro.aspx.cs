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

namespace SolCARDIP.Paginas.Registrador
{
    public partial class RegistradorCarneRegistro : System.Web.UI.Page
    {
        brGeneral obrGeneral = new brGeneral();
        CodigoUsuario oCodigoUsuario = new CodigoUsuario();
        UIEncriptador oUIEncriptador = new UIEncriptador();
        public static List<beEstado> ListaEstados = new List<beEstado>();
        public static List<beParametro> ListaTipoObs = new List<beParametro>();
        public static string valueTKSEG;
        public static string srtAl = "";
        public static beCarneIdentidad titularRelDep = new beCarneIdentidad();
        public static beRegistroLinea obeRegistroLinea = new beRegistroLinea();
        public static beCarneIdentidadRelacionDependencia RelacionDependenciaGen;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    #region Sesiones
                    Session["tempImagenFotografiaSave"] = null;
                    Session["tempImagenFotografiaNueva"] = null;
                    Session["tempImagenFirmaSave"] = null;
                    Session["tempImagenFirmaNueva"] = null;
                    #endregion
                    #region Session Principal
                    csUsuarioBE objUsuarioBE = new csUsuarioBE();
                    objUsuarioBE = (csUsuarioBE)Session["usuario"];
                    ViewState["Usuarioid"] = objUsuarioBE.UsuarioId;
                    ViewState["Oficinaid"] = objUsuarioBE.codOficina;
                    ViewState["IP"] = oCodigoUsuario.obtenerIP();
                    #endregion
                    #region Generales
                    //GENERALES
                    if (Session["Generales"] != null)
                    {
                        beGenerales obeGenerales = new beGenerales();
                        obeGenerales = (beGenerales)Session["Generales"];                        
                        // GENERO
                        obeGenerales.ListaParametroGenero.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                        ddlSexo.DataSource = obeGenerales.ListaParametroGenero;
                        ddlSexo.DataValueField = "Parametroid";
                        ddlSexo.DataTextField = "Descripcion";
                        ddlSexo.DataBind();
                        // DOCUMENTO IDENTIDAD
                        obeGenerales.ListaDocumentoIdentidad.Insert(0, new beDocumentoIdentidad { Tipodocumentoidentidadid = 0, DescripcionCorta = "<Seleccione>" });
                        ddlDocumentoIdent.DataSource = obeGenerales.ListaDocumentoIdentidad;
                        ddlDocumentoIdent.DataValueField = "Tipodocumentoidentidadid";
                        ddlDocumentoIdent.DataTextField = "DescripcionCorta";
                        ddlDocumentoIdent.DataBind();
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
                        cargarComboNull(ddlEstadoCivil);
                        // ESTADOS CARNE
                        //ListaEstados = new List<beEstado>();
                        ListaEstados = obeGenerales.ListaEstados;
                        ListaTipoObs = obeGenerales.TipoObservacion;
                    }
                    #endregion
                    #region UbiGeo
                    // UBIGEO
                    beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                    brUbicaciongeografica obrUbicaciongeografica = new brUbicaciongeografica();
                    beUbicaciongeografica parametros = new beUbicaciongeografica();
                    parametros.Siglapais = "PER";
                    obeUbigeoListas = obrUbicaciongeografica.obtenerGenerales(parametros);
                    if (obeUbigeoListas != null)
                    {
                        Session["Ubigeo"] = obeUbigeoListas;
                        if (obeUbigeoListas.Ubigeo01.Count > 0)
                        {
                            obeUbigeoListas.Ubigeo01.Insert(0, new beUbicaciongeografica { Ubi01 = "00", Departamento = "<Seleccione>" });
                            ddlDepartamento.DataSource = obeUbigeoListas.Ubigeo01;
                            ddlDepartamento.DataValueField = "Ubi01";
                            ddlDepartamento.DataTextField = "Departamento";
                            ddlDepartamento.DataBind();
                        }
                        if (obeUbigeoListas.Ubigeo02.Count > 0)
                        {
                            obeUbigeoListas.Ubigeo02.Insert(0, new beUbicaciongeografica { Ubi02 = "00", Ubi01 = "00", Provincia = "<Seleccione>" });
                            cargarComboNull(ddlProvincia);
                        }
                        if (obeUbigeoListas.Ubigeo03.Count > 0)
                        {
                            obeUbigeoListas.Ubigeo03.Insert(0, new beUbicaciongeografica { Ubi03 = "00", Ubi01 = "00", Ubi02 = "00", Distrito = "<Seleccione>" });
                            cargarComboNull(ddlDistrito);
                        }
                    }
                    #endregion

                    
                    #region Carga de Datos en Edicion
                    if (Request.QueryString["carneId"] != null)
                    {
                        if (Request.QueryString["renovar"] != null) 
                        { 
                            ViewState["renovar"] = Sanitizer.GetSafeHtmlFragment(oUIEncriptador.DesEncriptarCadena(Request.QueryString["renovar"].ToString().Replace(" ", "+")));
                            lblInfoRenovar.Text = "EDICIÓN PARA RENOVACIÓN (Todos los cambio que realice sobre el registro se reflejaran en la impresion del nuevo Carné)";

                        }
                        ViewState["carneId"] = Sanitizer.GetSafeHtmlFragment(oUIEncriptador.DesEncriptarCadena(Request.QueryString["carneId"].ToString().Replace(" ", "+")));
                        ViewState["identificador"] = Sanitizer.GetSafeHtmlFragment(oUIEncriptador.DesEncriptarCadena(Request.QueryString["identificador"].ToString().Replace(" ", "+")));
                        ViewState["pag"] = Sanitizer.GetSafeHtmlFragment(oUIEncriptador.DesEncriptarCadena(Request.QueryString["pag"].ToString().Replace(" ", "+")));
                        ViewState["regPrevId"] = (Request.QueryString["regPrevId"] != null ? Sanitizer.GetSafeHtmlFragment(oUIEncriptador.DesEncriptarCadena(Request.QueryString["regPrevId"].ToString().Replace(" ", "+"))) : "0");
                        ViewState["regComp"] = (Request.QueryString["regComp"] != null ? Sanitizer.GetSafeHtmlFragment(oUIEncriptador.DesEncriptarCadena(Request.QueryString["regComp"].ToString().Replace(" ", "+"))) : "0");

                        brCarneIdentidadPrincipal obrCarneIdentidadPrincipal = new brCarneIdentidadPrincipal();
                        beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
                        beCarneIdentidadPrincipal parametrosPrincipal = new beCarneIdentidadPrincipal();
                        // CARNÉ DE IDENTIDAD
                        beCarneIdentidad parametrosCarneIdentidad = new beCarneIdentidad();
                        parametrosCarneIdentidad.CarneIdentidadid = short.Parse(ViewState["carneId"].ToString());
                        parametrosPrincipal.CarneIdentidad = parametrosCarneIdentidad;
                        // REGISTRO PREVIO
                        beRegistroPrevio parametrosRegistroPrevio = new beRegistroPrevio();
                        parametrosRegistroPrevio.RegistroPrevioId = short.Parse(ViewState["regPrevId"].ToString());
                        parametrosPrincipal.RegistroPrevio = parametrosRegistroPrevio;

                        obeCarneIdentidadPrincipal = obrCarneIdentidadPrincipal.consultarRegistroEdicion(parametrosPrincipal);
                        if (obeCarneIdentidadPrincipal != null)
                        {
                            if (obeCarneIdentidadPrincipal.CarneIdentidad != null)
                            {
                                Session["Temp1"] = obeCarneIdentidadPrincipal;
                                if (obeCarneIdentidadPrincipal.CarneIdentidad.Renovar)
                                {
                                    ViewState["renovar"] = "1";
                                    lblInfoRenovar.Text = "EDICIÓN PARA RENOVACIÓN (Todos los cambio que realice sobre el registro se reflejarán en la impresión del nuevo Carné)";
                                    /*============en caso de que el registro sea observado=========*/
                                    string estado = (Request.QueryString["estado"] != null ? Request.QueryString["estado"].ToString() : "");
                                    ViewState["estado"] = estado;
                                    if (estado.Equals("OBSERVADO"))
                                    {
                                        lblInfoObserbacion.Text = "REGISTRO OBSERVADO: PARA CONTINUAR CON LA ANTENCIÓN, DEBE SUBSANAR LA OBSERVACIÓN";
                                        //btnSubsanar.Visible = true;
                                        btnGuardar.Text = "Subsanar y Registrar Carnet";
                                        btnGuardar.Style.Remove("width");
                                        btnGuardar.Style.Add("width", "220px");
                                        //btnGuardar.Enabled = false;
                                        //btnGuardarEdicion.Enabled = false;
                                    }
                                    /*==============*/
                                }
                                if (obeCarneIdentidadPrincipal.CarneIdentidad.FlagControlCalidad) 
                                {
                                    lblInfoRenovar.Text = "OBSERVADO EN CONTROL DE CALIDAD (Los controles resaltados reflejan el dato observado)";
                                    string controlCalidad = obeCarneIdentidadPrincipal.CarneIdentidad.ControlCalidad;
                                    string controlCalidadDetalle = obeCarneIdentidadPrincipal.CarneIdentidad.ControlCalidadDetalle;
                                    string[] listaControl = oCodigoUsuario.obtenerControlCalidad(controlCalidad);
                                    string[] listaControlDetalle = oCodigoUsuario.obtenerControlCalidadDetalle(controlCalidadDetalle);
                                    if (listaControl[0].Equals("0")) { ddlCalidadMigratoriaPri.CssClass = "dropdownlistObs"; ddlCalidadMigratoriaPri.ToolTip = "DETALLE OBSERVACIÓN: " + (listaControlDetalle[0] != null ? listaControlDetalle[0].ToUpper() : ""); }
                                    if (listaControl[1].Equals("0")) { txtApePat.CssClass = "textboxObs"; txtApePat.ToolTip = "DETALLE OBSERVACIÓN: " + (listaControlDetalle[1] != null ? listaControlDetalle[1].ToUpper() : ""); }
                                    if (listaControl[2].Equals("0")) { txtApeMat.CssClass = "textboxObs"; txtApeMat.ToolTip = "DETALLE OBSERVACIÓN: " + (listaControlDetalle[2] != null ? listaControlDetalle[2].ToUpper() : ""); }
                                    if (listaControl[3].Equals("0")) { txtNombres.CssClass = "textboxObs"; txtNombres.ToolTip = "DETALLE OBSERVACIÓN: " + (listaControlDetalle[3] != null ? listaControlDetalle[3].ToUpper() : ""); }
                                    if (listaControl[4].Equals("0")) { ddlNacionalidad.CssClass = "dropdownlistObs"; ddlNacionalidad.ToolTip = "DETALLE OBSERVACIÓN: " + (listaControlDetalle[4] != null ? listaControlDetalle[4].ToUpper() : ""); }
                                    if (listaControl[5].Equals("0")) { txtFechaNac.CssClass = "textboxObs"; txtFechaNac.ToolTip = "DETALLE OBSERVACIÓN: " + (listaControlDetalle[5] != null ? listaControlDetalle[5].ToUpper() : ""); }
                                    if (listaControl[6].Equals("0")) { ddlEstadoCivil.CssClass = "dropdownlistObs"; ddlEstadoCivil.ToolTip = "DETALLE OBSERVACIÓN: " + (listaControlDetalle[6] != null ? listaControlDetalle[6].ToUpper() : ""); }
                                    if (listaControl[7].Equals("0")) { ddlSexo.CssClass = "dropdownlistObs"; ddlSexo.ToolTip = "DETALLE OBSERVACIÓN: " + (listaControlDetalle[7] != null ? listaControlDetalle[7].ToUpper() : ""); }
                                    if (listaControl[8].Equals("0")) { txtFechaVencimiento.CssClass = "textboxObs"; txtFechaVencimiento.ToolTip = "DETALLE OBSERVACIÓN: " + (listaControlDetalle[8] != null ? listaControlDetalle[8].ToUpper() : ""); }
                                    if (listaControl[9].Equals("0")) { lblObsFirma.Text = "DETALLE OBSERVACIÓN: " + (listaControlDetalle[9] != null ? listaControlDetalle[9].ToUpper() : ""); }
                                    if (listaControl[10].Equals("0")) { ddlMision.CssClass = "dropdownlistObs"; ddlMision.ToolTip = "DETALLE OBSERVACIÓN: " + (listaControlDetalle[10] != null ? listaControlDetalle[10].ToUpper() : ""); }
                                    if (listaControl[11].Equals("0")) { ddlCalidadMigratoriaSec.CssClass = "dropdownlistObs"; ddlCalidadMigratoriaSec.ToolTip = "DETALLE OBSERVACIÓN: " + (listaControlDetalle[11] != null ? listaControlDetalle[11].ToUpper() : ""); }
                                    if (listaControl[12].Equals("0")) { txtDomicilio.CssClass = "textboxObs"; txtDomicilio.ToolTip = "DETALLE OBSERVACIÓN: " + (listaControlDetalle[12] != null ? listaControlDetalle[12].ToUpper() : ""); }
                                    if (listaControl[13].Equals("0")) { txtFechaEmision.CssClass = "textboxObs"; txtFechaEmision.ToolTip = "DETALLE OBSERVACIÓN: " + (listaControlDetalle[13] != null ? listaControlDetalle[13].ToUpper() : ""); }
                                    if (listaControl[14].Equals("0")) { lblObsFoto.Text = "DETALLE OBSERVACIÓN: " + (listaControlDetalle[14] != null ? listaControlDetalle[14].ToUpper() : ""); }
                                    lblObsImpresion.Text = (listaControlDetalle[15] != null ? "IMPRESION OBSERVADA: " + listaControlDetalle[15].ToUpper() : "");
                                }
                                txtMesaPartes.Text = obeCarneIdentidadPrincipal.CarneIdentidad.IdentMesaPartes;
                                // PERSONA --------------------------------------------------------------------------------------------
                                chkEsMenor.Checked = obeCarneIdentidadPrincipal.Persona.MenorEdad;
                                txtApePat.Text = obeCarneIdentidadPrincipal.Persona.Apellidopaterno;
                                txtApeMat.Text = obeCarneIdentidadPrincipal.Persona.Apellidomaterno;
                                txtNombres.Text = obeCarneIdentidadPrincipal.Persona.Nombres;
                                txtFechaNac.Text = obeCarneIdentidadPrincipal.Persona.Nacimientofecha.ToShortDateString();
                                ddlEstadoCivil.SelectedValue = obeCarneIdentidadPrincipal.Persona.Estadocivilid.ToString();
                                ddlSexo.SelectedValue = obeCarneIdentidadPrincipal.Persona.Generoid.ToString();
                                seleccionarSexo(sender, e);
                                ddlDocumentoIdent.SelectedValue = obeCarneIdentidadPrincipal.PersonaIdentificacion.Documentotipoid.ToString();
                                txtNumeroDocIdent.Text = obeCarneIdentidadPrincipal.PersonaIdentificacion.Documentonumero;
                                ddlNacionalidad.SelectedValue = obeCarneIdentidadPrincipal.Pais.Paisid.ToString();
                                lblNacionalidad.Text = obeCarneIdentidadPrincipal.Pais.Nacionalidad;
                                txtTelefono.Text = obeCarneIdentidadPrincipal.Persona.Telefono;
                                txtDomicilio.Text = obeCarneIdentidadPrincipal.Residencia.Residenciadireccion;
                                // UBIGEO ----------------------------------------------------------------------------------------------
                                ddlDepartamento.SelectedValue = obeCarneIdentidadPrincipal.UbiGeo.Ubi01;
                                seleccionarDepartamento(sender, e);
                                ddlProvincia.SelectedValue = obeCarneIdentidadPrincipal.UbiGeo.Ubi02;
                                seleccionarProvincia(sender, e);
                                ddlDistrito.SelectedValue = obeCarneIdentidadPrincipal.UbiGeo.Ubi03;
                                // CALIDAD MIGRATORIA --------------------------------------------------------------------------------
                                ddlCalidadMigratoriaPri.SelectedValue = obeCarneIdentidadPrincipal.CalidadMigratoriaPri.CalidadMigratoriaid.ToString();
                                ddlTitDep.SelectedValue = obeCarneIdentidadPrincipal.CalidadMigratoriaSec.FlagTitularDependiente.ToString();
                                seleccionarCalidadMigratoria(sender, e);
                                ddlCalidadMigratoriaSec.SelectedValue = obeCarneIdentidadPrincipal.CalidadMigratoriaSec.CalidadMigratoriaid.ToString();
                                lblIdentificador.Text = obeCarneIdentidadPrincipal.CarneIdentidad.ConIdent;
                                string Fecha = "01/01/0001";
                                DateTime FechaNull = DateTime.Parse(Fecha);
                                txtFechaEmision.Text = (obeCarneIdentidadPrincipal.CarneIdentidad.FechaEmision != FechaNull ? obeCarneIdentidadPrincipal.CarneIdentidad.FechaEmision.ToShortDateString() : "");
                                txtFechaVencimiento.Text = (obeCarneIdentidadPrincipal.CarneIdentidad.FechaVencimiento != FechaNull ? obeCarneIdentidadPrincipal.CarneIdentidad.FechaVencimiento.ToShortDateString() : "");
                                // OFICINA CONSULAR EX -------------------------------------------------------------------------------------
                                ddlCategoriaOfcoEx.SelectedValue = obeCarneIdentidadPrincipal.OficinaConsularExtranjera.Categoriaid.ToString();
                                seleccionarCategoriaOficina(sender, e);
                                ddlMision.SelectedValue = obeCarneIdentidadPrincipal.OficinaConsularExtranjera.OficinaconsularExtranjeraid.ToString();
                                // RUTA ARCHIVO -----------------------------------------------------------------------------------------------
                                string rutaArchivo = obrGeneral.rutaAdjuntos + obeCarneIdentidadPrincipal.CarneIdentidad.RutaArchivoFoto;
                                string rutaArchivoFirma = obrGeneral.rutaAdjuntos + obeCarneIdentidadPrincipal.CarneIdentidad.RutaArchivoFirma;
                                ViewState["rutaArchivo"] = obeCarneIdentidadPrincipal.CarneIdentidad.RutaArchivoFoto;
                                ViewState["rutaArchivoFirma"] = obeCarneIdentidadPrincipal.CarneIdentidad.RutaArchivoFirma;
                                cargarImagenAlmacenada(rutaArchivo);
                                cargarImagenAlmacenadaFirma(rutaArchivoFirma);
                                ibtVerImagenAlmacenada.Enabled = true;
                                ibtVerFirmaAlmacenada.Enabled = true;
                                // BOTONES ------------------------------------------------------------------------------------------------------
                                btnGuardar.Visible = false;
                                btnGuardarEdicion.Visible = true;
                                btnCancelar.Visible = true;
                                // RELACION DE DEPENDENCIA
                                short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
                                string ipCreacion = ViewState["IP"].ToString();
                                if (obeCarneIdentidadPrincipal.RelacionDependenciaEdicion != null)
                                {
                                    txtRelDep.Text = obeCarneIdentidadPrincipal.RelacionDependenciaEdicion.CarneNumero;
                                    RelacionDependenciaGen = obeCarneIdentidadPrincipal.RelacionDependencia;
                                    RelacionDependenciaGen.IpCreacion = ipCreacion;
                                    RelacionDependenciaGen.UsuarioCreacion = usuarioId;
                                    RelacionDependenciaGen.IpModificacion = ipCreacion;
                                    RelacionDependenciaGen.UsuarioModificacion = usuarioId;
                                    txtRelDep.Style.Add("background-color", "White");
                                    txtRelDep.Style.Add("color", "Black");
                                    //buscarRelacionDependencia(sender, e);
                                }
                                else 
                                {
                                    if (obeCarneIdentidadPrincipal.RelacionDependencia != null)
                                    {
                                        RelacionDependenciaGen = obeCarneIdentidadPrincipal.RelacionDependencia;
                                        RelacionDependenciaGen.IpCreacion = ipCreacion;
                                        RelacionDependenciaGen.UsuarioCreacion = usuarioId;
                                        RelacionDependenciaGen.IpModificacion = ipCreacion;
                                        RelacionDependenciaGen.UsuarioModificacion = usuarioId;
                                    }
                                }

                                if (obeCarneIdentidadPrincipal.RegistroLinea != null)
                                {
                                    txtRegLinea.Text = obeCarneIdentidadPrincipal.RegistroLinea.NumeroRegLinea;
                                    //txtRegLinea.Enabled = false;
                                    //ibtBuscarRegLinea.Visible = false;
                                }
                            }
                            #region Cargaba datos reg previo
                            //if (obeCarneIdentidadPrincipal.RegistroPrevio != null)
                            //{
                            //    Session["Temp1"] = obeCarneIdentidadPrincipal;
                            //    // CONTROLES
                            //    lblIdentificador.Text = ViewState["identificador"].ToString();
                            //    txtApePat.Text = obeCarneIdentidadPrincipal.RegistroPrevio.PrimerApellido;
                            //    txtApeMat.Text = obeCarneIdentidadPrincipal.RegistroPrevio.SegundoApellido;
                            //    txtNombres.Text = obeCarneIdentidadPrincipal.RegistroPrevio.Nombres;
                            //    ddlSexo.SelectedValue = obeCarneIdentidadPrincipal.RegistroPrevio.GeneroId.ToString();
                            //    seleccionarSexo(sender, e);
                            //    ddlCalidadMigratoriaPri.SelectedValue = obeCarneIdentidadPrincipal.RegistroPrevio.CalidadMigratoria.ToString();
                            //    seleccionarCalidadMigratoria(sender, e);
                            //    ddlCategoriaOfcoEx.SelectedValue = obeCarneIdentidadPrincipal.RegistroPrevio.CategoriaOfcoExId.ToString();
                            //    seleccionarCategoriaOficina(sender, e);
                            //    ddlMision.SelectedValue = obeCarneIdentidadPrincipal.RegistroPrevio.OficinaConsularExId.ToString();
                            //    txtApePat.CssClass = "textboxRegPrev";
                            //    txtApeMat.CssClass = "textboxRegPrev";
                            //    txtNombres.CssClass = "textboxRegPrev";
                            //    ddlSexo.CssClass = "dropdownlistRegPrev";
                            //    ddlCalidadMigratoriaPri.CssClass = "dropdownlistRegPrev";
                            //    ddlCategoriaOfcoEx.CssClass = "dropdownlistRegPrev";
                            //    ddlMision.CssClass = "dropdownlistRegPrev";
                            //    // BOTONES ------------------------------------------------------------------------------------------------------
                            //    btnGuardar.Visible = false;
                            //    btnGuardarEdicion.Visible = true;
                            //    btnCancelar.Visible = true;
                            //}
                            #endregion
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
                    valueTKSEG = oCodigoUsuario.generateValTK(Path.GetFileNameWithoutExtension(Request.Url.LocalPath));
                    TKSEGENC.Value = valueTKSEG;
                    #endregion
                    if (Request.QueryString["Solicitud"] != null)
                    {
                        if (Request.QueryString["Solicitud"] == "NUEVO" || Request.QueryString["Solicitud"] == "RENOVACION")
                        {
                            string carnetNumero = Request.QueryString["NumCarnet"].ToString();

                            txtRegLinea.Text = carnetNumero;
                            buscarRegistroLinea();
                        }
                        string estado = (Request.QueryString["estado"] != null ? Request.QueryString["estado"].ToString() : "");
                        ViewState["estado"] = estado;
                        if (estado.Equals("OBSERVADO") )
                        {
                        
                            lblInfoObserbacion.Text = "REGISTRO OBSERVADO: PARA CONTINUAR CON LA ANTENCIÓN, DEBE SUBSANAR LA OBSERVACIÓN ";
                            btnGuardar.Text = "Subsanar y Registrar Carnet";
                            btnGuardar.Style.Remove("width");
                            btnGuardar.Style.Add("width", "220px");
                        }
                    }
                }
                else
                {
                    #region PruebaValorURLAnterior
                    //csUsuarioBE objUsuarioBE = new csUsuarioBE();
                    //objUsuarioBE = (csUsuarioBE)Session["usuario"];
                    //string qs = "";
                    //bool exito = false;
                    //if (Request.QueryString["valS"] == null & !Request.QueryString["valS"].Equals(""))
                    //{
                    //    obrGeneral.grabarError("Variable vacia");
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensajeError", "location.href='../../mensajes.aspx';", true);
                    //    return;
                    //}
                    //else
                    //{
                    //    qs = Request.QueryString["valS"].ToString();
                    //    exito = oCodigoUsuario.validarSTR(qs, objUsuarioBE.Alias, obrGeneral.host, ViewState["srtAl"].ToString());
                    //    if (!exito) { obrGeneral.grabarError("Error al evaluar la cadena"); } else { obrGeneral.grabarError("Exito al evaluar la cadena"); }
                    //}
                    //if (!exito) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensajeError", "location.href='../../mensajes.aspx';", true); return; }
                    #endregion
                    #region PruebaValorURL
                    csUsuarioBE objUsuarioBE = new csUsuarioBE();
                    objUsuarioBE = (csUsuarioBE)Session["usuario"];
                    string qs = (Request.QueryString["valS"] != null & !Request.QueryString["valS"].ToString().Equals("") ? Request.QueryString["valS"].ToString() : "error");
                    bool exito = oCodigoUsuario.validarSTR(qs, objUsuarioBE.Alias, obrGeneral.host, ViewState["srtAl"].ToString());
                    if (!exito) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensajeError", "location.href='../../mensajes.aspx';", true); return; }
                    #endregion
                    #region Postback
                    divModal.Style.Add("display", "none");
                    divRelDep.Style.Add("display", "none");
                    if (hdnFile.Value.Equals("1"))
                    {
                        cargaImagen();
                    }
                    if (hdnFileFirma.Value.Equals("1"))
                    {
                        cargaImagenFirma();
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "changeRadioButton", "changeRadioButton();", true);
                    #endregion
                }
            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR AL CARGAR LA PÁGINA');", true);
            }
        }
        #region CARGA Y SELECCION DE CONTROLES
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
                        txtTelefono.Focus();
                    }
                }
            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
            }
        }

        protected void seleccionarDepartamento(object sender, EventArgs e) // LISTA EL DROPDOWN PROVINCIA
        {
            try
            {
                if(Session["Ubigeo"] != null)
                {
                    beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                    obeUbigeoListas = (beUbigeoListas)Session["Ubigeo"];
                    List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                    lbeUbicaciongeografica = oCodigoUsuario.obtenerListaUbiGeo("02", ddlDepartamento.SelectedValue,"", obeUbigeoListas.Ubigeo02);
                    ddlProvincia.DataSource = lbeUbicaciongeografica;
                    ddlProvincia.DataValueField = "Ubi02";
                    ddlProvincia.DataTextField = "Provincia";
                    ddlProvincia.DataBind();
                    cargarComboNull(ddlDistrito);
                    ddlDistrito.Enabled = false;
                    ddlProvincia.Enabled = (ddlDepartamento.SelectedValue.Equals("00") ? false : true);
                    ddlProvincia.Focus();
                }
            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
            }
        }

        protected void seleccionarProvincia(object sender, EventArgs e) // LISTA EL DROPDOWN DISTRITO
        {
            try
            {
                if (Session["Ubigeo"] != null)
                {
                    beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                    obeUbigeoListas = (beUbigeoListas)Session["Ubigeo"];
                    List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                    lbeUbicaciongeografica = oCodigoUsuario.obtenerListaUbiGeo("03", ddlDepartamento.SelectedValue, ddlProvincia.SelectedValue, obeUbigeoListas.Ubigeo03);
                    ddlDistrito.DataSource = lbeUbicaciongeografica;
                    ddlDistrito.DataValueField = "Ubi03";
                    ddlDistrito.DataTextField = "Distrito";
                    ddlDistrito.DataBind();
                    ddlDistrito.Enabled = (ddlProvincia.SelectedValue.Equals("00") ? false : true);
                    ddlDistrito.Focus();
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
            }
        }

        protected void seleccionarCalidadMigratoria(object sender, EventArgs e) // DROPDOWLIST - A LA SELECCION DE CALIDAD MUESTRA LOS CARGOS DISPONIBLES PARA ESA MISMA
        {
            try
            {
                if (Session["Generales"] != null)
                {
                    //RelacionDependenciaGen = null;
                    //titularRelDep = null;
                    //buscarRelacionDependencia(sender, e);
                    string titDep = oCodigoUsuario.detectarCaracterEspecial(ddlTitDep.SelectedItem.Text.Trim());
                    beGenerales obeGenerales = new beGenerales();
                    obeGenerales = (beGenerales)Session["Generales"];
                    obeGenerales.ListaCalidadMigratoriaNivelSecundario.Insert(0, new beCalidadMigratoria { CalidadMigratoriaid = 0, Nombre = "<Seleccione>" });
                    List<beCalidadMigratoria> lbeCalidadMigratoria = new List<beCalidadMigratoria>();
                    //int pos = obeGenerales.TitularDependienteParametros.FindIndex(x => x.Valor.Equals(hdrbt.Value));
                    short Referencia = short.Parse(ddlCalidadMigratoriaPri.SelectedValue);
                    calidadHumanitaria(sender, e);
                    short TitularDependiente = short.Parse(ddlTitDep.SelectedValue);
                    short genero = short.Parse(ddlSexo.SelectedValue);
                    if (TitularDependiente > 0 & genero > 0)
                    {
                        lbeCalidadMigratoria = oCodigoUsuario.obtenerListaTitularDependiente(Referencia, TitularDependiente, genero, obeGenerales.ListaCalidadMigratoriaNivelSecundario);
                    }
                    if (lbeCalidadMigratoria.Count > 0)
                    {
                        string valor = "0";
                        if (ddlCalidadMigratoriaSec != null)
                        {
                            valor = ddlCalidadMigratoriaSec.SelectedValue;
                            ddlCalidadMigratoriaSec.SelectedValue = "0";
                        }
                        ddlCalidadMigratoriaSec.DataSource = lbeCalidadMigratoria;
                        ddlCalidadMigratoriaSec.DataValueField = "CalidadMigratoriaid";
                        ddlCalidadMigratoriaSec.DataTextField = "Nombre";
                        ddlCalidadMigratoriaSec.DataBind();
                        lblMensajeCalidadMigratoria.Text = (lbeCalidadMigratoria.Count == 1 ? "No se encontraron opciones de cargo para esta calidad migratoria" : "");
                        ddlCalidadMigratoriaSec.Enabled = true;
                        
                    }
                    else
                    {
                        cargarComboNull(ddlCalidadMigratoriaSec);
                        lblMensajeCalidadMigratoria.Text = (!ddlCalidadMigratoriaPri.SelectedValue.Equals("0") & !ddlTitDep.SelectedValue.Equals("0") ? "No se encontraron opciones de cargo para esta calidad migratoria" : "");
                        ddlCalidadMigratoriaSec.Enabled = false;
                    }
                    // OBTIENE DEFINICION DE CALIDAD MIGRATORIA
                    if (!ddlCalidadMigratoriaPri.SelectedValue.Equals("0"))
                    {
                        int posTexto = obeGenerales.ListaCalidadMigratoriaNivelPrincipal.FindIndex(x => x.CalidadMigratoriaid == short.Parse(ddlCalidadMigratoriaPri.SelectedValue));
                        if (posTexto != -1)
                        {
                            txtCalidadMigratoria.Text = obeGenerales.ListaCalidadMigratoriaNivelPrincipal[posTexto].Definicion;
                        }
                    }
                    else
                    {
                        txtCalidadMigratoria.Text = "";
                    }
                    if (titDep.Equals("TITULAR"))
                    {
                        trRelDep.Style.Add("display", "none");
                    }
                    else
                    {
                        trRelDep.Style.Add("display", "table-row");
                    }
                    //buscarRelacionDependencia(sender, e);
                }
            }
            catch(Exception ex)
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
                            string valor = "0";
                            if (ddlMision != null)
                            {
                                valor = ddlMision.SelectedValue;
                                ddlMision.SelectedValue = "0";
                            }

                            lbeOficinaconsularExtranjera.Insert(0, new beOficinaconsularExtranjera { OficinaconsularExtranjeraid = 0, Nombre = "<Seleccione>" });
                            ddlMision.DataSource = lbeOficinaconsularExtranjera;
                            ddlMision.DataValueField = "OficinaconsularExtranjeraid";
                            ddlMision.DataTextField = "Nombre";
                            ddlMision.DataBind();
                            ddlMision.Enabled = true;

                            if (ddlMision.SelectedValue == "0")
                            {
                                if (valor != "0")
                                {
                                    ddlMision.SelectedValue = valor;
                                }
                            }
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
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
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
                                listaEstadoCivil.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                                ddlEstadoCivil.DataSource = listaEstadoCivil;
                                ddlEstadoCivil.DataValueField = "Parametroid";
                                ddlEstadoCivil.DataTextField = "Descripcion";
                                ddlEstadoCivil.DataBind();
                                ddlEstadoCivil.Enabled = true;
                                ddlEstadoCivil.Focus();
                                seleccionarCalidadMigratoria(sender, e);
                            }
                            else
                            {
                                cargarComboNull(ddlEstadoCivil);
                                ddlEstadoCivil.Enabled = false;
                            }
                        }
                    }
                }
                else
                {
                    cargarComboNull(ddlEstadoCivil);
                    cargarComboNull(ddlCalidadMigratoriaSec);
                    seleccionarCalidadMigratoria(sender, e);
                    ddlEstadoCivil.Enabled = false;
                    ddlCalidadMigratoriaSec.Enabled = false;
                }
            }
            catch(Exception ex)
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
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }
        #endregion
        #region REGISTRO DEL CARNE

        private void registrarCarneIdentidad()
        {
            string resultado = "";
            try
            {
                #region TKSEG
                string TKSEG = TKSEGENC.Value;
                bool exitoTKSEG = oCodigoUsuario.compareValTK(TKSEG, valueTKSEG);
                if (!exitoTKSEG)
                {
                    obrGeneral.grabarError("Error en la evaluacion del TK");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensajeError", "location.href='../../mensajes.aspx';", true);
                    return;
                }
                #endregion
                bool exitoEvaluar = evaluarControles();
                if (exitoEvaluar)
                {
                    short estadoReg = obtenerIdEstado("REGISTRADO");
                    short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
                    short oficinaId = short.Parse(ViewState["Oficinaid"].ToString());
                    string ipCreacion = ViewState["IP"].ToString();
                    string ubiGeo = ddlDepartamento.SelectedValue + ddlProvincia.SelectedValue + ddlDistrito.SelectedValue;
                    bool menorEdad = chkEsMenor.Checked;
                    // ADJUNTAR ARCHIVO
                    string rutaAdjuntos = obrGeneral.rutaAdjuntos + @"\" + DateTime.Now.Year.ToString() + @"\" + DateTime.Now.Month.ToString("D2") + @"\";
                    // FOTO
                    string fileName = "F1" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2") + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2") + DateTime.Now.Millisecond.ToString() + obrGeneral.fileExtFotografia;
                    bool exitoFotografia = guardarImagen(rutaAdjuntos, fileName);
                    bool FotoRegistroLinea = false;
                    bool FirmaRegistroLinea = false;
                    if (!exitoFotografia)
                    {
                        if (obeRegistroLinea != null)
                        {
                            if (obeRegistroLinea.RegistroLineaId > 0)
                            {
                                if (ViewState["rutaArchivo"].ToString().Length > 0)
                                {
                                    FotoRegistroLinea = true;
                                    exitoFotografia = true;
                                }
                            }
                        }
                    }

                    // FIRMA
                    string fileNameFirma = "";
                    bool exitoFirma = false;
                    if (menorEdad)
                    {
                        exitoFirma = true;
                    }
                    else
                    {
                        fileNameFirma = "F2" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2") + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2") + DateTime.Now.Millisecond.ToString() + obrGeneral.fileExtFotografia;
                        exitoFirma = guardarImagenFirma(rutaAdjuntos, fileNameFirma);

                        if (!exitoFirma)
                        {
                            if (obeRegistroLinea != null)
                            {
                                if (obeRegistroLinea.RegistroLineaId > 0)
                                {
                                    if (ViewState["rutaArchivoFirma"].ToString().Length > 0)
                                    {
                                        FirmaRegistroLinea = true;
                                        exitoFirma = true;
                                    }
                                }
                            }
                        }
                    }

                    if (exitoFotografia & exitoFirma)
                    {
                        string rutaRelativaFirma = "";
                        string rutaRelativa = "";
                        if (FotoRegistroLinea)
                        {
                            rutaRelativa = ViewState["rutaArchivo"].ToString();
                        }
                        else
                        {
                            rutaRelativa = @"\" + DateTime.Now.Year.ToString() + @"\" + DateTime.Now.Month.ToString("D2") + @"\" + fileName;
                        }

                        if (menorEdad) { 
                            rutaRelativaFirma = "MENOR_EDAD"; 
                        }
                        else {
                            if (FirmaRegistroLinea)
                            {
                                rutaRelativaFirma = ViewState["rutaArchivoFirma"].ToString();
                            }
                            else {
                                rutaRelativaFirma = @"\" + DateTime.Now.Year.ToString() + @"\" + DateTime.Now.Month.ToString("D2") + @"\" + fileNameFirma; 
                            }
                        }
                        beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
                        // PERSONA
                        bePersona obePersona = new bePersona();
                        obePersona.Estadocivilid = short.Parse(ddlEstadoCivil.SelectedValue);
                        obePersona.Generoid = short.Parse(ddlSexo.SelectedValue);
                        obePersona.Apellidopaterno = txtApePat.Text.Trim().ToUpper();
                        obePersona.Apellidomaterno = (!txtApeMat.Text.Equals("") ? txtApeMat.Text.Trim().ToUpper() : null);
                        obePersona.Nombres = txtNombres.Text.Trim().ToUpper();
                        obePersona.Telefono = (!txtTelefono.Text.Equals("") ? txtTelefono.Text.Trim() : null);
                        obePersona.Nacimientofecha = DateTime.Parse(txtFechaNac.Text);
                        obePersona.Paisid = short.Parse(ddlNacionalidad.SelectedValue);
                        obePersona.Usuariocreacion = usuarioId;
                        obePersona.Ipcreacion = ipCreacion;
                        obePersona.MenorEdad = menorEdad;
                        obeCarneIdentidadPrincipal.Persona = obePersona;
                        // PERSONA IDENTIFICACION
                        bePersonaidentificacion obePersonaidentificacion = new bePersonaidentificacion();
                        obePersonaidentificacion.Documentotipoid = short.Parse(ddlDocumentoIdent.SelectedValue);
                        obePersonaidentificacion.Documentonumero = txtNumeroDocIdent.Text.Trim().ToUpper();
                        obePersonaidentificacion.Usuariocreacion = usuarioId;
                        obePersonaidentificacion.Ipcreacion = ipCreacion;
                        obeCarneIdentidadPrincipal.PersonaIdentificacion = obePersonaidentificacion;
                        // RESIDENCIA
                        beResidencia obeResidencia = new beResidencia();
                        obeResidencia.Residenciadireccion = txtDomicilio.Text.Trim().ToUpper();
                        obeResidencia.Residenciaubigeo = ubiGeo;
                        obeResidencia.Usuariocreacion = usuarioId;
                        obeResidencia.Ipcreacion = ipCreacion;
                        obeCarneIdentidadPrincipal.Residencia = obeResidencia;
                        // PERSONA RESIDENCIA
                        bePersonaresidencia obePersonaresidencia = new bePersonaresidencia();
                        obePersonaresidencia.Usuariocreacion = usuarioId;
                        obePersonaresidencia.Ipcreacion = ipCreacion;
                        obeCarneIdentidadPrincipal.PersonaResidencia = obePersonaresidencia;
                        // CARNE IDENTIDAD
                        beCarneIdentidad obeCarneIdentidad = new beCarneIdentidad();
                        obeCarneIdentidad.Periodo = DateTime.Now.Year;
                        if (txtMesaPartes.Text.Equals("")) { obeCarneIdentidad.IdentMesaPartes = null; }
                        else { obeCarneIdentidad.IdentMesaPartes = txtMesaPartes.Text.ToUpper().Trim(); }
                        obeCarneIdentidad.CalidadMigratoriaid = short.Parse(ddlCalidadMigratoriaPri.SelectedValue);
                        obeCarneIdentidad.CalidadMigratoriaSecId = short.Parse(ddlCalidadMigratoriaSec.SelectedValue);
                        obeCarneIdentidad.OficinaConsularExid = short.Parse(ddlMision.SelectedValue);
                        obeCarneIdentidad.RutaArchivoFoto = rutaRelativa;
                        obeCarneIdentidad.RutaArchivoFirma = rutaRelativaFirma;
                        obeCarneIdentidad.Renovar = false;
                        obeCarneIdentidad.Estadoid = estadoReg;
                        obeCarneIdentidad.Usuariocreacion = usuarioId;
                        obeCarneIdentidad.Ipcreacion = ipCreacion;
                        obeCarneIdentidad.UsuarioDeriva = usuarioId;
                        obeCarneIdentidad.Usuariomodificacion = usuarioId;
                        obeCarneIdentidad.Ipmodificacion = ipCreacion;
                        obeCarneIdentidadPrincipal.CarneIdentidad = obeCarneIdentidad;
                        // HISTORICO DE PERSONA
                        bePersonaHistorico obePersonaHistorico = new bePersonaHistorico();
                        obePersonaHistorico.ApellidoPaterno = txtApePat.Text.Trim().ToUpper();
                        obePersonaHistorico.ApellidoMaterno = txtApeMat.Text.Trim().ToUpper();
                        obePersonaHistorico.Nombres = txtNombres.Text.Trim().ToUpper();
                        obePersonaHistorico.Telefono = (!txtTelefono.Text.Equals("") ? txtTelefono.Text.Trim() : null);
                        obePersonaHistorico.EstadoCivilid = short.Parse(ddlEstadoCivil.SelectedValue);
                        obePersonaHistorico.Generoid = short.Parse(ddlSexo.SelectedValue);
                        obePersonaHistorico.FechaNacimiento = DateTime.Parse(txtFechaNac.Text);
                        obePersonaHistorico.PaisNacionalidadid = short.Parse(ddlNacionalidad.SelectedValue);
                        obePersonaHistorico.Usuariocreacion = usuarioId;
                        obePersonaHistorico.Ipcreacion = ipCreacion;
                        obePersonaHistorico.MenorEdad = menorEdad;
                        obeCarneIdentidadPrincipal.PersonaHistorico = obePersonaHistorico;
                        // HISTORICO CARNE IDENTIDAD
                        beCarneIdentidadHistorico obeCarneIdentidadHistorico = new beCarneIdentidadHistorico();
                        obeCarneIdentidadHistorico.CalidadMigratoriaid = short.Parse(ddlCalidadMigratoriaPri.SelectedValue);
                        obeCarneIdentidadHistorico.CalidadMigratoriaSecId = short.Parse(ddlCalidadMigratoriaSec.SelectedValue);
                        obeCarneIdentidadHistorico.RutaArchivoFoto = rutaRelativa;
                        obeCarneIdentidadHistorico.OficinaConsularExid = short.Parse(ddlMision.SelectedValue);
                        obeCarneIdentidadHistorico.EstadoId = estadoReg;
                        obeCarneIdentidadHistorico.Usuariocreacion = usuarioId;
                        obeCarneIdentidadHistorico.Ipcreacion = ipCreacion;
                        obeCarneIdentidadHistorico.RutaArchivoFirma = rutaRelativaFirma;
                        obeCarneIdentidadPrincipal.CarneIdentidadHistorico = obeCarneIdentidadHistorico;
                        // MOVIMIENTO CARNE IDENTIDAD
                        beMovimientoCarneIdentidad obeMovimientoCarneIdentidad = new beMovimientoCarneIdentidad();
                        obeMovimientoCarneIdentidad.Estadoid = estadoReg;
                        obeMovimientoCarneIdentidad.Oficinaconsularid = oficinaId;
                        obeMovimientoCarneIdentidad.Usuariocreacion = usuarioId;
                        obeMovimientoCarneIdentidad.Ipcreacion = ipCreacion;
                        obeCarneIdentidadPrincipal.MovimientoCarne = obeMovimientoCarneIdentidad;
                        // RELACION DEPENDENCIA
                        obeCarneIdentidadPrincipal.RelacionDependenciaGen = RelacionDependenciaGen;
                        //if (titularRelDep != null)
                        //{

                        //}
                        //else
                        //{
                        //    titularRelDep = new beCarneIdentidad();
                        //    titularRelDep.Usuariocreacion = usuarioId;
                        //    titularRelDep.Ipcreacion = ipCreacion;
                        //    obeCarneIdentidadPrincipal.titularTitDep = titularRelDep;
                        //}
                        // REGISTRO EN LINEA
                        obeRegistroLinea.UsuarioModificacion = usuarioId;
                        obeRegistroLinea.IpModificacion = ipCreacion;
                        obeCarneIdentidadPrincipal.RegistroLinea = obeRegistroLinea;
                        brCarneIdentidadPrincipal obrCarneIdentidadPrincipal = new brCarneIdentidadPrincipal();

                        beCarneIdentidad obrCarneIdentidad = new beCarneIdentidad();
                        //obrCarneIdentidad.ApePatPersona = txtApePat.Text.Trim().ToUpper();
                        //obrCarneIdentidad.NombresPersona = txtNombres.Text.Trim().ToUpper();
                        //bool respuesta = obrCarneIdentidadPrincipal.ConsultarExistenciaCarnetPorNombre(obrCarneIdentidad);

                        //if (respuesta) // hay personas con un nombre igual con carnet
                        //{
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "myconfirm", "ValidarPersonaConMismoNombre();", true);
                        //}
                        //string estado = ViewState["estado"].ToString();
                        //if (estado.Equals("OBSERVADO"))
                        //{   //se actualiza el estado observado a (cabecera=enviado, en detalle=aprobado
                        //    bool exito = obrCarneIdentidadPrincipal.subsanarObservacion(obeRegistroLinea);
                        //}
                        int CarneIdentidadId = obrCarneIdentidadPrincipal.adicionar(obeCarneIdentidadPrincipal, out resultado);
                        
                        
                        hErrorRegistro.Value = resultado;
                        if (CarneIdentidadId > 0)
                        {
                            DisableControls(tablaMesaPartes, false);
                            DisableControls(tablaFuncionario, false);
                            DisableControls(tablaCalMig, false);
                            DisableControls(tablaMision, false);
                            DisableControls(tablaFotografia, false);
                            DisableControls(tablaFirma, false);
                            string ident = obrCarneIdentidadPrincipal.obtenerIdent(CarneIdentidadId);
                            lblIdentificador.Text = ident;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE GUARDÓ LA INFORMACIÓN CORRECTAMENTE');", true);
                        }
                        else
                        {
                            File.Delete(rutaAdjuntos + fileName);
                            if (!menorEdad) { File.Delete(rutaAdjuntos + fileNameFirma); }
                            //calidadHumanitaria(sender, e);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "habilitarControles", "disIntablaFuncionario(false);", true);
                            if (CarneIdentidadId == -1) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR AL GUARDAR LA INFORMACIÓN.');", true); }
                            if (CarneIdentidadId == -2) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('YA EXISTE UNA PERSONA CON ESE TIPO Y NUMERO DE IDENTIFICACION.');", true); ddlDocumentoIdent.Focus(); }
                        }
                    }
                    else
                    {
                        //calidadHumanitaria(sender, e);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "habilitarControles", "disIntablaFuncionario(false);", true);
                        if (!exitoFotografia) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('NO SE SELECCIONO UN ARCHIVO DE FOTOGRAFIA');", true); return; }
                        if (!exitoFirma) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('NO SE SELECCIONO UN ARCHIVO DE FIRMA');", true); return; }
                    }
                }
                else
                {
                    Session["mensaje"] = "OCURRIÓ UN ERROR";
                    obrGeneral.grabarError("Error en la evaluacion de los controles");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensajeError", "location.href='../../mensajes.aspx';", true);
                }

            }
            catch (Exception ex)
            {
                hErrorRegistro.Value = resultado;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "habilitarControles", "disIntablaFuncionario(false);", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
                //calidadHumanitaria(sender, e);
                obrGeneral.grabarLog(ex);
            }
        }


        protected void registrarCarne(object sender, EventArgs e) // REGISTR UN NUEVO REGISTRO DE CARNÉ DE IDENTIDAD
        {
            try
            {
                brCarneIdentidadPrincipal obrCarneIdentidadPrincipal = new brCarneIdentidadPrincipal();

                beCarneIdentidad obrCarneIdentidad = new beCarneIdentidad();
                obrCarneIdentidad.ApePatPersona = txtApePat.Text.Trim().ToUpper();
                obrCarneIdentidad.NombresPersona = txtNombres.Text.Trim().ToUpper();
                bool respuesta = obrCarneIdentidadPrincipal.ConsultarExistenciaCarnetPorNombre(obrCarneIdentidad);

                if (respuesta) // hay personas con un nombre igual con carnet
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "AbrirPopup();", true);
                }
                else
                {
                    registrarCarneIdentidad();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "habilitarControles", "disIntablaFuncionario(false);", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
                //calidadHumanitaria(sender, e);
                obrGeneral.grabarLog(ex);
            }
        }
       

        protected void guardarCambios(object sender, EventArgs e) // ACTUALIZA LA INFORMACIÓN DEL CARNÉ DE IDENTIDAD
        {
            try
            {
                #region TKSEG
                string TKSEG = TKSEGENC.Value;
                bool exitoTKSEG = oCodigoUsuario.compareValTK(TKSEG, valueTKSEG);
                if (!exitoTKSEG)
                {
                    obrGeneral.grabarError("Error en la evaluacion del TK");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensajeError", "location.href='../../mensajes.aspx';", true);
                    return;
                }
                #endregion
                bool exitoEvaluar = evaluarControles();
                if (exitoEvaluar)
                {
                    string rutaRelativa = "";
                    string rutaRelativaFirma = "";
                    // ESTADO REGISTRADO
                    short tipoObs = obtenerTipoObs("EDICIÓN DE REGISTRO");
                    short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
                    short oficinaId = short.Parse(ViewState["Oficinaid"].ToString());
                    string ipCreacion = ViewState["IP"].ToString();
                    string ubiGeo = ddlDepartamento.SelectedValue + ddlProvincia.SelectedValue + ddlDistrito.SelectedValue;
                    bool menorEdad = chkEsMenor.Checked;

                    bool tipoActualizacion = true;//bool.Parse(ViewState["regComp"].ToString());
                    #region Edicion Normal
                    string rutaAdjuntos = "";
                    string fileName = "";
                    bool exitoFotografia = false;
                    string fileNameFirma = "";
                    bool exitoFirma = false;
                    if (tipoActualizacion)
                    {
                        // ADJUNTAR ARCHIVO DE FOTOGRAFIA
                        rutaAdjuntos = obrGeneral.rutaAdjuntos + @"\" + DateTime.Now.Year.ToString() + @"\" + DateTime.Now.Month.ToString("D2") + @"\";
                        // FOTO
                        fileName = "F1" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2") + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2") + DateTime.Now.Millisecond.ToString() + obrGeneral.fileExtFotografia;
                        exitoFotografia = guardarImagen(rutaAdjuntos, fileName);
                        if (exitoFotografia)
                        {
                            rutaRelativa = @"\" + DateTime.Now.Year.ToString() + @"\" + DateTime.Now.Month.ToString("D2") + @"\" + fileName;
                        }
                        else
                        {
                            rutaRelativa = (ViewState["rutaArchivo"] != null ? ViewState["rutaArchivo"].ToString() : "error");
                        }

                        
                        if (menorEdad)
                        {
                            exitoFirma = true;
                        }
                        else
                        {
                            fileNameFirma = "F2" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2") + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2") + DateTime.Now.Millisecond.ToString() + obrGeneral.fileExtFotografia;
                            exitoFirma = guardarImagenFirma(rutaAdjuntos, fileNameFirma);
                        }
                        // FIRMA
                        if (exitoFirma)
                        {
                            if (menorEdad)
                            {
                                rutaRelativaFirma = "MENOR_EDAD";
                            }
                            else
                            {
                                rutaRelativaFirma = @"\" + DateTime.Now.Year.ToString() + @"\" + DateTime.Now.Month.ToString("D2") + @"\" + fileNameFirma;
                            }
                        }
                        else
                        {
                            if (menorEdad)
                            {
                                ViewState["rutaArchivoFirma"] = "MENOR_EDAD";
                            }
                            else
                            {
                                if (ViewState["rutaArchivoFirma"] != null)
                                {
                                    if (ViewState["rutaArchivoFirma"].ToString() == "MENOR_EDAD") { rutaRelativaFirma = "error"; }
                                    else { rutaRelativaFirma = (ViewState["rutaArchivoFirma"] != null ? ViewState["rutaArchivoFirma"].ToString() : "error"); }
                                }
                                else
                                {
                                    rutaRelativaFirma = "error";
                                }
                            }
                        }
                    }
                    #endregion
                    #region Edicion Registro Completo
                    else
                    {
                        // ADJUNTAR ARCHIVO
                        rutaAdjuntos = obrGeneral.rutaAdjuntos + @"\" + DateTime.Now.Year.ToString() + @"\" + DateTime.Now.Month.ToString("D2") + @"\";
                        // FOTO
                        fileName = "F1" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2") + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2") + DateTime.Now.Millisecond.ToString() + obrGeneral.fileExtFotografia;
                        exitoFotografia = guardarImagen(rutaAdjuntos, fileName);
                        // FIRMA
                        if (menorEdad)
                        {
                            exitoFirma = true;
                        }
                        else
                        {
                            fileNameFirma = "F2" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2") + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2") + DateTime.Now.Millisecond.ToString() + obrGeneral.fileExtFotografia;
                            exitoFirma = guardarImagenFirma(rutaAdjuntos, fileNameFirma);
                        }
                        if (exitoFotografia & exitoFirma)
                        {
                            rutaRelativa = @"\" + DateTime.Now.Year.ToString() + @"\" + DateTime.Now.Month.ToString("D2") + @"\" + fileName;
                            if (menorEdad) { rutaRelativaFirma = "MENOR_EDAD"; }
                            else { rutaRelativaFirma = @"\" + DateTime.Now.Year.ToString() + @"\" + DateTime.Now.Month.ToString("D2") + @"\" + fileNameFirma; }
                        }
                        else
                        {
                            if (!exitoFotografia)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('NO SE SELECCIONO UN ARCHIVO DE FOTOGRAFIA');", true);
                                return;
                            }
                            rutaRelativa = "error";
                            rutaRelativaFirma = "error";
                        }
                    }
                    #endregion
                    if (!rutaRelativa.Equals("error") & !rutaRelativaFirma.Equals("error"))
                    {
                        if (Session["Temp1"] != null)
                        {
                            string obsDet = null;
                            bool renovar = false;
                            if (!tipoActualizacion) { obsDet = "REGISTRO DE DATOS COMPLETO"; }
                            if (ViewState["renovar"] != null)
                            {
                                string renovarFlag = ViewState["renovar"].ToString();
                                if (renovarFlag.Equals("1"))
                                {
                                    renovar = true;
                                    obsDet = "PARA RENOVACION";
                                }
                            }
                            beCarneIdentidadPrincipal datosSave = new beCarneIdentidadPrincipal();
                            datosSave = (beCarneIdentidadPrincipal)Session["Temp1"];
                            beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
                            // DEFINE ESTADO DEL CARNE
                            short estadoReg = -1;
                            if (tipoActualizacion) {
                                estadoReg = obtenerIdEstado("REGISTRADO");
                            }
                            else 
                            {
                                if (datosSave.CarneIdentidad != null)
                                {
                                    if (datosSave.CarneIdentidad.FlagControlCalidad)
                                    {
                                        estadoReg = obtenerIdEstado("CONTROL DE CALIDAD");
                                    }
                                    else
                                    {
                                        estadoReg = obtenerIdEstado("REGISTRADO");
                                    }
                                }
                                else
                                {
                                    estadoReg = obtenerIdEstado("REGISTRADO");
                                }
                            }
                            /*String estadoDesc = "";
                            if (ViewState["estado"].ToString().Equals("OBSERVADO"))
                            {
                                estadoReg = obtenerIdEstado("APROBADO");
                                estadoDesc = ViewState["estado"].ToString();
                                brRegistroLinea obrRegistroLinea = new brRegistroLinea();
                                short carnetId = (datosSave.CarneIdentidad != null ? datosSave.CarneIdentidad.CarneIdentidadid : short.Parse(ViewState["carneId"].ToString()));
                                obeRegistroLinea = obrRegistroLinea.consultarRegistroLineaPorIdCarnet(carnetId);
                                beGenerales obeGenerales2 = (beGenerales)Session["Generales"];
                                int pos = obeGenerales2.ListaEstadosRegLinea.FindIndex(x => x.DescripcionCorta.Equals("APROBADO"));
                                short estadoIdRegLinea2 = -1;
                                if (pos != -1) { estadoIdRegLinea2 = obeGenerales2.ListaEstadosRegLinea[pos].Estadoid; }
                                obeRegistroLinea.EstadoId = estadoIdRegLinea2;
                                obeRegistroLinea.Estado = estadoDesc;
                            }*/
                            #region Reg Previo
                            // REGISTRO PREVIO - SE ACTUALIZA
                            //beRegistroPrevio obeRegistroPrevio = new beRegistroPrevio();
                            //obeRegistroPrevio.RegistroPrevioId = short.Parse(ViewState["regPrevId"].ToString());
                            //obeRegistroPrevio.ConCarneIdentidadId = short.Parse(ViewState["carneId"].ToString());
                            //obeRegistroPrevio.PrimerApellido = txtApePat.Text.ToUpper().Trim();
                            //obeRegistroPrevio.SegundoApellido = txtApeMat.Text.ToUpper().Trim();
                            //obeRegistroPrevio.Nombres = txtNombres.Text.ToUpper().Trim();
                            //obeRegistroPrevio.GeneroId = short.Parse(ddlSexo.SelectedValue);
                            //obeRegistroPrevio.OficinaConsularExId = short.Parse(ddlMision.SelectedValue);
                            //obeRegistroPrevio.CalidadMigratoria = short.Parse(ddlCalidadMigratoriaPri.SelectedValue);
                            //if (tipoActualizacion)
                            //{
                            //    obeRegistroPrevio.FechaConsulares = datosSave.RegistroPrevioEdicion.FechaConsulares; //DateTime.Parse(txtFechaConsulares.Text);
                            //    if (datosSave.RegistroPrevioEdicion.FechaPrivilegios != null)
                            //    {
                            //        obeRegistroPrevio.FechaPrivilegios = datosSave.RegistroPrevioEdicion.FechaPrivilegios;
                            //    }
                            //    obeRegistroPrevio.TipoEntrada = datosSave.RegistroPrevioEdicion.TipoEntrada;
                            //}
                            //else
                            //{
                            //    obeRegistroPrevio.FechaConsulares = datosSave.RegistroPrevio.FechaConsulares; //DateTime.Parse(txtFechaConsulares.Text);
                            //    if (datosSave.RegistroPrevio.FechaPrivilegios != null)
                            //    {
                            //        obeRegistroPrevio.FechaPrivilegios = datosSave.RegistroPrevio.FechaPrivilegios;
                            //    }
                            //    obeRegistroPrevio.TipoEntrada = datosSave.RegistroPrevio.TipoEntrada;
                            //}


                            //obeRegistroPrevio.UsuarioModificacion = usuarioId;
                            //obeRegistroPrevio.IpModificacion = ipCreacion;
                            //obeCarneIdentidadPrincipal.RegistroPrevio = obeRegistroPrevio;
                            #endregion
                            // PERSONA - SE ACTUALIZA
                            bePersona obePersona = new bePersona();
                            //obePersona.Personaid = datosSave.Persona.Personaid;
                            obePersona.Personaid = (datosSave.Persona != null ? datosSave.Persona.Personaid : 0);
                            obePersona.Estadocivilid = short.Parse(ddlEstadoCivil.SelectedValue);
                            obePersona.Generoid = short.Parse(ddlSexo.SelectedValue);
                            obePersona.Apellidopaterno = txtApePat.Text.Trim().ToUpper();
                            obePersona.Apellidomaterno = (!txtApeMat.Text.Equals("") ? txtApeMat.Text.Trim().ToUpper() : null);
                            obePersona.Nombres = txtNombres.Text.Trim().ToUpper();
                            obePersona.Telefono = (!txtTelefono.Text.Equals("") ? txtTelefono.Text.Trim() : null);
                            obePersona.Nacimientofecha = DateTime.Parse(txtFechaNac.Text);
                            obePersona.Paisid = short.Parse(ddlNacionalidad.SelectedValue);
                            if (datosSave.Persona != null)
                            {
                                obePersona.Usuariomodificacion = usuarioId;
                                obePersona.Ipmodificacion = ipCreacion;
                            }
                            else
                            {
                                obePersona.Usuariocreacion = usuarioId;
                                obePersona.Ipcreacion = ipCreacion;
                            }
                            obePersona.MenorEdad = menorEdad;
                            obeCarneIdentidadPrincipal.Persona = obePersona;
                            // PERSONA IDENTIFICACION - NUEVO
                            bePersonaidentificacion obePersonaidentificacion = new bePersonaidentificacion();
                            //obePersonaidentificacion.Personaid = datosSave.Persona.Personaid;
                            obePersonaidentificacion.Personaid = (datosSave.Persona != null ? datosSave.Persona.Personaid : 0);
                            obePersonaidentificacion.Documentotipoid = short.Parse(ddlDocumentoIdent.SelectedValue);
                            obePersonaidentificacion.Documentonumero = txtNumeroDocIdent.Text.Trim().ToUpper();
                            obePersonaidentificacion.Usuariocreacion = usuarioId;
                            obePersonaidentificacion.Ipcreacion = ipCreacion;
                            obeCarneIdentidadPrincipal.PersonaIdentificacion = obePersonaidentificacion;
                            // RESIDENCIA - NUEVO
                            beResidencia obeResidencia = new beResidencia();
                            obeResidencia.Residenciadireccion = txtDomicilio.Text.Trim().ToUpper();
                            obeResidencia.Residenciaubigeo = ubiGeo;
                            obeResidencia.Usuariocreacion = usuarioId;
                            obeResidencia.Ipcreacion = ipCreacion;
                            obeCarneIdentidadPrincipal.Residencia = obeResidencia;
                            // PERSONA RESIDENCIA - NUEVO
                            bePersonaresidencia obePersonaresidencia = new bePersonaresidencia();
                            obePersonaresidencia.Personaid = (datosSave.Persona != null ? datosSave.Persona.Personaid : 0);//datosSave.Persona.Personaid;
                            obePersonaresidencia.Usuariocreacion = usuarioId;
                            obePersonaresidencia.Ipcreacion = ipCreacion;
                            obeCarneIdentidadPrincipal.PersonaResidencia = obePersonaresidencia;
                            // CARNE IDENTIDAD  - SE ACTUALIZA
                            beCarneIdentidad obeCarneIdentidad = new beCarneIdentidad();
                            obeCarneIdentidad.CarneIdentidadid = (datosSave.CarneIdentidad != null ? datosSave.CarneIdentidad.CarneIdentidadid : short.Parse(ViewState["carneId"].ToString()));
                            obeCarneIdentidad.Periodo = DateTime.Now.Year;
                            if (txtMesaPartes.Text.Equals("")) { obeCarneIdentidad.IdentMesaPartes = null; }
                            else { obeCarneIdentidad.IdentMesaPartes = txtMesaPartes.Text.ToUpper().Trim(); }
                            obeCarneIdentidad.CalidadMigratoriaid = short.Parse(ddlCalidadMigratoriaPri.SelectedValue);
                            obeCarneIdentidad.CalidadMigratoriaSecId = short.Parse(ddlCalidadMigratoriaSec.SelectedValue);
                            obeCarneIdentidad.OficinaConsularExid = short.Parse(ddlMision.SelectedValue);
                            obeCarneIdentidad.RutaArchivoFoto = rutaRelativa;
                            obeCarneIdentidad.RutaArchivoFirma = rutaRelativaFirma;
                            obeCarneIdentidad.Renovar = renovar;
                            obeCarneIdentidad.FlagRegistroCompleto = true;
                            obeCarneIdentidad.Estadoid = estadoReg;
                            obeCarneIdentidad.Usuariomodificacion = usuarioId;
                            obeCarneIdentidad.Ipmodificacion = ipCreacion;
                            obeCarneIdentidad.Personaid = (datosSave.Persona != null ? datosSave.Persona.Personaid : 0);
                            obeCarneIdentidadPrincipal.CarneIdentidad = obeCarneIdentidad;
                            // HISTORICO DE PERSONA - NUEVO
                            bePersonaHistorico obePersonaHistorico = new bePersonaHistorico();
                            obePersonaHistorico.Personaid = (datosSave.Persona != null ? datosSave.Persona.Personaid : 0);//datosSave.Persona.Personaid;
                            obePersonaHistorico.ApellidoPaterno = txtApePat.Text.Trim().ToUpper();
                            obePersonaHistorico.ApellidoMaterno = txtApeMat.Text.Trim().ToUpper();
                            obePersonaHistorico.Nombres = txtNombres.Text.Trim().ToUpper();
                            obePersonaHistorico.EstadoCivilid = short.Parse(ddlEstadoCivil.SelectedValue);
                            obePersonaHistorico.Generoid = short.Parse(ddlSexo.SelectedValue);
                            obePersonaHistorico.FechaNacimiento = DateTime.Parse(txtFechaNac.Text);
                            obePersonaHistorico.PaisNacionalidadid = short.Parse(ddlNacionalidad.SelectedValue);
                            obePersonaHistorico.Usuariocreacion = usuarioId;
                            obePersonaHistorico.Ipcreacion = ipCreacion;
                            obePersonaHistorico.MenorEdad = menorEdad;
                            obeCarneIdentidadPrincipal.PersonaHistorico = obePersonaHistorico;
                            // HISTORICO CARNE IDENTIDAD - NUEVO
                            beCarneIdentidadHistorico obeCarneIdentidadHistorico = new beCarneIdentidadHistorico();
                            obeCarneIdentidadHistorico.CarneIdentidadid = (datosSave.CarneIdentidad != null ? datosSave.CarneIdentidad.CarneIdentidadid : short.Parse(ViewState["carneId"].ToString()));//datosSave.CarneIdentidad.CarneIdentidadid;
                            obeCarneIdentidadHistorico.CalidadMigratoriaid = short.Parse(ddlCalidadMigratoriaPri.SelectedValue);
                            obeCarneIdentidadHistorico.CalidadMigratoriaSecId = short.Parse(ddlCalidadMigratoriaSec.SelectedValue);
                            obeCarneIdentidadHistorico.RutaArchivoFoto = rutaRelativa;
                            obeCarneIdentidadHistorico.OficinaConsularExid = short.Parse(ddlMision.SelectedValue);
                            obeCarneIdentidadHistorico.EstadoId = estadoReg;
                            obeCarneIdentidadHistorico.Usuariocreacion = usuarioId;
                            obeCarneIdentidadHistorico.Ipcreacion = ipCreacion;
                            obeCarneIdentidadHistorico.RutaArchivoFirma = rutaRelativaFirma;
                            obeCarneIdentidadPrincipal.CarneIdentidadHistorico = obeCarneIdentidadHistorico;
                            // MOVIMIENTO CARNE IDENTIDAD - NUEVO
                            beMovimientoCarneIdentidad obeMovimientoCarneIdentidad = new beMovimientoCarneIdentidad();
                            obeMovimientoCarneIdentidad.CarneIdentidadid = (datosSave.CarneIdentidad != null ? datosSave.CarneIdentidad.CarneIdentidadid : short.Parse(ViewState["carneId"].ToString()));//datosSave.CarneIdentidad.CarneIdentidadid;
                            obeMovimientoCarneIdentidad.Estadoid = estadoReg;
                            obeMovimientoCarneIdentidad.Oficinaconsularid = oficinaId;
                            obeMovimientoCarneIdentidad.ObservacionTipo = tipoObs;
                            obeMovimientoCarneIdentidad.ObservacionDetalle = obsDet;
                            obeMovimientoCarneIdentidad.Usuariocreacion = usuarioId;
                            obeMovimientoCarneIdentidad.Ipcreacion = ipCreacion;
                            obeCarneIdentidadPrincipal.MovimientoCarne = obeMovimientoCarneIdentidad;
                            // REGISTRO EN LINEA
                            short estadoIdRegLinea = -1;
                            beGenerales obeGenerales = new beGenerales();
                            if (Session["Generales"] != null)
                            {
                                obeGenerales = (beGenerales)Session["Generales"];
                            }
                            if (obeRegistroLinea != null)
                            {
                                if (obeRegistroLinea.RegistroLineaId <= 0)
                                {
                                    if (datosSave.RegistroLinea != null)
                                    {
                                        int pos = obeGenerales.ListaEstadosRegLinea.FindIndex(x => x.DescripcionCorta.Equals("APROBADO"));
                                        if (pos != -1) { estadoIdRegLinea = obeGenerales.ListaEstadosRegLinea[pos].Estadoid; }
                                        obeRegistroLinea.RegistroLineaId = datosSave.RegistroLinea.RegistroLineaId;
                                        obeRegistroLinea.EstadoId = estadoIdRegLinea;
                                    }
                                }
                                obeRegistroLinea.UsuarioModificacion = usuarioId;
                                obeRegistroLinea.IpModificacion = ipCreacion;
                            }
                            else
                            {
                                if (datosSave.RegistroLinea != null)
                                {
                                    if(datosSave.RegistroLinea.RegistroLineaId > 0)
                                    {
                                        int pos = obeGenerales.ListaEstadosRegLinea.FindIndex(x => x.DescripcionCorta.Equals("ENVIADO"));
                                        if (pos != -1) { estadoIdRegLinea = obeGenerales.ListaEstadosRegLinea[pos].Estadoid; }
                                        obeRegistroLinea = new beRegistroLinea();
                                        obeRegistroLinea.RegistroLineaId = datosSave.RegistroLinea.RegistroLineaId;
                                        obeRegistroLinea.EstadoId = estadoIdRegLinea;
                                        obeRegistroLinea.UsuarioModificacion = usuarioId;
                                        obeRegistroLinea.IpModificacion = ipCreacion;
                                    }
                                }
                            }
                            //if (obeRegistroLinea.RegistroLineaId > 0)
                            //{
                            //    if (datosSave.RegistroLinea.RegistroLineaId > 0)
                            //    {
                            //        obeRegistroLinea.RegistroLineaId = datosSave.RegistroLinea.RegistroLineaId;
                            //    }
                            //}
                            
                            obeCarneIdentidadPrincipal.RegistroLinea = obeRegistroLinea;
                            // RELACION DEPENDENCIA
                            if (ddlTitDep.SelectedItem.Text.Equals("TITULAR")) 
                            {
                                if (ViewState["carneId"] != null)
                                {
                                    if (RelacionDependenciaGen != null)
                                    {
                                        RelacionDependenciaGen.CarneIdentidadTitId = short.Parse(ViewState["carneId"].ToString());
                                    }
                                }
                            }
                            obeCarneIdentidadPrincipal.RelacionDependencia = RelacionDependenciaGen; //datosSave.RelacionDependencia;
                            //if (titularRelDep != null)
                            //{
                            //    if (obeCarneIdentidadPrincipal.RelacionDependencia != null)
                            //    {
                            //        obeCarneIdentidadPrincipal.RelacionDependencia.CarneIdentidadTitId = titularRelDep.CarneIdentidadid;
                            //        obeCarneIdentidadPrincipal.RelacionDependencia.UsuarioModificacion = usuarioId;
                            //        obeCarneIdentidadPrincipal.RelacionDependencia.IpModificacion = ipCreacion;
                            //    }
                            //    else
                            //    {
                            //        beCarneIdentidadRelacionDependencia obeCarneIdentidadRelacionDependencia = new beCarneIdentidadRelacionDependencia();
                            //        obeCarneIdentidadRelacionDependencia.CarneIdentidadTitId = titularRelDep.CarneIdentidadid;
                            //        obeCarneIdentidadRelacionDependencia.CarneIdentidadDepId = datosSave.CarneIdentidad.CarneIdentidadid;
                            //        obeCarneIdentidadRelacionDependencia.UsuarioCreacion = usuarioId;
                            //        obeCarneIdentidadRelacionDependencia.IpCreacion = ipCreacion;
                            //        obeCarneIdentidadPrincipal.RelacionDependencia = obeCarneIdentidadRelacionDependencia;
                            //    }
                            //}
                            //else
                            //{
                            //    beCarneIdentidadRelacionDependencia obeCarneIdentidadRelacionDependencia = new beCarneIdentidadRelacionDependencia();
                            //    obeCarneIdentidadRelacionDependencia.TitularDependienteId = datosSave.RelacionDependencia.TitularDependienteId;
                            //    obeCarneIdentidadRelacionDependencia.CarneIdentidadTitId = datosSave.CarneIdentidad.CarneIdentidadid;
                            //    obeCarneIdentidadRelacionDependencia.CarneIdentidadDepId = datosSave.CarneIdentidad.CarneIdentidadid;
                            //    obeCarneIdentidadRelacionDependencia.UsuarioModificacion = usuarioId;
                            //    obeCarneIdentidadRelacionDependencia.IpModificacion = ipCreacion;
                            //    obeCarneIdentidadPrincipal.RelacionDependencia = obeCarneIdentidadRelacionDependencia;
                            //}
                            
                            brCarneIdentidadPrincipal obrCarneIdentidadPrincipal = new brCarneIdentidadPrincipal();
                            bool exito = false;
                            int CarneIdentidadId = -1;
                            if (tipoActualizacion)
                            {
                                //if (ViewState["estado"]!=null && ViewState["estado"].ToString().Equals("OBSERVADO"))
                                //{   //se actualiza el estado observado a (cabecera=enviado, en detalle=aprobado
                                //    obrCarneIdentidadPrincipal.subsanarObservacion(obeRegistroLinea);
                                //}
                                exito = obrCarneIdentidadPrincipal.actualizar(obeCarneIdentidadPrincipal);
                                if (exito)
                                {
                                    DisableControls(tablaMesaPartes, false);
                                    DisableControls(tablaFuncionario, false);
                                    DisableControls(tablaCalMig, false);
                                    DisableControls(tablaMision, false);
                                    DisableControls(tablaFotografia, false);
                                    DisableControls(tablaFirma, false);
                                    btnCancelar.Text = "Volver";
                                    btnCancelar.CssClass = "ImagenBotonVolver";
                                    btnGuardarEdicion.Enabled = false;
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE ACTUALIZÓ LA INFORMACIÓN CORRECTAMENTE');", true);
                                }
                                else
                                {
                                    if (exitoFotografia)
                                    {
                                        File.Delete(rutaAdjuntos + fileName);
                                        File.Delete(rutaAdjuntos + fileNameFirma);
                                    }
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "habilitarControles", "disIntablaFuncionario(false);", true);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR AL ACTUALIZAR LA INFORMACIÓN.');", true);
                                }
                            }
                            else
                            {
                                CarneIdentidadId = obrCarneIdentidadPrincipal.actualizarRegistroCompleto(obeCarneIdentidadPrincipal);
                                if (CarneIdentidadId > 0)
                                {
                                    DisableControls(tablaMesaPartes, false);
                                    DisableControls(tablaFuncionario, false);
                                    DisableControls(tablaCalMig, false);
                                    DisableControls(tablaMision, false);
                                    DisableControls(tablaFotografia, false);
                                    DisableControls(tablaFirma, false);
                                    btnCancelar.Text = "Volver";
                                    btnCancelar.CssClass = "ImagenBotonVolver";
                                    btnGuardarEdicion.Enabled = false;
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE ACTUALIZÓ LA INFORMACIÓN CORRECTAMENTE');", true);
                                }
                                else
                                {
                                    File.Delete(rutaAdjuntos + fileName);
                                    if (!menorEdad) { File.Delete(rutaAdjuntos + fileNameFirma); }
                                    //calidadHumanitaria(sender, e);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "habilitarControles", "disIntablaFuncionario(false);", true);
                                    if (CarneIdentidadId == -1) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR AL GUARDAR LA INFORMACIÓN.');", true); }
                                    if (CarneIdentidadId == -2) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('YA EXISTE UNA PERSONA CON ESE TIPO Y NUMERO DE IDENTIFICACION.');", true); ddlDocumentoIdent.Focus(); }
                                }
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "habilitarControles", "disIntablaFuncionario(false);", true);
                        }
                    }
                    else
                    {
                        if (!menorEdad) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('NO SE SELECCIONO UN ARCHIVO DE FIRMA');", true); }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "habilitarControles", "disIntablaFuncionario(false);", true);
                    }
                }
                else
                {
                    Session["mensaje"] = "OCURRIÓ UN ERROR";
                    obrGeneral.grabarError("Error en la evaluacion de los controles");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensajeError", "location.href='../../mensajes.aspx';", true);
                }
            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "habilitarControles", "disIntablaFuncionario(false);", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR.');", true);
            }
        }

        //protected void guardarImagenes()
        //{
        //    try
        //    {
        //        // ADJUNTAR ARCHIVO
        //        string rutaAdjuntos = obrGeneral.rutaAdjuntos + @"\" + DateTime.Now.Year.ToString() + @"\" + DateTime.Now.Month.ToString("D2") + @"\";
        //        // FOTO
        //        string fileName = "F1" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2") + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2") + DateTime.Now.Millisecond.ToString() + obrGeneral.fileExtFotografia;
        //        bool exitoFotografia = guardarImagen(rutaAdjuntos, fileName);
        //        // FIRMA
        //        string fileNameFirma = "";
        //        bool exitoFirma = false;
        //        if (menorEdad)
        //        {
        //            exitoFirma = true;
        //        }
        //        else
        //        {
        //            fileNameFirma = "F2" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2") + DateTime.Now.Hour.ToString("D2") + DateTime.Now.Minute.ToString("D2") + DateTime.Now.Second.ToString("D2") + DateTime.Now.Millisecond.ToString() + obrGeneral.fileExtFotografia;
        //            exitoFirma = guardarImagenFirma(rutaAdjuntos, fileNameFirma);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        obrGeneral.grabarLog(ex);
        //    }
        //}

        #endregion
        #region IMAGEN (FOTOGRAFIA)
        protected void cargaImagen() // CARGA LA IMAGEN DE FOTOGRAFIA EN MEMORIA Y LA MUESTRA EN PANTALLA
        {
            try
            {
                if (fulImagen.HasFile)
                {
                    FileInfo fi = new FileInfo(fulImagen.FileName);
                    //VALIDAR ARCHIVO ---------------------
                    bool exito = false;
                    bool exitoExtension = false;
                    bool exitoPeso = false;
                    exito = (oCodigoUsuario.validarExtensionArchivo(obrGeneral.fileExtFotografia, fi) ? exitoExtension = true : exitoExtension = false);
                    exito = (oCodigoUsuario.validarPesoArchivo(obrGeneral.filePesoFotografia.ToString(), fulImagen) ? exitoPeso = true : exitoPeso = false);
                    //---------------------------------------
                    if (exitoExtension & exitoPeso)
                    {
                        lblArchivoNombre.Text = fulImagen.FileName;
                        lblArchivoPeso.Text = Math.Round((fulImagen.PostedFile.ContentLength / 1024f) / 1024f, 2).ToString() + " MB";
                        lblArchivoExtension.Text = fi.Extension;
                        Byte[] imageByte = null;
                        imageByte = new Byte[fulImagen.PostedFile.ContentLength];
                        HttpPostedFile ImgFile = fulImagen.PostedFile;

                        using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
                        {
                            imageByte = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                        }
                        string base64String = Convert.ToBase64String(imageByte, 0, imageByte.Length);

                        lblArchivoNombre.ForeColor = Color.Green;
                        lblArchivoExtension.ForeColor = Color.Green;
                        lblArchivoPeso.ForeColor = Color.Green;
                        Session["fileUploadImage"] = fulImagen;
                        Session["tempImagenFotografiaNueva"] = base64String;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ibtVerImagen", "window.open('../VerImagenNueva.aspx','_blank','width=600,height=750,toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1','_blank');", true);
                    }
                    else
                    {
                        Session["fileUploadImage"] = null;
                        Session["tempImagenFotografiaNueva"] = null;
                        lblArchivoNombre.ForeColor = Color.Red;
                        lblArchivoExtension.ForeColor = Color.Red;
                        lblArchivoPeso.ForeColor = Color.Red;
                        if (!exitoExtension)
                        {
                            lblArchivoNombre.Text = "Tipo de archivo no valido. Seleccione un archivo de tipo JPEG.";
                            lblArchivoExtension.Text = "Archivo no valido.";
                            lblArchivoPeso.Text = "Archivo no valido.";
                        }
                        if (!exitoPeso)
                        {
                            lblArchivoNombre.Text = "Peso de archivo no valido. Seleccione un archivo NO mayor a 1 MB.";
                            lblArchivoExtension.Text = "Archivo no valido.";
                            lblArchivoPeso.Text = "Archivo no valido.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
                obrGeneral.grabarLog(ex);
            }
        }

        protected void cargaImagenFirma() // CARGA LA IMAGEN DE FIRMA EN MEMORIA Y LA MUESTRA EN PANTALLA
        {
            try
            {
                if (fulImagenFirma.HasFile)
                {
                    FileInfo fi = new FileInfo(fulImagenFirma.FileName);
                    //VALIDAR ARCHIVO ---------------------
                    bool exito = false;
                    bool exitoExtension = false;
                    bool exitoPeso = false;
                    exito = (oCodigoUsuario.validarExtensionArchivo(obrGeneral.fileExtFotografia, fi) ? exitoExtension = true : exitoExtension = false);
                    exito = (oCodigoUsuario.validarPesoArchivo(obrGeneral.filePesoFotografia.ToString(), fulImagenFirma) ? exitoPeso = true : exitoPeso = false);
                    //---------------------------------------
                    if (exitoExtension & exitoPeso)
                    {
                        lblArchivoNombreFirma.Text = fulImagenFirma.FileName;
                        lblArchivoPesoFirma.Text = Math.Round((fulImagenFirma.PostedFile.ContentLength / 1024f) / 1024f, 2).ToString() + " MB";
                        lblArchivoExtensionFirma.Text = fi.Extension;
                        Byte[] imageByte = null;
                        imageByte = new Byte[fulImagenFirma.PostedFile.ContentLength];
                        HttpPostedFile ImgFile = fulImagenFirma.PostedFile;

                        using (var binaryReader = new BinaryReader(Request.Files[1].InputStream))
                        {
                            imageByte = binaryReader.ReadBytes(Request.Files[1].ContentLength);
                        }
                        string base64String = Convert.ToBase64String(imageByte, 0, imageByte.Length);

                        lblArchivoNombreFirma.ForeColor = Color.Green;
                        lblArchivoExtensionFirma.ForeColor = Color.Green;
                        lblArchivoPesoFirma.ForeColor = Color.Green;
                        Session["fileUploadImageFirma"] = fulImagenFirma;
                        Session["tempImagenFirmaNueva"] = base64String;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ibtVerFirma", "window.open('../VerFirmaNueva.aspx','_blank','width=450,height=400,toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1','_blank');", true);
                    }
                    else
                    {
                        Session["fileUploadImageFirma"] = null;
                        Session["tempImagenFirmaNueva"] = null;
                        lblArchivoNombreFirma.ForeColor = Color.Red;
                        lblArchivoExtensionFirma.ForeColor = Color.Red;
                        lblArchivoPesoFirma.ForeColor = Color.Red;
                        if (!exitoExtension)
                        {
                            lblArchivoNombreFirma.Text = "Tipo de archivo no valido. Seleccione un archivo de tipo JPEG.";
                            lblArchivoExtensionFirma.Text = "Archivo no valido.";
                            lblArchivoPesoFirma.Text = "Archivo no valido.";
                        }
                        if (!exitoPeso)
                        {
                            lblArchivoNombreFirma.Text = "Peso de archivo no valido. Seleccione un archivo NO mayor a 1 MB.";
                            lblArchivoExtensionFirma.Text = "Archivo no valido.";
                            lblArchivoPesoFirma.Text = "Archivo no valido.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
                obrGeneral.grabarLog(ex);
            }
        }

        private bool guardarImagen(string SavePath, string fileName) // GUARDA LA IMAGEN DE FOTOGRAFIA
        {
            bool exito = false;
            try
            {
                exito = oCodigoUsuario.createDirectory(SavePath);
                if (exito)
                {
                    if (Session["tempImagenFotografiaNueva"] != null)
                    {
                        // CONVIERTE A STREAM -------------------------------------------------------
                        string base64String = (string)Session["tempImagenFotografiaNueva"];
                        byte[] imgByte = Convert.FromBase64String(base64String);
                        // VALIDA EL TIPO DE ARCHIVO ------------------------------------------------
                        if (Session["fileUploadImage"] != null)
                        {
                            FileUpload fileUpLoadImage = (FileUpload)Session["fileUploadImage"];
                            Regex reg = new Regex(@"(?i).*\.(gif|jpe?g|png|tif)$");
                            string uFile = fileUpLoadImage.FileName;
                            if (reg.IsMatch(uFile))
                            {
                                using (MemoryStream ms = new MemoryStream(imgByte))
                                {
                                    Bitmap b = (Bitmap)Bitmap.FromStream(ms);
                                    b.Save(SavePath + fileName, ImageFormat.Jpeg);
                                    ms.Close();
                                    if (File.Exists(SavePath + fileName)) { exito = true; }
                                    else { exito = false; }
                                }
                            }
                            else
                            {
                                exito = false;
                            }
                        }
                        else
                        {
                            exito = false;
                        }
                    }
                    else
                    {
                        exito = false;
                    }
                }
            }
            catch (Exception ex)
            {
                exito = false;
                obrGeneral.grabarLog(ex);
            }
            return (exito);
        }

        private bool guardarImagenFirma(string SavePath, string fileName) // GUARDA LA IMAGEN DE FIRMA
        {
            bool exito = false;
            try
            {
                exito = oCodigoUsuario.createDirectory(SavePath);
                if (exito)
                {
                    if (Session["tempImagenFirmaNueva"] != null)
                    {
                        // CONVIERTE A STREAM -------------------------------------------------------
                        string base64String = (string)Session["tempImagenFirmaNueva"];
                        byte[] imgByte = Convert.FromBase64String(base64String);
                        // VALIDA EL TIPO DE ARCHIVO ------------------------------------------------
                        if (Session["fileUploadImageFirma"] != null)
                        {
                            FileUpload fileUpLoadImage = (FileUpload)Session["fileUploadImageFirma"];
                            Regex reg = new Regex(@"(?i).*\.(gif|jpe?g|png|tif)$");
                            string uFile = fileUpLoadImage.FileName;
                            if (reg.IsMatch(uFile))
                            {
                                using (MemoryStream ms = new MemoryStream(imgByte))
                                {
                                    Bitmap b = (Bitmap)Bitmap.FromStream(ms);
                                    b.Save(SavePath + fileName, ImageFormat.Jpeg);
                                    ms.Close();
                                    if (File.Exists(SavePath + fileName)) { exito = true; }
                                    else { exito = false; }
                                }
                            }
                            else
                            {
                                exito = false;
                            }
                        }
                        else
                        {
                            exito = false;
                        }
                    }
                    else
                    {
                        exito = false;
                    }
                }
            }
            catch (Exception ex)
            {
                exito = false;
                obrGeneral.grabarLog(ex);
            }
            return (exito);
        }

        protected void cargarImagenAlmacenada(string Path) // CARGA LA IMAGEN DE FOTOGRAFIA DE UN REGISTRO YA CREADO, LA ALMACENA EN MEMORIA Y LA MUESTRA
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
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }
                
        protected void cargarImagenAlmacenadaFirma(string Path) // CARGA LA IMAGEN DE FIRMAD DE UN REGISTRO YA CREADO, LA ALMACENA EN MEMORIA Y LA MUESTRA
        {
            try
            {
                if (File.Exists(Path))
                {
                    Byte[] imageByte = null;
                    imageByte = File.ReadAllBytes(Path);
                    string base64String = Convert.ToBase64String(imageByte, 0, imageByte.Length);
                    Session["tempImagenFirmaSave"] = base64String;
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }
        #endregion

        protected void limpiarControles(object sender, EventArgs e) // LIMPIA LOS CONTROLES DEL FORMULARIO
        {
            Response.Redirect("~/Paginas/Registrador/RegistradorCarneRegistro.aspx");
        }

        protected void DisableControls(Control parent, bool State) // BLOQUEA LOS CONTROLES DE UN AREA ESPECIFICA EN EL FORMULARIO
        {
            foreach (Control c in parent.Controls)
            {
                if (c is DropDownList)
                {
                    ((DropDownList)(c)).Enabled = State;
                }
                if (c is TextBox)
                {
                    ((TextBox)(c)).Enabled = State;
                }
                if (c is ImageButton)
                {
                    ((ImageButton)(c)).Enabled = State;
                }
                if (c is FileUpload)
                {
                    ((FileUpload)(c)).Enabled = State;
                }
                DisableControls(c, State);
            }
            btnGuardar.Enabled = false;
        }

        protected void cancelarEdicion(object sender, EventArgs e) // DEVUELVE AL FORMULARIO ANTERIOR SIN GUARDAR CAMBIOS
        {
            try
            {
                string pag = oUIEncriptador.EncriptarCadena(ViewState["pag"].ToString());
                Response.Redirect(String.Format("RegistradorConsultaCarne.aspx?pag={0}", pag),false);
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

        protected short obtenerTipoObs(string claveObs) // OBTIENE EL ID DE UNA LISTA DE ACUERDO A UNA CLAVE
        {
            int pos = ListaTipoObs.FindIndex(x => x.Descripcion.Equals(claveObs));
            short estadoObs = (pos != -1 ? ListaTipoObs[pos].Parametroid : short.Parse("0"));
            return (estadoObs);
        }

        protected bool evaluarControles() // METODO DE SEGURIDAD PARA EVITAR LA INCLUSION DE CARACTERES NO VALIDOS
        {
            bool exitoEvaluar = false;
            // EVALUA ANTIXSS --------------------------------------------------
            // TEXTOS
            string MesaP = (!txtMesaPartes.Text.Equals("") ? oCodigoUsuario.evaluateAntiXSS(txtMesaPartes.Text.Trim().ToUpper()) : "");
            string ApePat = oCodigoUsuario.evaluateAntiXSS(txtApePat.Text.Trim().ToUpper());
            string ApeMat = (!txtApeMat.Text.Equals("") ? oCodigoUsuario.evaluateAntiXSS(txtApeMat.Text.Trim().ToUpper()) : "");
            string Nombres = oCodigoUsuario.evaluateAntiXSS(txtNombres.Text.Trim().ToUpper());
            string FechaNac = oCodigoUsuario.evaluateAntiXSS(txtFechaNac.Text);
            string NumDocIdent = oCodigoUsuario.evaluateAntiXSS(txtNumeroDocIdent.Text.Trim().ToUpper());
            string Telefono = oCodigoUsuario.evaluateAntiXSS(txtTelefono.Text);
            string Domicilio = oCodigoUsuario.evaluateAntiXSS(txtDomicilio.Text.Trim().ToUpper());
            // DROPDOWN
            string EstCiv = oCodigoUsuario.evaluateAntiXSS(ddlEstadoCivil.SelectedValue);
            string Sexo = oCodigoUsuario.evaluateAntiXSS(ddlSexo.SelectedValue);
            string DocIdent = oCodigoUsuario.evaluateAntiXSS(ddlDocumentoIdent.SelectedValue);
            string Pais = oCodigoUsuario.evaluateAntiXSS(ddlNacionalidad.SelectedValue);
            string Dep = oCodigoUsuario.evaluateAntiXSS(ddlDepartamento.SelectedValue);
            string Prov = oCodigoUsuario.evaluateAntiXSS(ddlProvincia.SelectedValue);
            string Distr = oCodigoUsuario.evaluateAntiXSS(ddlDistrito.SelectedValue);
            string CalMig = oCodigoUsuario.evaluateAntiXSS(ddlCalidadMigratoriaPri.SelectedValue);
            string Cargo = oCodigoUsuario.evaluateAntiXSS(ddlCalidadMigratoriaSec.SelectedValue);
            string Mision = oCodigoUsuario.evaluateAntiXSS(ddlMision.SelectedValue);
            // EVALUA CARACTERES ------------------------------------------------
            // TEXTOS
            exitoEvaluar = (!MesaP.Equals("") ? oCodigoUsuario.evaluarAlfaNumSim(MesaP) : true);
            //if (exitoEvaluar) { exitoEvaluar = oCodigoUsuario.evaluarLetras(ApePat); }
            //if (exitoEvaluar) { exitoEvaluar = (!ApeMat.Equals("") ? oCodigoUsuario.evaluarLetras(ApeMat) : true); }
            //if (exitoEvaluar) { exitoEvaluar = oCodigoUsuario.evaluarLetras(Nombres); }
            if (exitoEvaluar) { exitoEvaluar = oCodigoUsuario.evaluarFecha(FechaNac); }
            if (exitoEvaluar) { exitoEvaluar = oCodigoUsuario.evaluarAlfaNumSim(NumDocIdent); }
            if (exitoEvaluar) { exitoEvaluar = oCodigoUsuario.evaluarNumeros(Telefono); }
            if (exitoEvaluar) { exitoEvaluar = oCodigoUsuario.evaluarAlfaNum(Domicilio); }
            // DROPDOWN
            if (exitoEvaluar) { exitoEvaluar = oCodigoUsuario.evaluarNumeros(EstCiv); }
            if (exitoEvaluar) { exitoEvaluar = oCodigoUsuario.evaluarNumeros(Sexo); }
            if (exitoEvaluar) { exitoEvaluar = oCodigoUsuario.evaluarNumeros(DocIdent); }
            if (exitoEvaluar) { exitoEvaluar = oCodigoUsuario.evaluarNumeros(Pais); }
            if (exitoEvaluar) { exitoEvaluar = oCodigoUsuario.evaluarNumeros(Dep); }
            if (exitoEvaluar) { exitoEvaluar = oCodigoUsuario.evaluarNumeros(Prov); }
            if (exitoEvaluar) { exitoEvaluar = oCodigoUsuario.evaluarNumeros(Distr); }
            if (exitoEvaluar) { exitoEvaluar = oCodigoUsuario.evaluarNumeros(CalMig); }
            if (exitoEvaluar) { exitoEvaluar = oCodigoUsuario.evaluarNumeros(Cargo); }
            if (exitoEvaluar) { exitoEvaluar = oCodigoUsuario.evaluarNumeros(Mision); }

            return (exitoEvaluar);
        }

        protected void calidadHumanitaria(object sender, EventArgs e) // METODO PARTICULAR PARA LA SELECCION DE UNA CALIDAD MIGRATORIA DE TIPO HUMANITARIA
        {
            try
            {
                if (Session["Generales"] != null)
                {
                    beGenerales obeGenerales = new beGenerales();
                    obeGenerales = (beGenerales)Session["Generales"];
                    if (ddlCalidadMigratoriaPri.SelectedItem.Text.Equals("HUMANITARIA"))
                    {
                        int posCantegoria = obeGenerales.CategoriaOficinaExtranjera.FindIndex(x => x.Descripcion.Equals("OTROS"));
                        ddlCategoriaOfcoEx.SelectedValue = obeGenerales.CategoriaOficinaExtranjera[posCantegoria].Parametroid.ToString();
                        seleccionarCategoriaOficina(sender, e);
                        int posOfcoEx = obeGenerales.ListaOficinasConsularesExtranjeras.FindIndex(x => x.Nombre.Equals("-----"));
                        ddlMision.SelectedValue = obeGenerales.ListaOficinasConsularesExtranjeras[posOfcoEx].OficinaconsularExtranjeraid.ToString();
                        ddlCategoriaOfcoEx.Enabled = false;
                        ddlMision.Enabled = false;
                    }
                    else
                    {
                        //ddlCategoriaOfcoEx.SelectedValue = "0";
                        ddlCategoriaOfcoEx.Enabled = true;
                        //cargarComboNull(ddlMision);
                        ddlMision.Enabled = true;
                    }
                }
            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        #region Registro en Linea
        private void buscarRegistroLinea()
        {
            try
            {
                ImgDescargarResumenLinea.Visible = false;
                string regLineaNumero = txtRegLinea.Text.Trim();
                beRegistroLinea parametros = new beRegistroLinea();
                parametros.NumeroRegLinea = regLineaNumero;
                brRegistroLinea obrRegistroLinea = new brRegistroLinea();
                obeRegistroLinea = obrRegistroLinea.consultarRegistro(parametros);
                if (obeRegistroLinea != null)
                {
                    if (obeRegistroLinea.RegistroLineaId > 0)
                    {
                        if (Session["Generales"] != null)
                        {
                            short estadoIdRegLinea = -1;
                            string estado = "error";
                            beGenerales obeGenerales = new beGenerales();
                            obeGenerales = (beGenerales)Session["Generales"];
                            int posEstado = obeGenerales.ListaEstadosRegLinea.FindIndex(x => x.DescripcionCorta.Equals("ENVIADO"));
                            if (posEstado != -1)
                            {
                                estado = obeGenerales.ListaEstadosRegLinea[posEstado].DescripcionCorta;
                                if (estado == obeRegistroLinea.ConEstadoDesc || obeRegistroLinea.ConEstadoDesc=="OBSERVADO")
                                {
                                    int pos = obeGenerales.ListaEstadosRegLinea.FindIndex(x => x.DescripcionCorta.Equals("APROBADO"));
                                    if (pos != -1)
                                    {
                                        estadoIdRegLinea = obeGenerales.ListaEstadosRegLinea[pos].Estadoid;
                                        obeRegistroLinea.EstadoId = estadoIdRegLinea;
                                        llenadoCamposRegLinea(true, obeRegistroLinea, null, null);
                                        lblRegLineaMensaje.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0055aa");
                                        hRutaResumenLinea.Value = obeRegistroLinea.DpRutaResumen;
                                        lblRegLineaMensaje.Text = "Registro en Linea ENCONTRADO";
                                        ibtBuscarRegLinea.Enabled = false;
                                        ImgDescargarResumenLinea.Visible = true;
                                    }
                                }
                                else
                                {
                                    obeRegistroLinea = null;
                                    //llenadoCamposRegLinea(false, obeRegistroLinea, sender, e);
                                    lblRegLineaMensaje.ForeColor = Color.Red;
                                    lblRegLineaMensaje.Text = "El registro en linea no esta disponible";
                                }
                            }
                        }
                    }
                    else
                    {
                        obeRegistroLinea = null;
                        //llenadoCamposRegLinea(false, obeRegistroLinea, sender, e);
                        lblRegLineaMensaje.ForeColor = Color.Red;
                        lblRegLineaMensaje.Text = "No se encontro el registro en linea";
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR AL BUSCAR EL REGISTRO EN LINEA');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
            }
        }

        protected void buscarRegLinea(object sender, EventArgs e)
        {
            try
            {
                ImgDescargarResumenLinea.Visible = false;
                string regLineaNumero = txtRegLinea.Text.Trim();
                beRegistroLinea parametros = new beRegistroLinea();
                parametros.NumeroRegLinea = regLineaNumero;
                brRegistroLinea obrRegistroLinea = new brRegistroLinea();
                obeRegistroLinea = obrRegistroLinea.consultarRegistro(parametros);
                if (obeRegistroLinea != null)
                {
                    if (obeRegistroLinea.RegistroLineaId > 0)
                    {
                        if (obeRegistroLinea.ConTipoEmision != "NUEVO")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SOLO SE PUEDE CONSULTAR SOLICITUDES DE EMISIÓN: NUEVA');", true);
                            return;
                        }
                        if (Session["Generales"] != null)
                        {
                            short estadoIdRegLinea = -1;
                            string estado = "error";
                            beGenerales obeGenerales = new beGenerales();
                            obeGenerales = (beGenerales)Session["Generales"];
                            int posEstado = obeGenerales.ListaEstadosRegLinea.FindIndex(x => x.DescripcionCorta.Equals("ENVIADO"));
                            if (posEstado != -1)
                            {
                                estado = obeGenerales.ListaEstadosRegLinea[posEstado].DescripcionCorta;
                                if (estado == obeRegistroLinea.ConEstadoDesc)
                                {
                                    int pos = obeGenerales.ListaEstadosRegLinea.FindIndex(x => x.DescripcionCorta.Equals("APROBADO"));
                                    if (pos != -1)
                                    {
                                        estadoIdRegLinea = obeGenerales.ListaEstadosRegLinea[pos].Estadoid;
                                        obeRegistroLinea.EstadoId = estadoIdRegLinea;
                                        llenadoCamposRegLinea(true, obeRegistroLinea, sender, e);
                                        lblRegLineaMensaje.ForeColor = System.Drawing.ColorTranslator.FromHtml("#0055aa");
                                        hRutaResumenLinea.Value = obeRegistroLinea.DpRutaResumen;
                                        lblRegLineaMensaje.Text = "Registro en Linea ENCONTRADO";
                                        ibtBuscarRegLinea.Enabled = false;
                                        ImgDescargarResumenLinea.Visible = true;
                                    }
                                }
                                else
                                {
                                    obeRegistroLinea = null;
                                    //llenadoCamposRegLinea(false, obeRegistroLinea, sender, e);
                                    lblRegLineaMensaje.ForeColor = Color.Red;
                                    lblRegLineaMensaje.Text = "El registro en linea no esta disponible";
                                }
                            }
                        } 
                    }
                    else
                    {
                        obeRegistroLinea = null;
                        //llenadoCamposRegLinea(false, obeRegistroLinea, sender, e);
                        lblRegLineaMensaje.ForeColor = Color.Red;
                        lblRegLineaMensaje.Text = "No se encontro el registro en linea";
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR AL BUSCAR EL REGISTRO EN LINEA');", true);
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
            }
        }

        protected void llenadoCamposRegLinea(bool llenado, beRegistroLinea registroLinea, object sender, EventArgs e)
        {
            if (llenado)
            {
                styleTextbox(tablaFuncionario, "#CEE3F6");
                SetearColorRegistroLinea(registroLinea);
                txtApePat.Text = registroLinea.DpPrimerApellido;
                txtApeMat.Text = registroLinea.DpSegundoApellido;
                txtNombres.Text = registroLinea.DpNombres;
                txtFechaNac.Text = registroLinea.ConFechaNacimiento;
                ddlSexo.SelectedValue = registroLinea.DpGeneroId.ToString();
                seleccionarSexo(sender, e);
                ddlEstadoCivil.SelectedValue = registroLinea.DpEstadoCivilId.ToString();
                ddlDocumentoIdent.SelectedValue = registroLinea.DpTipoDocIdentidad.ToString();
                txtNumeroDocIdent.Text = registroLinea.DpNumeroDocIdentidad;
                ddlNacionalidad.SelectedValue = registroLinea.DpPaisNacionalidad.ToString();
                obtenerNacionalidadPais(sender, e);
                txtTelefono.Text = registroLinea.DpNumeroTelefono;
                txtDomicilio.Text = registroLinea.DpDomicilioPeru;
                ddlDistrito.SelectedValue = "0";
                separarUbigeo(registroLinea.DpUbigeo, sender, e);
                ddlTitDep.SelectedValue = registroLinea.DpReldepTitdep.ToString();
                ddlCalidadMigratoriaPri.SelectedValue = registroLinea.TIPO_CALIDAD_MIGRATORIA.ToString();
                seleccionarCalidadMigratoria(sender, e);
                ddlCalidadMigratoriaSec.SelectedValue = registroLinea.COD_CARGO.ToString();
                ddlCategoriaOfcoEx.SelectedValue = registroLinea.COD_CATEGORIA_MISION.ToString();
                seleccionarCategoriaOficina(sender, e);
                ddlMision.SelectedValue = registroLinea.COD_INSTITUCION.ToString();
                if (registroLinea.ConNumeroCarne != null)
                {
                    txtRelDep.Text = (registroLinea.ConNumeroCarne != null ? registroLinea.ConNumeroCarne : "");
                }
                else
                {
                    txtRelDep.Text = "";
                }
                buscarRelacionDependencia(sender, e);
                string rutaArchivo = obrGeneral.rutaAdjuntos + registroLinea.DpRutaAdjunto;
                ViewState["rutaArchivo"] = registroLinea.DpRutaAdjunto;
                cargarImagenAlmacenada(rutaArchivo);
                ibtVerImagenAlmacenada.Enabled = true;
                string rutaFirma = obrGeneral.rutaAdjuntos + registroLinea.DpRutaFirma;
                ViewState["rutaArchivoFirma"] = registroLinea.DpRutaFirma;
                cargarImagenAlmacenadaFirma(rutaFirma);
                ibtVerFirmaAlmacenada.Enabled = true;
            }
            else
            {
                txtApePat.Text = "";
                txtApeMat.Text = "";
                txtNombres.Text = "";
                txtFechaNac.Text = "";
                ddlSexo.SelectedValue = "0";
                ddlEstadoCivil.SelectedValue = "0";
                ddlDocumentoIdent.SelectedValue = "0";
                txtNumeroDocIdent.Text = "";
                ddlNacionalidad.SelectedValue = "0";
                txtTelefono.Text = "";
                txtDomicilio.Text = "";
                ddlDepartamento.SelectedValue = "00";
                seleccionarDepartamento(sender,e);
                styleTextbox(tablaFuncionario, "White");
                //separarUbigeo(registroLinea.DpUbigeo, sender, e);
            }
        }
        private void SetearColorRegistroLinea(beRegistroLinea registroLinea)
        {
            if (txtApePat.Text != registroLinea.DpPrimerApellido)
            {
                txtApePat.Style.Add("background-color", "#9FDFC1");
                txtApePat.ToolTip = "REGISTRO ANTERIOR: " + txtApePat.Text;
            }
            if (txtApeMat.Text != registroLinea.DpSegundoApellido)
            {
                txtApeMat.Style.Add("background-color", "#9FDFC1");
                txtApeMat.ToolTip = "REGISTRO ANTERIOR: " + txtApeMat.Text;
            }
            if (txtNombres.Text != registroLinea.DpNombres)
            {
                txtNombres.Style.Add("background-color", "#9FDFC1");
                txtNombres.ToolTip = "REGISTRO ANTERIOR: " + txtNombres.Text;
            }
            if (txtFechaNac.Text != registroLinea.ConFechaNacimiento)
            {
                txtFechaNac.Style.Add("background-color", "#9FDFC1");
                txtFechaNac.ToolTip = "REGISTRO ANTERIOR: " + txtFechaNac.Text;
            }
            if (ddlSexo.SelectedValue != registroLinea.DpGeneroId.ToString())
            {
                ddlSexo.Style.Add("background-color", "#9FDFC1");
                ddlSexo.ToolTip = "REGISTRO ANTERIOR: " + ddlSexo.SelectedItem.Text;
            }
            if (ddlEstadoCivil.SelectedValue != registroLinea.DpEstadoCivilId.ToString())
            {
                ddlEstadoCivil.Style.Add("background-color", "#9FDFC1");
                ddlEstadoCivil.ToolTip = "REGISTRO ANTERIOR: " + ddlEstadoCivil.SelectedItem.Text;
            }
            if (ddlDocumentoIdent.SelectedValue != registroLinea.DpTipoDocIdentidad.ToString())
            {
                ddlDocumentoIdent.Style.Add("background-color", "#9FDFC1");
                ddlDocumentoIdent.ToolTip = "REGISTRO ANTERIOR: " + ddlDocumentoIdent.SelectedItem.Text;
            }
            if (txtNumeroDocIdent.Text != registroLinea.DpNumeroDocIdentidad)
            {
                txtNumeroDocIdent.Style.Add("background-color", "#9FDFC1");
                txtNumeroDocIdent.ToolTip = "REGISTRO ANTERIOR: " + txtNumeroDocIdent.Text;
            }
            if (ddlNacionalidad.SelectedValue != registroLinea.DpPaisNacionalidad.ToString())
            {
                ddlNacionalidad.Style.Add("background-color", "#9FDFC1");
                ddlNacionalidad.ToolTip = "REGISTRO ANTERIOR: " + ddlNacionalidad.SelectedItem.Text;
            }
            if (txtTelefono.Text != registroLinea.DpNumeroTelefono)
            {
                txtTelefono.Style.Add("background-color", "#9FDFC1");
                txtTelefono.ToolTip = "REGISTRO ANTERIOR: " + txtTelefono.Text;
            }
            if (txtDomicilio.Text != registroLinea.DpDomicilioPeru)
            {
                txtDomicilio.Style.Add("background-color", "#9FDFC1");
                txtDomicilio.ToolTip = "REGISTRO ANTERIOR: " + txtDomicilio.Text;
            }


            if (ddlCalidadMigratoriaPri.SelectedValue != registroLinea.TIPO_CALIDAD_MIGRATORIA.ToString())
            {
                ddlCalidadMigratoriaPri.Style.Add("background-color", "#9FDFC1");
                ddlCalidadMigratoriaPri.ToolTip = "REGISTRO ANTERIOR: " + ddlCalidadMigratoriaPri.SelectedItem.Text;
            }
            if (ddlCalidadMigratoriaSec.SelectedValue != registroLinea.COD_CARGO.ToString())
            {
                ddlCalidadMigratoriaSec.Style.Add("background-color", "#9FDFC1");
                ddlCalidadMigratoriaSec.ToolTip = "REGISTRO ANTERIOR: " + ddlCalidadMigratoriaSec.SelectedItem.Text;
            }
            if (ddlCategoriaOfcoEx.SelectedValue != registroLinea.COD_CATEGORIA_MISION.ToString())
            {
                ddlCategoriaOfcoEx.Style.Add("background-color", "#9FDFC1");
                ddlCategoriaOfcoEx.ToolTip = "REGISTRO ANTERIOR: " + ddlCategoriaOfcoEx.SelectedItem.Text;
            }
            if (ddlMision.SelectedValue != registroLinea.COD_INSTITUCION.ToString())
            {
                ddlMision.Style.Add("background-color", "#9FDFC1");
                ddlMision.ToolTip = "REGISTRO ANTERIOR: " + ddlMision.SelectedItem.Text;
            }

            VerificarUbigeo(registroLinea.DpUbigeo);
        }
        protected void separarUbigeo(string ubigeo, object sender, EventArgs e)
        {
            if (ubigeo != null)
            {
                string ubi01 = "";
                string ubi02 = "";
                string ubi03 = "";

                if (ubigeo.Length == 6)
                {
                    ubi01 = ubigeo.Substring(0, 2);
                    ubi02 = ubigeo.Substring(2, 2);
                    ubi03 = ubigeo.Substring(4, 2);
                    if (ubi01.Length == 2 & ubi02.Length == 2 & ubi03.Length == 2)
                    {
                        ddlDepartamento.SelectedValue = ubi01;
                        seleccionarDepartamento(sender, e);
                        ddlProvincia.SelectedValue = ubi02;
                        seleccionarProvincia(sender, e);
                        ddlDistrito.SelectedValue = ubi03;
                    }
                }
            }
        }
        private void VerificarUbigeo(string ubigeoNuevo)
        {
            if (ubigeoNuevo != null)
            {
                string ubi01 = "";
                string ubi02 = "";
                string ubi03 = "";

                if (ubigeoNuevo.Length == 6)
                {
                    ubi01 = ubigeoNuevo.Substring(0, 2);
                    ubi02 = ubigeoNuevo.Substring(2, 2);
                    ubi03 = ubigeoNuevo.Substring(4, 2);
                    if (ubi01.Length == 2 & ubi02.Length == 2 & ubi03.Length == 2)
                    {
                        if (ddlDepartamento.SelectedValue != ubi01)
                        {
                            ddlDepartamento.Style.Add("background-color", "#9FDFC1");
                            ddlDepartamento.ToolTip = "REGISTRO ANTERIOR: " + ddlDepartamento.SelectedItem.Text;
                        }
                        if (ddlProvincia.SelectedValue != ubi02)
                        {
                            ddlProvincia.Style.Add("background-color", "#9FDFC1");
                            ddlProvincia.ToolTip = "REGISTRO ANTERIOR: " + ddlProvincia.SelectedItem.Text;
                        }
                        if (ddlDistrito.SelectedValue != ubi03)
                        {
                            ddlDistrito.Style.Add("background-color", "#9FDFC1");
                            ddlDistrito.ToolTip = "REGISTRO ANTERIOR: " + ddlDistrito.SelectedItem.Text;
                        }
                    }
                }
            }
        }
        protected void styleTextbox(Control parent, string colorBack)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is DropDownList)
                {
                    ((DropDownList)(c)).Style.Add("background-color", colorBack);
                }
                if (c is TextBox)
                {
                    ((TextBox)(c)).Style.Add("background-color", colorBack);
                }
                styleTextbox(c, colorBack);
            }
        }
        #endregion
        
        #region Relacion Dependencia
        protected void buscarRelacionDependencia(object sender, EventArgs e)
        {
            try
            {
                short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
                string ipCreacion = ViewState["IP"].ToString();
                string carneNumero = txtRelDep.Text.Trim();
                if (!carneNumero.Equals(""))
                {
                    beCarneIdentidad parametros = new beCarneIdentidad();
                    parametros.Periodo = 0;//int.Parse(ddlPeriodo.SelectedValue);
                    parametros.IdentMesaPartes = "";
                    parametros.IdentNumero = 0;// (txtNroExp.Text.Equals("") ? 0 : int.Parse(txtNroExp.Text));
                    parametros.CarneNumero = txtRelDep.Text;
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
                    parametros.CantReg = 1;
                    brRegistroLinea obrRegistroLinea = new brRegistroLinea();
                    beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
                    obeCarneIdentidadPrincipal = obrRegistroLinea.obtenerRelacionDependencia(parametros);
                    Session["Temporal"] = obeCarneIdentidadPrincipal.ListaConsulta;
                    if (obeCarneIdentidadPrincipal.ListaConsulta != null)
                    {
                        if (obeCarneIdentidadPrincipal.ListaConsulta.Count == 1)
                        {
                            if (RelacionDependenciaGen == null)
                            {
                                RelacionDependenciaGen = new beCarneIdentidadRelacionDependencia();
                            }
                            RelacionDependenciaGen.CarneIdentidadTitId = obeCarneIdentidadPrincipal.ListaConsulta[0].CarneIdentidadid;
                            RelacionDependenciaGen.CarneIdentidadDepId = 0;
                            RelacionDependenciaGen.UsuarioCreacion = usuarioId;
                            RelacionDependenciaGen.IpCreacion = ipCreacion;
                            RelacionDependenciaGen.UsuarioModificacion = usuarioId;
                            RelacionDependenciaGen.IpModificacion = ipCreacion;
                            //titularRelDep = new beCarneIdentidad();
                            //titularRelDep = obeCarneIdentidadPrincipal.ListaConsulta[0];
                            //titularRelDep.Usuariocreacion = usuarioId;
                            //titularRelDep.Ipcreacion = ipCreacion;
                            //ibtVerRelDep.Visible = true;
                            txtRelDep.Style.Add("background-color", "Green");
                            txtRelDep.Style.Add("color", "White");
                            //lblMensaje.Text = "Vinculado Relacion de Dependencia";
                        }
                        else
                        {
                            if (RelacionDependenciaGen == null)
                            {
                                RelacionDependenciaGen = new beCarneIdentidadRelacionDependencia();
                            }
                            RelacionDependenciaGen = new beCarneIdentidadRelacionDependencia();
                            RelacionDependenciaGen.CarneIdentidadTitId = 0;
                            RelacionDependenciaGen.CarneIdentidadDepId = 0;
                            RelacionDependenciaGen.UsuarioCreacion = usuarioId;
                            RelacionDependenciaGen.IpCreacion = ipCreacion;
                            RelacionDependenciaGen.UsuarioModificacion = usuarioId;
                            RelacionDependenciaGen.IpModificacion = ipCreacion;
                            txtRelDep.Text = "";
                            //titularRelDep = new beCarneIdentidad();
                            //titularRelDep = obeCarneIdentidadPrincipal.ListaConsulta[0];
                            //titularRelDep.Usuariocreacion = usuarioId;
                            //titularRelDep.Ipcreacion = ipCreacion;
                            txtRelDep.Style.Add("background-color", "White");
                            txtRelDep.Style.Add("color", "Black");
                            //titularRelDep = null;
                            ibtVerRelDep.Visible = false;
                            //lblMensaje.Text = "[NO SELECCIONADO]";
                        }
                    }
                    //divModal.Style.Add("display", "block");
                    //divRelDep.Style.Add("display", "block");
                }
                else
                {
                    if (RelacionDependenciaGen == null)
                    {
                        RelacionDependenciaGen = new beCarneIdentidadRelacionDependencia();
                    }
                    RelacionDependenciaGen = new beCarneIdentidadRelacionDependencia();
                    RelacionDependenciaGen.CarneIdentidadTitId = 0;
                    RelacionDependenciaGen.CarneIdentidadDepId = 0;
                    RelacionDependenciaGen.UsuarioCreacion = usuarioId;
                    RelacionDependenciaGen.IpCreacion = ipCreacion;
                    RelacionDependenciaGen.UsuarioModificacion = usuarioId;
                    RelacionDependenciaGen.IpModificacion = ipCreacion;
                    //titularRelDep = null;
                    txtRelDep.Style.Add("background-color", "White");
                    txtRelDep.Style.Add("color", "Black");
                    //titularRelDep = null;
                    ibtVerRelDep.Visible = false;
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "selectOption", "selectOption('ContentPlaceHolder2_rbtDependiente','rbtTitular',false)", true);
            }
        }

        protected void mostrarRelDep(object sender, EventArgs e)
        {
            try
            {
                lblRelDepNumCarne.Text = titularRelDep.CarneNumero;
                lblRelDepTitular.Text = titularRelDep.ConFuncionario;
                divModal.Style.Add("display", "block");
                divRelDep.Style.Add("display", "block");
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }
        #endregion

        protected void btnRegistrarCarnetPopup_Click(object sender, EventArgs e)
        {
            try
            {
                registrarCarneIdentidad();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "habilitarControles", "disIntablaFuncionario(false);", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
                //calidadHumanitaria(sender, e);
                obrGeneral.grabarLog(ex);
            }
        }
        protected beRegistroLinea buscarSolicitud(beRegistroLinea obeRegistroLinea)
        {
            beRegistroLinea consultaReporte = new beRegistroLinea();
            string numSol = obeRegistroLinea.NumeroRegLinea;
            consultaReporte.NumeroRegLinea = numSol;
            brRegistroLinea obrRegistroLinea = new brRegistroLinea();
            consultaReporte = obrRegistroLinea.consultarRegistro(obeRegistroLinea);
            return (consultaReporte);
        }
        protected void descargarResumen(object sender, ImageClickEventArgs e)
        {
            try
            {
                string documento = hRutaResumenLinea.Value;
                string numSol = obeRegistroLinea.NumeroRegLinea;
                if (documento.Length > 0)
                {
                    if (File.Exists(documento))
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(documento);
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
        private void descargarPDF(string nombreArchivo, string ruta)
        {

            try
            {
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + nombreArchivo);
                //Response.TransmitFile(path + nomFile);
                Response.BinaryWrite(System.IO.File.ReadAllBytes(ruta)); 
                //Response.WriteFile(ruta);
                Response.End();
            }
            catch (SystemException ex)
            {
                throw new Exception("Error al descargar", ex.InnerException);
            }
        }

    }
}