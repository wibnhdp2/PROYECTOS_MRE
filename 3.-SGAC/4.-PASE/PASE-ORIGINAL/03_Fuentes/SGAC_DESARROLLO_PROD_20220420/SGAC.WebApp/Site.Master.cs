using System;
using System.Data;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.Configuracion.Seguridad.BL;

namespace SGAC.WebApp
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        private double douDiferenciaHoraria = 0;
        private double intHorarioVerano = 0;
        private string formatoTCC = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["MODIFICO"] != null)
            {
                Session["MODIFICO"] = null;
                if (Session[Constantes.CONST_SESION_USUARIO] != null)
                {
                    if (Session[Constantes.CONST_SESION_USUARIO].ToString() != string.Empty)
                    {
                        lblTipoCambio.Visible = true;

                        if (Session[Constantes.CONST_SESION_TIPO_CAMBIO] != null && Session[Constantes.CONST_SESION_TIPO_MONEDA] != null)
                        {
                            lblTipoCambio.Text = "T.C. Consular: " +
                                                    Session[Constantes.CONST_SESION_TIPO_CAMBIO].ToString() + " " +
                                                    Session[Constantes.CONST_SESION_TIPO_MONEDA].ToString();
                        }
                        else
                        {
                            lblTipoCambio.Text = "T.C. Consular: ";
                        }
                        updTipoCambio.Update();
                    }
                }
            }
            if (!(IsPostBack))// || Session["AdjuntoPostback"] != null)
            {
                //Session["AdjuntoPostback"] = null;
                formatoTCC = WebConfigurationManager.AppSettings["FormatoDecimalTCC"];

                lblUserWelcome.Visible = false;

                NavigationMenu.Visible = false;
                lblOficinaConsular.Visible = false;

                lblTipoCambio.Visible = false;
                lblTipoCambioValue.Visible = false;

                NavigationMenu.Attributes.Add("onmenuitemclick", "return bPregunta();");


                try
                {
                    if (Session[Constantes.CONST_SESION_USUARIO] != null)
                    {
                        if (Session[Constantes.CONST_SESION_USUARIO].ToString() != string.Empty)
                        {
                            lblTipoCambio.Visible = true;

                            if (Session[Constantes.CONST_SESION_TIPO_CAMBIO] != null && Session[Constantes.CONST_SESION_TIPO_MONEDA] != null)
                            {
                                lblTipoCambio.Text = "T.C. Consular: " +
                                                        Session[Constantes.CONST_SESION_TIPO_CAMBIO].ToString() + " " +
                                                        Session[Constantes.CONST_SESION_TIPO_MONEDA].ToString();
                            }
                            else
                            {
                                lblTipoCambio.Text = "T.C. Consular: ";
                            }
                            updTipoCambio.Update();
                        }
                    }

                    if (Session[Constantes.CONST_SESION_USUARIO] != null)
                    {
                        if (Session[Constantes.CONST_SESION_USUARIO].ToString() != string.Empty)
                        {
                            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == (int)Constantes.CONST_OFICINACONSULAR_LIMA)
                            {
                                lblTipoCambioValue.Visible = false;
                                lblTipoCambio.Visible = false;
                            }

                            if (Session[Constantes.CONST_SESION_TIPO_MONEDA_ID] != null)
                            {
                                if (Convert.ToInt32(Session[Constantes.CONST_SESION_TIPO_MONEDA_ID]) == (int)Enumerador.enmMoneda.DOLAR_AMERICANO2)
                                {
                                    lblTipoCambioValue.Visible = false;
                                    lblTipoCambio.Visible = false;
                                }
                            }
                            lblOficinaConsular.Visible = true;

                            if (Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE] != null)
                            {
                                lblOficinaConsular.Text = Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE].ToString();
                            }
                            else
                            {
                                lblOficinaConsular.Text = "";
                            }
                            if (Session["Itinerante"].ToString() != "")
                            {
                                lblOficinaItinerante.Visible = true;
                                lblOficinaItinerante.Text = Session[Constantes.CONST_SESION_CIUDAD_ITINERANTE].ToString();
                            }
                            else
                            {
                                lblOficinaItinerante.Visible = false;
                                lblOficinaItinerante.Text = "";
                            }

                            lblTipoCambioValue.Visible = true;

                            /*Jose Caycho*/
                            lblUserWelcome.Visible = true;

                            if (Session[Constantes.CONST_SESION_IDIOMA].ToString() == "Spanish")
                            {
                                lblUserWelcome.Text = "Welcome, " + Session[Constantes.CONST_SESION_USUARIO].ToString();
                            }
                            else
                            {
                                lblUserWelcome.Text = "Bienvenido, " +
                                                        Session[Constantes.CONST_SESION_USUARIO].ToString();
                            }

                            lblRol.Text = Session[Constantes.CONST_SESION_USUARIO_ROL].ToString();
                            //if (!(IsPostBack))
                            //{
                            CargarMenu(NavigationMenu);
                            //Load_Menu();

                            //--------------------------------------------------------
                            //Fecha: 09/12/2020
                            //Autor: Miguel Márquez Beltrán
                            //Motivo: Validar la sesion cuando es nula.
                            //--------------------------------------------------------
                            if (Session[Constantes.CONST_SESION_DIFERENCIA_HORARIA] != null)
                            {
                                this.hdn_horariodiferencia.Value = Session[Constantes.CONST_SESION_DIFERENCIA_HORARIA].ToString();
                                douDiferenciaHoraria = Convert.ToDouble(this.hdn_horariodiferencia.Value);
                            }
                            if (Session[Constantes.CONST_SESION_HORARIO_VERANO] != null)
                            {
                                this.hdn_horarioverano.Value = Session[Constantes.CONST_SESION_HORARIO_VERANO].ToString();
                                intHorarioVerano = Convert.ToDouble(this.hdn_horarioverano.Value);
                            } 
                            
                            btnCerrarSesion.Visible = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        protected void smMaster_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            if (e.Exception.Data["ExtraInfo"] != null)
            {
                smMaster.AsyncPostBackErrorMessage =
                    e.Exception.Message +
                    e.Exception.Data["ExtraInfo"].ToString();
            }
            else
            {
                smMaster.AsyncPostBackErrorMessage = "Error desconocido.";
            }
        }

        #region METODOS
        StringBuilder sb_Menu = new StringBuilder();
        private void Load_Menu()
        {
            DataTable dtConfiguracion = (DataTable)Session[Constantes.CONST_SESION_DT_CONFIGURACION];
            int intReferenciaId = Convert.ToInt32(dtConfiguracion.Rows[0]["form_IReferenciaId"]);

            sb_Menu.Clear();
            sb_Menu.Append("<ul id=\"menu\">");

            foreach (DataRow dr in dtConfiguracion.Rows)
            {
                if (intReferenciaId == Convert.ToInt32(dr["form_IReferenciaId"]))
                {
                    sb_Menu.Append("<li><a href=\"" + dr["form_vRuta"].ToString() + "\">" + dr["form_vNombre"].ToString() + "</a>");
                    Load_Menu_Son(dr, dtConfiguracion);
                    sb_Menu.Append("</li>");
                }
                else
                    break;
            }
            dtConfiguracion.Dispose();
            sb_Menu.Append("<li><a href=\"#\">Cerrar Sesión</a></li>");

            sb_Menu.Append("</ul>");

        }

        private void Load_Menu_Son(DataRow drItem, DataTable dtConfiguracion)
        {
            int intFormuarioId = Convert.ToInt32(drItem["form_sFormularioId"]);
            DataView dvHijos = dtConfiguracion.DefaultView;
            dvHijos.RowFilter = " form_IReferenciaId = " + intFormuarioId;
            DataTable dtHijos = dvHijos.ToTable();
            if (dtHijos.Rows.Count > 0)
            {
                sb_Menu.Append("<ul>");
                foreach (DataRow drHijo in dtHijos.Rows)
                {
                    sb_Menu.Append("<li><a href=\"" + drHijo["form_vRuta"].ToString() + "\">" + drHijo["form_vNombre"].ToString() + "</a></li>");

                    Load_Menu_Son(drHijo, dtConfiguracion);
                }
                sb_Menu.Append("</ul>");
            }            
        }

        private void CargarMenu(Menu mnMenu)
        {
            DataTable dtConfiguracion = (DataTable)Session[Constantes.CONST_SESION_DT_CONFIGURACION];
            int intReferenciaId = Convert.ToInt32(dtConfiguracion.Rows[0]["form_IReferenciaId"]);
            mnMenu.Visible = false;

            foreach (DataRow dr in dtConfiguracion.Rows)
            {
                if (intReferenciaId == Convert.ToInt32(dr["form_IReferenciaId"]))
                {
                    if (Convert.ToBoolean(dr["form_bVisible"]))
                    {
                        MenuItem mit = new MenuItem(
                                            dr["form_vNombre"].ToString(),
                                            dr["form_sFormularioId"].ToString(),
                                            dr["form_vImagen"].ToString(),
                                            dr["form_vRuta"].ToString());

                        mnMenu.Items.Add(mit);
                        CargarHijo(mit, dr, dtConfiguracion);
                    }                    
                }
                else
                {
                    break;
                }
            }

            dtConfiguracion.Dispose();
           // mnMenu.Items.Add(new MenuItem("Cerrar Sesión", "Cerrar"));
            mnMenu.Visible = true;            
        }

        private void CargarHijo(MenuItem mitPadre, DataRow drItem, DataTable dtConfiguracion)
        {
            int intFormuarioId = Convert.ToInt32(drItem["form_sFormularioId"]);
            DataView dvHijos = dtConfiguracion.DefaultView;
            dvHijos.RowFilter = " form_IReferenciaId = " + intFormuarioId;
            DataTable dtHijos = dvHijos.ToTable();
            foreach (DataRow drHijo in dtHijos.Rows)
            {
                if (Convert.ToBoolean(drHijo["form_bVisible"]))
                {
                    MenuItem mit = new MenuItem(drHijo["form_vNombre"].ToString(),
                                            drHijo["form_sFormularioId"].ToString(),
                                            drHijo["form_vImagen"].ToString(),
                                            drHijo["form_vRuta"].ToString());

                    mitPadre.ChildItems.Add(mit);
                    CargarHijo(mit, drHijo, dtConfiguracion);
                }                
            }
        }

        //protected void NavigationMenu_MenuItemClick(object sender, MenuEventArgs e)
        //{
        //    if (e.Item.Value == "Cerrar")
        //    {
        //        UsuarioMantenimientoBL objBL = new UsuarioMantenimientoBL();
        //        objBL.Actualizar_Sesion_Activa((int)HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID], false, "");

        //        lblUserWelcome.Visible = false;
        //        Session.Abandon();
                
        //        Session.RemoveAll();

        //        Response.Redirect("~/Cuenta/FrmLogin.aspx", true);
        //    }
        //}

      
        #endregion       

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            UsuarioMantenimientoBL objBL = new UsuarioMantenimientoBL();
            objBL.Actualizar_Sesion_Activa((int)HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID], false, "");

            lblUserWelcome.Visible = false;
            Session.Abandon();

            Session.RemoveAll();

            Response.Redirect("~/Cuenta/FrmLogin.aspx", true);
        }
    } 
}
