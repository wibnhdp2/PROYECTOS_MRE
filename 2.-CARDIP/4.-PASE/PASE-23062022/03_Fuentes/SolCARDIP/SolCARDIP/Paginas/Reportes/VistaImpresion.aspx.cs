using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;
using SolCARDIP.Librerias.EntidadesNegocio;
using SolCARDIP.Librerias.ReglasNegocio;
using Seguridad.Logica.BussinessEntity;

namespace SolCARDIP.Paginas.Reportes
{
    public partial class VistaImpresion : System.Web.UI.Page
    {
        brGeneral obrGeneral = new brGeneral();
        CodigoUsuario oCodigoUsuario = new CodigoUsuario();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    List<DataTable> listaTablas = (List<DataTable>)Session["DataSetListaTablas"];
                    if (listaTablas != null)
                    {
                        string identRep = "";
                        string sReporte = "";
                        object[] arrParametros;
                        ReportDataSource rds1, rds2;
                        ReportParameter[] parametros;
                        if (Request.QueryString["identRep"] != null)
                        {
                            identRep = Request.QueryString["identRep"];
                        }
                        switch (identRep)
                        {
                            case "001":
                                
                                sReporte = (string)Session["reporteRuta"];
                                arrParametros = (object[])Session["ParametrosReporte"];
                                parametros = new ReportParameter[2];
                                parametros[0] = new ReportParameter("FechaInicio", arrParametros[0].ToString(), false);
                                parametros[1] = new ReportParameter("FechaFin", arrParametros[1].ToString(), false);

                                ReportViewer1.Reset();
                                ReportViewer1.LocalReport.ReportPath = Server.MapPath(sReporte);
                                ReportViewer1.LocalReport.DataSources.Clear();
                                ReportViewer1.ShowPrintButton = true;

                                rds1 = new ReportDataSource("ResumenxCalidad", listaTablas[0]);
                                rds2 = new ReportDataSource("Totales", listaTablas[1]);
                                ReportViewer1.LocalReport.DataSources.Add(rds1);
                                ReportViewer1.LocalReport.DataSources.Add(rds2);
                                ReportViewer1.LocalReport.SetParameters(parametros);
                                break;
                            case "002":
                                sReporte = (string)Session["reporteRuta"];
                                arrParametros = (object[])Session["ParametrosReporte"];
                                parametros = new ReportParameter[6];
                                parametros[0] = new ReportParameter("FechaInicio", arrParametros[0].ToString(), false);
                                parametros[1] = new ReportParameter("FechaFin", arrParametros[1].ToString(), false);
                                parametros[2] = new ReportParameter("CalMig", arrParametros[2].ToString(), false);
                                parametros[3] = new ReportParameter("Estado", arrParametros[3].ToString(), false);
                                parametros[4] = new ReportParameter("Institucion", arrParametros[4].ToString(), false);
                                parametros[5] = new ReportParameter("TotalReg", arrParametros[5].ToString(), false);

                                ReportViewer1.Reset();
                                ReportViewer1.LocalReport.ReportPath = Server.MapPath(sReporte);
                                ReportViewer1.LocalReport.DataSources.Clear();
                                ReportViewer1.ShowPrintButton = true;

                                rds1 = new ReportDataSource("DetallexCalidad", listaTablas[0]);
                                ReportViewer1.LocalReport.DataSources.Add(rds1);
                                ReportViewer1.LocalReport.SetParameters(parametros);
                                break;
                            case "003":
                                sReporte = (string)Session["reporteRuta"];
                                arrParametros = (object[])Session["ParametrosReporte"];
                               /* parametros = new ReportParameter[6];
                                parametros[0] = new ReportParameter("FechaInicio", arrParametros[0].ToString(), false);
                                parametros[1] = new ReportParameter("FechaFin", arrParametros[1].ToString(), false);
                                parametros[2] = new ReportParameter("CalMig", arrParametros[2].ToString(), false);
                                parametros[3] = new ReportParameter("Estado", arrParametros[3].ToString(), false);
                                parametros[4] = new ReportParameter("Institucion", arrParametros[4].ToString(), false);
                                parametros[5] = new ReportParameter("TotalReg", arrParametros[5].ToString(), false);
                                */
                                ReportViewer1.Reset();
                                ReportViewer1.LocalReport.ReportPath = Server.MapPath(sReporte);
                                ReportViewer1.LocalReport.DataSources.Clear();
                                ReportViewer1.ShowPrintButton = true;

                                rds1 = new ReportDataSource("DataSet1", listaTablas[0]);
                                ReportViewer1.LocalReport.DataSources.Add(rds1);
                                //ReportViewer1.LocalReport.SetParameters(parametros);
                                break;
                        }
                        ReportViewer1.DataBind();
                        ReportViewer1.LocalReport.Refresh();
                    }
                }
                else
                {

                }
            }
            catch(Exception ex)
            {
                obrGeneral.grabarLog(ex);
            }
        }
    }
}