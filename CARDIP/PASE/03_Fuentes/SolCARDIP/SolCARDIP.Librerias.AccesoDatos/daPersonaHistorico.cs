using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daPersonaHistorico
    {
        public short adicionar(SqlConnection con, SqlTransaction trx, bePersonaHistorico parametrosPersonaHistorico)
        {
            short PersonaHistoricoId = -1;
            SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_HISTORICO_ADICIONAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_PERSONA_ID", SqlDbType.BigInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosPersonaHistorico.Personaid;

            SqlParameter par2 = cmd.Parameters.Add("@P_APELLIDO_PAT", SqlDbType.VarChar, 100);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametrosPersonaHistorico.ApellidoPaterno;

            SqlParameter par3 = cmd.Parameters.Add("@P_APELLIDO_MAT", SqlDbType.VarChar, 100);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametrosPersonaHistorico.ApellidoMaterno;

            SqlParameter par4 = cmd.Parameters.Add("@P_NOMBRES", SqlDbType.VarChar, 100);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametrosPersonaHistorico.Nombres;

            if (parametrosPersonaHistorico.Telefono == null) { SqlParameter par14 = cmd.Parameters.Add("@P_TELEFONO", DBNull.Value); }
            else
            {
                SqlParameter par14 = cmd.Parameters.Add("@P_TELEFONO", SqlDbType.VarChar, 20);
                par14.Direction = ParameterDirection.Input;
                par14.Value = parametrosPersonaHistorico.Telefono;
            }

            SqlParameter par5 = cmd.Parameters.Add("@P_ESTADO_CIVIL", SqlDbType.SmallInt);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametrosPersonaHistorico.EstadoCivilid;

            SqlParameter par6 = cmd.Parameters.Add("@P_GENERO", SqlDbType.SmallInt);
            par6.Direction = ParameterDirection.Input;
            par6.Value = parametrosPersonaHistorico.Generoid;

            SqlParameter par7 = cmd.Parameters.Add("@P_FECHA_NAC", SqlDbType.DateTime);
            par7.Direction = ParameterDirection.Input;
            par7.Value = parametrosPersonaHistorico.FechaNacimiento;

            SqlParameter par8 = cmd.Parameters.Add("@P_PAIS", SqlDbType.SmallInt);
            par8.Direction = ParameterDirection.Input;
            par8.Value = parametrosPersonaHistorico.PaisNacionalidadid;

            SqlParameter par9 = cmd.Parameters.Add("@P_PERSONA_IDENTIFICACIOID", SqlDbType.BigInt);
            par9.Direction = ParameterDirection.Input;
            par9.Value = parametrosPersonaHistorico.PersonaIdentificacionId;

            SqlParameter par10 = cmd.Parameters.Add("@P_PERSONA_RESIDENCIAID", SqlDbType.BigInt);
            par10.Direction = ParameterDirection.Input;
            par10.Value = parametrosPersonaHistorico.PersonaResindenciaId;

            SqlParameter par11 = cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.SmallInt);
            par11.Direction = ParameterDirection.Input;
            par11.Value = parametrosPersonaHistorico.Usuariocreacion;

            SqlParameter par12 = cmd.Parameters.Add("@P_IP_CREACION", SqlDbType.VarChar, 50);
            par12.Direction = ParameterDirection.Input;
            par12.Value = parametrosPersonaHistorico.Ipcreacion;

            SqlParameter par15 = cmd.Parameters.Add("@P_MENOR_EDAD", SqlDbType.Bit);
            par15.Direction = ParameterDirection.Input;
            par15.Value = parametrosPersonaHistorico.MenorEdad;

            SqlParameter par13 = cmd.Parameters.Add("@@IDENTITY", SqlDbType.BigInt);
            par13.Direction = ParameterDirection.ReturnValue;

            int n = cmd.ExecuteNonQuery();
            if (n > 0) PersonaHistoricoId = Convert.ToInt16(par13.Value);
            return (PersonaHistoricoId);
        }
    }
}
