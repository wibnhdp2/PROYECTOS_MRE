using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices;
using System.Management;
using System.Security.Principal;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Data;
using System.Net;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using Seguridad.Logica.BussinessEntity;
using Seguridad.Logica.BussinessLogic;
using SAE.UInterfaces;

using SolCARDIP.Librerias.ReglasNegocio;
using SolCARDIP.Librerias.EntidadesNegocio;
using SolCARDIP.App_Code;
using System.Reflection;

namespace SolCARDIP
{
    public partial class Login : System.Web.UI.Page
    {
        private static DirectoryEntry entry;
        private static UIEncriptador UIEncripto = new UIEncriptador();
        private const string K_ERROR_AUTENTIFICACION_ACTIVE = "Error de inicio de sesión: nombre de usuario desconocido o contraseña incorrecta. ";
        private const string K_LOGON_USER_TEXT = "LOGON_USER";
        private const string K_BARRAS = "\\";
        private string _filterAttribute;
        Utilitarios objUtil = new Utilitarios();
        CodigoUsuario oCodigoUsuario = new CodigoUsuario();
        brGeneral obrGeneral = new brGeneral();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    ViewState["IP"] = oCodigoUsuario.obtenerIP();
                    this.txtLogin.Text = Request.ServerVariables[K_LOGON_USER_TEXT];
                    Version ver = Assembly.GetExecutingAssembly().GetName().Version; 
                    lblfechaUpdate.Text = System.Web.Configuration.WebConfigurationManager.AppSettings["FechaUpdate"];
                    lblVersion.Text = "Versión:" + ver.ToString();

