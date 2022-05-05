using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.BE;

namespace SGAC.WebApp.Accesorios.SharedControls
{
    class UbigeoAbierto
    {
        public UbigeoAbierto() { }
        public UbigeoAbierto(int departamentoId, int ciudadId, int distritoId)
        {
            this.DepartamentoId = departamentoId;
            this.CiudadId = ciudadId;
            this.DistritoId = distritoId;
        }

        public int? DepartamentoId { get; set; }
        public int? CiudadId { get; set; }
        public int? DistritoId { get; set; }
    }


    public partial class ctrlGridParticipante : System.Web.UI.UserControl
    {
        private static bool cargado = false;
        private static bool edicion = false;
        private static int edicion_rowindex = -1;

        private List<RE_PARTICIPANTE> loParticipanteContainer = new List<RE_PARTICIPANTE>();

        protected void Page_Load(object sender, EventArgs e)
        {
            imgBuscar.OnClientClick = "return ValidarPersona();";
        }

        public void SetControl(List<RE_PARTICIPANTE> participante) {
            
            mtFormInitialize();
            
            loParticipanteContainer = participante;
            Grd_Participantes.DataSource = mtParticipanteContainerToTable();
            Grd_Participantes.DataBind();
            
            LimpiarDatosParticipante();
        }

        private void mtFormInitialize()
        {
            this.btnAceptar.OnClientClick = "return ActoMilitar_Participantes()";
            DataTable dtTipDoc = new DataTable();
            dtTipDoc = comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmMaestro.DOCUMENTO_IDENTIDAD);
            DataView dv = dtTipDoc.DefaultView;
            DataTable dtOrdenado = dv.ToTable();
            dtOrdenado.DefaultView.Sort = "Id ASC";
            Util.CargarDropDownList(ddl_TipoDocParticipante, dtOrdenado, "Valor", "Id", true);
            
