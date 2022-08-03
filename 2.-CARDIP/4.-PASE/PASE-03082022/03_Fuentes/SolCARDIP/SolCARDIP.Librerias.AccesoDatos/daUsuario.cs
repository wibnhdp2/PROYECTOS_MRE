using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daUsuario
    {
        public List<beUsuario> listaUsuarios(SqlConnection con, string abreSistema, short Oficina)
        {
            List<beUsuario> lista = new List<beUsuario>();
            SqlCommand cmd = new SqlCommand("SC_GENERAL.USP_GR_OBTENER_USUARIOS", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_SISTEMA_ABR", SqlDbType.VarChar, 10);
            par1.Direction = ParameterDirection.Input;
            par1.Value = abreSistema;

            SqlParameter par2 = cmd.Parameters.Add("@P_OFICINA_CONSULAR_ID", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = Oficina;

            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int posidUsuario = drd.GetOrdinal("IDUSUARIO");
                int posAlias = drd.GetOrdinal("ALIAS");
                int posNombreCompleto = drd.GetOrdinal("NOMBRECOMPLETO");
                int posRol = drd.GetOrdinal("ROL");
                int posidOficinaConsular = drd.GetOrdinal("IDOFICON");
                int posNombreOficinaConsular = drd.GetOrdinal("OFICINA");
                beUsuario obeUsuario;
                while (drd.Read())
                {
                    obeUsuario = new beUsuario();
                    obeUsuario.Usuarioid = drd.GetInt16(posidUsuario);
                    obeUsuario.Alias = drd.GetString(posAlias);
                    obeUsuario.NombreCompleto = drd.GetString(posNombreCompleto);
                    obeUsuario.Rol = drd.GetString(posRol);
                    obeUsuario.idOficinaConsular = drd.GetInt16(posidOficinaConsular);
                    obeUsuario.NombreOficinaConsular = drd.GetString(posNombreOficinaConsular);
                    lista.Add(obeUsuario);
                }
                drd.Close();
            }
            return (lista);
        }

        public bool bloqueoActiva(SqlConnection con, SqlTransaction trx, beUsuario parametros_Crea, beUsuario parametros_Bloq, string comentario)
        {
            bool exito = false;

            SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIO_ACTUALIZAR_SESION_BLOQUEADA", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@usua_bBloqueoActiva", SqlDbType.Bit);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros_Bloq.BloqueoActiva;

            SqlParameter par2 = cmd.Parameters.Add("@usua_sUsuarioId_Crea", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros_Crea.Usuarioid;

            SqlParameter par7 = cmd.Parameters.Add("@usua_sUsuarioId_Bloq", SqlDbType.SmallInt);
            par7.Direction = ParameterDirection.Input;
            par7.Value = parametros_Bloq.Usuarioid;

            SqlParameter par3 = cmd.Parameters.Add("@usua_vDireccionIP", SqlDbType.VarChar, 50);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros_Bloq.Ipmodificacion;

            SqlParameter par4 = cmd.Parameters.Add("@usua_vHostName", SqlDbType.VarChar, 20);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametros_Bloq.Ipmodificacion;

            SqlParameter par6 = cmd.Parameters.Add("@audi_vComentario", SqlDbType.VarChar, 200);
            par6.Direction = ParameterDirection.Input;
            par6.Value = comentario;

            SqlParameter par5 = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            par5.Direction = ParameterDirection.ReturnValue;

            cmd.ExecuteNonQuery();
            int n = -1;
            n = (int)par5.Value;
            if (n > -1) { exito = true; }
            return (exito);
        }

        public List<beUsuario> obtenerInfoUsuarios(SqlConnection con, beUsuario parametros)
        {
            List<beUsuario> listaUsuarios = new List<beUsuario>();
            SqlCommand cmd = new SqlCommand("SC_GENERAL.USP_GR_OBTENER_INFO_USUARIOS", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_USUARIO_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.Usuarioid;

            SqlParameter par2 = cmd.Parameters.Add("@P_ROL", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.Rol_Id;

            SqlParameter par3 = cmd.Parameters.Add("@P_BLOQ", SqlDbType.Bit);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros.BloqueoActiva;

            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                int posUSUARIO_ID = drd.GetOrdinal("USUARIO_ID");
                int posUSUARIO_APEPAT = drd.GetOrdinal("USUARIO_APEPAT");
                int posUSUARIO_APEMAT = drd.GetOrdinal("USUARIO_APEMAT");
                int posUSUARIO_NOMBRES = drd.GetOrdinal("USUARIO_NOMBRES");
                int posBLOQUEO = drd.GetOrdinal("BLOQUEO");
                int posDOCTIPO_ID = drd.GetOrdinal("DOCTIPO_ID");
                int posDOCTIPO = drd.GetOrdinal("DOCTIPO");
                int posDOCNUMERO = drd.GetOrdinal("DOCNUMERO");
                int posUSUARIO_CORREO = drd.GetOrdinal("USUARIO_CORREO");
                int posUSUARIO_ALIAS = drd.GetOrdinal("USUARIO_ALIAS");
                int posUSUARIO_ROL_ID = drd.GetOrdinal("USUARIO_ROL_ID");
                int posPERFIL_ID = drd.GetOrdinal("PERFIL_ID");
                int posPERFIL = drd.GetOrdinal("PERFIL");
                beUsuario obeUsuario;
                while (drd.Read())
                {
                    obeUsuario = new beUsuario();
                    obeUsuario.Usuarioid = drd.GetInt16(posUSUARIO_ID);
                    obeUsuario.Apellidopaterno = drd.GetString(posUSUARIO_APEPAT);
                    obeUsuario.Apellidomaterno = drd.GetString(posUSUARIO_APEMAT);
                    obeUsuario.Nombres = drd.GetString(posUSUARIO_NOMBRES);
                    obeUsuario.Documentotipoid = drd.GetInt16(posDOCTIPO_ID);
                    obeUsuario.TipoDocIdentidad = drd.GetString(posDOCTIPO);
                    obeUsuario.Documentonumero = drd.GetString(posDOCNUMERO);
                    obeUsuario.Correoelectronico = drd.GetString(posUSUARIO_CORREO);
                    obeUsuario.Alias = drd.GetString(posUSUARIO_ALIAS);
                    obeUsuario.UsuarioRolId = drd.GetInt16(posUSUARIO_ROL_ID);
                    obeUsuario.Rol_Id = drd.GetInt16(posPERFIL_ID);
                    obeUsuario.Rol = drd.GetString(posPERFIL);
                    obeUsuario.BloqueoActiva = drd.GetBoolean(posBLOQUEO);
                    listaUsuarios.Add(obeUsuario);
                }
                drd.Close();
            }
            return (listaUsuarios);
        }

        public short adicionar(SqlConnection con, SqlTransaction trx, beUsuario parametros)
        {
            short UsuarioId = -1;

            SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIO_ADICIONAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par2 = cmd.Parameters.Add("@P_TIPO_IDENT", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.Documentotipoid;

            SqlParameter par3 = cmd.Parameters.Add("@P_NUMERO_DOC_IDENT", SqlDbType.VarChar, 20);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros.Documentonumero;

            SqlParameter par4 = cmd.Parameters.Add("@P_APELLIDO_PAT", SqlDbType.VarChar, 100);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametros.Apellidopaterno;

            SqlParameter par5 = cmd.Parameters.Add("@P_APELLIDO_MAT", SqlDbType.VarChar, 100);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametros.Apellidomaterno;

            SqlParameter par6 = cmd.Parameters.Add("@P_NOMBRES", SqlDbType.VarChar, 200);
            par6.Direction = ParameterDirection.Input;
            par6.Value = parametros.Nombres;

            SqlParameter par7 = cmd.Parameters.Add("@P_CORREO", SqlDbType.VarChar, 100);
            par7.Direction = ParameterDirection.Input;
            par7.Value = parametros.Correoelectronico;

            SqlParameter par8 = cmd.Parameters.Add("@P_ALIAS", SqlDbType.VarChar, 50);
            par8.Direction = ParameterDirection.Input;
            par8.Value = parametros.Alias;

            SqlParameter par9 = cmd.Parameters.Add("@P_USUARIO_CREA", SqlDbType.SmallInt);
            par9.Direction = ParameterDirection.Input;
            par9.Value = parametros.Usuariocreacion;

            SqlParameter par10 = cmd.Parameters.Add("@P_IP_CREA", SqlDbType.VarChar, 50);
            par10.Direction = ParameterDirection.Input;
            par10.Value = parametros.Ipcreacion;

            SqlParameter par1 = cmd.Parameters.Add("@@IDENTITY", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.ReturnValue;

            int n = cmd.ExecuteNonQuery();
            if (n > 0) UsuarioId = Convert.ToInt16(par1.Value);
            return (UsuarioId);
        }

        public bool actualizar(SqlConnection con, SqlTransaction trx, beUsuario parametros)
        {
            bool exito = false;

            SqlCommand cmd = new SqlCommand("PS_SEGURIDAD.USP_SE_USUARIO_ACTUALIZAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_USUARIO_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.Usuarioid;

            SqlParameter par2 = cmd.Parameters.Add("@P_TIPO_IDENT", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.Documentotipoid;

            SqlParameter par3 = cmd.Parameters.Add("@P_NUMERO_DOC_IDENT", SqlDbType.VarChar, 20);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros.Documentonumero;

            SqlParameter par4 = cmd.Parameters.Add("@P_APELLIDO_PAT", SqlDbType.VarChar, 100);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametros.Apellidopaterno;

            SqlParameter par5 = cmd.Parameters.Add("@P_APELLIDO_MAT", SqlDbType.VarChar, 100);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametros.Apellidomaterno;

            SqlParameter par6 = cmd.Parameters.Add("@P_NOMBRES", SqlDbType.VarChar, 200);
            par6.Direction = ParameterDirection.Input;
            par6.Value = parametros.Nombres;

            SqlParameter par7 = cmd.Parameters.Add("@P_CORREO", SqlDbType.VarChar, 100);
            par7.Direction = ParameterDirection.Input;
            par7.Value = parametros.Correoelectronico;

            SqlParameter par8 = cmd.Parameters.Add("@P_ALIAS", SqlDbType.VarChar, 50);
            par8.Direction = ParameterDirection.Input;
            par8.Value = parametros.Alias;

            SqlParameter par9 = cmd.Parameters.Add("@P_USUARIO_MOD", SqlDbType.SmallInt);
            par9.Direction = ParameterDirection.Input;
            par9.Value = parametros.Usuariomodificacion;

            SqlParameter par10 = cmd.Parameters.Add("@P_IP_MOD", SqlDbType.VarChar, 50);
            par10.Direction = ParameterDirection.Input;
            par10.Value = parametros.Ipmodificacion;

            int n = cmd.ExecuteNonQuery();
            if (n != 0)
            {
                exito = true;
            }
            return (exito);
        }
    }
}
