using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using SGAC.BE.MRE.Custom;
using SGAC.Accesorios;
using SGAC.WebApp.Accesorios;
using System.Data;

namespace SGAC.WebApp.Registro
{
    public partial class FrmCambiaEstadoMigratorio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hID.Value = Convert.ToString(Request.QueryString["ID"]);
            hEstadoId.Value = Convert.ToString(Request.QueryString["ESTADO"]);
            if (!IsPostBack)
            {
                CargarFuncionarios(Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), 0);

                var s_Estados = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.VISA_TRAMITE);

                Util.CargarParametroDropDownList(cbo_estado, s_Estados, true);

                int intEstadoId = Convert.ToInt32(hEstadoId.Value);

                if (intEstadoId == (int)Enumerador.enmEstadoVisa.SOLICITADO)
                {
                    cbo_estado.SelectedValue = ((int)Enumerador.enmEstadoTraminte.RECHAZADO).ToString();
                    Util.CargarParametroDropDownList(cbo_Motivo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTO_MIGRATORIO_MOTIVO_RECHAZO), true);
                }
                else if (intEstadoId == (int)Enumerador.enmEstadoVisa.APROBADO)
                {
                    cbo_estado.SelectedValue = ((int)Enumerador.enmEstadoTraminte.CANCELADO).ToString();
                    Util.CargarParametroDropDownList(cbo_Motivo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTO_MIGRATORIO_MOTIVO_CANCELADO), true);
                }
            }
        }

        private void CargarFuncionarios(int sOfConsularId, int IFuncionarioId)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = funcionario.dtFuncionario(sOfConsularId, IFuncionarioId);

                ddlFuncionario.Items.Clear();
                ddlFuncionario.DataTextField = "vFuncionario";
                ddlFuncionario.DataValueField = "IFuncionarioId";
                ddlFuncionario.DataSource = dt;
                ddlFuncionario.DataBind();
                ddlFuncionario.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public static string Cambiar_Estado(string acmi_iActoMigratorioId, string amhi_IFuncionarioId, string amhi_sMotivoId,
            string acmi_sEstadoId)
        {
            string StrScript = string.Empty;
            int i_Resultado = 0;
            CBE_MIGRATORIO oRE_ACTOMIGRATORIO = new CBE_MIGRATORIO();
            oRE_ACTOMIGRATORIO.ACTO.acmi_sEstadoId = Convert.ToInt16(acmi_sEstadoId);

            #region - Llenado los datos historicos -
            BE.MRE.RE_ACTOMIGRATORIOHISTORICO oRE_ACTOMIGRATORIOHISTORICO = new BE.MRE.RE_ACTOMIGRATORIOHISTORICO();
            oRE_ACTOMIGRATORIOHISTORICO.amhi_IFuncionarioId = Convert.ToInt16(amhi_IFuncionarioId);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_sMotivoId = Convert.ToInt16(amhi_sMotivoId);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_dFechaRegistro = DateTime.Now;
            oRE_ACTOMIGRATORIOHISTORICO.amhi_vObservaciones = "";
            oRE_ACTOMIGRATORIOHISTORICO.amhi_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
            oRE_ACTOMIGRATORIOHISTORICO.amhi_dFechaCreacion = DateTime.Now;
            oRE_ACTOMIGRATORIOHISTORICO.amhi_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
            oRE_ACTOMIGRATORIOHISTORICO.amhi_dFechaModificacion = DateTime.Now;
            oRE_ACTOMIGRATORIOHISTORICO.HostName = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_HOSTNAME]);
            oRE_ACTOMIGRATORIOHISTORICO.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_sEstadoId = Convert.ToInt16(acmi_sEstadoId);
            #endregion

            oRE_ACTOMIGRATORIO.ACTO.acmi_iActoMigratorioId = Convert.ToInt64(acmi_iActoMigratorioId);
            oRE_ACTOMIGRATORIO.ACTO.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            oRE_ACTOMIGRATORIO.ACTO.acmi_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
            oRE_ACTOMIGRATORIO.ACTO.acmi_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            oRE_ACTOMIGRATORIO.ACTO.acmi_dFechaModificacion = DateTime.Now;
            oRE_ACTOMIGRATORIO.ACTO.HostName = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_HOSTNAME]);
            oRE_ACTOMIGRATORIO.HISTORICO.Add(oRE_ACTOMIGRATORIOHISTORICO);


            i_Resultado = new SGAC.Registro.Actuacion.BL.ActoMigratorioMantenimientoBL().Actualizar_Estados(oRE_ACTOMIGRATORIO);


            if (i_Resultado > 0)
            {
                StrScript = "1";
            }
            else
            {
                StrScript = "0";
            }

            return StrScript;
        }
    }
}