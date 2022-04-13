using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP_REGLINEA.Librerias.EntidadesNegocio;

namespace SolCARDIP_REGLINEA.Librerias.AccesoDatos
{
    public class daRegistroLinea
    {
        public int adicionar(SqlConnection con, SqlTransaction trx, beRegistroLinea parametros)
		{
			int idRegistroLinea = -1;

			SqlCommand cmd = new SqlCommand("SC_REGLINEA.USP_RL_REGISTRO_LINEA_ADICIONAR", con);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Transaction = trx;

            #region comentados
            //SqlParameter par2 = cmd.Parameters.Add("@P_RELI_ICARNE_IDENTIDAD_ID", SqlDbType.SmallInt);
            //par2.Direction = ParameterDirection.Input;
            //par2.Value = parametros.CarneIdentidadId;

            //SqlParameter par5 = cmd.Parameters.Add("@P_RELI_STIPO_EMISION", SqlDbType.SmallInt);
            //par5.Direction = ParameterDirection.Input;
            //par5.Value = parametros.TipoEmision;
            
            //SqlParameter par6 = cmd.Parameters.Add("@P_RELI_SDP_RELDEP_TITDEP", SqlDbType.SmallInt);
            //par6.Direction = ParameterDirection.Input;
            //par6.Value = parametros.DpReldepTitdep;
            
            //SqlParameter par7 = cmd.Parameters.Add("@P_RELI_SDP_RELDEP_TITULAR", SqlDbType.SmallInt);
            //par7.Direction = ParameterDirection.Input;
            //par7.Value = parametros.DpReldepTitular;
            
            //SqlParameter par8 = cmd.Parameters.Add("@P_RELI_VDP_PRIMER_APELLIDO", SqlDbType.VarChar, 100);
            //par8.Direction = ParameterDirection.Input;
            //par8.Value = parametros.DpPrimerApellido;
            
            //SqlParameter par9 = cmd.Parameters.Add("@P_RELI_VDP_SEGUNDO_APELLIDO", SqlDbType.VarChar, 100);
            //par9.Direction = ParameterDirection.Input;
            //par9.Value = parametros.DpSegundoApellido;
            
            //SqlParameter par10 = cmd.Parameters.Add("@P_RELI_VDP_NOMBRES", SqlDbType.VarChar, 100);
            //par10.Direction = ParameterDirection.Input;
            //par10.Value = parametros.DpNombres;
            
            //SqlParameter par11 = cmd.Parameters.Add("@P_RELI_DDP_FECHA_NACIMIENTO", SqlDbType.DateTime);
            //par11.Direction = ParameterDirection.Input;
            //par11.Value = parametros.DpFechaNacimiento;
            
            //SqlParameter par12 = cmd.Parameters.Add("@P_RELI_SDP_GENERO_ID", SqlDbType.SmallInt);
            //par12.Direction = ParameterDirection.Input;
            //par12.Value = parametros.DpGeneroId;
            
            //SqlParameter par13 = cmd.Parameters.Add("@P_RELI_SDP_ESTADO_CIVIL_ID", SqlDbType.SmallInt);
            //par13.Direction = ParameterDirection.Input;
            //par13.Value = parametros.DpEstadoCivilId;
            
            //SqlParameter par14 = cmd.Parameters.Add("@P_RELI_SDP_TIPO_DOC_IDENTIDAD", SqlDbType.SmallInt);
            //par14.Direction = ParameterDirection.Input;
            //par14.Value = parametros.DpTipoDocIdentidad;
            
            //SqlParameter par15 = cmd.Parameters.Add("@P_RELI_VDP_NUMERO_DOC_IDENTIDAD", SqlDbType.VarChar, 20);
            //par15.Direction = ParameterDirection.Input;
            //par15.Value = parametros.DpNumeroDocIdentidad;
            
            //SqlParameter par16 = cmd.Parameters.Add("@P_RELI_SDP_PAIS_NACIONALIDAD", SqlDbType.SmallInt);
            //par16.Direction = ParameterDirection.Input;
            //par16.Value = parametros.DpPaisNacionalidad;
            
            //SqlParameter par17 = cmd.Parameters.Add("@P_RELI_VDP_DOMICILIO_PERU", SqlDbType.VarChar, 500);
            //par17.Direction = ParameterDirection.Input;
            //par17.Value = parametros.DpDomicilioPeru;
            
            //SqlParameter par18 = cmd.Parameters.Add("@P_RELI_CDP_UBIGEO", SqlDbType.Char, 6);
            //par18.Direction = ParameterDirection.Input;
            //par18.Value = parametros.DpUbigeo;
            
            //SqlParameter par19 = cmd.Parameters.Add("@P_RELI_VDP_CORREO_ELECTRONICO", SqlDbType.VarChar, 50);
            //par19.Direction = ParameterDirection.Input;
            //par19.Value = parametros.DpCorreoElectronico;
            
            //SqlParameter par20 = cmd.Parameters.Add("@P_RELI_VDP_NUMERO_TELEFONO", SqlDbType.VarChar, 50);
            //par20.Direction = ParameterDirection.Input;
            //par20.Value = parametros.DpNumeroTelefono;
            
            //SqlParameter par21 = cmd.Parameters.Add("@P_RELI_VDP_RUTA_ADJUNTO", SqlDbType.VarChar, 250);
            //par21.Direction = ParameterDirection.Input;
            //par21.Value = parametros.DpRutaAdjunto;
            
            //SqlParameter par22 = cmd.Parameters.Add("@P_RELI_SIN_TIPO_INSTITUCION", SqlDbType.SmallInt);
            //par22.Direction = ParameterDirection.Input;
            //par22.Value = parametros.InTipoInstitucion;
            
            //SqlParameter par23 = cmd.Parameters.Add("@P_RELI_VIN_NOMBRE_INSTITUCION", SqlDbType.VarChar, 250);
            //par23.Direction = ParameterDirection.Input;
            //par23.Value = parametros.InNombreInstitucion;
            
            //SqlParameter par24 = cmd.Parameters.Add("@P_RELI_VIN_PERSONA_CONTACTO", SqlDbType.VarChar, 250);
            //par24.Direction = ParameterDirection.Input;
            //par24.Value = parametros.InPersonaContacto;
            
            //SqlParameter par25 = cmd.Parameters.Add("@P_RELI_VIN_CARGO_CONTACTO", SqlDbType.VarChar, 250);
            //par25.Direction = ParameterDirection.Input;
            //par25.Value = parametros.InCargoContacto;
            
            //SqlParameter par26 = cmd.Parameters.Add("@P_RELI_VIN_CORREO_ELECTRONICO", SqlDbType.VarChar, 50);
            //par26.Direction = ParameterDirection.Input;
            //par26.Value = parametros.InCorreoElectronico;
            
            //SqlParameter par27 = cmd.Parameters.Add("@P_RELI_VIN_NUMERO_TELEFONO", SqlDbType.VarChar, 50);
            //par27.Direction = ParameterDirection.Input;
            //par27.Value = parametros.InNumeroTelefono;
            
            //SqlParameter par28 = cmd.Parameters.Add("@P_RELI_SIN_TIPO_CARGO", SqlDbType.SmallInt);
            //par28.Direction = ParameterDirection.Input;
            //par28.Value = parametros.InTipoCargo;
            
            //SqlParameter par29 = cmd.Parameters.Add("@P_RELI_VIN_CARGO_NOMBRE", SqlDbType.VarChar, 50);
            //par29.Direction = ParameterDirection.Input;
            //par29.Value = parametros.InCargoNombre;

            //SqlParameter par31 = cmd.Parameters.Add("@P_RELI_SUSUARIO_CREACION", SqlDbType.SmallInt);
            //par31.Direction = ParameterDirection.Input;
            //par31.Value = parametros.UsuarioCreacion;
            #endregion

            SqlParameter par32 = cmd.Parameters.Add("@P_RELI_VIP_CREACION", SqlDbType.VarChar, 50);
			par32.Direction = ParameterDirection.Input;
			par32.Value = parametros.IpCreacion;

			SqlParameter par37 = cmd.Parameters.Add("@@identity", SqlDbType.Int);
			par37.Direction = ParameterDirection.ReturnValue;

			int n = cmd.ExecuteNonQuery();
			if (n > 0) n = idRegistroLinea = (int)par37.Value;
			return (idRegistroLinea);
		}

