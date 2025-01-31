﻿using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.Controlador;
using SGAC.Configuracion.Seguridad.BL;
using SGAC.WebApp.Accesorios;
/*------------*/
using System.Runtime.InteropServices;  // DllImport
using System.Security.Principal; // WindowsImpersonationContext
using System.Security.Permissions;
using SGAC.Configuracion.Sistema.BL; // PermissionSetAttribute
/*------------*/
namespace SGAC.WebApp.Cuenta
{
    public partial class Login : Page
    {

        /*------------------------*/
        public enum SECURITY_IMPERSONATION_LEVEL : int
        {
            SecurityAnonymous = 0,
            SecurityIdentification = 1,
            SecurityImpersonation = 2,
            SecurityDelegation = 3
        }

        public enum ERROR_ACTIVE : int
        { 
            ERROR_LOGON_FAILURE = 1326,
            ERROR_ACCOUNT_EXPIRED = 1793,
            ERROR_PASSWORD_EXPIRED = 1330,
            ERROR_ACCOUNT_LOCKED_OUT = 1909,
            ERROR_ACCOUNT_ERRORDOMINIO = 1789,
            ERROR_ACCOUNT_ERRORRED = 1790,
            ERROR_ACCOUNT_ERROREAS = 1938
        }

        // obtains user token
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string pszUsername, string pszDomain, string pszPassword,
            int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        // closes open handes returned by LogonUser
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        // creates duplicate token handle
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateToken(IntPtr ExistingTokenHandle,
            int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);
        /*------------------------*/

        private const string K_LOGON_USER_TEXT = "LOGON_USER";

        #region Campos
        private string strMensajeCampoVacio = "No ha colocado correctamente la cuenta de usuario.";
        private string strMensajePasswordVacio = "No ha colocado correctamente la contraseña de usuario.";

