using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.WebApp.Accesorios;
using SGAC.Registro.Actuacion.BL;
using SGAC.BE.MRE.Custom;
using System.Web.Services;
using System.Web;
using System.Web.Script.Serialization;
using System.Configuration;

namespace SGAC.WebApp.Consulta
{
    public partial class FrmPasaporte : MyBasePage
    {
        #region Eventos
        private void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_PAGE_SIZE_ACTUACIONES;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlToolBarConsulta.VisibleIButtonBuscar = true;
            ctrlToolBarConsulta.VisibleIButtonCancelar = true;
            ctrlToolBarConsulta.btnBuscarHandler +=new Accesorios.SharedControls.ctrlToolBarButton.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);
            ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);
            ctrlToolBarConsulta.btnCancelar.Text = "   Limpiar";
            btn_grabar.OnClientClick = "return Validar_Grabar();";
            toolbarAnular.btnGrabar.OnClientClick = "return Validar_Cambiar_Estado();";
            btn_grabar.Visible = false;
            if (ddlTipoDocConsulta.SelectedIndex > 0)
            {
                int intTipoDocMigratorio = Convert.ToInt32(ddlTipoDocConsulta.SelectedValue);
                switch (intTipoDocMigratorio)
                {
                    case (int)Enumerador.enmDocumentoMigratorio.VISAS:
                        btn_grabar.Visible = true;
                        btn_grabar.Enabled = false;
                        break;
                }
            }

            
            toolbarModificar.VisibleIButtonGrabar = true;
            toolbarModificar.VisibleIButtonCancelar = true;
            toolbarModificar.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonGrabarClick(toolbarModificar_btnGrabarHandler);
            toolbarModificar.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonCancelarClick(toolbarModificar_btnCancelarHandler);

            toolbarAnular.VisibleIButtonGrabar = true;
            toolbarAnular.VisibleIButtonCancelar = true;
            toolbarAnular.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonGrabarClick(toolbarAnular_btnGrabarHandler);
            toolbarAnular.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarButton.OnButtonCancelarClick(toolbarAnular_btnCancelarHandler);

            txtFechaExpiracion.AllowFutureDate = true;
            txtFecFin.AllowFutureDate = true;

            txtFechaAprobacion.StartDate = ObtenerFechaActual(Session);
            txtFechaAprobacion.AllowFutureDate = true;

            if (!Page.IsPostBack)
            {
                pnlDatosTitular.Visible = false;

                gdvDetalle.Columns[13].Visible = IsConsulado();
                gdvDetalle.Columns[14].Visible = IsConsuladoLima();

                CargarListadosDesplegables();
                CargarDatosIniciales();

                int intOficinaConsular = Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                if (intOficinaConsular != Constantes.CONST_OFICINACONSULAR_LIMA)
                    ddlOficinaConsular.Enabled = false;
            }
        }        

        void toolbarModificar_btnGrabarHandler()
        {
        }

        void toolbarAnular_btnGrabarHandler()
        {
        }        

        void toolbarModificar_btnCancelarHandler()
        {
            txtFechaAprobacion.Text = "";
            string strScript = string.Empty;
            strScript += Util.DeshabilitarTab(1, "tabs", true);
            strScript += Util.HabilitarTab(0);
            Comun.EjecutarScript(Page, strScript);
        }

        void toolbarAnular_btnCancelarHandler()
        {
            string strScript = string.Empty;
            strScript += Util.DeshabilitarTab(2, "tabs", true);
            strScript += Util.HabilitarTab(0);
            Comun.EjecutarScript(Page, strScript);
        }

        protected void gdvMigratorio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strScript = string.Empty;
            int intSeleccionado = Convert.ToInt32(e.CommandArgument);

            hdn_acmi_iActuacionDetalleId.Value = Convert.ToInt64(gdvMigratorio.Rows[intSeleccionado].Cells[0].Text).ToString();
            hdn_acmi_iActoMigratorioId.Value = Convert.ToInt64(gdvMigratorio.Rows[intSeleccionado].Cells[1].Text).ToString();
            hdn_pers_iPersonaId.Value = Convert.ToInt64(gdvMigratorio.Rows[intSeleccionado].Cells[2].Text).ToString();
            string s_Tipo = gdvMigratorio.Rows[intSeleccionado].Cells[14].Text;
            if (s_Tipo.Equals("EXPEDIDO") || s_Tipo.Equals("REVALIDADO"))
            {
                Session["Tipo_Pasaporte"] = s_Tipo;
            }
            else
            {
                Session["Tipo_Pasaporte"] = "EXPEDIDO";
            }

            if (s_Tipo.Equals("REVALIDADO"))
            {
                lblTexto_3.Text = "Fecha Revalidación:"; 
            }
            else
            {
                lblTexto_3.Text = "Fecha Expedición:";
            }

            if (e.CommandName == "Consultar")
            {
                strScript += Util.DeshabilitarTab(2, "tabs", true);
                strScript += Util.ActivarTab(1, "Ver");

                HabilitarControlesFormato(false);
                CargarDatosActoMigratorio();
                Comun.EjecutarScript(Page, strScript);
                div_apr.Attributes.Add("style", "display:none;");

                toolbarModificar.btnGrabar.Enabled = false;

               
                txtOtros.Enabled = false;
                txtNumSal_Lam.Enabled = false;
                txtNumeroPass.Enabled = false;
                txtFecExpedicion.EnabledText = false;
                txtFecExpiracion.EnabledText = false;

            }
            else if (e.CommandName == "Editar")
            {
                txtNumeroPass.Enabled = false;
                txtOtros.Enabled = false;
                hId_AprobadoEstado.Value = "";
                string s_Estado = Convert.ToString(gdvMigratorio.Rows[intSeleccionado].Cells[8].Text);

                hId_Estado.Value = Convert.ToString(gdvMigratorio.Rows[intSeleccionado].Cells[15].Text);

                hdn_Estado.Value = s_Estado;
                strScript += Util.DeshabilitarTab(2, "tabs", true);
                strScript += Util.HabilitarTab(1);

                CargarDatosActoMigratorio();
                Load_Imagenes();
                Comun.EjecutarScript(Page, strScript);
                div_apr.Attributes.Remove("style");

                btnAgregarObservacion.Enabled = true;
                toolbarModificar.btnGrabar.Enabled = true;
                
                txtNumSal_Lam.Enabled = false; 
                txtFecExpedicion.EnabledText = false;
                txtFecExpiracion.EnabledText = false;

            }
            else if (e.CommandName == "Anular")
            {
                strScript += Util.DeshabilitarTab(1, "tabs", true);
                strScript += Util.ActivarTab(2, "Anular");
                Comun.EjecutarScript(Page, strScript);
                Comun.EjecutarScript(Page, strScript);
            }
            else if (e.CommandName == "DarBaja")
            {
                toolbarAnular.btnGrabar.Enabled = true;
                strScript += Util.DeshabilitarTab(1, "tabs", true);
                strScript += Util.ActivarTab(2, "Dar Baja");
                Comun.EjecutarScript(Page, strScript);
            }
            else if (e.CommandName == "Detalle")
            {
                lblTituloBaja.Visible = true;
                Load_Detalle_Baja(Comun.ToNullInt64(hdn_acmi_iActoMigratorioId.Value));
                divDetalle.Attributes.Remove("style");
            }
            
        }

        protected void gdvVisaMigratorio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string strScript = string.Empty;
            int intSeleccionado = Convert.ToInt32(e.CommandArgument);

            string s_Estado = gdvVisaMigratorio.Rows[intSeleccionado].Cells[7].Text.ToUpper();

            hdn_acmi_iActuacionDetalleId.Value = Convert.ToInt64(gdvVisaMigratorio.Rows[intSeleccionado].Cells[0].Text).ToString();
            hdn_acmi_iActoMigratorioId.Value = Convert.ToInt64(gdvVisaMigratorio.Rows[intSeleccionado].Cells[1].Text).ToString();
            hdn_pers_iPersonaId.Value = Convert.ToInt64(gdvVisaMigratorio.Rows[intSeleccionado].Cells[2].Text).ToString();            

            if (e.CommandName == "Consultar")
            {
                strScript += Util.DeshabilitarTab(2, "tabs", true);
                strScript += Util.ActivarTab(1, "Ver");

                HabilitarControlesFormato(false);
                CargarDatosActoMigratorio();

                Comun.EjecutarScript(Page, strScript);
                div_apr.Attributes.Add("style", "Display:none;");
                toolbarModificar.btnGrabar.Enabled = false;
                
                txtOtros.Enabled = false;
                txtNumSal_Lam.Enabled = false;
                txtNumeroPass.Enabled = false;
                txtFecExpedicion.EnabledText = false;
                txtFecExpiracion.EnabledText = false;
            }
            else if (e.CommandName == "Editar")
            {
                strScript += Util.DeshabilitarTab(2, "tabs", true);
                strScript += Util.ActivarTab(1, "Modificación");

                CargarDatosActoMigratorio();
                
                div_apr.Attributes.Add("style", "Display:none;");
                Comun.EjecutarScript(Page, strScript);

                toolbarModificar.btnGrabar.Enabled = true;
                
                txtOtros.Enabled = true;
                txtNumSal_Lam.Enabled = false;
                txtNumeroPass.Enabled = false;
                txtNumLamina.Enabled = false; 
                txtFecExpedicion.EnabledText = true;
                txtFecExpiracion.EnabledText = true;
            }
            else if (e.CommandName == "DarBaja")
            {
                toolbarAnular.btnGrabar.Enabled = true;
                lblTituloBaja.Visible = true;
                CargarDatosActoMigratorio_Baja();
                Load_Detalle_Baja(Comun.ToNullInt64(hdn_acmi_iActoMigratorioId.Value));
                divDetalle.Attributes.Remove("style");
            }
            else if (e.CommandName == "Cambiar")
            {
                int intEstadoId = Comun.ToNullInt16(gdvVisaMigratorio.Rows[intSeleccionado].Cells[18].Text);
                if (intEstadoId == (int)Enumerador.enmEstadoVisa.REGISTRADO)
                {
                    Comun.EjecutarScript(this,  Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Consulta Migratorio", "El trámite se encuentra " + Enumerador.enmEstadoVisa.REGISTRADO.ToString()));
                    return;
                }
                else if (intEstadoId == (int)Enumerador.enmEstadoTraminte.RECHAZADO)
                {
                    Comun.EjecutarScript(this,  Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Consulta Migratorio","El trámite se encuentra " + Enumerador.enmEstadoTraminte.RECHAZADO.ToString()));
                    return;
                }
                else if (intEstadoId == (int)Enumerador.enmEstadoTraminte.CANCELADO)
                {
                    Comun.EjecutarScript(this,  Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Consulta Migratorio","El trámite se encuentra " + Enumerador.enmEstadoTraminte.CANCELADO.ToString()));
                    return;
                }
                else
                {
                    switch (intEstadoId)
                    {
                        case (int)Enumerador.enmEstadoVisa.SOLICITADO:
                        case (int)Enumerador.enmEstadoVisa.APROBADO:
                            Comun.EjecutarScript(this, "showModalPopup('../Registro/FrmCambiaEstadoMigratorio.aspx?ID=" +
                                Comun.ToNullInt64(gdvVisaMigratorio.Rows[intSeleccionado].Cells[1].Text).ToString() +
                                "&ESTADO=" + Comun.ToNullInt16(gdvVisaMigratorio.Rows[intSeleccionado].Cells[18].Text) +
                                "','MOTIVO/RECHAZO-CANCELACIÓN',250, 630, '" + btn_Consultar_Evento.ClientID + "');");
                            break;
                    }
                    
                    
                    return;
                }
            }
            else if (e.CommandName == "Aprobado")
            {

                Comun.EjecutarScript(this,  Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Consulta Migratorio", "El trámite ya se encuentra " + s_Estado));
                return;
            }            
        }

        void Load_Imagenes()
        {
            int IntTotalCount = 0;
            int IntTotalPages = 0;

            var load_Imagen = new SGAC.Registro.Actuacion.BL.ActuacionAdjuntoConsultaBL().ActuacionAdjuntosObtener(
                Comun.ToNullInt64(hdn_acmi_iActuacionDetalleId.Value), "1",
                Constantes.CONST_PAGE_SIZE_ADJUNTOS, ref IntTotalCount, ref IntTotalPages);

            DataTable Ordenado = null;
            try
            {
                Ordenado = (from dr in load_Imagen.AsEnumerable()
                            orderby dr["acad_dFechaCreacion"]
                            select dr).CopyToDataTable();
            }
            catch
            {
                Ordenado = new DataTable();
            }

            string i_Imagen = "imagen-no-disponible.jpg";

            imgFirma.ImageUrl = string.Format("~/LoadImagen.ashx?vClass={0}", i_Imagen);
            imgFoto.ImageUrl = string.Format("~/LoadImagen.ashx?vClass={0}", i_Imagen);
            imgHuella.ImageUrl = string.Format("~/LoadImagen.ashx?vClass={0}", i_Imagen);

            String uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"];

            foreach (DataRow row in Ordenado.Rows)
            {
                switch (Comun.ToNullInt32(row["sAdjuntoTipoId"]))
                {
                    case (Int32)Enumerador.enmTipoAdjunto.FOTO:
                        imgFoto.ImageUrl = string.Format("~/LoadImagen.ashx?vClass={0}", Convert.ToString(row["vNombreArchivo"]));
                        break;
                    case (Int32)Enumerador.enmTipoAdjunto.FIRMA:
                        imgFirma.ImageUrl = string.Format("~/LoadImagen.ashx?vClass={0}", Convert.ToString(row["vNombreArchivo"]));
                        break;
                    case (Int32)Enumerador.enmTipoAdjunto.HUELLA:
                        imgHuella.ImageUrl = string.Format("~/LoadImagen.ashx?vClass={0}", Convert.ToString(row["vNombreArchivo"]));
                        break;
                }
            }
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            ctrlPaginador.InicializarPaginador();

            divDetalle.Attributes.Add("style", "Display:none;");

            if (ddlTipoDocConsulta.SelectedValue.Equals("0"))
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_TIPO_ACTO, true, Enumerador.enmTipoMensaje.ERROR);
                ddlTipoDocConsulta.Focus();
                return;
            }

            // Validaciones            
            DateTime datFechaExpedicion = txtFechaExpedicion.Value();
            DateTime datFechaExpiracion = txtFechaExpiracion.Value();
            if (datFechaExpedicion != DateTime.MinValue && datFechaExpiracion != DateTime.MinValue)
            {
                if (datFechaExpedicion > datFechaExpiracion)
                {
                    gdvMigratorio.DataSource = new DataTable();
                    gdvMigratorio.DataBind();

                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
                    return;
                }
            }
            btnAgregarObservacion.Enabled = true;
            DateTime datFechaInicio = txtFecInicio.Value();
            DateTime datFechaFin = txtFecFin.Value();
            if (datFechaInicio != DateTime.MinValue && datFechaFin != DateTime.MinValue)
            {
                if (datFechaExpedicion > datFechaExpiracion)
                {                    
                    gdvMigratorio.DataSource = new DataTable();
                    gdvMigratorio.DataBind();

                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_DOS_FECHAS, true, Enumerador.enmTipoMensaje.ERROR);
                    return;
                }
            }
            // Fin Validaciones
            CargarGrilla();

            tipoInsumo.Items.Clear();

            switch (Comun.ToNullInt64(ddlTipoDocConsulta.SelectedItem.Value))
            {
                case (Int64)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                    Cargar_Tipo_Pasaporte();
                    break;
                case (Int64)Enumerador.enmDocumentoMigratorio.VISAS:
                    Cargar_Tipo_Visa();
                    break;
                case (Int64)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                    Cargar_Tipo_Salvoconducto();
                    break;
            }

            if (Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString() ==
                        ((int)Constantes.CONST_OFICINACONSULAR_LIMA).ToString())
            {
                gdvMigratorio.Columns[11].Visible = true;
            }
            else
            {
                gdvMigratorio.Columns[11].Visible = false;
            }

        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            ddlTipoDocConsulta.SelectedIndex = 0;
            ddlTipoDocConsulta_SelectedIndexChanged(null, EventArgs.Empty);
        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            CargarGrilla();            
        }

        protected void btn_Consultar_Evento_Click(object sender, EventArgs e)
        {
            ctrlToolBarConsulta_btnBuscarHandler();
        }

        protected void ddlVisaTipoC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddlVisaTipoC.SelectedValue) == 0)
            {
                ddlSubTipo.Items.Clear();
                ddlSubTipo.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));

                ddlTitularFamiliar.SelectedIndex = -1;
            }
            else
            {
                if (Convert.ToInt32(ddlVisaTipoC.SelectedValue) == (int)Enumerador.enmMigratorioVisaTipo.RESIDENTE)
                {
                    Util.CargarParametroDropDownList(ddlSubTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_MIGRA_VISA_TIPO_RESIDENTE), true);
                }
                else
                {
                    Util.CargarParametroDropDownList(ddlSubTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_MIGRA_VISA_TIPO_TEMPORAL), true);
                }
            }
        }        

        protected void ddlTipoDocConsulta_SelectedIndexChanged(object sender, EventArgs e)
        {
            gdvMigratorio.DataSource = new DataTable();
            gdvMigratorio.DataBind();
            gdvVisaMigratorio.DataSource = new DataTable();
            gdvVisaMigratorio.DataBind();
            ctrlPaginador.Visible = false;
            divDetalle.Attributes.Add("style", "display:none;");
            if (ddlTipoDocConsulta.SelectedIndex > 0)
            {
                int intTipoDocMigratorio = Convert.ToInt32(ddlTipoDocConsulta.SelectedValue);                
                switch (intTipoDocMigratorio)
                {
                    case (int)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                        PantallaPasaporte();
                        break;
                    case (int)Enumerador.enmDocumentoMigratorio.VISAS:
                        PantallaVisa();
                        break;
                    case (int)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                        PantallaSalvoConducto();
                        break;
                }
            }

            Limpiar_Pestana();
        }

        void Limpiar_Pestana()
        {
            ddlSubTipoPasaporte.SelectedValue = "0";
            txtNumDocumento.Text = "";
            txtNumExpediente.Text = "";
            ddlVisaPaisResidenciaC.SelectedIndex = 0;
            if (ddlVisaTipoC.SelectedIndex != -1)
                ddlVisaTipoC.SelectedIndex = 0;

            if (ddlSubTipo.SelectedIndex != -1)
                ddlSubTipo.SelectedIndex = 0;
            if (ddlVisaEtiquetaEstado.SelectedIndex != -1)
                ddlVisaEtiquetaEstado.SelectedIndex = 0;
            if (ddlEstadoDoc.SelectedIndex != -1)
                ddlEstadoDoc.SelectedIndex = 0;
            txtPrimerApellido.Text = "";
            txtSegundoApellido.Text = "";
            txtNombre.Text = "";
            txtFechaExpedicion.Text = "";
            txtFechaExpiracion.Text = "";
            txtFecInicio.Text = "";
            txtFecFin.Text = "";

        }
        #endregion

        #region Layout Consulta

        private void PantallaPasaporte()
        {
            lblSubTipoPasaporte.Visible = true;
            ddlSubTipoPasaporte.Visible = true;
            lblVisaPaisResidenciaC.Visible = false;
            ddlVisaPaisResidenciaC.Visible = false;

            lblTexto_2.Visible = true;
            txtNumSal_Lam.Visible = true;

            lblTexto_2.Text = "Nro. Lámina:";
            lblTexto_3.Text = "Fecha Revalidación:";
            lblTexto_4.Text = "Fecha Expiración:";

            lblTexto_5.Text = "Nro. Pasaporte:";
            lblTexto_6.Visible = true;
            lblTexto_7.Text = "DATOS DE PASAPORTE / LÁMINA DADO DE BAJA";
            lblTexto_8.Text = "Motivo de Baja del Pasaporte:";

            

            lblTexto_9.Text = "Listado de Pasaportes";

            txtCorreActuacion.Visible = true;
            lblLamina.Visible = true;
            txtNumLamina.Visible = true;

            tbAnular.Width = "700";

            pnlVisaBaja.Visible = false;
            pnlTipoVisa.Visible = false;

            Util.CargarParametroDropDownList(ddlSubTipoPasaporte, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_MIGRA_PASAPORTE_TIPO), true);
            Util.CargarParametroDropDownList(ddlEstadoDoc, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.PASAPORTE), true);

            gdvMigratorio.Visible = true;
            gdvVisaMigratorio.Visible = false;
            ddlSubTipoPasaporte.SelectedValue = "0";

            divDetalle.Attributes.Add("style", "Display:none;");
            //UpnlBaja.Update();
        }
        private void PantallaVisa()
        {
            //TAB : CONSULTA
            Util.CargarParametroDropDownList(ddlVisaTipoC, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_MIGRA_VISA_TIPO), true);
            Util.CargarParametroDropDownList(ddlEstadoDoc, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.VISA_TRAMITE), true);
            Util.CargarParametroDropDownList(ddlVisaEtiquetaEstado, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.VISA), true);            

            //TAB : MODIFICACIÓN/VER
            lblSubTipoPasaporte.Visible = false;
            ddlSubTipoPasaporte.Visible = false;
            lblVisaPaisResidenciaC.Visible = true;
            ddlVisaPaisResidenciaC.Visible = true;

            lblTexto_1.Text = "Nro. Etiqueta";
            lblTexto_2.Visible = true;
            txtNumSal_Lam.Visible = true;

            lblTexto_2.Text = "Nro. Lámina:";
            lblTexto_3.Text = "Fecha Expedición:";
            lblTexto_4.Text = "Fecha Expiración:";

            lblTexto_5.Text = "Nro. Visa:";
            lblTexto_6.Visible = true;
            lblTexto_7.Text = "DATOS DE VISA / LÁMINA DADO DE BAJA";
            lblTexto_8.Text = "Motivo de Baja de la Visa:";
            lblTexto_9.Text = "Listado de Visas";

            txtCorreActuacion.Visible = true;
            lblLamina.Visible = true;
            txtNumLamina.Visible = true;
            
            pnl_Aprobaciones.Visible = false;

            // TAB : ANULACIÓN
            tbAnular.Width = "600";
            
            pnlVisaBaja.Visible = true;
            pnlTipoVisa.Visible = true;

            gdvMigratorio.Visible = false;
            gdvVisaMigratorio.Visible = true;
            ddlSubTipoPasaporte.SelectedValue = "0";
            //UpnlBaja.Update();
            divDetalle.Attributes.Add("style", "Display:none;");
        }
        private void PantallaSalvoConducto()
        {
            lblSubTipoPasaporte.Visible = false;
            ddlSubTipoPasaporte.Visible = false;
            lblVisaPaisResidenciaC.Visible = false;
            ddlVisaPaisResidenciaC.Visible = false;

            lblTexto_1.Text = "Nro. Pasaporte";
            lblTexto_2.Text = "Nro. Lámina:";
            lblTexto_3.Text = "Fecha Expedición:";
            lblTexto_4.Text = "Fecha Expiración:";
            lblTexto_5.Text = "Nro. Salvoconducto:";
            lblTexto_6.Visible = true;
            lblTexto_7.Text = "DATOS DE SALVOCONDUCTO / LÁMINA DADO DE BAJA";
            lblTexto_8.Text = "Motivo de Baja del Salvoconducto:";
            lblTexto_9.Text = "Listado de Salvoconductos";

            txtCorreActuacion.Visible = true;
            lblLamina.Visible = true;
            txtNumLamina.Visible = true;
            tbAnular.Width = "700";

            Util.CargarParametroDropDownList(ddlEstadoDoc, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.SALVOCONDUCTO), true);

            pnlVisaBaja.Visible = false;
            pnlTipoVisa.Visible = false;

            gdvMigratorio.Visible = true;
            gdvVisaMigratorio.Visible = false;
            ddlSubTipoPasaporte.SelectedValue = "0";
            //UpnlBaja.Update();
            divDetalle.Attributes.Add("style", "Display:none;");
        }

        private void LayoutConsultaPasaporte()
        {
            // HABILITACION DE CONTROLES
            lblVisaPaisResidenciaC.Visible = false;
            ddlVisaPaisResidenciaC.Visible = false;
            pnlTipoVisa.Visible = false;

            // CARGA DE DATOS
            ddlEstadoDoc.SelectedValue = ((int)Enumerador.enmDocumentoMigratorio.PASAPORTE).ToString();
            Util.CargarParametroDropDownList(ddlSubTipoPasaporte, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_MIGRA_PASAPORTE_TIPO), true);
            Util.CargarParametroDropDownList(ddlEstadoDoc, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmEstadoGrupo.PASAPORTE), true, "- TODOS -");
        }
        private void LayoutVerPasaporte()
        {

        }
        private void LayoutAnularPasaporte()
        {

        }

        private void LayoutConsultaSalvoconducto()
        {
            // HABILITACIÓN DE CONTROLES            
            lblVisaPaisResidenciaC.Visible = false;
            ddlVisaPaisResidenciaC.Visible = false;
            pnlTipoVisa.Visible = false;

            // CARGA DE DATOS
            ddlEstadoDoc.SelectedValue = ((int)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO).ToString();
        }
        private void LayoutVerSalconducto()
        {

        }
        private void LayoutAnularSalvoconducto()
        {

        }

        private void LayoutConsultaVisa()
        {

        }
        private void LayoutVerVisa()
        {

        }

        #endregion Layout Consulta

        #region Métodos
        private void CargarListadosDesplegables()
        {
            // TAB: CONSULTAR
            ddlOficinaConsular.Cargar();
            Util.CargarParametroDropDownList(ddlTipoDocConsulta, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTO_MIGRATORIO_DOCUMENTOS), true, "- SELECCIONAR -");
            Util.CargarParametroDropDownList(ddlSubTipoPasaporte, new DataTable(), true, "- TODOS -");
            Util.CargarParametroDropDownList(ddlEstadoDoc, new DataTable(), true, "- TODOS -");

            // TAB: MODIFICAR

            Util.CargarParametroDropDownList(ddlGenero, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_GENERO), true);
            Util.CargarParametroDropDownList(ddlEstadoCivil, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.ESTADO_CIVIL), true);
            Util.CargarParametroDropDownList(ddlOcupacionAct, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.OCUPACION), true);
            Util.CargarParametroDropDownList(ddlColorOjo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_COLOR_OJOS), true);
            Util.CargarParametroDropDownList(ddlColorCabello, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_COLOR_CABELLO), true);

            comun_Part3.CargarUbigeo(Session, ddlDomicilioNacDepa, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, string.Empty, string.Empty, true);
            comun_Part3.CargarUbigeo(Session, ddlDomicilioNacProv, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, string.Empty, string.Empty, true);
            comun_Part3.CargarUbigeo(Session, ddlDomicilioNacDist, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);

            comun_Part3.CargarUbigeo(Session, ddlDomicilioPeruDepa, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, string.Empty, string.Empty, true);
            comun_Part3.CargarUbigeo(Session, ddlDomicilioPeruProv, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, string.Empty, string.Empty, true);
            comun_Part3.CargarUbigeo(Session, ddlDomicilioPeruDist, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);

            comun_Part3.CargarUbigeo(Session, ddlDomicilioExtrDepa, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, string.Empty, string.Empty, true, Enumerador.enmNacionalidad.EXTRANJERA);
            comun_Part3.CargarUbigeo(Session, ddlDomicilioExtrProv, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, string.Empty, string.Empty, true);
            comun_Part3.CargarUbigeo(Session, ddlDomicilioExtrDist, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);

            Util.CargarParametroDropDownList(ddlTitularFiliacionPadre, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_TIPO_VINCULO), true);
            Util.CargarParametroDropDownList(ddlTitularFiliacionMadre, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_TIPO_VINCULO), true);

            // TAB: DE BAJA
            Util.CargarParametroDropDownList(ddlVisaTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_MIGRA_VISA_TIPO), true);
            
            // TAB: ANULAR
            Util.CargarDropDownList(ddlDatoTipoDocumento, comun_Part1.ObtenerDocumentoIdentidad(), "Valor", "Id", true);

            
            this.ddlDatoTipoDocumento.Items.Add(new System.Web.UI.WebControls.ListItem(Convert.ToString(Constantes.CONST_EXCEPCION_CUI), Convert.ToString(Constantes.CONST_EXCEPCION_CUI_ID)));
            Util.CargarParametroDropDownList(ddlDatoGenero, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_GENERO), true);
        }
        private void CargarDatosIniciales()
        {
            ddlTitularFiliacionPadre.SelectedValue = ((int)Enumerador.enmVinculo.PADRE).ToString();
            ddlTitularFiliacionMadre.SelectedValue = ((int)Enumerador.enmVinculo.MADRE).ToString();
        }
        private void CargarGrilla()
        {
            #region Filtros
            int? intOficinaConsularId = 0;
            int intTotalRegistros = 0, intTotalPaginas = 0;

            BE.MRE.RE_ACTOMIGRATORIO objActoMigratorioBE = new BE.MRE.RE_ACTOMIGRATORIO();
            BE.MRE.RE_PERSONA objPersonaBE = new BE.MRE.RE_PERSONA();

            if (ddlOficinaConsular.SelectedIndex > 0)
                intOficinaConsularId = Convert.ToInt32(ddlOficinaConsular.SelectedValue);
            else
                intOficinaConsularId = null;

            if (ddlTipoDocConsulta.SelectedIndex > 0) objActoMigratorioBE.acmi_sTipoDocumentoMigratorioId = Convert.ToInt16(ddlTipoDocConsulta.SelectedValue);

            if (ddlSubTipoPasaporte.SelectedIndex > 0) objActoMigratorioBE.acmi_sTipoId = Convert.ToInt16(ddlSubTipoPasaporte.SelectedValue);
            if (ddlSubTipo.SelectedIndex > 0) objActoMigratorioBE.acmi_sSubTipoId = Convert.ToInt16(ddlSubTipo.SelectedValue); // PASAPORTE
            if (ddlVisaTipoC.SelectedIndex > 0) objActoMigratorioBE.acmi_sTipoId = Convert.ToInt16(ddlVisaTipoC.SelectedValue); // VISAS            

            if (Comun.ToNullInt16(ddlTipoDocConsulta.SelectedItem.Value) == (Int16)Enumerador.enmDocumentoMigratorio.VISAS)
            {
                if (ddlEstadoDoc.SelectedIndex > 0)
                {
                    if (Comun.ToNullInt32(ddlEstadoDoc.SelectedItem.Value) == 52)
                        objActoMigratorioBE.acmi_sEstadoId = (Int16)Enumerador.enmEstadoVisa.APROBADO;
                    else
                        objActoMigratorioBE.acmi_sEstadoId = Convert.ToInt16(ddlEstadoDoc.SelectedValue);
                }
            }else
                if (ddlEstadoDoc.SelectedIndex > 0) objActoMigratorioBE.acmi_sEstadoId = Convert.ToInt16(ddlEstadoDoc.SelectedValue);

            if (txtNumDocumento.Text.Trim() != string.Empty) objActoMigratorioBE.acmi_vNumeroDocumento = txtNumDocumento.Text.Trim();
            if (txtNumExpediente.Text.Trim() != string.Empty) objActoMigratorioBE.acmi_vNumeroExpediente = txtNumExpediente.Text.Trim();
            if (txtFechaExpedicion.Value() != DateTime.MinValue) objActoMigratorioBE.acmi_dFechaExpedicion = txtFechaExpedicion.Value();
            if (txtFechaExpiracion.Value() != DateTime.MinValue) objActoMigratorioBE.acmi_dFechaExpiracion = txtFechaExpiracion.Value();
            if (txtFecInicio.Value() != DateTime.MinValue) objActoMigratorioBE.acmi_dFechaCreacion = txtFecInicio.Value();
            if (txtFecFin.Value() != DateTime.MinValue) objActoMigratorioBE.acmi_dFechaModificacion = txtFecFin.Value();

            if (txtPrimerApellido.Text.Trim() != string.Empty) objPersonaBE.pers_vApellidoPaterno = txtPrimerApellido.Text.Trim();
            if (txtSegundoApellido.Text.Trim() != string.Empty) objPersonaBE.pers_vApellidoMaterno = txtSegundoApellido.Text.Trim();
            if (txtNombre.Text.Trim() != string.Empty) objPersonaBE.pers_vNombres = txtNombre.Text.Trim();
            #endregion

            #region Consulta
            SGAC.Registro.Actuacion.BL.ActoMigratorioConsultaBL objBL = new SGAC.Registro.Actuacion.BL.ActoMigratorioConsultaBL();
            DataTable dt = new DataTable();

            dt = objBL.ConsultarDocumentosMigratorios(objActoMigratorioBE, objPersonaBE, intOficinaConsularId,
                ctrlPaginador.PaginaActual, Constantes.CONST_CANT_REGISTRO, ref intTotalRegistros, ref intTotalPaginas);

            if (ddlTipoDocConsulta.SelectedIndex > 0)
            {
                if (Convert.ToInt32(ddlTipoDocConsulta.SelectedValue) == (int)Enumerador.enmDocumentoMigratorio.VISAS)
                {
                    gdvVisaMigratorio.DataSource = dt;
                    gdvVisaMigratorio.DataBind();
                }
                else
                {
                    gdvMigratorio.DataSource = dt;
                    gdvMigratorio.DataBind();
                }
            }
            else
            {
                gdvMigratorio.DataSource = dt;
                gdvMigratorio.DataBind();
            }
            #endregion

            // Validación y Paginador
            
            if (dt.Rows.Count == 0)
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
            else
                ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + intTotalRegistros, true, Enumerador.enmTipoMensaje.INFORMATION);

            ctrlPaginador.TotalResgistros = intTotalRegistros;
            ctrlPaginador.TotalPaginas = intTotalPaginas;
            ctrlPaginador.Visible = false;
            if (ctrlPaginador.TotalPaginas > 1) ctrlPaginador.Visible = true;
        }

        private void CargarFuncionarios(int sOfConsularId, int IFuncionarioId, DropDownList ddlFuncionario)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = funcionario.dtFuncionario(sOfConsularId, IFuncionarioId);

                ddlFuncionario.Items.Clear();
                ddlFuncionario.DataTextField = "vFuncionario";
                ddlFuncionario.DataValueField = "IFuncionarioId";
                ddlFuncionario.DataSource = dt;
                ddlFuncionario.DataBind();
                ddlFuncionario.Items.Insert(0, new ListItem("- SELECCIONAR -", "0"));
                 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CargarSeguimientoActuacion(long lngActuacionDetalleId)
        {
            if (lngActuacionDetalleId != 0)
            {
                
                DataTable dtSeguimiento = new SGAC.Registro.Actuacion.BL.ActuacionConsultaBL().ObtenerSeguimientoActuacion(lngActuacionDetalleId);
                
            Regresar:
                foreach (DataRow row in dtSeguimiento.Rows)
                {
                    if (!Convert.ToString(row["amhi_sInsumoId"]).Equals(""))
                    {
                        dtSeguimiento.Rows.Remove(row);
                        goto Regresar;
                    }
                }
                Session["dtSeguimiento_Consulta"] = dtSeguimiento;
                gdvSeguimiento.DataSource = dtSeguimiento;
                gdvSeguimiento.DataBind();
                pnl_Aprobaciones.Visible = true;
                
            }
            else
            {
                gdvSeguimiento.DataSource = null;
                gdvSeguimiento.DataBind();
            }
        }

        private void CargarDatosActoMigratorio()
        {
            switch (Comun.ToNullInt32(ddlTipoDocConsulta.SelectedItem.Value))
            {
                case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                    var s_TipoDocumento = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTO_MIGRATORIO_TIPO_NRO_PASAPORTE);

                    Util.CargarParametroDropDownList(ddlTipoDocumento, s_TipoDocumento, true);
                    break;
                default:
                    Util.CargarDropDownList(ddlTipoDocumento, comun_Part1.ObtenerDocumentoIdentidad(), "Valor", "Id", true);
                    break;
            }



            this.ddlTipoDocumento.Items.Add(new System.Web.UI.WebControls.ListItem(Convert.ToString(Constantes.CONST_EXCEPCION_CUI), Convert.ToString(Constantes.CONST_EXCEPCION_CUI_ID)));

            var existe_registro = new SGAC.Registro.Actuacion.BL.ActoMigratorioConsultaBL().Consultar_Acto_Migratorio(
                     Comun.ToNullInt64(hdn_pers_iPersonaId.Value),
                    Comun.ToNullInt64(hdn_acmi_iActuacionDetalleId.Value));

            #region Validar personas
            // Validación
            if (existe_registro.PERSONA.pers_iPersonaId == 0)
            {
                string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Consulta Migratorio", "No se encontraron datos del titular en el documento migratorio seleccionado.");
                Comun.EjecutarScript(Page, strScript);
                return;
            }

            /*Llenando los datos de persona*/
            var obj_Persona = existe_registro.PERSONA;
            #region - Persona -
            txt_numero_doc.Text = obj_Persona.Identificacion.peid_vDocumentoNumero;
            ddlGenero.SelectedValue = obj_Persona.pers_sGeneroId.ToString();
            if (ddlGenero.SelectedIndex > 0) ddlGenero.Enabled = false;
            ddlEstadoCivil.SelectedValue = obj_Persona.pers_sEstadoCivilId.ToString();
            ddlOcupacionAct.SelectedValue = obj_Persona.pers_sOcupacionId.ToString();
            
            txtApePat.Text = obj_Persona.pers_vApellidoPaterno;
            txtApeMat.Text = obj_Persona.pers_vApellidoMaterno;
            txtNombres.Text = obj_Persona.pers_vNombres;
            if (obj_Persona.pers_dNacimientoFecha != DateTime.MinValue)
                txtFecNacimiento.Text = obj_Persona.pers_dNacimientoFecha.ToString("MMM-dd-yyyy");
            else
                txtFecNacimiento.Text = "";
            ddlColorOjo.SelectedValue = obj_Persona.pers_sColorOjosId.ToString();
            ddlColorCabello.SelectedValue = obj_Persona.pers_sColorCabelloId.ToString();
            hdn_pers_iPersonaId.Value = obj_Persona.pers_iPersonaId.ToString();
            txtEstatura.Text = obj_Persona.pers_vEstatura;
            #region - filiaciones-

            ddlTitularFiliacionPadre.SelectedValue = ((int)Enumerador.enmVinculo.PADRE).ToString();
            ddlTitularFiliacionMadre.SelectedValue = ((int)Enumerador.enmVinculo.MADRE).ToString();

            hId_Padre.Value = "0";
            hId_Madre.Value = "0";

            if (obj_Persona.FILIACIONES.Count > 0)
            {
                txtPadreNombres.Text = obj_Persona.FILIACIONES[0].pefi_vNombreFiliacion;
                hId_Padre.Value = Convert.ToString(obj_Persona.FILIACIONES[0].pefi_iPersonaFilacionId);

                if (obj_Persona.FILIACIONES.Count == 2)
                {
                    txtMadreNombres.Text = obj_Persona.FILIACIONES[1].pefi_vNombreFiliacion;
                    hId_Madre.Value = Convert.ToString(obj_Persona.FILIACIONES[1].pefi_iPersonaFilacionId);
                }
            }

            #endregion
            //txtDomicilioNac.Text = obj_Persona.pers_vLugarNacimiento;
            //txtTelefonoNac.Text = obj_Persona.RESIDENCIAS
            if (obj_Persona.pers_cNacimientoLugar != string.Empty)
            {
                SetearUbigeo(obj_Persona.pers_cNacimientoLugar,
                    ddlDomicilioNacDepa, ddlDomicilioNacProv, ddlDomicilioNacDist);
            }
            #endregion

            #region - Residencias -
            hdn_resi_iResidenciaId_extranjero.Value = "0";
            hdn_resi_iResidenciaId_peru.Value = "0";
            foreach (BE.RE_RESIDENCIA oRE_RESIDENCIA in obj_Persona.RESIDENCIAS)
            {
                if (oRE_RESIDENCIA.resi_cEstado.Equals("E"))
                {
                    if (oRE_RESIDENCIA.resi_cResidenciaUbigeo != string.Empty)
                    {
                        SetearUbigeo(oRE_RESIDENCIA.resi_cResidenciaUbigeo,
                            ddlDomicilioExtrDepa, ddlDomicilioExtrProv, ddlDomicilioExtrDist);
                    }
                    
                    hdn_resi_iResidenciaId_extranjero.Value = Convert.ToString(oRE_RESIDENCIA.resi_iResidenciaId);
                    txtDomicilioExtra.Text = oRE_RESIDENCIA.resi_vResidenciaDireccion;
                    txtTelefonoExtr.Text = oRE_RESIDENCIA.resi_vResidenciaTelefono;
                }

                if (oRE_RESIDENCIA.resi_cEstado.Equals("P"))
                {
                    if (oRE_RESIDENCIA.resi_cResidenciaUbigeo != string.Empty)
                    {
                        SetearUbigeo(oRE_RESIDENCIA.resi_cResidenciaUbigeo,
                            ddlDomicilioPeruDepa, ddlDomicilioPeruProv, ddlDomicilioPeruDist);
                    }
                    
                    hdn_resi_iResidenciaId_peru.Value = Convert.ToString(oRE_RESIDENCIA.resi_iResidenciaId);
                    txtDomicilioPeru.Text = oRE_RESIDENCIA.resi_vResidenciaDireccion;
                    txtTelefonoPeru.Text = oRE_RESIDENCIA.resi_vResidenciaTelefono;
                }
            }

            CargarSeguimientoActuacion(existe_registro.ACTO.acmi_iActuacionDetalleId);
            #endregion
            #endregion

            #region Acto Migratorio
            // Validación
            if (existe_registro.ACTO.acmi_iActoMigratorioId == 0)
            {
                string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Consulta Migratorio", "No se encontraron datos en el documento migratorio seleccionado.");
                Comun.EjecutarScript(Page, strScript);
                return;
            }

            /*Llenado los datos de migratorio*/

            var obj_Migratorio = existe_registro.ACTO;
            txtNumeroExp.Text = obj_Migratorio.acmi_vNumeroExpediente;
            if (obj_Migratorio.acmi_dFechaExpedicion == DateTime.MinValue)
                txtFecExpedicion.Text = "";
            else
                txtFecExpedicion.Text = obj_Migratorio.acmi_dFechaExpedicion.ToString("MMM-dd-yyyy");

            if (obj_Migratorio.acmi_dFechaExpiracion == DateTime.MinValue)
                txtFecExpiracion.Text = "";
            else
                txtFecExpiracion.Text = obj_Migratorio.acmi_dFechaExpiracion.ToString("MMM-dd-yyyy");
            txtNumeroPass.Text = obj_Migratorio.acmi_vNumeroDocumento;
            txtNumSal_Lam.Text = obj_Migratorio.acmi_vNumeroLamina;

            CargarFuncionarios(obj_Migratorio.OficinaConsultar, 0, ddl_acmi_iFuncionariId);

            CargarFuncionarios(Comun.ToNullInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), 0, ddlFuncionario);

            hdn_acmi_iActoMigratorioId.Value = obj_Migratorio.acmi_iActoMigratorioId.ToString();
            ddl_acmi_iFuncionariId.SelectedValue = obj_Migratorio.acmi_IFuncionarioId.ToString();
            HabilitarControlesFormato(true);

            switch (Comun.ToNullInt32(ddlTipoDocConsulta.SelectedItem.Value))
            {
                case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                    ddlTipoDocumento.SelectedValue = obj_Persona.Identificacion.peid_sDocumentoTipoId.ToString();
                    txtOtros.Text = obj_Migratorio.acmi_vNumeroDocumentoAnterior;
                    Cambiar_Pantalla(true);
                    txtOtros.Visible = true;
                    lblNumeroSalvoconducto.Visible = true;
                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                    ddlTipoDocumento.SelectedValue = obj_Persona.Identificacion.peid_sDocumentoTipoId.ToString();
                    txtOtros.Text = obj_Migratorio.acmi_vNumeroDocumentoAnterior;
                    Cambiar_Pantalla(true);
                    txtOtros.Visible = false;
                    lblNumeroSalvoconducto.Visible = false;
                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                    var s_TipoDocumento = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTO_MIGRATORIO_TIPO_NRO_PASAPORTE);

                    Util.CargarParametroDropDownList(ddlTipoDocumento, s_TipoDocumento, true);
                    txtOtros.Text = "";
                    ddlVisaTipo.SelectedValue = obj_Migratorio.acmi_sTipoId.ToString();
                    ddlVisaTipo_SelectedIndexChanged(null, EventArgs.Empty);
                    ddlVisaSubTipo.SelectedValue = obj_Migratorio.acmi_sSubTipoId.ToString();

                    Cambiar_Pantalla(false);
                    txtOtros.Visible = false;
                    lblNumeroSalvoconducto.Visible = false;

                    lblTitularTipoDocumento.Text = "Tipo Pasaporte:";
                    lblTitularNumDocumento.Text = "Número Pasaporte:";

                    #region - cargando los datos del formato -
                    var obj_Formato = existe_registro.FORMATO;
                    if (obj_Formato != null)
                    {
                                
                        ddlTipoDocumento.SelectedValue = obj_Formato.amfr_sTipoNumeroPasaporteId;
                        txt_numero_doc.Text = obj_Formato.amfr_vNumeroPasaporte;

                        hiActoMigratorioFormatoId.Value = obj_Formato.amfr_iActoMigratorioFormatoId.ToString();
                    }
                    #endregion

                    break;
            }
            #endregion
            #region Imágenes

            Load_Imagenes();

            #endregion

            txtFechaAprobacion.Text = Comun.ObtenerFechaActualTexto(Session);
            if (txtFechaAprobacion.Text == string.Empty)
                txtFechaAprobacion.Enabled = true;
            else
                txtFechaAprobacion.Enabled = false;
        }

        void Cambiar_Pantalla(bool valor)
        {
            tr_Caracteristicas.Visible = valor;
            tr_Carac.Visible = valor;
            tr_Filia_.Visible = valor;
            tr_Filia_1.Visible = valor;
            tr_Filia_2.Visible = valor;
            tr_Filia_3.Visible = valor;
            tr_Filia_4.Visible = valor;
        }

        private void CargarDatosActoMigratorio_Baja()
        {
            txtFechaBaja.EnabledText = true;
            ddlMotivoAnulacion.Enabled = true;
            ddlDatoFuncionario.Enabled = true;
            txtObservaciones.Enabled = true;

            ActoMigratorioConsultaBL objBL = new ActoMigratorioConsultaBL();
            CBE_MIGRATORIO cbeMigratorio = objBL.Consultar_Acto_Migratorio(Convert.ToInt64(hdn_pers_iPersonaId.Value), Convert.ToInt64(hdn_acmi_iActuacionDetalleId.Value));

            #region Acto Migratorio
            if (cbeMigratorio.ACTO.acmi_iActoMigratorioId == 0)
            {
                string strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Consulta Migratorio", "No se encontraron datos en el documento migratorio seleccionado.");
                Comun.EjecutarScript(Page, strScript);
                return;
            }

            if (cbeMigratorio.ACTO != null)
            {
                //cambiando estos datos en base a los datos de la grilla
                DataTable dt_Verificar = null;
                try
                {
                    dt_Verificar = (from obj in ((DataTable)Session[obj_Detalle_Baja]).AsEnumerable()
                                    where Comun.ToNullInt64(obj["acmi_iActuacionDetalleId"]) == Comun.ToNullInt64(hdn_acmi_iActuacionDetalleId.Value)
                                    && Comun.ToNullInt64(obj["insu_sInsumoTipoId"]) == Comun.ToNullInt64(hinsu_sInsumoTipoId.Value)
                                    && Convert.ToString(obj["amhi_sEstadoId"]) != "20"
                                    select obj).CopyToDataTable();

                }
                catch
                {
                    dt_Verificar = new DataTable();
                }

                txtNumExpediernte.Enabled = false;
                txtNumExpediernte.Text = cbeMigratorio.ACTO.acmi_vNumeroExpediente;
                txtNumPasaporte.Text = cbeMigratorio.ACTO.acmi_vNumeroDocumento;

                txt_expedicion.Text = cbeMigratorio.ACTO.acmi_dFechaExpedicion.ToString("MMM-dd-yyyy");
                txt_Expiracion.Text = cbeMigratorio.ACTO.acmi_dFechaExpiracion.ToString("MMM-dd-yyyy");

                if (dt_Verificar.Rows.Count > 0)
                {

                    if (Comun.ToNullInt64(hinsu_sInsumoTipoId.Value) == 6053 || Comun.ToNullInt64(hinsu_sInsumoTipoId.Value) == 6060)
                    {
                        lblLamina.Text = "Nro. Librillo";
                        tipoInsumo.SelectedValue = "1";
                    }
                    else
                    {
                        tipoInsumo.SelectedValue = "2";
                    }
                    tipoInsumo.Enabled = false;
                    
                    txtNumLamina.Text = Convert.ToString(dt_Verificar.Rows[0]["vCodigoInsumo"]);
                }
                else
                {
                    txtNumLamina.Text = cbeMigratorio.ACTO.acmi_vNumeroLamina;
                }

                if (cbeMigratorio.FORMATO != null)
                    txtTiempoPermanencia.Text = cbeMigratorio.FORMATO.amfr_sDiasPermanencia.ToString();

                switch (Comun.ToNullInt32(ddlTipoDocConsulta.SelectedValue))
                {
                    case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                        ddlVisaTipo.SelectedValue = cbeMigratorio.ACTO.acmi_sTipoId.ToString();
                        Util.CargarParametroDropDownList(ddlTitularFamiliar, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_MIGRA_TITULAR_FAMILIA), true);

                        ddlVisaTipo_SelectedIndexChanged(null, EventArgs.Empty);
                        ddlTitularFamiliar.SelectedValue = cbeMigratorio.FORMATO.amfr_sTitularFamiliaId.ToString();
                        ddlVisaSubTipo.SelectedValue = cbeMigratorio.ACTO.acmi_sSubTipoId.ToString();
                        ddlVisaTipo.Enabled = false;
                        ddlVisaSubTipo.Enabled = false;
                        gdvDetalle.Columns[13].Visible = false;
                        break;
                    case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                        txtNumPasaporte.Text = cbeMigratorio.ACTO.acmi_vNumeroDocumentoAnterior;
                        gdvDetalle.Columns[13].Visible = true;
                        break;
                    case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                        gdvDetalle.Columns[13].Visible = true;
                        break;
                }

                CargarFuncionarios(cbeMigratorio.ACTO.OficinaConsultar, 0, ddlDatoFuncionario);
            }
            #endregion
            
            #region Persona
            if (cbeMigratorio.PERSONA.pers_iPersonaId != 0)
            {
                ddlDatoTipoDocumento.SelectedValue = cbeMigratorio.PERSONA.Identificacion.peid_sDocumentoTipoId.ToString();
                txtDatoNumDocumento.Text = cbeMigratorio.PERSONA.Identificacion.peid_vDocumentoNumero;
                if (cbeMigratorio.PERSONA.pers_sGeneroId > 0)
                {
                    ddlDatoGenero.SelectedValue = cbeMigratorio.PERSONA.pers_sGeneroId.ToString();
                }

                txtDatoPrimerApe.Text = cbeMigratorio.PERSONA.pers_vApellidoPaterno;
                txtDatoSegundoApe.Text = cbeMigratorio.PERSONA.pers_vApellidoMaterno;
                txtDatoNombres.Text = cbeMigratorio.PERSONA.pers_vNombres;
                txt_fechaNacimiento.Text = cbeMigratorio.PERSONA.pers_dNacimientoFecha.ToString("MMM-dd-yyyy");
            }            
            #endregion
        }

        string obj_Detalle_Baja = "obj_Detalle_Baja";
        void Load_Detalle_Baja(long acmi_iActoMigratorioId)
        {
            Session[obj_Detalle_Baja] = new ActoMigratorioConsultaBL().Consultar_Detalle_Bajas(acmi_iActoMigratorioId);
            gdvDetalle.DataSource = (DataTable)Session[obj_Detalle_Baja];
            gdvDetalle.DataBind();
        }

        public bool IsConsulado()
        {
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == (Int32)Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                return false;
            }
            else
                return true;
        }

        public bool IsConsuladoLima()
        {
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == (Int32)Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                return true;
            }
            else
                return false;
        }

        private void SetearUbigeo(string strUbigeo, DropDownList ddlDepartamento, DropDownList ddlProvincia, DropDownList ddlDistrito)
        {
            string strCodigo = string.Empty;
            strCodigo = strUbigeo.Substring(0, 2);

            ddlDepartamento.SelectedValue = strCodigo;
            comun_Part3.CargarUbigeo(Session, ddlProvincia, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddlDepartamento.SelectedValue.ToString(), "", true);
            comun_Part3.CargarUbigeo(Session, ddlDistrito, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);

            strCodigo = strUbigeo.Substring(2, 2);
            ddlProvincia.SelectedValue = strCodigo;
            comun_Part3.CargarUbigeo(Session, ddlDistrito, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddlDepartamento.SelectedValue, strCodigo, true);

            strCodigo = strUbigeo.Substring(4, 2);
            ddlDistrito.SelectedValue = strCodigo;
        }

        private void HabilitarControlesFormato(bool bolHabilitar = true)
        {

            txtNumSal_Lam.Enabled = bolHabilitar;
            txtFecExpedicion.Enabled = bolHabilitar;
            txtFecExpiracion.Enabled = bolHabilitar;

            txtFechaAprobacion.Enabled = bolHabilitar;
            ddlActoMigratorioEstado.Enabled = bolHabilitar;
            txtActoMigratorioObservaciones.Enabled = bolHabilitar;
        }

        #endregion

        void Cargar_Tipo_Pasaporte()
        {
            tipoInsumo.Items.Add(new ListItem("LIBRILLO", "1"));
            tipoInsumo.Items.Add(new ListItem("LÁMINA", "2"));
        }

        void Cargar_Tipo_Visa()
        {
            tipoInsumo.Items.Add(new ListItem("LÁMINA","2"));   
        }

        void Cargar_Tipo_Salvoconducto()
        {
            
            tipoInsumo.Items.Add(new ListItem("LIBRILLO", "1"));
            tipoInsumo.Items.Add(new ListItem("LÁMINA", "2"));
        }

        [WebMethod]
        public static string Cambiar_Estado(string[] acmi_iActoMigratorioId)
        {
            string StrScript = string.Empty;
            int i_Resultado = 0;
            CBE_MIGRATORIO oRE_ACTOMIGRATORIO = new CBE_MIGRATORIO();

            if (acmi_iActoMigratorioId.Length > 0)
            {
                foreach (string s in acmi_iActoMigratorioId)
                {
                    oRE_ACTOMIGRATORIO = new CBE_MIGRATORIO();
                    oRE_ACTOMIGRATORIO.ACTO.acmi_sEstadoId = (Int16)Enumerador.enmEstadoVisa.APROBADO;
                    #region - Llenado los datos historicos -
                    BE.MRE.RE_ACTOMIGRATORIOHISTORICO oRE_ACTOMIGRATORIOHISTORICO = new BE.MRE.RE_ACTOMIGRATORIOHISTORICO();
                    oRE_ACTOMIGRATORIOHISTORICO.amhi_IFuncionarioId = Convert.ToInt16("0");
                    oRE_ACTOMIGRATORIOHISTORICO.amhi_sMotivoId = Convert.ToInt16("0");
                    oRE_ACTOMIGRATORIOHISTORICO.amhi_dFechaRegistro = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
                    oRE_ACTOMIGRATORIOHISTORICO.amhi_vObservaciones = "";
                    oRE_ACTOMIGRATORIOHISTORICO.amhi_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                    oRE_ACTOMIGRATORIOHISTORICO.amhi_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                    oRE_ACTOMIGRATORIOHISTORICO.amhi_dFechaCreacion = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
                    oRE_ACTOMIGRATORIOHISTORICO.amhi_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                    oRE_ACTOMIGRATORIOHISTORICO.amhi_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                    oRE_ACTOMIGRATORIOHISTORICO.amhi_dFechaModificacion = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
                    oRE_ACTOMIGRATORIOHISTORICO.HostName = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_HOSTNAME]);
                    oRE_ACTOMIGRATORIOHISTORICO.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    oRE_ACTOMIGRATORIOHISTORICO.amhi_sEstadoId = (Int16)Enumerador.enmEstadoVisa.APROBADO;
                    #endregion

                    oRE_ACTOMIGRATORIO.ACTO.acmi_iActoMigratorioId = Convert.ToInt64(s);
                    oRE_ACTOMIGRATORIO.ACTO.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                    oRE_ACTOMIGRATORIO.ACTO.acmi_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
                    oRE_ACTOMIGRATORIO.ACTO.acmi_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
                    oRE_ACTOMIGRATORIO.ACTO.acmi_dFechaModificacion = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
                    oRE_ACTOMIGRATORIO.ACTO.HostName = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_HOSTNAME]);

                    oRE_ACTOMIGRATORIO.HISTORICO.Add(oRE_ACTOMIGRATORIOHISTORICO);

                    i_Resultado = new SGAC.Registro.Actuacion.BL.ActoMigratorioMantenimientoBL().Actualizar_Estados(oRE_ACTOMIGRATORIO);

                    if (i_Resultado > 0)
                        StrScript = "1";
                    else
                    {
                        StrScript = "0";
                        break;
                    }
                }
            }

            return StrScript;
        }

        public bool IsVisible(object s_Parametro)
        {
            bool b_Valor = false;
            switch (Comun.ToNullInt32(ddlTipoDocConsulta.SelectedItem.Value))
            {
                case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                    if ((Convert.ToInt16(s_Parametro) == (Int16)Enumerador.enmEstadoVisa.APROBADO)
                        || (Convert.ToInt16(s_Parametro) == (Int16)Enumerador.enmEstadoVisa.CORREGIDO)
                        || (Convert.ToInt16(s_Parametro) == 53) || (Convert.ToInt16(s_Parametro) == 54)
                        || (Convert.ToInt16(s_Parametro) == (Int16)Enumerador.enmEstadoVisa.ENTREGADO))
                        b_Valor = false;
                    else
                        b_Valor = true;
                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                    if (Convert.ToInt16(s_Parametro) == (Int16)Enumerador.enmMigratorioPasaporteEstados.BAJA
                        || Convert.ToInt16(s_Parametro) == (Int16)Enumerador.enmMigratorioPasaporteEstados.ANULADO_2
                        || Convert.ToInt16(s_Parametro) == (Int16)Enumerador.enmMigratorioPasaporteEstados.IMPRESO
                        || Convert.ToInt16(s_Parametro) == (Int16)Enumerador.enmMigratorioPasaporteEstados.ENTREGADO)
                        b_Valor = false;
                    else
                        b_Valor = true;
                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                    if (Convert.ToInt16(s_Parametro) == (Int16)Enumerador.enmEstadoSalvodonducto.APROBADO
                        || Convert.ToInt16(s_Parametro) == (Int16)Enumerador.enmEstadoSalvodonducto.ANULADO
                        || Convert.ToInt16(s_Parametro) == (Int16)Enumerador.enmEstadoSalvodonducto.IMPRESO
                        || Convert.ToInt16(s_Parametro) == (Int16)Enumerador.enmEstadoSalvodonducto.ENTREGADO)
                        b_Valor = false;
                    else
                        b_Valor = true;
                    break;
            }

            return b_Valor;
        }

        /// <summary>
        /// Habilita el button por estado
        /// </summary>
        /// <param name="s_Parametro"></param>
        /// <returns></returns>
        public bool IsEnabled(object s_Parametro)
        {
            bool b_Valor = false;
            switch (Comun.ToNullInt32(ddlTipoDocConsulta.SelectedItem.Value))
            {
                case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                    if ((Convert.ToInt16(s_Parametro) == (Int16)Enumerador.enmEstadoVisa.APROBADO)
                        || (Convert.ToInt16(s_Parametro) == (Int16)Enumerador.enmEstadoVisa.SOLICITADO))
                        b_Valor = true;
                    else
                        b_Valor = false;
                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                    if (Convert.ToInt16(s_Parametro) == (Int16)Enumerador.enmMigratorioPasaporteEstados.BAJA)
                        b_Valor = false;
                    else
                        b_Valor = true;
                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                    if (Convert.ToInt16(s_Parametro) == (Int16)Enumerador.enmEstadoSalvodonducto.APROBADO)
                        b_Valor = false;
                    else
                        b_Valor = true;
                    break;
            }

            return b_Valor;
        }

        string s_TipoPasaporte = string.Empty;

        protected void gdvDetalle_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
            string strScript = string.Empty;
            int intSeleccionado = Comun.ToNullInt32(e.CommandArgument);

            hdn_acmi_iActuacionDetalleId.Value = Comun.ToNullInt64(gdvDetalle.Rows[intSeleccionado].Cells[0].Text).ToString();
            hId_Migratorio.Value = Comun.ToNullInt64(gdvDetalle.Rows[intSeleccionado].Cells[1].Text).ToString();
            hdn_pers_iPersonaId.Value = Comun.ToNullInt64(gdvDetalle.Rows[intSeleccionado].Cells[2].Text).ToString();

            hinsu_iInsumoId.Value = Comun.ToNullInt64(gdvDetalle.Rows[intSeleccionado].Cells[3].Text).ToString();
            hinsu_sInsumoTipoId.Value = Comun.ToNullInt64(gdvDetalle.Rows[intSeleccionado].Cells[4].Text).ToString();
            hTipo_Baja.Value = "";

            string str_TipoInsumo = gdvDetalle.Rows[intSeleccionado].Cells[8].Text.Replace("&nbsp;", ""); ;
            int s_EstadoBaja = Comun.ToNullInt32(gdvDetalle.Rows[intSeleccionado].Cells[5].Text);

            string strEstado = gdvDetalle.Rows[intSeleccionado].Cells[12].Text;
            string strTipoInsumo = gdvDetalle.Rows[intSeleccionado].Cells[9].Text.Replace("&nbsp;","INSUMO");
            string strSMS = string.Empty;

            tb_Formulario.Visible = true;
            btnFormulario.Visible = true;

            Limpiar_Anulacion();
            lblLamina.Text = "Nro. Lamina:";
            if (e.CommandName == "DarBaja")
            {
                Cambiar_Titulo("1");
                if (s_EstadoBaja != (int)Enumerador.enmInsumoEstado.BAJA)
                {
                    if (!hinsu_iInsumoId.Value.Equals("0"))
                    {
                        strScript += Util.DeshabilitarTab(1, "tabs", true);
                        strScript += Util.ActivarTab(2, "Dar Baja");
                        txtCorreActuacion.Text = hdn_acmi_iActuacionDetalleId.Value;
                        toolbarAnular.btnGrabar.Enabled = true;
                        Util.CargarParametroDropDownList(ddlMotivoAnulacion, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTO_MIGRATORIO_MOTIVOS), true);
                        pnlDatosTitular.Visible = false;
                        CargarDatosActoMigratorio_Baja();
                        btnFormulario.Enabled = false;
                        tr_fechas.Visible = false;

                        if (hinsu_sInsumoTipoId.Value.Equals("6057"))
                        {
                            tb_Formulario.Visible = false;
                            btnFormulario.Visible = false;
                        }
                    }
                    else
                    {
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Consulta Migratorio","El trámite no tiene " + strTipoInsumo);
                    }
                }
                else
                {
                    if (Comun.ToNullInt64(hinsu_sInsumoTipoId.Value) == 6057)
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Consulta Migratorio", "No se puede dar de BAJA esta " + str_TipoInsumo + " código "+ strTipoInsumo + " porque se encuentra en Estado " + strEstado);
                    else
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Consulta Migratorio","No se puede dar de BAJA este " + str_TipoInsumo + " código " + strTipoInsumo + " porque se encuentra en Estado " + strEstado);
                }

                
                Comun.EjecutarScript(Page,  strScript);

            }
            else if (e.CommandName == "Anular")
            {
                hTipo_Baja.Value = "Anulado";

                lblLamina.Text = "Nro. Lamina:";
                Cambiar_Titulo("2");
                if (s_EstadoBaja != (int)Enumerador.enmInsumoEstado.BAJA)
                {
                    if (!hinsu_iInsumoId.Value.Equals("0"))
                    {
                        Util.CargarParametroDropDownList(ddlMotivoAnulacion, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTO_MIGRATORIO_MOTIVOS_ANULAR), true);
                        
                        strScript += Util.DeshabilitarTab(1, "tabs", true);
                        strScript += Util.ActivarTab(2, "Anular");
                        hTipo_Baja.Value = "Anulado";
                        toolbarAnular.btnGrabar.Enabled = true;
                        pnlDatosTitular.Visible = true;
                        btnFormulario.Enabled = false;
                        txtCorreActuacion.Text = hdn_acmi_iActuacionDetalleId.Value;
                        CargarDatosActoMigratorio_Baja();
                        tr_fechas.Visible = true; 

                    }
                    else
                    {
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Consulta Migratorio","El trámite no tiene " + strTipoInsumo);
                    }
                }
                else
                {
                    if (Comun.ToNullInt64(hinsu_sInsumoTipoId.Value) == (int)Enumerador.enmInsumoTipo.LAMINA) /*6057*/
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Consulta Migratorio","No se puede ANULAR esta " + str_TipoInsumo + " código " + strTipoInsumo + " porque se encuentra en Estado " + strEstado);
                    else
                        strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Consulta Migratorio","No se puede ANULAR este " + str_TipoInsumo + " código " + strTipoInsumo + " porque se encuentra en Estado " + strEstado);
                }
                Comun.EjecutarScript(Page, strScript);
            }
        }

        private void Limpiar_Anulacion()
        {
            txtFechaBaja.Text = "";
            ddlMotivoAnulacion.SelectedIndex = -1;
            txt_expedicion.Text = "";
            txt_Expiracion.Text = "";
            ddlDatoFuncionario.SelectedIndex = -1;
            txtCorreActuacion.Text = "";
            txtObservaciones.Text = "";
        }

        private void Cambiar_Titulo(string s_Valor)
        {
            string s_Titulo = string.Empty;
            if (s_Valor.Equals("2"))
            {
                lblFechaAnulacion.Text = "Fecha de Anulación:";
                lblTexto_8.Text = "Motivo de Anulación ";
                s_Titulo = "ANULACIÓN/ANULADO";
            }
            else
            {
                lblFechaAnulacion.Text = "Fecha de Baja: ";
                s_Titulo = "BAJA";
                lblTexto_8.Text = "Motivo de Baja ";
            }
            if (Session["Tipo_Pasaporte"] != null)
            {
                cbo_estadoTramite.Items.Clear();
                if (!Convert.ToString(Session["Tipo_Pasaporte"]).Equals(""))
                {
                    cbo_estadoTramite.Items.Add(new ListItem { Value = "0", Text = Convert.ToString(Session["Tipo_Pasaporte"]) });
                }
            }
            cbo_estadoTramite.Enabled = false;
            switch (Comun.ToNullInt32(ddlTipoDocConsulta.SelectedItem.Value))
            {
                case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                    lblTexto_7.Text = "DATOS DE VISA / " + tipoInsumo.SelectedItem.Text.ToUpper() + " DADO DE " + s_Titulo;
                    Label39.Text = "FORMULARIO DGC: ";
                    btnFormulario.Text = "   Formulario";
                    tr_Estado.Visible = false;
                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                    if (s_Valor.Equals("2"))
                        lblTexto_7.Text = "DATOS ADICIONALES DE PASAPORTE A ANULAR";
                    else
                        lblTexto_7.Text = "DATOS DE PASAPORTE / " + tipoInsumo.SelectedItem.Text.ToUpper() + " DADO DE " + s_Titulo;

                    if (s_Valor.Equals("2"))
                    {
                        Label39.Text = "FORMULARIO DGC - 003";
                        btnFormulario.Text = "   Formulario DGC - 003";
                    }
                    else
                    {
                        Label39.Text = "FORMULARIO DGC - 004";
                        btnFormulario.Text = "   Formulario DGC - 004";
                    }
                    lblTexto_8.Text += "del pasaporte:";
                    if (s_Valor.Equals("2"))
                    {
                        tr_Estado.Visible = true;
                    }
                    else
                    {
                        tr_Estado.Visible = false;
                    }
                    Label18.Text = "Estado del Pasaporte: ";
                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                    if (s_Valor.Equals("2"))
                        lblTexto_7.Text = "DATOS ADICIONALES DE SALVOCONDUCTO A ANULAR";
                    else
                        lblTexto_7.Text = "DATOS DE SALVOCONDUCTO / " + tipoInsumo.SelectedItem.Text.ToUpper() + " DADO DE " + s_Titulo;
                    Label39.Text = "FORMULARIO DGC: ";
                    btnFormulario.Text = "   Formulario";
                    lblTexto_8.Text += "del salvoconducto:";
                    if (s_Valor.Equals("2"))
                    {
                        tr_Estado.Visible = true;
                    }
                    else
                    {
                        tr_Estado.Visible = false;
                    }
                    Label18.Text = "Estado del Salvoconducto: ";
                    break;
            }
        }

        protected void ddlVisaTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                if (Convert.ToInt32(ddlVisaTipo.SelectedValue) == 0)
                {
                    ddlVisaSubTipo.Items.Clear();
                    ddlVisaSubTipo.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- SELECCIONAR -", "0"));

                    ddlTitularFamiliar.SelectedIndex = -1;
                }
                else
                {
                    if (ddlVisaTipo.SelectedItem.Text.Trim() == "RESIDENTE")
                    {
                        Util.CargarParametroDropDownList(ddlVisaSubTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_MIGRA_VISA_TIPO_RESIDENTE), true);
                    }
                    else
                    {
                        Util.CargarParametroDropDownList(ddlVisaSubTipo, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_MIGRA_VISA_TIPO_TEMPORAL), true);
                    }
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

        [WebMethod]
        public static string Actualizar_Estado(string[] parametros)
        {
            string acmi_iActoMigratorioId = parametros[0];
            string amhi_IFuncionarioId = parametros[1];
            string amhi_sMotivoId = parametros[2];
            string s_Fecha = parametros[3];
            string insu_sInsumoTipoId = parametros[4];
            string insu_iInsumoId = parametros[5];
            string Observaciones = parametros[6].ToUpper();
            string estado = parametros[7].ToUpper();

            string StrScript = string.Empty;
            int i_Resultado = 0;
            CBE_MIGRATORIO oRE_ACTOMIGRATORIO = new CBE_MIGRATORIO();
            oRE_ACTOMIGRATORIO.ACTO.acmi_sEstadoId = 0;

            string s_Hora = Comun.ObtenerHoraActualTexto(HttpContext.Current.Session);

            #region - Llenado los datos historicos -
            BE.MRE.RE_ACTOMIGRATORIOHISTORICO oRE_ACTOMIGRATORIOHISTORICO = new BE.MRE.RE_ACTOMIGRATORIOHISTORICO();
            oRE_ACTOMIGRATORIOHISTORICO.amhi_IFuncionarioId = Convert.ToInt32(amhi_IFuncionarioId);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_sMotivoId = Convert.ToInt16(amhi_sMotivoId);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_sInsumoId = Convert.ToInt64(insu_iInsumoId);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_sTipoInsumoId = Convert.ToInt16(insu_sInsumoTipoId);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_dFechaRegistro = Comun.FormatearFecha(s_Fecha + " " + s_Hora);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_vObservaciones = Observaciones;
            oRE_ACTOMIGRATORIOHISTORICO.amhi_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
            oRE_ACTOMIGRATORIOHISTORICO.amhi_dFechaCreacion = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
            oRE_ACTOMIGRATORIOHISTORICO.amhi_dFechaModificacion = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
            oRE_ACTOMIGRATORIOHISTORICO.HostName = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_HOSTNAME]);
            oRE_ACTOMIGRATORIOHISTORICO.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

            if ( estado.ToUpper() == Enumerador.enmInsumoEstado.ANULADO.ToString())
            {
                oRE_ACTOMIGRATORIOHISTORICO.amhi_sEstadoId = (Int16)Enumerador.enmInsumoEstado.ANULADO;
            }
            else
            {
                oRE_ACTOMIGRATORIOHISTORICO.amhi_sEstadoId = (Int16)Enumerador.enmInsumoEstado.BAJA;
            }
            
            #endregion
            
            oRE_ACTOMIGRATORIO.ACTO.acmi_iActoMigratorioId = Convert.ToInt64(acmi_iActoMigratorioId);
            oRE_ACTOMIGRATORIO.ACTO.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            oRE_ACTOMIGRATORIO.ACTO.acmi_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
            oRE_ACTOMIGRATORIO.ACTO.acmi_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            oRE_ACTOMIGRATORIO.ACTO.acmi_dFechaModificacion = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
            oRE_ACTOMIGRATORIO.ACTO.HostName = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_HOSTNAME]);
            oRE_ACTOMIGRATORIO.HISTORICO.Add(oRE_ACTOMIGRATORIOHISTORICO);

            i_Resultado = new SGAC.Registro.Actuacion.BL.ActoMigratorioMantenimientoBL().Actualizar_Estados(oRE_ACTOMIGRATORIO);

            if (i_Resultado > 0)
            {
                StrScript = i_Resultado.ToString();
            }
            else
            {
                StrScript = "0";
            }

            return StrScript;
        }

        protected void btnAgregarObservacion_Click(object sender, EventArgs e)
        {
            if (Comun.ToNullInt32(ddlTipoDocConsulta.SelectedItem.Value) == (Int32)Enumerador.enmDocumentoMigratorio.VISAS)
            {
                if (Comun.ToNullInt32(ddlActoMigratorioEstado.SelectedItem.Value) == 1)
                {
                    Comun.EjecutarScript(this, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Consulta Migratorio", "Opción no habilitada para Visas."));
                    return;
                }
            }
            
            if (hdn_Estado.Value.Equals("REGISTRADO"))
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Consulta Migratorio", "Solo se puede aprobar/Observar cuando el trámite este solicitado"));
                return;
                
            }
            
            hId_AprobadoEstado.Value = "1";

            switch (Comun.ToNullInt32(ddlTipoDocConsulta.SelectedItem.Value))
            {
                case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                    if (ddlActoMigratorioEstado.SelectedItem.Text.Trim().Equals("OBSERVADO"))
                    {
                        txtNumeroPass.Enabled = false;
                        hId_AprobadoEstado.Value = "2";
                    }
                    else
                        txtNumeroPass.Enabled = true;
                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                    if (ddlActoMigratorioEstado.SelectedItem.Text.Trim().Equals("OBSERVADO"))
                    {
                        hId_AprobadoEstado.Value = "2";
                        txtOtros.Enabled = false;
                    }
                    else
                        txtOtros.Enabled = true;
                    txtNumeroPass.Enabled = false;
                    break;
            }

            try
            {
                var obj_Seguimiento = (DataTable)Session["dtSeguimiento_Consulta"];

                DataRow rowDetAct;

                rowDetAct = obj_Seguimiento.NewRow();

                rowDetAct["dFechaRegistro"] = ObtenerFechaActual(Session);
                rowDetAct["sEstadoId"] = ddlActoMigratorioEstado.SelectedItem.Value;
                rowDetAct["vEstado"] = ddlActoMigratorioEstado.SelectedItem.Text;
                rowDetAct["sFuncionarioId"] = ddlFuncionario.SelectedItem.Value;
                rowDetAct["vFuncionario"] = ddlFuncionario.SelectedItem.Text;
                rowDetAct["vObservacion"] = txtActoMigratorioObservaciones.Text.ToUpper();
                rowDetAct["amhi_sInsumoId"] = 0;

                obj_Seguimiento.Rows.Add(rowDetAct);

                gdvSeguimiento.DataSource = obj_Seguimiento;
                gdvSeguimiento.DataBind();

                Session["dtSeguimiento_Consulta"] = obj_Seguimiento;

                pnl_Aprobaciones.Visible = true;
                btnAgregarObservacion.Enabled = false;

                txtFechaAprobacion.Text = ObtenerFechaActual(Session).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                txtActoMigratorioObservaciones.Text = string.Empty;
                ddlActoMigratorioEstado.SelectedValue = "0";
            }
            catch
            {
            }
        }

        [WebMethod]
        public static string Estado_Aprobaciones(string[] parametros)
        {
            string acmi_iActoMigratorioId = parametros[0];
            string s_Fecha = parametros[1];
            string Observaciones = parametros[2];
            string acmi_sEstadoId = parametros[3];
            string Tipo_Actuacion = parametros[4];
            string amhi_IFuncionarioId = parametros[5];

            if (acmi_sEstadoId.Trim().Equals("0")) return "9";
            string StrScript = string.Empty;
            int i_Resultado = 0;
            CBE_MIGRATORIO oRE_ACTOMIGRATORIO = new CBE_MIGRATORIO();
            
            #region - Llenado los datos historicos -
            BE.MRE.RE_ACTOMIGRATORIOHISTORICO oRE_ACTOMIGRATORIOHISTORICO = new BE.MRE.RE_ACTOMIGRATORIOHISTORICO();
            oRE_ACTOMIGRATORIOHISTORICO.amhi_dFechaRegistro = Comun.FormatearFecha(s_Fecha);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_IFuncionarioId = Comun.ToNullInt32(amhi_IFuncionarioId);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_vObservaciones = Observaciones;
            oRE_ACTOMIGRATORIOHISTORICO.amhi_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
            oRE_ACTOMIGRATORIOHISTORICO.amhi_dFechaCreacion = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            oRE_ACTOMIGRATORIOHISTORICO.amhi_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
            oRE_ACTOMIGRATORIOHISTORICO.amhi_dFechaModificacion = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
            oRE_ACTOMIGRATORIOHISTORICO.HostName = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_HOSTNAME]);
            oRE_ACTOMIGRATORIOHISTORICO.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            
            #endregion

            oRE_ACTOMIGRATORIO.ACTO.acmi_iActoMigratorioId = Convert.ToInt64(acmi_iActoMigratorioId);
            oRE_ACTOMIGRATORIO.ACTO.OficinaConsultar = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            oRE_ACTOMIGRATORIO.ACTO.acmi_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
            oRE_ACTOMIGRATORIO.ACTO.acmi_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            oRE_ACTOMIGRATORIO.ACTO.acmi_dFechaModificacion = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
            oRE_ACTOMIGRATORIO.ACTO.HostName = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_HOSTNAME]);

            switch (Convert.ToInt32(Tipo_Actuacion))
            {
                case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                    oRE_ACTOMIGRATORIO.ACTO.acmi_sEstadoId = (acmi_sEstadoId == "1") ? (Int16)Enumerador.enmMigratorioPasaporteEstados.BAJA : (Int16)Enumerador.enmMigratorioPasaporteEstados.OBSERVADO;
                    oRE_ACTOMIGRATORIOHISTORICO.amhi_sEstadoId = (acmi_sEstadoId == "1") ? (Int16)Enumerador.enmMigratorioPasaporteEstados.BAJA : (Int16)Enumerador.enmMigratorioPasaporteEstados.OBSERVADO;
                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                    oRE_ACTOMIGRATORIO.ACTO.acmi_sEstadoId = (acmi_sEstadoId == "1") ? (Int16)Enumerador.enmEstadoSalvodonducto.APROBADO : (Int16)Enumerador.enmEstadoSalvodonducto.OBSERVADO;
                    oRE_ACTOMIGRATORIOHISTORICO.amhi_sEstadoId = (acmi_sEstadoId == "1") ? (Int16)Enumerador.enmEstadoSalvodonducto.APROBADO : (Int16)Enumerador.enmEstadoSalvodonducto.OBSERVADO;

                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                    /*NO APLICA PARA ESTE MÉTODO*/
                    break;
            }

            oRE_ACTOMIGRATORIO.HISTORICO.Add(oRE_ACTOMIGRATORIOHISTORICO);

            i_Resultado = new SGAC.Registro.Actuacion.BL.ActoMigratorioMantenimientoBL().Actualizar_Estados(oRE_ACTOMIGRATORIO);


            if (i_Resultado > 0)
            {
                StrScript = "1";
            }
            else
            {
                StrScript = "0";
            }

            return StrScript;
        }

        protected void btnFormulario_Click(object sender, EventArgs e)
        {
            try
            {
                if (Comun.ToNullInt64(hId_Baja.Value) != 0)
                {
                    toolbarAnular.btnGrabar.Enabled = false;

                    Load_Detalle_Baja(Comun.ToNullInt64(hdn_acmi_iActoMigratorioId.Value));
                }

                if (hTipo_Baja.Value == "Anulado")
                    Session["iTipo_Reporte"] = Enumerador.enmMigratorioFormato.DGC_003_PASAPORTE_ANULADO;
                else
                    Session["iTipo_Reporte"] = Enumerador.enmMigratorioFormato.DGC_004_PASAPORTE_BAJA;

                Session[Constantes.CONST_SESION_ACTUACIONDET_ID] = hdn_acmi_iActuacionDetalleId.Value;
                Session["Acto_Migratorio_ID"] = hId_Migratorio.Value;
                string strUrl = "../Registro/frmReporteMigratorio.aspx?vClass=3&vActo=" + hId_Baja.Value;
                string strScript = "window.open('" + strUrl + "', 'popup_window', 'scrollbars=1,resizable=1,width=500,height=700,left=100,top=100');";
                Comun.EjecutarScript(Page, strScript);
            }
            catch
            {
            }
        }

        [WebMethod]
        public static string actualizar_registro(string actonomigratorio)
        {
            string s_Resultado = string.Empty;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var jsonObject = serializer.Deserialize<dynamic>(actonomigratorio);

            string s_Tipo_Actuacion = Convert.ToString(jsonObject["acmi_sTipoActoMigratorio"]);

            CBE_MIGRATORIO obj_Migratorio = new CBE_MIGRATORIO();

            obj_Migratorio.ACTO.acmi_vNumeroLamina = Convert.ToString(jsonObject["acmi_vNumeroLamina"]);
            obj_Migratorio.PERSONA.pers_iPersonaId = Comun.ToNullInt64(jsonObject["acmi_sPersonaId"]);            

            int s_EstadoId = Comun.ToNullInt32(jsonObject["ami_EstadoId"]);

            #region - Validar el número de lámina -
            // T#000263
            string strTipoPasaporte = "";
            if (HttpContext.Current.Session["Tipo_Pasaporte"] != null)
                strTipoPasaporte = HttpContext.Current.Session["Tipo_Pasaporte"].ToString();

            if (!strTipoPasaporte.Equals("REVALIDADO"))
            {
                if (obj_Migratorio.ACTO.acmi_vNumeroLamina != "")
                {
                    int i_Resultado = new SGAC.Registro.Actuacion.BL.ActoMigratorioMantenimientoBL().Validar_Insumo(
                        (Int32)Enumerador.enmInsumoTipo.LAMINA, obj_Migratorio.ACTO.acmi_vNumeroLamina, Convert.ToInt32(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                        Convert.ToInt32(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]), obj_Migratorio.ACTO.acmi_vNumeroLamina, ref s_Resultado);

                    if (s_Resultado != "")
                        return s_Resultado;
                }
            }
            #endregion


            #region - Validar el número de passaporte -
            obj_Migratorio.ACTO.acmi_vNumeroDocumento = Convert.ToString(jsonObject["acmi_vNumeroDocumento"]);
            switch (Convert.ToInt32(s_Tipo_Actuacion))
            {
                case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                    if (!string.IsNullOrEmpty(obj_Migratorio.ACTO.acmi_vNumeroDocumento))
                    {
                        //int i_Resultado = new SGAC.Registro.Actuacion.BL.ActoMigratorioMantenimientoBL().Validar_Insumo(
                        //    (Int32)Enumerador.enmInsumoTipo.PASAPORTE, obj_Migratorio.ACTO.acmi_vNumeroDocumento, Convert.ToInt32(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                        //    Convert.ToInt32(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]), obj_Migratorio.ACTO.acmi_vNumeroDocumento, ref s_Resultado);

                        //if (s_Resultado != "")
                        //    return s_Resultado;

                        if ((s_EstadoId == (int)Enumerador.enmMigratorioPasaporteEstados.CORREGIDO) || (s_EstadoId == (int)Enumerador.enmEstadoSalvodonducto.CORREGIDO))
                        {

                        }
                        else
                        {
                            var existe_documento = new SGAC.Registro.Persona.BL.PersonaIdentificacionConsultaBL().Existe((int)Enumerador.enmTipoDocumento.PASAPORTE,
                                obj_Migratorio.ACTO.acmi_vNumeroDocumento, obj_Migratorio.PERSONA.pers_iPersonaId, 2);

                            if (existe_documento > 0)
                            {
                                return "El número de Pasaporte ingresado ya se encuentra registrado";
                            }
                        }
                    }
                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:

                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:

                    break;
            }

            #endregion

            obj_Migratorio.ACTO.acmi_iActuacionDetalleId = Convert.ToInt64(jsonObject["acmi_iActuacionDetalleId"]);
            obj_Migratorio.ACTO.acmi_iActoMigratorioId = Convert.ToInt64(jsonObject["acmi_iActoMigratorioId"]);
            obj_Migratorio.ACTO.acmi_iActuacionDetalleId = Convert.ToInt64(jsonObject["acmi_iActuacionDetalleId"]);
            obj_Migratorio.ACTO.acmi_IFuncionarioId = Convert.ToInt32(jsonObject["acmi_IFuncionarioId"]);
            obj_Migratorio.ACTO.acmi_vNumeroExpediente = Convert.ToString(jsonObject["acmi_vNumeroExpediente"]);
            obj_Migratorio.ACTO.acmi_dFechaExpedicion = Comun.FormatearFecha(jsonObject["acmi_dFechaExpedicion"]);

            switch (Convert.ToInt32(s_Tipo_Actuacion))
            {
                case (Int32)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                    obj_Migratorio.ACTO.acmi_sEstadoId = (Int16)Enumerador.enmMigratorioPasaporteEstados.EXPEDIDO;
                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.VISAS:
                    obj_Migratorio.ACTO.acmi_sEstadoId = (Int16)Enumerador.enmEstadoVisa.REGISTRADO;
                    break;
                case (Int32)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                    obj_Migratorio.ACTO.acmi_sEstadoId = (Int16)Enumerador.enmEstadoSalvodonducto.REGISTRADO;
                    break;
            }

            obj_Migratorio.ACTO.acmi_sUsuarioCreacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            obj_Migratorio.ACTO.acmi_dFechaCreacion = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
            obj_Migratorio.ACTO.acmi_vIPCreacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
            obj_Migratorio.ACTO.acmi_vIPModificacion = SGAC.Accesorios.Util.ObtenerDireccionIP();
            obj_Migratorio.ACTO.acmi_sUsuarioModificacion = Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]);
            obj_Migratorio.ACTO.acmi_dFechaModificacion = new MyBasePage().ObtenerFechaActual(HttpContext.Current.Session);
            obj_Migratorio.ACTO.HostName = Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_HOSTNAME]);
           
            switch (Convert.ToInt16(s_Tipo_Actuacion))
            {
                case (Int16)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO:
                    obj_Migratorio.ACTO.acmi_sTipoDocumentoMigratorioId = (int)Enumerador.enmDocumentoMigratorio.SALVOCONDUCTO;
                    obj_Migratorio.ACTO.acmi_vNumeroDocumentoAnterior = Convert.ToString(jsonObject["acmi_vNumeroDocumentoAnterior"]);
                    obj_Migratorio.ACTO.acmi_dFechaExpiracion = obj_Migratorio.ACTO.acmi_dFechaExpedicion.AddMonths(1);
                    
                    break;
                case (Int16)Enumerador.enmDocumentoMigratorio.VISAS:
                    obj_Migratorio.ACTO.acmi_sTipoDocumentoMigratorioId = (int)Enumerador.enmDocumentoMigratorio.VISAS;
                    obj_Migratorio.ACTO.acmi_dFechaExpiracion = Comun.FormatearFecha(jsonObject["acmi_dFechaExpiracion"]);
                    
                    break;
                case (Int16)Enumerador.enmDocumentoMigratorio.PASAPORTE:
                    obj_Migratorio.ACTO.acmi_sTipoDocumentoMigratorioId = (int)Enumerador.enmDocumentoMigratorio.PASAPORTE;
                    obj_Migratorio.ACTO.acmi_dFechaExpiracion = Comun.FormatearFecha(jsonObject["acmi_dFechaExpiracion"]);
                    
                    break;
            }

            s_Resultado = new ActoMigratorioMantenimientoBL().Actualizar_Migratorio(obj_Migratorio);

            return s_Resultado;
        }

        protected void chk_item_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;

            if (chk.Checked)
            {
                btn_grabar.Enabled = true;
            }
        }

        protected void btn_Habilitar_Click(object sender, EventArgs e)
        {
            btnFormulario.Enabled = true;

            toolbarAnular.btnGrabar.Enabled = false;

            txtFechaBaja.EnabledText = false;
            ddlMotivoAnulacion.Enabled = false;
            ddlDatoFuncionario.Enabled = false;
            txtObservaciones.Enabled = false;

            Load_Detalle_Baja(Comun.ToNullInt64(hdn_acmi_iActoMigratorioId.Value));

        }

        protected void gdvMigratorio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string _estado = DataBinder.Eval(e.Row.DataItem, "acmi_vEstado").ToString();

                if (_estado.Equals("OBSERVADO"))
                    e.Row.BackColor = System.Drawing.Color.LightCoral;
            }
        }

        protected void ddlActoMigratorioEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlActoMigratorioEstado.SelectedItem.Value.ToString().Equals("1"))
            {
                ddlFuncionario.SelectedValue = "0";
                ddlFuncionario.Enabled = true;
                txtOtros.Enabled = true; //Agregado sacostac
            }
            else
            {
                ddlFuncionario.SelectedValue = "0";
                ddlFuncionario.Enabled = false;
                txtOtros.Enabled = false;//Agregado sacostac
            }
        }
    }
}