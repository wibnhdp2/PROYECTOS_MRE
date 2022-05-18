using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using SGAC.Accesorios;
using SGAC.BE;
using SGAC.BE.MRE.Custom;
using SGAC.Controlador;
using SGAC.WebApp.Accesorios;
using SGAC.Registro.Persona.BL;
using System.Web.Script.Serialization;
using Microsoft.Security.Application;


namespace SGAC.WebApp.Registro
{
    public partial class FrmRegistroEmpresa : MyBasePage
    {
        #region Campos
        private string strVarRepresentanteDt = "DT_EMP_REPRESENTANTE";
        private string strVarDireccionDt = "DT_EMP_DIRECCION";
        private string strVarRepresentanteIndiceSel = "ID_EMP_REPRESENTANTE";
        private string strVarDireccionIndiceSel = "ID_EMP_REPRESENTANTE";
        private const string strVarAccion = "ACCION_EMP_VALIDAR_DEBUG";
        private const string strVarAccionRep = "ACCION_EMP_REP_VALIDAR_DEBUG";
        private const string strVarAccionDir = "ACCION_EMP_DIR_VALIDAR_DEBUG";
        private const string strNombreEntidad = "EMPRESA";
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {                        
            ctrlToolBar.VisibleIButtonGrabar = true;
            ctrlToolBar.VisibleIButtonNuevo = true;
            ctrlToolBar.VisibleIButtonBuscar = true;
            ctrlToolBar.VisibleIButtonEliminar = true;
            ctrlToolBar.VisibleIButtonSalir = true;
            ctrlToolBar.btnNuevoHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonNuevoClick(ctrlToolBar_btnNuevoHandler);
            ctrlToolBar.btnGrabarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonGrabarClick(ctrlToolBar_btnGrabarHandler);
            ctrlToolBar.btnBuscarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonBuscarClick(ctrlToolBar_btnBuscarHandler);
            ctrlToolBar.btnEliminarHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonEliminarClick(ctrlToolBar_btnEliminarHandler);
            ctrlToolBar.btnSalirHandler += new Accesorios.SharedControls.ctrlToolBarConfirm.OnButtonSalirClick(ctrlToolBar_btnSalirHandler);

            Button BtnGrabarE = (Button)ctrlToolBar.FindControl("btnGrabar");
            BtnGrabarE.OnClientClick = "return ValidarGrabar()";

            Button BtnEliminarE = (Button)ctrlToolBar.FindControl("btnEliminar");
            BtnEliminarE.Enabled = false;

            Comun.CargarPermisos(Session, ctrlToolBar, null, null, HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["CodPer"] != null)
                {
                    string codPersona = Util.DesEncriptar(Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString()));
                    if (Convert.ToInt64(codPersona) > 0)
                    {
                        GetDataPersona(Convert.ToInt64(codPersona));
                    }
                }
                
                Session["BUSQUEDA_TIPO_PERSONA"] = (int)Enumerador.enmTipoPersona.JURIDICA;

                Session[strVarAccion] = Enumerador.enmAccion.INSERTAR;
                CargarListadosDesplegables();
                Session["strVarRepresentanteDt"] = dtCargaRepresentante();
                Session["strVarDireccionDt"] = dtCargaResidencia();
                ViewState.Add("iRepresentanteId", 0);
                
                ViewState.Add(strVarAccionRep, Enumerador.enmAccion.INSERTAR);
                ViewState.Add(strVarAccionDir, Enumerador.enmAccion.INSERTAR);

                if (ViewState["iPersonaId"] != null)
                {
                        ViewState.Add("iEmpresaId", Convert.ToInt64(ViewState["iPersonaId"]));
                        CargarDatosEmpresa();
                }

                lblValidacion.Text = "";

                HF_ValoresDocumentoIdentidad.Value = string.Empty;

                DataTable dt = new DataTable();
                dt = Comun.ObtenerListaDocumentoIdentidad();

                //DataTable dt = (DataTable)Session[Constantes.CONST_SESION_DT_DOCUMENTOIDENTIDAD];

                foreach (DataRow dr in dt.Rows)
                {
                    HF_ValoresDocumentoIdentidad.Value += dr["doid_sTipoDocumentoIdentidadId"].ToString() + "," +
                        dr["doid_sDigitosMinimo"].ToString() + "," + dr["doid_sDigitos"].ToString() + "," +
                     dr["doid_bNumero"].ToString() + "," + dr["doid_sTipoNacionalidad"].ToString() + "," +
                     dr["vMensajeError"].ToString() + "|";
                }


                HF_TipoDocumento_Representante_Columna_Indice.Value = Util.ObtenerIndiceColumnaGrilla(gdvRepresentante, "sDocumentoTipoId").ToString();
                HF_NumeroDocumento_Representante_Columna_Indice.Value = Util.ObtenerIndiceColumnaGrilla(gdvRepresentante, "vDocumentoNumero").ToString();
            }

