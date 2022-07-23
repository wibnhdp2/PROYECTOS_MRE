using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daCarneIdentidadRelacionDependencia
    {
        public short adicionar(SqlConnection con, SqlTransaction trx, beCarneIdentidadRelacionDependencia parametros)
        {
            short idRelDep = -1;

            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CARNE_IDENTIDAD_RELACION_DEPENDENCIA_ADICIONAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_CATD_SCARNE_IDENTIDAD_TIT_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.CarneIdentidadTitId;

            SqlParameter par2 = cmd.Parameters.Add("@P_CATD_SCARNE_IDENTIDAD_DEP_ID", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.CarneIdentidadDepId;

            SqlParameter par3 = cmd.Parameters.Add("@P_CATD_SUSUARIO_CREACION", SqlDbType.SmallInt);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros.UsuarioCreacion;

            SqlParameter par4 = cmd.Parameters.Add("@P_CATD_VIP_CREACION", SqlDbType.VarChar, 50);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametros.IpCreacion;

            SqlParameter par5 = cmd.Parameters.Add("@@identity", SqlDbType.SmallInt);
            par5.Direction = ParameterDirection.ReturnValue;

            int n = cmd.ExecuteNonQuery();
            if (n > 0) n = idRelDep = short.Parse(par5.Value.ToString());
            return (idRelDep);
        }

        public bool actualizar(SqlConnection con, SqlTransaction trx, beCarneIdentidadRelacionDependencia parametros)
        {
            bool exito = false;
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CARNE_IDENTIDAD_RELACION_DEPENDENCIA_ACTUALIZA", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par0 = cmd.Parameters.Add("@P_CATD_STITULAR_DEPENDIENTE_ID", SqlDbType.SmallInt);
            par0.Direction = ParameterDirection.Input;
            par0.Value = parametros.TitularDependienteId;

            SqlParameter par1 = cmd.Parameters.Add("@P_CATD_SCARNE_IDENTIDAD_TIT_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.CarneIdentidadTitId;

            SqlParameter par2 = cmd.Parameters.Add("@P_CATD_SCARNE_IDENTIDAD_DEP_ID", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.CarneIdentidadDepId;

            SqlParameter par3 = cmd.Parameters.Add("@P_CATD_SUSUARIO_MODIFICACION", SqlDbType.SmallInt);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros.UsuarioModificacion;

            SqlParameter par4 = cmd.Parameters.Add("@P_CATD_VIP_MODIFICACION", SqlDbType.VarChar, 50);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametros.IpModificacion;

            int n = cmd.ExecuteNonQuery();
            if (n != 0)
            {
                exito = true;
            }
            return (exito);
        }
    }
}
