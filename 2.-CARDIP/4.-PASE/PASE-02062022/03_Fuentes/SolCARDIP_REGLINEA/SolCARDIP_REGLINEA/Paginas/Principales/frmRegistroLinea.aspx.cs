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
using System.Configuration;
using System.Diagnostics;
namespace SolCARDIP_REGLINEA.Paginas.Principales
{
    public partial class frmRegistroLinea : System.Web.UI.Page
    {
        brGeneral obrGeneral = new brGeneral();
        CodigoUsuario oCodigoUsuario = new CodigoUsuario();
        string conInterConexionSAM = ConfigurationManager.AppSettings["conInterConexionSAM"].ToString().ToUpper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Request.Params["__EVENTTARGET"] == "ACTUALIZA")
            {
                Response.Redirect("RegistroLinea.aspx", true);
            }
            if (!(IsPostBack))
            {
                if (!Convert.ToBoolean(Session["Verifica"]))
                {
                    Response.Redirect("~/Verificar.aspx");
                    return;
                }
                OcultarDivNOCARGADO();
                brGenerales obrGenerales = new brGenerales();
                beGenerales obeGenerales = obrGenerales.obtenerGenerales();
                if (obeGenerales != null)
                {
                    ViewState["Generales"] = obeGenerales;
                }
                

                Session["tempImagenSave"] = null;
                beRegistroLinea SessionGeneral = new beRegistroLinea();
                SessionGeneral = (beRegistroLinea)Session["SessionGeneral"];
                Session["SessionGeneral"] = null;
                CargarListas(SessionGeneral, obeGenerales);

                if (SessionGeneral != null)
                {
                    if (SessionGeneral.TipoEmisionObject == "EDITAR")
                    {
                        cargarDatosEditar(SessionGeneral, obeGenerales);
                    }
                }

            }
            else
            {

                //System.Diagnostics.Debug.Write("1_____________d");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "clientScript", "$('#ContentPlaceHolder1_lEliminarFirma').removeClass('d-none')", true);
                //System.Diagnostics.Debug.Write("1_____________d");

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

            ddlCategoriaInstitucion.SelectedValue = SessionGeneral.COD_CATEGORIA_MISION.ToString();
            seleccionarCategoriaOficina(null, null);
            ddlInstitucion.SelectedValue = SessionGeneral.COD_INSTITUCION.ToString();
            txtPersonaContacto.Text = SessionGeneral.InPersonaContacto;
            txtCorreoContacto.Text = SessionGeneral.InCorreoElectronico;
            txtCargoContacto.Text = SessionGeneral.InCargoContacto;
            txtCorreoContacto.Text = SessionGeneral.InCorreoElectronico;
            txtTelefonoContacto.Text = SessionGeneral.InNumeroTelefono;

            ddlTipoCargo.SelectedValue = SessionGeneral.TIPO_CALIDAD_MIGRATORIA.ToString();
            SeleccionarCargo(null, null);
            ddlCargo.SelectedValue = SessionGeneral.COD_CARGO.ToString();
            txtNroDoc.Text = SessionGeneral.numeroCanet;

            hRuta.Value = SessionGeneral.DpRutaAdjunto;
            hRutaFirma.Value = SessionGeneral.DpRutaFirma;
            hRutaPasaporte.Value = SessionGeneral.DpRutaPasaporte;
            hRutaDenuncia.Value = SessionGeneral.DpRutaDenunciaPolicial;
            if (hRuta.Value.Length > 0)
            {
                LlenarDatosPestañaGrabar();
                validarFotografia();
                cargarImagenAlmacenada(obrGeneral.rutaAdjuntos + hRuta.Value);
            }
            if (hRutaFirma.Value.Length > 0)
            {
                validarFirma();
                cargarFirmaAlmacenada(obrGeneral.rutaAdjuntos + hRutaFirma.Value);
            }
            if (hRutaPasaporte.Value.Length > 0)
            {
                validarPasaporte();
                cargarPasaporteAlmacenada(obrGeneral.rutaAdjuntos + hRutaPasaporte.Value);
            }
            if (hRutaDenuncia.Value.Length > 0)
            {
                validarDenuncia();
                cargarDenunciaAlmacenada(obrGeneral.rutaAdjuntos + hRutaDenuncia.Value);
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
                DesactivarTodosTabs("FirmaTab");
                DesactivarTodosTabs("PasaporteTab");
                DesactivarTodosTabs("DenunciaTab");
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
                    int carneID = SessionGeneral.DpReldepTitular;
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
            listaDocIdent = listaDocIdent.FindAll(x => x.DescripcionCorta != "DNI");
            //listaDocIdent.Remove()
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
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
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
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
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
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
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
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
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
                    lblNumeroSolicitudtxt.Text = SessionGeneral.NumeroRegLinea;
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
                //obeGenerales.ListaTipoInstitucion.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                //ddlTipoCargo.DataSource = obeGenerales.ListaTipoInstitucion;
                //ddlTipoCargo.DataValueField = "Parametroid";
                //ddlTipoCargo.DataTextField = "Descripcion";
                //ddlTipoCargo.DataBind();

                // CALIDAD MIGRATORIA
                obeGenerales.ListaCalidadMigratoriaNivelPrincipal.Insert(0, new beCalidadMigratoria { CalidadMigratoriaid = 0, Nombre = "<Seleccione>" });
                ddlTipoCargo.DataSource = obeGenerales.ListaCalidadMigratoriaNivelPrincipal;
                ddlTipoCargo.DataValueField = "CalidadMigratoriaid";
                ddlTipoCargo.DataTextField = "Nombre";
                ddlTipoCargo.DataBind();

                ddlCargo.DataSource = null;
                ddlCargo.DataBind();

                cargarComboNull(ddlTipDoc);

                // CATEGORIA OFICINA EXTRANJERA
                obeGenerales.CategoriaOficinaExtranjera.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                ddlCategoriaInstitucion.DataSource = obeGenerales.CategoriaOficinaExtranjera;
                ddlCategoriaInstitucion.DataValueField = "Parametroid";
                ddlCategoriaInstitucion.DataTextField = "Descripcion";
                ddlCategoriaInstitucion.DataBind();

                ddlInstitucion.DataSource = null;
                ddlInstitucion.DataBind();

                cargarComboNull(ddlInstitucion);

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
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        //protected void cargarTipoCargo(object sender, EventArgs e)
        //{
        //    beGenerales obeGenerales = (beGenerales)ViewState["Generales"];
        //    List<beParametro> listaTipoCargo = new List<beParametro>();
        //    string descTipo = oCodigoUsuario.detectarCaracterEspecial(ddlTipoCargo.SelectedItem.Text);
        //    int pos = obeGenerales.ListaTipoInstitucion.FindIndex(x => x.Descripcion.Equals(descTipo));
        //    string valor = obeGenerales.ListaTipoInstitucion[pos].Valor;
        //    listaTipoCargo = obeGenerales.ListaCargoInstitucion.FindAll(x => x.Valor.Equals(valor) && !x.Descripcion.Equals("HUMANITARIA"));
        //    ddlCargo.DataValueField = "Parametroid";
        //    ddlCargo.DataTextField = "Descripcion";
        //    ddlCargo.DataSource = listaTipoCargo;
        //    ddlCargo.DataBind();
        //}
        protected void SeleccionarCargo(object sender, EventArgs e) // DROPDOWLIST - A LA SELECCION DE CALIDAD MUESTRA LOS CARGOS DISPONIBLES PARA ESA MISMA
        {
            try
            {
                if (ViewState["Generales"] != null)
                {
                    beGenerales obeGenerales = new beGenerales();
                    obeGenerales = (beGenerales)ViewState["Generales"];
                    List<beCalidadMigratoria> list = obeGenerales.ListaCalidadMigratoriaNivelSecundario;
                    Boolean existe = false;
                    for(int i=0;i< list.Count; i++)
                    {
                        beCalidadMigratoria b = list[i];
                        if (b.CalidadMigratoriaid == 0)
                        {
                            existe = true;
                        }
                    }
                    if (existe == false)
                    {
                        
                        obeGenerales.ListaCalidadMigratoriaNivelSecundario.Insert(0, new beCalidadMigratoria { CalidadMigratoriaid = 0, Nombre = "<Seleccione>" });
                    }

                    
                    List<beCalidadMigratoria> lbeCalidadMigratoria = new List<beCalidadMigratoria>();
                    //int pos = obeGenerales.TitularDependienteParametros.FindIndex(x => x.Valor.Equals(hdrbt.Value));
                    short Referencia = short.Parse(ddlTipoCargo.SelectedValue);
                    short TitularDependiente = short.Parse(ddlRelDependencia.SelectedValue);
                    short genero = short.Parse(ddlSexo.SelectedValue);
                    if (TitularDependiente > 0 & genero > 0)
                    {
                        lbeCalidadMigratoria = oCodigoUsuario.obtenerListaTitularDependiente(Referencia, TitularDependiente, genero, obeGenerales.ListaCalidadMigratoriaNivelSecundario);
                    }
                    if (lbeCalidadMigratoria.Count > 0)
                    {
                        if (ddlCargo != null)
                        {
                            ddlCargo.SelectedValue = "0";
                        }
                        ddlCargo.DataSource = lbeCalidadMigratoria;
                        ddlCargo.DataValueField = "CalidadMigratoriaid";
                        ddlCargo.DataTextField = "Nombre";
                        ddlCargo.DataBind();
                        ddlCargo.Enabled = true;
                    }
                    else
                    {
                        cargarComboNull(ddlCargo);
                        ddlCargo.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
            }
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
                DIV_MOTIVO.Visible = false;
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
                DIV_MOTIVO.Visible = false;
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
                DIV_MOTIVO.Visible = true;
            }
        }
        protected void btnCancelar_click(object sender, EventArgs e)
        {
            //Response.Redirect(VirtualPathUtility.ToAbsolute("~/verificar.aspx"), false);
            //Paginas / Principales / frmRegistroLinea.aspx
            Response.Redirect("RegistroLinea.aspx", true);
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
                        if (ddlTipEmision.SelectedItem.Text == "DUPLICADO")
                        {
                            if (obeCarneIdentidadPrincipal.CarneIdentidad.Estado == "INHABILITADO" || obeCarneIdentidadPrincipal.CarneIdentidad.Estado == "VENCIDO")
                            {
                                Respuesta.Visible = true;
                                btnContinuar.Enabled = false;
                                lblResuesta.Text = "El carné " + parametros.CarneNumero + " Se encuentra " + obeCarneIdentidadPrincipal.CarneIdentidad.Estado + ".";
                            }
                            else
                            {
                                cargarDatosDuplicadoRenovacion(obeCarneIdentidadPrincipal);
                                Respuesta.Visible = true;
                                btnContinuar.Enabled = true;
                                lblResuesta.Text = "Se encontró el carné " + parametros.CarneNumero + ". Puede continuar.";
                            }
                        }
                        else {
                            cargarDatosDuplicadoRenovacion(obeCarneIdentidadPrincipal);
                            Respuesta.Visible = true;
                            btnContinuar.Enabled = true;
                            lblResuesta.Text = "Se encontró el carné " + parametros.CarneNumero + ". Puede continuar.";
                        }
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
            bool error = false;
            /*=======quitar conInterConexionSAM.Equals("SI") && del if, una ves ok el WS de SAM */
            if (conInterConexionSAM.Equals("SI") && (ddlTipEmision.SelectedItem.Text.Equals("NUEVO") || ddlTipEmision.SelectedItem.Text.Equals("RENOVACIÓN") || ddlTipEmision.SelectedItem.Text.Equals("DUPLICADO")) && ddlRelDependencia.SelectedItem.Text.Equals("TITULAR") && txtNroDocumentoSolicitante.Text.Equals(""))
            {
                //carga datos obtenidos desde el webservice de SAM, segun sugerencia de TRC: se debe prevalecer los datos de SAM por lo q es mas actual
                cargarDatosDeSam();

            }
            else {
                //carga datos obtenidos desde BD cardip
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
                
                try
                {
                    beGenerales obeGenerales = (beGenerales)ViewState["Generales"];
                    if (obeGenerales != null)
                    {
                        
                        ddlTipDoc.SelectedValue = carneIdentidad.PersonaIdentificacion.Documentotipoid.ToString();
                    }
                }
                catch
                {
                    ddlTipDoc.Style.Add("border", "solid Red 1px");
                    error = true;
                }
                txtNroDocumentoSolicitante.Text = carneIdentidad.PersonaIdentificacion.Documentonumero;
                 
            
            }
            

            txtDomicilio.Text = carneIdentidad.Residencia.Residenciadireccion;
            txtCorreo.Text = carneIdentidad.Persona.Correoelectronico;
            string ubigeo = carneIdentidad.UbiGeo.Ubi01 + carneIdentidad.UbiGeo.Ubi02 + carneIdentidad.UbiGeo.Ubi03;
            separarUbigeo(ubigeo, null, null);
            txtTelefono.Text = carneIdentidad.Persona.Telefono;
            try
            {
                ddlRelDependencia.SelectedValue = carneIdentidad.CalidadMigratoriaSec.FlagTitularDependiente.ToString();
            }
            catch
            {
                ddlRelDependencia.Style.Add("border", "solid Red 1px");
                error = true;
            }
            ddlRelDependencia_SelectedIndexChanged(null, null);
            try
            {
                ddlCategoriaInstitucion.SelectedValue = carneIdentidad.OficinaConsularExtranjera.Categoriaid.ToString();
            }
            catch
            {
                ddlCategoriaInstitucion.Style.Add("border", "solid Red 1px");
                error = true;
            }

            seleccionarCategoriaOficina(null, null);
            try
            {
                ddlInstitucion.SelectedValue = carneIdentidad.OficinaConsularExtranjera.OficinaconsularExtranjeraid.ToString();
            }
            catch
            {
                ddlInstitucion.Style.Add("border", "solid Red 1px");
                error = true;
            }

            try
            {
                ddlTipoCargo.SelectedValue = carneIdentidad.CalidadMigratoriaPri.CalidadMigratoriaid.ToString();
            }
            catch
            {
                ddlTipoCargo.Style.Add("border", "solid Red 1px");
                error = true;
            }

            SeleccionarCargo(null, null);
            try
            {
                ddlCargo.SelectedValue = carneIdentidad.CalidadMigratoriaSec.CalidadMigratoriaid.ToString();
            }
            catch
            {
                ddlCargo.Style.Add("border", "solid Red 1px");
                error = true;
            }

            hRuta.Value = carneIdentidad.CarneIdentidad.RutaArchivoFoto;
            hRutaFirma.Value = carneIdentidad.CarneIdentidad.RutaArchivoFirma;
            hRutaPasaporte.Value = carneIdentidad.CarneIdentidad.RutaArchivoPasaporte;
            hRutaDenuncia.Value = carneIdentidad.CarneIdentidad.RutaArchivoDenuncia;
            validarFotografia();
            validarFirma();
            validarPasaporte();
            //validarDenuncia();
            cargarImagenAlmacenada(obrGeneral.rutaAdjuntos + carneIdentidad.CarneIdentidad.RutaArchivoFoto);
            cargarFirmaAlmacenada(obrGeneral.rutaAdjuntos + carneIdentidad.CarneIdentidad.RutaArchivoFirma);
            cargarPasaporteAlmacenada(obrGeneral.rutaAdjuntos + carneIdentidad.CarneIdentidad.RutaArchivoPasaporte);
            cargarDenunciaAlmacenada(obrGeneral.rutaAdjuntos + carneIdentidad.CarneIdentidad.RutaArchivoDenuncia);

            if (!error)
            {
                ddlTipDoc.Style.Add("border", "solid #888888 1px");
                ddlRelDependencia.Style.Add("border", "solid #888888 1px");
                ddlCategoriaInstitucion.Style.Add("border", "solid #888888 1px");
                ddlInstitucion.Style.Add("border", "solid #888888 1px");
                ddlTipoCargo.Style.Add("border", "solid #888888 1px");
                ddlCargo.Style.Add("border", "solid #888888 1px");
            }

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
                    
                    imgFoto.ImageUrl = "data:image/png;base64," + base64String;
                }
            }
            catch (Exception ex)
            {
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        protected void cargarFirmaAlmacenada(string Path)
        {
            try
            {
                if (File.Exists(Path))
                {
                    Byte[] imageByte = null;
                    imageByte = File.ReadAllBytes(Path);
                    string base64String = Convert.ToBase64String(imageByte, 0, imageByte.Length);
                    Session["tempFirmaSave"] = base64String;
                    imgFirma.ImageUrl = "data:image/png;base64," + base64String;
                }
            }
            catch (Exception ex)
            {
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        protected void cargarDenunciaAlmacenada(string Path)
        {
            try
            {
                if (File.Exists(Path))
                {
                    Byte[] imageByte = null;
                    imageByte = File.ReadAllBytes(Path);
                    string base64String = Convert.ToBase64String(imageByte, 0, imageByte.Length);
                    Session["tempDenunciaSave"] = base64String;
                }
            }
            catch (Exception ex)
            {
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        protected void cargarPasaporteAlmacenada(string Path)
        {
            try
            {
                if (File.Exists(Path))
                {
                    Byte[] imageByte = null;
                    imageByte = File.ReadAllBytes(Path);
                    string base64String = Convert.ToBase64String(imageByte, 0, imageByte.Length);
                    Session["tempPasaporteSave"] = base64String;
                    imgPasaporte.ImageUrl = "data:image/png;base64," + base64String;
                }
            }
            catch (Exception ex)
            {
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
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
                if (txtCorreo.Text == "")
                {
                    DesactivarTodosTabs("SolicitanteTab");
                    txtApeMat.Enabled = false;
                    txtNombres.Enabled = false;
                    txtFecNac.Enabled = false;
                    if (ddlSexo.SelectedIndex == 0)
                    {
                        ddlSexo.Enabled = true;
                    }
                    else {
                        ddlSexo.Enabled = false;
                    }
                    if (ddlEstadoCivil.SelectedIndex == 0)
                    {
                        ddlEstadoCivil.Enabled = true;
                    }
                    else {
                        ddlEstadoCivil.Enabled = false;
                    }
                    if (ddlNacionalidad.SelectedIndex == 0)
                    {
                        ddlNacionalidad.Enabled = true;
                    }
                    else
                    {
                        ddlNacionalidad.Enabled = false;
                    }
                    if (ddlTipDoc.SelectedIndex == 0)
                    {
                        ddlTipDoc.Enabled = true;
                    }
                    else {
                        ddlTipDoc.Enabled = false;
                    }
                    txtNroDocumentoSolicitante.Enabled = false;
                    txtDomicilio.Enabled = false;
                    if (ddlProvincia.SelectedIndex == 0)
                    {
                        ddlProvincia.Enabled = true;
                    }
                    else { ddlProvincia.Enabled = false; }
                    if (ddlDepartamento.SelectedIndex == 0)
                    {
                        ddlDepartamento.Enabled = true;
                    }
                    else { ddlDepartamento.Enabled = false; }
                    if (ddlDistrito.SelectedIndex == 0)
                    {
                        ddlDistrito.Enabled = true;
                    }
                    else { ddlDistrito.Enabled = false; }
                    
                    txtCorreo.Enabled = true;
                    txtTelefono.Enabled = false;
                    txtTipoVisa.Enabled = false;
                    txtTipoVisaId.Enabled = false;
                    txtVisa.Enabled = false;
                    txtVisaId.Enabled = false;
                    txtTitularFamiliar.Enabled = false;
                    txtTitularFamiliarId.Enabled = false;
                    txtTiempoPermanencia.Enabled = false;
                }
                else {
                    DesactivarTodosTabs("InstitucionTab");
                }
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
                string styleFirmaTab = FirmaTab.Attributes["class"].ToString();
                string stylePasaporteTab = PasaporteTab.Attributes["class"].ToString();
                string styleDenunciaTab = DenunciaTab.Attributes["class"].ToString();
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
                    tab7.Enabled = true;
                    tab8.Enabled = true;
                    tab9.Enabled = true;

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

                    FirmaTab.Attributes["class"] = "nav-link disabled";
                    Firma.Attributes["class"] = "tab-pane disabled";

                    PasaporteTab.Attributes["class"] = "nav-link disabled";
                    Pasaporte.Attributes["class"] = "tab-pane disabled";

                    DenunciaTab.Attributes["class"] = "nav-link disabled";
                    Denuncia.Attributes["class"] = "tab-pane disabled";

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

                    if (!styleFirmaTab.Contains("disabled"))
                    {
                        FirmaTab.Attributes["class"] = "nav-link";
                        Firma.Attributes["class"] = "tab-pane";
                    }
                    if (!stylePasaporteTab.Contains("disabled"))
                    {
                        PasaporteTab.Attributes["class"] = "nav-link";
                        Pasaporte.Attributes["class"] = "tab-pane";
                    }
                    if (!styleDenunciaTab.Contains("disabled"))
                    {
                        DenunciaTab.Attributes["class"] = "nav-link";
                        Denuncia.Attributes["class"] = "tab-pane";
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
                    if (!styleFirmaTab.Contains("disabled"))
                    {
                        FirmaTab.Attributes["class"] = "nav-link";
                        Firma.Attributes["class"] = "tab-pane";
                    }
                    if (!stylePasaporteTab.Contains("disabled"))
                    {
                        PasaporteTab.Attributes["class"] = "nav-link";
                        Pasaporte.Attributes["class"] = "tab-pane";
                    }
                    if (!styleDenunciaTab.Contains("disabled"))
                    {
                        DenunciaTab.Attributes["class"] = "nav-link";
                        Denuncia.Attributes["class"] = "tab-pane";
                    }
                    if (!styleGuardarTab.Contains("disabled"))
                    {
                        GuardarTab.Attributes["class"] = "nav-link";
                        Guardar.Attributes["class"] = "tab-pane";
                    }
                }
                if (tabSetear == "InstitucionTab")
                {
                    tab1.Enabled = false;
                    tab2.Enabled = false;
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
                    if (!styleFirmaTab.Contains("disabled"))
                    {
                        FirmaTab.Attributes["class"] = "nav-link";
                        Firma.Attributes["class"] = "tab-pane";
                    }
                    if (!stylePasaporteTab.Contains("disabled"))
                    {
                        PasaporteTab.Attributes["class"] = "nav-link";
                        Pasaporte.Attributes["class"] = "tab-pane";
                    }
                    if (!styleDenunciaTab.Contains("disabled"))
                    {
                        DenunciaTab.Attributes["class"] = "nav-link";
                        Denuncia.Attributes["class"] = "tab-pane";
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
                    if (!styleFirmaTab.Contains("disabled"))
                    {
                        FirmaTab.Attributes["class"] = "nav-link";
                        Firma.Attributes["class"] = "tab-pane";
                    }
                    if (!stylePasaporteTab.Contains("disabled"))
                    {
                        PasaporteTab.Attributes["class"] = "nav-link";
                        Pasaporte.Attributes["class"] = "tab-pane";
                    }
                    if (!styleDenunciaTab.Contains("disabled"))
                    {
                        DenunciaTab.Attributes["class"] = "nav-link";
                        Denuncia.Attributes["class"] = "tab-pane";
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

                    if (!styleFirmaTab.Contains("disabled"))
                    {
                        FirmaTab.Attributes["class"] = "nav-link";
                        Firma.Attributes["class"] = "tab-pane";
                    }
                    if (!stylePasaporteTab.Contains("disabled"))
                    {
                        PasaporteTab.Attributes["class"] = "nav-link";
                        Pasaporte.Attributes["class"] = "tab-pane";
                    }
                    if (!styleDenunciaTab.Contains("disabled"))
                    {
                        DenunciaTab.Attributes["class"] = "nav-link";
                        Denuncia.Attributes["class"] = "tab-pane";
                    }

                    if (!styleGuardarTab.Contains("disabled"))
                    {
                        GuardarTab.Attributes["class"] = "nav-link";
                        Guardar.Attributes["class"] = "tab-pane";
                    }
                }
                if (tabSetear == "FirmaTab")
                {
                    tab6.Enabled = false;
                    FirmaTab.Attributes["class"] = "nav-link active";
                    Firma.Attributes["class"] = "tab-pane active";

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

                    if (!stylePasaporteTab.Contains("disabled"))
                    {
                        PasaporteTab.Attributes["class"] = "nav-link";
                        Pasaporte.Attributes["class"] = "tab-pane";
                    }
                    if (!styleDenunciaTab.Contains("disabled"))
                    {
                        DenunciaTab.Attributes["class"] = "nav-link";
                        Denuncia.Attributes["class"] = "tab-pane";
                    }

                    if (!styleGuardarTab.Contains("disabled"))
                    {
                        GuardarTab.Attributes["class"] = "nav-link";
                        Guardar.Attributes["class"] = "tab-pane";
                    }
                }
                if (tabSetear == "PasaporteTab")
                {
                    tab7.Enabled = false;
                    PasaporteTab.Attributes["class"] = "nav-link active";
                    Pasaporte.Attributes["class"] = "tab-pane active";

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
                    if (!styleFirmaTab.Contains("disabled"))
                    {
                        FirmaTab.Attributes["class"] = "nav-link";
                        Firma.Attributes["class"] = "tab-pane";
                    }

                    if (!styleDenunciaTab.Contains("disabled"))
                    {
                        DenunciaTab.Attributes["class"] = "nav-link";
                        Denuncia.Attributes["class"] = "tab-pane";
                    }

                    if (!styleGuardarTab.Contains("disabled"))
                    {
                        GuardarTab.Attributes["class"] = "nav-link";
                        Guardar.Attributes["class"] = "tab-pane";
                    }
                }

                if (tabSetear == "DenunciaTab")
                {
                    tab4.Enabled = false;
                    tab8.Enabled = false;
                    DenunciaTab.Attributes["class"] = "nav-link active";
                    Denuncia.Attributes["class"] = "tab-pane active";

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
                    if (!styleFirmaTab.Contains("disabled"))
                    {
                        FirmaTab.Attributes["class"] = "nav-link";
                        Firma.Attributes["class"] = "tab-pane";
                    }
                    if (!stylePasaporteTab.Contains("disabled"))
                    {
                        PasaporteTab.Attributes["class"] = "nav-link";
                        Pasaporte.Attributes["class"] = "tab-pane";
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
                    tab2.Enabled = false;
                    tab3.Enabled = false;
                    tab4.Enabled = false;
                    tab5.Enabled = false;
                    tab6.Enabled = false;
                    tab7.Enabled = false;
                    tab8.Enabled = false;
                    tab9.Enabled = false;
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
                    if (!styleFirmaTab.Contains("disabled"))
                    {
                        FirmaTab.Attributes["class"] = "nav-link";
                        Firma.Attributes["class"] = "tab-pane";
                    }
                    if (!stylePasaporteTab.Contains("disabled"))
                    {
                        PasaporteTab.Attributes["class"] = "nav-link";
                        Pasaporte.Attributes["class"] = "tab-pane";
                    }
                    if (!styleDenunciaTab.Contains("disabled"))
                    {
                        DenunciaTab.Attributes["class"] = "nav-link";
                        Denuncia.Attributes["class"] = "tab-pane";
                        lkbVerDenuncia.Visible = true;
                    }
                    else
                    {
                        lkbVerDenuncia.Visible = false;
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

                SolCARDIP_REGLINEA.Librerias.EntidadesNegocio.bePersona be = (SolCARDIP_REGLINEA.Librerias.EntidadesNegocio.bePersona)Session["bePersona"];
                string nombres = !be.Nombres.Equals("")?(be.Apellidopaterno + " " + be.Apellidomaterno).Trim() + ", " + be.Nombres  : "";
                DIV_TITULAR_NOMBRES.InnerHtml = nombres;
                if (nombres.Equals(""))
                {
                    div_row_titular.Visible = false;
                }
                else {
                    div_row_titular.Visible = true;
                }
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
                        hApePatTitular.Value = obeCarneIdentidadPrincipal.ListaConsulta[0].ApePatPersona.ToString();
                        hApeMatTitular.Value = obeCarneIdentidadPrincipal.ListaConsulta[0].ApeMatPersona.ToString();
                        hNomTitular.Value = obeCarneIdentidadPrincipal.ListaConsulta[0].NombresPersona.ToString();
                        hCalidadMigratoriaTitular.Value = obeCarneIdentidadPrincipal.ListaConsulta[0].ConCalidadMigratoria.ToString();
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
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
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
                btnEliminarFoto.Visible = true;
                //imgFoto.ImageUrl = obrGeneral.rutaAdjuntos + hRuta.Value;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "$('#ContentPlaceHolder1_btnEliminarFoto').removeClass('d-none')", true);
            }
            else {
                DIV_NO_CARGADO.Visible = true;
                DIV_CARGADO.Visible = false;
                btnEliminarFoto.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "$('#ContentPlaceHolder1_btnEliminarFoto').addClass('d-none')", true);
            }
        }
        private void validarPasaporte()
        {
            if (hRutaPasaporte.Value.Length > 0)
            {
                DIV_CARGADO_PASAPORTE.Visible = true;
                DIV_NO_CARGADO_PASAPORTE.Visible = false;
                btnEliminarPasaporte.Visible = true;
            }
            else
            {
                DIV_NO_CARGADO_PASAPORTE.Visible = true;
                DIV_CARGADO_PASAPORTE.Visible = false;
                btnEliminarPasaporte.Visible = false;
            }
        }

        private void validarDenuncia()
        {
            if (hRutaDenuncia.Value.Length > 0)
            {
                DIV_CARGADO_DENUNCIA.Visible = true;
                DIV_NO_CARGADO_DENUNCIA.Visible = false;
            }
            else
            {
                DIV_NO_CARGADO_DENUNCIA.Visible = true;
                DIV_CARGADO_DENUNCIA.Visible = false;
            }
        }
        private void validarFirma()
        {
            if (hRutaFirma.Value.Length > 0)
            {
                DIV_CARGADO_FIRMA.Visible = true;
                DIV_NO_CARGADO_FIRMA.Visible = false;
                btnEliminarFirma.Visible = true;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "clientScript", "$('#ContentPlaceHolder1_lEliminarFirma').removeClass('d-none')", true);
            }
            else
            {
                DIV_NO_CARGADO_FIRMA.Visible = true;
                DIV_CARGADO_FIRMA.Visible = false;
                btnEliminarFirma.Visible = false;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "clientScript", "$('#ContentPlaceHolder1_lEliminarFirma').addClass('d-none')", true);
            }
        }

        private void OcultarDivNOCARGADO()
        {
            DIV_NO_CARGADO_FIRMA.Visible = false;
            DIV_NO_CARGADO.Visible = false;
            DIV_NO_CARGADO_DENUNCIA.Visible = false;
            DIV_NO_CARGADO_PASAPORTE.Visible = false;
        }
        private bool validarFecha()
        {
            DateTime fecha;

            if (DateTime.TryParse(txtFecNac.Text, out fecha))
            {
                Color color = System.Drawing.ColorTranslator.FromHtml("#d1d3e2");
                txtFecNac.BorderColor = color;
                return true;

            }
            else {
                txtFecNac.BorderColor = System.Drawing.Color.Red;
                return false;
               
               
               
            }
        }
        private bool ValidarDatosSolicitanteParaContinuar()
        {
            bool resultado = true;
            try
            {
                Color color=System.Drawing.ColorTranslator.FromHtml("#d1d3e2");
                txtApePat.BorderColor = color;
                if (txtApePat.Text.Length == 0)
                {
                    resultado = false;
                    txtApePat.BorderColor = System.Drawing.Color.Red;
                }
                txtNombres.BorderColor = color;
                if (txtNombres.Text.Length == 0)
                {
                    resultado = false;
                    txtNombres.BorderColor = System.Drawing.Color.Red;
                }
                ddlTipDoc.BorderColor = color;
                if (ddlTipDoc.SelectedIndex == 0)
                {
                    resultado = false;
                    ddlTipDoc.BorderColor = System.Drawing.Color.Red;
                }
                txtFecNac.BorderColor = color;
                if (txtFecNac.Text.Length == 0)
                {
                    resultado = false;
                    txtFecNac.BorderColor = System.Drawing.Color.Red;
                }
                ddlSexo.BorderColor = color;
                if (ddlSexo.SelectedIndex == 0)
                {
                    resultado = false;
                    ddlSexo.BorderColor = System.Drawing.Color.Red;
                }
                ddlEstadoCivil.BorderColor = color;
                if (ddlEstadoCivil.SelectedIndex == 0)
                {
                    resultado = false;
                    ddlEstadoCivil.BorderColor = System.Drawing.Color.Red;
                }
                txtNroDocumentoSolicitante.BorderColor = color;
                if (txtNroDocumentoSolicitante.Text.Length == 0)
                {
                    resultado = false;
                    txtNroDocumentoSolicitante.BorderColor = System.Drawing.Color.Red;
                }
                txtDomicilio.BorderColor = color;
                if (txtDomicilio.Text.Length == 0)
                {
                    resultado = false;
                    txtDomicilio.BorderColor = System.Drawing.Color.Red;
                }
                ddlNacionalidad.BorderColor = color;
                if (ddlNacionalidad.SelectedIndex == 0)
                {
                    resultado = false;
                    ddlNacionalidad.BorderColor = System.Drawing.Color.Red;
                }
                ddlDepartamento.BorderColor = color;
                if (ddlDepartamento.SelectedIndex == 0)
                {
                    resultado = false;
                    ddlDepartamento.BorderColor = System.Drawing.Color.Red;
                }
                ddlProvincia.BorderColor = color;
                if (ddlProvincia.SelectedIndex == 0)
                {
                    resultado = false;
                    ddlProvincia.BorderColor = System.Drawing.Color.Red;
                }
                ddlDistrito.BorderColor = color;
                if (ddlDistrito.SelectedIndex == 0)
                {
                    resultado = false;
                    ddlDistrito.BorderColor = System.Drawing.Color.Red;
                }
                txtCorreo.BorderColor = color;
                if (txtCorreo.Text.Length == 0)
                {
                    resultado = false;
                    txtCorreo.BorderColor = System.Drawing.Color.Red;
                }
                txtTelefono.BorderColor = color;
                if (txtTelefono.Text.Length == 0)
                {
                    resultado = false;
                    txtTelefono.BorderColor = System.Drawing.Color.Red;
                }
                return resultado;
            }
            catch (Exception ex)
            {
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return resultado;
            }
        }
        protected void btnContinuarFuncionCargo_Click(object sender, EventArgs e)
        {
            if (ValidarDatosInsitucion())
            {
                if (ddlTipEmision.SelectedItem.Text == "DUPLICADO")
                {
                    LlenarDatosPestañaGrabar();
                    if (ddlMotivo.SelectedItem.Text == " PERDIDA O ROBO")
                    {
                        DesactivarTodosTabs("DenunciaTab");
                    }
                    else
                    {
                        DesactivarTodosTabs("GuardarTab");
                    }
                }
                else
                {
                    LlenarDatosPestañaGrabar();
                    DesactivarTodosTabs("FuncionCargoTab");
                }
            }
            else{
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('Ingrese los campos obligatorios');", true);
                return;
            }
        }
        private bool ValidarDatosInsitucion()
        {
            bool respuesta = true;
            Color color = System.Drawing.ColorTranslator.FromHtml("#d1d3e2");
            ddlCategoriaInstitucion.BorderColor = color;
            if (ddlCategoriaInstitucion.SelectedIndex == 0)
            {
                respuesta = false;
                ddlCategoriaInstitucion.BorderColor = System.Drawing.Color.Red;
            }
            ddlInstitucion.BorderColor = color;
            if (ddlInstitucion.SelectedIndex == 0)
            {
                respuesta = false;
                ddlInstitucion.BorderColor = System.Drawing.Color.Red;
            }
            txtPersonaContacto.BorderColor = color;
            if (txtPersonaContacto.Text.Length == 0)
            {
                respuesta = false;
                txtPersonaContacto.BorderColor = System.Drawing.Color.Red;
            }
            txtCargoContacto.BorderColor = color;
            if (txtCargoContacto.Text.Length == 0)
            {
                respuesta = false;
                txtCargoContacto.BorderColor = System.Drawing.Color.Red;
            }
            txtCorreoContacto.BorderColor = color;
            if (txtCorreoContacto.Text.Length == 0)
            {
                respuesta = false;
                txtCorreoContacto.BorderColor = System.Drawing.Color.Red;
            }
            txtTelefonoContacto.BorderColor = color;
            if (txtTelefonoContacto.Text.Length == 0)
            {
                respuesta = false;
                txtTelefonoContacto.BorderColor = System.Drawing.Color.Red;
            }
            return respuesta;
        }
        protected void btnContinuarFotografia_Click(object sender, EventArgs e)
        {
            LlenarDatosPestañaGrabar();
            if (ValidarDatosInsitucionCargoParaContinuar())
            {
                //validarFotografia();
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
                Color color = System.Drawing.ColorTranslator.FromHtml("#d1d3e2");
                ddlTipoCargo.BorderColor = color;
                if (ddlTipoCargo.SelectedIndex == 0)
                {
                    resultado = false;
                    ddlTipoCargo.BorderColor = System.Drawing.Color.Red;
                }
                ddlCargo.BorderColor = color;
                if (ddlCargo.SelectedIndex == 0)
                {
                    resultado = false;
                    ddlCargo.BorderColor = System.Drawing.Color.Red;
                }
                return resultado;
            }
            catch (Exception ex)
            {
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return resultado;
            }
        }
        protected void btnContinuarGuardar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string valor = btn.CommandArgument;
            bool respuesta = false;
            if (valor == "Denuncia")
            {
                respuesta = cargaDenuncia();

                if (cuUploadFile4.FileName.ToString().Length > 0)
                {
                    if (!respuesta)
                    {
                        return;
                    }
                }

                LlenarDatosPestañaGrabar();

                if (hRutaDenuncia.Value.Length > 0)
                {
                    validarDenuncia();
                    DesactivarTodosTabs("GuardarTab");
                }
                else
                {
                    if (respuesta)
                    {
                        DesactivarTodosTabs("GuardarTab");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ibtVerImagen", "alert('Debe seleccionar una imagen');", true);
                    }
                }  
            }
            if (valor == "Pasaporte")
            {
                respuesta = cargaPasaporte();

                if (cuUploadFile3.FileName.ToString().Length > 0)
                {
                    if (!respuesta)
                    {
                        return;
                    }
                }

                LlenarDatosPestañaGrabar();

                if (hRutaPasaporte.Value.Length > 0)
                {
                    validarPasaporte();
                    DesactivarTodosTabs("GuardarTab");
                }
                else
                {
                    if (respuesta)
                    {
                        DesactivarTodosTabs("GuardarTab");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ibtVerImagen", "alert('Debe seleccionar una imagen');", true);
                    }
                }  
            }
            if (valor == "Firma")
            {
                respuesta = cargaFirma();

                if (cuUploadFile2.FileName.ToString().Length > 0)
                {
                    if (!respuesta)
                    {
                        return;
                    }
                }

                LlenarDatosPestañaGrabar();

                if (hRuta.Value.Length > 0 && hRutaFirma.Value.Length > 0)
                {
                    validarFotografia();
                    validarFirma();
                    if (ddlTipEmision.SelectedItem.Text == "NUEVO")
                    {
                        DesactivarTodosTabs("PasaporteTab");
                    }
                    else
                    {
                        DesactivarTodosTabs("GuardarTab");
                    }
                }
                else
                {
                    if (respuesta)
                    {
                        validarFotografia();
                        validarFirma();
                        if (ddlTipEmision.SelectedItem.Text == "NUEVO")
                        {
                            DesactivarTodosTabs("PasaporteTab");
                        }
                        else
                        {
                            DesactivarTodosTabs("GuardarTab");
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ibtVerImagen", "alert('Debe seleccionar una imagen');", true);
                    }
                }  
            }
        }
        protected void btnContinuarFirma_Click(object sender, EventArgs e)
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
                DesactivarTodosTabs("FirmaTab");
            }
            else
            {
                if (respuesta)
                {
                    validarFotografia();
                    DesactivarTodosTabs("FirmaTab");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ibtVerImagen", "alert('Debe seleccionar una imagen');", true);
                }
            }    
        }
        protected void btnEliminarFoto_Click(object sender, EventArgs e)
        {
            
            string rutaAdjuntos = oCodigoUsuario.getPathAdjuntos();
            string fileName = hFileNameFinal.Value;
            
            bool respuesta = eliminarArchivo(rutaAdjuntos, fileName);
            string scriptMensaje = @"document.getElementById('ContentPlaceHolder1_cuUploadFile1_fileupload').value=''; document.getElementById('ContentPlaceHolder1_imgFoto').src ='';document.getElementById('ContentPlaceHolder1_hNombreArchivo').value=''";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", scriptMensaje, true);
            if (respuesta)
            {
                hFileNameFinal.Value = "";
                DIV_CARGADO.Visible = false;
                DIV_NO_CARGADO.Visible = false;
            }
        }
        protected void btnEliminarFirma_Click(object sender, EventArgs e)
        {
            
            string rutaAdjuntos = oCodigoUsuario.getPathAdjuntos();
            string fileName = hFirmaFileNameFinal.Value;
            
            bool respuesta = eliminarArchivo(rutaAdjuntos, fileName);
            string scriptMensaje = @"document.getElementById('ContentPlaceHolder1_cuUploadFile2_fileupload').value=''; document.getElementById('ContentPlaceHolder1_imgFirma').src ='';document.getElementById('ContentPlaceHolder1_hNombreArchivoFirma').value=''";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", scriptMensaje, true);
            if (respuesta)
            {
                hFirmaFileNameFinal.Value = "";
                DIV_CARGADO_FIRMA.Visible = false;
                DIV_NO_CARGADO_FIRMA.Visible = false;
            }
        }
        protected void btnEliminarPasaporte_Click(object sender, EventArgs e)
        {

            string rutaAdjuntos = oCodigoUsuario.getPathAdjuntos();
            string fileName = hPasaporteFileNameFinal.Value;

            bool respuesta = eliminarArchivo(rutaAdjuntos, fileName);
            string scriptMensaje = @"document.getElementById('ContentPlaceHolder1_cuUploadFile3_fileupload').value=''; document.getElementById('ContentPlaceHolder1_imgPasaporte').src ='';document.getElementById('ContentPlaceHolder1_hNombreArchivoPasaporte').value=''";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", scriptMensaje, true);
            if (respuesta)
            {
                hPasaporteFileNameFinal.Value = "";
                DIV_CARGADO_PASAPORTE.Visible = false;
                DIV_NO_CARGADO_PASAPORTE.Visible = false;
            }
        }
        private void cargarDatosDeSam()
        {
            
            if (ddlRelDependencia.SelectedItem.Text == "TITULAR" && txtNroDocumentoSolicitante.Text.Equals(""))
            {
                bePersona be = (bePersona)Session["bePersona"];
                txtApePat.Text = be.Apellidopaterno;
                txtApeMat.Text = be.Apellidomaterno;
                txtNombres.Text = be.Nombres;
                DateTime date = be.Nacimientofecha;
                txtFecNac.Text = date.ToString("yyyy-MM-dd");
                txtApePat.Enabled = false;
                txtApeMat.Enabled = false;
                txtNombres.Enabled = false;
                txtFecNac.Enabled = false;

                //=======Sexo
                var index = (be.sexo==null?0:(be.sexo.ToUpper()).TrimEnd().Equals("MASCULINO") ? 1 : be.sexo.ToUpper().Trim().Equals("FEMENINO") ? 2 : 0);
                ddlSexo.SelectedIndex = index;
                if (index > 0)
                {
                    ddlSexo.Enabled = false;
                    seleccionarSexo(null, null);
                }
                //=======EstadoCivil
                int count = 0;
                foreach (ListItem li in ddlEstadoCivil.Items)
                {
                    if (be.EstadoCivil == null)
                    {
                        ddlEstadoCivil.SelectedIndex = 0;
                        ddlEstadoCivil.Enabled = true;
                    }
                    else { 
                    
                        if (li.Text.Substring(0, 2).Equals(be.EstadoCivil.Substring(0, 2)))
                        {
                            ddlEstadoCivil.SelectedIndex = count;
                            ddlEstadoCivil.Enabled = true;
                        }        
                    }

                    count++;
                }
                //=======Nacionalidad
                beGenerales obeGenerales = (beGenerales)ViewState["Generales"];
                if (obeGenerales != null)
                {
                    short idpais = 0;
                    List<bePais> listaPaises = obeGenerales.ListaPaises;
                    for (int i = 0; i < listaPaises.Count; i++)
                    {   
                        bePais b = listaPaises[i];
                        if (b.Nombre != null && b.Nombre.Equals(be.Nacionalidad))
                        {
                            idpais = b.Paisid;
                            lblNacionalidad.Text = b.Nacionalidad;
                            lblNacionalidad.ForeColor = (b.Nacionalidad == null || b.Nacionalidad.Equals("[ NO DEFINIDO ]") ? Color.Red : Color.Green);
                            
                            foreach (ListItem li in ddlNacionalidad.Items)
                            {
                                
                                ddlNacionalidad.SelectedIndex = i;
                                //ddlNacionalidad.Enabled = false;
                                List<beDocumentoIdentidad> listaDocIdent = cargarDocumentoIdentidad(short.Parse(ddlNacionalidad.SelectedValue), obeGenerales.ListaDocumentoIdentidadRegLinea);
                                listaDocIdent.Insert(0, new beDocumentoIdentidad { Tipodocumentoidentidadid = 0, DescripcionLarga = "<Seleccione>" });
                                ddlTipDoc.DataSource = listaDocIdent;
                                ddlTipDoc.DataValueField = "Tipodocumentoidentidadid";
                                ddlTipDoc.DataTextField = "DescripcionCorta";
                                ddlTipDoc.DataBind();
                                ddlTipDoc.SelectedIndex = 2;
                                ddlTipDoc.Enabled = false;
                                break;
                        
                            }
                            break;
                        }
                        else {

                            foreach (ListItem li in ddlNacionalidad.Items)
                            {

                                ddlNacionalidad.SelectedIndex = 0;
                                //ddlNacionalidad.Enabled = false;
                                List<beDocumentoIdentidad> listaDocIdent = cargarDocumentoIdentidad(short.Parse(ddlNacionalidad.SelectedValue), obeGenerales.ListaDocumentoIdentidadRegLinea);
                                listaDocIdent.Insert(0, new beDocumentoIdentidad { Tipodocumentoidentidadid = 0, DescripcionLarga = "<Seleccione>" });
                                ddlTipDoc.DataSource = listaDocIdent;
                                ddlTipDoc.DataValueField = "Tipodocumentoidentidadid";
                                ddlTipDoc.DataTextField = "DescripcionCorta";
                                ddlTipDoc.DataBind();
                                ddlTipDoc.SelectedIndex = 2;
                                ddlTipDoc.Enabled = false;
                                break;
                 
                            }

                        }
                    }

                }
                //=======calidad migratorio
                txtNroDocumentoSolicitante.Text = be.Pasaporte;
                txtNroDocumentoSolicitante.Enabled = false;
                txtTiempoPermanencia.Text = be.TiempoPermanencia;
                txtTiempoPermanencia.Enabled = false;
                txtTipoVisaId.Text = be.TipoVisa;
                txtTipoVisa.Text = be.TipoVisa==null?"": (be.TipoVisa.Equals("R") ? "RESIDENTE" : be.TipoVisa.Equals("T") ? "TEMPORAL" : "");
                txtTipoVisa.Enabled = false;
                txtVisaId.Text = be.Visa;
                txtVisa.Text = be.Visa==null?"": (be.Visa.Equals("COR") ? "CONSULAR" : be.Visa.Equals("CPR") ? "COOPERANTE" : be.Visa.Equals("DIR") ? "DIPLOMÁTICA" : be.Visa.Equals("FO") ? "FAMILIAR OFICIAL" :
                                            be.Visa.Equals("ITR") ? "Intercambio" : be.Visa.Equals("OFR") ? "Oficial" : be.Visa.Equals("PTR") ? "Periodista" : "");
                txtVisa.Enabled = false;
                txtTitularFamiliar.Text = be.TitularFamiliar==null? "": (be.TitularFamiliar.Equals("A") ? "TITULAR" : be.TitularFamiliar.Equals("B") ? "ESPOSA(O)" : be.TitularFamiliar.Equals("C") ? "HIJOS" :
                                            be.TitularFamiliar.Equals("D") ? "PADRES" : be.TitularFamiliar.Equals("E") ? "DEPENDIENTES" : be.TitularFamiliar.Equals("N") ? "NO INDICA" : "");
                txtTitularFamiliarId.Text = be.TitularFamiliar;
                txtTitularFamiliar.Enabled = false;

            }
        }
        private void LlenarDatosPestañaGrabar()
        {
            /*============quitar esta linea comentada---*/
            if (conInterConexionSAM.Equals("NO")) { divctipovisa.Visible = false; divcvisa.Visible = false; divctitular.Visible = false; divcpermanencia.Visible = false; }
            /*============quitar false &&  del siguiente if y del metodo "cargarDatosDuplicadoRenovacion"*/
            
            if (conInterConexionSAM.Equals("SI") && (ddlTipEmision.SelectedItem.Text.Equals("NUEVO") || ddlTipEmision.SelectedItem.Text.Equals("RENOVACIÓN") || ddlTipEmision.SelectedItem.Text.Equals("DUPLICADO")) && ddlRelDependencia.SelectedItem.Text.Equals("TITULAR") && txtNroDocumentoSolicitante.Text.Equals(""))
            {
                    cargarDatosDeSam();
                    
            }
            lblTipomisionFinaltxt.Text = ddlTipEmision.SelectedItem.Text;
            if (ddlInstitucion.SelectedIndex == 0)
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
                lblDatosInstitucionFinaltxt.Text = ddlInstitucion.SelectedItem.Text;
            }
            
            lblDatSolicitanteFinaltxt.Text = txtApePat.Text + " " + txtApeMat.Text + " " + txtNombres.Text;
            if (ddlCargo.SelectedItem != null)
            {
                lblFuncionCargoFinaltxt.Text = ddlCargo.SelectedItem.Text;
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

            if (hRutaFirma.Value.Length > 0)
            {
                lblFirmaFinaltxt.Text = "SI";
            }
            else
            {
                lblFirmaFinaltxt.Text = "NO";
            }

            if (hRutaPasaporte.Value.Length > 0)
            {
                lblPasaporteFinaltxt.Text = "SI";
            }
            else
            {
                lblPasaporteFinaltxt.Text = "[NO SE REQUIERE]";
            }

            if (hRutaDenuncia.Value.Length > 0)
            {
                lblDenunciaFinaltxt.Text = "SI";
            }
            else
            {
                lblDenunciaFinaltxt.Text = "[NO SE REQUIERE]";
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
                        hFileNameFinal.Value = fileName;
                        bool exitoFotografia = guardarImagen(rutaAdjuntos, fileName, base64String);
                        hRuta.Value = @"\" + DtNow.Year.ToString() + @"\" + DtNow.Month.ToString("D2") + @"\" + fileName;
                        imgFoto.ImageUrl = "data:image/png;base64," + base64String  ;
                        imgFoto.DataBind();
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
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return false;
            }
        }
        protected bool cargaFirma()
        {
            try
            {
                bool respuesta = false;
                if (cuUploadFile2.HasFile)
                {
                    FileInfo fi = new FileInfo(cuUploadFile2.FileName);
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
                        hNombreArchivoFirma.Value = cuUploadFile2.FileName;
                        //lblPesoArchivo.Text = Math.Round((fulImagen.PostedFile.ContentLength / 1024f) / 1024f, 2).ToString() + " MB";
                        //lblExtension.Text = fi.Extension;
                        Byte[] imageByte = null;
                        imageByte = new Byte[Convert.ToInt32(cuUploadFile2.ContentLength)];
                        //HttpPostedFile ImgFile = cuUploadFile1.PostedFile;

                        using (var binaryReader = new BinaryReader(Request.Files[1].InputStream))
                        {
                            imageByte = binaryReader.ReadBytes(Request.Files[1].ContentLength);
                        }
                        string base64String = Convert.ToBase64String(imageByte, 0, imageByte.Length);

                        Session["tempFirmaSave"] = base64String;
                        respuesta = true;

                        string fileName = oCodigoUsuario.getFileName();
                        hFirmaFileNameFinal.Value = fileName;
                        bool exitoFotografia = guardarImagen(rutaAdjuntos, fileName, base64String);
                        hRutaFirma.Value = @"\" + DtNow.Year.ToString() + @"\" + DtNow.Month.ToString("D2") + @"\" + fileName;
                        imgFirma.ImageUrl = "data:image/png;base64," + base64String;
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
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return false;
            }
        }

        protected bool cargaDenuncia()
        {
            try
            {
                bool respuesta = false;
                if (cuUploadFile4.HasFile)
                {
                    FileInfo fi = new FileInfo(cuUploadFile4.FileName);
                    DateTime DtNow = DateTime.Now;
                    string rutaAdjuntos = oCodigoUsuario.getPathAdjuntos();
                    //VALIDAR ARCHIVO ---------------------
                    bool exito = false;
                    bool exitoExtension = false;
                    bool exitoPeso = false;
                    exito = (oCodigoUsuario.validarExtensionArchivo(obrGeneral.fileExtPDF, fi) ? exitoExtension = true : exitoExtension = false);
                    exito = (oCodigoUsuario.validarPesoArchivo(obrGeneral.filePesoFotografia.ToString(), cuUploadFile1.ContentLength) ? exitoPeso = true : exitoPeso = false);
                    //---------------------------------------
                    if (exitoExtension & exitoPeso)
                    {
                        hNombreArchivoDenuncia.Value = cuUploadFile4.FileName;
                        //lblPesoArchivo.Text = Math.Round((fulImagen.PostedFile.ContentLength / 1024f) / 1024f, 2).ToString() + " MB";
                        //lblExtension.Text = fi.Extension;
                        Byte[] imageByte = null;
                        imageByte = new Byte[Convert.ToInt32(cuUploadFile4.ContentLength)];
                        //HttpPostedFile ImgFile = cuUploadFile1.PostedFile;

                        using (var binaryReader = new BinaryReader(Request.Files[3].InputStream))
                        {
                            imageByte = binaryReader.ReadBytes(Request.Files[3].ContentLength);
                        }
                        string base64String = Convert.ToBase64String(imageByte, 0, imageByte.Length);

                        Session["tempDenunciaSave"] = base64String;
                        respuesta = true;

                        string fileName = oCodigoUsuario.getFileName();
                        bool exitoFotografia = guardarPDF(rutaAdjuntos, fileName, base64String);
                        hRutaDenuncia.Value = @"\" + DtNow.Year.ToString() + @"\" + DtNow.Month.ToString("D2") + @"\" + fileName;
                    }
                    else
                    {
                        if (!exitoExtension)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('Debe adjuntar un archivo de formato (PDF)');", true);
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
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return false;
            }
        }

        protected bool cargaPasaporte()
        {
            try
            {
                bool respuesta = false;
                if (cuUploadFile3.HasFile)
                {
                    FileInfo fi = new FileInfo(cuUploadFile3.FileName);
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
                        hNombreArchivoPasaporte.Value = cuUploadFile3.FileName;
                        //lblPesoArchivo.Text = Math.Round((fulImagen.PostedFile.ContentLength / 1024f) / 1024f, 2).ToString() + " MB";
                        //lblExtension.Text = fi.Extension;
                        Byte[] imageByte = null;
                        imageByte = new Byte[Convert.ToInt32(cuUploadFile3.ContentLength)];
                        //HttpPostedFile ImgFile = cuUploadFile1.PostedFile;

                        using (var binaryReader = new BinaryReader(Request.Files[2].InputStream))
                        {
                            imageByte = binaryReader.ReadBytes(Request.Files[2].ContentLength);
                        }
                        string base64String = Convert.ToBase64String(imageByte, 0, imageByte.Length);

                        Session["tempPasaporteSave"] = base64String;
                        respuesta = true;

                        string fileName = oCodigoUsuario.getFileName();
                        hPasaporteFileNameFinal.Value = fileName;
                        bool exitoFotografia = guardarImagen(rutaAdjuntos, fileName, base64String);
                        hRutaPasaporte.Value = @"\" + DtNow.Year.ToString() + @"\" + DtNow.Month.ToString("D2") + @"\" + fileName;
                        imgPasaporte.ImageUrl = "data:image/png;base64," + base64String;
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
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
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
                            string uFile = fileName;
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
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
            return (exito);
        }
        private bool eliminarArchivo(string SavePath, string fileName)
        {
            Boolean exito = false;
            try
            {   
                if (File.Exists(SavePath + fileName))
                {   
                    File.Delete(SavePath+fileName);
                    exito = true;
                }
                else
                {
                    exito = false;
                }
            }
            catch (IOException ioExp)
            {
                Console.WriteLine(ioExp.Message);
            }
            return exito;

        }


        private bool guardarPDF(string SavePath, string fileName, string base64String)
        {
            bool exito = false;
            try
            {
                exito = oCodigoUsuario.createDirectory(SavePath);

                if (exito)
                {
                    if (File.Exists(SavePath + fileName))
                    {
                        File.Delete(SavePath + fileName);
                    }

                    if (base64String.Length > 0)
                    {
                        // CONVIERTE A STREAM -------------------------------------------------------
                        byte[] imgByte = Convert.FromBase64String(base64String);
                        File.WriteAllBytes(SavePath + fileName, imgByte);
                    }

                    if (File.Exists(SavePath + fileName))
                    {
                        exito = true;
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
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
            return (exito);
        }
        protected void fotoupload_change(object sender, EventArgs e)
        {
            
            System.Diagnostics.Debug.Write("____________si:fotoupload_change");
        }
        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                beRegistroLinea SessionGeneral = new beRegistroLinea();
                brRegistroLinea obrRegistroLinea = new brRegistroLinea();
                string ip = oCodigoUsuario.obtenerIP();
                string rutaAdjuntos = oCodigoUsuario.getPathAdjuntos();
                string pRegistroLineaId;
                string pNumeroRegLinea;

                if (hCodRegistroLinea.Value.Length == 0)
                {
                    codigoSolicitud(out pRegistroLineaId, out pNumeroRegLinea);

                    hCodRegistroLinea.Value = pRegistroLineaId;
                    hCodSolicitudTexto.Value = pNumeroRegLinea;
                    lblNroSolicitud.Text = "Solicitud Nro. " + pNumeroRegLinea;
                    lblFechaRegistro.Text = "Fecha de Registro: " + DateTime.Now.ToShortDateString();
                    lblNumeroSolicitudtxt.Text = pNumeroRegLinea;
                }
                
                SessionGeneral.RegistroLineaId = Convert.ToInt32(hCodRegistroLinea.Value);
                if (hCodCarnetIdentidad.Value.Length > 0)
                {
                    SessionGeneral.CarneIdentidadId = Convert.ToInt32(hCodCarnetIdentidad.Value);
                }
                SessionGeneral.TipoEmision = Convert.ToInt16(ddlTipEmision.SelectedValue);
                SessionGeneral.DpReldepTitdep = Convert.ToInt16(ddlRelDependencia.SelectedValue);
                if (hCodigoCarnetTitular.Value.Length > 0)
                {
                    SessionGeneral.DpReldepTitular = Convert.ToInt32(hCodigoCarnetTitular.Value);
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
                if (ddlTipoCargo.SelectedValue != "" && ddlTipoCargo.SelectedValue != "0")
                {
                    SessionGeneral.TIPO_CALIDAD_MIGRATORIA = Convert.ToInt16(ddlTipoCargo.SelectedValue);
                }
                SessionGeneral.InNombreInstitucion = ddlInstitucion.SelectedItem.Text;
                SessionGeneral.COD_INSTITUCION = Convert.ToInt16(ddlInstitucion.SelectedValue);
                SessionGeneral.COD_CATEGORIA_MISION = Convert.ToInt16(ddlCategoriaInstitucion.SelectedValue);
                SessionGeneral.InPersonaContacto = txtPersonaContacto.Text;
                SessionGeneral.InCargoContacto = txtCargoContacto.Text;
                SessionGeneral.InCorreoElectronico = txtCorreoContacto.Text;
                SessionGeneral.InNumeroTelefono = txtTelefonoContacto.Text;
                if (ddlCargo.SelectedValue != "")
                {
                    SessionGeneral.COD_CARGO = Convert.ToInt16(ddlCargo.SelectedValue);
                }
                SessionGeneral.InCargoNombre = ddlCargo.SelectedItem.Text;
                SessionGeneral.IpModificacion = ip;
                SessionGeneral.DpRutaFirma = hRutaFirma.Value;
                SessionGeneral.DpRutaPasaporte = hRutaPasaporte.Value;
                SessionGeneral.DpRutaDenunciaPolicial = hRutaDenuncia.Value;

                /*=========adicion de campos calidad migratoria===========*/
                SessionGeneral.TipoVisa = txtTipoVisaId.Text;
                SessionGeneral.Visa = txtVisaId.Text;
                SessionGeneral.TitularidadFamiliar = txtTitularFamiliarId.Text;
                SessionGeneral.TiempoPermanencia = txtTiempoPermanencia.Text;
                if (conInterConexionSAM.Equals("NO"))
                {
                    SessionGeneral.TiempoPermanencia = "0";
                }
                /*================================*/
                if (ddlTipEmision.SelectedItem.Text == "DUPLICADO")
                {
                    if (ddlMotivo.SelectedItem != null)
                    {
                        SessionGeneral.DpMotivoDuplicado = ddlMotivo.SelectedItem.Text;
                    }
                }
                else {
                    SessionGeneral.DpMotivoDuplicado = "";
                }


                

                bool exito = obrRegistroLinea.actualizar(SessionGeneral);
                if (exito)
                {

                    beGenerales obeGenerales = (beGenerales)ViewState["Generales"];
                    int pos = obeGenerales.ListaEstadosRegLinea.FindIndex(x => x.DescripcionCorta.Equals("REGISTRADO"));
                    short idEstado = 0;
                    if (pos != -1)
                    {
                        idEstado = obeGenerales.ListaEstadosRegLinea[pos].Estadoid;
                    }
                    SessionGeneral.EstadoId = idEstado;
                    obrRegistroLinea.actualizarDetalleEstadoEnviado(SessionGeneral);

                    btnVerReporte.Visible = true;
                    btnVerReporte.Enabled = false;
                    btnEnviarSolicitud.Visible = true;
                    btnEditar.Visible = true;

                    lblMensaje.Text = "Nro. Solicitud: " + hCodSolicitudTexto.Value + "<br/>" + "PUEDE CONSULTAR SU SOLICITUD EN LA MISMA PÁGINA EN LA OPCIÓN DE BÚSQUEDA" + "<br/>" + "PARA FINALIZAR EL TRÁMITE DEBERÁ DARLE CLICK AL BOTON ENVIAR";

                    string mensaje = "*** Nro. Solicitud: " + hCodSolicitudTexto.Value + " ***" + "<br/>";
                    mensaje = mensaje + "*** " + "PUEDE CONSULTAR SU SOLICITUD EN LA MISMA PÁGINA EN LA OPCIÓN DE BÚSQUEDA" + " ***" + "<br/>";
                    mensaje = mensaje + "*** " + "PARA FINALIZAR EL TRÁMITE DEBERÁ DARLE CLICK AL BOTON ENVIAR" + " ***";

                    lblMensaje.Text = mensaje;
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#modalMensaje').modal();", true);
                    return;
                }
                else
                {
                    File.Delete(rutaAdjuntos + hRuta.Value);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL FINALIZAR EL REGISTRO');", true);
                }
            }
            catch (Exception ex)
            {
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        protected void codigoSolicitud(out string pRegistroLineaId, out string pNumeroRegLinea)
        {
            pRegistroLineaId = "";
            pNumeroRegLinea = "";
            int idRegistroLinea = -1;
            beRegistroLinea SessionGeneral = new beRegistroLinea();
            beRegistroLinea parametros = new beRegistroLinea();
            parametros.IpCreacion = oCodigoUsuario.obtenerIP();
            brRegistroLinea obrRegistroLinea = new brRegistroLinea();
            idRegistroLinea = obrRegistroLinea.adicionar(parametros);
            if (idRegistroLinea != -1)
            {
                SessionGeneral.RegistroLineaId = idRegistroLinea;
                SessionGeneral = obrRegistroLinea.obtenerNumero(SessionGeneral);
                if (SessionGeneral.NumeroRegLinea != null)
                {
                    if (!SessionGeneral.NumeroRegLinea.Equals(""))
                    {
                        pRegistroLineaId = idRegistroLinea.ToString();
                        pNumeroRegLinea = SessionGeneral.NumeroRegLinea;
                    }
                }
            }
        }
        protected void btnVerReporte_Click(object sender, EventArgs e)
        {
            try
            {
                    beRegistroLinea SessionGeneral = new beRegistroLinea();
                    SessionGeneral.NumeroRegLinea = hCodSolicitudTexto.Value;
                    beRegistroLinea consultaReporte = buscarSolicitud(SessionGeneral);
                    string NumeroCarnetTitular = txtNroDoc.Text;
                    string TipoCargo = "";
                    if (ddlCargo.SelectedItem != null)
                    {
                         TipoCargo = ddlCargo.SelectedItem.Text;
                    }
                    else {
                         TipoCargo = "";
                    }
                    string motivo = "";
                    if (ddlTipEmision.SelectedItem.Text == "DUPLICADO")
                    {
                        if (ddlMotivo.SelectedItem.Text.Trim() == "DETERIORO")
                        {
                            motivo = "DETERIORO";
                        }
                    }

                    byte[] pdfByte = oCodigoUsuario.GenerarPDF(consultaReporte, NumeroCarnetTitular, TipoCargo, hNomTitular.Value, hApePatTitular.Value, hApeMatTitular.Value, hCalidadMigratoriaTitular.Value, motivo); 
                    Session["bytePDF"] = pdfByte;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirPDF", "window.open('../PDF.aspx', '_blank')", true);
            }
            catch (Exception ex)
            {
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        private void GuardarResumenAnexos()
        {
            try
            {
                beRegistroLinea SessionGeneral = new beRegistroLinea();
                SessionGeneral.NumeroRegLinea = hCodSolicitudTexto.Value;
                beRegistroLinea consultaReporte = buscarSolicitud(SessionGeneral);
                string NumeroCarnetTitular = txtNroDoc.Text;
                string TipoCargo = "";
                if (ddlCargo.SelectedItem != null)
                {
                    TipoCargo = ddlCargo.SelectedItem.Text;
                }
                else
                {
                    TipoCargo = "";
                }

                byte[] pdfByte = oCodigoUsuario.GenerarPDF(consultaReporte, NumeroCarnetTitular, TipoCargo, hNomTitular.Value, hApePatTitular.Value, hApeMatTitular.Value, hCalidadMigratoriaTitular.Value);
                if (pdfByte != null)
                {
                    string FolderPath = obrGeneral.rutaAdjuntos;
                    byte[] pdfByteAnexo = oCodigoUsuario.GenerarAnexosPDF(consultaReporte);
                    hRutaDocumentoAnexo.Value = FolderPath + hCodSolicitudTexto.Value + ".pdf";
                    oCodigoUsuario.Unir2PdfsByte(pdfByte, pdfByteAnexo, hRutaDocumentoAnexo.Value);
                     
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "abrirPDF", "window.open('../PDF.aspx', '_blank')", true);
                    //oCodigoUsuario.Unir2PdfsByte(pdfByte, pdfByteAnexo, rutacompletaNuevo);
                }
                
            }
            catch (Exception ex)
            {
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
        
        protected beRegistroLinea buscarSolicitud(beRegistroLinea obeRegistroLinea)
        {
            beRegistroLinea consultaReporte = new beRegistroLinea();
            consultaReporte.NumeroRegLinea = obeRegistroLinea.NumeroRegLinea;
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
                        GuardarResumenAnexos();
                        SessionGeneral.RegistroLineaId = Convert.ToInt32(hCodRegistroLinea.Value);
                        SessionGeneral.EstadoId = idEstado;
                        SessionGeneral.DpRutaResumen = hRutaDocumentoAnexo.Value;
                        SessionGeneral.IpModificacion = ip;
                        brRegistroLinea obrRegistroLinea = new brRegistroLinea();
                        bool exito = obrRegistroLinea.actualizarEstado(SessionGeneral);
                        if (exito)
                        {
                            obrRegistroLinea.actualizarDetalleEstadoEnviado(SessionGeneral);
                            btnEnviarSolicitud.Enabled = false;
                            btnGrabar.Enabled = false;
                            btnVerReporte.Enabled = true;
                            btnEditar.Enabled = false;

                            string mensaje = "*** Nro. Solicitud: " + hCodSolicitudTexto.Value + " ***" + "<br/>";
                            mensaje = mensaje + "*** " + "PRONTO SE ATENDERÁ SU SOLICITUD" + " ***" + "<br/>";
                            mensaje = mensaje + "*** " + "PUEDE DESCARGAR EL CARGO DE LA SOLICITUD DANDO CLICK AL BOTÓN VER REPORTE" + " ***" + "<br/>";
                            mensaje = mensaje + "*** " + "PUEDE CONSULTAR SU SOLICITUD EN LA MISMA PÁGINA EN LA OPCIÓN DE BÚSQUEDA" + " ***" + "<br/>";

                            lblMensajeEnviar.Text = mensaje;
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#modalMensaje').modal('hide'); $('#modalMensajeEnviar').modal();", true);
                            return;

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL ENVIAR EL REGISTRO');", true);
                        }
                    }
            }
            catch (Exception ex)
            {
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
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
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }

        }

        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            try
            {
                string strNombreFile = "REQUISITOS CARNÉ DE IDENTIDAD 2019 TRC1.pdf";

                string FolderPath = obrGeneral.rutaAdjuntos;
                string strRutaCompleta = FolderPath + @"\" + strNombreFile;
                if (File.Exists(strRutaCompleta))
                {
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
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('NO SE ENCONTRÓ EL DOCUMENTO');", true);
                }

               
            }
            catch (Exception ex)
            {
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR AL DESCARGAR EL ARCHIVO');", true);
            }
        }
        protected void seleccionarCategoriaOficina(object sender, EventArgs e) // DROPDOWLIST - A LA SELECCION DE UNA CATEGORIA MUESTRA UNA LISTA DE INTITUCIONES QUE CORRESPONDEN A ESA MISMA
        {
            try
            {
                if (!ddlCategoriaInstitucion.SelectedValue.Equals("0"))
                {
                    if (ViewState["Generales"] != null)
                    {
                        beGenerales obeGenerales = new beGenerales();
                        obeGenerales = (beGenerales)ViewState["Generales"];
                        List<beOficinaconsularExtranjera> lbeOficinaconsularExtranjera = new List<beOficinaconsularExtranjera>();
                        lbeOficinaconsularExtranjera = oCodigoUsuario.obtenerOficinasConsularesExtranjeras(short.Parse(ddlCategoriaInstitucion.SelectedValue), obeGenerales.ListaOficinasConsularesExtranjeras);
                        if (lbeOficinaconsularExtranjera.Count > 0)
                        {
                            lbeOficinaconsularExtranjera.Insert(0, new beOficinaconsularExtranjera { OficinaconsularExtranjeraid = 0, Nombre = "<Seleccione>" });
                            ddlInstitucion.DataSource = lbeOficinaconsularExtranjera;
                            ddlInstitucion.DataValueField = "OficinaconsularExtranjeraid";
                            ddlInstitucion.DataTextField = "Nombre";
                            ddlInstitucion.DataBind();
                            ddlInstitucion.Enabled = true;
                        }
                        else
                        {
                            cargarComboNull(ddlInstitucion);
                            ddlInstitucion.Enabled = false;
                        }
                    }
                }
                else
                {
                    cargarComboNull(ddlInstitucion);
                    ddlInstitucion.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
                //obrGeneral.grabarLog(ex);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }
    }
}