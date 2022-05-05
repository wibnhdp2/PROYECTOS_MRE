using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.WebApp.Accesorios;
using SGAC.BE;
using SGAC.Accesorios;
using System.Data;
using SGAC.Registro.Actuacion.BL;
using SGAC.Controlador;

namespace SGAC.WebApp.Registro
{
    public partial class FrmRegistroBeneficiario : MyBasePage
    {
        private string srtBeneficiario = "BENEFICIARIO";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Util.CargarParametroDropDownList(ddl_Genero, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_GENERO), true);
                Util.CargarParametroDropDownList(ddl_TipoDocumento, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_CIVIL_PARTICIPANTE_NACIMIENTO), true);

                DataTable dtTipDoc = new DataTable();
                dtTipDoc = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.DOCUMENTO_IDENTIDAD);
                DataView dv = dtTipDoc.DefaultView;
                DataTable dtOrdenado = dv.ToTable();
                dtOrdenado.DefaultView.Sort = "Id ASC";
                Util.CargarDropDownList(ddl_TipoDocumento, dtOrdenado, "Valor", "Id", true);

                this.ddl_TipoDocumento.Items.Insert(6, new System.Web.UI.WebControls.ListItem("CUI", Convert.ToString(Constantes.CONST_EXCEPCION_CUI_ID)));


                Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session["BeneficiarioAccion"];
                switch (enmAccion)
                {
                    case Enumerador.enmAccion.INSERTAR:
                        btnAdicionar.Text = "Adicionar";
                        Limpiar_buscaBeneficiario();
                        break;
                    case Enumerador.enmAccion.MODIFICAR:
                        btnAdicionar.Text = "Modificar";
                        DataTable dt = (DataTable)Session[srtBeneficiario];
                        DataRow dr = dt.Rows[Comun.ToNullInt32(Request.QueryString["vClass"])];


                        ddl_TipoDocumento.SelectedValue = dr["asbe_sDocumentoTipoId"].ToString();
                        txtDocumento.Text = dr["asbe_vDocumentoNumero"].ToString().Trim();
                        txtApellidoPatBene.Text = dr["asbe_vApellidoPaterno"].ToString();
                        txtApellidoMatBene.Text = dr["asbe_vApellidoMaterno"].ToString().Trim();
                        txtNombreBene.Text = dr["asbe_vNombres"].ToString().Trim();
                        ddl_Genero.SelectedValue = dr["asbe_sGeneroId"].ToString();

                        txtApellidoPatBene.Enabled = true;
                        txtApellidoMatBene.Enabled = true;
                        txtNombreBene.Enabled = true;
                        ddl_Genero.Enabled = true;

                        break;
                }


                HF_ValoresDocumentoIdentidad.Value = string.Empty;

                DataTable dtDoc = new DataTable();

                dtDoc = Comun.ObtenerListaDocumentoIdentidad();
                
                //DataTable dtDoc = (DataTable)Session[Constantes.CONST_SESION_DT_DOCUMENTOIDENTIDAD];

                foreach (DataRow dr in dtDoc.Rows)
                {
                    HF_ValoresDocumentoIdentidad.Value += dr["doid_sTipoDocumentoIdentidadId"].ToString() + "," +
                        dr["doid_sDigitosMinimo"].ToString() + "," + dr["doid_sDigitos"].ToString() + "," +
                     dr["doid_bNumero"].ToString() + "," + dr["doid_sTipoNacionalidad"].ToString() + "," +
                     dr["vMensajeError"].ToString() + "|";
                }
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Session.Add("BeneficiarioAccion", Enumerador.enmAccion.INSERTAR);
            btnAdicionar.Text = "Adicionar";
            Limpiar_buscaBeneficiario();

        }

        private void Limpiar_buscaBeneficiario()
        {
            ddl_TipoDocumento.SelectedIndex = -1;
            txtDocumento.Text = string.Empty;
            txtNombreBene.Text = string.Empty;
            txtApellidoPatBene.Text = string.Empty;
            txtApellidoMatBene.Text = string.Empty;
            txtNombreBene.Enabled = false;
            txtApellidoPatBene.Enabled = false;
            txtApellidoMatBene.Enabled = false;
            ddl_Genero.SelectedIndex = -1;
        }

        protected void ddl_TipoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDocumento.Text = string.Empty;

            DataTable dtDocumentoIdentidad = new DataTable();

            dtDocumentoIdentidad = Comun.ObtenerListaDocumentoIdentidad();
            
            //DataTable dtDocumentoIdentidad = (DataTable)Session[Constantes.CONST_SESION_DT_DOCUMENTOIDENTIDAD];


            foreach (DataRow fila in dtDocumentoIdentidad.Rows)
            {
                if (fila["doid_sTipoDocumentoIdentidadId"].ToString() == ddl_TipoDocumento.SelectedValue.ToString())
                {
                    int iMaxLenght = 0;

                    if (!String.IsNullOrEmpty(fila["doid_bNumero"].ToString()))
                    {
                        bool bNumero = Convert.ToBoolean(fila["doid_bNumero"]);
                        hidDocumentoSoloNumero.Value = bNumero ? "1" : "0";
                    }

                    if (!String.IsNullOrEmpty(fila["doid_sDigitos"].ToString()))
                    {
                        iMaxLenght = Convert.ToInt16(fila["doid_sDigitos"]);                        
                    }

                    txtDocumento.MaxLength = iMaxLenght;

                 
                    break;
                }

            }
        }

        protected void imgBuscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                
                buscarRepresentante(new EnPersona());
            }
            catch (Exception ex)
            {
                Session["_LastException"] = ex;
                Response.Redirect("../PageError/GenericErrorPage.aspx");
            }
        }

        protected void btnAdicionar_Click(object sender, EventArgs e)
        {
            string StrScript = string.Empty;
            ActoCivilConsultaBL funActoCivil = new ActoCivilConsultaBL();
            Object[] miArrayPersIdentifBus = null;

            Proceso MiProc = new Proceso();

            int IntRpta = 0;

            try
            {
                Enumerador.enmAccion enmAccion = (Enumerador.enmAccion)Session["BeneficiarioAccion"];
                int index = Convert.ToInt32(Session["IndexBeneficiario"]);
                DataTable dt = (DataTable)Session[srtBeneficiario];

                switch (enmAccion)
                {
                    case Enumerador.enmAccion.INSERTAR:

                        funActoCivil = new ActoCivilConsultaBL();
                        miArrayPersIdentifBus = new Object[4] { Convert.ToInt32(ddl_TipoDocumento.SelectedValue),
                                                                         txtDocumento.Text.Trim(),
                                                                         Comun.ToNullInt32(this.hdn_actu_iPersonaId.Value),
                                                                         2};

                
                        IntRpta = (int)MiProc.Invocar(ref miArrayPersIdentifBus,
                                                                            "SGAC.BE.RE_PERSONAIDENTIFICACION",
                                                                            Enumerador.enmAccion.BUSCAR,
                                                                            Enumerador.enmAplicacion.WEB);

                        if (IntRpta == 1)
                        {
                            StrScript = "alert('Ya existe el número de documento que esta consignando.')";
                            Comun.EjecutarScript(Page, StrScript);
                            return;
                        }


                        # region Verifica si existe Beneficiario
                        foreach (DataRow row in dt.Rows)
                        {
                            if (Convert.ToInt16(row["asbe_sDocumentoTipoId"].ToString()) == Convert.ToInt16(ddl_TipoDocumento.SelectedValue))
                            {
                                if (row["asbe_vDocumentoNumero"].ToString().ToUpper() == txtDocumento.Text.Trim().ToUpper())
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('El Beneficiario ya está ingresado');", true);
                                    return;
                                }
                            }

                            if (row["asbe_iPersonaId"].ToString() == hdn_actu_iPersonaId.Value.ToString()
                                && hdn_actu_iPersonaId.Value.ToString() != "0")
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('El Beneficiario ya está ingresado con otro tipo de documento');", true);
                                return;
                            }
                        }
                        # endregion

                        DataRow newrow;
                        newrow = dt.NewRow();
                        newrow["asbe_iAsistenciaBeneficiarioId"] = 0;
                        newrow["asbe_iAsistenciaId"] = 0;
                        newrow["asbe_iPersonaId"] = this.hdn_actu_iPersonaId.Value;
                        newrow["asbe_sDocumentoTipoId"] = Convert.ToInt16(ddl_TipoDocumento.SelectedValue);
                        newrow["asbe_vNombreDocumento"] = ddl_TipoDocumento.SelectedItem.Text.Trim();
                        newrow["asbe_vDocumentoNumero"] = txtDocumento.Text.Trim();
                        newrow["asbe_vApellidoPaterno"] = txtApellidoPatBene.Text.Trim();
                        newrow["asbe_vApellidoMaterno"] = txtApellidoMatBene.Text.Trim();
                        newrow["asbe_vNombres"] = txtNombreBene.Text.Trim();
                        newrow["asbe_fMonto"] = 0;
                        newrow["asbe_ISolicitante"] = 0;
                        newrow["asbe_sGeneroId"] = Convert.ToInt16(ddl_Genero.SelectedValue);
                        newrow["Genero"] = ddl_Genero.SelectedItem.Text.Trim();

                        dt.Rows.Add(newrow);
                        this.hdn_actu_iPersonaId.Value = "0";
                        break;

                    case Enumerador.enmAccion.MODIFICAR:
                        int n = 0;

                        funActoCivil = new ActoCivilConsultaBL();
                        
                        miArrayPersIdentifBus = new Object[4] { Convert.ToInt32(ddl_TipoDocumento.SelectedValue),
                                                                         txtDocumento.Text.Trim(),
                                                                         Comun.ToNullInt32(this.hdn_actu_iPersonaId.Value),
                                                                         2};

                
                        IntRpta = (int)MiProc.Invocar(ref miArrayPersIdentifBus,
                                                                            "SGAC.BE.RE_PERSONAIDENTIFICACION",
                                                                            Enumerador.enmAccion.BUSCAR,
                                                                            Enumerador.enmAplicacion.WEB);

                        if (IntRpta == 1)
                        {
                            StrScript = "alert('Ya existe el número de documento que esta consignando.')";
                            Comun.EjecutarScript(Page, StrScript);
                            return;
                        }

                        foreach (DataRow row in dt.Rows)
                        {

                            if (Convert.ToInt32(Session["IndexBeneficiario"]) != n)
                            {
                                if (Convert.ToInt16(row["asbe_sDocumentoTipoId"].ToString()) == Convert.ToInt16(ddl_TipoDocumento.SelectedValue))
                                {
                                    if (row["asbe_vDocumentoNumero"].ToString() == txtDocumento.Text.Trim())
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", " alert('El Beneficiario ya existe');", true);
                                        return;
                                    }
                                }
                            }
                            n++;
                        }

                        dt.Rows[index]["asbe_sDocumentoTipoId"] = Convert.ToInt16(ddl_TipoDocumento.SelectedValue);
                        dt.Rows[index]["asbe_vNombreDocumento"] = ddl_TipoDocumento.SelectedItem.Text.Trim();
                        dt.Rows[index]["asbe_vDocumentoNumero"] = txtDocumento.Text.Trim();
                        dt.Rows[index]["asbe_vApellidoPaterno"] = txtApellidoPatBene.Text.Trim();
                        dt.Rows[index]["asbe_vApellidoMaterno"] = txtApellidoMatBene.Text.Trim();
                        dt.Rows[index]["asbe_vNombres"] = txtNombreBene.Text.Trim();
                        dt.Rows[index]["asbe_sGeneroId"] = Convert.ToInt16(ddl_Genero.SelectedValue);
                        dt.Rows[index]["Genero"] = ddl_Genero.SelectedItem.Text.Trim();
                        break;
                }

                dt.AcceptChanges();

                

                Session[srtBeneficiario] = dt;
                

                Session.Add("BeneficiarioAccion", Enumerador.enmAccion.INSERTAR);

                Comun.EjecutarScript(Page, "window.parent.close_ModalPopup('MainContent_btnRealizarBusqueda')");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void buscarRepresentante(EnPersona objEn)
        {
            try
            {
                #region Buscar persona
                if (objEn.iPersonaId == 0)
                {
                    if (ddl_TipoDocumento.SelectedIndex > 0 && txtDocumento.Text != string.Empty)
                    {
                        objEn.sDocumentoTipoId = Convert.ToInt16(ddl_TipoDocumento.SelectedValue);
                        objEn.vDocumentoNumero = txtDocumento.Text;
                    }
                }

                if (objEn.iPersonaId == 0 && txtDocumento.Text.Trim() == string.Empty)
                    return;

                object[] arrParametros = { objEn };
                objEn = SGAC.WebApp.Accesorios.Persona.oPersona(arrParametros);
                #endregion

                #region datos extra - MDIAZ
                if (objEn.iPersonaId != 0)
                {
                    ddl_TipoDocumento.SelectedValue = objEn.sDocumentoTipoId.ToString();
                    txtDocumento.Text = objEn.vDocumentoNumero;
                }
                #endregion

                #region Pintar Datos Persona

                Session.Add("iRepresentanteId", 0);

                if (objEn.iPersonaId > 0)
                {
                    Session.Add("iRepresentanteId", objEn.iPersonaId);
                    this.hdn_actu_iPersonaId.Value = Convert.ToInt64(objEn.iPersonaId).ToString();
                    txtApellidoPatBene.Text = HttpUtility.HtmlDecode(objEn.vApellidoPaterno);
                    txtApellidoMatBene.Text = HttpUtility.HtmlDecode(objEn.vApellidoMaterno);
                    txtNombreBene.Text = HttpUtility.HtmlDecode(objEn.vNombres);
                    if (objEn.sGeneroId > 0)
                    {
                        ddl_Genero.SelectedValue = objEn.sGeneroId.ToString();
                        //--------------------------------------------------------------
                        //Autor: Miguel Márquez Beltrán
                        //Fecha: 05/10/2016
                        //Objetivo: Permitir editar la fecha de nacimiento y el genero
                        //--------------------------------------------------------------

                       // ddl_Genero.Enabled = false;
                    }
                    else
                    {
                        ddl_Genero.SelectedIndex = 0;
                        ddl_Genero.Enabled = true;
                    }

                    txtApellidoPatBene.Enabled = false;
                    txtApellidoMatBene.Enabled = false;
                    txtNombreBene.Enabled = false;
                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AGREGAR BENEFICIARIO", " alert('El número de documento ingresado no existe.');", true);

                    txtApellidoPatBene.Text = string.Empty;
                    txtApellidoMatBene.Text = string.Empty;
                    txtNombreBene.Text = string.Empty;

                    txtApellidoPatBene.Enabled = true;
                    txtApellidoMatBene.Enabled = true;
                    txtNombreBene.Enabled = true;
                    ddl_Genero.Enabled = true;

                    Session.Add("iRepresentanteId", "0");
                    this.hdn_actu_iPersonaId.Value = "0";
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
    }
}