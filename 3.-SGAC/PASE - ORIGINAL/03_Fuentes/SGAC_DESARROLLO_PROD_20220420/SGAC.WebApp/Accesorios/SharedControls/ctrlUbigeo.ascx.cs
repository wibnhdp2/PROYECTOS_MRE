using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using SGAC.Accesorios;
using SGAC.Configuracion.Sistema.BL;

namespace SGAC.WebApp.Accesorios.SharedControls
{

    public partial class ctrlUbigeo : System.Web.UI.UserControl
    {
        private static bool cargado = false;
        public event EventHandler Click = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (cargado != true) {
                UbigeoRefresh();
            }
        }

        public void UbigeoRefresh() {
            //----------------------------------------------------
            //Fecha: 03/04/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Cargar el departamento desde session y
            //          limpiar controles provincias y distritos
            //----------------------------------------------------
            Comun.LlenarUbigeoDptoCont_ListaItems(Session, ddl_ContDepParticipanteAP);
            Comun.limpiaCombo("--SELECCIONAR--", ddl_PaisCiudadParticipanteAP, ddl_CiudadDistritoParticipanteAP);
            //----------------------------------------------------
            //Comun.CargarUbigeo(Session, ddl_ContDepParticipanteAP, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, string.Empty, string.Empty, true);
            //Comun.CargarUbigeo(Session, ddl_PaisCiudadParticipanteAP, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, string.Empty, string.Empty, true);
            //Comun.CargarUbigeo(Session, ddl_CiudadDistritoParticipanteAP, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);

            Util.CargarParametroDropDownList(ddl_CentroPobladoParticipanteAP, new DataTable(), true);
            cargado = true;
        }

