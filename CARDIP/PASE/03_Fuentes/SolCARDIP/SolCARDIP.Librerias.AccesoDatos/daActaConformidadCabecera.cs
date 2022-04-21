using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daActaConformidadCabecera
    {
        public short adicionar(SqlConnection con, SqlTransaction trx, beActaConformidadCabecera parametros)
		{
            short idActaConformidadCabecera = -1;


            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_ACTA_CONFORMIDAD_CABECERA_ADICIONAR", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Transaction = trx;


            SqlParameter par1 = cmd.Parameters.Add("@P_SOLICITANTE_ID", SqlDbType.SmallInt);
			par1.Direction = ParameterDirection.Input;
			par1.Value = parametros.SolicitanteId;

            if (parametros.Observacion == null) { SqlParameter par2 = cmd.Parameters.Add("@P_OBSERVACION", DBNull.Value); }
            else
            {
                SqlParameter par2 = cmd.Parameters.Add("@P_OBSERVACION", SqlDbType.VarChar, 250);
                par2.Direction = ParameterDirection.Input;
                par2.Value = parametros.Observacion;
            }

            SqlParameter par4 = cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.SmallInt);
			par4.Direction = ParameterDirection.Input;
			par4.Value = parametros.UsuarioCreacion;


            SqlParameter par5 = cmd.Parameters.Add("@P_IP_CREACION", SqlDbType.VarChar, 50);
			par5.Direction = ParameterDirection.Input;
			par5.Value = parametros.IpCreacion;

			SqlParameter par10 = cmd.Parameters.Add("@@identity", SqlDbType.Int);
			par10.Direction = ParameterDirection.ReturnValue;

			int n = cmd.ExecuteNonQuery();
            if (n > 0) idActaConformidadCabecera = Convert.ToInt16(par10.Value);
            return (idActaConformidadCabecera);
		}

        public beActaConformidadPrincipal consultar(SqlConnection con, beActaConformidadCabecera parametros)
        {
            beActaConformidadPrincipal obeActaConformidadPrincipal = new beActaConformidadPrincipal();
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_ACTA_CONFORMIDAD_CONSULTAR", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_ACTA_CAB_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.ActaConformidadCabeceraId;

            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                drd.Read();
                beActaConformidadCabecera obeActaConformidadCabecera;
                int posOBSERVACION = drd.GetOrdinal("OBSERVACION");
                int posSOLICITANTE = drd.GetOrdinal("SOLICITANTE");
                int posTIPO_DOC_IDENT = drd.GetOrdinal("TIPO_DOC_IDENT");
                int posNUM_DOC_IDENT = drd.GetOrdinal("NUM_DOC_IDENT");
                if (drd.HasRows)
                {
                    obeActaConformidadCabecera = new beActaConformidadCabecera();
                    obeActaConformidadCabecera.Observacion = drd.GetString(posOBSERVACION);
                    obeActaConformidadCabecera.ConSolicitante = drd.GetString(posSOLICITANTE);
                    obeActaConformidadCabecera.ConTipoDocIdent = drd.GetString(posTIPO_DOC_IDENT);
                    obeActaConformidadCabecera.ConNumeroDocIdent = drd.GetString(posNUM_DOC_IDENT);
                    obeActaConformidadPrincipal.ActaCabecera = obeActaConformidadCabecera;
                    if (drd.NextResult())
                    {
                        List<beActaConformidadDetalle> listaDetalle = new List<beActaConformidadDetalle>();
                        int posTITULAR_CARNE = drd.GetOrdinal("TITULAR_CARNE");
                        int posCARNE_NUMERO = drd.GetOrdinal("CARNE_NUMERO");
                        int posOFCOEX_ID = drd.GetOrdinal("OFCOEX_ID");
                        beActaConformidadDetalle obeActaConformidadDetalle;
                        while (drd.Read())
                        {
                            obeActaConformidadDetalle = new beActaConformidadDetalle();
                            obeActaConformidadDetalle.ConTitular = drd.GetString(posTITULAR_CARNE);
                            obeActaConformidadDetalle.ConNumeroCarne = drd.GetString(posCARNE_NUMERO);
                            obeActaConformidadDetalle.ConOficinaId = drd.GetInt16(posOFCOEX_ID);
                            listaDetalle.Add(obeActaConformidadDetalle);
                        }
                        obeActaConformidadPrincipal.ActaDetalle = listaDetalle;
                        if (drd.NextResult())
                        {
                            List<beOficinaconsularExtranjera> listaInstitucion = new List<beOficinaconsularExtranjera>();
                            int posOFCO_NOMBRE = drd.GetOrdinal("OFCO_NOMBRE");
                            int posOFCO_ID = drd.GetOrdinal("OFCO_ID");
                            beOficinaconsularExtranjera obeOficinaconsularExtranjera;
                            while (drd.Read())
                            {
                                obeOficinaconsularExtranjera = new beOficinaconsularExtranjera();
                                obeOficinaconsularExtranjera.Nombre = drd.GetString(posOFCO_NOMBRE);
                                obeOficinaconsularExtranjera.OficinaconsularExtranjeraid = drd.GetInt16(posOFCO_ID);
                                listaInstitucion.Add(obeOficinaconsularExtranjera);
                            }
                            obeActaConformidadPrincipal.Instituciones = listaInstitucion;
                        }
                    }
                }
                drd.Close();
            }
            return (obeActaConformidadPrincipal);
        }
    }
}
