using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daActaRecepcionCabecera
    {
        public short adicionar(SqlConnection con, SqlTransaction trx, beActaRecepcionCabecera parametros)
        {
            short idActaRecepcionCabecera = -1;


            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_ACTA_RECEPCION_CABECERA_ADICIONAR", con);
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
            if (n > 0) idActaRecepcionCabecera = Convert.ToInt16(par10.Value);
            return (idActaRecepcionCabecera);
        }

        public beActaRecepcionPrincipal consultar(SqlConnection con, beActaRecepcionCabecera parametros)
        {
            beActaRecepcionPrincipal obeActaRecepcionPrincipal = new beActaRecepcionPrincipal();
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_ACTA_RECEPCION_CONSULTAR", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_ACTA_REC_CAB_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.ActaRecepcionId;

            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                drd.Read();
                beActaRecepcionCabecera obeActaRecepcionCabecera;
                int posOBSERVACION = drd.GetOrdinal("OBSERVACION");
                int posSOLICITANTE = drd.GetOrdinal("SOLICITANTE");
                int posTIPO_DOC_IDENT = drd.GetOrdinal("TIPO_DOC_IDENT");
                int posNUM_DOC_IDENT = drd.GetOrdinal("NUM_DOC_IDENT");
                if (drd.HasRows)
                {
                    obeActaRecepcionCabecera = new beActaRecepcionCabecera();
                    obeActaRecepcionCabecera.Observacion = drd.GetString(posOBSERVACION);
                    obeActaRecepcionCabecera.ConSolicitante = drd.GetString(posSOLICITANTE);
                    obeActaRecepcionCabecera.ConTipoDocIdent = drd.GetString(posTIPO_DOC_IDENT);
                    obeActaRecepcionCabecera.ConNumeroDocIdent = drd.GetString(posNUM_DOC_IDENT);
                    obeActaRecepcionPrincipal.ActaCabecera = obeActaRecepcionCabecera;
                    if (drd.NextResult())
                    {
                        List<beActaRecepcionDetalle> listaDetalle = new List<beActaRecepcionDetalle>();
                        int posTITULAR_CARNE = drd.GetOrdinal("TITULAR_CARNE");
                        int posNUMERO_IDENT = drd.GetOrdinal("NUMERO_IDENT");
                        beActaRecepcionDetalle obeActaRecepcionDetalle;
                        while (drd.Read())
                        {
                            obeActaRecepcionDetalle = new beActaRecepcionDetalle();
                            obeActaRecepcionDetalle.ConTitular = drd.GetString(posTITULAR_CARNE);
                            obeActaRecepcionDetalle.ConNumeroIdent = drd.GetString(posNUMERO_IDENT);
                            listaDetalle.Add(obeActaRecepcionDetalle);
                        }
                        obeActaRecepcionPrincipal.ActaDetalle = listaDetalle;
                    }
                }
                drd.Close();
            }
            return (obeActaRecepcionPrincipal);
        }

        public beActaRecepcionCabecera obtenerCabecera(SqlConnection con, beActaRecepcionDetalle parametros)
        {
            beActaRecepcionCabecera obeActaRecepcionCabecera = new beActaRecepcionCabecera();
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_ACTA_RECEPCION_DETALLE_OBTENER_ID_CAB", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_CARNE_IDENTIDAD_ID", SqlDbType.Int);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.CarneIdentidadId;

            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                drd.Read();
                int posCAB = drd.GetOrdinal("CAB");
                if (drd.HasRows)
                {
                    obeActaRecepcionCabecera.ActaRecepcionId = drd.GetInt16(posCAB);
                }
                else
                {
                    obeActaRecepcionCabecera = null;
                }
                drd.Close();
            }
            return (obeActaRecepcionCabecera);
        }
    }
}
