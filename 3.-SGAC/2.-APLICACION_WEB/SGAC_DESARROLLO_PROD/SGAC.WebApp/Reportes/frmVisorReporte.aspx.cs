using System;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace SGAC.WebApp.Reportes
{
    public partial class frmVisorReporte : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strREPORTE = Convert.ToString(Request.QueryString["REP"]);
                if (strREPORTE == "" || strREPORTE == null)
                {
                    DataTable xDt = new DataTable();
                    ReportParameter[] parameters;

                    string StrNombreArchReporte = string.Empty;

                    parameters = (ReportParameter[])Session["objParametroReportes"];

                    StrNombreArchReporte = (string)Session["strNombreArchivo"];

                    xDt = (DataTable)Session["DtDatos"];

                    string strRutaBase;
                    strRutaBase = (string)Session["strNombreArchivo"];
                    string strDataSet;
                    strDataSet = Session["DataSet"].ToString();
                    rpsProtocolar.LocalReport.ReportEmbeddedResource = strRutaBase;
                    rpsProtocolar.LocalReport.ReportPath = strRutaBase;

                    rpsProtocolar.LocalReport.DataSources.Clear();
                    ReportDataSource datasource = new ReportDataSource(strDataSet, xDt);
                    if (parameters != null)
                    { rpsProtocolar.LocalReport.SetParameters(parameters); }
                    rpsProtocolar.LocalReport.DataSources.Add(datasource);
                }
                else
                {
                    DataTable xDt = new DataTable();
                    DataTable xDt2 = new DataTable();
                    ReportParameter[] parameters;

                    string StrNombreArchReporte = string.Empty;

                    parameters = (ReportParameter[])Session["objParametroReportes"];

                    StrNombreArchReporte = (string)Session["strNombreArchivo"];

                    xDt = (DataTable)Session["DtDatos"];
                    xDt2 = (DataTable)Session["DtDatos1"];

                    string strRutaBase;
                    strRutaBase = (string)Session["strNombreArchivo"];
                    string strDataSet, strDataSet2;
                    strDataSet = Session["DataSet"].ToString();
                    strDataSet2 = Session["DataSet2"].ToString();
                    rpsProtocolar.LocalReport.ReportEmbeddedResource = strRutaBase;
                    rpsProtocolar.LocalReport.ReportPath = strRutaBase;

                    rpsProtocolar.LocalReport.DataSources.Clear();
                    ReportDataSource datasource = new ReportDataSource(strDataSet, xDt);
                    ReportDataSource datasource2 = new ReportDataSource(strDataSet2, xDt2);
                    rpsProtocolar.LocalReport.SetParameters(parameters);
                    rpsProtocolar.LocalReport.DataSources.Add(datasource);
                    rpsProtocolar.LocalReport.DataSources.Add(datasource2);
                }
                //------------------------------------
                Session.Remove("DtDatos");
                Session.Remove("DtDatos1");
            }
        }
    }
}