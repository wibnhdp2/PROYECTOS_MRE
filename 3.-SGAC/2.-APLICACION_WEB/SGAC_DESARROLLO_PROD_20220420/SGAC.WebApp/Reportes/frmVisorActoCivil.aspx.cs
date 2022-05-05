using System;
using System.Data;
using SGAC.Controlador;
using SGAC.WebApp.Accesorios;
using Microsoft.Reporting.WebForms;
using SGAC.Accesorios;

namespace SGAC.WebApp.Reportes
{
    public partial class frmVisorActoCivil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Definir variables
                Int64 reci_iRegistroCivilId = 9; //Temporal para prueba
                Int64 reci_iActuacionDetalleId = 9; //Temporal para prueba
                String sTipoActoCivil = "Nacimiento"; //1.Nacimiento, 2.Nacimiento, 3.Nacimiento, 4.Nacimiento
                String sFormularioActoCivil = ""; //Variable para cargar el formulario requerido
                object[] arrParametros = new object[1]; //Variable para parametros del StoredProcedure
                Proceso p = new Proceso(); //Variable del StoredProcedure
                try
                {
                    if (sTipoActoCivil == "Nacimiento")
                    {
                        sFormularioActoCivil = "~/Reportes/rsActoCivilNacimiento.rdlc";
                    }
                    else if (sTipoActoCivil == "Matrimonio")
                    {
                        sFormularioActoCivil = "~/Reportes/rsActoCivilMatrimonio.rdlc";
                    }
                    else if (sTipoActoCivil == "Defuncion")
                    {
                        sFormularioActoCivil = "~/Reportes/rsActoCivilDefuncion.rdlc";
                    }
                    else if (sTipoActoCivil == "CopiaCertificada")
                    {
                        sFormularioActoCivil = "~/Reportes/rsActoCivilCopiaCertificada.rdlc";
                    }
                    
                    //Carga DataTable desde el StoredProcedure
                    DataTable dt = new Reportes.dsActoCivil().Tables[sTipoActoCivil];
                    arrParametros[0] = reci_iRegistroCivilId;
                    arrParametros[1] = reci_iActuacionDetalleId;
                    dt = (DataTable)p.Invocar(ref arrParametros, "PN_REGISTRO.USP_RE_REGISTROCIVIL_FORMATO", "CONSULTAR_HOJA_INSCRIPCION");

                    Microsoft.Reporting.WebForms.ReportViewer viewer = new Microsoft.Reporting.WebForms.ReportViewer();
                    Microsoft.Reporting.WebForms.ReportDataSource rptDataSource = new Microsoft.Reporting.WebForms.ReportDataSource("dsActoCivil", dt);
                    
                    // Copia variables de session para ser usadas dentro del reporte de Copia Certificada (Nombre Mision, Ciudad).
                    if (sTipoActoCivil == "CopiaCertificada")
                    {
                        ReportParameter strNombreMision = new ReportParameter("strNombreMision", comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ofco_vNombre").ToString());
                        ReportParameter strCiudadMision = new ReportParameter("strCiudadMision", comun_Part2.ObtenerDatoOficinaConsular(Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), "ubge_vDistrito").ToString());
                        viewer.LocalReport.SetParameters(new ReportParameter[] { strNombreMision });
                        viewer.LocalReport.SetParameters(new ReportParameter[] { strCiudadMision });
                    }
                    viewer.LocalReport.DataSources.Clear();
                    viewer.LocalReport.DataSources.Add(rptDataSource);
                    viewer.LocalReport.ReportEmbeddedResource = Server.MapPath(sFormularioActoCivil);
                    viewer.LocalReport.ReportPath = Server.MapPath(sFormularioActoCivil);

                    //Export to PDF
                    string mimeType;
                    string encoding;
                    string fileNameExtension;
                    string[] streams;
                    Microsoft.Reporting.WebForms.Warning[] warnings;

                    byte[] pdfContent = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

                    //Return PDF
                    this.Response.Clear();
                    this.Response.ContentType = "application/pdf";
                    this.Response.AddHeader("Content-disposition", "attachment; filename=" + sTipoActoCivil + ".pdf");
                    this.Response.BinaryWrite(pdfContent);
                    this.Response.End();
                }
                catch (Exception ex)
                {

                }

            }
        }
    }
}