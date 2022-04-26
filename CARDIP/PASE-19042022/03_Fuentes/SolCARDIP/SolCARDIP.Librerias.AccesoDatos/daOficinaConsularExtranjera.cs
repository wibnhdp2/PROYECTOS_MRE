using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daOficinaConsularExtranjera
    {
        public short adicionar(SqlConnection con, SqlTransaction trx, beOficinaconsularExtranjera parametros)
		{
            short idOficinaconsularExtranjera = -1;

            SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINA_CONSULAR_EX_ADICIONAR", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Transaction = trx;

			SqlParameter par1 = cmd.Parameters.Add("@P_OFCE_SCATEGORIAID", SqlDbType.SmallInt);
			par1.Direction = ParameterDirection.Input;
			par1.Value = parametros.Categoriaid;

			SqlParameter par2 = cmd.Parameters.Add("@P_OFCE_VSIGLAS", SqlDbType.VarChar, 50);
			par2.Direction = ParameterDirection.Input;
			par2.Value = parametros.Siglas;

			SqlParameter par3 = cmd.Parameters.Add("@P_OFCE_VNOMBRE", SqlDbType.VarChar, 250);
			par3.Direction = ParameterDirection.Input;
			par3.Value = parametros.Nombre;

            SqlParameter par10 = cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.SmallInt);
			par10.Direction = ParameterDirection.Input;
			par10.Value = parametros.Usuariocreacion;

            SqlParameter par11 = cmd.Parameters.Add("@P_IP_CREACION", SqlDbType.VarChar, 50);
			par11.Direction = ParameterDirection.Input;
			par11.Value = parametros.Ipcreacion;

			SqlParameter par17 = cmd.Parameters.Add("@@identity", SqlDbType.Int);
			par17.Direction = ParameterDirection.ReturnValue;

			int n = cmd.ExecuteNonQuery();
			if (n > 0) n = idOficinaconsularExtranjera = Convert.ToInt16(par17.Value);
            return (idOficinaconsularExtranjera);
		}

        public bool actualizar(SqlConnection con, SqlTransaction trx, beOficinaconsularExtranjera parametros)
        {
            bool exito = false;

            SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINA_CONSULAR_EX_ACTUALIZAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par0 = cmd.Parameters.Add("@P_OFCE_SOFICINACONSULAR_EXTRANJERAID", SqlDbType.SmallInt);
            par0.Direction = ParameterDirection.Input;
            par0.Value = parametros.OficinaconsularExtranjeraid;

            SqlParameter par1 = cmd.Parameters.Add("@P_OFCE_SCATEGORIAID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.Categoriaid;

            SqlParameter par2 = cmd.Parameters.Add("@P_OFCE_VSIGLAS", SqlDbType.VarChar, 50);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.Siglas;

            SqlParameter par3 = cmd.Parameters.Add("@P_OFCE_VNOMBRE", SqlDbType.VarChar, 250);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros.Nombre;

            SqlParameter par13 = cmd.Parameters.Add("@P_USUARIO_MODIFICACION", SqlDbType.SmallInt);
            par13.Direction = ParameterDirection.Input;
            par13.Value = parametros.Usuariomodificacion;

            SqlParameter par14 = cmd.Parameters.Add("@P_IP_MODIFICACION", SqlDbType.VarChar, 50);
            par14.Direction = ParameterDirection.Input;
            par14.Value = parametros.Ipmodificacion;

            int n = cmd.ExecuteNonQuery();
            exito = (n > 0);
            return (exito);
        }

        public List<beOficinaconsularExtranjera> consultar(SqlConnection con, beOficinaconsularExtranjera parametros)
        {
            List<beOficinaconsularExtranjera> lbeOficinaconsularExtranjera = new List<beOficinaconsularExtranjera>();

            SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_OFICINA_CONSULAR_EX_CONSULTAR", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_OFCE_SCATEGORIAID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.Categoriaid;

            SqlParameter par2 = cmd.Parameters.Add("@P_OFCE_VNOMBRE", SqlDbType.VarChar, 250);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.Nombre;

            SqlParameter par3 = cmd.Parameters.Add("@P_OFCE_VSIGLAS", SqlDbType.VarChar, 50);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros.Siglas;

            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                int posOFCOEX_ID = drd.GetOrdinal("OFCOEX_ID");
                int posCATEGORIA_ID = drd.GetOrdinal("CATEGORIA_ID");
                int posCATEGORIA_DESC = drd.GetOrdinal("CATEGORIA_DESC");
                int posOFCOEX_NOMBRE = drd.GetOrdinal("OFCOEX_NOMBRE");
                int posOFCOEX_SIGLAS = drd.GetOrdinal("OFCOEX_SIGLAS");
                beOficinaconsularExtranjera obeOficinaconsularExtranjera;
                while (drd.Read())
                {
                    obeOficinaconsularExtranjera = new beOficinaconsularExtranjera();
                    obeOficinaconsularExtranjera.OficinaconsularExtranjeraid = drd.GetInt16(posOFCOEX_ID);
                    obeOficinaconsularExtranjera.Categoriaid = drd.GetInt16(posCATEGORIA_ID);
                    obeOficinaconsularExtranjera.ConCategoria = drd.GetString(posCATEGORIA_DESC);
                    obeOficinaconsularExtranjera.Siglas = drd.GetString(posOFCOEX_SIGLAS);
                    obeOficinaconsularExtranjera.Nombre = drd.GetString(posOFCOEX_NOMBRE);
                    lbeOficinaconsularExtranjera.Add(obeOficinaconsularExtranjera);
                }
                drd.Close();
            }
            return(lbeOficinaconsularExtranjera);
        }

        public short validar(SqlConnection con, SqlTransaction trx, beOficinaconsularExtranjera parametros)
        {
            short idOficinaconsularExtranjera = 0;
            SqlCommand cmd = new SqlCommand("PS_SISTEMA.USP_SI_VALIDAR_OFICINA_CONSULAR_EXTRANJERA", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_OFCE_SCATEGORIAID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.Categoriaid;

            SqlParameter par2 = cmd.Parameters.Add("@P_OFCE_VNOMBRE", SqlDbType.VarChar, 250);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.Nombre;

            SqlParameter par3 = cmd.Parameters.Add("@return_value", SqlDbType.SmallInt);
            par3.Direction = ParameterDirection.ReturnValue;

            cmd.ExecuteNonQuery();
            idOficinaconsularExtranjera = Convert.ToInt16(par3.Value);
            return (idOficinaconsularExtranjera);
        }
    }
}
