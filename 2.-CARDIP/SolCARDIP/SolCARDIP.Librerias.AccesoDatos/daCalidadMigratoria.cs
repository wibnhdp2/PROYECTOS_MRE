using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daCalidadMigratoria
    {
        public short adicionarPri(SqlConnection con, SqlTransaction trx, beCalidadMigratoria parametrosCalidadMigratoria)
        {
            short CalidadMigratoriaPri = -1;
            SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_CALIDAD_MIGRATORIA_ADICIONAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_NOMBRE", SqlDbType.VarChar, 100);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosCalidadMigratoria.Nombre;

            SqlParameter par2 = cmd.Parameters.Add("@P_DEFINICION", SqlDbType.VarChar, 1000);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametrosCalidadMigratoria.Definicion;
            
            SqlParameter par3 = cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.SmallInt);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametrosCalidadMigratoria.Usuariocreacion;

            SqlParameter par4 = cmd.Parameters.Add("@P_IP_CREACION", SqlDbType.VarChar, 50);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametrosCalidadMigratoria.Ipcreacion;

            SqlParameter par5 = cmd.Parameters.Add("@@IDENTITY", SqlDbType.SmallInt);
            par5.Direction = ParameterDirection.ReturnValue;

            int n = cmd.ExecuteNonQuery();
            if (n > 0) CalidadMigratoriaPri = Convert.ToInt16(par5.Value);
            return (CalidadMigratoriaPri);
        }

        public short adicionarCargo(SqlConnection con, SqlTransaction trx, beCalidadMigratoria parametros)
        {
            short CalidadMigratoriaSec = -1;
            SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_CALIDAD_MIGRATORIA_SEC_ADICIONAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par7 = cmd.Parameters.Add("@P_GENERO", SqlDbType.SmallInt);
            par7.Direction = ParameterDirection.Input;
            par7.Value = parametros.GeneroId;

            SqlParameter par1 = cmd.Parameters.Add("@P_NOMBRE", SqlDbType.VarChar, 100);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.Nombre;

            SqlParameter par2 = cmd.Parameters.Add("@P_FLAG_TIT_DEP", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.FlagTitularDependiente;

            SqlParameter par3 = cmd.Parameters.Add("@P_REFEENCIA", SqlDbType.SmallInt);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros.ReferenciaId;

            SqlParameter par4 = cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.SmallInt);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametros.Usuariocreacion;

            SqlParameter par5 = cmd.Parameters.Add("@P_IP_CREACION", SqlDbType.VarChar, 50);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametros.Ipcreacion;

            SqlParameter par6 = cmd.Parameters.Add("@@IDENTITY", SqlDbType.SmallInt);
            par6.Direction = ParameterDirection.ReturnValue;

            int n = cmd.ExecuteNonQuery();
            if (n > 0) CalidadMigratoriaSec = Convert.ToInt16(par6.Value);
            return (CalidadMigratoriaSec);
        }

        public bool actualizarCargo(SqlConnection con, SqlTransaction trx, beCalidadMigratoria parametros)
        {
            bool exito = false;
            SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_CALIDAD_MIGRATORIA_SEC_ACTUALIZAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par6 = cmd.Parameters.Add("@P_CALMIG_ID", SqlDbType.SmallInt);
            par6.Direction = ParameterDirection.Input;
            par6.Value = parametros.CalidadMigratoriaid;

            SqlParameter par7 = cmd.Parameters.Add("@P_GENERO", SqlDbType.SmallInt);
            par7.Direction = ParameterDirection.Input;
            par7.Value = parametros.GeneroId;

            SqlParameter par1 = cmd.Parameters.Add("@P_NOMBRE", SqlDbType.VarChar, 100);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.Nombre;

            SqlParameter par2 = cmd.Parameters.Add("@P_FLAG_TIT_DEP", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.FlagTitularDependiente;

            SqlParameter par3 = cmd.Parameters.Add("@P_REFEENCIA", SqlDbType.SmallInt);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros.ReferenciaId;

            SqlParameter par4 = cmd.Parameters.Add("@P_USUARIO_MODIFICACION", SqlDbType.SmallInt);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametros.Usuariocreacion;

            SqlParameter par5 = cmd.Parameters.Add("@P_IP_MODIFICACION", SqlDbType.VarChar, 50);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametros.Ipcreacion;

            int n = cmd.ExecuteNonQuery();
            if (n != 0)
            {
                exito = true;
            }
            return (exito);
        }

        public short validarCalidadMigratoria(SqlConnection con, SqlTransaction trx, beCalidadMigratoria parametrosCalidadMigratoria)
        {
            short CalidadMigratoria = -2;
            SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_CALIDAD_MIGRATORIA_VALIDAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_NOMBRE", SqlDbType.VarChar, 100);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosCalidadMigratoria.Nombre;

            SqlParameter par2 = cmd.Parameters.Add("@P_FLAG_NIVEL_CALIDAD", SqlDbType.Bit);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametrosCalidadMigratoria.FlagNivelCalidad;

            if (parametrosCalidadMigratoria.FlagTitularDependiente == 0) { SqlParameter par3 = cmd.Parameters.Add("@P_FLAG_TIT_DEP", DBNull.Value); }
            else
            {
                SqlParameter par3 = cmd.Parameters.Add("@P_FLAG_TIT_DEP", SqlDbType.SmallInt);
                par3.Direction = ParameterDirection.Input;
                par3.Value = parametrosCalidadMigratoria.FlagTitularDependiente;
            }

            SqlParameter par4 = cmd.Parameters.Add("@return_value", SqlDbType.SmallInt);
            par4.Direction = ParameterDirection.ReturnValue;

            cmd.ExecuteNonQuery();
            CalidadMigratoria = Convert.ToInt16(par4.Value);
            return (CalidadMigratoria);
        }

        public short validarCargo(SqlConnection con, SqlTransaction trx, beCalidadMigratoria parametros)
        {
            short CalidadMigratoria = -2;
            SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_CALIDAD_MIGRATORIA_SEC_VALIDAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_NOMBRE", SqlDbType.VarChar, 100);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.Nombre;

            SqlParameter par2 = cmd.Parameters.Add("@P_REFEENCIA", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.ReferenciaId;

            SqlParameter par3 = cmd.Parameters.Add("@P_FLAG_NIVEL_CALIDAD", SqlDbType.Bit);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros.FlagNivelCalidad;

            SqlParameter par4 = cmd.Parameters.Add("@P_FLAG_TIT_DEP", SqlDbType.SmallInt);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametros.FlagTitularDependiente;

            SqlParameter par6 = cmd.Parameters.Add("@P_GENERO", SqlDbType.SmallInt);
            par6.Direction = ParameterDirection.Input;
            par6.Value = parametros.GeneroId;

            SqlParameter par5 = cmd.Parameters.Add("@return_value", SqlDbType.SmallInt);
            par5.Direction = ParameterDirection.ReturnValue;

            cmd.ExecuteNonQuery();
            CalidadMigratoria = Convert.ToInt16(par5.Value);
            return (CalidadMigratoria);
        }
    }
}
