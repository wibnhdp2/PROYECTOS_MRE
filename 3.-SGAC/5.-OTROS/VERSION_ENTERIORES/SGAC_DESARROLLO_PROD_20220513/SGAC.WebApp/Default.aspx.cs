using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.WebApp.Accesorios;
using System.Configuration;

namespace SGAC.WebApp
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
         
            try{
                if (((int)Session[Constantes.CONST_SESION_USUARIO_ID])==0){
                    Response.Redirect("~/Cuenta/FrmLogin.aspx");
                }
            }
            catch
            {
                Response.Redirect("~/Cuenta/FrmLogin.aspx");
            }                       

            if (!Page.IsPostBack)
            {

                if (Session["OC_NOTIFICA_REMESA"] == null)
                return;


                if (Convert.ToInt32(Session["OC_NOTIFICA_REMESA"]) == 1)
                {
                    Session.Remove("OC_NOTIFICA_REMESA");
                    Comun.EjecutarScript(this, "showModalPopup('FrmNotificacion.aspx','ALERTAS CONTABILIDAD CONSULAR',540, 1000, '" + btnEjecutarFuncionario.ClientID + "');");                    
                }
                
            }
        }

        protected void btnEjecutarFuncionario_Click(object sender, EventArgs e)
        {
        }

        //protected void btnBuscar_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("Consultas.aspx");
        //}
       
    }
}