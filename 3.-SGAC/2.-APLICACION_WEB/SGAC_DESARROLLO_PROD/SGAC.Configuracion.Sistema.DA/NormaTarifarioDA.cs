using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Sistema.DA
{
    public class NormaTarifarioDA
    {
        ~NormaTarifarioDA()
        {
            GC.Collect();
        }
        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        #region Metodos_Norma_Tarifario

        public DataTable Consultar(Int16 intPagoId, Int16 intNormaId, string strTarifaLetra, string strFecha, bool bExcepcion,
            int intPageSize, int intPageNumber, string strContar, ref int IntTotalCount, ref int IntTotalPages)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_NORMA_TARIFARIO_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_NOTA_SPAGOTIPOID", intPagoId));
                        cmd.Parameters.Add(new SqlParameter("@P_NOTA_SNORMAID", intNormaId));
                        cmd.Parameters.Add(new SqlParameter("@P_TARI_VNUMEROLETRA", strTarifaLetra));
                        cmd.Parameters.Add(new SqlParameter("@P_CFECHA", strFecha));
                        cmd.Parameters.Add(new SqlParameter("@P_BEXCEPCION", bExcepcion));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGESIZE", intPageSize));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGENUMBER", intPageNumber));
                        cmd.Parameters.Add(new SqlParameter("@P_CCONTAR", strContar));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@P_IPAGECOUNT", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@P_IRECORDCOUNT", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }
                        IntTotalCount = Convert.ToInt32(lReturn2.Value);
                        IntTotalPages = Convert.ToInt32(lReturn1.Value);
                    }
                }
            }

            catch (SqlException exec)
            {
                dtResultado = null;
                throw exec;
            }

            return dtResultado;
        }

        public bool ConsultarCantidadPagoNormaTarifario(Int16 intNormaId, Int16 intTarifarioId, Int16 intPagoId)
        {
            bool bExiste = false;
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PAGO_SI_NORMA_TARIFARIO_CONSULTAR_CANTIDAD_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_NOTA_SNORMAID", intNormaId));
                        cmd.Parameters.Add(new SqlParameter("@P_NOTA_STARIFARIOID", intTarifarioId));
                        cmd.Parameters.Add(new SqlParameter("@P_NOTA_SPAGOTIPOID", intPagoId));

                        
                        cmd.Connection.Open();
                        using (SqlDataReader loReader = cmd.ExecuteReader())
                        {
                            while (loReader.Read())
                            {
                                Int64 intCantidad = 0;

                                intCantidad = Convert.ToInt64(loReader["Cantidad"].ToString());
                                if (intCantidad > 0)
                                {
                                    bExiste = true;
                                    break;
                                }
                            }
                        }

                    }
                }
            }

            catch (SqlException exec)
            {
                throw exec;
            }

            return bExiste;
        }

        public SI_NORMA_TARIFARIO InsertarNormaTarifario(SI_NORMA_TARIFARIO objNormaTarifario)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_NORMA_TARIFARIO_ADICIONAR_MRE", cnx))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@nota_sOficinaConsularId", objNormaTarifario.nota_sOficinaConsularId));

                        if (objNormaTarifario.nota_sNormaId > 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@nota_sNormaId", objNormaTarifario.nota_sNormaId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@nota_sNormaId", null));
                        }
                        cmd.Parameters.Add(new SqlParameter("@nota_sTarifarioId", objNormaTarifario.nota_sTarifarioId));
                        cmd.Parameters.Add(new SqlParameter("@nota_sPagoTipoId", objNormaTarifario.nota_sPagoTipoId));

                        cmd.Parameters.Add(new SqlParameter("@nota_sUsuarioCreacion", objNormaTarifario.nota_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@nota_vIPCreacion", objNormaTarifario.nota_vIPCreacion));


                        SqlParameter lReturn = cmd.Parameters.Add("@nota_iNormaTarifarioId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objNormaTarifario.nota_iNormaTarifarioId = Convert.ToInt64(lReturn.Value);
                        objNormaTarifario.Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                objNormaTarifario.Error = true;
                objNormaTarifario.Message = ex.Message.ToString();
            }
            return objNormaTarifario;
        }

        public SI_NORMA_TARIFARIO ActualizarNormaTarifario(SI_NORMA_TARIFARIO objNormaTarifario)
        {

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_NORMA_TARIFARIO_ACTUALIZAR_MRE", cnx))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@nota_iNormaTarifarioId", objNormaTarifario.nota_iNormaTarifarioId));
                        cmd.Parameters.Add(new SqlParameter("@nota_sOficinaConsularId", objNormaTarifario.nota_sOficinaConsularId));

                        if (objNormaTarifario.nota_sNormaId > 0)
                        {
                            cmd.Parameters.Add(new SqlParameter("@nota_sNormaId", objNormaTarifario.nota_sNormaId));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@nota_sNormaId", null));
                        }
                        cmd.Parameters.Add(new SqlParameter("@nota_sTarifarioId", objNormaTarifario.nota_sTarifarioId));
                        cmd.Parameters.Add(new SqlParameter("@nota_sPagoTipoId", objNormaTarifario.nota_sPagoTipoId));
                        cmd.Parameters.Add(new SqlParameter("@nota_sUsuarioModificacion", objNormaTarifario.nota_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@nota_vIPModificacion", objNormaTarifario.nota_vIPModificacion));


                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objNormaTarifario.Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                objNormaTarifario.Error = true;
                objNormaTarifario.Message = ex.Message.ToString();
            }
            return objNormaTarifario;
        }


        public SI_NORMA_TARIFARIO AnularNormaTarifario(SI_NORMA_TARIFARIO objNormaTarifario)
        {

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_NORMA_TARIFARIO_ANULAR_MRE", cnx))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@nota_sPagoTipoId", objNormaTarifario.nota_sPagoTipoId));
                        cmd.Parameters.Add(new SqlParameter("@nota_sNormaId", objNormaTarifario.nota_sNormaId)); 
                        cmd.Parameters.Add(new SqlParameter("@nota_sOficinaConsularId", objNormaTarifario.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@nota_sUsuarioModificacion", objNormaTarifario.nota_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@nota_vIPModificacion", objNormaTarifario.nota_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@nota_sTarifarioId", objNormaTarifario.nota_sTarifarioId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objNormaTarifario.Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                objNormaTarifario.Error = true;
                objNormaTarifario.Message = ex.Message.ToString();
            }
            return objNormaTarifario;
        }

        #endregion


        #region Metodos_Norma

        public DataTable ConsultarNorma(Int16 intTipoNormaId, Int16 intObjetoNormaId, string strDescripcionCortaNorma,
            string strFechaInicial, string strFechaFinal, Int16 intEstadoId, int intPageSize, int intPageNumber, string strContar, ref int IntTotalCount, ref int IntTotalPages)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_NORMA_CONSULTAR_MRE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_NORM_STIPONORMAID", intTipoNormaId));
                        cmd.Parameters.Add(new SqlParameter("@P_NORM_SOBJETONORMAID", intObjetoNormaId));
                        cmd.Parameters.Add(new SqlParameter("@P_NORM_VDESCRIPCIONCORTA", strDescripcionCortaNorma));
                        cmd.Parameters.Add(new SqlParameter("@P_NORM_DVIGENCIAINICIO", strFechaInicial));
                        cmd.Parameters.Add(new SqlParameter("@P_NORM_DVIGENCIAFIN", strFechaFinal));
                        cmd.Parameters.Add(new SqlParameter("@P_NORM_SESTADOID", intEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGESIZE", intPageSize));
                        cmd.Parameters.Add(new SqlParameter("@P_IPAGENUMBER", intPageNumber));
                        cmd.Parameters.Add(new SqlParameter("@P_CCONTAR", strContar));

                        SqlParameter lReturn1 = cmd.Parameters.Add("@P_IPAGECOUNT", SqlDbType.Int);
                        lReturn1.Direction = ParameterDirection.Output;

                        SqlParameter lReturn2 = cmd.Parameters.Add("@P_IRECORDCOUNT", SqlDbType.Int);
                        lReturn2.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        DataSet dsObjeto = new DataSet();
                        using (SqlDataAdapter adap = new SqlDataAdapter(cmd))
                        {
                            adap.Fill(dsObjeto);
                            dtResultado = dsObjeto.Tables[0];
                        }
                        IntTotalCount = Convert.ToInt32(lReturn2.Value);
                        IntTotalPages = Convert.ToInt32(lReturn1.Value);
                    }
                }
            }

            catch (SqlException exec)
            {
                dtResultado = null;
                throw exec;
            }

            return dtResultado;
        }

        public SI_NORMA InsertarNorma(SI_NORMA objNorma)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_NORMA_ADICIONAR_MRE", cnx))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@norm_sOficinaConsularId", objNorma.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@norm_sTipoNormaId", objNorma.norm_sTipoNormaId));
                        cmd.Parameters.Add(new SqlParameter("@norm_sObjetoNormaId", objNorma.norm_sObjetoNormaId));
                        cmd.Parameters.Add(new SqlParameter("@norm_vNumeroArticulo", objNorma.norm_vNumeroArticulo));
                        cmd.Parameters.Add(new SqlParameter("@norm_vInciso", objNorma.norm_vInciso));
                        cmd.Parameters.Add(new SqlParameter("@norm_vNombreArticulo", objNorma.norm_vNombreArticulo));
                        cmd.Parameters.Add(new SqlParameter("@norm_vDescripcionCorta", objNorma.norm_vDescripcionCorta));
                        cmd.Parameters.Add(new SqlParameter("@norm_vDescripcion", objNorma.norm_vDescripcion));
                        cmd.Parameters.Add(new SqlParameter("@norm_dVigenciaInicio", objNorma.norm_dVigenciaInicio));
                        cmd.Parameters.Add(new SqlParameter("@norm_dVigenciaFin", objNorma.norm_dVigenciaFin));
                        cmd.Parameters.Add(new SqlParameter("@norm_sEstadoId", objNorma.norm_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@norm_sUsuarioCreacion", objNorma.norm_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@norm_vIPCreacion", objNorma.norm_vIPCreacion));


                        SqlParameter lReturn = cmd.Parameters.Add("@norm_sNormaId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objNorma.norm_sNormaId = Convert.ToInt16(lReturn.Value);
                        objNorma.Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                objNorma.Error = true;
                objNorma.Message = ex.Message.ToString();
            }
            return objNorma;
        }

        public SI_NORMA ActualizarNorma(SI_NORMA objNorma)
        {            

            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_NORMA_ACTUALIZAR_MRE", cnx))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@norm_sNormaId", objNorma.norm_sNormaId));
                        cmd.Parameters.Add(new SqlParameter("@norm_sOficinaConsularId", objNorma.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@norm_sTipoNormaId", objNorma.norm_sTipoNormaId));
                        cmd.Parameters.Add(new SqlParameter("@norm_sObjetoNormaId", objNorma.norm_sObjetoNormaId));
                        cmd.Parameters.Add(new SqlParameter("@norm_vNumeroArticulo", objNorma.norm_vNumeroArticulo));
                        cmd.Parameters.Add(new SqlParameter("@norm_vInciso", objNorma.norm_vInciso));
                        cmd.Parameters.Add(new SqlParameter("@norm_vNombreArticulo", objNorma.norm_vNombreArticulo));
                        cmd.Parameters.Add(new SqlParameter("@norm_vDescripcionCorta", objNorma.norm_vDescripcionCorta));
                        cmd.Parameters.Add(new SqlParameter("@norm_vDescripcion", objNorma.norm_vDescripcion));
                        cmd.Parameters.Add(new SqlParameter("@norm_dVigenciaInicio", objNorma.norm_dVigenciaInicio));
                        cmd.Parameters.Add(new SqlParameter("@norm_dVigenciaFin", objNorma.norm_dVigenciaFin));
                        cmd.Parameters.Add(new SqlParameter("@norm_sEstadoId", objNorma.norm_sEstadoId));
                        cmd.Parameters.Add(new SqlParameter("@norm_sUsuarioModificacion", objNorma.norm_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@norm_vIPModificacion", objNorma.norm_vIPModificacion));


                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objNorma.Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                objNorma.Error = true;
                objNorma.Message = ex.Message.ToString();
            }
            return objNorma;
        }

        public SI_NORMA AnularNorma(SI_NORMA objNorma)
        {
            try
            {
                using (SqlConnection cnx = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_NORMA_ANULAR_MRE", cnx))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@norm_sNormaId", objNorma.norm_sNormaId));
                        cmd.Parameters.Add(new SqlParameter("@norm_sOficinaConsularId", objNorma.OficinaConsultar));
                        cmd.Parameters.Add(new SqlParameter("@norm_sUsuarioModificacion", objNorma.norm_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@norm_vIPModificacion", objNorma.norm_vIPModificacion));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        objNorma.Error = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                objNorma.Error = true;
                objNorma.Message = ex.Message.ToString();
            }
            return objNorma;
        }

        #endregion
               
    }
}
