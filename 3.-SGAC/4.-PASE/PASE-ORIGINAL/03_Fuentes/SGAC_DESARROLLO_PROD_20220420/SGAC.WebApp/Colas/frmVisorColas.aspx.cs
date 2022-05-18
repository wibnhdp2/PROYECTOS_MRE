using System;
using System.Configuration;
using System.Data;
using Microsoft.Reporting.WebForms;
using SGAC.WebApp.Accesorios;

namespace SGAC.WebApp.Colas
{
    public partial class frmVisorColas : MyBasePage
    {
        public string SSRS = ConfigurationManager.AppSettings["SSRS"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Imprimir();
            }
        }

        private void Imprimir()
        {
            DataTable xDt = new DataTable();
            ReportParameter[] parameters;
            string StrNombreArchReporte = string.Empty;

            parameters = (ReportParameter[])Session["objParametroReportes"];
            StrNombreArchReporte = (string)Session["strNombreArchivo"];

            xDt = (DataTable)Session["DtDatos"];
            xDt.TableName = "Tabla";

            dsReport.LocalReport.ReportEmbeddedResource = "Reportes\\RSColas\\" + StrNombreArchReporte;
            dsReport.LocalReport.ReportPath = @"Reportes\\RSColas\\" + StrNombreArchReporte;

            dsReport.LocalReport.DataSources.Clear();
            ReportDataSource datasource = new ReportDataSource("DsColas", xDt);
            dsReport.LocalReport.SetParameters(parameters);
            dsReport.LocalReport.DataSources.Add(datasource);

            //----------------------------------------
            Session.Remove("DtDatos");
        }
    }
}