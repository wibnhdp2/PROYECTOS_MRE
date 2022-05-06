using System;
using System.Data;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using SGAC.Accesorios;
using SGAC.Controlador;
using SGAC.WebApp.Accesorios.SharedControls;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Drawing;

namespace SGAC.WebApp.Accesorios
{
    public class comun_Part3
    {
        #region UBICACION GEOGRAFICA
        public static DataTable ObtenerUbicacionGeografica(HttpSessionState Session,
            string strDepaCont, string strProvPais, string strDistEst, string strCodigo = "")
        {
            //DataTable dt = (DataTable)Session[Constantes.CONST_SESION_DT_UBIGEO];
            SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
            DataTable dt = objUbigeoBL.Consultar(strDepaCont, strProvPais, strDistEst);
            DataView dv = dt.DefaultView;

            string strFiltro = "";
            if (strCodigo != string.Empty)
            {
                strFiltro += " ubge_cCodigo = '" + strCodigo + "'";
            }
            else
            {
                if (strDepaCont != string.Empty)
                {
                    strFiltro += " ubge_cUbi01 = '" + strDepaCont + "'";
                }
                if (strProvPais != string.Empty)
                {
                    if (strFiltro != string.Empty)
                        strFiltro += " and";
                    strFiltro += " ubge_cUbi02 = '" + strProvPais + "'";
                }
                if (strDistEst != string.Empty)
                {
                    if (strFiltro != string.Empty)
                        strFiltro += " and";
                    strFiltro += " ubge_cUbi03 = '" + strDistEst + "'";
                }
            }

            DataTable dtFiltrado = new DataTable();
            if (strFiltro != string.Empty)
            {
                dv.RowFilter = strFiltro;
            }
            dtFiltrado = dv.ToTable();
            return dtFiltrado;
        }

        //string strDepaCont, string strProvPais, string strDistEst, string strCodigo = ""
        public static DataTable ObtenerUbicacionGeograficaPaginado(HttpSessionState Session,
            ref object[] arrParametros)
        {
            string strDepaCont = arrParametros[0].ToString();
            string strProvPais = arrParametros[1].ToString();
            string strDistEst = arrParametros[2].ToString();
            string strCodigo = arrParametros[3].ToString();
            int intPaginaActual = (int)arrParametros[4];//0; 
            int intPaginaCantidad = (int)arrParametros[5];//0; 
            int intTotalRegistros = (int)arrParametros[6];//0; 
            int intTotalPaginas = (int)arrParametros[7];//0; 

            //DataTable dt = (DataTable)Session[Constantes.CONST_SESION_DT_UBIGEO];
            SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
            DataTable dt = objUbigeoBL.Consultar(strDepaCont, strProvPais, strDistEst);
            DataView dv = dt.DefaultView;

            string strFiltro = "";
            if (strCodigo != string.Empty)
            {
                strFiltro += " ubge_cCodigo = '" + strCodigo + "'";
            }
            else
            {
                if (strDepaCont != string.Empty)
                {
                    strFiltro += " ubge_cUbi01 = '" + strDepaCont + "'";
                }
                if (strProvPais != string.Empty)
                {
                    if (strFiltro != string.Empty)
                        strFiltro += " and";
                    strFiltro += " ubge_cUbi02 = '" + strProvPais + "'";
                }
                if (strDistEst != string.Empty)
                {
                    if (strFiltro != string.Empty)
                        strFiltro += " and";
                    strFiltro += " ubge_cUbi03 = '" + strDistEst + "'";
                }
            }

            DataTable dtFiltrado = new DataTable();
            if (strFiltro != string.Empty)
            {
                dv.RowFilter = strFiltro;
            }
            dtFiltrado = dv.ToTable();

            DataTable dtPaginado = new DataTable();
            dtPaginado = dtFiltrado.Clone();
            int intInicio = (intPaginaActual - 1) * intPaginaCantidad;
            int intFin = (intPaginaActual - 1) * intPaginaCantidad + intPaginaCantidad;
            for (int i = intInicio; i < intFin; i++)
            {
                if (i < dtFiltrado.Rows.Count)
                    dtPaginado.Rows.Add(dtFiltrado.Rows[i].ItemArray);
                else
                    break;
            }
            intTotalRegistros = dtFiltrado.Rows.Count;
            if (intTotalRegistros % intPaginaCantidad == 0)
                intTotalPaginas = intTotalRegistros / intPaginaCantidad;
            else
                intTotalPaginas = intTotalRegistros / intPaginaCantidad + 1;

            arrParametros[6] = intTotalRegistros;
            arrParametros[7] = intTotalPaginas;

            return dtPaginado;
        }