        protected void ddl_ContDepParticipanteAP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_ContDepParticipanteAP.SelectedIndex > 0)
            {
                //----------------------------------------------------
                //Fecha: 03/04/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Cargar la provincia y 
                //          limpiar el distrito
                //----------------------------------------------------
                Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_ContDepParticipanteAP.SelectedValue, "", "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, ddl_PaisCiudadParticipanteAP);
                Comun.limpiaCombo("--SELECCIONAR--", ddl_CiudadDistritoParticipanteAP);
                //----------------------------------------------------
                ddl_PaisCiudadParticipanteAP.Enabled = true;
                //Comun.CargarUbigeo(Session, ddl_PaisCiudadParticipanteAP, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_ContDepParticipanteAP.SelectedValue.ToString(), "", true);
                //Comun.CargarUbigeo(Session, ddl_CiudadDistritoParticipanteAP, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);
                
                ddl_ContDepParticipanteAP.Focus();
            }
            else
            {
                ddl_PaisCiudadParticipanteAP.DataSource = new DataTable();
                ddl_PaisCiudadParticipanteAP.DataBind();

                ddl_CiudadDistritoParticipanteAP.DataSource = new DataTable();
                ddl_CiudadDistritoParticipanteAP.DataBind();
            }
            if (Click != null)
            {
                Click(this, EventArgs.Empty);
            }
            ddl_ContDepParticipanteAP.Focus();
        }

        protected void ddl_PaisCiudadParticipanteAP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_PaisCiudadParticipanteAP.SelectedIndex > 0)
            {
                //----------------------------------------------------
                //Fecha: 03/04/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Cargar el Distrito
                //----------------------------------------------------
                Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_ContDepParticipanteAP.SelectedValue, ddl_PaisCiudadParticipanteAP.SelectedValue, "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, ddl_CiudadDistritoParticipanteAP);
                //----------------------------------------------------

                //Comun.CargarUbigeo(Session, ddl_CiudadDistritoParticipanteAP, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_ContDepParticipanteAP.SelectedValue.ToString(), ddl_PaisCiudadParticipanteAP.SelectedValue.ToString(), true);
                
                ddl_PaisCiudadParticipanteAP.Focus();
            }
            else
            {
                ddl_CiudadDistritoParticipanteAP.SelectedIndex = -1;
            }
            if (Click != null)
            {
                Click(this, EventArgs.Empty);
            }

            ddl_PaisCiudadParticipanteAP.Focus();

           
        }

        public string getResidenciaUbigeo()
        {
            string lReturn = null;
            if (ddl_ContDepParticipanteAP.SelectedIndex > 0)
            {
                int intCodigoDepa = 0;
                int intCodigoProv = 0;
                int intCodigoDist = 0;

                if (ddl_ContDepParticipanteAP.SelectedValue != "")
                {
                    intCodigoDepa = Convert.ToInt32(ddl_ContDepParticipanteAP.SelectedValue);
                }
                if (ddl_PaisCiudadParticipanteAP.SelectedValue != "")
                {
                    intCodigoProv = Convert.ToInt32(ddl_PaisCiudadParticipanteAP.SelectedValue);
                }
                if (ddl_CiudadDistritoParticipanteAP.SelectedValue != "")
                {
                    intCodigoDist = Convert.ToInt32(ddl_CiudadDistritoParticipanteAP.SelectedValue);
                }
                lReturn = intCodigoDepa.ToString("00") + intCodigoProv.ToString("00") + intCodigoDist.ToString("00");
            }
            else {
                return null;
            }
            return lReturn;
        }

        public Int32? getDepartamentoId()
        {
            Int32? loDepartamentoId = null;
            if (ddl_ContDepParticipanteAP.SelectedIndex != 0){
                loDepartamentoId = Convert.ToInt16(ddl_ContDepParticipanteAP.SelectedValue);
            }
            return loDepartamentoId;
        }
        
        public Int32? getCiudadId()
        {
            Int32? loCiudadId = null;
            if (ddl_PaisCiudadParticipanteAP.SelectedIndex != 0){
                loCiudadId = Convert.ToInt16(ddl_PaisCiudadParticipanteAP.SelectedValue);
            }
            return loCiudadId;
        }

        public Int32? getDistritoId()
        {
            Int32? loDistritoId = null;
            if (ddl_CiudadDistritoParticipanteAP.SelectedIndex != 0){
                loDistritoId = Convert.ToInt16(ddl_CiudadDistritoParticipanteAP.SelectedValue);
            }
            return loDistritoId;
        }

        public Int32? getCentroPobladoId()
        {
            Int32? loCentroPobladoId = null;
            if (ddl_CentroPobladoParticipanteAP.SelectedIndex > 0)
            {
                loCentroPobladoId = Convert.ToInt16(ddl_CentroPobladoParticipanteAP.SelectedValue);
            }
            return loCentroPobladoId;
        }


        public void setUbigeo(string ubigeo)
        {
            try{
                if (ubigeo != null)
                {
                    if (ubigeo.Trim().Length == 6)
                    {
                        string departamentoId = ubigeo.Substring(0, 2).ToString();
                        string ciudadId = ubigeo.Substring(2, 2).ToString();
                        string distritoId = ubigeo.Substring(4, 2).ToString();

                        setDepartamentoId(departamentoId);
                        setCiudadId(departamentoId, ciudadId);
                        setDistrito(departamentoId, ciudadId, distritoId);
                    }
                }
            }
            catch (Exception ex){
                throw ex;
            }
        }


      
        public void setDepartamentoId(string departamentoId)
        {
            ddl_ContDepParticipanteAP.SelectedIndex = ddl_ContDepParticipanteAP.Items.IndexOf(ddl_ContDepParticipanteAP.Items.FindByValue(departamentoId));
        }

        public void setCiudadId(string departamentoId, string ciudadId) {
            //----------------------------------------------------
            //Fecha: 03/04/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Cargar la provincia 
            //----------------------------------------------------
            Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, departamentoId, "", "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, ddl_PaisCiudadParticipanteAP);
            //----------------------------------------------------

            //Comun.CargarUbigeo(Session, ddl_PaisCiudadParticipanteAP, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, departamentoId, string.Empty, true);

            ddl_PaisCiudadParticipanteAP.SelectedIndex = ddl_PaisCiudadParticipanteAP.Items.IndexOf(ddl_PaisCiudadParticipanteAP.Items.FindByValue(ciudadId));
        }

        public void setDistrito(string departamentoId, string ciudadId, string distritoId)
        {
            //----------------------------------------------------
            //Fecha: 03/04/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Cargar el Distrito 
            //----------------------------------------------------
            Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.DISTRITO_CIUD, departamentoId, ciudadId, "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, ddl_CiudadDistritoParticipanteAP);
            //----------------------------------------------------

            //Comun.CargarUbigeo(Session, ddl_CiudadDistritoParticipanteAP, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, departamentoId, ciudadId, true);
            ddl_CiudadDistritoParticipanteAP.SelectedIndex = ddl_CiudadDistritoParticipanteAP.Items.IndexOf(ddl_CiudadDistritoParticipanteAP.Items.FindByValue(distritoId));
        }

        public void ClearControl()
        {
            ddl_ContDepParticipanteAP.ClearSelection();
            ddl_CiudadDistritoParticipanteAP.ClearSelection();
            ddl_PaisCiudadParticipanteAP.ClearSelection();
        }

        public void HabilitaControl(bool bHabilitado = true)
        {
            ddl_ContDepParticipanteAP.Enabled = bHabilitado;
            ddl_PaisCiudadParticipanteAP.Enabled = bHabilitado;
            ddl_CiudadDistritoParticipanteAP.Enabled = bHabilitado;
            ddl_CentroPobladoParticipanteAP.Enabled = bHabilitado;
        }
    }
}