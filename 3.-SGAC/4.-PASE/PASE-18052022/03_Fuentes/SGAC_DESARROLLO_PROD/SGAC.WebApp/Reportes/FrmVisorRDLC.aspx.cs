using System;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace SGAC.WebApp.Reportes
{
    public partial class FrmVisorRDLC : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable xDt = new DataTable();
                ReportParameter[] parameters;

                string StrNombreArchReporte = string.Empty;

                parameters = (ReportParameter[])Session["objParametroReportes"];

                StrNombreArchReporte = (string)Session["strNombreArchivo"];

                xDt = (DataTable)Session["DtDatos"];

                string strRutaBase;
                strRutaBase = Server.MapPath("~/Reportes/rsActoJudicialConsulta2.rdlc");

                rpsProtocolar.LocalReport.ReportEmbeddedResource = strRutaBase;
                rpsProtocolar.LocalReport.ReportPath = strRutaBase;
                                     
                rpsProtocolar.LocalReport.DataSources.Clear();
                ReportDataSource datasource = new ReportDataSource("Expediente", xDt);
                rpsProtocolar.LocalReport.SetParameters(parameters);
                rpsProtocolar.LocalReport.DataSources.Add(datasource);

                //------------------------------------------
                Session.Remove("DtDatos");
            }
        }
    }
}