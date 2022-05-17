using ClosedXML.Excel;
using SGAC.Accesorios;
using SGAC.WebApp.Accesorios;
using SUNARP.BE;
using SUNARP.Registro.Inscripcion.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SGAC.WebApp.Sunarp
{
    public partial class SeguimientoSolicitudesSunarp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                cargarAniosMeses();
                CargarOficina();
                LlenarListas();
                setearValores();
            }

        }
        private void CargarOficina()
        {
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ctrlOficinaConsular1.Cargar(true, true, " - SELECCIONAR - ", "");
            }
            else
            {
                ctrlOficinaConsular1.Cargar(false, false);
            }
        }
        private void setearValores()
        {
            txtFechaFin.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-dd");
            txtFechaInicio.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-dd");
            txtFecSolFin.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-dd");
            txtFecSolIni.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-dd");
        }
        private void cargarAniosMeses()
        {
            try
            {
                //CARGA AÑOS DESDE 2017 ------------------------------------------------------------
                ListItem yearTodos = new ListItem();
                yearTodos.Text = "[ SELECCIONE ]";
                yearTodos.Value = "0";
                ddlAnio.Items.Add(yearTodos);

                int currentYear = DateTime.Today.Year;
                for (int year = 2017; year <= currentYear; year++)
                {
                    ListItem yearItem = new ListItem();
                    yearItem.Text = year.ToString();
                    yearItem.Value = year.ToString();
                    ddlAnio.Items.Add(yearItem);

                }
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The operation is not valid for the state") || ex.ToString().Contains("La operación no es válida para el estado"))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", "HTTP 404 - intentelo de nuevo", true);
                }
                else
                {
                    Session["_LastException"] = ex;
                    Response.Redirect("../PageError/GenericErrorPage.aspx");
                }
            }
        }

        private void LlenarListas()
        {
            DataTable dt = new DataTable();
            dt = comun_Part1.ObtenerParametrosPorGrupoMRE("SUNARP-SOLICITUDES");
            Util.CargarParametroDropDownList(ddlEstado, dt, true);
        }
        protected void VerHistorial(object sender, ImageClickEventArgs e)
        {
            SU_SOLICITUD_INSCRIPCION objSolicitudBE = new SU_SOLICITUD_INSCRIPCION();
            SolicitudBL objSolicitudBL = new SolicitudBL();

            string _iSolicitud;
            _iSolicitud = ((ImageButton)sender).Attributes["iSolicitud"].ToString();
            DataTable dt = objSolicitudBL.SolicitudInscripcionConsultaHistorico(Convert.ToInt64(_iSolicitud));

            if (dt.Rows.Count > 0)
            {
                lblCUOHistorico.Text = dt.Rows[0]["CUO"].ToString();
                grvDetalle.DataSource = dt;
                grvDetalle.DataBind();
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", "Popup();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", "alert('No se encontraron datos a mostrar');", true);
            }
            buscar();
        }

        //protected void VerParte(object sender, ImageClickEventArgs e)
        //{

        //}
        protected void grvSolicitudes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnVer = e.Row.FindControl("btnVer") as ImageButton;
                ImageButton btnParte = e.Row.FindControl("btnParte") as ImageButton;


                btnParte.Visible = false;
                btnVer.Visible = true;

         
                    if (Server.HtmlDecode(e.Row.Cells[12].Text.Trim()).Length > 1)
                    {
                        btnParte.Visible = true;
                        e.Row.BackColor = Color.FromName("#A4F7EB");
                    }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                buscar();
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The operation is not valid for the state") || ex.ToString().Contains("La operación no es válida para el estado"))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", "HTTP 404 - intentelo de nuevo", true);
                }
                else
                {
                    Session["_LastException"] = ex;
                    Response.Redirect("../PageError/GenericErrorPage.aspx");
                }
            }
        }
        private void buscar()
        {
            try
            {
                SU_SOLICITUD_INSCRIPCION objSolicitudBE = new SU_SOLICITUD_INSCRIPCION();
                SolicitudBL objSolicitudBL = new SolicitudBL();

                DateTime? fechaInicioExpedicion = null;
                DateTime? fechaFinExpedicion = null;

                DateTime? fechaInicioSolicitud = null;
                DateTime? fechaFinSolicitud = null;

                if (txtFechaInicio.Text.Length > 0)
                {
                    fechaInicioExpedicion = Convert.ToDateTime(txtFechaInicio.Text);
                }
                if (txtFechaFin.Text.Length > 0)
                {
                    fechaFinExpedicion = Convert.ToDateTime(txtFechaFin.Text);
                }

                if (txtFecSolIni.Text.Length > 0)
                {
                    fechaInicioSolicitud = Convert.ToDateTime(txtFecSolIni.Text);
                }
                if (txtFecSolFin.Text.Length > 0)
                {
                    fechaFinSolicitud = Convert.ToDateTime(txtFecSolFin.Text);
                }


                DataTable dt = objSolicitudBL.consultaSolicitud(Convert.ToInt16(ctrlOficinaConsular1.SelectedValue), txtEscritura.Text, fechaInicioExpedicion, fechaFinExpedicion, ddlAnio.SelectedValue, txtNroTitulo.Text,
                    txtNumCuo.Text, Convert.ToInt16(ddlEstado.SelectedValue), fechaInicioSolicitud, fechaFinSolicitud);

                grvSolicitudes.DataSource = dt;
                grvSolicitudes.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }
        private void limpiar()
        {
            txtNumCuo.Text = "";
            txtNroTitulo.Text = "";
            txtFecSolIni.Text = "";
            txtFecSolFin.Text = "";
            txtFechaInicio.Text = "";
            txtFechaFin.Text = "";
            txtEscritura.Text = "";
            ddlAnio.SelectedIndex = 0;
            ddlEstado.SelectedIndex = 0;
            grvSolicitudes.DataSource = null;
            grvSolicitudes.DataBind();
        }

        protected void btnReporte_Click(object sender, EventArgs e)
        {
            try
            {
                SU_SOLICITUD_INSCRIPCION objSolicitudBE = new SU_SOLICITUD_INSCRIPCION();
                SolicitudBL objSolicitudBL = new SolicitudBL();

                DateTime? fechaInicioExpedicion = null;
                DateTime? fechaFinExpedicion = null;

                DateTime? fechaInicioSolicitud = null;
                DateTime? fechaFinSolicitud = null;

                if (txtFechaInicio.Text.Length > 0)
                {
                    fechaInicioExpedicion = Convert.ToDateTime(txtFechaInicio.Text);
                }
                if (txtFechaFin.Text.Length > 0)
                {
                    fechaFinExpedicion = Convert.ToDateTime(txtFechaFin.Text);
                }

                if (txtFecSolIni.Text.Length > 0)
                {
                    fechaInicioSolicitud = Convert.ToDateTime(txtFecSolIni.Text);
                }
                if (txtFecSolFin.Text.Length > 0)
                {
                    fechaFinSolicitud = Convert.ToDateTime(txtFecSolFin.Text);
                }


                DataTable dt = objSolicitudBL.consultaSolicitud(Convert.ToInt16(ctrlOficinaConsular1.SelectedValue), txtEscritura.Text, fechaInicioExpedicion, fechaFinExpedicion, ddlAnio.SelectedValue, txtNroTitulo.Text,
                    txtNumCuo.Text, Convert.ToInt16(ddlEstado.SelectedValue), fechaInicioSolicitud, fechaFinSolicitud);

                ImprimirExcel(dt);

            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("The operation is not valid for the state") || ex.ToString().Contains("La operación no es válida para el estado"))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", "HTTP 404 - intentelo de nuevo", true);
                }
                else
                {
                    Session["_LastException"] = ex;
                    Response.Redirect("../PageError/GenericErrorPage.aspx");
                }
            }
        }
        private void ImprimirExcel(DataTable dt)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                // creamos la hoja
                var Worksheet = wb.Worksheets.Add("Detallado");

                //Ponemos algunos valores en el documento
                Worksheet.Range("A1:L1").Merge()
                    .SetValue("MINISTERIO DE RELACIONES EXTERIORES")
                    .Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left)
                    .Font.Bold = true;
                Worksheet.Cell("A1").Style.Fill.SetBackgroundColor(XLColor.BlueGray);
                Worksheet.Cell("A1").Style.Font.FontColor = XLColor.White;
                Worksheet.Cell("A1").Style.Font.FontSize = 16;

                // Agregar Borde a la celda
                Worksheet.Range("A1:L1").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                Worksheet.Range("A1:L1").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                Worksheet.Range("A1:L1").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                Worksheet.Range("A1:L1").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                Worksheet.Range("A1:L1").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                Worksheet.Range("A1:L1").Style.Border.TopBorder = XLBorderStyleValues.Thin;

                Worksheet.Cell("M1").Value = "FECHA IMPRESIÓN: " + DateTime.Now.ToString("dd/MM/yyyy");
                Worksheet.Cell("M1").Style.Fill.SetBackgroundColor(XLColor.BlueGray);
                Worksheet.Cell("M1").Style.Font.FontColor = XLColor.White;
                Worksheet.Cell("M1").Style.Font.FontSize = 16;
                Worksheet.Cell("M1").Style.Font.Bold = true;

                //Ponemos algunos valores en el documento
                Worksheet.Range("A2:M2").Merge()
                    .SetValue("REPORTE - " + "SOLICITUDES SUNARP")
                    .Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
                    .Font.Bold = true;
                Worksheet.Cell("A2").Style.Fill.SetBackgroundColor(XLColor.BlueGray);
                Worksheet.Cell("A2").Style.Font.FontColor = XLColor.White;
                Worksheet.Cell("A2").Style.Font.FontSize = 16;

                // Agregar Borde a la celda
                Worksheet.Range("A2:M2").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                Worksheet.Range("A2:M2").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                Worksheet.Range("A2:M2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                Worksheet.Range("A2:M2").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                Worksheet.Range("A2:M2").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                Worksheet.Range("A2:M2").Style.Border.TopBorder = XLBorderStyleValues.Thin;

                //Podemos insertar un DataTable
                wb.Worksheets.Worksheet(1).Cell("A4").InsertTable(dt);
                //Aplicamos los filtros y formatos a la tabla 
                wb.Worksheets.Worksheet(1).Table("Table1").ShowAutoFilter = true;
                wb.Worksheets.Worksheet(1).Table("Table1").Style.Alignment.Vertical =
                    XLAlignmentVerticalValues.Center;
                wb.Worksheets.Worksheet(1).Columns(1, 2 + dt.Columns.Count).AdjustToContents();

                //Limitamos el ancho de las columnas a 60
                foreach (var column in wb.Worksheets.Worksheet(1).Columns())
                    if (column.Width > 60)
                    {
                        column.Width = 60;
                        column.Style.Alignment.WrapText = true;
                    }

                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                //Enviamos el archivo al cliente
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=\"" + "Solicitudes - Sunarp" + ".xlsx\"");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    HttpContext.Current.Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                   ScriptManager.RegisterStartupScript(this, typeof(Page), "invocarfuncion", "cerrarPopup();", true);
                }
            }
        }
    }
}