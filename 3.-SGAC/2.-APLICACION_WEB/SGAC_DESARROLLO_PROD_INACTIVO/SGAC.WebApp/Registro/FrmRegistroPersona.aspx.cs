using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Text;
using SGAC.WebApp.Accesorios;
using SGAC.Registro.Persona.BL;
using SGAC.Configuracion.Sistema.BL;
using SGAC.BE;
using SGAC.Controlador;
using SGAC.Accesorios;
using System.IO;
using System.Web;
using System.Drawing;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using SGAC.Registro.Actuacion.BL;
using System.Diagnostics;
using Microsoft.Security.Application;


namespace SGAC.WebApp.Registro
{
    using System.Linq;
    using System.Reflection;
    using SGAC.BE.MRE.Custom;
    using System.Text.RegularExpressions;
    using SGAC.Registro.Actuacion.BL;
    using SAE.UInterfaces;

    public partial class FrmRegistroPersona : MyBasePage
    {
        private string strNombreEntidad = "RUNE";
        private int IntEdadMinima = Convert.ToInt16(WebConfigurationManager.AppSettings["edadmaximo"]);
        private static UIEncriptador UIEncripto = new UIEncriptador();
        //Se crea la estructura del registro temporal de Direcciones del RUNE
        private DataTable CrearDtRegDirecciones()
        {
            DataTable DtDirecciones = new DataTable();

            DtDirecciones.Columns.Clear();

            DataColumn workCol0 = DtDirecciones.Columns.Add("iResidenciaId", typeof(Int64));
            workCol0.AllowDBNull = true;
            workCol0.Unique = false;

            DataColumn workCo1 = DtDirecciones.Columns.Add("iPersonaId", typeof(Int64));
            workCo1.AllowDBNull = true;
            workCo1.Unique = false;

            DataColumn workCo2 = DtDirecciones.Columns.Add("vCodigoPostal", typeof(String));
            workCo2.AllowDBNull = true;
            workCo2.Unique = false;

            DataColumn workCo3 = DtDirecciones.Columns.Add("vResidenciaDireccion", typeof(String));
            workCo3.AllowDBNull = true;
            workCo3.Unique = false;

            DataColumn workCo4 = DtDirecciones.Columns.Add("sResidenciaTipoId", typeof(Int32));
            workCo4.AllowDBNull = true;
            workCo4.Unique = false;

            DataColumn workCo5 = DtDirecciones.Columns.Add("vResidenciaTipo", typeof(String));
            workCo5.AllowDBNull = true;
            workCo5.Unique = false;

            DataColumn workCo6 = DtDirecciones.Columns.Add("cResidenciaUbigeo", typeof(String));
            workCo6.AllowDBNull = true;
            workCo6.Unique = false;

            DataColumn workCo7 = DtDirecciones.Columns.Add("DptoCont", typeof(String));
            workCo7.AllowDBNull = true;
            workCo7.Unique = false;

            DataColumn workCo8 = DtDirecciones.Columns.Add("ProvPais", typeof(String));
            workCo8.AllowDBNull = true;
            workCo8.Unique = false;

            DataColumn workCo9 = DtDirecciones.Columns.Add("DistCiu", typeof(String));
            workCo9.AllowDBNull = true;
            workCo9.Unique = false;

            DataColumn workCo10 = DtDirecciones.Columns.Add("vResidenciaTelefono", typeof(String));
            workCo10.AllowDBNull = true;
            workCo10.Unique = false;

            return DtDirecciones;
        }

        private DataTable CrearDtRegFilaciones()
        {
            DataTable DtRegFilaciones = new DataTable();

            DtRegFilaciones.Columns.Clear();

            DataColumn workCol0 = DtRegFilaciones.Columns.Add("pefi_iPersonaFilacionId", typeof(Int64));
            workCol0.AllowDBNull = true;
            workCol0.Unique = false;

            DataColumn workCol1 = DtRegFilaciones.Columns.Add("pefi_vNombreFiliacion", typeof(string));
            workCol1.AllowDBNull = true;
            workCol1.Unique = false;

            DataColumn workCol2 = DtRegFilaciones.Columns.Add("pefi_vLugarNacimiento", typeof(string));
            workCol2.AllowDBNull = true;
            workCol2.Unique = false;

            DataColumn workCol3 = DtRegFilaciones.Columns.Add("pefi_vFechaNacimiento", typeof(string));
            workCol3.AllowDBNull = true;
            workCol3.Unique = false;

            DataColumn workCol4 = DtRegFilaciones.Columns.Add("pefi_sNacionalidadId", typeof(int));
            workCol4.AllowDBNull = true;
            workCol4.Unique = false;

            DataColumn workCol5 = DtRegFilaciones.Columns.Add("pefi_vNacionalidad", typeof(string));
            workCol5.AllowDBNull = true;
            workCol5.Unique = false;

            DataColumn workCol6 = DtRegFilaciones.Columns.Add("pefi_sTipoFilacionId", typeof(int));
            workCol6.AllowDBNull = true;
            workCol6.Unique = false;

            DataColumn workCol7 = DtRegFilaciones.Columns.Add("pefi_vTipoFiliacion", typeof(string));
            workCol7.AllowDBNull = true;
            workCol7.Unique = false;

            DataColumn workCol8 = DtRegFilaciones.Columns.Add("pefi_vNroDocumento", typeof(string));
            workCol8.AllowDBNull = true;
            workCol8.Unique = false;

            DataColumn workCol9 = DtRegFilaciones.Columns.Add("pefi_iPersonaId", typeof(Int64));
            workCol9.AllowDBNull = true;
            workCol9.Unique = false;

            DataColumn workCol10 = DtRegFilaciones.Columns.Add("pefi_dFechaNacimiento", typeof(DateTime));
            workCol10.AllowDBNull = true;
            workCol10.Unique = false;

            DataColumn workCol11 = DtRegFilaciones.Columns.Add("pefi_sNacionalidad_desc", typeof(string));
            workCol11.AllowDBNull = true;
            workCol11.Unique = false;

            DataColumn workCol12 = DtRegFilaciones.Columns.Add("pefi_sTipoFilacionId_desc", typeof(string));
            workCol12.AllowDBNull = true;
            workCol12.Unique = false;

            DataColumn workCol13 = DtRegFilaciones.Columns.Add("pefi_sNacionalidad", typeof(int));
            workCol13.AllowDBNull = true;
            workCol13.Unique = false;

            DataColumn workCol14 = DtRegFilaciones.Columns.Add("pefi_sDocumentoTipoId", typeof(int));
            workCol14.AllowDBNull = true;
            workCol14.Unique = false;

            DataColumn workCol15 = DtRegFilaciones.Columns.Add("pefi_iFiliadoId", typeof(Int64));
            workCol15.AllowDBNull = true;
            workCol15.Unique = false;

            DataColumn workCol16 = DtRegFilaciones.Columns.Add("pefi_cEstado", typeof(string));
            workCol16.AllowDBNull = true;
            workCol16.Unique = false;

            DataColumn workCol17 = DtRegFilaciones.Columns.Add("pers_vApellidoPaterno", typeof(string));
            workCol17.AllowDBNull = true;
            workCol17.Unique = false;

            DataColumn workCol18 = DtRegFilaciones.Columns.Add("pers_vApellidoMaterno", typeof(string));
            workCol18.AllowDBNull = true;
            workCol18.Unique = false;

            DataColumn workCol19 = DtRegFilaciones.Columns.Add("pers_vNombres", typeof(string));
            workCol19.AllowDBNull = true;
            workCol19.Unique = false;


            return DtRegFilaciones;
        }

        private DataTable CrearDtRegImagenes()
        {
            DataTable DtRegImagenes = new DataTable();

            DtRegImagenes.Columns.Clear();

            DataColumn workCol0 = DtRegImagenes.Columns.Add("iPersonaFotoId", typeof(long));
            workCol0.AllowDBNull = true;
            workCol0.Unique = false;

            DataColumn workCol1 = DtRegImagenes.Columns.Add("iPersonaId", typeof(long));
            workCol1.AllowDBNull = true;
            workCol1.Unique = false;

            DataColumn workCol2 = DtRegImagenes.Columns.Add("sFotoTipoId", typeof(Int32));
            workCol2.AllowDBNull = true;
            workCol2.Unique = false;

            DataColumn workCol3 = DtRegImagenes.Columns.Add("GFoto", typeof(byte[]));
            workCol3.AllowDBNull = true;
            workCol3.Unique = false;

            return DtRegImagenes;
        }

        #region Eventos

        private void Page_Init(object sender, System.EventArgs e)
        {
            ScriptCliente();
            
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            

            try
            {
                string StrScript = string.Empty;              
                ctrlToolBarButton.btnNuevoHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonNuevoClick(MyControl_btnNuevo);
                ctrlToolBarButton.btnEliminarHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonEliminarClick(MyControl_btnEliminar);
                ctrlToolBarButton.btnGrabarHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonGrabarClick(MyControl_btnGrabar);
                ctrlToolBarButton.btnBuscarHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonBuscarClick(MyControl_btnBuscar);
                ctrlToolBarButton.btnSalirHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonSalirClick(MyControl_btnSalir);
                ctrlToolBarButton.btnPrintHandler += new SGAC.WebApp.Accesorios.SharedControls.ctrlToolBarButton.OnButtonPrintClick(MyControl_btnImprimir);                

                CmbTipoDoc.AutoPostBack = true;

                //CmbEstCiv.AutoPostBack = true;
                //CmbEstCiv.SelectedIndexChanged += new EventHandler(CmbEstCiv_SelectedIndexChanged);

                //CmbGenero.AutoPostBack = true;
                
                Comun.CargarPermisos(Session, ctrlToolBarButton, HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);

                Button BtnGrabar = (Button)ctrlToolBarButton.FindControl("btnGrabar");
                BtnGrabar.OnClientClick = "return IsValidDatosRUNE();";

                Button BtnEliminarE = (Button)ctrlToolBarButton.FindControl("btnEliminar");
                BtnEliminarE.OnClientClick = "return confirm('¿Desea eliminar a la persona?');";

                btn_GrabarDocumento.OnClientClick = "return IsValidDatosPestañaDocumentos();";

                this.ctrFecNac.StartDate = new DateTime(1900, 1, 1);
                
                
                this.ctrFecNac.EndDate = DateTime.Today;

                this.ctrFecExped.StartDate = new DateTime(1900, 1, 1);
                this.ctrFecExped.EndDate = DateTime.Today.AddDays(-1);

                this.ctrFecVcto.AllowFutureDate = true;
                this.ctrFecRenov.AllowFutureDate = true;

                if (ctrFecNac.Text != string.Empty)
                {
                    DateTime datFechaInicio = new DateTime();
                    if (!DateTime.TryParse(ctrFecNac.Text, out datFechaInicio))
                    {
                        datFechaInicio = Comun.FormatearFecha(ctrFecNac.Text);// Convert.ToDateTime(ctrFecNac.Text, System.Globalization.CultureInfo.GetCultureInfo("en-Us").DateTimeFormat);
                    }
                    ctrFecNac.Text = datFechaInicio.ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                }
                if (!IsPostBack)
                {
                    ctrFecNac.Text = "";
                    txtApePat.Enabled = false;
                    txtApeMat.Enabled = false;
                    txtNombres.Enabled = false;
                    txtApepCas.Enabled = false;
                    HF_ModoEdicion.Value = "new";
                    if(Request.QueryString["CodPer"] != null)
                    {
                        string codPersona = Util.DesEncriptar(Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodPer"].ToString()));
                        // ESTE VALOR LLEGA CUANDO EDITAN A UNA PERSONA DESDE TRAMITES
                        // TAMBIEN CUANDO LE DAN BUSCAR DESDE RUNE Y CLIC EN LISTA DE PESONAS
                        
                        if (codPersona != "")
                        {
                            if (codPersona.Length > 0)
                            {
                                //------------------------------------------------------
                                //Fecha: 11/01/2022
                                //Autor: VPipa
                                //Motivo: inicializar el boton Validar Con  Reniec
                                //------------------------------------------------------   
                                HF_ModoEdicion.Value = "edit";
                                btnValidarDni.Visible = false;
                                chkValidarConReniec.Visible = false;
                                txtApePat.Enabled = true;
                                txtApeMat.Enabled = true;
                                txtNombres.Enabled = true;
                                txtApepCas.Enabled = true;
                                //------------------------------------------------------
                                //Fecha: 19/10/2021
                                //Autor: Miguel Márquez Beltrán
                                //Motivo: Obtener el tipo y numero de documento
                                //------------------------------------------------------
                                string codTipoDoc = "";
                                string codNroDocumento = "";
                                if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                                {
                                    codTipoDoc = Util.DesEncriptar(Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString()));
                                    codNroDocumento = Util.DesEncriptar(Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString()));
                                }

                                if (codTipoDoc.Length > 0 && codNroDocumento.Length > 0)
                                {
                                    GetDataPersona(Convert.ToInt64(codPersona), Convert.ToInt16(codTipoDoc), codNroDocumento);
                                }
                                else
                                {
                                    GetDataPersona(Convert.ToInt64(codPersona));
                                }
                                
                                //------------------------------------------------------
                                DataTable dtVerificar = new DataTable();
                                PersonaConsultaBL objBL = new PersonaConsultaBL();
                                dtVerificar = objBL.Tiene58B(Convert.ToInt64(codPersona));

                                if (dtVerificar.Rows.Count > 0)
                                {
                                    //------------------------------
                                    //Fecha: 26/10/2021
                                    //Autor: Miguel Márquez Beltrán
                                    //Motivo: LLenar lista de idiomas.
                                    //------------------------------
                                    string strIdioma = Session[Constantes.CONST_SESION_IDIOMA_TEXTO].ToString();
                                    string strIdiomaId = Session[Constantes.CONST_SESION_IDIOMA_ID].ToString();
                                    ddlIdiomas.Items.Clear();
                                    ddlIdiomas.Items.Insert(0, new ListItem("CASTELLANO", "0"));
                                    if (strIdioma.ToUpper() != "CASTELLANO")
                                    {
                                        ddlIdiomas.Items.Insert(1, new ListItem(strIdioma, strIdiomaId));
                                    }
                                    //------------------------------
                                    btnImprimirConstancia.Visible = true;
                                    ViewState["MLocal"] = dtVerificar.Rows[0]["pago_FMontoMonedaLocal"].ToString();
                                    ViewState["MSolesCon"] = dtVerificar.Rows[0]["pago_FMontoSolesConsulares"].ToString();
                                }
                                else
                                {
                                    btnImprimirConstancia.Visible = false;
                                }
                            }
                        }
                    }
                    
                    //if (Request.QueryString["GUID"] != null)
                    //{
                    //    HFGUID.Value = Sanitizer.GetSafeHtmlFragment(Request.QueryString["GUID"].ToString());
                    //    lkbTramite.PostBackUrl = "~/Registro/FrmTramite.aspx?GUID=" + HFGUID.Value;
                    //}
                    //else
                    //{
                    //    HFGUID.Value = "";
                    //}

                    //-----------------------------------------------------------------------------------
                    // Fecha: 25/07/2018
                    // Autor: Miguel Márquez Beltrán
                    // Objetivo: Limitar el ingreso de caracteres en la caja de texto.
                    //-----------------------------------------------------------------------------------

                    TxtObsRune.Attributes.Add("onkeypress", " ValidarCaracteres(this, 1000);");
                    TxtObsRune.Attributes.Add("onkeyup", " ValidarCaracteres(this, 1000);");
                    txtNroDoc.Attributes.Add("onchange", "editoDNI();");

                    Session["BUSQUEDA_TIPO_PERSONA"] = (int)Enumerador.enmTipoPersona.NATURAL;

                    GetValoresEspeciales();

                    txtObsRENIEC.Text = "NINGUNO";

                    Session["OperRune"] = true;

                    Session["IntOperFiliacion"] = true;
                    Session["iOperDir"] = true;

                    Session["DtRegDirecciones"] = CrearDtRegDirecciones();
                    ((DataTable)Session["DtRegDirecciones"]).Clear();

                    Session["DtRegFilaciones"] = CrearDtRegFilaciones();
                    ((DataTable)Session["DtRegFilaciones"]).Clear();

                    Session["DtRegImagenes"] = CrearDtRegImagenes();
                    ((DataTable)Session["DtRegImagenes"]).Clear();

                    //**************************************
                    //* No mover esta variable que gestiona
                    // * la carga de las imagenes
                     //--
                    //if (HFGUID.Value.Length > 0)
                    //{
                    //    ViewState["iPersonaId"] = 0;
                    //    ViewState["iTipoId"] = ((int)Enumerador.enmTipoPersona.NATURAL).ToString();
                    //}
                    //else
                    //{
                        ViewState["iPersonaId"] = 0;
                        ViewState["iTipoId"] = ((int)Enumerador.enmTipoPersona.NATURAL).ToString();
                    //}



                    //if (HFGUID.Value.Length > 0)
                    //{
                    //    Session["iPersonaId" + HFGUID.Value] = 0;
                    //    Session["iTipoId" + HFGUID.Value] = ((int)Enumerador.enmTipoPersona.NATURAL).ToString();
                    //}
                    ViewState.Add("AccionDocumento", "Nuevo");
                    Session["iOperDoc"] = true;

                    Util.CargarParametroDropDownList(CmbNacRecurr, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_NACIONALIDAD), true);
                    CmbNacRecurr.Items.RemoveAt(0);
                    //--------------------------------------------------------
                    //Ordenar la Tabla de Tipo de Documento por Id
                    DataTable dtTipDoc = new DataTable();
                    dtTipDoc = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.DOCUMENTO_IDENTIDAD);
                    DataView dv = dtTipDoc.DefaultView;
                    DataTable dtOrdenado = dv.ToTable();
                    dtOrdenado.DefaultView.Sort = "Id ASC";

                    //Cargar los combos de tipo de documento con el datatable ordenado
                    Util.CargarDropDownList(CmbTipoDoc, dtOrdenado, "Valor", "Id", true);
                    CmbTipoDoc.Items.RemoveAt(0);
                    ListItem lListItem = new ListItem(Convert.ToString(Enumerador.enmTipoDocumento.CUI), Convert.ToString((int)Enumerador.enmTipoDocumento.CUI));

                    CmbTipoDoc.Items.Add(lListItem);

                    Util.CargarDropDownList(ddl_TipoDocumentoM, dtOrdenado, "Valor", "Id", true);
                    ddl_TipoDocumentoM.Items.RemoveAt(0);

                    ListItem lListItem1 = new ListItem(Convert.ToString(Enumerador.enmTipoDocumento.CUI), Convert.ToString((int)Enumerador.enmTipoDocumento.CUI));
                    ddl_TipoDocumentoM.Items.Add(lListItem);

                    
                    //-------------------------------------------------------------
                    //Fecha: 05/02/2020
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Cargar los registros desde la tabla Parametros.
                    //-------------------------------------------------------------
                    Enumerador.enmGrupo[] arrGrupos = { Enumerador.enmGrupo.PERSONA_GENERO, Enumerador.enmGrupo.PERSONA_GRADO_INSTRUCCION, Enumerador.enmGrupo.PERSONA_TIPO_VINCULO,
                                                    Enumerador.enmGrupo.PERSONA_TIPO_RESIDENCIA, Enumerador.enmGrupo.PERSONA_TIPO, Enumerador.enmGrupo.PERSONA_TIPO_VINCULO};
                    DropDownList[] arrDDL = { CmbGenero, CmbGradInst, CmbRelCto, CmbTipRes, CmbTipPers, ddl_TipFiliacion };

                    DataTable dtGrupoParametros = new DataTable();

                    dtGrupoParametros = comun_Part1.ObtenerParametrosListaGrupos(Session, arrGrupos);

                    Util.CargarParametroDropDownListDesdeListaGrupos(arrDDL, arrGrupos, dtGrupoParametros, true);
                    //-------------------------------------                

                    CmbRelCto.SelectedValue = "0";

                    //-------------------------------------------------------------
                    //Fecha: 05/02/2020
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Cargar las tablas independientemente.
                    //-------------------------------------------------------------
                    Util.CargarParametroDropDownList(CmbEstCiv, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.ESTADO_CIVIL), true);

                    DataTable dtOcupacion = new DataTable();