            ValidarDocumentos();
            if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA")
            {
                Button[] arrButtons = { ctrlToolBar.btnNuevo, ctrlToolBar.btnGrabar, ctrlToolBar.btnEliminar, btnAgregarRepresentante, btnAgregarDireccion};
                Comun.ModoLectura(ref arrButtons);
            }
        }

        private void ValidarDocumentos()
        {
            string vTipoDoc = ddlRepresentanteTipoDoc.SelectedValue;

            if (Convert.ToInt16(vTipoDoc) > 0)
            {
                DataTable dt = new DataTable();

                dt = Comun.ObtenerListaDocumentoIdentidad();

                //DataTable dt = (DataTable)Session[Constantes.CONST_SESION_DT_DOCUMENTOIDENTIDAD];

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["doid_sTipoDocumentoIdentidadId"].ToString() == vTipoDoc)
                    {
                        txtRepresentanteNroDoc.MaxLength = Convert.ToInt16(dr["doid_sDigitos"]);

                        if (dr["doid_sTipoNacionalidad"].ToString()!=string.Empty)    
                            ddlNacionalidad.Enabled= false;
                        else
                            ddlNacionalidad.Enabled=true;
                     

                        if(Convert.ToBoolean(dr["doid_bNumero"]))                     
                            hidDocumentoSoloNumero.Value="1";                       
                        else                       
                            hidDocumentoSoloNumero.Value="0";
                       
                    }                  
                }
            }
        }

        void ctrlToolBar_btnSalirHandler()
        {
            LimpiaCampos();
            Response.Redirect("~/Default.aspx");
        }

        void ctrlToolBar_btnEliminarHandler()
        {
            try
            {                
                //Proceso p = new Proceso();

                Int64 intResultado = 0;

                #region - llena la entidad -

                SGAC.BE.RE_EMPRESA objEmp = new BE.RE_EMPRESA();
                objEmp.empr_iEmpresaId = Convert.ToInt64(ViewState["iEmpresaId"]);
                objEmp.empr_sTipoDocumentoId = Convert.ToInt16(ddlTipoDocumento.SelectedValue);
                objEmp.empr_vNumeroDocumento = txtNroDocumento.Text;
                objEmp.empr_vRazonSocial = txtRazonSocial.Text;
                objEmp.empr_sTipoEmpresaId = Convert.ToInt16(ddlTipoEmpresa.SelectedValue);
                objEmp.empr_vTelefono = txtTelefono.Text;
                objEmp.empr_vActividadComercial = txtActividadComercial.Text;
                objEmp.empr_vCorreo = txtCorreo.Text;

                objEmp.empr_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objEmp.empr_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objEmp.empr_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objEmp.empr_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objEmp.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                #endregion
                //object[] arrParEliminar = { objEmp };
                //intResultado = Convert.ToInt64(p.Invocar(ref arrParEliminar, "SGAC.BE.RE_EMPRESA", Enumerador.enmAccion.ELIMINAR));

                EmpresaMantenimientoBL objEmpresaMantenimientoBL = new EmpresaMantenimientoBL();
                intResultado = objEmpresaMantenimientoBL.Eliminar(objEmp);


                if (intResultado == 0)
                {
                    lblValidacion.Attributes.Remove("class");
                    lblValidacion.Text = "No se puede eliminar empresa porque tiene actuaciones registradas";
                }
                else
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, Constantes.CONST_MENSAJE_ELIMINADO));
                    LimpiaCampos();
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

        void ctrlToolBar_btnBuscarHandler()
        {
            ViewState.Add("AccionDocumento", "Nuevo");
            Session["strBusqueda"] = "R";
            Session["iTipoId"] = (int)Enumerador.enmTipoPersona.JURIDICA;
            Response.Redirect("~/Registro/FrmBusquedaPersona.aspx");                        
        }
        
        void ctrlToolBar_btnGrabarHandler()
        {
            try
            {
                if (ddlTipoDocumento.SelectedItem.Text.Equals("RUC"))
                {
                    //Validar el número de ruc
                    if (txtNroDocumento.Text.Trim().Length != 11)
                    {
                        lblValidacion.Text = Constantes.CONST_RUC_NO_VALIDO;
                        txtNroDocumento.Focus();
                        return;
                    }
                }
                
                object[] arrParEmpresa = ObtenerFiltro();
                DataTable dt = new DataTable();
                lblValidacion.Attributes.Add("class", "hideControl");
                
                Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session[strVarAccion];

                if (enmAccion == Enumerador.enmAccion.INSERTAR)
                {
                    Int64 iExiste = 0;
                    iExiste = SGAC.WebApp.Accesorios.Empresa.Empresa_Existe(arrParEmpresa);
                    if (iExiste > 0)
                    {
                        lblValidacion.Attributes.Remove("class");
                        lblValidacion.Text = "La Empresa ya está Registrada";
                        
                        return;
                    }
                }
                int CantRepresentante = gdvRepresentante.Rows.Count;
                int CantDireccion = gdvDirecciones.Rows.Count;

                Int64 intResultado = 0;

                DataTable dtRepresentante = new DataTable();
                DataTable dtdireccion = new DataTable();
                
                dtRepresentante = (DataTable)Session["strVarRepresentanteDt"];
                dtdireccion = (DataTable)Session["strVarDireccionDt"];


                #region - llena la entidad -
                
                SGAC.BE.RE_EMPRESA objEmp = new BE.RE_EMPRESA();
                objEmp.empr_iEmpresaId = Convert.ToInt64(ViewState["iEmpresaId"]);
                objEmp.empr_sTipoDocumentoId = Convert.ToInt16(ddlTipoDocumento.SelectedValue);
                objEmp.empr_vNumeroDocumento = txtNroDocumento.Text;
                objEmp.empr_vRazonSocial = txtRazonSocial.Text;
                objEmp.empr_sTipoEmpresaId = Convert.ToInt16(ddlTipoEmpresa.SelectedValue);
                objEmp.empr_vTelefono = txtTelefono.Text;
                objEmp.empr_vActividadComercial = txtActividadComercial.Text;
                objEmp.empr_vCorreo = txtCorreo.Text;

                objEmp.empr_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objEmp.empr_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objEmp.empr_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objEmp.empr_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objEmp.OficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                #endregion   

                #region MDIAZ - rune rápido BE.MRE
                PersonaConsultaBL objPersonaBL = new PersonaConsultaBL();

                BE.MRE.RE_EMPRESA objEmpresa = new BE.MRE.RE_EMPRESA();
                objEmpresa.empr_iEmpresaId = Convert.ToInt64(ViewState["iEmpresaId"]);
                objEmpresa.empr_sTipoDocumentoId = Convert.ToInt16(ddlTipoDocumento.SelectedValue);
                objEmpresa.empr_vNumeroDocumento = txtNroDocumento.Text;
                objEmpresa.empr_vRazonSocial = txtRazonSocial.Text;
                objEmpresa.empr_sTipoEmpresaId = Convert.ToInt16(ddlTipoEmpresa.SelectedValue);
                objEmpresa.empr_vTelefono = txtTelefono.Text;
                objEmpresa.empr_vActividadComercial = txtActividadComercial.Text;
                objEmpresa.empr_vCorreo = txtCorreo.Text;

                objEmpresa.empr_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objEmpresa.empr_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                objEmpresa.empr_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objEmpresa.empr_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                objEmpresa.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                #endregion

                EmpresaMantenimientoBL lEmpresaMantenimientoBL = new EmpresaMantenimientoBL();
                if (enmAccion == Enumerador.enmAccion.INSERTAR) 
                {
                    #region MDIAZ - rune rápido BE.MRE
                    List<BE.MRE.RE_REPRESENTANTELEGAL> lstRepresentantes = new List<BE.MRE.RE_REPRESENTANTELEGAL>();
                    BE.MRE.RE_REPRESENTANTELEGAL objRepresentanteLegal;
                    foreach (DataRow dr in dtRepresentante.Rows)
                    {
                        objRepresentanteLegal = new BE.MRE.RE_REPRESENTANTELEGAL();

                        objRepresentanteLegal.rele_iEmpresaId = objEmpresa.empr_iEmpresaId;

                        objRepresentanteLegal.PERSONA.pers_iPersonaId = Convert.ToInt64(dr["sPersonaId"]);
                        objRepresentanteLegal.PERSONA.Identificacion.peid_sDocumentoTipoId = Convert.ToInt16(dr["sDocumentoTipoId"]);
                        objRepresentanteLegal.PERSONA.Identificacion.peid_vDocumentoNumero = Convert.ToString(dr["vDocumentoNumero"]);
                        objRepresentanteLegal.PERSONA.pers_sNacionalidadId = Convert.ToInt16(dr["sNacionalidadId"]);
                        objRepresentanteLegal.PERSONA.pers_vApellidoPaterno = Convert.ToString(dr["vApellidoPaterno"]);
                        objRepresentanteLegal.PERSONA.pers_vApellidoMaterno = Convert.ToString(dr["vApellidoMaterno"]);
                        objRepresentanteLegal.PERSONA.pers_vNombres = Convert.ToString(dr["vNombres"]);
                        objRepresentanteLegal.PERSONA.pers_sPersonaTipoId = Convert.ToInt16(Enumerador.enmTipoPersona.NATURAL);
                        objRepresentanteLegal.PERSONA.pers_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        objRepresentanteLegal.PERSONA.pers_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        objRepresentanteLegal.PERSONA.pers_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                        objRepresentanteLegal.PERSONA.pers_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                        objRepresentanteLegal.PERSONA.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                        if (objRepresentanteLegal.PERSONA.pers_iPersonaId != 0)
                            objRepresentanteLegal.PERSONA = objPersonaBL.ObtenerRuneRapido(objRepresentanteLegal.PERSONA);

                        objRepresentanteLegal.rele_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        objRepresentanteLegal.rele_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        objRepresentanteLegal.rele_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                        objRepresentanteLegal.rele_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                        objRepresentanteLegal.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                        lstRepresentantes.Add(objRepresentanteLegal);
                    }

                    intResultado = lEmpresaMantenimientoBL.Insertar(objEmpresa, lstRepresentantes, dtdireccion);
                    #endregion
                }
                else 
                {
                    #region MDIAZ - rune rápido
                    // Representantes 
                    List<BE.MRE.RE_REPRESENTANTELEGAL> lstRepresentantes = new List<BE.MRE.RE_REPRESENTANTELEGAL>();
                    BE.MRE.RE_REPRESENTANTELEGAL objRepresentanteLegal = new BE.MRE.RE_REPRESENTANTELEGAL();
                    foreach (DataRow dr in dtRepresentante.Rows)
                    {
                        objRepresentanteLegal = new BE.MRE.RE_REPRESENTANTELEGAL();

                        objRepresentanteLegal.rele_iEmpresaId = objEmpresa.empr_iEmpresaId;
                        if (dr["iRepresentante"].ToString() == string.Empty)
                            objRepresentanteLegal.rele_iRepresentanteLegalId = 0;
                        else
                            objRepresentanteLegal.rele_iRepresentanteLegalId = Convert.ToInt64(dr["iRepresentante"]);                        

                        objRepresentanteLegal.PERSONA.pers_iPersonaId = Convert.ToInt64(dr["sPersonaId"]);
                        objRepresentanteLegal.PERSONA.Identificacion.peid_sDocumentoTipoId = Convert.ToInt16(dr["sDocumentoTipoId"]);
                        objRepresentanteLegal.PERSONA.Identificacion.peid_vDocumentoNumero = Convert.ToString(dr["vDocumentoNumero"]);
                        if (!dr["sNacionalidadId"].ToString().Trim().Equals(""))
                            objRepresentanteLegal.PERSONA.pers_sNacionalidadId = Convert.ToInt16(dr["sNacionalidadId"]);
                        objRepresentanteLegal.PERSONA.pers_vApellidoPaterno = Convert.ToString(dr["vApellidoPaterno"]);
                        objRepresentanteLegal.PERSONA.pers_vApellidoMaterno = Convert.ToString(dr["vApellidoMaterno"]);
                        objRepresentanteLegal.PERSONA.pers_vNombres = Convert.ToString(dr["vNombres"]);
                        objRepresentanteLegal.PERSONA.pers_sPersonaTipoId = Convert.ToInt16(Enumerador.enmTipoPersona.NATURAL);
                        objRepresentanteLegal.PERSONA.pers_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        objRepresentanteLegal.PERSONA.pers_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        objRepresentanteLegal.PERSONA.pers_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                        objRepresentanteLegal.PERSONA.pers_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                        objRepresentanteLegal.PERSONA.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                        if (objRepresentanteLegal.PERSONA.pers_iPersonaId != 0)
                            objRepresentanteLegal.PERSONA = objPersonaBL.ObtenerRuneRapido(objRepresentanteLegal.PERSONA);

                        objRepresentanteLegal.rele_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        objRepresentanteLegal.rele_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        objRepresentanteLegal.rele_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                        objRepresentanteLegal.rele_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                        objRepresentanteLegal.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);

                        lstRepresentantes.Add(objRepresentanteLegal);
                    }

                    #region Direcciones - Lista - pendiente
                    /*
                    List<BE.MRE.RE_EMPRESARESIDENCIA> lstResidencias = new List<BE.MRE.RE_EMPRESARESIDENCIA>();
                    BE.MRE.RE_EMPRESARESIDENCIA objEmpresaResidencia;
                    foreach (DataRow dr in dtdireccion.Rows)
                    {
                        objEmpresaResidencia = new BE.MRE.RE_EMPRESARESIDENCIA();
                        objEmpresaResidencia.emre_iEmpresaId = Convert.ToInt64(dr["iEmpresaId"]);
                        objEmpresaResidencia.emre_iResidenciaId = Convert.ToInt64(dr["iResidenciaId"]);
                        objEmpresaResidencia.emre_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        objEmpresaResidencia.emre_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        objEmpresaResidencia.emre_vIPCreacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                        objEmpresaResidencia.emre_vIPModificacion = Session[Constantes.CONST_SESION_DIRECCION_IP].ToString();
                        objEmpresaResidencia.OficinaConsultar = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                        objEmpresaResidencia.RESIDENCIA.resi_iResidenciaId = Convert.ToInt64(dr["iResidenciaId"]);
                        objEmpresaResidencia.RESIDENCIA.resi_sResidenciaTipoId = Convert.ToInt16(dr["sResidenciaTipoId"]);
                        objEmpresaResidencia.RESIDENCIA.resi_vResidenciaDireccion = Convert.ToString(dr["vResidenciaDireccion"]);
                        objEmpresaResidencia.RESIDENCIA.resi_vResidenciaTelefono = Convert.ToString(dr["vResidenciaTelefono"]);
                        objEmpresaResidencia.RESIDENCIA.resi_cResidenciaUbigeo = Convert.ToString(dr["cUbigeo01"]) +
                            Convert.ToString(dr["cUbigeo02"]) + Convert.ToString(dr["cUbigeo03"]);
                        objEmpresaResidencia.RESIDENCIA.resi_vCodigoPostal = Convert.ToString(dr["vCodigoPostal"]);                        
                        objEmpresaResidencia.RESIDENCIA.resi_sUsuarioCreacion = objEmpresaResidencia.emre_sUsuarioCreacion;
                        objEmpresaResidencia.RESIDENCIA.resi_sUsuarioModificacion = Convert.ToInt16(objEmpresaResidencia.emre_sUsuarioModificacion);
                        objEmpresaResidencia.RESIDENCIA.resi_vIPCreacion = objEmpresaResidencia.emre_vIPCreacion;
                        objEmpresaResidencia.RESIDENCIA.resi_vIPModificacion = objEmpresaResidencia.emre_vIPModificacion;

                        lstResidencias.Add(objEmpresaResidencia);
                    }
                    */
                    #endregion

                    intResultado = lEmpresaMantenimientoBL.Actualizar(objEmpresa, lstRepresentantes, dtdireccion);
                    #endregion
                }
                
                LimpiaCampos();

                if (intResultado > 0)
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, Constantes.CONST_MENSAJE_EXITO));
                else
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, Constantes.CONST_MENSAJE_OPERACION_FALLIDA));

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

        void ctrlToolBar_btnNuevoHandler()
        {
            Session[strVarAccion] = Enumerador.enmAccion.INSERTAR;
            ctrlToolBar.btnGrabar.Enabled = true;
            ctrlToolBar.btnEliminar.Enabled = false;
            LimpiaCampos();
        }

        protected void btnAgregarRepresentante_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["strVarRepresentanteDt"];
            int index = Convert.ToInt32(ViewState["indexRepresentante"]);

            //Verificando si el representante ya existe en la grilla

            try
            {
                var dExiste = (from dr in dt.AsEnumerable()
                               where dr.Field<string>("vDocumentoTipo" + "vDocumentoNumero").ToString().Trim().Equals(ddlRepresentanteTipoDoc.SelectedItem.Text.Trim() + txtRepresentanteNroDoc.Text.Trim())
                               select dr).CopyToDataTable();

                if (dExiste != null)
                {
                    if (dExiste.Rows.Count > 0)
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, strNombreEntidad, Constantes.CONST_MENSAJE_DNI_DUPLICADO));
                        return;
                    }
                }

                foreach (DataRow row in dt.Rows)
                {
                    if (row["sPersonaId"].ToString() == Convert.ToInt32(ViewState["iRepresentanteId"]).ToString())
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('El representante ya está ingresado con otro tipo de documento');", true);
                        return;
                    }
                }
            }
            catch
            {
            }
            
            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)ViewState[strVarAccionRep];
            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:
                    DataRow row;
                    row = dt.NewRow();

                    row["iRepresentante"] = 0;
                    row["sDocumentoTipoId"] = Convert.ToInt32(ddlRepresentanteTipoDoc.SelectedValue);
                    row["vDocumentoTipo"] = ddlRepresentanteTipoDoc.SelectedItem.Text;
                    row["sNacionalidadId"] = Convert.ToInt32(ddlNacionalidad.SelectedValue);
                    row["vDocumentoNumero"] = txtRepresentanteNroDoc.Text.Trim();
                    row["sPersonaId"] = Convert.ToInt64(ViewState["iRepresentanteId"]);

                    row["vApellidoPaterno"] = txtPrimerApellido.Text.Trim();
                    row["vApellidoMaterno"] = txtSegundoApellido.Text.Trim();
                    row["vNombres"] = txtNombres.Text.Trim();

                    dt.Rows.Add(row);

                    break;
                case Enumerador.enmAccion.MODIFICAR:
                    dt.Rows[index]["iRepresentante"] = Convert.ToInt64(hid_iRepresentanteId.Value);
                    dt.Rows[index]["sDocumentoTipoId"] = Convert.ToInt32(ddlRepresentanteTipoDoc.SelectedValue);
                    dt.Rows[index]["vDocumentoTipo"] = ddlRepresentanteTipoDoc.SelectedItem.Text;
                    dt.Rows[index]["sNacionalidadId"] = Convert.ToInt32(ddlNacionalidad.SelectedValue);
                    dt.Rows[index]["vDocumentoNumero"] = txtRepresentanteNroDoc.Text.Trim().ToUpper();
                    dt.Rows[index]["vApellidoPaterno"] = txtPrimerApellido.Text.Trim();
                    dt.Rows[index]["vApellidoMaterno"] = txtSegundoApellido.Text.Trim();
                    dt.Rows[index]["vNombres"] = txtNombres.Text.Trim();

                    break;
            }

            dt.AcceptChanges();

            gdvRepresentante.DataSource = dt;
            gdvRepresentante.DataBind();

            Session["strVarRepresentanteDt"] = dt;

            gdvRepresentante.Visible = true;

            UpnlRepresentante.Update();
            LimpiarCamposRepresentante();
        }

        private DataTable dtCargaRepresentante()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iRepresentante", typeof(string));
            dt.Columns.Add("sDocumentoTipoId", typeof(string));
            dt.Columns.Add("vDocumentoTipo", typeof(string));
            dt.Columns.Add("sNacionalidadId", typeof(string));
            dt.Columns.Add("vDocumentoNumero", typeof(string));
            dt.Columns.Add("sPersonaId", typeof(string));
            dt.Columns.Add("vApellidoPaterno", typeof(string));
            dt.Columns.Add("vApellidoMaterno", typeof(string));
            dt.Columns.Add("vNombres", typeof(string));

            return dt;
        }

        protected void btnAgregarDireccion_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["strVarDireccionDt"];
            int index = Convert.ToInt32(ViewState["indexDireccion"]);

            int ubigeo01 = Convert.ToInt32(ddlContinente.SelectedValue);
            int ubigeo02 = Convert.ToInt32(ddlPais.SelectedValue);
            int ubigeo03 = Convert.ToInt32(ddlCiudad.SelectedValue);

            Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)ViewState[strVarAccionDir];
            switch (enmAccion)
            {
                case Enumerador.enmAccion.INSERTAR:
                    DataRow row;
                    row = dt.NewRow();

                    row["iEmpresaId"] = Convert.ToInt64(ViewState["iEmpresaId"]);
                    row["iResidenciaId"] = 0;
                    row["vResidenciaDireccion"] = txtEmpresaDireccion.Text.ToUpper().Trim();
                    row["sResidenciaTipoId"] = Convert.ToInt16(ddlTipoResidencia.SelectedValue);
                    row["vDescripcionResidencia"] = ddlTipoResidencia.SelectedItem.Text;

                    row["cUbigeo01"] = ubigeo01.ToString("00");
                    row["vDepartamento"] = ddlContinente.SelectedItem.Text;

                    row["cUbigeo02"] = ubigeo02.ToString("00");
                    row["vProvincia"] = ddlPais.SelectedItem.Text;

                    row["cUbigeo03"] = ubigeo03.ToString("00");
                    row["vDistrito"] = ddlCiudad.SelectedItem.Text;
                    row["vCodigoPostal"] = txtCodigoPostal.Text.Trim().ToUpper();
                    row["vResidenciaTelefono"] = txtResidenciaTelefono.Text.Trim();
                    row["cResidenciaUbigeo"] = ubigeo01.ToString("00") + ubigeo02.ToString("00") + ubigeo03.ToString("00");

                    dt.Rows.Add(row);

                    break;
                case Enumerador.enmAccion.MODIFICAR:
                    dt.Rows[index]["vResidenciaDireccion"] = txtEmpresaDireccion.Text.Trim().ToUpper();
                    dt.Rows[index]["sResidenciaTipoId"] = Convert.ToInt16(ddlTipoResidencia.SelectedValue);
                    dt.Rows[index]["vDescripcionResidencia"] = ddlTipoResidencia.SelectedItem.Text;

                    dt.Rows[index]["cUbigeo01"] = ubigeo01.ToString("00");
                    dt.Rows[index]["vDepartamento"] = ddlContinente.SelectedItem.Text;

                    dt.Rows[index]["cUbigeo02"] = ubigeo02.ToString("00");
                    dt.Rows[index]["vProvincia"] = ddlPais.SelectedItem.Text;

                    dt.Rows[index]["cUbigeo03"] = ubigeo03.ToString("00");
                    dt.Rows[index]["vDistrito"] = ddlCiudad.SelectedItem.Text;
                    dt.Rows[index]["vCodigoPostal"] = txtCodigoPostal.Text.Trim().ToUpper();
                    dt.Rows[index]["vResidenciaTelefono"] = txtResidenciaTelefono.Text.Trim();
                    dt.Rows[index]["cResidenciaUbigeo"] = ubigeo01.ToString("00") + ubigeo02.ToString("00") + ubigeo03.ToString("00");

                    break;
            }

            dt.AcceptChanges();

            gdvDirecciones.DataSource = dt;
            gdvDirecciones.DataBind();

            Session["strVarDireccionDt"] = dt;

            gdvDirecciones.Visible = true;

            LimpiaCamposResidencia();

            UpnlDirecciones.Update();
        }

        private DataTable dtCargaResidencia()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("iEmpresaId", typeof(string));
            dt.Columns.Add("iResidenciaId", typeof(string));
            dt.Columns.Add("vResidenciaDireccion", typeof(string));
            dt.Columns.Add("sResidenciaTipoId", typeof(string));
            dt.Columns.Add("vDescripcionResidencia", typeof(string));
            dt.Columns.Add("cResidenciaUbigeo", typeof(string));
            dt.Columns.Add("cUbigeo01", typeof(string));
            dt.Columns.Add("cUbigeo02", typeof(string));
            dt.Columns.Add("cUbigeo03", typeof(string));
            dt.Columns.Add("vDepartamento", typeof(string));
            dt.Columns.Add("vProvincia", typeof(string));
            dt.Columns.Add("vDistrito", typeof(string));
            dt.Columns.Add("vCodigoPostal", typeof(string));
            dt.Columns.Add("vResidenciaTelefono", typeof(string));

            return dt;
        }

        #endregion

        #region Métodos

        private DataRow ObtenerGrillaRepresentanteSeleccionado()
        {
            int intSeleccionado = (int)Session[strVarRepresentanteIndiceSel];
            return ((DataTable)Session[strVarRepresentanteDt]).Rows[intSeleccionado];
        }

        private DataRow ObtenerGrillaDireccionSeleccionado()
        {
            int intSeleccionado = (int)Session[strVarDireccionIndiceSel];
            return ((DataTable)Session[strVarDireccionDt]).Rows[intSeleccionado];
        }

        private void CargarListadosDesplegables()
        {
            Util.CargarParametroDropDownList(ddlTipoEmpresa, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.EMPRESA_TIPO), true);
            Util.CargarParametroDropDownList(ddlTipoDocumento, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.EMPRESA_TIPO_DOCUMENTO), true);
            Util.CargarDropDownList(ddlRepresentanteTipoDoc, comun_Part1.ObtenerDocumentoIdentidad(), "Valor", "Id", true);
            Util.CargarParametroDropDownList(ddlNacionalidad, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_NACIONALIDAD), true);
            Util.CargarParametroDropDownList(ddlTipoResidencia, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_TIPO_RESIDENCIA), true);

            comun_Part3.CargarUbigeo(Session, ddlContinente, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, string.Empty, string.Empty, true);
            comun_Part3.CargarUbigeo(Session, ddlPais, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, string.Empty, string.Empty, true);
            comun_Part3.CargarUbigeo(Session, ddlCiudad, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, string.Empty, string.Empty, true);
        }

        private void LimpiaCampos()
        {
            try
            {
                txtRazonSocial.Text = "";
                txtNroDocumento.Text = "";
                txtTelefono.Text = "";
                txtCorreo.Text = "";
                ddlTipoDocumento.SelectedIndex = -1;
                txtActividadComercial.Text = "";
                ddlTipoEmpresa.SelectedIndex = -1;
                ddlTipoDocumento.SelectedIndex = -1;

                LimpiarCamposRepresentante();
                Session["strVarRepresentanteDt"] = dtCargaRepresentante();
                gdvRepresentante.DataSource = dtCargaRepresentante();
                gdvRepresentante.DataBind();

                LimpiaCamposResidencia();
                Session["strVarDireccionDt"] = dtCargaResidencia();
                gdvDirecciones.DataSource = dtCargaResidencia();
                gdvDirecciones.DataBind();
                lblValidacion.Attributes.Add("class", "hideControl");

                ViewState.Remove("iEmpresaId");
                Session[strVarAccion] = Enumerador.enmAccion.INSERTAR;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LimpiarCamposRepresentante()
        {
            ddlRepresentanteTipoDoc.SelectedIndex = -1;
            txtRepresentanteNroDoc.Text = "";
            ddlNacionalidad.SelectedIndex = -1;
            txtNombres.Text = "";
            txtPrimerApellido.Text = "";
            txtSegundoApellido.Text = "";

            txtRepresentanteNroDoc.Enabled = true;
            ddlRepresentanteTipoDoc.Enabled = true;
            ddlNacionalidad.Enabled = true;
            txtNombres.Enabled = true;
            txtPrimerApellido.Enabled = true;
            txtSegundoApellido.Enabled = true;

            ViewState.Add("iRepresentanteId", 0);
            ViewState.Add(strVarAccionRep, Enumerador.enmAccion.INSERTAR);
        }

        private void LimpiaCamposResidencia()
        {
            try
            {
                txtEmpresaDireccion.Text = "";
                ddlTipoResidencia.SelectedIndex = -1;
                ddlContinente.SelectedIndex = -1;
                ddlPais.SelectedIndex = -1;
                ddlCiudad.SelectedIndex = -1;
                txtCodigoPostal.Text = "";
                txtResidenciaTelefono.Text = "";

                Util.CargarParametroDropDownList(ddlPais, new DataTable(), true);
                Util.CargarParametroDropDownList(ddlCiudad, new DataTable(), true);

                ViewState.Add("iDireccionId", 0);
                ViewState.Add(strVarAccionDir, Enumerador.enmAccion.INSERTAR);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CargarDatosEmpresa()
        {
            try
            {
                EmpresaConsultaBL objBL = new EmpresaConsultaBL();
                DataSet ds = objBL.ConsultarId(Convert.ToInt64(ViewState["iEmpresaId"]));

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        DataTable dtEmpresa = ds.Tables[0];
                        if (dtEmpresa.Rows.Count > 0)
                        {                            
                            txtNroDocumento.Text = dtEmpresa.Rows[0]["vNumeroDocumento"].ToString();
                            ddlTipoDocumento.SelectedValue = dtEmpresa.Rows[0]["sTipoDocumentoId"].ToString();
                            txtRazonSocial.Text = dtEmpresa.Rows[0]["vRazonSocial"].ToString();
                            ddlTipoEmpresa.SelectedValue = dtEmpresa.Rows[0]["sTipoEmpresaId"].ToString();
                            txtTelefono.Text = dtEmpresa.Rows[0]["vTelefono"].ToString();
                            txtCorreo.Text = dtEmpresa.Rows[0]["vCorreo"].ToString();
                            txtActividadComercial.Text = dtEmpresa.Rows[0]["vActividadComercial"].ToString();
                        }

                        DataTable dtRepresentanteLegal = ds.Tables[1];
                        Session["strVarRepresentanteDt"] = dtRepresentanteLegal;
                        gdvRepresentante.DataSource = dtRepresentanteLegal;
                        gdvRepresentante.DataBind();

                        DataTable dtDireccionEmpresa = ds.Tables[2];
                        Session["strVarDireccionDt"] = dtDireccionEmpresa;
                        gdvDirecciones.DataSource = dtDireccionEmpresa;
                        gdvDirecciones.DataBind();
                        UpnlRepresentante.Update();
                        UpnlDirecciones.Update();

                        Session[strVarAccion] = Enumerador.enmAccion.MODIFICAR;

                        Button BtnEliminarE = (Button)ctrlToolBar.FindControl("btnEliminar");
                        BtnEliminarE.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        protected void gdvRepresentante_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                ViewState.Add("indexRepresentante", index);

                if (e.CommandName == "Eliminar")
                {
                    DataTable dt = ((DataTable)Session["strVarRepresentanteDt"]).Copy();

                    dt.Rows[index].Delete();
                    dt.AcceptChanges();

                    if (gdvRepresentante.Rows.Count == 1)
                    {
                        dt = dtCargaRepresentante();
                    }

                    Session["strVarRepresentanteDt"] = dt;
                    gdvRepresentante.DataSource = dt;
                    gdvRepresentante.DataBind();

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

        protected void gdvDirecciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                ViewState.Add("indexDireccion", index);

                if (e.CommandName == "Editar")
                {
                    ViewState.Add(strVarAccion, Enumerador.enmAccion.MODIFICAR);
                    ViewState.Add(strVarAccionDir, Enumerador.enmAccion.MODIFICAR);

                    ViewState.Add("iResidenciaId", gdvDirecciones.DataKeys[index].Values["iResidenciaId"].ToString());

                    DataTable dt = (DataTable)Session["strVarDireccionDt"];

                    txtEmpresaDireccion.Text = dt.Rows[index]["vResidenciaDireccion"].ToString();
                    ddlTipoResidencia.SelectedValue = gdvDirecciones.DataKeys[index].Values["sResidenciaTipoId"].ToString();

                    ddlContinente.SelectedValue = gdvDirecciones.DataKeys[index].Values["cUbigeo01"].ToString();
                    comun_Part3.CargarUbigeo(Session, ddlPais, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddlContinente.SelectedValue.ToString(), "", true);

                    ddlPais.SelectedValue = gdvDirecciones.DataKeys[index].Values["cUbigeo02"].ToString();
                    comun_Part3.CargarUbigeo(Session, ddlCiudad, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddlContinente.SelectedValue.ToString(), ddlPais.SelectedValue.ToString(), true);

                    ddlCiudad.SelectedValue = gdvDirecciones.DataKeys[index].Values["cUbigeo03"].ToString();

                    txtCodigoPostal.Text = dt.Rows[index]["vCodigoPostal"].ToString();
                    txtResidenciaTelefono.Text = dt.Rows[index]["vResidenciaTelefono"].ToString();

                    dt.Dispose();

                }
                else if (e.CommandName == "Eliminar")
                {
                    DataTable dt = ((DataTable)Session["strVarDireccionDt"]).Copy();

                    dt.Rows[index].Delete();
                    dt.AcceptChanges();

                    if (gdvDirecciones.Rows.Count == 1)
                    {
                        dt = dtCargaResidencia();
                    }

                    Session["strVarDireccionDt"] = dt;
                    gdvDirecciones.DataSource = dt;
                    gdvDirecciones.DataBind();

                    //Limpiando los datos de la grilla
                    txtEmpresaDireccion.Text = string.Empty;
                    ddlTipoResidencia.SelectedValue = "0";
                    ddlContinente.SelectedValue = "0";
                    ddlPais.SelectedValue = "0";
                    ddlCiudad.SelectedValue = "0";
                    txtCodigoPostal.Text = string.Empty;
                    txtResidenciaTelefono.Text = string.Empty;
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

        protected void ddlContinente_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlContinente.SelectedIndex > 0)
                {
                    ddlPais.Enabled = true;
                    comun_Part3.CargarUbigeo(Session, ddlPais, Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddlContinente.SelectedValue.ToString(), "", true);

                    ddlCiudad.DataSource = new DataTable();
                    ddlCiudad.DataBind();

                    ddlPais_SelectedIndexChanged(sender, e);
                    ddlContinente.Focus();
                }
                else
                {
                    ddlPais.DataSource = new DataTable();
                    ddlPais.DataBind();

                    ddlCiudad.DataSource = new DataTable();
                    ddlCiudad.DataBind();
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

        protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlPais.SelectedIndex > 0)
                {
                    comun_Part3.CargarUbigeo(Session, ddlCiudad, Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddlContinente.SelectedValue.ToString(), ddlPais.SelectedValue.ToString(), true);
                    ddlPais.Focus();
                }
                else
                {
                    ddlCiudad.SelectedIndex = -1;
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

        protected void imgBuscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (this.ddlRepresentanteTipoDoc.SelectedValue == "0") 
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "Registro Empresa", Constantes.CONST_MENSAJE_TIPODOC_NOSELECCIONADO));
                } 
                else 
                { 
                    buscarRepresentante();
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

        protected void btnLimpiarDir_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiaCamposResidencia();

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

        protected void btnLimpiarRep_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarCamposRepresentante();
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

        private object[] ObtenerFiltro()
        {
            try
            {
                RE_EmpresaFiltro objEn = new RE_EmpresaFiltro();

                objEn.empr_sTipoDocumentoId = Convert.ToInt32(ddlTipoDocumento.SelectedValue); ;
                objEn.empr_vRazonSocial = txtRazonSocial.Text.Trim();
                objEn.empr_vNumeroDocumento = txtNroDocumento.Text.Trim();

                objEn.iPaginaActual = 1;
                objEn.iPaginaCantidad = 10;

                object[] arrParametros = { objEn };

                return arrParametros;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gdvDirecciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes.Add("style", "word-break:break-all;word-wrap:break-word;");
            }
        }

        protected void gdvRepresentante_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Attributes.Add("style", "word-break:break-all;word-wrap:break-word;");
                e.Row.Cells[3].Attributes.Add("style", "word-break:break-all;word-wrap:break-word;");
                e.Row.Cells[4].Attributes.Add("style", "word-break:break-all;word-wrap:break-word;");
            }
        }

        protected void txtRepresentanteNroDoc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                buscarRepresentante();
                txtNombres.Focus();
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

        private void buscarRepresentante()
        {
            try
            {
                #region Buscar persona
                EnPersona objEn = new EnPersona();
                objEn.sDocumentoTipoId = Convert.ToInt16(ddlRepresentanteTipoDoc.SelectedValue);
                objEn.vDocumentoNumero = txtRepresentanteNroDoc.Text;
                object[] arrParametros = { objEn };
                objEn = SGAC.WebApp.Accesorios.Persona.oPersona(arrParametros);
                #endregion

                #region Pintar Datos Persona

                ViewState.Add("iRepresentanteId", 0);

                if (objEn.iPersonaId > 0)
                {
                    ViewState.Add("iRepresentanteId", objEn.iPersonaId);
                    
                    ddlNacionalidad.SelectedValue = objEn.sNacionalidadId.ToString();
                    
                    txtPrimerApellido.Text = HttpUtility.HtmlDecode(objEn.vApellidoPaterno);
                    txtSegundoApellido.Text = HttpUtility.HtmlDecode(objEn.vApellidoMaterno);
                    txtNombres.Text = HttpUtility.HtmlDecode(objEn.vNombres);

                    //desabilitamos los campos para que no se puedan editar
                    ddlNacionalidad.Enabled = false;
                    if (ddlNacionalidad.SelectedItem.Value == "0") ddlNacionalidad.Enabled = true;
                    txtPrimerApellido.Enabled = false;
                    txtSegundoApellido.Enabled = false;
                    txtNombres.Enabled = false;
                    UpnlRepresentante.Update();
                }
                else
                {

                    if (Comun.ToNullInt32(ddlRepresentanteTipoDoc.SelectedItem.Value) == (int)Enumerador.enmTipoDocumento.DNI)
                    {
                        if (txtRepresentanteNroDoc.Text.Trim().Length != 8)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "AGREGAR REPRESENTANTE", " alert('El número de documento inválido.');", true);

                            txtPrimerApellido.Text = string.Empty;
                            txtSegundoApellido.Text = string.Empty;
                            txtNombres.Text = string.Empty;
                            ddlNacionalidad.Enabled = false;
                            txtPrimerApellido.Enabled = false;
                            txtSegundoApellido.Enabled = false;
                            txtNombres.Enabled = false;

                        }

                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "AGREGAR REPRESENTANTE", " alert('El número de documento ingresado no existe.');", true);

                            txtPrimerApellido.Text = string.Empty;
                            txtSegundoApellido.Text = string.Empty;
                            txtNombres.Text = string.Empty;
                            ddlNacionalidad.Enabled = true;
                            txtPrimerApellido.Enabled = true;
                            txtSegundoApellido.Enabled = true;
                            txtNombres.Enabled = true;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "AGREGAR REPRESENTANTE", " alert('El número de documento ingresado no existe.');", true);

                        txtPrimerApellido.Text = string.Empty;
                        txtSegundoApellido.Text = string.Empty;
                        txtNombres.Text = string.Empty;
                        ddlNacionalidad.Enabled = true;
                        txtPrimerApellido.Enabled = true;
                        txtSegundoApellido.Enabled = true;
                        txtNombres.Enabled = true;
                    }
                    
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [System.Web.Services.WebMethod]
        public static string obtener_provincias(string ubigeo)
        {
            SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
            DataTable lDataTable = objBL.Consultar(ubigeo, null, "01");

            DataView lDataView = lDataTable.DefaultView;
            lDataView.Sort = "ubge_vProvincia  ASC";

            List<CBE_DROPDOWNLIST> loProvincias = (lDataView.ToTable(true, "ubge_vProvincia", "ubge_cUbi02").AsEnumerable().Select(
                                                   x => new CBE_DROPDOWNLIST { ValueField = x.ItemArray[1].ToString(), TextField = x.ItemArray[0].ToString() })
                                                   ).ToList();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(loProvincias).ToString();
        }

        [System.Web.Services.WebMethod]
        public static string obtener_distritos(string departamento, string provincia)
        {
            SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
            DataTable lDataTable = objBL.Consultar(departamento, provincia, null);

            DataView lDataView = lDataTable.DefaultView;

            lDataView.Sort = "ubge_vDistrito  ASC";

            List<CBE_DROPDOWNLIST> loDistritos = (lDataView.ToTable(true, "ubge_vDistrito", "ubge_cUbi03").AsEnumerable().Select(
                                   x => new CBE_DROPDOWNLIST { ValueField = x.ItemArray[1].ToString(), TextField = x.ItemArray[0].ToString() })
                                   ).ToList();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(loDistritos).ToString();
        }
        private void GetDataPersona(long LonPersonaId, Int16 intDocumentoId = 0, string strDocumentoNumero = "")
        {
            try
            {
                DataTable dt = new DataTable();
                SGAC.Registro.Persona.BL.PersonaConsultaBL objPersonaBL = new SGAC.Registro.Persona.BL.PersonaConsultaBL();
                EmpresaConsultaBL objEmpresa = new EmpresaConsultaBL();

                if (Request.QueryString["Juridica"] != null) // si es persona juridica
                {
                    DataSet ds = objEmpresa.ConsultarId(LonPersonaId);
                    dt = ds.Tables[0];
                }
                else
                {
                    dt = objPersonaBL.PersonaGetById(LonPersonaId, intDocumentoId, strDocumentoNumero);
                }

                if (Request.QueryString["Juridica"] != null) // si es persona juridica
                {
                    ViewState["Nombre"] = string.Empty;
                    ViewState["flgModoBusquedaAct"] = null;
                    ViewState["ApePat"] = dt.Rows[0]["vRazonSocial"].ToString();
                    ViewState["ApeMat"] = string.Empty;
                    ViewState["ApeCasada"] = string.Empty;
                    ViewState["Nombres"] = string.Empty;

                    ViewState["DescTipDoc"] = dt.Rows[0]["empr_vTipoDocumento"].ToString();
                    ViewState["NroDoc"] = dt.Rows[0]["vNumeroDocumento"].ToString();
                    ViewState["PER_NACIONALIDAD"] = string.Empty;
                    ViewState["iPersonaId"] = LonPersonaId;

                    ViewState["iTipoId"] = "2102";
                    ViewState["iDocumentoTipoId"] = dt.Rows[0]["sTipoDocumentoId"].ToString();
                    ViewState["iPersonaTipoId"] = dt.Rows[0]["sTipoEmpresaId"].ToString();
                    ViewState["FecNac"] = string.Empty;
                    ViewState["iCodPersonaId"] = LonPersonaId;
                    ViewState["DescTipDoc_OTRO"] = string.Empty;
                }
                else
                { // Persona natural
                    ViewState["Nombre"] = dt.Rows[0]["vNombres"].ToString();
                    ViewState["flgModoBusquedaAct"] = null;
                    ViewState["ApePat"] = dt.Rows[0]["vApellidoPaterno"].ToString();
                    ViewState["ApeMat"] = dt.Rows[0]["vApellidoMaterno"].ToString();
                    ViewState["ApeCasada"] = dt.Rows[0]["vApellidoCasada"].ToString();
                    ViewState["Nombres"] = ViewState["ApePat"] + " " + ViewState["ApeMat"] + ViewState["ApeCasada"] + " , " + ViewState["Nombre"];

                    ViewState["DescTipDoc"] = dt.Rows[0]["vDescTipDoc"].ToString();
                    ViewState["NroDoc"] = dt.Rows[0]["vNroDocumento"].ToString();
                    ViewState["PER_NACIONALIDAD"] = dt.Rows[0]["sNacionalidadId"].ToString();
                    ViewState["iPersonaId"] = LonPersonaId;

                    ViewState["iTipoId"] = dt.Rows[0]["sPersonaTipoId"].ToString();
                    ViewState["iDocumentoTipoId"] = dt.Rows[0]["sDocumentoTipoId"].ToString();
                    ViewState["iPersonaTipoId"] = dt.Rows[0]["sPersonaTipoId"].ToString();
                    ViewState["FecNac"] = dt.Rows[0]["dNacimientoFecha"].ToString();
                    ViewState["PER_GENERO"] = dt.Rows[0]["sGeneroId"].ToString();
                    ViewState["iCodPersonaId"] = LonPersonaId;
                    ViewState["DescTipDoc_OTRO"] = dt.Rows[0]["vTipoDocumento"].ToString();

                    ViewState["DtPersonaAct"] = null;
                }

                dt = null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
