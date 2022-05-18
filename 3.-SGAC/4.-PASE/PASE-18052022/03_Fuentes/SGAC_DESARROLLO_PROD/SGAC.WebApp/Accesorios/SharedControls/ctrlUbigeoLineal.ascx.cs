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
    public partial class ctrlUbigeoLineal : System.Web.UI.UserControl
    {
        private static bool cargado = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (cargado != true)
            {
                UbigeoRefresh();
            }
        }

        public void UbigeoRefresh()
        {   
            comun_Part3.CargarUbigeo(Session, ddl_ContDepParticipanteAP, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, string.Empty, string.Empty, true);
            comun_Part3.CargarUbigeo(Session, ddl_PaisCiudadParticipanteAP, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, string.Empty, string.Empty, true);
            comun_Part3.CargarUbigeo(Session, ddl_CiudadDistritoParticipanteAP, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);

            
            cargado = true;
        }

        protected void ddl_ContDepParticipanteAP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_ContDepParticipanteAP.SelectedIndex > 0)
            {
                ddl_PaisCiudadParticipanteAP.Enabled = true;
                comun_Part3.CargarUbigeo(Session, ddl_PaisCiudadParticipanteAP, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddl_ContDepParticipanteAP.SelectedValue.ToString(), "", true);
                comun_Part3.CargarUbigeo(Session, ddl_CiudadDistritoParticipanteAP, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);
                //ddl_CiudadDistritoParticipanteAP.DataSource = new DataTable();
                //ddl_CiudadDistritoParticipanteAP.DataBind();

                //if (ddl_PaisCiudadParticipanteAP.Enabled == true)
                //    ddl_PaisCiudadParticipanteAP.Focus();
            }
            else
            {
                ddl_PaisCiudadParticipanteAP.DataSource = new DataTable();
                ddl_PaisCiudadParticipanteAP.DataBind();

                ddl_CiudadDistritoParticipanteAP.DataSource = new DataTable();
                ddl_CiudadDistritoParticipanteAP.DataBind();
            }

            ddl_ContDepParticipanteAP.Focus();
        }

        protected void ddl_PaisCiudadParticipanteAP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_PaisCiudadParticipanteAP.SelectedIndex > 0)
            {
                comun_Part3.CargarUbigeo(Session, ddl_CiudadDistritoParticipanteAP, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddl_ContDepParticipanteAP.SelectedValue.ToString(), ddl_PaisCiudadParticipanteAP.SelectedValue.ToString(), true);
                //if (ddl_CiudadDistritoParticipanteAP.Enabled == true)
                //    ddl_CiudadDistritoParticipanteAP.Focus();
            }
            else
            {
                ddl_CiudadDistritoParticipanteAP.SelectedIndex = -1;
            }
            ddl_PaisCiudadParticipanteAP.Focus();
        }

        public string getResidenciaUbigeo()
        {
            string lReturn = null;
            if (ddl_ContDepParticipanteAP.SelectedIndex > 0)
            {
                int intCodigoDepa = Convert.ToInt32(ddl_ContDepParticipanteAP.SelectedValue);
                int intCodigoProv = Convert.ToInt32(ddl_PaisCiudadParticipanteAP.SelectedValue);
                int intCodigoDist = Convert.ToInt32(ddl_CiudadDistritoParticipanteAP.SelectedValue);
                lReturn = intCodigoDepa.ToString("00") + intCodigoProv.ToString("00") + intCodigoDist.ToString("00");
            }
            return lReturn;
        }

        public Int32? getDepartamentoId()
        {
            Int32? loDepartamentoId = null;
            if (ddl_ContDepParticipanteAP.SelectedIndex != 0)
            {
                loDepartamentoId = Convert.ToInt16(ddl_ContDepParticipanteAP.SelectedValue);
            }
            return loDepartamentoId;
        }

        public Int32? getCiudadId()
        {
            Int32? loCiudadId = null;
            if (ddl_PaisCiudadParticipanteAP.SelectedIndex != 0)
            {
                loCiudadId = Convert.ToInt16(ddl_PaisCiudadParticipanteAP.SelectedValue);
            }
            return loCiudadId;
        }

        public Int32? getDistritoId()
        {
            Int32? loDistritoId = null;
            if (ddl_CiudadDistritoParticipanteAP.SelectedIndex != 0)
            {
                loDistritoId = Convert.ToInt16(ddl_CiudadDistritoParticipanteAP.SelectedValue);
            }
            return loDistritoId;
        }



        public void setUbigeo(string ubigeo)
        {
            try
            {
                if (ubigeo != null)
                {
                    if (ubigeo.Length == 6)
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void setDepartamentoId(string departamentoId)
        {
            ddl_ContDepParticipanteAP.SelectedIndex = ddl_ContDepParticipanteAP.Items.IndexOf(ddl_ContDepParticipanteAP.Items.FindByValue(departamentoId));
        }

        public void setCiudadId(string departamentoId, string ciudadId)
        {
            comun_Part3.CargarUbigeo(Session, ddl_PaisCiudadParticipanteAP, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, departamentoId, string.Empty, true);
            ddl_PaisCiudadParticipanteAP.SelectedIndex = ddl_PaisCiudadParticipanteAP.Items.IndexOf(ddl_PaisCiudadParticipanteAP.Items.FindByValue(ciudadId));
        }

        public void setDistrito(string departamentoId, string ciudadId, string distritoId)
        {
            comun_Part3.CargarUbigeo(Session, ddl_CiudadDistritoParticipanteAP, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, departamentoId, ciudadId, true);
            ddl_CiudadDistritoParticipanteAP.SelectedIndex = ddl_CiudadDistritoParticipanteAP.Items.IndexOf(ddl_CiudadDistritoParticipanteAP.Items.FindByValue(distritoId));
        }

        public void ClearControl()
        {
            ddl_ContDepParticipanteAP.ClearSelection();
            ddl_CiudadDistritoParticipanteAP.ClearSelection();
            ddl_PaisCiudadParticipanteAP.ClearSelection();
        }

        public void HabilitaControl(bool bHabilitado=true)
        {
            ddl_ContDepParticipanteAP.Enabled = bHabilitado;
            ddl_PaisCiudadParticipanteAP.Enabled = bHabilitado;
            ddl_CiudadDistritoParticipanteAP.Enabled = bHabilitado;
        }

    }
}
