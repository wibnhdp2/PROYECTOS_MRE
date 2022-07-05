using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daReportes
    {
        public beReportesListas traerDatos(SqlConnection con, beReporteResumenxCalidad parametros)
        {
            beReportesListas obeReportesListas = new beReportesListas();
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_REPORTE_CARNE_IDENTIDAD_x_CALIDAD_RESUMEN", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_FECHA_INICIO", SqlDbType.DateTime);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.FechaInicio;

            SqlParameter par2 = cmd.Parameters.Add("@P_FECHA_FIN", SqlDbType.DateTime);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.FechaFin;

            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                List<beReporteResumenxCalidad> lista = new List<beReporteResumenxCalidad>();
                int posNOMBRE_CALIDAD = drd.GetOrdinal("NOMBRE_CALIDAD");
                int posREGISTRADOS = drd.GetOrdinal("REGISTRADOS");
                int posEMITIDOS = drd.GetOrdinal("EMITIDOS");
                int posVIGENTES = drd.GetOrdinal("VIGENTES");
                int posVENCIDOS = drd.GetOrdinal("VENCIDOS");
                beReporteResumenxCalidad obeReporteResumenxCalidad;
                if (drd.HasRows)
                {
                    while (drd.Read())
                    {
                        obeReporteResumenxCalidad = new beReporteResumenxCalidad();
                        obeReporteResumenxCalidad.CalidadMigratoria = drd.GetString(posNOMBRE_CALIDAD);
                        obeReporteResumenxCalidad.Registrados = drd.GetInt32(posREGISTRADOS);
                        obeReporteResumenxCalidad.Emitidos = drd.GetInt32(posEMITIDOS);
                        obeReporteResumenxCalidad.Vigentes = drd.GetInt32(posVIGENTES);
                        obeReporteResumenxCalidad.Vencidos = drd.GetInt32(posVENCIDOS);
                        lista.Add(obeReporteResumenxCalidad);
                    }
                    obeReportesListas.ListaResultado = lista;
                    if (drd.NextResult())
                    {
                        int posTOTAL = drd.GetOrdinal("TOTAL");
                        int posREGISTRADOS_TOT = drd.GetOrdinal("REGISTRADOS");
                        int posEMITIDOS_TOT = drd.GetOrdinal("EMITIDOS");
                        int posVIGENTES_TOT = drd.GetOrdinal("VIGENTES");
                        int posVENCIDOS_TOT = drd.GetOrdinal("VENCIDOS");
                        beReporteResumenxCalidad totales = new beReporteResumenxCalidad();
                        if (drd.HasRows)
                        {
                            drd.Read();
                            totales.CalidadMigratoria = drd.GetString(posTOTAL);
                            totales.Registrados = drd.GetInt32(posREGISTRADOS_TOT);
                            totales.Emitidos = drd.GetInt32(posEMITIDOS_TOT);
                            totales.Vigentes = drd.GetInt32(posVIGENTES_TOT);
                            totales.Vencidos = drd.GetInt32(posVENCIDOS_TOT);
                            obeReportesListas.Totales = totales;
                        }
                    }
                }
                drd.Close();
            }
            return (obeReportesListas);
        }

        public beReportesListas traerDatosDetalle(SqlConnection con, beReporteDetallexCalidad parametros)
        {
            beReportesListas obeReportesListas = new beReportesListas();
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_REPORTE_CARNE_IDENTIDAD_x_CALIDAD_DETALLE", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@P_FECHA_INICIO", SqlDbType.DateTime);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.FechaInicio;

            SqlParameter par2 = cmd.Parameters.Add("@P_FECHA_FIN", SqlDbType.DateTime);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.FechaFin;

            SqlParameter par3 = cmd.Parameters.Add("@P_CALIDAD_MIG", SqlDbType.SmallInt);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros.CalMigId;

            SqlParameter par4 = cmd.Parameters.Add("@P_ESTADO", SqlDbType.SmallInt);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametros.EstadoId;

            SqlParameter par5 = cmd.Parameters.Add("@P_OFCO_EX", SqlDbType.SmallInt);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametros.OfcoExId;

            SqlParameter par6 = cmd.Parameters.Add("@P_TIPO_BUSQUEDA", SqlDbType.VarChar,20);
            par6.Direction = ParameterDirection.Input;
            par6.Value = parametros.TipoBusqueda;

            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                List<beReporteDetallexCalidad> lista = new List<beReporteDetallexCalidad>();
                int posNUMERO_CARNE = drd.GetOrdinal("NUMERO_CARNE");
                int posESTADO = drd.GetOrdinal("ESTADO");
                int posCALIDAD_MIGRATORIA = drd.GetOrdinal("CALIDAD_MIGRATORIA");
                int posFECHA_INSCRIPCION = drd.GetOrdinal("FECHA_INSCRIPCION");
                int posFECHA_EMISION = drd.GetOrdinal("FECHA_EMISION");
                int posFECHA_VENCIMIENTO = drd.GetOrdinal("FECHA_VENCIMIENTO");
                int posTITULAR = drd.GetOrdinal("TITULAR");
                int posSTATUS_MIG = drd.GetOrdinal("STATUS_MIG");
                int posPAIS_NACIONALIDAD = drd.GetOrdinal("PAIS_NACIONALIDAD");
                int posOFICINA_CONSULAR_EXTRANJERA = drd.GetOrdinal("OFICINA_CONSULAR_EXTRANJERA");
                int posCARGO = drd.GetOrdinal("CARGO");
                int posECIVIL = drd.GetOrdinal("ECIVIL");
                int posTELEFONO = drd.GetOrdinal("TELEFONO");
                int posCORREO_ELECTRONICO = drd.GetOrdinal("CORREO_ELECTRONICO");
                int posDEPARTAMENTO = drd.GetOrdinal("DEPARTAMENTO");
                int posPROVINCIA = drd.GetOrdinal("PROVINCIA");
                int posDISTRITO = drd.GetOrdinal("DISTRITO");
                int posDIRECCION = drd.GetOrdinal("DIRECCION");

                beReporteDetallexCalidad obeReporteDetallexCalidad;
                while (drd.Read())
                {
                    obeReporteDetallexCalidad = new beReporteDetallexCalidad();
                    obeReporteDetallexCalidad.NumeroCarne = drd.GetString(posNUMERO_CARNE);
                    obeReporteDetallexCalidad.Estado = drd.GetString(posESTADO);
                    obeReporteDetallexCalidad.CalidadMigratoria = drd.GetString(posCALIDAD_MIGRATORIA);
                    obeReporteDetallexCalidad.FechaReg = drd.GetString(posFECHA_INSCRIPCION);
                    obeReporteDetallexCalidad.FechaEmi = drd.GetString(posFECHA_EMISION);
                    obeReporteDetallexCalidad.FechaVen = drd.GetString(posFECHA_VENCIMIENTO);
                    obeReporteDetallexCalidad.Titular = drd.GetString(posTITULAR);
                    obeReporteDetallexCalidad.TitDep = drd.GetString(posSTATUS_MIG);
                    obeReporteDetallexCalidad.PaisNac = drd.GetString(posPAIS_NACIONALIDAD);
                    obeReporteDetallexCalidad.OficinaConsularEx = drd.GetString(posOFICINA_CONSULAR_EXTRANJERA);
                    obeReporteDetallexCalidad.Cargo = drd.GetString(posCARGO);
                    obeReporteDetallexCalidad.Ecivil = drd.GetString(posECIVIL);
                    obeReporteDetallexCalidad.Telefono = drd.GetString(posTELEFONO);
                    obeReporteDetallexCalidad.CorreoElectronico = drd.GetString(posCORREO_ELECTRONICO);
                    obeReporteDetallexCalidad.Departamento = drd.GetString(posDEPARTAMENTO);
                    obeReporteDetallexCalidad.Provincia = drd.GetString(posPROVINCIA);
                    obeReporteDetallexCalidad.Distrito = drd.GetString(posDISTRITO);
                    obeReporteDetallexCalidad.Direccion = drd.GetString(posDIRECCION);

                    lista.Add(obeReporteDetallexCalidad);
                }
                obeReportesListas.ListaResultadoDetalle = lista;
                drd.Close();
            }
            return (obeReportesListas);
        }
        public beReportesListas consultarCarnet(SqlConnection con, beReporteDetallexCalidad parametros)
        {
            beReportesListas obeReportesListas = new beReportesListas();
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_REPORTE_CONSULTA_CARNET", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter par1 = cmd.Parameters.Add("@p_RELI_STIPO_EMISION", SqlDbType.SmallInt);
            par1.Direction = ParameterDirection.Input;
            par1.Value = parametros.StipoEmision;

            SqlParameter par2 = cmd.Parameters.Add("@P_FECHA_INICIO", SqlDbType.DateTime);
            par2.Direction = ParameterDirection.Input;
            par2.Value = parametros.FechaInicio;

            SqlParameter par3 = cmd.Parameters.Add("@P_FECHA_FIN", SqlDbType.DateTime);
            par3.Direction = ParameterDirection.Input;
            par3.Value = parametros.FechaFin;

            SqlParameter par4 = cmd.Parameters.Add("@P_CAID_SESTADOID", SqlDbType.SmallInt);
            par4.Direction = ParameterDirection.Input;
            par4.Value = parametros.EstadoId;

            SqlParameter par5 = cmd.Parameters.Add("@P_TIPODOC", SqlDbType.VarChar);
            par5.Direction = ParameterDirection.Input;
            par5.Value = parametros.TipoDoc;

            SqlParameter par6 = cmd.Parameters.Add("@DOCUMENTONUMERO", SqlDbType.VarChar);
            par6.Direction = ParameterDirection.Input;
            par6.Value = parametros.Num_documento;

            SqlParameter par7 = cmd.Parameters.Add("@NUM_SOLICITUD", SqlDbType.VarChar);
            par7.Direction = ParameterDirection.Input;
            par7.Value = parametros.Num_solicitud;

            SqlParameter par8 = cmd.Parameters.Add("@NUMERO_CARNE", SqlDbType.VarChar);
            par8.Direction = ParameterDirection.Input;
            par8.Value = parametros.Numero_carne;

            SqlParameter par9 = cmd.Parameters.Add("@APELLIDOS_NOMBRES", SqlDbType.VarChar);
            par9.Direction = ParameterDirection.Input;
            par9.Value = parametros.Apellidos;

            SqlParameter par10 = cmd.Parameters.Add("@FECHA_NACIMIENTOFECHA", SqlDbType.DateTime);
            par10.Direction = ParameterDirection.Input;
            par10.Value = parametros.FechaNacimiento;

            SqlParameter par11 = cmd.Parameters.Add("@PERS_SPAISID", SqlDbType.SmallInt);
            par11.Direction = ParameterDirection.Input;
            par11.Value = parametros.Paisid;

            SqlParameter par12 = cmd.Parameters.Add("@PARA_SPARAMETROID", SqlDbType.SmallInt);
            par12.Direction = ParameterDirection.Input;
            par12.Value = parametros.TitulaFamiliar;

            SqlParameter par13 = cmd.Parameters.Add("@OFICINA_CONSULARID", SqlDbType.SmallInt);
            par13.Direction = ParameterDirection.Input;
            par13.Value = parametros.OfCoId;

            SqlParameter par14 = cmd.Parameters.Add("@CAID_SUSUARIOCREACION", SqlDbType.SmallInt);
            par14.Direction = ParameterDirection.Input;
            par14.Value = parametros.Usuario;


           SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                List<beReporteDetallexCalidad> lista = new List<beReporteDetallexCalidad>();
                int posNUM_SOLICITUD = drd.GetOrdinal("NUM_SOLICITUD");
                int posRELI_STIPO_EMISION = drd.GetOrdinal("RELI_STIPO_EMISION");
                int posAPELLIDOS_NOMBRES = drd.GetOrdinal("APELLIDOS_NOMBRES");
                int posFECHA_NACIMIENTOFECHA = drd.GetOrdinal("FECHA_NACIMIENTOFECHA");
                int posTIPODOC = drd.GetOrdinal("TIPODOC");
                int posDOCUMENTONUMERO = drd.GetOrdinal("DOCUMENTONUMERO");
                int posPARA_SPARAMETROID = drd.GetOrdinal("PARA_SPARAMETROID");
                int posSTATUS_MIG = drd.GetOrdinal("STATUS_MIG");
                int posDOCUMENTO_TITULAR = drd.GetOrdinal("DOCUMENTO_TITULAR");
                int posPERS_SPAISID = drd.GetOrdinal("PERS_SPAISID");
                int posPAIS_NACIONALIDAD = drd.GetOrdinal("PAIS_NACIONALIDAD");
                int posCALIDAD_MIGRATORIA = drd.GetOrdinal("CALIDAD_MIGRATORIA");
                int posCAID_SESTADOID = drd.GetOrdinal("CAID_SESTADOID");
                int posESTADO = drd.GetOrdinal("ESTADO");
                int posNUMERO_CARNE = drd.GetOrdinal("NUMERO_CARNE");
                int posFECHA_EMISION = drd.GetOrdinal("FECHA_EMISION");
                int posFECHA_VENCIMIENTO = drd.GetOrdinal("FECHA_VENCIMIENTO");
                int posOFICINA_CONSULARID = drd.GetOrdinal("OFICINA_CONSULARID");
                int posOFICINA_CONSULAR_EXTRANJERA = drd.GetOrdinal("OFICINA_CONSULAR_EXTRANJERA");
                int posCAID_DFECHACREACION = drd.GetOrdinal("CAID_DFECHACREACION");
                int posCAID_SUSUARIOCREACION = drd.GetOrdinal("CAID_SUSUARIOCREACION");
                beReporteDetallexCalidad obe;
                while (drd.Read())
                {
                    obe = new beReporteDetallexCalidad();
                    obe.Num_solicitud = drd.IsDBNull(posNUM_SOLICITUD) ?"": drd.GetString(posNUM_SOLICITUD);
                    
                    //obe.Reli_stipo_emision = drd.GetString(posRELI_STIPO_EMISION);
                    obe.Apellidos_nombres = drd.IsDBNull(posAPELLIDOS_NOMBRES) ? "" : drd.GetString(posAPELLIDOS_NOMBRES);
                    obe.Fecha_nacimientofecha = drd.IsDBNull(posFECHA_NACIMIENTOFECHA) ? "" : drd.GetString(posFECHA_NACIMIENTOFECHA);
                    obe.TipoDoc = drd.IsDBNull(posTIPODOC) ? "" : "" + drd.GetString(posTIPODOC);
                    obe.Documentonumero = drd.IsDBNull(posDOCUMENTONUMERO) ? "" : "" + drd.GetString(posDOCUMENTONUMERO);
                    //obe.Para_sparametroid = "" + drd.GetInt(posPARA_SPARAMETROID);
                    obe.Status_mig = drd.IsDBNull(posSTATUS_MIG) ? "" : "" + drd.GetString(posSTATUS_MIG);
                    obe.Documento_titular = drd.IsDBNull(posDOCUMENTO_TITULAR) ? "" : "" + drd.GetString(posDOCUMENTO_TITULAR);
                    //obe.Pers_spaisid = "" + drd.GetInt16(posPERS_SPAISID);
                    obe.Pais_nacionalidad = drd.IsDBNull(posPAIS_NACIONALIDAD) ? "" : "" + drd.GetString(posPAIS_NACIONALIDAD);
                    obe.Calidad_migratoria = drd.IsDBNull(posCALIDAD_MIGRATORIA) ? "" : "" + drd.GetString(posCALIDAD_MIGRATORIA);
                    //obe.Caid_sestadoid = "" + drd.GetInt16(posCAID_SESTADOID);
                    obe.Estado = drd.IsDBNull(posESTADO) ? "" : "" + drd.GetString(posESTADO);
                    obe.Numero_carne = drd.IsDBNull(posNUMERO_CARNE) ? "" : "" + drd.GetString(posNUMERO_CARNE);
                    obe.Fecha_emision = drd.IsDBNull(posFECHA_EMISION) ? "" : "" + drd.GetString(posFECHA_EMISION);
                    obe.Fecha_vencimiento = drd.IsDBNull(posFECHA_VENCIMIENTO) ? "" : "" + drd.GetString(posFECHA_VENCIMIENTO);
                    //obe.Oficina_consularid = "" + drd.GetInt16(posOFICINA_CONSULARID);
                    obe.Oficina_consular_extranjera = drd.IsDBNull(posOFICINA_CONSULAR_EXTRANJERA) ? "" : "" + drd.GetString(posOFICINA_CONSULAR_EXTRANJERA);
                    //obe.Caid_dfechacreacion = "" + drd.GetString(posCAID_DFECHACREACION);
                    obe.Caid_susuariocreacion = drd.IsDBNull(posCAID_SUSUARIOCREACION) ? "" : "" + drd.GetInt16(posCAID_SUSUARIOCREACION);
                    lista.Add(obe);
                }
                obeReportesListas.ListaResultadoDetalle = lista;
                drd.Close();
            }
            return (obeReportesListas);
        }
    }
}
