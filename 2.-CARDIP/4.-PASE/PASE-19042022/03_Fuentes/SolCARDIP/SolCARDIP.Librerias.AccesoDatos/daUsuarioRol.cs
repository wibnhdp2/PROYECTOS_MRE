using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daUsuarioRol
    {
        public beUsuarioRol usuarioAutenticar(SqlConnection con, beUsuarioRol parametros)
        {
            beUsuarioRol obeUsuarioRol = new beUsuarioRol();
            SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIOROL_AUTENTICAR", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@usro_iAplicacionId", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.idSIstema;

            SqlParameter par2 = cmd.Parameters.Add("@usua_vAlias", SqlDbType.VarChar, 50);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.usuarioAlias;

            SqlParameter par3 = cmd.Parameters.Add("@vHostName", SqlDbType.VarChar, 20);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros.Ipmodificacion;

            SqlParameter par4 = cmd.Parameters.Add("@vDireccionIP", SqlDbType.VarChar, 20);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametros.Ipmodificacion;

            SqlDataReader drd = cmd.ExecuteReader(CommandBehavior.SingleRow);

            if (drd != null)
            {
                int posUsuariorolid = drd.GetOrdinal("usro_sUsuarioRolId");
                int posUsuarioid = drd.GetOrdinal("usro_sUsuarioId");
                int posusuarioAlias = drd.GetOrdinal("usua_vAlias");
                int posOficinaconsularid = drd.GetOrdinal("usro_sOficinaConsularId");
                if (drd.HasRows)
                {
                    drd.Read();
                    obeUsuarioRol.Usuariorolid = drd.GetInt16(posUsuariorolid);
                    obeUsuarioRol.Usuarioid = drd.GetInt16(posUsuarioid);
                    obeUsuarioRol.usuarioAlias = drd.GetString(posusuarioAlias);
                    obeUsuarioRol.Oficinaconsularid = drd.GetInt16(posOficinaconsularid);
                }
                drd.Close();
            }
            return (obeUsuarioRol);
        }

        public short adicionar(SqlConnection con, SqlTransaction trx, beUsuarioRol parametros)
        {
            short UsuarioRolId = -1;

            SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIO_ROL_ADICIONAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par2 = cmd.Parameters.Add("@P_USUARIO_ID", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.Usuarioid;

            SqlParameter par3 = cmd.Parameters.Add("@P_ROLCON_ID", SqlDbType.SmallInt);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros.Rolconfiguracionid;

            SqlParameter par6 = cmd.Parameters.Add("@P_OFICINA_CONSULAR", SqlDbType.SmallInt);
            par6.Direction = ParameterDirection.Input;
            par6.Value = parametros.Oficinaconsularid;

            SqlParameter par4 = cmd.Parameters.Add("@P_USUARIO_CREA", SqlDbType.SmallInt);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametros.Usuariocreacion;

            SqlParameter par5 = cmd.Parameters.Add("@P_IP_CREA", SqlDbType.VarChar, 50);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametros.Ipcreacion;

            SqlParameter par1 = cmd.Parameters.Add("@@IDENTITY", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.ReturnValue;

            int n = cmd.ExecuteNonQuery();
            if (n > 0) UsuarioRolId = Convert.ToInt16(par1.Value);
            return (UsuarioRolId);
        }

        public bool actualizar(SqlConnection con, SqlTransaction trx, beUsuarioRol parametros)
        {
            bool exito = false;

            SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIO_ROL_ACTUALIZAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_USUARIO_ROL_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.Usuariorolid;

            SqlParameter par2 = cmd.Parameters.Add("@P_USUARIO_ID", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.Usuarioid;

            SqlParameter par3 = cmd.Parameters.Add("@P_ROLCON_ID", SqlDbType.SmallInt);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros.Rolconfiguracionid;

            SqlParameter par4 = cmd.Parameters.Add("@P_USUARIO_MOD", SqlDbType.SmallInt);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametros.Usuariomodificacion;

            SqlParameter par5 = cmd.Parameters.Add("@P_IP_MOD", SqlDbType.VarChar, 50);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametros.Ipmodificacion;

            int n = cmd.ExecuteNonQuery();
            if (n != 0)
            {
                exito = true;
            }
            return (exito);
        }
    }
}
