using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;

namespace SGAC.Almacen.DA
{
    public class PedidoMantenimientoDA
    {
        public string strError = string.Empty;

        private string strConnectionName()
        {
            return ConfigurationManager.AppSettings["ConexionSGAC"];
        }        

        ~PedidoMantenimientoDA()
        {
            GC.Collect();
        }


        #region Método ADICIONAR

        public int PedidoAdicionar(BE.AL_PEDIDO objBE)
        {

            Int16 iResultQuery = Convert.ToInt16(Enumerador.enmResultadoQuery.ERR);

            try
            {

                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_PEDIDO_ADICIONAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@pedi_sPedidoTipoId", SqlDbType.SmallInt).Value = objBE.pedi_sPedidoTipoId;
                        cmd.Parameters.Add("@pedi_sPedidoMotivoId", SqlDbType.SmallInt).Value = objBE.pedi_sPedidoMotivoId;
                        cmd.Parameters.Add("@pedi_cPedidoCodigo", SqlDbType.Char, 12).Value = objBE.pedi_cPedidoCodigo;
                        cmd.Parameters.Add("@pedi_sOficinaConsularIdOrigen", SqlDbType.SmallInt).Value = objBE.pedi_sOficinaConsularIdOrigen;
                        cmd.Parameters.Add("@pedi_sBovedaTipoIdOrigen", SqlDbType.SmallInt).Value = objBE.pedi_sBovedaTipoIdOrigen;
                        cmd.Parameters.Add("@pedi_sBodegaOrigenId", SqlDbType.SmallInt).Value = objBE.pedi_sBodegaOrigenId;

                        cmd.Parameters.Add("@pedi_sOficinaConsularIdDestino", SqlDbType.SmallInt).Value = objBE.pedi_sOficinaConsularIdDestino;
                        cmd.Parameters.Add("@pedi_sBovedaTipoIdDestino", SqlDbType.SmallInt).Value = objBE.pedi_sBovedaTipoIdDestino;
                        cmd.Parameters.Add("@pedi_sBodegaDestinoId", SqlDbType.SmallInt).Value = objBE.pedi_sBodegaDestinoId;

                        cmd.Parameters.Add("@pedi_sInsumoTipoId", SqlDbType.SmallInt).Value = objBE.pedi_sInsumoTipoId;
                        cmd.Parameters.Add("@pedi_dFechaRegistro", SqlDbType.DateTime).Value = objBE.pedi_dFechaRegistro;
                        cmd.Parameters.Add("@pedi_vActaRemision", SqlDbType.VarChar,12).Value = objBE.pedi_vActaRemision;
                        cmd.Parameters.Add("@pedi_vDescripcion", SqlDbType.VarChar, 150).Value = objBE.pedi_vDescripcion;

                        cmd.Parameters.Add("@pedi_ICantidad", SqlDbType.Int).Value = objBE.pedi_ICantidad;
                        cmd.Parameters.Add("@pedi_sEstadoId", SqlDbType.SmallInt).Value = objBE.pedi_sEstadoId;
                        cmd.Parameters.Add("@pedi_vObservacion", SqlDbType.VarChar, 150).Value = objBE.pedi_vObservacion;

                        cmd.Parameters.Add("@pedi_sUsuarioCreacion", SqlDbType.VarChar, 50).Value = objBE.pedi_sUsuarioCreacion;
                        cmd.Parameters.Add("@pedi_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@pedi_vIPCreacion", SqlDbType.VarChar, 50).Value = objBE.pedi_vIPCreacion;

                        SqlParameter lPedidoIdReturn = cmd.Parameters.Add("@pedi_iPedidoId", SqlDbType.BigInt);
                        lPedidoIdReturn.Direction = ParameterDirection.Output;


                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        

                        if (lPedidoIdReturn.Value != null)
                        {
                            if (lPedidoIdReturn.Value.ToString().Trim() != string.Empty)
                            {
                                objBE.pedi_iPedidoId = Convert.ToInt64(lPedidoIdReturn.Value);
                            }
                        }

                        iResultQuery = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);

                        
                    }
                }

