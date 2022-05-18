using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;
using SGAC.BE.MRE;

namespace SGAC.Cliente.Colas.DA
{
    public class VentanillaMantenimientoDA
    {
        ~VentanillaMantenimientoDA()
        {
            GC.Collect();
        }

        string conexion()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }


        public Int16 Insertar(BE.MRE.CL_VENTANILLA pobjBE)
        {
            Int16 intResultado = 0;
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_VENTANILLA_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@vent_sVentanillaId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@vent_sOficinaConsularId", SqlDbType.SmallInt).Value = pobjBE.vent_sOficinaConsularId;
                        cmd.Parameters.Add("@vent_vDescripcion", SqlDbType.VarChar, 50).Value = pobjBE.vent_vDescripcion;
                        cmd.Parameters.Add("@vent_INumeroOrden", SqlDbType.Int).Value = pobjBE.vent_INumeroOrden;
                        cmd.Parameters.Add("@vent_sSectorId", SqlDbType.SmallInt).Value = pobjBE.vent_sSectorId;
                        cmd.Parameters.Add("@vent_cEstado", SqlDbType.Char, 1).Value = pobjBE.vent_cEstado;
                        cmd.Parameters.Add("@vent_sUsuarioCreacion", SqlDbType.SmallInt).Value = pobjBE.vent_sUsuarioCreacion;
                        cmd.Parameters.Add("@vent_vIPCreacion", SqlDbType.Char, 50).Value = pobjBE.vent_vIPCreacion;
                        cmd.Parameters.Add("@vent_dFechaCreacion", SqlDbType.DateTime).Value = pobjBE.vent_dFechaCreacion;
                        cmd.Parameters.Add("@vent_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        intResultado = Convert.ToInt16(cmd.Parameters["@vent_sVentanillaId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                intResultado = 0;
            }
            return intResultado;
        }

        public Int16 InsertarDetalle(BE.MRE.CL_VENTANILLASERVICIO objListVenServicio)
        {
            Int16 intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_VENTANILLASERVICIO_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@vede_sVentanillaId", SqlDbType.SmallInt).Value = objListVenServicio.vede_sVentanillaId;
                        cmd.Parameters.Add("@vede_sServicioId", SqlDbType.SmallInt).Value = objListVenServicio.vede_sServicioId;
                        cmd.Parameters.Add("@vede_IObligatorio", SqlDbType.Int).Value = objListVenServicio.vede_IObligatorio;
                        cmd.Parameters.Add("@vede_cEstado", SqlDbType.Char, 1).Value = objListVenServicio.vede_cEstado;
                        cmd.Parameters.Add("@vede_sUsuarioCreacion", SqlDbType.SmallInt).Value = objListVenServicio.vede_sUsuarioCreacion;
                        cmd.Parameters.Add("@vede_vIPCreacion", SqlDbType.Char, 50).Value = objListVenServicio.vede_vIPCreacion;
                        cmd.Parameters.Add("@vede_dFechaCreacion", SqlDbType.DateTime).Value = objListVenServicio.vede_dFechaCreacion;
                        cmd.Parameters.Add("@vede_sIdOfConsular", SqlDbType.SmallInt).Value = objListVenServicio.vent_sOficinaConsularId;
                        cmd.Parameters.Add("@vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

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

        public int Actualizar(BE.MRE.CL_VENTANILLA pobjBE)
        {
            Int16 intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_VENTANILLA_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@vent_sVentanillaId", SqlDbType.SmallInt).Value = pobjBE.vent_sVentanillaId;
                        cmd.Parameters.Add("@vent_sOficinaConsularId", SqlDbType.SmallInt).Value = pobjBE.vent_sOficinaConsularId;
                        cmd.Parameters.Add("@vent_vDescripcion", SqlDbType.VarChar, 50).Value = pobjBE.vent_vDescripcion;
                        cmd.Parameters.Add("@vent_INumeroOrden", SqlDbType.Int).Value = pobjBE.vent_INumeroOrden;
                        cmd.Parameters.Add("@vent_sSectorId", SqlDbType.SmallInt).Value = pobjBE.vent_sSectorId;
                        cmd.Parameters.Add("@vent_cEstado", SqlDbType.Char, 1).Value = pobjBE.vent_cEstado;
                        cmd.Parameters.Add("@vent_sUsuarioModificacion", SqlDbType.SmallInt).Value = pobjBE.vent_sUsuarioModificacion;
                        cmd.Parameters.Add("@vent_vIPModificacion", SqlDbType.Char, 50).Value = pobjBE.vent_vIPModificacion;
                        cmd.Parameters.Add("@vent_dFechaModificacion", SqlDbType.DateTime).Value = pobjBE.vent_dFechaModificacion;
                        cmd.Parameters.Add("@vent_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        intResultado = Convert.ToInt16(cmd.Parameters["@vent_sVentanillaId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                intResultado = 0;
            }
            return intResultado;
        }

        public int ActualizarDetalle(BE.MRE.CL_VENTANILLASERVICIO objSerVenDetalle)
        {
            Int16 intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;
            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_VENTANILLASERVICIO_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@vede_sVentanillaId", SqlDbType.SmallInt).Value = objSerVenDetalle.vede_sVentanillaId;
                        cmd.Parameters.Add("@vede_sServicioId", SqlDbType.SmallInt).Value = objSerVenDetalle.vede_sServicioId;
                        cmd.Parameters.Add("@vede_IObligatorio", SqlDbType.Int).Value = objSerVenDetalle.vede_IObligatorio;
                        cmd.Parameters.Add("@vede_cEstado", SqlDbType.Char, 1).Value = objSerVenDetalle.vede_cEstado;
                        cmd.Parameters.Add("@vede_sUsuarioModificacion", SqlDbType.SmallInt).Value = objSerVenDetalle.vede_sUsuarioModificacion;
                        cmd.Parameters.Add("@vede_vIPModificacion", SqlDbType.Char, 50).Value = objSerVenDetalle.vede_vIPModificacion;
                        cmd.Parameters.Add("@vede_dFechaModificacion", SqlDbType.DateTime).Value = objSerVenDetalle.vede_dFechaModificacion;
                        cmd.Parameters.Add("@vent_sOficinaConsularId", SqlDbType.SmallInt).Value = objSerVenDetalle.vent_sOficinaConsularId;
                        cmd.Parameters.Add("@vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

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


        public int Eliminar(BE.MRE.CL_VENTANILLA pobjBE)
        {
            int intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.conexion()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_CLIENTE.USP_CL_VENTANILLA_ANULAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@vent_sVentanillaId", pobjBE.vent_sVentanillaId));
                        cmd.Parameters.Add(new SqlParameter("@vent_sUsuarioModificacion", pobjBE.vent_sUsuarioModificacion));
                        cmd.Parameters.Add(new SqlParameter("@vent_vIPModificacion", pobjBE.vent_vIPModificacion));
                        cmd.Parameters.Add(new SqlParameter("@vent_sOficinaConsularId", pobjBE.vent_sOficinaConsularId));
                        cmd.Parameters.Add(new SqlParameter("@vent_vHostName", Util.ObtenerHostName()));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        intResultado = (int)Enumerador.enmResultadoOperacion.OK;
                    }
                }
            }
            catch (Exception)
            {
                intResultado = (int)Enumerador.enmResultadoOperacion.ERROR;
                throw;
            }

            return intResultado;
        }

    }
}
