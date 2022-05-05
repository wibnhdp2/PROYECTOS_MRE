using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace SGAC.WebApp.Registro.Preview
{
    public partial class FrmPreview_ActoMilitar_Constancia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new Reportes.dsActoMilitar().Tables["RM_CONSTANCIA"];

                //var row = dt.NewRow();
                //row["NroDocumento"] = "43971780";

                //dt.Rows.Add(row);

                ReportViewer1.LocalReport.ReportEmbeddedResource = Server.MapPath("~/Reportes/rsActoMilitarConstancia.rdlc");
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reportes/rsActoMilitarConstancia.rdlc");

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource datasource = new ReportDataSource("dsActoMilitar", dt);
                ReportViewer1.LocalReport.DataSources.Add(datasource);
                ReportViewer1.DataBind();

            }
        }
    }
}