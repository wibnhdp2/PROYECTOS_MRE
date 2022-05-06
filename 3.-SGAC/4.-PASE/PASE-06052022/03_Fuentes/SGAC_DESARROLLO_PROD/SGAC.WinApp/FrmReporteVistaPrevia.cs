using System;
using System.Data;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.Reporting.WinForms;

namespace SGAC.WinApp
{
    public partial class FrmReporteVistaPrevia : Form
    {
        public int IntAnchoFormulario = 0;
        public int IntAltoFormulario = 0;
        public string StrTituloReporte;
        public string StrRutaReporte;
        public ParameterFields ParParametroReportes;
        public ReportParameter[] parameters;
        public DataSet DsDatos = new DataSet();
        public DataTable dt1;
        public DataTable dt2;
        public string FechaFiltro;
        public FrmReporteVistaPrevia()
        {
            InitializeComponent();            
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {
            if (IntAnchoFormulario != 0) { this.Width = IntAnchoFormulario; }
            if (IntAltoFormulario != 0) { this.Height = IntAltoFormulario; }

            this.Text = StrTituloReporte;

            reportViewer1.LocalReport.ReportEmbeddedResource = StrRutaReporte;
            reportViewer1.LocalReport.ReportPath = StrRutaReporte;
            reportViewer1.LocalReport.DataSources.Clear();

            switch (StrTituloReporte)
            {
                case "SISTEMA DE COLAS - EMISION DE TICKET":
                    ReportTicket();
                    break;
                case "Sistema de Colas - Detalle de Ticket Emitidos x Dia":
                    ReportResumenTicket();
                    break;
                case "Sistema de Colas - Resumen de Ticket Emitidos x Dia":

                    ReportDetalleTicket();
                    break;
            }

            //ReportDataSource datasource = new ReportDataSource("TicketData",dt);

            //ReportParameter[] para = new ReportParameter[parameters.Length];
            //para = parameters;

            //parameters[0] = new ReportParameter("TituloReporte", "REPORTE DE REMESAS CONSULARES");
            //parameters[1] = new ReportParameter("SubTituloReporte", Constantes.CONST_REPORTE_SUB_TITULO);
            //parameters[2] = new ReportParameter("NombreOficina", sNombreOficinaConsular);
            //parameters[3] = new ReportParameter("UsuarioImpresion", Session[Constantes.CONST_SESION_USUARIO].ToString());
            //parameters[4] = new ReportParameter("FechaHaber", Session["FechaIntervalo"].ToString());

            //reportViewer1.LocalReport.SetParameters(para);

            //reportViewer1.LocalReport.DataSources.Add(datasource);

            //ReportDocument MiRpt = new ReportDocument();

            //MiRpt.Load(StrRutaReporte);                       // CARGAMOS EL REPORTE
            //MiRpt.SetDataSource(DsDatos.Copy());              // CARGAMOS LOS DATOS
            //crystalReportViewer1.ParameterFieldInfo = ParParametroReportes;
            //crystalReportViewer1.ReportSource = MiRpt;  

            //this.reportViewer1.RefreshReport();

        }

        private void ReportTicket()
        {
            try
            {
                ReportParameter[] para = new ReportParameter[parameters.Length];
                para = parameters;
                reportViewer1.LocalReport.SetParameters(para);
                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private void ReportDetalleTicket()
        {
            try
            {
                dt1 = new DataTable();
                dt1 = DsDatos.Tables[1].Copy();
                dt2 = new DataTable();
                dt2 = DsDatos.Tables[2].Copy();


                //ds.Tables.Add(dt2);
                dt1.Columns.Add("serv_vDescripcion");

                foreach (System.Data.DataRow dr in dt2.Rows)
                {
                    foreach (System.Data.DataRow dr2 in dt1.Rows)
                    {
                        if (Convert.ToInt32(dr2["tick_sTipoServicioId"]) == Convert.ToInt32(dr["serv_sServicioId"]))
                            dr2["serv_vDescripcion"] = dr["serv_vDescripcion"];
                    }
                }

                DataSet dsss = new DataSet1();
                foreach (DataRow dr in dt1.Rows)
                {
                    DataRow drs = dsss.Tables[1].NewRow();
                    drs["tick_iTicketId"] = dr["tick_iTicketId"];
                    drs["tick_iOficinaConsularId"] = dr["tick_iOficinaConsularId"];
                    drs["tick_sTipoServicioId"] = dr["tick_sTipoServicioId"];

                    if (dr["tick_iPersonalId"] == null || dr["tick_iPersonalId"].ToString() == "")
                        drs["tick_iPersonalId"] = DBNull.Value;
                    else
                        drs["tick_iPersonalId"] = dr["tick_iPersonalId"];                    
                    
                    drs["tick_iNumero"] = dr["tick_iNumero"];

                    //--------------------------------------------------------------------------
                    //Autor: Miguel
                    //Descripción: Se incluyo la validación de la fecha / hora de generación.
                    //--------------------------------------------------------------------------

                    if (dr["tick_dFechaHoraGeneracion"] == null || dr["tick_dFechaHoraGeneracion"].ToString() == "")
                    {
                        drs["tick_dFechaHoraGeneracion"] = DBNull.Value;
                    }
                    else
                    {
                        string strFecha = dr["tick_dFechaHoraGeneracion"].ToString();
                        drs["tick_dFechaHoraGeneracion"] = new DateTime
                        (
                        Convert.ToInt16(strFecha.Substring(0, 4)),
                        Convert.ToInt16(strFecha.Substring(4, 2)),
                        Convert.ToInt16(strFecha.Substring(6, 2)),
                        Convert.ToInt16(strFecha.Substring(9, 2)),
                        Convert.ToInt16(strFecha.Substring(11, 2)),
                        0
                        );
                    }
                    if (dr["tick_dAtencionInicio"] == null || dr["tick_dAtencionInicio"].ToString() == "")
                        drs["tick_dAtencionInicio"] = DBNull.Value;                    
                    else
                        drs["tick_dAtencionInicio"] = dr["tick_dAtencionInicio"];

                    if (dr["tick_dAtencionFinal"] == null || dr["tick_dAtencionFinal"].ToString() == "")
                        drs["tick_dAtencionFinal"] = DBNull.Value;                    
                    else
                        drs["tick_dAtencionFinal"] = dr["tick_dAtencionFinal"];

                    drs["tick_sPrioridadId"] = dr["tick_sPrioridadId"];
                    drs["tick_sTipoCliente"] = dr["tick_sTipoCliente"];
                    drs["tick_sTamanoTicket"] = dr["tick_sTamanoTicket"];
                    drs["tick_sTipoEstado"] = dr["tick_sTipoEstado"];
                    drs["tick_sTicketeraId"] = dr["tick_sTicketeraId"];
                    drs["tick_vLlamada"] = dr["tick_vLlamada"];
                    drs["tick_sUsuarioAtendio"] = dr["tick_sUsuarioAtendio"];
                    drs["tick_cEstado"] = dr["tick_cEstado"];
                    drs["tick_sUsuarioCreacion"] = dr["tick_sUsuarioCreacion"];
                    drs["tick_vIPCreacion"] = dr["tick_vIPCreacion"];
                    drs["tick_dFechaCreacion"] = dr["tick_dFechaCreacion"];
                    drs["tick_sUsuarioModificacion"] = dr["tick_sUsuarioModificacion"];
                    drs["tick_vIPModificacion"] = dr["tick_vIPModificacion"];
                    drs["tick_dFechaModificacion"] = dr["tick_dFechaModificacion"];
                    drs["serv_vDescripcion"] = dr["serv_vDescripcion"];

                    dsss.Tables[1].Rows.Add(drs);
                }

                DataTable dt_filtro = null;
                try
                {
                    dt_filtro = (from dt_1 in dsss.Tables[1].AsEnumerable()
                                 where Convert.ToDateTime(dt_1["tick_dFechaHoraGeneracion"]).ToShortDateString() == Convert.ToDateTime(FechaFiltro).ToShortDateString()
                                     select dt_1).CopyToDataTable();
                }
                catch
                {
                    dt_filtro = new DataTable();
                }

                ReportDataSource datasource1 = new ReportDataSource("DataSet1", dt_filtro);


                ReportParameter[] para = new ReportParameter[parameters.Length];
                para = parameters;
                reportViewer1.LocalReport.DataSources.Add(datasource1);
                reportViewer1.LocalReport.SetParameters(para);
                this.reportViewer1.RefreshReport();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        private void ReportResumenTicket()
        {
            try
            {
                dt1 = new DataTable();
                dt1 = DsDatos.Tables[1].Copy();
                dt2 = new DataTable();
                dt2 = DsDatos.Tables[2].Copy();


                //ds.Tables.Add(dt2);
                dt1.Columns.Add("serv_vDescripcion");

                foreach (System.Data.DataRow dr in dt2.Rows)
                {
                    foreach (System.Data.DataRow dr2 in dt1.Rows)
                    {
                        if (Convert.ToInt32(dr2["tick_sTipoServicioId"]) == Convert.ToInt32(dr["serv_sServicioId"]))
                            dr2["serv_vDescripcion"] = dr["serv_vDescripcion"];
                    }
                }

                DataSet dsss = new DataSet1();
                foreach (DataRow dr in dt1.Rows)
                {
                    DataRow drs = dsss.Tables[1].NewRow();
                    drs["tick_iTicketId"] = dr["tick_iTicketId"];
                    drs["tick_iOficinaConsularId"] = dr["tick_iOficinaConsularId"];
                    drs["tick_sTipoServicioId"] = dr["tick_sTipoServicioId"];

                    if (dr["tick_iPersonalId"] == null || dr["tick_iPersonalId"].ToString() == "")
                        drs["tick_iPersonalId"] = DBNull.Value;
                    else
                        drs["tick_iPersonalId"] = dr["tick_iPersonalId"];

                    drs["tick_iNumero"] = dr["tick_iNumero"];

                    //--------------------------------------------------------------------------
                    //Autor: Miguel
                    //Descripción: Se incluyo la validación de la fecha / hora de generación.
                    //--------------------------------------------------------------------------
                    if (dr["tick_dFechaHoraGeneracion"] == null || dr["tick_dFechaHoraGeneracion"].ToString() == "")
                    {
                        drs["tick_dFechaHoraGeneracion"] = DBNull.Value;
                    }
                    else
                    {
                        string strFecha = dr["tick_dFechaHoraGeneracion"].ToString();
                        drs["tick_dFechaHoraGeneracion"] = new DateTime
                        (
                        Convert.ToInt16(strFecha.Substring(0, 4)),
                        Convert.ToInt16(strFecha.Substring(4, 2)),
                        Convert.ToInt16(strFecha.Substring(6, 2)),
                        Convert.ToInt16(strFecha.Substring(9, 2)),
                        Convert.ToInt16(strFecha.Substring(11, 2)),
                        0
                        );
                    }
                    if (dr["tick_dAtencionInicio"] == null || dr["tick_dAtencionInicio"].ToString() == "")
                        drs["tick_dAtencionInicio"] = DBNull.Value;
                    else
                        drs["tick_dAtencionInicio"] = dr["tick_dAtencionInicio"];

                    if (dr["tick_dAtencionFinal"] == null || dr["tick_dAtencionFinal"].ToString() == "")
                        drs["tick_dAtencionFinal"] = DBNull.Value;
                    else
                        drs["tick_dAtencionFinal"] = dr["tick_dAtencionFinal"];

                    drs["tick_sPrioridadId"] = dr["tick_sPrioridadId"];
                    drs["tick_sTipoCliente"] = dr["tick_sTipoCliente"];
                    drs["tick_sTamanoTicket"] = dr["tick_sTamanoTicket"];
                    drs["tick_sTipoEstado"] = dr["tick_sTipoEstado"];
                    drs["tick_sTicketeraId"] = dr["tick_sTicketeraId"];
                    drs["tick_vLlamada"] = dr["tick_vLlamada"];
                    drs["tick_sUsuarioAtendio"] = dr["tick_sUsuarioAtendio"];
                    drs["tick_cEstado"] = dr["tick_cEstado"];
                    drs["tick_sUsuarioCreacion"] = dr["tick_sUsuarioCreacion"];
                    drs["tick_vIPCreacion"] = dr["tick_vIPCreacion"];
                    drs["tick_dFechaCreacion"] = dr["tick_dFechaCreacion"];
                    drs["tick_sUsuarioModificacion"] = dr["tick_sUsuarioModificacion"];
                    drs["tick_vIPModificacion"] = dr["tick_vIPModificacion"];
                    drs["tick_dFechaModificacion"] = dr["tick_dFechaModificacion"];
                    drs["serv_vDescripcion"] = dr["serv_vDescripcion"];

                    dsss.Tables[1].Rows.Add(drs);
                }

                DataTable dt_filtro = null;
                try
                {
                    dt_filtro = (from dt_1 in dsss.Tables[1].AsEnumerable()
                                 where Convert.ToDateTime(dt_1["tick_dFechaHoraGeneracion"]).ToShortDateString() == Convert.ToDateTime(FechaFiltro).ToShortDateString()
                                 select dt_1).CopyToDataTable();
                }
                catch
                {
                    dt_filtro = new DataTable();
                }

                ReportDataSource datasource1 = new ReportDataSource("DataSet1", dt_filtro);


                ReportParameter[] para = new ReportParameter[parameters.Length];
                para = parameters;
                reportViewer1.LocalReport.DataSources.Add(datasource1);
                reportViewer1.LocalReport.SetParameters(para);
                this.reportViewer1.RefreshReport();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}