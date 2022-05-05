using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SGAC.Reportes.DA
{
    public class ReportesGerencialesConsultasDA
    {

        private string strConnectionName = string.Empty;

        public ReportesGerencialesConsultasDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public DataSet ObtenerMayorVentaDetalle(DateTime dFecInicio, DateTime dFecFin)
        {
            try {
                using (SqlConnection cnn = new SqlConnection(strConnectionName)) {
                    using (SqlCommand cmd = new SqlCommand("[PN_REPORTES].[USP_RP_REGE_MAYOR_VENTA_DETALLE]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FECHAINICIO", SqlDbType.DateTime).Value = dFecInicio;
                        cmd.Parameters.Add("@FECHAFIN", SqlDbType.DateTime).Value = dFecFin;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd)) {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex) {
                return null;
            }
        }

        public DataSet ObtenerVentaMes(Int16 OficinaConsular, DateTime dFecInicio, DateTime dFecFin)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REPORTES].[USP_RP_REGE_VENTAS_MES]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FECHAINICIO", SqlDbType.DateTime).Value = dFecInicio;
                        cmd.Parameters.Add("@FECHAFIN", SqlDbType.DateTime).Value = dFecFin;
                        cmd.Parameters.Add("@ofco_sOficinaConsularId",SqlDbType.Int).Value = OficinaConsular;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        //-------------------------------------------------------------------------
        //Autor: Miguel Márquez Beltrán
        //Fecha: 26/06/2017
        //Objetivo: Obtener el consolidado de las actuaciones por tipo de pago.
        //-------------------------------------------------------------------------

        public DataSet ObtenerConsolidadoActuacionesPorTipoPago(Int16 sOficinaConsular, string strPeriodo)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REPORTES].[USP_RP_CONSOLIDADO_ACTUACIONES_TIPO_PAGO_MRE]", cnn))
                    {                        
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.Int).Value = sOficinaConsular;
                        cmd.Parameters.Add("@vPeriodo", SqlDbType.Char).Value = strPeriodo;                        

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        public DataSet ObtenerTarifaConsularPaís(string tari_sTarifarioId, DateTime dFecInicio, DateTime dFecFin)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REPORTES].[USP_RP_REGE_TARIFA_CONSULAR_PAIS]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@tari_sTarifarioId", SqlDbType.VarChar).Value = tari_sTarifarioId;
                        cmd.Parameters.Add("@FECHAINICIO", SqlDbType.DateTime).Value = dFecInicio;
                        cmd.Parameters.Add("@FECHAFIN", SqlDbType.DateTime).Value = dFecFin;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet ObtenerRecordActuacion(DateTime dFecInicio, DateTime dFecFin)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REPORTES].[USP_RP_REGE_RECORD_ACTUACIONES]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FECHAINICIO", SqlDbType.DateTime).Value = dFecInicio;
                        cmd.Parameters.Add("@FECHAFIN", SqlDbType.DateTime).Value = dFecFin;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet ObtenerMayorVentaPais(DateTime dFecInicio, DateTime dFecFin)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REPORTES].[USP_RP_REGE_MAYOR_VENTA_PAIS]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FECHAINICIO", SqlDbType.DateTime).Value = dFecInicio;
                        cmd.Parameters.Add("@FECHAFIN", SqlDbType.DateTime).Value = dFecFin;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet ObtenerMayorVentaContinente(DateTime dFecInicio, DateTime dFecFin)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REPORTES].[USP_RP_REGE_CONTINENTE]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FECHAINICIO",SqlDbType.DateTime).Value = dFecInicio;
                        cmd.Parameters.Add("@FECHAFIN", SqlDbType.DateTime).Value = dFecFin;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet ObtenerCategoriaVentas(DateTime dFecInicio, DateTime dFecFin)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REPORTES].[USP_RP_REGE_CATEGORIA_VENTAS]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FECHAINICIO", SqlDbType.DateTime).Value = dFecInicio;
                        cmd.Parameters.Add("@FECHAFIN", SqlDbType.DateTime).Value = dFecFin;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet ObtenerConsolidado(string strContinente, string strPais, Int16? idcategoriaoficinaconsular,
                                          Int16? idoficinaconsular, string idtarifaconsular, Int16? idtipopago,
                                          DateTime datFechainicio, DateTime datFechafin,
            Int16? intUsuarioId)
        {
            // REPORTE Nº 25 DEL EXCEL
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REPORTES].[USP_RP_REGE_CONSOLIDADO]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@cContinente", SqlDbType.Char).Value = strContinente;
                        cmd.Parameters.Add("@cPais", SqlDbType.Char).Value = strPais;
                        cmd.Parameters.Add("@sCategoriaOficinaConsularId", SqlDbType.SmallInt).Value = idcategoriaoficinaconsular;
                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.SmallInt).Value = idoficinaconsular;
                        if (Convert.ToInt32(idtarifaconsular) == 0)
                            cmd.Parameters.Add("@sTarifarioId", SqlDbType.SmallInt).Value = DBNull.Value;
                        else
                            cmd.Parameters.Add("@sTarifarioId", SqlDbType.SmallInt).Value = idtarifaconsular;
                        cmd.Parameters.Add("@sTipoPagoId", SqlDbType.SmallInt).Value = idtipopago;
                        cmd.Parameters.Add("@dFechaInicio", SqlDbType.DateTime).Value = datFechainicio;
                        cmd.Parameters.Add("@dFechaFin", SqlDbType.DateTime).Value = datFechafin;
                        cmd.Parameters.Add("@sUsuarioId", SqlDbType.SmallInt).Value = intUsuarioId;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet ObtenerRecordVenta(Int16? sUsuarioCreacion, Int16? sOficinaConsularId, DateTime fechainicio, DateTime fechafin)
        {
            // REPORTE Nº 29 DEL EXCEL
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REPORTES].[USP_RP_REGE_RECORD_VENTA]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@actu_sUsuarioCreacion", SqlDbType.SmallInt).Value = sUsuarioCreacion;
                        cmd.Parameters.Add("@OficinaConsularId", SqlDbType.SmallInt).Value = sOficinaConsularId;
                        cmd.Parameters.Add("@fechainicio", SqlDbType.DateTime).Value = fechainicio;
                        cmd.Parameters.Add("@fechafin", SqlDbType.DateTime).Value = fechafin;


                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet ObtenerRGExCategoria(DateTime fechainicio, DateTime fechafin)
        {
            // REPORTE Nº 34 DEL EXCEL
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REPORTES].[USP_RP_REGE_CATEGORIA]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FECHAINICIO", SqlDbType.DateTime).Value = fechainicio;
                        cmd.Parameters.Add("@FECHAFIN", SqlDbType.DateTime).Value = fechafin;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataSet USP_REPORTE_AUTOADHESIVOS_USUARIO_OFICINACONSULAR(int intOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin, int intUsuarioId, Int16 sEstadoInsumo)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_REPORTE_AUTOADHESIVOS_USUARIO_OFICINACONSULAR", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.BigInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@dFechaInicio", SqlDbType.DateTime).Value = dFechaInicio;
                        cmd.Parameters.Add("@dFechaFin", SqlDbType.DateTime).Value = dFechaFin;
                        cmd.Parameters.Add("@sUsuarioId", SqlDbType.SmallInt).Value = intUsuarioId;
                        cmd.Parameters.Add("@ESTADOINSUMO", SqlDbType.SmallInt).Value = sEstadoInsumo;
                  
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

           
            
        }
        //----------------------------------------------------
        //Fecha: 07/05/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Asignar el parametro opcional bTramiteSinVincular
        //        y se adiciona el parametro: @cTramitesSinVincular
        //Requerimiento: OBSERVACIONES_SGAC_06052021.doc  Item 1.
        //----------------------------------------------------
        public DataSet REPORTE_ACTUACIONES_USUARIO_OFICINACONSULAR(int intOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin, int intUsuarioId, int intTipoPago, int intTipoRol, int intTarifaId = 0, bool bTramiteSinVincular = false)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_REPORTE_ACTUACIONES_USUARIO_OFICINACONSULAR", cnn))
                    {
                        //cmd.CommandTimeout = 30; //valor predeterminado
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.BigInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@dFechaInicio", SqlDbType.DateTime).Value = dFechaInicio;
                        cmd.Parameters.Add("@dFechaFin", SqlDbType.DateTime).Value = dFechaFin;
                        cmd.Parameters.Add("@sUsuarioId", SqlDbType.SmallInt).Value = intUsuarioId;
                        cmd.Parameters.Add("@sTipoPago", SqlDbType.SmallInt).Value = intTipoPago;
                        cmd.Parameters.Add("@sTipoRol", SqlDbType.SmallInt).Value = intTipoRol;
                        cmd.Parameters.Add("@sTarifarioId", SqlDbType.SmallInt).Value = intTarifaId;
                        if (bTramiteSinVincular)
                        {
                            cmd.Parameters.Add("@cTramitesSinVincular", SqlDbType.Char, 1).Value = "S";
                        }
                        else
                        {
                            cmd.Parameters.Add("@cTramitesSinVincular", SqlDbType.Char, 1).Value = "N";
                        }
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
 
        }
        //------------------------------------------------
        //Fecha: 29/12/2016
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE ACTUACIONES PARA TARIFA 20B
        //------------------------------------------------
        //----------------------------------------------------
        //Fecha: 07/05/2021
        //Autor: Miguel Márquez Beltrán
        //Motivo: Asignar el parametro opcional bTramiteSinVincular
        //        y se adiciona el parametro: @cTramitesSinVincular
        //Requerimiento: OBSERVACIONES_SGAC_06052021.doc  Item 1.
        //----------------------------------------------------
        public DataSet REPORTE_ACTUACIONES_USUARIO_OFICINACONSULAR_PARATARIFA20B(int intOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin, int intUsuarioId, int intTipoPago, int intTipoRol, int intTarifaId = 0, int intClasificacionId = 0, bool bTramiteSinVincular = false)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_REPORTE_ACTUACIONES_USUARIO_OFICINACONSULAR_MRE", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.BigInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@dFechaInicio", SqlDbType.DateTime).Value = dFechaInicio;
                        cmd.Parameters.Add("@dFechaFin", SqlDbType.DateTime).Value = dFechaFin;
                        cmd.Parameters.Add("@sUsuarioId", SqlDbType.SmallInt).Value = intUsuarioId;
                        cmd.Parameters.Add("@sTipoPago", SqlDbType.SmallInt).Value = intTipoPago;
                        cmd.Parameters.Add("@sTipoRol", SqlDbType.SmallInt).Value = intTipoRol;
                        cmd.Parameters.Add("@sTarifarioId", SqlDbType.SmallInt).Value = intTarifaId;
                        cmd.Parameters.Add("@sClasificacionId", SqlDbType.SmallInt).Value = intClasificacionId;
                        if (bTramiteSinVincular)
                        {
                            cmd.Parameters.Add("@cTramitesSinVincular", SqlDbType.Char, 1).Value = "S";
                        }
                        else
                        {
                            cmd.Parameters.Add("@cTramitesSinVincular", SqlDbType.Char, 1).Value = "N";
                        }
                        

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        //------------------------------------------------
        //Fecha: 29/12/2016
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE DE PERSONAS
        //------------------------------------------------
        public DataSet USP_REPORTE_PERSONAS_USUARIO_OFICINACONSULAR(int intOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin, 
            Int16 EstadoCivil = 0,
            Int16 GeneroId = 0,
            string CodigoPostal = "",
            Int16 OcupacionId = 0,
            Int16 ProfesionId = 0,
            Int16 GradoInstruccionID = 0,
            bool BuscarResidencia = false,
            Int16 TipoResidencia = 0,
            string ResidenciaUbigeo = "",
            Int16 Nacionalidad = 0)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REPORTE_PERSONAS_USUARIO_OFICINACONSULAR", cnn))
                    {
                        cmd.CommandTimeout = 300; // 5 minutos
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.SmallInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@dFechaInicio", SqlDbType.DateTime).Value = dFechaInicio;
                        cmd.Parameters.Add("@dFechaFin", SqlDbType.DateTime).Value = dFechaFin;
                        //cmd.Parameters.Add("@sUsuarioId", SqlDbType.SmallInt).Value = intUsuarioId;

                        cmd.Parameters.Add("@pers_sEstadoCivilId", SqlDbType.SmallInt).Value = EstadoCivil;
                        cmd.Parameters.Add("@pers_sGeneroId", SqlDbType.SmallInt).Value = GeneroId;
                        cmd.Parameters.Add("@vCodigoPostal", SqlDbType.VarChar).Value = CodigoPostal;
                        cmd.Parameters.Add("@pers_sOcupacionId", SqlDbType.SmallInt).Value = OcupacionId;
                        cmd.Parameters.Add("@pers_sProfesionId", SqlDbType.SmallInt).Value = ProfesionId;
                        cmd.Parameters.Add("@pers_sGradoInstruccionId", SqlDbType.SmallInt).Value = GradoInstruccionID;
                        cmd.Parameters.Add("@bBuscarResidencia", SqlDbType.Bit).Value = BuscarResidencia;
                        cmd.Parameters.Add("@resi_sResidenciaTipoId", SqlDbType.SmallInt).Value = TipoResidencia;
                        cmd.Parameters.Add("@resi_cResidenciaUbigeo", SqlDbType.VarChar).Value = ResidenciaUbigeo;
                        cmd.Parameters.Add("@pers_sNacionalidadId", SqlDbType.SmallInt).Value = Nacionalidad;
                        
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch 
            {
                return null;
            }
        }
        //------------------------------------------------
        //Fecha: 02/01/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE CANTIDAD DE ACTUACIONES POR CONSULADO
        //------------------------------------------------
        public DataSet ObtenerCantidadActuacionesPorConsulado(string strContinente, string strPais, Int16? idcategoriaoficinaconsular,
                                          Int16? idoficinaconsular, string idtarifaconsular, Int16? idtipopago,
                                          DateTime datFechainicio, DateTime datFechafin,
            Int16? intUsuarioId)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REPORTES].[USP_RP_ACTUACIONES_CANTIDAD_CONSULADOS]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@cContinente", SqlDbType.Char).Value = strContinente;
                        cmd.Parameters.Add("@cPais", SqlDbType.Char).Value = strPais;
                        cmd.Parameters.Add("@sCategoriaOficinaConsularId", SqlDbType.SmallInt).Value = idcategoriaoficinaconsular;
                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.SmallInt).Value = idoficinaconsular;
                        if (Convert.ToInt32(idtarifaconsular) == 0)
                            cmd.Parameters.Add("@sTarifarioId", SqlDbType.SmallInt).Value = DBNull.Value;
                        else
                            cmd.Parameters.Add("@sTarifarioId", SqlDbType.SmallInt).Value = idtarifaconsular;
                        cmd.Parameters.Add("@sTipoPagoId", SqlDbType.SmallInt).Value = idtipopago;
                        cmd.Parameters.Add("@dFechaInicio", SqlDbType.DateTime).Value = datFechainicio;
                        cmd.Parameters.Add("@dFechaFin", SqlDbType.DateTime).Value = datFechafin;
                        cmd.Parameters.Add("@sUsuarioId", SqlDbType.SmallInt).Value = intUsuarioId;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch 
            {
                return null;
            }
        }
        //------------------------------------------------
        //Fecha: 02/01/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE CANTIDAD DE TARIFAS
        //------------------------------------------------
        public DataSet ObtenerCantidadMaximaTarifas(string strContinente, string strPais, Int16? idcategoriaoficinaconsular,
                                          Int16? idoficinaconsular, string idtarifaconsular, Int16? idtipopago,
                                          DateTime datFechainicio, DateTime datFechafin,
            Int16? intUsuarioId)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REPORTES].[USP_RP_TARIFAS_CANTIDAD]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@cContinente", SqlDbType.Char).Value = strContinente;
                        cmd.Parameters.Add("@cPais", SqlDbType.Char).Value = strPais;
                        cmd.Parameters.Add("@sCategoriaOficinaConsularId", SqlDbType.SmallInt).Value = idcategoriaoficinaconsular;
                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.SmallInt).Value = idoficinaconsular;
                        if (Convert.ToInt32(idtarifaconsular) == 0)
                            cmd.Parameters.Add("@sTarifarioId", SqlDbType.SmallInt).Value = DBNull.Value;
                        else
                            cmd.Parameters.Add("@sTarifarioId", SqlDbType.SmallInt).Value = idtarifaconsular;
                        cmd.Parameters.Add("@sTipoPagoId", SqlDbType.SmallInt).Value = idtipopago;
                        cmd.Parameters.Add("@dFechaInicio", SqlDbType.DateTime).Value = datFechainicio;
                        cmd.Parameters.Add("@dFechaFin", SqlDbType.DateTime).Value = datFechafin;
                        cmd.Parameters.Add("@sUsuarioId", SqlDbType.SmallInt).Value = intUsuarioId;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }


        //------------------------------------------------
        //Fecha: 11/04/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE RANKING DE RECAUDACIÓN Y ACTUACIONES CONSULARES 
        //------------------------------------------------
        public DataSet ObtenerRankingRecaudacion(Int16? idoficinaconsular,
                                          DateTime datFechainicio, DateTime datFechafin)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_CONTABILIDAD].[USP_CO_REPORTE_RANKING_RECAUDACION_ACTUACIONES]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.SmallInt).Value = idoficinaconsular;
                        cmd.Parameters.Add("@dFechaInicio", SqlDbType.DateTime).Value = datFechainicio;
                        cmd.Parameters.Add("@dFechaFin", SqlDbType.DateTime).Value = datFechafin;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        //------------------------------------------------
        //Fecha: 12/07/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE RANKING DE CAPTACIÓN CONSULAR
        //------------------------------------------------
        public DataSet ObtenerRankingCaptacion(Int16? idoficinaconsular,
                                          Int16 Anio, string Ordenado, string pagoLima)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_CONTABILIDAD].[USP_CO_REPORTE_RANKING_CAPTACION_CONSULAR]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.SmallInt).Value = idoficinaconsular;
                        cmd.Parameters.Add("@sAnio", SqlDbType.SmallInt).Value = Anio;
                        cmd.Parameters.Add("@cOrdenado", SqlDbType.VarChar).Value = Ordenado;
                        cmd.Parameters.Add("@cIncluyePagosLima", SqlDbType.VarChar).Value = pagoLima;
                        
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        //------------------------------------------------
        //Fecha: 12/07/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE CUADRO DE SALDOS DE AUTOAHDESIVOS
        //------------------------------------------------
        public DataSet ObtenerCuadroAutoahdesivos(Int16? idoficinaconsular,
                                          Int16 Anio, string Ordenado)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_CONTABILIDAD].[USP_CO_REPORTE_CUADRO_SALDOS_AUTOADHESIVOS]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.SmallInt).Value = idoficinaconsular;
                        cmd.Parameters.Add("@sAnio", SqlDbType.SmallInt).Value = Anio;
                        cmd.Parameters.Add("@cOrdenado", SqlDbType.VarChar).Value = Ordenado;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        //------------------------------------------------
        //Fecha: 12/07/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE CUADRO DE SALDOS DE AUTOAHDESIVOS UTILIZADOS
        //------------------------------------------------
        public DataSet ObtenerCuadroAutoahdesivosUtilizados(Int16? idoficinaconsular,
                                          Int16 Anio, string Ordenado)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_CONTABILIDAD].[USP_CO_REPORTE_CUADRO_SALDOS_AUTOADHESIVOS_UTILIZADOS]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.SmallInt).Value = idoficinaconsular;
                        cmd.Parameters.Add("@sAnio", SqlDbType.SmallInt).Value = Anio;
                        cmd.Parameters.Add("@cOrdenado", SqlDbType.VarChar).Value = Ordenado;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        //------------------------------------------------
        //Fecha: 11/10/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE ACTUACIONES_PERIODO_USUARIO_OFICINACONSULARS
        //------------------------------------------------
        public DataSet USP_REPORTE_ACTUACIONES_PERIODO_USUARIO_OFICINACONSULAR(int intOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin, int intUsuarioId, int intTipoPago, int intTipoRol, int intTarifaId = 0)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_REPORTE_ACTUACIONES_PERIODO_USUARIO_OFICINACONSULAR", cnn))
                    {
                        //cmd.CommandTimeout = 30; //valor predeterminado
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.BigInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@dFechaInicio", SqlDbType.DateTime).Value = dFechaInicio;
                        cmd.Parameters.Add("@dFechaFin", SqlDbType.DateTime).Value = dFechaFin;
                        cmd.Parameters.Add("@sUsuarioId", SqlDbType.SmallInt).Value = intUsuarioId;
                        cmd.Parameters.Add("@sTipoPago", SqlDbType.SmallInt).Value = intTipoPago;
                        cmd.Parameters.Add("@sTarifarioId", SqlDbType.SmallInt).Value = intTarifaId;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        //------------------------------------------------
        //Fecha: 11/10/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE ACTUACIONES_CONSOLIDADO_USUARIO_OFICINACONSULAR
        //------------------------------------------------
        public DataSet USP_REPORTE_CONSOLIDADO_ACTUACIONES_USUARIO_OFICINACONSULAR(int intOficinaConsularId, DateTime dFechaInicio, DateTime dFechaFin)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_REPORTE_ACTUACIONES_CONSOLIDADO_USUARIO_OFICINACONSULAR", cnn))
                    {
                        //cmd.CommandTimeout = 30; //valor predeterminado
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.BigInt).Value = intOficinaConsularId;
                        cmd.Parameters.Add("@dFechaInicio", SqlDbType.DateTime).Value = dFechaInicio;
                        cmd.Parameters.Add("@dFechaFin", SqlDbType.DateTime).Value = dFechaFin;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //------------------------------------------------
        //Fecha: 09/01/2019
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE DE CORRELATIVOS
        //------------------------------------------------
        public DataSet USP_REPORTE_CORRELATIVOS_TARIFA(int iOficinaConsularId, Int16 Anio)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RE_REPORTE_CORRELATIVOS_TARIFAS", cnn))
                    {
                        //cmd.CommandTimeout = 30; //valor predeterminado
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@P_sOficinaConsular", SqlDbType.SmallInt).Value = iOficinaConsularId;
                        cmd.Parameters.Add("@P_Anio", SqlDbType.SmallInt).Value = Anio;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet USP_REPORTE_CARGA_INICIAL_CORRELATIVO(int iOficinaConsularId)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_TARIFARIO_CORRELATIVO_CARGA_INICIAL_REPORTE", cnn))
                    {
                        //cmd.CommandTimeout = 30; //valor predeterminado
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@corr_sOficinaConsularId", SqlDbType.SmallInt).Value = iOficinaConsularId;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //#############################
        //--    PIPA
        //##############################
        public DataSet USP_REPORTE_Actuaciones_Mensuales(int anioDesde,int mesDesde,int anioHasta,int mesHasta,string tipoReporte)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    string procedure = "";
                    if (tipoReporte == "cantidad") 
                        procedure = "PN_REPORTES.USP_RE_ACTUACIONES_CONSULARES_CANTIDAD_POR_TARIFA";
                    else 
                        procedure = "PN_REPORTES.USP_RE_ACTUACIONES_CONSULARES_MONTO_POR_TARIFA";
                    using (SqlCommand cmd = new SqlCommand(procedure, cnn))
                    {
                        //cmd.CommandTimeout = 30; //valor predeterminado
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@V_ANIO_DESDE", SqlDbType.SmallInt).Value = anioDesde;
                        cmd.Parameters.Add("@V_MES_DESDE", SqlDbType.SmallInt).Value = mesDesde;
                        cmd.Parameters.Add("@V_ANIO_HASTA", SqlDbType.SmallInt).Value = anioHasta;
                        cmd.Parameters.Add("@V_MES_HASTA", SqlDbType.SmallInt).Value = mesHasta;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        //------------------------------------------------
        //Fecha: 09/01/2019
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE DE RECAUDACION POR TARIFA
        //------------------------------------------------
        public DataSet USP_REPORTE_RECAUDACION_TARIFA(int iOficinaConsularId, Int16 Anio)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REPORTES.USP_RE_REPORTE_RECAUDACION_TARIFAS", cnn))
                    {
                        //cmd.CommandTimeout = 30; //valor predeterminado
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@P_sOficinaConsular", SqlDbType.SmallInt).Value = iOficinaConsularId;
                        cmd.Parameters.Add("@P_sAnio", SqlDbType.SmallInt).Value = Anio;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //------------------------------------------------
        //Fecha: 09/01/2019
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE DE RECAUDACION MENSUAL
        //------------------------------------------------
        public DataSet USP_REPORTE_RECAUDACION_MENSUAL(int iOficinaConsularId, Int16 Anio)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REPORTE_CAPTACION_CONSULAR_OFICINA", cnn))
                    {
                        //cmd.CommandTimeout = 30; //valor predeterminado
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@P_sOficinaConsularId", SqlDbType.SmallInt).Value = iOficinaConsularId;
                        cmd.Parameters.Add("@P_sAnio", SqlDbType.SmallInt).Value = Anio;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
         //------------------------------------------------
        //Fecha: 09/01/2019
        //Autor: Jonatan Silva Cachay
        //Objetivo: REPORTE DE RECAUDACION DIARIO
        //------------------------------------------------
        public DataSet USP_REPORTE_RECAUDACION_DIARIO(int iOficinaConsularId, DateTime Fecha)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CONTABILIDAD.USP_CO_REPORTE_CAPTACION_CONSULAR_OFICINA_MES", cnn))
                    {
                        //cmd.CommandTimeout = 30; //valor predeterminado
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@P_sOficinaConsularId", SqlDbType.SmallInt).Value = iOficinaConsularId;
                        cmd.Parameters.Add("@P_dPeriodoFecha", SqlDbType.Date).Value = Fecha;

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        

        //------------------------------------------------
        //Fecha: 24/10/2017
        //Autor: Jonatan Silva Cachay
        //Objetivo: Reporte de Itinerantes
        //------------------------------------------------
        public DataSet ReporteItinerantes(Int16? idoficinaconsular,
                                          DateTime datFechainicio, DateTime datFechafin)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(strConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("[PN_REPORTES].[USP_RP_ITINERANTES_CANTIDAD_ACTUACIONES]", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@sOficinaConsularId", SqlDbType.SmallInt).Value = idoficinaconsular;
                        cmd.Parameters.Add("@dFechaInicio", SqlDbType.DateTime).Value = datFechainicio;
                        cmd.Parameters.Add("@dFechaFin", SqlDbType.DateTime).Value = datFechafin;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