                    txtLogin.Focus();
                    crearGrafico();
                    ViewState["contadorLogin"] = "0";
                }
                else
                {

                }
            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }

        protected void btnLoginUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    login();
                }
                catch (Exception ex)
                {
                    objUtil.mensajeAJAX(this.Page, ex.Message);
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                objUtil.mensajeAJAX(this.Page, ex.Message);
                throw ex;
            }

        }

        private void login()
        {

            //string strdb = UIEncripto.DesEncriptarCadena("jUuHzEblhvt4O/lGyPzI+t8GDSx3c/ba16QpqqG9hbWLAgWQ3qveQdp44Z3utHCi2Qp1+mQjrBSeIBzdQiPGOVshayilmcKz+0xRtKkqCPirovHTzVZVzV5CF1LGRI23xdzUS8DAAHXYgD1VcjlohQ==");
            
            int iMarca = 0;
            string sError = "N";
            int contador = int.Parse(ViewState["contadorLogin"].ToString());
            string strUsuarioNT = string.Empty, strDominio = string.Empty;
            int intPos = 0;
            
            intPos = txtLogin.Text.IndexOf(K_BARRAS);
            if (intPos > 0)
            {
                if (intPos > 0) intPos++;
                strUsuarioNT = txtLogin.Text.Substring(intPos, txtLogin.Text.Length - intPos);
                strDominio = txtLogin.Text.Substring(0, intPos - 1);
            }
            else
            {
                strDominio = "MRENT";
                strUsuarioNT = txtLogin.Text.Trim();
            }
            //if (!Regex.IsMatch(this.txtLogin.Text, @"^[a-zA-Z0-9_]{8,15}$"))
            //{
            //    objUtil.muestraMensaje(this.Page, "Formato de Usuario no valido");
            //    this.txtLogin.Text = "";
            //    return;
            //}
            if (this.txtLogin.Text.Trim().Length == 0)
            {
                //objUtil.mensajeAJAX(this.Page, "Ingrese el nombre de usuario");
                this.txtLogin.Text = "";
                txtLogin.Focus();
                lblMensajeCaptcha.Text = "Ingrese el nombre de usuario";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "desLbl", "desLbl();", true);
                crearGrafico();
                return;
            }
            // Verificar que una contraseña contiene letras tanto en mayusculas 
            // y minusculas como caracteres numericos. También verifica que el 
            // tamaño de la contraseña sea de 8 a 10 caracteres.
            // @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,10}$"

            //if (!Regex.IsMatch(this.txtPassword.Text,
            //                  @"^[a-zA-Z0-9_]{8,15}$"))
            //{
            //    objUtil.muestraMensaje(this.Page, "Formato de Contraseña no valido");
            //    this.txtPassword.Text = "";
            //    return;
            //}

            if (this.txtPassword.Text.Trim().Length == 0)
            {
                //objUtil.mensajeAJAX(this.Page, "Ingrese la contraseña");
                this.txtPassword.Text = "";
                txtPassword.Focus();
                lblMensajeCaptcha.Text = "Ingrese la contraseña";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "desLbl", "desLbl();", true);
                crearGrafico();
                return;
            }
            string strIngresado = txtCodCaptcha.Text.Trim();
            if (strIngresado.Equals(""))
            {
                //objUtil.mensajeAJAX(this.Page, "Ingrese el codigo captcha solicitado");
                this.txtCodCaptcha.Text = "";
                txtCodCaptcha.Focus();
                lblMensajeCaptcha.Text = "Ingrese el codigo captcha";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "desLbl", "desLbl();", true);
                crearGrafico();
                return;
            }
            strIngresado = strIngresado.ToUpper();
            //bool exitoCaptcha = compararCaptcha(strIngresado);
            //bool exitoCaptcha = true;
            try
            {
                if (!compararCaptcha(strIngresado))
                {
                    sError = "Codigo Captcha incorrecto.";
                    crearGrafico();
                    contador++;
                    ViewState["contadorLogin"] = contador.ToString();
                }
                else
                {
                    
                    
                    bool respuesta = false;
                    string sLDAPEnc = System.Web.Configuration.WebConfigurationManager.AppSettings["Dominio"];
                    string sLDAP = UIEncripto.DesEncriptarCadena(sLDAPEnc);

                    if (obrGeneral.host.Contains("localhost"))
                    {
                        respuesta = true;
                    }
                    else
                    {
                        respuesta =  IsAuthenticated(sLDAP, strDominio, strUsuarioNT, txtPassword.Text.Trim());
                    }
                    
                    if (respuesta == true)
                    {
                        //SI respuesta ES TRUE SE CONSULTA DE ACUERDO AL ALIAS EN BASE DE DATOS
                        csUsuarioBE objUsuarioBE = new csUsuarioBE();
                        csUsuarioBL objUsuarioBL = new csUsuarioBL();
                        DataTable dt = new DataTable();
                        csTablaBE objBE = new csTablaBE();
                        string scodsistemaEnc = System.Web.Configuration.WebConfigurationManager.AppSettings["IdSistema"];
                        string scodsistema =  UIEncripto.DesEncriptarCadena(scodsistemaEnc);
                        objBE = objUsuarioBL.ConsultarLogin(strUsuarioNT, scodsistema, 1, 1, "N");
                        dt = objBE.dtRegistros;
                        if (dt.Rows.Count > 0)
                        {
                            objUsuarioBE.UsuarioId = dt.Rows[0]["CODIGO_USUARIO"].ToString().Trim();
                            objUsuarioBE.EmpresaId = dt.Rows[0]["USUA_SEMPRESAID"].ToString().Trim();
                            objUsuarioBE.Alias = dt.Rows[0]["CUENTA"].ToString().Trim();
                            objUsuarioBE.ApellidoPaterno = dt.Rows[0]["PATERNO"].ToString().Trim();
                            objUsuarioBE.ApellidoMaterno = dt.Rows[0]["MATERNO"].ToString().Trim();
                            objUsuarioBE.Nombres = dt.Rows[0]["NOMBRES"].ToString().Trim();
                            objUsuarioBE.NombreCompleto = dt.Rows[0]["NOMBRE_COMPLETO"].ToString().Trim();
                            objUsuarioBE.DocumentoTipoId = dt.Rows[0]["TIPO_DOC"].ToString().Trim();
                            objUsuarioBE.Documento = dt.Rows[0]["DOCUMENTO"].ToString().Trim();
                            objUsuarioBE.DocumentoNumero = dt.Rows[0]["NUMERO_DOC"].ToString().Trim();
                            objUsuarioBE.CorreoElectronico = dt.Rows[0]["EMAIL"].ToString().Trim();
                            objUsuarioBE.DireccionIP = dt.Rows[0]["DIRECCIONIP"].ToString().Trim();
                            objUsuarioBE.Entidad = dt.Rows[0]["ENTIDAD"].ToString().Trim();
                            objUsuarioBE.codUsuarioRol = dt.Rows[0]["USRO_SUSUARIOROLID"].ToString().Trim();
                            objUsuarioBE.codRol = dt.Rows[0]["USRO_SROLCONFIGURACIONID"].ToString().Trim();
                            objUsuarioBE.codOficina = dt.Rows[0]["USRO_SOFICINACONSULARID"].ToString().Trim();
                            objUsuarioBE.Acceso = dt.Rows[0]["USRO_SACCESO"].ToString().Trim();
                            objUsuarioBE.codAplicacion = dt.Rows[0]["ROCO_SAPLICACIONID"].ToString().Trim();
                            objUsuarioBE.codTipoRol = dt.Rows[0]["ROCO_SROLTIPOID"].ToString().Trim();
                            objUsuarioBE.RolOpcion = dt.Rows[0]["ROCO_VROLOPCION"].ToString().Trim();
                            objUsuarioBE.NombreRol = dt.Rows[0]["NOMBRE_ROL"].ToString().Trim();
                            objUsuarioBE.NombreOficina = dt.Rows[0]["NOMBRE_OFICINA"].ToString().Trim();
                            objUsuarioBE.NombreAcceso = dt.Rows[0]["NOMBRE_ACCESO"].ToString().Trim();
                            objUsuarioBE.NombreTipoRol = dt.Rows[0]["NOMBRE_TIPOROL"].ToString().Trim();
                            objUsuarioBE.NombreSistema = dt.Rows[0]["NOMBRE_SISTEMA"].ToString().Trim();
                            objUsuarioBE.IPLocal = GetDireccionIp();
                            Session["portal"] = "N";
                            Session["usuario"] = objUsuarioBE;
                            iMarca = 1;
                        }
                        else
                        {
                            //sError = "Usuario no tiene perfil.";
                            sError = "Error al autenticar";
                            contador++;
                            ViewState["contadorLogin"] = contador.ToString();
                        }
                    }
                    else
                    {
                        sError = "Error al autenticar.";
                        contador++;
                        ViewState["contadorLogin"] = contador.ToString();
                    }
                }
                if (contador == 3)
                {
                    beUsuarioRol obeUsuarioRol = new beUsuarioRol();
                    obeUsuarioRol = usuarioAutenticar(strUsuarioNT);
                    if (obeUsuarioRol != null)
                    {
                        bool exito = usuarioBloqueo(obeUsuarioRol);
                    }
                    sError = sError + " Cuenta bloqueada por seguridad.";
                    Session["mensaje"] = "El usuario " + strUsuarioNT.ToUpper() + " ha sido bloqueado por seguridad. Comuniquese con el administrador";
                    Response.Redirect("mensajes.aspx",false);
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                contador++;
                ViewState["contadorLogin"] = contador.ToString();
                if (contador == 3)
                {
                    beUsuarioRol obeUsuarioRol = new beUsuarioRol();
                    obeUsuarioRol = usuarioAutenticar(strUsuarioNT);
                    if (obeUsuarioRol != null)
                    {
                        bool exito = usuarioBloqueo(obeUsuarioRol);
                    }
                    sError = "Error al autenticar. Cuenta bloqueada por seguridad.";
                    Session["mensaje"] = "El usuario " + strUsuarioNT.ToUpper() + " ha sido bloqueado por seguridad. Comuniquese con el administrador";
                    Response.Redirect("mensajes.aspx",false);
                }
                else
                {
                    sError = ex.Message.ToString();
                }
                crearGrafico();
            }
            if (iMarca == 1)
                Response.Redirect("Default.aspx",false);
            else
                //crearGrafico();
            objUtil.mensajeAJAX(this.Page, sError);
        }

        public bool IsAuthenticated(String _path, String domain, String username, String pwd)
        {
            try
            {
                String domainAndUsername = domain + @"\" + username;
                DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);
                Object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (result == null)
                    return false;

                _path = result.Path;
                _filterAttribute = (String)result.Properties["cn"][0];
                return true;
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                throw ex;
            }
        }

        public static string GetDireccionIp()
        {
            string strHostName = Dns.GetHostName();
            IPHostEntry IPEntry = Dns.GetHostEntry(strHostName);

            string strDireccionIP = IPEntry.AddressList.GetValue(1).ToString();
            return strDireccionIP;
        }

        public static bool ExisteUsuario(string strAlias)
        {
            bool bolExiste = false;

            string _pathEnc = ConfigurationManager.AppSettings["Dominio"];
            string _path = UIEncripto.DesEncriptarCadena(_pathEnc);

            string domain = "MRENT";
            String domainAndUsername = domain + @"\" + strAlias;
            entry = new DirectoryEntry();
            Object obj = entry.NativeObject;

            DirectorySearcher search = new DirectorySearcher(entry);
            search.Filter = "(SAMAccountName=" + strAlias + ")";
            search.PropertiesToLoad.Add("cn");
            SearchResult result = search.FindOne();

            if (result == null)
                bolExiste = false;
            else
            {
                _path = result.Path;
                bolExiste = true;
            }

            return bolExiste;
        }

        public beUsuarioRol usuarioAutenticar(string usuarioAlias)
        {
            beUsuarioRol obeUsuarioRol = new beUsuarioRol();
            try
            {
                string idSistemaEnc = ConfigurationManager.AppSettings["IdSistema"].ToString();
                short idSistema = short.Parse(UIEncripto.DesEncriptarCadena(idSistemaEnc));

                beUsuarioRol parametros = new beUsuarioRol();
                parametros.usuarioAlias = usuarioAlias;
                parametros.idSIstema = idSistema;
                parametros.Ipmodificacion = ViewState["IP"].ToString();
                brUsuarioRol obrUsuarioRol = new brUsuarioRol();
                obeUsuarioRol = obrUsuarioRol.usuarioAutenticar(parametros);
                if (obeUsuarioRol.Usuarioid <= 0)
                {
                    obeUsuarioRol = null;
                }
            }
            catch (Exception ex)
            {
                obrGeneral.grabarLog(ex);
                obeUsuarioRol = null;
            }
            return obeUsuarioRol;
        }

        public bool usuarioBloqueo(beUsuarioRol parametros)
        {
            bool exito = false;
            try
            {
                beUsuario obeUsuario_Crea = new beUsuario();
                obeUsuario_Crea.Usuarioid = parametros.Usuarioid;
                beUsuario obeUsuario_Bloq = new beUsuario();
                obeUsuario_Bloq.BloqueoActiva = true;
                obeUsuario_Bloq.Usuarioid = parametros.Usuarioid;
                obeUsuario_Bloq.Ipmodificacion = ViewState["IP"].ToString();
                brUsuario obrUsuario = new brUsuario();
                exito = obrUsuario.bloqueoActiva(obeUsuario_Crea, obeUsuario_Bloq, "Sesión Bloqueada");
            }
            catch (Exception ex)
            {
                exito = false;
                obrGeneral.grabarLog(ex);
            }
            return exito;
        }

        private char generarCaracter()
        {
            Random oAzar = new Random();
            int n = oAzar.Next(26) + 65;
            System.Threading.Thread.Sleep(15);
            return ((char)n);
        }

        private void crearGrafico()
        {
            #region Random Color
            var arrColor1 = new List<string> { "#E7ECF2", "#7C8EA6", "#9CBAC4", "#CAE5D6", "#E5E4CA", "#B28F8A", "#C6B5C6" };
            var arrColor2 = new List<string> { "#21497C", "#303A46", "#057398", "#437A5B", "#95936B", "#90493F", "#766876" };
            Random colorRandom = new Random();
            int indexColor1 = colorRandom.Next(arrColor1.Count);
            int indexColor2 = colorRandom.Next(arrColor1.Count);
            string color1 = arrColor1[indexColor1];
            string color2 = arrColor2[indexColor2];
            Color _color1 = ColorTranslator.FromHtml(color1);
            Color _color2 = ColorTranslator.FromHtml(color2);
            #endregion
            Rectangle rect = new Rectangle(0, 0, 200, 80);
            LinearGradientBrush deg = new LinearGradientBrush(rect, _color1, _color2, LinearGradientMode.Vertical);
            Bitmap bmp = new Bitmap(200, 80);
            Graphics grafico = Graphics.FromImage(bmp);
            grafico.FillRectangle(deg, rect);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                sb.Append(generarCaracter());
            }
            ViewState["CodCaptcha"] = sb.ToString();
            grafico.DrawString(sb.ToString(), new Font("Consolas", 30, FontStyle.Italic | FontStyle.Underline), Brushes.White, 35, 10);
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Jpeg);
            byte[] buffer = ms.ToArray();
            ms.Close();
            imgCaptcha.Src = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(buffer));
            if (obrGeneral.host.Contains("localhost"))
            {
                txtCodCaptcha.Text = sb.ToString();
            }
        }

        protected void lbnActualizarCaptcha_Click(object sender, EventArgs e)
        {
            crearGrafico();
        }

        protected bool compararCaptcha(string strIngresado)
        {
            string codCaptcha = ViewState["CodCaptcha"].ToString();
            return String.Equals(strIngresado, codCaptcha);
        }
    }
}