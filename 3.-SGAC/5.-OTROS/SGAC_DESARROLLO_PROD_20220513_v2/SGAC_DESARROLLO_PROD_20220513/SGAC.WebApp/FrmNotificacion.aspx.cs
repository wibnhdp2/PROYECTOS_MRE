using System;
using System.Data;
using System.Web.UI;
using SGAC.Accesorios;
using SGAC.WebApp.Accesorios;
using SGAC.Contabilidad.Remesa.BL;
using System.Web.UI.WebControls;
using System.Text;

namespace SGAC.WebApp
{
    public partial class FrmNotificacion : MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarAlertas();
            }
        }

        private void CargarAlertas()
        {
            // Alerta: plazo límite : 3- 5 - 10 días envío de remesa
            if (Session["MostrarAlertaPendiente"] != null)
            {
                if (Convert.ToInt32(Session["MostrarAlertaPendiente"]) == 1)
                {
                    Session["MostrarAlertaPendiente"] = 2;
                    lblMensaje.Text = "Tiene remesas pendiente de envío (Plazo finalizado).";
                }
            }

            // Detalle - Bandeja de Alertas
            Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            RemesaConsultasBL objBL = new RemesaConsultasBL();
            DataTable dt = objBL.ObtenerAlertas(intOficinaConsularId);

            if (dt != null)
            {
                gdvAlerta.DataSource = dt;
                gdvAlerta.DataBind();

                if (dt.Rows.Count > 0)
                {
                    StringBuilder strDetalle = new StringBuilder();

                    strDetalle.Append("Periodo Actual: " + DateTime.Today.Year + "-" + (DateTime.Today.Month).ToString().PadLeft(2, '0'));
                    strDetalle.Append("\n");
                    strDetalle.Append(" - Hay " + dt.Rows[0]["NUM_CONSULADOS_PEND"].ToString() + " oficina(s) consular(es) que no enviaron sus remesas.");
                    strDetalle.Append("\n");

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (intOficinaConsularId == Convert.ToInt32(dr["reno_sOficinaConsularOrigenId"]))
                        {
                            if (Convert.ToInt32(dr["PENDIENTE"]) > 0)
                            {
                                strDetalle.Append("\n");
                                strDetalle.Append("Tiene " + Convert.ToInt32(dr["PENDIENTE"]) + " remesa(s) pendiente(s) de envío.");
                            }
                            else if (Convert.ToInt32(dr["OBSERVADO"]) > 0)
                            {
                                strDetalle.Append("\n");
                                strDetalle.Append("Tiene " + Convert.ToInt32(dr["OBSERVADO"]) + " remesa(s) observada(s) por atender.");
                            }
                        }
                        else if (intOficinaConsularId == Convert.ToInt32(dr["reno_sOficinaConsularDestinoId"]))
                        {
                            if (Convert.ToInt32(dr["ENVIADO"]) > 0)
                            {
                                strDetalle.Append("\n");
                                strDetalle.Append("Tiene " + Convert.ToInt32(dr["ENVIADO"]) + " remesa(s) enviada(s) por revisar.");
                            }
                        }
                        if (intOficinaConsularId == Constantes.CONST_OFICINACONSULAR_LIMA)
                        {
                            if (Convert.ToInt32(dr["APROBADO"]) > 0)
                            {
                                strDetalle.Append("\n");
                                strDetalle.Append("Tiene " + Convert.ToInt32(dr["APROBADO"]) + " remesa(s) por cerrar.");
                            }
                        }
                    }

                    if (strDetalle.Length > 0)
                    {
                        lblTituloDetalle.Visible = true;
                        txtDetalle.Text = strDetalle.ToString();
                        txtDetalle.Visible = true;
                    }
                }
                else
                {                    
                    lblAviso.Text = "No hay alertas de remesas.";
                    lblAviso.Visible = true;
                }
            }
        }

        protected void gdvAlerta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                
                Int16 intOficinaConsularOrigenId = Convert.ToInt16(e.Row.Cells[0].Text);
                Int16 intOficinaConsularDestinoId = Convert.ToInt16(e.Row.Cells[2].Text);

                int intCantidad = 0;
                if (intOficinaConsularId == intOficinaConsularOrigenId)
                {
                    intCantidad = Convert.ToInt32(e.Row.Cells[5].Text);
                    if (intCantidad > 0)
                        e.Row.Cells[5].BackColor = System.Drawing.Color.FromName("#FFAAAA");

                    intCantidad = Convert.ToInt32(e.Row.Cells[6].Text);
                    if (intCantidad > 0)
                        e.Row.Cells[6].BackColor = System.Drawing.Color.FromName("#FFAAAA");
                }
                if (intOficinaConsularId == intOficinaConsularDestinoId)
                {
                    intCantidad = Convert.ToInt32(e.Row.Cells[7].Text);
                    if (intCantidad > 0)
                        e.Row.Cells[7].BackColor = System.Drawing.Color.FromName("#FFAAAA");

                    intCantidad = Convert.ToInt32(e.Row.Cells[8].Text);
                    if (intCantidad > 0)
                        e.Row.Cells[8].BackColor = System.Drawing.Color.FromName("#FFAAAA");
                }
            }
        }

        protected void lbIrRemesa_Click(object sender, EventArgs e)
        {
            Session.Add("IR_A_REMESA_CONSULAR", 1);
        }
    }
}