       // private string strMensajeCaptchaNoValido = "Captcha no válido.";
        private string strMensajeCredencialIncorrecta = "Usuario o contraseña errada, </br> comuníquese con soportetecnico@rree.gob.pe";
        
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantes.CONST_SESION_USUARIO] != null)
            {
                if (Session[Constantes.CONST_SESION_USUARIO].ToString() != string.Empty)
                {
                    Session.RemoveAll();
                }
            }
            txtPassword.Attributes.Add("value", txtPassword.Text);
           // txtimgcode.Attributes.Add("onKeyPress", "doClick('" + btnLogin.ClientID + "',event)"); 

            if (!Page.IsPostBack)
            {
                ocultarItinerante.Visible = false;
                /*-----------------------*/
                //string sTempUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                string sTempUser = Request.ServerVariables[K_LOGON_USER_TEXT];
                

                if (sTempUser.IndexOf("\\") != -1)
                {
                    string[] aryUser = new String[2];
                    char[] splitter = { '\\' };
                    aryUser = sTempUser.Split(splitter);
                    hDominio.Value = aryUser[0];
                    hUserPC.Value = aryUser[1];
                }
                /*-----------------------*/
                this.Form.DefaultButton = this.btnLogin.UniqueID;
                
                //txtAlias.Text = HttpContext.Current.User.Identity.Name;
                //txtAlias.Focus();

                //-------------------------------------------------
                //Fecha: 27/04/2017
                //Autor: Miguel Márquez Beltrán
                //Cambio: El siguiente cambio permite asignar el 
                //         usuario de red.
                txtAlias.Text = hUserPC.Value;
                if (hUserPC.Value.Length == 0)
                { txtAlias.Focus(); }
                else
                { txtPassword.Focus(); }
                //-------------------------------------------------

                Session.Add("NumeroIntentos", 0);
                // Obtener la versión del compilado SGAC.WebApp.dll:
                Version ver = Assembly.GetExecutingAssembly().GetName().Version;                
                lblVersion.Text = "SGAC v." + ver.ToString();     
                //-----------------------------------------------------------
                // Creado por: Miguel Angel Márquez Beltrán
                // Fecha: 16-08-2016
                // Objetivo: Mostrar el titulo del sistema y la fecha de actualización del SGAC
                //-----------------------------------------------------------
                CambiarIdiomaFormulario(Enumerador.enmIdioma.ESPANIOL);
                lblFechaActualizacion.Text = "Fecha de actualización: " + System.Web.Configuration.WebConfigurationManager.AppSettings["FechaUpdate"];
                //-----------------------------------------------------------
            }
        }

        private string getPAddress()
        {
            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            return Convert.ToString(ipEntry.AddressList[ipEntry.AddressList.Length - 1]);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string strItinerante = string.Empty;
            string strCodItinerante = string.Empty;
            string strAlias = txtAlias.Text.Trim().ToUpper();
            string strPassword = txtPassword.Text.Trim();

            lblMensaje1.Text = string.Empty;
            lblMensaje.Text = string.Empty;


            if (strAlias.Length == 0)
            {
                this.lblMensaje1.Text = strMensajeCampoVacio;
                this.txtAlias.BorderColor = System.Drawing.Color.Red;
                this.lblAliasValidation.Visible = true;
                this.txtAlias.Focus();
                return;
            }
            else
            {
                this.txtAlias.BorderColor = System.Drawing.Color.Empty;
                this.lblAliasValidation.Visible = false;
                this.lblMensaje1.Text = "";
            }

            if (strPassword.Length == 0)
            {
                this.lblMensaje1.Text = strMensajePasswordVacio;
                this.txtPassword.BorderColor = System.Drawing.Color.Red;
                this.lblPasswordValidation.Visible = true;
                this.txtPassword.Focus();
                return;
            }
            else
            {
                this.txtPassword.BorderColor = System.Drawing.Color.Empty;
                this.lblPasswordValidation.Visible = false;
            }

            if (chkItinerante.Checked)
            {
                if (ddlCiudad.SelectedIndex == 0)
                {
                    Enumerador.enmTipoMensaje enmTipoMensaje = Enumerador.enmTipoMensaje.INFORMATION;
                    string strCorreo = "Debe seleccionar la ciudad Itinerante!";
                    string strScriptCorreo = Mensaje.MostrarMensaje(enmTipoMensaje, "ITINERANTES", strCorreo);
                    Comun.EjecutarScript(Page, strScriptCorreo);
                    strItinerante = "";
                    strCodItinerante = "";
                    return;
                }
                else
                {
                    strItinerante = "ITINERANTE - " + ddlCiudad.SelectedItem.Text;
                    strCodItinerante = ddlCiudad.SelectedValue;
                }
            }
            else {
                strItinerante = "";
                strCodItinerante = "";
            }

            string str_IP = getPAddress();

            //int Intentos = Convert.ToInt32(Session["NumeroIntentos"]);
            int Intentos = 0;
            if (Page.IsValid)
            {
                try
                {
                    string cadena = HttpContext.Current.Request.Url.AbsoluteUri;
                    if (cadena.Contains("localhost"))
                    {
                        chkSalto.Checked = true;
                    }
                    else {
                        chkSalto.Checked = false;
                    }
                    //if (Session["CaptchaImageText"] == null)
                    //{
                    //    this.lblMensaje.Text = strMensajeCaptchaNoValido;
                    //    this.txtimgcode.Text = string.Empty;
                    //    this.txtPassword.Focus();
                    //    return;
                    //}

                    //if (this.txtimgcode.Text == Session["CaptchaImageText"].ToString())
                    //{
                        if (strAlias != "" && strPassword != "")
                        {
                            DataTable dtUsuario = new DataTable();
                            DateTime dtFecha = new DateTime();
                            bool Bloqueo = Usuario_Bloqueado(ref dtFecha, ref dtUsuario);

                            if (dtUsuario == null)
                            {
                                lblMensaje.Text = "El usuario ingresado no se encuentra registrado en el sistema. ";
                                return;
                            }
                            //---------------------------------------------------
                            //Fecha: 30/07/2020
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Validar si existe el usuario.
                            //---------------------------------------------------
                            if (dtUsuario.Rows.Count == 0)
                            {
                                lblMensaje.Text = "El usuario ingresado no se encuentra registrado en el sistema. ";
                                return;
                            }
                            //---------------------------------------------------
                            if (Bloqueo)
                            {
                                //---------------------------------------------------
                                //Fecha: 21/03/2019
                                //Autor: Miguel Márquez Beltrán
                                //Objetivo: Desactivar la sesión del usuario
                                //---------------------------------------------------
                                if (Autenticar(strAlias, strPassword))
                                {
                                    UsuarioMantenimientoBL obj = new UsuarioMantenimientoBL();
                                    obj.Actualizar_Sesion_Bloqueada(Convert.ToInt32(dtUsuario.Rows[0]["usro_sUsuarioId"].ToString()), false, getPAddress());
                                    obj.Actualizar_Sesion_Activa(Convert.ToInt32(dtUsuario.Rows[0]["usro_sUsuarioId"].ToString()), false, getPAddress());
                                    Bloqueo = Usuario_Bloqueado(ref dtFecha, ref dtUsuario);
                                }
                                //---------------------------------------------------

                                //this.txtimgcode.Text = string.Empty;
                                //lblMensaje.Text = "El usuario ingresado se encuentra bloqueado, por favor comuníquese con el administrador del sistema. ";
                                //EnviarCorreoDesbloquear(dtUsuario.Rows[0]["usua_vCorreoElectronico"].ToString());
                                //txtPassword.Focus();
                                Intentos = 0;
                                Session["NumeroIntentos"] = Intentos;
                                //return;
                            }
                            if (Intentos >= Convert.ToInt32(WebConfigurationManager.AppSettings["IntentosSession"]))
                            {
                                /*Se ejecuta el bloqueo del usuario*/

                                if (Session[Constantes.CONST_SESION_USUARIO_ID] != null)
                                {
                                    new SGAC.Configuracion.Seguridad.BL.UsuarioMantenimientoBL().Actualizar_Sesion_Bloqueada((int)Session[Constantes.CONST_SESION_USUARIO_ID],
                                    true, Util.ObtenerDireccionIP());

                                 //   this.txtimgcode.Text = string.Empty;
                                    lblMensaje.Text = "La Sesión ha sido bloqueada";
                                    //EnviarCorreoDesbloquear(dtUsuario.Rows[0]["usua_vCorreoElectronico"].ToString());
                                    txtPassword.Focus();
                                    Intentos = 0;
                                    Session["NumeroIntentos"] = Intentos;
                                }
                                else
                                {
                                    Intentos = 0;
                                    Session["NumeroIntentos"] = Intentos;
                                }
                                return;
                            }

                            if (Bloqueo == false)
                            {
                                //---------------------------------------------------
                                //Fecha: 21/03/2019
                                //Autor: Miguel Márquez Beltrán
                                //Objetivo: Desactivar la sesión del usuario
                                //---------------------------------------------------
                                if (chkSalto.Checked)
                                {
                                    UsuarioMantenimientoBL obj = new UsuarioMantenimientoBL();
                                    obj.Actualizar_Sesion_Activa(Convert.ToInt32(dtUsuario.Rows[0]["usro_sUsuarioId"].ToString()), false, getPAddress());
                                    Bloqueo = Usuario_Bloqueado(ref dtFecha, ref dtUsuario);
                                }
                                else{
                                    if (Autenticar(strAlias, strPassword))
                                    {
                                        UsuarioMantenimientoBL obj = new UsuarioMantenimientoBL();
                                        obj.Actualizar_Sesion_Activa(Convert.ToInt32(dtUsuario.Rows[0]["usro_sUsuarioId"].ToString()), false, getPAddress());
                                        Bloqueo = Usuario_Bloqueado(ref dtFecha, ref dtUsuario);
                                    }
                                }
                                
                                //---------------------------------------------------
                            }

                            if (chkSalto.Checked)
                            {
                                if (AutenticarSinAC(strAlias, strPassword, ref strMensajeCredencialIncorrecta, ref dtUsuario))
                                {
                                    Session.Add(Constantes.CONST_SESION_USUARIO, strAlias);
                                    Session.Add(Constantes.CONST_SESION_IDIOMA, btnFlagEs.ToolTip);

                                    Autorizar((int)Session[Constantes.CONST_SESION_USUARIO_ID]);
                                    ConfigurarSistema();

                                    Session[Constantes.CONST_SESION_CIUDAD_ITINERANTE] = strItinerante;
                                    Session[Constantes.CONST_SESION_CIUDAD_CODIGO_ITINERANTE] = strCodItinerante;
                                    //ConfigurarIdioma();
                                    EnvioAlertaPorCierreCuenta();

                                    /*Cambiando el estado de sesión*/

                                    new SGAC.Configuracion.Seguridad.BL.UsuarioMantenimientoBL().Actualizar_Sesion_Activa((int)Session[Constantes.CONST_SESION_USUARIO_ID],
                                        true, str_IP);

                                    #region Verificar Tipo de Cambio

                                    if (Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]) == 0)
                                    {
                                        // ADMINISTRADOR y SUPERADMIN
                                        if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) == (int)Enumerador.enmTipoRol.ADMINISTRATIVO ||
                                            Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) == (int)Enumerador.enmTipoRol.SUPERADMIN)
                                        {
                                            // EVALUA SI LA OFICINA ES ES JEFATURA
                                            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_REF_ID]) == 1)
                                            {
                                                Response.Redirect("~/Configuracion/FrmTipoCambioBancario.aspx");
                                            }
                                            else
                                            {
                                                Response.Redirect("~/Configuracion/FrmTipoCambioBancario.aspx");
                                                //Session.RemoveAll();
                                                //lblMensaje.Text = "El administrador de la jefatura no ha ingresado el Tipo de Cambio del día.\nComuníquese con el Administrador.";
                                            }
                                        }
                                        else
                                        {
                                            Response.Redirect("~/");
                                            //Session.RemoveAll();
                                            //lblMensaje.Text = "No ha ingresado el Tipo de Cambio del día.\nComuníquese con el Administrador.";
                                        }
                                    }
                                    else
                                    {
                                        Response.Redirect("~/");
                                    }
                                    #endregion
                                }
                                else
                                {
                                    // No hubo Autenticación
                                    //  this.txtimgcode.Text = string.Empty;
                                    lblMensaje.Text = strMensajeCredencialIncorrecta;
                                    txtPassword.Focus();

                                    Intentos++;
                                    Session["NumeroIntentos"] = Intentos;
                                }
                            }
                            else { 
                                if (Autenticar(strAlias, strPassword, ref strMensajeCredencialIncorrecta, ref dtUsuario))
                                {
                                    Session.Add(Constantes.CONST_SESION_USUARIO, strAlias);
                                    Session.Add(Constantes.CONST_SESION_IDIOMA, btnFlagEs.ToolTip);

                                    Autorizar((int)Session[Constantes.CONST_SESION_USUARIO_ID]);
                                    ConfigurarSistema();

                                    Session[Constantes.CONST_SESION_CIUDAD_ITINERANTE] = strItinerante;
                                    Session[Constantes.CONST_SESION_CIUDAD_CODIGO_ITINERANTE] = strCodItinerante;
                                    //ConfigurarIdioma();
                                    EnvioAlertaPorCierreCuenta();

                                    /*Cambiando el estado de sesión*/

                                    new SGAC.Configuracion.Seguridad.BL.UsuarioMantenimientoBL().Actualizar_Sesion_Activa((int)Session[Constantes.CONST_SESION_USUARIO_ID],
                                        true, str_IP);

                                    #region Verificar Tipo de Cambio

                                    if (Convert.ToDouble(Session[Constantes.CONST_SESION_TIPO_CAMBIO]) == 0)
                                    {
                                        // ADMINISTRADOR y SUPERADMIN
                                        if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) == (int)Enumerador.enmTipoRol.ADMINISTRATIVO ||
                                            Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) == (int)Enumerador.enmTipoRol.SUPERADMIN)
                                        {
                                            // EVALUA SI LA OFICINA ES ES JEFATURA
                                            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_REF_ID]) == 1)
                                            {
                                                Response.Redirect("~/Configuracion/FrmTipoCambioBancario.aspx");
                                            }
                                            else
                                            {
                                                Response.Redirect("~/Configuracion/FrmTipoCambioBancario.aspx");
                                                //Session.RemoveAll();
                                                //lblMensaje.Text = "El administrador de la jefatura no ha ingresado el Tipo de Cambio del día.\nComuníquese con el Administrador.";
                                            }
                                        }
                                        else
                                        {
                                            Response.Redirect("~/");
                                            //Session.RemoveAll();
                                            //lblMensaje.Text = "No ha ingresado el Tipo de Cambio del día.\nComuníquese con el Administrador.";
                                        }
                                    }
                                    else
                                    {
                                        Response.Redirect("~/");
                                    }
                                    #endregion
                                }
                                else
                                {
                                    // No hubo Autenticación
                                    //  this.txtimgcode.Text = string.Empty;
                                    lblMensaje.Text = strMensajeCredencialIncorrecta;
                                    txtPassword.Focus();

                                    Intentos++;
                                    Session["NumeroIntentos"] = Intentos;
                                }
                            }
                        }
                    //}
                    //else
                    //{
                    //    lblMensaje.Text = strMensajeCaptchaNoValido;
                    //}
                        
                }
                catch (SGACExcepcion ex)
                {
                    #region Registro Incidencia
                    new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new BE.MRE.SI_AUDITORIA
                    {
                        audi_vNombreRuta = Util.ObtenerNameForm(),
                        audi_vValoresTabla = "LOGIN",
                        audi_sOperacionTipoId = (int)Enumerador.enmTipoIncidencia.ERROR_APLICATION,
                        audi_sOperacionResultadoId = (int)Enumerador.enmResultadoAuditoria.ERR,
                        audi_sTablaId = null,
                        audi_sClavePrimaria = null,
                        audi_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                        audi_vComentario = ex.Message,
                        audi_vMensaje = ex.StackTrace,
                        audi_vHostName = Util.ObtenerHostName(),
                        audi_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]),
                        audi_vIPCreacion = Util.ObtenerDireccionIP()
                    });
                    #endregion

                    this.lblMensaje.Text = ex.Message;
                }
            }
            //else
            //{   
            //    // Página no valida
            //    if (strAlias != string.Empty && strPassword != string.Empty)
            //    {
            //        this.lblMensaje.Text = strMensajeCaptchaNoValido;
            //    }
            //}

            //if (lblMensaje1.Text != string.Empty || lblMensaje.Text != string.Empty)
            //{
            //    //this.txtimgcode.Text = string.Empty;
            //    if (txtAlias.Text.Trim() == string.Empty)
            //        this.txtAlias.Focus();
            //    else
            //        this.txtPassword.Focus();
            //}
        }

        protected void btnFlagEs_Click(object sender, ImageClickEventArgs e)
        {            
            rblLanguage.Items[0].Selected = true;
            rblLanguage.Items[1].Selected = false;            
            CambiarIdiomaFormulario(Enumerador.enmIdioma.ESPANIOL);
        }

        protected void btnFlagEn_Click(object sender, ImageClickEventArgs e)
        {
            rblLanguage.Items[0].Selected = false;
            rblLanguage.Items[1].Selected = true;                       
            CambiarIdiomaFormulario(Enumerador.enmIdioma.INGLES);
        }

        protected void rblLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblLanguage.Items[0].Selected)
            {
                CambiarIdiomaFormulario(Enumerador.enmIdioma.ESPANIOL);
            }

            if (rblLanguage.Items[1].Selected)
            {
                CambiarIdiomaFormulario(Enumerador.enmIdioma.INGLES);
            }
        }
        #endregion

        #region Metodos
        private void CambiarIdiomaFormulario(Enumerador.enmIdioma enmIdioma)
        { 
            switch (enmIdioma)
            {
                case Enumerador.enmIdioma.ESPANIOL:
                    lblAlias.Text = "Alias";
                    lblPassword.Text = "Contraseña";
                    btnLogin.Text = "Inicia Sesión";
                    lblAliasValidation.Text = "Alias es necesario";
                    lblPasswordValidation.Text = "Contraseña es necesaria";
                    lblInicioSesion.Text = "Sistema de Gestión de Autoadhesivos Consulares";
                    btnFlagEs.ToolTip = rblLanguage.Items[0].Text = "Español";
                    btnFlagEn.ToolTip = rblLanguage.Items[1].Text = "Inglés";    
                
                    break;

                case Enumerador.enmIdioma.INGLES:
                    lblAlias.Text = "Alias";
                    lblPassword.Text = "Password";
                    btnLogin.Text = "Login";                   
                    lblAliasValidation.Text = "Alias is required";
                    lblPasswordValidation.Text = "Password is required";
                    lblInicioSesion.Text = "Sistema de Gestión de Autoadhesivos Consulares";
                    btnFlagEs.ToolTip = rblLanguage.Items[0].Text = "Spanish";
                    btnFlagEn.ToolTip = rblLanguage.Items[1].Text = "English";  
                  
                    break;

                default:

                    break;
            }            
        }

        private void ConfigurarSistema()
        {
            //DataSet ds = Comun.Obtener((int)Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            //DataTable dtParametros = ds.Tables["dtParametros"];

            //---------------------------------------------------------------------
            //Autor: Miguel Márquez Beltrán
            //Fecha: 25/07/2017
            //Objetivo: Obtener el id del idioma castellano.
            //---------------------------------------------------------------------
            String strIdiomaCastellanoId = "";

            DataTable dtIdiomaCastellano = new DataTable();


            dtIdiomaCastellano = Comun.ObtenerParametroPorValor("TRADUCCION-IDIOMA", "", "", "A", "CASTELLANO");

            //dtIdiomaCastellano = dtParametros.AsEnumerable()
            //                        .Where(row => row.Field<String>("grupo") == "TRADUCCION-IDIOMA"
            //                               && row.Field<String>("descripcion") == "CASTELLANO")
            //                        .OrderBy(row => row.Field<Int16>("id"))
            //                        .CopyToDataTable();
            if (dtIdiomaCastellano.Rows.Count > 0)
            {
                strIdiomaCastellanoId = dtIdiomaCastellano.Rows[0]["para_sParametroId"].ToString();
                //strIdiomaCastellanoId = dtIdiomaCastellano.Rows[0]["id"].ToString();
                
            }
            dtIdiomaCastellano.Dispose();
            Session["NotarialIdioma"] = strIdiomaCastellanoId;
            //---------------------------------------------------------------------                                                          

            DataTable dtOficinaConsular = new DataTable();

            dtOficinaConsular = Comun.ObtenerOficinaConsularPorId(Session);

            //---------------------------------------------------------------------
            //Modificado por: Miguel Márquez Beltrán
            //Fecha: 11/11/2019
            //Motivo: Cambiar las sessiones que contienen tablas por 
            //          métodos que invocan a los procedimientos almacenados.
            //---------------------------------------------------------------------

            #region Comentada
            //DataTable dtOficinaConsular = ds.Tables["dtOficinaConsular"];
            //Session.Add(Constantes.CONST_SESION_OFICINACONSULTA_DT, dtOficinaConsular);


            //DataTable dtOficinasConsulares = ds.Tables["dtOficinasConsulares"];
            //DataTable dtTarifario = ds.Tables["dtTarifario"];
            //DataTable dtBoveda = ds.Tables["dtBoveda"];
            //DataTable dtMaestra = ds.Tables["dtMaestra"];
            //DataTable dtEstados = ds.Tables["dtEstados"];
            //DataTable dtSistema = ds.Tables["dtSistema"];
            //DataTable dtDocumentoIdentidad = ds.Tables["dtDocumentoIdentidad"];
            //DataTable dtTarifarioConsultas = ds.Tables["dtTarifarioConsultas"];
            //DataTable dtOficinasActivas = ds.Tables["dtOficinasActivas"];
            
            //DataTable dtPaises = new DataTable();
            //dtPaises = Comun.ConsultarPaises();
            //Session.Add(Constantes.CONST_SESION_TABLA_PAISES, dtPaises);
            

            //Session.Add(Constantes.CONST_SESION_DT_OFICINACONSULAR, dtOficinasConsulares);
            //Session.Add(Constantes.CONST_SESION_DT_PARAMETRO, dtParametros);
            //Session.Add(Constantes.CONST_SESION_DT_TARIFARIO, dtTarifario);
            //Session.Add(Constantes.CONST_SESION_DT_ESTADO, dtEstados);
            //Session.Add(Constantes.CONST_SESION_DT_MAESTRA, dtMaestra);
            //Session.Add(Constantes.CONST_SESION_DT_SISTEMA, dtSistema);
            //Session.Add(Constantes.CONST_SESION_DT_DOCUMENTOIDENTIDAD, dtDocumentoIdentidad);
            //Session.Add(Constantes.CONST_SESION_DT_TARIFARIOCONSULTAS, dtTarifarioConsultas);
            //Session.Add(Constantes.CONST_SESION_DT_OFICINACONSULARACTIVAS, dtOficinasActivas);            
            //Session.Add(Constantes.CONST_SESION_DT_BOVEDA, dtBoveda.AsEnumerable().OrderBy(x=>x["Descripcion"]).CopyToDataTable());
            #endregion

            if (dtOficinaConsular.Rows.Count > 0) 
            {
                double decTipoCambioBancario = Convert.ToDouble(dtOficinaConsular.Rows[0]["tica_FValorBancario"]);
                double decTipoCambio = Convert.ToDouble(dtOficinaConsular.Rows[0]["tica_FValorConsular"]);

                string intMonedaTipoid = dtOficinaConsular.Rows[0]["tica_IMonedaTipoId"].ToString();
                string strTipoMoneda = dtOficinaConsular.Rows[0]["tica_vMonedaTipo"].ToString();

                int intMonedaPaisId = Convert.ToInt32(dtOficinaConsular.Rows[0]["sMonedaPaisId"]);

                double intDiferenciaHoraria = Convert.ToDouble(dtOficinaConsular.Rows[0]["ofco_sDiferenciaHoraria"].ToString());
                Int16 intHorarioVerano = Convert.ToInt16(dtOficinaConsular.Rows[0]["ofco_sHorarioVerano"].ToString());
                Session.Add(Constantes.CONST_SESION_DIFERENCIA_HORARIA, intDiferenciaHoraria);
                Session.Add(Constantes.CONST_SESION_HORARIO_VERANO, intHorarioVerano);
                //------------------------------------------
                //Solo Variables
                //------------------------------------------
                Session["CiudadOficinaConsular"] = Convert.ToString(dtOficinaConsular.Rows[0]["vDistritoNombre"]);

                string formatoTCB, formatoTCC;
                formatoTCB = WebConfigurationManager.AppSettings["sessionFormatoDecimalTCB"];
                formatoTCC = WebConfigurationManager.AppSettings["sessionFormatoDecimalTCC"];

                Session.Add(Constantes.CONST_SESION_TIPO_CAMBIO_BANCARIO, string.Format(formatoTCB, decTipoCambioBancario));
                Session.Add(Constantes.CONST_SESION_TIPO_CAMBIO, string.Format(formatoTCC, decTipoCambio));

                //------------------------------------------
                //Solo Variables
                //------------------------------------------
                Session.Add(Constantes.CONST_SESION_TIPO_MONEDA, strTipoMoneda);
                Session.Add(Constantes.CONST_SESION_TIPO_MONEDA_ID, intMonedaTipoid);
                Session.Add(Constantes.CONST_SESION_TIPO_MONEDA_PAIS_ID, intMonedaPaisId);


                int intPaisId = Convert.ToInt32(dtOficinaConsular.Rows[0]["sPaisId"]);
                Session.Add(Constantes.CONST_SESION_PAIS_ID, intPaisId);
                
                //-----------------------------------------
                PaisConsultasBL objPaisBL = new PaisConsultasBL();
                DataTable dtPais = new DataTable();
                int intIdiomaId = 0;
                string strIdioma="";
                int IntTotalCount = 0;
                int IntTotalPages = 0;

                dtPais = objPaisBL.Consultar_Pais(intPaisId, "A", "1", 1, "N", ref IntTotalCount, ref IntTotalPages);
                if (dtPais.Rows.Count > 0)
                {
                    intIdiomaId = Convert.ToInt32(dtPais.Rows[0]["PAIS_SIDIOMA"].ToString());
                    strIdioma  = dtPais.Rows[0]["IDIOMA"].ToString().Trim().ToUpper();
                }
                Session.Add(Constantes.CONST_SESION_IDIOMA_ID, intIdiomaId);
                Session.Add(Constantes.CONST_SESION_IDIOMA_TEXTO, strIdioma);
                dtPais.Dispose();
                //-----------------------------------------

                string strOficinaConsular = dtOficinaConsular.Rows[0]["ofco_vNombre"].ToString();                
                int intOficinaConsularSuperiorId = Convert.ToInt32(dtOficinaConsular.Rows[0]["ofco_iReferenciaPadreId"]);
                
                string strAccesoOficina = dtOficinaConsular.Rows[0]["ofco_vAccesoOficina"].ToString();
                Session["bAccesoOFC"] = EsJefatura(strAccesoOficina);

                string strUbbigeo = dtOficinaConsular.Rows[0]["ofco_cUbigeoCodigo"].ToString();
                bool bEsJefatura = Convert.ToBoolean(dtOficinaConsular.Rows[0]["ofco_iJefaturaFlag"]);
                //------------------------------------------
                //Solo Variables
                //------------------------------------------
                Session.Add(Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE, strOficinaConsular);                
                Session.Add(Constantes.CONST_SESION_OFICINACONSULAR_REF_ID, intOficinaConsularSuperiorId);
                Session.Add(Constantes.CONST_SESION_UBIGEO, strUbbigeo);
                //------------------------------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 07/12/2021
                // Objetivo: Adicionar la constante Siglas de la Oficina Consular
                //------------------------------------------------------------------------
                string strSiglas = dtOficinaConsular.Rows[0]["ofco_vSiglas"].ToString();

                Session.Add(Constantes.CONST_SESSION_VSIGLAS, strSiglas);
                //------------------------------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 15/02/2017
                // Objetivo: Adicionar la constante código del local
                //------------------------------------------------------------------------
                string strCodigoLocal = dtOficinaConsular.Rows[0]["vCodigoLocal"].ToString();
                Session.Add(Constantes.CONST_SESION_OFICINACONSULAR_CODIGOLOCAL, strCodigoLocal);

                //------------------------------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 18/08/2016
                // Objetivo: Adicionar la constante si es jefatura
                //------------------------------------------------------------------------
                Session.Add(Constantes.CONST_SESION_JEFATURA_FLAG, bEsJefatura);                
                //------------------------------------------------------------------------

                //------------------------------------------------------------------------
                // Autor: Miguel Angel Márquez Beltrán
                // Fecha: 03/04/2017
                // Objetivo: Adicionar la constante lista de departamentos.
                //------------------------------------------------------------------------                
                //Session.Add(Constantes.CONST_SESION_LISTA_DPTO,Comun.CargarUbigeoDptoCont(true, "--SELECCIONAR--", "0"));
            }           
        }
        
        //private void ConfigurarIdioma()
        //{
        //}

        private bool Usuario_Bloqueado(ref DateTime dtFecha, ref DataTable dtUsuario)
        {            
            UsuarioConsultasBL objUsuarioBL = new UsuarioConsultasBL();

            dtUsuario = objUsuarioBL.Autenticar((int)Enumerador.enmAplicacion.WEB, txtAlias.Text.Trim().ToUpper(), Util.ObtenerHostName(), Util.ObtenerDireccionIP());

            if (dtUsuario != null)
            {
                //---------------------------------------------
                //Fecha: 10/07/2020
                //Autor: Miguel Márquez Beltrán
                //Motivo: Validar que exista el usuario
                //---------------------------------------------
                if (dtUsuario.Rows.Count > 0)
                {
                    int intUsuarioId = Convert.ToInt32(dtUsuario.Rows[0]["usro_sUsuarioId"]);
                    string strTipoAcceso = dtUsuario.Rows[0]["vTipoAcceso"].ToString().Trim().ToUpper();

                    try
                    {
                        dtFecha = Comun.FormatearFecha(dtUsuario.Rows[0]["usua_dFechaCaducidad"].ToString());
                    }
                    catch
                    {
                        dtFecha = DateTime.Now.AddDays(2);
                    }

                    Session.Add(Constantes.CONST_SESION_USUARIO_ID, intUsuarioId);

                    Session.Add(Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO, strTipoAcceso);

                    return Convert.ToBoolean(dtUsuario.Rows[0]["usua_bBloqueoActiva"]);
                }
            }

            return false;
        }
        private bool Autenticar(string strAlias, string strContrasenia, ref string str_Mensaje, ref DataTable dtUsuario)
        {
            bool bolAutenticado = false;

            if (DirectorioActivo.Autenticar(strAlias, strContrasenia))
            {
                #region autenticar

                Session.Add(Constantes.CONST_SESION_HOSTNAME, Util.ObtenerHostName());
                Session.Add(Constantes.CONST_SESION_DIRECCION_IP, Util.ObtenerDireccionIP());

                UsuarioConsultasBL objUsuarioBL = new UsuarioConsultasBL();                

                if (dtUsuario != null)
                {
                    if (dtUsuario.Rows.Count > 0)
                    {
                        Session.Remove(Constantes.CONST_SESION_USUARIO_ID);

                        int intUsuarioId = Convert.ToInt32(dtUsuario.Rows[0]["usro_sUsuarioId"]);
                        int intOficinaConsularId = Convert.ToInt32(dtUsuario.Rows[0]["usro_sOficinaConsularId"]);
                        int intAcceso = Convert.ToInt32(dtUsuario.Rows[0]["usro_sAcceso"]);
                        int intGrupoId = 0; 

                        if (dtUsuario.Rows[0]["usro_sGrupoId"] != null)
                        {
                            if (dtUsuario.Rows[0]["usro_sGrupoId"].ToString() != string.Empty)
                            {
                                intGrupoId = Convert.ToInt32(dtUsuario.Rows[0]["usro_sGrupoId"]);
                            }
                        }

                        int intTipoRol = Convert.ToInt32(dtUsuario.Rows[0]["roco_sRolTipoId"]);

                        Session.Add(Constantes.CONST_SESION_USUARIO_ID, intUsuarioId);
                        Session.Add(Constantes.CONST_SESION_GRUPO_ID, intGrupoId);
                        Session.Add(Constantes.CONST_SESION_ROL_ID, intTipoRol);
                        Session.Add(Constantes.CONST_SESION_ACCESO_ID, intAcceso);
                        Session.Add(Constantes.CONST_SESION_USUARIO_ROL, dtUsuario.Rows[0]["roco_vNombre"]);
                        Session.Add(Constantes.CONST_SESION_NOTI_REMESA, dtUsuario.Rows[0]["usua_bNotificaRemesa"]);

                        Session.Add(Constantes.CONST_SESION_CONTINENTE, dtUsuario.Rows[0]["vContinente"]);

                        Session.Add(Constantes.CONST_SESION_OFICINACONSULAR_ID, intOficinaConsularId);

                        Session.Add("OC_NOTIFICA_REMESA", Convert.ToInt32(dtUsuario.Rows[0]["usua_bNotificaRemesa"]));

                        str_Mensaje = "";
                        if (Convert.ToInt32(dtUsuario.Rows[0]["usua_bSesionActiva"]) == 1)
                        {
                            if (getPAddress() == Convert.ToString(dtUsuario.Rows[0]["usua_vDireccionIP"]))
                                bolAutenticado = true;
                            else
                            {
                                str_Mensaje = "El usuario ingresado ya ha iniciado sesión en el sistema, por favor comuníquese con el administrador del sistema.";
                                bolAutenticado = false;
                            }
                        }
                        else
                            bolAutenticado = true;
                    }
                }
                #endregion
            }
            else
            {
                try
                {
                    this.ImpersonateUser(txtAlias.Text, hDominio.Value, txtPassword.Text, ref str_Mensaje);
                    bolAutenticado = false;
                }
                catch {
                    str_Mensaje = strMensajeCredencialIncorrecta;
                    bolAutenticado = false;
                }
                //this.ImpersonateUser(txtAlias.Text, hDominio.Value, txtPassword.Text, ref str_Mensaje);
                //str_Mensaje = "Credenciales incorrectas";
                //bolAutenticado = false;
            }
            return bolAutenticado;
        }

        private bool AutenticarSinAC(string strAlias, string strContrasenia, ref string str_Mensaje, ref DataTable dtUsuario)
        {
            bool bolAutenticado = false;

                #region autenticar

                Session.Add(Constantes.CONST_SESION_HOSTNAME, Util.ObtenerHostName());
                Session.Add(Constantes.CONST_SESION_DIRECCION_IP, Util.ObtenerDireccionIP());

                UsuarioConsultasBL objUsuarioBL = new UsuarioConsultasBL();

                if (dtUsuario != null)
                {
                    if (dtUsuario.Rows.Count > 0)
                    {
                        Session.Remove(Constantes.CONST_SESION_USUARIO_ID);

                        int intUsuarioId = Convert.ToInt32(dtUsuario.Rows[0]["usro_sUsuarioId"]);
                        int intOficinaConsularId = Convert.ToInt32(dtUsuario.Rows[0]["usro_sOficinaConsularId"]);
                        int intAcceso = Convert.ToInt32(dtUsuario.Rows[0]["usro_sAcceso"]);
                        int intGrupoId = 0;

                        if (dtUsuario.Rows[0]["usro_sGrupoId"] != null)
                        {
                            if (dtUsuario.Rows[0]["usro_sGrupoId"].ToString() != string.Empty)
                            {
                                intGrupoId = Convert.ToInt32(dtUsuario.Rows[0]["usro_sGrupoId"]);
                            }
                        }

                        int intTipoRol = Convert.ToInt32(dtUsuario.Rows[0]["roco_sRolTipoId"]);

                        Session.Add(Constantes.CONST_SESION_USUARIO_ID, intUsuarioId);
                        Session.Add(Constantes.CONST_SESION_GRUPO_ID, intGrupoId);
                        Session.Add(Constantes.CONST_SESION_ROL_ID, intTipoRol);
                        Session.Add(Constantes.CONST_SESION_ACCESO_ID, intAcceso);
                        Session.Add(Constantes.CONST_SESION_USUARIO_ROL, dtUsuario.Rows[0]["roco_vNombre"]);
                        Session.Add(Constantes.CONST_SESION_NOTI_REMESA, dtUsuario.Rows[0]["usua_bNotificaRemesa"]);

                        Session.Add(Constantes.CONST_SESION_CONTINENTE, dtUsuario.Rows[0]["vContinente"]);

                        Session.Add(Constantes.CONST_SESION_OFICINACONSULAR_ID, intOficinaConsularId);

                        Session.Add("OC_NOTIFICA_REMESA", Convert.ToInt32(dtUsuario.Rows[0]["usua_bNotificaRemesa"]));

                        str_Mensaje = "";
                        if (Convert.ToInt32(dtUsuario.Rows[0]["usua_bSesionActiva"]) == 1)
                        {
                            if (getPAddress() == Convert.ToString(dtUsuario.Rows[0]["usua_vDireccionIP"]))
                                bolAutenticado = true;
                            else
                            {
                                str_Mensaje = "El usuario ingresado ya ha iniciado sesión en el sistema, por favor comuníquese con el administrador del sistema.";
                                bolAutenticado = false;
                            }
                        }
                        else
                            bolAutenticado = true;
                    }
                }
                #endregion
            
            return bolAutenticado;
        }
        private bool Autenticar(string strAlias, string strContrasenia)
        {
            bool bolAutenticado = false;

            if (DirectorioActivo.Autenticar(strAlias, strContrasenia))
            {
                bolAutenticado = true;
            }
            return bolAutenticado;
        }  

        private void Autorizar(int intUsuarioId)
        {
            UsuarioConsultasBL objUsuarioBL = new UsuarioConsultasBL();

            DataSet dsConfiguracion = objUsuarioBL.Autorizar((int)Enumerador.enmAplicacion.WEB, intUsuarioId);

            Session.Add(Constantes.CONST_SESION_DT_CONFIGURACION, dsConfiguracion.Tables[0]);
            Session.Add(Constantes.CONST_SESION_DT_CONFIGURACION_EXTRA, dsConfiguracion.Tables[1]);


        }

        public void EnvioAlertaPorCierreCuenta()
        {
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_ROL_ID]) == (int)Enumerador.enmTipoRol.ADMINISTRATIVO)
            {
                //object[] arrParametros = { Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) };
                //Proceso p = new Proceso();
                //bool bolAvisar = Convert.ToBoolean(p.Invocar(ref arrParametros, "SGAC.BE.CO_REMESA", "AVISAR"));

                SGAC.Contabilidad.Remesa.BL.RemesaConsultasBL objRemesaConsultaBL = new SGAC.Contabilidad.Remesa.BL.RemesaConsultasBL();
                bool bolAvisar = objRemesaConsultaBL.ConsultarAvisoEnvioRemesa(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                

                if (bolAvisar)
                {
                    Session["MostrarAlertaPendiente"] = 1;
                }
            }
        }

        private bool EsJefatura(string strAccesoOficina)
        {
            bool bolEsDependiente = false;

            if (strAccesoOficina != null)
            {
                string[] arrDependencias = strAccesoOficina.Split(',');
                if (arrDependencias != null)
                {
                    if (arrDependencias.Length > 1)
                    {
                        bolEsDependiente = true;
                    }
                }
            }

            return bolEsDependiente;
        }

        #endregion

        //--------------------------------------------                    
        // Creador por: Jonatan Silva Cachay
        // Fecha: 02/02/2017
        // Objetivo: Crea un datatable que se utiliza para reemplazar datos de la plantilla HTML del formato del correo
        //--------------------------------------------
        private DataTable crearTabla()
        {
            DataTable dtReemplazaCorreo = new DataTable();
            DataColumn dc1 = new DataColumn();
            DataColumn dc2 = new DataColumn();
            dc1.ColumnName = "valor";
            dc2.ColumnName = "reemplazo";
            dtReemplazaCorreo.Columns.Add(dc1);
            dtReemplazaCorreo.Columns.Add(dc2);

            /*Obtener Connacional*/
            string strEtiquetaSolicitante = string.Empty;
            string strCadenaEncriptada;
            strCadenaEncriptada = this.txtAlias.Text + DateTime.Now.ToShortDateString();
            strEtiquetaSolicitante = this.txtAlias.Text;
            strCadenaEncriptada = Util.Encriptar(strCadenaEncriptada);
            
            DataRow dr = default(DataRow);
            dr = dtReemplazaCorreo.NewRow();
            dr[0] = "{CONNACIONAL}";
            dr[1] = strEtiquetaSolicitante.ToUpper();
            dtReemplazaCorreo.Rows.Add(dr);

            DataRow dr1 = default(DataRow);
            dr1 = dtReemplazaCorreo.NewRow();
            dr1[0] = "{Codigo}";
            dr1[1] = strCadenaEncriptada;
            dtReemplazaCorreo.Rows.Add(dr1);

            DataRow dr2 = default(DataRow);
            dr2 = dtReemplazaCorreo.NewRow();
            dr2[0] = "{fechaActual}";
            dr2[1] = System.DateTime.Now;
            dtReemplazaCorreo.Rows.Add(dr2);

            return dtReemplazaCorreo;
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

            strSMTPServer = "VICUS.RREE.GOB.PE";
            strSMTPPuerto = "25";
            strEmailFrom = "ALERTAS_SIGC@RREE.GOB.PE"; //ConfigurationManager.AppSettings["ConexionSGAC"];
            strEmailPassword = "";

            strEmailTo = CorreoElectronico;
            string strTitulo = strASUNTO;

            // ENVIAR CORREO
            Enumerador.enmTipoMensaje enmTipoMensaje = Enumerador.enmTipoMensaje.INFORMATION;
            bool bEnviado = false;
            string strMensaje = string.Empty;
            string strCorreo = string.Empty;
            string strRutaCorreo = string.Empty;
            strRutaCorreo = Server.MapPath("~") + strPlantilla;
            try
            {
                bEnviado = Correo.EnviarCorreoPlantillaHTML(strRutaCorreo, _dtReemplazo, strSMTPServer, strSMTPPuerto,
                                               strEmailFrom, strEmailPassword,
                                               strEmailTo, strTitulo, System.Net.Mail.MailPriority.High, null);
            }
            catch
            {
                throw;
            }
            //--
            strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CORREO", "Se envió un correo con el código de desbloqueo.", false, 160, 300);

            #endregion

            string strScriptCorreo = string.Empty;
            if (!bEnviado)
            {
                enmTipoMensaje = Enumerador.enmTipoMensaje.ERROR;
                strCorreo = "Correo no enviado." + strCorreo;
                strScriptCorreo = Mensaje.MostrarMensaje(enmTipoMensaje, "ENVÍO CORREO", strCorreo);

                Comun.EjecutarScript(Page, strScriptCorreo);
            }
            else
            {
                if (strMensaje != string.Empty)
                {
                    strScript += Mensaje.MostrarMensaje(enmTipoMensaje, "ENVÍO CORREO", strMensaje);
                }

                Comun.EjecutarScript(Page, strScript);
            }
            return bEnviado;
        }

        //------------------------------------------------------------------------
        // Autor: JOnatan Silva Cachay
        // Fecha: 07/02/2017
        // Objetivo: Envia Correo
        //------------------------------------------------------------------------
        private void EnviarCorreoDesbloquear(string correoElectronico)
        {
            bool bEnvio = false;
            if (correoElectronico == "")
            {
                correoElectronico = txtAlias.Text.Trim() + "@RREE.GOB.PE";
            }

            DataTable _dtDatos = new DataTable();
            _dtDatos = crearTabla();
            bEnvio = EnviarCorreo(_dtDatos, correoElectronico, "CÓDIGO PARA DESBLOQUEAR", "/Registro/Plantillas/CorreoDesbloqueo.html");
        }
        //------------------------------------------------------------------------
        // Autor: JOnatan Silva Cachay
        // Fecha: 07/02/2017
        // Objetivo: Actualizar el bit de session activa y usario bloqueado
        //------------------------------------------------------------------------
        protected void btnDesbloquear_Click(object sender, EventArgs e)
        {
            if (txtCodigo.Text != "" && txtUsuario.Text != "")
            {
                DataTable dtUsuario = new DataTable();
                DateTime dtFecha = new DateTime();
                //string strValorDesencriptado =string.Empty;
                //try {
                //    strValorDesencriptado = Util.DesEncriptar(txtCodigo.Text.Trim()).ToUpper();
                //}
                //catch
                //{
                //    strValorDesencriptado = "";
                //}
                //string strCadenaDesEncriptada;
                //strCadenaDesEncriptada = this.txtUsuario.Text + DateTime.Now.ToShortDateString();
                //if (strCadenaDesEncriptada.Trim().ToUpper() == strValorDesencriptado)
                //{
                Usuario_Bloqueado(ref dtFecha, ref dtUsuario);
                if (Autenticar(txtUsuario.Text.Trim(), txtCodigo.Text.Trim()))
                {
                    UsuarioMantenimientoBL obj = new UsuarioMantenimientoBL();
                    obj.Actualizar_Sesion_Bloqueada(Convert.ToInt32(dtUsuario.Rows[0]["usro_sUsuarioId"].ToString()), false, getPAddress());
                    obj.Actualizar_Sesion_Activa(Convert.ToInt32(dtUsuario.Rows[0]["usro_sUsuarioId"].ToString()), false, getPAddress());
                    lblMensaje.Text = "";

                    Enumerador.enmTipoMensaje enmTipoMensaje = Enumerador.enmTipoMensaje.INFORMATION;
                    string strCorreo = "Se ha desbloqueado el usuario, intentar nuevamente!.";
                    string strScriptCorreo = Mensaje.MostrarMensaje(enmTipoMensaje, "Desbloqueo", strCorreo);
                    Comun.EjecutarScript(Page, strScriptCorreo);
                }
                else {
                    Enumerador.enmTipoMensaje enmTipoMensaje = Enumerador.enmTipoMensaje.INFORMATION;
                    string strCorreo = "Contraseña incorrecta, no se ha podido desbloquear.";
                    string strScriptCorreo = Mensaje.MostrarMensaje(enmTipoMensaje, "Desbloqueo", strCorreo);
                    Comun.EjecutarScript(Page, strScriptCorreo);
                }
                //bool Bloqueo = Usuario_Bloqueado(ref dtFecha, ref dtUsuario);
                //if (Bloqueo)
                //{

                    //Enumerador.enmTipoMensaje enmTipoMensaje = Enumerador.enmTipoMensaje.INFORMATION;
                    //string strCorreo = "Se ha desbloqueado el usuario, intentar nuevamente!.";
                    //string strScriptCorreo = Mensaje.MostrarMensaje(enmTipoMensaje, "Desbloqueo", strCorreo);
                    //Comun.EjecutarScript(Page, strScriptCorreo);
                //}
               // }
            }
            txtCodigo.Text = "";
            txtUsuario.Text = "";
        }
        /*------------------------*/
        //------------------------------------------------------------------------
        // Autor: JOnatan Silva Cachay
        // Fecha: 07/02/2017
        // Objetivo: Verificar la causa del error al ingresar usuario y contraseña
        //------------------------------------------------------------------------
        public WindowsImpersonationContext ImpersonateUser(string sUsername, string sDomain, string sPassword, ref string str_Mensaje)
        {
            // initialize tokens
            IntPtr pExistingTokenHandle = new IntPtr(0);
            IntPtr pDuplicateTokenHandle = new IntPtr(0);
            pExistingTokenHandle = IntPtr.Zero;
            pDuplicateTokenHandle = IntPtr.Zero;

            // if domain name was blank, assume local machine
            if (sDomain == "")
                sDomain = System.Environment.MachineName;

            try
            {
                string sResult = null;

                const int LOGON32_PROVIDER_DEFAULT = 0;

                // create token
                const int LOGON32_LOGON_INTERACTIVE = 2;
                //const int SecurityImpersonation = 2;

                // get handle to token
                bool bImpersonated = LogonUser(sUsername, sDomain, sPassword,
                    LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref pExistingTokenHandle);

                // did impersonation fail?
                if (false == bImpersonated)
                {
                    int nErrorCode = Marshal.GetLastWin32Error();
                    sResult = "LogonUser() failed with error code: " + nErrorCode + "\r\n";

                    if (nErrorCode == Convert.ToInt32(ERROR_ACTIVE.ERROR_ACCOUNT_EXPIRED))
                    {str_Mensaje = "Su cuenta ha expirado, comunicarse con OTI";}
                    else if (nErrorCode == Convert.ToInt32(ERROR_ACTIVE.ERROR_ACCOUNT_LOCKED_OUT))
                    { str_Mensaje = "Su cuenta ha sido bloqueada, comunicarse con OTI"; }
                    else if (nErrorCode == Convert.ToInt32(ERROR_ACTIVE.ERROR_LOGON_FAILURE))
                    { str_Mensaje = strMensajeCredencialIncorrecta; }
                    else if (nErrorCode == Convert.ToInt32(ERROR_ACTIVE.ERROR_PASSWORD_EXPIRED))
                    { str_Mensaje = "Su contraseña ha expirado, comunicarse con OTI"; }
                    else if (nErrorCode == Convert.ToInt32(ERROR_ACTIVE.ERROR_ACCOUNT_ERRORDOMINIO))
                    { str_Mensaje = "Ha ocurrido un error entre la estación de trabajo y el dominio principal, comunicarse con OTI"; }
                    else if (nErrorCode == Convert.ToInt32(ERROR_ACTIVE.ERROR_ACCOUNT_ERRORRED))
                    { str_Mensaje = "Se produjo un error en el inicio de sesión de red, comunicarse con OTI"; }
                    else if (nErrorCode == Convert.ToInt32(ERROR_ACTIVE.ERROR_ACCOUNT_ERROREAS))
                    { str_Mensaje = "Fallo de inicio de sesión: la directiva EAS requiere que el usuario cambie su contraseña antes de que se pueda realizar esta operación, comunicarse con OTI"; }
                    else { str_Mensaje = strMensajeCredencialIncorrecta; }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // close handle(s)
                if (pExistingTokenHandle != IntPtr.Zero)
                    CloseHandle(pExistingTokenHandle);
                if (pDuplicateTokenHandle != IntPtr.Zero)
                    CloseHandle(pDuplicateTokenHandle);
            }
        }

        protected void chkItinerante_CheckedChanged(object sender, EventArgs e)
        {
            if (chkItinerante.Checked)
            {
                try
                {
                    if (txtAlias.Text == "")
                    {
                        Enumerador.enmTipoMensaje enmTipoMensaje = Enumerador.enmTipoMensaje.INFORMATION;
                        string strCorreo = "Debe ingresar el usuario para poder seleccionar la opción de itinerario";
                        string strScriptCorreo = Mensaje.MostrarMensaje(enmTipoMensaje, "ITINERARIO", strCorreo);
                        Comun.EjecutarScript(Page, strScriptCorreo);
                        ocultarItinerante.Visible = false;
                        chkItinerante.Checked = false;
                        return;
                    }

                    ParametroConsultasBL _obj = new ParametroConsultasBL();
                    UsuarioConsultasBL objUsuarioBL = new UsuarioConsultasBL();
                    DataTable dtUsuario = objUsuarioBL.Autenticar((int)Enumerador.enmAplicacion.WEB, txtAlias.Text.Trim().ToUpper(),
                    Util.ObtenerHostName(), Util.ObtenerDireccionIP());

                    if (dtUsuario != null)
                    {
                        DataTable _dt = new DataTable();
                        _dt = _obj.ConsultarParametroPorValor("CONSULADOS - ITINERANTES", dtUsuario.Rows[0]["usro_sOficinaConsularId"].ToString());
                        if (_dt.Rows.Count > 0)
                        {
                            ddlCiudad.DataSource = _dt;
                            ddlCiudad.DataTextField = "para_vDescripcion";
                            ddlCiudad.DataValueField = "para_sParametroId";
                            ddlCiudad.DataBind();
                            ocultarItinerante.Visible = true;
                        }
                        else
                        {
                            Enumerador.enmTipoMensaje enmTipoMensaje = Enumerador.enmTipoMensaje.INFORMATION;
                            string strCorreo = "No se encontrarion datos para mostrar";
                            string strScriptCorreo = Mensaje.MostrarMensaje(enmTipoMensaje, "ITINERANTES", strCorreo);
                            Comun.EjecutarScript(Page, strScriptCorreo);
                            ocultarItinerante.Visible = false;
                            chkItinerante.Checked = false;
                            //return;
                        }
                    }
                    else {
                        Enumerador.enmTipoMensaje enmTipoMensaje = Enumerador.enmTipoMensaje.INFORMATION;
                        string strCorreo = "El usuario ingresado no existe en el sistema";
                        string strScriptCorreo = Mensaje.MostrarMensaje(enmTipoMensaje, "ITINERANTES", strCorreo);
                        Comun.EjecutarScript(Page, strScriptCorreo);
                        ocultarItinerante.Visible = false;
                        chkItinerante.Checked = false;
                       // return;
                    }   
                }
                catch { 
                    ocultarItinerante.Visible = false;
                    chkItinerante.Checked = false;
                }
                
            }
            else { ocultarItinerante.Visible = false; }
        }
        /*------------------------*/

    }
}
