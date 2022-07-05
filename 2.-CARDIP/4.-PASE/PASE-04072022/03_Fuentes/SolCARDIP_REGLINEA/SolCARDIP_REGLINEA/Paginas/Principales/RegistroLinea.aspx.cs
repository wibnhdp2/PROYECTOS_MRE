using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolCARDIP_REGLINEA.Paginas.Principales
{
    public partial class RegistroLinea : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //AttributeCollection attrCollection = PDFdocument.Attributes;
            //string src = attrCollection["src"];
            //string AF = Session["AF"].ToString();
            //if (AF.Equals("1")) { btnNextForm.Visible = false; }
            if (!Page.IsPostBack)
            {
                if (!Convert.ToBoolean(Session["Verifica"]))
                {
                    Response.Redirect("~/Verificar.aspx");
                    return;
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#mensajePrincipal').modal();", true);
            }
        }

        protected void nextPage(object sender, EventArgs e)
        {
            //bool exitoCaptcha = compararCaptcha(hdfldCaptchaUsuario.Value);
            //if (exitoCaptcha)
            //{
            //    PDFdocument.Attributes.Add("src", @"..\01_Gen_Codigo\GeneraCodigo.aspx");
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "mensaje", "alert('CODIGO CAPTCHA INCORRECTO');", true);
            //}
        }
    }
}