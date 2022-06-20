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
    public partial class AdministradorMantenimiento : System.Web.UI.Page
    {
        brGeneral obrGeneral = new brGeneral();
        CodigoUsuario oCodigoUsuario = new CodigoUsuario();
        UIEncriptador oUIEncriptador = new UIEncriptador();
        public static string valueTKSEG;
        public static string srtAl = "";
        public static List<beOficinaconsularExtranjera> listaInstituciones;
        public static List<beSolicitante> listaSolicitantes;
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
                    #endregion
                    #region Generales
                    //GENERALES
                    cargarGenerales();
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(0);", true);
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
                    divModal.Style.Add("display", "none");
                    divModal2.Style.Add("display", "none");
                    divEditarCargo.Style.Add("display", "none");
                    divEditarDocIdent.Style.Add("display", "none");
                    divEditarinstitucion.Style.Add("display", "none");
                    divEditarSolicitante.Style.Add("display", "none");
                    divEditarMensajeEstado.Style.Add("display", "none");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "preloader", "preloader();", true);
                }
            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR AL CARGAR LA PÁGINA');", true);
            }
        }

        //protected void seleccionarTipoDocIdent(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!ddlTipoDocDC.SelectedValue.Equals("0"))
        //        {
        //            if (Session["Generales"] != null)
        //            {
        //                beGenerales obeGenerales = new beGenerales();
        //                obeGenerales = (beGenerales)Session["Generales"];
        //                int pos = obeGenerales.ListaDocumentoIdentidad.FindIndex(x => x.DescripcionCorta.Equals(ddlTipoDocDC.SelectedItem.Text));
        //                if (pos != -1)
        //                {
        //                    lblTipoDocDL.Text = obeGenerales.ListaDocumentoIdentidad[pos].DescripcionLarga;
        //                }
        //                else
        //                {
        //                    lblTipoDocDL.Text = "[ NO SELECCIONADO ]";
        //                }
        //            }
        //        }
        //        else
        //        {
        //            lblTipoDocDL.Text = "[ NO SELECCIONADO ]";
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        obrGeneral.grabarLog(ex);
        //    }
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(0);", true);
        //}

        #region Conrtoles
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
                    //cargarComboNull(ddlCalidadMigratoriaSec);
                    seleccionarCalidadMigratoria(sender, e);
                    //ddlEstadoCivil.Enabled = false;
                    //ddlCalidadMigratoriaSec.Enabled = false;
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
                    //obeGenerales.ListaCalidadMigratoriaNivelSecundario.Insert(0, new beCalidadMigratoria { CalidadMigratoriaid = 0, Nombre = "<Seleccione>" });
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
                        gvCargos.DataSource = lbeCalidadMigratoria;
                        gvCargos.DataBind();
                    }
                    else
                    {
                        gvCargos.DataSource = null;
                        gvCargos.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(1);", true);
        }

        protected void editarCalidadMigratoria(object sender, EventArgs e) // SIN USO 
        {
            try
            {
                if (Session["Generales"] != null)
                {
                    beGenerales obeGenerales = new beGenerales();
                    obeGenerales = (beGenerales)Session["Generales"];
                    int pos = obeGenerales.ListaCalidadMigratoriaNivelPrincipal.FindIndex(x => x.CalidadMigratoriaid == short.Parse(ddlCalidadMigratoriaPri.SelectedValue));
                    if (pos != -1)
                    {
                        //txtEditarCalMig.Text = obeGenerales.ListaCalidadMigratoriaNivelPrincipal[pos].Nombre;
                        //txtEditarCalMigDef.Text = obeGenerales.ListaCalidadMigratoriaNivelPrincipal[pos].Definicion;
                        short referenciaID = obeGenerales.ListaCalidadMigratoriaNivelPrincipal[pos].CalidadMigratoriaid;
                        List<beCalidadMigratoria> ListaCalMigSec = new List<beCalidadMigratoria>();
                        ListaCalMigSec = obeGenerales.ListaCalidadMigratoriaNivelSecundario.FindAll(x => x.ReferenciaId == referenciaID);
                        gvCargos.DataSource = ListaCalMigSec;
                        gvCargos.DataBind();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(2);", true);
                    }

                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
            }
        }

        protected void guardarNuevo(object sender, EventArgs e) // GUARDA UN NUWVO REGISTRO EN LOS DISTINTOS MANTENIMIENTOS DE ACUERDO AL COMMANDNAME
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
                bool exito = false;
                string comando = "";
                // OBTIENE COMMAND NAME
                if (sender.GetType().Name.Equals("ImageButton")) { ImageButton control = (ImageButton)sender; comando = control.CommandName; }
                if (sender.GetType().Name.Equals("Button")) { Button control = (Button)sender; comando = control.CommandName; }

                beGenerales obeGenerales = new beGenerales();
                obeGenerales = (beGenerales)Session["Generales"];

                short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
                short oficinaId = short.Parse(ViewState["Oficinaid"].ToString());
                string ipCreacion = ViewState["IP"].ToString();
                string valorAccion = Request.Form["hdnAccionGuardar"];

                short refId = short.Parse(ddlCalidadMigratoriaPri.SelectedValue);
                short titDep = short.Parse(ddlTitDep.SelectedValue);

                brDocumentoIdentidad obrDocumentoIdentidad = new brDocumentoIdentidad();
                switch (comando)
                {
                    case "agregarCalMig":
                        #region agregarCalMig
                        //beCalidadMigratoria parametrosCalMigPri = new beCalidadMigratoria();
                        //parametrosCalMigPri.Nombre = txtNuevoCalMig.Text.ToUpper().Trim();
                        //parametrosCalMigPri.FlagNivelCalidad = false;
                        //parametrosCalMigPri.Definicion = txtCalidadMigratoriaDef.Text.Trim();
                        //parametrosCalMigPri.Usuariocreacion = usuarioId;
                        //parametrosCalMigPri.Ipcreacion = ipCreacion;
                        #endregion
                        break;
                    case "agregarDocIdent":
                        #region agregarDocIdent
                        beDocumentoIdentidad parametrosDocIdent = new beDocumentoIdentidad();
                        parametrosDocIdent.DescripcionCorta = txtNuevoDocIdent.Text.ToUpper().Trim();
                        parametrosDocIdent.DescripcionLarga = txtNuevoDocIdentDesc.Text.ToUpper().Trim();
                        parametrosDocIdent.Usuariocreacion = usuarioId;
                        parametrosDocIdent.Ipcreacion = ipCreacion;
                        short DocumentoIdentidadId = obrDocumentoIdentidad.adicionar(parametrosDocIdent);
                        if (DocumentoIdentidadId > 0)
                        {
                            exito = true;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE ADICIONÓ EL REGISTRO CORRECTAMENTE');", true);
                        }
                        else
                        {
                            if (DocumentoIdentidadId == -1) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR AL PROCESAR LA INFORMACIÓN');", true); }
                            if (DocumentoIdentidadId == -2) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('YA EXISTE UN DOCUMENTO DE IDENTIDAD CON ESE NOMBRE');", true); }
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "valorHD", "document.getElementById('idhdnAccionGuardar').value = '1';", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarTR", "mostrarTR('trNuevoDocIdent');", true);
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(1);", true);
                        #endregion
                        break;
                    case "agregarCargo":
                        #region agregarCargo
                        beCalidadMigratoria parametrosCalMigSec = new beCalidadMigratoria();
                        parametrosCalMigSec.GeneroId = short.Parse(ddlSexo.SelectedValue);
                        parametrosCalMigSec.CalidadMigratoriaid = short.Parse(ddlCalidadMigratoriaPri.SelectedValue);
                        parametrosCalMigSec.ReferenciaId = short.Parse(ddlCalidadMigratoriaPri.SelectedValue);
                        parametrosCalMigSec.Nombre = txtNuevoCargo.Text.ToUpper().Trim();
                        parametrosCalMigSec.FlagNivelCalidad = true;
                        parametrosCalMigSec.FlagTitularDependiente = short.Parse(ddlTitDep.SelectedValue);
                        parametrosCalMigSec.Usuariocreacion = usuarioId;
                        parametrosCalMigSec.Ipcreacion = ipCreacion;
                        brCalidadMigratoria obrCalidadMigratoria = new brCalidadMigratoria();
                        short cargoId = obrCalidadMigratoria.adicionarCargo(parametrosCalMigSec);
                        if (cargoId > 0)
                        {
                            exito = true;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE ADICIONÓ EL REGISTRO CORRECTAMENTE');", true);
                        }
                        else
                        {
                            if (cargoId == -1) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR AL PROCESAR LA INFORMACIÓN');", true); }
                            if (cargoId == -2) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('YA EXISTE UN CARGO CON ESE NOMBRE');", true); }
                        }
                        txtNuevoCargo.Text = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(1);", true);
                        #endregion
                        break;
                    case "editarCargo":

                        #region editarCargo
                        ImageButton imageButton = (ImageButton)sender;
                        TableCell tableCell = (TableCell)imageButton.Parent;
                        GridViewRow row = (GridViewRow)tableCell.Parent;
                        gvCargos.SelectedIndex = row.RowIndex;
                        int fila = row.RowIndex;
                        string dato = oCodigoUsuario.detectarCaracterEspecial(row.Cells[0].Text);
                        int pos = obeGenerales.ListaCalidadMigratoriaNivelSecundario.FindIndex(x => x.Nombre.Equals(dato) & x.ReferenciaId == refId & x.FlagTitularDependiente == titDep);
                        if (pos != -1)
                        {
                            ddlEditarSexo.SelectedValue = obeGenerales.ListaCalidadMigratoriaNivelSecundario[pos].GeneroId.ToString();
                            ddlEditarTitDep.SelectedValue = obeGenerales.ListaCalidadMigratoriaNivelSecundario[pos].FlagTitularDependiente.ToString();
                            txteditarCargo.Text = obeGenerales.ListaCalidadMigratoriaNivelSecundario[pos].Nombre;
                            ViewState["cardoID"] = obeGenerales.ListaCalidadMigratoriaNivelSecundario[pos].CalidadMigratoriaid;
                            divModal.Style.Add("display", "block");
                            divEditarCargo.Style.Add("display", "block");
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(1);", true);
                        #endregion
                        break;
                    case "guardarEditarCargo":
                        #region guardarCambioCargo
                        beCalidadMigratoria parametrosCalMigSecEditar = new beCalidadMigratoria();
                        parametrosCalMigSecEditar.GeneroId = short.Parse(ddlEditarSexo.SelectedValue);
                        parametrosCalMigSecEditar.CalidadMigratoriaid = short.Parse(ViewState["cardoID"].ToString());
                        parametrosCalMigSecEditar.ReferenciaId = short.Parse(ddlCalidadMigratoriaPri.SelectedValue);
                        parametrosCalMigSecEditar.Nombre = txteditarCargo.Text.ToUpper().Trim();
                        parametrosCalMigSecEditar.FlagNivelCalidad = true;
                        parametrosCalMigSecEditar.FlagTitularDependiente = short.Parse(ddlEditarTitDep.SelectedValue);
                        parametrosCalMigSecEditar.Usuariocreacion = usuarioId;
                        parametrosCalMigSecEditar.Ipcreacion = ipCreacion;
                        brCalidadMigratoria obrCalidadMigratoriaEditar = new brCalidadMigratoria();
                        short cargoIdEditar = obrCalidadMigratoriaEditar.actualizarCargo(parametrosCalMigSecEditar);
                        if (cargoIdEditar > 0)
                        {
                            exito = true;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE ACTUALIZÓ EL REGISTRO CORRECTAMENTE');", true);
                        }
                        else
                        {
                            if (cargoIdEditar == -1) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR AL PROCESAR LA INFORMACIÓN');", true); }
                            if (cargoIdEditar == -2) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('YA EXISTE UN CARGO CON ESE NOMBRE');", true); }
                        }
                        txtNuevoCargo.Text = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(1);", true);
                        #endregion
                        break;
                    case "editarDocIdent":
                        #region editarDocIdent
                        ImageButton imageButton1 = (ImageButton)sender;
                        TableCell tableCell1 = (TableCell)imageButton1.Parent;
                        GridViewRow row1 = (GridViewRow)tableCell1.Parent;
                        gvDocIdent.SelectedIndex = row1.RowIndex;
                        int fila1 = row1.RowIndex;

                        string dato1 = oCodigoUsuario.detectarCaracterEspecial(row1.Cells[0].Text);
                        string dato2 = oCodigoUsuario.detectarCaracterEspecial(row1.Cells[1].Text);

                        int pos1 = obeGenerales.ListaDocumentoIdentidad.FindIndex(x => x.DescripcionCorta.Equals(dato1) & x.DescripcionLarga.Equals(dato2));
                        if (pos1 != -1)
                        {
                            txtEditarDescCorta.Text = obeGenerales.ListaDocumentoIdentidad[pos1].DescripcionCorta;
                            txtEditarDescLarga.Text = obeGenerales.ListaDocumentoIdentidad[pos1].DescripcionLarga;
                            ViewState["docIdentID"] = obeGenerales.ListaDocumentoIdentidad[pos1].Tipodocumentoidentidadid;
                            divModal.Style.Add("display", "block");
                            divEditarDocIdent.Style.Add("display", "block");
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(0);", true);
                        #endregion
                        break;
                    case "guardarEditarDocIdent":
                        #region confirmarEditarDocIdent
                        beDocumentoIdentidad parametrosDocIdentEditar = new beDocumentoIdentidad();
                        parametrosDocIdentEditar.Tipodocumentoidentidadid = short.Parse(ViewState["docIdentID"].ToString());
                        parametrosDocIdentEditar.DescripcionCorta = txtEditarDescCorta.Text.ToUpper().Trim();
                        parametrosDocIdentEditar.DescripcionLarga = txtEditarDescLarga.Text.ToUpper().Trim();
                        parametrosDocIdentEditar.Usuariomodificacion = usuarioId;
                        parametrosDocIdentEditar.Ipmodificacion = ipCreacion;
                        short DocumentoIdentidadIdEditar = obrDocumentoIdentidad.actualizar(parametrosDocIdentEditar);
                        if (DocumentoIdentidadIdEditar > 0)
                        {
                            exito = true;
                            txtNuevoDocIdent.Text = string.Empty;
                            txtNuevoDocIdentDesc.Text = string.Empty;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ocultarTR", "ocultarTR('trNuevoDocIdent');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE ACTUALIZÓ EL REGISTRO CORRECTAMENTE');", true);
                        }
                        else
                        {
                            if (DocumentoIdentidadIdEditar == -1) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR AL PROCESAR LA INFORMACIÓN');", true); }
                            if (DocumentoIdentidadIdEditar == -2) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('YA EXISTE UN DOCUMENTO DE IDENTIDAD CON ESE NOMBRE');", true); }
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "valorHD", "document.getElementById('idhdnAccionGuardar').value = '3';", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarTR", "mostrarTR('trNuevoDocIdent');", true);
                        }
                        divModal.Style.Add("display", "none");
                        divEditarDocIdent.Style.Add("display", "none");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(0);", true);
                        #endregion
                        break;
                    case "guardarEditarInstitucion":
                        short idOficinaconsularExtranjera = -1;
                        beOficinaconsularExtranjera parametrosEditarOfco = new beOficinaconsularExtranjera();
                        parametrosEditarOfco.OficinaconsularExtranjeraid = short.Parse(ViewState["OfcoExId"].ToString());
                        parametrosEditarOfco.Categoriaid = short.Parse(ddlEditarCategoriaInst.SelectedValue);
                        parametrosEditarOfco.Nombre = txtEditarNombreInst.Text.Trim().ToUpper();
                        parametrosEditarOfco.Siglas = txtEditarSiglasInst.Text.Trim().ToUpper();
                        parametrosEditarOfco.Usuariomodificacion = usuarioId;
                        parametrosEditarOfco.Ipmodificacion = ipCreacion;
                        brOficinaConsularExtranjera obrOficinaConsularExtranjera = new brOficinaConsularExtranjera();
                        idOficinaconsularExtranjera = obrOficinaConsularExtranjera.actualizar(parametrosEditarOfco);
                        if (idOficinaconsularExtranjera > 0)
                        {
                            exito = true;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE ACTUALIZÓ EL REGISTRO CORRECTAMENTE');", true);
                        }
                        else
                        {
                            if (idOficinaconsularExtranjera == -1) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR AL PROCESAR LA INFORMACIÓN');", true); }
                            if (idOficinaconsularExtranjera == -2) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('YA EXISTE UNA INSTITUCIÓN EN ESA CATEGORIA CON ESE NOMBRE. REVISE.');", true); }
                            divModal.Style.Add("display", "block");
                            divEditarinstitucion.Style.Add("display", "block");
                        }
                        buscarInstitucion(sender, e);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(2);", true);
                        break;
                    case "guardarEditarSolicitante":
                        short SolicitanteId = -1;
                        beSolicitante parametrosEditarSoli = new beSolicitante();
                        parametrosEditarSoli.SolicitanteId = short.Parse(ViewState["SoliId"].ToString());
                        parametrosEditarSoli.PrimerApellido = txtEditarPriApeSoli.Text.Trim().ToUpper();
                        parametrosEditarSoli.SegundoApellido = txtEditarSegApeSoli.Text.Trim().ToUpper();
                        parametrosEditarSoli.Nombres = txtEditarNombresSoli.Text.Trim().ToUpper();
                        parametrosEditarSoli.TipoDocumentoIdentidadId = short.Parse(ddlEditarTipoDocIdentSoli.SelectedValue);
                        parametrosEditarSoli.NumeroDocumentoIdentidad = txtEditarNumIdentSoli.Text.Trim();
                        parametrosEditarSoli.Telefono = txtEditarTelefonoSoli.Text.Trim();
                        parametrosEditarSoli.Usuariomodificacion = usuarioId;
                        parametrosEditarSoli.Ipmodificacion = ipCreacion;
                        brSolicitante obrSolicitante = new brSolicitante();
                        SolicitanteId = obrSolicitante.actualizar(parametrosEditarSoli);
                        if (SolicitanteId > 0)
                        {
                            exito = true;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE ACTUALIZÓ EL REGISTRO CORRECTAMENTE');", true);
                        }
                        else
                        {
                            if (SolicitanteId == -1) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR AL PROCESAR LA INFORMACIÓN');", true); }
                            if (SolicitanteId == -2) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('YA EXISTE UNA PERSONA CON EL TIPO Y NUMERO DE IDENTIFICACIÓN. REVISE');", true); }
                            divModal.Style.Add("display", "block");
                            divEditarSolicitante.Style.Add("display", "block");
                        }
                        buscarSolicitante(sender, e);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(3);", true);
                        break;
                }
                if (exito)
                {
                    actualizarGenerales();
                    cargarGenerales();
                    gvCargos.DataSource = null;
                    gvCargos.DataBind();
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
            }
        }

        protected void buscarInstitucion(object sender, EventArgs e) // LISTA LAS INSTITUCIONES PERTENECIENTES A UNA CATEGORIA
        {
            try
            {
                listaInstituciones = new List<beOficinaconsularExtranjera>();
                beOficinaconsularExtranjera parametros = new beOficinaconsularExtranjera();
                parametros.Categoriaid = short.Parse(ddlMantCategoriaOfcoEx.SelectedValue);
                parametros.Siglas = txtMantSiglas.Text.Trim().ToUpper();
                parametros.Nombre = txtMantNombre.Text.Trim().ToUpper();
                brOficinaConsularExtranjera obrOficinaConsularExtranjera = new brOficinaConsularExtranjera();
                listaInstituciones = obrOficinaConsularExtranjera.consultar(parametros);
                lblMantTotalInst.Text = "Total Registros: " + (listaInstituciones != null ? listaInstituciones.Count.ToString() : "0");
                gvMantOfcoEx.DataSource = listaInstituciones;
                gvMantOfcoEx.DataBind();
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(2);", true);
        }

        protected void buscarSolicitante(object sender, EventArgs e) // LISTA LOS SOLICITANTES DE ACUERDO A FILTROS DE BUSQUEDA
        {
            try
            {
                listaSolicitantes = new List<beSolicitante>();
                beSolicitante parametros = new beSolicitante();
                parametros.PrimerApellido = txtMantPriApeSol.Text.Trim().ToUpper();
                parametros.SegundoApellido = txtMantSegApeSol.Text.Trim().ToUpper();
                parametros.Nombres = txtMantNombresSol.Text.Trim().ToUpper();
                parametros.TipoDocumentoIdentidadId = short.Parse(ddlMantTipoDodIdent.SelectedValue);
                parametros.NumeroDocumentoIdentidad = txtMantNumDocIdentSol.Text.Trim().ToUpper();
                parametros.Telefono = txtMantTelefonoSol.Text.Trim();
                brSolicitante obrSolicitante = new brSolicitante();
                listaSolicitantes = obrSolicitante.consultar(parametros);
                lblMantTotalSoli.Text = "Total Registros: " + (listaSolicitantes != null ? listaSolicitantes.Count.ToString() : "0");
                gvMantSolicitante.DataSource = listaSolicitantes;
                gvMantSolicitante.DataBind();
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR');", true);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(3);", true);
        }
        

        protected void seleccionarRegistro(object sender, EventArgs e) // SELECCION DE REGISTROS EN LAS DISTINTAS HERRAMIENTAS QUE OBEDECEN AL COMMANDNAME
        {
            ImageButton imageButton = (ImageButton)sender;
            TableCell tableCell = null;
            GridViewRow row = null;
            if (imageButton.Parent.ToString().Equals("System.Web.UI.WebControls.DataControlFieldCell"))
            {
                tableCell = (TableCell)imageButton.Parent;
                row = (GridViewRow)tableCell.Parent;
            }
            string comando = imageButton.CommandName;
            try
            {
                short usuarioId = short.Parse(ViewState["Usuarioid"].ToString());
                short oficinaId = short.Parse(ViewState["Oficinaid"].ToString());
                string ipCreacion = ViewState["IP"].ToString();
                switch (comando)
                {
                    case "agregarInsitucion":
                        short idOficinaconsularExtranjera = -1;
                        beOficinaconsularExtranjera parametrosNuevaInst = new beOficinaconsularExtranjera();
                        parametrosNuevaInst.Categoriaid = short.Parse(ddlMantCategoriaOfcoEx.SelectedValue);
                        parametrosNuevaInst.Nombre = txtMantNombre.Text.Trim().ToUpper();
                        parametrosNuevaInst.Siglas = txtMantSiglas.Text.Trim().ToUpper();
                        parametrosNuevaInst.Usuariocreacion = usuarioId;
                        parametrosNuevaInst.Ipcreacion = ipCreacion;
                        brOficinaConsularExtranjera obrOficinaConsularExtranjera = new brOficinaConsularExtranjera();
                        idOficinaconsularExtranjera = obrOficinaConsularExtranjera.adicionar(parametrosNuevaInst);
                        if (idOficinaconsularExtranjera > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE AGREGO EL REGISTRO CON ÉXITO');", true);
                        }
                        else
                        {
                            if (idOficinaconsularExtranjera == -1) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR AL ADICIONAR EL REGISTRO');", true); }
                            if (idOficinaconsularExtranjera == -2) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('YA EXISTE UNA INSTITUCIÓN EN ESA CATEGORIA CON ESE NOMBRE. REVISE.');", true); }
                        }
                        buscarInstitucion(sender, e);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(2);", true);
                        break;
                    case "agregarSolicitante":
                        short idSolicitante = -1;
                        beSolicitante parametrosNuevoSoli = new beSolicitante();
                        parametrosNuevoSoli.PrimerApellido = txtMantPriApeSol.Text.Trim().ToUpper();
                        parametrosNuevoSoli.SegundoApellido = txtMantSegApeSol.Text.Trim().ToUpper();
                        parametrosNuevoSoli.Nombres = txtMantNombresSol.Text.Trim().ToUpper();
                        parametrosNuevoSoli.TipoDocumentoIdentidadId = short.Parse(ddlMantTipoDodIdent.SelectedValue);
                        parametrosNuevoSoli.NumeroDocumentoIdentidad = txtMantNumDocIdentSol.Text.Trim();
                        parametrosNuevoSoli.Telefono = txtMantTelefonoSol.Text.Trim();
                        parametrosNuevoSoli.Usuariocreacion = usuarioId;
                        parametrosNuevoSoli.Ipcreacion = ipCreacion;
                        brSolicitante obrSolicitante = new brSolicitante();
                        idSolicitante = obrSolicitante.adicionar(parametrosNuevoSoli);
                        if (idSolicitante > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE AGREGO EL REGISTRO CON ÉXITO');", true);
                        }
                        else
                        {
                            if (idSolicitante == -1) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR AL ADICIONAR EL REGISTRO');", true); }
                            if (idSolicitante == -2) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('YA EXISTE UNA PERSONA CON EL TIPO Y NUMERO DE IDENTIFICACION. REVISE.');", true); }
                        }
                        buscarSolicitante(sender, e);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(3);", true);
                        break;
                    case "editarInstitucion":
                        string categoriaDesc = oCodigoUsuario.detectarCaracterEspecial(row.Cells[0].Text);
                        string nombre = oCodigoUsuario.detectarCaracterEspecial(row.Cells[1].Text);
                        int posEditarInst = listaInstituciones.FindIndex(x => x.ConCategoria.Equals(categoriaDesc) & x.Nombre.Equals(nombre));
                        if (posEditarInst != -1)
                        {
                            ViewState["OfcoExId"] = listaInstituciones[posEditarInst].OficinaconsularExtranjeraid;
                            ddlEditarCategoriaInst.SelectedValue = listaInstituciones[posEditarInst].Categoriaid.ToString();
                            txtEditarNombreInst.Text = listaInstituciones[posEditarInst].Nombre;
                            txtEditarSiglasInst.Text = listaInstituciones[posEditarInst].Siglas;
                            divModal.Style.Add("display", "block");
                            divEditarinstitucion.Style.Add("display", "block");
                        }
                        break;
                    case "editarSolicitante":
                        string tipoDoc = oCodigoUsuario.detectarCaracterEspecial(row.Cells[1].Text);
                        string numDoc = oCodigoUsuario.detectarCaracterEspecial(row.Cells[2].Text);
                        int posEditarSoli = listaSolicitantes.FindIndex(x => x.ConTipoDocIdent.Equals(tipoDoc) & x.NumeroDocumentoIdentidad.Equals(numDoc));
                        if (posEditarSoli != -1)
                        {
                            ViewState["SoliId"] = listaSolicitantes[posEditarSoli].SolicitanteId;
                            txtEditarPriApeSoli.Text = listaSolicitantes[posEditarSoli].PrimerApellido;
                            txtEditarSegApeSoli.Text = listaSolicitantes[posEditarSoli].SegundoApellido;
                            txtEditarNombresSoli.Text = listaSolicitantes[posEditarSoli].Nombres;
                            ddlEditarTipoDocIdentSoli.SelectedValue = listaSolicitantes[posEditarSoli].TipoDocumentoIdentidadId.ToString();
                            txtEditarNumIdentSoli.Text = listaSolicitantes[posEditarSoli].NumeroDocumentoIdentidad;
                            txtEditarTelefonoSoli.Text = listaSolicitantes[posEditarSoli].Telefono;
                            divModal.Style.Add("display", "block");
                            divEditarSolicitante.Style.Add("display", "block");
                        }
                        break;
                    case "editarMensajeEstado":                        
                        divModal.Style.Add("display", "block");
                        divEditarMensajeEstado.Style.Add("display", "block");
                        GridViewRow myrow=gvEstadoMensaje.Rows[row.RowIndex];
                        beGenerales obeGenerales = new beGenerales();
                        obeGenerales = (beGenerales)Session["Generales"];
                        int pos1 = obeGenerales.ListaMensajeEstado.FindIndex(x => x.EstadoDesc.Equals(myrow.Cells[1].Text) );
                        string idEstado = ""+obeGenerales.ListaMensajeEstado[pos1].Estadoid;
                        string estadoDesc = myrow.Cells[1].Text;
                        string mensaje = System.Net.WebUtility.HtmlDecode( myrow.Cells[2].Text);
                        ddlEstadoCarnet.SelectedValue = idEstado;
                        txtMensaje.Text = mensaje;
                        hOperacion.Value = "edit";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(4);", true);
                        break;
                    case "eliminarMensajeEstado":
                        divModal.Style.Add("display", "block");
                        divEditarMensajeEstado.Style.Add("display", "block");
                        GridViewRow myrowDel = gvEstadoMensaje.Rows[row.RowIndex];
                        beGenerales obeGeneralesDel = new beGenerales();
                        obeGenerales = (beGenerales)Session["Generales"];
                        int posDel = obeGenerales.ListaMensajeEstado.FindIndex(x => x.EstadoDesc.Equals(myrowDel.Cells[1].Text));
                        int idEstadoDel =  obeGenerales.ListaMensajeEstado[posDel].Estadoid;

                        brMensajeEstado mees = new brMensajeEstado();
                        beMensajeEstado be = new beMensajeEstado();
                        be.Estadoid = idEstadoDel;
                        be.Mensaje = "";
                        int resultado = mees.adicionar(be,"delete");
                        if (resultado > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OPERACIÓN REALIZADA CON ÉXITO');", true);
                            actualizarGenerales();
                            cargarGenerales();
                        }
                        else
                        {
                            if (resultado == -1) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR AL ADICIONAR EL REGISTRO');", true); }
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(4);", true);
                        break;
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

        protected void actualizarGenerales() // ACTUALIZA LAS SESSION DE LISTAS DE TABLAS MAESTRAS DEL SISTEMA
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
            brGenerales obrGenerales = new brGenerales();
            beGenerales obeGenerales = obrGenerales.obtenerGenerales();
            if (obeGenerales != null)
            {
                Session["Generales"] = obeGenerales;
            }
        }

        protected void cargarGenerales() // ACTUALIZA LAS LISTAS DESPLEGABLES DEL FORMULARIO
        {
            if (Session["Generales"] != null)
            {
                beGenerales obeGenerales = new beGenerales();
                obeGenerales = (beGenerales)Session["Generales"];
                // CATEGORIA OFCOEX
                obeGenerales.CategoriaOficinaExtranjera.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                ddlMantCategoriaOfcoEx.DataSource = obeGenerales.CategoriaOficinaExtranjera;
                ddlMantCategoriaOfcoEx.DataValueField = "Parametroid";
                ddlMantCategoriaOfcoEx.DataTextField = "Descripcion";
                ddlMantCategoriaOfcoEx.DataBind();

                ddlEditarCategoriaInst.DataSource = obeGenerales.CategoriaOficinaExtranjera;
                ddlEditarCategoriaInst.DataValueField = "Parametroid";
                ddlEditarCategoriaInst.DataTextField = "Descripcion";
                ddlEditarCategoriaInst.DataBind();

                // TIPO DOC IDENT
                gvDocIdent.DataSource = obeGenerales.ListaDocumentoIdentidad;
                gvDocIdent.DataBind();

                obeGenerales.ListaDocumentoIdentidad.Insert(0, new beDocumentoIdentidad { Tipodocumentoidentidadid = 0, DescripcionCorta = "<Seleccione>" });
                ddlMantTipoDodIdent.DataSource = obeGenerales.ListaDocumentoIdentidad;
                ddlMantTipoDodIdent.DataValueField = "Tipodocumentoidentidadid";
                ddlMantTipoDodIdent.DataTextField = "DescripcionCorta";
                ddlMantTipoDodIdent.DataBind();

                ddlEditarTipoDocIdentSoli.DataSource = obeGenerales.ListaDocumentoIdentidad;
                ddlEditarTipoDocIdentSoli.DataValueField = "Tipodocumentoidentidadid";
                ddlEditarTipoDocIdentSoli.DataTextField = "DescripcionCorta";
                ddlEditarTipoDocIdentSoli.DataBind();

                // CALIDAD MIGRATORIA
                obeGenerales.ListaCalidadMigratoriaNivelPrincipal.Insert(0, new beCalidadMigratoria { CalidadMigratoriaid = 0, Nombre = "<Seleccione>" });
                ddlCalidadMigratoriaPri.DataSource = obeGenerales.ListaCalidadMigratoriaNivelPrincipal;
                ddlCalidadMigratoriaPri.DataValueField = "CalidadMigratoriaid";
                ddlCalidadMigratoriaPri.DataTextField = "Nombre";
                ddlCalidadMigratoriaPri.DataBind();
                // TITULAR - DEPENDIENTE (PARAMETRO)
                obeGenerales.TitularDependienteParametros.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                ddlTitDep.DataSource = obeGenerales.TitularDependienteParametros;
                ddlTitDep.DataValueField = "Parametroid";
                ddlTitDep.DataTextField = "Descripcion";
                ddlTitDep.DataBind();

                ddlEditarTitDep.DataSource = obeGenerales.TitularDependienteParametros;
                ddlEditarTitDep.DataValueField = "Parametroid";
                ddlEditarTitDep.DataTextField = "Descripcion";
                ddlEditarTitDep.DataBind();
                // GENERO
                obeGenerales.ListaParametroGenero.Insert(0, new beParametro { Parametroid = 0, Descripcion = "<Seleccione>" });
                ddlSexo.DataSource = obeGenerales.ListaParametroGenero;
                ddlSexo.DataValueField = "Parametroid";
                ddlSexo.DataTextField = "Descripcion";
                ddlSexo.DataBind();

                ddlEditarSexo.DataSource = obeGenerales.ListaParametroGenero;
                ddlEditarSexo.DataValueField = "Parametroid";
                ddlEditarSexo.DataTextField = "Descripcion";
                ddlEditarSexo.DataBind();


                // TIPO DOC IDENTIDAD
                //obeGenerales.ListaDocumentoIdentidad.Insert(0, new beDocumentoIdentidad { Tipodocumentoidentidadid = 0, DescripcionCorta = "<Seleccione>" });
                //ddlTipoDocDC.DataSource = obeGenerales.ListaDocumentoIdentidad;
                //ddlTipoDocDC.DataValueField = "Tipodocumentoidentidadid";
                //ddlTipoDocDC.DataTextField = "DescripcionCorta";
                //ddlTipoDocDC.DataBind();
                int pos = obeGenerales.ListaEstados.FindIndex(x => x.DescripcionCorta.Equals("EMITIDO (IMPRESO)"));
                short referenciaID = (pos != -1 ? obeGenerales.ListaEstados[pos].Estadoid : short.Parse("0"));

                obeGenerales.ListaEstados.Insert(0, new beEstado { Estadoid = 0, DescripcionCorta = "<Seleccione>" });
                List <beEstado> listestado = obeGenerales.ListaEstados.FindAll(x => x.Estadoid == referenciaID);

                ddlEstadoCarnet.DataSource = listestado;
                ddlEstadoCarnet.DataValueField = "Estadoid";
                ddlEstadoCarnet.DataTextField = "DescripcionCorta";
                ddlEstadoCarnet.DataBind();

                // grid mensaje de Estados
                gvEstadoMensaje.DataSource = obeGenerales.ListaMensajeEstado;
                gvEstadoMensaje.DataBind();
                //gvEstadoMensaje.Columns[0].Visible = false;
            }
        }
        protected void btnNuevoMensaje_click(object sender, EventArgs e) {

            ddlEstadoCarnet.SelectedIndex = 0;
            txtMensaje.Text = "";
            hOperacion.Value = "new";
            divModal.Style.Add("display", "block");
            divEditarMensajeEstado.Style.Add("display", "block");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(4);", true);
        }
        protected void grabarMensajeEstado(object sender, EventArgs e)
        {
            try { 
                short nuevoIdEstado = -1;
                beMensajeEstado ParamEstado = new beMensajeEstado();
                ParamEstado.Estadoid = short.Parse(ddlEstadoCarnet.SelectedValue);
                ParamEstado.Mensaje = txtMensaje.Text.Trim().ToUpper();
                Boolean existe = false;
                foreach (GridViewRow row in gvEstadoMensaje.Rows)
                {
                    if (row.Cells[1].Text.Equals(ddlEstadoCarnet.SelectedItem.Text))
                    {
                        existe = true;
                    }
                }
                if (existe & hOperacion.Value.Equals("new"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('YA EXISTE EL MENSAJE PARA EL ESTADO SELECCIONADO');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(4);", true);
                    return;
                }

                brMensajeEstado mees = new brMensajeEstado();
                nuevoIdEstado = mees.adicionar(ParamEstado, hOperacion.Value.Equals("new")?"insert":"update");
                if (nuevoIdEstado > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE AGREGO EL REGISTRO CON ÉXITO');", true);
                    actualizarGenerales();
                    cargarGenerales();
                }
                else
                {
                    if (nuevoIdEstado == -1) { ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIÓ UN ERROR AL ADICIONAR EL REGISTRO');", true); }
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(4);", true);
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }

}
        #endregion
    }
    
}