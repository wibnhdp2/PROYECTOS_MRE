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
    public partial class SupervisorRegistroPrevio : System.Web.UI.Page
    {
        brGeneral obrGeneral = new brGeneral();
        CodigoUsuario oCodigoUsuario = new CodigoUsuario();
        UIEncriptador oUIEncriptador = new UIEncriptador();
        public static string srtAl = "";
        public static List<beEstado> ListaEstados = new List<beEstado>();
        public static List<beParametro> ListaTipoObs = new List<beParametro>();
        protected void Page_Load(object sender, EventArgs e)
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
                #region Otros Controles
                txtFechaConsulares.Text = DateTime.Now.ToShortDateString();
                #endregion
                #region Generales
                //GENERALES
                if (Session["Generales"] != null)
                {
                    beGenerales obeGenerales = new beGenerales();
                    obeGenerales = (beGenerales)Session["Generales"];
                    // DOCUMENTO IDENTIDAD
                    obeGenerales.ListaDocumentoIdentidad.Insert(0, new beDocumentoIdentidad { Tipodocumentoidentidadid = 0, DescripcionCorta = "<Seleccione>" });
                    ddlSolDocumentoIdent.DataSource = obeGenerales.ListaDocumentoIdentidad;
                    ddlSolDocumentoIdent.DataValueField = "Tipodocumentoidentidadid";
                    ddlSolDocumentoIdent.DataTextField = "DescripcionCorta";
                    ddlSolDocumentoIdent.DataBind();
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
                    // TIPO ENTRADA
                    obeGenerales.TipoEntrada.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                    ddlTipoEntrada.DataSource = obeGenerales.TipoEntrada;
                    ddlTipoEntrada.DataValueField = "Parametroid";
                    ddlTipoEntrada.DataTextField = "Descripcion";
                    ddlTipoEntrada.DataBind();

                    ListaEstados = obeGenerales.ListaEstados;
                    ListaTipoObs = obeGenerales.TipoObservacion;
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
                #region Carga de Datos en Edicion
                if (Request.QueryString["regprevid"] != null)
                {
                    ViewState["regprevid"] = Sanitizer.GetSafeHtmlFragment(oUIEncriptador.DesEncriptarCadena(Request.QueryString["regprevid"].ToString().Replace(" ", "+")));
                    ViewState["identificador"] = Sanitizer.GetSafeHtmlFragment(oUIEncriptador.DesEncriptarCadena(Request.QueryString["identificador"].ToString().Replace(" ", "+")));
                    ViewState["pag"] = Sanitizer.GetSafeHtmlFragment(oUIEncriptador.DesEncriptarCadena(Request.QueryString["pag"].ToString().Replace(" ", "+")));
                    brRegistroPrevio obrRegistroPrevio = new brRegistroPrevio();
                    beRegistroPrevio obeRegistroPrevio = new beRegistroPrevio();
                    beRegistroPrevio parametros = new beRegistroPrevio();
                    parametros.RegistroPrevioId = short.Parse(ViewState["regprevid"].ToString());
                    obeRegistroPrevio = obrRegistroPrevio.consultarRegistroEdicion(parametros);
                    if (obeRegistroPrevio != null)
                    {
                        string Fecha = "01/01/0001";
                        DateTime Fecha1 = DateTime.Parse(Fecha);
                        ViewState["carneId"] = obeRegistroPrevio.ConCarneIdentidadId.ToString();
                        lblIdentiSolicitud.Text = ViewState["identificador"].ToString();
                        txtApePat.Text = obeRegistroPrevio.PrimerApellido;
                        txtApeMat.Text = obeRegistroPrevio.SegundoApellido;
                        txtNombres.Text = obeRegistroPrevio.Nombres;
                        ddlSexo.SelectedValue = obeRegistroPrevio.GeneroId.ToString();
                        ddlCalidadMigratoriaPri.SelectedValue = obeRegistroPrevio.CalidadMigratoria.ToString();
                        ddlCategoriaOfcoEx.SelectedValue = obeRegistroPrevio.CategoriaOfcoExId.ToString();
                        seleccionarCategoriaOficina(sender, e);
                        ddlMision.SelectedValue = obeRegistroPrevio.OficinaConsularExId.ToString();
                        ddlTipoEntrada.SelectedValue = obeRegistroPrevio.TipoEntrada.ToString();
                        txtFechaConsulares.Text = obeRegistroPrevio.FechaConsulares.ToShortDateString();
                        if (obeRegistroPrevio.FechaPrivilegios != Fecha1) { txtFechaPrivilegios.Text = obeRegistroPrevio.FechaPrivilegios.ToShortDateString(); }
                        // BOTONES ------------------------------------------------------------------------------------------------------
                        btnGuardar.Visible = false;
                        btnGuardarEdicion.Visible = true;
                        btnCancelar.Visible = true;
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarFechaPrivilegios", "mostrarFechaPrivilegios();", true);
                }
                #endregion
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
                #region PostBack
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarFechaPrivilegios", "mostrarFechaPrivilegios();", true);
                #endregion
            }
        }

        #region Controles
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
                            ddlMision.Focus();
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

        protected void registrarRegPrevio(object sender, EventArgs e) // REGISTRA O CREA UN REGISTRO DE CARNÉ DE IDENTIDAD
        {
            try
            {
                int idRegistroPrevio = -1;
                string identCarneIdentidad = "error";
                short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
                short oficinaId = short.Parse(ViewState["Oficinaid"].ToString());
                string ipCreacion = ViewState["IP"].ToString();
                short estadoReg = obtenerIdEstado("REGISTRO PREVIO");
                beRegistroPrevioListas obeRegistroPrevioListas = new beRegistroPrevioListas();
                beSolicitante obeSolicitante = new beSolicitante();
                if (!ddlSolDocumentoIdent.SelectedValue.Equals("0"))
                {
                    // SOLICITANTE
                    obeSolicitante.PrimerApellido = txtSolApePat.Text.Trim().ToUpper();
                    obeSolicitante.SegundoApellido = txtSolApeMat.Text.Trim().ToUpper();
                    obeSolicitante.Nombres = txtSolNombres.Text.Trim().ToUpper();
                    obeSolicitante.TipoDocumentoIdentidadId = short.Parse(ddlSolDocumentoIdent.SelectedValue);
                    obeSolicitante.NumeroDocumentoIdentidad = txtSolNumeroDocIdent.Text.Trim();
                    obeSolicitante.Telefono = txtSolTelefono.Text.Trim();
                    obeSolicitante.Usuariocreacion = usuarioId;
                    obeSolicitante.Ipcreacion = ipCreacion;
                    obeSolicitante.Usuariomodificacion = usuarioId;
                    obeSolicitante.Ipmodificacion = ipCreacion;
                    // ACTA RECEPCION CAB
                    beActaRecepcionCabecera obeActaRecepcionCabecera = new beActaRecepcionCabecera();
                    obeActaRecepcionCabecera.Observacion = (!txtSolObservacion.Text.Trim().Equals("") ? txtSolObservacion.Text.Trim().ToUpper() : null);
                    obeActaRecepcionCabecera.UsuarioCreacion = usuarioId;
                    obeActaRecepcionCabecera.IpCreacion = ipCreacion;
                    obeRegistroPrevioListas.RecepcionCabecera = obeActaRecepcionCabecera;
                    // ACTA RECEPCION DET
                    beActaRecepcionDetalle obeActaRecepcionDetalle = new beActaRecepcionDetalle();
                    obeActaRecepcionDetalle.UsuarioCreacion = usuarioId;
                    obeActaRecepcionDetalle.IpCreacion = ipCreacion;
                    obeRegistroPrevioListas.RecepcionDetalle = obeActaRecepcionDetalle;
                }
                obeRegistroPrevioListas.Solicitante = obeSolicitante;
                // REGISTRO PREVIO
                beRegistroPrevio obeRegistroPrevio = new beRegistroPrevio();
                obeRegistroPrevio.PrimerApellido = txtApePat.Text.ToUpper().Trim();
                obeRegistroPrevio.SegundoApellido = txtApeMat.Text.ToUpper().Trim();
                obeRegistroPrevio.Nombres = txtNombres.Text.ToUpper().Trim();
                obeRegistroPrevio.GeneroId = short.Parse(ddlSexo.SelectedValue);
                obeRegistroPrevio.OficinaConsularExId = short.Parse(ddlMision.SelectedValue);
                obeRegistroPrevio.CalidadMigratoria = short.Parse(ddlCalidadMigratoriaPri.SelectedValue);
                obeRegistroPrevio.FechaConsulares = DateTime.Parse(txtFechaConsulares.Text);
                if (!txtFechaPrivilegios.Text.Equals(""))
                {
                    obeRegistroPrevio.FechaPrivilegios = DateTime.Parse(txtFechaPrivilegios.Text);
                }
                obeRegistroPrevio.Periodo = DateTime.Now.Year;
                obeRegistroPrevio.EstadoId = estadoReg;
                obeRegistroPrevio.TipoEntrada = short.Parse(ddlTipoEntrada.SelectedValue);
                obeRegistroPrevio.UsuarioCreacion = usuarioId;
                obeRegistroPrevio.IpCreacion = ipCreacion;
                obeRegistroPrevioListas.RegistroPrevio = obeRegistroPrevio;
                // MOVIMIENTO CARNÉ
                beMovimientoCarneIdentidad obeMovimientoCarneIdentidad = new beMovimientoCarneIdentidad();
                obeMovimientoCarneIdentidad.Estadoid = estadoReg;
                obeMovimientoCarneIdentidad.Oficinaconsularid = oficinaId;
                obeMovimientoCarneIdentidad.Usuariocreacion = usuarioId;
                obeMovimientoCarneIdentidad.Ipcreacion = ipCreacion;
                obeRegistroPrevioListas.MovimientoCarne = obeMovimientoCarneIdentidad;
                brRegistroPrevio obrRegistroPrevio = new brRegistroPrevio();
                idRegistroPrevio = obrRegistroPrevio.adicionar(obeRegistroPrevioListas);
                if (idRegistroPrevio > 0)
                {
                    brCarneIdentidadPrincipal obrCarneIdentidadPrincipal = new brCarneIdentidadPrincipal();
                    identCarneIdentidad = obrCarneIdentidadPrincipal.obtenerIdent(idRegistroPrevio);
                    lblIdentiSolicitud.Text = identCarneIdentidad;
                    DisableControls(tablaRegistro, false);
                    DisableControls(tablaRegistroSol, false);
                    btnGuardar.Visible = false;
                    if (!ddlSolDocumentoIdent.SelectedValue.Equals("0"))
                    {
                        brActaRecepcionCabecera obrActaRecepcionCabecera = new brActaRecepcionCabecera();
                        beActaRecepcionCabecera obeActaRecepcionCabecera = new beActaRecepcionCabecera();
                        beActaRecepcionDetalle obeActaRecepcionDetalle = new beActaRecepcionDetalle();
                        obeActaRecepcionDetalle.CarneIdentidadId = idRegistroPrevio;
                        obeActaRecepcionCabecera = obrActaRecepcionCabecera.obtenerCabecera(obeActaRecepcionDetalle);
                        btnImprimir.Visible = true;
                        ViewState["idActaRecepcion"] = obeActaRecepcionCabecera.ActaRecepcionId;
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE GUARDO LA INFORMACIÓN CORRECTAMENTE');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL GUARDAR LA INFORMACION.');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
                obrGeneral.grabarLog(ex);
            }
        }

        protected void actualizarRegistroPrevio(object sender, EventArgs e) // ACTUALIZA LOS DATOS DE UN REGISTRO PREVIO YA EXISTENTE
        {
            try
            {
                short idRegistroPrevio = -1;
                if (ViewState["regprevid"] != null) { idRegistroPrevio = short.Parse(ViewState["regprevid"].ToString()); }
                short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
                short oficinaId = short.Parse(ViewState["Oficinaid"].ToString());
                string ipCreacion = ViewState["IP"].ToString();
                short estadoReg = obtenerIdEstado("REGISTRO PREVIO");
                short tipoObs = obtenerTipoObs("EDICIÓN DE REGISTRO");
                beRegistroPrevioListas obeRegistroPrevioListas = new beRegistroPrevioListas();
                // SOLICITANTE
                beSolicitante obeSolicitante = new beSolicitante();
                if (!ddlSolDocumentoIdent.SelectedValue.Equals("0"))
                {
                    // SOLICITANTE
                    obeSolicitante.PrimerApellido = txtSolApePat.Text.Trim().ToUpper();
                    obeSolicitante.SegundoApellido = txtSolApeMat.Text.Trim().ToUpper();
                    obeSolicitante.Nombres = txtSolNombres.Text.Trim().ToUpper();
                    obeSolicitante.TipoDocumentoIdentidadId = short.Parse(ddlSolDocumentoIdent.SelectedValue);
                    obeSolicitante.NumeroDocumentoIdentidad = txtSolNumeroDocIdent.Text.Trim();
                    obeSolicitante.Telefono = txtSolTelefono.Text.Trim();
                    obeSolicitante.Usuariocreacion = usuarioId;
                    obeSolicitante.Ipcreacion = ipCreacion;
                    obeSolicitante.Usuariomodificacion = usuarioId;
                    obeSolicitante.Ipmodificacion = ipCreacion;
                    // ACTA RECEPCION CAB
                    beActaRecepcionCabecera obeActaRecepcionCabecera = new beActaRecepcionCabecera();
                    obeActaRecepcionCabecera.Observacion = (!txtSolObservacion.Text.Trim().Equals("") ? txtSolObservacion.Text.Trim().ToUpper() : null);
                    obeActaRecepcionCabecera.UsuarioCreacion = usuarioId;
                    obeActaRecepcionCabecera.IpCreacion = ipCreacion;
                    obeRegistroPrevioListas.RecepcionCabecera = obeActaRecepcionCabecera;
                    // ACTA RECEPCION DET
                    beActaRecepcionDetalle obeActaRecepcionDetalle = new beActaRecepcionDetalle();
                    obeActaRecepcionDetalle.UsuarioCreacion = usuarioId;
                    obeActaRecepcionDetalle.IpCreacion = ipCreacion;
                    obeRegistroPrevioListas.RecepcionDetalle = obeActaRecepcionDetalle;
                }
                obeRegistroPrevioListas.Solicitante = obeSolicitante;
                // REGISTRO PREVIO
                beRegistroPrevio obeRegistroPrevio = new beRegistroPrevio();
                obeRegistroPrevio.RegistroPrevioId = short.Parse(ViewState["regprevid"].ToString());
                obeRegistroPrevio.ConCarneIdentidadId = short.Parse(ViewState["carneId"].ToString());
                obeRegistroPrevio.PrimerApellido = txtApePat.Text.ToUpper().Trim();
                obeRegistroPrevio.SegundoApellido = txtApeMat.Text.ToUpper().Trim();
                obeRegistroPrevio.Nombres = txtNombres.Text.ToUpper().Trim();
                obeRegistroPrevio.GeneroId = short.Parse(ddlSexo.SelectedValue);
                obeRegistroPrevio.OficinaConsularExId = short.Parse(ddlMision.SelectedValue);
                obeRegistroPrevio.CalidadMigratoria = short.Parse(ddlCalidadMigratoriaPri.SelectedValue);
                obeRegistroPrevio.FechaConsulares = DateTime.Parse(txtFechaConsulares.Text);
                if (!txtFechaPrivilegios.Text.Equals(""))
                {
                    obeRegistroPrevio.FechaPrivilegios = DateTime.Parse(txtFechaPrivilegios.Text);
                }
                obeRegistroPrevio.TipoEntrada = short.Parse(ddlTipoEntrada.SelectedValue);
                obeRegistroPrevio.UsuarioModificacion = usuarioId;
                obeRegistroPrevio.IpModificacion = ipCreacion;
                obeRegistroPrevioListas.RegistroPrevio = obeRegistroPrevio;
                // MOVIMIENTO CARNÉ
                beMovimientoCarneIdentidad obeMovimientoCarneIdentidad = new beMovimientoCarneIdentidad();
                obeMovimientoCarneIdentidad.CarneIdentidadid = short.Parse(ViewState["carneId"].ToString());
                obeMovimientoCarneIdentidad.Estadoid = estadoReg;
                obeMovimientoCarneIdentidad.Oficinaconsularid = oficinaId;
                obeMovimientoCarneIdentidad.ObservacionTipo = tipoObs;
                //obeMovimientoCarneIdentidad.ObservacionDetalle = "EDICION DE REGISTRO PREVIO";
                obeMovimientoCarneIdentidad.Usuariocreacion = usuarioId;
                obeMovimientoCarneIdentidad.Ipcreacion = ipCreacion;
                obeRegistroPrevioListas.MovimientoCarne = obeMovimientoCarneIdentidad;
                brRegistroPrevio obrRegistroPrevio = new brRegistroPrevio();
                //bool exito = obrRegistroPrevio.actualizar(obeRegistroPrevioListas);
                idRegistroPrevio = obrRegistroPrevio.actualizar(obeRegistroPrevioListas);
                if (idRegistroPrevio > 0)
                {
                    btnGuardarEdicion.Visible = false;
                    btnImprimir.Visible = (!ddlSolDocumentoIdent.SelectedValue.Equals("0") ? true : false);
                    ViewState["idActaRecepcion"] = idRegistroPrevio;
                    btnCancelar.Visible = false;
                    btnVolver.Visible = true;
                    DisableControls(tablaRegistro, false);
                    DisableControls(tablaRegistroSol, false);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE ACTUALIZO LA INFORMACIÓN CORRECTAMENTE');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL ACTUALIZAR LA INFORMACIÓN.');", true);
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
            }
        }

        protected void nuevoRegistroPrevio(object sender, EventArgs e) // REDIRECCIONA A LA MISMA PAGINA A FIN DE COMENZAR UN NUEVO REGISTRO
        {
            Response.Redirect("~/Paginas/Supervisor/SupervisorRegistroPrevio.aspx");
        }

        protected void cancelarEdicion(object sender, EventArgs e) // DEVUELVE AL FORMULARIO ANTERIOR
        {
            try
            {
                string pag = oUIEncriptador.EncriptarCadena(ViewState["pag"].ToString());
                Response.Redirect(String.Format("SupervisorConsulta.aspx?pag={0}", pag));
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected void verActaRecepcion(object sender, EventArgs e) // CARGA EN MEMORIA EL ACTA DE RECEPCION DEL REGISTRO
        {
            brActaRecepcionCabecera obrActaRecepcionCabecera = new brActaRecepcionCabecera();
            beActaRecepcionCabecera parametrosConsultaRec = new beActaRecepcionCabecera();
            parametrosConsultaRec.ActaRecepcionId = short.Parse(ViewState["idActaRecepcion"].ToString());
            beActaRecepcionPrincipal resultadoRecConsulta = new beActaRecepcionPrincipal();
            resultadoRecConsulta = obrActaRecepcionCabecera.consultar(parametrosConsultaRec);
            if (resultadoRecConsulta != null)
            {
                byte[] pdfByte = oCodigoUsuario.crearActaRecepcion(resultadoRecConsulta, ViewState["UserNomComp"].ToString());
                Session["bytePDF"] = pdfByte;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirPDF", "window.open('../PDF.aspx', '_blank')", true);
            }
        }

        protected void obtenerSolicitante(object sender, EventArgs e) // OBTIENE LA INFORMACION DEL SOLICITANTE DE ACUERDO AL TIPO Y NUMERO DE IDENTIFICACION
        {
            try
            {
                ImageButton imageButton = (ImageButton)sender;
                string comando = imageButton.CommandName;

                beSolicitante parametros = new beSolicitante();
                parametros.TipoDocumentoIdentidadId = short.Parse(ddlSolDocumentoIdent.SelectedValue);
                parametros.NumeroDocumentoIdentidad = txtSolNumeroDocIdent.Text.Trim();
                brSolicitante obrSolicitante = new brSolicitante();
                beSolicitante obeSolicitante = obrSolicitante.consultarxIdentificacion(parametros);
                if (obeSolicitante != null)
                {
                    txtSolApePat.Text = obeSolicitante.PrimerApellido;
                    txtSolApeMat.Text = obeSolicitante.SegundoApellido;
                    txtSolNombres.Text = obeSolicitante.Nombres;
                    txtSolTelefono.Text = obeSolicitante.Telefono;
                    bloquearSolicitante(false);
                }
                else
                {
                    bloquearSolicitante(true);
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR.');", true);
            }
        }

        protected void limpiarSolicitante(object sender, EventArgs e) // LIMPIA LOS CONTROLES DEL SOLICITANTE
        {
            ddlSolDocumentoIdent.SelectedValue = "0";
            txtSolNumeroDocIdent.Text = string.Empty;
            txtSolApePat.Text = string.Empty;
            txtSolApeMat.Text = string.Empty;
            txtSolNombres.Text = string.Empty;
            txtSolTelefono.Text = string.Empty;
            bloquearSolicitante(true);
        }
        #endregion
        #region Funciones
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

        protected void bloquearSolicitante(bool valor) // BLOQUEA LOS CONTROLES DEL SOLICITANTE
        {
            ddlSolDocumentoIdent.Enabled = valor;
            txtSolNumeroDocIdent.Enabled = valor;
            txtSolApePat.Enabled = valor;
            txtSolApeMat.Enabled = valor;
            txtSolNombres.Enabled = valor;
            txtSolTelefono.Enabled = valor;
        }
        #endregion
    }
}