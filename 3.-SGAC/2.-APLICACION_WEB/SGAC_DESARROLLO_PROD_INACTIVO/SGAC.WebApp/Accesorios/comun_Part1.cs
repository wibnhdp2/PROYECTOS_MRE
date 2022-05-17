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
    public class comun_Part1
    {
        #region PARAMETRO

        public static DataTable ObtenerEstadosPorGrupo(HttpSessionState Session, string strNombreGrupo)
        {
            DataTable dtParametros = comun_Part1.ObtenerParametrosPorGrupoMRE(strNombreGrupo);

            return dtParametros;
        }

        public static DataTable ObtenerParametrosPorGrupo(HttpSessionState Session, Enumerador.enmEstadoGrupo enmEstado)
        {
            //Comun.ObtenerParametrosPorGrupoMRE(SGAC.Accesorios.Constantes.CONST_GUIA_DESPACHO_ESTADO);

            DataTable dtParametros = comun_Part1.ObtenerParametrosPorGrupoMRE(Util.ObtenerNombreGrupoParametro(enmEstado));

            return dtParametros;
        }

        //-------------------------------------------------------------
        //Fecha: 20/11/2019
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Asignar la tabla Parametros para optimizar la 
        //          carga de listas desplegables
        //-------------------------------------------------------------
        public static DataTable ObtenerParametrosPorGrupo(HttpSessionState Session, Enumerador.enmMaestro enmMaestro, DataTable dtParametros)
        {
            if (dtParametros != null)
            {
                DataView dv = dtParametros.DefaultView;
                dv.RowFilter = " grupo = '" + enmMaestro.ToString() + "'";
                DataTable dtParametrosFiltrados = dv.ToTable();

                if (enmMaestro == Enumerador.enmMaestro.DOCUMENTO_IDENTIDAD)
                {
                    dtParametrosFiltrados.DefaultView.Sort = "Id ASC";
                }
                else
                {
                    dtParametrosFiltrados.DefaultView.Sort = "valor ASC";
                }
                return dtParametrosFiltrados;
            }
            return dtParametros;
        }

        public static DataTable ObtenerParametrosPorGrupo(HttpSessionState Session, Enumerador.enmMaestro enmMaestro)
        {
            int IntTotalPages = 0;
            int intPageSize = 10000;
            DataTable dtParametros = new DataTable();
            //---------------------------------------------------
            //Fecha: 04/02/2020
            //Autor: Miguel Márquez Beltrán
            //Motivo: Consultar los datos por cada tabla.
            //---------------------------------------------------
            try
            {

                switch (enmMaestro)
                {
                    case Enumerador.enmMaestro.BANCO:
                        DataTable dtBanco = new DataTable();
                        dtBanco = ObtenerMaestroBancos(0, "A", "1", intPageSize, "N", ref IntTotalPages);

                        return dtBanco;
                    case Enumerador.enmMaestro.BASE_PERCEPCION:
                        DataTable dtBasePercepcion = new DataTable();
                        dtBasePercepcion = ObtenerMaestroBasePercepcion(0, "A", "1", intPageSize, "N", ref IntTotalPages);

                        return dtBasePercepcion;
                    case Enumerador.enmMaestro.CARGO_FUNCIONARIO_LISTA:
                        DataTable dtCargoFuncionario = new DataTable();
                        dtCargoFuncionario = ObtenerMaestroCargoFuncionario(0, "A", "1", intPageSize, "N", ref IntTotalPages);

                        return dtCargoFuncionario;

                    case Enumerador.enmMaestro.DOCUMENTO_IDENTIDAD:
                        DataTable dtDocumentoIdentidad = new DataTable();
                        dtDocumentoIdentidad = ObtenerMaestroDocumentoIdentidad(0, "", "A", "1", intPageSize, "N", ref IntTotalPages);
                        DataView dvDocumentoIdentidad = dtDocumentoIdentidad.DefaultView;
                        DataTable dtDocumentoIdentidadordenado = dvDocumentoIdentidad.ToTable();

                        dtDocumentoIdentidadordenado.DefaultView.Sort = "Id ASC";
                        return dtDocumentoIdentidadordenado;


                    case Enumerador.enmMaestro.CONTINENTE:
                        DataTable dtContinente = new DataTable();
                        dtContinente = ObtenerMaestroContinente(0, "", "A", "1", intPageSize, "N", ref IntTotalPages);

                        return dtContinente;


                    case Enumerador.enmMaestro.ESTADO_CIVIL:
                        DataTable dtEstadoCivil = new DataTable();
                        dtEstadoCivil = ObtenerMaestroEstadoCivil(0, "A", "1", intPageSize, "N", ref IntTotalPages);

                        return dtEstadoCivil;

                    case Enumerador.enmMaestro.MONEDA:
                        DataTable dtMoneda = new DataTable();
                        dtMoneda = ObtenerMaestroMoneda(0, "A", "1", intPageSize, "N", ref IntTotalPages);

                        return dtMoneda;

                    case Enumerador.enmMaestro.PROFESION:
                        DataTable dtProfesion = new DataTable();
                        dtProfesion = ObtenerMaestroProfesion(0, "A", "1", intPageSize, "N", ref IntTotalPages);

                        return dtProfesion;

                    case Enumerador.enmMaestro.OCUPACION:
                        DataTable dtOcupacion = new DataTable();
                        dtOcupacion = ObtenerMaestroOcupacion(0, "A", "1", intPageSize, "N", ref IntTotalPages);

                        return dtOcupacion;

                    case Enumerador.enmMaestro.REQUISITO_ACTUACION:
                        DataTable dtRequisitoActuacion = new DataTable();
                        dtRequisitoActuacion = ObtenerMaestroRequisitoActuacion(0, "A", "1", intPageSize, "N", ref IntTotalPages);

                        return dtRequisitoActuacion;

                    case Enumerador.enmMaestro.SECCION:
                        DataTable dtSeccion = new DataTable();
                        dtSeccion = ObtenerMaestroSeccion(0, "A", "1", intPageSize, "N", ref IntTotalPages);

                        return dtSeccion;


                    default:
                        dtParametros = ObtenerParametrosCargaInicial();
                        //DataTable dtParametros = (DataTable)Session[Constantes.CONST_SESION_DT_MAESTRA];
                        if (dtParametros != null)
                        {
                            DataView dvParametros = dtParametros.DefaultView;
                            dvParametros.RowFilter = " grupo = '" + enmMaestro.ToString() + "'";
                            DataTable dtParametrosFiltrados = dvParametros.ToTable();

                            dtParametrosFiltrados.DefaultView.Sort = "valor ASC";

                            return dtParametrosFiltrados;
                        }
                        break;
                }
                //---------------------------------------------------

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            
            return dtParametros;
        }

        
        public static DataTable ObtenerParametrosPorGrupo(HttpSessionState Session, Enumerador.enmGrupo enmGrupo)
        {
            Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
            SGAC.Configuracion.Sistema.BL.ParametroConsultasBL BL = new SGAC.Configuracion.Sistema.BL.ParametroConsultasBL();

            DataTable dtParametros = new DataTable();

            Int16 intParametroId = 0;
            string strGrupo = Util.ObtenerNombreGrupoParametro(enmGrupo);
            string strDescripcion = "";
            int intPageSize = 10000;
            int IntTotalPages = 0;
            int IntPageNumber = 1;
            string strContar = "N";
            string strTodos = "S";

            try
            {

                dtParametros = BL.ConsultarParametroMRE(intParametroId, strGrupo, strDescripcion, 0, "A", intPageSize, IntPageNumber, "1", strContar,
                                                        ref IntTotalPages, intOficinaConsularId, strTodos);
                if (enmGrupo == Enumerador.enmGrupo.CONFIG_MES)
                {
                    dtParametros.DefaultView.Sort = "id";
                }

                return dtParametros;
            }
            catch (Exception ex)
            {
                return dtParametros;
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        //----------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 21/11/2019
        // Objetivo: Acepta una lista de grupos
        //----------------------------------------------------
        public static DataTable ObtenerParametrosListaGrupos(HttpSessionState Session, Enumerador.enmGrupo[] listaGrupos)
        {
            Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
            SGAC.Configuracion.Sistema.BL.ParametroConsultasBL BL = new SGAC.Configuracion.Sistema.BL.ParametroConsultasBL();

            DataTable dtParametros = new DataTable();
            Int16 intParametroId = 0;
            string strDescripcion = "";
            int intPageSize = 10000;
            int IntTotalPages = 0;
            int IntPageNumber = 1;
            string strContar = "N";
            string strTodos = "S";
            string strCadenaGrupos = "";

            for (int i = 0; i < listaGrupos.Length; i++)
            {
                strCadenaGrupos = strCadenaGrupos + Util.ObtenerNombreGrupoParametro(listaGrupos[i]) + ",";
            }
            if (strCadenaGrupos.Length > 0)
            {
                strCadenaGrupos = strCadenaGrupos.Substring(0, strCadenaGrupos.Length - 1);
            }
            dtParametros = BL.ConsultarParametroMRE(intParametroId, strCadenaGrupos, strDescripcion, 0, "A", intPageSize, IntPageNumber, "1", strContar,
                                                   ref IntTotalPages, intOficinaConsularId, strTodos);
            return dtParametros;
        }



        //-----------------------------------------------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 15-08-2016
        // Objetivo: Obtener los registros de parametros por el nombre del grupo
        // Referencia: Requerimiento No.001_2.doc
        // Modificado: 30/10/2019
        // Motivo: Obtener los registros desde el procedimiento almacenado.
        //-----------------------------------------------------------------------------------------------------------------
        public static DataTable ObtenerParametrosPorGrupo(HttpSessionState Session, string strNombreGrupo)
        {
            Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
            SGAC.Configuracion.Sistema.BL.ParametroConsultasBL BL = new SGAC.Configuracion.Sistema.BL.ParametroConsultasBL();
            DataTable dtParametros = new DataTable();
            Int16 intParametroId = 0;
            string strDescripcion = "";
            int intPageSize = 10000;
            int IntTotalPages = 0;
            int IntPageNumber = 1;
            string strContar = "N";
            string strTodos = "S";

            dtParametros = BL.ConsultarParametroMRE(intParametroId, strNombreGrupo, strDescripcion, 0, "A", intPageSize, IntPageNumber, "1", strContar,
                                                    ref IntTotalPages, intOficinaConsularId, strTodos);

            return dtParametros;
        }

        public static DataTable ObtenerParametrosPorGrupoOrdenadoID(HttpSessionState Session, string strNombreGrupo)
        {
            Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
            SGAC.Configuracion.Sistema.BL.ParametroConsultasBL BL = new SGAC.Configuracion.Sistema.BL.ParametroConsultasBL();
            DataTable dtParametros = new DataTable();
            Int16 intParametroId = 0;
            string strDescripcion = "";
            int intPageSize = 10000;
            int IntPageNumber = 1;
            int IntTotalPages = 0;
            string strContar = "N";
            string strTodos = "S";

            dtParametros = BL.ConsultarParametroMRE(intParametroId, strNombreGrupo, strDescripcion, 0, "A", intPageSize, IntPageNumber, "1", strContar,
                                                    ref IntTotalPages, intOficinaConsularId, strTodos);
            //DataTable dtParametros = (DataTable)Session[Constantes.CONST_SESION_DT_PARAMETRO];
            if (dtParametros != null)
            {

                dtParametros.DefaultView.Sort = "id ASC";
            }
            return dtParametros;
        }

        //----------------------------------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 15-08-2016
        // Objetivo: Obtener los registros de la tabla Maestro por el nombre del grupo
        //----------------------------------------------------------------------------------------------------
        //public static DataTable ObtenerEstadoPorGrupo(HttpSessionState Session, string strNombreGrupo)
        public static DataTable ObtenerParametrosPorGrupo(DataTable dtParametros, string strNombreGrupo)
        {
            if (dtParametros != null)
            {
                DataView dv = dtParametros.DefaultView;
                dv.RowFilter = " grupo = '" + strNombreGrupo + "'";
                DataTable dtParametrosFiltrados = dv.ToTable();
                return dtParametrosFiltrados;
            }
            return dtParametros;
        }
        //-----------------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------
        // Autor: Miguel Angel Márquez Beltrán
        // Fecha: 29-10-2019
        // Objetivo: Obtener los registros de la tabla Maestro por el nombre del grupo
        //----------------------------------------------------------------------------------------------------
        public static DataTable ObtenerParametrosPorGrupoMRE(Int16 intEstadoId = 0, string strNombreGrupo = "", string strDesCorta = "")
        {
            Int16 intCantidadPaginas = 0;
            Int16 intPageSize = 10000;
            Int16 intNumeroPagina = 1;
            string strContar = "N";
            string strEstado = "A";
            DataTable dtGrupoEstado = new DataTable();
            SGAC.Configuracion.Maestro.BL.EstadoConsultaBL BL = new SGAC.Configuracion.Maestro.BL.EstadoConsultaBL();
            dtGrupoEstado = BL.ConsultaGrupoMRE(ref intCantidadPaginas, intEstadoId, strDesCorta, strEstado,
                                                intPageSize, intNumeroPagina, strContar, strNombreGrupo);
            return dtGrupoEstado;
        }
        public static DataTable ObtenerParametrosPorGrupoMRE(string strNombreGrupo = "")
        {
            Int16 intCantidadPaginas = 0;
            Int16 intPageSize = 10000;
            Int16 intNumeroPagina = 1;
            string strDesCorta = "";
            string strContar = "N";
            string strEstado = "A";
            Int16 intEstadoId = 0;
            DataTable dtGrupoEstado = new DataTable();
            SGAC.Configuracion.Maestro.BL.EstadoConsultaBL BL = new SGAC.Configuracion.Maestro.BL.EstadoConsultaBL();
            dtGrupoEstado = BL.ConsultaGrupoMRE(ref intCantidadPaginas, intEstadoId, strDesCorta, strEstado,
                                                intPageSize, intNumeroPagina, strContar, strNombreGrupo);
            return dtGrupoEstado;
        }
        public static DataTable ObtenerParametrosPorGrupoMRE(Int16 intEstadoId = 0, string strNombreGrupo = "")
        {
            Int16 intCantidadPaginas = 0;
            Int16 intPageSize = 10000;
            Int16 intNumeroPagina = 1;
            string strDesCorta = "";
            string strContar = "N";
            string strEstado = "A";
            DataTable dtGrupoEstado = new DataTable();
            SGAC.Configuracion.Maestro.BL.EstadoConsultaBL BL = new SGAC.Configuracion.Maestro.BL.EstadoConsultaBL();
            dtGrupoEstado = BL.ConsultaGrupoMRE(ref intCantidadPaginas, intEstadoId, strDesCorta, strEstado,
                                                intPageSize, intNumeroPagina, strContar, strNombreGrupo);
            return dtGrupoEstado;
        }

        public static DataTable ObtenerParametroPorId(HttpSessionState Session, int intParametroId,
            Enumerador.enmGrupo enmGrupo = Enumerador.enmGrupo.NINGUNO)
        {
            Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
            SGAC.Configuracion.Sistema.BL.ParametroConsultasBL BL = new SGAC.Configuracion.Sistema.BL.ParametroConsultasBL();
            DataTable dtParametros = new DataTable();

            string strGrupo = "";
            string strDescripcion = "";
            int intPageSize = 10000;
            int IntPageNumber = 1;
            int IntTotalPages = 0;
            string strContar = "N";
            string strTodos = "S";

            dtParametros = BL.ConsultarParametroMRE(Convert.ToInt16(intParametroId), strGrupo, strDescripcion, 0, "A", intPageSize, IntPageNumber, "1", strContar,
                                                       ref IntTotalPages, intOficinaConsularId, strTodos);
            return dtParametros;

        }
        public static DataTable ObtenerParametroPorId(HttpSessionState Session, int intEstadoId, Enumerador.enmEstadoGrupo enmEstadoGrupo)
        {

            DataTable dtParametro = comun_Part1.ObtenerParametrosPorGrupoMRE(Convert.ToInt16(intEstadoId), "");

            //return dtFiltrado;
            return dtParametro;
        }
       
        public static DataTable ObtenerDocumentoIdentidad()
        {           
            int IntTotalPages = 0;
            int intPageSize = 10000;
            DataTable dtDocumentoIdentidad = new DataTable();
            dtDocumentoIdentidad = ObtenerMaestroDocumentoIdentidad(0, "", "A", "1", intPageSize, "N", ref IntTotalPages);
            DataView dvDocumentoIdentidad = dtDocumentoIdentidad.DefaultView;
            DataTable dtDocumentoIdentidadordenado = dvDocumentoIdentidad.ToTable();

            dtDocumentoIdentidadordenado.DefaultView.Sort = "Id ASC";
            return dtDocumentoIdentidadordenado;


        }
        public static string ObtenerParametroDatoPorCampo(HttpSessionState Session, string strGrupo, string strParametro,
            string strCampoNombre = "")
        {
            Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
            SGAC.Configuracion.Sistema.BL.ParametroConsultasBL BL = new SGAC.Configuracion.Sistema.BL.ParametroConsultasBL();

            DataTable dtParametros = new DataTable();
            Int16 intParametroId = 0;

            int intPageSize = 10000;
            int IntPageNumber = 1;
            int IntTotalPages = 0;
            string strContar = "N";
            string strTodos = "S";


            dtParametros = BL.ConsultarParametroMRE(intParametroId, strGrupo, strParametro, 0, "A", intPageSize, IntPageNumber, "1", strContar,
                                                    ref IntTotalPages, intOficinaConsularId, strTodos);

            string strDato = string.Empty;

            if (dtParametros.Rows.Count > 0)
                strDato = dtParametros.Rows[0][strCampoNombre].ToString();
            return strDato;
        }
        public static string ObtenerParametroDatoPorCampo(HttpSessionState Session, Enumerador.enmGrupo enmGrupo,
                                    int intParametroId, string strCampoNombre = "")
        {
            try
            {
                Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
                SGAC.Configuracion.Sistema.BL.ParametroConsultasBL BL = new SGAC.Configuracion.Sistema.BL.ParametroConsultasBL();
                DataTable dtParametros = new DataTable();
                string strGrupo = Util.ObtenerNombreGrupoParametro(enmGrupo);
                string strDescripcion = "";
                int intPageSize = 10000;
                int IntPageNumber = 1;
                int IntTotalPages = 0;
                string strContar = "N";
                string strTodos = "S";

                dtParametros = BL.ConsultarParametroMRE(Convert.ToInt16(intParametroId), strGrupo, strDescripcion, 0, "A", intPageSize, IntPageNumber, "1", strContar,
                                                        ref IntTotalPages, intOficinaConsularId, strTodos);


                string strDato = string.Empty;

                if (dtParametros.Rows.Count > 0)
                    strDato = dtParametros.Rows[0][strCampoNombre].ToString();
                return strDato;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            
        }
        public static string ObtenerParametroDatoPorCampo(HttpSessionState Session, string strGrupo, int intParametroId,
                                                            string strCampoNombre = "")
        {
            try
            {
                Int16 intOficinaConsularId = Convert.ToInt16(Session[Constantes.CONST_SESION_OFICINACONSULAR_ID].ToString());
                SGAC.Configuracion.Sistema.BL.ParametroConsultasBL BL = new SGAC.Configuracion.Sistema.BL.ParametroConsultasBL();
                DataTable dtParametros = new DataTable();

                string strDescripcion = "";
                int intPageSize = 10000;
                int IntPageNumber = 1;
                int IntTotalPages = 0;
                string strContar = "N";
                string strTodos = "S";

                dtParametros = BL.ConsultarParametroMRE(Convert.ToInt16(intParametroId), strGrupo, strDescripcion, 0, "A", intPageSize, IntPageNumber, "1", strContar,
                                                        ref IntTotalPages, intOficinaConsularId, strTodos);



                string strDato = string.Empty;

                if (dtParametros.Rows.Count > 0)
                    strDato = dtParametros.Rows[0][strCampoNombre].ToString();

                return strDato;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public static DataTable ObtenerTablasMaestras(HttpSessionState Session)
        {
            DataTable dtParametros = ObtenerParametrosPorGrupo(Session, Enumerador.enmGrupo.CONFIG_TABLAS);
            if (dtParametros != null)
            {
                DataView dv = dtParametros.DefaultView;
                dv.RowFilter = "descripcion LIKE 'MA%'";
                DataTable dtParametrosFiltrados = dv.ToTable();
                return dtParametrosFiltrados;
            }
            return dtParametros;
        }

        public static DataTable ObtenerParametrosCargaInicial()
        {
            SGAC.Configuracion.Maestro.BL.TablaMaestraConsultaBL BL = new SGAC.Configuracion.Maestro.BL.TablaMaestraConsultaBL();
            DataTable dtTablasMaestras = new DataTable();

            dtTablasMaestras = BL.ConsultarTablasMaestrasCargaInicial();

            return dtTablasMaestras;
        }
        //--------------------------------------------
        //Fecha: 04/02/2020
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Consultar los registros de Bancos.
        //--------------------------------------------

        public static DataTable ObtenerMaestroBancos(short intBancoId, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            SGAC.Configuracion.Maestro.BL.BancoConsultaBL BL = new SGAC.Configuracion.Maestro.BL.BancoConsultaBL();
            DataTable dtMaestroBancos = new DataTable();
            dtMaestroBancos = BL.Consultar_Banco(intBancoId, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);

            return dtMaestroBancos;
        }
        //-----------------------------------------------------------------
        //Fecha: 04/02/2020
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Consultar los registros de Documentos de Identidad.
        //-----------------------------------------------------------------
        public static DataTable ObtenerMaestroDocumentoIdentidad(short intDocumentoIdentidadId, string strDescripcionCorta, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            SGAC.Configuracion.Maestro.BL.DocumentoIdentidadConsultaBL BL = new SGAC.Configuracion.Maestro.BL.DocumentoIdentidadConsultaBL();
            DataTable dtMaestroDocumentosIdentidad = new DataTable();
            dtMaestroDocumentosIdentidad = BL.Consultar_DocumentoIdentidad(intDocumentoIdentidadId, strDescripcionCorta, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);

            return dtMaestroDocumentosIdentidad;
        }
        
        //-----------------------------------------------
        //Fecha: 04/02/2020
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Consultar los registros de Continente.
        //-----------------------------------------------
        public static DataTable ObtenerMaestroContinente(int intContinenteId, string strNombre, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            SGAC.Configuracion.Maestro.BL.ContinenteConsultaBL BL = new SGAC.Configuracion.Maestro.BL.ContinenteConsultaBL();
            DataTable dtMaestroContinente = new DataTable();
            dtMaestroContinente = BL.Consultar_Continente(intContinenteId, strNombre, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);

            return dtMaestroContinente;
        }
        //--------------------------------------------------
        //Fecha: 04/02/2020
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Consultar los registros de Estado Civil.
        //--------------------------------------------------
        public static DataTable ObtenerMaestroEstadoCivil(int intEstacoCivilId, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            SGAC.Configuracion.Maestro.BL.EstadoCivilConsultaBL BL = new SGAC.Configuracion.Maestro.BL.EstadoCivilConsultaBL();
            DataTable dtMaestroEstadoCivil = new DataTable();
            dtMaestroEstadoCivil = BL.Consultar_EstadoCivil(intEstacoCivilId, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);

            return dtMaestroEstadoCivil;
        }
        //--------------------------------------------------
        //Fecha: 04/02/2020
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Consultar los registros de Moneda.
        //--------------------------------------------------
        public static DataTable ObtenerMaestroMoneda(int intMonedaId, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            SGAC.Configuracion.Maestro.BL.MonedaConsultaBL BL = new SGAC.Configuracion.Maestro.BL.MonedaConsultaBL();
            DataTable dtmaestroMoneda = new DataTable();
            dtmaestroMoneda = BL.Consultar_Moneda(intMonedaId, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);

            return dtmaestroMoneda;
        }
        //-----------------------------------------------
        //Fecha: 04/02/2020
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Consultar los registros de Ocupación.
        //-----------------------------------------------
        public static DataTable ObtenerMaestroOcupacion(int intOcupacionId, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            SGAC.Configuracion.Maestro.BL.OcupacionConsultaBL BL = new SGAC.Configuracion.Maestro.BL.OcupacionConsultaBL();
            DataTable dtMaestroOcupacion = new DataTable();
            dtMaestroOcupacion = BL.Consultar_Ocupacion(intOcupacionId, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);

            return dtMaestroOcupacion;
        }
        //--------------------------------------------------
        //Fecha: 04/02/2020
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Consultar los registros de Sección.
        //--------------------------------------------------
        public static DataTable ObtenerMaestroSeccion(short intSeccionId, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            SGAC.Configuracion.Maestro.BL.SeccionConsultaBL BL = new SGAC.Configuracion.Maestro.BL.SeccionConsultaBL();
            DataTable dtMaestroSeccion = new DataTable();
            dtMaestroSeccion = BL.Consultar_Seccion(intSeccionId, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);

            return dtMaestroSeccion;
        }
        //--------------------------------------------------
        //Fecha: 05/02/2020
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Consultar los registros de Profesión.
        //--------------------------------------------------
        public static DataTable ObtenerMaestroProfesion(short intProfesionId, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            SGAC.Configuracion.Maestro.BL.ProfesionConsultaBL BL = new SGAC.Configuracion.Maestro.BL.ProfesionConsultaBL();
            DataTable dtMaestroProfesion = new DataTable();
            dtMaestroProfesion = BL.Consultar_Profesion(intProfesionId, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);

            return dtMaestroProfesion;
        }
        //-------------------------------------------------------------
        //Fecha: 05/02/2020
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Consultar los registros de Cargo-Funcionario.
        //-------------------------------------------------------------
        public static DataTable ObtenerMaestroCargoFuncionario(short intCargoFuncionarioId, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            SGAC.Configuracion.Maestro.BL.CargoFuncionarioConsultaBL BL = new SGAC.Configuracion.Maestro.BL.CargoFuncionarioConsultaBL();
            DataTable dtMaestroCargoFuncionario = new DataTable();
            dtMaestroCargoFuncionario = BL.Consultar_CargoFuncionario(intCargoFuncionarioId, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);

            return dtMaestroCargoFuncionario;
        }
        //------------------------------------------------------
        //Fecha: 05/02/2020
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Consultar los registros de Base Percepción.
        //------------------------------------------------------
        public static DataTable ObtenerMaestroBasePercepcion(short intBasePercepcionId, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            SGAC.Configuracion.Maestro.BL.BasePercepcionConsultaBL BL = new SGAC.Configuracion.Maestro.BL.BasePercepcionConsultaBL();
            DataTable dtMaestroBasePercepcion = new DataTable();
            dtMaestroBasePercepcion = BL.Consultar_BasePercepcion(intBasePercepcionId, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);

            return dtMaestroBasePercepcion;
        }
        //---------------------------------------------------------
        //Fecha: 05/02/2020
        //Autor: Miguel Márquez Beltrán
        //Objetivo: Consultar los registros de Requisito_Actuacion.
        //---------------------------------------------------------
        public static DataTable ObtenerMaestroRequisitoActuacion(short intRequisitoActuacionId, string strEstado, string StrCurrentPage, int IntPageSize, string strContar, ref int IntTotalPages)
        {
            SGAC.Configuracion.Maestro.BL.RequisitoActuacionConsultaBL BL = new SGAC.Configuracion.Maestro.BL.RequisitoActuacionConsultaBL();
            DataTable dtMaestroRequisitoActuacion = new DataTable();
            dtMaestroRequisitoActuacion = BL.Consultar_RequisitoActuacion(intRequisitoActuacionId, strEstado, StrCurrentPage, IntPageSize, strContar, ref IntTotalPages);

            return dtMaestroRequisitoActuacion;
        }

        #endregion
    }

}