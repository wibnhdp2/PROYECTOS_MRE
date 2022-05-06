using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;

namespace SGAC.Registro.Actuacion.DA
{
    public class ActuacionAdjuntoMantenimientoDA
    {
        private string StrConnectionName = string.Empty;

        public ActuacionAdjuntoMantenimientoDA()
        {
            StrConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~ActuacionAdjuntoMantenimientoDA()
        {
            GC.Collect();
        }

        //public int Insertar(BE.RE_ACTUACIONADJUNTO ObjActAdjBE,
        //                    int IntOficinaConsularId)
        //{
        //    int IntResultQuery;

        //    try
        //    {
        //        SqlParameter[] prmParameterHeader = new SqlParameter[10];

        //        prmParameterHeader[0] = new SqlParameter("@acad_iActuacionAdjuntoId", SqlDbType.BigInt);
        //        prmParameterHeader[0].Direction = ParameterDirection.Output;

        //        prmParameterHeader[1] = new SqlParameter("@acad_iActuacionDetalleId", SqlDbType.BigInt);
        //        prmParameterHeader[1].Value = ObjActAdjBE.acad_iActuacionDetalleId;

        //        prmParameterHeader[2] = new SqlParameter("@acad_sAdjuntoTipoId", SqlDbType.SmallInt);
        //        prmParameterHeader[2].Value = ObjActAdjBE.acad_sAdjuntoTipoId;

        //        prmParameterHeader[3] = new SqlParameter("@acad_vNombreArchivo", SqlDbType.VarChar, 100);
        //        prmParameterHeader[3].Value = ObjActAdjBE.acad_vNombreArchivo;

        //        prmParameterHeader[4] = new SqlParameter("@acad_vDescripcion", SqlDbType.VarChar, 1000);
        //        prmParameterHeader[4].Value = ObjActAdjBE.acad_vDescripcion;

        //        prmParameterHeader[5] = new SqlParameter("@acad_sOficinaConsularId", SqlDbType.SmallInt);
        //        prmParameterHeader[5].Value = IntOficinaConsularId;

        //        prmParameterHeader[6] = new SqlParameter("@acad_sUsuarioCreacion", SqlDbType.SmallInt);
        //        prmParameterHeader[6].Value = ObjActAdjBE.acad_sUsuarioCreacion;

        //        prmParameterHeader[7] = new SqlParameter("@acad_vIPCreacion", SqlDbType.VarChar, 50);
        //        prmParameterHeader[7].Value = ObjActAdjBE.acad_vIPCreacion;

        //        prmParameterHeader[8] = new SqlParameter("@acad_vHostName", SqlDbType.VarChar, 20);
        //        prmParameterHeader[8].Value = Util.ObtenerHostName();


        //        if (ObjActAdjBE.acad_bBloqueoAdjunto != null)
        //        {
        //            prmParameterHeader[9] = new SqlParameter("@acad_bBloqueoAdjunto", SqlDbType.Bit);
        //            prmParameterHeader[9].Value = ObjActAdjBE.acad_bBloqueoAdjunto;
        //        }


        //        IntResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
        //                                                 CommandType.StoredProcedure,
        //                                                 "PN_REGISTRO.USP_RE_ACTUACIONADJUNTO_ADICIONAR",
        //                                                 prmParameterHeader);

        //        return (int)Enumerador.enmResultadoQuery.OK;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //------------------------------------------------------------------
        //Fecha: 19/12/2019
        //Autor: Miguel Márquez Beltrán
        //Motivo: Reemplazar el SQLHelper por SqlConnection y SqlCommand.
        //------------------------------------------------------------------
        public int Insertar(BE.RE_ACTUACIONADJUNTO ObjActAdjBE,
                            int IntOficinaConsularId)
        {

            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);
            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONADJUNTO_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter lReturn1 = cmd.Parameters.Add("@acad_iActuacionAdjuntoId", SqlDbType.BigInt);
                        lReturn1.Direction = ParameterDirection.Output;

                        cmd.Parameters.Add("@acad_iActuacionDetalleId", SqlDbType.BigInt).Value = ObjActAdjBE.acad_iActuacionDetalleId;
                        cmd.Parameters.Add("@acad_sAdjuntoTipoId", SqlDbType.SmallInt).Value = ObjActAdjBE.acad_sAdjuntoTipoId;
                        cmd.Parameters.Add("@acad_vNombreArchivo", SqlDbType.VarChar, 100).Value = ObjActAdjBE.acad_vNombreArchivo;
                        cmd.Parameters.Add("@acad_vDescripcion", SqlDbType.VarChar, 1000).Value = ObjActAdjBE.acad_vDescripcion;
                        cmd.Parameters.Add("@acad_sOficinaConsularId", SqlDbType.SmallInt).Value = IntOficinaConsularId;
                        cmd.Parameters.Add("@acad_sUsuarioCreacion", SqlDbType.SmallInt).Value = ObjActAdjBE.acad_sUsuarioCreacion;
                        cmd.Parameters.Add("@acad_vIPCreacion", SqlDbType.VarChar, 50).Value = ObjActAdjBE.acad_vIPCreacion;
                        cmd.Parameters.Add("@acad_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        if (ObjActAdjBE.acad_bBloqueoAdjunto != null)
                        {
                            cmd.Parameters.Add("@acad_bBloqueoAdjunto", SqlDbType.Bit).Value = ObjActAdjBE.acad_bBloqueoAdjunto;
                        }
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);
                    }
                }

