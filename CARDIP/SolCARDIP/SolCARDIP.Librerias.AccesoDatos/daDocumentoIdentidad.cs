using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daDocumentoIdentidad
    {
        public short adicionar(SqlConnection con, SqlTransaction trx, beDocumentoIdentidad parametrosDocumentoIdentidad)
        {
            short DocumentoIdentidadId = -1;
            SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_DOCUMENTO_IDENTIDAD_ADICIONAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_DESCRIPCION_CORTA", SqlDbType.VarChar, 50);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosDocumentoIdentidad.DescripcionCorta;

            SqlParameter par2 = cmd.Parameters.Add("@P_DESCRIPCION_LARGA", SqlDbType.VarChar, 100);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametrosDocumentoIdentidad.DescripcionLarga;

            SqlParameter par3 = cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.SmallInt);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametrosDocumentoIdentidad.Usuariocreacion;

            SqlParameter par4 = cmd.Parameters.Add("@P_IP_CREACION", SqlDbType.VarChar, 50);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametrosDocumentoIdentidad.Ipcreacion;

            SqlParameter par5 = cmd.Parameters.Add("@@IDENTITY", SqlDbType.SmallInt);
            par5.Direction = ParameterDirection.ReturnValue;

            int n = cmd.ExecuteNonQuery();
            if (n > 0) DocumentoIdentidadId = Convert.ToInt16(par5.Value);
            return (DocumentoIdentidadId);
        }

        public bool actualizar(SqlConnection con, SqlTransaction trx, beDocumentoIdentidad parametrosDocumentoIdentidad)
        {
            bool exito = false;
            SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_DOCUMENTO_IDENTIDAD_ACTUALIZAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_DOC_IDENT_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosDocumentoIdentidad.Tipodocumentoidentidadid;

            SqlParameter par2 = cmd.Parameters.Add("@P_DESCRIPCION_CORTA", SqlDbType.VarChar, 50);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametrosDocumentoIdentidad.DescripcionCorta;

            SqlParameter par3 = cmd.Parameters.Add("@P_DESCRIPCION_LARGA", SqlDbType.VarChar, 100);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametrosDocumentoIdentidad.DescripcionLarga;

            SqlParameter par4 = cmd.Parameters.Add("@P_USUARIO_MODIFICACION", SqlDbType.SmallInt);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametrosDocumentoIdentidad.Usuariomodificacion;

            SqlParameter par5 = cmd.Parameters.Add("@P_IP_MODIFICACION", SqlDbType.VarChar, 50);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametrosDocumentoIdentidad.Ipmodificacion;

            int n = cmd.ExecuteNonQuery();
            if (n != 0)
            {
                exito = true;
            }
            return (exito);
        }

        public short validarDocumentoIdentidad(SqlConnection con, SqlTransaction trx, beDocumentoIdentidad parametrosDocumentoIdentidad)
        {
            short DocumentoIdentidadId = -2;
            SqlCommand cmd = new SqlCommand("SC_MAESTRO.USP_MA_DOCUMENTO_IDENTIDAD_VALIDAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_DESCRIPCION_CORTA", SqlDbType.VarChar, 50);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosDocumentoIdentidad.DescripcionCorta;

            SqlParameter par3 = cmd.Parameters.Add("@return_value", SqlDbType.BigInt);
            par3.Direction = ParameterDirection.ReturnValue;

            cmd.ExecuteNonQuery();
            DocumentoIdentidadId = Convert.ToInt16(par3.Value);
            return (DocumentoIdentidadId);
        }
    }
}