            int lDatoParticipante = Convert.ToInt32(Session["TIPO_ACTO_PARTICIPANTE"]);
            if (lDatoParticipante == (int)Enumerador.enmTipoActa.NACIMIENTO)
            {
                Util.CargarParametroDropDownList(ddl_TipoParticipante, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_CIVIL_PARTICIPANTE_NACIMIENTO), true);
                ddl_TipoDatoParticipante.Visible = true;
                lblPartTipoVinc.Visible = true;
            }
            else if (lDatoParticipante == (int)Enumerador.enmTipoActa.MATRIMONIO)
            {
                Util.CargarParametroDropDownList(ddl_TipoParticipante, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_CIVIL_PARITICPANTE_MATRIMONIO), true);
                ddl_TipoDatoParticipante.Visible = false;
                lblPartTipoVinc.Visible = false;
            }
            else if (lDatoParticipante == (int)Enumerador.enmTipoActa.DEFUNCION)
            {
                Util.CargarParametroDropDownList(ddl_TipoParticipante, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REG_CIVIL_PARTICIPANTE_DEFUNCION), true);
                ddl_TipoDatoParticipante.Visible = true;
                lblPartTipoVinc.Visible = true;
            }
            else if (lDatoParticipante == Convert.ToInt32(Enumerador.enmGrupo.REGISTRO_MILITAR_TIPO_PARTICIPANTE))
            {
                Util.CargarParametroDropDownList(ddl_TipoParticipante, comun_Part1.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.REGISTRO_MILITAR_TIPO_PARTICIPANTE), true);
                ddl_TipoDatoParticipante.Visible = false;
                lblPartTipoVinc.Visible = false;
            }

            //-------------------------------------
            Enumerador.enmGrupo[] arrGrupos = { Enumerador.enmGrupo.ACTO_CIVIL_PARTICIPANTE_TIPO_DATO, Enumerador.enmGrupo.PERSONA_TIPO_VINCULO, Enumerador.enmGrupo.PERSONA_NACIONALIDAD };
            DropDownList[] arrDDL = { ddl_TipoDatoParticipante, ddl_TipoVinculoParticipante, ddl_NacParticipante };

            DataTable dtGrupoParametros = new DataTable();

            dtGrupoParametros = comun_Part1.ObtenerParametrosListaGrupos(Session, arrGrupos);

            Util.CargarParametroDropDownListDesdeListaGrupos(arrDDL, arrGrupos, dtGrupoParametros, true);
            //-------------------------------------                

            //Util.CargarParametroDropDownList(ddl_TipoDatoParticipante, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.ACTO_CIVIL_PARTICIPANTE_TIPO_DATO), true);
            //Util.CargarParametroDropDownList(ddl_TipoVinculoParticipante, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_TIPO_VINCULO), true);
            //Util.CargarParametroDropDownList(ddl_NacParticipante, Comun.ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.PERSONA_NACIONALIDAD), true);
            
            
            ctrlUbigeo1.UbigeoRefresh();
        }

        private void LimpiarDatosParticipante()
        {
            this.ddl_TipoParticipante.SelectedIndex = 0;
            if (this.ddl_TipoDatoParticipante.SelectedIndex != -1) { this.ddl_TipoDatoParticipante.SelectedIndex = 0; };
            this.ddl_TipoVinculoParticipante.SelectedIndex = 0;
            this.ddl_TipoDocParticipante.SelectedIndex = 0;
            this.ddl_NacParticipante.SelectedIndex = 0;
            this.ddl_NacParticipante.Enabled = true;
            this.txtNroDocParticipante.Text = string.Empty;         
            this.txtNomParticipante.Text = string.Empty;
            this.txtNomParticipante.Enabled = true;
            this.txtApePatParticipante.Text = string.Empty;
            this.txtApePatParticipante.Enabled = true;
            this.txtApeMatParticipante.Text = string.Empty;
            this.txtApeMatParticipante.Enabled = true;
            this.txtDireccionParticipante.Text = string.Empty;
            this.txtPersonaId.Text = string.Empty;
            this.ctrlUbigeo1.ClearControl();
        }

        private DataTable mtParticipanteContainerToTable()
        {
            DataTable dt = CrearTablaParticipante();
            int ItemRow = 1;
            #region Creando DataTable
            foreach (RE_PARTICIPANTE item in loParticipanteContainer.Where(p=>p.cEstado !="E"))
            {
                DataRow dr = dt.NewRow();
                dr["iItemRow"] = ItemRow++;
                dr["iActuacionParticipanteId"] = item.iParticipanteId;
                dr["iPersonaId"] = item.iPersonaId;
                dr["vApellidoPaterno"] = item.vPrimerApellido;
                dr["vApellidoMaterno"] = item.vSegundoApellido;
                dr["vNombres"] = item.vNombres;
                dr["sTipoParticipanteId"] = item.sTipoParticipanteId;
                dr["vTipoParticipante"] = item.vTipoParticipante; //ddl_TipoParticipante.SelectedItem.Text.ToString(); //item.sTipoParticipanteId;
                dr["sTipoDatoId"] = item.sTipoDatoId;
                dr["sTipoVinculoId"] = item.sTipoVinculoId;
                dr["sDocumentoTipoId"] = item.sTipoDocumentoId;
                dr["vDocumentoTipo"] = item.vTipoDocumento; // IDM-CREADO
                dr["vDocumentoNumero"] = item.vNumeroDocumento;
                dr["vDocumentoCompleto"] = item.vTipoDocumento.ToString() + " - " + item.vNumeroDocumento.ToString();
                dr["sNacionalidadId"] = item.sNacionalidadId;
                dr["vResidenciaDireccion"] = item.vDireccion;
                dr["cResidenciaUbigeo"] = item.vUbigeo;
                dr["ICentroPobladoId"] = item.ICentroPobladoId;
                dr["cEstado"] = item.cEstado;
                dr["vNombreCompleto"] = item.vPrimerApellido + " " + item.vSegundoApellido + "," + item.vNombres;

                dt.Rows.Add(dr);
            }
            #endregion
            return dt;
        }

        protected void imgBuscar_Click(object sender, ImageClickEventArgs e)
        {
            #region Buscar persona
            EnPersona objEn = new EnPersona();
            objEn.iPersonaId = 0;
            objEn.sDocumentoTipoId = Convert.ToInt32(ddl_TipoDocParticipante.SelectedValue);
            if (ddl_TipoDatoParticipante.SelectedItem != null) {
                objEn.vDocumentoTipo = ddl_TipoDatoParticipante.SelectedItem.Text;
            }
            objEn.vDocumentoNumero = txtNroDocParticipante.Text;
            if (ddl_TipoVinculoParticipante.SelectedIndex > 0)
            {
                objEn.sTipoVinculoId = Convert.ToInt32(ddl_TipoVinculoParticipante.SelectedValue);
            }
            if (ddl_TipoDatoParticipante.SelectedIndex > 0)
            {
                objEn.sTipoDatoId = Convert.ToInt32(ddl_TipoDatoParticipante.SelectedValue);
            }
            object[] arrParametros = { objEn };
            objEn = SGAC.WebApp.Accesorios.Persona.oPersona(arrParametros);
            #endregion

            #region Pintar Datos Persona
            string strScript = string.Empty;
            txtPersonaId.Text = "0";
            if (objEn != null)
            {
                txtPersonaId.Text = objEn.iPersonaId.ToString();
                ddl_NacParticipante.SelectedValue = objEn.sNacionalidadId.ToString();
                
                txtNomParticipante.Text = objEn.vNombres;
                txtApePatParticipante.Text = objEn.vApellidoPaterno;
                txtApeMatParticipante.Text = objEn.vApellidoMaterno;
                txtDireccionParticipante.Text = ""; // IDM-PENDIENTE

                if (objEn.vNombres != string.Empty && objEn.vNombres != null)
                {
                    txtNomParticipante.Enabled = false;
                    txtApeMatParticipante.Enabled = false;
                    txtApePatParticipante.Enabled = false;
                }
                else
                {
                    strScript += Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.INFORMATION,
                        "PARTICIPANTE", "El número de documento no esta registrado en el sistema.");
                    Comun.EjecutarScript(Page, strScript);
                    txtNomParticipante.Enabled = true;
                    txtApeMatParticipante.Enabled = true;
                    txtApePatParticipante.Enabled = true;
                }

         
                ctrlUbigeo1.setDepartamentoId(objEn.iDptoContId.ToString());
                ctrlUbigeo1.setCiudadId(objEn.iDptoContId.ToString(), objEn.iProvPaisId.ToString());
                ctrlUbigeo1.setDistrito(objEn.iDptoContId.ToString(), objEn.iProvPaisId.ToString(), objEn.iDistCiuId.ToString());



                //ctrlUbigeo1.setUbigeo(objEn.vU)

                //if (objEn.vDptoCont != string.Empty)
                //{
                //    //ddl_ContDepParticipante.SelectedValue = objEn.vDptoCont;
                //    //if (objEn.vProvPais != string.Empty)
                //    //    ddl_PaisCiudadParticipante.SelectedValue = objEn.vProvPais;
                //    //else
                //    //{
                //    //    if (objEn.vDistCiu != string.Empty)
                //    //        ddl_CiudadDistritoParticipante.SelectedValue = objEn.vDistCiu;
                //    //}
                //}
            }
            #endregion
        }


        #region Eventos de controls del control
        
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarDatosParticipante();
            
            edicion = false;
            edicion_rowindex = -1;
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {

            if ((loParticipanteContainer.Count == 0) && (edicion_rowindex <= 0)) { edicion_rowindex = -1; }

            if ((edicion) && (edicion_rowindex != -1))
            {                    
                loParticipanteContainer[edicion_rowindex].sTipoParticipanteId = Convert.ToInt16(ddl_TipoParticipante.SelectedValue);
                loParticipanteContainer[edicion_rowindex].vTipoParticipante = ddl_TipoParticipante.SelectedItem.Text.ToString();
                if (ddl_TipoDatoParticipante.SelectedValue != "")
                {
                loParticipanteContainer[edicion_rowindex].sTipoDatoId = Convert.ToInt16(ddl_TipoDatoParticipante.SelectedValue);
                }
                loParticipanteContainer[edicion_rowindex].sTipoVinculoId = Convert.ToInt16(ddl_TipoVinculoParticipante.SelectedValue);
                 
                loParticipanteContainer[edicion_rowindex].sNacionalidadId = Convert.ToInt16(ddl_NacParticipante.SelectedValue);
                loParticipanteContainer[edicion_rowindex].vNombres = txtNomParticipante.Text.ToString();
                loParticipanteContainer[edicion_rowindex].vPrimerApellido = txtApePatParticipante.Text.ToString();
                loParticipanteContainer[edicion_rowindex].vSegundoApellido = txtApeMatParticipante.Text.ToString();
                loParticipanteContainer[edicion_rowindex].vDireccion = txtDireccionParticipante.Text.ToString();
                if (this.ctrlUbigeo1.getResidenciaUbigeo() != null) { loParticipanteContainer[edicion_rowindex].vUbigeo = (string)this.ctrlUbigeo1.getResidenciaUbigeo(); }
                if (this.ctrlUbigeo1.getCentroPobladoId() != null) { loParticipanteContainer[edicion_rowindex].ICentroPobladoId = (int)this.ctrlUbigeo1.getCentroPobladoId(); }
                loParticipanteContainer[edicion_rowindex].cEstado = "M";

            }
            else
            {
                RE_PARTICIPANTE loParticipante = new RE_PARTICIPANTE();
                #region Creando objecto PARTICIPANTE

                loParticipante.sTipoParticipanteId = Convert.ToInt16(ddl_TipoParticipante.SelectedValue);
                loParticipante.vTipoParticipante = ddl_TipoParticipante.SelectedItem.Text.ToString();
                if (ddl_TipoDatoParticipante.SelectedValue != "")
                {
                    loParticipante.sTipoDatoId = Convert.ToInt16(ddl_TipoDatoParticipante.SelectedValue);
                }
                loParticipante.sTipoVinculoId = Convert.ToInt16(ddl_TipoVinculoParticipante.SelectedValue);

                if (txtPersonaId.Text.ToString() != "") { loParticipante.iPersonaId = Convert.ToInt64(txtPersonaId.Text.ToString()); }

                loParticipante.sTipoDocumentoId = Convert.ToInt16(ddl_TipoDocParticipante.SelectedValue);
                loParticipante.vTipoDocumento = ddl_TipoDocParticipante.SelectedItem.Text;
                loParticipante.vNumeroDocumento = txtNroDocParticipante.Text.ToString();
                loParticipante.sNacionalidadId = Convert.ToInt16(ddl_NacParticipante.SelectedValue);
                loParticipante.vNombres = txtNomParticipante.Text.ToString();
                loParticipante.vPrimerApellido = txtApePatParticipante.Text.ToString();
                loParticipante.vSegundoApellido = txtApeMatParticipante.Text.ToString();
                loParticipante.vDireccion = txtDireccionParticipante.Text.ToString();
                if (this.ctrlUbigeo1.getResidenciaUbigeo() != null) { loParticipante.vUbigeo = (string)this.ctrlUbigeo1.getResidenciaUbigeo(); }
                if (this.ctrlUbigeo1.getCentroPobladoId() != null) { loParticipante.ICentroPobladoId = (int)this.ctrlUbigeo1.getCentroPobladoId(); }

                if ((loParticipante.cEstado != null) && (loParticipante.cEstado == "A") && (edicion) && (edicion_rowindex != -1))
                {
                    loParticipante.cEstado = "M";
                }
                loParticipante.sUsuarioCreacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                loParticipante.sUsuarioModificacion = Convert.ToInt16(Session[Constantes.CONST_SESION_USUARIO_ID]);
                loParticipante.sOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID]);
                #endregion
                if (!ExistTipoParticipante(Convert.ToInt16(ddl_TipoParticipante.SelectedValue)))
                {
                    loParticipanteContainer.Add(loParticipante);
                }
                else
                {
                    Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "PARTICIPANTE", "Ya existe participante con el tipo seleccionado."));
                }
            }
            Grd_Participantes.DataSource = mtParticipanteContainerToTable();
            Grd_Participantes.DataBind();

            edicion = false;
            edicion_rowindex = -1;
            LimpiarDatosParticipante();
        }
            
        protected void Grd_Participantes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int lRowIndex = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Editar")
            {
                if (loParticipanteContainer != null)
                {
                    if (loParticipanteContainer.Count > 0)
                    {
                        RE_PARTICIPANTE loParticipante = loParticipanteContainer[lRowIndex];

                        if (loParticipante.sTipoParticipanteId > 0)
                            ddl_TipoParticipante.SelectedValue = Convert.ToInt16(loParticipante.sTipoParticipanteId).ToString();
                        else
                            ddl_TipoParticipante.SelectedIndex = 0;

                        if (loParticipante.sTipoDatoId > 0)
                            ddl_TipoDatoParticipante.SelectedValue = Convert.ToInt16(loParticipante.sTipoDatoId).ToString();
                        if (loParticipante.sTipoVinculoId > 0)
                            ddl_TipoVinculoParticipante.SelectedValue = Convert.ToInt16(loParticipante.sTipoVinculoId).ToString();
                        else
                            ddl_TipoVinculoParticipante.SelectedIndex = 0;
                        if (loParticipante.sTipoDocumentoId > 0)
                            ddl_TipoDocParticipante.SelectedValue = Convert.ToInt16(loParticipante.sTipoDocumentoId).ToString();
                        else
                            ddl_TipoDocParticipante.SelectedIndex = 0;
                        txtNroDocParticipante.Text = loParticipante.vNumeroDocumento;
                        if (loParticipante.sNacionalidadId > 0)
                            ddl_NacParticipante.SelectedValue = Convert.ToInt16(loParticipante.sNacionalidadId).ToString();
                        else
                            ddl_NacParticipante.SelectedIndex = 0;
                        txtNomParticipante.Text = loParticipante.vNombres;
                        txtApePatParticipante.Text = loParticipante.vPrimerApellido;
                        txtApeMatParticipante.Text = loParticipante.vSegundoApellido;
                        txtDireccionParticipante.Text = loParticipante.vDireccion;

                        this.ctrlUbigeo1.setUbigeo(loParticipante.vUbigeo);

                        edicion = true;
                        edicion_rowindex = lRowIndex;

                        this.btnAceptar.Enabled = true;
                        this.btnCancelar.Enabled = true;
                    }
                    else
                    {
                        Comun.EjecutarScript(Page, Mensaje.MostrarMensaje(Enumerador.enmTipoMensaje.ERROR, "PARTICIPANTE", "No existe participante"));
                    }
                }
            }
            if (e.CommandName == "Eliminar")
            {
                loParticipanteContainer[lRowIndex].cEstado = "E";
                LimpiarDatosParticipante();
            }
            this.Grd_Participantes.DataSource = mtParticipanteContainerToTable();
            this.Grd_Participantes.DataBind();
        }

        #endregion

        private DataTable CrearTablaParticipante()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iActuacionParticipanteId", typeof(string));
            dt.Columns.Add("iPersonaId", typeof(string));
            dt.Columns.Add("vApellidoPaterno", typeof(string));
            dt.Columns.Add("vApellidoMaterno", typeof(string));
            dt.Columns.Add("vNombres", typeof(string));
            dt.Columns.Add("sTipoParticipanteId", typeof(string));
            dt.Columns.Add("vTipoParticipante", typeof(string));
            dt.Columns.Add("sTipoDatoId", typeof(string));
            dt.Columns.Add("sTipoVinculoId", typeof(string));
            dt.Columns.Add("sDocumentoTipoId", typeof(string));
            dt.Columns.Add("vDocumentoTipo", typeof(string)); 
            dt.Columns.Add("vDocumentoNumero", typeof(string));
            dt.Columns.Add("vDocumentoCompleto", typeof(string));
            dt.Columns.Add("sNacionalidadId", typeof(string));
            dt.Columns.Add("vResidenciaDireccion", typeof(string));
            dt.Columns.Add("cResidenciaUbigeo", typeof(string));
            dt.Columns.Add("ICentroPobladoId", typeof(string));
            dt.Columns.Add("cEstado", typeof(string));
            dt.Columns.Add("vNombreCompleto", typeof(string));
            dt.Columns.Add("iItemRow", typeof(int));
            return dt;
        }

        #region Validaciones & Reglas de Control
        private bool ExistTipoParticipante(Int16 tp)
        {
            bool lReturn = false;
            foreach (RE_PARTICIPANTE p in loParticipanteContainer.Where(p=>p.cEstado !="E"))
            {
                if (p.sTipoParticipanteId == tp)
                {
                    lReturn = true;
                }
            }
            return lReturn;
        }
        #endregion

        public List<RE_PARTICIPANTE> getParticipantes()
        {
            return loParticipanteContainer;
        }

        public void HabilitaControl(bool bHabilitado = true)
        {
            ddl_TipoParticipante.Enabled = bHabilitado;
            ddl_TipoDatoParticipante.Enabled = bHabilitado;
            ddl_TipoVinculoParticipante.Enabled = bHabilitado;
            ddl_TipoDocParticipante.Enabled = bHabilitado;

            txtNroDocParticipante.Enabled = bHabilitado;
            ddl_NacParticipante.Enabled = bHabilitado;
            txtNomParticipante.Enabled = bHabilitado;
            txtApePatParticipante.Enabled = bHabilitado;
            txtApeMatParticipante.Enabled = bHabilitado;
            txtDireccionParticipante.Enabled = bHabilitado;
            txtObservacionesAP.Enabled = bHabilitado;
            btnAceptar.Enabled = bHabilitado;
            btnCancelar.Enabled = bHabilitado;
            Grd_Participantes.Enabled = bHabilitado;

            ctrlUbigeo1.HabilitaControl(false);

        }
    }
}