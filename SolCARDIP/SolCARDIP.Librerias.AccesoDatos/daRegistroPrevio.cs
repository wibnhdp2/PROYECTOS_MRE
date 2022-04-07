using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daRegistroPrevio
    {
        public short adicionar(SqlConnection con, SqlTransaction trx, beRegistroPrevio obeRegistroPrevio)
		{
			short idRegistroPrevio = -1;
            string Fecha = "01/01/0001";
            DateTime FechaNull = DateTime.Parse(Fecha);

            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_REGISTRO_PREVIO_ADICIONAR", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_PRIMER_APELLIDO", SqlDbType.VarChar, 100);
			par1.Direction = ParameterDirection.Input;
			par1.Value = obeRegistroPrevio.PrimerApellido;

            if (obeRegistroPrevio.SegundoApellido == null | obeRegistroPrevio.SegundoApellido == "") { SqlParameter par2 = cmd.Parameters.Add("@P_SEGUNDO_APELLIDO", DBNull.Value); }
            else
            {
                SqlParameter par2 = cmd.Parameters.Add("@P_SEGUNDO_APELLIDO", SqlDbType.VarChar, 100);
                par2.Direction = ParameterDirection.Input;
                par2.Value = obeRegistroPrevio.SegundoApellido;
            }

            SqlParameter par11 = cmd.Parameters.Add("@P_NOMBRES", SqlDbType.VarChar, 100);
            par11.Direction = ParameterDirection.Input;
            par11.Value = obeRegistroPrevio.Nombres;

            if (obeRegistroPrevio.GeneroId == 0) { SqlParameter par3 = cmd.Parameters.Add("@P_GENERO", DBNull.Value); }
            else
            {
                SqlParameter par3 = cmd.Parameters.Add("@P_GENERO", SqlDbType.SmallInt);
                par3.Direction = ParameterDirection.Input;
                par3.Value = obeRegistroPrevio.GeneroId;
            }

            SqlParameter par4 = cmd.Parameters.Add("@P_OFICINA_CONSULAR_EX", SqlDbType.SmallInt);
			par4.Direction = ParameterDirection.Input;
			par4.Value = obeRegistroPrevio.OficinaConsularExId;

            if (obeRegistroPrevio.CalidadMigratoria == 0) { SqlParameter par5 = cmd.Parameters.Add("@P_CALIDAD_MIGRATORIA", DBNull.Value); }
            else
            {
                SqlParameter par5 = cmd.Parameters.Add("@P_CALIDAD_MIGRATORIA", SqlDbType.SmallInt);
                par5.Direction = ParameterDirection.Input;
                par5.Value = obeRegistroPrevio.CalidadMigratoria;
            }

            SqlParameter par6 = cmd.Parameters.Add("@P_PERIODO", SqlDbType.Int);
			par6.Direction = ParameterDirection.Input;
			par6.Value = obeRegistroPrevio.Periodo;

            SqlParameter par7 = cmd.Parameters.Add("@P_ESTADO_ID", SqlDbType.SmallInt);
            par7.Direction = ParameterDirection.Input;
            par7.Value = obeRegistroPrevio.EstadoId;

            SqlParameter par13 = cmd.Parameters.Add("@P_FECHA_CON", SqlDbType.DateTime);
            par13.Direction = ParameterDirection.Input;
            par13.Value = obeRegistroPrevio.FechaConsulares;

            if (obeRegistroPrevio.FechaPrivilegios == FechaNull) { SqlParameter par14 = cmd.Parameters.Add("@P_FECHA_PRI", DBNull.Value); }
            else
            {
                SqlParameter par14 = cmd.Parameters.Add("@P_FECHA_PRI", SqlDbType.DateTime);
                par14.Direction = ParameterDirection.Input;
                par14.Value = obeRegistroPrevio.FechaPrivilegios;
            }

            SqlParameter par8 = cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.SmallInt);
			par8.Direction = ParameterDirection.Input;
			par8.Value = obeRegistroPrevio.UsuarioCreacion;

            SqlParameter par9 = cmd.Parameters.Add("@P_IP_CREACION", SqlDbType.VarChar, 50);
			par9.Direction = ParameterDirection.Input;
			par9.Value = obeRegistroPrevio.IpCreacion;

            SqlParameter par12 = cmd.Parameters.Add("@P_TIPO_ENTRADA", SqlDbType.SmallInt);
            par12.Direction = ParameterDirection.Input;
            par12.Value = obeRegistroPrevio.TipoEntrada;

            SqlParameter par10 = cmd.Parameters.Add("@@IDENTITY", SqlDbType.SmallInt);
            par10.Direction = ParameterDirection.ReturnValue;

			int n = cmd.ExecuteNonQuery();
            if (n > 0) n = idRegistroPrevio = Convert.ToInt16(par10.Value);
            return (idRegistroPrevio);
		}

        public beRegistroPrevioListas consultar(SqlConnection con, beRegistroPrevio parametros)
        {
            beRegistroPrevioListas obeRegistroPrevioListas = new beRegistroPrevioListas();
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_REGISTRO_PREVIO_CONSULTAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            #region Parametros
            SqlParameter par1 = cmd.Parameters.Add("@P_PERIODO", SqlDbType.Int);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.Periodo;

            SqlParameter par2 = cmd.Parameters.Add("@P_NUMERO_IDENT", SqlDbType.Int);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.Ident;

            SqlParameter par10 = cmd.Parameters.Add("@P_NUMERO_CARNE", SqlDbType.VarChar, 8);
            par10.Direction = ParameterDirection.Input;
            par10.Value = parametros.CarneNumero;

            SqlParameter par3 = cmd.Parameters.Add("@P_APELLIDO_PAT", SqlDbType.VarChar, 100);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros.PrimerApellido;

            SqlParameter par4 = cmd.Parameters.Add("@P_APELLIDO_MAT", SqlDbType.VarChar, 100);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametros.SegundoApellido;

            SqlParameter par5 = cmd.Parameters.Add("@P_NOMBRES", SqlDbType.VarChar, 100);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametros.Nombres;

            SqlParameter par6 = cmd.Parameters.Add("@P_FECHA_REGISTRO_DESDE", SqlDbType.DateTime);
            par6.Direction = ParameterDirection.Input;
            par6.Value = parametros.FechaRegistroDesde;

            SqlParameter par7 = cmd.Parameters.Add("@P_FECHA_REGISTRO_HASTA", SqlDbType.DateTime);
            par7.Direction = ParameterDirection.Input;
            par7.Value = parametros.FechaRegistroHasta;

            SqlParameter par11 = cmd.Parameters.Add("@P_TIPO_ENTRADA", SqlDbType.SmallInt);
            par11.Direction = ParameterDirection.Input;
            par11.Value = parametros.TipoEntrada;

            SqlParameter par12 = cmd.Parameters.Add("@P_CALIDAD_MIG", SqlDbType.SmallInt);
            par12.Direction = ParameterDirection.Input;
            par12.Value = parametros.CalidadMigratoria;

            SqlParameter par13 = cmd.Parameters.Add("@P_OFICINA_CON_EX", SqlDbType.SmallInt);
            par13.Direction = ParameterDirection.Input;
            par13.Value = parametros.OficinaConsularExId;

            SqlParameter par14 = cmd.Parameters.Add("@P_ESTADO", SqlDbType.SmallInt);
            par14.Direction = ParameterDirection.Input;
            par14.Value = parametros.EstadoId;

            SqlParameter par8 = cmd.Parameters.Add("@P_NUM_PAG", SqlDbType.BigInt);
            par8.Direction = ParameterDirection.Input;
            par8.Value = parametros.NumPag;

            SqlParameter par9 = cmd.Parameters.Add("@P_CANT_REG_PAG", SqlDbType.BigInt);
            par9.Direction = ParameterDirection.Input;
            par9.Value = parametros.CantReg;
            #endregion
            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                List<beRegistroPrevio> lista = new List<beRegistroPrevio>();
                int posIDENT = drd.GetOrdinal("IDENT");
                int posTIPO_ENTRADA = drd.GetOrdinal("TIPO_ENTRADA");
                int posCARNE_NUMERO = drd.GetOrdinal("CARNE_NUMERO");
                int posESTADO = drd.GetOrdinal("ESTADO");
                int posNOMBRE_COMPLETO = drd.GetOrdinal("NOMBRE_COMPLETO");
                //int posGENERO = drd.GetOrdinal("GENERO");
                int posOFCO_NOMBRE = drd.GetOrdinal("OFCO_NOMBRE");
                int posCALIDAD_MIGRATORIA = drd.GetOrdinal("CALIDAD_MIGRATORIA");
                int posFECHA_INSCRIPCION = drd.GetOrdinal("FECHA_INSCRIPCION");
                int posFECHA_EMISION = drd.GetOrdinal("FECHA_EMISION");
                int posFECHA_VENCIMIENTO = drd.GetOrdinal("FECHA_VENCIMIENTO");
                int posFECHA_CON = drd.GetOrdinal("FECHA_CON");
                int posFECHA_PRI = drd.GetOrdinal("FECHA_PRI");
                int posCARDIP_ID = drd.GetOrdinal("CARDIP_ID");
                int posREGPREV_ID = drd.GetOrdinal("REGPREV_ID");
                int posFLAG_REGCOMP = drd.GetOrdinal("FLAG_REGCOMP");
                int posREGISTRADOR_ID = drd.GetOrdinal("REGISTRADOR_ID");
                int posFLAG_ENTREGADO = drd.GetOrdinal("FLAG_ENTREGADO");
                int posACTACONCAB_ID = drd.GetOrdinal("ACTACONCAB_ID");
                int posACTARECCAB_ID = drd.GetOrdinal("ACTARECCAB_ID");
                beRegistroPrevio obeRegistroPrevio;
                while (drd.Read())
                {
                    obeRegistroPrevio = new beRegistroPrevio();
                    obeRegistroPrevio.ConIdent = drd.GetString(posIDENT);
                    obeRegistroPrevio.ConTipoEntrada = drd.GetString(posTIPO_ENTRADA);
                    obeRegistroPrevio.ConCarneNumero = drd.GetString(posCARNE_NUMERO);
                    obeRegistroPrevio.ConEstadoDesc = drd.GetString(posESTADO);
                    obeRegistroPrevio.ConNombreCompleto = drd.GetString(posNOMBRE_COMPLETO);
                    //obeRegistroPrevio.ConGenero = drd.GetString(posGENERO);
                    obeRegistroPrevio.ConOficinaConsular = drd.GetString(posOFCO_NOMBRE);
                    obeRegistroPrevio.ConCalidadMigratoriaDesc = drd.GetString(posCALIDAD_MIGRATORIA);
                    obeRegistroPrevio.ConFechaRegistro = drd.GetString(posFECHA_INSCRIPCION);
                    obeRegistroPrevio.ConFechaEmision = drd.GetString(posFECHA_EMISION);
                    obeRegistroPrevio.ConFechaVencimiento = drd.GetString(posFECHA_VENCIMIENTO);
                    obeRegistroPrevio.ConFechaConsulares = drd.GetString(posFECHA_CON);
                    obeRegistroPrevio.ConFechaPrivilegios = drd.GetString(posFECHA_PRI);
                    obeRegistroPrevio.ConCarneIdentidadId = drd.GetInt16(posCARDIP_ID);
                    obeRegistroPrevio.RegistroPrevioId = drd.GetInt16(posREGPREV_ID);
                    obeRegistroPrevio.FlagRegistroCompleto = drd.GetBoolean(posFLAG_REGCOMP);
                    obeRegistroPrevio.ConRegistradorId = drd.GetInt16(posREGISTRADOR_ID);
                    obeRegistroPrevio.ConFlagEntregado = drd.GetBoolean(posFLAG_ENTREGADO);
                    obeRegistroPrevio.ConActaConformidad = drd.GetInt16(posACTACONCAB_ID);
                    obeRegistroPrevio.ConActaRecepcion = drd.GetInt16(posACTARECCAB_ID);
                    lista.Add(obeRegistroPrevio);
                }
                obeRegistroPrevioListas.listaRegistros = lista;
                if (drd.NextResult())
                {
                    int posTotal = drd.GetOrdinal("TOTALPAG");
                    int posResiduo = drd.GetOrdinal("RESIDUO");
                    int posTotalRegistros = drd.GetOrdinal("TOTALREGISTROS");
                    bePaginacion obePaginacion;
                    if (drd.HasRows)
                    {
                        drd.Read();
                        obePaginacion = new bePaginacion();
                        obePaginacion.Total = drd.GetInt64(posTotal);
                        obePaginacion.Residuo = drd.GetInt64(posResiduo);
                        obePaginacion.TotalRegistros = drd.GetInt64(posTotalRegistros);
                        obeRegistroPrevioListas.Paginacion = obePaginacion;
                    }
                }
                drd.Close();
            }
            return (obeRegistroPrevioListas);
        }

        public beRegistroPrevio consultarRegistroEdicion(SqlConnection con, beRegistroPrevio parametros)
        {
            beRegistroPrevio obeRegistroPrevio = new beRegistroPrevio();
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_REGISTRO_PREVIO_CONSULTAR_REGISTRO_EDICION", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_REGISTRO_PREVIO_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.RegistroPrevioId;

            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                int posREGPREV_ID = drd.GetOrdinal("REGPREV_ID");
                int posPRIMER_APELLIDO = drd.GetOrdinal("PRIMER_APELLIDO");
                int posSEGUNDO_APELLIDO = drd.GetOrdinal("SEGUNDO_APELLIDO");
                int posNOMBRES = drd.GetOrdinal("NOMBRES");
                int posGENERO = drd.GetOrdinal("GENERO");
                int posOFOCEX_ID = drd.GetOrdinal("OFOCEX_ID");
                int posCATEGORIA_OFCOEX = drd.GetOrdinal("CATEGORIA_OFCOEX");
                int posCALMIG_ID = drd.GetOrdinal("CALMIG_ID");
                int posCARDIP_ID = drd.GetOrdinal("CARDIP_ID");
                int posTIPO_ENTRADA = drd.GetOrdinal("TIPO_ENTRADA");
                int posFECHACON = drd.GetOrdinal("FECHACON");
                int posFECHAPRI = drd.GetOrdinal("FECHAPRI");
                if (drd.HasRows)
                {
                    drd.Read();
                    obeRegistroPrevio.RegistroPrevioId = drd.GetInt16(posREGPREV_ID);
                    obeRegistroPrevio.PrimerApellido = drd.GetString(posPRIMER_APELLIDO);
                    if (!drd.IsDBNull(posSEGUNDO_APELLIDO)) { obeRegistroPrevio.SegundoApellido = drd.GetString(posSEGUNDO_APELLIDO); }
                    else { obeRegistroPrevio.SegundoApellido = ""; }
                    obeRegistroPrevio.Nombres = drd.GetString(posNOMBRES);
                    if (!drd.IsDBNull(posGENERO)) { obeRegistroPrevio.GeneroId = drd.GetInt16(posGENERO); }
                    else { obeRegistroPrevio.GeneroId = 0; }
                    obeRegistroPrevio.OficinaConsularExId = drd.GetInt16(posOFOCEX_ID);
                    obeRegistroPrevio.CategoriaOfcoExId = drd.GetInt16(posCATEGORIA_OFCOEX);
                    if (!drd.IsDBNull(posCALMIG_ID)) { obeRegistroPrevio.CalidadMigratoria = drd.GetInt16(posCALMIG_ID); }
                    else { obeRegistroPrevio.CalidadMigratoria = 0; }
                    obeRegistroPrevio.ConCarneIdentidadId = drd.GetInt16(posCARDIP_ID);
                    obeRegistroPrevio.TipoEntrada = drd.GetInt16(posTIPO_ENTRADA);
                    if (!drd.IsDBNull(posFECHACON)) { obeRegistroPrevio.FechaConsulares = drd.GetDateTime(posFECHACON); }
                    if (!drd.IsDBNull(posFECHAPRI)) { obeRegistroPrevio.FechaPrivilegios = drd.GetDateTime(posFECHAPRI); }
                }
                drd.Close();
            }
            return (obeRegistroPrevio);
        }

        public bool actualizar(SqlConnection con, SqlTransaction trx, beRegistroPrevio obeRegistroPrevio)
        {
            bool exito = false;
            string Fecha = "01/01/0001";
            DateTime FechaNull = DateTime.Parse(Fecha);
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_REGISTRO_PREVIO_ACTUALIZAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par13 = cmd.Parameters.Add("@P_REGISTRO_PREVIO_ID", SqlDbType.SmallInt);
            par13.Direction = ParameterDirection.Input;
            par13.Value = obeRegistroPrevio.RegistroPrevioId;

            SqlParameter par14 = cmd.Parameters.Add("@P_CARNE_IDENTIDAD_ID", SqlDbType.SmallInt);
            par14.Direction = ParameterDirection.Input;
            par14.Value = obeRegistroPrevio.ConCarneIdentidadId;

            SqlParameter par1 = cmd.Parameters.Add("@P_PRIMER_APELLIDO", SqlDbType.VarChar, 100);
            par1.Direction = ParameterDirection.Input;
            par1.Value = obeRegistroPrevio.PrimerApellido;

            if (obeRegistroPrevio.SegundoApellido == null | obeRegistroPrevio.SegundoApellido == "") { SqlParameter par2 = cmd.Parameters.Add("@P_SEGUNDO_APELLIDO", DBNull.Value); }
            else
            {
                SqlParameter par2 = cmd.Parameters.Add("@P_SEGUNDO_APELLIDO", SqlDbType.VarChar, 100);
                par2.Direction = ParameterDirection.Input;
                par2.Value = obeRegistroPrevio.SegundoApellido;
            }

            SqlParameter par11 = cmd.Parameters.Add("@P_NOMBRES", SqlDbType.VarChar, 100);
            par11.Direction = ParameterDirection.Input;
            par11.Value = obeRegistroPrevio.Nombres;

            if (obeRegistroPrevio.GeneroId == 0) { SqlParameter par3 = cmd.Parameters.Add("@P_GENERO", DBNull.Value); }
            else
            {
                SqlParameter par3 = cmd.Parameters.Add("@P_GENERO", SqlDbType.SmallInt);
                par3.Direction = ParameterDirection.Input;
                par3.Value = obeRegistroPrevio.GeneroId;
            }

            SqlParameter par4 = cmd.Parameters.Add("@P_OFICINA_CONSULAR_EX", SqlDbType.SmallInt);
            par4.Direction = ParameterDirection.Input;
            par4.Value = obeRegistroPrevio.OficinaConsularExId;

            if (obeRegistroPrevio.CalidadMigratoria == 0) { SqlParameter par5 = cmd.Parameters.Add("@P_CALIDAD_MIGRATORIA", DBNull.Value); }
            else
            {
                SqlParameter par5 = cmd.Parameters.Add("@P_CALIDAD_MIGRATORIA", SqlDbType.SmallInt);
                par5.Direction = ParameterDirection.Input;
                par5.Value = obeRegistroPrevio.CalidadMigratoria;
            }

            SqlParameter par15 = cmd.Parameters.Add("@P_FECHA_CON", SqlDbType.DateTime);
            par15.Direction = ParameterDirection.Input;
            par15.Value = obeRegistroPrevio.FechaConsulares;

            if (obeRegistroPrevio.FechaPrivilegios == FechaNull) { SqlParameter par16 = cmd.Parameters.Add("@P_FECHA_PRI", DBNull.Value); }
            else
            {
                SqlParameter par16 = cmd.Parameters.Add("@P_FECHA_PRI", SqlDbType.DateTime);
                par16.Direction = ParameterDirection.Input;
                par16.Value = obeRegistroPrevio.FechaPrivilegios;
            }

            SqlParameter par8 = cmd.Parameters.Add("@P_USUARIO_MODIFICACION", SqlDbType.SmallInt);
            par8.Direction = ParameterDirection.Input;
            par8.Value = obeRegistroPrevio.UsuarioModificacion;

            SqlParameter par9 = cmd.Parameters.Add("@P_IP_MODIFICACION", SqlDbType.VarChar, 50);
            par9.Direction = ParameterDirection.Input;
            par9.Value = obeRegistroPrevio.IpModificacion;

            SqlParameter par12 = cmd.Parameters.Add("@P_TIPO_ENTRADA", SqlDbType.SmallInt);
            par12.Direction = ParameterDirection.Input;
            par12.Value = obeRegistroPrevio.TipoEntrada;

            int n = cmd.ExecuteNonQuery();
            if (n != 0)
            {
                exito = true;
            }
            return (exito);
        }

        public bool agregarSolicitante(SqlConnection con, SqlTransaction trx, beRegistroPrevio obeRegistroPrevio)
        {
            bool exito = false;
            string Fecha = "01/01/0001";
            DateTime FechaNull = DateTime.Parse(Fecha);
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_REGISTRO_PREVIO_AGREGAR_SOLICITANTE", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par13 = cmd.Parameters.Add("@P_REGISTRO_PREVIO_ID", SqlDbType.SmallInt);
            par13.Direction = ParameterDirection.Input;
            par13.Value = obeRegistroPrevio.RegistroPrevioId;

            SqlParameter par17 = cmd.Parameters.Add("@P_SOLICITANTE", SqlDbType.SmallInt);
            par17.Direction = ParameterDirection.Input;
            par17.Value = obeRegistroPrevio.SolicitanteId;

            SqlParameter par8 = cmd.Parameters.Add("@P_USUARIO_MODIFICACION", SqlDbType.SmallInt);
            par8.Direction = ParameterDirection.Input;
            par8.Value = obeRegistroPrevio.UsuarioModificacion;

            SqlParameter par9 = cmd.Parameters.Add("@P_IP_MODIFICACION", SqlDbType.VarChar, 50);
            par9.Direction = ParameterDirection.Input;
            par9.Value = obeRegistroPrevio.IpModificacion;

            int n = cmd.ExecuteNonQuery();
            if (n != 0)
            {
                exito = true;
            }
            return (exito);
        }
    }
}
