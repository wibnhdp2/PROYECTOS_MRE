using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolCARDIP_REGLINEA.Librerias.ReglasNegocio;
using SAE.UInterfaces;
using System.Configuration;
namespace SolCARDIP_REGLINEA
{
    public partial class Verificar : System.Web.UI.Page
    {
        brGeneral obrGeneral = new brGeneral();
        private static UIEncriptador UIEncripto = new UIEncriptador();
       
        string conInterConexionSAM = ConfigurationManager.AppSettings["conInterConexionSAM"].ToString().ToUpper();
        protected void Page_Load(object sender, EventArgs e)
        {

            //System.Diagnostics.Debug.Write("con_______"+UIEncripto.DesEncriptarCadena("jUuHzEblhvsgPC9duBQYcRCijUvCtvVykYC1xS07uumMLH2OwhbV+eFTIRRCO2xwVZl9Rte6tbNaP4jQZi3TjXaeDkW1cJ8oh+2DF/6w7v/9d++2HsjoJWCtG36wrWmhtFPMQN3x7Z0adZB6Y/+ZdQ==")); 
            //System.Diagnostics.Debug.Write("\ncon_______" + UIEncripto.EncriptarCadena(@"Data Source=vpipa\SQLSERVER2012; Initial Catalog=BD_CARDIP; User ID=sa; Password=1234; Trusted_Connection=False") +"\n"); 
            /*borrar=================borrar este fragmente de codigo despues de que los REqs de SAM sea subsanado=====*/
            if (conInterConexionSAM.Equals("NO"))
            {
                try
                {
                    Session["Verifica"] = true;
                    Librerias.EntidadesNegocio.bePersona be = new Librerias.EntidadesNegocio.bePersona();
                    be.Apellidopaterno = "";
                    be.Apellidomaterno = "";
                    be.Nombres = "";
                    Session["bePersona"] = be;
                    Response.Redirect("Paginas/Principales/RegistroLinea.aspx", false);
                }catch(Exception ex)
                {
                    obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                }
            }
            /*fin borrar===============================*/
        }

        protected void btnVerificar_Click(object sender, EventArgs e)
        {
            try
            {
                /*
                brRegistroLinea obj = new brRegistroLinea();
                string resultado = "NO";
                resultado = obj.VerificarInformacionSAM(txtNumDocumento.Text, txtApePaterno.Text, txtApeMaterno.Text, txtNombres.Text, Convert.ToDateTime(txtFecNac.Text));
               */
                DateTime date=Convert.ToDateTime(txtFecNac.Text);
                string d = date.Day < 10 ? "0" + date.Day : "" + date.Day;
                string m = date.Month < 10 ? "0" + date.Month : "" + date.Month;
                string fecha = "" + d + "/" +m + "/" + date.Year;
                WSsam.Service_MREClient wsSam = new WSsam.Service_MREClient();
                string resultado = wsSam.ObtenerMensajeCarpid(txtNumDocumento.Text, txtApePaterno.Text, txtApeMaterno.Text, txtNombres.Text, fecha);


                if (resultado == "SI")
                {
                   
                    Session["Verifica"] = true;
                    SolCARDIP_REGLINEA.Librerias.EntidadesNegocio.bePersona be = new SolCARDIP_REGLINEA.Librerias.EntidadesNegocio.bePersona();
                    be.Apellidopaterno = txtApePaterno.Text.ToUpper();
                    be.Apellidomaterno = txtApeMaterno.Text.ToUpper();
                    be.Nombres = txtNombres.Text.ToUpper();
                    be.Nacimientofecha = Convert.ToDateTime(txtFecNac.Text);
                    WSsam.BECarpid bc = wsSam.ObtenerDatosCarpid(txtNumDocumento.Text, fecha);
                    be.EstadoCivil=bc.EstadoCivil;
                    be.Nacionalidad = bc.Nacionalidad;
                    be.Pasaporte = bc.Pasaporte;
                    be.sexo = bc.Sexo;
                    be.TiempoPermanencia = bc.TiempoPermanencia;
                    be.TipoVisa = bc.TipoVisa;
                    be.TitularFamiliar = bc.TitularFamiliar;
                    be.Visa = bc.Visa;
                    Session["bePersona"] = be;
                    HttpContext.Current.Session["bePersona"]=be;
                    
                    Response.Redirect("Paginas/Principales/RegistroLinea.aspx",false);
                }
                else
                {
                    Session["Verifica"] = false;
                    txtApePaterno.Text = "";
                    txtApeMaterno.Text = "";
                    txtNombres.Text = "";
                    txtFecNac.Text = "";
                    txtNumDocumento.Text = "";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "infoAlert", "swal('No se encuentra la información','verifique que los datos sean válidos y vuelve a intentar');", true);
                    
                }   
                
            }
            catch (Exception ex)
            {
                string scriptError= @"swal('Verificar!', '" + ex.Message.ToString().Replace("'","") + "', 'info')";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", scriptError, true);
                obrGeneral.grabarLogEnBD(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
            
        }
    }
}