        public bool actualizar(SqlConnection con, SqlTransaction trx, beRegistroLinea parametros)
        {
            bool exito = false;
            SqlCommand cmd = new SqlCommand("SC_REGLINEA.USP_RL_REGISTRO_LINEA_ACTUALIZAR", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par0 = cmd.Parameters.Add("@P_RELI_IREGISTRO_LINEA_ID", SqlDbType.Int);
            par0.Direction = ParameterDirection.Input;
            par0.Value = parametros.RegistroLineaId;

            if (parametros.CarneIdentidadId == 0) { SqlParameter par2 = cmd.Parameters.Add("@P_RELI_ICARNE_IDENTIDAD_ID", DBNull.Value); }
            else
            {
                SqlParameter par2 = cmd.Parameters.Add("@P_RELI_ICARNE_IDENTIDAD_ID", SqlDbType.Int);
                par2.Direction = ParameterDirection.Input;
                par2.Value = parametros.CarneIdentidadId;
            }
            
            SqlParameter par5 = cmd.Parameters.Add("@P_RELI_STIPO_EMISION", SqlDbType.SmallInt);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametros.TipoEmision;

            SqlParameter par6 = cmd.Parameters.Add("@P_RELI_SDP_RELDEP_TITDEP", SqlDbType.SmallInt);
            par6.Direction = ParameterDirection.Input;
            par6.Value = parametros.DpReldepTitdep;

            if (parametros.DpReldepTitular == 0) { SqlParameter par7 = cmd.Parameters.Add("@P_RELI_SDP_RELDEP_TITULAR", DBNull.Value); }
            else
            {
                SqlParameter par7 = cmd.Parameters.Add("@P_RELI_SDP_RELDEP_TITULAR", SqlDbType.SmallInt);
                par7.Direction = ParameterDirection.Input;
                par7.Value = parametros.DpReldepTitular;
            }

            SqlParameter par8 = cmd.Parameters.Add("@P_RELI_VDP_PRIMER_APELLIDO", SqlDbType.VarChar, 100);
            par8.Direction = ParameterDirection.Input;
            par8.Value = parametros.DpPrimerApellido;

            if (parametros.DpSegundoApellido == null) { SqlParameter par9 = cmd.Parameters.Add("@P_RELI_VDP_SEGUNDO_APELLIDO", DBNull.Value); }
            else
            {
                SqlParameter par9 = cmd.Parameters.Add("@P_RELI_VDP_SEGUNDO_APELLIDO", SqlDbType.VarChar, 100);
                par9.Direction = ParameterDirection.Input;
                par9.Value = parametros.DpSegundoApellido;
            }

            SqlParameter par10 = cmd.Parameters.Add("@P_RELI_VDP_NOMBRES", SqlDbType.VarChar, 100);
            par10.Direction = ParameterDirection.Input;
            par10.Value = parametros.DpNombres;

            SqlParameter par11 = cmd.Parameters.Add("@P_RELI_DDP_FECHA_NACIMIENTO", SqlDbType.DateTime);
            par11.Direction = ParameterDirection.Input;
            par11.Value = parametros.DpFechaNacimiento;

            SqlParameter par12 = cmd.Parameters.Add("@P_RELI_SDP_GENERO_ID", SqlDbType.SmallInt);
            par12.Direction = ParameterDirection.Input;
            par12.Value = parametros.DpGeneroId;

            SqlParameter par13 = cmd.Parameters.Add("@P_RELI_SDP_ESTADO_CIVIL_ID", SqlDbType.SmallInt);
            par13.Direction = ParameterDirection.Input;
            par13.Value = parametros.DpEstadoCivilId;

            SqlParameter par14 = cmd.Parameters.Add("@P_RELI_SDP_TIPO_DOC_IDENTIDAD", SqlDbType.SmallInt);
            par14.Direction = ParameterDirection.Input;
            par14.Value = parametros.DpTipoDocIdentidad;

            SqlParameter par15 = cmd.Parameters.Add("@P_RELI_VDP_NUMERO_DOC_IDENTIDAD", SqlDbType.VarChar, 20);
            par15.Direction = ParameterDirection.Input;
            par15.Value = parametros.DpNumeroDocIdentidad;

            SqlParameter par16 = cmd.Parameters.Add("@P_RELI_SDP_PAIS_NACIONALIDAD", SqlDbType.SmallInt);
            par16.Direction = ParameterDirection.Input;
            par16.Value = parametros.DpPaisNacionalidad;

            SqlParameter par17 = cmd.Parameters.Add("@P_RELI_VDP_DOMICILIO_PERU", SqlDbType.VarChar, 500);
            par17.Direction = ParameterDirection.Input;
            par17.Value = parametros.DpDomicilioPeru;

            SqlParameter par18 = cmd.Parameters.Add("@P_RELI_CDP_UBIGEO", SqlDbType.Char, 6);
            par18.Direction = ParameterDirection.Input;
            par18.Value = parametros.DpUbigeo;

            if (parametros.DpCorreoElectronico == null) { SqlParameter par19 = cmd.Parameters.Add("@P_RELI_VDP_CORREO_ELECTRONICO", DBNull.Value); }
            else
            {
                SqlParameter par19 = cmd.Parameters.Add("@P_RELI_VDP_CORREO_ELECTRONICO", SqlDbType.VarChar, 50);
                par19.Direction = ParameterDirection.Input;
                par19.Value = parametros.DpCorreoElectronico;
            }

            if (parametros.DpNumeroTelefono == null) { SqlParameter par20 = cmd.Parameters.Add("@P_RELI_VDP_NUMERO_TELEFONO", DBNull.Value); }
            else
            {
                SqlParameter par20 = cmd.Parameters.Add("@P_RELI_VDP_NUMERO_TELEFONO", SqlDbType.VarChar, 50);
                par20.Direction = ParameterDirection.Input;
                par20.Value = parametros.DpNumeroTelefono;
            }

            SqlParameter par21 = cmd.Parameters.Add("@P_RELI_VDP_RUTA_ADJUNTO", SqlDbType.VarChar, 250);
            par21.Direction = ParameterDirection.Input;
            par21.Value = parametros.DpRutaAdjunto;

            SqlParameter par22 = cmd.Parameters.Add("@P_RELI_SIN_TIPO_INSTITUCION", SqlDbType.SmallInt);
            par22.Direction = ParameterDirection.Input;
            par22.Value = parametros.InTipoInstitucion;

            SqlParameter par23 = cmd.Parameters.Add("@P_RELI_VIN_NOMBRE_INSTITUCION", SqlDbType.VarChar, 250);
            par23.Direction = ParameterDirection.Input;
            par23.Value = parametros.InNombreInstitucion;

            SqlParameter par24 = cmd.Parameters.Add("@P_RELI_VIN_PERSONA_CONTACTO", SqlDbType.VarChar, 250);
            par24.Direction = ParameterDirection.Input;
            par24.Value = parametros.InPersonaContacto;

            SqlParameter par25 = cmd.Parameters.Add("@P_RELI_VIN_CARGO_CONTACTO", SqlDbType.VarChar, 250);
            par25.Direction = ParameterDirection.Input;
            par25.Value = parametros.InCargoContacto;

            if (parametros.InCorreoElectronico == null) { SqlParameter par26 = cmd.Parameters.Add("@P_RELI_VIN_CORREO_ELECTRONICO", DBNull.Value); }
            else
            {
                SqlParameter par26 = cmd.Parameters.Add("@P_RELI_VIN_CORREO_ELECTRONICO", SqlDbType.VarChar, 50);
                par26.Direction = ParameterDirection.Input;
                par26.Value = parametros.InCorreoElectronico;
            }

            if (parametros.InNumeroTelefono == null) { SqlParameter par27 = cmd.Parameters.Add("@P_RELI_VIN_NUMERO_TELEFONO", DBNull.Value); }
            else
            {
                SqlParameter par27 = cmd.Parameters.Add("@P_RELI_VIN_NUMERO_TELEFONO", SqlDbType.VarChar, 50);
                par27.Direction = ParameterDirection.Input;
                par27.Value = parametros.InNumeroTelefono;
            }

            SqlParameter par28 = cmd.Parameters.Add("@P_RELI_SIN_TIPO_CARGO", SqlDbType.SmallInt);
            par28.Direction = ParameterDirection.Input;
            par28.Value = parametros.InTipoCargo;

            SqlParameter par29 = cmd.Parameters.Add("@P_RELI_VIN_CARGO_NOMBRE", SqlDbType.VarChar, 150);
            par29.Direction = ParameterDirection.Input;
            par29.Value = parametros.InCargoNombre;

            SqlParameter par35 = cmd.Parameters.Add("@P_RELI_VIP_MODIFICACION", SqlDbType.VarChar, 50);
            par35.Direction = ParameterDirection.Input;
            par35.Value = parametros.IpModificacion;


            SqlParameter par36 = cmd.Parameters.Add("@P_RELI_VDP_RUTA_FIRMA", SqlDbType.VarChar, 100);
            par36.Direction = ParameterDirection.Input;
            par36.Value = parametros.DpRutaFirma;

            SqlParameter par37 = cmd.Parameters.Add("@P_RELI_VDP_RUTA_PASAPORTE", SqlDbType.VarChar, 100);
            par37.Direction = ParameterDirection.Input;
            par37.Value = parametros.DpRutaPasaporte;

            SqlParameter par38 = cmd.Parameters.Add("@P_RELI_VDP_RUTA_DEN_POLICIAL", SqlDbType.VarChar, 100);
            par38.Direction = ParameterDirection.Input;
            par38.Value = parametros.DpRutaDenunciaPolicial;

            SqlParameter par39 = cmd.Parameters.Add("@P_RELI_VDP_MOTIVO_DUPLICADO", SqlDbType.VarChar, 100);
            par39.Direction = ParameterDirection.Input;
            par39.Value = parametros.DpMotivoDuplicado;


            SqlParameter par40 = cmd.Parameters.Add("@P_RELI_SIN_COD_CARGO", SqlDbType.SmallInt);
            par40.Direction = ParameterDirection.Input;
            par40.Value = parametros.COD_CARGO;

            SqlParameter par41 = cmd.Parameters.Add("@P_RELI_SIN_COD_CATEGORIA_MISION", SqlDbType.SmallInt);
            par41.Direction = ParameterDirection.Input;
            par41.Value = parametros.COD_CATEGORIA_MISION;

            SqlParameter par42 = cmd.Parameters.Add("@P_RELI_SIN_COD_INSTITUCION", SqlDbType.SmallInt);
            par42.Direction = ParameterDirection.Input;
            par42.Value = parametros.COD_INSTITUCION;

            SqlParameter par43 = cmd.Parameters.Add("@P_RELI_SIN_TIPO_CALIDAD_MIGRATORIA", SqlDbType.SmallInt);
            par43.Direction = ParameterDirection.Input;
            par43.Value = parametros.TIPO_CALIDAD_MIGRATORIA;

            
            SqlParameter par44 = cmd.Parameters.Add("@P_RELI_VTIPO_VISA", SqlDbType.VarChar,2);
            par44.Direction = ParameterDirection.Input;
            par44.Value = parametros.TipoVisa;

            SqlParameter par45 = cmd.Parameters.Add("@P_RELI_VVISA", SqlDbType.VarChar,5);
            par45.Direction = ParameterDirection.Input;
            par45.Value = parametros.Visa;

            SqlParameter par46 = cmd.Parameters.Add("@P_RELI_VTITULARIDAD_FAMILIAR", SqlDbType.VarChar,2);
            par46.Direction = ParameterDirection.Input;
            par46.Value = parametros.TitularidadFamiliar;

            SqlParameter par47 = cmd.Parameters.Add("@P_RELI_STIEMPO_PERMANENCIA", SqlDbType.SmallInt);
            par47.Direction = ParameterDirection.Input;
            par47.Value = parametros.TiempoPermanencia;







            //int n = cmd.ExecuteNonQuery();
            //if (n != 0)
            //{
            //    exito = true;
            //}
            //return (exito);

            int n = cmd.ExecuteNonQuery();
            exito = (n > 0);
            return (exito);
        }

        public beRegistroLinea consultarRegistro(SqlConnection con, beRegistroLinea parametros)
        {
            SqlCommand cmd = new SqlCommand("SC_REGLINEA.USP_CD_CONSULTA_EXTERNA", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_RELI_VNUMERO_REG_LINEA", SqlDbType.VarChar, 9);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.NumeroRegLinea;

            SqlDataReader drd = cmd.ExecuteReader();
            beRegistroLinea obeRegistroLinea = new beRegistroLinea();
            if (drd != null)
            {
                int posREGLINEA_ID = drd.GetOrdinal("REGLINEA_ID");
                int posRELI_NUMERO = drd.GetOrdinal("RELI_NUMERO");
                int posRELI_CARDIP_ID = drd.GetOrdinal("RELI_CARDIP_ID");
                int posRELI_TIPO_EMISION = drd.GetOrdinal("RELI_TIPO_EMISION");
                int posRELI_TITDEP = drd.GetOrdinal("RELI_TITDEP");
                int posRELI_TITULAR = drd.GetOrdinal("RELI_TITULAR");
                int posRELI_PRIAPE = drd.GetOrdinal("RELI_PRIAPE");
                int posRELI_SEGAPE = drd.GetOrdinal("RELI_SEGAPE");
                int posRELI_NOMBRES = drd.GetOrdinal("RELI_NOMBRES");
                int posRELI_FECNAC = drd.GetOrdinal("RELI_FECNAC");
                int posRELI_GENERO = drd.GetOrdinal("RELI_GENERO");
                int posRELI_ESTCIVIL = drd.GetOrdinal("RELI_ESTCIVIL");
                int posRELI_TIPODOCIDENT = drd.GetOrdinal("RELI_TIPODOCIDENT");
                int posRELI_NUMDOCIDENT = drd.GetOrdinal("RELI_NUMDOCIDENT");
                int posRELI_NACIONALIDAD = drd.GetOrdinal("RELI_NACIONALIDAD");
                int posRELI_DOMIPERU = drd.GetOrdinal("RELI_DOMIPERU");
                int posRELI_UBIGEO = drd.GetOrdinal("RELI_UBIGEO");
                int posRELI_CORREO = drd.GetOrdinal("RELI_CORREO");
                int posRELI_TELEFONO = drd.GetOrdinal("RELI_TELEFONO");
                int posRELI_ADJUNTO = drd.GetOrdinal("RELI_ADJUNTO");
                int posRELI_TIPOINST = drd.GetOrdinal("RELI_TIPOINST");
                int posRELI_NOMINST = drd.GetOrdinal("RELI_NOMINST");
                int posRELI_PERCONTACTO = drd.GetOrdinal("RELI_PERCONTACTO");
                int posRELI_CARGOCONTACTO = drd.GetOrdinal("RELI_CARGOCONTACTO");
                int posRELI_CORREOINST = drd.GetOrdinal("RELI_CORREOINST");
                int posRELI_TELEFONOINST = drd.GetOrdinal("RELI_TELEFONOINST");
                int posRELI_TIPOCARGO = drd.GetOrdinal("RELI_TIPOCARGO");
                int posRELI_NOMBRECARGO = drd.GetOrdinal("RELI_NOMBRECARGO");
                int posRELI_FECHACREA = drd.GetOrdinal("RELI_FECHACREA");
                int posRELI_HORACREA = drd.GetOrdinal("RELI_HORACREA");
                int posRELI_NOMBRE_COMPLETO = drd.GetOrdinal("RELI_NOMBRE_COMPLETO");
                int posRELI_ESTADO_DESC = drd.GetOrdinal("RELI_ESTADO_DESC");
                int posRELI_GENERO_DESC = drd.GetOrdinal("RELI_GENERO_DESC");
                int posRELI_ESTADOC_DESC = drd.GetOrdinal("RELI_ESTADOC_DESC");
                int posRELI_DOC_IDENT_DESC = drd.GetOrdinal("RELI_DOC_IDENT_DESC");
                int posRELI_DEPARTAMENTO = drd.GetOrdinal("RELI_DEPARTAMENTO");
                int posRELI_PROVINCIA = drd.GetOrdinal("RELI_PROVINCIA");
                int posRELI_DISTRITO = drd.GetOrdinal("RELI_DISTRITO");
                int posRELI_TITDEPDESC = drd.GetOrdinal("RELI_TITDEPDESC");
                int posRELI_FECNAC_STR = drd.GetOrdinal("RELI_FECNAC_STR");
                int posRELI_PAIS = drd.GetOrdinal("RELI_PAIS");
                int posRELI_TIPOEMISION = drd.GetOrdinal("RELI_TIPOEMISION");

                int posRELI_VDP_RUTA_FIRMA = drd.GetOrdinal("RELI_VDP_RUTA_FIRMA");
                int posRELI_VDP_RUTA_PASAPORTE = drd.GetOrdinal("RELI_VDP_RUTA_PASAPORTE");
                int posRELI_VDP_RUTA_DEN_POLICIAL = drd.GetOrdinal("RELI_VDP_RUTA_DEN_POLICIAL");

                int posRELI_SIN_COD_INSTITUCION = drd.GetOrdinal("RELI_SIN_COD_INSTITUCION");
                int posRELI_SIN_COD_CATEGORIA_MISION = drd.GetOrdinal("RELI_SIN_COD_CATEGORIA_MISION");
                int posRELI_SIN_TIPO_CALIDAD_MIGRATORIA = drd.GetOrdinal("RELI_SIN_TIPO_CALIDAD_MIGRATORIA");
                int posRELI_SIN_COD_CARGO = drd.GetOrdinal("RELI_SIN_COD_CARGO");
                int posNumeroCarnet = drd.GetOrdinal("NUMERO_CARNE");
                //int posOBSERCACION = drd.GetOrdinal("OBSERVACION");

                if (drd.HasRows)
                {
                    drd.Read();
                    
                    obeRegistroLinea.RegistroLineaId = drd.GetInt32(posREGLINEA_ID);
                    obeRegistroLinea.NumeroRegLinea = drd.GetString(posRELI_NUMERO);
                    obeRegistroLinea.CarneIdentidadId = drd.GetInt32(posRELI_CARDIP_ID);
                    obeRegistroLinea.TipoEmision = drd.GetInt16(posRELI_TIPO_EMISION);
                    obeRegistroLinea.DpReldepTitdep = drd.GetInt16(posRELI_TITDEP);
                    obeRegistroLinea.DpReldepTitular = drd.GetInt16(posRELI_TITULAR);
                    obeRegistroLinea.DpPrimerApellido = drd.GetString(posRELI_PRIAPE);
                    obeRegistroLinea.DpSegundoApellido = drd.GetString(posRELI_SEGAPE);
                    obeRegistroLinea.DpNombres = drd.GetString(posRELI_NOMBRES);
                    obeRegistroLinea.DpFechaNacimiento = drd.GetDateTime(posRELI_FECNAC);
                    obeRegistroLinea.DpGeneroId = drd.GetInt16(posRELI_GENERO);
                    obeRegistroLinea.DpEstadoCivilId = drd.GetInt16(posRELI_ESTCIVIL);
                    obeRegistroLinea.DpTipoDocIdentidad = drd.GetInt16(posRELI_TIPODOCIDENT);
                    obeRegistroLinea.DpNumeroDocIdentidad = drd.GetString(posRELI_NUMDOCIDENT);
                    obeRegistroLinea.DpPaisNacionalidad = drd.GetInt16(posRELI_NACIONALIDAD);
                    obeRegistroLinea.DpDomicilioPeru = drd.GetString(posRELI_DOMIPERU);
                    obeRegistroLinea.DpUbigeo = drd.GetString(posRELI_UBIGEO);
                    obeRegistroLinea.DpCorreoElectronico = drd.GetString(posRELI_CORREO);
                    obeRegistroLinea.DpNumeroTelefono = drd.GetString(posRELI_TELEFONO);
                    obeRegistroLinea.DpRutaAdjunto = drd.GetString(posRELI_ADJUNTO);
                    obeRegistroLinea.InTipoInstitucion = drd.GetInt16(posRELI_TIPOINST);
                    obeRegistroLinea.InNombreInstitucion = drd.GetString(posRELI_NOMINST);
                    obeRegistroLinea.InPersonaContacto = drd.GetString(posRELI_PERCONTACTO);
                    obeRegistroLinea.InCargoContacto = drd.GetString(posRELI_CARGOCONTACTO);
                    obeRegistroLinea.InCorreoElectronico = drd.GetString(posRELI_CORREOINST);
                    obeRegistroLinea.InNumeroTelefono = drd.GetString(posRELI_TELEFONOINST);
                    obeRegistroLinea.InTipoCargo = drd.GetInt16(posRELI_TIPOCARGO);
                    obeRegistroLinea.InCargoNombre = drd.GetString(posRELI_NOMBRECARGO);
                    obeRegistroLinea.ConFechaCreacion = drd.GetString(posRELI_FECHACREA);
                    obeRegistroLinea.ConHoraCreacion = drd.GetString(posRELI_HORACREA);
                    obeRegistroLinea.ConNombreCompleto = drd.GetString(posRELI_NOMBRE_COMPLETO);
                    obeRegistroLinea.ConEstadoDesc = drd.GetString(posRELI_ESTADO_DESC);
                    obeRegistroLinea.ConGenero = drd.GetString(posRELI_GENERO_DESC);
                    obeRegistroLinea.ConEstadoCivil = drd.GetString(posRELI_ESTADOC_DESC);
                    obeRegistroLinea.ConTipoDocIdent = drd.GetString(posRELI_DOC_IDENT_DESC);
                    obeRegistroLinea.ConDepartamento = drd.GetString(posRELI_DEPARTAMENTO);
                    obeRegistroLinea.ConProvincia = drd.GetString(posRELI_PROVINCIA);
                    obeRegistroLinea.ConDistrito = drd.GetString(posRELI_DISTRITO);
                    obeRegistroLinea.ConTitDep = drd.GetString(posRELI_TITDEPDESC);
                    obeRegistroLinea.ConFechaNacimiento = drd.GetString(posRELI_FECNAC_STR);
                    obeRegistroLinea.ConPais = drd.GetString(posRELI_PAIS);
                    obeRegistroLinea.ConTipoEmision = drd.GetString(posRELI_TIPOEMISION);

                    obeRegistroLinea.DpRutaFirma = drd.GetString(posRELI_VDP_RUTA_FIRMA);
                    obeRegistroLinea.DpRutaPasaporte = drd.GetString(posRELI_VDP_RUTA_PASAPORTE);
                    obeRegistroLinea.DpRutaDenunciaPolicial = drd.GetString(posRELI_VDP_RUTA_DEN_POLICIAL);

                    obeRegistroLinea.COD_INSTITUCION = drd.GetInt16(posRELI_SIN_COD_INSTITUCION);
                    obeRegistroLinea.COD_CATEGORIA_MISION = drd.GetInt16(posRELI_SIN_COD_CATEGORIA_MISION);
                    obeRegistroLinea.TIPO_CALIDAD_MIGRATORIA = drd.GetInt16(posRELI_SIN_TIPO_CALIDAD_MIGRATORIA);
                    obeRegistroLinea.COD_CARGO = drd.GetInt16(posRELI_SIN_COD_CARGO);
                    obeRegistroLinea.numeroCanet = drd.GetString(posNumeroCarnet);
                    //obeRegistroLinea.Observacion = drd.GetString(posOBSERCACION);
                }
                drd.Close();
            }
            return (obeRegistroLinea);
        }
        public List<beRegistroLinea> consultarRegistroTrazabilidad(SqlConnection con, beRegistroLinea parametros)
        {
            SqlCommand cmd = new SqlCommand("SC_REGLINEA.USP_CD_CONSULTA_TRAZABILIDAD", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_RELI_VNUMERO_REG_LINEA", SqlDbType.VarChar, 9);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.NumeroRegLinea;

            SqlDataReader drd = cmd.ExecuteReader();
            List<beRegistroLinea> lista = new List<beRegistroLinea>();
            if (drd != null)
            {
                int posREGLINEA_ID = drd.GetOrdinal("REGLINEA_ID");
                int posRELI_NUMERO = drd.GetOrdinal("RELI_NUMERO");
                int posRELI_CARDIP_ID = drd.GetOrdinal("RELI_CARDIP_ID");
                int posRELI_TIPO_EMISION = drd.GetOrdinal("RELI_TIPO_EMISION");
                int posRELI_TITDEP = drd.GetOrdinal("RELI_TITDEP");
                int posRELI_TITULAR = drd.GetOrdinal("RELI_TITULAR");
                int posRELI_PRIAPE = drd.GetOrdinal("RELI_PRIAPE");
                int posRELI_SEGAPE = drd.GetOrdinal("RELI_SEGAPE");
                int posRELI_NOMBRES = drd.GetOrdinal("RELI_NOMBRES");
                int posRELI_FECNAC = drd.GetOrdinal("RELI_FECNAC");
                int posRELI_GENERO = drd.GetOrdinal("RELI_GENERO");
                int posRELI_ESTCIVIL = drd.GetOrdinal("RELI_ESTCIVIL");
                int posRELI_TIPODOCIDENT = drd.GetOrdinal("RELI_TIPODOCIDENT");
                int posRELI_NUMDOCIDENT = drd.GetOrdinal("RELI_NUMDOCIDENT");
                int posRELI_NACIONALIDAD = drd.GetOrdinal("RELI_NACIONALIDAD");
                int posRELI_DOMIPERU = drd.GetOrdinal("RELI_DOMIPERU");
                int posRELI_UBIGEO = drd.GetOrdinal("RELI_UBIGEO");
                int posRELI_CORREO = drd.GetOrdinal("RELI_CORREO");
                int posRELI_TELEFONO = drd.GetOrdinal("RELI_TELEFONO");
                int posRELI_ADJUNTO = drd.GetOrdinal("RELI_ADJUNTO");
                int posRELI_TIPOINST = drd.GetOrdinal("RELI_TIPOINST");
                int posRELI_NOMINST = drd.GetOrdinal("RELI_NOMINST");
                int posRELI_PERCONTACTO = drd.GetOrdinal("RELI_PERCONTACTO");
                int posRELI_CARGOCONTACTO = drd.GetOrdinal("RELI_CARGOCONTACTO");
                int posRELI_CORREOINST = drd.GetOrdinal("RELI_CORREOINST");
                int posRELI_TELEFONOINST = drd.GetOrdinal("RELI_TELEFONOINST");
                int posRELI_TIPOCARGO = drd.GetOrdinal("RELI_TIPOCARGO");
                int posRELI_NOMBRECARGO = drd.GetOrdinal("RELI_NOMBRECARGO");
                int posRELI_FECHACREA = drd.GetOrdinal("RELI_FECHACREA");
                int posRELI_HORACREA = drd.GetOrdinal("RELI_HORACREA");
                int posRELI_NOMBRE_COMPLETO = drd.GetOrdinal("RELI_NOMBRE_COMPLETO");
                int posRELI_ESTADO_DESC = drd.GetOrdinal("RELI_ESTADO_DESC");
                int posRELI_GENERO_DESC = drd.GetOrdinal("RELI_GENERO_DESC");
                int posRELI_ESTADOC_DESC = drd.GetOrdinal("RELI_ESTADOC_DESC");
                int posRELI_DOC_IDENT_DESC = drd.GetOrdinal("RELI_DOC_IDENT_DESC");
                int posRELI_DEPARTAMENTO = drd.GetOrdinal("RELI_DEPARTAMENTO");
                int posRELI_PROVINCIA = drd.GetOrdinal("RELI_PROVINCIA");
                int posRELI_DISTRITO = drd.GetOrdinal("RELI_DISTRITO");
                int posRELI_TITDEPDESC = drd.GetOrdinal("RELI_TITDEPDESC");
                int posRELI_FECNAC_STR = drd.GetOrdinal("RELI_FECNAC_STR");
                int posRELI_PAIS = drd.GetOrdinal("RELI_PAIS");
                int posRELI_TIPOEMISION = drd.GetOrdinal("RELI_TIPOEMISION");

                int posRELI_VDP_RUTA_FIRMA = drd.GetOrdinal("RELI_VDP_RUTA_FIRMA");
                int posRELI_VDP_RUTA_PASAPORTE = drd.GetOrdinal("RELI_VDP_RUTA_PASAPORTE");
                int posRELI_VDP_RUTA_DEN_POLICIAL = drd.GetOrdinal("RELI_VDP_RUTA_DEN_POLICIAL");

                int posRELI_SIN_COD_INSTITUCION = drd.GetOrdinal("RELI_SIN_COD_INSTITUCION");
                int posRELI_SIN_COD_CATEGORIA_MISION = drd.GetOrdinal("RELI_SIN_COD_CATEGORIA_MISION");
                int posRELI_SIN_TIPO_CALIDAD_MIGRATORIA = drd.GetOrdinal("RELI_SIN_TIPO_CALIDAD_MIGRATORIA");
                int posRELI_SIN_COD_CARGO = drd.GetOrdinal("RELI_SIN_COD_CARGO");
                int posNumeroCarnet = drd.GetOrdinal("NUMERO_CARNE");
                int posOBSERCACION = drd.GetOrdinal("OBSERVACION");

                while (drd.Read())
                {
                    beRegistroLinea obeRegistroLinea = new beRegistroLinea();
                    obeRegistroLinea.RegistroLineaId = drd.GetInt32(posREGLINEA_ID);
                    obeRegistroLinea.NumeroRegLinea = drd.GetString(posRELI_NUMERO);
                    obeRegistroLinea.CarneIdentidadId = drd.GetInt32(posRELI_CARDIP_ID);
                    obeRegistroLinea.TipoEmision = drd.GetInt16(posRELI_TIPO_EMISION);
                    obeRegistroLinea.DpReldepTitdep = drd.GetInt16(posRELI_TITDEP);
                    obeRegistroLinea.DpReldepTitular = drd.GetInt16(posRELI_TITULAR);
                    obeRegistroLinea.DpPrimerApellido = drd.GetString(posRELI_PRIAPE);
                    obeRegistroLinea.DpSegundoApellido = drd.GetString(posRELI_SEGAPE);
                    obeRegistroLinea.DpNombres = drd.GetString(posRELI_NOMBRES);
                    obeRegistroLinea.DpFechaNacimiento = drd.GetDateTime(posRELI_FECNAC);
                    obeRegistroLinea.DpGeneroId = drd.GetInt16(posRELI_GENERO);
                    obeRegistroLinea.DpEstadoCivilId = drd.GetInt16(posRELI_ESTCIVIL);
                    obeRegistroLinea.DpTipoDocIdentidad = drd.GetInt16(posRELI_TIPODOCIDENT);
                    obeRegistroLinea.DpNumeroDocIdentidad = drd.GetString(posRELI_NUMDOCIDENT);
                    obeRegistroLinea.DpPaisNacionalidad = drd.GetInt16(posRELI_NACIONALIDAD);
                    obeRegistroLinea.DpDomicilioPeru = drd.GetString(posRELI_DOMIPERU);
                    obeRegistroLinea.DpUbigeo = drd.GetString(posRELI_UBIGEO);
                    obeRegistroLinea.DpCorreoElectronico = drd.GetString(posRELI_CORREO);
                    obeRegistroLinea.DpNumeroTelefono = drd.GetString(posRELI_TELEFONO);
                    obeRegistroLinea.DpRutaAdjunto = drd.GetString(posRELI_ADJUNTO);
                    obeRegistroLinea.InTipoInstitucion = drd.GetInt16(posRELI_TIPOINST);
                    obeRegistroLinea.InNombreInstitucion = drd.GetString(posRELI_NOMINST);
                    obeRegistroLinea.InPersonaContacto = drd.GetString(posRELI_PERCONTACTO);
                    obeRegistroLinea.InCargoContacto = drd.GetString(posRELI_CARGOCONTACTO);
                    obeRegistroLinea.InCorreoElectronico = drd.GetString(posRELI_CORREOINST);
                    obeRegistroLinea.InNumeroTelefono = drd.GetString(posRELI_TELEFONOINST);
                    obeRegistroLinea.InTipoCargo = drd.GetInt16(posRELI_TIPOCARGO);
                    obeRegistroLinea.InCargoNombre = drd.GetString(posRELI_NOMBRECARGO);
                    obeRegistroLinea.ConFechaCreacion = drd.GetString(posRELI_FECHACREA);
                    obeRegistroLinea.ConHoraCreacion = drd.GetString(posRELI_HORACREA);
                    obeRegistroLinea.ConNombreCompleto = drd.GetString(posRELI_NOMBRE_COMPLETO);
                    obeRegistroLinea.ConEstadoDesc = drd.GetString(posRELI_ESTADO_DESC);
                    obeRegistroLinea.ConGenero = drd.GetString(posRELI_GENERO_DESC);
                    obeRegistroLinea.ConEstadoCivil = drd.GetString(posRELI_ESTADOC_DESC);
                    obeRegistroLinea.ConTipoDocIdent = drd.GetString(posRELI_DOC_IDENT_DESC);
                    obeRegistroLinea.ConDepartamento = drd.GetString(posRELI_DEPARTAMENTO);
                    obeRegistroLinea.ConProvincia = drd.GetString(posRELI_PROVINCIA);
                    obeRegistroLinea.ConDistrito = drd.GetString(posRELI_DISTRITO);
                    obeRegistroLinea.ConTitDep = drd.GetString(posRELI_TITDEPDESC);
                    obeRegistroLinea.ConFechaNacimiento = drd.GetString(posRELI_FECNAC_STR);
                    obeRegistroLinea.ConPais = drd.GetString(posRELI_PAIS);
                    obeRegistroLinea.ConTipoEmision = drd.GetString(posRELI_TIPOEMISION);

                    obeRegistroLinea.DpRutaFirma = drd.GetString(posRELI_VDP_RUTA_FIRMA);
                    obeRegistroLinea.DpRutaPasaporte = drd.GetString(posRELI_VDP_RUTA_PASAPORTE);
                    obeRegistroLinea.DpRutaDenunciaPolicial = drd.GetString(posRELI_VDP_RUTA_DEN_POLICIAL);

                    obeRegistroLinea.COD_INSTITUCION = drd.GetInt16(posRELI_SIN_COD_INSTITUCION);
                    obeRegistroLinea.COD_CATEGORIA_MISION = drd.GetInt16(posRELI_SIN_COD_CATEGORIA_MISION);
                    obeRegistroLinea.TIPO_CALIDAD_MIGRATORIA = drd.GetInt16(posRELI_SIN_TIPO_CALIDAD_MIGRATORIA);
                    obeRegistroLinea.COD_CARGO = drd.GetInt16(posRELI_SIN_COD_CARGO);
                    obeRegistroLinea.numeroCanet = drd.GetString(posNumeroCarnet);
                    obeRegistroLinea.Observacion = drd.GetString(posOBSERCACION);
                    lista.Add(obeRegistroLinea);
                }
                drd.Close();
            }
            return (lista);
        }

        public beRegistroLinea obtenerNumero(SqlConnection con, beRegistroLinea parametros)
        {
            beRegistroLinea obeRegistroLinea = new beRegistroLinea();
            SqlCommand cmd = new SqlCommand("SC_REGLINEA.USP_RL_REGISTRO_LINEA_OBTENER_NUMERO", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_RELI_IREGISTRO_LINEA_ID", SqlDbType.VarChar, 9);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.RegistroLineaId;

            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                int posRELI_NUMERO = drd.GetOrdinal("RELI_NUMERO");
                if (drd.HasRows)
                {
                    drd.Read();
                    obeRegistroLinea.NumeroRegLinea = drd.GetString(posRELI_NUMERO);
                }
                drd.Close();
            }
            return (obeRegistroLinea);
        }

        public beCarneIdentidadPrincipal consultarCarne(SqlConnection con, beCarneIdentidad parametros)
        {
            beCarneIdentidadPrincipal obeCarneIdentidadPrincipal = new beCarneIdentidadPrincipal();
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_CARNE_IDENTIDAD_CONSULTAR_REGISTRO_EDICION", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_CARNE_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.CarneIdentidadid;

            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                int posCARDIP_ID = drd.GetOrdinal("CARDIP_ID");
                int posCARDIP_RUTA_ARCHIVO = drd.GetOrdinal("CARDIP_RUTA_ARCHIVO");
                int posCARDIP_RUTA_ARCHIVO_FIRMA = drd.GetOrdinal("CARDIP_RUTA_ARCHIVO_FIRMA");
                int posCARDIP_NUMERO_CARNE = drd.GetOrdinal("CARDIP_NUMERO_CARNE");
                int posCARDIP_FECHA_EMISION = drd.GetOrdinal("CARDIP_FECHA_EMISION");
                int posCARDIP_FECHA_VENC = drd.GetOrdinal("CARDIP_FECHA_VENC");
                int posNUMERO_IDENT = drd.GetOrdinal("NUMERO_IDENT");
                int posPERSONA_APE_PAT = drd.GetOrdinal("PERSONA_APE_PAT");
                int posPERSONA_APE_MAT = drd.GetOrdinal("PERSONA_APE_MAT");
                int posPERSONA_NOMBRES = drd.GetOrdinal("PERSONA_NOMBRES");
                int posTELEFONO = drd.GetOrdinal("TELEFONO");
                int posPERSONA_FECNAC = drd.GetOrdinal("PERSONA_FECNAC");
                int posPERSONA_ESTCIVIL_ID = drd.GetOrdinal("PERSONA_ESTCIVIL_ID");
                int posPERSONA_GENERO_ID = drd.GetOrdinal("PERSONA_GENERO_ID");
                int posPERIDENT_ID = drd.GetOrdinal("PERIDENT_ID");
                int posPERIDENT_TIPODOC_ID = drd.GetOrdinal("PERIDENT_TIPODOC_ID");
                int posPERIDENT_NUMDOCIDENT = drd.GetOrdinal("PERIDENT_NUMDOCIDENT");
                int posPAIS_ID = drd.GetOrdinal("PAIS_ID");
                int posPAIS_NACIONALIDAD = drd.GetOrdinal("PAIS_NACIONALIDAD");
                int posPERES_ID = drd.GetOrdinal("PERES_ID");
                int posPERES_RESI_ID = drd.GetOrdinal("PERES_RESI_ID");
                int posRESI_DIRECCION = drd.GetOrdinal("RESI_DIRECCION");
                int posUBIGEO_DEPARTAMENTO = drd.GetOrdinal("UBIGEO_DEPARTAMENTO");
                int posUBIGEO_PROVINCIA = drd.GetOrdinal("UBIGEO_PROVINCIA");
                int posUBIGEO_DISTRITO = drd.GetOrdinal("UBIGEO_DISTRITO");
                int posCALMIG_PRI = drd.GetOrdinal("CALMIG_PRI");
                int posCALMIGSEC_SEC = drd.GetOrdinal("CALMIGSEC_SEC");
                int posCALMIGSEC_TITDEP = drd.GetOrdinal("CALMIGSEC_TITDEP");
                int posOFCOEX_ID = drd.GetOrdinal("OFCOEX_ID");
                int posOFCOEX_CATEGORIA = drd.GetOrdinal("OFCOEX_CATEGORIA");
                int posOFCOEX_ESTADOID = drd.GetOrdinal("ESTADOID");
                int posOFCOEX_ESTADO_DESCRIPCION = drd.GetOrdinal("ESTADO_DESCRIPCION");
                
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
                    drd.Read();
                    // PERSONA ---------------------------------------------------------------------------------
                    Persona.Apellidopaterno = drd.GetString(posPERSONA_APE_PAT);
                    Persona.Apellidomaterno = (!drd.IsDBNull(posPERSONA_APE_MAT) ? drd.GetString(posPERSONA_APE_MAT) : "");
                    Persona.Nombres = drd.GetString(posPERSONA_NOMBRES);
                    Persona.Telefono = (!drd.IsDBNull(posTELEFONO) ? drd.GetString(posTELEFONO) : "");
                    Persona.Nacimientofecha = drd.GetDateTime(posPERSONA_FECNAC);
                    Persona.Estadocivilid = drd.GetInt16(posPERSONA_ESTCIVIL_ID);
                    Persona.Generoid = drd.GetInt16(posPERSONA_GENERO_ID);
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
                    UbiGeo.Ubi01 = drd.GetString(posUBIGEO_DEPARTAMENTO);
                    UbiGeo.Ubi02 = drd.GetString(posUBIGEO_PROVINCIA);
                    UbiGeo.Ubi03 = drd.GetString(posUBIGEO_DISTRITO);
                    // CARDIP ---------------------------------------------------------------------------------
                    CarneIdentidad.ConIdent = drd.GetString(posNUMERO_IDENT);
                    CarneIdentidad.CarneIdentidadid = drd.GetInt16(posCARDIP_ID);
                    CarneIdentidad.RutaArchivoFoto = drd.GetString(posCARDIP_RUTA_ARCHIVO);
                    CarneIdentidad.RutaArchivoFirma = drd.GetString(posCARDIP_RUTA_ARCHIVO_FIRMA);
                    

                    if (!drd.IsDBNull(posCARDIP_NUMERO_CARNE)) { CarneIdentidad.CarneNumero = drd.GetString(posCARDIP_NUMERO_CARNE); }
                    else { CarneIdentidad.CarneNumero = "[ NO DEFINIDO ]"; }
                    if (!drd.IsDBNull(posCARDIP_FECHA_EMISION)) { CarneIdentidad.FechaEmision = drd.GetDateTime(posCARDIP_FECHA_EMISION); }
                    if (!drd.IsDBNull(posCARDIP_FECHA_VENC)) { CarneIdentidad.FechaVencimiento = drd.GetDateTime(posCARDIP_FECHA_VENC); }
                    //CALIDAD MIGRATORIA
                    CalidadMigratoriaPri.CalidadMigratoriaid = drd.GetInt16(posCALMIG_PRI);
                    CalidadMigratoriaSec.CalidadMigratoriaid = drd.GetInt16(posCALMIGSEC_SEC);
                    CalidadMigratoriaSec.FlagTitularDependiente = drd.GetInt16(posCALMIGSEC_TITDEP);
                    // OFCOEX ---------------------------------------------------------------------------------
                    OficinaConEx.OficinaconsularExtranjeraid = drd.GetInt16(posOFCOEX_ID);
                    OficinaConEx.Categoriaid = drd.GetInt16(posOFCOEX_CATEGORIA);
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
                    obeCarneIdentidadPrincipal.CarneIdentidad.Estado = drd.GetString(posOFCOEX_ESTADO_DESCRIPCION);
                    obeCarneIdentidadPrincipal.CarneIdentidad.Estadoid = drd.GetInt16(posOFCOEX_ESTADOID);
                }
                drd.Close();
            }
            return (obeCarneIdentidadPrincipal);
        }

        public beCarneIdentidad obtenerCarneId(SqlConnection con, beCarneIdentidad parametros)
        {
            beCarneIdentidad obeCarneIdentidad = new beCarneIdentidad();
            SqlCommand cmd = new SqlCommand("SC_REGLINEA.USP_RL_REGISTRO_LINEA_CONSULTAR_CARNE_OBTENER_CARNE_ID", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_CARNE_IDENTIDAD_NUMERO", SqlDbType.VarChar, 10);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.CarneNumero;

            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                int posCARDIP_ID = drd.GetOrdinal("CARDIP_ID");
                if (drd.HasRows)
                {
                    drd.Read();
                    obeCarneIdentidad.CarneIdentidadid = drd.GetInt16(posCARDIP_ID);
                }
                drd.Close();
            }
            return (obeCarneIdentidad);
        }

        public beCarneIdentidadPrincipal obtenerRelacionDependencia(SqlConnection con, beCarneIdentidad parametrosCarneIdentidad)
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
                int posFLAG_SOLICITUD = drd.GetOrdinal("FLAG_SOLICITUD");
                int posTIPO_ENTRADA_ID = drd.GetOrdinal("TIPO_ENTRADA_ID");
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
                    obeCarneIdentidad.FlagSolicitudActiva = drd.GetBoolean(posFLAG_SOLICITUD);
                    obeCarneIdentidad.TipoEntrada = drd.GetInt16(posTIPO_ENTRADA_ID);
                    lbeCarneIdentidad.Add(obeCarneIdentidad);
                }
                obeCarneIdentidadPrincipal.ListaConsulta = lbeCarneIdentidad;
                drd.Close();
            }
            return (obeCarneIdentidadPrincipal);
        }

        public beCarneIdentidad consultarCarnexId(SqlConnection con, short carneID)
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

        public bool actualizarEstado(SqlConnection con, SqlTransaction trx, beRegistroLinea parametros)
        {
            bool exito = false;
            SqlCommand cmd = new SqlCommand("SC_REGLINEA.USP_RL_REGISTRO_LINEA_ACTUALIZAR_ESTADO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par0 = cmd.Parameters.Add("@P_RELI_IREGISTRO_LINEA_ID", SqlDbType.Int);
            par0.Direction = ParameterDirection.Input;
            par0.Value = parametros.RegistroLineaId;

            SqlParameter par1 = cmd.Parameters.Add("@P_RELI_SESTADO_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.EstadoId;

            SqlParameter par2 = cmd.Parameters.Add("@P_RELI_VDP_RUTA_RESUMEN_ANEXOS", SqlDbType.VarChar,100);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.DpRutaResumen;
            

            SqlParameter par3 = cmd.Parameters.Add("@P_RELI_VIP_MODIFICACION", SqlDbType.VarChar, 50);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros.IpModificacion;

            int n = cmd.ExecuteNonQuery();
            exito = (n > 0);
            return (exito);
        }
        public bool actualizarDetalleEstadoEnviado(SqlConnection con, SqlTransaction trx, beRegistroLinea parametros)
        {
            bool exito = false;
            SqlCommand cmd = new SqlCommand("SC_REGLINEA.USP_RL_REGISTRO_LINEA_ACTUALIZAR_DETALLEESTADO_ENVIADO", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = trx;

            SqlParameter par0 = cmd.Parameters.Add("@P_RELI_IREGISTRO_LINEA_ID", SqlDbType.Int);
            par0.Direction = ParameterDirection.Input;
            par0.Value = parametros.RegistroLineaId;

            SqlParameter par1 = cmd.Parameters.Add("@P_RELI_SESTADO_ID", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.EstadoId;

            int n = cmd.ExecuteNonQuery();
            exito = (n > 0);
            return (exito);
        }

        public string VerificarInformacionSAM(SqlConnection con, string numeroPasaporte, string apePaterno,string apeMaterno, string nombres, DateTime fechaNac)
        {
            string respuesta = "NO";
            SqlCommand cmd = new SqlCommand("SC_REGLINEA.USP_RL_REGISTRO_LINEA_VERIFICAR_SAM", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_NUM_PASAPORTE", SqlDbType.VarChar,12);
            par1.Direction = ParameterDirection.Input;
            par1.Value = numeroPasaporte;

            SqlParameter par2 = cmd.Parameters.Add("@P_APE_PATERNO", SqlDbType.VarChar, 50);
            par2.Direction = ParameterDirection.Input;
            par2.Value = apePaterno;

            SqlParameter par3 = cmd.Parameters.Add("@P_APE_MATERNO", SqlDbType.VarChar, 50);
            par3.Direction = ParameterDirection.Input;
            par3.Value = apeMaterno;

            SqlParameter par4 = cmd.Parameters.Add("@P_APE_NOMBRES", SqlDbType.VarChar, 50);
            par4.Direction = ParameterDirection.Input;
            par4.Value = nombres;

            SqlParameter par5 = cmd.Parameters.Add("@P_FECHA_NACIMIENTO", SqlDbType.DateTime);
            par5.Direction = ParameterDirection.Input;
            par5.Value = fechaNac;

            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int posRespuesta  = drd.GetOrdinal("RESPUESTA");
                if (drd.HasRows)
                {
                    drd.Read();
                    respuesta = drd.GetString(posRespuesta);
                }
                drd.Close();
            }
            return respuesta;
        }
    }
}
