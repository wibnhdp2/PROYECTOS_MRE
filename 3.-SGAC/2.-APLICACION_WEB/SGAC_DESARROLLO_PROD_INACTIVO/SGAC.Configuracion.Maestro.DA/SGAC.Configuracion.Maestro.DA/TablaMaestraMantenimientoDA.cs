using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using SGAC.BE.MRE;
using SGAC.Accesorios;

namespace SGAC.Configuracion.Maestro.DA
{
    public class TablaMaestraMantenimientoDA
    {
        ~TablaMaestraMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }


        public int Insertar(MA_TABLA_MAESTRA pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_TABLA_MAESTRA_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@sTablaId", pobjBE.tama_sTablaId));
                        cmd.Parameters.Add(new SqlParameter("@vCodigo", pobjBE.tama_cCodigo));
                        cmd.Parameters.Add(new SqlParameter("@vGrupo", pobjBE.tama_vGrupo));
                        cmd.Parameters.Add(new SqlParameter("@vNombre", pobjBE.tama_vNombre));
                        cmd.Parameters.Add(new SqlParameter("@vDescripcionCorta", pobjBE.tama_vDescripcionCorta));
                        cmd.Parameters.Add(new SqlParameter("@vDescripcion", pobjBE.tama_vDescripcion));
                        cmd.Parameters.Add(new SqlParameter("@vDescripcionLarga", pobjBE.tama_vDescripcionLarga));
                        cmd.Parameters.Add(new SqlParameter("@vSimbolo", pobjBE.tama_vSimbolo));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", pobjBE.tama_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@sUsuarioCreacion", pobjBE.tama_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@vIPCreacion", pobjBE.tama_vIPCreacion));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@sRegistroId", pobjBE.tama_sRegistroId)).Direction = ParameterDirection.Output;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoQuery.OK;
                    }
                }
            }
            catch (SqlException ex)
            {
                intResultado = (int)Enumerador.enmResultadoQuery.ERR;
                throw ex;
            }
            return intResultado;
        }


        public int Actualizar(MA_TABLA_MAESTRA pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_TABLA_MAESTRA_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@sRegistroId", pobjBE.tama_sRegistroId));
                        cmd.Parameters.Add(new SqlParameter("@sTablaId", pobjBE.tama_sTablaId));
                        cmd.Parameters.Add(new SqlParameter("@vCodigo", pobjBE.tama_cCodigo));
                        cmd.Parameters.Add(new SqlParameter("@vGrupo", pobjBE.tama_vGrupo));
                        cmd.Parameters.Add(new SqlParameter("@vNombre", pobjBE.tama_vNombre));
                        cmd.Parameters.Add(new SqlParameter("@vDescripcionCorta", pobjBE.tama_vDescripcionCorta));
                        cmd.Parameters.Add(new SqlParameter("@vDescripcion", pobjBE.tama_vDescripcion));
                        cmd.Parameters.Add(new SqlParameter("@vDescripcionLarga", pobjBE.tama_vDescripcionLarga));
                        cmd.Parameters.Add(new SqlParameter("@vSimbolo", pobjBE.tama_vSimbolo));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", pobjBE.tama_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@sUsuarioModificacion", pobjBE.tama_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@vIPModificacion", pobjBE.tama_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));
                        cmd.Parameters.Add(new SqlParameter("@cEstado", pobjBE.tama_cEstado));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoQuery.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                intResultado = (int)Enumerador.enmResultadoQuery.ERR;
                throw ex;
            }

            return intResultado;
        }


        public int Eliminar(MA_TABLA_MAESTRA pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;
          
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_TABLA_MAESTRA_ELIMINAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@sTablaId", pobjBE.tama_sTablaId));
                        cmd.Parameters.Add(new SqlParameter("@sRegistroId", pobjBE.tama_sRegistroId));
                        cmd.Parameters.Add(new SqlParameter("@sOficinaConsularId", pobjBE.tama_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@sUsuarioModificacion", pobjBE.tama_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@vIPModificacion", pobjBE.tama_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoQuery.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                intResultado = (int)Enumerador.enmResultadoQuery.ERR;
                throw ex;
            }

            return intResultado;
        }

        public int EliminarMargenImpresion(long mado_iCorrelativo,
                                           Int16 mado_sUsuarioCreacion)
        {

            int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_MARGENESDOCUMENTO_ELIMINAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_mado_iCorrelativo", mado_iCorrelativo));
                        cmd.Parameters.Add(new SqlParameter("@P_mado_sUsuarioCreacion", mado_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@P_mado_vIPCreacion", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoQuery.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                intResultado = (int)Enumerador.enmResultadoQuery.ERR;
                throw ex;
            }

            return intResultado;
        }

        public int RegistrarMargenImpresion(long mado_iCorrelativo,
                                            Int16 mado_sTipDocImpresion,
                                            Int16 mado_sOficinaConsular,
                                            byte mado_sSeccion,
                                            string mado_vDescripcion,
                                            Int16 mado_sMargenSuperior,
                                            Int16 mado_sMargenIzquierdo,
                                            Int16 mado_sUsuarioCreacion)
        { 
        int intResultado = (int)Enumerador.enmResultadoQuery.ERR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_MARGENESDOCUMENTO_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@P_mado_iCorrelativo", mado_iCorrelativo));
                        cmd.Parameters.Add(new SqlParameter("@P_mado_sTipDocImpresion", mado_sTipDocImpresion));
                        cmd.Parameters.Add(new SqlParameter("@P_mado_sOficinaConsular", mado_sOficinaConsular));
                        cmd.Parameters.Add(new SqlParameter("@P_mado_sSeccion", mado_sSeccion));
                        cmd.Parameters.Add(new SqlParameter("@P_mado_vDescripcion", mado_vDescripcion));
                        cmd.Parameters.Add(new SqlParameter("@P_mado_sMargenSuperior", mado_sMargenSuperior));
                        cmd.Parameters.Add(new SqlParameter("@P_mado_sMargenIzquierdo", mado_sMargenIzquierdo));
                        cmd.Parameters.Add(new SqlParameter("@P_mado_sUsuarioCreacion", mado_sUsuarioCreacion));
                        cmd.Parameters.Add(new SqlParameter("@P_mado_vIPCreacion", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoQuery.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                intResultado = (int)Enumerador.enmResultadoQuery.ERR;
                throw ex;
            }

            return intResultado;
        }
    }
}

