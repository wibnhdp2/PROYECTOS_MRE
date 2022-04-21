using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.WebApp.Accesorios;
using SGAC.Configuracion.Sistema.BL;
using SGAC.Registro.Actuacion.BL;


namespace SGAC.WebApp.Consulta
{
    public partial class FrmAutoadhesivoConsular : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            //ctrlPaginadorActuacion.PageSize = Constantes.CONST_CANT_REGISTRO;
            //ctrlPaginadorActuacion.Visible = false;
            //ctrlPaginadorActuacion.PaginaActual = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //ctrlToolBarConsulta.VisibleIButtonBuscar = true;
            try
            {
                if (!Page.IsPostBack)
                {
                    HFGUID.Value = PageUniqueId.Replace("-", "");

                    ctrlBusquedaAutoadhesivo.GUID = HFGUID.Value;
                    ctrlBusquedaAutoadhesivo.Direcciona = Enumerador.enmBusquedaDirecciona.TRAMITE;
                }
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The operation is not valid for the state") || ex.ToString().Contains("La operación no es válida para el estado"))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", "HTTP 404 - intentelo de nuevo", true);
                }
                else
                {
                    Session["_LastException"] = ex;
                    Response.Redirect("../PageError/GenericErrorPage.aspx");
                }
            }
        }

        protected void btn_Consultar_Evento_Click(object sender, EventArgs e)
        {

        }

        protected void gdvActuaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gdvActuaciones_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gdvActuaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void ctrlPaginadorActuacion_Click(object sender, EventArgs e)
        {

        }

        //------------------------------------------
        //Fecha: 27/08/2018
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Crear GUID 
        //------------------------------------------        
        private string _pageUniqueId = Guid.NewGuid().ToString();

        public string PageUniqueId
        {
            get { return _pageUniqueId; }
        }

    }
}