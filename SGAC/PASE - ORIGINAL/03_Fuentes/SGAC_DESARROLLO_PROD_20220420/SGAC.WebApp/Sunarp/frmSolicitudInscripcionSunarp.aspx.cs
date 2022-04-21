using SGAC.Accesorios;
using SGAC.WebApp.Accesorios;
using SUNARP.Registro.Inscripcion.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SUNARP.BE;
using SGAC.Registro.Actuacion.BL;
using System.Drawing;
using iTextSharp.text.pdf;
using System.IO;
using System.Web.Services;

namespace SGAC.WebApp.Sunarp
{
    public partial class frmSolicitudInscripcionSunarp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LlenarListas();
                CargarOficina();
                setearValores();
                LlenarListasParticipante();
                //Comun.EjecutarScript(Page, Util.DeshabilitarTab(1) + Util.DeshabilitarTab(2));
            }
        }

        protected void grvSolicitudes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnSolicitud = e.Row.FindControl("btnSolicitud") as ImageButton;
                ImageButton btnReingreso = e.Row.FindControl("btnReingreso") as ImageButton;
                ImageButton btnVer = e.Row.FindControl("btnVer") as ImageButton;
                ImageButton btnEnviar = e.Row.FindControl("btnEnviar") as ImageButton;
                ImageButton btnFirmar = e.Row.FindControl("btnFirmar") as ImageButton;


                btnSolicitud.Visible = false;
                btnReingreso.Visible = false;
                btnVer.Visible = false;
                btnFirmar.Visible = false;
                btnEnviar.Visible = false;

                if (e.Row.Cells[6].Text == "VINCULADO")
                {
                    if (Convert.ToBoolean(e.Row.Cells[15].Text) == true)
                    {
                        btnReingreso.Visible = true;
                    }
                    else {
                        btnSolicitud.Visible = true;
                        btnReingreso.Visible = true;
                    }
                }
                else if (e.Row.Cells[6].Text == "REGISTRADO")
                {
                    btnFirmar.Visible = true;
                    btnVer.Visible = true;

                    if (Server.HtmlDecode(e.Row.Cells[14].Text.Trim()).Length > 1)
                    {
                        btnEnviar.Visible = true;
                        btnFirmar.Visible = false;
                        e.Row.BackColor = Color.FromName("#A4F7EB");
                    }
                }
                else
                {
                    if (Server.HtmlDecode(e.Row.Cells[14].Text.Trim()).Length > 1)
                    {
                        btnEnviar.Visible = false;
                        btnFirmar.Visible = false;
                        e.Row.BackColor = Color.FromName("#A4F7EB");
                    }
                    btnVer.Visible = true;
                }
            }
        }
        private void LlenarListasParticipante()
        {

            DataTable dtTipoParticipantes = new DataTable();


            dtTipoParticipantes = comun_Part1.ObtenerParametrosPorGrupo(Session, "REGISTRO NOTARIAL- PROTOCOLAR TIPO PARTICIPANTE");
            Util.CargarDropDownList(ddlTipParticipante, dtTipoParticipantes, "descripcion", "id", true);

            //-----------------------------

            DataTable dtTipoDocumento = new DataTable();
            dtTipoDocumento = comun_Part1.ObtenerDocumentoIdentidad();

            DataView dvTipoDocumento = dtTipoDocumento.DefaultView;
            dvTipoDocumento.RowFilter = "TipoDocumentoSUNARP <> ''";
            DataTable dtTipoDocumentoSUNARP = dvTipoDocumento.ToTable();

            Util.CargarDropDownList(ddlTipDoc, dtTipoDocumentoSUNARP, "Valor", "Id", true);

        }
        private void setearValores()
        {
            txtFechaFin.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-dd");
            txtFechaInicio.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-dd");
        }
        private void LlenarListas()
        {
            DataTable dt = new DataTable();
            dt = comun_Part1.ObtenerParametrosPorGrupo(Session, "SUNARP-ACTO-REGISTRAL");
            Util.CargarDropDownList(ddlActoRegistral, dt, "descripcion", "para_vvalor", true);

            //dt = comun_Part1.ObtenerParametrosPorGrupoMRE("SUNARP-SOLICITUDES");
            //Util.CargarParametroDropDownList(ddlEstado, dt, true);


            dt = comun_Part1.ObtenerParametrosPorGrupo(Session, "SUNARP-TIPO-REINGRESO");
            Util.CargarDropDownList(ddlTipReingreso, dt, "descripcion", "para_vvalor", true);

            // TIPO PARTICIPANTE
            Util.CargarParametroDropDownList(ddlTipParticipante, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_NOTARIAL_TIPO_ACTO_PROTOCOLAR), true);

            ////----------------------------------------------------

            MaestroOficinasBL objOficinasRegistralesBL = new MaestroOficinasBL();
            int intTotalPages = 0;
            dt = objOficinasRegistralesBL.consultar(0, "", "00", "A", "S", 1000, "1", "N", ref intTotalPages);
            Util.CargarDropDownList(ddlZonaRegistral, dt, "ofic_vDescripcion", "ofic_cCodigoZona", true);
            Util.CargarDropDownList(ddlZonaRegistralReingreso, dt, "ofic_vDescripcion", "ofic_cCodigoZona", true);

            ddlOfiRegistral.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
            ddlOfiRegistral.DataBind();
            ddlOfiRegistralReingreso.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
            ddlOfiRegistralReingreso.DataBind();
            //////----------------------------------------------------



            int intOficinaConsularId = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            dt = funcionario.dtFuncionario(intOficinaConsularId, 0);

            Util.CargarDropDownList(ddlFuncionario, dt, "vFuncionario", "iFuncionarioId", true);

        }

        private void CargarOficina()
        {
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ctrlOficinaConsular.Cargar(true, true, " - SELECCIONAR - ", "");
            }
            else
            {
                ctrlOficinaConsular.Cargar(false, false);
            }
        }
        private void Buscar()
        {
            SU_SOLICITUD_INSCRIPCION objSolicitudBE = new SU_SOLICITUD_INSCRIPCION();
            SolicitudBL objSolicitudBL = new SolicitudBL();


            DateTime? fechaInicio = null;
            DateTime? fechaFin = null;

            if (txtFechaInicio.Text.Length > 0)
            {
                fechaInicio = Convert.ToDateTime(txtFechaInicio.Text);
            }
            if (txtFechaFin.Text.Length > 0)
            {
                fechaFin = Convert.ToDateTime(txtFechaFin.Text);
            }
            DataTable dt = objSolicitudBL.consultaMultiple(Convert.ToInt16(ctrlOficinaConsular.SelectedValue), txtEscritura.Text.Trim(), fechaInicio, fechaFin, Convert.ToInt16(ddlTipParticipante.SelectedValue),
                Convert.ToInt16(ddlTipDoc.SelectedValue), txtNumDoc.Text, txtPrimerApellido.Text.Trim(), txtSegundoApellido.Text.Trim(), txtNombres.Text.Trim(), chkReingreso.Checked);

            grvSolicitudes.DataSource = dt;
            grvSolicitudes.DataBind();
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                limpiarDatosRegistro();
                Buscar();


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
        protected void RegistrarSolicitud(object sender, ImageClickEventArgs e)
        {
            try
            {
                BloquearRegistro(true);
                string _iSolicitud;
                _iSolicitud = ((ImageButton)sender).Attributes["iSolicitud"].ToString();

                Int64 _iActuacion;
                _iActuacion = Int64.Parse(((ImageButton)sender).Attributes["iActuacion"]);

                Int64 _iActoNotarial;
                _iActoNotarial = Int64.Parse(((ImageButton)sender).Attributes["iActoNotarial"]);


                hiActuacion.Value = _iActuacion.ToString();
                hISolicitud.Value = _iSolicitud.ToString();
                hiActoNotarial.Value = _iActoNotarial.ToString();

                if (lblCUO.Text.Length == 0)
                {
                    btnEliminar.Enabled = false;
                }
                else {
                    btnEliminar.Enabled = true;
                }


                DataTable dt = ConsultaActoNotarial();
                try {
                    ddlFuncionario.SelectedValue = dt.Rows[0]["acno_IFuncionarioAutorizadorId"].ToString();
                }
                catch
                {
                }
                txtCorreoSolicitante.Text = dt.Rows[0]["Correo_Solicitante"].ToString();
                grvParticipantes.DataSource = dt;
                grvParticipantes.DataBind();
                Comun.EjecutarScript(Page, Util.HabilitarTab(1));

                return;
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
        private void llenarZonaRegistral()
        {
            if (ddlZonaRegistral.SelectedIndex == 0)
            {
                ddlOfiRegistral.Items.Clear();
                ddlOfiRegistral.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
                ddlOfiRegistral.DataBind();
            }
            else
            {
                string strCodigoZona = ddlZonaRegistral.SelectedValue;
                DataTable dtOficinasRegistrales = new DataTable();
                MaestroOficinasBL objOficinasRegistralesBL = new MaestroOficinasBL();
                int intTotalPages = 0;
                dtOficinasRegistrales = objOficinasRegistralesBL.consultar(0, strCodigoZona, "", "A", "S", 1000, "1", "N", ref intTotalPages);
                Util.CargarDropDownList(ddlOfiRegistral, dtOficinasRegistrales, "ofic_vDescripcion", "ofic_cCodigoOficina", true);
            }
        }
        private void llenarZonaRegistralReingreso()
        {
            if (ddlZonaRegistralReingreso.SelectedIndex == 0)
            {
                ddlZonaRegistralReingreso.Items.Clear();
                ddlZonaRegistralReingreso.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
                ddlZonaRegistralReingreso.DataBind();
            }
            else
            {
                string strCodigoZona = ddlZonaRegistralReingreso.SelectedValue;
                DataTable dtOficinasRegistrales = new DataTable();
                MaestroOficinasBL objOficinasRegistralesBL = new MaestroOficinasBL();
                int intTotalPages = 0;
                dtOficinasRegistrales = objOficinasRegistralesBL.consultar(0, strCodigoZona, "", "A", "S", 1000, "1", "N", ref intTotalPages);
                Util.CargarDropDownList(ddlOfiRegistralReingreso, dtOficinasRegistrales, "ofic_vDescripcion", "ofic_cCodigoOficina", true);
            }
        }
        private void LimpiarDatosBusqueda()
        {
            txtFechaFin.Text = "";
            txtFechaInicio.Text = "";
            txtEscritura.Text = "";
            txtNombres.Text = "";
            txtNumDoc.Text = "";
            txtPrimerApellido.Text = "";
            txtSegundoApellido.Text = "";
            txtNombres.Text = "";
            ddlTipDoc.SelectedIndex = 0;
            ddlTipParticipante.SelectedIndex = 0;
            grvSolicitudes.DataSource = null;
            grvSolicitudes.DataBind();
            hiActuacion.Value = "0";
            hISolicitud.Value = "0";

        }
        private DataTable ConsultaActoNotarial()
        {
            DataTable dtActuacion = new DataTable();
            ActuacionConsultaBL objActuacionBL = new ActuacionConsultaBL();
            Int64 intActoNotarial = Convert.ToInt64(hiActoNotarial.Value.ToString());

            dtActuacion = objActuacionBL.ConsultarActoNotarialPorIDActuacion(intActoNotarial);
            return dtActuacion;
        }
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarDatosBusqueda();
        }

        protected void ddlZonaRegistral_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenarZonaRegistral();
            Comun.EjecutarScript(Page, Util.HabilitarTab(1));
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                SU_SOLICITUD_INSCRIPCION objSolicitudBE = new SU_SOLICITUD_INSCRIPCION();
                SolicitudBL objSolicitudBL = new SolicitudBL();

                objSolicitudBE.acno_iActoNotarialId = Convert.ToInt64(hiActoNotarial.Value);
                objSolicitudBE.SOIN_VCODZONAREGISTRAL = ddlZonaRegistral.SelectedValue;
                objSolicitudBE.SOIN_VCODOFICINAREGISTRA = ddlOfiRegistral.SelectedValue;
                objSolicitudBE.OFICINACONSULARID = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                objSolicitudBE.SOIN_IACTUACIONID = Convert.ToInt64(hiActuacion.Value);
                objSolicitudBE.SOIN_VCODACTO = ddlActoRegistral.SelectedValue;
                objSolicitudBE.SOIN_VDESACTO = ddlActoRegistral.SelectedItem.Text;
                objSolicitudBE.SOIN_VNOINSCRIBIR = txtNoInscribir.Text.ToUpper();
                objSolicitudBE.IFuncionarioId = Convert.ToInt32(ddlFuncionario.SelectedValue);
                objSolicitudBE.SOIN_VCORREOPRESENTANTE = txtCorreoPresentante.Text.ToUpper();
                objSolicitudBE.SOIN_VCORREOSOLICITANTE = txtCorreoSolicitante.Text.ToUpper();
                objSolicitudBE.SOIN_SUSUARIOCREACION = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objSolicitudBE.SOIN_VIPCREACION = Util.ObtenerDireccionIP();

                objSolicitudBL.insertar(objSolicitudBE);

                lblCUO.Text = objSolicitudBE.SOIN_VCUO;
                hISolicitud.Value = objSolicitudBE.SOIN_ISOLICITUDINSCRIPCIONID.ToString();
                btnEliminar.Enabled = true;
                btnBuscar_Click(sender, e);

                string script = Util.HabilitarTab(0) + Util.DeshabilitarTab(1);
                script = script + "alert('Se registro correctamente la solicitud: " + lblCUO.Text + "' );";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", script, true);
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

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                SU_SOLICITUD_INSCRIPCION objSolicitudBE = new SU_SOLICITUD_INSCRIPCION();
                SolicitudBL objSolicitudBL = new SolicitudBL();
                objSolicitudBE.SOIN_ISOLICITUDINSCRIPCIONID = Convert.ToInt64(hISolicitud.Value);
                objSolicitudBE.SOIN_SUSUARIOCREACION = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objSolicitudBE.SOIN_VIPCREACION = Util.ObtenerDireccionIP();

                objSolicitudBL.eliminar(objSolicitudBE);

                lblCUO.Text = "";
                hISolicitud.Value = "0";
                btnEliminar.Enabled = false;
                LimpiarDatosBusqueda();
                limpiarDatosRegistro();
                string script = Util.HabilitarTab(0) + Util.DeshabilitarTab(1);
                script = script + "alert('Se anuló correctamente la solicitud');";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", script, true);

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
        protected void btnEliminarReingreso_Click(object sender, EventArgs e)
        {
            try
            {
                SU_SOLICITUD_INSCRIPCION objSolicitudBE = new SU_SOLICITUD_INSCRIPCION();
                SolicitudBL objSolicitudBL = new SolicitudBL();
                objSolicitudBE.SOIN_ISOLICITUDINSCRIPCIONID = Convert.ToInt64(hISolicitud.Value);
                objSolicitudBE.SOIN_SUSUARIOCREACION = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objSolicitudBE.SOIN_VIPCREACION = Util.ObtenerDireccionIP();

                objSolicitudBL.eliminar(objSolicitudBE);

                LimpiarDatosReingreso();
                lblCUOReingreso.Text = "";
                hISolicitud.Value = "0";
                btnEliminarReingreso.Enabled = false;
                LimpiarDatosBusqueda();
                limpiarDatosRegistro();
                string script = Util.HabilitarTab(0) + Util.DeshabilitarTab(2);
                script = script + "alert('Se anuló correctamente la solicitud');";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", script, true);

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
        private void limpiarDatosRegistro()
        {
            ddlActoRegistral.SelectedIndex = 0;
            ddlFuncionario.SelectedIndex = 0;
            ddlOfiRegistral.SelectedIndex = 0;
            ddlZonaRegistral.SelectedIndex = 0;
            txtCorreoPresentante.Text = "";
            txtCorreoSolicitante.Text = "";
            txtNoInscribir.Text = "";
            grvParticipantes.DataSource = null;
            grvParticipantes.DataBind();
        }
        private void BloquearRegistro(bool valor = false)
        {
            ddlActoRegistral.Enabled = valor;
            ddlFuncionario.Enabled = valor;
            ddlOfiRegistral.Enabled = valor;
            ddlZonaRegistral.Enabled = valor;
            txtCorreoPresentante.Enabled = valor;
            txtCorreoSolicitante.Enabled = valor;
            txtNoInscribir.Enabled = valor;
            btnEliminar.Enabled = valor;
            btnGrabar.Enabled = valor;
        }
        private void BloquearReingreso(bool valor = false)
        {
            ddlOfiRegistralReingreso.Enabled = valor;
            txtNroCUOBusqueda.Enabled = valor;
            btnBuscarSolicitudAnterior.Enabled = valor;
            btnLimpiarBusquedaSolAnterior.Enabled = valor;
            ddlTipReingreso.Enabled = valor;
            ddlZonaRegistralReingreso.Enabled = valor;
            ddlOfiRegistralReingreso.Enabled = valor;
            txtAnioTituloReingreso.Enabled = valor;
            txtNumTituloReingreso.Enabled = valor;
            btnGrabarReingreso.Enabled = valor;
            btnEliminarReingreso.Enabled = valor;
        }
        protected void Ver(object sender, ImageClickEventArgs e)
        {
            try
            {
                string _iSolicitud;
                _iSolicitud = ((ImageButton)sender).Attributes["iSolicitud"].ToString();

                Int64 _iActuacion;
                _iActuacion = Int64.Parse(((ImageButton)sender).Attributes["iActuacion"]);

                Int64 _iActoNotarial;
                _iActoNotarial = Int64.Parse(((ImageButton)sender).Attributes["iActoNotarial"]);

                string _estado;
                _estado = ((ImageButton)sender).Attributes["estado"].ToString();

                string _correlativo;
                _correlativo = ((ImageButton)sender).Attributes["correlativo"].ToString();

                int correlativo = Convert.ToInt16(_correlativo);

                hiActuacion.Value = _iActuacion.ToString();
                hISolicitud.Value = _iSolicitud.ToString();
                hiActoNotarial.Value = _iActoNotarial.ToString();


                if (correlativo > 1)// REINGRESO
                {
                    if (_estado != "REGISTRADO")
                    {
                        BloquearDatosReingreso();
                    }
                    else
                    {
                        BloquearDatosReingreso();
                        btnEliminarReingreso.Enabled = true;
                    }

                    DataTable dtSolicitud = new DataTable();
                    SolicitudBL obj = new SolicitudBL();
                    Int64 intSolicitud = Convert.ToInt64(hISolicitud.Value.ToString());

                    dtSolicitud = obj.SolicitudInscripcionConsulta(intSolicitud);

                    lblCUOReingreso.Text = dtSolicitud.Rows[0]["SOIN_VCUO"].ToString();
                    ddlZonaRegistralReingreso.SelectedValue = dtSolicitud.Rows[0]["SOIN_VCODZONAREGISTRAL"].ToString();
                    llenarZonaRegistralReingreso();
                    ddlOfiRegistralReingreso.SelectedValue = dtSolicitud.Rows[0]["SOIN_VCODOFICINAREGISTRAL"].ToString();
                    txtAnioTituloReingreso.Text = dtSolicitud.Rows[0]["SOIN_VANIOTITULOSUNARP"].ToString();
                    txtNumTituloReingreso.Text = dtSolicitud.Rows[0]["SOIN_VNUMTITULOSUNARP"].ToString();
                    ddlTipReingreso.SelectedValue = dtSolicitud.Rows[0]["SOIN_VTIPREINGRESO"].ToString(); 

                    Comun.EjecutarScript(Page, Util.HabilitarTab(2));
                }
                else { // REGISTRO
                    if (_estado != "REGISTRADO")
                    {
                        BloquearRegistro();
                    }
                    else
                    {
                        BloquearRegistro();
                        btnEliminar.Enabled = true;
                    }

                    DataTable dtSolicitud = new DataTable();
                    SolicitudBL obj = new SolicitudBL();
                    Int64 intSolicitud = Convert.ToInt64(hISolicitud.Value.ToString());

                    dtSolicitud = obj.SolicitudInscripcionConsulta(intSolicitud);

                    ddlActoRegistral.SelectedValue = dtSolicitud.Rows[0]["SOIN_VCODACTO"].ToString();
                    ddlZonaRegistral.SelectedValue = dtSolicitud.Rows[0]["SOIN_VCODZONAREGISTRAL"].ToString();
                    llenarZonaRegistral();
                    ddlOfiRegistral.SelectedValue = dtSolicitud.Rows[0]["SOIN_VCODOFICINAREGISTRAL"].ToString();
                    txtNoInscribir.Text = dtSolicitud.Rows[0]["SOIN_VNOINSCRIBIR"].ToString();
                    txtCorreoPresentante.Text = dtSolicitud.Rows[0]["SOIN_VCORREOPRESENTANTE"].ToString();
                    txtCorreoSolicitante.Text = dtSolicitud.Rows[0]["SOIN_VCORREOSOLICITANTE"].ToString();
                    lblCUO.Text = dtSolicitud.Rows[0]["SOIN_VCUO"].ToString();
                    DataTable dt = ConsultaActoNotarial();
                    try
                    {
                        ddlFuncionario.SelectedValue = dt.Rows[0]["acno_IFuncionarioAutorizadorId"].ToString();
                    }
                    catch
                    {
                    }
                    grvParticipantes.DataSource = dt;
                    grvParticipantes.DataBind();
                    Comun.EjecutarScript(Page, Util.HabilitarTab(1));
                }
                Buscar();
                return;
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

        protected void ddlZonaRegistralReingreso_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenarZonaRegistralReingreso();
            Comun.EjecutarScript(Page, Util.HabilitarTab(2));
        }
        private void LimpiarDatosReingreso()
        {
            txtNroCUOBusqueda.Text = "";
            txtNroCUOBusqueda.Enabled = true;
            btnBuscarSolicitudAnterior.Enabled = true;
            lblCUOReingreso.Text = "";
            txtAnioTituloReingreso.Text = "";
            txtNumTituloReingreso.Text = "";
            ddlZonaRegistralReingreso.SelectedIndex = 0;
            ddlOfiRegistralReingreso.SelectedIndex = 0;
            ddlTipReingreso.SelectedIndex = 0;
        }
        private void BloquearDatosReingreso(bool valor = false)
        {
            txtNroCUOBusqueda.Enabled = valor;
            txtNroCUOBusqueda.Enabled = valor;
            btnBuscarSolicitudAnterior.Enabled = valor;
            lblCUOReingreso.Enabled = valor;
            txtAnioTituloReingreso.Enabled = valor;
            txtNumTituloReingreso.Enabled = valor;
            ddlZonaRegistralReingreso.Enabled = valor;
            ddlOfiRegistralReingreso.Enabled = valor;
            ddlTipReingreso.Enabled = valor;
            btnGrabarReingreso.Enabled = valor;
            btnLimpiarBusquedaSolAnterior.Enabled = false;
        }
        protected void btnLimpiarBusquedaSolAnterior_Click(object sender, EventArgs e)
        {
            LimpiarDatosReingreso();
            Comun.EjecutarScript(Page, Util.HabilitarTab(2));
        }
        protected void Reingreso(object sender, ImageClickEventArgs e)
        {
            try
            {
                BloquearReingreso(true);
                btnEliminarReingreso.Enabled = false;
                string _iSolicitud;
                _iSolicitud = ((ImageButton)sender).Attributes["iSolicitud"].ToString();

                Int64 _iActuacion;
                _iActuacion = Int64.Parse(((ImageButton)sender).Attributes["iActuacion"]);

                Int64 _iActoNotarial;
                _iActoNotarial = Int64.Parse(((ImageButton)sender).Attributes["iActoNotarial"]);


                hiActuacion.Value = _iActuacion.ToString();
                hISolicitud.Value = _iSolicitud.ToString();
                hiActoNotarial.Value = _iActoNotarial.ToString();

                Comun.EjecutarScript(Page, Util.HabilitarTab(2));
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

        protected void btnBuscarSolicitudAnterior_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtSolicitud = new DataTable();
                SolicitudBL obj = new SolicitudBL();
                string CUO = txtNroCUOBusqueda.Text.Trim();
                string script = Util.HabilitarTab(2);
                dtSolicitud = obj.SolicitudInscripcionConsulta(0,CUO);

                if (dtSolicitud.Rows.Count > 0)
                {
                    ddlZonaRegistralReingreso.SelectedValue = dtSolicitud.Rows[0]["SOIN_VCODZONAREGISTRAL"].ToString();
                    llenarZonaRegistralReingreso();
                    ddlOfiRegistralReingreso.SelectedValue = dtSolicitud.Rows[0]["SOIN_VCODOFICINAREGISTRAL"].ToString();
                    lblCUOReingreso.Text = CUO;
                    txtAnioTituloReingreso.Text = dtSolicitud.Rows[0]["SOIN_VANIOTITULOSUNARP"].ToString();
                    txtNumTituloReingreso.Text = dtSolicitud.Rows[0]["SOIN_VNUMTITULOSUNARP"].ToString();
                    txtNroCUOBusqueda.Enabled = false;
                    btnBuscarSolicitudAnterior.Enabled = false;
                }
                else {
                    script = script + "alert('No existe registros para la solicitud solicitada.');";
                }
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", script, true);
            }
            catch(Exception ex)
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

        protected void btnGrabarReingreso_Click(object sender, EventArgs e)
        {
            try
            {
                SU_SOLICITUD_INSCRIPCION objSolicitudBE = new SU_SOLICITUD_INSCRIPCION();
                SolicitudBL objSolicitudBL = new SolicitudBL();

                objSolicitudBE.SOIN_VCUO = lblCUOReingreso.Text;
                objSolicitudBE.SOIN_VCODZONAREGISTRAL = ddlZonaRegistralReingreso.SelectedValue;
                objSolicitudBE.SOIN_VCODOFICINAREGISTRA = ddlOfiRegistralReingreso.SelectedValue;
                objSolicitudBE.SOIN_IACTUACIONID = Convert.ToInt64(hiActuacion.Value);
                objSolicitudBE.SOIN_VANIOTITULOSUNARP = txtAnioTituloReingreso.Text;
                objSolicitudBE.SOIN_VNUMTITULOSUNARP = txtNumTituloReingreso.Text;
                objSolicitudBE.SOIN_VTIPREINGRESO = ddlTipReingreso.SelectedValue;
                objSolicitudBE.SOIN_SUSUARIOCREACION = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objSolicitudBE.SOIN_VIPCREACION = Util.ObtenerDireccionIP();

                objSolicitudBL.reingreso(objSolicitudBE);

                hISolicitud.Value = objSolicitudBE.SOIN_ISOLICITUDINSCRIPCIONID.ToString();
                btnEliminar.Enabled = true;
                btnBuscar_Click(sender, e);

                string script = Util.HabilitarTab(0) + Util.DeshabilitarTab(1);
                script = script + "alert('Se registro correctamente la solicitud de reingreso con el mismo número de CUO: " + lblCUOReingreso.Text + "' );";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", script, true);
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

        protected void Firmar(object sender, ImageClickEventArgs e)
        {
            try
            {
                string _iSolicitud;
                _iSolicitud = ((ImageButton)sender).Attributes["iSolicitud"].ToString();
                string _archivo;
                _archivo = ((ImageButton)sender).Attributes["cuo"].ToString();
                string _correlativo;
                _correlativo = ((ImageButton)sender).Attributes["correlativo"].ToString();

                hISolicitud.Value = _iSolicitud;

                // OBTENER NOMBRE DE DE ARCHIVO MIGUEL
                // GUARDAR EN DOCUMENTS
                string rutacompletaDocLinea = "";
                string nombreArchivo = _archivo + "_" + _correlativo + "FIRMADO.PDF";

                Session["NombreArchivoSinFirma"] = "20211025151017339.PDF";
                Session["NombreArchivoConFirma"] = nombreArchivo;
                int cantHojas = 0;//ObtenerCantidadHojasPDF(rutacompletaDocLinea);
                
                Session["CantHojasPDFOriginal"] = cantHojas;

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", "PopupMensaje();", true);
               
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
        private int ObtenerCantidadHojasPDF(string fileName)
        {
            string ppath = fileName;
            PdfReader pdfReader = new PdfReader(ppath);
            int numberOfPages = pdfReader.NumberOfPages;
            return numberOfPages;
        }
        private void CopiarRenombrarArchivo(string rutaArchivoCopiar, string rutaArchivoPegar)
        {
            string oldFile = rutaArchivoCopiar;
            string newFile = rutaArchivoPegar;
            File.Copy(oldFile, newFile);
        }

        [WebMethod]
        public static void ActualizarDocumentoSolicitud(string idSolicitud)
        {
            try
            {
                string documentName = HttpContext.Current.Session["NombreArchivoConFirma"].ToString();
                SU_SOLICITUD_INSCRIPCION objSolicitudBE = new SU_SOLICITUD_INSCRIPCION();
                SolicitudBL objSolicitudBL = new SolicitudBL();
                objSolicitudBE.SOIN_ISOLICITUDINSCRIPCIONID = Convert.ToInt64(idSolicitud);
                objSolicitudBE.SOIN_VPARTEFIRMADO = documentName;
                objSolicitudBE.SOIN_SUSUARIOCREACION = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                objSolicitudBE.SOIN_VIPCREACION = Util.ObtenerDireccionIP();
                objSolicitudBL.ActualizarDocumentoFirmado(objSolicitudBE);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Session["_LastException"] = ex;
                HttpContext.Current.Response.Redirect("../PageError/GenericErrorPage.aspx");
            }
        }

        protected void Enviar(object sender, ImageClickEventArgs e)
        {
            try
            {
                SU_SOLICITUD_INSCRIPCION objSolicitudBE = new SU_SOLICITUD_INSCRIPCION();
                SolicitudBL objSolicitudBL = new SolicitudBL();

                string _iSolicitud;
                _iSolicitud = ((ImageButton)sender).Attributes["iSolicitud"].ToString();

                objSolicitudBE.SOIN_ISOLICITUDINSCRIPCIONID = Convert.ToInt64(_iSolicitud);
                objSolicitudBE.SOIN_SUSUARIOCREACION = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                objSolicitudBE.SOIN_VIPCREACION = Util.ObtenerDireccionIP();
                objSolicitudBL.enviar(objSolicitudBE);


                // ENVIAR POR WEBSERVICE EL ID DE LA SOLICITUD

                btnBuscar_Click(sender, e);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", "alert('Se envio Correctamente la solicitud');", true);

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
    }
}