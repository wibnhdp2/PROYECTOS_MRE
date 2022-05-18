using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Almacen.BL;
using SGAC.BE;
using SGAC.WebApp.Accesorios;
using SGAC.Accesorios;
namespace SGAC.WebApp.Accesorios.SharedControls
{
    public partial class ctrlBajaAutoadhesivo : System.Web.UI.UserControl
    {
        public delegate void OnButtonAnularClick();
        public event OnButtonAnularClick btnAnularHandler;

        public delegate void OnButtonAceptarAnulacionClick();
        public event OnButtonAceptarAnulacionClick btnAceptarAnularHandler;
        public bool Activar
        {
            set { btnEliminar.Enabled = value; }
            get { return btnEliminar.Enabled; }
        }
        public string CodInsumo
        {
            set { hInsumoID.Value = value; }
            get { return hInsumoID.Value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //------------------------------------------------------------------------
        // Autor: Jonatan Silva Cachay
        // Fecha: 16/02/2017
        // Objetivo: Ejecuta el Boton del Popup para anular el insumo
        //------------------------------------------------------------------------
        protected void BtnAceptarBaja_Click(object sender, EventArgs e)
        {
            Int64 intInsumoId = 0;

            if (hInsumoID.Value == "undefined")
            {
                return;
            }

            if (hInsumoID.Value != "")
            {
                intInsumoId = Convert.ToInt32(hInsumoID.Value);
            }

            try
            {
                AL_INSUMO oAL_INSUMO = new AL_INSUMO();
                oAL_INSUMO.insu_iInsumoId = intInsumoId;
                oAL_INSUMO.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                oAL_INSUMO.insu_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                oAL_INSUMO.insu_vMotivoBaja = txtMotivo.Text;
                InsumoMantenimientoBL oInsumoMantenimientoBL = new InsumoMantenimientoBL();
                oInsumoMantenimientoBL.InsumoDarDeBaja(oAL_INSUMO);

                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "ANULAR AUTOADHESIVO", "El insumo ha sido dado de baja satisfactoriamente."));

                txtMotivo.Text = "";
                if (btnAceptarAnularHandler != null)
                {
                    btnAceptarAnularHandler();
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

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (btnAnularHandler != null)
            {
                btnAnularHandler();
            }
            //Comun.EjecutarScript(this, "Popup(" + hInsumoID.Value.ToString() + ");");
        }
    }
}