                return intResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error en el procedimiento: USP_RE_ACTUACIONADJUNTO_ADICIONAR", ex);
            }
        }

        //public int Actualizar(BE.RE_ACTUACIONADJUNTO ObjActAdjBE,
        //                      int IntOficinaConsularId)
        //{
        //    int IntResultQuery;

        //    try
        //    {
        //        SqlParameter[] prmParameterHeader = new SqlParameter[9];

        //        prmParameterHeader[0] = new SqlParameter("@acad_iActuacionAdjuntoId", SqlDbType.BigInt);
        //        prmParameterHeader[0].Value = ObjActAdjBE.acad_iActuacionAdjuntoId;

        //        prmParameterHeader[1] = new SqlParameter("@acad_iActuacionDetalleId", SqlDbType.BigInt);
        //        prmParameterHeader[1].Value = ObjActAdjBE.acad_iActuacionDetalleId;

        //        prmParameterHeader[2] = new SqlParameter("@acad_sAdjuntoTipoId", SqlDbType.SmallInt);
        //        prmParameterHeader[2].Value = ObjActAdjBE.acad_sAdjuntoTipoId;

        //        prmParameterHeader[3] = new SqlParameter("@acad_vNombreArchivo", SqlDbType.VarChar, 100);
        //        prmParameterHeader[3].Value = ObjActAdjBE.acad_vNombreArchivo;

        //        prmParameterHeader[4] = new SqlParameter("@acad_vDescripcion", SqlDbType.VarChar, 1000);
        //        prmParameterHeader[4].Value = ObjActAdjBE.acad_vDescripcion;

        //        prmParameterHeader[5] = new SqlParameter("@acad_sOficinaConsularId", SqlDbType.SmallInt);
        //        prmParameterHeader[5].Value = IntOficinaConsularId;

        //        prmParameterHeader[6] = new SqlParameter("@acad_sUsuarioModificacion", SqlDbType.SmallInt);
        //        prmParameterHeader[6].Value = ObjActAdjBE.acad_sUsuarioModificacion;

        //        prmParameterHeader[7] = new SqlParameter("@acad_vIPModificacion", SqlDbType.VarChar, 50);
        //        prmParameterHeader[7].Value = ObjActAdjBE.acad_vIPModificacion;

        //        prmParameterHeader[8] = new SqlParameter("@acad_vHostName", SqlDbType.VarChar, 20);
        //        prmParameterHeader[8].Value = Util.ObtenerHostName();

        //        IntResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
        //                                                 CommandType.StoredProcedure,
        //                                                 "PN_REGISTRO.USP_RE_ACTUACIONADJUNTO_ACTUALIZAR",
        //                                                 prmParameterHeader);

        //        return (int)Enumerador.enmResultadoQuery.OK;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //------------------------------------------------------------------
        //Fecha: 19/12/2019
        //Autor: Miguel Márquez Beltrán
        //Motivo: Reemplazar el SQLHelper por SqlConnection y SqlCommand.
        //------------------------------------------------------------------

        public int Actualizar(BE.RE_ACTUACIONADJUNTO ObjActAdjBE,
                              int IntOficinaConsularId)
        {
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);
            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONADJUNTO_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@acad_iActuacionAdjuntoId", SqlDbType.BigInt).Value = ObjActAdjBE.acad_iActuacionAdjuntoId;
                        cmd.Parameters.Add("@acad_iActuacionDetalleId", SqlDbType.BigInt).Value = ObjActAdjBE.acad_iActuacionDetalleId;
                        cmd.Parameters.Add("@acad_sAdjuntoTipoId", SqlDbType.SmallInt).Value = ObjActAdjBE.acad_sAdjuntoTipoId;
                        cmd.Parameters.Add("@acad_vNombreArchivo", SqlDbType.VarChar, 100).Value = ObjActAdjBE.acad_vNombreArchivo;
                        cmd.Parameters.Add("@acad_vDescripcion", SqlDbType.VarChar, 1000).Value = ObjActAdjBE.acad_vDescripcion;
                        cmd.Parameters.Add("@acad_sOficinaConsularId", SqlDbType.SmallInt).Value = IntOficinaConsularId;
                        cmd.Parameters.Add("@acad_sUsuarioModificacion", SqlDbType.SmallInt).Value = ObjActAdjBE.acad_sUsuarioModificacion;
                        cmd.Parameters.Add("@acad_vIPModificacion", SqlDbType.VarChar, 50).Value = ObjActAdjBE.acad_vIPModificacion;
                        cmd.Parameters.Add("@acad_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);
                    }
                }

                return intResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error en el procedimiento: USP_RE_ACTUACIONADJUNTO_ACTUALIZAR", ex);
            }
        }
        //public int Eliminar(BE.RE_ACTUACIONADJUNTO ObjActAdjBE,
        //                    int IntOficinaConsularId)
        //{
        //    int IntResultQuery;

        //    try
        //    {
        //        SqlParameter[] prmParameterHeader = new SqlParameter[5];

        //        prmParameterHeader[0] = new SqlParameter("@acad_iActuacionAdjuntoId", SqlDbType.BigInt);
        //        prmParameterHeader[0].Value = ObjActAdjBE.acad_iActuacionAdjuntoId;

        //        prmParameterHeader[1] = new SqlParameter("@acad_sOficinaConsularId", SqlDbType.SmallInt);
        //        prmParameterHeader[1].Value = IntOficinaConsularId;

        //        prmParameterHeader[2] = new SqlParameter("@acad_sUsuarioModificacion", SqlDbType.SmallInt);
        //        prmParameterHeader[2].Value = ObjActAdjBE.acad_sUsuarioModificacion;

        //        prmParameterHeader[3] = new SqlParameter("@acad_vIPModificacion", SqlDbType.VarChar, 50);
        //        prmParameterHeader[3].Value = ObjActAdjBE.acad_vIPModificacion;

        //        prmParameterHeader[4] = new SqlParameter("@acad_vHostName", SqlDbType.VarChar, 20);
        //        prmParameterHeader[4].Value = Util.ObtenerHostName();

        //        IntResultQuery = SqlHelper.ExecuteNonQuery(StrConnectionName,
        //                                                 CommandType.StoredProcedure,
        //                                                 "PN_REGISTRO.USP_RE_ACTUACIONADJUNTO_ELIMINAR",
        //                                                 prmParameterHeader);

        //        return (int)Enumerador.enmResultadoQuery.OK;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //------------------------------------------------------------------
        //Fecha: 19/12/2019
        //Autor: Miguel Márquez Beltrán
        //Motivo: Reemplazar el SQLHelper por SqlConnection y SqlCommand.
        //------------------------------------------------------------------  
        public int Eliminar(BE.RE_ACTUACIONADJUNTO ObjActAdjBE,
                            int IntOficinaConsularId)
        {
            
            Int16 intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            try
            {
                using (SqlConnection cn = new SqlConnection(StrConnectionName))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_ACTUACIONADJUNTO_ELIMINAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@acad_iActuacionAdjuntoId", SqlDbType.BigInt).Value = ObjActAdjBE.acad_iActuacionAdjuntoId;
                        cmd.Parameters.Add("@acad_sOficinaConsularId", SqlDbType.SmallInt).Value = IntOficinaConsularId;
                        cmd.Parameters.Add("@acad_sUsuarioModificacion", SqlDbType.SmallInt).Value = ObjActAdjBE.acad_sUsuarioModificacion;
                        cmd.Parameters.Add("@acad_vIPModificacion", SqlDbType.VarChar, 50).Value = ObjActAdjBE.acad_vIPModificacion;
                        cmd.Parameters.Add("@acad_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        intResult = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);
                    }
                }


                return intResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error en el procedimiento: USP_RE_ACTUACIONADJUNTO_ELIMINAR", ex);
            }
        }
    
    }
}