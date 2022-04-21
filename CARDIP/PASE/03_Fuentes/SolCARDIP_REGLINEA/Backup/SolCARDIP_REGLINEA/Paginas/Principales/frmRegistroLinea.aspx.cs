using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.Web.UI.WebControls;
using System.IO;
using SolCARDIP_REGLINEA.Librerias.EntidadesNegocio;
using SolCARDIP_REGLINEA.Librerias.ReglasNegocio;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;

namespace SolCARDIP_REGLINEA.Paginas.Principales
{
    public partial class frmRegistroLinea : System.Web.UI.Page
    {
        brGeneral obrGeneral = new brGeneral();
        CodigoUsuario oCodigoUsuario = new CodigoUsuario();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Request.Params["__EVENTTARGET"] == "ACTUALIZA")
            {
                Response.Redirect("RegistroLinea.aspx", true);
            }
            if (!(IsPostBack))
            {
                brGenerales obrGenerales = new brGenerales();
                beGenerales obeGenerales = obrGenerales.obtenerGenerales();
                if (obeGenerales != null)
                {
                    ViewState["Generales"] = obeGenerales;
                }
                if (Session["SessionGeneral"] != null)
                {
                    Session["tempImagenSave"] = null;
                    beRegistroLinea SessionGeneral = new beRegistroLinea();
                    SessionGeneral = (beRegistroLinea)Session["SessionGeneral"];
                    Session["SessionGeneral"] = null;
                    CargarListas(SessionGeneral, obeGenerales);
                    if (SessionGeneral.TipoEmisionObject == "EDITAR")
                    {
                        cargarDatosEditar(SessionGeneral, obeGenerales);
                    }
                }
                else {
                    Response.Redirect("RegistroLinea.aspx");
                }
            }
        }
        private void cargarDatosEditar(beRegistroLinea SessionGeneral, beGenerales obeGenerales)
        {
            hCodRegistroLinea.Value = SessionGeneral.RegistroLineaId.ToString();
            hCodSolicitudTexto.Value = SessionGeneral.NumeroRegLinea;
            ddlTipEmision.SelectedValue = SessionGeneral.TipoEmision.ToString();
            hCodCarnetIdentidad.Value = SessionGeneral.CarneIdentidadId.ToString();
            OcultarBusqueda();
            
            bool relacionDep = CargarEditarRelacionDependencia(SessionGeneral, obeGenerales);
            
            txtApePat.Text = SessionGeneral.DpPrimerApellido;
            txtApeMat.Text = SessionGeneral.DpSegundoApellido;
            txtNombres.Text = SessionGeneral.DpNombres;
            DateTime date = Convert.ToDateTime(SessionGeneral.DpFechaNacimiento.ToString());
            txtFecNac.Text = date.ToString("yyyy-MM-dd");
            ddlSexo.SelectedValue = SessionGeneral.DpGeneroId.ToString();
            seleccionarSexo(null, null);
            ddlEstadoCivil.SelectedValue = SessionGeneral.DpEstadoCivilId.ToString();
            ddlNacionalidad.SelectedValue = SessionGeneral.DpPaisNacionalidad.ToString();
            obtenerNacionalidadPais(null, null);
            ddlTipDoc.SelectedValue = SessionGeneral.DpTipoDocIdentidad.ToString();
            txtNroDocumentoSolicitante.Text = SessionGeneral.DpNumeroDocIdentidad;
            txtDomicilio.Text = SessionGeneral.DpDomicilioPeru;
            txtCorreo.Text = SessionGeneral.DpCorreoElectronico;
            string ubigeo = SessionGeneral.DpUbigeo;
            separarUbigeo(ubigeo, null, null);
            txtTelefono.Text = SessionGeneral.DpNumeroTelefono;

            txtInstitucion.Text = SessionGeneral.InNombreInstitucion;
            txtPersonaContacto.Text = SessionGeneral.InPersonaContacto;
            txtCorreoContacto.Text = SessionGeneral.InCorreoElectronico;
            txtCargoContacto.Text = SessionGeneral.InCargoContacto;
            txtCorreoContacto.Text = SessionGeneral.InCorreoElectronico;
            txtTelefonoContacto.Text = SessionGeneral.InNumeroTelefono;
            
            ddlTipoInstitucion.SelectedValue = SessionGeneral.InTipoInstitucion.ToString();
            cargarTipoCargo(null,null);
            ddlCargo.SelectedValue = SessionGeneral.InTipoCargo.ToString();
            txtOtroCargo.Text = SessionGeneral.InCargoNombre;
            
            hRuta.Value = SessionGeneral.DpRutaAdjunto;
            if (hRuta.Value.Length > 0)
            {
                LlenarDatosPestañaGrabar();
                cargarImagenAlmacenada(obrGeneral.rutaAdjuntos + hRuta.Value);
            }

            if (ddlTipEmision.SelectedItem.Text == "DUPLICADO")
            {
                DesactivarTodosTabs("GuardarTab");
            }
            else {
                DesactivarTodosTabs("RelacionDepTab");
                DesactivarTodosTabs("SolicitanteTab");
                DesactivarTodosTabs("InstitucionTab");
                DesactivarTodosTabs("FuncionCargoTab");
                DesactivarTodosTabs("FotografiaTab");
                DesactivarTodosTabs("GuardarTab");
            }
            btnEditar.Visible = true;
        }
        private bool CargarEditarRelacionDependencia(beRegistroLinea SessionGeneral, beGenerales obeGenerales)
        {
            bool resultado = false;
            beCarneIdentidad obeCarneIdentidad = new beCarneIdentidad();
            if (SessionGeneral.DpReldepTitdep > 0)
            {
                ddlRelDependencia.SelectedValue = SessionGeneral.DpReldepTitdep.ToString();
                hCodigoCarnetTitular.Value = SessionGeneral.DpReldepTitular.ToString();

                //beGenerales obeGenerales = (beGenerales)ViewState["Generales"];
                int pos = obeGenerales.TitularDependienteParametros.FindIndex(x => x.Parametroid == SessionGeneral.DpReldepTitdep);
                string key = obeGenerales.TitularDependienteParametros[pos].Descripcion.ToUpper();
                if (key.Equals("TITULAR"))
                {
                    ddlRelDependencia_SelectedIndexChanged(null,null);
                    resultado = true;
                }
                if (key.Equals("DEPENDIENTE"))
                {
                    ddlRelDependencia_SelectedIndexChanged(null, null);
                    short carneID = SessionGeneral.DpReldepTitular;
                    if (carneID > 0)
                    {
                        brRegistroLinea obrRegistroLinea = new brRegistroLinea();
                        beCarneIdentidad carneOBJ = new beCarneIdentidad();
                        carneOBJ = obrRegistroLinea.consultarCarnexId(carneID);
                        if (carneOBJ != null)
                        {
                            obeCarneIdentidad = carneOBJ;
                            List<beCarneIdentidad> lista = new List<beCarneIdentidad>();
                            lista.Add(carneOBJ);
                            gvRelDep.DataSource = lista;
                            gvRelDep.DataBind();
                            btnContinuarSolicitante.Enabled = true;
                            resultado = true;
                        }
                    }
                }
            }
            return resultado;
        }
        protected void cargarComboNull(DropDownList controlDropDown)
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
        public List<beDocumentoIdentidad> cargarDocumentoIdentidad(short idPais, List<beDocumentoIdentidad> lista)
        {
            List<beDocumentoIdentidad> listaDocIdent = new List<beDocumentoIdentidad>();
            listaDocIdent = lista.FindAll(x => x.PaisId == 0 | x.PaisId == idPais);
            return (listaDocIdent);
        }
        protected void obtenerNacionalidadPais(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["Generales"] != null)
                {
                    beGenerales obeGenerales = (beGenerales)ViewState["Generales"];
                    short PaisId = short.Parse(ddlNacionalidad.SelectedValue);
                    if (PaisId > 0)
                    {
                        bePais obePais = oCodigoUsuario.obtenerDatosPais(PaisId, obeGenerales.ListaPaises);
                        if (obePais != null)
                        {
                            lblNacionalidad.Text = obePais.Nacionalidad;
                            lblNacionalidad.ForeColor = (obePais.Nacionalidad.Equals("[ NO DEFINIDO ]") ? Color.Red : Color.Green);
                            List<beDocumentoIdentidad> listaDocIdent = cargarDocumentoIdentidad(short.Parse(ddlNacionalidad.SelectedValue), obeGenerales.ListaDocumentoIdentidadRegLinea);
                            listaDocIdent.Insert(0, new beDocumentoIdentidad { Tipodocumentoidentidadid = 0, DescripcionLarga = "<Seleccione>" });
                            ddlTipDoc.DataSource = listaDocIdent;
                            ddlTipDoc.DataValueField = "Tipodocumentoidentidadid";
                            ddlTipDoc.DataTextField = "DescripcionLarga";
                            ddlTipDoc.DataBind();
                        }
                    }
                    else
                    {
                        cargarComboNull(ddlTipDoc);
                    }
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }
        protected void seleccionarSexo(object sender, EventArgs e)
        {
            try
            {
                if (!ddlSexo.SelectedValue.Equals("0"))
                {
                    if (ViewState["Generales"] != null)
                    {
                        beGenerales obeGenerales = (beGenerales)ViewState["Generales"];
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
                    ddlEstadoCivil.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }
        protected void seleccionarDepartamento(object sender, EventArgs e)
        {
            try
            {
                if (Session["Ubigeo"] != null)
                {
                    beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                    obeUbigeoListas = (beUbigeoListas)Session["Ubigeo"];
                    List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                    lbeUbicaciongeografica = oCodigoUsuario.obtenerListaUbiGeo("02", ddlDepartamento.SelectedValue, "", obeUbigeoListas.Ubigeo02);
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
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected void seleccionarProvincia(object sender, EventArgs e)
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
            }
        }
        private void CargarListas(beRegistroLinea SessionGeneral, beGenerales obeGenerales)
        {
            try
            {
                if (SessionGeneral != null)
                {
                    lblNroSolicitud.Text = "Solicitud Nro. " + SessionGeneral.NumeroRegLinea;
                    hCodSolicitudTexto.Value =  SessionGeneral.NumeroRegLinea;
                    lblFechaRegistro.Text = "Fecha de Registro: " +  DateTime.Today.ToShortDateString();
                    hCodRegistroLinea.Value = SessionGeneral.RegistroLineaId.ToString();
                }

                //beGenerales obeGenerales = (beGenerales)ViewState["Generales"];
                ddlTipEmision.DataTextField = "Descripcion";
                ddlTipEmision.DataValueField = "Parametroid";
                ddlTipEmision.DataSource = obeGenerales.TipoEmision;
                ddlTipEmision.DataBind();
                ddlTipEmision.Items.Remove(ddlTipEmision.Items.FindByText("RECTIFICACIÓN"));
                ddlTipEmision.Items.Remove(ddlTipEmision.Items.FindByText("ACTUALIZACIÓN"));

                ddlRelDependencia.DataTextField = "Descripcion";
                ddlRelDependencia.DataValueField = "Parametroid";
                ddlRelDependencia.DataSource = obeGenerales.TitularDependienteParametros;
                ddlRelDependencia.DataBind();

                obeGenerales.ListaPaises.Insert(0, new bePais { Paisid = 0, Nombre = "<Seleccione>" });
                ddlNacionalidad.DataSource = obeGenerales.ListaPaises;
                ddlNacionalidad.DataValueField = "Paisid";
                ddlNacionalidad.DataTextField = "Nombre";
                ddlNacionalidad.DataBind();

                obeGenerales.ListaParametroGenero.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                ddlSexo.DataSource = obeGenerales.ListaParametroGenero;
                ddlSexo.DataValueField = "Parametroid";
                ddlSexo.DataTextField = "Descripcion";
                ddlSexo.DataBind();

                obeGenerales.ListaParametroEstadoCivil.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>", Valor="0" });
                ddlEstadoCivil.DataSource = obeGenerales.ListaParametroEstadoCivil;
                ddlEstadoCivil.DataValueField = "Parametroid";
                ddlEstadoCivil.DataTextField = "Descripcion";
                ddlEstadoCivil.DataBind();

                obeGenerales.ListaTipoInstitucion.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                ddlTipoInstitucion.DataSource = obeGenerales.ListaTipoInstitucion;
                ddlTipoInstitucion.DataValueField = "Parametroid";
                ddlTipoInstitucion.DataTextField = "Descripcion";
                ddlTipoInstitucion.DataBind();

                ddlCargo.DataSource = null;
                ddlCargo.DataBind();

                cargarComboNull(ddlTipDoc);
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
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }
        protected void cargarTipoCargo(object sender, EventArgs e)
        {
            beGenerales obeGenerales = (beGenerales)ViewState["Generales"];
            List<beParametro> listaTipoCargo = new List<beParametro>();
            string descTipo = oCodigoUsuario.detectarCaracterEspecial(ddlTipoInstitucion.SelectedItem.Text);
            int pos = obeGenerales.ListaTipoInstitucion.FindIndex(x => x.Descripcion.Equals(descTipo));
            string valor = obeGenerales.ListaTipoInstitucion[pos].Valor;
            listaTipoCargo = obeGenerales.ListaCargoInstitucion.FindAll(x => x.Valor.Equals(valor));
            ddlCargo.DataValueField = "Parametroid";
            ddlCargo.DataTextField = "Descripcion";
            ddlCargo.DataSource = listaTipoCargo;
            ddlCargo.DataBind();
        }

        private void OcultarBusqueda()
        {
            if (ddlTipEmision.SelectedItem.Text == "NUEVO")
            {
                DIV_DOCUMENTO.Visible = false;
                Div_NUEVO.Visible = true;
                Div_RENOVACION.Visible = false;
                Div_DUPLICADO.Visible = false;
                btnContinuar.Enabled = true;
                lblResuesta.Text = "";
                txtNroDoc.Text = "";
                hCodCarnetIdentidad.Value = "";
                Div_DUPLICADO_RENOVA.Visible = false;
            }
            if (ddlTipEmision.SelectedItem.Text == "RENOVACIÓN")
            {
                DIV_DOCUMENTO.Visible = true;
                Div_RENOVACION.Visible = true;
                Div_NUEVO.Visible = false;
                Div_DUPLICADO.Visible = false;
                btnContinuar.Enabled = false;
                lblResuesta.Text = "";
                txtNroDoc.Text = "";
                hCodCarnetIdentidad.Value = "";
                Div_DUPLICADO_RENOVA.Visible = true;
            }
            if (ddlTipEmision.SelectedItem.Text == "DUPLICADO")
            {
                DIV_DOCUMENTO.Visible = true;
                Div_DUPLICADO.Visible = true;
                Div_NUEVO.Visible = false;
                Div_RENOVACION.Visible = false;
                btnContinuar.Enabled = false;
                lblResuesta.Text = "";
                txtNroDoc.Text = "";
                hCodCarnetIdentidad.Value = "";
                Div_DUPLICADO_RENOVA.Visible = true;
            }
        }
        
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtNroDoc.Text.Trim().Length == 0)
            {
                lblResuesta.Text = "No encontro el carné. Revise el numero ingresado";
                return;
            }
            Respuesta.Visible = false;
            beCarneIdentidad parametros = new beCarneIdentidad();
            brRegistroLinea obrRegistroLinea = new brRegistroLinea();
            parametros.CarneNumero = txtNroDoc.Text.Trim();
            beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = obrRegistroLinea.obtenerCarneId(parametros);
            if (obeCarneIdentidadPrincipal != null)
            {
                if (obeCarneIdentidadPrincipal.CarneIdentidad != null)
                {
                    if (obeCarneIdentidadPrincipal.CarneIdentidad.CarneIdentidadid > 0)
                    {
                        cargarDatosDuplicadoRenovacion(obeCarneIdentidadPrincipal);
                        Respuesta.Visible = true;
                        btnContinuar.Enabled = true;
                        lblResuesta.Text = "Se encontró el carné " + parametros.CarneNumero + ". Puede continuar.";
                    }
                    else
                    {
                        Respuesta.Visible = true;
                        btnContinuar.Enabled = false;
                        lblResuesta.Text = "No encontró el carné " + parametros.CarneNumero + ". Revise el número ingresado";
                    }
                }
                else {
                    Respuesta.Visible = true;
                    btnContinuar.Enabled = false;
                    lblResuesta.Text = "No encontró el carné " + parametros.CarneNumero + ". Revise el número ingresado";
                }   
            }
        }

        protected void ddlTipEmision_SelectedIndexChanged(object sender, EventArgs e)
        {
            OcultarBusqueda();
        }
        protected void cargarDatosDuplicadoRenovacion(beCarneIdentidadPrincipal carneIdentidad)
        {
            hCodCarnetIdentidad.Value = carneIdentidad.CarneIdentidad.CarneIdentidadid.ToString();
            txtApePat.Text = carneIdentidad.Persona.Apellidopaterno;
            txtApeMat.Text = carneIdentidad.Persona.Apellidomaterno;
            txtNombres.Text = carneIdentidad.Persona.Nombres;
            txtNombres.Enabled = false;
            DateTime date = (DateTime)carneIdentidad.Persona.Nacimientofecha;
            txtFecNac.Text = date.ToString("yyyy-MM-dd");
            ddlSexo.SelectedValue = carneIdentidad.Persona.Generoid.ToString();
            seleccionarSexo(null, null);
            ddlEstadoCivil.SelectedValue = carneIdentidad.Persona.Estadocivilid.ToString();
            ddlNacionalidad.SelectedValue = carneIdentidad.Pais.Paisid.ToString();
            obtenerNacionalidadPais(null, null);
            ddlTipDoc.SelectedValue = carneIdentidad.PersonaIdentificacion.Documentotipoid.ToString();
            txtNroDocumentoSolicitante.Text = carneIdentidad.PersonaIdentificacion.Documentonumero;
            txtDomicilio.Text = carneIdentidad.Residencia.Residenciadireccion;
            txtCorreo.Text = carneIdentidad.Persona.Correoelectronico;
            string ubigeo = carneIdentidad.UbiGeo.Ubi01 + carneIdentidad.UbiGeo.Ubi02 + carneIdentidad.UbiGeo.Ubi03;
            separarUbigeo(ubigeo, null, null);
            txtTelefono.Text = carneIdentidad.Persona.Telefono;
            hRuta.Value = carneIdentidad.CarneIdentidad.RutaArchivoFoto;
            validarFotografia();
            cargarImagenAlmacenada(obrGeneral.rutaAdjuntos + carneIdentidad.CarneIdentidad.RutaArchivoFoto);
        }
        protected void cargarImagenAlmacenada(string Path)
        {
            try
            {
                if (File.Exists(Path))
                {
                    Byte[] imageByte = null;
                    imageByte = File.ReadAllBytes(Path);
                    string base64String = Convert.ToBase64String(imageByte, 0, imageByte.Length);
                    Session["tempImagenSave"] = base64String;
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
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
        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            if (ddlTipEmision.SelectedItem.Text == "DUPLICADO")
            {
                LlenarDatosPestañaGrabar();
                DesactivarTodosTabs("GuardarTab");
            }
            else {
                LlenarDatosPestañaGrabar();
                DesactivarTodosTabs("RelacionDepTab");
            }
        }
       
        private void DesactivarTodosTabs(string tabSetear)
        {
            try
            {
                string styleTipEmisionTab = TipEmisionTab.Attributes["class"].ToString();
                string styleRelacionDepTab = RelacionDepTab.Attributes["class"].ToString();
                string styleSolicitanteTab = SolicitanteTab.Attributes["class"].ToString();
                string styleInstitucionTab = InstitucionTab.Attributes["class"].ToString();
                string styleFuncionCargoTab = FuncionCargoTab.Attributes["class"].ToString();
                string styleFotografiaTab = FotografiaTab.Attributes["class"].ToString();
                string styleGuardarTab = GuardarTab.Attributes["class"].ToString();
                TabName.Value = tabSetear;
                if (tabSetear == "TipEmisionTab")
                {
                    tab1.Enabled = true;
                    tab2.Enabled = true;
                    tab3.Enabled = true;
                    tab4.Enabled = true;
                    tab5.Enabled = true;
                    tab6.Enabled = true;

                    TipEmisionTab.Attributes["class"] = "nav-link active";
                    TipEmision.Attributes["class"] = "tab-pane active";

                    RelacionDepTab.Attributes["class"] = "nav-link disabled";
                    RelacionDep.Attributes["class"] = "tab-pane disabled";

                    SolicitanteTab.Attributes["class"] = "nav-link disabled";
                    Solicitante.Attributes["class"] = "tab-pane disabled";

                    InstitucionTab.Attributes["class"] = "nav-link disabled";
                    Institucion.Attributes["class"] = "tab-pane disabled";

                    FuncionCargoTab.Attributes["class"] = "nav-link disabled";
                    FuncionCargo.Attributes["class"] = "tab-pane disabled";

                    FotografiaTab.Attributes["class"] = "nav-link disabled";
                    Fotografia.Attributes["class"] = "tab-pane disabled";

                    GuardarTab.Attributes["class"] = "nav-link disabled";
                    Guardar.Attributes["class"] = "tab-pane disabled";

                }
                if (tabSetear == "RelacionDepTab")
                {
                    RelacionDepTab.Attributes["class"] = "nav-link active";
                    RelacionDep.Attributes["class"] = "tab-pane active";
                    tab1.Enabled = false;
                    if (!styleTipEmisionTab.Contains("disabled"))
                    {
                        TipEmisionTab.Attributes["class"] = "nav-link";
                        TipEmision.Attributes["class"] = "tab-pane";
                    }
                    if (!styleSolicitanteTab.Contains("disabled"))
                    {
                        SolicitanteTab.Attributes["class"] = "nav-link";
                        Solicitante.Attributes["class"] = "tab-pane";
                    }
                    if (!styleInstitucionTab.Contains("disabled"))
                    {
                        InstitucionTab.Attributes["class"] = "nav-link";
                        Institucion.Attributes["class"] = "tab-pane";
                    }
                    if (!styleFuncionCargoTab.Contains("disabled"))
                    {
                        FuncionCargoTab.Attributes["class"] = "nav-link";
                        FuncionCargo.Attributes["class"] = "tab-pane";
                    }
                    if (!styleFotografiaTab.Contains("disabled"))
                    {
                        FotografiaTab.Attributes["class"] = "nav-link";
                        Fotografia.Attributes["class"] = "tab-pane";
                    }
                    if (!styleGuardarTab.Contains("disabled"))
                    {
                        GuardarTab.Attributes["class"] = "nav-link";
                        Guardar.Attributes["class"] = "tab-pane";
                    }
                }
                if (tabSetear == "SolicitanteTab")
                {
                    tab2.Enabled = false;
                    SolicitanteTab.Attributes["class"] = "nav-link active";
                    Solicitante.Attributes["class"] = "tab-pane active";

                    if (!styleTipEmisionTab.Contains("disabled"))
                    {
                        TipEmisionTab.Attributes["class"] = "nav-link";
                        TipEmision.Attributes["class"] = "tab-pane";
                    }
                    if (!styleRelacionDepTab.Contains("disabled"))
                    {
                        RelacionDepTab.Attributes["class"] = "nav-link";
                        RelacionDep.Attributes["class"] = "tab-pane";
                    }

                    if (!styleInstitucionTab.Contains("disabled"))
                    {
                        InstitucionTab.Attributes["class"] = "nav-link";
                        Institucion.Attributes["class"] = "tab-pane";
                    }
                    if (!styleFuncionCargoTab.Contains("disabled"))
                    {
                        FuncionCargoTab.Attributes["class"] = "nav-link";
                        FuncionCargo.Attributes["class"] = "tab-pane";
                    }
                    if (!styleFotografiaTab.Contains("disabled"))
                    {
                        FotografiaTab.Attributes["class"] = "nav-link";
                        Fotografia.Attributes["class"] = "tab-pane";
                    }
                    if (!styleGuardarTab.Contains("disabled"))
                    {
                        GuardarTab.Attributes["class"] = "nav-link";
                        Guardar.Attributes["class"] = "tab-pane";
                    }
                }
                if (tabSetear == "InstitucionTab")
                {
                    tab3.Enabled = false;
                    InstitucionTab.Attributes["class"] = "nav-link active";
                    Institucion.Attributes["class"] = "tab-pane active";

                    if (!styleTipEmisionTab.Contains("disabled"))
                    {
                        TipEmisionTab.Attributes["class"] = "nav-link";
                        TipEmision.Attributes["class"] = "tab-pane";
                    }
                    if (!styleRelacionDepTab.Contains("disabled"))
                    {
                        RelacionDepTab.Attributes["class"] = "nav-link";
                        RelacionDep.Attributes["class"] = "tab-pane";
                    }
                    if (!styleSolicitanteTab.Contains("disabled"))
                    {
                        SolicitanteTab.Attributes["class"] = "nav-link";
                        Solicitante.Attributes["class"] = "tab-pane";
                    }

                    if (!styleFuncionCargoTab.Contains("disabled"))
                    {
                        FuncionCargoTab.Attributes["class"] = "nav-link";
                        FuncionCargo.Attributes["class"] = "tab-pane";
                    }
                    if (!styleFotografiaTab.Contains("disabled"))
                    {
                        FotografiaTab.Attributes["class"] = "nav-link";
                        Fotografia.Attributes["class"] = "tab-pane";
                    }
                    if (!styleGuardarTab.Contains("disabled"))
                    {
                        GuardarTab.Attributes["class"] = "nav-link";
                        Guardar.Attributes["class"] = "tab-pane";
                    }
                }
                if (tabSetear == "FuncionCargoTab")
                {
                    tab4.Enabled = false;
                    FuncionCargoTab.Attributes["class"] = "nav-link active";
                    FuncionCargo.Attributes["class"] = "tab-pane active";

                    if (!styleTipEmisionTab.Contains("disabled"))
                    {
                        TipEmisionTab.Attributes["class"] = "nav-link";
                        TipEmision.Attributes["class"] = "tab-pane";
                    }
                    if (!styleRelacionDepTab.Contains("disabled"))
                    {
                        RelacionDepTab.Attributes["class"] = "nav-link";
                        RelacionDep.Attributes["class"] = "tab-pane";
                    }
                    if (!styleSolicitanteTab.Contains("disabled"))
                    {
                        SolicitanteTab.Attributes["class"] = "nav-link";
                        Solicitante.Attributes["class"] = "tab-pane";
                    }
                    if (!styleInstitucionTab.Contains("disabled"))
                    {
                        InstitucionTab.Attributes["class"] = "nav-link";
                        Institucion.Attributes["class"] = "tab-pane";
                    }

                    if (!styleFotografiaTab.Contains("disabled"))
                    {
                        FotografiaTab.Attributes["class"] = "nav-link";
                        Fotografia.Attributes["class"] = "tab-pane";
                    }
                    if (!styleGuardarTab.Contains("disabled"))
                    {
                        GuardarTab.Attributes["class"] = "nav-link";
                        Guardar.Attributes["class"] = "tab-pane";
                    }
                }
                if (tabSetear == "FotografiaTab")
                {
                    tab5.Enabled = false;
                    FotografiaTab.Attributes["class"] = "nav-link active";
                    Fotografia.Attributes["class"] = "tab-pane active";

                    if (!styleTipEmisionTab.Contains("disabled"))
                    {
                        TipEmisionTab.Attributes["class"] = "nav-link";
                        TipEmision.Attributes["class"] = "tab-pane";
                    }
                    if (!styleRelacionDepTab.Contains("disabled"))
                    {
                        RelacionDepTab.Attributes["class"] = "nav-link";
                        RelacionDep.Attributes["class"] = "tab-pane";
                    }
                    if (!styleSolicitanteTab.Contains("disabled"))
                    {
                        SolicitanteTab.Attributes["class"] = "nav-link";
                        Solicitante.Attributes["class"] = "tab-pane";
                    }
                    if (!styleInstitucionTab.Contains("disabled"))
                    {
                        InstitucionTab.Attributes["class"] = "nav-link";
                        Institucion.Attributes["class"] = "tab-pane";
                    }
                    if (!styleFuncionCargoTab.Contains("disabled"))
                    {
                        FuncionCargoTab.Attributes["class"] = "nav-link";
                        FuncionCargo.Attributes["class"] = "tab-pane";
                    }

                    if (!styleGuardarTab.Contains("disabled"))
                    {
                        GuardarTab.Attributes["class"] = "nav-link";
                        Guardar.Attributes["class"] = "tab-pane";
                    }
                }
                if (tabSetear == "GuardarTab")
                {
                    tab1.Enabled = false;
                    tab6.Enabled = false;
                    GuardarTab.Attributes["class"] = "nav-link active";
                    Guardar.Attributes["class"] = "tab-pane active";

                    if (!styleTipEmisionTab.Contains("disabled"))
                    {
                        TipEmisionTab.Attributes["class"] = "nav-link";
                        TipEmision.Attributes["class"] = "tab-pane";
                    }
                    if (!styleRelacionDepTab.Contains("disabled"))
                    {
                        RelacionDepTab.Attributes["class"] = "nav-link";
                        RelacionDep.Attributes["class"] = "tab-pane";
                    }
                    if (!styleSolicitanteTab.Contains("disabled"))
                    {
                        SolicitanteTab.Attributes["class"] = "nav-link";
                        Solicitante.Attributes["class"] = "tab-pane";
                    }
                    if (!styleInstitucionTab.Contains("disabled"))
                    {
                        InstitucionTab.Attributes["class"] = "nav-link";
                        Institucion.Attributes["class"] = "tab-pane";
                    }
                    if (!styleFuncionCargoTab.Contains("disabled"))
                    {
                        FuncionCargoTab.Attributes["class"] = "nav-link";
                        FuncionCargo.Attributes["class"] = "tab-pane";
                    }
                    if (!styleFotografiaTab.Contains("disabled"))
                    {
                        FotografiaTab.Attributes["class"] = "nav-link";
                        Fotografia.Attributes["class"] = "tab-pane";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected void ddlRelDependencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRelDependencia.SelectedItem.Text == "TITULAR")
            {
                DIV_CARNET_IDENTIDAD.Visible = false;
                gvRelDep.DataSource = null;
                gvRelDep.DataBind();
                hCodigoCarnetTitular.Value = "";
                btnContinuarSolicitante.Enabled = true;
            }
            else {
                DIV_CARNET_IDENTIDAD.Visible = true;
                gvRelDep.DataSource = null;
                gvRelDep.DataBind();
                hCodigoCarnetTitular.Value = "";
                btnContinuarSolicitante.Enabled = false;
            }
        }

        protected void btnBuscarCarnet_Click(object sender, EventArgs e)
        {
            try
            {
                beCarneIdentidad parametros = new beCarneIdentidad();
                parametros.Periodo = 0;//int.Parse(ddlPeriodo.SelectedValue);
                parametros.IdentMesaPartes = "";
                parametros.IdentNumero = 0;
                parametros.CarneNumero = txtCarnet.Text;
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
                parametros.CantReg = 1000;
                brRegistroLinea obrRegistroLinea = new brRegistroLinea();
                beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
                obeCarneIdentidadPrincipal = obrRegistroLinea.obtenerRelacionDependencia(parametros);
                if (obeCarneIdentidadPrincipal.ListaConsulta != null)
                {
                    if (obeCarneIdentidadPrincipal.ListaConsulta.Count == 1)
                    {
                        btnContinuarSolicitante.Enabled = true;
                        hCodigoCarnetTitular.Value = obeCarneIdentidadPrincipal.ListaConsulta[0].CarneIdentidadid.ToString();
                        gvRelDep.DataSource = (obeCarneIdentidadPrincipal.ListaConsulta != null ? obeCarneIdentidadPrincipal.ListaConsulta : null);
                        gvRelDep.DataBind();
                    }
                    else
                    {
                        hCodigoCarnetTitular.Value = "";
                        btnContinuarSolicitante.Enabled = false;
                        gvRelDep.DataSource = null;
                        gvRelDep.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }
        
        protected void gvRelDep_PreRender(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;

            if ((gv.ShowHeader == true && gv.Rows.Count > 0)
                || (gv.ShowHeaderWhenEmpty == true))
            {
                //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
                gv.HeaderRow.TableSection = TableRowSection.TableHeader;
                gv.HeaderRow.CssClass = "thead-light";
            }
            if (gv.ShowFooter == true && gv.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody> - 11/03/2013 - MCR.
                gv.FooterRow.TableSection = TableRowSection.TableFooter;
                gv.HeaderRow.CssClass = "thead-light";
            }
        }

        protected void btnContinuarSolicitante_Click(object sender, EventArgs e)
        {
            LlenarDatosPestañaGrabar();
            DesactivarTodosTabs("SolicitanteTab");
            
        }

        protected void btnContinuarIntitucion_Click(object sender, EventArgs e)
        {
            if (ValidarDatosSolicitanteParaContinuar() == false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('Ingrese los campos obligatorios');", true);
                return;
            }

            bool validar = validarFecha();
            if (validar)
            {
                LlenarDatosPestañaGrabar();
                DesactivarTodosTabs("InstitucionTab");
            }
            else {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('Ingrese la fecha en el siguiente formato: DD/MM/AAAA');", true);
            }
        }
        private void validarFotografia()
        {
            if (hRuta.Value.Length > 0)
            {
                DIV_CARGADO.Visible = true;
                DIV_NO_CARGADO.Visible = false;
            }
            else {
                DIV_NO_CARGADO.Visible = true;
                DIV_CARGADO.Visible = false;
            }
        }
        private bool validarFecha()
        {
            DateTime fecha;

            if (DateTime.TryParse(txtFecNac.Text, out fecha))
            {
                return true;
            }
            else {
                return false;
            }
        }
        private bool ValidarDatosSolicitanteParaContinuar()
        {
            bool resultado = true;
            try
            {
                if (txtApePat.Text.Length == 0)
                {
                    resultado = false;
                }
                if (txtNombres.Text.Length == 0)
                {
                    resultado = false;
                }
                if (ddlTipDoc.SelectedIndex == 0)
                {
                    resultado = false;
                }
                if (txtFecNac.Text.Length == 0)
                {
                    resultado = false;
                }
                if (ddlSexo.SelectedIndex == 0)
                {
                    resultado = false;
                }
                if (ddlEstadoCivil.SelectedIndex == 0)
                {
                    resultado = false;
                }
                if (txtNroDocumentoSolicitante.Text.Length == 0)
                {
                    resultado = false;
                }
                if (txtDomicilio.Text.Length == 0)
                {
                    resultado = false;
                }
                if (ddlNacionalidad.SelectedIndex == 0)
                {
                    resultado = false;
                }
                if (ddlDepartamento.SelectedIndex == 0)
                {
                    resultado = false;
                }
                if (ddlProvincia.SelectedIndex == 0)
                {
                    resultado = false;
                }
                if (ddlDistrito.SelectedIndex == 0)
                {
                    resultado = false;
                }
                if (ddlDistrito.SelectedIndex == 0)
                {
                    resultado = false;
                }
                if (txtCorreo.Text.Length == 0)
                {
                    resultado = false;
                }
                if (txtTelefono.Text.Length == 0)
                {
                    resultado = false;
                }
                return resultado;
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                return resultado;
            }
        }
        protected void btnContinuarFuncionCargo_Click(object sender, EventArgs e)
        {
            if (ValidarDatosInsitucion())
            {
                LlenarDatosPestañaGrabar();
                DesactivarTodosTabs("FuncionCargoTab");
            }
            else{
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('Ingrese los campos obligatorios');", true);
                return;
            }
        }
        private bool ValidarDatosInsitucion()
        {
            bool respuesta = true;

            if (txtInstitucion.Text.Length == 0)
            {
                respuesta = false;
            }
            if (txtPersonaContacto.Text.Length == 0)
            {
                respuesta = false;
            }
            if (txtCargoContacto.Text.Length == 0)
            {
                respuesta = false;
            }
            if (txtCorreoContacto.Text.Length == 0)
            {
                respuesta = false;
            }
            if (txtTelefonoContacto.Text.Length == 0)
            {
                respuesta = false;
            }
            return respuesta;
        }
        protected void btnContinuarFotografia_Click(object sender, EventArgs e)
        {
            LlenarDatosPestañaGrabar();
            if (ValidarDatosInsitucionCargoParaContinuar())
            {
                validarFotografia();
                if (obrGeneral.PermitirFotografia == "SI")
                {
                    DesactivarTodosTabs("FotografiaTab");
                }
                else {
                    DesactivarTodosTabs("GuardarTab");
                }
            }
            else {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('Ingrese los datos de la Insitución');", true);
            }
        }
        private bool ValidarDatosInsitucionCargoParaContinuar()
        {
            bool resultado = true;
            try
            {

                if (ddlTipoInstitucion.SelectedIndex == 0)
                {
                    resultado = false;
                }
                if (ddlCargo.SelectedIndex < 0)
                {
                    resultado = false;
                }
                if (txtOtroCargo.Text.Length == 0)
                {
                    resultado = false;
                }
                return resultado;
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                return resultado;
            }
        }
        protected void btnContinuarGuardar_Click(object sender, EventArgs e)
        {
            bool respuesta = cargaImagen();
            LlenarDatosPestañaGrabar();
            if (cuUploadFile1.FileName.ToString().Length > 0)
            {
                if (!respuesta)
                {
                    return;
                }
            }
            if (hRuta.Value.Length > 0)
            {
                validarFotografia();
                DesactivarTodosTabs("GuardarTab");
            }
            else {
                if (respuesta)
                {
                    validarFotografia();
                    DesactivarTodosTabs("GuardarTab");
                }
                else {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ibtVerImagen", "alert('Debe seleccionar una imagen');", true);
                }
            }           
        }
        private void LlenarDatosPestañaGrabar()
        {
            lblTipomisionFinaltxt.Text = ddlTipEmision.SelectedItem.Text;
            if (txtInstitucion.Text.Length == 0)
            {
                if (ddlTipEmision.SelectedItem.Text == "DUPLICADO")
                {
                    lblDatosInstitucionFinaltxt.Text = "[SIN ESPECIFICAR - DUPLICADO]";
                }
                else {
                    lblDatosInstitucionFinaltxt.Text = "[SIN ESPECIFICAR]";
                }
            }
            else {
                lblDatosInstitucionFinaltxt.Text = txtInstitucion.Text;
            }
            
            lblDatSolicitanteFinaltxt.Text = txtApePat.Text + " " + txtApeMat.Text + " " + txtNombres.Text;
            if (ddlCargo.SelectedItem != null)
            {
                lblFuncionCargoFinaltxt.Text = ddlCargo.SelectedItem.Text + " - " + txtOtroCargo.Text;
            }
            else {
                lblFuncionCargoFinaltxt.Text = "[SIN ESPECIFICAR - DUPLICADO]";
            }
            lblRelDepFinaltxt.Text = ddlRelDependencia.SelectedItem.Text;

            if (hRuta.Value.Length > 0)
            {
                lblFotografiaFinaltxt.Text = "SI";
            }
            else
            {
                lblFotografiaFinaltxt.Text = "NO";
            }
        }
        private bool GuardarArchivoJPG(string SavePath, string fileName)
        {
            bool resultado = false;
            if (cuUploadFile1.HasFile)
            {
                if (cuUploadFile1.ContentType == "application/jpg")
                {
                    if (cuUploadFile1.ContentLength < 5120000)
                    {
                        string Ruta = SavePath;
                        string FilePath;
                        FilePath = Ruta + @"\" + fileName;
                        cuUploadFile1.SaveAs(FilePath);
                        resultado = true;
                    }
                    else
                    {
                        string scriptMensaje = @"swal('Error!', 'Ingrese un archivo de maximo 5 MEGABYTES!.', 'error')";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", scriptMensaje, true);
                        resultado = false;
                    }
                }
                else
                {
                    string scriptMensaje = @"swal('Error!', 'Ingrese un archivo JPG!.', 'error')";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", scriptMensaje, true);
                    resultado = false;
                }
            }
            else
            {
                resultado = false;
            }
            return resultado;
        }
        protected bool cargaImagen()
        {
            try
            {
                bool respuesta = false;
                if (cuUploadFile1.HasFile)
                {
                    FileInfo fi = new FileInfo(cuUploadFile1.FileName);
                    DateTime DtNow = DateTime.Now;
                    string rutaAdjuntos = oCodigoUsuario.getPathAdjuntos();
                    //VALIDAR ARCHIVO ---------------------
                    bool exito = false;
                    bool exitoExtension = false;
                    bool exitoPeso = false;
                    exito = (oCodigoUsuario.validarExtensionArchivo(obrGeneral.fileExtFotografia, fi) ? exitoExtension = true : exitoExtension = false);
                    exito = (oCodigoUsuario.validarPesoArchivo(obrGeneral.filePesoFotografia.ToString(), cuUploadFile1.ContentLength) ? exitoPeso = true : exitoPeso = false);
                    //---------------------------------------
                    if (exitoExtension & exitoPeso)
                    {
                        hNombreArchivo.Value = cuUploadFile1.FileName;
                        //lblPesoArchivo.Text = Math.Round((fulImagen.PostedFile.ContentLength / 1024f) / 1024f, 2).ToString() + " MB";
                        //lblExtension.Text = fi.Extension;
                        Byte[] imageByte = null;
                        imageByte = new Byte[Convert.ToInt32(cuUploadFile1.ContentLength)];
                        //HttpPostedFile ImgFile = cuUploadFile1.PostedFile;

                        using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
                        {
                            imageByte = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                        }
                        string base64String = Convert.ToBase64String(imageByte, 0, imageByte.Length);

                        Session["tempImagenSave"] = base64String;
                        respuesta = true;

                        string fileName = oCodigoUsuario.getFileName();
                        bool exitoFotografia = guardarImagen(rutaAdjuntos, fileName, base64String);
                        hRuta.Value = @"\" + DtNow.Year.ToString() + @"\" + DtNow.Month.ToString("D2") + @"\" + fileName;
                    }
                    else
                    {
                        if (!exitoExtension)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('Debe adjuntar un archivo de formato (JPEG)');", true);
                        }
                        if (!exitoPeso)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('Debe adjuntar un archivo con un peso no mayor a 1 MB');", true);
                        }
                    }
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
                obrGeneral.grabarLog(ex);
                return false;
            }
        }
        private bool guardarImagen(string SavePath, string fileName, string base64String)
        {
            bool exito = false;
            try
            {
                exito = oCodigoUsuario.createDirectory(SavePath);
                if (exito)
                {
                    if (base64String.Length > 0)
                    {
                        // CONVIERTE A STREAM -------------------------------------------------------
                        byte[] imgByte = Convert.FromBase64String(base64String);
                        // VALIDA EL TIPO DE ARCHIVO ------------------------------------------------
                            Regex reg = new Regex(@"(?i).*\.(gif|jpe?g|png|tif)$");
                            string uFile = cuUploadFile1.FileName;
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
            }
            catch (Exception ex)
            {
                exito = false;
                obrGeneral.grabarLog(ex);
            }
            return (exito);
        }
        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                beRegistroLinea SessionGeneral = new beRegistroLinea();
                brRegistroLinea obrRegistroLinea = new brRegistroLinea();
                string ip = oCodigoUsuario.obtenerIP();
                string rutaAdjuntos = oCodigoUsuario.getPathAdjuntos();

                SessionGeneral.RegistroLineaId = Convert.ToInt32(hCodRegistroLinea.Value);
                if (hCodCarnetIdentidad.Value.Length > 0)
                {
                    SessionGeneral.CarneIdentidadId = Convert.ToInt16(hCodCarnetIdentidad.Value);
                }
                SessionGeneral.TipoEmision = Convert.ToInt16(ddlTipEmision.SelectedValue);
                SessionGeneral.DpReldepTitdep = Convert.ToInt16(ddlRelDependencia.SelectedValue);
                if (hCodigoCarnetTitular.Value.Length > 0)
                {
                    SessionGeneral.DpReldepTitular = Convert.ToInt16(hCodigoCarnetTitular.Value);
                }
                SessionGeneral.DpPrimerApellido = txtApePat.Text;
                SessionGeneral.DpSegundoApellido = txtApeMat.Text;
                SessionGeneral.DpNombres = txtNombres.Text;
                if (txtFecNac.Text != "")
                {
                    SessionGeneral.DpFechaNacimiento = Convert.ToDateTime(txtFecNac.Text);
                }
                if (ddlSexo.SelectedIndex > 0)
                {
                    SessionGeneral.DpGeneroId = Convert.ToInt16(ddlSexo.SelectedValue);
                }
                if (ddlEstadoCivil.SelectedIndex > 0)
                {
                    SessionGeneral.DpEstadoCivilId = Convert.ToInt16(ddlEstadoCivil.SelectedValue);
                }
                if (ddlTipDoc.SelectedIndex > 0)
                {
                    SessionGeneral.DpTipoDocIdentidad = Convert.ToInt16(ddlTipDoc.SelectedValue);
                }
                SessionGeneral.DpNumeroDocIdentidad = txtNroDocumentoSolicitante.Text;
                if (ddlNacionalidad.SelectedIndex > 0)
                {
                    SessionGeneral.DpPaisNacionalidad = Convert.ToInt16(ddlNacionalidad.SelectedValue);
                }
            
                SessionGeneral.DpDomicilioPeru = txtDomicilio.Text;
                string ubiGeo = ddlDepartamento.SelectedValue + ddlProvincia.SelectedValue + ddlDistrito.SelectedValue;
                SessionGeneral.DpUbigeo = ubiGeo;
                SessionGeneral.DpCorreoElectronico = txtCorreo.Text;
                SessionGeneral.DpNumeroTelefono = txtTelefono.Text;
                SessionGeneral.DpRutaAdjunto = hRuta.Value;
                if (ddlTipoInstitucion.SelectedValue != "" && ddlTipoInstitucion.SelectedValue != "0")
                {
                    SessionGeneral.InTipoInstitucion = Convert.ToInt16(ddlTipoInstitucion.SelectedValue);
                }
                SessionGeneral.InNombreInstitucion = txtInstitucion.Text;
                SessionGeneral.InPersonaContacto = txtPersonaContacto.Text;
                SessionGeneral.InCargoContacto = txtCargoContacto.Text;
                SessionGeneral.InCorreoElectronico = txtCorreoContacto.Text;
                SessionGeneral.InNumeroTelefono = txtTelefonoContacto.Text;
                if (ddlCargo.SelectedValue != "")
                {
                    SessionGeneral.InTipoCargo = Convert.ToInt16(ddlCargo.SelectedValue);
                }
                SessionGeneral.InCargoNombre = txtOtroCargo.Text;
                SessionGeneral.IpModificacion = ip;

                bool exito = obrRegistroLinea.actualizar(SessionGeneral);
                if (exito)
                {
                    btnVerReporte.Visible = true;
                    btnEnviarSolicitud.Visible = true;
                    btnEditar.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE FINALIZO CON EL REGISTRO CORRECTAMENTE');", true);
                }
                else
                {
                    File.Delete(rutaAdjuntos + hRuta.Value);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL FINALIZAR EL REGISTRO');", true);
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected void btnVerReporte_Click(object sender, EventArgs e)
        {
            try
            {
                    beRegistroLinea SessionGeneral = new beRegistroLinea();
                    SessionGeneral.NumeroRegLinea = hCodSolicitudTexto.Value;
                    beRegistroLinea consultaReporte = buscarSolicitud(SessionGeneral);
                    byte[] pdfByte = oCodigoUsuario.crearPDF1(consultaReporte); //ms.ToArray();
                    Session["bytePDF"] = pdfByte;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirPDF", "window.open('../PDF.aspx', '_blank')", true);
            }
            catch (Exception ex)
            {
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
        protected void btnEnviarSolicitud_Click(object sender, EventArgs e)
        {
            try
            {
                string ip = oCodigoUsuario.obtenerIP();
                beRegistroLinea SessionGeneral = new beRegistroLinea();
                    short idEstado = -1;
                    if (ViewState["Generales"] != null)
                    {
                        beGenerales obeGenerales = (beGenerales)ViewState["Generales"];
                        int pos = obeGenerales.ListaEstadosRegLinea.FindIndex(x => x.DescripcionCorta.Equals("ENVIADO"));
                        if (pos != -1)
                        {
                            idEstado = obeGenerales.ListaEstadosRegLinea[pos].Estadoid;
                        }
                        SessionGeneral.RegistroLineaId = Convert.ToInt32(hCodRegistroLinea.Value);
                        SessionGeneral.EstadoId = idEstado;
                        SessionGeneral.IpModificacion = ip;
                        brRegistroLinea obrRegistroLinea = new brRegistroLinea();
                        bool exito = obrRegistroLinea.actualizarEstado(SessionGeneral);
                        if (exito)
                        {
                            btnEnviarSolicitud.Enabled = false;
                            btnGrabar.Enabled = false;
                            btnVerReporte.Enabled = false;
                            btnEditar.Enabled = false;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE ENVIÓ EL REGISTRO CON ÉXITO');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL ENVIAR EL REGISTRO');", true);
                        }
                    }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                DesactivarTodosTabs("TipEmisionTab");
                btnEditar.Visible = false;
                btnVerReporte.Visible = false;
                btnEnviarSolicitud.Visible = false;
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }

        }

        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            try
            {
                string strNombreFile = "REQUISITOS CARNÉ DE IDENTIDAD 2019 TRC1.pdf";

                string FolderPath = obrGeneral.rutaAdjuntos;
                string strRutaCompleta = FolderPath + @"\" + strNombreFile;

                FileStream fs = new FileStream(strRutaCompleta, FileMode.Open, FileAccess.Read);
                byte[] fileData;
                fileData = new byte[fs.Length + 1];
                long bytesRead = fs.Read(fileData, 0, System.Convert.ToInt32(fs.Length));
                fs.Close();
                Session["bytePDF"] = fileData;

                string _open = "window.open('../PDF.aspx', '_newtab');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                //// Se borran las cabeceras actuales de HTTP
                //Response.ClearContent();
                //Response.ClearHeaders();

                //// Se establecen las cabeceras correspondientes
                //// Nombre del archivo para que el usuario no vea download.aspx en el cuadro de descarga
                //Response.AddHeader("Content-Disposition", "attachment;filename=REQUISITOS.pdf");

                //// Tipo MIME del archivo a descargar. Si tienen diferentes tipos de archivos hacen un case
                //// Response.ContentType = "application/msword"
                //Response.ContentType = "application/pdf";

                //// Cabecera que establece el tamaño de la respuesta (tamaño del archivo en bytes)
                //Response.AddHeader("Content-length", bytesRead.ToString());

                //// Se escribe la respuesta al usuario que verá la ventana de descarga del archivo.
                //Response.BinaryWrite(fileData);
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ AL DESCARGAR EL ARCHIVO');", true);
            }
        }
    }
}