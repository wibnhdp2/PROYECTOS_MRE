using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;
using SGAC.BE.MRE;

namespace SGAC.Cliente.Colas.DA
{
    public class ServicioMantenimientoDA
    {
        ~ServicioMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        public Int64 InsertarDet(BE.MRE.CL_SERVICIO ObjTicteraBE)
        {
            Int64 intResultado = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_SERVICIO_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@serv_sServicioId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@serv_sOficinaConsularId", SqlDbType.BigInt).Value = ObjTicteraBE.serv_sOficinaConsularId;
                        cmd.Parameters.Add("@serv_vDescripcion", SqlDbType.VarChar, 100).Value = ObjTicteraBE.serv_vDescripcion;
                        cmd.Parameters.Add("@serv_IOrden", SqlDbType.Int).Value = ObjTicteraBE.serv_IOrden;
                        cmd.Parameters.Add("@serv_sServicioIdCab", SqlDbType.BigInt).Value = ObjTicteraBE.serv_sServicioIdCab;
                        cmd.Parameters.Add("@serv_sTipo", SqlDbType.BigInt).Value = ObjTicteraBE.serv_sTipo;
                        cmd.Parameters.Add("@serv_cEstado", SqlDbType.Char, 1).Value = ObjTicteraBE.serv_cEstado;
                        cmd.Parameters.Add("@serv_sUsuarioCreacion", SqlDbType.BigInt).Value = ObjTicteraBE.serv_sUsuarioCreacion;
                        cmd.Parameters.Add("@serv_vIPCreacion", SqlDbType.VarChar, 20).Value = ObjTicteraBE.serv_vIPCreacion;
                        cmd.Parameters.Add("@serv_dFechaCreacion", SqlDbType.DateTime).Value = ObjTicteraBE.serv_dFechaCreacion;
                        cmd.Parameters.Add("@vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        //------------------------------------------------------
                        //Fecha: 23/02/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Adicionar el parametro: @serv_vServicioDireccion
                        //------------------------------------------------------
                        cmd.Parameters.Add("@serv_vServicioDireccion", SqlDbType.VarChar, 100).Value = ObjTicteraBE.serv_vServicioDireccion;
                        //------------------------------------------------------

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        intResultado = Convert.ToInt64(cmd.Parameters["@serv_sServicioId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                intResultado = 0;             
            }
            return intResultado;
        }

        public int InsertDetail(BE.MRE.CL_SERVICIO ObjServDetalle)
        {
            int intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_SERVICIO_ADICIONAR_DETALLE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //------------------------------------------------------------------------------------------------
                        //Fecha: 08/02/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Se modifica la longitud de la descripción del servicio a 100.
                        //------------------------------------------------------------------------------------------------
                        cmd.Parameters.Add("@serv_sServicioId", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@serv_sOficinaConsularId", SqlDbType.BigInt).Value = ObjServDetalle.serv_sOficinaConsularId;
                        cmd.Parameters.Add("@serv_vDescripcion", SqlDbType.VarChar, 100).Value = ObjServDetalle.serv_vDescripcion;
                        cmd.Parameters.Add("@serv_IOrden", SqlDbType.Int).Value = ObjServDetalle.serv_IOrden;
                        cmd.Parameters.Add("@serv_sServicioIdCab", SqlDbType.Int).Value = ObjServDetalle.serv_sServicioIdCab;
                        cmd.Parameters.Add("@serv_sTipo", SqlDbType.Int).Value = ObjServDetalle.serv_sTipo;
                        cmd.Parameters.Add("@serv_cEstado", SqlDbType.Char, 1).Value = ObjServDetalle.serv_cEstado;
                        cmd.Parameters.Add("@serv_sUsuarioCreacion", SqlDbType.Int).Value = ObjServDetalle.serv_sUsuarioCreacion;
                        cmd.Parameters.Add("@serv_vIPCreacion", SqlDbType.VarChar, 50).Value = ObjServDetalle.serv_vIPCreacion;
                        cmd.Parameters.Add("@serv_dFechaCreacion", SqlDbType.DateTime).Value = ObjServDetalle.serv_dFechaCreacion;
                        cmd.Parameters.Add("@vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        //------------------------------------------------------
                        //Fecha: 23/02/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Adicionar el parametro: @serv_vServicioDireccion
                        //------------------------------------------------------
                        cmd.Parameters.Add("@serv_vServicioDireccion", SqlDbType.VarChar, 100).Value = ObjServDetalle.serv_vServicioDireccion;
                        //------------------------------------------------------

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoOperacion.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;
                throw ex;
            }
            return intResultado;
        }


        public int Delete(BE.MRE.CL_SERVICIO ObjTicteraBE)
        {
            int intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_SERVICIO_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@serv_sServicioId", SqlDbType.BigInt).Value = ObjTicteraBE.serv_sServicioId;
                        cmd.Parameters.Add("@serv_sOficinaConsularId", SqlDbType.BigInt).Value = 0;

                        cmd.Parameters.Add("@serv_sUsuarioModificacion", SqlDbType.SmallInt).Value = ObjTicteraBE.serv_sUsuarioModificacion;
                        cmd.Parameters.Add("@serv_vIPModificacion", SqlDbType.VarChar, 50).Value = ObjTicteraBE.serv_vIPModificacion;
                        cmd.Parameters.Add("@serv_dFechaModificacion", SqlDbType.DateTime).Value = ObjTicteraBE.serv_dFechaModificacion;
                        cmd.Parameters.Add("@vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        //------------------------------------------------------
                        //Fecha: 23/02/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Adicionar el parametro: @serv_vServicioDireccion
                        //------------------------------------------------------
                        cmd.Parameters.Add("@serv_vServicioDireccion", SqlDbType.VarChar, 100).Value = ObjTicteraBE.serv_vServicioDireccion;
                        //------------------------------------------------------

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoOperacion.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;
                throw ex;
            }
            return intResultado;
        }

        public int Update(BE.MRE.CL_SERVICIO ObjTicteraBE)
        {
            int intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_SERVICIO_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //------------------------------------------------------------------------------------------------
                        //Fecha: 08/02/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Se modifica la longitud de la descripción del servicio a 100.
                        //------------------------------------------------------------------------------------------------

                        cmd.Parameters.Add("@serv_sServicioId", SqlDbType.BigInt).Value = ObjTicteraBE.serv_sServicioId;
                        cmd.Parameters.Add("@serv_sOficinaConsularId", SqlDbType.BigInt).Value = ObjTicteraBE.serv_sOficinaConsularId;
                        cmd.Parameters.Add("@serv_vDescripcion", SqlDbType.VarChar, 100).Value = ObjTicteraBE.serv_vDescripcion;
                        cmd.Parameters.Add("@serv_IOrden", SqlDbType.Int).Value = ObjTicteraBE.serv_IOrden;
                        cmd.Parameters.Add("@serv_sTipo", SqlDbType.Int).Value = ObjTicteraBE.serv_sTipo;
                        cmd.Parameters.Add("@serv_sUsuarioModificacion", SqlDbType.Int).Value = ObjTicteraBE.serv_sUsuarioModificacion;
                        cmd.Parameters.Add("@serv_vIPModificacion", SqlDbType.VarChar, 50).Value = ObjTicteraBE.serv_vIPModificacion;
                        cmd.Parameters.Add("@serv_dFechaModificacion", SqlDbType.DateTime).Value = ObjTicteraBE.serv_dFechaModificacion;
                        cmd.Parameters.Add("@vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        //------------------------------------------------------
                        //Fecha: 23/02/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Adicionar el parametro: @serv_vServicioDireccion
                        //------------------------------------------------------
                        cmd.Parameters.Add("@serv_vServicioDireccion", SqlDbType.VarChar, 100).Value = ObjTicteraBE.serv_vServicioDireccion;
                        //------------------------------------------------------

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoOperacion.OK;
                    }
                }

            }
            catch (Exception ex)
            {
                intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;
                throw ex;
            }
            return intResultado;
        }


        public int UpdateDetail(BE.MRE.CL_SERVICIO ObjServDetalle)
        {
            int intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_SERVICIO_ACTUALIZAR_DETALLE", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //------------------------------------------------------------------------------------------------
                        //Fecha: 08/02/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Se modifica la longitud de la descripción del servicio a 100.
                        //------------------------------------------------------------------------------------------------

                        cmd.Parameters.Add("@serv_sServicioId", SqlDbType.BigInt).Value = ObjServDetalle.serv_sServicioId;
                        cmd.Parameters.Add("@serv_sOficinaConsularId", SqlDbType.BigInt).Value = ObjServDetalle.serv_sOficinaConsularId;
                        cmd.Parameters.Add("@serv_vDescripcion", SqlDbType.VarChar, 100).Value = ObjServDetalle.serv_vDescripcion;
                        cmd.Parameters.Add("@serv_IOrden", SqlDbType.Int).Value = ObjServDetalle.serv_IOrden;
                        cmd.Parameters.Add("@serv_sServicioIdCab", SqlDbType.SmallInt).Value = ObjServDetalle.serv_sServicioIdCab;
                        cmd.Parameters.Add("serv_cEstado", SqlDbType.Char, 1).Value = ObjServDetalle.serv_cEstado;
                        cmd.Parameters.Add("@serv_sTipo", SqlDbType.Int).Value = ObjServDetalle.serv_sTipo;
                        cmd.Parameters.Add("@serv_dFechaCreacion", SqlDbType.DateTime).Value = ObjServDetalle.serv_dFechaCreacion;
                        cmd.Parameters.Add("@serv_sUsuarioCreacion", SqlDbType.Int).Value = ObjServDetalle.serv_sUsuarioCreacion;
                        cmd.Parameters.Add("@serv_vIPCreacion", SqlDbType.VarChar, 50).Value = ObjServDetalle.serv_vIPCreacion;
                        cmd.Parameters.Add("@serv_dFechaModificacion", SqlDbType.DateTime).Value = ObjServDetalle.serv_dFechaModificacion;
                        cmd.Parameters.Add("@serv_sUsuarioModificacion", SqlDbType.Int).Value = ObjServDetalle.serv_sUsuarioModificacion;
                        cmd.Parameters.Add("@serv_vIPModificacion", SqlDbType.VarChar, 50).Value = ObjServDetalle.serv_vIPModificacion;
                        cmd.Parameters.Add("@vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        //------------------------------------------------------
                        //Fecha: 23/02/2021
                        //Autor: Miguel Márquez Beltrán
                        //Motivo: Adicionar el parametro: @serv_vServicioDireccion
                        //------------------------------------------------------
                        cmd.Parameters.Add("@serv_vServicioDireccion", SqlDbType.VarChar, 100).Value = ObjServDetalle.serv_vServicioDireccion;
                        //------------------------------------------------------
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoOperacion.OK;
                    }
                }

            }
            catch (Exception ex)
            {
                intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;
                throw ex;
            }
            return intResultado;
        }
    }
}