                    dtOcupacion = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.OCUPACION);

                    Util.CargarParametroDropDownList(CmbOcupacion, dtOcupacion, true);

                    Util.CargarParametroDropDownList(ddl_Profesion, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.PROFESION), true);

                    Util.CargarParametroDropDownList(CbmOcupPeru, dtOcupacion, true);

                    Util.CargarParametroDropDownList(CbmOcupExterior, dtOcupacion, true);
                    dtOcupacion.Dispose();
                    //-----------------------------------------------------------------------

                    FillWebCombo(SGAC.WebApp.Accesorios.SysTables.TraerSysTable("", "MesesAño"), ddl_MesVivDesde, CultureDescription(), "Valor");
                    FillWebCombo(SGAC.WebApp.Accesorios.SysTables.TraerSysTable("", "MesesAño"), ddl_MesRegreso, CultureDescription(), "Valor");

                    CmbNacRecurr.SelectedValue = Convert.ToString((int)Enumerador.enmNacionalidad.PERUANA);
                    CmbTipoDoc.SelectedValue = Convert.ToString((int)Enumerador.enmTipoDocumento.DNI);
                    CmbTipPers.SelectedValue = Convert.ToString((int)Enumerador.enmTipoPersona.NATURAL);

                    Button BtnEliminarEnabled = (Button)ctrlToolBarButton.FindControl("btnEliminar");
                    BtnEliminarEnabled.Enabled = false;

                    Button BtnImprimirEnabled = (Button)ctrlToolBarButton.FindControl("btnImprimir");
                    BtnImprimirEnabled.Enabled = false;
                    

                    // Jonatan Silva, Se llena todo los combos de continente con una sola consulta
                    beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                    UbigeoConsultasBL objUbigeoBL = new UbigeoConsultasBL();

                    obeUbigeoListas = objUbigeoBL.obtenerUbiGeo();

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string jsonStringProvincia = serializer.Serialize(obeUbigeoListas.Ubigeo02);
                    string jsonStringDistrito = serializer.Serialize(obeUbigeoListas.Ubigeo03);

                    string javaScript = "Guardarlocalstorage(" + jsonStringProvincia + "," + jsonStringDistrito + ");";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", javaScript, true);

                    ViewState["Ubigeo"] = obeUbigeoListas;
                    if (obeUbigeoListas != null)
                    {
                        if (obeUbigeoListas.Ubigeo01.Count > 0)
                        {
                            obeUbigeoListas.Ubigeo01.Insert(0, new beUbicaciongeografica { Ubi01 = "00", Departamento = "-- SELECCIONE --" });
                            CmbDptoContNac.DataSource = obeUbigeoListas.Ubigeo01;
                            CmbDptoContNac.DataValueField = "Ubi01";
                            CmbDptoContNac.DataTextField = "Departamento";
                            CmbDptoContNac.DataBind();

                            CmbDptoContDir.DataSource = obeUbigeoListas.Ubigeo01;
                            CmbDptoContDir.DataValueField = "Ubi01";
                            CmbDptoContDir.DataTextField = "Departamento";
                            CmbDptoContDir.DataBind();

                            ddlDptContinenteResidencia.DataSource = obeUbigeoListas.Ubigeo01;
                            ddlDptContinenteResidencia.DataValueField = "Ubi01";
                            ddlDptContinenteResidencia.DataTextField = "Departamento";
                            ddlDptContinenteResidencia.DataBind();
                        }
                    }




                    //List<listaUbigeo> listaUbigeo = new List<listaUbigeo>();
                    //listaUbigeo = Comun.LlenarListaUbigeoDptoCont();

                    //foreach (listaUbigeo item in listaUbigeo)
                    //{
                    //    CmbDptoContNac.Items.Add(new ListItem(item.PartName, item.PartId));
                    //    CmbDptoContDir.Items.Add(new ListItem(item.PartName, item.PartId));
                    //    ddlDptContinenteResidencia.Items.Add(new ListItem(item.PartName, item.PartId));
                    //}


                    //CARGAR PAISES DE NACIONALIDADES
                    DataTable dtPaises = new DataTable();
                    dtPaises = Comun.ConsultarPaises();
                    //--------------------------------------------------
                    //Fecha: 03/04/2020
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Asignar Paises a un ViewState.
                    //--------------------------------------------------
                    ViewState["Paises"] = dtPaises;
                    //dtPaises = (DataTable)Session[Constantes.CONST_SESION_TABLA_PAISES];
                    Util.CargarDropDownList(ddlPais_Nacion, dtPaises, "PAIS_VNOMBRE", "PAIS_SPAISID", true);
                    ddlPais_Nacion.SelectedIndex = 0;
                    //FIN


                   
                    //Comun.CargarUbigeo(Session, CmbDptoContNac, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, CmbNacRecurr.SelectedValue.ToString(), "", true, Enumerador.enmNacionalidad.PERUANA);
                    this.CmbProvPaisNac.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
                    this.CmbDistCiudadNac.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
                    ddlPais_Nacion.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));

                    //FillWebCombo(Comun.ObtenerDepartamentos(Session), CmbDptoContDir, "ubge_vDepartamento", "ubge_cUbi01");
                    //this.CmbProvPaisDir.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
                    //this.CmbDistCiuDir.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));

                    StrScript = @"$(function(){{
                                    DisableTabIndex(1);
                                }});";
                    StrScript = string.Format(StrScript);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DisableTabIndex1", StrScript, true);

                    StrScript = @"$(function(){{
                                    DisableTabIndex(2);
                                }});";
                    StrScript = string.Format(StrScript);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "DisableTabIndex2", StrScript, true);

                    if ((bool)Session["OperRune"])
                    {
                        StrScript = @"$(function(){{
                                    DisableTabIndex(3);
                                }});";
                        StrScript = string.Format(StrScript);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "DisableTabIndex3", StrScript, true);
                    }

                    if ((bool)Session["OperRune"])
                    {
                        StrScript = @"$(function(){{
                                    DisableTabIndex(4);
                                }});";
                        StrScript = string.Format(StrScript);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "DisableTabIndex4", StrScript, true);
                    }
                    if ((bool)Session["OperRune"])
                    {
                        StrScript = @"$(function(){{
                                    DisableTabIndex(5);
                                }});";
                        StrScript = string.Format(StrScript);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "DisableTabIndex5", StrScript, true);
                    }
                    if ((bool)Session["OperRune"])
                    {
                        StrScript = @"$(function(){{
                                    DisableTabIndex(6);
                                }});";
                        StrScript = string.Format(StrScript);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "DisableTabIndex6", StrScript, true);
                    }
                    /*************************************************
                     Metodo que carga datos del connacional cuando
                     en el modulo de actuaciones se ejecuta una
                     actualización de datos
                     ************************************************/
                    //DataTable TipoDocumentoFil = new DataTable();
                    //TipoDocumentoFil = Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.DOCUMENTO_IDENTIDAD);
                    DataView dvt = dtTipDoc.DefaultView;
                    DataTable dtOrdenadot = dv.ToTable();
                    dtOrdenadot.DefaultView.Sort = "Id ASC";

                    Util.CargarDropDownList(ddl_peid_sDocumentoTipoId, dtOrdenadot, "Valor", "Id", true);

                    Util.CargarParametroDropDownList(ddl_pers_sNacionalidadId, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_NACIONALIDAD), true);

                    Session["GrdFiliacion_Persona"] = null;
                    Session["GrdFiliacion"] = null;
                    Session["GrdFiliacion_Otros"] = null;


                    //--------------------------------------------------
                    //Fecha: 20/03/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Inicializar Otro documento
                    //--------------------------------------------------
                    LblOtroDocumento.Visible = false;
                    txtDescOtroDocumento.Visible = false;
                    lblCO_OtroDoc.Visible = false;
                    //--------------------------------------------------

                    CargaCamposRune();

                    #region Hidden Field Documento de Identidad

                    HF_ValoresDocumentoIdentidad.Value = string.Empty;
                    DataTable dt = new DataTable();

                    //DataTable dt = (DataTable)Session[Constantes.CONST_SESION_DT_DOCUMENTOIDENTIDAD];

                    dt = Comun.ObtenerListaDocumentoIdentidad();

                    foreach (DataRow dr in dt.Rows)
                    {
                        HF_ValoresDocumentoIdentidad.Value += dr["doid_sTipoDocumentoIdentidadId"].ToString() + "," +
                            dr["doid_sDigitosMinimo"].ToString() + "," + dr["doid_sDigitos"].ToString() + "," +
                         dr["doid_bNumero"].ToString() + "," + dr["doid_sTipoNacionalidad"].ToString() + "," +
                         dr["vMensajeError"].ToString() + "|";
                    }

                    #endregion

                    #region Hidden Field Documento de Identidad

                    HF_ValoresFiliacion.Value = string.Empty;


                    dt = new DataTable();
                    dt = comun_Part1.ObtenerParametrosPorGrupo(Session, "PERSONA-TIPO VÍNCULO");

                    //dt = (DataTable)Session[Constantes.CONST_SESION_DT_PARAMETRO];

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["grupo"].ToString() == "PERSONA-TIPO VÍNCULO")
                        {
                            HF_ValoresFiliacion.Value += dr["id"].ToString() + "," +
                                dr["valor"].ToString() + "|";
                        }
                    }

                    #endregion

                    HF_TipoFiliacionColumnaIndice.Value = Util.ObtenerIndiceColumnaGrilla(GrdFiliacion, "pefi_sTipoFilacionId").ToString();

                    // Pasaporte E Identificador
                    DataTable dtDocumentoIdentidad = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.DOCUMENTO_IDENTIDAD);
                    DataView dvPasaporte = dtDocumentoIdentidad.DefaultView;
                    dvPasaporte.RowFilter = " descripcion like 'PASAPORTE E%'";

                    DataTable dtPasaporte = dvPasaporte.ToTable();
                    if (dtPasaporte.Rows.Count > 0)
                        HF_Pasaporte_ID.Value = dtPasaporte.Rows[0]["id"].ToString();

                    HF_TipoDocumento_Editando.Value = "-1";
                    HF_NumeroDocumento_Editando.Value = "";
                    HF_ValidaDocumento.Value = ((int)Enumerador.enmTipoDocumento.CUI).ToString();
                    // ----
                    bool bolEsNuevo = (bool)Session["OperRune"];

                    if (bolEsNuevo == true)
                    {
                        string strUbigeo = Session[Constantes.CONST_SESION_UBIGEO].ToString();
                        string strDpto = strUbigeo.Substring(0, 2);
                        string strProv = strUbigeo.Substring(2, 2);
                        string strDist = strUbigeo.Substring(4, 2);

                        CmbDptoContDir.SelectedValue = strDpto;
                        CmbDptoContDir_SelectedIndexChanged(sender, e);
                        CmbProvPaisDir.SelectedValue = strProv;
                        CmbProvPaisDir_SelectedIndexChanged(sender, e);
                        CmbDistCiuDir.SelectedValue = strDist;
                        Session["CmbDistCiuDir"] = CmbDistCiuDir.SelectedValue;
                    }
                    else {
                        direccionResidencia.Visible = false;
                    }

                    Int64 iPersonaID = Convert.ToInt64(ViewState["iPersonaId"]);
                    if (iPersonaID > 0)
                    {
                        ListarNacionalidades(iPersonaID);
                    }
                    //-----------------------------------------------------------------
                    //Fecha: 20/04/2021
                    //Autor: Miguel Márquez Beltrán
                    //Motivo: Cambiar por defecto el check de Nacionalidad a vigente.
                    //-----------------------------------------------------------------
                    chkNacVigente.Checked = true;
                }
                //--------------------------------------------------------
                //Fecha: 05/02/2020
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Conocer si una Oficina Consular esta activa. 
                //--------------------------------------------------------                
                Int16 intOficinaConsular = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                SGAC.Configuracion.Sistema.BL.OficinaConsularConsultasBL OficinaBL = new SGAC.Configuracion.Sistema.BL.OficinaConsularConsultasBL();
                string strOficinaActiva = "N";
                strOficinaActiva = OficinaBL.OficinaEsActiva(intOficinaConsular);                
                //----------------------------------------------------
                if (Session[Constantes.CONST_SESION_USUARIO_ROL_TIPO_ACCESO].ToString() == "LECTURA" || strOficinaActiva == "N")
                {
                    Button[] arrButtons = {ctrlToolBarButton.btnNuevo, ctrlToolBarButton.btnGrabar, ctrlToolBarButton.btnEliminar,
                                      btn_GrabarDir, btnGrabarFiliacion, btn_GrabarDocumento, btn_GrabarDir, btnGrabarFiliacion};
                    GridView[] arrGridView = { Grd_Documentos, GrdDirecciones, GrdFiliacion };

                    Comun.ModoLectura(ref arrButtons);
                    Comun.ModoLectura(ref arrGridView);
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


        private void LimpiarPersona()
        {
            //--------------------------------------------------------------------
            //Fecha: 05/02/2020
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Cargar cada tablas por grupos desde la tabla parametros.
            //--------------------------------------------------------------------
            
            Enumerador.enmGrupo[] arrGrupos = { Enumerador.enmGrupo.PERSONA_GENERO, Enumerador.enmGrupo.PERSONA_GRADO_INSTRUCCION, Enumerador.enmGrupo.PERSONA_TIPO_VINCULO,
                                                    Enumerador.enmGrupo.PERSONA_TIPO_RESIDENCIA, Enumerador.enmGrupo.PERSONA_TIPO, Enumerador.enmGrupo.PERSONA_TIPO_VINCULO};
            DropDownList[] arrDDL = { CmbGenero, CmbGradInst, CmbRelCto, CmbTipRes, CmbTipPers, ddl_TipFiliacion };

            DataTable dtGrupoParametros = new DataTable();

            dtGrupoParametros = comun_Part1.ObtenerParametrosListaGrupos(Session, arrGrupos);

            Util.CargarParametroDropDownListDesdeListaGrupos(arrDDL, arrGrupos, dtGrupoParametros, true);
            //-------------------------------------                
                                            
            CmbRelCto.SelectedValue = "0";

            //-------------------------------------------------------------
            //Fecha: 05/02/2020
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Cargar cada tabla independientemente.
            //-------------------------------------------------------------
            Util.CargarParametroDropDownList(CmbEstCiv, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.ESTADO_CIVIL), true);

            DataTable dtOcupacion = new DataTable();

            dtOcupacion = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.OCUPACION);

            Util.CargarParametroDropDownList(CmbOcupacion, dtOcupacion, true);

            Util.CargarParametroDropDownList(CbmOcupPeru, dtOcupacion, true);
            Util.CargarParametroDropDownList(CbmOcupExterior, dtOcupacion, true);

            Util.CargarParametroDropDownList(ddl_Profesion, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.PROFESION), true);

            dtOcupacion.Dispose();
           //--------------------------------

            

            FillWebCombo(SGAC.WebApp.Accesorios.SysTables.TraerSysTable("", "MesesAño"), ddl_MesVivDesde, CultureDescription(), "Valor");
            FillWebCombo(SGAC.WebApp.Accesorios.SysTables.TraerSysTable("", "MesesAño"), ddl_MesRegreso, CultureDescription(), "Valor");

            CmbNacRecurr.SelectedValue = Convert.ToString((int)Enumerador.enmNacionalidad.PERUANA);
            CmbTipoDoc.SelectedValue = Convert.ToString((int)Enumerador.enmTipoDocumento.DNI);
            CmbTipPers.SelectedValue = Convert.ToString((int)Enumerador.enmTipoPersona.NATURAL);

            //////////////////////////////////////////

            this.CmbDptoContNac.Items.Clear();
            comun_Part3.CargarUbigeo(Session, CmbDptoContNac, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, CmbNacRecurr.SelectedValue.ToString(), "", true, Enumerador.enmNacionalidad.PERUANA);
            this.CmbProvPaisNac.Items.Clear();
            this.CmbDistCiudadNac.Items.Clear();
            this.CmbProvPaisDir.Items.Clear();
            this.CmbDistCiuDir.Items.Clear();

            this.CmbProvPaisNac.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
            this.CmbDistCiudadNac.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
            this.CmbProvPaisDir.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
            this.CmbDistCiuDir.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));

            TxtObsRune.Text = String.Empty;
            TxtAcuerConv.Text = String.Empty;

            RdioSiBenExter.Checked = false;
            RdioSiAportSegSoc.Checked = false;
            RdioSiAfilAFP.Checked = false;
            RdioSiAfilSegSoc.Checked = false;
            RdioSiRetornExt.Checked = false;

            txtAñoRegreso.Text = String.Empty;
            txtAñoVivDesde.Text = String.Empty;

            TxtMailCont.Text = String.Empty;
            TxtDirPerCont.Text = String.Empty;
            TxtTelfCont.Text = String.Empty;
            TxtCodPostCont.Text = String.Empty;

            TxtDirExtrCont.Text = String.Empty;
            TxtNomCompCont.Text = String.Empty;

            TxtTelfDir.Text = String.Empty;
            txtCodPost.Text = String.Empty;
            TxtDirDir.Text = String.Empty;

            txtLugRenov.Text = String.Empty;
            txtLugExp.Text = String.Empty;

            ctrFecRenov.Text = String.Empty;
            ctrFecExped.Text = String.Empty;

            ctrFecVcto.Text = String.Empty;
            txtNroDocumentoM.Text = String.Empty;

            ctrFecNac.Text = String.Empty;
            LblEdad2.Text = String.Empty;

            TxtEmail.Text = String.Empty;

            GrdDirecciones.DataSource = null;
            GrdDirecciones.DataBind();

            updTabDatAdic.Update();
            UpdDirecciones.Update();
            UpdContacto.Update();
            Updmiratorio.Update();
            updobservaciones.Update();

            Session["DtRegFilaciones"] = null;
            Session["GrdFiliacion"] = null;
            Session["GrdFiliacion_Persona"] = null;
            Session["GrdFiliacion_Otros"] = null;

            DataTable lDATATABLE = (DataTable)((Session["DtRegFilaciones"] == null) ? CrearDtRegFilaciones() : Session["DtRegFilaciones"]);

            this.GrdFiliacion.DataSource = lDATATABLE;
            this.GrdFiliacion.DataBind();

            updFiliacion.Update();
        }

        protected void RdoReniec_CheckedChanged(object sender, EventArgs e)
        {
            string StrScript = string.Empty;

            StrScript = @"$(function(){{
                            EnableTipoBusqueda(1);
                        }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTipoBusqueda", StrScript, true);
        }

        protected void RdoDNI_CheckedChanged(object sender, EventArgs e)
        {
            string StrScript = string.Empty;

            StrScript = @"$(function(){{
                            EnableTipoBusqueda(2);
                        }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTipoBusqueda", StrScript, true);
        }

        protected void RdoManual_CheckedChanged(object sender, EventArgs e)
        {
            string StrScript = string.Empty;

            StrScript = @"$(function(){{
                            EnableTipoBusqueda(3);
                        }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTipoBusqueda", StrScript, true);
        }

        protected void lnk_CapturarImagen_Click(object sender, EventArgs e)
        {
            string url = "../Registro/FrmCaptureimage.aspx";
            string s = "window.open('" + url + "', 'popup_window', 'width=900,height=460,left=350,right=350,top=200,resizable=no');";
            Comun.EjecutarScript(Page, s);
        }

        protected void CmbDptoContNac_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CmbDistCiudadNac.Items.Clear();
            this.CmbProvPaisNac.Items.Clear();
            CmbProvPaisNac.Enabled = false;
            if (ViewState["Ubigeo"] != null)
            {
                beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                obeUbigeoListas = (beUbigeoListas)ViewState["Ubigeo"];
                List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                lbeUbicaciongeografica = Comun.obtenerListaUbiGeo("02", CmbDptoContNac.SelectedValue, "", obeUbigeoListas.Ubigeo02);
                lbeUbicaciongeografica.Insert(0, new beUbicaciongeografica { Ubi02 = "00", Provincia = "-- SELECCIONE --" });
                CmbProvPaisNac.DataSource = lbeUbicaciongeografica;
                CmbProvPaisNac.DataValueField = "Ubi02";
                CmbProvPaisNac.DataTextField = "Provincia";
                CmbProvPaisNac.DataBind();
            }


            //this.CmbProvPaisNac.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
            this.CmbDistCiudadNac.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
            this.LblDescGentilicio.Text = "";
            if (CmbDptoContNac.SelectedIndex > 0)
            {
                CmbProvPaisNac.Enabled = true;

            //    Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, CmbDptoContNac.SelectedValue, "", "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, CmbProvPaisNac);
            //    //FillWebCombo(Comun.ObtenerProvincias(Session, CmbDptoContNac.SelectedValue.ToString()), CmbProvPaisNac, "ubge_vProvincia", "ubge_cUbi02");

            //    CmbDistCiudadNac.SelectedIndex = -1;

            //    CmbDptoContNac.Focus();
            }
            //else
            //{
            //    CmbProvPaisNac.SelectedIndex = -1;

            //    CmbDistCiudadNac.SelectedIndex = -1;
            //}
            ActualizarGenero();
            updTabDatAdic.Update();

        }

        protected void CmbProvPaisNac_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbProvPaisNac.SelectedIndex > 0)
            {
                CmbDistCiudadNac.Enabled = true;

                if (ViewState["Ubigeo"] != null)
                {
                    beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                    obeUbigeoListas = (beUbigeoListas)ViewState["Ubigeo"];
                    List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                    lbeUbicaciongeografica = Comun.obtenerListaUbiGeo("03", CmbDptoContNac.SelectedValue, CmbProvPaisNac.SelectedValue, obeUbigeoListas.Ubigeo03);
                    lbeUbicaciongeografica.Insert(0, new beUbicaciongeografica { Ubi03 = "00", Distrito = "-- SELECCIONE --" });
                    CmbDistCiudadNac.DataSource = lbeUbicaciongeografica;
                    CmbDistCiudadNac.DataValueField = "Ubi03";
                    CmbDistCiudadNac.DataTextField = "Distrito";
                    CmbDistCiudadNac.DataBind();
                    CmbDistCiudadNac.Enabled = (CmbProvPaisNac.SelectedValue.Equals("00") ? false : true);
                    CmbDistCiudadNac.Focus();
                }

                //Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.DISTRITO_CIUD, CmbDptoContNac.SelectedValue, CmbProvPaisNac.SelectedValue, "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, CmbDistCiudadNac);
                //FillWebCombo(Comun.ObtenerDistritos(Session, CmbDptoContNac.SelectedValue.ToString(), CmbProvPaisNac.SelectedValue.ToString()), CmbDistCiudadNac, "ubge_vDistrito", "ubge_cUbi03");

                //Session["CmbProvPaisNac"] = CmbProvPaisNac.SelectedValue;

                if (CmbDistCiudadNac.Enabled == true)
                    CmbProvPaisNac.Focus();
                //-----------------------------------------------
                //Fecha: 23/03/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Asignar el gentilicio
                //-----------------------------------------------
                string strDptoContNac = CmbDptoContNac.SelectedValue.ToString();
                string strProvPaisNac = CmbProvPaisNac.SelectedValue.ToString();
                string strGenero = CmbGenero.SelectedValue.ToString();


                //string strGentilicio = Comun.AsignarGentilicio(Session, strDptoContNac, strProvPaisNac, strGenero).ToString();
                //jonatan silva - se obtiene el gentilicio sin ir nuevamente a la base de datos 12/01/2018
                string strGentilicio = Comun.AsignarGentilicioPorCodigoPais(Session, strDptoContNac, strProvPaisNac, strGenero).ToString();

                this.LblDescGentilicio.Text = strGentilicio;
                //-----------------------------------------------               
            }
            else
            {
                CmbDistCiudadNac.SelectedIndex = -1;
                CmbDistCiudadNac.Enabled = false;
                this.LblDescGentilicio.Text = "";
            }


            VerificarPadresPeruanos();
            ActualizarGenero();
            updTabDatAdic.Update();
        }

        private void VerificarPadresPeruanos()
        {
            if (Convert.ToInt16(CmbNacRecurr.SelectedValue) == Convert.ToInt16(Enumerador.enmNacionalidad.PERUANA))
            {
                if (Convert.ToInt16(CmbTipoDoc.SelectedValue) == Convert.ToInt16(Enumerador.enmTipoDocumento.CUI)
                    || Convert.ToInt16(CmbTipoDoc.SelectedValue) == Convert.ToInt16(Enumerador.enmTipoDocumento.DNI))
                {
                    if (this.LblDescGentilicio.Text != "")
                    {
                        chkPeruanoMadrePadrePeruno.Visible = true;
                        chkPeruanoMadrePadrePeruno.Checked = true;
                    }
                    else
                    {
                        if (Convert.ToInt16(CmbDptoContNac.SelectedValue) > 90)
                        {
                            chkPeruanoMadrePadrePeruno.Visible = true;
                            chkPeruanoMadrePadrePeruno.Checked = true;
                        }
                        else
                        {
                            chkPeruanoMadrePadrePeruno.Visible = false;
                            chkPeruanoMadrePadrePeruno.Checked = false;
                        }
                    }
                }
                else {
                    chkPeruanoMadrePadrePeruno.Visible = false;
                    chkPeruanoMadrePadrePeruno.Checked = false;
                }
                
            }
            else {
                chkPeruanoMadrePadrePeruno.Visible = false;
                chkPeruanoMadrePadrePeruno.Checked = false;
            }
            UpdDatosPersona.Update(); 
        }

        protected void CmbDistCiudadNac_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbDistCiudadNac.SelectedIndex > 0)
            {
                Session["CmbDistCiudadNac"] = CmbDistCiudadNac.SelectedValue;
                CmbDistCiudadNac.Focus();
            }
            ActualizarGenero();
        }

        void MyControl_btnNuevo()
        {
            string StrScript = string.Empty;

            Button BtnPrintE = (Button)ctrlToolBarButton.FindControl("btnImprimir");
            BtnPrintE.Enabled = false;

            LblEdad1.Visible = false;
            //LblEdad2.Visible = false;

            txtObsRENIEC.Text = "NINGUNO";
            ViewState.Add("AccionDocumento", "Nuevo");



            Session["OperRune"] = true;

            Session["IntOperFiliacion"] = true;

            /***********************************************************************************************************************************************/
            /*************************************************Seccion para la cabecera del Formulario del RUNE**********************************************/
            /***********************************************************************************************************************************************/
            Button BtnEliminar = (Button)ctrlToolBarButton.FindControl("btnEliminar");
            BtnEliminar.Enabled = false;

            Button BtnGuardar = (Button)ctrlToolBarButton.FindControl("btnGrabar");
            BtnGuardar.Enabled = true;


            ValidarApellidoCasadaMostrar();

            ViewState["DtPersonaAct"] = null;

            imgPersona.ImageUrl = "~/Images/People.png";
            imgFirma.ImageUrl = "~/Images/Firma.png";

            StrScript = @"$(function(){{
                            LimpiarCabeceraRUNE();
                        }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "LimpiarCabeceraRUNE", StrScript, true);

            RdoGrabCrearNewAct.Checked = true;
            chk_PJ.Checked = false;
            RdoViveSi.Checked = true;
            RdoSoloGrab.Checked = false;
            RdoViveNo.Checked = false;

            CmbTipoDoc.Enabled = true;
            txtDescOtroDocumento.Enabled = true;
            txtNroDoc.Enabled = true;
            lblCO_OtroDoc.Visible = true;

            btnValidarDni.Visible = true;
            btnValidarDni.Enabled = true;
            chkValidarConReniec.Visible=true;
            chkValidarConReniec.Checked = true;
            txtApePat.Enabled = false;
            txtApeMat.Enabled = false;
            txtApepCas.Enabled = false;
            txtNombres.Enabled = false;
            
            Session["dniAnterior"] = "";

            UpdDatosPersona.Update();
            /***********************************************************************************************************************************************/

            /***********************************************************************************************************************************************/
            /****************************************Seccion para la pestaña de datos adicionales y observaciones*******************************************/
            /***********************************************************************************************************************************************/
            StrScript = @"$(function(){{
                            LimpiarPestañaAdicionalesYObservaciones();
                        }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "LimpiarPestañaAdicionalesYObservaciones", StrScript, true);
            //this.CmbDptoContNac.Items.Clear();
            this.CmbProvPaisNac.Items.Clear();
            this.CmbDistCiudadNac.Items.Clear();
            //comun_Part3.CargarUbigeo(Session, CmbDptoContNac, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, CmbNacRecurr.SelectedValue.ToString(), "", true, Enumerador.enmNacionalidad.PERUANA);
            this.CmbProvPaisNac.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
            this.CmbDistCiudadNac.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
            this.ctrFecNac.Text = String.Empty;
            
            updTabDatAdic.Update();
            /***********************************************************************************************************************************************/

            /***********************************************************************************************************************************************/
            /*************************************************Seccion para la pestaña de documentos*********************************************************/
            /***********************************************************************************************************************************************/
            StrScript = @"$(function(){{
                            LimpiarPestañaDocumentos();
                        }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "LimpiarPestañaDocumentos", StrScript, true);

            Grd_Documentos.DataSource = null;
            Grd_Documentos.DataBind();

            this.ctrFecVcto.Text = String.Empty;
            this.ctrFecRenov.Text = String.Empty;
            this.ctrFecExped.Text = String.Empty;

            updDocumentos.Update();
            /***********************************************************************************************************************************************/

            /***********************************************************************************************************************************************/
            /**************************************************Seccion para la pestaña de direcciones*******************************************************/
            /***********************************************************************************************************************************************/
            StrScript = @"$(function(){{
                            LimpiarPestañaDirecciones();
                        }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "LimpiarPestañaFilaciones", StrScript, true);

            Session.Remove("DtRegDirecciones");
            Session["DtRegDirecciones"] = CrearDtRegDirecciones();
            ((DataTable)Session["DtRegDirecciones"]).Clear();

            GrdDirecciones.DataSource = Session["DtRegDirecciones"];
            GrdDirecciones.DataBind();

            UpdDirecciones.Update();
            /***********************************************************************************************************************************************/

            /***********************************************************************************************************************************************/
            /************************************************Seccion para la pestaña de filiaciones*********************************************************/
            /***********************************************************************************************************************************************/
            StrScript = @"$(function(){{
                            LimpiarPestañaFilaciones();
                        }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "LimpiarPestañaFilaciones", StrScript, true);

            Session.Remove("DtRegFilaciones");
            Session["DtRegFilaciones"] = CrearDtRegFilaciones();
            ((DataTable)Session["DtRegFilaciones"]).Clear();

            ddl_TipFiliacion.SelectedValue = "0";

            /***********************************************************************************************************************************************/

            /***********************************************************************************************************************************************/
            /***************************************Seccion para la pestaña de contactos y datos migratorios************************************************/
            /***********************************************************************************************************************************************/
            StrScript = @"$(function(){{
                            LimpiarPestañaContactosYDatosMigratorios();
                        }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "LimpiarPestañaContactosYDatosMigratorios", StrScript, true);
            /***********************************************************************************************************************************************/
            RdioSiBenExter.Checked = false;
            RdioSiRetornExt.Checked = false;

            StrScript = @"$(function(){{
                            MoveTabIndex(0);
                        }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex0", StrScript, true);

            StrScript = @"$(function(){{
                                    DisableTabIndex(1);
                                }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "DisableTabIndex1", StrScript, true);
            StrScript = @"$(function(){{
                                    DisableTabIndex(2);
                                }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "DisableTabIndex2", StrScript, true);
            StrScript = @"$(function(){{
                                    DisableTabIndex(3);
                                }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "DisableTabIndex3", StrScript, true);

            StrScript = @"$(function(){{
                                    DisableTabIndex(4);
                                }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "DisableTabIndex4", StrScript, true);

            StrScript = @"$(function(){{
                                    DisableTabIndex(5);
                                }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "DisableTabIndex5", StrScript, true);

            StrScript = @"$(function(){{
                                    DisableTabIndex(6);
                                }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "DisableTabIndex6", StrScript, true);
            ///
            direccionResidencia.Visible = true;
            txtDireccionResidencia.Text = "";
            ddlDptContinenteResidencia.SelectedIndex = 0;
            ddlDptContinenteResidencia_SelectedIndexChanged(null, null);
            ddlDistCiudadResidencia.ClearSelection();
            Session["DtRegFilaciones"] = null;
            Session["GrdFiliacion"] = null;
            Session["GrdFiliacion_Persona"] = null;
            Session["GrdFiliacion_Otros"] = null;

            DataTable lDATATABLE = (DataTable)((Session["DtRegFilaciones"] == null) ? CrearDtRegFilaciones() : Session["DtRegFilaciones"]);

            this.GrdFiliacion.DataSource = lDATATABLE;
            this.GrdFiliacion.DataBind();

            updFiliacion.Update();

        }

        void MyControl_btnBuscar()
        {
            ViewState.Add("AccionDocumento", "Nuevo");
            Session["strBusqueda"] = "R";
            //if (HFGUID.Value.Length > 0)
            //{
            //    Response.Redirect("~/Registro/FrmBusquedaPersona.aspx?GUID=" + HFGUID.Value);
            //}
            //else
            //{
            if (Request.QueryString["CodPer"] != null)
            {
                string codPersona = Request.QueryString["CodPer"].ToString();
                if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                {
                    Response.Redirect("~/Registro/FrmBusquedaPersona.aspx?CodPer=" + codPersona + "&Juridica=1", false);
                }
                else
                { // PERSONA NATURAL
                    string codTipoDocEncriptada = "";
                    string codNroDocumentoEncriptada = "";

                    if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                    {
                        codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                        codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                    }
                    if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                    {
                        Response.Redirect("~/Registro/FrmBusquedaPersona.aspx?CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                    }
                    else
                    {
                        Response.Redirect("~/Registro/FrmBusquedaPersona.aspx?CodPer=" + codPersona, false);
                    }
                }
                
            }
            else {
                Response.Redirect("~/Registro/FrmBusquedaPersona.aspx", false);
            }
            //}
        }

        void MyControl_btnEliminar()
        {


            Int16 sTipoRolId = Convert.ToInt16(Session[Constantes.CONST_SESION_ROL_ID].ToString());

            ViewState.Add("AccionDocumento", "Nuevo");
            string StrScript = string.Empty;

            BE.MRE.RE_PERSONA ObjPersBE = new BE.MRE.RE_PERSONA();

            int IntRpta = 0;

            Proceso MiProc = new Proceso();

            //if (HFGUID.Value.Length > 0)
            //{
            //    ObjPersBE.pers_iPersonaId = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
            //}
            //else
            //{
                ObjPersBE.pers_iPersonaId = Convert.ToInt64(ViewState["iPersonaId"]);
            //}
            
            
            ObjPersBE.pers_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
            ObjPersBE.pers_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

            Object[] miArrayPersIns = new Object[3] { ObjPersBE,
                                                      Convert.ToInt64(Session["RegistroUnicoId"]),
                                                      Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) };


            //se retiro esta validacion por el ticket 387 
            //if ((int)Enumerador.enmTipoRol.ADMINISTRATIVO != sTipoRolId)
            //{
            bool bPoseeActuaciones = bPoseeActuaciones = PoseeActuaciones(Convert.ToInt16(HF_Persona_TipoDocumento_Editando.Value),
                    HF_Persona_NumeroDocumento_Editando.Value);

            if (bPoseeActuaciones)
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", "La persona no puede ser eliminada porque posee actuaciones pendientes.", false, 190, 250);
                Comun.EjecutarScript(Page, StrScript);
                return;
            }

                DataTable _dtParticipacionesPersona;
                PersonaConsultaBL _obj = new PersonaConsultaBL();

                _dtParticipacionesPersona = _obj.ObtenerParticipacionesPersona(Convert.ToInt16(HF_Persona_TipoDocumento_Editando.Value),
                        HF_Persona_NumeroDocumento_Editando.Value);

                if (_dtParticipacionesPersona.Rows[0][0].ToString() != "Sin Participación")
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", _dtParticipacionesPersona.Rows[0][0].ToString(), false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }
            //}


            IntRpta = (int)MiProc.Invocar(ref miArrayPersIns,
                                          "SGAC.BE.RE_PERSONA",
                                          Enumerador.enmAccion.ELIMINAR,
                                          Enumerador.enmAplicacion.WEB);


            if (IntRpta > 0)
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "RUNE", "Se Eliminó los datos con éxito.", false, 190, 250);
                Comun.EjecutarScript(Page, StrScript);

                MyControl_btnNuevo();
            }
            else
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "RUNE", "Error. No se pudo realizar la operación", false, 190, 250);
                Comun.EjecutarScript(Page, StrScript);
            }
        }

        void MyControl_btnGrabar()
        {
            string StrScript = string.Empty;
            if (CmbDptoContNac.SelectedIndex > 0)
            {
                if (hubigeoNacimiento.Value.Length == 0)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Debe ingresar el lugar de nacimiento."));
                }
            }
            else
            {
                CmbDptoContNac.Style.Add("border", "solid #888888 1px");
                CmbProvPaisNac.Style.Add("border", "solid #888888 1px");
                CmbDistCiudadNac.Style.Add("border", "solid #888888 1px");
            }
            
            if (CmbNacRecurr.SelectedValue == Convert.ToString((int)Enumerador.enmNacionalidad.PERUANA))
            {
                if (CmbGenero.SelectedValue.ToString() == string.Empty || CmbGenero.SelectedIndex == 0)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Se debe seleccionar el género de la persona."));
                    return;
                }
            }

            if (CmbTipoDoc.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.OTROS) ||
                    CmbTipoDoc.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.PASAPORTE_E))
            {
                if (txtDescOtroDocumento.Text.Length == 0)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Se debe ingresar el nombre del documento"));
                    txtDescOtroDocumento.Style.Add("border", "solid red 1px");
                    return;
                }
                else {
                    txtDescOtroDocumento.Style.Add("border", "solid #888888 1px");
                }
            }


            if (HD_FecNac.Value.Length > 0)
            {
                if (Comun.EsFecha(HD_FecNac.Value) == false)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "RUNE", "La fecha de nacimiento no es válida."));
                    return;
                }
                else
                {
                    ctrFecNac.set_Value = Comun.FormatearFecha(HD_FecNac.Value);
                }
            }

            //----------------------------------------------------
            // Autor: Miguel Márquez Beltrán
            // Fecha: 09/06/2017
            // Validar el registro de la dirección de residencia
            //----------------------------------------------------
            // En caso sea peruano no debe ser obligatorio la direccion -- REQ. RITA
            if (CmbNacRecurr.SelectedValue == Convert.ToString((int)Enumerador.enmNacionalidad.PERUANA))
            {
                bool bolEsNuevo = (bool)Session["OperRune"];

                if (bolEsNuevo)
                {
                    if (txtDireccionResidencia.Text.Length == 0)
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Se debe ingresar la dirección de residencia."));
                        txtDireccionResidencia.Style.Add("border", "solid red 1px");
                        return;
                    }
                    if (hubigeoResidencia.Value.Length == 0)
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Ingrese los datos del lugar de residencia."));
                        return;
                    }
                    ddlDptContinenteResidencia.Style.Add("border", "solid #888888 1px");
                    ddlProvPaisResidencia.Style.Add("border", "solid #888888 1px");
                    ddlDistCiudadResidencia.Style.Add("border", "solid #888888 1px");
                }
                else {
                    if (Session["DtRegDirecciones"] == null)
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Se debe registrar la dirección de residencia de la persona."));
                        return;
                    }
                    else
                    {
                        DataTable dtDireccion = new DataTable();
                        dtDireccion = (DataTable)Session["DtRegDirecciones"];
                        bool bExisteDirResidencia = false;
                        for (int i = 0; i < dtDireccion.Rows.Count; i++)
                        {
                            if (dtDireccion.Rows[i]["vResidenciaTipo"].ToString().ToUpper() == "RESIDENCIA")
                            {
                                bExisteDirResidencia = true;
                                break;
                            }
                        }
                        if (!bExisteDirResidencia)
                        {
                            Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Se debe registrar la dirección de residencia de la persona."));
                            return;
                        }
                        dtDireccion.Dispose();
                    }
                }

                
            }
            
            //----------------------------------------------------

            ViewState.Add("AccionDocumento", "Nuevo");

            txtObsRENIEC.Text = "NINGUNO";

            
            int bFlagIngresoFoto = 0;

            BE.MRE.RE_PERSONA ObjPersBE = new BE.MRE.RE_PERSONA();
            BE.MRE.RE_PERSONAIDENTIFICACION ObjPersIdentBE = new BE.MRE.RE_PERSONAIDENTIFICACION();
            BE.MRE.RE_REGISTROUNICO ObjRegistroUnicoBE = new BE.MRE.RE_REGISTROUNICO();

            //---------------------------------------------------------------
            ObjPersBE.pers_sNacionalidadId = Convert.ToInt16(CmbNacRecurr.SelectedValue);

            string strPaisOrigen = "0";

            strPaisOrigen = Comun.AsignarPaisOrigen(Session, CmbDptoContNac, CmbProvPaisNac);
            if (strPaisOrigen != "0")
            {
                ObjPersBE.pers_sPaisId = Convert.ToInt16(strPaisOrigen);
            }
            ObjPersBE.pers_sPersonaTipoId = Convert.ToInt16(CmbTipPers.SelectedValue);

            ObjPersIdentBE.peid_sDocumentoTipoId = Convert.ToInt16(CmbTipoDoc.SelectedValue);


            /*Para controlar los espacios en blanco a la hora de pistolear DNI*/
            txtNroDoc.Text = txtNroDoc.Text.Trim();
            txtRazSoc.Text = txtRazSoc.Text.Trim();
            txtApePat.Text = txtApePat.Text.Trim();
            txtApeMat.Text = txtApeMat.Text.Trim();
            txtApepCas.Text = txtApepCas.Text.Trim();
            txtNombres.Text = txtNombres.Text.Trim();

            ObjPersIdentBE.peid_vDocumentoNumero = txtNroDoc.Text.Trim().ToUpper();// se movio de la linea 884 a 894
            ObjPersIdentBE.peid_vTipodocumento = txtDescOtroDocumento.Text.Trim().ToUpper();
            ObjPersIdentBE.pers_bValidacionReniec = txtValidacionReniec.Text.Trim().Equals("1");

            ObjPersBE.pers_vApellidoPaterno = txtApePat.Text.Trim().ToUpper();
            ObjPersBE.pers_vApellidoMaterno = txtApeMat.Text.Trim().ToUpper();
            ObjPersBE.pers_vApellidoCasada = txtApepCas.Text.Trim().ToUpper();
            ObjPersBE.pers_vNombres = txtNombres.Text.Trim().ToUpper();
            ObjPersBE.pers_sEstadoCivilId = Convert.ToInt16(CmbEstCiv.SelectedValue);
            ObjPersBE.pers_sGeneroId = Convert.ToInt16(CmbGenero.SelectedValue);
            ObjPersBE.pers_sOcupacionId = Convert.ToInt16(CmbOcupacion.SelectedValue);
            ObjPersBE.pers_sProfesionId = Convert.ToInt16(ddl_Profesion.SelectedValue);
            ObjPersBE.pers_sGradoInstruccionId = Convert.ToInt16(CmbGradInst.SelectedValue);
            
            //if (ctrFecNac.Text.Length > 0)
            if (HD_FecNac.Value.Length > 0)
            {               
                DateTime datFecha = new DateTime();
                //if (!DateTime.TryParse(ctrFecNac.Text, out datFecha))
                if (!DateTime.TryParse(HD_FecNac.Value, out datFecha))
                {
                    //datFecha = Comun.FormatearFecha(ctrFecNac.Text);
                    datFecha = Comun.FormatearFecha(HD_FecNac.Value);
                }
                ObjPersBE.pers_dNacimientoFecha = datFecha;
            }

            if (CmbDptoContNac.SelectedValue == "0" && CmbProvPaisNac.SelectedValue == "00" && CmbDistCiudadNac.SelectedValue == "00")
            {
                Session["CmbProvPaisNac"] = null;
                Session["CmbDistCiudadNac"] = null;
            }
            
            //ObjPersBE.pers_cNacimientoLugar = CmbDptoContNac.SelectedValue + CmbProvPaisNac.SelectedValue + CmbDistCiudadNac.SelectedValue;
            ObjPersBE.pers_cNacimientoLugar = hubigeoNacimiento.Value;
            
            if (ObjPersBE.pers_cNacimientoLugar == "00000" || ObjPersBE.pers_cNacimientoLugar == "000000")
            {
                ObjPersBE.pers_cNacimientoLugar = null;
            }
            if (ObjPersBE.pers_cNacimientoLugar == "")
            {
                ObjPersBE.pers_cNacimientoLugar = null;
            }
            ObjPersBE.pers_vCorreoElectronico = TxtEmail.Text.ToUpper();
            ObjPersBE.pers_vObservaciones = TxtObsRune.Text.Trim().ToUpper().Replace("'", "''");

            Boolean pers_bFallecidoFlag = true;
            if (RdoViveSi.Checked)
                pers_bFallecidoFlag = false;

            ObjPersBE.pers_bFallecidoFlag = pers_bFallecidoFlag;
            //======pipa: falta verifiacr el porque no graba form datos  de contacto
            ObjRegistroUnicoBE.reun_vEmergenciaNombre = TxtNomCompCont.Text.ToUpper();
            ObjRegistroUnicoBE.reun_sEmergenciaRelacionId = Convert.ToInt16(CmbRelCto.SelectedValue);
            ObjRegistroUnicoBE.reun_vEmergenciaDireccionLocal = TxtDirExtrCont.Text.Trim().ToUpper().Replace("'", "''");
            ObjRegistroUnicoBE.reun_vEmergenciaCodigoPostal = TxtCodPostCont.Text.ToUpper();
            ObjRegistroUnicoBE.reun_vEmergenciaTelefono = TxtTelfCont.Text;
            ObjRegistroUnicoBE.reun_vEmergenciaDireccionPeru = TxtDirPerCont.Text.Trim().ToUpper().Replace("'", "''");
            ObjRegistroUnicoBE.reun_vEmergenciaCorreoElectronico = TxtMailCont.Text.ToUpper();

            ObjRegistroUnicoBE.reun_cViveExteriorDesde = txtAñoVivDesde.Text + ddl_MesVivDesde.SelectedValue;

            ObjRegistroUnicoBE.reun_bPiensaRetornarAlPeru = RdioSiRetornExt.Checked;

            ObjRegistroUnicoBE.reun_cCuandoRetornaAlPeru = txtAñoRegreso.Text + ddl_MesRegreso.SelectedValue;
            ObjRegistroUnicoBE.reun_bAfiliadoSeguroSocial = RdioSiAfilSegSoc.Checked;
            ObjRegistroUnicoBE.reun_bAfiliadoAFP = RdioSiAfilAFP.Checked;
            ObjRegistroUnicoBE.reun_bAportaSeguroSocial = RdioSiAportSegSoc.Checked;

            ObjRegistroUnicoBE.reun_bBeneficiadoExterior = RdioSiBenExter.Checked;

            ObjRegistroUnicoBE.reun_sOcupacionPeru = Convert.ToInt16(CbmOcupPeru.SelectedValue);
            ObjRegistroUnicoBE.reun_sOcupacionExtranjero = Convert.ToInt16(CbmOcupExterior.SelectedValue);
            ObjRegistroUnicoBE.reun_vNombreConvenio = TxtAcuerConv.Text.Trim().ToUpper().Replace("'", "''");

            /*Asigna valor para generar la 58A*/
            bool bC58A = false;
            Int16 CiudadItinerante = 0;
            if (chk_PJ.Checked)
            {
                bC58A = true;
                if (Session[Constantes.CONST_SESION_CIUDAD_ITINERANTE].ToString() != "")
                {
                    CiudadItinerante = Convert.ToInt16(Session[Constantes.CONST_SESION_CIUDAD_CODIGO_ITINERANTE]);
                }
            }            

            ActoCivilConsultaBL funActoCivil = new ActoCivilConsultaBL(); //Caso Para Validar la Existencia de un CUI

            PersonaMantenimientoBL oPersonaMantenimientoBL = new PersonaMantenimientoBL();
            PersonaIdentificacionConsultaBL objPersonasIdBL = new PersonaIdentificacionConsultaBL();
            int IntRpta = 0;
            if (Convert.ToBoolean(Session["OperRune"]))
            {
                long LonPersonaId = 0;

                #region Validación
                IntRpta = objPersonasIdBL.Existe(Convert.ToInt32(CmbTipoDoc.SelectedValue), txtNroDoc.Text.Trim(), 0, 1);

                if (IntRpta == 1)
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", "Ya existe el número de documento que esta consignando.", false, 190, 250);
                    StrScript = StrScript + "RecargarUbigeoNacimiento(); RecargarUbigeoResidencia();";
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }

                if (CmbTipoDoc.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.DNI))
                {
                    if (txtNroDoc.Text.Trim().Length != 0)
                    {
                        IntRpta = funActoCivil.ExisteCui(txtNroDoc.Text, 0, 1, Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]), Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_DIRECCION_IP]), Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                        if (IntRpta == 1)
                        {
                            StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", "Ya existe el número de documento que esta consignando.", false, 190, 250);
                            Comun.EjecutarScript(Page, StrScript);
                            return;
                        }
                    }
                }
                #endregion

                #region Persona Foto
                capturaconvertimagenes();

                if (imgPersona.ImageUrl == "~/Images/People.png")
                {
                    bFlagIngresoFoto = 0;
                    Session["DtRegImagenes"] = null;
                }
                else
                {
                    DataRow row;
                    row = ((DataTable)Session["DtRegImagenes"]).NewRow();
                    row["iPersonaFotoId"] = 0;
                    row["iPersonaId"] = 0;
                    row["sFotoTipoId"] = 1;
                    row["GFoto"] = image;
                    ((DataTable)Session["DtRegImagenes"]).Rows.Add(row);
                    ((DataTable)this.Session["DtRegImagenes"]).AcceptChanges();
                    bFlagIngresoFoto = 1;
                    (Session["CapturedImageDevuelta"]) = null;
                }
                #endregion

                #region Registrar Persona
                ObjPersBE.pers_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjPersBE.pers_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                ObjPersBE.pers_bPadresPeruanos = chkPeruanoMadrePadrePeruno.Checked;
                BE.MRE.RE_RESIDENCIA objResidencia = new BE.MRE.RE_RESIDENCIA();

                objResidencia.resi_vResidenciaDireccion = txtDireccionResidencia.Text.Trim().ToUpper();
                objResidencia.resi_sResidenciaTipoId = (Int16)Enumerador.enmTipoResidencia.RESIDENCIA;
                //objResidencia.resi_cResidenciaUbigeo = ddlDptContinenteResidencia.SelectedValue +
                //                           ddlProvPaisResidencia.SelectedValue +
                //                           ddlDistCiudadResidencia.SelectedValue;
                objResidencia.resi_cResidenciaUbigeo = hubigeoResidencia.Value;

                objResidencia.resi_vCodigoPostal = txtCodPostalDir.Text;
                objResidencia.resi_vResidenciaTelefono = txtTelefonoDir.Text;

                //-------------------------------------------------------------------------
                //Fecha: 21/04/2021
                //Autor: Miguel Márquez Beltrán
                //Motivo: Actualizar la dirección desde el objeto: objResidencia.
                //Requerimiento: Whatsap
                //-------------------------------------------------------------------------
              //  if (Convert.ToInt16(CmbNacRecurr.SelectedValue) == Convert.ToInt16(Enumerador.enmNacionalidad.PERUANA))
              //  {
                    
                    IntRpta = oPersonaMantenimientoBL.Insertar(ObjPersBE, ObjPersIdentBE, ObjRegistroUnicoBE, bFlagIngresoFoto, objResidencia, (DataTable)Session["DtRegFilaciones"], (DataTable)Session["DtRegImagenes"], Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), bC58A, CiudadItinerante, ref LonPersonaId);

                //}
                //else {
                //    IntRpta = oPersonaMantenimientoBL.Insertar(ObjPersBE, ObjPersIdentBE, ObjRegistroUnicoBE, bFlagIngresoFoto, (DataTable)Session["DtRegDirecciones"], (DataTable)Session["DtRegFilaciones"], (DataTable)Session["DtRegImagenes"], Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]), bC58A, CiudadItinerante, ref LonPersonaId);
                //}

                //==== Fecha:09/11/2020, Autor: Pipa
                //==== Motivo: se adiciona la siguiente linea, por lo q es necesario que retorne el valor identity de la 
                //==== tabla RegistroUnico (reun_iRegistroUnicoId) para reutilizar en boton grabar en pestaña Datos del Contacto
                Session["RegistroUnicoId"] = ObjPersBE.REGISTROUNICO.reun_iRegistroUnicoId;

                if (LonPersonaId > 0)
                {
                    
                    //if (HFGUID.Value.Length > 0)
                    //{
                    //    Session["iPersonaId" + HFGUID.Value] = LonPersonaId;
                    //}
                    //else
                    //{
                        ViewState["iPersonaId"] = LonPersonaId;
                    //}

                    //------------------------------------INIT MODIFICACION VPIPA
                    // AUTOR:VPIPA
                    // FECHA:21/01/2022
                    // MOTIVO: EN CASO PERUANA Y CON DNI, DEBE DE INSERTAR POR DEFECTO NACIONALIDAD PERUANA  COMO VIGENTE 
                    // REQ: NRO 16: IVAN Y MMAR ECPP-SAGACv.2.2.31.284554_IVAN_2.DOCX
                    if (CmbNacRecurr.SelectedItem.Text.Equals("PERUANA") && CmbTipoDoc.SelectedItem.Text.Equals("DNI"))
                    {
                        Int16 paisID = 0;
                        DataTable dtPaises = new DataTable();
                        dtPaises = Comun.ConsultarPaises();
                        for (int i = 0; i < dtPaises.Rows.Count; i++)
                        {
                            if (dtPaises.Rows[i]["PAIS_VNOMBRE"].ToString().Equals("PERÚ"))
                            {
                                paisID = Convert.ToInt16(dtPaises.Rows[i]["PAIS_SPAISID"].ToString());
                                break;
                            }
                        }
                        Int64 iPersonaID = Convert.ToInt64(ViewState["iPersonaId"]);
                        Int16 iUsuario = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                        int resultado = oPersonaMantenimientoBL.RegistrarNacionalidad(iPersonaID, paisID, CmbNacRecurr.SelectedItem.Text, true, "A", iUsuario);
                        if (chkNacVigente.Checked)
                        {
                            if (resultado > -1)
                            {
                                lblNacionalidadVigenteCabecera.Text = lblNacionalidadVigente.Text;
                                UpdDatosPersona.Update();
                            }
                        }
                        LimpiarDatosNacionalidad();
                        ListarNacionalidades(iPersonaID);
                    }
                    //------------------------------------FIN MODIFICACION VPIPA

                    /*Para Enviar mensaje cuando se registra una actuación 58A*/
                    //--------------------------------------------                    
                    // Creador por: Jonatan Silva Cachay
                    // Fecha: 02/02/2017
                    // Objetivo: Envio de Correo y actualiza el flag de enviado
                    //--------------------------------------------
                    DateTime dtFechaInicioEnvio = Convert.ToDateTime(comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.CONFIG_SERVIDOR_CORREO, (int)Enumerador.enmConfiguracionCorreo.FECHA_INICIO_ENVIO, "descripcion"));

                    if (DateTime.Now >= dtFechaInicioEnvio)
                    {

                        if (bC58A)
                        {
                            //----------------------------------------------
                            //Fecha: 07/02/2017
                            //Autor: Jonatan Silva Cachay
                            //Se agrego consulta para obtener datos del Registro 58A
                            //----------------------------------------------
                            ActuacionConsultaBL oActuacionConsulta = new ActuacionConsultaBL();
                            DataTable _dt = new DataTable();
                            _dt = oActuacionConsulta.ObtenerActuacion58A(LonPersonaId);
                            if (_dt.Rows.Count > 0)
                            {
                                BE.RE_ACTUACION ObjActuacBE = new BE.RE_ACTUACION();
                                ObjActuacBE.actu_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                                ObjActuacBE.actu_iActuacionId = Convert.ToInt64(_dt.Rows[0]["iActuacionId"].ToString());

                                if (TxtEmail.Text != "")
                                {
                                    bool bEnvio = false;
                                    DataTable _dtDatos = new DataTable();
                                    _dtDatos = crearTabla(Convert.ToInt64(_dt.Rows[0]["vCorrelativoTarifario"].ToString()));
                                    bEnvio = EnviarCorreoRegistro(_dtDatos, TxtEmail.Text, "REGISTRO DE ACTUACIÓN", "/Registro/Plantillas/CorreoRegistroActuacion.html");
                                    if (bEnvio)
                                    {
                                        ActuacionMantenimientoBL BL = new ActuacionMantenimientoBL();
                                        BL.ActualizarFlagEnvioCorreo(ObjActuacBE);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (TxtEmail.Text != "")
                            {
                                bool bEnvio = false;
                                DataTable _dtDatos = new DataTable();
                                _dtDatos = crearTabla();
                                bEnvio = EnviarCorreoRegistro(_dtDatos, TxtEmail.Text, "REGISTRO DE PERSONA", "/Registro/Plantillas/CorreoRegistroPersona.html");
                            }
                        }
                    }

                    
                    //--------------------------------------------------
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "RUNE", "Se ha registrado a una nueva persona.", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);

                   
                    //--------------------------------------------------
                    if (RdoGrabCrearNewAct.Checked)
                    {
                        //-----------------------------
                        #region CrearNewAct

                        //if (HFGUID.Value.Length > 0)
                        //{
                        //    Session["iPersonaTipoId" + HFGUID.Value] = CmbTipPers.SelectedValue;
                        //    Session["iDocumentoTipoId" + HFGUID.Value] = CmbTipoDoc.SelectedValue;
                        //    Session["DescTipDoc" + HFGUID.Value] = CmbTipoDoc.SelectedItem.ToString();
                        //    Session["ApePat" + HFGUID.Value] = txtApePat.Text.ToUpper();
                        //    Session["ApeMat" + HFGUID.Value] = txtApeMat.Text.ToUpper();
                        //    Session["Nombres" + HFGUID.Value] = txtNombres.Text.ToUpper();
                        //    Session["Nombre" + HFGUID.Value] = txtApePat.Text.ToUpper() + ' ' + txtApeMat.Text.ToUpper() + ' ' + txtNombres.Text.ToUpper();
                        //    Session["NroDoc" + HFGUID.Value] = txtNroDoc.Text.ToUpper();
                        //    Session["FecNac" + HFGUID.Value] = ctrFecNac.Text.ToUpper();
                        //    Session["PER_NACIONALIDAD" + HFGUID.Value] = ObjPersBE.pers_sNacionalidadId.ToString();
                        //}
                        //else
                        //{
                            ViewState["iPersonaTipoId"] = CmbTipPers.SelectedValue;
                            ViewState["iDocumentoTipoId"] = CmbTipoDoc.SelectedValue;
                            ViewState["DescTipDoc"] = CmbTipoDoc.SelectedItem.ToString();
                            ViewState["ApePat"] = txtApePat.Text.ToUpper();
                            ViewState["ApeMat"] = txtApeMat.Text.ToUpper();
                            //------------------------------------------------------
                            //Autor: Miguel Márquez Beltrán
                            //Fecha: 05/01/2021
                            //Motivo: Mostrar el apellido de casada.
                            //------------------------------------------------------
                            ViewState["ApeCasada"] = txtApepCas.Text.ToUpper();
                            //------------------------------------------------------
                            ViewState["Nombre"] = txtNombres.Text.ToUpper();
                            ViewState["Nombres"] = txtApePat.Text.ToUpper() + ' ' + txtApeMat.Text.ToUpper() + ' ' + txtNombres.Text.ToUpper();
                            ViewState["NroDoc"] = txtNroDoc.Text.ToUpper();
                            //ViewState["FecNac"] = ctrFecNac.Text.ToUpper();
                            ViewState["FecNac"] = HD_FecNac.Value.ToUpper();
                            ViewState["PER_NACIONALIDAD"] = ObjPersBE.pers_sNacionalidadId.ToString();
                        //}
                        //-----------------------------

                        DateTime dFechaNac;
                        string strEdad = "0";
                        //if (ctrFecNac.Text.ToUpper() != "")
                        if (HD_FecNac.Value.ToUpper() != "")
                        {
                            //dFechaNac = Comun.FormatearFecha(ctrFecNac.Text.ToUpper());
                            dFechaNac = Comun.FormatearFecha(HD_FecNac.Value.ToUpper());
                            strEdad = ObtenerEdad(dFechaNac, true);
                        }
                        else {
                            strEdad = "200";
                        }

                        //if (HFGUID.Value.Length > 0)
                        //{
                        //    Response.Redirect("~/Registro/FrmActuacion.aspx?RecuE=" + strEdad + "&GUID=" + HFGUID.Value);
                        //}
                        //else
                        //{
                        if (Request.QueryString["CodPer"] != null)
                        {
                            string codPersona = Request.QueryString["CodPer"].ToString();
                            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                            {
                                Response.Redirect("~/Registro/FrmActuacion.aspx?RecuE=" + strEdad + "&CodPer=" + codPersona + "&Juridica=1", false);
                            }
                            else
                            { // PERSONA NATURAL
                                string codTipoDocEncriptada = "";
                                string codNroDocumentoEncriptada = "";

                                if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                                {
                                    codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                                    codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                                }
                                if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                                {
                                    Response.Redirect("~/Registro/FrmActuacion.aspx?RecuE=" + strEdad + "&CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                                }
                                else
                                {
                                    Response.Redirect("~/Registro/FrmActuacion.aspx?RecuE=" + strEdad + "&CodPer=" + codPersona, false);
                                }
                            }
                            
                        }
                        else {
                            string codPersona = Util.Encriptar(LonPersonaId.ToString());
                            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                            {
                                Response.Redirect("~/Registro/FrmActuacion.aspx?RecuE=" + strEdad + "&CodPer=" + codPersona + "&Juridica=1", false);
                            }
                            else
                            { // PERSONA NATURAL
                                string codTipoDocEncriptada = "";
                                string codNroDocumentoEncriptada = "";

                                if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                                {
                                    codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                                    codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                                }
                                if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                                {
                                    Response.Redirect("~/Registro/FrmActuacion.aspx?RecuE=" + strEdad + "&CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                                }
                                else
                                {
                                    Response.Redirect("~/Registro/FrmActuacion.aspx?RecuE=" + strEdad + "&CodPer=" + codPersona, false);
                                }
                            }
                            
                        }
                        
                        //}

                        #endregion
                    }
                    else
                    {

                        #region NO_CrearNewAct

                                        //----------------------------------------------
                                        //Fecha: 05/01/2017
                                        //Autor: Miguel Márquez Beltrán
                                        //Objetivo: Habilitar el botón imprimir RUNE y 
                                        //          actualizar los datos de la persona
                                        //----------------------------------------------
                                        //Session["OperRune"] = true;
                                        //----------------------------------------------
                                        Session["OperRune"] = false;

                                        Button BtnEliminar = (Button)ctrlToolBarButton.FindControl("btnEliminar");
                                        BtnEliminar.Enabled = false;

                                        Button BtnImprimir = (Button)ctrlToolBarButton.FindControl("btnImprimir");
                                        //----------------------------------------------
                                        //Fecha: 05/01/2017
                                        //Autor: Miguel Márquez Beltrán
                                        //Objetivo: Habilitar el botón imprimir RUNE y 
                                        //          actualizar los datos de la persona
                                        //----------------------------------------------
                                        //BtnImprimir.Enabled = false;
                                        //----------------------------------------------
                                        BtnImprimir.Enabled = true;


                                        Button BtnGuardar = (Button)ctrlToolBarButton.FindControl("btnGrabar");
                                        BtnGuardar.Enabled = true;


                                        StrScript = @"$(function(){{
                                                        EnableTabIndex(7);
                                                    }});";
                                        StrScript = string.Format(StrScript);
                                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTabIndex", StrScript, true);

                                        StrScript = @"$(function(){{
                                                        MoveTabIndex(0);
                                                    }});";
                                        StrScript = string.Format(StrScript);
                                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex", StrScript, true);

                                        StrScript = @"$(function(){{
                                                        EnableTabIndex(1);
                                                    }});";
                                        StrScript = string.Format(StrScript);
                                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTabIndex1", StrScript, true);

                                        StrScript = @"$(function(){{
                                                        EnableTabIndex(2);
                                                    }});";
                                        StrScript = string.Format(StrScript);
                                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTabIndex2", StrScript, true);

                                        StrScript = @"$(function(){{
                                                        EnableTabIndex(3);
                                                    }});";
                                        StrScript = string.Format(StrScript);
                                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTabIndex3", StrScript, true);

                                        StrScript = @"$(function(){{
                                                        EnableTabIndex(4);
                                                    }});";
                                        StrScript = string.Format(StrScript);
                                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTabIndex4", StrScript, true);

                                        StrScript = @"$(function(){{
                                                        EnableTabIndex(5);
                                                    }});";
                                        StrScript = string.Format(StrScript);
                                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTabIndex5", StrScript, true);

                                        StrScript = @"$(function(){{
                                                        EnableTabIndex(6);
                                                    }});";
                                        StrScript = string.Format(StrScript);
                                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTabIndex6", StrScript, true);

                                        StrScript = @"$(function(){{
                                                        RecargarUbigeoNacimiento(); RecargarUbigeoResidencia();
                                                    }});";
                                        StrScript = string.Format(StrScript);
                                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "RecargaUbigeoNac", StrScript, true);
                        
                                        
                                           
                                        direccionResidencia.Visible = false;
                                        CmbTipPers.Enabled = false;
                                        CmbTipoDoc.Enabled = false;
                                        txtDescOtroDocumento.Enabled = false;
                                        txtNroDoc.Enabled = false;
                                         
                                        //if (HFGUID.Value.Length > 0)
                                        //{
                                        //    BindGridDocumentos(Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]));
                                        //    BindGridDirecciones(Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]));
                                        //    ListarNacionalidades(Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]));
                                        //}
                                        //else
                                        //{
                                        BindGridDocumentos(Convert.ToInt64(ViewState["iPersonaId"]));
                                        BindGridDirecciones(Convert.ToInt64(ViewState["iPersonaId"]));
                                        ListarNacionalidades(Convert.ToInt64(ViewState["iPersonaId"]));
                                        //}
                                        updDocumentos.Update();
                                        updNacionalidad.Update();
                                        UpdDirecciones.Update();
                                        updTabDatAdic.Update();
                                        //----------------------------------------------
                                        #endregion
                   
                    }

                    //----------------------------------------------
                    //Fecha: 05/01/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Habilitar el botón imprimir RUNE y 
                    //          actualizar los datos de la persona
                    //----------------------------------------------
                    // LimpiarCamposPersona();
                    //----------------------------------------------

                    UpdDatosPersona.Update();

                   //----------------------------------------------
                    //Fecha: 05/01/2017
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Habilitar el botón imprimir RUNE y 
                    //          actualizar los datos de la persona
                    //----------------------------------------------
                    //DataTable tmp = (DataTable)Session["DtRegDirecciones"];
                    //tmp.Clear();
                    //Session["DtRegDirecciones"] = tmp;
                    //MyControl_btnNuevo();
                    //----------------------------------------------
                }
                else
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "RUNE", Constantes.CONST_MENSAJE_OPERACION_FALLIDA, false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                }
                #endregion
            }
            else
            {
                //if (HFGUID.Value.Length > 0)
                //{
                //    ObjPersBE.pers_iPersonaId = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
                //}
                //else
                //{
                ObjPersBE.pers_iPersonaId = Convert.ToInt64(ViewState["iPersonaId"]);
                //}

                
                
                ObjPersBE.pers_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjPersBE.pers_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);
                ObjPersBE.pers_bPadresPeruanos = chkPeruanoMadrePadrePeruno.Checked;
                //--------------------------------------------------
                //Autor: Miguel Márquez Beltrán
                //Fecha: 30/09/2016
                //Objetivo: Validar la sessión: Session["RegistroUnicoId"] cuando esta vacia.
                //---------------------------------------------------

                if (Session["RegistroUnicoId"].ToString() == "")
                {
                    ObjRegistroUnicoBE.reun_iRegistroUnicoId = 0;
                }
                else
                {
                    ObjRegistroUnicoBE.reun_iRegistroUnicoId = Convert.ToInt64(Session["RegistroUnicoId"]);
                }
                #region Persona Foto
                capturaconvertimagenes();

                if (imgPersona.ImageUrl == "~/Images/People.png")
                {
                    bFlagIngresoFoto = 0;
                    Session["DtRegImagenes"] = null;
                }
                else
                {
                    DataRow row;
                    row = ((DataTable)Session["DtRegImagenes"]).NewRow();
                    if (Session["IdFotoPersona"] != null)
                    {
                        row["iPersonaFotoId"] = (Convert.ToInt64(Session["IdFotoPersona"]));
                    }

                    //if (HFGUID.Value.Length > 0)
                    //{
                    //    row["iPersonaId"] = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
                    //}
                    //else
                    //{
                    row["iPersonaId"] = Convert.ToInt64(ViewState["iPersonaId"]);
                    //}
                    
                    
                    
                    
                    row["sFotoTipoId"] = 1;
                    row["GFoto"] = image;
                    ((DataTable)Session["DtRegImagenes"]).Rows.Add(row);
                    ((DataTable)this.Session["DtRegImagenes"]).AcceptChanges();
                    bFlagIngresoFoto = 1;
                    (Session["CapturedImageDevuelta"]) = null;
                }
                #endregion

                #region Validación
                IntRpta = objPersonasIdBL.Existe(Convert.ToInt32(CmbTipoDoc.SelectedValue),
                                        txtNroDoc.Text.Trim(), ObjPersBE.pers_iPersonaId, 2);

                if (IntRpta == 1)
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", "Ya existe el número de documento que esta consignando.", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }

                if (CmbTipoDoc.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.DNI))
                {
                    if (txtNroDoc.Text.Trim().Length != 0)
                    {
                        IntRpta = funActoCivil.ExisteCui(txtNroDoc.Text, ObjPersBE.pers_iPersonaId, 2, Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_USUARIO_ID]), Convert.ToString(HttpContext.Current.Session[Constantes.CONST_SESION_DIRECCION_IP]), Convert.ToInt16(HttpContext.Current.Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
                        if (IntRpta == 1)
                        {
                            StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", "Ya existe el número de documento que esta consignando.", false, 190, 250);
                            Comun.EjecutarScript(Page, StrScript);
                            return;
                        }
                    }
                }
                #endregion

                // autor:vpipa, se comenta esto por lo q solicitan no bloquear la actualizacion de datos

                //if (CmbTipoDoc.SelectedValue != HF_Persona_TipoDocumento_Editando.Value ||
                //    txtNroDoc.Text != HF_Persona_NumeroDocumento_Editando.Value ||
                //    txtNombres.Text != HF_Persona_Nombre_Editando.Value ||
                //    txtApeMat.Text != HF_Persona_ApellidoMaterno_Editando.Value ||
                //    txtApePat.Text != HF_Persona_ApellidoPaterno_Editando.Value)
                //{

                //    Int16 sTipoRolId = Convert.ToInt16(Session[Constantes.CONST_SESION_ROL_ID].ToString());

                //    if ((int)Enumerador.enmTipoRol.ADMINISTRATIVO != sTipoRolId)
                //    {
                //        bool bPoseeActuaciones = PoseeActuaciones(Convert.ToInt16(HF_Persona_TipoDocumento_Editando.Value),
                //             HF_Persona_NumeroDocumento_Editando.Value);

                //        if (bPoseeActuaciones)
                //        {
                //            StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", "La persona no puede ser modificada porque posee actuaciones pendientes.", false, 190, 250);
                //            Comun.EjecutarScript(Page, StrScript);
                //            return;
                //        }
                //    }

                //}
                IntRpta = oPersonaMantenimientoBL.Actualizar(ObjPersBE,
                                                          ObjPersIdentBE,
                                                          ObjRegistroUnicoBE,
                                                          bFlagIngresoFoto,
                                                          (DataTable)Session["DtRegImagenes"],
                                                          (DataTable)Session["DtRegFilaciones"],
                                                          Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                                                          bC58A, CiudadItinerante);
                #region Consulta si IntRpta > 0
                if (IntRpta > 0)
                {
                    if (RdoGrabCrearNewAct.Checked)
                    {
                        #region CrearNewAct

                        //----------------------------------
                        //Autor: Miguel Márquez Beltrán
                        //Fecha: 28/08/2018
                        //Asignar valores a las sesiones  
                        //----------------------------------
                        //if (HFGUID.Value.Length > 0)
                        //{
                        //    Session["iPersonaTipoId" + HFGUID.Value] = CmbTipPers.SelectedValue;
                        //    Session["iDocumentoTipoId" + HFGUID.Value] = CmbTipoDoc.SelectedValue;
                        //    Session["DescTipDoc" + HFGUID.Value] = CmbTipoDoc.SelectedItem.ToString();
                        //    Session["ApePat" + HFGUID.Value] = txtApePat.Text.ToUpper();
                        //    Session["ApeMat" + HFGUID.Value] = txtApeMat.Text.ToUpper();
                        //    Session["Nombres" + HFGUID.Value] = txtNombres.Text.ToUpper();
                        //    Session["Nombre" + HFGUID.Value] = txtApePat.Text.ToUpper() + ' ' + txtApeMat.Text.ToUpper() + ' ' + txtNombres.Text.ToUpper();
                        //    Session["NroDoc" + HFGUID.Value] = txtNroDoc.Text.ToUpper();
                        //    Session["FecNac" + HFGUID.Value] = ctrFecNac.Text.ToUpper();
                        //    Session["PER_NACIONALIDAD" + HFGUID.Value] = Convert.ToInt16(CmbNacRecurr.SelectedValue);
                        //}
                        //else
                        //{
                        ViewState["iPersonaTipoId"] = CmbTipPers.SelectedValue;
                        ViewState["iDocumentoTipoId"] = CmbTipoDoc.SelectedValue;
                        ViewState["DescTipDoc"] = CmbTipoDoc.SelectedItem.ToString();
                        ViewState["ApePat"] = txtApePat.Text.ToUpper();
                        ViewState["ApeMat"] = txtApeMat.Text.ToUpper();
                        //------------------------------------------------------
                        //Autor: Miguel Márquez Beltrán
                        //Fecha: 05/01/2021
                        //Motivo: Mostrar el apellido de casada.
                        //------------------------------------------------------
                        ViewState["ApeCasada"] = txtApepCas.Text.ToUpper();
                        //------------------------------------------------------
                        ViewState["Nombre"] = txtNombres.Text.ToUpper();
                        ViewState["Nombres"] = txtApePat.Text.ToUpper() + ' ' + txtApeMat.Text.ToUpper() + ' ' + txtNombres.Text.ToUpper();
                        ViewState["NroDoc"] = txtNroDoc.Text.ToUpper();
                        //ViewState["FecNac"] = ctrFecNac.Text.ToUpper();
                        ViewState["FecNac"] = HD_FecNac.Value.ToUpper();
                        ViewState["PER_NACIONALIDAD"] = Convert.ToInt16(CmbNacRecurr.SelectedValue);
                        //}

                        DateTime dFechaNac;
                        string strEdad = "0";
                        //if (ctrFecNac.Text.ToUpper() != "")
                        if (HD_FecNac.Value.ToUpper() != "")
                        {
                            //dFechaNac = Comun.FormatearFecha(ctrFecNac.Text.ToUpper());
                            dFechaNac = Comun.FormatearFecha(HD_FecNac.Value.ToUpper());
                            strEdad = ObtenerEdad(dFechaNac, true);
                        }
                        else
                        {
                            strEdad = "200";
                        }

                        //if (HFGUID.Value.Length > 0)
                        //{
                        //    Response.Redirect("~/Registro/FrmActuacion.aspx?RecuE=" + strEdad + "&GUID=" + HFGUID.Value);
                        //}
                        //else
                        //{
                        string codPersona;
                        if(Request.QueryString["CodPer"]!=null)
                        {
                            codPersona = Request.QueryString["CodPer"].ToString();
                        }
                        else{
                            codPersona = Util.Encriptar(ObjPersBE.pers_iPersonaId.ToString());
                        }
                        
                        if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
                        {
                            Response.Redirect("~/Registro/FrmActuacion.aspx?RecuE=" + strEdad + "&CodPer=" + codPersona + "&Juridica=1", false);
                        }
                        else
                        { // PERSONA NATURAL
                            string codTipoDocEncriptada = "";
                            string codNroDocumentoEncriptada = "";

                            if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                            {
                                codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                                codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                            }
                            if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                            {
                                Response.Redirect("~/Registro/FrmActuacion.aspx?RecuE=" + strEdad + "&CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                            }
                            else
                            {
                                Response.Redirect("~/Registro/FrmActuacion.aspx?RecuE=" + strEdad + "&CodPer=" + codPersona, false);
                            }
                        }
                        
                        //}
                        #endregion
                    }
                    else
                    {
                        #region NO_CreaNewAct

                        Session["OperRune"] = false;

                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "RUNE", "Se ha modificado los datos de la persona", false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);

//                        StrScript = @"$(function(){{
//                                        MoveTabIndex(0);
//                                    }});";
//                        StrScript = string.Format(StrScript);
//                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex", StrScript, true);
                        //if (ctrFecNac.Text.Length != 0)
                        if (HD_FecNac.Value.Length != 0)
                        {
                            LblEdad1.Visible = true;
                            //LblEdad2.Visible = true;

                            DateTime datFecha = new DateTime();
                            //if (!DateTime.TryParse(ctrFecNac.Text, out datFecha))
                            if (!DateTime.TryParse(HD_FecNac.Value, out datFecha))
                            {
                                //datFecha = Comun.FormatearFecha(ctrFecNac.Text);
                                datFecha = Comun.FormatearFecha(HD_FecNac.Value);
                            }
                            LblEdad2.Text = Convert.ToString(CalcularEdad(datFecha));

                            updTabDatAdic.Update();
                        }
                        //if (HFGUID.Value.Length > 0)
                        //{
                        //    BindGridDocumentos(Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]));
                        //}
                        //else
                        //{
                        BindGridDocumentos(Convert.ToInt64(ViewState["iPersonaId"]));
                        //}                        

                        #endregion
                    }
                    UpdDatosPersona.Update();
                    updDocumentos.Update();
                    if (RdoGrabCrearNewAct.Checked == false)
                    {
                        //------------------------------------------
                        //Fecha: 27/03/2019
                        //Autor: Miguel Márquez Beltrán
                        //Objetivo: Activar la pestaña de documentos
                        //------------------------------------------
                        StrScript = @"$(function(){{
                                    EnableTabIndex(1);MoveTabIndex(0);
                                }});";
                        StrScript = string.Format(StrScript);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTabIndex1_MoveTabIndex0", StrScript, true);
                        //------------------------------------------

                        StrScript = @"$(function(){{
                                    EnableTabIndex(2);MoveTabIndex(0);
                                }});";
                        StrScript = string.Format(StrScript);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTabIndex2_MoveTabIndex0", StrScript, true);

                        StrScript = @"$(function(){{
                                    EnableTabIndex(3);MoveTabIndex(0);
                                }});";
                        StrScript = string.Format(StrScript);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTabIndex3_MoveTabIndex0", StrScript, true);
                        StrScript = @"$(function(){{
                                    EnableTabIndex(4);MoveTabIndex(0);
                                }});";
                        StrScript = string.Format(StrScript);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTabIndex4_MoveTabIndex0", StrScript, true);

                        StrScript = @"$(function(){{
                                    EnableTabIndex(5);MoveTabIndex(0);
                                }});";
                        StrScript = string.Format(StrScript);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTabIndex5_MoveTabIndex0", StrScript, true);

                        StrScript = @"$(function(){{
                                    EnableTabIndex(6);MoveTabIndex(0);
                                }});";
                        StrScript = string.Format(StrScript);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTabIndex6_MoveTabIndex0", StrScript, true);

                        if (hubigeoNacimiento.Value.Length == 6)
                        {
                            string strUbiNac = string.Empty;
                            strUbiNac = @"$(function(){{
                                         RecargarUbigeoNacimiento();
                                        }});";
                            strUbiNac = string.Format(strUbiNac);
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "actualiza ubigeoNac", strUbiNac, true);
                        }
                    }
                }
                else
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "RUNE", "Error. No se pudo grabar el registro: - " + ObjPersBE.Message, false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }
                
                #endregion
            }

        }
        private string ObtenerEdad(DateTime dFechaNac, bool numerico = false)
        {
            DateTime datFechaHoy = new DateTime();
            datFechaHoy = Comun.FormatearFecha((Accesorios.Comun.ObtenerFechaActualTexto(HttpContext.Current.Session)));

            string strEdad = Comun.DiferenciaFechas(datFechaHoy, dFechaNac, "--", numerico);

            return strEdad;
        }
        void MyControl_btnImprimir()
        {
            try
            {
                ViewState.Add("AccionDocumento", "Nuevo");

                PersonaConsultaBL objPersonaBL = new PersonaConsultaBL();
                DataSet dsRune = new DataSet();

                string nombrereporte = string.Empty;
                string ruta = string.Empty;

                BE.RE_PERSONA objBE = new BE.RE_PERSONA();

                //if (HFGUID.Value.Length > 0)
                //{
                //    objBE.pers_iPersonaId = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
                //}
                //else
                //{
                objBE.pers_iPersonaId = Convert.ToInt64(ViewState["iPersonaId"]);
                //}
                
                
                dsRune = objPersonaBL.Persona_Imprimir_Rune(objBE);

                if (dsRune.Tables[1].Rows.Count > 0)
                {
                    if (dsRune.Tables[1].Rows[0]["pers_sNacionalidadId"].ToString() == Convert.ToString((Int32)Enumerador.enmNacionalidad.EXTRANJERA))
                    {
                        string StrScript = string.Empty;
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", "Para imprimir el RUNE tiene que ser PERUANO", false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);
                        return;
                    }
                    else
                    {

                    }
                }

                Session["strNombreArchivo"] = "crRegisterUnico.rdlc";
                Session["DtDatos"] = dsRune;
                Session["REGISTRO_RPT"] = Enumerador.enmRegistroReporte.RUNE;

                //-------------------------------------------------------------------------
                //Autor: Miguel Márquez Beltrán
                //Fecha: 30/09/2016
                //Objetivo: Crear una sesion para permitir actualizar el formato del RUNE
                //-------------------------------------------------------------------------
                Session["printRUNE"] = "1";
                //-------------------------------------------------------------------------
                string strUrl = "../Registro/FrmReporteRune.aspx";
                string strScript = "window.open('" + strUrl + "', 'popup_window', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=yes,width=750,height=800,left=150,top=10');";

                EjecutarScript(Page, strScript);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void EjecutarScript(Page Page, string strScript)
        {
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, typeof(System.Web.UI.Page), "OpenPopup", strScript, true);
        }

        void MyControl_btnSalir()
        {
            Response.Redirect("~/Default.aspx");
        }

        /***************************************************************************************************************************************************/
        /*******************************************Eventos de la funcionalidades de la pestaña Documentos**************************************************/
        /***************************************************************************************************************************************************/
        private void habilitarCajasDocumentos(Boolean Estado)
        {
            ddl_TipoDocumentoM.Enabled = Estado;
            txtNroDocumentoM.Enabled = Estado;
            txtLugExp.Enabled = Estado;

            txtLugRenov.Enabled = Estado;
            btn_GrabarDocumento.Enabled = Estado;
        }

        private void LimpiarCajasDocumentos()
        {
            ddl_TipoDocumentoM.SelectedIndex = -1;
            txtNroDocumentoM.Text = String.Empty;
            ctrFecVcto.Text = String.Empty;
            ctrFecExped.Text = String.Empty;
            txtLugExp.Text = String.Empty;
            ctrFecRenov.Text = String.Empty;
            txtLugRenov.Text = String.Empty;
        }

        protected void btn_GrabarDocumento_Click(object sender, EventArgs e)
        {
            string StrScript = string.Empty;

            RE_PERSONAIDENTIFICACION ObjDocBE = new RE_PERSONAIDENTIFICACION();

            Proceso MiProc = new Proceso();
            int IntRpta = 0;

            ObjDocBE.peid_sDocumentoTipoId = Convert.ToInt16(ddl_TipoDocumentoM.SelectedValue);
            ObjDocBE.peid_vDocumentoNumero = txtNroDocumentoM.Text.ToUpper();

            DateTime? FechaVcto = null;
            if (ctrFecVcto.Text.Length == 0)
            {
                FechaVcto = null;
            }
            else
            {
                if (Comun.EsFecha(ctrFecVcto.Text.Trim()) == false)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "RUNE", "La fecha de vencimiento no es válida."));
                    return;
                }

                FechaVcto = ObtenerDateTimeFormateado(ctrFecVcto.Text);
            }
            ObjDocBE.peid_dFecVcto = FechaVcto;

            DateTime? FechaExped = null;
            if (ctrFecExped.Text.Length == 0)
            {
                FechaExped = null;
            }
            else
            {
                if (Comun.EsFecha(ctrFecExped.Text.Trim()) == false)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "RUNE", "La fecha de expedición no es válida."));
                    return;
                }

                FechaExped = ObtenerDateTimeFormateado(ctrFecExped.Text);
            }
            ObjDocBE.peid_dFecExpedicion = FechaExped;

            ObjDocBE.peid_vLugarExpedicion = txtLugExp.Text.Trim().ToUpper().Replace("'", "''");

            DateTime? FechaRenov = null;
            if (ctrFecRenov.Text.Length == 0)
            {
                FechaRenov = null;
            }
            else
            {
                if (Comun.EsFecha(ctrFecRenov.Text.Trim()) == false)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "RUNE", "La fecha de renovación no es válida."));
                    return;
                }
                FechaRenov = ObtenerDateTimeFormateado(ctrFecRenov.Text);
            }
            ObjDocBE.peid_dFecRenovacion = FechaRenov;

            ObjDocBE.peid_vLugarRenovacion = txtLugRenov.Text.Trim().ToUpper().Replace("'", "''");

            ObjDocBE.peid_bActivoEnRune = chk_ActRUNE.Checked;

            if (Convert.ToBoolean(Session["iOperDoc"]))
            {
                ObjDocBE.peid_iPersonaIdentificacionId = 0;

                //if (HFGUID.Value.Length > 0)
                //{
                //    ObjDocBE.peid_iPersonaId = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
                //}
                //else
                //{
                ObjDocBE.peid_iPersonaId = Convert.ToInt64(ViewState["iPersonaId"]);
                //}

                ObjDocBE.peid_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjDocBE.peid_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);


                if (((DataTable)Session["dtDocumentos"]).Rows.Count > 0)
                {
                    Int32 totalfilas = ((DataTable)Session["dtDocumentos"]).Rows.Count;
                    for (int i = 0; i < totalfilas; i++)
                    {
                        if (ddl_TipoDocumentoM.SelectedValue == ((DataTable)Session["dtDocumentos"]).Rows[i][2].ToString())
                        {
                            chk_ActRUNE.Checked = false;
                            updDocumentos.Update();
                            StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", "Ya se Encontró un Registro con este Tipo de Documento", false, 190, 250);
                            Comun.EjecutarScript(Page, StrScript);
                            return;
                        }
                    }
                }



                Object[] miArrayPersIdentifBus = new Object[4] { Convert.ToInt32(ddl_TipoDocumentoM.SelectedValue),
                                                                 txtNroDocumentoM.Text.Trim(),
                                                                 ObjDocBE.peid_iPersonaId,
                                                                 1};

                IntRpta = (int)MiProc.Invocar(ref miArrayPersIdentifBus,
                                                "SGAC.BE.RE_PERSONAIDENTIFICACION",
                                                Enumerador.enmAccion.BUSCAR,
                                                Enumerador.enmAplicacion.WEB);

                if (IntRpta == 1)
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", "Ya existe el número de documento que esta intentando consignando.", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }

                Object[] miArrayDel = new Object[2] { ObjDocBE,                                                    
                                                      Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) };

                IntRpta = (int)MiProc.Invocar(ref miArrayDel,
                                              "SGAC.BE.RE_PERSONAIDENTIFICACION",
                                              Enumerador.enmAccion.INSERTAR,
                                              Enumerador.enmAplicacion.WEB);

            }
            else
            {
                ObjDocBE.peid_iPersonaIdentificacionId = Convert.ToInt64(Session["IntPersonaIdentId"]);

                //if (HFGUID.Value.Length > 0)
                //{
                //    ObjDocBE.peid_iPersonaId = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
                //}
                //else
                //{
                ObjDocBE.peid_iPersonaId = Convert.ToInt64(ViewState["iPersonaId"]);
                //}
                
                
                ObjDocBE.peid_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                ObjDocBE.peid_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                Object[] miArrayPersIdentifBus = new Object[4] { Convert.ToInt32(ddl_TipoDocumentoM.SelectedValue),
                                                                 txtNroDocumentoM.Text.Trim(),
                                                                 ObjDocBE.peid_iPersonaId,
                                                                 2};

                IntRpta = (int)MiProc.Invocar(ref miArrayPersIdentifBus,
                                                "SGAC.BE.RE_PERSONAIDENTIFICACION",
                                                Enumerador.enmAccion.BUSCAR,
                                                Enumerador.enmAplicacion.WEB);

                if (IntRpta == 1)
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", "Ya existe el número de documento que esta intentando consignando.", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }



                Object[] miArrayDel = new Object[2] { ObjDocBE,                                                    
                                                      Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) };

                IntRpta = (int)MiProc.Invocar(ref miArrayDel,
                                              "SGAC.BE.RE_PERSONAIDENTIFICACION",
                                              Enumerador.enmAccion.MODIFICAR,
                                              Enumerador.enmAplicacion.WEB);
            }

            if (IntRpta > 0)
            {
                //if (HFGUID.Value.Length > 0)
                //{
                //    BindGridDocumentos(Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]));
                //}
                //else
                //{
                BindGridDocumentos(Convert.ToInt64(ViewState["iPersonaId"]));
                //}

                
                if (Convert.ToInt32(ddl_TipoDocumentoM.SelectedValue) == Convert.ToInt32(Enumerador.enmTipoDocumento.DNI))
                {
                    txtNroDoc.Text = txtNroDocumentoM.Text.Trim();
                }
            }
            else
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "RUNE", "Error. No se pudo realizar la operación", false, 190, 250);
                Comun.EjecutarScript(Page, StrScript);
            }

            hidDocumento.Value = "";
            Session["iOperDoc"] = true;

            StrScript = @"$(function(){{
                            LimpiarPestañaDocumentos();
                        }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "LimpiarPestañaDocumentos", StrScript, true);

            ViewState.Add("AccionDocumento", "Nuevo");

            ddl_TipoDocumentoM.Enabled = true;
            UpdDatosPersona.Update();
            updDocumentos.Update();
        }



        private bool PoseeActuaciones(short sTipoDocumento, string vNumeroDocumento)
        {
            bool bPoseeActuaciones = false;
            if (vNumeroDocumento != string.Empty &&
                 sTipoDocumento != -1)
            {
                PersonaConsultaBL oPersonaConsultaBL = new PersonaConsultaBL();
                bPoseeActuaciones = oPersonaConsultaBL.PersonaPoseeActuaciones(sTipoDocumento, vNumeroDocumento.Trim());
            }

            return bPoseeActuaciones;
        }

        protected void btn_CancelarMD_Click(object sender, EventArgs e)
        {
            string StrScript = string.Empty;

            hidDocumento.Value = "";
            Session["iOperDoc"] = true;

            StrScript = @"$(function(){{
                            LimpiarPestañaDocumentos();
                        }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "LimpiarPestañaDocumentos", StrScript, true);

            ViewState.Add("AccionDocumento", "Nuevo");

            HF_TipoDocumento_Editando.Value = "-1";
            HF_NumeroDocumento_Editando.Value = "";
            txtNroDocumentoM.Enabled = true;
            ddl_TipoDocumentoM.Enabled = true;
            chk_ActRUNE.Checked = false;
            updDocumentos.Update();
        }

        protected void Grd_Documentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex > -1)
                {
                    String vTipoDocumento = Convert.ToString(e.Row.Cells[2].Text);
                    String iTipoDocumentoID = Convert.ToString(e.Row.Cells[1].Text);

                    System.Web.UI.WebControls.Image imagen = (System.Web.UI.WebControls.Image)e.Row.FindControl("btnEditar");
                    System.Web.UI.WebControls.Image imagen1 = (System.Web.UI.WebControls.Image)e.Row.FindControl("btnEliminar");


                    if (vTipoDocumento == "CUI")
                    {
                        imagen.ImageUrl = "../Images/img_16_edit.png";
                        imagen1.ImageUrl = "../Images/img_16_delete_disabled.png";
                        e.Row.Cells[11].Enabled = true;
                        e.Row.Cells[12].Enabled = false;
                    }
                    else
                    {
                        imagen.ImageUrl = "../Images/img_16_edit.png";
                        imagen1.ImageUrl = "../Images/img_16_delete.png";
                        e.Row.Cells[11].Enabled = true;
                        e.Row.Cells[12].Enabled = true;
                    }
                   

                    if (iTipoDocumentoID == CmbTipoDoc.SelectedValue)
                    {
                        imagen.ImageUrl = "../Images/img_16_edit.png";
                        imagen1.ImageUrl = "../Images/img_16_delete_disabled.png";
                        e.Row.Cells[11].Enabled = true;
                        e.Row.Cells[12].Enabled = false;
                    }
                    else
                    {
                        imagen.ImageUrl = "../Images/img_16_edit.png";
                        imagen1.ImageUrl = "../Images/img_16_delete.png";
                        e.Row.Cells[11].Enabled = true;
                        e.Row.Cells[12].Enabled = true;
                    }

                    e.Row.Cells[6].Attributes.Add("style", "word-break:break-all;word-wrap:break-word;");
                    e.Row.Cells[8].Attributes.Add("style", "word-break:break-all;word-wrap:break-word;");

                    if (e.Row.Cells[4].Text.Trim() != "&nbsp;")
                    {
                        e.Row.Cells[4].Text = (Comun.FormatearFecha(e.Row.Cells[4].Text)).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                    }

                    if (e.Row.Cells[5].Text.Trim() != "&nbsp;")
                    {
                        e.Row.Cells[5].Text = (Comun.FormatearFecha(e.Row.Cells[5].Text)).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                    }

                    if (e.Row.Cells[7].Text.Trim() != "&nbsp;")
                    {
                        e.Row.Cells[7].Text = (Comun.FormatearFecha(e.Row.Cells[7].Text)).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                    }

                    e.Row.Attributes.Add("onmouseover", "this.colorOriginal=this.style.backgroundColor; this.style.backgroundColor='#FFFF99'");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.colorOriginal");
                }
            }
        }

        protected void Grd_Documentos_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[10].ColumnSpan = 2;
                e.Row.Cells[11].Visible = false;
            }
        }

        protected void Grd_Documentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string StrScript = string.Empty;

            Proceso MiProc = new Proceso();
            int IntRpta = 0;

            int IntIndexGrdDocumento = Convert.ToInt32(e.CommandArgument);

            string formatoFecha = ConfigurationManager.AppSettings["FormatoFechas"];

            Session["iOperDoc"] = false;
            ViewState.Add("AccionDocumento", "Nuevo");

            int IntIndexGrdDocumentoMigratorio = Convert.ToInt16(Page.Server.HtmlDecode(Grd_Documentos.Rows[IntIndexGrdDocumento].Cells[13].Text));

            if (IntIndexGrdDocumentoMigratorio == 1)
            {
                StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "RUNE", "No se puede modificar o eliminar documento de tramite migratorio", false, 190, 250);
                Comun.EjecutarScript(Page, StrScript);

                return;
            }
            else
            {

                if (e.CommandName == "Editar")
                {
                    //---------------------------------------------------------------------------
                    //Fecha: 27/03/2019
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Deshabilitar la validación para permitir modificar.
                    //---------------------------------------------------------------------------

                    //bool bPoseeActuaciones = bPoseeActuaciones = PoseeActuaciones(Convert.ToInt16(Convert.ToString(Page.Server.HtmlDecode(Grd_Documentos.Rows[IntIndexGrdDocumento].Cells[1].Text))),
                    //    Convert.ToString(Page.Server.HtmlDecode(Grd_Documentos.Rows[IntIndexGrdDocumento].Cells[3].Text)));

                    //if (bPoseeActuaciones)
                    //{
                    //    chk_ActRUNE.Checked = false;
                    //    updDocumentos.Update();
                    //    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", "El documento no puede ser modificado porque el usuario posee actuaciones pendientes.", false, 190, 250);
                    //    Comun.EjecutarScript(Page, StrScript);
                    //    return;
                    //}


                    ViewState.Add("AccionDocumento", "Editar");

                    hidDocumento.Value = Page.Server.HtmlDecode(Grd_Documentos.Rows[IntIndexGrdDocumento].Cells[0].Text);
                    Session["IntPersonaIdentId"] = Convert.ToInt64(Page.Server.HtmlDecode(Grd_Documentos.Rows[IntIndexGrdDocumento].Cells[0].Text));
                    ddl_TipoDocumentoM.SelectedValue = Convert.ToString(Page.Server.HtmlDecode(Grd_Documentos.Rows[IntIndexGrdDocumento].Cells[1].Text));

                    ddl_TipoDocumentoM.Enabled = false;
                    if (ddl_TipoDocumentoM.SelectedValue != "1") // DNI
                    {
                        ddl_TipoDocumentoM.Enabled = true;
                        txtNroDocumentoM.Enabled = true;
                        chk_ActRUNE.Enabled = true;
                    }

                    txtNroDocumentoM.Text = Convert.ToString(Page.Server.HtmlDecode(Grd_Documentos.Rows[IntIndexGrdDocumento].Cells[3].Text));


                    //---------------------------------------------------------------------------
                    //Fecha: 27/03/2019
                    //Autor: Miguel Márquez Beltrán
                    //Objetivo: Deshabilitar la edición de los controles
                    //          Tipo de documento y número de documento del primer documento
                    //---------------------------------------------------------------------------
                    if (ddl_TipoDocumentoM.SelectedValue == CmbTipoDoc.SelectedValue)
                    {
                        ddl_TipoDocumentoM.Enabled = false;
                        txtNroDocumentoM.Enabled = false;
                        //chk_ActRUNE.Enabled = false;
                    }


                    HF_TipoDocumento_Editando.Value = ddl_TipoDocumentoM.SelectedValue;
                    HF_NumeroDocumento_Editando.Value = txtNroDocumentoM.Text;

                    ctrFecVcto.Text = Convert.ToString(Grd_Documentos.Rows[IntIndexGrdDocumento].Cells[4].Text).EncodeString();
                    ctrFecExped.Text = Convert.ToString(Grd_Documentos.Rows[IntIndexGrdDocumento].Cells[5].Text).EncodeString();

                    txtLugExp.Text = Convert.ToString(Page.Server.HtmlDecode(Grd_Documentos.Rows[IntIndexGrdDocumento].Cells[6].Text));

                    ctrFecRenov.Text = Convert.ToString(Grd_Documentos.Rows[IntIndexGrdDocumento].Cells[7].Text).EncodeString();

                    txtLugRenov.Text = Convert.ToString(Page.Server.HtmlDecode(Grd_Documentos.Rows[IntIndexGrdDocumento].Cells[8].Text));
                    chk_ActRUNE.Checked = Convert.ToBoolean(Page.Server.HtmlDecode(Grd_Documentos.Rows[IntIndexGrdDocumento].Cells[9].Text));

                    if (ctrFecVcto.Text != string.Empty)
                    {
                        ctrFecVcto.Text = ObtenerDateTimeFormateado(ctrFecVcto.Text).ToString(formatoFecha);
                    }

                    if (ctrFecExped.Text != string.Empty)
                    {
                        ctrFecExped.Text = ObtenerDateTimeFormateado(ctrFecExped.Text).ToString(formatoFecha);
                    }

                    if (ctrFecRenov.Text != string.Empty)
                    {
                        ctrFecRenov.Text = ObtenerDateTimeFormateado(ctrFecRenov.Text).ToString(formatoFecha);
                    }

                    Grd_Documentos.SelectedIndex = Convert.ToInt32(e.CommandArgument);
                }

                if (e.CommandName == "Eliminar")
                {

                    bool bPoseeActuaciones = PoseeActuaciones(Convert.ToInt16(Convert.ToString(Page.Server.HtmlDecode(Grd_Documentos.Rows[IntIndexGrdDocumento].Cells[1].Text))),
                        Convert.ToString(Page.Server.HtmlDecode(Grd_Documentos.Rows[IntIndexGrdDocumento].Cells[3].Text)));


                    if (bPoseeActuaciones && IntIndexGrdDocumento == 0)
                    {
                        chk_ActRUNE.Checked = false;
                        updDocumentos.Update();
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", "El documento no puede ser eliminado porque el usuario posee actuaciones pendientes.", false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);
                        return;
                    }


                    if (IntIndexGrdDocumento == 0)
                    {
                        DataTable _dtParticipacionesPersona;
                        PersonaConsultaBL _obj = new PersonaConsultaBL();

                        _dtParticipacionesPersona = _obj.ObtenerParticipacionesPersona(Convert.ToInt16(HF_Persona_TipoDocumento_Editando.Value),
                                HF_Persona_NumeroDocumento_Editando.Value);



                        if (_dtParticipacionesPersona.Rows[0][0].ToString() != "Sin Participación")
                        {
                            StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", _dtParticipacionesPersona.Rows[0][0].ToString(), false, 190, 250);
                            Comun.EjecutarScript(Page, StrScript);
                            return;
                        }
                    }
                    Boolean BolActRUNE = Convert.ToBoolean(Page.Server.HtmlDecode(Grd_Documentos.Rows[IntIndexGrdDocumento].Cells[9].Text));

                    if (BolActRUNE == true)
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", "Este documento esta activo en el RUNE. No se puedo realizar la operación", false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);

                        return;
                    }

                    if (CmbNacRecurr.SelectedValue == Convert.ToString((int)Enumerador.enmNacionalidad.PERUANA))
                    {
                        if (Grd_Documentos.Rows[IntIndexGrdDocumento].Cells[1].Text == Convert.ToString((int)Enumerador.enmTipoDocumento.DNI))
                        {
                            StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", "No puede eliminar el DNI de esta persona. No se puedo realizar la operación", false, 190, 250);
                            Comun.EjecutarScript(Page, StrScript);

                            return;
                        }
                    }



                    RE_PERSONAIDENTIFICACION ObjDocBE = new RE_PERSONAIDENTIFICACION();

                    ObjDocBE.peid_iPersonaIdentificacionId = Convert.ToInt64(Page.Server.HtmlDecode(Grd_Documentos.Rows[IntIndexGrdDocumento].Cells[0].Text));
                    ObjDocBE.peid_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    ObjDocBE.peid_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                    Object[] miArrayDel = new Object[2] { ObjDocBE,                                                    
                                                      Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) };

                    IntRpta = (int)MiProc.Invocar(ref miArrayDel,
                                                  "SGAC.BE.RE_PERSONAIDENTIFICACION",
                                                  Enumerador.enmAccion.ELIMINAR,
                                                  Enumerador.enmAplicacion.WEB);

                    if (IntRpta > 0)
                    {
                        Session["iOperDoc"] = true;

                        //if (HFGUID.Value.Length > 0)
                        //{
                        //    BindGridDocumentos(Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]));
                        //}
                        //else
                        //{
                        BindGridDocumentos(Convert.ToInt64(ViewState["iPersonaId"]));
                        //}                        
                    }
                    else
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "RUNE", "Error. No se pudo realizar la operación", false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);
                    }

                    Session["iOperDoc"] = true;
                    hidDocumento.Value = "";

                    HF_TipoDocumento_Editando.Value = "-1";
                    HF_NumeroDocumento_Editando.Value = "";
                }

            }

            updDocumentos.Update();
        }
        /***************************************************************************************************************************************************/

        /***************************************************************************************************************************************************/
        /*******************************************Eventos de la funcionalidades de la pestaña Direcciones*************************************************/
        /***************************************************************************************************************************************************/
        protected void CmbDptoContDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbDptoContDir.SelectedIndex > 0)
            {
                CmbProvPaisDir.Enabled = true;



                if (ViewState["Ubigeo"] != null)
                {
                    beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                    obeUbigeoListas = (beUbigeoListas)ViewState["Ubigeo"];
                    List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                    lbeUbicaciongeografica = Comun.obtenerListaUbiGeo("02", CmbDptoContDir.SelectedValue, "", obeUbigeoListas.Ubigeo02);

                    lbeUbicaciongeografica.Insert(0, new beUbicaciongeografica { Ubi02 = "00", Provincia = "-- SELECCIONE --" });

                    CmbProvPaisDir.DataSource = lbeUbicaciongeografica;
                    CmbProvPaisDir.DataValueField = "Ubi02";
                    CmbProvPaisDir.DataTextField = "Provincia";
                    CmbProvPaisDir.DataBind();
                }

                //Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, CmbDptoContDir.SelectedValue, "", "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, CmbProvPaisDir);
                //FillWebCombo(Comun.ObtenerProvincias(Session, CmbDptoContDir.SelectedValue.ToString()),
                //             CmbProvPaisDir,
                //             "ubge_vProvincia",
                //             "ubge_cUbi02");

                CmbDptoContDir.Focus();
            }
            else
            {
                this.CmbProvPaisDir.Items.Clear();
                this.CmbProvPaisDir.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));

            }

            this.CmbDistCiuDir.Items.Clear();
            this.CmbDistCiuDir.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
        }

        protected void CmbProvPaisDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbProvPaisDir.SelectedIndex > 0)
            {
                CmbDistCiuDir.Enabled = true;

                if (ViewState["Ubigeo"] != null)
                {
                    beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                    obeUbigeoListas = (beUbigeoListas)ViewState["Ubigeo"];
                    List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                    lbeUbicaciongeografica = Comun.obtenerListaUbiGeo("03", CmbDptoContDir.SelectedValue, CmbProvPaisDir.SelectedValue, obeUbigeoListas.Ubigeo03);
                    lbeUbicaciongeografica.Insert(0, new beUbicaciongeografica { Ubi03 = "00", Distrito = "-- SELECCIONE --" });
                    CmbDistCiuDir.DataSource = lbeUbicaciongeografica;
                    CmbDistCiuDir.DataValueField = "Ubi03";
                    CmbDistCiuDir.DataTextField = "Distrito";
                    CmbDistCiuDir.DataBind();
                    CmbDistCiuDir.Enabled = (CmbProvPaisDir.SelectedValue.Equals("00") ? false : true);
                    CmbDistCiuDir.Focus();
                }

                //Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.DISTRITO_CIUD, CmbDptoContDir.SelectedValue, CmbProvPaisDir.SelectedValue, "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, CmbDistCiuDir);
                //FillWebCombo(Comun.ObtenerDistritos(Session, CmbDptoContDir.SelectedValue.ToString(), CmbProvPaisDir.SelectedValue.ToString()),
                //             CmbDistCiuDir,
                //             "ubge_vDistrito",
                //             "ubge_cUbi03");

                Session["CmbProvPaisDir"] = CmbProvPaisDir.SelectedValue;

                CmbProvPaisDir.Focus();
            }
            else
            {
                this.CmbDistCiuDir.Items.Clear();
                this.CmbDistCiuDir.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
            }
        }

        protected void CmbDistCiuDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbDistCiuDir.SelectedIndex > 0)
            {
                Session["CmbDistCiuDir"] = CmbDistCiuDir.SelectedValue;
                CmbDistCiuDir.Focus();
            }
        }

        protected void btn_GrabarDir_Click(object sender, EventArgs e)
        {

            if (!ValidarDireccion())
            { return; }

            string StrScript = string.Empty;

            if (Convert.ToBoolean(Session["OperRune"]))
            {
                #region Nuevo_RUNE
                if (Convert.ToBoolean(Session["iOperDir"]))
                {
                    #region Nueva_Dirección

                    DataRow row;
                    row = ((DataTable)Session["DtRegDirecciones"]).NewRow();

                    row["iResidenciaId"] = 0;
                    row["iPersonaId"] = 0;
                    row["vResidenciaDireccion"] = TxtDirDir.Text.Trim().ToUpper();
                    row["vCodigoPostal"] = txtCodPost.Text;
                    row["sResidenciaTipoId"] = CmbTipRes.SelectedValue;
                    row["vResidenciaTipo"] = CmbTipRes.SelectedItem.Text.Trim();

                    row["cResidenciaUbigeo"] = CmbDptoContDir.SelectedValue +
                                               Session["CmbProvPaisDir"] +
                                               Session["CmbDistCiuDir"];

                    row["DptoCont"] = CmbDptoContDir.SelectedItem.Text.Trim();
                    row["ProvPais"] = CmbProvPaisDir.SelectedItem.Text.Trim();
                    row["DistCiu"] = CmbDistCiuDir.SelectedItem.Text.Trim();
                    row["vResidenciaTelefono"] = TxtTelfDir.Text.Trim();

                    ((DataTable)Session["DtRegDirecciones"]).Rows.Add(row);

                    StrScript = @"$(function(){{
                            LimpiarPestañaDirecciones();
                        }});";
                    StrScript = string.Format(StrScript);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "LimpiarPestañaFilaciones", StrScript, true);

                    Session["iOperDir"] = true;
                    
                    #endregion
                }
                else
                {
                    #region Edita_Dirección

                    ((DataTable)Session["DtRegDirecciones"]).Rows[Convert.ToInt32(Session["iIndexGrdDirecciones"])]["iResidenciaId"] = hidDirID.Value;
                    ((DataTable)Session["DtRegDirecciones"]).Rows[Convert.ToInt32(Session["iIndexGrdDirecciones"])]["iPersonaId"] = hidPersID.Value;
                    ((DataTable)Session["DtRegDirecciones"]).Rows[Convert.ToInt32(Session["iIndexGrdDirecciones"])]["vResidenciaDireccion"] = TxtDirDir.Text.Trim().ToUpper();
                    ((DataTable)Session["DtRegDirecciones"]).Rows[Convert.ToInt32(Session["iIndexGrdDirecciones"])]["vCodigoPostal"] = txtCodPost.Text.Trim();
                    ((DataTable)Session["DtRegDirecciones"]).Rows[Convert.ToInt32(Session["iIndexGrdDirecciones"])]["sResidenciaTipoId"] = CmbTipRes.SelectedValue;
                    ((DataTable)Session["DtRegDirecciones"]).Rows[Convert.ToInt32(Session["iIndexGrdDirecciones"])]["vResidenciaTipo"] = CmbTipRes.SelectedItem.Text.Trim();

                    ((DataTable)Session["DtRegDirecciones"]).Rows[Convert.ToInt32(Session["iIndexGrdDirecciones"])]["cResidenciaUbigeo"] = CmbDptoContDir.SelectedValue +
                                                                                                                                           Session["CmbProvPaisDir"] +
                                                                                                                                           Session["CmbDistCiuDir"];

                    ((DataTable)Session["DtRegDirecciones"]).Rows[Convert.ToInt32(Session["iIndexGrdDirecciones"])]["DptoCont"] = CmbDptoContDir.SelectedItem.Text.Trim();
                    ((DataTable)Session["DtRegDirecciones"]).Rows[Convert.ToInt32(Session["iIndexGrdDirecciones"])]["ProvPais"] = CmbProvPaisDir.SelectedItem.Text.Trim();
                    ((DataTable)Session["DtRegDirecciones"]).Rows[Convert.ToInt32(Session["iIndexGrdDirecciones"])]["DistCiu"] = CmbDistCiuDir.SelectedItem.Text.Trim();
                    ((DataTable)Session["DtRegDirecciones"]).Rows[Convert.ToInt32(Session["iIndexGrdDirecciones"])]["vResidenciaTelefono"] = TxtTelfDir.Text.Trim();

                    StrScript = @"$(function(){{
                            LimpiarPestañaDirecciones();
                        }});";
                    StrScript = string.Format(StrScript);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "LimpiarPestañaFilaciones", StrScript, true);
                    Session["iOperDir"] = true;

                    #endregion
                }

                ((DataTable)this.Session["DtRegDirecciones"]).AcceptChanges();

                this.GrdDirecciones.DataSource = Session["DtRegDirecciones"];
                this.GrdDirecciones.DataBind();
                #endregion
            }
            else
            {
                #region Editar_RUNE

                RE_RESIDENCIA ObjResBE = new RE_RESIDENCIA();
                RE_PERSONARESIDENCIA ObjPersResBE = new RE_PERSONARESIDENCIA();
                Proceso MiProc = new Proceso();
                int IntRpta = 0;

                if (Convert.ToBoolean(Session["iOperDir"]))
                {
                    #region Insertar Dirección

                    ObjResBE.resi_iResidenciaId = 0;
                    ObjResBE.resi_sResidenciaTipoId = Convert.ToInt16(CmbTipRes.SelectedValue);
                    ObjResBE.resi_vResidenciaDireccion = TxtDirDir.Text.Trim().ToUpper().Replace("'", "''");
                    ObjResBE.resi_vCodigoPostal = txtCodPost.Text.ToUpper();
                    ObjResBE.resi_vResidenciaTelefono = TxtTelfDir.Text;


                    if (CmbDptoContDir.SelectedValue == "")
                    {
                        CmbDptoContDir.Focus();
                        return;
                    }
                    if (CmbProvPaisDir.SelectedValue == "")
                    {
                        CmbProvPaisDir.Focus();
                        return;
                    }
                    if (CmbDistCiuDir.SelectedValue == "")
                    {
                        CmbDistCiuDir.Focus();
                        return;
                    }


                    if (CmbDptoContDir.SelectedItem.Text == "")
                    {
                        CmbDptoContDir.Focus();
                        return;
                    }
                    if (CmbProvPaisDir.SelectedItem.Text == "")
                    {
                        CmbProvPaisDir.Focus();
                        return;
                    }
                    if (CmbDistCiuDir.SelectedItem.Text == "")
                    {
                        CmbDistCiuDir.Focus();
                        return;
                    }

                    
                    if (CmbDptoContDir.SelectedValue == "00" && CmbProvPaisDir.SelectedValue == "00" && CmbDistCiuDir.SelectedValue == "00")
                    {
                        Session["CmbProvPaisDir"] = null;
                        Session["CmbDistCiuDir"] = null;
                    }
                    ObjResBE.resi_cResidenciaUbigeo = CmbDptoContDir.SelectedValue + CmbProvPaisDir.SelectedValue + CmbDistCiuDir.SelectedValue; // Session["CmbProvPaisDir"] + Session["CmbDistCiuDir"];

                    ObjResBE.resi_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    ObjResBE.resi_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                    //if (HFGUID.Value.Length > 0)
                    //{
                    //    ObjPersResBE.pere_iPersonaId = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
                    //}
                    //else
                    //{
                    ObjPersResBE.pere_iPersonaId = Convert.ToInt64(ViewState["iPersonaId"]);
                    //}
                    
                    ObjResBE.resi_iResidenciaId = 0;
                    ObjPersResBE.pere_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    ObjPersResBE.pere_vIPCreacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                    Object[] miArrayIns = new Object[3] { ObjResBE,
                                                          ObjPersResBE,
                                                          Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) };

                    IntRpta = (int)MiProc.Invocar(ref miArrayIns,
                                                  "SGAC.BE.RE_PERSONARESIDENCIA",
                                                  Enumerador.enmAccion.INSERTAR,
                                                  Enumerador.enmAplicacion.WEB);

                    StrScript = @"$(function(){{
                            LimpiarPestañaDirecciones();
                        }});";
                    StrScript = string.Format(StrScript);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "LimpiarPestañaFilaciones", StrScript, true);

                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "RUNE", Constantes.CONST_MENSAJE_EXITO, false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);

                    #endregion
                }
                else
                {
                    #region Actualizar_Residencia

                    ObjResBE.resi_iResidenciaId = Convert.ToInt64(hidDirID.Value);
                    ObjResBE.resi_sResidenciaTipoId = Convert.ToInt16(CmbTipRes.SelectedValue);
                    ObjResBE.resi_vResidenciaDireccion = TxtDirDir.Text.ToUpper();
                    ObjResBE.resi_vCodigoPostal = txtCodPost.Text.ToUpper();
                    ObjResBE.resi_vResidenciaTelefono = TxtTelfDir.Text;

                    if (CmbDptoContDir.SelectedValue == "00" && CmbProvPaisDir.SelectedValue == "00" && CmbDistCiuDir.SelectedValue == "00")
                    {
                        Session["CmbProvPaisDir"] = null;
                        Session["CmbDistCiuDir"] = null;
                    }
                    ObjResBE.resi_cResidenciaUbigeo = CmbDptoContDir.SelectedValue + Session["CmbProvPaisDir"] + Session["CmbDistCiuDir"];

                    ObjResBE.resi_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    ObjResBE.resi_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);


                    //if (HFGUID.Value.Length > 0)
                    //{
                    //    ObjPersResBE.pere_iPersonaId = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
                    //}
                    //else
                    //{
                    ObjPersResBE.pere_iPersonaId = Convert.ToInt64(ViewState["iPersonaId"]);
                    //}
                    
                    
                    
                    ObjPersResBE.pere_iResidenciaId = Convert.ToInt64(hidDirID.Value);
                    ObjPersResBE.pere_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    ObjPersResBE.pere_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                    Object[] miArrayUpd = new Object[3] { ObjResBE, 
                                                          ObjPersResBE,
                                                          Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) };

                    IntRpta = (int)MiProc.Invocar(ref miArrayUpd,
                                                  "SGAC.BE.RE_PERSONARESIDENCIA",
                                                  Enumerador.enmAccion.MODIFICAR,
                                                  Enumerador.enmAplicacion.WEB);

                    StrScript = @"$(function(){{
                            LimpiarPestañaDirecciones();
                        }});";
                    StrScript = string.Format(StrScript);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "LimpiarPestañaFilaciones", StrScript, true);

                    #endregion
                }

                if (IntRpta > 0)
                {
                    //if (HFGUID.Value.Length > 0)
                    //{
                    //    BindGridDirecciones(Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]));
                    //}
                    //else
                    //{
                    BindGridDirecciones(Convert.ToInt64(ViewState["iPersonaId"]));
                    //}                    

                }
                else
                {
                    StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "RUNE", "Error. No se pudo realizar la operación", false, 190, 250);
                    Comun.EjecutarScript(Page, StrScript);
                }
                #endregion
            }

            StrScript = @"$(function(){{
                            LimpiarPestañaDirecciones();
                        }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "LimpiarPestañaFilaciones", StrScript, true);

            //-----------------------------------------------
            //Fecha: 03/02/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Por defecto cuando sea nueva dirección
            // asignar el ubigeo del consulado.
            //-----------------------------------------------
            string strUbigeo = Session[Constantes.CONST_SESION_UBIGEO].ToString();
            string strDpto = strUbigeo.Substring(0, 2);
            string strProv = strUbigeo.Substring(2, 2);
            string strDist = strUbigeo.Substring(4, 2);

            CmbDptoContDir.SelectedValue = strDpto;
            CmbDptoContDir_SelectedIndexChanged(sender, e);
            if (strProv == "00")
            {
                CmbProvPaisDir.SelectedIndex = 0;
                CmbDistCiuDir.SelectedIndex = 0;
            }
            else {
                CmbProvPaisDir.SelectedValue = strProv;
                CmbProvPaisDir_SelectedIndexChanged(sender, e);
                CmbDistCiuDir.SelectedValue = strDist;
            }
            
            Session["CmbDistCiuDir"] = CmbDistCiuDir.SelectedValue;
            //-----------------------------------------------

            Session["iOperDir"] = true;
            UpdDirecciones.Update();

        }

        protected void GrdDirecciones_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[11].ColumnSpan = 2;
                e.Row.Cells[12].Visible = false;
            }
        }

        protected void GrdDirecciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string StrScript = string.Empty;
            Proceso MiProc = new Proceso();
            int IntRpta = 0;

            Session["iIndexGrdDirecciones"] = Convert.ToInt32(e.CommandArgument);

            hidDirID.Value = Convert.ToString(GrdDirecciones.Rows[Convert.ToInt32(Session["iIndexGrdDirecciones"])].Cells[0].Text);
            hidPersID.Value = Convert.ToString(GrdDirecciones.Rows[Convert.ToInt32(Session["iIndexGrdDirecciones"])].Cells[1].Text);


            ViewState.Add("AccionDocumento", "Nuevo");

            if (e.CommandName == "Editar")
            {
                ViewState.Add("AccionDocumento", "Editar");

                Session["iOperDir"] = false;
                txtCodPost.Text = Convert.ToString(GrdDirecciones.Rows[Convert.ToInt32(Session["iIndexGrdDirecciones"])].Cells[2].Text);

                txtCodPost.Text = Regex.Replace(txtCodPost.Text, @"<[^>]+>|&nbsp;", "").Trim();

                TxtDirDir.Text = Convert.ToString(Page.Server.HtmlDecode(GrdDirecciones.Rows[Convert.ToInt32(Session["iIndexGrdDirecciones"])].Cells[3].Text));
                TxtDirDir.Text = Regex.Replace(TxtDirDir.Text, @"<[^>]+>|&nbsp;", "").Trim().ToUpper();

                foreach (DataRow item in comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_TIPO_RESIDENCIA).Rows)
                {
                    if (item["ID"].ToString() == Convert.ToString(GrdDirecciones.Rows[Convert.ToInt32(Session["iIndexGrdDirecciones"])].Cells[4].Text))
                    {
                        CmbTipRes.SelectedValue = item["ID"].ToString();
                        break;
                    }
                    else
                    {
                        CmbTipRes.SelectedValue = "0";
                    }
                }

                //CmbTipRes.SelectedValue = Convert.ToString(GrdDirecciones.Rows[Convert.ToInt32(Session["iIndexGrdDirecciones"])].Cells[4].Text);

                Session["CodUbigeoDir"] = Convert.ToString(GrdDirecciones.Rows[Convert.ToInt32(Session["iIndexGrdDirecciones"])].Cells[6].Text);
                TxtTelfDir.Text = Convert.ToString(GrdDirecciones.Rows[Convert.ToInt32(Session["iIndexGrdDirecciones"])].Cells[10].Text);
                TxtTelfDir.Text = Regex.Replace(TxtTelfDir.Text, @"<[^>]+>|&nbsp;", "").Trim();
                /*Carga los combos de ubigeo*/
                UbigeoConsultasBL BL = new UbigeoConsultasBL();
                Session["dtUbigeoDirecciones"] = BL.ObtenerUbigeo((string)Session["CodUbigeoDir"]).Copy();

                if (((DataTable)Session["dtUbigeoDirecciones"]).Rows.Count > 0)
                {
                    this.CmbDptoContDir.Items.Clear();
                    FillWebCombo(comun_Part3.ObtenerDepartamentos(Session), CmbDptoContDir, "ubge_vDepartamento", "ubge_cUbi01");
                    CmbDptoContDir.SelectedValue = (string)((DataTable)Session["dtUbigeoDirecciones"]).Rows[0][1];

                    this.CmbProvPaisDir.Items.Clear();

                    FillWebCombo(comun_Part3.ObtenerProvincias(Session, CmbDptoContDir.SelectedValue.ToString()),
                             CmbProvPaisDir,
                             "ubge_vProvincia",
                             "ubge_cUbi02");

                    CmbProvPaisDir.SelectedValue = (string)((DataTable)Session["dtUbigeoDirecciones"]).Rows[0][2];
                    Session["CmbProvPaisDir"] = (string)((DataTable)Session["dtUbigeoDirecciones"]).Rows[0][2];

                    this.CmbDistCiuDir.Items.Clear();

                    FillWebCombo(comun_Part3.ObtenerDistritos(Session, CmbDptoContDir.SelectedValue.ToString(), CmbProvPaisDir.SelectedValue.ToString()),
                        CmbDistCiuDir,
                        "ubge_vDistrito",
                        "ubge_cUbi03");


                    CmbDistCiuDir.SelectedValue = (string)((DataTable)Session["dtUbigeoDirecciones"]).Rows[0][3];
                    Session["CmbDistCiuDir"] = (string)((DataTable)Session["dtUbigeoDirecciones"]).Rows[0][3];

                    CmbDistCiuDir.Enabled = true;
                    CmbProvPaisDir.Enabled = true;

                }

                StrScript = @"$(function(){{
                                EditarDirecciones('{0}');
                            }});";

                StrScript = string.Format(StrScript,
                                         ((string)Session["CodUbigeoDir"]).EncodeString());
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "EditarDirecciones", StrScript, true);

                UpdDirecciones.Update();
            }

            if (e.CommandName == "Eliminar")
            {
                if (Convert.ToBoolean(Session["OperRune"]))
                {
                    ((DataTable)Session["DtRegDirecciones"]).Rows.RemoveAt(Convert.ToInt32(Session["iIndexGrdDirecciones"]));
                    ((DataTable)this.Session["DtRegDirecciones"]).AcceptChanges();

                    this.GrdDirecciones.DataSource = Session["DtRegDirecciones"];
                    this.GrdDirecciones.DataBind();

                    //Héctor Vásquez
                    //Le asigno un número mayor a cero porque se está borrando satistactoriamente 
                    IntRpta = 1;
                    UpdDirecciones.Update();
                }
                else
                {
                    RE_RESIDENCIA ObjResBE = new RE_RESIDENCIA();
                    RE_PERSONARESIDENCIA ObjPersResBE = new RE_PERSONARESIDENCIA();

                    ObjResBE.resi_iResidenciaId = Convert.ToInt32(hidDirID.Value);
                    ObjResBE.resi_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    ObjResBE.resi_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);


                    //if (HFGUID.Value.Length > 0)
                    //{
                    //    ObjPersResBE.pere_iPersonaId = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
                    //}
                    //else
                    //{
                    ObjPersResBE.pere_iPersonaId = Convert.ToInt64(ViewState["iPersonaId"]);
                    //}
                    
                    
                    ObjPersResBE.pere_iResidenciaId = Convert.ToInt32(hidDirID.Value);
                    ObjPersResBE.pere_sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                    ObjPersResBE.pere_vIPModificacion = Convert.ToString(Session[Constantes.CONST_SESION_DIRECCION_IP]);

                    Object[] miArrayDel = new Object[3] { ObjResBE,
                                                          ObjPersResBE,
                                                          Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]) };

                    IntRpta = (int)MiProc.Invocar(ref miArrayDel,
                                                  "SGAC.BE.RE_PERSONARESIDENCIA",
                                                  Enumerador.enmAccion.ELIMINAR,
                                                  Enumerador.enmAplicacion.WEB);

                    if (IntRpta > 0)
                    {
                        //if (HFGUID.Value.Length > 0)
                        //{
                        //    BindGridDirecciones(Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]));
                        //}
                        //else
                        //{
                        BindGridDirecciones(Convert.ToInt64(ViewState["iPersonaId"]));
                        //}                        
                    }
                    else
                    {
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "RUNE", "Error. No se guardaron los cambios", false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);
                    }
                }



                StrScript = @"$(function(){{
                            LimpiarPestañaDirecciones();
                        }});";
                StrScript = string.Format(StrScript);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "LimpiarPestañaFilaciones", StrScript, true);

                Session["iOperDir"] = true;
            }
        }

        /***************************************************************************************************************************************************/
        /*******************************************Eventos de la funcionalidades de la pestaña Filiaciones*************************************************/
        /***************************************************************************************************************************************************/

        protected void Grd_Filiaciones_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[9].ColumnSpan = 2;
                e.Row.Cells[10].Visible = false;
            }
        }

        /***************************************************************************************************************************************************/
        #endregion

        #region Metodos

        private void LimpiarCamposPersona()
        {
            txtNroDoc.Text = string.Empty;
            txtRazSoc.Text = string.Empty;
            txtApePat.Text = string.Empty;
            txtApeMat.Text = string.Empty;
            txtNombres.Text = string.Empty;
            txtApepCas.Text = string.Empty;

            CmbTipoDoc.SelectedValue = ((int)Enumerador.enmTipoDocumento.DNI).ToString();
            CmbNacRecurr.SelectedValue = ((int)Enumerador.enmNacionalidad.PERUANA).ToString();

            UpdDatosPersona.Update();
        }

        private void ValidarApellidoCasadaMostrar()
        {
            if (CmbGenero.SelectedValue.ToString() == ((int)Enumerador.enmGenero.FEMENINO).ToString() &&
               (CmbEstCiv.SelectedValue.ToString() == ((int)Enumerador.enmEstadoCivil.CASADO).ToString() ||
               CmbEstCiv.SelectedValue.ToString() == ((int)Enumerador.enmEstadoCivil.VIUDO).ToString()))
            {

                txtApepCas.Enabled = true;
                txtApepCas.Text = HF_Persona_ApellidoCasada_Editando.Value;
            }
            else
            {
                txtApepCas.Text = string.Empty;
                txtApepCas.Enabled = false;
            }

            UpdDatosPersona.Update();
            updTabDatAdic.Update();
        }

        private void BindGridDirecciones(long LonPersonaId)
        {
            DataTable DtDirecciones = new DataTable();
            Proceso MiProc = new Proceso();

            Object[] miArrayObt = new Object[1] { LonPersonaId };

            DtDirecciones = (DataTable)MiProc.Invocar(ref miArrayObt,
                                                      "SGAC.BE.RE_PERSONARESIDENCIA",
                                                      Enumerador.enmAccion.OBTENER,
                                                      Enumerador.enmAplicacion.WEB);
            //---------------------------------------------
            // Autor: Miguel Márquez Beltrán
            // Fecha: 06/07/2017
            // Objetivo: Asignar las direcciones a la sesión
            //---------------------------------------------
            Session["DtRegDirecciones"] = DtDirecciones;
            //---------------------------------------------
            this.GrdDirecciones.DataSource = DtDirecciones;
            this.GrdDirecciones.DataBind();
        }

        private void CargaCamposRune()
        {
            if (ViewState["DtPersonaAct"] != null)
            {
                string StrScript = string.Empty;

                // Wilder
                Session["OperRune"] = false;


                string strTipoBusqueda = string.Empty;
                if (Session["strBusqueda"] != null)
                    strTipoBusqueda = Session["strBusqueda"].ToString();
                if (strTipoBusqueda != "")
                {
                    if (strTipoBusqueda == "AC")
                    {
                        lkbTramite.Visible = true;
                        CmbTipoDoc.Enabled = false;
                        txtDescOtroDocumento.Enabled = false;
                        txtNroDoc.Enabled = false;

                        Button BtnNuevo = (Button)ctrlToolBarButton.FindControl("btnNuevo");
                        BtnNuevo.Visible = false;

                        Button BtnBuscar = (Button)ctrlToolBarButton.FindControl("btnBuscar");
                        BtnBuscar.Visible = false;

                        Button BtnEliminar = (Button)ctrlToolBarButton.FindControl("btnEliminar");
                        BtnEliminar.Visible = false;

                        Button BtnImprimir = (Button)ctrlToolBarButton.FindControl("btnImprimir");
                        //BtnImprimir.Visible = false;
                        BtnImprimir.Enabled = true;
                    }
                    else
                    {

                        Button BtnEliminar = (Button)ctrlToolBarButton.FindControl("btnEliminar");
                        BtnEliminar.Enabled = true;

                        Button BtnImprimir = (Button)ctrlToolBarButton.FindControl("btnImprimir");
                        BtnImprimir.Enabled = true;

                        CmbTipoDoc.Enabled = false;
                        txtDescOtroDocumento.Enabled = false;
                        txtNroDoc.Enabled = false;
                    }

                }
                // JCaycho
                if (Session["strBusqueda"] == null)
                {
                    //if (HFGUID.Value.Length > 0)
                    //{
                    //    if (Session["iPersonaId" + HFGUID.Value] != null)
                    //        Session.Remove("iPersonaId" + HFGUID.Value);
                    //}
                    //else
                    //{
                    //if (ViewState["iPersonaId"] != null)
                    //        Session.Remove("iPersonaId");                 
                    //}

                    Session["OperRune"] = true;
                    return;
                }

                //if (HFGUID.Value.Length > 0)
                //{
                //    Session["iPersonaId" + HFGUID.Value] = Convert.ToInt64(((DataTable)Session["DtPersonaAct"]).Rows[0]["iPersonaId"]);
                //}
                //else
                //{
                ViewState["iPersonaId"] = Convert.ToInt64(((DataTable)ViewState["DtPersonaAct"]).Rows[0]["iPersonaId"]);
                //}

                CmbNacRecurr.SelectedValue = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["sNacionalidadId"].ToString();
                CmbTipPers.SelectedValue = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["sPersonaTipoId"].ToString();
                CmbTipoDoc.SelectedValue = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["sDocumentoTipoId"].ToString();

                //txtNroDocumentoM.Text = ((DataTable)Session["DtPersonaAct"]).Rows[0]["vNroDocumento"].ToString();

                if (((DataTable)ViewState["DtPersonaAct"]).Rows[0]["sDocumentoTipoId"].ToString() == "6")
                {
                    //Oculta y muestra
                    //lbl_CUI.Visible = true;
                    ListItem lListItem = new ListItem(Convert.ToString(Enumerador.enmTipoDocumento.CUI), Convert.ToString((int)Enumerador.enmTipoDocumento.CUI));
                    CmbTipoDoc.Items.Add(lListItem);
                    CmbTipoDoc.SelectedValue = "6";
                    CmbTipoDoc.Enabled = false;
                }


                txtNroDoc.Text = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["vNroDocumento"].ToString();
                txtValidacionReniec.Text = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["bValidacionReniec"].ToString();
                lblNacionalidadVigenteCabecera.Text = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["pena_vNacionalidad"].ToString();

                //--------------------------------------------------------------------
                //Fecha:20/03/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Asignar la especificación de Tipo de documentos otros
                //--------------------------------------------------------------------
                txtDescOtroDocumento.Text = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["vTipoDocumento"].ToString();
                //--------------------------------------------------------------------
                if (CmbTipoDoc.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.OTROS)||
                    CmbTipoDoc.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.PASAPORTE_E))
                {
                    LblOtroDocumento.Visible = true;
                    txtDescOtroDocumento.Visible = true;
                    txtDescOtroDocumento.Enabled = true;
                    lblCO_OtroDoc.Visible = true;
                    if (CmbTipoDoc.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.OTROS))
                    {
                        LblOtroDocumento.Text = "Otro documento :";
                    }
                    else
                    {
                        LblOtroDocumento.Text = "Tipo Pasaporte :";                        
                    }
                }
                else
                {
                    LblOtroDocumento.Visible = false;
                    txtDescOtroDocumento.Visible = false;
                    lblCO_OtroDoc.Visible = false;
                }

                txtApePat.Text = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["vApellidoPaterno"].ToString();
                txtApeMat.Text = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["vApellidoMaterno"].ToString();
                txtApepCas.Text = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["vApellidoCasada"].ToString();
                txtNombres.Text = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["vNombres"].ToString();
                
                HF_Persona_TipoDocumento_Editando.Value = CmbTipoDoc.SelectedValue;
                HF_Persona_NumeroDocumento_Editando.Value = txtNroDoc.Text;
                HF_Persona_Nombre_Editando.Value = txtNombres.Text;
                HF_Persona_ApellidoMaterno_Editando.Value = txtApeMat.Text;
                HF_Persona_ApellidoPaterno_Editando.Value = txtApePat.Text;
                HF_Persona_ApellidoCasada_Editando.Value = txtApepCas.Text;

                CmbEstCiv.SelectedValue = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["sEstadoCivilId"].ToString();
                CmbGenero.SelectedValue = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["sGeneroId"].ToString();
                CmbOcupacion.SelectedValue = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["sOcupacionId"].ToString();
                ddl_Profesion.SelectedValue = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["sProfesionId"].ToString();
                CmbGradInst.SelectedValue = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["sGradoInstruccionId"].ToString();


                ValidarApellidoCasadaMostrar();
                string fecha = string.Empty;
                if (((DataTable)ViewState["DtPersonaAct"]).Rows[0]["dNacimientoFecha"].ToString() != "")
                {
                    ctrFecNac.Text = (Comun.FormatearFecha(((DataTable)ViewState["DtPersonaAct"]).Rows[0]["dNacimientoFecha"].ToString()).ToString(ConfigurationManager.AppSettings["FormatoFechas"]));

                    LblEdad1.Visible = true;
                    //LblEdad2.Visible = true;

                    DateTime datFecha = new DateTime();
                    if (!DateTime.TryParse(ctrFecNac.Text, out datFecha))
                    {
                        datFecha = Comun.FormatearFecha(ctrFecNac.Text);
                    }
                    LblEdad2.Text = Convert.ToString(CalcularEdad(datFecha));
                }
                else
                {
                    ctrFecNac.Text = "";
                    LblEdad2.Text = "";
                }
                LblDescGentilicio.Text = "";
                /*Carga los combos de ubigeo*/
                if (((DataTable)ViewState["DtPersonaAct"]).Rows[0]["cNacimientoLugar"].ToString().Length > 0)
                {
                    UbigeoConsultasBL BL = new UbigeoConsultasBL();
                    Session["dtUbigeo"] = BL.ObtenerUbigeo(((DataTable)ViewState["DtPersonaAct"]).Rows[0]["cNacimientoLugar"].ToString()).Copy();

                    if (((DataTable)Session["dtUbigeo"]).Rows.Count > 0)
                    {
                        CmbDptoContNac.SelectedValue = (string)((DataTable)Session["dtUbigeo"]).Rows[0][1];
                        //CmbProvPaisNac.SelectedValue = (string)((DataTable)Session["dtUbigeo"]).Rows[0][2];
                        //CmbDistCiudadNac.SelectedValue = (string)((DataTable)Session["dtUbigeo"]).Rows[0][3];

                        this.CmbProvPaisNac.Items.Clear();
                        FillWebCombo(comun_Part3.ObtenerProvincias(Session, CmbDptoContNac.SelectedValue.ToString()), CmbProvPaisNac, "ubge_vProvincia", "ubge_cUbi02");
                        
                        //this.CmbProvPaisNac.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
                        //this.CmbProvPaisNac.Items.Insert(1, new ListItem((string)((DataTable)Session["dtUbigeo"]).Rows[0][5], (string)((DataTable)Session["dtUbigeo"]).Rows[0][2]));
                        CmbProvPaisNac.SelectedValue = (string)((DataTable)Session["dtUbigeo"]).Rows[0][2];
                        Session["CmbProvPaisNac"] = (string)((DataTable)Session["dtUbigeo"]).Rows[0][2];

                        this.CmbDistCiudadNac.Items.Clear();
                        FillWebCombo(comun_Part3.ObtenerDistritos(Session, CmbDptoContNac.SelectedValue.ToString(), CmbProvPaisNac.SelectedValue.ToString()), CmbDistCiudadNac, "ubge_vDistrito", "ubge_cUbi03");

                        CmbDistCiudadNac.SelectedValue = (string)((DataTable)Session["dtUbigeo"]).Rows[0][3];
                        Session["CmbDistCiudadNac"] = (string)((DataTable)Session["dtUbigeo"]).Rows[0][3];

                        hubigeoNacimiento.Value = CmbDptoContNac.SelectedValue + CmbProvPaisNac.SelectedValue + CmbDistCiudadNac.SelectedValue;
                        string strDptoContNac = CmbDptoContNac.SelectedValue.ToString();
                        string strProvPaisNac = CmbProvPaisNac.SelectedValue.ToString();
                        string strGenero = CmbGenero.SelectedValue.ToString();

                        //string strGentilicio = Comun.AsignarGentilicio(Session, strDptoContNac, strProvPaisNac, strGenero);
                        //jonatan silva - se obtiene el gentilicio sin ir nuevamente a la base de datos 12/01/2018
                        string strGentilicio = Comun.AsignarGentilicioPorCodigoPais(Session, strDptoContNac, strProvPaisNac, strGenero).ToString();

                        this.LblDescGentilicio.Text = strGentilicio;
                    }
                }
                VerificarPadresPeruanos();
                chkPeruanoMadrePadrePeruno.Checked = Convert.ToBoolean(((DataTable)ViewState["DtPersonaAct"]).Rows[0]["pers_bPadresPeruanos"]);
                TxtEmail.Text = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["vCorreoElectronico"].ToString();

                /*Carga Grilla de Documentos*/

                //if (HFGUID.Value.Length > 0)
                //{
                //    BindGridDocumentos(Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]));
                //}
                //else
                //{
                BindGridDocumentos(Convert.ToInt64(ViewState["iPersonaId"]));
                //}
                
                /*Carga el Registro Temporal de Direcciones*/
                PersonaResidenciaConsultaBL PersonaResidenciaConsultaBL = new PersonaResidenciaConsultaBL();
                Session.Remove("DtRegDirecciones");

                //if (HFGUID.Value.Length > 0)
                //{
                //    Session["DtRegDirecciones"] = PersonaResidenciaConsultaBL.Obtener(Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]));
                //}
                //else
                //{
                Session["DtRegDirecciones"] = PersonaResidenciaConsultaBL.Obtener(Convert.ToInt64(ViewState["iPersonaId"]));
                //}
                /*Carga la grilla de Direcciones*/
                GrdDirecciones.DataSource = Session["DtRegDirecciones"];
                GrdDirecciones.DataBind();

                //--------------------------
                Session.Remove("GrdFiliacion_Persona");
                Session.Remove("GrdFiliacion");
                Session.Remove("GrdFiliacion_Otros");


                //if (HFGUID.Value.Length > 0)
                //{
                //    FiliacionX(Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]));
                //}
                //else
                //{
                FiliacionX(Convert.ToInt64(ViewState["iPersonaId"]));
                //}

                Session["RegistroUnicoId"] = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["iRegistroUnicoId"].ToString();

                CmbRelCto.Text = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["sEmergenciaRelacionId"].ToString();
                TxtNomCompCont.Text = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["vEmergenciaNombre"].ToString();
                TxtDirExtrCont.Text = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["vEmergenciaDireccionLocal"].ToString();
                TxtCodPostCont.Text = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["vEmergenciaCodigoPostal"].ToString();
                TxtTelfCont.Text = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["vEmergenciaTelefono"].ToString();
                TxtDirPerCont.Text = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["vEmergenciaDireccionPeru"].ToString();
                TxtMailCont.Text = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["vEmergenciaCorreoElectronico"].ToString();

                if (((DataTable)ViewState["DtPersonaAct"]).Rows[0]["dVivExtDesde"].ToString().Length != 0)
                {
                    txtAñoVivDesde.Text = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["dVivExtDesde"].ToString().Substring(0, 4);
                    ddl_MesVivDesde.SelectedValue = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["dVivExtDesde"].ToString().Substring(4, 2);
                }

                var existe = (DataTable)ViewState["DtPersonaAct"];

                bool retorno = false;
                if (existe.Rows.Count > 0)
                {
                    bool.TryParse(Convert.ToString(existe.Rows[0]["bPiensaRetornarAlPeru"]), out retorno);
                    RdioSiRetornExt.Checked = retorno;
                    RdioNoRetornExt.Checked = !retorno;
                    
                    bool.TryParse(Convert.ToString(existe.Rows[0]["bAfiliadoSeguroSocial"]), out retorno);
                    RdioSiAfilSegSoc.Checked = retorno;
                    RdioNoAfilSegSoc.Checked = !retorno;

                    bool.TryParse(Convert.ToString(existe.Rows[0]["bAfiliadoAFP"]), out retorno);
                    RdioSiAfilAFP.Checked = retorno;
                    RdioNoAfilAFP.Checked = !retorno;

                    bool.TryParse(Convert.ToString(existe.Rows[0]["bAportaSeguroSocial"]), out retorno);
                    RdioSiAportSegSoc.Checked = retorno;
                    RdioNoAportSegSoc.Checked = !retorno;

                    bool.TryParse(Convert.ToString(existe.Rows[0]["bBeneficiadoExterior"]), out retorno);
                    RdioSiBenExter.Checked = retorno;
                    RdioNoBenExter.Checked = !retorno;
                }
                else
                {
                    RdioSiRetornExt.Checked = false;
                }

                if (((DataTable)ViewState["DtPersonaAct"]).Rows[0]["dRetorExt"].ToString().Length != 0)
                {
                    txtAñoRegreso.Text = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["dRetorExt"].ToString().Substring(0, 4);
                    ddl_MesRegreso.SelectedValue = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["dRetorExt"].ToString().Substring(4, 2);
                }

                CbmOcupPeru.SelectedValue = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["sOcupacionPeru"].ToString();
                CbmOcupExterior.SelectedValue = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["sOcupacionExtranjero"].ToString();
                TxtAcuerConv.Text = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["vNombreConvenio"].ToString();

                TxtObsRune.Text = ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["vObservaciones"].ToString();

                Boolean pers_bFallecidoFlag;

                if (((DataTable)ViewState["DtPersonaAct"]).Rows[0]["pers_bFallecidoFlag"].ToString() == null || ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["pers_bFallecidoFlag"].ToString().Trim() == "")
                {
                    pers_bFallecidoFlag = false;
                }
                else
                {
                    pers_bFallecidoFlag = Convert.ToBoolean(((DataTable)ViewState["DtPersonaAct"]).Rows[0]["pers_bFallecidoFlag"].ToString());
                }

                if (pers_bFallecidoFlag)
                {
                    RdoViveSi.Checked = false;
                    RdoViveNo.Checked = true;
                    RdoViveNo.Enabled = true;
                    lblFecFallece.Visible = true;
                    lblFechaFallecimiento.Visible = true;
                }
                else
                {
                    RdoViveSi.Checked = true;
                    RdoViveNo.Checked = false;
                    RdoViveNo.Enabled = false;
                    lblFecFallece.Visible = false;
                    lblFechaFallecimiento.Visible = false;
                }

                StrScript = @"$(function(){{
                                    EnableTabIndex(1);
                                }});";
                StrScript = string.Format(StrScript);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTabIndex1", StrScript, true);

                StrScript = @"$(function(){{
                                    EnableTabIndex(2);
                                }});";
                StrScript = string.Format(StrScript);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTabIndex2", StrScript, true);

                StrScript = @"$(function(){{
                                    EnableTabIndex(3);
                                }});";
                StrScript = string.Format(StrScript);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTabIndex3", StrScript, true);


                StrScript = @"$(function(){{
                                    EnableTabIndex(4);
                                }});";
                StrScript = string.Format(StrScript);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTabIndex4", StrScript, true);

                StrScript = @"$(function(){{
                                    EnableTabIndex(5);
                                }});";
                StrScript = string.Format(StrScript);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTabIndex5", StrScript, true);

                StrScript = @"$(function(){{
                                    EnableTabIndex(6);
                                }});";
                StrScript = string.Format(StrScript);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "EnableTabIndex6", StrScript, true);
                /*Carga las imagenes (Foto y Firma del Connacional)*/
                DataTable tbRegistro = new DataTable();
                PersonaFotoConsultaBL PersonaFotoConsultaBL = new PersonaFotoConsultaBL();

                //if (HFGUID.Value.Length > 0)
                //{
                //    tbRegistro = PersonaFotoConsultaBL.PersonaFotoGetFotoFirma(Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]), 1);
                //}
                //else
                //{
                tbRegistro = PersonaFotoConsultaBL.PersonaFotoGetFotoFirma(Convert.ToInt64(ViewState["iPersonaId"]), 1);
                //}

                if (tbRegistro.Rows.Count == 0)
                {
                    this.imgPersona.ImageUrl = "~/Images/People.png";
                }

                if (tbRegistro.Rows.Count > 0)
                {
                    Session["IdFotoPersona"] = Convert.ToInt64(tbRegistro.Rows[0][1]);
                    Context.Session["Registro"] = tbRegistro;
                    this.imgPersona.ImageUrl = "~/ImageHandler.ashx";
                }

                //Proceso MiProc = new Proceso();
                int IntRpta = 0;


                Int64 intiPersonaID = 0;
                //if (HFGUID.Value.Length > 0)
                //{
                //    intiPersonaID = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
                //}
                //else
                //{
                intiPersonaID = Convert.ToInt64(ViewState["iPersonaId"]);
                //}

                //Object[] miArrayPersIdentifBus = new Object[1] { intiPersonaID };


                PersonaConsultaBL obj = new PersonaConsultaBL();
                IntRpta = obj.Tiene58A(intiPersonaID);
                //IntRpta = (int)MiProc.Invocar(ref miArrayPersIdentifBus,
                //                              "SGAC.BE.RE_PERSONA",
                //                              Enumerador.enmAccion.OBTENERRPTAVALOR,
                //                              Enumerador.enmAplicacion.WEB);
                chk_PJ.Checked = false;
                if (IntRpta == 1)
                {
                    chk_PJ.Checked = true;
                }

                CmbProvPaisNac.Enabled = true;
                CmbDistCiudadNac.Enabled = true;
                Session.Remove("DtPersonaAct");



                if (RdioSiRetornExt.Checked == true)
                {
                    txtAñoRegreso.Enabled = true;
                    ddl_MesRegreso.Enabled = true;
                }
                else
                {

                    txtAñoRegreso.Enabled = false;
                    ddl_MesRegreso.Enabled = false;
                }


                if (RdioSiBenExter.Checked == true)
                {
                    TxtAcuerConv.Enabled = true;
                }
                else
                {
                    TxtAcuerConv.Enabled = false;
                }
            }
        }

        private void FillWebCombo(DataTable pDataTable,
                                  DropDownList pWebCombo,
                                  String str_pDescripcion,
                                  String str_pValor)
        {
            pWebCombo.DataSource = pDataTable;
            pWebCombo.DataTextField = str_pDescripcion;
            pWebCombo.DataValueField = str_pValor;
            pWebCombo.DataBind();
            pWebCombo.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
        }

        private void BindGridDocumentos(long LonPersonaId)
        {
            DataTable DtDocumentos = new DataTable();
            //Proceso MiProc = new Proceso();

            //Object[] miArray = new Object[1] { LonPersonaId };

            PersonaIdentificacionConsultaBL Obj = new PersonaIdentificacionConsultaBL();
            DtDocumentos = Obj.Consultar(LonPersonaId);

            //DtDocumentos = (DataTable)MiProc.Invocar(ref miArray,
            //                                         "SGAC.BE.RE_PERSONAIDENTIFICACION",
            //                                         Enumerador.enmAccion.CONSULTAR,
            //                                         Enumerador.enmAplicacion.WEB);

            Session["dtDocumentos"] = DtDocumentos;
            Grd_Documentos.DataSource = DtDocumentos;
            Grd_Documentos.DataBind();
        }

        // Obtiene la imagen convertida en byte
        byte[] image;
        private void capturaconvertimagenes()
        {
            string nombrearchivo = string.Empty;

            if ((string)(Session["CapturedImageDevuelta"]) != null)
            {
                image = null;

                nombrearchivo = (string)(Session["CapturedImageDevuelta"].ToString());
                string saveLocation = Server.MapPath(@"\Registro\" + nombrearchivo);
                this.imgPersona.ImageUrl = saveLocation;

                image = Imagen_A_Bytes(saveLocation);

                if (image.Length == 0)
                {
                    return;
                }

                if (image.Length > 0)
                {

                }
            }

            if ((string)(Session["CapturedImageDevuelta"]) == null)
            {
                imgPersona.ImageUrl = "~/Images/People.png";
                return;
            }
        }

        //Usandose para convertir imagen a byte con ayuda de OpenFile
        public byte[] Imagen_A_Bytes(string ruta)
        {
            FileStream foto = new FileStream(ruta, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            byte[] arreglo = new byte[foto.Length];
            BinaryReader reader = new BinaryReader(foto);
            arreglo = reader.ReadBytes(Convert.ToInt32(foto.Length));
            return arreglo;
        }

        private void ScriptCliente()
        {
            if (!ClientScript.IsClientScriptBlockRegistered("__ScriptCliente__"))
            {
                StringBuilder sScript = new StringBuilder();

                sScript.AppendLine("");

                sScript.AppendLine("function SoloNumeros(e){");
                sScript.AppendLine("    var key;");
                sScript.AppendLine("    if(window.event){");
                sScript.AppendLine("        key = e.keyCode;");
                sScript.AppendLine("   }else if(e.which){");
                sScript.AppendLine("       key = e.which;");
                sScript.AppendLine("   }");
                sScript.AppendLine("   if (key < 48 || key > 57){");
                sScript.AppendLine("    return false;");
                sScript.AppendLine("   }");
                sScript.AppendLine("    return true;");
                sScript.AppendLine("}");

                ClientScript.RegisterStartupScript(Page.GetType(), "__ScriptCliente__", sScript.ToString(), true);
            }
        }

        //-----------------------------------------------------------
        // Autor: Miguel Márquez Beltrán
        // Fecha de Cambio: 13/09/2016
        // Objetivo: Mostrar Mese(s) y día(s) cuando el año es cero
        //-----------------------------------------------------------

        public string CalcularEdad(DateTime birthDate)
        {
            DateTime now = DateTime.Today;

            string strEdad = "";
            lblFechaFallecimiento.Text = "";
            if (ViewState["DtPersonaAct"] != null)
            {
                if (Convert.ToBoolean(((DataTable)ViewState["DtPersonaAct"]).Rows[0]["pers_bFallecidoFlag"].ToString()) == true &&
                ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["pers_dFechaDefuncion"].ToString() != null && ((DataTable)ViewState["DtPersonaAct"]).Rows[0]["pers_dFechaDefuncion"].ToString() != string.Empty)
                {
                    DateTime fecha_defuncion = Convert.ToDateTime(((DataTable)ViewState["DtPersonaAct"]).Rows[0]["pers_dFechaDefuncion"]);
                    lblFechaFallecimiento.Text = fecha_defuncion.ToShortDateString();
                    return Comun.DiferenciaFechas(fecha_defuncion, birthDate, "Fecha Invalida");
                }
            }

            strEdad = Comun.DiferenciaFechas(now, birthDate, "Fecha Invalida");

            return strEdad;
        }

        public DateTime ObtenerDateTimeFormateado(string strFecha)
        {
            DateTime datFechaValija = new DateTime();
            if (!DateTime.TryParse(strFecha, out datFechaValija))
            {
                return Comun.FormatearFecha(strFecha);
            }
            else
            {
                return Comun.FormatearFecha(strFecha);
            }
        }

        #endregion

        protected void CmbTipoDoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtNroDoc.Text = HF_ModoEdicion.Value == "new" ? "" : txtNroDoc.Text;
            VerificarPadresPeruanos();
            btnValidarDni.Visible = false;
            chkValidarConReniec.Visible = false;
            txtApePat.Enabled = true;
            txtApeMat.Enabled = true;
            txtNombres.Enabled = true;
            txtValidacionReniec.Text = "0";
            if (CmbTipoDoc.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.OTROS)||
                CmbTipoDoc.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.PASAPORTE_E))
            {
                LblOtroDocumento.Visible = true;
                txtDescOtroDocumento.Visible = true;
                lblCO_OtroDoc.Visible = true;
                txtDescOtroDocumento.Focus();

                if (CmbTipoDoc.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.OTROS))
                {
                    LblOtroDocumento.Text = "Otro documento :";
                }
                else {
                    CmbNacRecurr.SelectedValue = CmbNacRecurr.Items.FindByText("EXTRANJERA").Value;
                    LblOtroDocumento.Text = "Tipo Pasaporte :";
                }
            }
            else if (CmbTipoDoc.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.DNI) && HF_ModoEdicion.Value=="new")
            {
                btnValidarDni.Visible = true;
                chkValidarConReniec.Visible = true;
                txtApePat.Enabled = false;
                txtApeMat.Enabled = false;
                txtNombres.Enabled = false;
            }
            else
            {
                LblOtroDocumento.Visible = false;
                txtDescOtroDocumento.Visible = false;
                lblCO_OtroDoc.Visible = false;
                txtNroDoc.Focus();
            }

            UpdDatosPersona.Update();
        }
        protected void TipoDocumentoM_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Write("\nsi__" + ddl_TipoDocumentoM.SelectedValue);
            if (ddl_TipoDocumentoM.SelectedValue == Convert.ToString((int)Enumerador.enmTipoDocumento.DNI))
            {
                btnValidarDNI2.Visible = true;
                chkValidarConReniec.Visible = true;
            }
            else
            {
                btnValidarDNI2.Visible = false;
                chkValidarConReniec.Visible = false;
            }
        }

        protected void CmbEstCiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            CmbEstCiv.Focus();
            ValidarApellidoCasadaMostrar();
            updTabDatAdic.Update();
        }

        protected void CmbOcupacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            CmbOcupacion.Focus();
            updTabDatAdic.Update();
        }

        protected void ddl_Profesion_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_Profesion.Focus();
            ActualizarGenero();
            updTabDatAdic.Update();
        }

        protected void CmbGradInst_SelectedIndexChanged(object sender, EventArgs e)
        {
            CmbGradInst.Focus();
            updTabDatAdic.Update();
        }

        protected void Grd_Filiaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Attributes.Add("style", "word-break:break-all;word-wrap:break-word;");
                e.Row.Cells[3].Attributes.Add("style", "word-break:break-all;word-wrap:break-word;");

                if (e.Row.Cells[4].Text.Trim() != "&nbsp;")
                {
                    e.Row.Cells[4].Text = (Comun.FormatearFecha(e.Row.Cells[4].Text)).ToString(ConfigurationManager.AppSettings["FormatoFechas"]);
                }
            }
        }

        protected void CmbNacRecurr_SelectedIndexChanged(object sender, EventArgs e)
        {
            CmbNacRecurr.Focus();
            this.CmbProvPaisNac.Items.Clear();
            this.CmbDistCiudadNac.Items.Clear();
            this.CmbProvPaisNac.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
            this.CmbDistCiudadNac.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
            this.LblGentilicio.Text = "";

            

            if (CmbNacRecurr.SelectedValue == Constantes.CONST_NACIONALIDAD_PERUANA)
            {
                chk_PJ.Checked = false;
                chk_PJ.Enabled = true;

                comun_Part3.CargarUbigeo(Session, CmbDptoContNac, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, CmbNacRecurr.SelectedValue.ToString(), "", true, Enumerador.enmNacionalidad.PERUANA);
                CmbTipoDoc.SelectedValue = ((int)Enumerador.enmTipoDocumento.DNI).ToString();

                VisibilidadAsteriscos(true);
            }
            else
            {
                CmbTipoDoc.SelectedValue = HF_Pasaporte_ID.Value;

                chk_PJ.Checked = false;
                chk_PJ.Enabled = false;

                //--------------------------------------------------
                //Fecha: 27/01/2017
                //Autor: Miguel Márquez Beltrán
                //Objetivo: Cargar el Ubigeo Completo
                //--------------------------------------------------
                comun_Part3.CargarUbigeo(Session, CmbDptoContNac, Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT, CmbNacRecurr.SelectedValue.ToString(), "", true, Enumerador.enmNacionalidad.PERUANA);

                VisibilidadAsteriscos(false);
            }

            //-------------------------------------------------------
            //Fecha: 21/12/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Visibilidad de los asteriscos.
            //-------------------------------------------------------

            //-------------------------------------------------------

            CmbTipoDoc_SelectedIndexChanged(sender, e);
            CmbProvPaisNac.Enabled = true;
            CmbDistCiudadNac.Enabled = true;
            UpdDatosPersona.Update();
            updTabDatAdic.Update();

        }

        protected void GrdDirecciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Attributes.Add("style", "word-break:break-all;word-wrap:break-word;");
            }
        }

        public void GetValoresEspeciales()
        {
            String CaracterEspecial1 = String.Empty;
            String CaracterEspecial2 = String.Empty;
            String CaracterEspecial3 = String.Empty;
            String CaracterEspecialRune = String.Empty;

            CaracterEspecial1 = ConfigurationManager.AppSettings["ValidarText"].ToString();
            CaracterEspecial2 = ConfigurationManager.AppSettings["ValidarNumeroEntero"].ToString();
            CaracterEspecial3 = ConfigurationManager.AppSettings["ValidarNumeroDecimal"].ToString();
            CaracterEspecialRune = ConfigurationManager.AppSettings["validarCaracterRune"].ToString();

            HFValidarTexto.Value = CaracterEspecial1;
            HFValidarNumero.Value = CaracterEspecial2;
            HFValidarNumeroDecimal.Value = CaracterEspecial3;
            HFValidarTextoRune.Value = CaracterEspecialRune;
        }


        #region WEBMETHOD
        private void FiliacionX(Int64 pefi_iPersonaId)
        {
            PersonaFiliacionConsultaBL PersonaFiliacionConsultaBL = new PersonaFiliacionConsultaBL();
            Session.Remove("DtRegFilaciones");

            Int64 intiPersonaId = 0;

            //if (HFGUID.Value.Length > 0)
            //{
            //    intiPersonaId = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
            //}
            //else
            //{
            intiPersonaId = Convert.ToInt64(ViewState["iPersonaId"]);
            //}

            if (intiPersonaId != 0)
            {
                DataTable dtFiliados = (DataTable)PersonaFiliacionConsultaBL.Obtener(pefi_iPersonaId);

                PersonaConsultaBL loPersonaConsultaBL = new PersonaConsultaBL();

                if (dtFiliados.Rows.Count != 0)
                {
                    DataTable lDATATABLE = (DataTable)((Session["DtRegFilaciones"] == null) ? CrearDtRegFilaciones() : Session["DtRegFilaciones"]);

                    foreach (DataRow row in dtFiliados.Rows)
                    {
                        DataRow lDataRow = lDATATABLE.NewRow();
                        lDataRow["pefi_iPersonaFilacionId"] = row["pefi_iPersonaFilacionId"];
                        lDataRow["pefi_iPersonaId"] = row["pefi_iPersonaId"];
                        lDataRow["pefi_vNombreFiliacion"] = Convert.ToString(row["pefi_vNombreFiliacion"]).ToUpper();
                        lDataRow["pefi_vLugarNacimiento"] = row["pefi_vLugarNacimiento"];
                        lDataRow["pefi_sTipoFilacionId"] = row["pefi_sTipoFilacionId"];
                        lDataRow["pefi_vNroDocumento"] = row["pefi_vNroDocumento"];
                        lDataRow["pefi_sTipoFilacionId_desc"] = row["pefi_sTipoFilacionId_desc"];
                        lDataRow["pefi_sNacionalidad_desc"] = row["pefi_sNacionalidad_desc"];
                        lDataRow["pefi_sDocumentoTipoId"] = row["pefi_sDocumentoTipoId"];
                        lDataRow["pefi_iFiliadoId"] = row["pefi_iFiliadoId"];
                        lDataRow["pefi_cEstado"] = row["pefi_cEstado"];
                        lDATATABLE.Rows.Add(lDataRow);
                    }

                    #region Creando variables ...
                    foreach (DataRow row in lDATATABLE.Rows)
                    {
                        SGAC.BE.MRE.RE_PERSONAFILIACION lPERSONAFILIACION = new SGAC.BE.MRE.RE_PERSONAFILIACION();
                        lPERSONAFILIACION.pefi_iPersonaFilacionId = Convert.ToInt32(row["pefi_iPersonaFilacionId"]);
                        lPERSONAFILIACION.pefi_iPersonaId = Convert.ToInt32(row["pefi_iPersonaId"]);
                        lPERSONAFILIACION.pefi_sTipoFilacionId = Convert.ToInt16(row["pefi_sTipoFilacionId"]);

                        object tmp;
                        if (DBNull.Value != (tmp = row["pefi_dFechaNacimiento"])) lPERSONAFILIACION.pefi_dFechaNacimiento = Comun.FormatearFecha(row["pefi_dFechaNacimiento"].ToString());
                        if (DBNull.Value != (tmp = row["pefi_sNacionalidad"])) lPERSONAFILIACION.pefi_sNacionalidad = Convert.ToInt16(row["pefi_sNacionalidad"]);

                        SGAC.BE.MRE.RE_PERSONA lPERSONA = new SGAC.BE.MRE.RE_PERSONA();

                        if (DBNull.Value != (tmp = row["pefi_iFiliadoId"])) lPERSONA.pers_iPersonaId = Convert.ToInt32(row["pefi_iFiliadoId"]);
                        if (lPERSONA.pers_iPersonaId != 0)
                        {
                            lPERSONA = loPersonaConsultaBL.Obtener(lPERSONA);
                        }
                        else
                        {
                            lPERSONA.pers_vApellidoPaterno = Convert.ToString(row["pefi_vNombreFiliacion"]).ToUpper();
                        }
                        Dictionary<string, object> lOTROS = new Dictionary<string, object>();
                        lOTROS.Add("pefi_sTipoFilacionId_desc", Convert.ToString(row["pefi_sTipoFilacionId_desc"]));
                        lOTROS.Add("pefi_vLugarNacimiento_desc", Convert.ToString(row["pefi_vLugarNacimiento"]));
                        lOTROS.Add("pefi_sNacionalidad_desc", Convert.ToString(row["pefi_sNacionalidad_desc"]));

                        SessionGrid(lPERSONA, lPERSONAFILIACION, lOTROS);

                        ////// MDIAZ ---- 29/04/2015 11:31AM
                        ////if (row["pefi_iFiliadoId"] != null)
                        ////{
                        ////    if (row["pefi_iFiliadoId"].ToString() != string.Empty)
                        ////    {
                        ////        if (Convert.ToInt32(row["pefi_iFiliadoId"]) != 0)
                        ////        {
                        ////            lPERSONA.pers_iPersonaId = Convert.ToInt32(row["pefi_iFiliadoId"]);

                        ////            lPERSONA = loPersonaConsultaBL.Obtener(lPERSONA);

                        ////            Dictionary<string, object> lOTROS = new Dictionary<string, object>();
                        ////            lOTROS.Add("pefi_sTipoFilacionId_desc", Convert.ToString(row["pefi_sTipoFilacionId_desc"]));
                        ////            lOTROS.Add("pefi_vLugarNacimiento_desc", Convert.ToString(row["pefi_vLugarNacimiento"]));
                        ////            lOTROS.Add("pefi_sNacionalidad_desc", Convert.ToString(row["pefi_sNacionalidad_desc"]));

                        ////            SessionGrid(lPERSONA, lPERSONAFILIACION, lOTROS);
                        ////        }
                        ////    }
                        ////}                        
                    }
                    #endregion

                    Session["DtRegFilaciones"] = (DataTable)lDATATABLE;

                }
            }

            GrdFiliacion.DataSource = (DataTable)Session["DtRegFilaciones"];
            GrdFiliacion.DataBind();

        }

        [System.Web.Services.WebMethod]
        public static string obtener_persona(string tipodocumento, string documento)
        {
            SGAC.BE.MRE.RE_PERSONA lPersona = new SGAC.BE.MRE.RE_PERSONA();
            SGAC.BE.MRE.RE_PERSONAIDENTIFICACION lPersonaIdentificacion = new SGAC.BE.MRE.RE_PERSONAIDENTIFICACION();

            #region Identificación
            PersonaIdentificacionConsultaBL lPersonaIdentificacionConsultaBL = new PersonaIdentificacionConsultaBL();
            if (tipodocumento != null) lPersonaIdentificacion.peid_sDocumentoTipoId = Convert.ToInt16(tipodocumento);
            if (documento != null) lPersonaIdentificacion.peid_vDocumentoNumero = documento.ToString().ToUpper();
            lPersonaIdentificacion = lPersonaIdentificacionConsultaBL.Obtener(lPersonaIdentificacion);
            #endregion

            #region Verificar si existe Persona
            if (lPersonaIdentificacion.peid_iPersonaId != 0)
            {
                PersonaConsultaBL lPersonaConsultaBL = new PersonaConsultaBL();
                lPersona.pers_iPersonaId = lPersonaIdentificacion.peid_iPersonaId;
                lPersona.pers_vNombres = lPersonaIdentificacion.pers_vNombres;
                lPersona.pers_vApellidoPaterno = lPersonaIdentificacion.pers_vApellidoPaterno;
                lPersona.pers_vApellidoMaterno = lPersonaIdentificacion.pers_vApellidoMaterno;
                lPersona.pers_sNacionalidadId = lPersonaIdentificacion.pers_sNacionalidadId;
                //lPersona = lPersonaConsultaBL.Obtener(lPersona);
                //lPersona.Identificacion = lPersonaIdentificacion;
            }
            #endregion

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(lPersona).ToString();
        }

        [System.Web.Services.WebMethod]
        public static string obtener_departamento(string nacionalidad)
        {
            SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
            DataTable lDataTable = objBL.Consultar("01", null, "01");

            DataView lDataView = lDataTable.DefaultView;

            List<CBE_DROPDOWNLIST> loProvincias = (lDataView.ToTable(true, "ubge_vDepartamento", "ubge_cUbi01").AsEnumerable().Select(
                                                   x => new CBE_DROPDOWNLIST { ValueField = x.ItemArray[1].ToString(), TextField = x.ItemArray[0].ToString() })
                                                   ).ToList();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(loProvincias).ToString();
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

        [System.Web.Services.WebMethod]
        public static string set_GrdFiliacion(string filiacion, string identificacion, string persona, string otros)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Dictionary<string, object> dPersona = jss.Deserialize<dynamic>(persona);
            Dictionary<string, object> dFiliacion = jss.Deserialize<dynamic>(filiacion);
            Dictionary<string, object> dIdentificacion = jss.Deserialize<dynamic>(identificacion);
            Dictionary<string, object> dOtros = jss.Deserialize<dynamic>(otros);

            SGAC.BE.MRE.RE_PERSONAIDENTIFICACION lRE_PERSONAIDENTIFICACION = new BE.MRE.RE_PERSONAIDENTIFICACION();
            foreach (var itm in dIdentificacion)
            {
                PropertyInfo pInfo = lRE_PERSONAIDENTIFICACION.GetType().GetProperty(itm.Key);
                pInfo.SetValue(lRE_PERSONAIDENTIFICACION, Convert.ChangeType(itm.Value, pInfo.PropertyType), null);
            }

            SGAC.BE.MRE.RE_PERSONA lRE_PERSONA = new BE.MRE.RE_PERSONA();
            foreach (var itm in dPersona)
            {
                PropertyInfo pInfo = lRE_PERSONA.GetType().GetProperty(itm.Key);
                pInfo.SetValue(lRE_PERSONA, Convert.ChangeType(itm.Value, pInfo.PropertyType), null);
            }

            SGAC.BE.MRE.RE_PERSONAFILIACION lRE_PERSONAFILIACION = new SGAC.BE.MRE.RE_PERSONAFILIACION();
            foreach (var itm in dFiliacion)
            {
                PropertyInfo pInfo = lRE_PERSONAFILIACION.GetType().GetProperty(itm.Key);
                pInfo.SetValue(lRE_PERSONAFILIACION, Convert.ChangeType(itm.Value, pInfo.PropertyType), null);
            }

            lRE_PERSONA.Identificacion = lRE_PERSONAIDENTIFICACION;

            List<SGAC.BE.MRE.RE_PERSONA> lPERSONA_Container = (List<SGAC.BE.MRE.RE_PERSONA>)HttpContext.Current.Session["GrdFiliacion_Persona"];
            List<SGAC.BE.MRE.RE_PERSONAFILIACION> lPERSONAFILIACION_Container = (List<BE.MRE.RE_PERSONAFILIACION>)HttpContext.Current.Session["GrdFiliacion"];
            List<Dictionary<string, object>> lOTROSFILIACION_Container = (List<Dictionary<string, object>>)HttpContext.Current.Session["GrdFiliacion_Otros"];

            if (lPERSONA_Container == null)
            {
                lPERSONA_Container = new List<SGAC.BE.MRE.RE_PERSONA>();
                lPERSONAFILIACION_Container = new List<SGAC.BE.MRE.RE_PERSONAFILIACION>();
                lOTROSFILIACION_Container = new List<Dictionary<string, object>>();
            }

            lPERSONA_Container.Add(lRE_PERSONA);
            lPERSONAFILIACION_Container.Add(lRE_PERSONAFILIACION);
            lOTROSFILIACION_Container.Add(dOtros);

            HttpContext.Current.Session["GrdFiliacion"] = lPERSONAFILIACION_Container;
            HttpContext.Current.Session["GrdFiliacion_Persona"] = lPERSONA_Container;
            HttpContext.Current.Session["GrdFiliacion_Otros"] = lOTROSFILIACION_Container;
            return "";
        }

        [System.Web.Services.WebMethod]
        public static string get_Session(string nombre)
        {
            return HttpContext.Current.Session[nombre].ToString();
        }

        void SessionGrid(SGAC.BE.MRE.RE_PERSONA Persona,
                         SGAC.BE.MRE.RE_PERSONAFILIACION Filiacion,
                         Dictionary<string, object> Otros)
        {
            List<SGAC.BE.MRE.RE_PERSONA> lPERSONA_Container = (List<SGAC.BE.MRE.RE_PERSONA>)Session["GrdFiliacion_Persona"];
            List<SGAC.BE.MRE.RE_PERSONAFILIACION> lPERSONAFILIACION_Container = (List<SGAC.BE.MRE.RE_PERSONAFILIACION>)Session["GrdFiliacion"];
            List<Dictionary<string, object>> lOTROSFILIACION_Container = (List<Dictionary<string, object>>)Session["GrdFiliacion_Otros"];

            if (lPERSONA_Container == null)
            {
                lPERSONA_Container = new List<SGAC.BE.MRE.RE_PERSONA>();
                lPERSONAFILIACION_Container = new List<SGAC.BE.MRE.RE_PERSONAFILIACION>();
                lOTROSFILIACION_Container = new List<Dictionary<string, object>>();
            }

            lPERSONA_Container.Add(Persona);
            lPERSONAFILIACION_Container.Add(Filiacion);
            lOTROSFILIACION_Container.Add(Otros);

            Session["GrdFiliacion_Persona"] = (List<SGAC.BE.MRE.RE_PERSONA>)lPERSONA_Container;
            Session["GrdFiliacion"] = (List<SGAC.BE.MRE.RE_PERSONAFILIACION>)lPERSONAFILIACION_Container;
            Session["GrdFiliacion_Otros"] = (List<Dictionary<string, object>>)lOTROSFILIACION_Container;
        }

        protected void btn_GridFiliacionRefresh_Click(object sender, EventArgs e)
        {
            List<SGAC.BE.MRE.RE_PERSONA> lPERSONA_Container = (List<SGAC.BE.MRE.RE_PERSONA>)Session["GrdFiliacion_Persona"];
            List<SGAC.BE.MRE.RE_PERSONAFILIACION> lPERSONAFILIACION_Container = (List<SGAC.BE.MRE.RE_PERSONAFILIACION>)Session["GrdFiliacion"];
            List<Dictionary<string, object>> lOTROSFILIACION_Container = (List<Dictionary<string, object>>)Session["GrdFiliacion_Otros"];

            //
            DataTable FiliacionDataTable = (DataTable)Session["DtRegFilaciones"];
            if (FiliacionDataTable != null) { FiliacionDataTable.Clear(); }

            // Crear DataTable
            DataTable dt = new DataTable();
            dt.Columns.Add("pefi_iPersonaFilacionId", typeof(int));
            dt.Columns.Add("pefi_iPersonaId", typeof(int));
            dt.Columns.Add("pefi_sTipoFilacionId", typeof(int));
            dt.Columns.Add("pefi_sTipoFilacionId_desc", typeof(string));
            dt.Columns.Add("pefi_vNombreFiliacion", typeof(string));
            dt.Columns.Add("pefi_dFechaNacimiento", typeof(DateTime));
            dt.Columns.Add("pefi_vLugarNacimiento", typeof(string));
            dt.Columns.Add("pefi_sNacionalidad_desc", typeof(string));
            dt.Columns.Add("pers_vApellidoPaterno", typeof(string));
            dt.Columns.Add("pers_vApellidoMaterno", typeof(string));
            dt.Columns.Add("pers_vNombres", typeof(string));

            string strPrimerApellido = string.Empty;
            string strSegundoApellido = string.Empty;
            string strNombres = string.Empty;

            //
            for (var i = 0; i <= lPERSONA_Container.Count - 1; i++)
            {
                DataRow lRow = dt.NewRow();
                lRow["pefi_iPersonaFilacionId"] = lPERSONAFILIACION_Container[i].pefi_iPersonaFilacionId;
                lRow["pefi_iPersonaId"] = lPERSONA_Container[i].pers_iPersonaId;
                lRow["pefi_sTipoFilacionId"] = lPERSONAFILIACION_Container[i].pefi_sTipoFilacionId;
                lRow["pefi_sTipoFilacionId_desc"] = lOTROSFILIACION_Container[i]["pefi_sTipoFilacionId_desc"].ToString();

                if (lPERSONA_Container[i].pers_vApellidoPaterno != null)
                    strPrimerApellido = lPERSONA_Container[i].pers_vApellidoPaterno.ToUpper();
                if (lPERSONA_Container[i].pers_vApellidoMaterno != null)
                    strSegundoApellido = lPERSONA_Container[i].pers_vApellidoMaterno.ToUpper();
                if (lPERSONA_Container[i].pers_vNombres != null)
                    strNombres = lPERSONA_Container[i].pers_vNombres.ToUpper();

                lRow["pefi_vNombreFiliacion"] = strPrimerApellido + " " + strSegundoApellido + "," + strNombres;
                lRow["pefi_vLugarNacimiento"] = lOTROSFILIACION_Container[i]["pefi_vLugarNacimiento_desc"].ToString().ToUpper();
                lRow["pefi_sNacionalidad_desc"] = lOTROSFILIACION_Container[i]["pefi_sNacionalidad_desc"].ToString();
                lRow["pers_vApellidoPaterno"] = strPrimerApellido;
                lRow["pers_vApellidoMaterno"] = strSegundoApellido;
                lRow["pers_vNombres"] = strNombres;
                dt.Rows.Add(lRow);

                //Pasando al DataTable(Existente)
                if (FiliacionDataTable == null) { FiliacionDataTable = CrearDtRegFilaciones(); }
                DataRow lRowFiliacion = FiliacionDataTable.NewRow();

                lRowFiliacion["pefi_iPersonaFilacionId"] = lPERSONAFILIACION_Container[i].pefi_iPersonaFilacionId;
                lRowFiliacion["pefi_vNombreFiliacion"] = strPrimerApellido + " " + strSegundoApellido + "," + strNombres;
                lRowFiliacion["pefi_vLugarNacimiento"] = lOTROSFILIACION_Container[i]["pefi_vLugarNacimiento_desc"].ToString().ToUpper();
                lRowFiliacion["pefi_sNacionalidad"] = lPERSONA_Container[i].pers_sNacionalidadId;
                lRowFiliacion["pefi_sNacionalidad_desc"] = lOTROSFILIACION_Container[i]["pefi_sNacionalidad_desc"].ToString();
                lRowFiliacion["pefi_sTipoFilacionId"] = lPERSONAFILIACION_Container[i].pefi_sTipoFilacionId;
                lRowFiliacion["pefi_sTipoFilacionId_desc"] = lOTROSFILIACION_Container[i]["pefi_sTipoFilacionId_desc"].ToString();
                lRowFiliacion["pefi_vNroDocumento"] = lPERSONA_Container[i].Identificacion.peid_vDocumentoNumero.ToUpper();

                //if (HFGUID.Value.Length > 0)
                //{
                //    lRowFiliacion["pefi_iPersonaId"] = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
                //}
                //else
                //{
                lRowFiliacion["pefi_iPersonaId"] = Convert.ToInt64(ViewState["iPersonaId"]);
                //}

                lRowFiliacion["pefi_sDocumentoTipoId"] = lPERSONA_Container[i].Identificacion.peid_sDocumentoTipoId;
                lRowFiliacion["pefi_iFiliadoId"] = lPERSONA_Container[i].pers_iPersonaId;
                lRowFiliacion["pers_vApellidoPaterno"] = strPrimerApellido;
                lRowFiliacion["pers_vApellidoMaterno"] = strSegundoApellido;
                lRowFiliacion["pers_vNombres"] = strNombres;

                FiliacionDataTable.Rows.Add(lRowFiliacion);

            }
            // Bind
            GrdFiliacion.DataSource = dt;
            GrdFiliacion.DataBind();

            Session["DtRegFilaciones"] = (DataTable)FiliacionDataTable;
        }
        #endregion

        object getColumn(DataTable dt, string colname)
        {
            var qry = (from r in dt.AsEnumerable()
                       select r.Field<object>(colname)).ToArray();
            return qry[0];
        }

        protected void GrdFiliacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable FiliacionDataTable = (DataTable)Session["DtRegFilaciones"];
            List<SGAC.BE.MRE.RE_PERSONA> lPERSONA_Container = (List<SGAC.BE.MRE.RE_PERSONA>)Session["GrdFiliacion_Persona"];
            List<SGAC.BE.MRE.RE_PERSONAFILIACION> lPERSONAFILIACION_Container = (List<SGAC.BE.MRE.RE_PERSONAFILIACION>)Session["GrdFiliacion"];
            List<Dictionary<string, object>> lOTROSFILIACION_Container = (List<Dictionary<string, object>>)Session["GrdFiliacion_Otros"];

            int RowIndex = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case "Editar":
                    //ddl_TipFiliacion.SelectedValue = FiliacionDataTable.Rows[RowIndex]["pefi_sTipoFilacionId"].ToString();
                    //ddl_peid_sDocumentoTipoId.SelectedValue = FiliacionDataTable.Rows[RowIndex]["pefi_sDocumentoTipoId"].ToString();
                    //txt_peid_vDocumentoNumero.Text = FiliacionDataTable.Rows[RowIndex]["pefi_vNroDocumento"].ToString();
                    //ddl_pers_sNacionalidadId.SelectedValue = FiliacionDataTable.Rows[RowIndex]["pefi_sNacionalidad"].ToString();
                    //txt_pers_vNombres.Text = FiliacionDataTable.Rows[RowIndex]["pers_vNombres"].ToString();
                    //txt_pers_vApellidoPaterno.Text = FiliacionDataTable.Rows[RowIndex]["pers_vApellidoPaterno"].ToString();
                    //txt_pers_vApellidoMaterno.Text = FiliacionDataTable.Rows[RowIndex]["pers_vApellidoMaterno"].ToString();
                    //txt_pefi_vLugarNacimiento.Text = FiliacionDataTable.Rows[RowIndex]["pefi_vLugarNacimiento"].ToString();

                    //break;
                case "Eliminar":
                    Int64 iPersonaFilacionId = Convert.ToInt32(GrdFiliacion.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].Text);
                    Int64 iPersonaId = Convert.ToInt32(GrdFiliacion.Rows[Convert.ToInt32(e.CommandArgument)].Cells[1].Text);
                    if (iPersonaFilacionId == 0)
                    {

                        /*
                         * CASO 1.- Cuando es nuevo (pefi_iPersonaFilacionId = 0)
                         * Trae de memoria el grid
                         * buscar en el grid e iliminar
                         * actualizar el grid
                         */
                        FiliacionDataTable.Rows[Convert.ToInt32(e.CommandArgument)].Delete();

                        //SGAC.BE.MRE.RE_PERSONA borrar_PERSONA = lPERSONA_Container[Convert.ToInt32(e.CommandArgument)];
                        //lPERSONA_Container.Remove(borrar_PERSONA);

                        //SGAC.BE.MRE.RE_PERSONAFILIACION borrar_PERSONAFILIACION = lPERSONAFILIACION_Container[Convert.ToInt32(e.CommandArgument)];

                        //lPERSONAFILIACION_Container.Remove(borrar_PERSONAFILIACION);

                        //Dictionary<string, object> borrar_OTROS = lOTROSFILIACION_Container[Convert.ToInt32(e.CommandArgument)];
                        //lOTROSFILIACION_Container.Remove(borrar_OTROS);

                        Session["DtRegFilaciones"] = (DataTable)FiliacionDataTable;
                        //Session["GrdFiliacion_Persona"] = (List<SGAC.BE.MRE.RE_PERSONA>)lPERSONA_Container;
                        //Session["GrdFiliacion"] = (List<SGAC.BE.MRE.RE_PERSONAFILIACION>)lPERSONAFILIACION_Container;
                        //Session["GrdFiliacion_Otros"] = (List<Dictionary<string, object>>)lOTROSFILIACION_Container;

                        GrdFiliacion.DataSource = FiliacionDataTable;
                        GrdFiliacion.DataBind();
                    }
                    else
                    {
                        /*
                        * CASO 2.- Cuando es edicion (pefi_iPersonaFilacionId != 0)
                        * Trae de memoria el grid
                        * buscar en el grid e iliminar
                        * actualizar el grid
                        */

                        SGAC.BE.MRE.RE_PERSONAFILIACION borrar_PERSONAFILIACION = lPERSONAFILIACION_Container[Convert.ToInt32(e.CommandArgument)];
                        lPERSONAFILIACION_Container.Remove(borrar_PERSONAFILIACION);

                        FiliacionDataTable.Rows[Convert.ToInt32(e.CommandArgument)].Delete();

                        Session["DtRegFilaciones"] = (DataTable)FiliacionDataTable;

                        GrdFiliacion.DataSource = FiliacionDataTable;
                        GrdFiliacion.DataBind();
                    }


                    break;
            }
        }

        protected void CmbGenero_SelectedIndexChanged(object sender, EventArgs e)
        {

            ValidarApellidoCasadaMostrar();
            //-----------------------------------------------
            //Fecha: 23/03/2017
            //Autor: Miguel Márquez Beltrán
            //Objetivo: Asignar el gentilicio
            //-----------------------------------------------
            string strDptoContNac = CmbDptoContNac.SelectedValue.ToString();
            string strProvPaisNac = CmbProvPaisNac.SelectedValue.ToString();
            string strGenero = CmbGenero.SelectedValue.ToString();

            //jonatan silva - se obtiene el gentilicio sin ir nuevamente a la base de datos 12/01/2018
            string strGentilicio = Comun.AsignarGentilicioPorCodigoPais(Session, strDptoContNac, strProvPaisNac, strGenero).ToString();
            //string strGentilicio = Comun.AsignarGentilicio(Session, strDptoContNac, strProvPaisNac, strGenero).ToString();

            this.LblDescGentilicio.Text = strGentilicio;
            //-----------------------------------------------
            CmbGenero.Focus();
            ActualizarGenero();
            updTabDatAdic.Update();
        }


        //--------------------------------------------                    
        // Creador por: Jonatan Silva Cachay
        // Fecha: 02/02/2017
        // Objetivo: Crea un datatable que se utiliza para reemplazar datos de la plantilla HTML del formato del correo
        //--------------------------------------------
        private DataTable crearTabla(long LonActuacionId1)
        {
            DataTable dtReemplazaCorreo = new DataTable();
            DataColumn dc1 = new DataColumn();
            DataColumn dc2 = new DataColumn();
            dc1.ColumnName = "valor";
            dc2.ColumnName = "reemplazo";
            dtReemplazaCorreo.Columns.Add(dc1);
            dtReemplazaCorreo.Columns.Add(dc2);

            /*Obtener Connacional*/
            string strEtiquetaSolicitante = string.Empty;
            if (txtApePat.Text != "")
                strEtiquetaSolicitante += txtApePat.Text + " ";
            if (txtApeMat.Text != "")
                strEtiquetaSolicitante += txtApeMat.Text + " ";
            if (txtNombres.Text != "")
            {
                strEtiquetaSolicitante += ", " + txtNombres.Text + " ";
            }

            DataRow dr = default(DataRow);
            dr = dtReemplazaCorreo.NewRow();
            dr[0] = "{CONNACIONAL}";
            dr[1] = strEtiquetaSolicitante.ToString().ToUpper();
            dtReemplazaCorreo.Rows.Add(dr);

            DataRow dr1 = default(DataRow);
            dr1 = dtReemplazaCorreo.NewRow();
            dr1[0] = "{NROACTUACION}";
            dr1[1] = Convert.ToString(LonActuacionId1);
            dtReemplazaCorreo.Rows.Add(dr1);

            DataRow dr2 = default(DataRow);
            dr2 = dtReemplazaCorreo.NewRow();
            dr2[0] = "{FECHAACTUACION}";
            dr2[1] = System.DateTime.Now;
            dtReemplazaCorreo.Rows.Add(dr2);

            DataRow dr3 = default(DataRow);
            dr3 = dtReemplazaCorreo.NewRow();
            dr3[0] = "{NATURALEZA}";
            dr3[1] = "58A - POR INSCRIPCION EN REGISTRO DE NACIONALES";
            dtReemplazaCorreo.Rows.Add(dr3);

            DataRow dr4 = default(DataRow);
            dr4 = dtReemplazaCorreo.NewRow();
            dr4[0] = "{MISIONCONSULAR}";
            dr4[1] = comun_Part2.ObtenerNombreOficinaPorId(Session, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
            dtReemplazaCorreo.Rows.Add(dr4);

            DataRow dr5 = default(DataRow);
            dr5 = dtReemplazaCorreo.NewRow();
            dr5[0] = "{MONTO}";
            dr5[1] = "GRATUITO POR LEY";
            dtReemplazaCorreo.Rows.Add(dr5);

            DataRow dr6 = default(DataRow);
            dr6 = dtReemplazaCorreo.NewRow();
            dr6[0] = "{SITUACION}";
            dr6[1] = "REGISTRADO";
            dtReemplazaCorreo.Rows.Add(dr6);

            DataRow dr7 = default(DataRow);
            dr7 = dtReemplazaCorreo.NewRow();
            dr7[0] = "{fechaActual}";
            dr7[1] = System.DateTime.Now;
            dtReemplazaCorreo.Rows.Add(dr7);

            DataRow dr8 = default(DataRow);
            dr8 = dtReemplazaCorreo.NewRow();
            dr8[0] = "{MONTOMONEDALOCAL}";
            dr8[1] = "";
            dtReemplazaCorreo.Rows.Add(dr8);

            DataRow dr9 = default(DataRow);
            dr9 = dtReemplazaCorreo.NewRow();
            dr9[0] = "{MONEDALOCAL}";
            dr9[1] = "";
            dtReemplazaCorreo.Rows.Add(dr9);

            return dtReemplazaCorreo;
        }

        //--------------------------------------------                    
        // Creador por: Jonatan Silva Cachay
        // Fecha: 02/02/2017
        // Objetivo: Crea un datatable que se utiliza para reemplazar datos de la plantilla HTML del formato del correo
        //--------------------------------------------
        private DataTable crearTabla()
        {
            DataTable dtReemplazaCorreo = new DataTable();
            DataColumn dc1 = new DataColumn();
            DataColumn dc2 = new DataColumn();
            dc1.ColumnName = "valor";
            dc2.ColumnName = "reemplazo";
            dtReemplazaCorreo.Columns.Add(dc1);
            dtReemplazaCorreo.Columns.Add(dc2);

            /*Obtener Connacional*/
            string strEtiquetaSolicitante = string.Empty;
            if (txtApePat.Text != "")
                strEtiquetaSolicitante += txtApePat.Text + " ";
            if (txtApeMat.Text != "")
                strEtiquetaSolicitante += txtApeMat.Text + " ";
            if (txtNombres.Text != "")
            {
                strEtiquetaSolicitante += ", " + txtNombres.Text + " ";
            }

            DataRow dr = default(DataRow);
            dr = dtReemplazaCorreo.NewRow();
            dr[0] = "{CONNACIONAL}";
            dr[1] = strEtiquetaSolicitante.ToString().ToUpper();
            dtReemplazaCorreo.Rows.Add(dr);

            DataRow dr1 = default(DataRow);
            dr1 = dtReemplazaCorreo.NewRow();
            dr1[0] = "{MISIONCONSULAR}";
            dr1[1] = comun_Part2.ObtenerNombreOficinaPorId(Session, Convert.ToInt32(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]));
            dtReemplazaCorreo.Rows.Add(dr1);

            DataRow dr2 = default(DataRow);
            dr2 = dtReemplazaCorreo.NewRow();
            dr2[0] = "{fechaActual}";
            dr2[1] = System.DateTime.Now;
            dtReemplazaCorreo.Rows.Add(dr2);

            return dtReemplazaCorreo;
        }
        //--------------------------------------------                    
        // Creador por: Jonatan Silva Cachay
        // Fecha: 02/02/2017
        // Objetivo: Envio de Correo
        //--------------------------------------------
        private bool EnviarCorreoRegistro(DataTable _dtReemplazo, string CorreoElectronico, string strASUNTO, string strPlantilla)
        {
            #region Envío Correo
            string strScript = string.Empty;

            string strSMTPServer = string.Empty;
            string strSMTPPuerto = string.Empty;
            string strEmailFrom = string.Empty;
            string strEmailPassword = string.Empty;
            string strEmailTo = string.Empty;

            strSMTPServer = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.CONFIG_SERVIDOR_CORREO, (int)Enumerador.enmConfiguracionCorreo.SERVIDOR, "descripcion");
            strSMTPPuerto = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.CONFIG_SERVIDOR_CORREO, (int)Enumerador.enmConfiguracionCorreo.PUERTO, "descripcion");

            strEmailFrom = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.CONFIG_SERVIDOR_CORREO, (int)Enumerador.enmConfiguracionCorreo.CORREO_DE, "descripcion");
            strEmailPassword = comun_Part1.ObtenerParametroDatoPorCampo(Session, Enumerador.enmGrupo.CONFIG_SERVIDOR_CORREO, (int)Enumerador.enmConfiguracionCorreo.CONTRASENIA, "descripcion");

            strEmailTo = CorreoElectronico;
            string strTitulo = strASUNTO + " - " + Session[Constantes.CONST_SESION_OFICINACONSULAR_NOMBRE];

            // ENVIAR CORREO
            Enumerador.enmTipoMensaje enmTipoMensaje = Enumerador.enmTipoMensaje.INFORMATION;
            bool bEnviado = false;
            string strMensaje = string.Empty;
            string strCorreo = string.Empty;
            string strRutaCorreo = string.Empty;
            strRutaCorreo = Server.MapPath("~") + strPlantilla;
            try
            {
                bEnviado = Correo.EnviarCorreoPlantillaHTML(strRutaCorreo, _dtReemplazo, strSMTPServer, strSMTPPuerto,
                                               strEmailFrom, strEmailPassword,
                                               strEmailTo, strTitulo, System.Net.Mail.MailPriority.High, null);
            }
            catch (Exception ex)
            {
                strCorreo += " (Error al enviar el correo: " + ex.Message + ")";

                #region Registro Incidencia
                new SGAC.Auditoria.BL.AuditoriaMantenimientoBL().Insertar_Error(new BE.MRE.SI_AUDITORIA
                {
                    audi_vNombreRuta = Util.ObtenerNameForm(),
                    audi_vValoresTabla = "ENVIO CORREO",
                    audi_sOperacionTipoId = (int)Enumerador.enmTipoIncidencia.ERROR_APLICATION,
                    audi_sOperacionResultadoId = (int)Enumerador.enmResultadoAuditoria.ERR,
                    audi_sTablaId = null,
                    audi_sClavePrimaria = null,
                    audi_sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]),
                    audi_vComentario = ex.Message,
                    audi_vMensaje = ex.StackTrace,
                    audi_vHostName = Util.ObtenerHostName(),
                    audi_sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]),
                    audi_vIPCreacion = Util.ObtenerDireccionIP()
                });
                #endregion
            }
            //--

            strScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, strNombreEntidad, "Correo enviado exitosamente.", false, 160, 300);

            #endregion

            string strScriptCorreo = string.Empty;
            if (!bEnviado)
            {
                enmTipoMensaje = Enumerador.enmTipoMensaje.ERROR;
                strCorreo = "Correo no enviado." + strCorreo;
                strScriptCorreo = Mensaje.MostrarMensaje(enmTipoMensaje, "ENVÍO CORREO", strCorreo);

                Comun.EjecutarScript(Page, strScriptCorreo);
            }
            else
            {
                if (strMensaje != string.Empty)
                {
                    strScript += Mensaje.MostrarMensaje(enmTipoMensaje, "ENVÍO CORREO", strMensaje);
                }

                Comun.EjecutarScript(Page, strScript);
            }
            return bEnviado;
        }
        private void ActualizarGenero()
        {
            string strScript = string.Empty;
            strScript = @"$(function(){{
                                        ObtenerElementosGenero();
                                    }});";
            strScript = string.Format(strScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "Genero", strScript, true);
        }

        protected void btnLimpiarDireccion_Click(object sender, EventArgs e)
        {
            string StrScript = string.Empty;

            hidDirID.Value = "";
            Session["iOperDoc"] = true;

            StrScript = @"$(function(){{
                            LimpiarPestañaDirecciones();
                        }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "LimpiarPestañaDirecciones", StrScript, true);

            ViewState.Add("AccionDocumento", "Nuevo");
            Session["iOperDir"] = true;
            HF_TipoDocumento_Editando.Value = "-1";
            HF_NumeroDocumento_Editando.Value = "";

            ddl_TipoDocumentoM.Enabled = true;
            UpdDirecciones.Update();
        }

        protected void btnGrabarFiliacion_Click(object sender, EventArgs e)
        {
            bool bolEsNuevo = (bool)Session["OperRune"];

            //if (bolEsNuevo)
            //{
                DataTable _dt = (DataTable)Session["DtRegFilaciones"];
                DataTable _PersonaFiliacion = (DataTable)ViewState["PersonaFiliacion"];

                if (_PersonaFiliacion == null) {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Filiación", "Se debe realizar la búsqueda por Nro. de Documento."));    
                    return; 
                }

                if (_PersonaFiliacion.Rows.Count == 0)
                {
                    if (ddl_TipFiliacion.SelectedIndex == 0)
                    {
                        ddl_TipFiliacion.Focus();
                        ddl_TipFiliacion.Style.Add("border", "solid Red 1px");
                        string StrScript2 = @"$(function(){{
                            MoveTabIndex(4);
                        }});";
                        StrScript2 = string.Format(StrScript2);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex4", StrScript2, true);
                        return;
                    }
                    else{
                        ddl_TipFiliacion.Style.Add("border", "solid #888888 1px");
                    }
                    if (ddl_peid_sDocumentoTipoId.SelectedIndex == 0)
                    {
                        ddl_peid_sDocumentoTipoId.Focus();
                        ddl_peid_sDocumentoTipoId.Style.Add("border", "solid Red 1px");
                        string StrScript2 = @"$(function(){{
                            MoveTabIndex(4);
                        }});";
                        StrScript2 = string.Format(StrScript2);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex4", StrScript2, true);
                        return;
                    }
                    else
                    {
                        ddl_peid_sDocumentoTipoId.Style.Add("border", "solid #888888 1px");
                    }
                    if (txt_peid_vDocumentoNumero.Text.Length == 0)
                    {
                        txt_peid_vDocumentoNumero.Focus();
                        txt_peid_vDocumentoNumero.Style.Add("border", "solid Red 1px");
                        string StrScript2 = @"$(function(){{
                            MoveTabIndex(4);
                        }});";
                        StrScript2 = string.Format(StrScript2);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex4", StrScript2, true);
                        return;
                    }
                    else {
                        txt_peid_vDocumentoNumero.Style.Add("border", "solid #888888 1px");
                    }
                    if (ddl_pers_sNacionalidadId.SelectedIndex == 0)
                    {
                        ddl_pers_sNacionalidadId.Focus();
                        ddl_pers_sNacionalidadId.Style.Add("border", "solid Red 1px");
                        string StrScript2 = @"$(function(){{
                            MoveTabIndex(4);
                        }});";
                        StrScript2 = string.Format(StrScript2);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex4", StrScript2, true);
                        return;
                    }
                    else
                    {
                        ddl_pers_sNacionalidadId.Style.Add("border", "solid #888888 1px");
                    }
                    if (txt_pers_vNombres.Text.Length == 0)
                    {
                        txt_pers_vNombres.Focus();
                        txt_pers_vNombres.Style.Add("border", "solid Red 1px");
                        string StrScript2 = @"$(function(){{
                            MoveTabIndex(4);
                        }});";
                        StrScript2 = string.Format(StrScript2);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex4", StrScript2, true);
                        return;
                    }
                    else {
                        txt_pers_vNombres.Style.Add("border", "solid #888888 1px");
                    }
                    if (txt_pers_vApellidoPaterno.Text.Length == 0)
                    {
                        txt_pers_vApellidoPaterno.Focus();
                        txt_pers_vApellidoPaterno.Style.Add("border", "solid Red 1px");
                        string StrScript2 = @"$(function(){{
                            MoveTabIndex(4);
                        }});";
                        StrScript2 = string.Format(StrScript2);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex4", StrScript2, true);
                        return;
                    }
                    else {
                        txt_pers_vApellidoPaterno.Style.Add("border", "solid #888888 1px");
                    }                    
                }
                //Pasando al DataTable(Existente)
                DataTable FiliacionDataTable = (DataTable)Session["DtRegFilaciones"];
                //if (FiliacionDataTable != null) { FiliacionDataTable.Clear(); }

                if (FiliacionDataTable == null) { FiliacionDataTable = CrearDtRegFilaciones(); }
                DataRow lRowFiliacion = FiliacionDataTable.NewRow();

                if (_PersonaFiliacion.Rows.Count > 0)
                {
                    ddl_pers_sNacionalidadId.SelectedValue = _PersonaFiliacion.Rows[0]["pers_sNacionalidadId"].ToString();
                    txt_pers_vNombres.Text = _PersonaFiliacion.Rows[0]["pers_vNombres"].ToString();
                    txt_pers_vApellidoPaterno.Text = _PersonaFiliacion.Rows[0]["pers_vApellidoPaterno"].ToString();
                    txt_pers_vApellidoMaterno.Text = _PersonaFiliacion.Rows[0]["pers_vApellidoMaterno"].ToString();


                    ddl_pers_sNacionalidadId.Enabled = false;
                    txt_pers_vNombres.Enabled = false;
                    txt_pers_vApellidoPaterno.Enabled = false;
                    txt_pers_vApellidoMaterno.Enabled = false;


                    lRowFiliacion["pefi_sNacionalidad"] = Convert.ToInt16(_PersonaFiliacion.Rows[0]["pers_sNacionalidadId"].ToString());
                    lRowFiliacion["pefi_iPersonaId"] = Convert.ToInt64(_PersonaFiliacion.Rows[0]["peid_iPersonaId"]);
                    lRowFiliacion["pefi_iFiliadoId"] = Convert.ToInt64(_PersonaFiliacion.Rows[0]["peid_iPersonaId"]);
                }
                else
                {
                    ddl_pers_sNacionalidadId.Enabled = true;
                    txt_pers_vNombres.Enabled = true;
                    txt_pers_vApellidoPaterno.Enabled = true;
                    txt_pers_vApellidoMaterno.Enabled = true;

                    lRowFiliacion["pefi_sNacionalidad"] = Convert.ToInt16(ddl_pers_sNacionalidadId.SelectedValue);

                    //if (HFGUID.Value.Length > 0)
                    //{
                    //    lRowFiliacion["pefi_iPersonaId"] = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
                    //    lRowFiliacion["pefi_iFiliadoId"] = Convert.ToInt64(Session["iPersonaId" + HFGUID.Value]);
                    //}
                    //else
                    //{
                    lRowFiliacion["pefi_iPersonaId"] = Convert.ToInt64(ViewState["iPersonaId"]);
                    lRowFiliacion["pefi_iFiliadoId"] = Convert.ToInt64(ViewState["iPersonaId"]);
                    //}
                }

                lRowFiliacion["pefi_iPersonaFilacionId"] = 0;
                lRowFiliacion["pefi_vNombreFiliacion"] = txt_pers_vApellidoPaterno.Text.ToUpper() + " " + txt_pers_vApellidoMaterno.Text.ToUpper() + "," + txt_pers_vNombres.Text.ToUpper();
                lRowFiliacion["pefi_vLugarNacimiento"] = txt_pefi_vLugarNacimiento.Text.ToUpper();

                lRowFiliacion["pefi_sNacionalidad_desc"] = ddl_pers_sNacionalidadId.SelectedItem.Text.ToUpper();
                lRowFiliacion["pefi_sTipoFilacionId"] = Convert.ToInt16(ddl_TipFiliacion.SelectedValue);
                lRowFiliacion["pefi_sTipoFilacionId_desc"] = ddl_TipFiliacion.SelectedItem.Text.ToUpper();
                lRowFiliacion["pefi_vNroDocumento"] = txt_peid_vDocumentoNumero.Text;

                lRowFiliacion["pefi_sDocumentoTipoId"] = ddl_peid_sDocumentoTipoId.SelectedValue;
                lRowFiliacion["pers_vApellidoPaterno"] = txt_pers_vApellidoPaterno.Text.ToUpper();
                lRowFiliacion["pers_vApellidoMaterno"] = txt_pers_vApellidoMaterno.Text.ToUpper();
                lRowFiliacion["pers_vNombres"] = txt_pers_vNombres.Text.ToUpper();

                FiliacionDataTable.Rows.Add(lRowFiliacion);

                Session["DtRegFilaciones"] = FiliacionDataTable;

                GrdFiliacion.DataSource = FiliacionDataTable;
                GrdFiliacion.DataBind();

                txt_pers_vNombres.Text = "";
                txt_pers_vApellidoPaterno.Text = "";
                txt_pers_vApellidoMaterno.Text = "";
                ddl_pers_sNacionalidadId.SelectedIndex = 0;
                txt_pefi_vLugarNacimiento.Text = "";
            //}

            ddl_TipFiliacion.SelectedIndex = 0;
            ddl_peid_sDocumentoTipoId.SelectedIndex = 0;
            txt_peid_vDocumentoNumero.Text = "";
            ddl_pers_sNacionalidadId.SelectedIndex = 0;
            txt_pers_vNombres.Text = "";
            txt_pers_vApellidoPaterno.Text = "";
            txt_pers_vApellidoMaterno.Text = "";
            txt_pefi_vLugarNacimiento.Text = "";
            ddl_TipFiliacion.Focus();

            _PersonaFiliacion = null;
            ViewState["PersonaFiliacion"] = null;
            string StrScript = @"$(function(){{
                            MoveTabIndex(4);
                        }});";
            StrScript = string.Format(StrScript);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex4", StrScript, true);
        }

        protected void imgBuscarFiliado_Click(object sender, ImageClickEventArgs e)
        {
            string StrScript = "";
            string strNumeroDocumento = "";
            Int16 intTipoDocumento = 0;

            strNumeroDocumento = txt_peid_vDocumentoNumero.Text;
            intTipoDocumento = Convert.ToInt16(ddl_peid_sDocumentoTipoId.SelectedValue);

            if (strNumeroDocumento == "")
            {
                txt_peid_vDocumentoNumero.Style.Add("border", "solid red 1px");
                //Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Ingrese el Número de documento."));
                StrScript = @"$(function(){{
                            MoveTabIndex(4);
                        }});";
                StrScript = string.Format(StrScript);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex4", StrScript, true);
                //ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", "Ingrese el Número de documento", true);
                return;
            }
            else {
                txt_peid_vDocumentoNumero.Style.Add("border", "solid #888888 1px");
            }
            if (intTipoDocumento == 0)
            {
                //Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, strNombreEntidad, "Ingrese el Tipo de Documento."));
                ddl_peid_sDocumentoTipoId.Style.Add("border", "solid red 1px");
                StrScript = @"$(function(){{
                            MoveTabIndex(4);
                        }});";
                StrScript = string.Format(StrScript);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex4", StrScript, true);
                //ScriptManager.RegisterStartupScript(Page, typeof(Page), "alerta", "Ingrese Tipo de Documento", true);
                return;
            }
            else {
                ddl_peid_sDocumentoTipoId.Style.Add("border", "solid #888888 1px");
            }
            PersonaConsultaBL lPersonaConsultaBL = new PersonaConsultaBL();
            DataTable _dtPersona = lPersonaConsultaBL.ObtenerDatosPersonaFiliacion(intTipoDocumento, strNumeroDocumento);


            if (_dtPersona.Rows.Count > 0)
            {
                ddl_pers_sNacionalidadId.SelectedValue = _dtPersona.Rows[0]["pers_sNacionalidadId"].ToString();
                txt_pers_vNombres.Text = _dtPersona.Rows[0]["pers_vNombres"].ToString();
                txt_pers_vApellidoPaterno.Text = _dtPersona.Rows[0]["pers_vApellidoPaterno"].ToString();
                txt_pers_vApellidoMaterno.Text = _dtPersona.Rows[0]["pers_vApellidoMaterno"].ToString();


                ddl_pers_sNacionalidadId.Enabled = false;
                txt_pers_vNombres.Enabled = false;
                txt_pers_vApellidoPaterno.Enabled = false;
                txt_pers_vApellidoMaterno.Enabled = false;

            }
            else {
                ddl_pers_sNacionalidadId.Enabled = true;
                txt_pers_vNombres.Enabled = true;
                txt_pers_vApellidoPaterno.Enabled = true;
                txt_pers_vApellidoMaterno.Enabled = true;

                txt_pers_vNombres.Text = "";
                txt_pers_vApellidoPaterno.Text = "";
                txt_pers_vApellidoMaterno.Text = "";
                ddl_pers_sNacionalidadId.SelectedIndex = 0;
            }


            ViewState["PersonaFiliacion"] = _dtPersona;

            string StrScript2 = @"$(function(){{
                            MoveTabIndex(4);
                        }});";
            StrScript2 = string.Format(StrScript2);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex4", StrScript2, true);
        }


        private bool ValidarDireccion()
        {

            string strDireccion = TxtDirDir.Text.Trim().ToUpper();

            TxtDirDir.Style.Add("border", "solid #888888 1px");
            CmbTipRes.Style.Add("border", "solid #888888 1px");
            CmbDptoContDir.Style.Add("border", "solid #888888 1px");
            CmbProvPaisDir.Style.Add("border", "solid #888888 1px");
            CmbDistCiuDir.Style.Add("border", "solid #888888 1px");

            if (strDireccion.Length == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Registro de la Dirección", "Se debe registrar la dirección."));
                TxtDirDir.Style.Add("border", "solid red 1px");
                TxtDirDir.Focus();
                return false;
            }
            if (CmbTipRes.SelectedIndex == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Registro de la Dirección", "Se debe seleccionar el tipo de residencia."));
                CmbTipRes.Style.Add("border", "solid red 1px");
                CmbTipRes.Focus();
                return false;
            }
            if (CmbDptoContDir.SelectedIndex == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Registro de la Dirección", "Se debe seleccionar el Dpto./Continente."));
                CmbDptoContDir.Style.Add("border", "solid red 1px");
                CmbDptoContDir.Focus();
                return false;
            }
            if (CmbProvPaisDir.SelectedIndex == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Registro de la Dirección", "Se debe seleccionar Prov./País."));
                CmbProvPaisDir.Style.Add("border", "solid red 1px");
                CmbProvPaisDir.Focus();
                return false;
            }
            if (CmbDistCiuDir.SelectedIndex == 0)
            {
                Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Registro de la Dirección", "Se debe seleccionar Dist./Ciudad/Estado"));
                CmbDistCiuDir.Style.Add("border", "solid red 1px");
                CmbDistCiuDir.Focus();
                return false;
            }
            //---------------------------------------------------
            //Fecha: 25/02/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Validar si ya se registro la dirección.
            //---------------------------------------------------
            bool bNuevo = false;
            bool bDireccionModificada = false;
            
            if (Convert.ToBoolean(Session["iOperDir"]))
            {
                bNuevo = true;
            }
            else
            {
                if (((DataTable)Session["DtRegDirecciones"]).Rows[Convert.ToInt32(Session["iIndexGrdDirecciones"])]["vResidenciaDireccion"].ToString().Trim().ToUpper() != strDireccion)
                {
                    bDireccionModificada = true;
                }
            }
            if (bNuevo || bDireccionModificada)
            {
                DataTable dt = new DataTable();
                dt = (DataTable)Session["DtRegDirecciones"];
                bool bExiste = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["vResidenciaDireccion"].ToString().Trim().ToUpper() == strDireccion)
                    {
                        bExiste = true;
                        break;
                    }
                }
                if (bExiste)
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Registro de la Dirección", "Ya existe la dirección."));
                    TxtDirDir.Style.Add("border", "solid red 1px");
                    TxtDirDir.Focus();
                    return false;
                }
            }

            //---------------------------------------------------
            return true;
        }

        protected void ddlContinente_Nacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            //---------------------------------------------------
            //Fecha: 03/04/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Asignar el ViewState a DataTable Paises.
            DataTable dtPaises = new DataTable();

            dtPaises = (DataTable)ViewState["Paises"];
            //---------------------------------------------------
            //lblNacionalidadVigente.Text = Comun.AsignarGentilicio(Session, ddlPais_Nacion, CmbGenero).ToUpper();
            lblNacionalidadVigente.Text = Comun.AsignarGentilicio(dtPaises, ddlPais_Nacion, CmbGenero).ToUpper();
        }

        protected void btnREGISTRAR_NACIONALIDAD_Click(object sender, EventArgs e)
        {
            try {
                PersonaMantenimientoBL oPersonaMantenimientoBL = new PersonaMantenimientoBL();
                Int64 iPersonaID = Convert.ToInt64(ViewState["iPersonaId"]);
                Int16 iUsuario = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                int resultado = oPersonaMantenimientoBL.RegistrarNacionalidad(iPersonaID, Convert.ToInt16(ddlPais_Nacion.SelectedValue), lblNacionalidadVigente.Text, chkNacVigente.Checked, "A", iUsuario);
                if (chkNacVigente.Checked)
                {
                    if (resultado > -1)
                    {
                        lblNacionalidadVigenteCabecera.Text = lblNacionalidadVigente.Text;
                        UpdDatosPersona.Update();
                    }

                }
                LimpiarDatosNacionalidad();

                string strScriptCorreo = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "ENVÍO CORREO", "Se registro correctamente");

                Comun.EjecutarScript(Page, strScriptCorreo);
                ListarNacionalidades(iPersonaID);
                UpdDatosPersona.Update();
                
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

        private void LimpiarDatosNacionalidad()
        {
            ddlPais_Nacion.SelectedIndex = 0;
            lblNacionalidadVigente.Text = "[Sin Nacionalidad]";
            //-----------------------------------------------------------------
            //Fecha: 20/04/2021
            //Autor: Miguel Márquez Beltrán
            //Motivo: Cambiar por defecto el check de Nacionalidad a vigente.
            //-----------------------------------------------------------------
            chkNacVigente.Checked = true;
            //-----------------------------------------------------------------
        }

        private void ListarNacionalidades(long LonPersonaId)
        {
            DataTable dtNacionalidades = new DataTable();

            PersonaConsultaBL lPersonaConsultaBL = new PersonaConsultaBL();
            dtNacionalidades = lPersonaConsultaBL.PersonaListarNacionalidades(LonPersonaId);

            gdvNacionalidad.DataSource = dtNacionalidades;
            gdvNacionalidad.DataBind();

            updNacionalidad.Update();
        }
        
        protected void gdvNacionalidad_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int RowIndex = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {           
                case "Eliminar":
                    PersonaMantenimientoBL oPersonaMantenimientoBL = new PersonaMantenimientoBL();

                    Int64 iPersonaId = Convert.ToInt64(gdvNacionalidad.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].Text);
                    Int16 iPais = Convert.ToInt16(gdvNacionalidad.Rows[Convert.ToInt32(e.CommandArgument)].Cells[1].Text);
                    string sVigente = Convert.ToString(gdvNacionalidad.Rows[Convert.ToInt32(e.CommandArgument)].Cells[3].Text);
                    Int16 iUsuario = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);

                    if (sVigente == "VIGENTE")
                    {
                        string strScriptCorreo = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "NACIONALIDAD", "No se puede eliminar una Nacionalidad Vigente, Primero registre otra nacionalidad Vigente");
                        Comun.EjecutarScript(Page, strScriptCorreo);
                    }
                    else {
                        int error = oPersonaMantenimientoBL.EliminarNacionalidad(iPersonaId, iPais, iUsuario);
                        if (error == -1)
                        {
                            string strScriptCorreo = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "NACIONALIDAD", "Ocurrió un error inesperado");
                            Comun.EjecutarScript(Page, strScriptCorreo);
                        }
                    }
                    

                    
                    ListarNacionalidades(iPersonaId);
                    break;
            }
        }

        protected void ddlDptContinenteResidencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDptContinenteResidencia.SelectedIndex > 0)
            {
                ddlProvPaisResidencia.Enabled = true;

                //Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.PROVINCIA_PAIS, ddlDptContinenteResidencia.SelectedValue, "", "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, ddlProvPaisResidencia);
                if (ViewState["Ubigeo"] != null)
                {
                    beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                    obeUbigeoListas = (beUbigeoListas)ViewState["Ubigeo"];
                    List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                    lbeUbicaciongeografica = Comun.obtenerListaUbiGeo("02", ddlDptContinenteResidencia.SelectedValue, "", obeUbigeoListas.Ubigeo02);
                    lbeUbicaciongeografica.Insert(0, new beUbicaciongeografica { Ubi02 = "00", Provincia = "-- SELECCIONE --" });
                    ddlProvPaisResidencia.DataSource = lbeUbicaciongeografica;
                    ddlProvPaisResidencia.DataValueField = "Ubi02";
                    ddlProvPaisResidencia.DataTextField = "Provincia";
                    ddlProvPaisResidencia.DataBind();
                }
                ddlProvPaisResidencia.Focus();
            }
            else
            {
                this.ddlProvPaisResidencia.Items.Clear();
                this.ddlProvPaisResidencia.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));

            }
            this.ddlDistCiudadResidencia.Items.Clear();
            this.ddlDistCiudadResidencia.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
        }

        protected void ddlProvPaisResidencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProvPaisResidencia.SelectedIndex > 0)
            {
                ddlDistCiudadResidencia.Enabled = true;

                if (ViewState["Ubigeo"] != null)
                {
                    beUbigeoListas obeUbigeoListas = new beUbigeoListas();
                    obeUbigeoListas = (beUbigeoListas)ViewState["Ubigeo"];
                    List<beUbicaciongeografica> lbeUbicaciongeografica = new List<beUbicaciongeografica>();
                    lbeUbicaciongeografica = Comun.obtenerListaUbiGeo("03", ddlDptContinenteResidencia.SelectedValue, ddlProvPaisResidencia.SelectedValue, obeUbigeoListas.Ubigeo03);
                    lbeUbicaciongeografica.Insert(0, new beUbicaciongeografica { Ubi03 = "00", Distrito = "-- SELECCIONE --" });
                    ddlDistCiudadResidencia.DataSource = lbeUbicaciongeografica;
                    ddlDistCiudadResidencia.DataValueField = "Ubi03";
                    ddlDistCiudadResidencia.DataTextField = "Distrito";
                    ddlDistCiudadResidencia.DataBind();
                    ddlDistCiudadResidencia.Enabled = (ddlProvPaisResidencia.SelectedValue.Equals("00") ? false : true);
                    ddlDistCiudadResidencia.Focus();
                }
                //Comun.CargarUbigeo(Enumerador.enmTipoUbigeo.DISTRITO_CIUD, ddlDptContinenteResidencia.SelectedValue, ddlProvPaisResidencia.SelectedValue, "", true, "--SELECCIONAR--", "", Enumerador.enmNacionalidad.NINGUNA, ddlDistCiudadResidencia);

                //Session["CmbProvPaisDir"] = ddlProvPaisResidencia.SelectedValue;

                ddlProvPaisResidencia.Focus();
            }
            else
            {
                this.ddlDistCiudadResidencia.Items.Clear();
                this.ddlDistCiudadResidencia.Items.Insert(0, new ListItem("- SELECCIONAR -", "00"));
            }
        }

        protected void lkbTramite_Click(object sender, EventArgs e)
        {
            string codPersona = Request.QueryString["CodPer"].ToString();
            if (Request.QueryString["Juridica"] != null) // SI ES PERSONA JURIDICA
            {
                Response.Redirect("FrmTramite.aspx?CodPer=" + codPersona + "&Juridica=1", false);
            }
            else
            { // PERSONA NATURAL
                string codTipoDocEncriptada = "";
                string codNroDocumentoEncriptada = "";

                if (Request.QueryString["CodTipoDoc"] != null && Request.QueryString["codNroDoc"] != null)
                {
                    codTipoDocEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["CodTipoDoc"].ToString());
                    codNroDocumentoEncriptada = Sanitizer.GetSafeHtmlFragment(Request.QueryString["codNroDoc"].ToString());
                }
                if (codTipoDocEncriptada.Length > 0 && codNroDocumentoEncriptada.Length > 0)
                {
                    Response.Redirect("FrmTramite.aspx?CodPer=" + codPersona + "&CodTipoDoc=" + codTipoDocEncriptada + "&codNroDoc=" + codNroDocumentoEncriptada, false);
                }
                else
                {
                    Response.Redirect("FrmTramite.aspx?CodPer=" + codPersona, false);
                }
            }
            
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

                    ViewState["DtPersonaAct"] = dt;
                }

                dt = null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnLimpiarFiliacion_Click(object sender, EventArgs e)
        {
            ViewState["PersonaFiliacion"] = null;
            txt_pers_vNombres.Text = "";
            txt_pers_vApellidoPaterno.Text = "";
            txt_pers_vApellidoMaterno.Text = "";
            ddl_pers_sNacionalidadId.SelectedIndex = 0;
            string StrScript2 = @"$(function(){{
                            MoveTabIndex(4);
                        }});";
            StrScript2 = string.Format(StrScript2);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "MoveTabIndex4", StrScript2, true);
            return;
        }

        protected void ImprimirConstancia(object sender, EventArgs e)
        {
            try
            {
                ViewState.Add("AccionDocumento", "Nuevo");

                PersonaConsultaBL objPersonaBL = new PersonaConsultaBL();
                DataSet dsRune = new DataSet();

                string nombrereporte = string.Empty;
                string ruta = string.Empty;

                BE.RE_PERSONA objBE = new BE.RE_PERSONA();

                objBE.pers_iPersonaId = Convert.ToInt64(ViewState["iPersonaId"]);

                dsRune = objPersonaBL.Persona_Imprimir_Rune(objBE);

                if (dsRune.Tables[1].Rows.Count > 0)
                {
                    if (dsRune.Tables[1].Rows[0]["pers_sNacionalidadId"].ToString() == Convert.ToString((Int32)Enumerador.enmNacionalidad.EXTRANJERA))
                    {
                        string StrScript = string.Empty;
                        StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", "Para imprimir la Constancia de Inscripción tiene que ser PERUANO", false, 190, 250);
                        Comun.EjecutarScript(Page, StrScript);
                        return;
                    }
                }

                if (ddlIdiomas.SelectedItem.Text.ToUpper() == "CASTELLANO")
                {
                    Session["strNombreArchivo"] = "crConstanciaInscripcion_Idioma_CAST.rdlc";
                }
                else
                {
                    Session["strNombreArchivo"] = "crConstanciaInscripcion_Idioma_OTR.rdlc";
                }
                Session["DtDatos"] = dsRune;
                Session["REGISTRO_RPT"] = Enumerador.enmRegistroReporte.FILIACION;

                //-------------------------------------------------------------------------
                //Autor: Miguel Márquez Beltrán
                //Fecha: 30/09/2016
                //Objetivo: Crear una sesion para permitir actualizar el formato del RUNE
                //-------------------------------------------------------------------------
                Session["printRUNE"] = "1";
                //-------------------------------------------------------------------------
                Session["MontoLocal"] = ViewState["MLocal"] + ".00";
                Session["SolesConsulares"] = ViewState["MSolesCon"] + ".00";
                string strUrl = "../Registro/FrmReporteRune.aspx";
                string strScript = "window.open('" + strUrl + "', 'popup_window', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=yes,width=750,height=800,left=150,top=10');";

                EjecutarScript(Page, strScript);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void VisibilidadAsteriscos(bool Visible)
        {
            lblCO_CmbEstCiv.Visible = Visible;
            lblCO_CmbGenero.Visible = Visible;
            lblCO_CmbOcupacion.Visible = Visible;
            lblCO_CmbDptoContNac.Visible = Visible;
            lblCO_CmbProvPaisNac.Visible = Visible;
            lblCO_CmbDistCiudadNac.Visible = Visible;
            lblCO_txtDireccionResidencia.Visible = Visible;
            lblCO_DptContinenteResidencia.Visible = Visible;
            lblCO_ProvPaisResidencia.Visible = Visible;
            lblCO_DistCiudadResidencia.Visible = Visible; 
        }

        
        //-------------------------------------------------------
        protected void chkValidarConReniec_change(object sender, EventArgs e)
        { 
            if (chkValidarConReniec.Checked) {
                txtApePat.Enabled=false;
                txtApeMat.Enabled=false;
                txtNombres.Enabled=false;
                btnValidarDni.Enabled = true;
            } else {
                txtApePat.Enabled = true;
                txtApeMat.Enabled = true;
                txtNombres.Enabled = true;
                btnValidarDni.Enabled = false;
            }
        }
        protected void btnConsultarPideReniecDNI_Click(object sender, EventArgs e)
        {
            try
            {
                string NumDNI = txtNroDoc.Text;


                string respuesta = "";
                if (NumDNI.Equals(""))
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Consulta a Reniec", "Falta ingresar el Nro Documento."));
                    return;
                }
                if (Session["dniAnterior"] != null && ((string)Session["dniAnterior"]).Equals(NumDNI) && txtValidacionReniec.Text.Equals("1") && !txtApePat.Text.Equals(""))
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Consulta a Reniec", "Ya ha sido Validado por RENIEC"));
                    return;
                }
                PersonaIdentificacionConsultaBL objPersonasIdBL = new PersonaIdentificacionConsultaBL();
                int IntRpta = objPersonasIdBL.Existe(Convert.ToInt32(CmbTipoDoc.SelectedValue), txtNroDoc.Text.Trim(), 0, 1);

                if (IntRpta == 1)
                {
                    string StrScript = Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.WARNING, "RUNE", "Ya existe el número de documento que esta consignando.", false, 190, 250);
                    StrScript = StrScript + "RecargarUbigeoNacimiento(); RecargarUbigeoResidencia();";
                    Comun.EjecutarScript(Page, StrScript);
                    return;
                }

                WSPIDE_Persona Persona = new WSPIDE_Persona();
                Persona = DatosPide(NumDNI, out respuesta);
                if (Persona.codigoMensajeError.Equals("0000"))
                {
                    txtApePat.Text = Persona.Paterno;
                    txtApeMat.Text = Persona.Materno;
                    txtNombres.Text = Persona.Nombres;
                    //txtDireccionResidencia.Text = Persona.direccion;
                    Session["dniAnterior"] = NumDNI;
                    txtApePat.Enabled = false;
                    txtApeMat.Enabled = false;
                    txtNombres.Enabled = false;
                    txtValidacionReniec.Text = "1";
                }
                else
                {
                    if (Persona.codigoMensajeError == "1003" || Persona.codigoMensajeError == "0002" || Persona.codigoMensajeError == "0003")
                    {   //en caso: couta agutado, no ha comunicacion, timeout se activa los campos para editar y desaparece el boton validar
                        txtApePat.Enabled = true;
                        txtApeMat.Enabled = true;
                        txtNombres.Enabled = true;
                        btnValidarDni.Enabled = false;
                        chkValidarConReniec.Checked = false;
                        txtNroDoc.Attributes.Remove("onchange");

                    }
                    if (Persona.codigoMensajeError == "0001")
                    {   //en caso de menor de edad, se activa los campos para editar 
                        txtApePat.Enabled = true;
                        txtApeMat.Enabled = true;
                        txtNombres.Enabled = true;

                    }
                    txtApePat.Text = "";
                    txtApeMat.Text = "";
                    txtNombres.Text = "";
                    //txtDireccionResidencia.Text = "";
                    txtValidacionReniec.Text = "0";
                    respuesta = WSPIDE_VerificacionError(Persona.codigoMensajeError);
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Consulta a Reniec", respuesta));
                }

                updTabDatAdic.Update();
                UpdDatosPersona.Update();

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
        protected void btnConsultarPideReniecDNI2_Click(object sender, EventArgs e)
        {
            try
            {
                string NumDNI = txtNroDocumentoM.Text;
                
                
                string respuesta = "";
                if (NumDNI.Equals(""))
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Consulta a Reniec", "Falta ingresar el Nro Documento."));
                    return;
                }
                

                WSPIDE_Persona Persona = new WSPIDE_Persona();
                Persona = DatosPide(NumDNI, out respuesta);
                if (Persona.codigoMensajeError.Equals("0000"))
                {
                    if (!txtApePat.Text.Equals(Persona.Paterno) || !txtApeMat.Text.Equals(Persona.Materno) || !txtNombres.Text.Equals(Persona.Nombres))
                    {
                        //Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION, "Consulta a Reniec", "Apellidos o nombres es distinto segun la RENIEN "));
                        String nombres = "";
                        nombres += (!txtApePat.Text.Equals(Persona.Paterno) ? "<tr><td style='background:#ddd;'>Ap.Paterno:</td><td>" + txtApePat.Text + "</td><td>" + Persona.Paterno + "</td></tr>" : "");
                        nombres += (!txtApeMat.Text.Equals(Persona.Materno) ? "<tr><td style='background:#ddd;'>Ap.Materno:</td><td>" + txtApeMat.Text + "</td><td> " + Persona.Materno + "</td></tr>" : "");
                        nombres += (!txtNombres.Text.Equals(Persona.Nombres) ? "<tr><td style='background:#ddd;'>Nombres:</td><td>" + txtNombres.Text + "</td><td> " + Persona.Nombres + "</td></tr>" : "");
                        nombres = nombres.Length > 0 ? "<table border='1' style='border-collapse: collapse;width:100%'><tr style='background:#ddd;'><td>&nbsp;</td><td>En RUNE</td><td>Segun RENIEC</td></tr>" + nombres + "</table>" : "";
                        string modal = "";
                        modal += "$('#msg-dialog').html(\"<table style='width:100%'><tr><td><img src='../images/img_msg_info.png' height='50' width='50' border='0' style='vertical-align:middle;' ></td><td>Los datos no coinciden:<br><br>" + nombres + " <br><br><b>Desea reemplazar?</b> </td></tr></table>\");";
                        modal +="$(\"#msg-dialog\").dialog({";
                        modal +="title: 'Consulta a RENIEC',";
                        modal +="autoOpen: true,";
                        modal += "resizable: false,";
                        modal +="height: 300,";
                        modal +="width: 450,";
                        modal +="modal: true,";
                        modal +="buttons: {";
                        modal +="'Si': function () {";
                        modal +="$('#MainContent_txtApePat').val('" + Persona.Paterno + "');";
                        modal += "$('#MainContent_txtApeMat').val('" + Persona.Materno + "');";
                        modal += "$('#MainContent_txtNombres').val('" + Persona.Nombres + "');";
                        modal += "$('#MainContent_txtValidacionReniec').val('1');";
                        modal += "$(this).dialog('close');";
                        modal +="},";
                        modal +="'No':function(){ $(this).dialog('close'); }";
                        modal +="}";
                        modal += "});";
                        ScriptManager.RegisterClientScriptBlock(this, GetType(), "__Mensagem" + DateTime.Now.Ticks.ToString(), modal, true);
                        
                    }
                }
                else
                {
                    respuesta = WSPIDE_VerificacionError(Persona.codigoMensajeError);
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "Consulta a Reniec", respuesta));
                }

                //updTabDatAdic.Update();
                //UpdDatosPersona.Update();
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
        public WSPIDE_Persona DatosPide(string numero, out string resultado)
        {
            WSPIDE_Persona dato =  new WSPIDE_Persona();;
            try
            {
                

                System.Diagnostics.Debug.Write("\n__" + UIEncripto.EncriptarCadena("45537460"));
                System.Diagnostics.Debug.Write("\n__" + UIEncripto.EncriptarCadena("45626110"));
                System.Diagnostics.Debug.Write("\n__" + UIEncripto.EncriptarCadena("42262598")+"\n");

                using (WSPideReniec.ReniecConsultaDniPortTypeClient serviceAU = new WSPideReniec.ReniecConsultaDniPortTypeClient())
                {
                    var urlProxy = System.Configuration.ConfigurationManager.AppSettings["URLPROXY"];
                    var dominioProxy = System.Configuration.ConfigurationManager.AppSettings["USRPROXYDOMINIO"];
                    var userProxy = System.Configuration.ConfigurationManager.AppSettings["USRPROXYUSER"];
                    var pwdProxy = System.Configuration.ConfigurationManager.AppSettings["USRPROXYCLAVE"];

                    var userPIDE = System.Configuration.ConfigurationManager.AppSettings["IDUSERTICKET"];
                    var pwdPIDE = System.Configuration.ConfigurationManager.AppSettings["IDPWDTICKET"];
                    var rucPIDE = System.Configuration.ConfigurationManager.AppSettings["IDRUCTICKET"];

                    string userPIDE_1 = System.Configuration.ConfigurationManager.AppSettings["IDUSERTICKET_1"];
                    string pwdPIDE_1 = System.Configuration.ConfigurationManager.AppSettings["IDPWDTICKET_1"];

                    string userPIDE_2 = System.Configuration.ConfigurationManager.AppSettings["IDUSERTICKET_2"];
                    string pwdPIDE_2 = System.Configuration.ConfigurationManager.AppSettings["IDPWDTICKET_2"];

                    string userPIDE_3 = System.Configuration.ConfigurationManager.AppSettings["IDUSERTICKET_3"];
                    string pwdPIDE_3 = System.Configuration.ConfigurationManager.AppSettings["IDPWDTICKET_3"];

                    string userPIDE_4 = System.Configuration.ConfigurationManager.AppSettings["IDUSERTICKET_4"];
                    string pwdPIDE_4 = System.Configuration.ConfigurationManager.AppSettings["IDPWDTICKET_4"];

                    string userPIDE_5 = System.Configuration.ConfigurationManager.AppSettings["IDUSERTICKET_5"];
                    string pwdPIDE_5 = System.Configuration.ConfigurationManager.AppSettings["IDPWDTICKET_5"];

                    string userPIDE_6 = System.Configuration.ConfigurationManager.AppSettings["IDUSERTICKET_6"];
                    string pwdPIDE_6 = System.Configuration.ConfigurationManager.AppSettings["IDPWDTICKET_6"];

                    userPIDE = UIEncripto.DesEncriptarCadena(userPIDE);
                    pwdPIDE = UIEncripto.DesEncriptarCadena(pwdPIDE);
                    rucPIDE = UIEncripto.DesEncriptarCadena(rucPIDE);

                    userPIDE_1 = UIEncripto.DesEncriptarCadena(userPIDE_1);
                    pwdPIDE_1 = UIEncripto.DesEncriptarCadena(pwdPIDE_1);

                    userPIDE_2 = UIEncripto.DesEncriptarCadena(userPIDE_2);
                    pwdPIDE_2 = UIEncripto.DesEncriptarCadena(pwdPIDE_2);

                    userPIDE_3 = UIEncripto.DesEncriptarCadena(userPIDE_3);
                    pwdPIDE_3 = UIEncripto.DesEncriptarCadena(pwdPIDE_3);

                    WSPideReniec.peticionConsulta ws1 = new WSPideReniec.peticionConsulta();
                    ws1.nuDniConsulta = numero;
                    ws1.nuDniUsuario = userPIDE;
                    ws1.nuRucUsuario = rucPIDE;
                    ws1.password = pwdPIDE;
                    var result = serviceAU.consultar(ws1);
                    
                    dato.codigoMensajeError = result.coResultado;
                    if (dato.codigoMensajeError == "1003" || dato.codigoMensajeError == "1999")
                    {
                        // Buscamos usando primero usuario
                        ws1.nuDniConsulta = numero;
                        ws1.nuDniUsuario = userPIDE_1;
                        ws1.nuRucUsuario = rucPIDE;
                        ws1.password = pwdPIDE_1;
                        result = serviceAU.consultar(ws1);
                        dato = new WSPIDE_Persona();
                        dato.codigoMensajeError = result.coResultado;
                        if (dato.codigoMensajeError == "1003" || dato.codigoMensajeError == "1999")
                        {
                            // Buscamos usando segundo usuario
                            ws1.nuDniConsulta = numero;
                            ws1.nuDniUsuario = userPIDE_2;
                            ws1.nuRucUsuario = rucPIDE;
                            ws1.password = pwdPIDE_2;
                            result = serviceAU.consultar(ws1);
                            dato = new WSPIDE_Persona();
                            dato.codigoMensajeError = result.coResultado;
                            if (dato.codigoMensajeError == "1003" || dato.codigoMensajeError == "1999")
                            {
                                // Buscamos usando tercer usuario
                                ws1.nuDniConsulta = numero;
                                ws1.nuDniUsuario = userPIDE_3;
                                ws1.nuRucUsuario = rucPIDE;
                                ws1.password = pwdPIDE_3;
                                result = serviceAU.consultar(ws1);
                                dato = new WSPIDE_Persona();
                                dato.codigoMensajeError = result.coResultado;
                                if (dato.codigoMensajeError == "1003" || dato.codigoMensajeError == "1999")
                                {
                                    // Buscamos usando CUARTO usuario
                                    ws1.nuDniConsulta = numero;
                                    ws1.nuDniUsuario = userPIDE_4;
                                    ws1.nuRucUsuario = rucPIDE;
                                    ws1.password = pwdPIDE_4;
                                    result = serviceAU.consultar(ws1);
                                    dato = new WSPIDE_Persona();
                                    dato.codigoMensajeError = result.coResultado;
                                    if (dato.codigoMensajeError == "1003" || dato.codigoMensajeError == "1999")
                                    {
                                        // Buscamos usando QUINTO usuario
                                        ws1.nuDniConsulta = numero;
                                        ws1.nuDniUsuario = userPIDE_5;
                                        ws1.nuRucUsuario = rucPIDE;
                                        ws1.password = pwdPIDE_5;
                                        result = serviceAU.consultar(ws1);
                                        dato = new WSPIDE_Persona();
                                        dato.codigoMensajeError = result.coResultado;
                                        if (dato.codigoMensajeError == "1003" || dato.codigoMensajeError == "1999")
                                        {
                                            // Buscamos usando SEXTO usuario
                                            ws1.nuDniConsulta = numero;
                                            ws1.nuDniUsuario = userPIDE_6;
                                            ws1.nuRucUsuario = rucPIDE;
                                            ws1.password = pwdPIDE_6;
                                            result = serviceAU.consultar(ws1);
                                            dato = new WSPIDE_Persona();
                                            dato.codigoMensajeError = result.coResultado;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    resultado = "";
                    if (result.coResultado == "0000")
                    {
                        dato.Nombres = result.datosPersona.prenombres;
                        dato.Paterno = result.datosPersona.apPrimer;
                        dato.Materno = result.datosPersona.apSegundo;
                        dato.codigoMensajeError = result.coResultado;
                        dato.ubigeo = result.datosPersona.ubigeo;
                        dato.direccion = result.datosPersona.direccion;
                        dato.foto = result.datosPersona.foto;
                    }
                    else
                    {
                        dato.codigoMensajeError = result.coResultado;
                        dato.MensajeError = result.deResultado;
                    }
                }
            }
            catch (System.ServiceModel.CommunicationException tae)
            {
                dato.codigoMensajeError = "0002";
            }
            catch (TimeoutException tae)
            {
                dato.codigoMensajeError = "0003";
            }
            catch (Exception fe)
            {
                dato.codigoMensajeError = "1999";
            }
            finally {
                resultado = "";
            }
            return dato;
        }

        public string WSPIDE_VerificacionError(string resultado)
        {
            string msg = string.Empty;
            switch (resultado)
            {
                case "0001": msg = "PIDE: El número de DNI corresponde a un menor de edad"; break;
                case "0999": msg = "PIDE: No se ha encontrado información para el número de DNI"; break;
                case "1000": msg = "PIDE: Uno o más datos de la petición no son válidos"; break;
                case "1001": msg = "PIDE: El DNI, RUC y contraseña no corresponden a un usuario válido"; break;
                case "1002": msg = "PIDE: La contraseña para el DNI y RUC está caducada"; break; 
                case "1003": msg = "PIDE: Se ha alcanzado el límite de consultas permitidas por día"; break;
                case "1999": msg = "PIDE: Error desconocido / inesperado"; break;
                case "0002": msg = "No hay comunicación con la RENIEC, por favor vuelva a intentar mas tarde"; break;
                case "0003": msg = "La comunicación con la RENIEC está tardando mas de lo normal, por favor vuelva a intentar mas tarde"; break;
                default: msg = "1"; break;
            }
            return msg;
        }

     }




    public static class StringExtension
    {
        public static string EncodeString(this string cadena)
        {
            return cadena.Replace("&nbsp;", "");
        }

        public static string EncodeNumeric(this string cadena)
        {
            return cadena.Replace("&nbsp;", "0");
        }

        public static string EncodeNumeric2(this string cadena)
        {
            return cadena.Replace("&nbsp;", "00");
        }

        public static string EncodeBooleano(this string cadena)
        {
            return cadena.Replace("&nbsp;", "0");
        }



        //------------------------------------------------
    }

}
