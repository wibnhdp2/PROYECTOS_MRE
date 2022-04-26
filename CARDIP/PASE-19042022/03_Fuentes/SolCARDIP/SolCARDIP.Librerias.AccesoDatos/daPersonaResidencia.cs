using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daPersonaResidencia
    {
        public long adicionar(SqlConnection con, SqlTransaction trx, bePersonaresidencia parametrosPersResid)
        {
            long PersonaResidenciaId = -1;
            SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_RESIDENCIA_ADICIONAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_PERSONA_ID", SqlDbType.BigInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosPersResid.Personaid;

            SqlParameter par2 = cmd.Parameters.Add("@P_RESIDENCIA_ID", SqlDbType.BigInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametrosPersResid.Residenciaid;

            SqlParameter par3 = cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.SmallInt);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametrosPersResid.Usuariocreacion;

            SqlParameter par4 = cmd.Parameters.Add("@P_IP_CREACION", SqlDbType.VarChar, 50);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametrosPersResid.Ipcreacion;

            SqlParameter par5 = cmd.Parameters.Add("@@IDENTITY", SqlDbType.BigInt);
            par5.Direction = ParameterDirection.ReturnValue;

            int n = cmd.ExecuteNonQuery();
            if (n > 0) PersonaResidenciaId = Convert.ToInt64(par5.Value);
            return (PersonaResidenciaId);

        }
    }
}
