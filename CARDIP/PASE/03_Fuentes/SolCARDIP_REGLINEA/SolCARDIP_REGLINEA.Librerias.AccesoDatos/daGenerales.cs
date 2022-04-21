using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP_REGLINEA.Librerias.EntidadesNegocio;

namespace SolCARDIP_REGLINEA.Librerias.AccesoDatos
{
    public class daGenerales
    {
        public beGenerales obtenerGenerales(SqlConnection con)
        {
            beGenerales obeGenerales = new beGenerales();
            SqlCommand cmd = new SqlCommand("SC_REGLINEA.USP_CD_GENERALES_MRE_REGISTRO_LINEA", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader drd = cmd.ExecuteReader();

            if (drd != null)
            {
                int posParametroidGenero = drd.GetOrdinal("PARA_GENERO_ID");
                int posGrupoGenero = drd.GetOrdinal("PARA_GENERO_GRUPO");
                int posDescripcionGenero = drd.GetOrdinal("PARA_GENERO_DESC");
                int posValorGenero = drd.GetOrdinal("PARA_GENERO_VALOR");
                // GENERO PERSONA (PARAMETRO)
                List<beParametro> lbeParametroGenero = new List<beParametro>();
                beParametro obeParametroGenero;
                while (drd.Read())
                {
                    obeParametroGenero = new beParametro();
                    obeParametroGenero.Parametroid = drd.GetInt16(posParametroidGenero);
                    obeParametroGenero.Grupo = drd.GetString(posGrupoGenero);
                    obeParametroGenero.Descripcion = drd.GetString(posDescripcionGenero);
                    obeParametroGenero.Valor = drd.GetString(posValorGenero);
                    lbeParametroGenero.Add(obeParametroGenero);
                }
                obeGenerales.ListaParametroGenero = lbeParametroGenero;
                if (drd.NextResult())
                {
                    // ESTADO CIVIL PERSONA (PARAMETRO)
                    List<beParametro> lbeParametroEstCivil = new List<beParametro>();
                    int posParametroidEstCivil = drd.GetOrdinal("PARA_ESTCIV_ID");
                    int posGrupoEstCivil = drd.GetOrdinal("PARA_ESTCIV_GRUPO");
                    int posDescripcionEstCivil = drd.GetOrdinal("PARA_ESTCIV_DESC");
                    int posValorEstCivil = drd.GetOrdinal("PARA_ESTCIV_VALOR");
                    beParametro obeParametroEstCivil;
                    while (drd.Read())
                    {
                        obeParametroEstCivil = new beParametro();
                        obeParametroEstCivil.Parametroid = drd.GetInt16(posParametroidEstCivil);
                        obeParametroEstCivil.Grupo = drd.GetString(posGrupoEstCivil);
                        obeParametroEstCivil.Descripcion = drd.GetString(posDescripcionEstCivil);
                        obeParametroEstCivil.Valor = drd.GetString(posValorEstCivil);
                        lbeParametroEstCivil.Add(obeParametroEstCivil);
                    }
                    obeGenerales.ListaParametroEstadoCivil = lbeParametroEstCivil;
                    if (drd.NextResult())
                    {
                    // PAISES
                        List<bePais> lbePais = new List<bePais>();
                        int posPaisid = drd.GetOrdinal("PAIS_ID");
                        int posContinenteidPais = drd.GetOrdinal("PAIS_CONTI_ID");
                        int posNombrePais = drd.GetOrdinal("PAIS_NOMBRE");
                        int posNacionalidadPais = drd.GetOrdinal("PAIS_NACIONALIDAD");
                        int posUbigeoPais = drd.GetOrdinal("PAIS_UBIGEI");
                        bePais obePais;
                        while (drd.Read())
                        {
                            obePais = new bePais();
                            obePais.Paisid = drd.GetInt16(posPaisid);
                            if (drd.IsDBNull(posContinenteidPais)) { obePais.Continenteid = 0; }
                            else { obePais.Continenteid = drd.GetInt16(posContinenteidPais); }
                            obePais.Nombre = drd.GetString(posNombrePais);
                            if (drd.IsDBNull(posNacionalidadPais)) { obePais.Nacionalidad = "[ NO DEFINIDO ]"; }
                            else { obePais.Nacionalidad = drd.GetString(posNacionalidadPais); }
                            if (drd.IsDBNull(posUbigeoPais)) { obePais.Ubigeo = "[ NO DEFINIDO ]"; }
                            else { obePais.Ubigeo = drd.GetString(posUbigeoPais); }
                            lbePais.Add(obePais);
                        }
                        obeGenerales.ListaPaises = lbePais;
                        if (drd.NextResult())
                        {
                            List<beParametro> lbeParametroTitDep = new List<beParametro>();
                            int posParametroidTitDep = drd.GetOrdinal("PARA_TITDEP_ID");
                            int posGrupoTitDep = drd.GetOrdinal("PARA_TITDEP_GRUPO");
                            int posDescripcionTitDep = drd.GetOrdinal("PARA_TITDEP_DESC");
                            int posValorTitDep = drd.GetOrdinal("PARA_TITDEP_VALOR");
                            beParametro obeParametroTitDep;
                            while (drd.Read())
                            {
                                obeParametroTitDep = new beParametro();
                                obeParametroTitDep.Parametroid = drd.GetInt16(posParametroidTitDep);
                                obeParametroTitDep.Grupo = drd.GetString(posGrupoTitDep);
                                obeParametroTitDep.Descripcion = drd.GetString(posDescripcionTitDep);
                                obeParametroTitDep.Valor = drd.GetString(posValorTitDep);
                                lbeParametroTitDep.Add(obeParametroTitDep);
                            }
                            obeGenerales.TitularDependienteParametros = lbeParametroTitDep;
                                                                                
                                                                                    
                            if (drd.NextResult())
                            {
                            // TIPO EMISION
                                List<beParametro> listaTipoEmision = new List<beParametro>();
                                int posPARA_TIPOEMI_ID = drd.GetOrdinal("PARA_TIPOEMI_ID");
                                int posPARA_TIPOEMI_GRUPO = drd.GetOrdinal("PARA_TIPOEMI_GRUPO");
                                int posPARA_TIPOEMI_DESC = drd.GetOrdinal("PARA_TIPOEMI_DESC");
                                int posPARA_TIPOEMI_VALOR = drd.GetOrdinal("PARA_TIPOEMI_VALOR");
                                beParametro obeParametroTipoEmi;
                                while (drd.Read())
                                {
                                    obeParametroTipoEmi = new beParametro();
                                    obeParametroTipoEmi.Parametroid = drd.GetInt16(posPARA_TIPOEMI_ID);
                                    obeParametroTipoEmi.Grupo = drd.GetString(posPARA_TIPOEMI_GRUPO);
                                    obeParametroTipoEmi.Descripcion = drd.GetString(posPARA_TIPOEMI_DESC);
                                    obeParametroTipoEmi.Valor = drd.GetString(posPARA_TIPOEMI_VALOR);
                                    listaTipoEmision.Add(obeParametroTipoEmi);
                                }
                                obeGenerales.TipoEmision = listaTipoEmision;
                                                                                                                   
                                if (drd.NextResult())
                                {
                                    List<beParametro> listaTipoInstitucion = new List<beParametro>();
                                    int posPARA_TIPOINS_ID = drd.GetOrdinal("PARA_TIPOINS_ID");
                                    int posPARA_TIPOINS_GRUPO = drd.GetOrdinal("PARA_TIPOINS_GRUPO");
                                    int posPARA_TIPOINS_DESC = drd.GetOrdinal("PARA_TIPOINS_DESC");
                                    int posPARA_TIPOINS_VALOR = drd.GetOrdinal("PARA_TIPOINS_VALOR");
                                    beParametro obeParametroTipoInst;
                                    while (drd.Read())
                                    {
                                        obeParametroTipoInst = new beParametro();
                                        obeParametroTipoInst.Parametroid = drd.GetInt16(posPARA_TIPOINS_ID);
                                        obeParametroTipoInst.Grupo = drd.GetString(posPARA_TIPOINS_GRUPO);
                                        obeParametroTipoInst.Descripcion = drd.GetString(posPARA_TIPOINS_DESC);
                                        obeParametroTipoInst.Valor = drd.GetString(posPARA_TIPOINS_VALOR);
                                        listaTipoInstitucion.Add(obeParametroTipoInst);
                                    }
                                    obeGenerales.ListaTipoInstitucion = listaTipoInstitucion;
                                    if (drd.NextResult())
                                    {
                                        List<beParametro> ListaCargoInstitucion = new List<beParametro>();
                                        int posPARA_TIPOINS_CARGO_ID = drd.GetOrdinal("PARA_TIPOINS_CARGO_ID");
                                        int posPARA_TIPOINS_CARGO_GRUPO = drd.GetOrdinal("PARA_TIPOINS_CARGO_GRUPO");
                                        int posPARA_TIPOINS_CARGO_DESC = drd.GetOrdinal("PARA_TIPOINS_CARGO_DESC");
                                        int posPARA_TIPOINS_CARGO_VALOR = drd.GetOrdinal("PARA_TIPOINS_CARGO_VALOR");
                                        beParametro obeParametroTipoCargpInst;
                                        while (drd.Read())
                                        {
                                            obeParametroTipoCargpInst = new beParametro();
                                            obeParametroTipoCargpInst.Parametroid = drd.GetInt16(posPARA_TIPOINS_CARGO_ID);
                                            obeParametroTipoCargpInst.Grupo = drd.GetString(posPARA_TIPOINS_CARGO_GRUPO);
                                            obeParametroTipoCargpInst.Descripcion = drd.GetString(posPARA_TIPOINS_CARGO_DESC);
                                            obeParametroTipoCargpInst.Valor = drd.GetString(posPARA_TIPOINS_CARGO_VALOR);
                                            ListaCargoInstitucion.Add(obeParametroTipoCargpInst);
                                        }
                                        obeGenerales.ListaCargoInstitucion = ListaCargoInstitucion;
                                        if (drd.NextResult())
                                        {
                                            List<beParametro> ListaRelacionDependencia = new List<beParametro>();
                                            int posPARA_REDE_ID = drd.GetOrdinal("PARA_REDE_ID");
                                            int posPARA_REDE_GRUPO = drd.GetOrdinal("PARA_REDE_GRUPO");
                                            int posPARA_REDE_DESC = drd.GetOrdinal("PARA_REDE_DESC");
                                            int posPARA_REDE_VALOR = drd.GetOrdinal("PARA_REDE_VALOR");
                                            beParametro obeParametroRelacionDep;
                                            while (drd.Read())
                                            {
                                                obeParametroRelacionDep = new beParametro();
                                                obeParametroRelacionDep.Parametroid = drd.GetInt16(posPARA_REDE_ID);
                                                obeParametroRelacionDep.Grupo = drd.GetString(posPARA_REDE_GRUPO);
                                                obeParametroRelacionDep.Descripcion = drd.GetString(posPARA_REDE_DESC);
                                                obeParametroRelacionDep.Valor = drd.GetString(posPARA_REDE_VALOR);
                                                ListaRelacionDependencia.Add(obeParametroRelacionDep);
                                            }
                                            obeGenerales.ListaRelacionDependencia = ListaRelacionDependencia;
                                            if (drd.NextResult())
                                            {
                                                List<beEstado> ListaEstadosRegLinea = new List<beEstado>();
                                                int posESTADO_REGLINEA_ID = drd.GetOrdinal("ESTADO_REGLINEA_ID");
                                                int posESTADO_REGLINEA_DESCRIPCION = drd.GetOrdinal("ESTADO_REGLINEA_DESCRIPCION");
                                                int posESTADO_REGLINEA_GRUPO = drd.GetOrdinal("ESTADO_REGLINEA_GRUPO");
                                                beEstado obeEstadoRegLinea;
                                                while (drd.Read())
                                                {
                                                    obeEstadoRegLinea = new beEstado();
                                                    obeEstadoRegLinea.Estadoid = drd.GetInt16(posESTADO_REGLINEA_ID);
                                                    obeEstadoRegLinea.DescripcionCorta = drd.GetString(posESTADO_REGLINEA_DESCRIPCION);
                                                    obeEstadoRegLinea.Grupo = drd.GetString(posESTADO_REGLINEA_GRUPO);
                                                    ListaEstadosRegLinea.Add(obeEstadoRegLinea);
                                                }
                                                obeGenerales.ListaEstadosRegLinea = ListaEstadosRegLinea;
                                                if (drd.NextResult())
                                                {
                                                    // DOCUMENTO DE IDENTIDAD
                                                    List<beDocumentoIdentidad> ListaDocumentoIdentidadRegLinea = new List<beDocumentoIdentidad>();
                                                    int posDOCIDENT_ID = drd.GetOrdinal("DOCIDENT_ID");
                                                    int posDOCIDENT_DESCORTA = drd.GetOrdinal("DOCIDENT_DESCORTA");
                                                    int posDOCIDENT_DESCLARGA = drd.GetOrdinal("DOCIDENT_DESCLARGA");
                                                    int posDOCIDENT_PAIS = drd.GetOrdinal("DOCIDENT_PAIS");
                                                    beDocumentoIdentidad obeDocumentoIdentidadRegLinea;
                                                    while (drd.Read())
                                                    {
                                                        obeDocumentoIdentidadRegLinea = new beDocumentoIdentidad();
                                                        obeDocumentoIdentidadRegLinea.Tipodocumentoidentidadid = drd.GetInt16(posDOCIDENT_ID);
                                                        obeDocumentoIdentidadRegLinea.DescripcionCorta = drd.GetString(posDOCIDENT_DESCORTA);
                                                        obeDocumentoIdentidadRegLinea.DescripcionLarga = drd.GetString(posDOCIDENT_DESCLARGA);
                                                        obeDocumentoIdentidadRegLinea.PaisId = drd.GetInt16(posDOCIDENT_PAIS);
                                                        ListaDocumentoIdentidadRegLinea.Add(obeDocumentoIdentidadRegLinea);
                                                    }
                                                    obeGenerales.ListaDocumentoIdentidadRegLinea = ListaDocumentoIdentidadRegLinea;
                                                    if (drd.NextResult())
                                                    {
                                                        // CALIDAD MIGRATORIA NIVEL 0
                                                        List<beCalidadMigratoria> lbeCalidadMigratoriaNivelPrincipal = new List<beCalidadMigratoria>();
                                                        int posCalidadMigratoriaid = drd.GetOrdinal("CALMIG_ID");
                                                        int posNumeroOrden = drd.GetOrdinal("CALMIG_NUMORDEN");
                                                        int posFlagNivelCalidad = drd.GetOrdinal("CALMIG_NIVEL");
                                                        int posNombreCalMig = drd.GetOrdinal("CALMIG_NOMBRE");
                                                        int posDefinicionCalMig = drd.GetOrdinal("CALMIG_DEFINICION");
                                                        beCalidadMigratoria obeCalidadMigratoriaPri;
                                                        while (drd.Read())
                                                        {
                                                            obeCalidadMigratoriaPri = new beCalidadMigratoria();
                                                            obeCalidadMigratoriaPri.CalidadMigratoriaid = drd.GetInt16(posCalidadMigratoriaid);
                                                            obeCalidadMigratoriaPri.NumeroOrden = drd.GetString(posNumeroOrden);
                                                            obeCalidadMigratoriaPri.FlagNivelCalidad = drd.GetBoolean(posFlagNivelCalidad);
                                                            obeCalidadMigratoriaPri.Nombre = drd.GetString(posNombreCalMig);
                                                            obeCalidadMigratoriaPri.Definicion = drd.GetString(posDefinicionCalMig);
                                                            lbeCalidadMigratoriaNivelPrincipal.Add(obeCalidadMigratoriaPri);
                                                        }
                                                        obeGenerales.ListaCalidadMigratoriaNivelPrincipal = lbeCalidadMigratoriaNivelPrincipal;
                                                        if (drd.NextResult())
                                                        {
                                                            // CALIDAD MIGRATORIA NIVEL 1
                                                            List<beCalidadMigratoria> lbeCalidadMigratoriaNivelSecundario = new List<beCalidadMigratoria>();
                                                            int posCalidadMigratoriaidSec = drd.GetOrdinal("CALMIG_ID");
                                                            int posFlagTitularDependiente = drd.GetOrdinal("CALMIG_TITDEP");
                                                            int posFlagNivelCalidadSec = drd.GetOrdinal("CALMIG_NIVEL");
                                                            int posReferenciaId = drd.GetOrdinal("CALMIG_REFERENCIA");
                                                            int posNombreCalMigSec = drd.GetOrdinal("CALMIG_NOMBRE");
                                                            int posTIT_DEP = drd.GetOrdinal("TIT_DEP");
                                                            int posCALMIG_GENERO = drd.GetOrdinal("CALMIG_GENERO");
                                                            beCalidadMigratoria obeCalidadMigratoriaSec;
                                                            while (drd.Read())
                                                            {
                                                                obeCalidadMigratoriaSec = new beCalidadMigratoria();
                                                                obeCalidadMigratoriaSec.CalidadMigratoriaid = drd.GetInt16(posCalidadMigratoriaidSec);
                                                                obeCalidadMigratoriaSec.FlagTitularDependiente = drd.GetInt16(posFlagTitularDependiente);
                                                                obeCalidadMigratoriaSec.FlagNivelCalidad = drd.GetBoolean(posFlagNivelCalidadSec);
                                                                obeCalidadMigratoriaSec.ReferenciaId = drd.GetInt16(posReferenciaId);
                                                                obeCalidadMigratoriaSec.Nombre = drd.GetString(posNombreCalMigSec);
                                                                obeCalidadMigratoriaSec.TitularDependiente = drd.GetString(posTIT_DEP);
                                                                obeCalidadMigratoriaSec.GeneroId = drd.GetInt16(posCALMIG_GENERO);
                                                                lbeCalidadMigratoriaNivelSecundario.Add(obeCalidadMigratoriaSec);
                                                            }
                                                            obeGenerales.ListaCalidadMigratoriaNivelSecundario = lbeCalidadMigratoriaNivelSecundario;
                                                            if (drd.NextResult())
                                                            {
                                                                List<beParametro> lbeParametroCatOfcoExt = new List<beParametro>();
                                                                int posParametroidCatOfcoExt = drd.GetOrdinal("PARA_TITDEP_ID");
                                                                int posGrupoCatOfcoExt = drd.GetOrdinal("PARA_TITDEP_GRUPO");
                                                                int posDescripcionCatOfcoExt = drd.GetOrdinal("PARA_TITDEP_DESC");
                                                                int posValorCatOfcoExt = drd.GetOrdinal("PARA_TITDEP_VALOR");
                                                                beParametro obeParametroCatOfcoExt;
                                                                while (drd.Read())
                                                                {
                                                                    obeParametroCatOfcoExt = new beParametro();
                                                                    obeParametroCatOfcoExt.Parametroid = drd.GetInt16(posParametroidCatOfcoExt);
                                                                    obeParametroCatOfcoExt.Grupo = drd.GetString(posGrupoCatOfcoExt);
                                                                    obeParametroCatOfcoExt.Descripcion = drd.GetString(posDescripcionCatOfcoExt);
                                                                    obeParametroCatOfcoExt.Valor = drd.GetString(posValorCatOfcoExt);
                                                                    lbeParametroCatOfcoExt.Add(obeParametroCatOfcoExt);
                                                                }
                                                                obeGenerales.CategoriaOficinaExtranjera = lbeParametroCatOfcoExt;
                                                                if (drd.NextResult())
                                                                {
                                                                    // OFICINAS CONSULARES EXTRANJERAS
                                                                    List<beOficinaconsularExtranjera> lbeOficinaconsularExtranjera = new List<beOficinaconsularExtranjera>();
                                                                    int posOficinaconsularExtranjeraid = drd.GetOrdinal("OFCOEX_ID");
                                                                    int posCategoriaidOfcoEx = drd.GetOrdinal("OFCOEX_CATEGORIA");
                                                                    int posSiglasOfcoEx = drd.GetOrdinal("OFCOEX_SIGLAS");
                                                                    int posNombreOfcoEx = drd.GetOrdinal("OFCOEX_NOMBRE");
                                                                    int posUbigeocodigoOfcoEx = drd.GetOrdinal("OFCOEX_UBIGEO");
                                                                    int posJefaturaflagOfcoEx = drd.GetOrdinal("OFCOEX_JEFAFLAG");
                                                                    beOficinaconsularExtranjera obeOficinaconsularExtranjera;
                                                                    while (drd.Read())
                                                                    {
                                                                        obeOficinaconsularExtranjera = new beOficinaconsularExtranjera();
                                                                        obeOficinaconsularExtranjera.OficinaconsularExtranjeraid = drd.GetInt16(posOficinaconsularExtranjeraid);
                                                                        obeOficinaconsularExtranjera.Categoriaid = drd.GetInt16(posCategoriaidOfcoEx);
                                                                        if (drd.IsDBNull(posSiglasOfcoEx)) { obeOficinaconsularExtranjera.Siglas = "[ NO DEFINIDO ]"; }
                                                                        else { obeOficinaconsularExtranjera.Siglas = drd.GetString(posSiglasOfcoEx); }
                                                                        obeOficinaconsularExtranjera.Nombre = drd.GetString(posNombreOfcoEx);
                                                                        if (drd.IsDBNull(posUbigeocodigoOfcoEx)) { obeOficinaconsularExtranjera.Ubigeocodigo = "[ NO DEFINIDO ]"; }
                                                                        else { obeOficinaconsularExtranjera.Ubigeocodigo = drd.GetString(posUbigeocodigoOfcoEx); }
                                                                        obeOficinaconsularExtranjera.Jefaturaflag = drd.GetBoolean(posJefaturaflagOfcoEx);
                                                                        lbeOficinaconsularExtranjera.Add(obeOficinaconsularExtranjera);
                                                                    }
                                                                    obeGenerales.ListaOficinasConsularesExtranjeras = lbeOficinaconsularExtranjera;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                drd.Close();
            }
            return (obeGenerales);
        }
    }
}
