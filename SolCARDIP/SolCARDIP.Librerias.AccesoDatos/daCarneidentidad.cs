using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daCarneidentidad
    {
        public short adicionar(SqlConnection con, SqlTransaction trx, beCarneIdentidad parametrosCarneIdentidad)
        {
            short CarneIdentidadId = -1;
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CARNE_IDENTIDAD_ADICIONAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_PERSONA_ID", SqlDbType.BigInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosCarneIdentidad.Personaid;

            if (parametrosCarneIdentidad.IdentMesaPartes == null)
            { SqlParameter par11 = cmd.Parameters.Add("@P_MESA_PARTES", DBNull.Value); }
            else
            {
                SqlParameter par11 = cmd.Parameters.Add("@P_MESA_PARTES", SqlDbType.VarChar, 20);
                par11.Direction = ParameterDirection.Input;
                par11.Value = parametrosCarneIdentidad.IdentMesaPartes;
            }

            SqlParameter par10 = cmd.Parameters.Add("@P_PERIODO", SqlDbType.Int);
            par10.Direction = ParameterDirection.Input;
            par10.Value = parametrosCarneIdentidad.Periodo;

            SqlParameter par2 = cmd.Parameters.Add("@P_CALIDAD_MIGRATORIA", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametrosCarneIdentidad.CalidadMigratoriaid;

            SqlParameter par9 = cmd.Parameters.Add("@P_CALIDAD_MIGRATORIA_SEC", SqlDbType.SmallInt);
            par9.Direction = ParameterDirection.Input;
            par9.Value = parametrosCarneIdentidad.CalidadMigratoriaSecId;

            SqlParameter par3 = cmd.Parameters.Add("@P_OFICINA_CONSULAR_EX", SqlDbType.SmallInt);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametrosCarneIdentidad.OficinaConsularExid;

            SqlParameter par4 = cmd.Parameters.Add("@P_RUTA_ARCHIVO_FOTO", SqlDbType.VarChar, 250);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametrosCarneIdentidad.RutaArchivoFoto;

            SqlParameter par12 = cmd.Parameters.Add("@P_RUTA_ARCHIVO_FIRMA", SqlDbType.VarChar, 250);
            par12.Direction = ParameterDirection.Input;
            par12.Value = parametrosCarneIdentidad.RutaArchivoFirma;

            SqlParameter par5 = cmd.Parameters.Add("@P_USUARIO_CREACION", SqlDbType.SmallInt);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametrosCarneIdentidad.Usuariocreacion;

            SqlParameter par6 = cmd.Parameters.Add("@P_ESTADO_ID", SqlDbType.SmallInt);
            par6.Direction = ParameterDirection.Input;
            par6.Value = parametrosCarneIdentidad.Estadoid;

            SqlParameter par7 = cmd.Parameters.Add("@P_IP_CREACION", SqlDbType.VarChar, 50);
            par7.Direction = ParameterDirection.Input;
            par7.Value = parametrosCarneIdentidad.Ipcreacion;

            SqlParameter par8 = cmd.Parameters.Add("@@IDENTITY", SqlDbType.SmallInt);
            par8.Direction = ParameterDirection.ReturnValue;

            int n = cmd.ExecuteNonQuery();
            if (n > 0) CarneIdentidadId = Convert.ToInt16(par8.Value);
            return (CarneIdentidadId);
        }

        public bool actualizar(SqlConnection con, SqlTransaction trx, beCarneIdentidad parametrosCarneIdentidad)
        {
            bool exito = false;
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CARNE_IDENTIDAD_ACTUALIZAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_CARNE_IDENTIDAD_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosCarneIdentidad.CarneIdentidadid;

            if (parametrosCarneIdentidad.IdentMesaPartes == null)
            { SqlParameter par10 = cmd.Parameters.Add("@P_MESA_PARTES", DBNull.Value); }
            else
            {
                SqlParameter par10 = cmd.Parameters.Add("@P_MESA_PARTES", SqlDbType.VarChar, 20);
                par10.Direction = ParameterDirection.Input;
                par10.Value = parametrosCarneIdentidad.IdentMesaPartes;
            }

            SqlParameter par2 = cmd.Parameters.Add("@P_CALIDAD_MIGRATORIA", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametrosCarneIdentidad.CalidadMigratoriaid;

            SqlParameter par9 = cmd.Parameters.Add("@P_CALIDAD_MIGRATORIA_SEC", SqlDbType.SmallInt);
            par9.Direction = ParameterDirection.Input;
            par9.Value = parametrosCarneIdentidad.CalidadMigratoriaSecId;

            SqlParameter par3 = cmd.Parameters.Add("@P_OFICINA_CONSULAR_EX", SqlDbType.SmallInt);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametrosCarneIdentidad.OficinaConsularExid;

            SqlParameter par4 = cmd.Parameters.Add("@P_RUTA_ARCHIVO_FOTO", SqlDbType.VarChar, 250);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametrosCarneIdentidad.RutaArchivoFoto;

            SqlParameter par12 = cmd.Parameters.Add("@P_RUTA_ARCHIVO_FIRMA", SqlDbType.VarChar, 250);
            par12.Direction = ParameterDirection.Input;
            par12.Value = parametrosCarneIdentidad.RutaArchivoFirma;

            SqlParameter par11 = cmd.Parameters.Add("@P_RENOVAR", SqlDbType.Bit);
            par11.Direction = ParameterDirection.Input;
            par11.Value = parametrosCarneIdentidad.Renovar;

            SqlParameter par14 = cmd.Parameters.Add("@P_FLAG_REG_COMP", SqlDbType.Bit);
            par14.Direction = ParameterDirection.Input;
            par14.Value = parametrosCarneIdentidad.FlagRegistroCompleto;

            SqlParameter par13 = cmd.Parameters.Add("@P_PERSONA_ID", SqlDbType.BigInt);
            par13.Direction = ParameterDirection.Input;
            par13.Value = parametrosCarneIdentidad.Personaid;
            
            SqlParameter par5 = cmd.Parameters.Add("@P_USUARIO_MODIFICACION", SqlDbType.SmallInt);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametrosCarneIdentidad.Usuariomodificacion;

            SqlParameter par7 = cmd.Parameters.Add("@P_IP_MODIFICACION", SqlDbType.VarChar, 50);
            par7.Direction = ParameterDirection.Input;
            par7.Value = parametrosCarneIdentidad.Ipmodificacion;

            SqlParameter par15 = cmd.Parameters.Add("@P_ESTADO_ID", SqlDbType.SmallInt);
            par15.Direction = ParameterDirection.Input;
            par15.Value = parametrosCarneIdentidad.Estadoid;

            int n = cmd.ExecuteNonQuery();
            if (n != 0)
            {
                exito = true;
            }
            return (exito);
        }
        public void GenerarDuplicadoCarnet(SqlConnection con, SqlTransaction trx, beCarneIdentidad parametrosCarneIdentidad, beRegistroLinea parametros)
        {
            try
            {
                    #region Beneficiario
                using (SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CARNE_IDENTIDAD_GENERAR_DUPLICADO", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Transaction = trx;
                        cmd.Parameters.Add("@P_CARNE_IDENTIDAD_ID", SqlDbType.BigInt).Value = parametrosCarneIdentidad.CarneIdentidadid;
                        cmd.Parameters.Add("@P_RELI_VDP_NUMERO_REG_LINEA", SqlDbType.VarChar, 50).Value = parametros.NumeroRegLinea;
                        cmd.Parameters.Add("@P_ESTADO_ID", SqlDbType.SmallInt).Value = parametrosCarneIdentidad.Estadoid;
                        cmd.Parameters.Add("@P_CAID_SUSUARIOMODIFICACION", SqlDbType.SmallInt).Value = parametrosCarneIdentidad.Usuariomodificacion;
                        cmd.Parameters.Add("@P_CAID_VIPMODIFICACION", SqlDbType.VarChar, 50).Value = parametrosCarneIdentidad.Ipmodificacion;
                        cmd.ExecuteNonQuery();
                    }
                    #endregion Beneficiario
            }
            catch (SqlException exec)
            {
                throw exec;
            }
        }
        public bool ConsultarExistenciaCarnetPorNombre(SqlConnection con, SqlTransaction trx, beCarneIdentidad parametrosCarneIdentidad)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CARNE_IDENTIDAD_EXISTENCIA_CARNET_NOMBRES", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = trx;
                    cmd.Parameters.Add("@P_APELLIDO_PAT", SqlDbType.VarChar,100).Value = parametrosCarneIdentidad.ApePatPersona;
                    cmd.Parameters.Add("@P_NOMBRES", SqlDbType.VarChar, 100).Value = parametrosCarneIdentidad.NombresPersona;
                    SqlDataReader drd = cmd.ExecuteReader();
                    if (drd != null)
                    {
                        if (drd.Read())
                        {
                            drd.Close();
                            return true;
                        }
                        else {
                            drd.Close();
                            return false;
                        }
                    }
                    else {
                        return false;
                    }
                }
            }
            catch (SqlException exec)
            {
                throw exec;
            }
        }
        
        public bool actualizarEstado(SqlConnection con, SqlTransaction trx, beCarneIdentidad parametrosCarneIdentidad)
        {
            bool exito = false;
            string Fecha = "01/01/0001";
            DateTime FechaNull = DateTime.Parse(Fecha);
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CARNE_IDENTIDAD_ACTUALIZAR_ESTADO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_CARNE_IDENTIDAD_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosCarneIdentidad.CarneIdentidadid;

            if (parametrosCarneIdentidad.FechaEmision == FechaNull) { SqlParameter par2 = cmd.Parameters.Add("@P_FECHA_EMISION", DBNull.Value); }
            else
            {
                SqlParameter par2 = cmd.Parameters.Add("@P_FECHA_EMISION", SqlDbType.DateTime);
                par2.Direction = ParameterDirection.Input;
                par2.Value = parametrosCarneIdentidad.FechaEmision;
            }

            if (parametrosCarneIdentidad.FechaEmision == FechaNull) { SqlParameter par3 = cmd.Parameters.Add("@P_FECHA_VENCIMIENTO", DBNull.Value); }
            else
            {
                SqlParameter par3 = cmd.Parameters.Add("@P_FECHA_VENCIMIENTO", SqlDbType.DateTime);
                par3.Direction = ParameterDirection.Input;
                par3.Value = parametrosCarneIdentidad.FechaVencimiento;
            }

            SqlParameter par4 = cmd.Parameters.Add("@P_ESTADO_ID", SqlDbType.SmallInt);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametrosCarneIdentidad.Estadoid;

            SqlParameter par5 = cmd.Parameters.Add("@P_NUEVO_SI_NO", SqlDbType.Bit);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametrosCarneIdentidad.NuevoCarne;

            SqlParameter par6 = cmd.Parameters.Add("@P_RENOVAR", SqlDbType.Bit);
            par6.Direction = ParameterDirection.Input;
            par6.Value = parametrosCarneIdentidad.Renovar;

            SqlParameter par7 = cmd.Parameters.Add("@P_FLAG_CONTROL_CALIDAD", SqlDbType.Bit);
            par7.Direction = ParameterDirection.Input;
            par7.Value = parametrosCarneIdentidad.FlagControlCalidad;

            if (parametrosCarneIdentidad.ControlCalidad != null)
            {
                SqlParameter par8 = cmd.Parameters.Add("@P_CONTROL_CALIDAD", SqlDbType.VarChar, 29);
                par8.Direction = ParameterDirection.Input;
                par8.Value = parametrosCarneIdentidad.ControlCalidad;
            }
            else
            {
                SqlParameter par8 = cmd.Parameters.Add("@P_CONTROL_CALIDAD", SqlDbType.VarChar, 29);
                par8.Direction = ParameterDirection.Input;
                par8.Value = "1|1|1|1|1|1|1|1|1|1|1|1|1|1|1";
            }

            if (parametrosCarneIdentidad.ControlCalidadDetalle != null)
            {
                SqlParameter par9 = cmd.Parameters.Add("@P_CONTROL_CALIDAD_DETALLE", SqlDbType.VarChar, 500);
                par9.Direction = ParameterDirection.Input;
                par9.Value = parametrosCarneIdentidad.ControlCalidadDetalle;
            }
            else
            {
                SqlParameter par9 = cmd.Parameters.Add("@P_CONTROL_CALIDAD_DETALLE", DBNull.Value);
            }

            int n = cmd.ExecuteNonQuery();
            if (n != 0)
            {
                exito = true;
            }
            return (exito);
        }
        public bool actualizarEstadoObservado(SqlConnection con, SqlTransaction trx, beCarneIdentidad parametrosCarneIdentidad)
        {
            bool exito = false;
            string Fecha = "01/01/0001";
            DateTime FechaNull = DateTime.Parse(Fecha);
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CARNE_IDENTIDAD_ACTUALIZAR_ESTADO_OBSERVADO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@RELI_SREG_LINEA_ID", SqlDbType.Int);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosCarneIdentidad.ConSolicitudId;

            SqlParameter par2 = cmd.Parameters.Add("@P_ESTADO_ID", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametrosCarneIdentidad.Estadoid;

            SqlParameter par3 = cmd.Parameters.Add("@P_OBSERVACION", SqlDbType.VarChar,  250);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametrosCarneIdentidad.Oservacion;

            SqlParameter par4 = cmd.Parameters.Add("@CAID_SUSUARIOMODIFICACION", SqlDbType.SmallInt);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametrosCarneIdentidad.Usuariomodificacion;

            int n = cmd.ExecuteNonQuery();
            if (n != 0)
            {
                exito = true;
            }
            return (exito);
        }
        public beCarneIdentidad obtenerCorreoCiudadano(SqlConnection con,  beCarneIdentidad parametrosCarneIdentidad)
        {
            beCarneIdentidad beCcarne = null;
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CARNE_IDENTIDAD_OBTENER_CORREO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            

            SqlParameter par1 = cmd.Parameters.Add("@P_RELI_SREG_LINEA_ID", SqlDbType.Int);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosCarneIdentidad.ConSolicitudId;

            SqlDataReader drd = cmd.ExecuteReader();
            int posDPCorr = drd.GetOrdinal("RELI_VDP_CORREO_ELECTRONICO");
            int posNumero = drd.GetOrdinal("RELI_VDP_NUMERO_REG_LINEA");

            if (drd.Read())
            {
                beCcarne = new beCarneIdentidad();
                beCcarne.Correo = drd.GetString(posDPCorr);
                beCcarne.Numero_reg_linea = drd.GetString(posNumero);
            }
                return beCcarne;
        }
        
        public string obtenerMesajeEstado(SqlConnection con,  beCarneIdentidad parametrosCarneIdentidad)
        {
            string  mensaje = "";
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CONSULTA_MENSAJE_ESTADO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            

            SqlParameter par1 = cmd.Parameters.Add("@CAID_ICARNE_IDENTIDADID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosCarneIdentidad.CarneIdentidadid;

            SqlParameter par2 = cmd.Parameters.Add("@MEES_SESTADOID", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametrosCarneIdentidad.Estadoid;

            SqlDataReader drd = cmd.ExecuteReader();
            int posmenj = drd.GetOrdinal("MEES_VMENSAJE");

            if (drd.Read())
            {
                mensaje = drd.GetString(posmenj);
            }
                return mensaje;
        }

        public beCarneIdentidadPrincipal consultar(SqlConnection con, beCarneIdentidad parametrosCarneIdentidad)
        {
            beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CARNE_IDENTIDAD_CONSULTAR", con);
            cmd.CommandType = CommandType.StoredProcedure;

            #region Parametros
            SqlParameter par15 = cmd.Parameters.Add("@P_PERIODO", SqlDbType.Int);
            par15.Direction = ParameterDirection.Input;
            par15.Value = parametrosCarneIdentidad.Periodo;

            SqlParameter par16 = cmd.Parameters.Add("@P_MESA_PARTES", SqlDbType.VarChar, 20);
            par16.Direction = ParameterDirection.Input;
            par16.Value = parametrosCarneIdentidad.IdentMesaPartes;

            SqlParameter par1 = cmd.Parameters.Add("@P_NUMERO_IDENT", SqlDbType.Int);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosCarneIdentidad.IdentNumero;

            SqlParameter par2 = cmd.Parameters.Add("@P_NUMERO_CARNE", SqlDbType.VarChar, 10);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametrosCarneIdentidad.CarneNumero;

            SqlParameter par3 = cmd.Parameters.Add("@P_ESTADO", SqlDbType.SmallInt);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametrosCarneIdentidad.Estadoid;

            SqlParameter par4 = cmd.Parameters.Add("@P_CALIDAD_MIG", SqlDbType.SmallInt);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametrosCarneIdentidad.CalidadMigratoriaid;

            SqlParameter par5 = cmd.Parameters.Add("@P_CARGO", SqlDbType.SmallInt);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametrosCarneIdentidad.CalidadMigratoriaSecId;

            SqlParameter par17 = cmd.Parameters.Add("@P_CAT_OFICINA", SqlDbType.SmallInt);
            par17.Direction = ParameterDirection.Input;
            par17.Value = parametrosCarneIdentidad.CatMision;

            SqlParameter par6 = cmd.Parameters.Add("@P_OFICINA_CON_EX", SqlDbType.SmallInt);
            par6.Direction = ParameterDirection.Input;
            par6.Value = parametrosCarneIdentidad.OficinaConsularExid;

            SqlParameter par7 = cmd.Parameters.Add("@P_FECHA_EMISION", SqlDbType.DateTime);
            par7.Direction = ParameterDirection.Input;
            par7.Value = parametrosCarneIdentidad.FechaEmision;

            SqlParameter par8 = cmd.Parameters.Add("@P_FECHA_VENC", SqlDbType.DateTime);
            par8.Direction = ParameterDirection.Input;
            par8.Value = parametrosCarneIdentidad.FechaVencimiento;

            SqlParameter par9 = cmd.Parameters.Add("@P_APELLIDO_PAT", SqlDbType.VarChar, 100);
            par9.Direction = ParameterDirection.Input;
            par9.Value = parametrosCarneIdentidad.ApePatPersona;

            SqlParameter par10 = cmd.Parameters.Add("@P_APELLIDO_MAT", SqlDbType.VarChar, 100);
            par10.Direction = ParameterDirection.Input;
            par10.Value = parametrosCarneIdentidad.ApeMatPersona;

            SqlParameter par11 = cmd.Parameters.Add("@P_NOMBRES", SqlDbType.VarChar, 100);
            par11.Direction = ParameterDirection.Input;
            par11.Value = parametrosCarneIdentidad.NombresPersona;

            SqlParameter par12 = cmd.Parameters.Add("@P_PAIS", SqlDbType.SmallInt);
            par12.Direction = ParameterDirection.Input;
            par12.Value = parametrosCarneIdentidad.PaisPersona;

            SqlParameter par13 = cmd.Parameters.Add("@P_NUM_PAG", SqlDbType.BigInt);
            par13.Direction = ParameterDirection.Input;
            par13.Value = parametrosCarneIdentidad.NumPag;

            SqlParameter par14 = cmd.Parameters.Add("@P_CANT_REG_PAG", SqlDbType.BigInt);
            par14.Direction = ParameterDirection.Input;
            par14.Value = parametrosCarneIdentidad.CantReg;
            #endregion
            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                List<beCarneIdentidad> lbeCarneIdentidad = new List<beCarneIdentidad>();
                int posCarneIdentidadid = drd.GetOrdinal("CARDIP_ID");
                int posMesaPartes = drd.GetOrdinal("MESA_PARTES");
                int posConIdent = drd.GetOrdinal("NUMERO_IDENT");
                int posCarneNumero = drd.GetOrdinal("NUMERO_CARNE");
                int posConEstado = drd.GetOrdinal("ESTADO");
                int posConFuncionario = drd.GetOrdinal("FUNCIONARIO");
                int posConPaisNacionalidad = drd.GetOrdinal("PAIS_NACIONALIDAD");
                int posConCalidadMigratoria = drd.GetOrdinal("CALIDAD_MIGRATORIA");
                int posConCargo = drd.GetOrdinal("CARGO");
                int posConOficinaConsularEx = drd.GetOrdinal("OFICINA_CONSULAR_EXTRANJERA");
                int posConFechaInscripcion = drd.GetOrdinal("FECHA_INSCRIPCION");
                int posConFechaEmision = drd.GetOrdinal("FECHA_EMISION");
                int posConFechaVen = drd.GetOrdinal("FECHA_VENCIMIENTO");
                int posRutaArchivoFoto = drd.GetOrdinal("RUTA_ARCHIVO");
                int posRUTA_ARCHIVO_FIRMA = drd.GetOrdinal("RUTA_ARCHIVO_FIRMA");
                int posCALMIG_ID = drd.GetOrdinal("CALMIG_ID");
                int posCALMIGSEC_ID = drd.GetOrdinal("CALMIGSEC_ID");
                int posOFCOEX_ID = drd.GetOrdinal("OFCOEX_ID");
                int posRENOVAR = drd.GetOrdinal("RENOVAR");
                // ULTIMOS
                int posFUNC_APEP = drd.GetOrdinal("FUNC_APEP");
                int posFUNC_APEM = drd.GetOrdinal("FUNC_APEM");
                int posFUNC_NOMBRES = drd.GetOrdinal("FUNC_NOMBRES");
                int posFUNC_FECNAC = drd.GetOrdinal("FUNC_FECNAC");
                int posFUNC_GENERO = drd.GetOrdinal("FUNC_GENERO");
                int posOFCOEX_CATEGORIA = drd.GetOrdinal("OFCOEX_CATEGORIA");
                int posESTCIVIL_DESC = drd.GetOrdinal("ESTCIVIL_DESC");
                int posREGISTRADOR_ID = drd.GetOrdinal("REGISTRADOR_ID");
                int posREGPREV_COMPLETO = drd.GetOrdinal("REGPREV_COMPLETO");
                int posREGPREV_ID = drd.GetOrdinal("REGPREV_ID");
                int posFLAG_CONTROL_CALIDAD = drd.GetOrdinal("FLAG_CONTROL_CALIDAD");
                int posCONTROL_CALIDAD = drd.GetOrdinal("CONTROL_CALIDAD");
                int posCONTROL_CALIDAD_DETALLE = drd.GetOrdinal("CONTROL_CALIDAD_DETALLE");
                int posUBIGEO = drd.GetOrdinal("UBIGEO");
                int posDIRECCION = drd.GetOrdinal("DIRECCION");
                int posTITDEP = drd.GetOrdinal("TITDEP");
                int posEstadoId = drd.GetOrdinal("ESTADO_ID");

                int posDuplicado = drd.GetOrdinal("DUPLICADO");
                int posTipoDoc = drd.GetOrdinal("PERIDENT_TIPODOC_DESC");
                int posNumdocident = drd.GetOrdinal("PERIDENT_NUMDOCIDENT");
                int posRegistroLinea = drd.GetOrdinal("RELI_SREG_LINEA_ID");

                beCarneIdentidad obeCarneIdentidad;
                while (drd.Read())
                {
                    obeCarneIdentidad = new beCarneIdentidad();
                    obeCarneIdentidad.CarneIdentidadid = drd.GetInt16(posCarneIdentidadid);
                    obeCarneIdentidad.IdentMesaPartes = drd.GetString(posMesaPartes);
                    obeCarneIdentidad.ConIdent = drd.GetString(posConIdent);
                    obeCarneIdentidad.CarneNumero = drd.GetString(posCarneNumero);
                    obeCarneIdentidad.ConEstado = drd.GetString(posConEstado);
                    obeCarneIdentidad.ConFuncionario = drd.GetString(posConFuncionario);
                    obeCarneIdentidad.ConPaisNacionalidad = drd.GetString(posConPaisNacionalidad);
                    obeCarneIdentidad.ConCalidadMigratoria = drd.GetString(posConCalidadMigratoria);
                    obeCarneIdentidad.ConCargo = drd.GetString(posConCargo);
                    obeCarneIdentidad.ConOficinaConsularEx = drd.GetString(posConOficinaConsularEx);
                    obeCarneIdentidad.ConFechaInscripcion = drd.GetString(posConFechaInscripcion);
                    obeCarneIdentidad.ConFechaEmision = drd.GetString(posConFechaEmision);
                    obeCarneIdentidad.ConFechaVen = drd.GetString(posConFechaVen);
                    obeCarneIdentidad.RutaArchivoFoto = drd.GetString(posRutaArchivoFoto);
                    if (!drd.IsDBNull(posRUTA_ARCHIVO_FIRMA)) { obeCarneIdentidad.RutaArchivoFirma = drd.GetString(posRUTA_ARCHIVO_FIRMA); }
                    else { obeCarneIdentidad.RutaArchivoFirma = "error"; }
                    obeCarneIdentidad.CalidadMigratoriaid = drd.GetInt16(posCALMIG_ID);
                    obeCarneIdentidad.CalidadMigratoriaSecId = drd.GetInt16(posCALMIGSEC_ID);
                    obeCarneIdentidad.OficinaConsularExid = drd.GetInt16(posOFCOEX_ID);
                    obeCarneIdentidad.Renovar = drd.GetBoolean(posRENOVAR);
                    obeCarneIdentidad.ApePatPersona = drd.GetString(posFUNC_APEP);
                    obeCarneIdentidad.ApeMatPersona = drd.GetString(posFUNC_APEM);
                    obeCarneIdentidad.NombresPersona = drd.GetString(posFUNC_NOMBRES);
                    obeCarneIdentidad.ConFechaNac = drd.GetString(posFUNC_FECNAC);
                    obeCarneIdentidad.ConGenero = drd.GetString(posFUNC_GENERO);
                    obeCarneIdentidad.ConCatMision = drd.GetString(posOFCOEX_CATEGORIA);
                    obeCarneIdentidad.ConEstCivil = drd.GetString(posESTCIVIL_DESC);
                    obeCarneIdentidad.UsuarioDeriva = drd.GetInt16(posREGISTRADOR_ID);
                    obeCarneIdentidad.FlagRegistroCompleto = drd.GetBoolean(posREGPREV_COMPLETO);
                    obeCarneIdentidad.ConRegistroPrevioId = drd.GetInt16(posREGPREV_ID);
                    obeCarneIdentidad.FlagControlCalidad = drd.GetBoolean(posFLAG_CONTROL_CALIDAD);
                    obeCarneIdentidad.ControlCalidad = drd.GetString(posCONTROL_CALIDAD);
                    obeCarneIdentidad.ControlCalidadDetalle = drd.GetString(posCONTROL_CALIDAD_DETALLE);
                    obeCarneIdentidad.ConUbigeo = drd.GetString(posUBIGEO);
                    obeCarneIdentidad.ConDireccion = drd.GetString(posDIRECCION);
                    obeCarneIdentidad.ConTitDep = drd.GetString(posTITDEP);
                    obeCarneIdentidad.Estadoid = drd.GetInt16(posEstadoId);
                    obeCarneIdentidad.Duplicado = drd.GetBoolean(posDuplicado);
                    obeCarneIdentidad.TipodocDesc = drd.GetString(posTipoDoc);
                    obeCarneIdentidad.DocumentoNumero = drd.GetString(posNumdocident);
                    obeCarneIdentidad.ConSolicitudId = drd.GetInt32(posRegistroLinea);
                    lbeCarneIdentidad.Add(obeCarneIdentidad);
                    
                }
                obeCarneIdentidadPrincipal.ListaConsulta = lbeCarneIdentidad;
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
                        obeCarneIdentidadPrincipal.Paginacion = obePaginacion;
                    }
                }
                drd.Close();
            }
            return (obeCarneIdentidadPrincipal);
        }

        public beCarneIdentidad consultarxId(SqlConnection con, short carneID)
        {
            beCarneIdentidad obeCarneIdentidad = new beCarneIdentidad();
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CARNE_IDENTIDAD_CONSULTAR_x_ID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_CARNE_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = carneID;
            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                List<beCarneIdentidad> lbeCarneIdentidad = new List<beCarneIdentidad>();
                int posCarneIdentidadid = drd.GetOrdinal("CARDIP_ID");
                int posMesaPartes = drd.GetOrdinal("MESA_PARTES");
                int posConIdent = drd.GetOrdinal("NUMERO_IDENT");
                int posCarneNumero = drd.GetOrdinal("NUMERO_CARNE");
                int posConEstado = drd.GetOrdinal("ESTADO");
                int posConFuncionario = drd.GetOrdinal("FUNCIONARIO");
                int posConPaisNacionalidad = drd.GetOrdinal("PAIS_NACIONALIDAD");
                int posConCalidadMigratoria = drd.GetOrdinal("CALIDAD_MIGRATORIA");
                int posConCargo = drd.GetOrdinal("CARGO");
                int posConOficinaConsularEx = drd.GetOrdinal("OFICINA_CONSULAR_EXTRANJERA");
                int posConFechaInscripcion = drd.GetOrdinal("FECHA_INSCRIPCION");
                int posConFechaEmision = drd.GetOrdinal("FECHA_EMISION");
                int posConFechaVen = drd.GetOrdinal("FECHA_VENCIMIENTO");
                int posRutaArchivoFoto = drd.GetOrdinal("RUTA_ARCHIVO");
                int posRUTA_ARCHIVO_FIRMA = drd.GetOrdinal("RUTA_ARCHIVO_FIRMA");
                int posCALMIG_ID = drd.GetOrdinal("CALMIG_ID");
                int posCALMIGSEC_ID = drd.GetOrdinal("CALMIGSEC_ID");
                int posOFCOEX_ID = drd.GetOrdinal("OFCOEX_ID");
                int posRENOVAR = drd.GetOrdinal("RENOVAR");
                // ULTIMOS
                int posFUNC_APEP = drd.GetOrdinal("FUNC_APEP");
                int posFUNC_APEM = drd.GetOrdinal("FUNC_APEM");
                int posFUNC_NOMBRES = drd.GetOrdinal("FUNC_NOMBRES");
                int posFUNC_FECNAC = drd.GetOrdinal("FUNC_FECNAC");
                int posFUNC_GENERO = drd.GetOrdinal("FUNC_GENERO");
                int posOFCOEX_CATEGORIA = drd.GetOrdinal("OFCOEX_CATEGORIA");
                int posESTCIVIL_DESC = drd.GetOrdinal("ESTCIVIL_DESC");
                int posREGISTRADOR_ID = drd.GetOrdinal("REGISTRADOR_ID");
                int posREGPREV_COMPLETO = drd.GetOrdinal("REGPREV_COMPLETO");
                int posREGPREV_ID = drd.GetOrdinal("REGPREV_ID");
                int posFLAG_CONTROL_CALIDAD = drd.GetOrdinal("FLAG_CONTROL_CALIDAD");
                int posCONTROL_CALIDAD = drd.GetOrdinal("CONTROL_CALIDAD");
                int posCONTROL_CALIDAD_DETALLE = drd.GetOrdinal("CONTROL_CALIDAD_DETALLE");
                int posUBIGEO = drd.GetOrdinal("UBIGEO");
                int posDIRECCION = drd.GetOrdinal("DIRECCION");
                int posTITDEP = drd.GetOrdinal("TITDEP");
                if (drd.HasRows)
                {
                    drd.Read();
                    obeCarneIdentidad.CarneIdentidadid = drd.GetInt16(posCarneIdentidadid);
                    obeCarneIdentidad.IdentMesaPartes = drd.GetString(posMesaPartes);
                    obeCarneIdentidad.ConIdent = drd.GetString(posConIdent);
                    obeCarneIdentidad.CarneNumero = drd.GetString(posCarneNumero);
                    obeCarneIdentidad.ConEstado = drd.GetString(posConEstado);
                    obeCarneIdentidad.ConFuncionario = drd.GetString(posConFuncionario);
                    obeCarneIdentidad.ConPaisNacionalidad = drd.GetString(posConPaisNacionalidad);
                    obeCarneIdentidad.ConCalidadMigratoria = drd.GetString(posConCalidadMigratoria);
                    obeCarneIdentidad.ConCargo = drd.GetString(posConCargo);
                    obeCarneIdentidad.ConOficinaConsularEx = drd.GetString(posConOficinaConsularEx);
                    obeCarneIdentidad.ConFechaInscripcion = drd.GetString(posConFechaInscripcion);
                    obeCarneIdentidad.ConFechaEmision = drd.GetString(posConFechaEmision);
                    obeCarneIdentidad.ConFechaVen = drd.GetString(posConFechaVen);
                    obeCarneIdentidad.RutaArchivoFoto = drd.GetString(posRutaArchivoFoto);
                    if (!drd.IsDBNull(posRUTA_ARCHIVO_FIRMA)) { obeCarneIdentidad.RutaArchivoFirma = drd.GetString(posRUTA_ARCHIVO_FIRMA); }
                    else { obeCarneIdentidad.RutaArchivoFirma = "error"; }
                    obeCarneIdentidad.CalidadMigratoriaid = drd.GetInt16(posCALMIG_ID);
                    obeCarneIdentidad.CalidadMigratoriaSecId = drd.GetInt16(posCALMIGSEC_ID);
                    obeCarneIdentidad.OficinaConsularExid = drd.GetInt16(posOFCOEX_ID);
                    obeCarneIdentidad.Renovar = drd.GetBoolean(posRENOVAR);
                    obeCarneIdentidad.ApePatPersona = drd.GetString(posFUNC_APEP);
                    obeCarneIdentidad.ApeMatPersona = drd.GetString(posFUNC_APEM);
                    obeCarneIdentidad.NombresPersona = drd.GetString(posFUNC_NOMBRES);
                    obeCarneIdentidad.ConFechaNac = drd.GetString(posFUNC_FECNAC);
                    obeCarneIdentidad.ConGenero = drd.GetString(posFUNC_GENERO);
                    obeCarneIdentidad.ConCatMision = drd.GetString(posOFCOEX_CATEGORIA);
                    obeCarneIdentidad.ConEstCivil = drd.GetString(posESTCIVIL_DESC);
                    obeCarneIdentidad.UsuarioDeriva = drd.GetInt16(posREGISTRADOR_ID);
                    obeCarneIdentidad.FlagRegistroCompleto = drd.GetBoolean(posREGPREV_COMPLETO);
                    obeCarneIdentidad.ConRegistroPrevioId = drd.GetInt16(posREGPREV_ID);
                    obeCarneIdentidad.FlagControlCalidad = drd.GetBoolean(posFLAG_CONTROL_CALIDAD);
                    obeCarneIdentidad.ControlCalidad = drd.GetString(posCONTROL_CALIDAD);
                    obeCarneIdentidad.ControlCalidadDetalle = drd.GetString(posCONTROL_CALIDAD_DETALLE);
                    obeCarneIdentidad.ConUbigeo = drd.GetString(posUBIGEO);
                    obeCarneIdentidad.ConDireccion = drd.GetString(posDIRECCION);
                    obeCarneIdentidad.ConTitDep = drd.GetString(posTITDEP);
                }
                drd.Close();
            }
            return (obeCarneIdentidad);
        }

        public beCarneIdentidadPrincipal consultarPrivilegios(SqlConnection con, beCarneIdentidad parametrosCarneIdentidad)
        {
            beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CARNE_IDENTIDAD_CONSULTAR_PRIVILEGIOS", con);
            cmd.CommandType = CommandType.StoredProcedure;

            #region Parametros
            SqlParameter par15 = cmd.Parameters.Add("@P_PERIODO", SqlDbType.Int);
            par15.Direction = ParameterDirection.Input;
            par15.Value = parametrosCarneIdentidad.Periodo;

            SqlParameter par16 = cmd.Parameters.Add("@P_MESA_PARTES", SqlDbType.VarChar, 20);
            par16.Direction = ParameterDirection.Input;
            par16.Value = parametrosCarneIdentidad.IdentMesaPartes;

            SqlParameter par1 = cmd.Parameters.Add("@P_NUMERO_IDENT", SqlDbType.Int);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosCarneIdentidad.IdentNumero;

            SqlParameter par2 = cmd.Parameters.Add("@P_NUMERO_CARNE", SqlDbType.VarChar, 10);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametrosCarneIdentidad.CarneNumero;

            SqlParameter par3 = cmd.Parameters.Add("@P_ESTADO", SqlDbType.SmallInt);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametrosCarneIdentidad.Estadoid;

            SqlParameter par4 = cmd.Parameters.Add("@P_CALIDAD_MIG", SqlDbType.SmallInt);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametrosCarneIdentidad.CalidadMigratoriaid;

            SqlParameter par5 = cmd.Parameters.Add("@P_CARGO", SqlDbType.SmallInt);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametrosCarneIdentidad.CalidadMigratoriaSecId;

            SqlParameter par17 = cmd.Parameters.Add("@P_CAT_OFICINA", SqlDbType.SmallInt);
            par17.Direction = ParameterDirection.Input;
            par17.Value = parametrosCarneIdentidad.CatMision;

            SqlParameter par6 = cmd.Parameters.Add("@P_OFICINA_CON_EX", SqlDbType.SmallInt);
            par6.Direction = ParameterDirection.Input;
            par6.Value = parametrosCarneIdentidad.OficinaConsularExid;

            SqlParameter par7 = cmd.Parameters.Add("@P_FECHA_EMISION", SqlDbType.DateTime);
            par7.Direction = ParameterDirection.Input;
            par7.Value = parametrosCarneIdentidad.FechaEmision;

            SqlParameter par8 = cmd.Parameters.Add("@P_FECHA_VENC", SqlDbType.DateTime);
            par8.Direction = ParameterDirection.Input;
            par8.Value = parametrosCarneIdentidad.FechaVencimiento;

            SqlParameter par9 = cmd.Parameters.Add("@P_APELLIDO_PAT", SqlDbType.VarChar, 100);
            par9.Direction = ParameterDirection.Input;
            par9.Value = parametrosCarneIdentidad.ApePatPersona;

            SqlParameter par10 = cmd.Parameters.Add("@P_APELLIDO_MAT", SqlDbType.VarChar, 100);
            par10.Direction = ParameterDirection.Input;
            par10.Value = parametrosCarneIdentidad.ApeMatPersona;

            SqlParameter par11 = cmd.Parameters.Add("@P_NOMBRES", SqlDbType.VarChar, 100);
            par11.Direction = ParameterDirection.Input;
            par11.Value = parametrosCarneIdentidad.NombresPersona;

            SqlParameter par12 = cmd.Parameters.Add("@P_PAIS", SqlDbType.SmallInt);
            par12.Direction = ParameterDirection.Input;
            par12.Value = parametrosCarneIdentidad.PaisPersona;

            SqlParameter par13 = cmd.Parameters.Add("@P_NUM_PAG", SqlDbType.BigInt);
            par13.Direction = ParameterDirection.Input;
            par13.Value = parametrosCarneIdentidad.NumPag;

            SqlParameter par14 = cmd.Parameters.Add("@P_CANT_REG_PAG", SqlDbType.BigInt);
            par14.Direction = ParameterDirection.Input;
            par14.Value = parametrosCarneIdentidad.CantReg;
            #endregion
            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                List<beCarneIdentidad> lbeCarneIdentidad = new List<beCarneIdentidad>();
                int posCarneIdentidadid = drd.GetOrdinal("CARDIP_ID");
                int posMesaPartes = drd.GetOrdinal("MESA_PARTES");
                int posConIdent = drd.GetOrdinal("NUMERO_IDENT");
                int posCarneNumero = drd.GetOrdinal("NUMERO_CARNE");
                int posConEstado = drd.GetOrdinal("ESTADO");
                int posConFuncionario = drd.GetOrdinal("FUNCIONARIO");
                int posConPaisNacionalidad = drd.GetOrdinal("PAIS_NACIONALIDAD");
                int posConCalidadMigratoria = drd.GetOrdinal("CALIDAD_MIGRATORIA");
                int posConCargo = drd.GetOrdinal("CARGO");
                int posConOficinaConsularEx = drd.GetOrdinal("OFICINA_CONSULAR_EXTRANJERA");
                int posConFechaInscripcion = drd.GetOrdinal("FECHA_INSCRIPCION");
                int posConFechaEmision = drd.GetOrdinal("FECHA_EMISION");
                int posConFechaVen = drd.GetOrdinal("FECHA_VENCIMIENTO");
                int posRutaArchivoFoto = drd.GetOrdinal("RUTA_ARCHIVO");
                int posRUTA_ARCHIVO_FIRMA = drd.GetOrdinal("RUTA_ARCHIVO_FIRMA");
                int posCALMIG_ID = drd.GetOrdinal("CALMIG_ID");
                int posCALMIGSEC_ID = drd.GetOrdinal("CALMIGSEC_ID");
                int posOFCOEX_ID = drd.GetOrdinal("OFCOEX_ID");
                int posRENOVAR = drd.GetOrdinal("RENOVAR");
                // ULTIMOS
                int posFUNC_APEP = drd.GetOrdinal("FUNC_APEP");
                int posFUNC_APEM = drd.GetOrdinal("FUNC_APEM");
                int posFUNC_NOMBRES = drd.GetOrdinal("FUNC_NOMBRES");
                int posFUNC_FECNAC = drd.GetOrdinal("FUNC_FECNAC");
                int posFUNC_GENERO = drd.GetOrdinal("FUNC_GENERO");
                int posOFCOEX_CATEGORIA = drd.GetOrdinal("OFCOEX_CATEGORIA");
                int posESTCIVIL_DESC = drd.GetOrdinal("ESTCIVIL_DESC");
                beCarneIdentidad obeCarneIdentidad;
                while (drd.Read())
                {
                    obeCarneIdentidad = new beCarneIdentidad();
                    obeCarneIdentidad.CarneIdentidadid = drd.GetInt16(posCarneIdentidadid);
                    obeCarneIdentidad.IdentMesaPartes = drd.GetString(posMesaPartes);
                    obeCarneIdentidad.ConIdent = drd.GetString(posConIdent);
                    obeCarneIdentidad.CarneNumero = drd.GetString(posCarneNumero);
                    obeCarneIdentidad.ConEstado = drd.GetString(posConEstado);
                    obeCarneIdentidad.ConFuncionario = drd.GetString(posConFuncionario);
                    obeCarneIdentidad.ConPaisNacionalidad = drd.GetString(posConPaisNacionalidad);
                    obeCarneIdentidad.ConCalidadMigratoria = drd.GetString(posConCalidadMigratoria);
                    obeCarneIdentidad.ConCargo = drd.GetString(posConCargo);
                    obeCarneIdentidad.ConOficinaConsularEx = drd.GetString(posConOficinaConsularEx);
                    obeCarneIdentidad.ConFechaInscripcion = drd.GetString(posConFechaInscripcion);
                    obeCarneIdentidad.ConFechaEmision = drd.GetString(posConFechaEmision);
                    obeCarneIdentidad.ConFechaVen = drd.GetString(posConFechaVen);
                    obeCarneIdentidad.RutaArchivoFoto = drd.GetString(posRutaArchivoFoto);
                    if (!drd.IsDBNull(posRUTA_ARCHIVO_FIRMA)) { obeCarneIdentidad.RutaArchivoFirma = drd.GetString(posRUTA_ARCHIVO_FIRMA); }
                    else { obeCarneIdentidad.RutaArchivoFirma = "error"; }
                    obeCarneIdentidad.CalidadMigratoriaid = drd.GetInt16(posCALMIG_ID);
                    obeCarneIdentidad.CalidadMigratoriaSecId = drd.GetInt16(posCALMIGSEC_ID);
                    obeCarneIdentidad.OficinaConsularExid = drd.GetInt16(posOFCOEX_ID);
                    obeCarneIdentidad.Renovar = drd.GetBoolean(posRENOVAR);
                    obeCarneIdentidad.ApePatPersona = drd.GetString(posFUNC_APEP);
                    obeCarneIdentidad.ApeMatPersona = drd.GetString(posFUNC_APEM);
                    obeCarneIdentidad.NombresPersona = drd.GetString(posFUNC_NOMBRES);
                    obeCarneIdentidad.ConFechaNac = drd.GetString(posFUNC_FECNAC);
                    obeCarneIdentidad.ConGenero = drd.GetString(posFUNC_GENERO);
                    obeCarneIdentidad.ConCatMision = drd.GetString(posOFCOEX_CATEGORIA);
                    obeCarneIdentidad.ConEstCivil = drd.GetString(posESTCIVIL_DESC);
                    lbeCarneIdentidad.Add(obeCarneIdentidad);
                }
                obeCarneIdentidadPrincipal.ListaConsulta = lbeCarneIdentidad;
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
                        obeCarneIdentidadPrincipal.Paginacion = obePaginacion;
                    }
                }
                drd.Close();
            }
            return (obeCarneIdentidadPrincipal);
        }

        public beCarneIdentidadPrincipal consultarRegistroEdicion(SqlConnection con, beCarneIdentidad parametrosCarneIdentidad)
        {
            beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CARNE_IDENTIDAD_CONSULTAR_REGISTRO_EDICION", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_CARNE_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosCarneIdentidad.CarneIdentidadid;

            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                int posMESA_PARTES = drd.GetOrdinal("MESA_PARTES");
                int posNUMERO_IDENT = drd.GetOrdinal("NUMERO_IDENT");
                int posPERSONA_ID = drd.GetOrdinal("PERSONA_ID");
                int posPERSONA_APE_PAT = drd.GetOrdinal("PERSONA_APE_PAT");
                int posPERSONA_APE_MAT = drd.GetOrdinal("PERSONA_APE_MAT");
                int posPERSONA_NOMBRES = drd.GetOrdinal("PERSONA_NOMBRES");
                int posTELEFONO = drd.GetOrdinal("TELEFONO");
                int posPERSONA_FECNAC = drd.GetOrdinal("PERSONA_FECNAC");
                int posPERSONA_ESTCIVIL_ID = drd.GetOrdinal("PERSONA_ESTCIVIL_ID");
                int posPERSONA_GENERO_ID = drd.GetOrdinal("PERSONA_GENERO_ID");
                int posPERSONA_MENOR_EDAD = drd.GetOrdinal("PERSONA_MENOR_EDAD");
                int posPERIDENT_ID = drd.GetOrdinal("PERIDENT_ID");
                int posPERIDENT_TIPODOC_ID = drd.GetOrdinal("PERIDENT_TIPODOC_ID");
                int posPERIDENT_NUMDOCIDENT = drd.GetOrdinal("PERIDENT_NUMDOCIDENT");
                int posPAIS_ID = drd.GetOrdinal("PAIS_ID");
                int posPAIS_NACIONALIDAD = drd.GetOrdinal("PAIS_NACIONALIDAD");
                int posPERES_ID = drd.GetOrdinal("PERES_ID");
                int posPERES_RESI_ID = drd.GetOrdinal("PERES_RESI_ID");
                int posRESI_DIRECCION = drd.GetOrdinal("RESI_DIRECCION");
                int posRESI_UBIGEO_DEPARTAMENTO = drd.GetOrdinal("UBIGEO_DEPARTAMENTO");
                int posRESI_UBIGEO_PROVINCIA = drd.GetOrdinal("UBIGEO_PROVINCIA");
                int posRESI_UBIGEO_DISTRITO = drd.GetOrdinal("UBIGEO_DISTRITO");
                int posCALMIG_PRI = drd.GetOrdinal("CALMIG_PRI");
                int posCALMIGSEC_SEC = drd.GetOrdinal("CALMIGSEC_SEC");
                int posCALMIGSEC_TITDEP = drd.GetOrdinal("CALMIGSEC_TITDEP");
                int posOFCOEX_ID = drd.GetOrdinal("OFCOEX_ID");
                int posOFCOEX_CATEGORIA = drd.GetOrdinal("OFCOEX_CATEGORIA");
                int posCARDIP_ID = drd.GetOrdinal("CARDIP_ID");
                int posCARDIP_RUTA_ARCHIVO = drd.GetOrdinal("CARDIP_RUTA_ARCHIVO");
                int posCARDIP_RUTA_ARCHIVO_FIRMA = drd.GetOrdinal("CARDIP_RUTA_ARCHIVO_FIRMA");
                int posCARDIP_NUMERO_CARNE = drd.GetOrdinal("CARDIP_NUMERO_CARNE");
                int posCARDIP_FECHA_EMISION = drd.GetOrdinal("CARDIP_FECHA_EMISION");
                int posCARDIP_FECHA_VENC = drd.GetOrdinal("CARDIP_FECHA_VENC");
                int posRENOVAR = drd.GetOrdinal("RENOVAR");
                int posFLAG_CONTROL_CALIDAD = drd.GetOrdinal("FLAG_CONTROL_CALIDAD");
                int posCONTROL_CALIDAD = drd.GetOrdinal("CONTROL_CALIDAD");
                int posCONTROL_CALIDAD_DETALLE = drd.GetOrdinal("CONTROL_CALIDAD_DETALLE");
                int posREGPREV_FECHACON = drd.GetOrdinal("REGPREV_FECHACON");
                int posREGPREV_FECHAPRI = drd.GetOrdinal("REGPREV_FECHAPRI");
                int posREGPREV_ID = drd.GetOrdinal("REGPREV_ID");
                int posCARDIP_TIPO_ENTRADA = drd.GetOrdinal("CARDIP_TIPO_ENTRADA");
                beCarneIdentidad CarneIdentidad;
                bePersona Persona;
                bePersonaidentificacion PersonaIdentificacion;
                beResidencia Residencia;
                bePersonaresidencia PersonaResidencia;
                bePais Pais;
                beUbicaciongeografica UbiGeo;
                beOficinaconsularExtranjera OficinaConEx;
                beCalidadMigratoria CalidadMigratoriaPri;
                beCalidadMigratoria CalidadMigratoriaSec;
                beRegistroPrevio obeRegistroPrevio;
                if (drd.HasRows)
                {
                    CarneIdentidad = new beCarneIdentidad();
                    Persona = new bePersona();
                    PersonaIdentificacion = new bePersonaidentificacion();
                    Residencia = new beResidencia();
                    PersonaResidencia = new bePersonaresidencia();
                    Pais = new bePais();
                    UbiGeo = new beUbicaciongeografica();
                    OficinaConEx = new beOficinaconsularExtranjera();
                    CalidadMigratoriaPri = new beCalidadMigratoria();
                    CalidadMigratoriaSec = new beCalidadMigratoria();
                    obeRegistroPrevio = new beRegistroPrevio();
                    drd.Read();
                    // PERSONA ---------------------------------------------------------------------------------
                    Persona.Personaid = drd.GetInt64(posPERSONA_ID);
                    Persona.Apellidopaterno = drd.GetString(posPERSONA_APE_PAT);
                    Persona.Apellidomaterno = (!drd.IsDBNull(posPERSONA_APE_MAT) ? drd.GetString(posPERSONA_APE_MAT) : "");
                    Persona.Nombres = drd.GetString(posPERSONA_NOMBRES);
                    Persona.Telefono = (!drd.IsDBNull(posTELEFONO) ? drd.GetString(posTELEFONO) : "");
                    Persona.Nacimientofecha = drd.GetDateTime(posPERSONA_FECNAC);
                    Persona.Estadocivilid = drd.GetInt16(posPERSONA_ESTCIVIL_ID);
                    Persona.Generoid = drd.GetInt16(posPERSONA_GENERO_ID);
                    Persona.MenorEdad = drd.GetBoolean(posPERSONA_MENOR_EDAD);
                    // PERSONA IDENTIFICACION ---------------------------------------------------------------------------------
                    PersonaIdentificacion.Personaidentificacionid = drd.GetInt64(posPERIDENT_ID);
                    PersonaIdentificacion.Documentotipoid = drd.GetInt16(posPERIDENT_TIPODOC_ID);
                    PersonaIdentificacion.Documentonumero = drd.GetString(posPERIDENT_NUMDOCIDENT);
                    // PAIS ---------------------------------------------------------------------------------
                    Pais.Paisid = drd.GetInt16(posPAIS_ID);
                    Pais.Nacionalidad = drd.GetString(posPAIS_NACIONALIDAD);
                    //PERSONA RESIDENCIA ---------------------------------------------------------------------------------
                    PersonaResidencia.PersonaResidenciaId = drd.GetInt64(posPERES_ID);
                    PersonaResidencia.Residenciaid = drd.GetInt64(posPERES_RESI_ID);
                    // RESIDENCIA ---------------------------------------------------------------------------------
                    Residencia.Residenciadireccion = drd.GetString(posRESI_DIRECCION);
                    // UBIGEO ---------------------------------------------------------------------------------
                    UbiGeo.Ubi01 = drd.GetString(posRESI_UBIGEO_DEPARTAMENTO);
                    UbiGeo.Ubi02 = drd.GetString(posRESI_UBIGEO_PROVINCIA);
                    UbiGeo.Ubi03 = drd.GetString(posRESI_UBIGEO_DISTRITO);
                    // CARDIP ---------------------------------------------------------------------------------
                    if (!drd.IsDBNull(posMESA_PARTES)) { CarneIdentidad.IdentMesaPartes = drd.GetString(posMESA_PARTES); }
                    else { CarneIdentidad.IdentMesaPartes = ""; }
                    CarneIdentidad.ConIdent = drd.GetString(posNUMERO_IDENT);
                    CarneIdentidad.CarneIdentidadid = drd.GetInt16(posCARDIP_ID);
                    CarneIdentidad.RutaArchivoFoto = drd.GetString(posCARDIP_RUTA_ARCHIVO);
                    if (!drd.IsDBNull(posCARDIP_RUTA_ARCHIVO_FIRMA)) { CarneIdentidad.RutaArchivoFirma = drd.GetString(posCARDIP_RUTA_ARCHIVO_FIRMA); }
                    else { CarneIdentidad.RutaArchivoFirma = "error"; }
                    if (!drd.IsDBNull(posCARDIP_NUMERO_CARNE)) { CarneIdentidad.CarneNumero = drd.GetString(posCARDIP_NUMERO_CARNE); }
                    else { CarneIdentidad.CarneNumero = "[ NO DEFINIDO ]"; }
                    if (!drd.IsDBNull(posCARDIP_FECHA_EMISION)) { CarneIdentidad.FechaEmision = drd.GetDateTime(posCARDIP_FECHA_EMISION); }
                    if (!drd.IsDBNull(posCARDIP_FECHA_VENC)) { CarneIdentidad.FechaVencimiento = drd.GetDateTime(posCARDIP_FECHA_VENC); }
                    CarneIdentidad.Renovar = drd.GetBoolean(posRENOVAR);
                    CarneIdentidad.FlagControlCalidad = drd.GetBoolean(posFLAG_CONTROL_CALIDAD);
                    CarneIdentidad.ControlCalidad = drd.GetString(posCONTROL_CALIDAD);
                    CarneIdentidad.ControlCalidadDetalle = drd.GetString(posCONTROL_CALIDAD_DETALLE);
                    //CALIDAD MIGRATORIA
                    CalidadMigratoriaPri.CalidadMigratoriaid = drd.GetInt16(posCALMIG_PRI);
                    CalidadMigratoriaSec.CalidadMigratoriaid = drd.GetInt16(posCALMIGSEC_SEC);
                    CalidadMigratoriaSec.FlagTitularDependiente = drd.GetInt16(posCALMIGSEC_TITDEP);
                    // OFCOEX ---------------------------------------------------------------------------------
                    OficinaConEx.OficinaconsularExtranjeraid = drd.GetInt16(posOFCOEX_ID);
                    OficinaConEx.Categoriaid = drd.GetInt16(posOFCOEX_CATEGORIA);
                    // REGISTRO PREVIO
                    if (!drd.IsDBNull(posREGPREV_FECHACON)) { obeRegistroPrevio.FechaConsulares = drd.GetDateTime(posREGPREV_FECHACON); }
                    if (!drd.IsDBNull(posREGPREV_FECHAPRI)) { obeRegistroPrevio.FechaPrivilegios = drd.GetDateTime(posREGPREV_FECHAPRI); }
                    obeRegistroPrevio.RegistroPrevioId = drd.GetInt16(posREGPREV_ID);
                    obeRegistroPrevio.TipoEntrada = drd.GetInt16(posCARDIP_TIPO_ENTRADA);
                    // ----------------------------------------------------------------------------------------
                    obeCarneIdentidadPrincipal.CarneIdentidad = CarneIdentidad;
                    obeCarneIdentidadPrincipal.Persona = Persona;
                    obeCarneIdentidadPrincipal.PersonaIdentificacion = PersonaIdentificacion;
                    obeCarneIdentidadPrincipal.Residencia = Residencia;
                    obeCarneIdentidadPrincipal.PersonaResidencia = PersonaResidencia;
                    obeCarneIdentidadPrincipal.Pais = Pais;
                    obeCarneIdentidadPrincipal.UbiGeo = UbiGeo;
                    obeCarneIdentidadPrincipal.OficinaConsularExtranjera = OficinaConEx;
                    obeCarneIdentidadPrincipal.CalidadMigratoriaPri = CalidadMigratoriaPri;
                    obeCarneIdentidadPrincipal.CalidadMigratoriaSec = CalidadMigratoriaSec;
                    obeCarneIdentidadPrincipal.RegistroPrevioEdicion = obeRegistroPrevio;
                    if (drd.NextResult())
                    {
                        drd.Read();
                        beCarneIdentidadRelacionDependencia obeCarneIdentidadRelacionDependencia;
                        int posTIT_DEP_ID = drd.GetOrdinal("TIT_DEP_ID");
                        int posTIT_ID = drd.GetOrdinal("TIT_ID");
                        int posDEP_ID = drd.GetOrdinal("DEP_ID");
                        if (drd.HasRows)
                        {
                            obeCarneIdentidadRelacionDependencia = new beCarneIdentidadRelacionDependencia();
                            obeCarneIdentidadRelacionDependencia.TitularDependienteId = drd.GetInt16(posTIT_DEP_ID);
                            obeCarneIdentidadRelacionDependencia.CarneIdentidadTitId = drd.GetInt16(posTIT_ID);
                            obeCarneIdentidadRelacionDependencia.CarneIdentidadDepId = drd.GetInt16(posDEP_ID);
                            obeCarneIdentidadPrincipal.RelacionDependencia = obeCarneIdentidadRelacionDependencia;
                            if (drd.NextResult())
                            {
                                drd.Read();
                                beRegistroLinea obeRegistroLinea;
                                int posREGLINEA_ID = drd.GetOrdinal("REGLINEA_ID");
                                int posREGLINEA_NUMERO = drd.GetOrdinal("REGLINEA_NUMERO");
                                int posTIPO_EMISION = drd.GetOrdinal("TIPO_EMISION");
                                if (drd.HasRows)
                                {
                                    obeRegistroLinea = new beRegistroLinea();
                                    obeRegistroLinea.RegistroLineaId = drd.GetInt32(posREGLINEA_ID);
                                    obeRegistroLinea.NumeroRegLinea = drd.GetString(posREGLINEA_NUMERO);
                                    obeRegistroLinea.ConTipoEmision = drd.GetString(posTIPO_EMISION);
                                    obeCarneIdentidadPrincipal.RegistroLinea = obeRegistroLinea;
                                }
                            }
                        }
                    }
                }
                drd.Close();
            }
            return (obeCarneIdentidadPrincipal);
        }

        public beConsultaExterna consultaExternaDatos(SqlConnection con, SqlTransaction trx, beConsultaExterna parametrosConsultaExterna)
        {
            beConsultaExterna obeConsultaExterna = new beConsultaExterna();
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CARNE_IDENTIDAD_CONSULTA_EXTERNA_UPDATE", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_NUMERO_CARNE", SqlDbType.VarChar, 8);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosConsultaExterna.CarneNumero;

            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                int posCARNE_NUMERO = drd.GetOrdinal("CARNE_NUMERO");
                int posFECHA_NAC = drd.GetOrdinal("FECHA_NAC");
                int posAPE_PAT = drd.GetOrdinal("APE_PAT");
                int posAPE_MAT = drd.GetOrdinal("APE_MAT");
                int posNOMBRES = drd.GetOrdinal("NOMBRES");
                int posPAIS_NACI = drd.GetOrdinal("PAIS_NACI");
                int posFECHA_VEN = drd.GetOrdinal("FECHA_VEN");
                int posRUTA_ARCHIVO = drd.GetOrdinal("RUTA_ARCHIVO");
                if (drd.HasRows)
                {
                    drd.Read();
                    obeConsultaExterna.CarneNumero = drd.GetString(posCARNE_NUMERO);
                    obeConsultaExterna.FechaNacimiento = drd.GetString(posFECHA_NAC);
                    obeConsultaExterna.ApePat = drd.GetString(posAPE_PAT);
                    obeConsultaExterna.APeMat = drd.GetString(posAPE_MAT);
                    obeConsultaExterna.Nombres = drd.GetString(posNOMBRES);
                    obeConsultaExterna.Nacionalidad = drd.GetString(posPAIS_NACI);
                    obeConsultaExterna.FechaVencimiento = drd.GetString(posFECHA_VEN);
                    obeConsultaExterna.RutaArchivo = drd.GetString(posRUTA_ARCHIVO);
                }
                drd.Close();
            }
            return(obeConsultaExterna);
        }

        public int consultaExternaAdicionar(SqlConnection con, SqlTransaction trx, beConsultaExterna parametrosConsultaExterna)
        {
            int ConsultaId = -1;
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CONSULTA_ADICIONAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_NUMERO_CARNE", SqlDbType.VarChar, 8);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosConsultaExterna.CarneNumero;

            SqlParameter par2 = cmd.Parameters.Add("@P_FECHA_NAC", SqlDbType.VarChar, 10);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametrosConsultaExterna.FechaNacimiento;

            SqlParameter par9 = cmd.Parameters.Add("@P_APE_PAT", SqlDbType.VarChar, 50);
            par9.Direction = ParameterDirection.Input;
            par9.Value = parametrosConsultaExterna.ApePat;

            SqlParameter par3 = cmd.Parameters.Add("@P_APE_MAT", SqlDbType.VarChar, 50);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametrosConsultaExterna.APeMat;

            SqlParameter par4 = cmd.Parameters.Add("@P_NOMBRES", SqlDbType.VarChar, 250);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametrosConsultaExterna.Nombres;

            SqlParameter par5 = cmd.Parameters.Add("@P_PAIS_NACI", SqlDbType.VarChar, 150);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametrosConsultaExterna.Nacionalidad;

            SqlParameter par6 = cmd.Parameters.Add("@P_FECHA_VEN", SqlDbType.VarChar, 10);
            par6.Direction = ParameterDirection.Input;
            par6.Value = parametrosConsultaExterna.FechaVencimiento;

            SqlParameter par7 = cmd.Parameters.Add("@P_RUTA_ARCHIVO", SqlDbType.VarChar, 250);
            par7.Direction = ParameterDirection.Input;
            par7.Value = parametrosConsultaExterna.RutaArchivo;

            SqlParameter par8 = cmd.Parameters.Add("@@IDENTITY", SqlDbType.Int);
            par8.Direction = ParameterDirection.ReturnValue;

            int n = cmd.ExecuteNonQuery();
            if (n > 0) ConsultaId = Convert.ToInt32(par8.Value);
            return (ConsultaId);
        }

        public bool consultaExternaAnular(SqlConnection con, SqlTransaction trx, beConsultaExterna parametrosConsultaExterna)
        {
            bool exito = false;
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CONSULTA_ANULAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_NUMERO_CARNE", SqlDbType.VarChar, 8);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosConsultaExterna.CarneNumero;

            int n = cmd.ExecuteNonQuery();
            if (n != 0)
            {
                exito = true;
            }
            return (exito);
        }

        public beConsultaExterna validar(SqlConnection con, SqlTransaction trx, beConsultaExterna parametrosConsulta)
        {
            beConsultaExterna obeConsultaExterna = new beConsultaExterna();
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CONSULTA_EXTERNA", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_NUMERO_CARNE", SqlDbType.VarChar, 8);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametrosConsulta.CarneNumero;

            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                int posCARNE_NUMERO = drd.GetOrdinal("CARNE_NUMERO");
                int posFECHA_NAC = drd.GetOrdinal("FECHA_NAC");
                int posAPE_PAT = drd.GetOrdinal("APE_PAT");
                int posAPE_MAT = drd.GetOrdinal("APE_MAT");
                int posNOMBRES = drd.GetOrdinal("NOMBRES");
                int posPAIS_NACI = drd.GetOrdinal("PAIS_NACI");
                int posFECHA_VEN = drd.GetOrdinal("FECHA_VEN");
                int posRUTA_ARCHIVO = drd.GetOrdinal("RUTA_ARCHIVO");
                if (drd.HasRows)
                {
                    drd.Read();
                    obeConsultaExterna.CarneNumero = drd.GetString(posCARNE_NUMERO);
                    //obeConsultaExterna.FechaNacimiento = drd.GetString(posFECHA_NAC);
                    //obeConsultaExterna.ApePat = drd.GetString(posAPE_PAT);
                    //obeConsultaExterna.APeMat = drd.GetString(posAPE_MAT);
                    //obeConsultaExterna.Nombres = drd.GetString(posNOMBRES);
                    //obeConsultaExterna.Nacionalidad = drd.GetString(posPAIS_NACI);
                    //obeConsultaExterna.FechaVencimiento = drd.GetString(posFECHA_VEN);
                    //obeConsultaExterna.RutaArchivo = drd.GetString(posRUTA_ARCHIVO);
                }
                drd.Close();
            }
            return (obeConsultaExterna);
        }

        public string obtenerIdent(SqlConnection con, short parametro)
        {
            string Ident = "error";
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CARNE_IDENTIDAD_CONSULTAR_IDENT", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_CARNE_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametro;

            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                int posIDENT = drd.GetOrdinal("IDENT");
                if (drd.HasRows)
                {
                    drd.Read();
                    Ident = drd.GetString(posIDENT);
                }
                drd.Close();
            }
            return (Ident);
        }

        public bool derivar(SqlConnection con, SqlTransaction trx, beCarneIdentidad parametros)
        {
            bool exito = false;
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CARNE_IDENTIDAD_DERIVAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_CARNE_IDENTIDAD_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.CarneIdentidadid;

            SqlParameter par2 = cmd.Parameters.Add("@P_ESTADO_ID", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.Estadoid;

            SqlParameter par3 = cmd.Parameters.Add("@P_REGISTRADOR_ID", SqlDbType.SmallInt);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros.UsuarioDeriva;

            SqlParameter par4 = cmd.Parameters.Add("@P_USUARIO_MODIFICACION", SqlDbType.SmallInt);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametros.Usuariomodificacion;

            SqlParameter par5 = cmd.Parameters.Add("@P_IP_MODIFICACION", SqlDbType.VarChar, 50);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametros.Ipmodificacion;

            int n = cmd.ExecuteNonQuery();
            if (n != 0)
            {
                exito = true;
            }
            return (exito);
        }

        public bool derivarRenovacion(SqlConnection con, SqlTransaction trx, beCarneIdentidad parametros)
        {
            bool exito = false;
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CARNE_IDENTIDAD_DERIVAR_PARA_RENOVACION", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par1 = cmd.Parameters.Add("@P_CARNE_IDENTIDAD_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.CarneIdentidadid;

            SqlParameter par2 = cmd.Parameters.Add("@P_ESTADO_ID", SqlDbType.SmallInt);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.Estadoid;

            SqlParameter par3 = cmd.Parameters.Add("@P_REGISTRADOR_ID", SqlDbType.SmallInt);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros.UsuarioDeriva;

            SqlParameter par4 = cmd.Parameters.Add("@P_USUARIO_MODIFICACION", SqlDbType.SmallInt);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametros.Usuariomodificacion;

            SqlParameter par5 = cmd.Parameters.Add("@P_IP_MODIFICACION", SqlDbType.VarChar, 50);
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
