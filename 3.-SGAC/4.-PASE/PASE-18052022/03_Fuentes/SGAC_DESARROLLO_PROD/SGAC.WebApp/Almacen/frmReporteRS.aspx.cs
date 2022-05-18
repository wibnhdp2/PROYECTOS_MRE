using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using SGAC.Accesorios;

namespace SGAC.WebApp.Almacen
{
    public partial class frmReporteRS : System.Web.UI.Page
    {

        #region CAMPOS

        ReportParameter[] parameters;
        String sNombreDsReporteServices = String.Empty;
        String strRutaBase = String.Empty;

        DataTable dtReporte = null;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarReporte();
        }

        private void CargarReporte()
        {
            dtReporte = Session["dtDatos"] as DataTable;

            string sTitulo = string.Empty;
            string sReporteDS = string.Empty;
            string sReporteRS = string.Empty;

            if (Convert.ToInt32(Session["AlmacenReporteId"]) == (int)Enumerador.enmAlmacenReporte.KARDEX_DE_INSUMOS)
            {
                sTitulo = "KARDEX DE INSUMO";
                sReporteDS = "dsInsumos";
                sReporteRS = "rsInsumos.rdlc";
            }
            else if(Convert.ToInt32(Session["AlmacenReporteId"]) == (int)Enumerador.enmAlmacenReporte.KARDEX_DE_INSUMOS_DETALLADO)
            {
                sTitulo = "KARDEX DE INSUMO DETALLADO";
                sReporteDS = "dsInsumosDetallado";
                sReporteRS = "rsInsumosDetallado.rdlc";
            }
            else if (Convert.ToInt32(Session["AlmacenReporteId"]) == (int)Enumerador.enmAlmacenReporte.INSUMOS_REMITIDOS)
            {
                sTitulo = "INSUMOS REMITIDOS";
                sReporteDS = "dsInsumosRemitidos";
                sReporteRS = "rsInsumosRemitidos.rdlc";
            }

            Session["AlmacenReporteId"] = null;

            parameters = new ReportParameter[2];

            parameters[0] = new ReportParameter("Titulo1", sTitulo);
            parameters[1] = new ReportParameter("Usuario", Session[Constantes.CONST_SESION_USUARIO_ID].ToString());


            strRutaBase = Server.MapPath("~/Almacen/" + sReporteRS);


            ReportViewer1.LocalReport.ReportEmbeddedResource = strRutaBase;
            ReportViewer1.LocalReport.ReportPath = strRutaBase;

            ReportViewer1.LocalReport.DataSources.Clear();

            ReportDataSource datasource = new ReportDataSource(sReporteDS, dtReporte);

            ReportViewer1.LocalReport.SetParameters(parameters);
            ReportViewer1.LocalReport.DataSources.Add(datasource);

            //------------------------------------
            Session.Remove("AlmacenReporteId");
            Session.Remove("dtDatos");
        }
    }
}