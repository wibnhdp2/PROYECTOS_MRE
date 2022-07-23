using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daMovimientoCarneIdentidad
    {
        public int adicionar(SqlConnection con, SqlTransaction trx, beMovimientoCarneIdentidad parametrosMovimientoCarneIdentidad)
        {
            int MovCarneIdent = -1;
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_MOVIMIENTO_CARNE_IDENTIDAD_ADICIONAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_CARNE_IDENTIDAD_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosMovimientoCarneIdentidad.CarneIdentidadid;

            SqlParameter par3 = cmd.Parameters.Add("@P_ESTADO_ID", SqlDbType.SmallInt);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametrosMovimientoCarneIdentidad.Estadoid;

            SqlParameter par4 = cmd.Parameters.Add("@P_OFICNA_MOVIMIENTO", SqlDbType.SmallInt);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametrosMovimientoCarneIdentidad.Oficinaconsularid;

            if (parametrosMovimientoCarneIdentidad.ObservacionTipo == 0) { SqlParameter par5 = cmd.Parameters.Add("@P_OBSERVACION_TIPO", DBNull.Value); }
            else
            {
                SqlParameter par5 = cmd.Parameters.Add("@P_OBSERVACION_TIPO", SqlDbType.SmallInt);
                par5.Direction = ParameterDirection.Input;
                par5.Value = parametrosMovimientoCarneIdentidad.ObservacionTipo;
            }

            if (parametrosMovimientoCarneIdentidad.ObservacionDetalle == null) { SqlParameter par6 = cmd.Parameters.Add("@P_OBSERVACION_DETALLE", DBNull.Value); }
            else
            {
                SqlParameter par6 = cmd.Parameters.Add("@P_OBSERVACION_DETALLE", SqlDbType.VarChar, 250);
                par6.Direction = ParameterDirection.Input;
                par6.Value = parametrosMovimientoCarneIdentidad.ObservacionDetalle;
            }

            SqlParameter par7 = cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.SmallInt);
            par7.Direction = ParameterDirection.Input;
            par7.Value = parametrosMovimientoCarneIdentidad.Usuariocreacion;

            SqlParameter par8 = cmd.Parameters.Add("@P_IP_CREACION", SqlDbType.VarChar, 50);
            par8.Direction = ParameterDirection.Input;
            par8.Value = parametrosMovimientoCarneIdentidad.Ipcreacion;

            SqlParameter par9 = cmd.Parameters.Add("@@IDENTITY", SqlDbType.Int);
            par9.Direction = ParameterDirection.ReturnValue;

            int n = cmd.ExecuteNonQuery();
            if (n > 0) MovCarneIdent = Convert.ToInt32(par9.Value);
            return (MovCarneIdent);
        }

        public List<beMovimientoCarneIdentidad> consultarMovimientos(SqlConnection con, beMovimientoCarneIdentidad parametrosMovimientos)
        {
            List<beMovimientoCarneIdentidad> lbeMovimientoCarneIdentidad = new List<beMovimientoCarneIdentidad>();
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_MOVIMIENTO_CARNE_IDENTIDAD_CONSULTAR", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_CARNE_IDENTIDAD_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosMovimientos.CarneIdentidadid;

            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                int posMOV_USUARIO = drd.GetOrdinal("MOV_USUARIO");
                int posMOV_FECHA = drd.GetOrdinal("MOV_FECHA");
                int posMOV_HORA = drd.GetOrdinal("MOV_HORA");
                int posMOV_ESTADO = drd.GetOrdinal("MOV_ESTADO");
                int posMOV_OFICINA = drd.GetOrdinal("MOV_OFICINA");
                int posMOV_TIPOOBS = drd.GetOrdinal("MOV_TIPOOBS");
                int posMOV_OBSDESC = drd.GetOrdinal("MOV_OBSDESC");
                beMovimientoCarneIdentidad obeMovimientoCarneIdentidad;
                while (drd.Read())
                {
                    obeMovimientoCarneIdentidad = new beMovimientoCarneIdentidad();
                    obeMovimientoCarneIdentidad.MovUsuario = drd.GetString(posMOV_USUARIO);
                    obeMovimientoCarneIdentidad.MovFecha = drd.GetString(posMOV_FECHA);
                    obeMovimientoCarneIdentidad.MovHora = drd.GetString(posMOV_HORA);
                    obeMovimientoCarneIdentidad.MovEstado = drd.GetString(posMOV_ESTADO);
                    obeMovimientoCarneIdentidad.MovOficina = drd.GetString(posMOV_OFICINA);
                    obeMovimientoCarneIdentidad.MovTipoObs = drd.GetString(posMOV_TIPOOBS);
                    obeMovimientoCarneIdentidad.MovObsDesc = drd.GetString(posMOV_OBSDESC);
                    lbeMovimientoCarneIdentidad.Add(obeMovimientoCarneIdentidad);
                }
                drd.Close();
            }
            return (lbeMovimientoCarneIdentidad);
        }
    }
}
