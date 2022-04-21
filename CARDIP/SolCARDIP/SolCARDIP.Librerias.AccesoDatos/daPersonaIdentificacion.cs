using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;
namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daPersonaIdentificacion
    {
        public long adicionar(SqlConnection con, SqlTransaction trx, bePersonaidentificacion parametrosPersonaIdent)
        {
            long PersonaIdentId = -1;
            SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_IDENTIFICACION_ADICIONAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_PERSONA_ID", SqlDbType.BigInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosPersonaIdent.Personaid;

            SqlParameter par2 = cmd.Parameters.Add("@P_DOCUMENTO_TIPO_ID", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametrosPersonaIdent.Documentotipoid;

            SqlParameter par3 = cmd.Parameters.Add("@P_DOCUMENTO_NUMERO", SqlDbType.VarChar, 20);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametrosPersonaIdent.Documentonumero;

            SqlParameter par4 = cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.SmallInt);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametrosPersonaIdent.Usuariocreacion;

            SqlParameter par5 = cmd.Parameters.Add("@P_IP_CREACION", SqlDbType.VarChar, 50);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametrosPersonaIdent.Ipcreacion;

            SqlParameter par6 = cmd.Parameters.Add("@@IDENTITY", SqlDbType.BigInt);
            par6.Direction = ParameterDirection.ReturnValue;

            int n = cmd.ExecuteNonQuery();
            if (n > 0) PersonaIdentId = Convert.ToInt64(par6.Value);
            return (PersonaIdentId);

        }

        public long validarPersonaIdentificacion(SqlConnection con, SqlTransaction trx, bePersonaidentificacion parametrosPersonaIdent)
        {
            long PersonaIdentId = 0;
            SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_VALIDAR_PERSONA_IDENTIFICACION", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_DOCUMENTO_TIPO_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosPersonaIdent.Documentotipoid;

            SqlParameter par2 = cmd.Parameters.Add("@P_DOCUMENTO_NUMERO", SqlDbType.VarChar, 20);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametrosPersonaIdent.Documentonumero;

            SqlParameter par3 = cmd.Parameters.Add("@return_value", SqlDbType.BigInt);
            par3.Direction = ParameterDirection.ReturnValue;

            cmd.ExecuteNonQuery();
            PersonaIdentId = Convert.ToInt64(par3.Value);
            return (PersonaIdentId);
        }
    }
}
