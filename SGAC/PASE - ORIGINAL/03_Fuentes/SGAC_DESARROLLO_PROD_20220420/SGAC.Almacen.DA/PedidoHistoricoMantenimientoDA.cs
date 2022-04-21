using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DL.DAC;
using SGAC.Accesorios;

namespace SGAC.Almacen.DA
{
    public class PedidoHistoricoMantenimientoDA
    {
        private string strConnectionName = string.Empty;
        public string strError = string.Empty;

        public PedidoHistoricoMantenimientoDA()
        {
            strConnectionName = ConfigurationManager.AppSettings["ConexionSGAC"];
        }

        ~PedidoHistoricoMantenimientoDA()
        {
            GC.Collect();
        }

        #region Método ADICIONAR

        public int PedidoHistoricoAdicionar(BE.AL_PEDIDOHISTORICO objBE, int intOficinaConsularId)
        {
            int iResultQuery;

            try
            {
                SqlParameter[] prmParameterHeader = new SqlParameter[9];

                prmParameterHeader[0] = new SqlParameter("@pehi_IPedidoId", SqlDbType.BigInt);
                prmParameterHeader[0].Value = objBE.pehi_iPedidoId;
                prmParameterHeader[1] = new SqlParameter("@pehi_sEstadoId", SqlDbType.SmallInt);
                prmParameterHeader[1].Value = objBE.pehi_sEstadoId;
                prmParameterHeader[2] = new SqlParameter("@pehi_dFechaRegistro", SqlDbType.DateTime);
                prmParameterHeader[2].Value = objBE.pehi_dFechaRegistro;
                prmParameterHeader[3] = new SqlParameter("@pehi_sMotivoId", SqlDbType.SmallInt);
                prmParameterHeader[3].Value = objBE.pehi_sMotivoId;
                prmParameterHeader[4] = new SqlParameter("@pehi_vObservacion", SqlDbType.VarChar, 150);
                prmParameterHeader[4].Value = objBE.pehi_vObservacion;
                prmParameterHeader[5] = new SqlParameter("@pehi_sUsuarioCreacion", SqlDbType.SmallInt);
                prmParameterHeader[5].Value = objBE.pehi_sUsuarioCreacion;
                prmParameterHeader[6] = new SqlParameter("@pehi_sOficinaConsularId", SqlDbType.Int);
                prmParameterHeader[6].Value = intOficinaConsularId;
                prmParameterHeader[7] = new SqlParameter("@pehi_vHostName", SqlDbType.VarChar, 20);
                prmParameterHeader[7].Value = Util.ObtenerHostName();
                prmParameterHeader[8] = new SqlParameter("@pehi_vIPCreacion", SqlDbType.VarChar, 50);
                prmParameterHeader[8].Value = objBE.pehi_vIPCreacion;

                iResultQuery = SqlHelper.ExecuteNonQuery(strConnectionName,
                                         CommandType.StoredProcedure,
                                         "PN_ALMACEN.USP_AL_PEDIDOHISTORICO_ADICIONAR",
                                         prmParameterHeader);

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

        public int PedidoHistoricoActualizar(BE.AL_PEDIDOHISTORICO objBE, int intOficinaConsularId)
        {
            int iResultQuery;

            try
            {
                SqlParameter[] prmParameterHeader = new SqlParameter[9];

                prmParameterHeader[0] = new SqlParameter("@pehi_sEstadoId", SqlDbType.SmallInt);
                prmParameterHeader[0].Value = objBE.pehi_sEstadoId;
                prmParameterHeader[1] = new SqlParameter("@pehi_dFechaRegistro", SqlDbType.DateTime);
                prmParameterHeader[1].Value = objBE.pehi_dFechaRegistro;
                prmParameterHeader[2] = new SqlParameter("@pehi_sMotivoId", SqlDbType.SmallInt);
                prmParameterHeader[2].Value = objBE.pehi_sMotivoId;
                prmParameterHeader[3] = new SqlParameter("@pehi_vObservacion", SqlDbType.VarChar, 150);
                prmParameterHeader[3].Value = objBE.pehi_vObservacion;
                prmParameterHeader[4] = new SqlParameter("@pehi_sUsuarioModificacion", SqlDbType.SmallInt);
                prmParameterHeader[4].Value = objBE.pehi_sUsuarioModificacion;
                prmParameterHeader[5] = new SqlParameter("@pehi_iPedidoId", SqlDbType.BigInt);
                prmParameterHeader[5].Value = objBE.pehi_iPedidoId;
                prmParameterHeader[6] = new SqlParameter("@pehi_sOficinaConsularId", SqlDbType.SmallInt);
                prmParameterHeader[6].Value = intOficinaConsularId;
                prmParameterHeader[7] = new SqlParameter("@pehi_vHostName", SqlDbType.VarChar, 20);
                prmParameterHeader[7].Value = Util.ObtenerHostName();
                prmParameterHeader[8] = new SqlParameter("@pehi_vIPModificacion", SqlDbType.VarChar, 50);
                prmParameterHeader[8].Value = objBE.pehi_vIPCreacion;

                iResultQuery = SqlHelper.ExecuteNonQuery(strConnectionName,
                                         CommandType.StoredProcedure,
                                         "PN_ALMACEN.USP_AL_PEDIDOHISTORICO_ACTUALIZAR",
                                         prmParameterHeader);
                return iResultQuery;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                throw ex;
            }
        }

        #endregion Método ACTUALIZAR
    }
}