        /// <summary>
        /// Obtiene el listado de Paises incluido Peru
        /// </summary>
        /// <param name="Session"></param>
        /// <param name="intContinenteId"></param>
        /// <param name="ddlUbigeo"></param>
        /// <param name="bSeleccion"></param>
        public static void CargarPaisesPorContinente(HttpSessionState Session, int intContinenteId, DropDownList ddlUbigeo, bool bSeleccion = false)
        {
            try
            {
                DataTable dtFiltrado = new DataTable();
                //DataTable dt = (DataTable)Session[Constantes.CONST_SESION_DT_UBIGEO];
                SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
                DataTable dt = objUbigeoBL.Consultar(null, null, null);
                DataView dv = dt.DefaultView;

                dv.RowFilter = " ubge_cUbi01 = " + intContinenteId;
                dv.Sort = "ubge_vProvincia  ASC";
                dtFiltrado = dv.ToTable(true, "ubge_vProvincia", "ubge_cUbi02");

                Util.CargarDropDownList(ddlUbigeo, dtFiltrado, "ubge_vProvincia", "ubge_cUbi02");
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public static void CargarContinenteDepartamento(HttpSessionState Session, DropDownList ddlUbigeo, bool bolSeleccion = false)
        {
            try
            {
                DataTable dtFiltrado = new DataTable();
                //DataTable dt = (DataTable)Session[Constantes.CONST_SESION_DT_UBIGEO];
                SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
                DataTable dt = objUbigeoBL.Consultar(null, null, null);
                DataView dv = dt.DefaultView;
                dv.Sort = "ubge_vDepartamento  ASC";
                dtFiltrado = dv.ToTable(true, "ubge_vDepartamento", "ubge_cUbi01");
                Util.CargarDropDownList(ddlUbigeo, dtFiltrado, "ubge_vDepartamento", "ubge_cUbi01", bolSeleccion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public static void CargarProvinciaPais(HttpSessionState Session, DropDownList ddlUbigeo, int intProvPais, bool bolSeleccion = false)
        {
            try
            {
                DataTable dtFiltrado = new DataTable();
                //DataTable dt = (DataTable)Session[Constantes.CONST_SESION_DT_UBIGEO];
                SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
                DataTable dt = objUbigeoBL.Consultar(null, null, null);
                DataView dv = dt.DefaultView;
                dv.Sort = "ubge_vProvincia  ASC";
                dtFiltrado = dv.ToTable(true, "ubge_vProvincia", "ubge_cUbi02");
                Util.CargarDropDownList(ddlUbigeo, dtFiltrado, "ubge_vProvincia", "ubge_cUbi02", bolSeleccion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public static void CargarDistritoEstado(HttpSessionState Session, DropDownList ddlUbigeo, int intDistEsta, bool bolSeleccion = false)
        {
            try
            {
                DataTable dtFiltrado = new DataTable();
                //DataTable dt = (DataTable)Session[Constantes.CONST_SESION_DT_UBIGEO];
                SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
                DataTable dt = objUbigeoBL.Consultar(null, null, null);
                DataView dv = dt.DefaultView;
                dv.Sort = "ubge_vDistrito  ASC";
                dtFiltrado = dv.ToTable();
                Util.CargarDropDownList(ddlUbigeo, dtFiltrado, "ubge_vDistrito", "ubge_cUbi03", bolSeleccion);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public static DataTable ObtenerDepartamentos(HttpSessionState Session)
        {
            try
            {
                //DataTable dt = (DataTable)Session[Constantes.CONST_SESION_DT_UBIGEO];
                SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
                DataTable dt = objUbigeoBL.Consultar(null, null, "01"); // MDIAZ - 21/07/2015
                DataView dv = dt.DefaultView;
                //dv.RowFilter = "ubge_cUbi02 = '01' and ubge_cUbi03 = '01'";
                dv.Sort = "ubge_cUbi01  ASC";
                DataTable dtFiltrado = dv.ToTable(true, "ubge_vDepartamento", "ubge_cUbi01");
                return dtFiltrado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public static DataTable ObtenerDepartamentosSinContinentes(HttpSessionState Session)
        {
            try
            {
                //DataTable dt = (DataTable)Session[Constantes.CONST_SESION_DT_UBIGEO];
                SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
                DataTable dt = objUbigeoBL.Consultar(null, null, "01"); // MDIAZ - 21/07/2015
                DataView dv = dt.DefaultView;
                //dv.RowFilter = "ubge_cUbi02 = '01' and ubge_cUbi03 = '01'";
                dv.RowFilter = "ubge_cUbi01 < 90";
                dv.Sort = "ubge_cUbi01  ASC";
                DataTable dtFiltrado = dv.ToTable(true, "ubge_vDepartamento", "ubge_cUbi01");
                return dtFiltrado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public static DataTable ObtenerPaises(HttpSessionState Session)
        {
            try
            {
                //DataTable dt = (DataTable)Session[Constantes.CONST_SESION_DT_UBIGEO];
                SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
                DataTable dt = objUbigeoBL.Consultar(null, null, "01");
                DataView dv = dt.DefaultView;
                dv.RowFilter = "ubge_cUbi01 > 90";
                dv.Sort = "ubge_vProvincia  ASC";
                DataTable dtFiltrado = dv.ToTable(true, "ubge_vProvincia", "ubge_cUbi02");
                return dtFiltrado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public static DataTable ObtenerContinente(HttpSessionState Session)
        {
            try
            {
                //DataTable dt = (DataTable)Session[Constantes.CONST_SESION_DT_UBIGEO];
                SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
                DataTable dt = objUbigeoBL.Consultar(null, null, "01");
                DataView dv = dt.DefaultView;
                dv.RowFilter = "ubge_cUbi01 like '9%'";
                dv.Sort = "ubge_cUbi01  ASC";
                DataTable dtFiltrado = dv.ToTable(true, "ubge_vDepartamento", "ubge_cUbi01");
                return dtFiltrado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable ObtenerProvincias(HttpSessionState Session, string strUbi01)
        {
            try
            {
                //DataTable dt = (DataTable)Session[Constantes.CONST_SESION_DT_UBIGEO];
                SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
                DataTable dt = objUbigeoBL.Consultar(strUbi01, null, "01");
                DataView dv = dt.DefaultView;
                //dv.RowFilter = "ubge_cUbi01 = " + strUbi01 + " and ubge_cUbi03 = '01'";
                dv.Sort = "ubge_vProvincia  ASC";
                DataTable dtFiltrado = dv.ToTable(true, "ubge_vProvincia", "ubge_cUbi02");
                return dtFiltrado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public static DataTable ObtenerDistritos(HttpSessionState Session, string strUbi01, string strUbi02)
        {
            try {
                //DataTable dt = (DataTable)Session[Constantes.CONST_SESION_DT_UBIGEO];
                SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
                DataTable dt = objUbigeoBL.Consultar(strUbi01, strUbi02, null);
                DataView dv = dt.DefaultView;
                //dv.RowFilter = "ubge_cUbi01 = " + strUbi01 + " AND ubge_cUbi02 = " + strUbi02;
                dv.Sort = "ubge_vDistrito  ASC";
                DataTable dtFiltrado = dv.ToTable(true, "ubge_vDistrito", "ubge_cUbi03");
                return dtFiltrado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public static DataTable ObtenerContDepa(HttpSessionState Session, Enumerador.enmNacionalidad enmNacionalidad)
        {
            try
            {
                DataTable dtFiltrado = new DataTable();
                //DataTable dt = ((DataTable)Session[Constantes.CONST_SESION_DT_UBIGEO]).Copy();

                SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
                DataTable dt = objUbigeoBL.Consultar(null, null, null);

                DataView dv = dt.DefaultView;
                if (enmNacionalidad == Enumerador.enmNacionalidad.PERUANA)
                    dv.RowFilter = "ubge_cUbi01 < 90";
                else
                    dv.RowFilter = "ubge_cUbi01 > 90";
                dv.Sort = "ubge_vDepartamento  ASC";
                dtFiltrado = dv.ToTable(true, "ubge_vDepartamento", "ubge_cUbi01");
                return dtFiltrado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public static DataTable ObtenerPaisProv(HttpSessionState Session, string strContDepa)
        {
            try
            {
                DataTable dtFiltrado = new DataTable();
                //DataTable dt = ((DataTable)Session[Constantes.CONST_SESION_DT_UBIGEO]).Copy();
                SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
                //DataTable dt = objUbigeoBL.Consultar(strContDepa, null, null);
                DataTable dt = objUbigeoBL.Consultar(strContDepa, null, "01"); // MDIAZ - 21/07/2015
                DataView dv = dt.DefaultView;
                //dv.RowFilter = "ubge_cUbi01 = " + strContDepa;
                dv.Sort = "ubge_vProvincia  ASC";
                dtFiltrado = dv.ToTable(true, "ubge_vProvincia", "ubge_cUbi02");
                return dtFiltrado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ObtenerPaisProv(HttpSessionState Session, string strContDepa, bool Todos)
        {
            try
            {
                DataTable dtFiltrado = new DataTable();
                //DataTable dt = ((DataTable)Session[Constantes.CONST_SESION_DT_UBIGEO]).Copy();
                SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
                //DataTable dt = objUbigeoBL.Consultar(strContDepa, null, null);
                DataTable dt = objUbigeoBL.Consultar(strContDepa, null, null); // MDIAZ - 21/07/2015
                DataView dv = dt.DefaultView;
                //dv.RowFilter = "ubge_cUbi01 = " + strContDepa;
                dv.Sort = "ubge_vProvincia  ASC";
                dtFiltrado = dv.ToTable(true, "ubge_vProvincia", "ubge_cUbi02");
                return dtFiltrado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ObtenerRegiDist(HttpSessionState Session, string strContDepa, string strPaisProv)
        {
            try
            {
                DataTable dtFiltrado = new DataTable();
                //DataTable dt = ((DataTable)Session[Constantes.CONST_SESION_DT_UBIGEO]).Copy();
                SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL objUbigeoBL = new SGAC.Configuracion.Sistema.BL.UbigeoConsultasBL();
                DataTable dt = objUbigeoBL.Consultar(strContDepa, strPaisProv, null);
                DataView dv = dt.DefaultView;
                //dv.RowFilter = "ubge_cUbi01 = " + strContDepa + " AND ubge_cUbi02 = " + strPaisProv;
                dv.Sort = "ubge_vDistrito  ASC";
                dtFiltrado = dv.ToTable(true, "ubge_vDistrito", "ubge_cUbi03");
                return dtFiltrado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CargarUbigeo(HttpSessionState Session,
                                        DropDownList ddlSeleccionado,
                                        Enumerador.enmTipoUbigeo enmUbigeo,
                                        string strProvPais = "",
                                        string strDistCiud = "",
                                        bool bolAgregarItemAdicional = false,
                                        Enumerador.enmNacionalidad enmNacionalidad = Enumerador.enmNacionalidad.NINGUNA, bool SoloDepartamentos = false)
        {
            try
            {
                DataTable dtUbigeo = new DataTable();
                string strDescripcion = string.Empty;
                string strValor = string.Empty;

                // Limpiar
                ddlSeleccionado.Items.Clear();
                ddlSeleccionado.SelectedIndex = -1;
                ddlSeleccionado.SelectedValue = null;
                ddlSeleccionado.ClearSelection();
                // --
                switch (enmUbigeo)
                {
                    case Enumerador.enmTipoUbigeo.DEPARTAMENTO_CONT:
                        if (enmNacionalidad == Enumerador.enmNacionalidad.EXTRANJERA)
                        {
                            dtUbigeo = ObtenerContinente(Session);
                        }
                        else
                        {
                            if (SoloDepartamentos)
                            {
                                dtUbigeo = ObtenerDepartamentosSinContinentes(Session);
                            }
                            else
                            {
                                dtUbigeo = ObtenerDepartamentos(Session);
                            }
                        }
                        strDescripcion = "ubge_vDepartamento";
                        strValor = "ubge_cUbi01";
                        break;
                    case Enumerador.enmTipoUbigeo.PROVINCIA_PAIS:
                        if (strProvPais != string.Empty)
                        {
                            dtUbigeo = ObtenerProvincias(Session, strProvPais);
                        }
                        strDescripcion = "ubge_vProvincia";
                        strValor = "ubge_cUbi02";
                        break;
                    case Enumerador.enmTipoUbigeo.DISTRITO_CIUD:
                        if (strProvPais != string.Empty || strDistCiud != string.Empty)
                            dtUbigeo = ObtenerDistritos(Session, strProvPais, strDistCiud);
                        strDescripcion = "ubge_vDistrito";
                        strValor = "ubge_cUbi03";
                        break;
                }

                ddlSeleccionado.DataSource = dtUbigeo;
                ddlSeleccionado.DataTextField = strDescripcion;
                ddlSeleccionado.DataValueField = strValor;
                ddlSeleccionado.DataBind();

                if (bolAgregarItemAdicional)
                {
                    ddlSeleccionado.AppendDataBoundItems = bolAgregarItemAdicional;
                    ddlSeleccionado.Items.Insert(0, new ListItem("- SELECCIONAR - ", "0"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        #endregion
    }
}