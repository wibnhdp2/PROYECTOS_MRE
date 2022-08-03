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
    public partial class AdminsitradorHerramientas : System.Web.UI.Page
    {
        private class beBool
        {
            public string texto { get; set; }
            public short valor { get; set; }
        }
        brGeneral obrGeneral = new brGeneral();
        CodigoUsuario oCodigoUsuario = new CodigoUsuario();
        UIEncriptador oUIEncriptador = new UIEncriptador();
        public static List<beUsuario> listaUsuarios = new List<beUsuario>();
        public static List<beUsuario> xlistaUsuarios = new List<beUsuario>();
        public static string valueTKSEG;
        public static string srtAl = "";
        public static short idUsuario = -1;
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
                    cargarGenerales();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "preloader", "preloader();", true);
                }
            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL CARGAR LA PAGINA');", true);
            }
        }

        protected void actualizarGenerales() // ACTUALIZA LAS SESSION DE LISTAS DE TABLAS MAESTRAS DEL SISTEMA
        {
            //#region TKSEG
            //string TKSEG = TKSEGENC.Value;
            //bool exitoTKSEG = oCodigoUsuario.compareValTK(TKSEG, valueTKSEG);
            //if (!exitoTKSEG)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensajeError", "location.href='../../mensajes.aspx';", true);
            //    return;
            //}
            //#endregion
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
                // TIPO DOCUMENTO IDENTIDAD
                obeGenerales.ListaDocumentoIdentidad.Insert(0, new beDocumentoIdentidad { DescripcionCorta = "<Seleccione>", Tipodocumentoidentidadid = 0 });
                ddlTipoDoc.DataSource = obeGenerales.ListaDocumentoIdentidad;
                ddlTipoDoc.DataValueField = "Tipodocumentoidentidadid";
                ddlTipoDoc.DataTextField = "DescripcionCorta";
                ddlTipoDoc.DataBind();
                // ROLES
                obeGenerales.ListaRoles.Insert(0, new beRolconfiguracion { Rolconfiguracionid = 0, Nombre = "<Seleccione>" });
                ddlRol.DataSource = obeGenerales.ListaRoles;
                ddlRol.DataValueField = "Rolconfiguracionid";
                ddlRol.DataTextField = "Nombre";
                ddlRol.DataBind();

                ddlFiltroRol.DataSource = obeGenerales.ListaRoles;
                ddlFiltroRol.DataValueField = "Rolconfiguracionid";
                ddlFiltroRol.DataTextField = "Nombre";
                ddlFiltroRol.DataBind();

                List<beBool> listaBloqDes = new List<beBool>();
                beBool obeBool;
                obeBool = new beBool();
                obeBool.texto = "Bloqueado";
                obeBool.valor = 1;
                listaBloqDes.Add(obeBool);
                obeBool = new beBool();
                obeBool.texto = "Desbloqueado";
                obeBool.valor = 0;
                listaBloqDes.Add(obeBool);
                listaBloqDes.Insert(0, new beBool { valor = 3, texto = "<Seleccione>" });
                ddlFiltroBloqDes.DataSource = listaBloqDes;
                ddlFiltroBloqDes.DataValueField = "valor";
                ddlFiltroBloqDes.DataTextField = "texto";
                ddlFiltroBloqDes.DataBind();

                //gvUsuarios.Columns[7].Visible = true;
                //xlistaUsuarios = obeGenerales.ListaUsuarios;
                //gvUsuarios.DataSource = xlistaUsuarios; //obeGenerales.ListaUsuarios;
                //gvUsuarios.DataBind();
                //gvUsuarios.Columns[7].Visible = false;
            }
        }

        protected void verTemplate(object sender, GridViewRowEventArgs e) // PERMITE MOSTRAR LAS HERRAMIENTAS EN LOS REGISTROS DE LA GRILLA DE ACUERDO A SU ESTADO O CONDICION PARTICULAR
        {
            try
            {
                if (e.Row.DataItemIndex > -1)
                {
                    if (e.Row.Cells[7].Text.Equals("False"))
                    {
                        e.Row.Cells[5].Controls[3].Visible = false;
                        oCodigoUsuario.colorCeldas("#CEF6D8", "#000000", sender, e);
                    }
                    else
                    {
                        e.Row.Cells[5].Controls[1].Visible = false;
                        oCodigoUsuario.colorCeldas("#FA5858", "#000000", sender, e);
                    }
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR AL MOSTRAR LA INFORMACION');", true);
            }
        }

        protected List<beUsuario> seleccionarRol()
        {
            List<beUsuario> lista = new List<beUsuario>();
            if (Session["Generales"] != null)
            {
                beGenerales obeGenerales = new beGenerales();
                obeGenerales = (beGenerales)Session["Generales"];
                lista = obeGenerales.ListaUsuarios.FindAll(x => x.Rol_Id == short.Parse(ddlFiltroRol.SelectedValue));
            }
            return (lista);
        }

        protected void ddlFiltroRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ddlFiltroRol.SelectedValue.Equals("3")) 
                {
                    if (Session["Generales"] != null)
                    {
                        beGenerales obeGenerales = new beGenerales();
                        obeGenerales = (beGenerales)Session["Generales"];
                        //Boolean valor = Convert.ToBoolean(ddlFiltroBloqDes.SelectedValue);
                        bool valor;
                        if(bool.TryParse(ddlFiltroBloqDes.SelectedValue,out valor))
                        {

                        }
                        List<beUsuario> lista = obeGenerales.ListaUsuarios.FindAll(x => x.Rol_Id == short.Parse(ddlFiltroRol.SelectedValue) & x.BloqueoActiva == valor);
                        if (lista != null & lista.Count > 0)
                        {
                            gvUsuarios.DataSource = lista;
                            gvUsuarios.DataBind();
                        }
                    }
                }
                limpiarControles();
                bloqControles(false);

            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(0);", true);
        }

        public bool usuarioBloqueo(beUsuarioRol parametros, bool bloqueo)
        {
            bool exito = false;
            try
            {
                beUsuario obeUsuario_Crea = new beUsuario();
                obeUsuario_Crea.Usuarioid = short.Parse(ViewState["Usuarioid"].ToString());
                beUsuario obeUsuario_Bloq = new beUsuario();
                obeUsuario_Bloq.BloqueoActiva = bloqueo;
                obeUsuario_Bloq.Usuarioid = parametros.Usuarioid;
                obeUsuario_Bloq.Ipmodificacion = ViewState["IP"].ToString();
                string comentario = (bloqueo ? "Sesión Bloqueada" : "Sesión Desbloqueada");
                brUsuario obrUsuario = new brUsuario();
                exito = obrUsuario.bloqueoActiva(obeUsuario_Crea, obeUsuario_Bloq, comentario);
            }
            catch (Exception ex)
            {
                exito = false;
                obrGeneral.grabarLog(ex);
            }
            return exito;
        }

        protected void bloquearDesbloquear(object sender, EventArgs e)
        {
            try
            {
                ImageButton control = (ImageButton)sender;
                TableCell tableCell = (TableCell)control.Parent;
                GridViewRow row = (GridViewRow)tableCell.Parent;
                gvUsuarios.SelectedIndex = row.RowIndex;
                int fila = row.RowIndex;

                if (Session["Generales"] != null)
                {
                    beGenerales obeGenerales = new beGenerales();
                    obeGenerales = (beGenerales)Session["Generales"];
                    int pos = obeGenerales.ListaUsuarios.FindIndex(x => x.Alias.Equals(row.Cells[0].Text));
                    if (pos != -1)
                    {
                        idUsuario = obeGenerales.ListaUsuarios[pos].Usuarioid;
                        beUsuarioRol parametros = new beUsuarioRol();
                        parametros.Usuarioid = idUsuario; //short.Parse(ddlUsuario.SelectedValue);
                        bool opcion = (control.CommandName.Equals("blockUser") ? true : false);
                        bool exito = usuarioBloqueo(parametros, opcion);
                        if (exito)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE GUARDO LA INFORMACION CORRECTAMENTE');", true);
                            idUsuario = -1;
                            actualizarGenerales();
                            cargarGenerales();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
            }
        }

        protected beUsuario obtenerRegistro(object sender, EventArgs e)
        {
            beUsuario obeUsuario = new beUsuario();
            try
            {
                ImageButton imageButton = (ImageButton)sender;
                TableCell tableCell = (TableCell)imageButton.Parent;
                GridViewRow row = (GridViewRow)tableCell.Parent;
                gvUsuarios.SelectedIndex = row.RowIndex;
                int fila = row.RowIndex;

                if (Session["Generales"] != null)
                {
                    beGenerales obeGenerales = new beGenerales();
                    obeGenerales = (beGenerales)Session["Generales"];
                    int pos = obeGenerales.ListaUsuarios.FindIndex(x => x.Alias.Equals(row.Cells[0].Text));
                    if (pos != -1)
                    {

                    }
                    else { obeUsuario = null; }
                }
            }
            catch (Exception ex)
            {
                obeUsuario = null;
                obrGeneral.grabarLog(ex);
            }
            return (obeUsuario);
        }

        protected void seleccionarAccion(object sender, EventArgs e)
        {
            try
            {
                string comando = "";
                short rolId = -1;
                bool bloDes = false;
                //short idUsuario = -1;
                // IDENTIFICA QUE TIPO DE CONTROL INVOCA AL METODO ------
                if (sender.GetType().Name.Equals("ImageButton")) 
                { 
                    ImageButton control = (ImageButton)sender;
                    TableCell tableCell = (TableCell)control.Parent;
                    GridViewRow row = (GridViewRow)tableCell.Parent;
                    gvUsuarios.SelectedIndex = row.RowIndex;
                    int fila = row.RowIndex;
                    comando = control.CommandName;
                    if (Session["Generales"] != null)
                    {
                        beGenerales obeGenerales = new beGenerales();
                        obeGenerales = (beGenerales)Session["Generales"];
                        int pos = obeGenerales.ListaUsuarios.FindIndex(x => x.Alias.Equals(row.Cells[0].Text));
                        if (pos != -1)
                        {
                            idUsuario = obeGenerales.ListaUsuarios[pos].Usuarioid;
                            rolId = obeGenerales.ListaUsuarios[pos].Rol_Id;
                            bloDes = obeGenerales.ListaUsuarios[pos].BloqueoActiva;
                        }
                    }
                }
                if (sender.GetType().Name.Equals("Button")) { Button control = (Button)sender; comando = control.CommandName; }
                // ------------------------------------------------------------------------------------
                beUsuario parametros = new beUsuario();
                //List<beUsuario> listaUsuarios = new List<beUsuario>();

                switch (comando)
                {
                    case "verInfoUsuario":
                        parametros.Usuarioid = idUsuario;
                        parametros.Rol_Id = rolId;
                        parametros.BloqueoActiva = bloDes;
                        listaUsuarios = obtenerInfoUsuarios(parametros);
                        ddlTipoDoc.SelectedValue = listaUsuarios[0].Documentotipoid.ToString();
                        txtNumeroDoc.Text = listaUsuarios[0].Documentonumero;
                        txtApePat.Text = listaUsuarios[0].Apellidopaterno;
                        txtApeMat.Text = listaUsuarios[0].Apellidomaterno;
                        txtNombres.Text = listaUsuarios[0].Nombres;
                        txtCorreo.Text = listaUsuarios[0].Correoelectronico;
                        txtAlias.Text = listaUsuarios[0].Alias;
                        ddlRol.SelectedValue = listaUsuarios[0].Rol_Id.ToString();
                        btnEditar.Enabled = true;
                        //limpiarControles();
                        bloqControles(false);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(1);", true);
                        break;
                    case "guardar":
                        brUsuario obrUsuario = new brUsuario();
                        string accionGuardar = ViewState["accionGuardar"].ToString();
                        switch (accionGuardar)
                        {
                            case "nuevo":
                                // USUARIO
                                short UsuarioId = -1;
                                beUsuario obeUsuarioNuevo = new beUsuario();
                                obeUsuarioNuevo.Documentotipoid = short.Parse(ddlTipoDoc.SelectedValue);
                                obeUsuarioNuevo.Documentonumero = txtNumeroDoc.Text.Trim();
                                obeUsuarioNuevo.Apellidopaterno = txtApePat.Text.ToUpper().Trim();
                                obeUsuarioNuevo.Apellidomaterno = txtApeMat.Text.ToUpper().Trim();
                                obeUsuarioNuevo.Nombres = txtNombres.Text.ToUpper().Trim();
                                obeUsuarioNuevo.Correoelectronico = txtCorreo.Text.ToUpper().Trim();
                                obeUsuarioNuevo.Alias = txtAlias.Text.ToUpper().Trim();
                                obeUsuarioNuevo.Usuariocreacion = short.Parse(ViewState["Usuarioid"].ToString());
                                obeUsuarioNuevo.Ipcreacion = ViewState["IP"].ToString();
                                // USUARIO - ROL
                                beUsuarioRol obeUsuarioRolNuevo = new beUsuarioRol();
                                //obeUsuarioRolNuevo.Usuariorolid = listaUsuarios[0].UsuarioRolId;
                                //obeUsuarioRolNuevo.Usuarioid = listaUsuarios[0].Usuarioid;
                                obeUsuarioRolNuevo.Rolconfiguracionid = short.Parse(ddlRol.SelectedValue);
                                obeUsuarioRolNuevo.Oficinaconsularid = short.Parse(ViewState["Oficinaid"].ToString());
                                obeUsuarioRolNuevo.Usuariocreacion = short.Parse(ViewState["Usuarioid"].ToString());
                                obeUsuarioRolNuevo.Ipcreacion = ViewState["IP"].ToString();
                                UsuarioId = obrUsuario.adicionar(obeUsuarioNuevo, obeUsuarioRolNuevo);
                                if (UsuarioId > 0)
                                {
                                    bloqControles(false);
                                    limpiarControles();
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE ADICIONO EL REGISTRO CON EXITO');", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR MIENTRAS SE ADICIONABA EL REGISTRO');", true);
                                }
                                break;
                            case "editar":
                                // USUARIO
                                beUsuario obeUsuarioEditar = new beUsuario();
                                obeUsuarioEditar.Usuarioid = idUsuario;//short.Parse(ddlUsuario.SelectedValue);
                                obeUsuarioEditar.Documentotipoid = short.Parse(ddlTipoDoc.SelectedValue);
                                obeUsuarioEditar.Documentonumero = txtNumeroDoc.Text.Trim();
                                obeUsuarioEditar.Apellidopaterno = txtApePat.Text.ToUpper().Trim();
                                obeUsuarioEditar.Apellidomaterno = txtApeMat.Text.ToUpper().Trim();
                                obeUsuarioEditar.Nombres = txtNombres.Text.ToUpper().Trim();
                                obeUsuarioEditar.Correoelectronico = txtCorreo.Text.ToUpper().Trim();
                                obeUsuarioEditar.Alias = txtAlias.Text.ToUpper().Trim();
                                obeUsuarioEditar.Usuariomodificacion = short.Parse(ViewState["Usuarioid"].ToString());
                                obeUsuarioEditar.Ipmodificacion = ViewState["IP"].ToString();
                                // USUARIO - ROL
                                beUsuarioRol obeUsuarioRolEditar = new beUsuarioRol();
                                obeUsuarioRolEditar.Usuariorolid = listaUsuarios[0].UsuarioRolId;
                                obeUsuarioRolEditar.Usuarioid = idUsuario; //listaUsuarios[0].Usuarioid;
                                obeUsuarioRolEditar.Rolconfiguracionid = short.Parse(ddlRol.SelectedValue);
                                obeUsuarioRolEditar.Usuariomodificacion = short.Parse(ViewState["Usuarioid"].ToString());
                                obeUsuarioRolEditar.Ipmodificacion = ViewState["IP"].ToString();
                                bool exito = obrUsuario.actualizar(obeUsuarioEditar, obeUsuarioRolEditar);
                                if (exito)
                                {
                                    bloqControles(false);
                                    limpiarControles();
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('SE ACTUALIZO EL REGISTRO CON EXITO');", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR MIENTRAS SE ACTUALIZABA EL REGISTRO');", true);
                                }
                                break;
                        }
                        actualizarGenerales();
                        cargarGenerales();
                        //ddlUsuario.SelectedValue = listaUsuarios[0].Usuarioid.ToString();
                        //seleccionarUsuario();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(0);", true);
                        break;
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('OCURRIO UN ERROR');", true);
            }
            //idUsuario = -1;
        }

        protected List<beUsuario> obtenerInfoUsuarios(beUsuario parametros)
        {
            List<beUsuario> listaUsuarios = new List<beUsuario>();
            brUsuario obrUsuario = new brUsuario();
            listaUsuarios = obrUsuario.obtenerInfoUsuarios(parametros);
            return (listaUsuarios);
        }

        protected void limpiarControles()
        {
            ddlTipoDoc.SelectedValue = "0";
            txtNumeroDoc.Text = String.Empty;
            txtApePat.Text = String.Empty;
            txtApeMat.Text = String.Empty;
            txtNombres.Text = String.Empty;
            txtCorreo.Text = String.Empty;
            txtAlias.Text = String.Empty;
            ddlRol.SelectedValue = "0";
        }

        protected void bloqControles(bool valor)
        {
            ddlTipoDoc.Enabled = valor;
            txtNumeroDoc.Enabled = valor;
            txtApePat.Enabled = valor;
            txtApeMat.Enabled = valor;
            txtNombres.Enabled = valor;
            txtCorreo.Enabled = valor;
            txtAlias.Enabled = valor;
            ddlRol.Enabled = valor;
            btnNuevo.Visible = !valor;
            btnEditar.Visible = !valor;
            btnGuardar.Visible = valor;
            btnCancelar.Visible = valor;
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiarControles();
            bloqControles(true);
            ViewState["accionGuardar"] = "nuevo";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(1);", true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            bloqControles(false);
            ViewState["accionGuardar"] = null;
            btnEditar.Enabled = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(1);", true);
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            bloqControles(true);
            ViewState["accionGuardar"] = "editar";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(1);", true);
        }

        protected void cargarOtros(object sender, EventArgs e)
        {
            try
            {
                // CADENA CON
                string cadenaCon = obrGeneral.CadenaConexion;
                lblConexion.Text = cadenaCon;
                // LOG
                string rutaLog = obrGeneral.RutaLog;
                if (File.Exists(rutaLog))
                {
                    txtLog.Text = File.ReadAllText(rutaLog);
                }
            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tabActual", "tabActual(2);", true);
        }

        protected void buscarUsuarios(object sender, EventArgs e)
        {
            try
            {
                List<beUsuario> listafiltro = new List<beUsuario>();
                beUsuario parametros = new beUsuario();
                parametros.Usuarioid = 0;
                xlistaUsuarios = obtenerInfoUsuarios(parametros); 
                gvUsuarios.Columns[7].Visible = true;
                gvUsuarios.DataSource = xlistaUsuarios; 
                gvUsuarios.DataBind();
                gvUsuarios.Columns[7].Visible = false;
            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }
    }
}