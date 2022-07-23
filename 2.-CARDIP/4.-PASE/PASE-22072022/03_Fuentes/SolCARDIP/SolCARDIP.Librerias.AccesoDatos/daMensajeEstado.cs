using SolCARDIP.Librerias.EntidadesNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daMensajeEstado
    {
        public short adicionar(SqlConnection con, SqlTransaction trx, beMensajeEstado parametros,string operacion)
        {
            short idOficinaconsularExtranjera = -1;

            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_MANTENER_MENSAJE_ESTADO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@MEES_SESTADOID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.Estadoid;

            SqlParameter par2 = cmd.Parameters.Add("@MEES_VMENSAJE", SqlDbType.VarChar, 250);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.Mensaje;

            SqlParameter par3 = cmd.Parameters.Add("@OPERACION", SqlDbType.VarChar, 10);
            par3.Direction = ParameterDirection.Input;
            par3.Value = operacion.ToUpper();

            int n = cmd.ExecuteNonQuery();
            if (n > 0) n = idOficinaconsularExtranjera = 1;
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
            return (lbeOficinaconsularExtranjera);
        }
    }
}
