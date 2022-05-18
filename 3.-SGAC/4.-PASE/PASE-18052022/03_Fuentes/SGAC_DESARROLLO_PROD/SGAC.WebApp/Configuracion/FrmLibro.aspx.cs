using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.WebApp.Accesorios;
using SGAC.Configuracion.Sistema.BL;
using SGAC.Registro.Actuacion.BL;

namespace SGAC.WebApp.Configuracion
{
    public partial class FrmLibro : MyBasePage
    {
        #region Campos

        #endregion

        #region Eventos
        protected void Page_Init(object sender, EventArgs e)
        {
            ctrlPaginador.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginador.Visible = false;
            ctrlPaginador.PaginaActual = 1;

            ctrlPaginadorStock.PageSize = Constantes.CONST_CANT_REGISTRO;
            ctrlPaginadorStock.Visible = false;
            ctrlPaginadorStock.PaginaActual = 1;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ctrlToolBarConsulta.VisibleIButtonBuscar = true;
            ctrlToolBarConsulta.VisibleIButtonCancelar = true;
            ctrlToolBarConsulta.btnCancelar.CssClass = "btnLimpiar";
            ctrlToolBarConsulta.btnCancelar.Text = "    Limpiar";

            ctrlToolBarConsulta.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBarConsulta_btnBuscarHandler);
            ctrlToolBarConsulta.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarConsulta_btnCancelarHandler);

            ctrlToolBarMantenimiento.VisibleIButtonNuevo = true;
            ctrlToolBarMantenimiento.VisibleIButtonCancelar = true;
            ctrlToolBarMantenimiento.VisibleIButtonEditar = true;
            ctrlToolBarMantenimiento.VisibleIButtonEliminar = true;
            ctrlToolBarMantenimiento.VisibleIButtonGrabar = true;

            ctrlToolBarMantenimiento.btnGrabar.OnClientClick = "return ValidarRegistro()";

