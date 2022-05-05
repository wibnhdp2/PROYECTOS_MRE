using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using SGAC.Accesorios;
using SGAC.Controlador;
using System.Data;
using SGAC.WebApp.Accesorios;

namespace SGAC.WebApp.Registro
{
    public partial class FrmReporteRune : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //-------------------------------------------------------------------------
            //Autor: Miguel Márquez Beltrán
            //Fecha: 30/09/2016
            //Objetivo: Crear una sesion para permitir actualizar el formato del RUNE
            //          por única vez cada vez que se imprime el formato.
            //-------------------------------------------------------------------------

            //if (!IsPostBack)
            //{
            if (Session["printRUNE"].Equals("1"))
            {
                dsReport.ShowPrintButton = true;
                VerVistaPreviaPorReporte();
                Session["printRUNE"] = "";
            }
            //}
        }

        
        private void VerVistaPreviaPorReporte()
        {
            Enumerador.enmRegistroReporte enmReporte;
            enmReporte = (Enumerador.enmRegistroReporte)Session["REGISTRO_RPT"];

            switch (enmReporte)
            {
                case Enumerador.enmRegistroReporte.RUNE:
                    Imprimir_PDF();
                    break;
                case Enumerador.enmRegistroReporte.ANOTACION:
                    VerAnotacion();
                    break;
                case Enumerador.enmRegistroReporte.FILIACION:
                    ImprimirFiliacion_PDF();
                    break;
                default:
                    break;
            }
        }


        private void ImprimirFiliacion_PDF()
        {
            DataTable dt_per = new DataTable();
            DataTable dt_dni = new DataTable();

            DataSet ds = new DataSet();
            
            string StrNombreArchReporte = string.Empty;
            ds = (DataSet)Session["DtDatos"];
            StrNombreArchReporte = (string)Session["strNombreArchivo"];

            ReportParameter[] param = new ReportParameter[0];

            if (StrNombreArchReporte == "crConstanciaInscripcion_Idioma_CAST.rdlc")
            {
                param = new ReportParameter[2];
                param[0] = new ReportParameter("SolesConsulares", Session["SolesConsulares"].ToString());
                param[1] = new ReportParameter("MontoLocal", Session["MontoLocal"].ToString());

            }
            else
            {
                #region Si_el_Idioma_es_diferente_al_Castellano
                #region LeerTipoPlantilla
                DataTable dtPlantilla = new DataTable();
                dtPlantilla = comun_Part1.ObtenerParametrosPorGrupo(Session, "ACTUACIÓN-TIPO PLANTILLA");
                Int16 intTipoPlantillaConstanciaInscripcionId = 0;

                for (int i = 0; i < dtPlantilla.Rows.Count; i++)
                {
                    if (dtPlantilla.Rows[i]["descripcion"].ToString().Trim().ToUpper().Equals("CONSTANCIA DE INSCRIPCIÓN"))
                    {
                        intTipoPlantillaConstanciaInscripcionId = Convert.ToInt16(dtPlantilla.Rows[i]["id"].ToString());
                        break;
                    }
                }
                #endregion

                #region LeerTraduccion

                if (intTipoPlantillaConstanciaInscripcionId > 0)
                {
                    DataTable dtTraduccion = new DataTable();
                    SGAC.Configuracion.Sistema.BL.PlantillaTraduccionConsultasBL BL = new SGAC.Configuracion.Sistema.BL.PlantillaTraduccionConsultasBL();
                    int IntTotalCount = 0;
                    int IntTotalPages = 0;

                    Int16 intIdiomaId = Convert.ToInt16(Session[Constantes.CONST_SESION_IDIOMA_ID].ToString());
                    string strEtiqueta = "";
                    string strTraduccion = "";

                    if (intIdiomaId > 0)
                    {
                        dtTraduccion = BL.Consultar(0, intTipoPlantillaConstanciaInscripcionId, intIdiomaId, 0, "A", "1", 1000, "N", ref IntTotalCount, ref IntTotalPages);

                        if (dtTraduccion.Rows.Count > 0)
                        {
                            int intTotalIndices = 52;
                            param = new ReportParameter[intTotalIndices];
                            param[0] = new ReportParameter("SolesConsulares", Session["SolesConsulares"].ToString());
                            param[1] = new ReportParameter("MontoLocal", Session["MontoLocal"].ToString());

                            for (int i = 2; i < intTotalIndices; i++)
                            {
                                strEtiqueta = dtTraduccion.Rows[i - 2]["etiq_vEtiqueta"].ToString().Trim();
                                strTraduccion = dtTraduccion.Rows[i - 2]["pltr_vTraduccion"].ToString().Trim();
                                param[i] = new ReportParameter(strEtiqueta, strTraduccion);

                            }
                        }
                    }
                }
                #endregion
                #endregion
            }

            dt_per = ds.Tables[0];  //persona
            dt_dni = ds.Tables[1];  //documento
            
            String strRutaBase = Server.MapPath("~/Registro/" + StrNombreArchReporte);

            dsReport.LocalReport.ReportEmbeddedResource = strRutaBase;
            dsReport.LocalReport.ReportPath = strRutaBase;

            dsReport.LocalReport.DataSources.Clear();
            ReportDataSource datasourceRune = new ReportDataSource("Rune", dt_per);
            ReportDataSource datasourceDocumento = new ReportDataSource("Documento", dt_dni);
            

            
            dsReport.LocalReport.EnableExternalImages = true;

            dsReport.LocalReport.SetParameters(param);
            dsReport.LocalReport.DataSources.Add(datasourceRune);
            dsReport.LocalReport.DataSources.Add(datasourceDocumento);

            Session.Remove("DtDatos");
        }

        #region RUNE
        private void Imprimir_PDF()
        {
            DataTable dt_per = new DataTable();
            DataTable dt_dni = new DataTable();
            DataTable dt_fili = new DataTable();
            DataTable dt_filiT = new DataTable();

            DataSet ds = new DataSet();
            //ReportParameter[] parameters;
            //String ruta = "file:D:/Lighthouse.jpg";
            //parameters = new ReportParameter[1];
            //parameters[0] = new ReportParameter("ruta_img", ruta);
            string StrNombreArchReporte = string.Empty;
            ds = (DataSet)Session["DtDatos"];
            StrNombreArchReporte = (string)Session["strNombreArchivo"];

            dt_per = ds.Tables[0];  //persona
            dt_dni = ds.Tables[1];  //documento
            dt_fili = ds.Tables[2]; //filiacion hijo
            dt_filiT = ds.Tables[3]; //filiacion hijo

            String strRutaBase = Server.MapPath("~/Registro/" + StrNombreArchReporte);


            //ReportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local
            // ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;


            dsReport.LocalReport.ReportEmbeddedResource = strRutaBase;
            dsReport.LocalReport.ReportPath = strRutaBase;

            dsReport.LocalReport.DataSources.Clear();
            ReportDataSource datasourceRune = new ReportDataSource("Rune", dt_per);
            ReportDataSource datasourceDocumento = new ReportDataSource("Documento", dt_dni);
            ReportDataSource Filiacion = new ReportDataSource("Filiacion", dt_fili);
            ReportDataSource FiliacionTodos = new ReportDataSource("FilacionTodos", dt_filiT);

            //    ReportViewer1.LocalReport.EnableHyperlinks = true;
            dsReport.LocalReport.EnableExternalImages = true;
            // ReportViewer1.LocalReport.SetParameters(parameters);

            dsReport.LocalReport.DataSources.Add(datasourceRune);
            dsReport.LocalReport.DataSources.Add(datasourceDocumento);
            dsReport.LocalReport.DataSources.Add(Filiacion);
            dsReport.LocalReport.DataSources.Add(FiliacionTodos);

            Session.Remove("DtDatos");
        }
       
        #endregion

        #region Anotación

        private void VerAnotacion()
        {

            DataTable dt = new DataTable("Anotacion");

            dt.Columns.Add("Prueba", typeof(Int32));

            dt.Rows.Add("1");

            ReportParameter[] param = new ReportParameter[1];
            param[0] = new ReportParameter("Descripcion", Session["ANOTACION_DESC"].ToString());

            String strRutaBase = Server.MapPath("~/Registro/rsAnotacion.rdlc");
            dsReport.LocalReport.ReportEmbeddedResource = strRutaBase;
            dsReport.LocalReport.ReportPath = strRutaBase;


        
            Session.Remove("ANOTACION_DESC");
            dsReport.LocalReport.DataSources.Clear();
            ReportDataSource rdsAnotacion = new ReportDataSource("Anotacion",dt);

            dsReport.LocalReport.SetParameters(param);
            dsReport.LocalReport.DataSources.Add(rdsAnotacion);
            
        }
        #endregion

 
    }
}