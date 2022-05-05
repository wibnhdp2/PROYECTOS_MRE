using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SGAC.BE.MRE;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.WebApp.Accesorios;
using SGAC.Controlador;
using SGAC.Registro.Persona.BL;
using System.Web.Services;

namespace SGAC.WebApp.Registro
{
    public partial class FrmAnularTramite : System.Web.UI.Page
    {
        #region CAMPOS
        private string strVariableDecision = "TramiteAnulacion_Decision";
        private string strAuditoriaDataAnulacion = "Auditoria_Data_Anulacion";
        private string strFuncionarioAnulaId = "FuncionarioAnulacionId";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarFuncionarios();             
            }
            txtMotivo.Focus();
        }
        protected void CargarFuncionarios()
        {
            try
            {
                FuncionarioConsultaBL BL = new FuncionarioConsultaBL();                
                DataTable dt = new DataTable();

                dt=BL.Funcionario_MRE(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
               
                ddlFuncionarioAnulacion.Items.Clear();
                ddlFuncionarioAnulacion.DataTextField = "vFuncionario";
                ddlFuncionarioAnulacion.DataValueField = "IFuncionarioId";
                ddlFuncionarioAnulacion.DataSource = dt;
                ddlFuncionarioAnulacion.DataBind();
                ddlFuncionarioAnulacion.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));

                ddlFuncionarioDoc.Items.Clear();
                ddlFuncionarioDoc.DataTextField = "sDocumentoFuncionario";
                ddlFuncionarioDoc.DataValueField = "IFuncionarioId";
                ddlFuncionarioDoc.DataSource = dt;
                ddlFuncionarioDoc.DataBind();
                ddlFuncionarioDoc.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));

            }
            catch (Exception ex)
            {
                #region Registro Incidencia
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_vValoresTabla = "ANULAR TRÁMITE - CARGA DE FUNCIONARIOS",
                    audi_sOperacionTipoId = (int)Enumerador.enmTipoIncidencia.ERROR_APLICATION,
                    audi_sOperacionResultadoId = (int)Enumerador.enmResultadoAuditoria.ERR,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                    audi_vComentario = ex.Message,
                    audi_vMensaje = ex.StackTrace,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]),
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });
                #endregion
            }
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string AuditoriaData;
                string FuncionarioData;
                string MotivoData;
                if (txtAutorizacion.Text == (ddlFuncionarioDoc.SelectedItem).ToString())
                {
                    Session[strVariableDecision] = 1;
                    FuncionarioData = ddlFuncionarioAnulacion.SelectedItem.ToString();
                    MotivoData = txtMotivo.Text.ToUpper();
                    AuditoriaData = FuncionarioData + " | " + MotivoData;
                    Session[strAuditoriaDataAnulacion] = AuditoriaData;
                    Session[strFuncionarioAnulaId] = ddlFuncionarioDoc.SelectedValue.ToString();

                    Comun.EjecutarScript(this, "window.parent.close_ModalPopup('MainContent_btnEjecutarAnulacion');");
                }
                else
                {
                    Session[strVariableDecision] = 0;
                    lblValidacion.Visible = true;
                    lblValidacion.Text = "La Clave ingresada no es correcta";
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }                 
            
        }

        protected void ddlFuncionarioAnulacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlFuncionarioAnulacion.SelectedIndex > 0)
                ddlFuncionarioDoc.SelectedIndex = ddlFuncionarioAnulacion.SelectedIndex;
            else
                ddlFuncionarioDoc.SelectedIndex = 0;
            lblValidacion.Text = " ";
        }

        protected void txtMotivo_TextChanged(object sender, EventArgs e)
        {
            lblValidacion.Text = " ";
        }

        protected void txtAutorizacion_TextChanged(object sender, EventArgs e)
        {
            lblValidacion.Text = " ";
        }            
    }
}