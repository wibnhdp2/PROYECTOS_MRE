using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daPersona
    {
        public long adicionar(SqlConnection con, SqlTransaction trx, bePersona parametrosPersona)
        {
            long PersonaId = -1;
            string Fecha = "01/01/0001";
            DateTime FechaNull = DateTime.Parse(Fecha);
            SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_ADICIONAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_ESTADO_CIVIL", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosPersona.Estadocivilid;

            SqlParameter par2 = cmd.Parameters.Add("@P_GENERO", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametrosPersona.Generoid;

            SqlParameter par3 = cmd.Parameters.Add("@P_APELLIDO_PAT", SqlDbType.VarChar, 100);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametrosPersona.Apellidopaterno;

            if (parametrosPersona.Apellidomaterno == null) { SqlParameter par4 = cmd.Parameters.Add("@P_APELLIDO_MAT", DBNull.Value); }
            else
            {
                SqlParameter par4 = cmd.Parameters.Add("@P_APELLIDO_MAT", SqlDbType.VarChar, 100);
                par4.Direction = ParameterDirection.Input;
                par4.Value = parametrosPersona.Apellidomaterno;
            }

            SqlParameter par5 = cmd.Parameters.Add("@P_NOMBRES", SqlDbType.VarChar, 100);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametrosPersona.Nombres;

            if (parametrosPersona.Telefono == null) { SqlParameter par11 = cmd.Parameters.Add("@P_TELEFONO", DBNull.Value); }
            else
            {
                SqlParameter par11 = cmd.Parameters.Add("@P_TELEFONO", SqlDbType.VarChar, 20);
                par11.Direction = ParameterDirection.Input;
                par11.Value = parametrosPersona.Telefono;
            }

            SqlParameter par6 = cmd.Parameters.Add("@P_FECHA_NAC", SqlDbType.DateTime);
            par6.Direction = ParameterDirection.Input;
            par6.Value = parametrosPersona.Nacimientofecha;

            SqlParameter par7 = cmd.Parameters.Add("@P_PAIS", SqlDbType.SmallInt);
            par7.Direction = ParameterDirection.Input;
            par7.Value = parametrosPersona.Paisid;

            SqlParameter par8 = cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.SmallInt);
            par8.Direction = ParameterDirection.Input;
            par8.Value = parametrosPersona.Usuariocreacion;

            SqlParameter par9 = cmd.Parameters.Add("@P_IP_CREACION", SqlDbType.VarChar, 50);
            par9.Direction = ParameterDirection.Input;
            par9.Value = parametrosPersona.Ipcreacion;

            SqlParameter par12 = cmd.Parameters.Add("@P_MENOR_EDAD", SqlDbType.Bit);
            par12.Direction = ParameterDirection.Input;
            par12.Value = parametrosPersona.MenorEdad;

            SqlParameter par10 = cmd.Parameters.Add("@@IDENTITY", SqlDbType.BigInt);
            par10.Direction = ParameterDirection.ReturnValue;

            long n = cmd.ExecuteNonQuery();
            if (n > 0) PersonaId = Convert.ToInt64(par10.Value);
            return (PersonaId);
        }

        public bool actualizar(SqlConnection con, SqlTransaction trx, bePersona parametrosPersona)
        {
            bool exito = false;
            SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_PERSONA_ACTUALIZAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par10 = cmd.Parameters.Add("@P_PERSONA_ID", SqlDbType.BigInt);
            par10.Direction = ParameterDirection.Input;
            par10.Value = parametrosPersona.Personaid;

            SqlParameter par1 = cmd.Parameters.Add("@P_ESTADO_CIVIL", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosPersona.Estadocivilid;

            SqlParameter par2 = cmd.Parameters.Add("@P_GENERO", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametrosPersona.Generoid;

            SqlParameter par3 = cmd.Parameters.Add("@P_APELLIDO_PAT", SqlDbType.VarChar, 100);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametrosPersona.Apellidopaterno;

            if (parametrosPersona.Apellidomaterno == null) { SqlParameter par4 = cmd.Parameters.Add("@P_APELLIDO_MAT", DBNull.Value); }
            else
            {
                SqlParameter par4 = cmd.Parameters.Add("@P_APELLIDO_MAT", SqlDbType.VarChar, 100);
                par4.Direction = ParameterDirection.Input;
                par4.Value = parametrosPersona.Apellidomaterno;
            }

            SqlParameter par5 = cmd.Parameters.Add("@P_NOMBRES", SqlDbType.VarChar, 100);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametrosPersona.Nombres;

            if (parametrosPersona.Telefono == null) { SqlParameter par11 = cmd.Parameters.Add("@P_TELEFONO", DBNull.Value); }
            else
            {
                SqlParameter par11 = cmd.Parameters.Add("@P_TELEFONO", SqlDbType.VarChar, 20);
                par11.Direction = ParameterDirection.Input;
                par11.Value = parametrosPersona.Telefono;
            }

            SqlParameter par6 = cmd.Parameters.Add("@P_FECHA_NAC", SqlDbType.DateTime);
            par6.Direction = ParameterDirection.Input;
            par6.Value = parametrosPersona.Nacimientofecha;

            SqlParameter par7 = cmd.Parameters.Add("@P_PAIS", SqlDbType.SmallInt);
            par7.Direction = ParameterDirection.Input;
            par7.Value = parametrosPersona.Paisid;

            SqlParameter par8 = cmd.Parameters.Add("@P_USUARIO_MODIFICACION", SqlDbType.SmallInt);
            par8.Direction = ParameterDirection.Input;
            par8.Value = parametrosPersona.Usuariomodificacion;

            SqlParameter par9 = cmd.Parameters.Add("@P_IP_MODIFICACION", SqlDbType.VarChar, 50);
            par9.Direction = ParameterDirection.Input;
            par9.Value = parametrosPersona.Ipmodificacion;

            SqlParameter par12 = cmd.Parameters.Add("@P_MENOR_EDAD", SqlDbType.Bit);
            par12.Direction = ParameterDirection.Input;
            par12.Value = parametrosPersona.MenorEdad;

            int n = cmd.ExecuteNonQuery();
            if (n != 0)
            {
                exito = true;
            }
            return (exito);
        }
    }
}