                return iResultQuery;
            }

            catch (Exception ex)
            {
                strError = ex.Message;
                throw ex;
            }



            
        }

        #endregion Método ADICIONAR

        #region Método ACTUALIZAR

        public int PedidoActualizar(BE.AL_PEDIDO objBE)
        {
            int iResultQuery;

           
            try
            {

                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_PEDIDO_ACTUALIZAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@pedi_sPedidoTipoId", SqlDbType.SmallInt).Value = objBE.pedi_sPedidoTipoId;
                        cmd.Parameters.Add("@pedi_sPedidoMotivoId", SqlDbType.SmallInt).Value = objBE.pedi_sPedidoMotivoId;
                        cmd.Parameters.Add("@pedi_cPedidoCodigo", SqlDbType.Char, 12).Value = objBE.pedi_cPedidoCodigo;


                        cmd.Parameters.Add("@pedi_sOficinaConsularIdOrigen", SqlDbType.SmallInt).Value = objBE.pedi_sOficinaConsularIdOrigen;
                        cmd.Parameters.Add("@pedi_sBovedaTipoIdOrigen", SqlDbType.SmallInt).Value = objBE.pedi_sBovedaTipoIdOrigen;
                        cmd.Parameters.Add("@pedi_sBodegaOrigenId", SqlDbType.SmallInt).Value = objBE.pedi_sBodegaOrigenId;

                        cmd.Parameters.Add("@pedi_sOficinaConsularIdDestino", SqlDbType.SmallInt).Value = objBE.pedi_sOficinaConsularIdDestino;
                        cmd.Parameters.Add("@pedi_sBovedaTipoIdDestino", SqlDbType.SmallInt).Value = objBE.pedi_sBovedaTipoIdDestino;
                        cmd.Parameters.Add("@pedi_sBodegaDestinoId", SqlDbType.SmallInt).Value = objBE.pedi_sBodegaDestinoId;

                        cmd.Parameters.Add("@pedi_sInsumoTipoId", SqlDbType.SmallInt).Value = objBE.pedi_sInsumoTipoId;
                        cmd.Parameters.Add("@pedi_dFechaRegistro", SqlDbType.DateTime).Value = objBE.pedi_dFechaRegistro;
                        cmd.Parameters.Add("@pedi_vActaRemision", SqlDbType.VarChar, 12).Value = objBE.pedi_vActaRemision;
                        cmd.Parameters.Add("@pedi_vDescripcion", SqlDbType.VarChar, 150).Value = objBE.pedi_vDescripcion;

                        cmd.Parameters.Add("@pedi_ICantidad", SqlDbType.Int).Value = objBE.pedi_ICantidad;
                        cmd.Parameters.Add("@pedi_sEstadoId", SqlDbType.SmallInt).Value = objBE.pedi_sEstadoId;
                        cmd.Parameters.Add("@pedi_vObservacion", SqlDbType.VarChar, 150).Value = objBE.pedi_vObservacion;

                        cmd.Parameters.Add("@pedi_sUsuarioModificacion", SqlDbType.VarChar, 50).Value = objBE.pedi_sUsuarioModificacion;
                        cmd.Parameters.Add("@pedi_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();

                        cmd.Parameters.Add("@pedi_vIPModificacion", SqlDbType.VarChar, 50).Value = objBE.pedi_vIPModificacion;

                        cmd.Parameters.Add("@pedi_iPedidoId", SqlDbType.BigInt).Value = objBE.pedi_iPedidoId;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        iResultQuery = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);

                    }
                }

                return iResultQuery;
            }

            catch (Exception ex)
            {
                strError = ex.Message;
                throw ex;
            }


        }

        #endregion Método ACTUALIZAR

        #region Método ELIMINAR

        public int PedidoEliminar(BE.AL_PEDIDO objBE)
        {
            int iResultQuery;

            try
            {
                using (SqlConnection cn = new SqlConnection(this.strConnectionName()))
                {
                    using (SqlCommand cmd = new SqlCommand("PN_ALMACEN.USP_AL_PEDIDO_ELIMINAR", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@pedi_sOficinaConsularIdOrigen", SqlDbType.SmallInt).Value = objBE.pedi_sOficinaConsularIdOrigen;
                        cmd.Parameters.Add("@pedi_iPedidoId", SqlDbType.BigInt).Value = objBE.pedi_iPedidoId;
                        cmd.Parameters.Add("@pedi_sPedidoMotivoId", SqlDbType.SmallInt).Value = objBE.pedi_sPedidoMotivoId;
                        cmd.Parameters.Add("@pedi_cPedidoCodigo", SqlDbType.VarChar, 50).Value = objBE.pedi_cPedidoCodigo;
                        cmd.Parameters.Add("@pedi_vObservacion", SqlDbType.VarChar, 20).Value = objBE.pedi_vObservacion;
                        cmd.Parameters.Add("@pedi_sEstadoId", SqlDbType.VarChar, 20).Value = objBE.pedi_sEstadoId;
                        cmd.Parameters.Add("@pedi_sUsuarioModificacion", SqlDbType.VarChar, 20).Value = objBE.pedi_sUsuarioModificacion;
                        cmd.Parameters.Add("@pedi_vHostName", SqlDbType.VarChar, 20).Value = Util.ObtenerHostName();
                        cmd.Parameters.Add("@pedi_vIPModificacion", SqlDbType.VarChar, 20).Value = objBE.pedi_vIPModificacion;

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        iResultQuery = Convert.ToInt16(Enumerador.enmResultadoQuery.OK);

                    }
                }
                

                return (int)Enumerador.enmResultadoQuery.OK;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return (int)Enumerador.enmResultadoQuery.ERR;
            }
        }

        #endregion Método ELIMINAR
    }
}