            ctrlToolBarMantenimiento.btnNuevoHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonNuevoClick(ctrlToolBarMantenimiento_btnNuevoHandler);
            ctrlToolBarMantenimiento.btnEditarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEditarClick(ctrlToolBarMantenimiento_btnEditarHandler);
            ctrlToolBarMantenimiento.btnEliminarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEliminarClick(ctrlToolBarMantenimiento_btnEliminarHandler);
            ctrlToolBarMantenimiento.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBarMantenimiento_btnGrabarHandler);
            ctrlToolBarMantenimiento.btnCancelarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonCancelarClick(ctrlToolBarMantenimiento_btnCancelarHandler);

            if (!Page.IsPostBack)
            {
                hdn_sOficinaConsularId.Value = Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString();
                hdn_sUsuarioId.Value = Session[Constantes.CONST_SESION_USUARIO_ID].ToString();
                hdn_sAccionId.Value = ((int)Enumerador.enmAccion.INSERTAR).ToString();

                CargarListadosDesplegables();

                lblPeriodoMant.Visible = false;
                ddlPeriodoMant.Visible = false;
                lblLibroNumero.Visible = false;
                txtNumeroLibro.Visible = false;
                lblNumeroEscritura.Visible = false;
                txtNumeroEscritura.Visible = false;
                lblNumeroFolioInicial.Visible = false;
                txtNumFolioInicial.Visible = false;
                lblNumeroFolioActual.Visible = false;
                txtNumFolioActual.Visible = false;
                lblNumeroFoliosTotales.Visible = false;
                txtNumeroFoliosTotales.Visible = false;
                //--------------------------------------------------------
                //Fecha: 23/06/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Poner visible a los objetos.
                //---------------------------------------------------------
                lblEtiquetaFoliosDisponibles.Visible = false;
                lblFoliosDisponibles.Visible = false;
                lblIntervaloFolios.Visible = false;
                txtIntervaloFolios.Visible = false;
                lblTextoIntervalo.Visible = false;
                imgFolioNC.Visible = false;
                btnImprimirFojasNC.Visible = false;
                //---------------------------------------------------------
                lblEstadoLibro.Visible = false;
                chkEstadoLibro.Visible = false;
                lblTipoInsumo.Visible = false;
                ddlTipoInsumo.Visible = false;
                lblCantidadStockMinimo.Visible = false;
                txtStockMinimo.Visible = false;
                lblCO_ddlPeriodo.Visible = false;
                Label1.Visible = false;
                Label2.Visible = false;
                Label3.Visible = false;
                Label4.Visible = false;
                Label5.Visible = false;


                lblTipoInsumoConsulta.Visible = false;
                ddlTipoInsumoConsulta.Visible = false;
                lblPeriodo.Visible = false;
                ddlPeriodo.Visible = false;

                //--------------------------------------------------------
                //Fecha: 23/06/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Poner invisible a los objetos.
                //---------------------------------------------------------
                lblEtiquetaFoliosDisponibles.Visible = false;
                lblFoliosDisponibles.Visible = false;
                lblIntervaloFolios.Visible = false;
                txtIntervaloFolios.Visible = false;
                lblTextoIntervalo.Visible = false;
                imgFolioNC.Visible = false;
                btnImprimirFojasNC.Visible = false;
                //---------------------------------------------------------

                CargarDatosIniciales();
            }
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBarMantenimiento.btnNuevo, ctrlToolBarMantenimiento.btnEditar, ctrlToolBarMantenimiento.btnGrabar, ctrlToolBarMantenimiento.btnEliminar };
                GridView[] arrGridView = { gdvLibroCorrelativos };
                Comun.ModoLectura(ref arrButtons);
                Comun.ModoLectura(ref arrGridView);
            }
        }

        void ctrlToolBarConsulta_btnBuscarHandler()
        {
            bool bolTipoDocSeleccionado = true;
            if (ddlTipoConfiguracion.SelectedValue == ((int)Enumerador.enmNotarialLibroTipo.ESCRITURA_PUBLICA).ToString())
            {
                ctrlPaginador.InicializarPaginador();
                gdvLibroCorrelativos.DataSource = null;
                gdvLibroCorrelativos.DataBind();

                CargarDatosGrilla();
                this.gdvLibroCorrelativos.Visible = true;
                this.gdvStockAlmacen.Visible = false;
            }
            else if (ddlTipoConfiguracion.SelectedItem.Text == Constantes.CONST_ALMACEN.ToString())
            {
                ctrlPaginadorStock.InicializarPaginador();
                gdvStockAlmacen.DataSource = null;
                gdvStockAlmacen.DataBind();

                CargarDatosGrillaStockAlmacen();
                this.gdvStockAlmacen.Visible = true;
                this.gdvLibroCorrelativos.Visible = false;
            }
            else
            {
                bolTipoDocSeleccionado = false;
            }

            if (!bolTipoDocSeleccionado)
            {
                ctrlValidacion.MostrarValidacion("Seleccione un Tipo de Configuración", true, Enumerador.enmTipoMensaje.WARNING);
            }
        }

        void ctrlToolBarConsulta_btnCancelarHandler()
        {
            ddlOficinaConsular.SelectedIndex = 0;
            ddlTipoConfiguracion.SelectedIndex = 0;

            Session["DT_LIBROS"] = new DataTable();
            gdvLibroCorrelativos.DataSource = new DataTable();
            gdvLibroCorrelativos.DataBind();

            Session["DT_STOCK_ALMACEN"] = new DataTable();
            gdvStockAlmacen.DataSource = new DataTable();
            gdvStockAlmacen.DataBind();
        }


        void ctrlToolBarMantenimiento_btnEliminarHandler()
        {
            hdn_sAccionId.Value = ((int)Enumerador.enmAccion.ELIMINAR).ToString();
            ctrlToolBarMantenimiento_btnGrabarHandler();
        }

        void ctrlToolBarMantenimiento_btnEditarHandler()
        {
            hdn_sAccionId.Value = ((int)Enumerador.enmAccion.MODIFICAR).ToString();

            HabilitarOpcionesPorAccion();
        }

        void ctrlToolBarMantenimiento_btnNuevoHandler()
        {
            hdn_sAccionId.Value = ((int)Enumerador.enmAccion.INSERTAR).ToString();
            chkEstadoLibro.Checked = false;
            HabilitarOpcionesPorAccion();
        }

        void ctrlToolBarMantenimiento_btnCancelarHandler()
        {
            LimpiarDatosMantenimiento();
            Comun.EjecutarScript(Page, Util.ActivarTab(0, Constantes.CONST_TAB_INICIAL) + Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR));
        }

        void ctrlToolBarMantenimiento_btnGrabarHandler()
        {
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                if (ddlOficinaConsularMant.SelectedIndex == 0) {

                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGA INICIAL - LIBROS - ALMACÉN", "Seleccione la Oficina Consular."));
                    ddlOficinaConsularMant.Focus();
                    return;
                }
            }
            

            if (ddlLibroTipoMant.SelectedValue == ((int)Enumerador.enmNotarialLibroTipo.ESCRITURA_PUBLICA).ToString())
            {
                if (validarRegistroLibro() == false)
                { return; }

                LibroMantenimientoBL objBL = new LibroMantenimientoBL();
                BE.MRE.SI_LIBRO objLibro = new BE.MRE.SI_LIBRO();
                ActoNotarialConsultaBL oActoNotarialConsultaBL = new ActoNotarialConsultaBL();
                objLibro = ObtenerDatosLibro();

                int intAccionId = Convert.ToInt32(hdn_sAccionId.Value);
                String StrScript = String.Empty;
                string strMensaje = string.Empty;

                if (intAccionId != (int)Enumerador.enmAccion.ELIMINAR)
                {
                    strMensaje = oActoNotarialConsultaBL.ExisteNumeroDocumento(objLibro);
                    if (strMensaje != string.Empty)
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Libro", strMensaje, true, 200, 250);
                        Comun.EjecutarScript(Page, StrScript);
                        return;
                    }
                }

                switch (intAccionId)
                {
                    case (int)Enumerador.enmAccion.INSERTAR:
                        objLibro = objBL.insertar(objLibro);
                        break;
                    case (int)Enumerador.enmAccion.MODIFICAR:
                        objLibro = objBL.actualizar(objLibro);
                        break;
                    case (int)Enumerador.enmAccion.ELIMINAR:
                        objLibro = objBL.eliminar(objLibro);
                        break;
                    default:
                        break;
                }

                string strScript = string.Empty;
                if (!objLibro.Error)
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGA INICIAL - LIBROS - ALMACÉN", Constantes.CONST_MENSAJE_EXITO);
                    strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                    strScript += Util.ActivarTab(0, Constantes.CONST_TAB_INICIAL);

                    hdn_sAccionId.Value = ((int)Enumerador.enmAccion.INSERTAR).ToString();
                    HabilitarCamposMantenimiento();
                    LimpiarDatosMantenimiento();
                    // -- Carga
                    gdvLibroCorrelativos.DataSource = null;
                    gdvLibroCorrelativos.DataBind();
                    ctrlPaginador.InicializarPaginador();
                    updConsulta.Update();
                }
                else
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGA INICIAL - LIBROS - ALMACÉN", Constantes.CONST_MENSAJE_OPERACION_FALLIDA);
                }

                Comun.EjecutarScript(Page, strScript);
            }
            else if (ddlLibroTipoMant.SelectedItem.Text == Constantes.CONST_ALMACEN.ToString())
            {
                if (validarRegistroAlmacen() == false)
                {
                    return;
                }
                StockMinimoMantenimientoBL objeBL = new StockMinimoMantenimientoBL();
                BE.MRE.SI_STOCK_ALMACEN objStock = new BE.MRE.SI_STOCK_ALMACEN();
                Stock_Almacen_ConsultaBL oStockAlmacenConsultaBL = new Stock_Almacen_ConsultaBL();
                objStock = ObtenerDatosStock();

                int intAccionStockId = Convert.ToInt32(hdn_sAccionId.Value);

                //----------------------------------------------------
                //Fecha: 14/02/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Obtener el id del stock si existe se modifica.
                //----------------------------------------------------
                Int16 intStockId = ObtenerStockId();

                if (intStockId > 0)
                {
                    objStock.stck_sStockId = intStockId;
                    //intAccionStockId = (int)Enumerador.enmAccion.MODIFICAR;
                    if (intAccionStockId == 1 || intAccionStockId == 0)
                    {
                        intAccionStockId = (int)Enumerador.enmAccion.MODIFICAR;
                    }
                    else if (intAccionStockId == 2)
                    {
                        intAccionStockId = (int)Enumerador.enmAccion.ELIMINAR;
                    }
                    //else
                    //{
                    //    intAccionStockId = (int)Enumerador.enmAccion.INSERTAR;
                    //}
                    
                }
                //----------------------------------------------------

                String StrScriptStock = String.Empty;
                string strMensajeStock = string.Empty;

                switch (intAccionStockId)
                {
                    case (int)Enumerador.enmAccion.INSERTAR:
                        objStock = objeBL.insertar(objStock);
                        break;
                    case (int)Enumerador.enmAccion.MODIFICAR:
                        objStock = objeBL.actualizar(objStock);
                        break;
                    case (int)Enumerador.enmAccion.ELIMINAR:
                        objStock = objeBL.eliminar(objStock);
                        break;
                    default:
                        break;
                }

                string strScript = string.Empty;
                if (!objStock.Error)
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGA INICIAL - LIBROS - ALMACÉN", Constantes.CONST_MENSAJE_EXITO);
                    strScript += Util.NombrarTab(1, Constantes.CONST_TAB_REGISTRAR);
                    strScript += Util.ActivarTab(0, Constantes.CONST_TAB_INICIAL);

                    hdn_sAccionId.Value = ((int)Enumerador.enmAccion.INSERTAR).ToString();
                    HabilitarCamposMantenimiento();
                    LimpiarDatosMantenimiento();
                    gdvLibroCorrelativos.DataSource = null;
                    gdvLibroCorrelativos.DataBind();
                    ctrlPaginador.InicializarPaginador();
                    updConsulta.Update();
                    gdvStockAlmacen.DataSource = null;
                    gdvStockAlmacen.DataBind();
                    ctrlPaginadorStock.InicializarPaginador();
                    updConsulta.Update();
                }
                else
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGA INICIAL - LIBROS - ALMACÉN", Constantes.CONST_MENSAJE_OPERACION_FALLIDA);
                }

                Comun.EjecutarScript(Page, strScript);
            }
        }

        protected void ctrlPaginador_Click(object sender, EventArgs e)
        {
            CargarDatosGrilla();
            updConsulta.Update();
        }
        protected void ctrlPaginadorStock_Click(object sender, EventArgs e)
        {
            CargarDatosGrillaStockAlmacen();
            updConsulta.Update();
        }


        protected void gdvLibroCorrelativos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int intSeleccionadoDet = Convert.ToInt32(e.CommandArgument);
            hdn_sIndiceSeleccionado.Value = intSeleccionadoDet.ToString();

            string vLibroestado = gdvLibroCorrelativos.Rows[intSeleccionadoDet].Cells[Util.ObtenerIndiceColumnaGrilla(gdvLibroCorrelativos, "libr_bCerrado")].Text;

            if (vLibroestado == "True")
                chkEstadoLibro.Checked = true;
            else if (vLibroestado == "False")
                chkEstadoLibro.Checked = false;

            if (e.CommandName == "Consultar")
            {
                hdn_sAccionId.Value = ((int)Enumerador.enmAccion.CONSULTAR).ToString();

            }
            else if (e.CommandName == "Editar")
            {
                hdn_sAccionId.Value = ((int)Enumerador.enmAccion.MODIFICAR).ToString();

            }
            //---------------------------------------------------------------------
            //Fecha: 22/05/2020
            //Autor: Miguel Márquez Beltrán
            //Obs.: No.5. La configuración de carga se encuentra 
            //en estado cerrado sin embargo permite editar 
            //el registro. Y que se muestre como Libros – Escritura Pública.
            //---------------------------------------------------------------------
            // Se incluyo: if (vLibroestado == "False")
            //---------------------------------------------------------------------
            if (vLibroestado == "False")
            {
                HabilitarOpcionesPorAccion();
                PintarSeleccionado();
            }
        }
        #endregion

        #region Métodos
        public void CargarListadosDesplegables()
        {
            if (Convert.ToInt32(hdn_sOficinaConsularId.Value) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ddlOficinaConsular.Cargar(false, true, "- TODAS -");
            }
            else
            {
                ddlOficinaConsular.Cargar(false, false);
            }
            
            //------------------------------------------------------------------------
            // Autor: Sandra del Carmen Acosta Celis
            // Fecha: 16/02/2017
            // Objetivo: Mostrar la oficina consular a la que pertenece, sólo Lima deberá visualizar todas las Oficinas Consulares.
            //------------------------------------------------------------------------

           // ddlOficinaConsular.Cargar(true, true, "- TODOS -");
            Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
            Int16 intOficinaConsularLimaId = Convert.ToInt16(Constantes.CONST_OFICINACONSULAR_LIMA);
            // SI LA OFICINA CONSULAR ES DIFERNTE A LIMA
            if (intOficinaConsularId != intOficinaConsularLimaId)
            {
                ddlOficinaConsular.SelectedValue = Convert.ToString(intOficinaConsularId);
                ddlOficinaConsular.Enabled = false;
            }
           
            Util.CargarComboAnios(ddlPeriodo, DateTime.Now.Year - 10, DateTime.Now.Year);
            ddlPeriodo.SelectedIndex = ddlPeriodo.Items.Count - 1;

            Util.CargarComboAnios(ddlPeriodoMant, DateTime.Now.Year - 10, DateTime.Now.Year);
            ddlPeriodoMant.SelectedIndex = ddlPeriodo.Items.Count - 1;

            DataTable dt = new DataTable();

            dt = Comun.ObtenerOficinasConsularesCargaInicial().Copy();

            //dt = ((DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULAR]).Copy();
            Util.CargarDropDownList(ddlOficinaConsularMant, dt, "ofco_vNombre", "ofco_sOficinaConsularId", true, "-SELECCIONAR-");

            SGAC.Accesorios.Util.CargarParametroDropDownList(ddlLibroTipoMant, comun_Part1.ObtenerParametrosPorGrupo(Session, SGAC.Accesorios.Enumerador.enmGrupo.REG_NOTARIAL_TIPO_LIBRO), true);
            SGAC.Accesorios.Util.CargarParametroDropDownList(ddlTipoInsumo, comun_Part1.ObtenerParametrosPorGrupo(Session, SGAC.Accesorios.Enumerador.enmGrupo.ALMACEN_TIPO_INSUMO), true);


            SGAC.Accesorios.Util.CargarParametroDropDownList(ddlTipoConfiguracion, comun_Part1.ObtenerParametrosPorGrupo(Session, SGAC.Accesorios.Enumerador.enmGrupo.REG_NOTARIAL_TIPO_LIBRO), true);
            SGAC.Accesorios.Util.CargarParametroDropDownList(ddlTipoInsumoConsulta, comun_Part1.ObtenerParametrosPorGrupo(Session, SGAC.Accesorios.Enumerador.enmGrupo.ALMACEN_TIPO_INSUMO), true);
        }
        public void CargarDatosIniciales()
        {
            ddlLibroTipoMant.SelectedValue = ((int)Enumerador.enmNotarialLibroTipo.ESCRITURA_PUBLICA).ToString();
           // HabilitarOpcionesPorAccion();
            //-------------------------------------------------
            lblPeriodoMant.Visible = true;
            ddlPeriodoMant.Visible = true;
            lblLibroNumero.Visible = true;
            txtNumeroLibro.Visible = true;
            lblNumeroEscritura.Visible = true;
            txtNumeroEscritura.Visible = true;
            lblNumeroFolioInicial.Visible = true;
            txtNumFolioInicial.Visible = true;
            lblNumeroFolioActual.Visible = true;
            txtNumFolioActual.Visible = true;
            lblNumeroFoliosTotales.Visible = true;
            txtNumeroFoliosTotales.Visible = true;
            lblEstadoLibro.Visible = true;
            chkEstadoLibro.Visible = true;
            //--------------------------------------------------------
            //Fecha: 23/06/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Poner visible a los objetos.
            //---------------------------------------------------------

            lblEtiquetaFoliosDisponibles.Visible = true;
            lblFoliosDisponibles.Visible = true;
            lblIntervaloFolios.Visible = true;
            txtIntervaloFolios.Visible = true;
            lblTextoIntervalo.Visible = true;
            imgFolioNC.Visible = true;
            btnImprimirFojasNC.Visible = true;
            lblTipoInsumo.Visible = false;
            chkEstadoLibro.Visible = true;
            ddlTipoInsumo.Visible = false;
            lblCantidadStockMinimo.Visible = false;
            txtStockMinimo.Visible = false;
            updMantenimiento.Update();
            //-------------------------------------------------


            Int16 intOficinaConsularIdMant = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);//
            ddlOficinaConsularMant.SelectedValue = Convert.ToString(intOficinaConsularIdMant);
            if (Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                ddlOficinaConsularMant.Enabled = true;
            }
            else
            {
                ddlOficinaConsularMant.Enabled = false;
            }
            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

        }

        public BE.MRE.SI_LIBRO ObtenerDatosLibro()
        {
            BE.MRE.SI_LIBRO objLibro = new BE.MRE.SI_LIBRO();
            objLibro.libr_sLibroId = Convert.ToInt16(hdn_sLibroId.Value);
            objLibro.libr_sOficinaConsularId = Convert.ToInt16(ddlOficinaConsularMant.SelectedValue);
            //--------------------------------------------------------
            //Fecha: 28/12/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Asignar el perido del registro de mantenimiento.
            //---------------------------------------------------------
            //objLibro.libr_sPeriodo = Convert.ToInt16(ddlPeriodo.SelectedItem.Value.ToString());
            objLibro.libr_sPeriodo = Convert.ToInt16(ddlPeriodoMant.SelectedItem.Value.ToString());
            //---------------------------------------------------------
            objLibro.libr_sTipoLibroId = Convert.ToInt16(ddlLibroTipoMant.SelectedValue);
            objLibro.libr_INumeroEscritura = Convert.ToInt32(txtNumeroEscritura.Text);
            objLibro.libr_INumeroLibro = Convert.ToInt32(txtNumeroLibro.Text);
            objLibro.libr_INumeroFolioInicial = Convert.ToInt32(txtNumFolioInicial.Text);
            objLibro.libr_INumeroFolioActual = Convert.ToInt32(txtNumFolioActual.Text);
            objLibro.libr_INumeroFolioTotal = Convert.ToInt32(txtNumeroFoliosTotales.Text);
            objLibro.libr_sUsuarioCreacion = Convert.ToInt16(hdn_sUsuarioId.Value);
            objLibro.libr_vIPCreacion = Util.ObtenerDireccionIP();
            objLibro.libr_sUsuarioModificacion = Convert.ToInt16(hdn_sUsuarioId.Value);
            objLibro.libr_vIPModificacion = Util.ObtenerDireccionIP();
            objLibro.OficinaConsultar = Convert.ToInt16(hdn_sOficinaConsularId.Value);
            objLibro.HostName = Util.ObtenerHostName();
            //--------------------------------------------------------
            //Fecha: 23/06/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Asignar al atributo del rango de folios No corre.
            //---------------------------------------------------------
            objLibro.libr_vRangoFoliosNC = txtIntervaloFolios.Text.Trim();

            return objLibro;
        }

        public BE.MRE.SI_STOCK_ALMACEN ObtenerDatosStock()
        {
            BE.MRE.SI_STOCK_ALMACEN objStock = new BE.MRE.SI_STOCK_ALMACEN();
            objStock.stck_sStockId = Convert.ToInt16(hdn_sStockId.Value);
            objStock.stck_sOficinaConsularId = Convert.ToInt16(ddlOficinaConsularMant.SelectedValue);
            objStock.stck_sInsumoId = Convert.ToInt16(ddlTipoInsumo.SelectedItem.Value.ToString());
            objStock.stck_INumeroStockMinimo = Convert.ToInt32(txtStockMinimo.Text);
            objStock.stck_sUsuarioCreacion = Convert.ToInt16(hdn_sUsuarioId.Value);
            objStock.stck_vIPCreacion = Util.ObtenerDireccionIP();
            objStock.stck_sUsuarioModificacion = Convert.ToInt16(hdn_sUsuarioId.Value);
            objStock.stck_vIPModificacion = Util.ObtenerDireccionIP();
            objStock.OficinaConsultar = Convert.ToInt16(hdn_sOficinaConsularId.Value);
            objStock.HostName = Util.ObtenerHostName();

            return objStock;
        }

        public void LimpiarDatosMantenimiento()
        {
            hdn_sLibroId.Value = "0";
            hdn_sIndiceSeleccionado.Value = "0";

            ctrlToolBarMantenimiento.btnGrabar.Enabled = true;
            ctrlToolBarMantenimiento.btnEditar.Enabled = false;
            ctrlToolBarMantenimiento.btnEliminar.Enabled = false;
            ddlLibroTipoMant.SelectedIndex = 0;
            txtNumeroLibro.Text = string.Empty;
            txtNumeroEscritura.Text = string.Empty;
            txtNumFolioInicial.Text = string.Empty;
            txtNumFolioActual.Text = string.Empty;
            txtNumeroFoliosTotales.Text = string.Empty;
            //--------------------------------------------------------
            //Fecha: 23/06/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Limpiar el objeto txtIntervaloFolios.
            txtIntervaloFolios.Text = string.Empty;
            //--------------------------------------------------------

            HabilitarCamposMantenimiento();
            hdn_sStockId.Value = "0";
            hdn_sIndiceSeleccionado.Value = "0";
            ddlTipoInsumo.SelectedIndex = 0;
            txtStockMinimo.Text = string.Empty;

        }

        public void CargarDatosGrilla()
        {
            bool bolTipoDocSeleccionado = true;
            int intOficinaConsularSel = 0;
            if (Convert.ToInt32(hdn_sOficinaConsularId.Value) == Constantes.CONST_OFICINACONSULAR_LIMA)
            {
                if (ddlOficinaConsular.SelectedIndex > 0)
                    intOficinaConsularSel = Convert.ToInt32(ddlOficinaConsular.SelectedValue);
            }
            else
            {
                intOficinaConsularSel = Convert.ToInt32(ddlOficinaConsular.SelectedValue);
            }

            int intPeriodo = Convert.ToInt32(ddlPeriodo.SelectedValue);

            LibroConsultasBL objBL = new LibroConsultasBL();

            int intPaginaActual = ctrlPaginador.PaginaActual;
            int intPaginaCantidad = ctrlPaginador.PageSize;
            int intTotalRegistros = 0;
            int intTotalPaginas = 0;

            DataTable dtLibros = objBL.obtener(intOficinaConsularSel, intPeriodo, intPaginaActual, intPaginaCantidad,
                ref intTotalRegistros, ref intTotalPaginas);

            if (dtLibros.Rows.Count == 0)
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
            }
            else
            {
                ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + intTotalRegistros, true, Enumerador.enmTipoMensaje.INFORMATION);
            }

            Session["DT_LIBROS"] = dtLibros;
            gdvLibroCorrelativos.DataSource = dtLibros;
            gdvLibroCorrelativos.DataBind();

            ctrlPaginador.TotalResgistros = intTotalRegistros;
            ctrlPaginador.TotalPaginas = intTotalPaginas;
            ctrlPaginador.Visible = false;
            if (ctrlPaginador.TotalPaginas > 1)
            {
                ctrlPaginador.Visible = true;
            }
        }

        public void CargarDatosGrillaStockAlmacen()
        {
            bool bolTipoDocSeleccionado = true;
            if (ddlTipoInsumoConsulta.SelectedIndex > 0)
            {
                int intOficinaConsularSel = 0;
                if (Convert.ToInt32(hdn_sOficinaConsularId.Value) == Constantes.CONST_OFICINACONSULAR_LIMA)
                {
                    if (ddlOficinaConsular.SelectedIndex > 0)
                        intOficinaConsularSel = Convert.ToInt32(ddlOficinaConsular.SelectedValue);
                }
                else
                {
                    intOficinaConsularSel = Convert.ToInt32(ddlOficinaConsular.SelectedValue);
                }
                int intTipoConfiguracion = 0;
                if (ddlTipoConfiguracion.SelectedValue == ((int)Enumerador.enmGrupo.REG_NOTARIAL_TIPO_LIBRO).ToString())
                {
                    if (ddlTipoConfiguracion.SelectedIndex > 0)
                        intTipoConfiguracion = Convert.ToInt32(ddlTipoConfiguracion.SelectedValue);
                }
                else
                {
                    intTipoConfiguracion = Convert.ToInt32(ddlTipoConfiguracion.SelectedValue);
                }

                int intTipoInsumo = 0;
                if ((ddlTipoInsumoConsulta.SelectedValue) == SGAC.Accesorios.Enumerador.enmGrupo.ALMACEN_TIPO_INSUMO.ToString())
                {
                    if (ddlTipoInsumoConsulta.SelectedIndex > 0)
                        intTipoInsumo = Convert.ToInt32(ddlTipoInsumoConsulta.SelectedValue);
                }
                else
                {
                    intTipoInsumo = Convert.ToInt32(ddlTipoInsumoConsulta.SelectedValue);
                }
                

                StockConsultaBL objBL = new StockConsultaBL();

                int intPaginaActual = ctrlPaginadorStock.PaginaActual;
                int intPaginaCantidad = ctrlPaginadorStock.PageSize;
                int intTotalRegistros = 0;
                int intTotalPaginas = 0;

                DataTable dtStockMinimo = objBL.obtener(intOficinaConsularSel, intTipoInsumo, intPaginaActual, intPaginaCantidad,
                    ref intTotalRegistros, ref intTotalPaginas);

                if (dtStockMinimo.Rows.Count == 0)
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_VALIDACION_BUSQUEDA);
                }
                else
                {
                    ctrlValidacion.MostrarValidacion(Constantes.CONST_MENSAJE_BUSQUEDA_EXITO + intTotalRegistros, true, Enumerador.enmTipoMensaje.INFORMATION);
                }

                Session["DT_STOCK_ALMACEN"] = dtStockMinimo;
                gdvStockAlmacen.DataSource = dtStockMinimo;
                gdvStockAlmacen.DataBind();

                ctrlPaginadorStock.TotalResgistros = intTotalRegistros;
                ctrlPaginadorStock.TotalPaginas = intTotalPaginas;
                ctrlPaginadorStock.Visible = false;
                if (ctrlPaginadorStock.TotalPaginas > 1)
                {
                    ctrlPaginadorStock.Visible = true;
                }
            }

            else
            {
                bolTipoDocSeleccionado = false;
            }

            if (!bolTipoDocSeleccionado)
            {
                ctrlValidacion.MostrarValidacion("Seleccione un Tipo de Insumo", true, Enumerador.enmTipoMensaje.WARNING);
            }
        }

        public void HabilitarCamposMantenimiento(bool bolHabilitar = true)
        {
            ddlLibroTipoMant.Enabled = bolHabilitar;

            txtNumeroEscritura.Enabled = bolHabilitar;

            txtNumeroLibro.Enabled = bolHabilitar;
            txtNumFolioInicial.Enabled = bolHabilitar;
            txtNumFolioActual.Enabled = bolHabilitar;
            txtNumeroFoliosTotales.Enabled = bolHabilitar;
            //--------------------------------------------------------
            //Fecha: 23/06/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Habilitar o deshabilitar el rango de folios.
            //---------------------------------------------------------
            txtIntervaloFolios.Enabled = bolHabilitar;

            ddlTipoInsumo.Enabled = bolHabilitar; 
            txtStockMinimo.Enabled = bolHabilitar; 
        }
        public void HabilitarOpcionesPorAccion()
        {
            string strScript = string.Empty;
            int intAccion = Convert.ToInt32(hdn_sAccionId.Value);

            switch (intAccion)
            {
                case (int)Enumerador.enmAccion.INSERTAR:
                    HabilitarCamposMantenimiento();
                    LimpiarDatosMantenimiento();

                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_REGISTRAR);
                    break;
                case (int)Enumerador.enmAccion.MODIFICAR:
                    HabilitarCamposMantenimiento();

                    ctrlToolBarMantenimiento.btnEditar.Enabled = false;
                    ctrlToolBarMantenimiento.btnEliminar.Enabled = true;
                    ctrlToolBarMantenimiento.btnGrabar.Enabled = true;

                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
                    break;
                case (int)Enumerador.enmAccion.ELIMINAR:
                    HabilitarCamposMantenimiento();

                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_EDITAR);
                    break;
                default:
                    HabilitarCamposMantenimiento(false);

                    ctrlToolBarMantenimiento.btnNuevo.Enabled = true;
                    ctrlToolBarMantenimiento.btnEditar.Enabled = true;
                    ctrlToolBarMantenimiento.btnEliminar.Enabled = false;

                    ctrlToolBarMantenimiento.btnGrabar.Enabled = false;

                    strScript = Util.ActivarTab(1, Constantes.CONST_TAB_CONSULTAR);
                    break;
            }
            Comun.EjecutarScript(Page, strScript);
        }
        public void PintarSeleccionado()
        {
            lblPeriodoMant.Visible = true;
            ddlPeriodoMant.Visible = true;
            lblLibroNumero.Visible = true;
            txtNumeroLibro.Visible = true;
            lblNumeroEscritura.Visible = true;
            txtNumeroEscritura.Visible = true;
            lblNumeroFolioInicial.Visible = true;
            txtNumFolioInicial.Visible = true;
            lblNumeroFolioActual.Visible = true;
            txtNumFolioActual.Visible = true;
            lblNumeroFoliosTotales.Visible = true;
            txtNumeroFoliosTotales.Visible = true;
            lblEstadoLibro.Visible = true;
            chkEstadoLibro.Visible = true;
            //--------------------------------------------------------
            //Fecha: 23/06/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Poner visible a los objetos.
            //---------------------------------------------------------
            lblEtiquetaFoliosDisponibles.Visible = true;
            lblFoliosDisponibles.Visible = true;
            lblIntervaloFolios.Visible = true;
            txtIntervaloFolios.Visible = true;
            lblTextoIntervalo.Visible = true;
            imgFolioNC.Visible = true;
            btnImprimirFojasNC.Visible = true;
            //---------------------------------------------------------

            lblTipoInsumo.Visible = false;
            ddlTipoInsumo.Visible = false;
            lblCantidadStockMinimo.Visible = false;
            txtStockMinimo.Visible = false;

            DataTable dt = (DataTable)Session["DT_LIBROS"];

            if (dt == null)
                return;
            if (dt.Rows.Count == 0)
                return;

            LibroConsultasBL objBL = new LibroConsultasBL();
            BE.MRE.SI_LIBRO objLibro = new BE.MRE.SI_LIBRO();

            objLibro.libr_sLibroId = Convert.ToInt16(dt.Rows[Convert.ToInt32(hdn_sIndiceSeleccionado.Value)][0]);

            objLibro = objBL.Consultar(objLibro);

            hdn_sLibroId.Value = objLibro.libr_sLibroId.ToString();
            ddlOficinaConsularMant.SelectedValue = Convert.ToString(objLibro.libr_sOficinaConsularId);
            ddlLibroTipoMant.SelectedValue = Convert.ToString(objLibro.libr_sTipoLibroId);
            ddlPeriodoMant.SelectedValue = Convert.ToString(objLibro.libr_sPeriodo);
            txtNumeroLibro.Text = Convert.ToString(objLibro.libr_INumeroLibro);
            txtNumeroEscritura.Text = Convert.ToString(objLibro.libr_INumeroEscritura);
            txtNumFolioInicial.Text = Convert.ToString(objLibro.libr_INumeroFolioInicial);
            txtNumFolioActual.Text = Convert.ToString(objLibro.libr_INumeroFolioActual);
            txtNumeroFoliosTotales.Text = Convert.ToString(objLibro.libr_INumeroFolioTotal);
            //--------------------------------------------------------
            //Fecha: 23/06/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Asignar al atributo del rango de folios No corre.
            //---------------------------------------------------------
            txtIntervaloFolios.Text = objLibro.libr_vRangoFoliosNC.ToString().Trim();

            int intNroFoliosTotales = 0;
            int intNroFolioActual = 0;

            if (txtNumeroFoliosTotales.Text.Trim().Length > 0)
            {
                intNroFoliosTotales = Convert.ToInt32(txtNumeroFoliosTotales.Text.Trim());
            }
            if (txtNumFolioActual.Text.Trim().Length > 0)
            {
                intNroFolioActual = Convert.ToInt32(txtNumFolioActual.Text.Trim());                
            }
            lblFoliosDisponibles.Text = (intNroFoliosTotales - intNroFolioActual + 1).ToString();

            updMantenimiento.Update();
        }
        #endregion

        protected void ddlLibroTipoMant_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Comun.ToNullInt32(ddlLibroTipoMant.SelectedValue))
            {
                case (int)Enumerador.enmNotarialLibroTipo.ESCRITURA_PUBLICA:
                    lblPeriodoMant.Visible = true;
                    ddlPeriodoMant.Visible = true;
                    lblLibroNumero.Visible = true;
                    txtNumeroLibro.Visible = true;
                    lblNumeroEscritura.Visible = true;
                    txtNumeroEscritura.Visible = true;
                    lblNumeroFolioInicial.Visible = true;
                    txtNumFolioInicial.Visible = true;
                    lblNumeroFolioActual.Visible = true;
                    txtNumFolioActual.Visible = true;
                    lblNumeroFoliosTotales.Visible = true;
                    txtNumeroFoliosTotales.Visible = true;
                    lblEstadoLibro.Visible = true;
                    chkEstadoLibro.Visible = true;

                    //--------------------------------------------------------
                    //Fecha: 23/06/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Poner invisible a los objetos.
                    //---------------------------------------------------------
                    lblEtiquetaFoliosDisponibles.Visible = true;
                    lblFoliosDisponibles.Visible = true;
                    lblIntervaloFolios.Visible = true;
                    txtIntervaloFolios.Visible = true;
                    lblTextoIntervalo.Visible = true;
                    imgFolioNC.Visible = true;
                    btnImprimirFojasNC.Visible = true;
                    //---------------------------------------------------------

                    lblTipoInsumo.Visible = false;
                    chkEstadoLibro.Visible = true;
                    ddlTipoInsumo.Visible = false;
                    lblCantidadStockMinimo.Visible = false;
                    txtStockMinimo.Visible = false;
                    break;
            }

            if (ddlLibroTipoMant.SelectedItem.Text == Constantes.CONST_ALMACEN.ToString())
            {
                    lblPeriodoMant.Visible = false;
                    ddlPeriodoMant.Visible = false;
                    lblLibroNumero.Visible = false;
                    txtNumeroLibro.Visible = false;
                    lblNumeroEscritura.Visible = false;
                    txtNumeroEscritura.Visible = false;
                    lblNumeroFolioInicial.Visible = false;
                    txtNumFolioInicial.Visible = false;
                    lblNumeroFolioActual.Visible = false;
                    txtNumFolioActual.Visible = false;
                    lblNumeroFoliosTotales.Visible = false;
                    txtNumeroFoliosTotales.Visible = false;
                    //--------------------------------------------------------
                    //Fecha: 23/06/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Poner visible a los objetos.
                    //---------------------------------------------------------
                    lblEtiquetaFoliosDisponibles.Visible = false;
                    lblFoliosDisponibles.Visible = false;
                    lblIntervaloFolios.Visible = false;
                    txtIntervaloFolios.Visible = false;
                    lblTextoIntervalo.Visible = false;
                    imgFolioNC.Visible = false;
                    btnImprimirFojasNC.Visible = false;
                    //---------------------------------------------------------

                    lblEstadoLibro.Visible = false;
                    lblTipoInsumo.Visible = true;
                    chkEstadoLibro.Visible = false;
                    ddlTipoInsumo.Visible = true;
                    lblCantidadStockMinimo.Visible = true;
                    txtStockMinimo.Visible = true;

            }

            updMantenimiento.Update();
        }

        protected void ddlTipoConfiguracion_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Comun.ToNullInt32(ddlTipoConfiguracion.SelectedValue))
            {
                case (int)Enumerador.enmNotarialLibroTipo.ESCRITURA_PUBLICA:
                    lblTipoInsumoConsulta.Visible = false;
                    ddlTipoInsumoConsulta.Visible = false;
                    lblPeriodo.Visible = true;
                    ddlPeriodo.Visible = true;

                    pnlLibroCorrelativos.Visible = true;
                    pnlStockAlmacen.Visible = false;
                    break;
            }
            if (ddlTipoConfiguracion.SelectedItem.Text == Constantes.CONST_ALMACEN.ToString())
            {
                lblTipoInsumoConsulta.Visible = true;
                ddlTipoInsumoConsulta.Visible = true;
                lblPeriodo.Visible = false;
                ddlPeriodo.Visible = false;

                pnlLibroCorrelativos.Visible = false;
                pnlStockAlmacen.Visible = true;
            }
            updMantenimiento.Update();
        }

        protected void gdvStockAlmacen_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int intSeleccionadoDet = Convert.ToInt32(e.CommandArgument);
            hdn_sIndiceSeleccionado.Value = intSeleccionadoDet.ToString();

            string vStock = gdvStockAlmacen.Rows[intSeleccionadoDet].Cells[Util.ObtenerIndiceColumnaGrilla(gdvStockAlmacen, "stck_sStockId")].Text;

            if (e.CommandName == "Consultar")
            {
                hdn_sAccionId.Value = ((int)Enumerador.enmAccion.CONSULTAR).ToString();

            }
            else if (e.CommandName == "Editar")
            {
                hdn_sAccionId.Value = ((int)Enumerador.enmAccion.MODIFICAR).ToString();

            }
            HabilitarOpcionesPorAccion();
            PintarSeleccionadoStockAlmacen();
        }

        public void PintarSeleccionadoStockAlmacen()
        {
            lblTipoInsumo.Visible = true;
            ddlTipoInsumo.Visible = true;
            lblCantidadStockMinimo.Visible = true;
            txtStockMinimo.Visible = true;

            lblPeriodoMant.Visible = false;
            ddlPeriodoMant.Visible = false;
            lblLibroNumero.Visible = false;
            txtNumeroLibro.Visible = false;
            lblNumeroEscritura.Visible = false;
            txtNumeroEscritura.Visible = false;
            lblNumeroFolioInicial.Visible = false;
            txtNumFolioInicial.Visible = false;
            lblNumeroFolioActual.Visible = false;
            txtNumFolioActual.Visible = false;
            lblNumeroFoliosTotales.Visible = false;
            txtNumeroFoliosTotales.Visible = false;
            //--------------------------------------------------------
            //Fecha: 23/06/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Poner visible a los objetos.
            //---------------------------------------------------------
            lblEtiquetaFoliosDisponibles.Visible = false;
            lblFoliosDisponibles.Visible = false;
            lblIntervaloFolios.Visible = false;
            txtIntervaloFolios.Visible = false;
            lblTextoIntervalo.Visible = false;
            imgFolioNC.Visible = false;
            btnImprimirFojasNC.Visible = false;
            //---------------------------------------------------------
            lblEstadoLibro.Visible = false;
            chkEstadoLibro.Visible = false;

            DataTable dt = (DataTable)Session["DT_STOCK_ALMACEN"];

            if (dt == null)
                return;
            if (dt.Rows.Count == 0)
                return;

            StockConsultaBL objBL = new StockConsultaBL();
            BE.MRE.SI_STOCK_ALMACEN objStock = new BE.MRE.SI_STOCK_ALMACEN();

            objStock.stck_sStockId = Convert.ToInt16(dt.Rows[Convert.ToInt32(hdn_sIndiceSeleccionado.Value)][0]);

            objStock.stck_sInsumoId = Convert.ToInt16(dt.Rows[Convert.ToInt32(hdn_sIndiceSeleccionado.Value)][3]);



            objStock = objBL.Consultar(objStock);

            hdn_sStockId.Value = objStock.stck_sStockId.ToString();
            hdn_sTipoInsumo.Value = objStock.stck_sInsumoId.ToString();
            
            for (int i = 0; i < ddlLibroTipoMant.Items.Count; i++)
            {
                if (ddlLibroTipoMant.Items[i].Text.Equals(Constantes.CONST_ALMACEN.ToString()))
                {
                    ddlLibroTipoMant.SelectedValue = ddlLibroTipoMant.Items[i].Value;
                }
            }
            ddlOficinaConsularMant.SelectedValue = Convert.ToString(objStock.stck_sOficinaConsularId);
            ddlTipoInsumo.SelectedValue = Convert.ToString(objStock.stck_sInsumoId);
            txtStockMinimo.Text = Convert.ToString(objStock.stck_INumeroStockMinimo);
            updMantenimiento.Update();
        }

        //----------------------------------------------------
        //Fecha: 14/02/2017
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Obtener el id del stock 
        //----------------------------------------------------

        private Int16 ObtenerStockId()
        {
            int intOficinaConsularSel = Convert.ToInt32(ddlOficinaConsular.SelectedValue);
            int intTipoInsumo = Convert.ToInt32(ddlTipoInsumoConsulta.SelectedValue);
            int intPaginaActual = 1;
            int intPaginaCantidad = 10;
            int intTotalRegistros = 0;
            int intTotalPaginas = 0;

            StockConsultaBL objBL = new StockConsultaBL();
            DataTable dt = new DataTable();

            Int16 intStockId = 0;
            dt = objBL.obtener(intOficinaConsularSel, intTipoInsumo, intPaginaActual, intPaginaCantidad, ref intTotalRegistros, ref intTotalPaginas);

            if (dt.Rows.Count > 0)
            {
                intStockId = Convert.ToInt16(dt.Rows[0]["stck_sStockId"].ToString());
            }
            return intStockId;
        }
        //-------------------------------------------------
        //Fecha: 08/07/2020
        //Autor: Miguel Márquez Beltrán
        //Motivo: validar el registro Libro
        //-------------------------------------------------

        private bool validarRegistroLibro()
        {
            bool esCorrecto = true;
            int intNumeroLibro = 0;
            int intNumeroEscritura = 0;
            int intNumeroFolioInicial = 0;
            int intNumeroFolioActual = 0;
            int intNumeroFolioTotales = 0;

            if (txtNumeroLibro.Text.Length == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGA INICIAL - LIBROS - ALMACÉN", "Digite el Número del Libro."));
                return false;
            }
            //--------------------------------------------
            //Fecha: 20/07/2020
            //Autor: Miguel Márquez
            //Motivo: Validar el Número de Libro.
            //--------------------------------------------
            intNumeroLibro = Convert.ToInt32(txtNumeroLibro.Text);
            if (intNumeroLibro <= 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGA INICIAL - LIBROS - ALMACÉN", "El Número del Libro debe ser mayor a cero."));
                return false;
            }
            //--------------------------------------------

            if (txtNumeroEscritura.Text.Length == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGA INICIAL - LIBROS - ALMACÉN", "Digite el Número de Escritura."));
                return false;
            }
            //--------------------------------------------
            //Fecha: 20/07/2020
            //Autor: Miguel Márquez
            //Motivo: Validar el Número de Escritura.
            //--------------------------------------------
            intNumeroEscritura = Convert.ToInt32(txtNumeroEscritura.Text);
            if (intNumeroEscritura <= 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGA INICIAL - LIBROS - ALMACÉN", "El Número de Escritura debe ser mayor a cero."));
                return false;
            }
            //--------------------------------------------

            if (txtNumFolioInicial.Text.Length == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGA INICIAL - LIBROS - ALMACÉN", "Digite el Número de Folio Inicial."));
                return false;
            }
            //--------------------------------------------
            //Fecha: 20/07/2020
            //Autor: Miguel Márquez
            //Motivo: Validar el Número de Folio Inicial.
            //--------------------------------------------
            intNumeroFolioInicial = Convert.ToInt32(txtNumFolioInicial.Text);
            if (intNumeroFolioInicial <= 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGA INICIAL - LIBROS - ALMACÉN", "El Número de Folio Inicial debe ser mayor a cero."));
                return false;
            }
            //--------------------------------------------

            if (txtNumFolioActual.Text.Length == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGA INICIAL - LIBROS - ALMACÉN", "Digite el Número de Folio Actual."));
                return false;
            }
            //--------------------------------------------
            //Fecha: 20/07/2020
            //Autor: Miguel Márquez
            //Motivo: Validar el Número de Folio Actual.
            //--------------------------------------------
            intNumeroFolioActual = Convert.ToInt32(txtNumFolioActual.Text);
            if (intNumeroFolioActual <= 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGA INICIAL - LIBROS - ALMACÉN", "El Número de Folio Actual debe ser menor o igual a cero."));
                return false;
            }
            //--------------------------------------------

            if (txtNumeroFoliosTotales.Text.Length == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGA INICIAL - LIBROS - ALMACÉN", "Digite el Número de Folio Totales."));

                return false;
            }
            //--------------------------------------------
            //Fecha: 20/07/2020
            //Autor: Miguel Márquez
            //Motivo: Validar el Número de Folios Totales.
            //--------------------------------------------
            intNumeroFolioTotales = Convert.ToInt32(txtNumeroFoliosTotales.Text);
            if (intNumeroFolioTotales <= 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGA INICIAL - LIBROS - ALMACÉN", "El Número de Folios Totales debe ser mayor a cero."));
                return false;
            }
            //--------------------------------------------
            return esCorrecto;
        }
        //-------------------------------------------------
        //Fecha: 08/07/2020
        //Autor: Miguel Márquez Beltrán
        //Motivo: validar el registro de almacen
        //-------------------------------------------------

        private bool validarRegistroAlmacen()
        {
            bool esCorrecto = true;

            if (ddlTipoInsumo.SelectedIndex == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGA INICIAL - LIBROS - ALMACÉN", "Seleccione el tipo de insumo."));
                esCorrecto = false;
            }
            if (txtStockMinimo.Text.Length == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "CARGA INICIAL - LIBROS - ALMACÉN", "Digite el stock minimo."));
                esCorrecto = false;
            }
            return esCorrecto;
        }
        //---------------------------------------------
        //Fecha: 08/07/2020
        //Autor: Miguel Márquez Beltrán
        //Motivo: Ocultar el botón editar cuando el
        //        estado este cerrado.
        //---------------------------------------------
        protected void gdvLibroCorrelativos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                ImageButton btnEditar = (ImageButton)e.Row.FindControl("btnEditar");

                Label lblestado = (Label)e.Row.FindControl("lblestado");

                if (lblestado.Text == "CERRADO")
                {
                    btnEditar.Visible = false;
                }
                else
                {
                    btnEditar.Visible = true;
                }
            }
        }

        //-----------------------------------------
        //Fecha: 23/06/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Impresión de las fojas No Corre.
        //-----------------------------------------
        protected void btnImprimirFojasNC_Click(object sender, EventArgs e)
        {
            string strIntervaloFolios = txtIntervaloFolios.Text.Trim();
            //-------------------------------------
            if (strIntervaloFolios.Length > 0)
            {
                string[] ListaRangos = strIntervaloFolios.Split(',');
                int[,] Rangos = new int[ListaRangos.Length, 2];
                Rangos = obtieneRangoFojasInicioFin(ListaRangos);
                
                //-------------------------------------
                // Imprimir
                //-------------------------------------
                Comun.CrearDocumentoiTextSharpTextoDiagonal(this.Page, "NO CORRE", Rangos);                
            }
            //-------------------------------------
        }

        //------------------------------------------------------------
        //Fecha: 23/06/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Obtienes la lista de fojas de inicio y fin.
        //Entrada: 1,5,8-10
        //Salida: [0, 0] = 1, [0, 1] = 1, [1, 0] = 5, [1, 1] = 5
        //        [2, 0] = 8, [2, 1] = 10  
        //------------------------------------------------------------
        private int[,] obtieneRangoFojasInicioFin(string[] ListaRangos)
        {            
            int[,] Rangos = new int[ListaRangos.Length, 2];
            int intRi = 0;
            int intRf = 0;
            int intIndice = 0;

            for (int i = 0; i < ListaRangos.Length; i++)
            {
                if (ListaRangos[i].IndexOf("-") > -1)
                {
                    intIndice = ListaRangos[i].IndexOf("-");
                    intRi = Convert.ToInt32(ListaRangos[i].Substring(0, intIndice));
                    intRf = Convert.ToInt32(ListaRangos[i].Substring(intIndice + 1));
                    Rangos[i, 0] = intRi;
                    Rangos[i, 1] = intRf;
                }
                else
                {
                    Rangos[i, 0] = Convert.ToInt32(ListaRangos[i]);
                    Rangos[i, 1] = Convert.ToInt32(ListaRangos[i]);
                }
            }

            return Rangos;
        }
            
//--------------------------------------------------
    }
}
