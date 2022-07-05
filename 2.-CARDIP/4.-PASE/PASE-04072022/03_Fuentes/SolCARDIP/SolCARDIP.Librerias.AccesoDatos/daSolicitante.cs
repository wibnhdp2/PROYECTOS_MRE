using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daSolicitante
    {

        public short validarSolicitante(SqlConnection con, SqlTransaction trx, beSolicitante parametros)
        {
            short SolicitanteId = 0;
            SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_VALIDAR_SOLICITANTE", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_DOCUMENTO_TIPO_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.TipoDocumentoIdentidadId;

            SqlParameter par2 = cmd.Parameters.Add("@P_DOCUMENTO_NUMERO", SqlDbType.VarChar, 20);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.NumeroDocumentoIdentidad;

            SqlParameter par3 = cmd.Parameters.Add("@return_value", SqlDbType.SmallInt);
            par3.Direction = ParameterDirection.ReturnValue;

            cmd.ExecuteNonQuery();
            SolicitanteId = Convert.ToInt16(par3.Value);
            return (SolicitanteId);
        }

        public short adicionar(SqlConnection con, SqlTransaction trx, beSolicitante obeSolicitante)
		{
            short idSolicitante = -1;

            SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_SOLICITANTE_ADICIONAR", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Transaction = trx;

			SqlParameter par1 = cmd.Parameters.Add("@P_SOLI_VPRIMER_APELLIDO", SqlDbType.VarChar, 100);
			par1.Direction = ParameterDirection.Input;
			par1.Value = obeSolicitante.PrimerApellido;

            if (obeSolicitante.SegundoApellido == null & obeSolicitante.SegundoApellido.Equals("")) { SqlParameter par2 = cmd.Parameters.Add("@P_SOLI_VSEGUNDO_APELLIDO", DBNull.Value); }
            else
            {
                SqlParameter par2 = cmd.Parameters.Add("@P_SOLI_VSEGUNDO_APELLIDO", SqlDbType.VarChar, 100);
                par2.Direction = ParameterDirection.Input;
                par2.Value = obeSolicitante.SegundoApellido;
            }

			SqlParameter par3 = cmd.Parameters.Add("@P_SOLI_VNOMBRES", SqlDbType.VarChar, 100);
			par3.Direction = ParameterDirection.Input;
			par3.Value = obeSolicitante.Nombres;

			SqlParameter par4 = cmd.Parameters.Add("@P_SOLI_STIPO_DOCUMENTO_IDENTIDAD_ID", SqlDbType.SmallInt);
			par4.Direction = ParameterDirection.Input;
			par4.Value = obeSolicitante.TipoDocumentoIdentidadId;

			SqlParameter par5 = cmd.Parameters.Add("@P_SOLI_VNUMERO_DOCUMENTO_IDENTIDAD", SqlDbType.VarChar, 20);
			par5.Direction = ParameterDirection.Input;
			par5.Value = obeSolicitante.NumeroDocumentoIdentidad;

            if (obeSolicitante.Telefono == null & obeSolicitante.Telefono.Equals("")) { SqlParameter par6 = cmd.Parameters.Add("@P_SOLI_VTELEFONO", DBNull.Value); }
            else
            {
                SqlParameter par6 = cmd.Parameters.Add("@P_SOLI_VTELEFONO", SqlDbType.VarChar, 20);
                par6.Direction = ParameterDirection.Input;
                par6.Value = obeSolicitante.Telefono;
            }

            SqlParameter par8 = cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.SmallInt);
			par8.Direction = ParameterDirection.Input;
			par8.Value = obeSolicitante.Usuariocreacion;

            SqlParameter par9 = cmd.Parameters.Add("@P_IP_CREACION", SqlDbType.VarChar, 50);
			par9.Direction = ParameterDirection.Input;
			par9.Value = obeSolicitante.Ipcreacion;

			SqlParameter par14 = cmd.Parameters.Add("@@identity", SqlDbType.SmallInt);
			par14.Direction = ParameterDirection.ReturnValue;

			int n = cmd.ExecuteNonQuery();
			if (n > 0) n = idSolicitante = Convert.ToInt16(par14.Value);
            return (idSolicitante);
		}

        public bool actualizar(SqlConnection con, SqlTransaction trx, beSolicitante obeSolicitante)
        {
            bool exito = false;

            SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_SOLICITANTE_ACTUALIZAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par0 = cmd.Parameters.Add("@P_SOLI_SSOLICITANTE_ID", SqlDbType.SmallInt);
            par0.Direction = ParameterDirection.Input;
            par0.Value = obeSolicitante.SolicitanteId;

            SqlParameter par1 = cmd.Parameters.Add("@P_SOLI_VPRIMER_APELLIDO", SqlDbType.VarChar, 100);
            par1.Direction = ParameterDirection.Input;
            par1.Value = obeSolicitante.PrimerApellido;

            SqlParameter par2 = cmd.Parameters.Add("@P_SOLI_VSEGUNDO_APELLIDO", SqlDbType.VarChar, 100);
            par2.Direction = ParameterDirection.Input;
            par2.Value = obeSolicitante.SegundoApellido;

            SqlParameter par4 = cmd.Parameters.Add("@P_SOLI_STIPO_DOCUMENTO_IDENTIDAD_ID", SqlDbType.SmallInt);
            par4.Direction = ParameterDirection.Input;
            par4.Value = obeSolicitante.TipoDocumentoIdentidadId;

            SqlParameter par5 = cmd.Parameters.Add("@P_SOLI_VNUMERO_DOCUMENTO_IDENTIDAD", SqlDbType.VarChar, 20);
            par5.Direction = ParameterDirection.Input;
            par5.Value = obeSolicitante.NumeroDocumentoIdentidad;

            SqlParameter par3 = cmd.Parameters.Add("@P_SOLI_VNOMBRES", SqlDbType.VarChar, 100);
            par3.Direction = ParameterDirection.Input;
            par3.Value = obeSolicitante.Nombres;

            SqlParameter par6 = cmd.Parameters.Add("@P_SOLI_VTELEFONO", SqlDbType.VarChar, 20);
            par6.Direction = ParameterDirection.Input;
            par6.Value = obeSolicitante.Telefono;

            SqlParameter par11 = cmd.Parameters.Add("@P_SOLI_SUSUARIOMODIFICACION", SqlDbType.SmallInt);
            par11.Direction = ParameterDirection.Input;
            par11.Value = obeSolicitante.Usuariomodificacion;

            SqlParameter par12 = cmd.Parameters.Add("@P_SOLI_VIPMODIFICACION", SqlDbType.VarChar, 50);
            par12.Direction = ParameterDirection.Input;
            par12.Value = obeSolicitante.Ipmodificacion;

            int n = cmd.ExecuteNonQuery();
            exito = (n > 0);
            return (exito);
        }

        public beSolicitante consultarxIdentificacion(SqlConnection con, beSolicitante parametros)
        {
            beSolicitante obeSolicitante = new beSolicitante(); ;
            SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_SOLICITANTE_CONSULTAR_x_IDENTIFICACION", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_SOLI_STIPO_DOCUMENTO_IDENTIDAD_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.TipoDocumentoIdentidadId;

            SqlParameter par2 = cmd.Parameters.Add("@P_SOLI_VNUMERO_DOCUMENTO_IDENTIDAD", SqlDbType.VarChar, 20);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.NumeroDocumentoIdentidad;

            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                int posSOLI_ID = drd.GetOrdinal("SOLI_ID");
                int posSOLI_PRIAPE = drd.GetOrdinal("SOLI_PRIAPE");
                int posSOLI_SEGAPE = drd.GetOrdinal("SOLI_SEGAPE");
                int posSOLI_NOMBRES = drd.GetOrdinal("SOLI_NOMBRES");
                int posSOLI_TIPODOCIDENT = drd.GetOrdinal("SOLI_TIPODOCIDENT");
                int posSOLI_NUMERODOCIDENT = drd.GetOrdinal("SOLI_NUMERODOCIDENT");
                int posSOLI_TELEFONO = drd.GetOrdinal("SOLI_TELEFONO");
                if (drd.HasRows)
                {
                    drd.Read();
                    obeSolicitante.SolicitanteId = drd.GetInt16(posSOLI_ID);
                    obeSolicitante.PrimerApellido = drd.GetString(posSOLI_PRIAPE);
                    obeSolicitante.SegundoApellido = drd.GetString(posSOLI_SEGAPE);
                    obeSolicitante.Nombres = drd.GetString(posSOLI_NOMBRES);
                    obeSolicitante.TipoDocumentoIdentidadId = drd.GetInt16(posSOLI_TIPODOCIDENT);
                    obeSolicitante.NumeroDocumentoIdentidad = drd.GetString(posSOLI_NUMERODOCIDENT);
                    obeSolicitante.Telefono = drd.GetString(posSOLI_TELEFONO);
                }
                else
                {
                    obeSolicitante = null;
                }
                drd.Close();
            }
            return (obeSolicitante);
        }

        public List<beSolicitante> consultar(SqlConnection con, beSolicitante parametros)
        {
            List<beSolicitante> lbeSolicitante = new List<beSolicitante>(); ;
            SqlCommand cmd = new SqlCommand("PN_REGISTRO.USP_RE_SOLICITANTE_CONSULTAR", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_PRIMER_APELLIDO", SqlDbType.VarChar, 100);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.PrimerApellido;

            SqlParameter par2 = cmd.Parameters.Add("@P_SEGUNDO_APELLIDO", SqlDbType.VarChar, 100);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.SegundoApellido;

            SqlParameter par3 = cmd.Parameters.Add("@P_NOMBRES", SqlDbType.VarChar, 100);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros.Nombres;

            SqlParameter par4 = cmd.Parameters.Add("@P_TIPO_DOC_IDENT", SqlDbType.SmallInt);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametros.TipoDocumentoIdentidadId;

            SqlParameter par5 = cmd.Parameters.Add("@P_NUM_DOC_IDENT", SqlDbType.VarChar, 20);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametros.NumeroDocumentoIdentidad;

            SqlParameter par6 = cmd.Parameters.Add("@P_TELEFONO", SqlDbType.VarChar, 20);
            par6.Direction = ParameterDirection.Input;
            par6.Value = parametros.Telefono;

            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                int posSOLI_ID = drd.GetOrdinal("SOLI_ID");
                int posSOLI_NOMBRECOMPLETO = drd.GetOrdinal("SOLI_NOMBRECOMPLETO");
                int posSOLI_PRIAPE = drd.GetOrdinal("SOLI_PRIAPE");
                int posSOLI_SEGAPE = drd.GetOrdinal("SOLI_SEGAPE");
                int posSOLI_NOMBRES = drd.GetOrdinal("SOLI_NOMBRES");
                int posDOCIDENT_ID = drd.GetOrdinal("DOCIDENT_ID");
                int posDOCIDENT_DESC = drd.GetOrdinal("DOCIDENT_DESC");
                int posSOLI_NUMDOCIDENT = drd.GetOrdinal("SOLI_NUMDOCIDENT");
                int posSOLI_TELEFONO = drd.GetOrdinal("SOLI_TELEFONO");
                beSolicitante obeSolicitante;
                while (drd.Read())
                {
                    obeSolicitante = new beSolicitante();
                    obeSolicitante.SolicitanteId = drd.GetInt16(posSOLI_ID);
                    obeSolicitante.ConNombreCompleto = drd.GetString(posSOLI_NOMBRECOMPLETO);
                    obeSolicitante.PrimerApellido = drd.GetString(posSOLI_PRIAPE);
                    obeSolicitante.SegundoApellido = drd.GetString(posSOLI_SEGAPE);
                    obeSolicitante.Nombres = drd.GetString(posSOLI_NOMBRES);
                    obeSolicitante.TipoDocumentoIdentidadId = drd.GetInt16(posDOCIDENT_ID);
                    obeSolicitante.ConTipoDocIdent = drd.GetString(posDOCIDENT_DESC);
                    obeSolicitante.NumeroDocumentoIdentidad = drd.GetString(posSOLI_NUMDOCIDENT);
                    obeSolicitante.Telefono = drd.GetString(posSOLI_TELEFONO);
                    lbeSolicitante.Add(obeSolicitante);
                }
                drd.Close();
            }
            return (lbeSolicitante);
        }
    }
}
