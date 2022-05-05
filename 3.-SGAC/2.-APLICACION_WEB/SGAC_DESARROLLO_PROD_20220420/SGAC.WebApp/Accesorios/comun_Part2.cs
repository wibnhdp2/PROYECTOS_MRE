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
    public class comun_Part2
    {
        #region TARIFARIO
        // intSeccionId=0, strNumeroLetra="", strDescripcion="", strEstado='A'
        // intPaginaActual=0, intPaginaCantidad=0, intTotalRegistros=0, intTotalPaginas=0
        public static DataTable ObtenerTarifario(HttpSessionState Session,
                            ref object[] arrParametros, string strIncluirExcepcion = "N")
        {
            try
            {
                int intSeccionId = (int)arrParametros[0];//0; 
                string strNumeroLetra = arrParametros[1].ToString();//""; 
                string strDescripcion = arrParametros[2].ToString();//"";
                string strEstado = arrParametros[3].ToString();//""; 
                int intPaginaActual = (int)arrParametros[4];//0; 
                int intPaginaCantidad = (int)arrParametros[5];//0; 
                int intTotalRegistros = (int)arrParametros[6];//0; 
                int intTotalPaginas = (int)arrParametros[7];//0; 

                DataTable dtTarifario = new DataTable();
                DataTable dtFiltrado = new DataTable();

                string[] arrFiltro = ObtenerSeparacion(strNumeroLetra).Split('|');
                string strNumero = arrFiltro[0];
                string strLetra = arrFiltro[1];

                dtTarifario = ObtenerTarifarioCargaInicial(Session);

                //dtTarifario = (DataTable)Session[Constantes.CONST_SESION_DT_TARIFARIO];            


                DataView dv = dtTarifario.Copy().DefaultView;

                string strFiltro = "";
                if (intSeccionId != 0)
                {
                    strFiltro += "tari_sSeccionId = " + intSeccionId;
                }

                if (strLetra.Contains("%") ||
                    strLetra.Contains("'"))
                    return new DataTable();

                if (strNumero != "0")
                {
                    if (strFiltro != string.Empty)
                        strFiltro += " and";
                    strFiltro += " tari_sNumero = " + strNumero;
                }
                if (!string.IsNullOrEmpty(strLetra))
                {
                    if (strFiltro != string.Empty)
                        strFiltro += " and";
                    strFiltro += " tari_vLetra like '" + strLetra + "%'";
                }
                if (strDescripcion != string.Empty)
                {
                    if (strFiltro != string.Empty)
                        strFiltro += " and";
                    strFiltro += " tari_vDescripcion like '%" + strDescripcion + "%'";
                }
                if (strIncluirExcepcion.Equals("N"))
                {
                    if (strFiltro != string.Empty)
                        strFiltro += " and";
                    strFiltro += " tari_bFlagExcepcion = 0";
                }

                dv.RowFilter = strFiltro;
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
            catch (Exception ex)
            {
                return new DataTable();
                throw new Exception(ex.Message, ex.InnerException);
            }


        }
        public static DataTable ObtenerTarifarioConsulta(HttpSessionState Session,
                            ref object[] arrParametros)
        {
            int intSeccionId = (int)arrParametros[0];//0; 
            string strNumeroLetra = arrParametros[1].ToString();//""; 
            string strDescripcion = arrParametros[2].ToString();//"";
            string strEstado = arrParametros[3].ToString();//""; 
            int intPaginaActual = (int)arrParametros[4];//0; 
            int intPaginaCantidad = (int)arrParametros[5];//0; 
            int intTotalRegistros = (int)arrParametros[6];//0; 
            int intTotalPaginas = (int)arrParametros[7];//0; 

            DataTable dtTarifario = new DataTable();
            DataTable dtTarifarioConsulta = new DataTable();
            DataTable dtFiltrado = new DataTable();

            string[] arrFiltro = ObtenerSeparacion(strNumeroLetra).Split('|');
            string strNumero = arrFiltro[0];
            string strLetra = arrFiltro[1];


            //            dtTarifarioConsulta = (DataTable)Session[Constantes.CONST_SESION_DT_TARIFARIOCONSULTAS];
            dtTarifarioConsulta = ObtenerTarifarioTotalCargaInicial();

            DataView dv = dtTarifarioConsulta.DefaultView;
            dv.Sort = "tari_sTarifarioId Asc";

            string strFiltro = "";
            if (intSeccionId != 0)
            {
                strFiltro += "tari_sSeccionId = " + intSeccionId;
            }

            if (strLetra.Contains("%") ||
                strLetra.Contains("'"))
                return new DataTable();

            if (strNumero != "0")
            {
                if (strFiltro != string.Empty)
                    strFiltro += " and";
                strFiltro += " tari_sNumero = " + strNumero;
            }
            if (!string.IsNullOrEmpty(strLetra))
            {
                if (strFiltro != string.Empty)
                    strFiltro += " and";
                strFiltro += " tari_vLetra like '" + strLetra + "%'";
            }
            if (strDescripcion != string.Empty)
            {
                if (strFiltro != string.Empty)
                    strFiltro += " and";
                strFiltro += " tari_vDescripcion like '%" + strDescripcion + "%'";
            }

            dv.RowFilter = strFiltro;
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

        
        #endregion

        #region OFICINA CONSULAR
        public static object ObtenerDatoOficinaConsular(Int32 sOficinaConsular, string strNombreCampo)
        {
            try
            {
                object objDato;

                DataTable dtOficinaConsular = new DataTable();

                dtOficinaConsular = ObtenerOficinaConsularPorId(sOficinaConsular);

                //if (Session[Constantes.CONST_SESION_OFICINACONSULTA_DT] != null)
                if (dtOficinaConsular != null)
                {
                    //DataTable dtOficinaConsular = ((DataTable)Session[Constantes.CONST_SESION_OFICINACONSULTA_DT]);
                    //if (dtOficinaConsular.Rows.Count > 0)
                    if (dtOficinaConsular.Rows.Count > 0)
                    {
                        objDato = dtOficinaConsular.Rows[0][strNombreCampo];
                        return objDato;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void CargarOficinaConsular(HttpSessionState Session, DropDownList ddlOficinaConsular, bool bSeleccion = false)
        {
            DataTable dtOficinasConsulares = new DataTable();
            dtOficinasConsulares = ObtenerOficinasConsularesCargaInicial();


            //if (Session[Constantes.CONST_SESION_DT_OFICINACONSULAR] != null)
            if (dtOficinasConsulares != null)
            {
                //DataTable dtOficinasConsulares = (DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULAR];
                DataTable dtParametrosFiltrados = new DataTable();
                if (dtOficinasConsulares != null)
                {
                    DataView dv = dtOficinasConsulares.DefaultView;
                    dv.RowFilter = "ofco_iReferenciaPadreId = 1";
                    dtParametrosFiltrados = dv.ToTable();
                }
                Util.CargarDropDownList(ddlOficinaConsular, dtParametrosFiltrados, "ofco_vNombre", "ofco_sOficinaConsularId", bSeleccion);
            }
            else
            {
                Util.CargarDropDownList(ddlOficinaConsular, new DataTable(), "ofco_vNombre", "ofco_sOficinaConsularId", bSeleccion);
            }
        }

        public static string ObtenerNombreOficinaPorId(HttpSessionState Session, int intOficinaConsularId)
        {
            DataTable dtOficinasConsulares = new DataTable();

            dtOficinasConsulares = ObtenerOficinasConsularesCargaInicial();

            string strNombreOficinaConsular = string.Empty;
            //DataTable dtOficinasConsulares = ((DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULAR]).Copy();
            DataView dv = dtOficinasConsulares.DefaultView;
            dv.RowFilter = "ofco_sOficinaConsularId =" + intOficinaConsularId;
            DataTable dtOficinaConsular = dv.ToTable();
            if (dtOficinaConsular != null)
            {
                if (dtOficinaConsular.Rows.Count > 0)
                {
                    strNombreOficinaConsular = dtOficinaConsular.Rows[0]["ofco_vNombre"].ToString();
                }
            }
            return strNombreOficinaConsular;
        }
        public static string ObtenerPaisOficinaPorId(int intOficinaConsularId)
        {
            string strNombrePaisOficinaConsular = string.Empty;

            strNombrePaisOficinaConsular = comun_Part2.ObtenerDatoOficinaConsular(intOficinaConsularId, "vPaisNombre").ToString();


            return strNombrePaisOficinaConsular;
        }
        public static string ObtenerPaisOficinaPorIdDT(HttpSessionState Session, int intOficinaConsularId)
        {
            DataTable dtOficinasConsulares = new DataTable();

            dtOficinasConsulares = ObtenerOficinasConsularesCargaInicial();

            string strNombrePaisOficinaConsular = string.Empty;
            //DataTable dtOficinasConsulares = ((DataTable)Session[Constantes.CONST_SESION_DT_OFICINACONSULAR]).Copy();
            DataView dv = dtOficinasConsulares.DefaultView;
            dv.RowFilter = "ofco_sOficinaConsularId =" + intOficinaConsularId;
            DataTable dtOficinaConsular = dv.ToTable();
            if (dtOficinaConsular != null)
            {
                if (dtOficinaConsular.Rows.Count > 0)
                {
                    strNombrePaisOficinaConsular = dtOficinaConsular.Rows[0]["vPaisNombre"].ToString();
                }
            }
            return strNombrePaisOficinaConsular;
        }
        private static string ObtenerSeparacion(string strNumeroLetra)
        {
            char[] arrNumeros = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int intContador = 0;
            bool esNumero = false;
            foreach (char caracter in strNumeroLetra.ToCharArray())
            {
                esNumero = false;
                foreach (char numero in arrNumeros)
                {
                    if (caracter == numero)
                    {
                        esNumero = true;
                        intContador++;
                        break;
                    }
                }
                if (!esNumero)
                    break;
            }
            if (intContador == 0)
            {
                strNumeroLetra = 0 + "|" +
                                strNumeroLetra.Substring(intContador, strNumeroLetra.Length - intContador);
            }
            else
            {
                strNumeroLetra = strNumeroLetra.Substring(0, intContador) + "|" +
                                 strNumeroLetra.Substring(intContador, strNumeroLetra.Length - intContador);
            }
            return strNumeroLetra;
        }
        public static DataTable ObtenerOficinasConsularesCargaInicial()
        {
            DataTable dtOficinasConsulares = new DataTable();
            SGAC.Configuracion.Sistema.BL.OficinaConsularConsultasBL BL = new SGAC.Configuracion.Sistema.BL.OficinaConsularConsultasBL();

            dtOficinasConsulares = BL.ConsultarOficinasConsularesCargaInicial();
            return dtOficinasConsulares;
        }
        public static DataTable ObtenerTarifarioCargaInicial(HttpSessionState Session)
        {
            try
            {
                Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());

                SGAC.Configuracion.Sistema.BL.TarifarioConsultasBL BL = new SGAC.Configuracion.Sistema.BL.TarifarioConsultasBL();
                DataTable dtTarifario = new DataTable();

                dtTarifario = BL.ConsultarTarifarioCargaInicial(intOficinaConsularId);

                return dtTarifario;
            }
            catch (Exception ex)
            {
                return new DataTable();
                throw new Exception(ex.Message, ex.InnerException);
            }

        }
        public static DataTable ObtenerTarifarioTotalCargaInicial()
        {

            SGAC.Configuracion.Sistema.BL.TarifarioConsultasBL BL = new SGAC.Configuracion.Sistema.BL.TarifarioConsultasBL();
            DataTable dtTarifario = new DataTable();

            dtTarifario = BL.ConsultarTarifarioTotalCargaInicial();

            return dtTarifario;
        }
        public static DataTable ObtenerOficinaConsularPorId(Int32 OficinaConsultar)
        {
            try
            {
                int intOficinaConsularId = OficinaConsultar;
                DataTable dtOficinaConsularPorId = new DataTable();
                SGAC.Configuracion.Sistema.BL.OficinaConsularConsultasBL BL = new SGAC.Configuracion.Sistema.BL.OficinaConsularConsultasBL();

                dtOficinaConsularPorId = BL.ObtenerPorId(intOficinaConsularId);
                return dtOficinaConsularPorId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        #endregion
    }
}