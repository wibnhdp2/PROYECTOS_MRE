 using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Globalization;
using System.Threading;
using SGAC.Accesorios;
using System.Web.SessionState;
using System.Data;

namespace SGAC.WebApp.Accesorios 
{
    public class MyBasePage : System.Web.UI.Page
    {
        #region Atributos

        #endregion

        #region Propiedades

        #endregion

        #region Eventos
        /// <summary>
        /// Se genera cuando la fase de inicio se ha completado y antes de que comience la fase de inicialización.
        /// Utilice este evento para lo siguiente:
        /// --Examine la propiedad IsPostBack para determinar si es la primera vez que se procesa la página. 
        ///     En este momento también se han establecido las propiedades IsCallback e IsCrossPagePostBack.
        /// --Crear o volver a crear controles dinámicos.
        ///    Normalmente es usado para añadir dinámicamente controles a la pagina, porque añadiéndolos aquí 
        ///    garantizamos que a dichos controles se les apliquen adecuadamente los Skins del Theme definido 
        ///    (si no hemos definido ningún Theme daría un poco igual añadirlos aquí o en Init, por ejemplo).
        /// --Establecer una página maestra de forma dinámica.
        /// --Establecer la propiedad Theme de forma dinámica.
        /// --Leer o establecer los valores de las propiedades de perfil.
        /// Nota	
        ///     Si la solicitud es una devolución de datos, los valores de los controles todavía no se han restaurado 
        ///     del estado de vista.Si establece una propiedad de un control en esta fase, es posible que su valor se
        ///     sobrescriba en el evento siguiente.    
        /// </summary>   
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            Response.Cache.SetNoStore();
        }

        /// <summary>
        /// Se provoca cuanto todos los controles se han inicializado y se aplicado la configuración de máscara. 
        /// El evento Init de controles individuales se produce antes del evento Init de la página.
        /// Este evento ocurre después de que todos los controles de la pagina ya tienen definidos sus Theme . 
        /// Utilice este evento para leer o inicializar las propiedades del control.
        /// </summary>    
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Session[Constantes.CONST_SESION_USUARIO] == null)
            {
                Response.Redirect("~/Cuenta/FrmLogin.aspx");
            }
        }

        /// <summary>
        /// si se produce una excepción no controlada durante el proceso de la página, se desencadena el evento
        /// donde el evento no es determinista
        /// </summary>   
        protected override void OnError(System.EventArgs e)
        {
            base.OnError(e);

            Exception exc_Exception = (Exception)Server.GetLastError();
            string error = exc_Exception.Message;
            if (error.Contains("The operation is not valid for the state") || error.Contains("La operación no es válida para el estado"))
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", "HTTP 404 - intentelo de nuevo", true);
            }
            else {
                Session["_LastException"] = exc_Exception;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }
        }
       
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            string str_NamePagina = this.Request.Url.AbsolutePath.Substring(this.Request.Url.AbsolutePath.LastIndexOf('/') + 1, this.Request.Url.AbsolutePath.Length - this.Request.Url.AbsolutePath.LastIndexOf('/') - 1);

            if (str_NamePagina.Equals("FrmNotificacion.aspx") &&
                Convert.ToInt32(Session[Constantes.CONST_SESION_NOTI_REMESA]) == 1)
            {
                return;
            }
            else
            {
                DataTable dtConfiguracion = (DataTable)Session[Constantes.CONST_SESION_DT_CONFIGURACION];
                try
                {
                    if (dtConfiguracion.Rows.Count > 0)
                    {
                        // Revisión formularios independientes
                        var existe_form = (from dt in dtConfiguracion.AsEnumerable()
                                           where dt["form_vRuta"].ToString().Contains(str_NamePagina)
                                           select dt).CopyToDataTable();
                    }
                }
                catch
                {
                    // Revisión formularios dependientes
                    try
                    {
                        dtConfiguracion = (DataTable)Session[Constantes.CONST_SESION_DT_CONFIGURACION_EXTRA];
                        var existe_form = (from dt in dtConfiguracion.AsEnumerable()
                                           where dt["form_vRuta"].ToString().Contains(str_NamePagina)
                                           select dt).CopyToDataTable();
                    }
                    catch
                    {
                        Session["PAGINA_ACCEDER"] = str_NamePagina;
                        Response.Redirect("~/PageError/DeniedPage.aspx");
                    }
                }
            }
        }

        #endregion

        #region Metodos
        public DateTime  ObtenerFechaActual(HttpSessionState Session)
        {
            DateTime datFecha = DateTime.UtcNow;

            double dblDiferenciaHoraria = Convert.ToDouble(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sDiferenciaHoraria"));
            double dblHorarioVerano = Convert.ToDouble(comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_sHorarioVerano"));
            double dblHorasConsiderar = Convert.ToDouble(dblDiferenciaHoraria + dblHorarioVerano);

            datFecha = datFecha.AddHours(dblHorasConsiderar);
            return Comun.FormatearFecha(datFecha.ToString(ConfigurationManager.AppSettings["FormatoFechaLarga"]));
           
        }

        public string Ruta_Logo()
        {
            return Server.MapPath("~/Images/Escudo.PNG");
        }
        public String CultureDescription()
        {
            return CombineCultureString("Descripcion");
        }

        public String CombineCultureString(String str_pText)
        {
            return String.Format("{0}_{1}", str_pText, Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
        }
        #endregion


    }
}