using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActuacionAnotacionMantenimientoDA
    {
        private string StrConnectionName = string.Empty;

        public ActuacionAnotacionMantenimientoDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ActuacionAnotacionMantenimientoDA()
        {
            GC.Collect();
        }

        //public int Insertar(BE.RE_ACTUACIONANOTACION ObjAnotBE, int IntOficinaConsular)
        //{
        //    int IntResultQuery;

        //    try
        //    {
        //        SqlParameter[] prmParameterHeader = new SqlParameter[12];

        //        prmParameterHeader[0] = new SqlParameter("@anot_iAnotacionId", SqlDbType.BigInt);
        //        prmParameterHeader[0].Direction = ParameterDirection.Output;

        //        prmParameterHeader[1] = new SqlParameter("@anot_iActuacionDetalleId", SqlDbType.BigInt);
        //        prmParameterHeader[1].Value = ObjAnotBE.anot_iActuacionDetalleId;

        //        prmParameterHeader[2] = new SqlParameter("@anot_sTipAnotacionId", SqlDbType.SmallInt);
        //        prmParameterHeader[2].Value = ObjAnotBE.anot_sTipoAnotacionId;

        //        prmParameterHeader[3] = new SqlParameter("@anot_vComentarios", SqlDbType.VarChar);
        //        prmParameterHeader[3].Value = ObjAnotBE.anot_vComentarios;

        //        prmParameterHeader[4] = new SqlParameter("@anot_dFecRegistro", SqlDbType.DateTime);
        //        prmParameterHeader[4].Value = ObjAnotBE.anot_dFechaRegistro;

        //        prmParameterHeader[5] = new SqlParameter("@anot_sOficinaConsularId", SqlDbType.SmallInt);
        //        prmParameterHeader[5].Value = IntOficinaConsular;

        //        prmParameterHeader[6] = new SqlParameter("@anot_sUsuarioCreacion", SqlDbType.SmallInt);
        //        prmParameterHeader[6].Value = ObjAnotBE.anot_sUsuarioCreacion;

        //        prmParameterHeader[7] = new SqlParameter("@anot_vIPCreacion", SqlDbType.VarChar, 50);
        //        prmParameterHeader[7].Value = ObjAnotBE.anot_vIPCreacion;

        //        prmParameterHeader[8] = new SqlParameter("@anot_vHostName", SqlDbType.VarChar, 20);
        //        prmParameterHeader[8].Value = Util.ObtenerHostName();

        //        prmParameterHeader[9] = new SqlParameter("@anot_sTipoActaId", SqlDbType.SmallInt);
        //        prmParameterHeader[9].Value = ObjAnotBE.anot_sTipoActaId;

        //        prmParameterHeader[10] = new SqlParameter("@anot_vTitular", SqlDbType.VarChar, 200);
        //        prmParameterHeader[10].Value = ObjAnotBE.anot_vTitular;

        //        prmParameterHeader[11] = new SqlParameter("@anot_iNumeroActaAnterior", SqlDbType.Int);
        //        prmParameterHeader[11].Value = ObjAnotBE.anot_iNumeroActaAnterior;

        //        IntResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
        //                                                 CommandType.StoredProcedure,
        //                                                 "PN_REGISTRO.USP_RE_ANOTACION_ADICIONAR",
        //                                                 prmParameterHeader);

        //        return (int)Enumerador.enmResultadoQuery.OK;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //------------------------------------------------------------------
        //Fecha: 20/12/2019
        //Autor: Miguel Márquez Beltrán
        //Motivo: Reemplazar el SQLHelper por SqlConnection y SqlCommand.
        //------------------------------------------------------------------

        public int Insertar(BE.RE_ACTUACIONANOTACION ObjAnotBE, int IntOficinaConsular)
        {
            int intResultado = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ANOTACION_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter lReturn = cmd.Parameters.Add("@anot_iAnotacionId", SqlDbType.BigInt);
                        lReturn.Direction = ParameterDirection.Output;

                        cmd.Parameters.Add("@anot_iActuacionDetalleId", SqlDbType.BigInt).Value = ObjAnotBE.anot_iActuacionDetalleId;
                        cmd.Parameters.Add("@anot_sTipAnotacionId", SqlDbType.SmallInt).Value = ObjAnotBE.anot_sTipoAnotacionId;
                        cmd.Parameters.Add("@anot_vComentarios", SqlDbType.VarChar).Value = ObjAnotBE.anot_vComentarios;
                        cmd.Parameters.Add("@anot_dFecRegistro", SqlDbType.DateTime).Value = ObjAnotBE.anot_dFechaRegistro;
                        cmd.Parameters.Add("@anot_sOficinaConsularId", SqlDbType.SmallInt).Value = IntOficinaConsular;
                        cmd.Parameters.Add("@anot_sUsuarioCreacion", SqlDbType.SmallInt).Value = ObjAnotBE.anot_sUsuarioCreacion;
                        cmd.Parameters.Add("@anot_vIPCreacion", SqlDbType.VarChar, 50).Value = ObjAnotBE.anot_vIPCreacion;
                        cmd.Parameters.Add("@anot_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@anot_sTipoActaId", SqlDbType.SmallInt).Value = ObjAnotBE.anot_sTipoActaId;
                        cmd.Parameters.Add("@anot_vTitular", SqlDbType.VarChar, 200).Value = ObjAnotBE.anot_vTitular;
                        cmd.Parameters.Add("@anot_iNumeroActaAnterior", SqlDbType.Int).Value = ObjAnotBE.anot_iNumeroActaAnterior;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                intResultado = 1;
                
            }
            catch (Exception ex)
            {
                intResultado = -1;
                throw new Exception("Ocurrio un error en el procedimiento: USP_RE_ANOTACION_ADICIONAR", ex);
            }
            return intResultado;
        }


        //public int Actualizar(BE.RE_ACTUACIONANOTACION ObjAnotBE, int IntOficinaConsular)
        //{
        //    int IntResultQuery;

        //    try
        //    {
        //        SqlParameter[] prmParameterHeader = new SqlParameter[12];

        //        prmParameterHeader[0] = new SqlParameter("@anot_iAnotacionId", SqlDbType.BigInt);
        //        prmParameterHeader[0].Value = ObjAnotBE.anot_iActuacionAnotacionId;

        //        prmParameterHeader[1] = new SqlParameter("@anot_iActuacionDetalleId", SqlDbType.BigInt);
        //        prmParameterHeader[1].Value = ObjAnotBE.anot_iActuacionDetalleId;

        //        prmParameterHeader[2] = new SqlParameter("@anot_sTipAnotacionId", SqlDbType.SmallInt);
        //        prmParameterHeader[2].Value = ObjAnotBE.anot_sTipoAnotacionId;

        //        prmParameterHeader[3] = new SqlParameter("@anot_vComentarios", SqlDbType.VarChar);
        //        prmParameterHeader[3].Value = ObjAnotBE.anot_vComentarios;

        //        prmParameterHeader[4] = new SqlParameter("@anot_dFecRegistro", SqlDbType.DateTime);
        //        prmParameterHeader[4].Value = ObjAnotBE.anot_dFechaRegistro;

        //        prmParameterHeader[5] = new SqlParameter("@anot_sOficinaConsularId", SqlDbType.SmallInt);
        //        prmParameterHeader[5].Value = IntOficinaConsular;

        //        prmParameterHeader[6] = new SqlParameter("@anot_sUsuarioModificacion", SqlDbType.SmallInt);
        //        prmParameterHeader[6].Value = ObjAnotBE.anot_sUsuarioModificacion;

        //        prmParameterHeader[7] = new SqlParameter("@anot_vIPModificacion", SqlDbType.VarChar, 50);
        //        prmParameterHeader[7].Value = ObjAnotBE.anot_vIPModificacion;

        //        prmParameterHeader[8] = new SqlParameter("@anot_vHostName", SqlDbType.VarChar, 20);
        //        prmParameterHeader[8].Value = Util.ObtenerHostName();

        //        prmParameterHeader[9] = new SqlParameter("@anot_sTipoActaId", SqlDbType.SmallInt);
        //        prmParameterHeader[9].Value = ObjAnotBE.anot_sTipoActaId;

        //        prmParameterHeader[10] = new SqlParameter("@anot_vTitular", SqlDbType.VarChar, 200);
        //        prmParameterHeader[10].Value = ObjAnotBE.anot_vTitular;

        //        prmParameterHeader[11] = new SqlParameter("@anot_iNumeroActaAnterior", SqlDbType.Int);
        //        prmParameterHeader[11].Value = ObjAnotBE.anot_iNumeroActaAnterior;

        //        IntResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
        //                                                 CommandType.StoredProcedure,
        //                                                 "PN_REGISTRO.USP_RE_ANOTACION_ACTUALIZAR",
        //                                                 prmParameterHeader);

        //        return (int)Enumerador.enmResultadoQuery.OK;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //------------------------------------------------------------------
        //Fecha: 20/12/2019
        //Autor: Miguel Márquez Beltrán
        //Motivo: Reemplazar el SQLHelper por SqlConnection y SqlCommand.
        //------------------------------------------------------------------

        public int Actualizar(BE.RE_ACTUACIONANOTACION ObjAnotBE, int IntOficinaConsular)
        {
            int intResultado = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ANOTACION_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@anot_iAnotacionId", SqlDbType.BigInt).Value = ObjAnotBE.anot_iActuacionAnotacionId;
                        cmd.Parameters.Add("@anot_iActuacionDetalleId", SqlDbType.BigInt).Value = ObjAnotBE.anot_iActuacionDetalleId;
                        cmd.Parameters.Add("@anot_sTipAnotacionId", SqlDbType.SmallInt).Value = ObjAnotBE.anot_sTipoAnotacionId;
                        cmd.Parameters.Add("@anot_vComentarios", SqlDbType.VarChar, 1000).Value = ObjAnotBE.anot_vComentarios;
                        cmd.Parameters.Add("@anot_dFecRegistro", SqlDbType.DateTime).Value = ObjAnotBE.anot_dFechaRegistro;
                        cmd.Parameters.Add("@anot_sOficinaConsularId", SqlDbType.SmallInt).Value = IntOficinaConsular;
                        cmd.Parameters.Add("@anot_sUsuarioModificacion", SqlDbType.SmallInt).Value = ObjAnotBE.anot_sUsuarioModificacion;
                        cmd.Parameters.Add("@anot_vIPModificacion", SqlDbType.VarChar, 50).Value = ObjAnotBE.anot_vIPModificacion;
                        cmd.Parameters.Add("@anot_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@anot_sTipoActaId", SqlDbType.SmallInt).Value = ObjAnotBE.anot_sTipoActaId;
                        cmd.Parameters.Add("@anot_vTitular", SqlDbType.VarChar, 200).Value = ObjAnotBE.anot_vTitular;
                        cmd.Parameters.Add("@anot_iNumeroActaAnterior", SqlDbType.Int).Value = ObjAnotBE.anot_iNumeroActaAnterior;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                intResultado = 1;
                
            }
            catch (Exception ex)
            {
                intResultado = -1;
                throw new Exception("Ocurrio un error en el procedimiento: USP_RE_ANOTACION_ACTUALIZAR", ex);
            }
            return intResultado;
        }


        //public int Eliminar(BE.RE_ACTUACIONANOTACION ObjAnotBE, int IntOficinaConsular)
        //{
        //    int IntResultQuery;

        //    try
        //    {
        //        SqlParameter[] prmParameterHeader = new SqlParameter[5];

        //        prmParameterHeader[0] = new SqlParameter("@anot_iAnotacionId", SqlDbType.BigInt);
        //        prmParameterHeader[0].Value = ObjAnotBE.anot_iActuacionAnotacionId;

        //        prmParameterHeader[1] = new SqlParameter("@anot_sOficinaConsularId", SqlDbType.SmallInt);
        //        prmParameterHeader[1].Value = IntOficinaConsular;

        //        prmParameterHeader[2] = new SqlParameter("@anot_sUsuarioModificacion", SqlDbType.SmallInt);
        //        prmParameterHeader[2].Value = ObjAnotBE.anot_sUsuarioModificacion;

        //        prmParameterHeader[3] = new SqlParameter("@anot_vIPModificacion", SqlDbType.VarChar, 20);
        //        prmParameterHeader[3].Value = ObjAnotBE.anot_vIPModificacion;

        //        prmParameterHeader[4] = new SqlParameter("@anot_vHostName", SqlDbType.VarChar, 20);
        //        prmParameterHeader[4].Value = Util.ObtenerHostName();

        //        IntResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
        //                                                 CommandType.StoredProcedure,
        //                                                 "PN_REGISTRO.USP_RE_ANOTACION_ELIMINAR",
        //                                                 prmParameterHeader);

        //        return (int)Enumerador.enmResultadoQuery.OK;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //------------------------------------------------------------------
        //Fecha: 20/12/2019
        //Autor: Miguel Márquez Beltrán
        //Motivo: Reemplazar el SQLHelper por SqlConnection y SqlCommand.
        //------------------------------------------------------------------

        public int Eliminar(BE.RE_ACTUACIONANOTACION ObjAnotBE, int IntOficinaConsular)
        {
            int intResultado = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ANOTACION_ELIMINAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@anot_iAnotacionId", SqlDbType.BigInt).Value = ObjAnotBE.anot_iActuacionAnotacionId;
                        cmd.Parameters.Add("@anot_sOficinaConsularId", SqlDbType.SmallInt).Value = IntOficinaConsular;
                        cmd.Parameters.Add("@anot_sUsuarioModificacion", SqlDbType.SmallInt).Value = ObjAnotBE.anot_sUsuarioModificacion;
                        cmd.Parameters.Add("@anot_vIPModificacion", SqlDbType.VarChar, 50).Value = ObjAnotBE.anot_vIPModificacion;
                        cmd.Parameters.Add("@anot_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                intResultado = 1;

                                
            }
            catch (Exception ex)
            {
                intResultado = -1;
                throw new Exception("Ocurrio un error el procedimiento: USP_RE_ANOTACION_ELIMINAR", ex);
            }
            return intResultado;
        }

        //-------------------------
    }
}