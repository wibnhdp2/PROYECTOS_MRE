using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daActaConformidadDetalle
    {
        public short adicionar(SqlConnection con, SqlTransaction trx, beActaConformidadDetalle parametros)
		{
            short idActaConformidadDetalle = -1;


            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_ACTA_CONFORMIDAD_DETALLE_ADICIONAR", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_ACTA_CONF_CAB_ID", SqlDbType.SmallInt);
			par1.Direction = ParameterDirection.Input;
			par1.Value = parametros.ActaConformidadCabId;


            SqlParameter par2 = cmd.Parameters.Add("@P_CARNE_IDENTIDAD_ID", SqlDbType.Int);
			par2.Direction = ParameterDirection.Input;
			par2.Value = parametros.CarneIdentidadId;

            SqlParameter par4 = cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.SmallInt);
			par4.Direction = ParameterDirection.Input;
			par4.Value = parametros.UsuarioCreacion;


            SqlParameter par5 = cmd.Parameters.Add("@P_IP_CREACION", SqlDbType.VarChar, 50);
			par5.Direction = ParameterDirection.Input;
			par5.Value = parametros.IpCreacion;

			SqlParameter par10 = cmd.Parameters.Add("@@identity", SqlDbType.Int);
			par10.Direction = ParameterDirection.ReturnValue;


			int n = cmd.ExecuteNonQuery();
            if (n > 0) idActaConformidadDetalle = Convert.ToInt16(par10.Value);
            return (idActaConformidadDetalle);
		}

    }
}
