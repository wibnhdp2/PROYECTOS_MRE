using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daResidencia
    {
        public long adicionar(SqlConnection con, SqlTransaction trx, beResidencia parametrosResidencia)
        {
            long ResidenciaId = -1;
            SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_RESIDENCIA_ADICIONAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_DIRECCION", SqlDbType.VarChar, 500);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosResidencia.Residenciadireccion;

            SqlParameter par2 = cmd.Parameters.Add("@P_UBIGEO", SqlDbType.Char, 6);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametrosResidencia.Residenciaubigeo;

            SqlParameter par3 = cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.SmallInt);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametrosResidencia.Usuariocreacion;

            SqlParameter par4 = cmd.Parameters.Add("@P_IP_CREACION", SqlDbType.VarChar, 50);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametrosResidencia.Ipcreacion;

            SqlParameter par5 = cmd.Parameters.Add("@@IDENTITY", SqlDbType.BigInt);
            par5.Direction = ParameterDirection.ReturnValue;

            int n = cmd.ExecuteNonQuery();
            if (n > 0) ResidenciaId = Convert.ToInt64(par5.Value);
            return (ResidenciaId);

        }
    }
}
