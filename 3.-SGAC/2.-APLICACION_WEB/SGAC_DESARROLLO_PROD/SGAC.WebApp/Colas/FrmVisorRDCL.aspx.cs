using System;
using System.Data;
using Microsoft.Reporting.WebForms;
using SGAC.Cliente.Colas.BL;
using SGAC.Accesorios;
using SGAC.Controlador;
using SGAC.Registro.Persona.BL;

namespace SGAC.WebApp.Colas
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PersonaAsistenciaConsultaBL FunAsistencia = new PersonaAsistenciaConsultaBL();
                ServicioConsultaBL xMiFun = new ServicioConsultaBL();
                DataTable xDt = new DataTable();
                ReportParameter[] parameters;

                string StrNombreArchReporte = string.Empty;
                object[] arrParametros;

                arrParametros = (object[])Session["objParametroClase"];
                parameters = (ReportParameter[])Session["objParametroReportes"];
                StrNombreArchReporte = (string)Session["strNombreArchivo"];

                xDt = (DataTable)Session["DtDatos"];
                xDt.TableName = "ASISTENCIA";

                dsReport.LocalReport.ReportEmbeddedResource = "Reportes\\Rs1\\" + StrNombreArchReporte;
                dsReport.LocalReport.ReportPath = @"Reportes\\Rs1\\" + StrNombreArchReporte;

                dsReport.LocalReport.DataSources.Clear();
                ReportDataSource datasource = new ReportDataSource("DataSet1", xDt);
                dsReport.LocalReport.SetParameters(parameters);
                dsReport.LocalReport.DataSources.Add(datasource);
                //------------------------------------
                Session.Remove("DtDatos");
            }
        }
    }
}