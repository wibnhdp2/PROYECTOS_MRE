using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using SolCARDIP.Librerias.EntidadesNegocio;

namespace SolCARDIP.Librerias.AccesoDatos
{
    public class daGenerales
    {
        public beGenerales obtenerGenerales(SqlConnection con)
        {
            beGenerales obeGenerales = new beGenerales();
            SqlCommand cmd = new SqlCommand("SC_CARDIP.USP_CD_GENERALES_MRE", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader drd = cmd.ExecuteReader();
            if (drd != null)
            {
                drd.Read();
                // SISTEMA
                int posSistemaid = drd.GetOrdinal("IDSISTEMA");
                int posNombreSistema = drd.GetOrdinal("NOMBRESISTEMA");
                int posDescripcionSistema = drd.GetOrdinal("DESCRIPCIONSISTEMA");
                int posAbreviaturaSistema = drd.GetOrdinal("ABREVSISTEMA");
                beSistema obeSistema;
                if (drd.HasRows)
                {
                    obeSistema = new beSistema();
                    obeSistema.Sistemaid = drd.GetInt16(posSistemaid);
                    obeSistema.Nombre = drd.GetString(posNombreSistema);
                    obeSistema.Descripcion = drd.GetString(posDescripcionSistema);
                    obeSistema.Abreviatura = drd.GetString(posAbreviaturaSistema);
                    obeGenerales.SistemaInfo = obeSistema;
                    if (drd.NextResult())
                    {
                        // ESTADOS
                        List<beEstado> lbeEstado = new List<beEstado>();
                        int posEstadoid = drd.GetOrdinal("ESTADO_ID");
                        int posDescripcionCortaEstado = drd.GetOrdinal("ESTADO_DESCRIPCION");
                        int posGrupoEstado = drd.GetOrdinal("ESTADO_GRUPO");
                        beEstado obeEstado;
                        while (drd.Read())
                        {
                            obeEstado = new beEstado();
                            obeEstado.Estadoid = drd.GetInt16(posEstadoid);
                            obeEstado.DescripcionCorta = drd.GetString(posDescripcionCortaEstado);
                            obeEstado.Grupo = drd.GetString(posGrupoEstado);
                            lbeEstado.Add(obeEstado);
                        }
                        obeGenerales.ListaEstados = lbeEstado;
                        if (drd.NextResult())
                        {
                            // OFICINAS EN PERU
                            List<beOficinaconsular> lbeOficinaConsularPeru = new List<beOficinaconsular>();
                            int posIdOficinaConsularPeru = drd.GetOrdinal("OFICINAPERUID");
                            int posNombrePeru = drd.GetOrdinal("SIGLASPERU");
                            beOficinaconsular obeOficinaConsularPeru;
                            while (drd.Read())
                            {
                                obeOficinaConsularPeru = new beOficinaconsular();
                                obeOficinaConsularPeru.Oficinaconsularid = drd.GetInt16(posIdOficinaConsularPeru);
                                obeOficinaConsularPeru.Nombre = drd.GetString(posNombrePeru);
                                lbeOficinaConsularPeru.Add(obeOficinaConsularPeru);
                            }
                            obeGenerales.ListaOficinasPeru = lbeOficinaConsularPeru;
                            if (drd.NextResult())
                            {
                                //OFICINAS CONSULARES PERUANAS
                                List<beOficinaconsular> lbeOficinaConsularExtranjeroPeru = new List<beOficinaconsular>();
                                int posIdOficinaConsularEx = drd.GetOrdinal("OFICINAEXID");
                                int posNombreEx = drd.GetOrdinal("SIGLASEX");
                                beOficinaconsular obeOficinaConsularEx;
                                while (drd.Read())
                                {
                                    obeOficinaConsularEx = new beOficinaconsular();
                                    obeOficinaConsularEx.Oficinaconsularid = drd.GetInt16(posIdOficinaConsularPeru);
                                    obeOficinaConsularEx.Nombre = drd.GetString(posNombrePeru);
                                    lbeOficinaConsularExtranjeroPeru.Add(obeOficinaConsularEx);
                                }
                                obeGenerales.ListaOficinasConsularesPeru = lbeOficinaConsularExtranjeroPeru;
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
                                                // CATEGORIA FUNCIONARIO
                                                List<beCategoriaFuncionario> lbeCategoriaFuncionario = new List<beCategoriaFuncionario>();
                                                int posCategoriafuncionarioid = drd.GetOrdinal("CATFUN_ID");
                                                int posDescripcionCortaCatFun = drd.GetOrdinal("CATFUN_DESCCORTA");
                                                int posDescripcionLargaCatFun = drd.GetOrdinal("CATFUN_DESCLARGA");
                                                beCategoriaFuncionario obeCategoriaFuncionario;
                                                while (drd.Read())
                                                {
                                                    obeCategoriaFuncionario = new beCategoriaFuncionario();
                                                    obeCategoriaFuncionario.Categoriafuncionarioid = drd.GetInt16(posCategoriafuncionarioid);
                                                    obeCategoriaFuncionario.DescripcionCorta = drd.GetString(posDescripcionCortaCatFun);
                                                    obeCategoriaFuncionario.DescripcionLarga = drd.GetString(posDescripcionLargaCatFun);
                                                    lbeCategoriaFuncionario.Add(obeCategoriaFuncionario);
                                                }
                                                obeGenerales.ListaCategoriaFuncionario = lbeCategoriaFuncionario;
                                                if (drd.NextResult())
                                                {
                                                    // GENERO PERSONA (PARAMETRO)
                                                    List<beParametro> lbeParametroGenero = new List<beParametro>();
                                                    int posParametroidGenero = drd.GetOrdinal("PARA_GENERO_ID");
                                                    int posGrupoGenero = drd.GetOrdinal("PARA_GENERO_GRUPO");
                                                    int posDescripcionGenero = drd.GetOrdinal("PARA_GENERO_DESC");
                                                    int posValorGenero = drd.GetOrdinal("PARA_GENERO_VALOR");
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
                                                            // DOCUMENTO DE IDENTIDAD
                                                            List<beDocumentoIdentidad> lbeDocumentoIdentidad = new List<beDocumentoIdentidad>();
                                                            int posTipodocumentoidentidadid = drd.GetOrdinal("DOCIDENT_ID");
                                                            int posDescripcionCortaDocIdent = drd.GetOrdinal("DOCIDENT_DESCORTA");
                                                            int posDescripcionLargaDocIdent = drd.GetOrdinal("DOCIDENT_DESCLARGA");
                                                            beDocumentoIdentidad obeDocumentoIdentidad;
                                                            while (drd.Read())
                                                            {
                                                                obeDocumentoIdentidad = new beDocumentoIdentidad();
                                                                obeDocumentoIdentidad.Tipodocumentoidentidadid = drd.GetInt16(posTipodocumentoidentidadid);
                                                                obeDocumentoIdentidad.DescripcionCorta = drd.GetString(posDescripcionCortaDocIdent);
                                                                obeDocumentoIdentidad.DescripcionLarga = drd.GetString(posDescripcionLargaDocIdent);
                                                                lbeDocumentoIdentidad.Add(obeDocumentoIdentidad);
                                                            }
                                                            obeGenerales.ListaDocumentoIdentidad = lbeDocumentoIdentidad;
                                                            if (drd.NextResult())
                                                            {
                                                                // CONTINENTE
                                                                List<beContinente> lbeContinente = new List<beContinente>();
                                                                int posContinenteid = drd.GetOrdinal("CONTI_ID");
                                                                int posNombreConti = drd.GetOrdinal("CONTI_NOMBRE");
                                                                beContinente obeContinente;
                                                                while (drd.Read())
                                                                {
                                                                    obeContinente = new beContinente();
                                                                    obeContinente.Continenteid = drd.GetInt16(posContinenteid);
                                                                    obeContinente.Nombre = drd.GetString(posNombreConti);
                                                                    lbeContinente.Add(obeContinente);
                                                                }
                                                                obeGenerales.ListaContinentes = lbeContinente;
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
                                                                                List<beParametro> beParametroTipoObs = new List<beParametro>();
                                                                                int posParametroidTipoObs = drd.GetOrdinal("PARA_TIPOOBS_ID");
                                                                                int posGrupoTipoObs = drd.GetOrdinal("PARA_TIPOOBS_GRUPO");
                                                                                int posDescripcionTipoObs = drd.GetOrdinal("PARA_TIPOOBS_DESC");
                                                                                int posValorTipoObs = drd.GetOrdinal("PARA_TIPOOBS_VALOR");
                                                                                beParametro obeParametroTipoObs;
                                                                                while (drd.Read())
                                                                                {
                                                                                    obeParametroTipoObs = new beParametro();
                                                                                    obeParametroTipoObs.Parametroid = drd.GetInt16(posParametroidCatOfcoExt);
                                                                                    obeParametroTipoObs.Grupo = drd.GetString(posGrupoCatOfcoExt);
                                                                                    obeParametroTipoObs.Descripcion = drd.GetString(posDescripcionCatOfcoExt);
                                                                                    obeParametroTipoObs.Valor = drd.GetString(posValorCatOfcoExt);
                                                                                    beParametroTipoObs.Add(obeParametroTipoObs);
                                                                                }
                                                                                obeGenerales.TipoObservacion = beParametroTipoObs;
                                                                                if (drd.NextResult())
                                                                                {
                                                                                    List<beParametro> beParametroTipoDup = new List<beParametro>();
                                                                                    int posParametroidTipoDup = drd.GetOrdinal("PARA_TIPODUP_ID");
                                                                                    int posGrupoTipoDup = drd.GetOrdinal("PARA_TIPODUP_GRUPO");
                                                                                    int posDescripcionTipoDup = drd.GetOrdinal("PARA_TIPODUP_DESC");
                                                                                    int posValorTipoDup = drd.GetOrdinal("PARA_TIPODUP_VALOR");
                                                                                    beParametro obeParametroTipoDup;
                                                                                    while (drd.Read())
                                                                                    {
                                                                                        obeParametroTipoDup = new beParametro();
                                                                                        obeParametroTipoDup.Parametroid = drd.GetInt16(posParametroidCatOfcoExt);
                                                                                        obeParametroTipoDup.Grupo = drd.GetString(posGrupoCatOfcoExt);
                                                                                        obeParametroTipoDup.Descripcion = drd.GetString(posDescripcionCatOfcoExt);
                                                                                        obeParametroTipoDup.Valor = drd.GetString(posValorCatOfcoExt);
                                                                                        beParametroTipoDup.Add(obeParametroTipoDup);
                                                                                    }
                                                                                    obeGenerales.TipoDuplicado = beParametroTipoDup;
                                                                                    if (drd.NextResult())
                                                                                    {
                                                                                        List<beUsuario> listaUsuarios = new List<beUsuario>();
                                                                                        int posNDOCIDENT = drd.GetOrdinal("NDOCIDENT");
                                                                                        int posIDUSUARIO = drd.GetOrdinal("IDUSUARIO");
                                                                                        int posALIAS = drd.GetOrdinal("ALIAS");
                                                                                        int posAPEPAT = drd.GetOrdinal("APEPAT");
                                                                                        int posAPEMAT = drd.GetOrdinal("APEMAT");
                                                                                        int posNOMBRES = drd.GetOrdinal("NOMBRES");
                                                                                        int posNOMBRECOMPLETO = drd.GetOrdinal("NOMBRECOMPLETO");
                                                                                        int posRO_ID = drd.GetOrdinal("RO_ID");
                                                                                        int posROL = drd.GetOrdinal("ROL");
                                                                                        int posIDOFICON = drd.GetOrdinal("IDOFICON");
                                                                                        int posOFICINA = drd.GetOrdinal("OFICINA");
                                                                                        int posBLOQUEO_ACTIVA = drd.GetOrdinal("BLOQUEO_ACTIVA");
                                                                                        beUsuario obeUsuario;
                                                                                        while (drd.Read())
                                                                                        {
                                                                                            obeUsuario = new beUsuario();
                                                                                            obeUsuario.Documentonumero = drd.GetString(posNDOCIDENT);
                                                                                            obeUsuario.Usuarioid = drd.GetInt16(posIDUSUARIO);
                                                                                            obeUsuario.Alias = drd.GetString(posALIAS);
                                                                                            obeUsuario.Apellidopaterno = drd.GetString(posAPEPAT);
                                                                                            obeUsuario.Apellidomaterno = drd.GetString(posAPEMAT);
                                                                                            obeUsuario.Nombres = drd.GetString(posNOMBRES);
                                                                                            obeUsuario.NombreCompleto = drd.GetString(posNOMBRECOMPLETO);
                                                                                            obeUsuario.Rol_Id = drd.GetInt16(posRO_ID);
                                                                                            obeUsuario.Rol = drd.GetString(posROL);
                                                                                            obeUsuario.idOficinaConsular = drd.GetInt16(posIDOFICON);
                                                                                            obeUsuario.NombreOficinaConsular = drd.GetString(posOFICINA);
                                                                                            obeUsuario.BloqueoActiva = drd.GetBoolean(posBLOQUEO_ACTIVA);
                                                                                            listaUsuarios.Add(obeUsuario);
                                                                                        }
                                                                                        obeGenerales.ListaUsuarios = listaUsuarios;
                                                                                        if (drd.NextResult())
                                                                                        {
                                                                                            List<beRolconfiguracion> listaRoles = new List<beRolconfiguracion>();
                                                                                            int posROL_ID = drd.GetOrdinal("ROL_ID");
                                                                                            int posROL_NOMBRE = drd.GetOrdinal("ROL_NOMBRE");
                                                                                            beRolconfiguracion obeRolconfiguracion;
                                                                                            while (drd.Read())
                                                                                            {
                                                                                                obeRolconfiguracion = new beRolconfiguracion();
                                                                                                obeRolconfiguracion.Rolconfiguracionid = drd.GetInt16(posROL_ID);
                                                                                                obeRolconfiguracion.Nombre = drd.GetString(posROL_NOMBRE);
                                                                                                listaRoles.Add(obeRolconfiguracion);
                                                                                            }
                                                                                            obeGenerales.ListaRoles = listaRoles;
                                                                                            if (drd.NextResult())
                                                                                            {
                                                                                                List<beParametro> listaTipoEntrada = new List<beParametro>();
                                                                                                int posPARA_TIPOENT_ID = drd.GetOrdinal("PARA_TIPOENT_ID");
                                                                                                int posPARA_TIPOENT_GRUPO = drd.GetOrdinal("PARA_TIPOENT_GRUPO");
                                                                                                int posPARA_TIPOENTP_DESC = drd.GetOrdinal("PARA_TIPOENTP_DESC");
                                                                                                int posPARA_TIPOENT_VALOR = drd.GetOrdinal("PARA_TIPOENT_VALOR");
                                                                                                beParametro obeParametroTipoEnt;
                                                                                                while (drd.Read())
                                                                                                {
                                                                                                    obeParametroTipoEnt = new beParametro();
                                                                                                    obeParametroTipoEnt.Parametroid = drd.GetInt16(posPARA_TIPOENT_ID);
                                                                                                    obeParametroTipoEnt.Grupo = drd.GetString(posPARA_TIPOENT_GRUPO);
                                                                                                    obeParametroTipoEnt.Descripcion = drd.GetString(posPARA_TIPOENTP_DESC);
                                                                                                    obeParametroTipoEnt.Valor = drd.GetString(posPARA_TIPOENT_VALOR);
                                                                                                    listaTipoEntrada.Add(obeParametroTipoEnt);
                                                                                                }
                                                                                                obeGenerales.TipoEntrada = listaTipoEntrada;
                                                                                                if (drd.NextResult())
                                                                                                {
                                                                                                    List<beParametro> listaTipoAdjunto = new List<beParametro>();
                                                                                                    int posPARA_TIPOADJ_ID = drd.GetOrdinal("PARA_TIPOADJ_ID");
                                                                                                    int posPARA_TIPOADJ_GRUPO = drd.GetOrdinal("PARA_TIPOADJ_GRUPO");
                                                                                                    int posPARA_TIPOADJ_DESC = drd.GetOrdinal("PARA_TIPOADJ_DESC");
                                                                                                    int posPARA_TIPOADJ_VALOR = drd.GetOrdinal("PARA_TIPOADJ_VALOR");
                                                                                                    beParametro obeParametroTipoAdj;
                                                                                                    while (drd.Read())
                                                                                                    {
                                                                                                        obeParametroTipoAdj = new beParametro();
                                                                                                        obeParametroTipoAdj.Parametroid = drd.GetInt16(posPARA_TIPOADJ_ID);
                                                                                                        obeParametroTipoAdj.Grupo = drd.GetString(posPARA_TIPOADJ_GRUPO);
                                                                                                        obeParametroTipoAdj.Descripcion = drd.GetString(posPARA_TIPOADJ_DESC);
                                                                                                        obeParametroTipoAdj.Valor = drd.GetString(posPARA_TIPOADJ_VALOR);
                                                                                                        listaTipoAdjunto.Add(obeParametroTipoAdj);
                                                                                                    }
                                                                                                    obeGenerales.TipoAdjunto = listaTipoAdjunto;
                                                                                                    if (drd.NextResult())
                                                                                                    {
                                                                                                        List<beParametro> listaTipoArchivo = new List<beParametro>();
                                                                                                        int posPARA_TIPOARC_ID = drd.GetOrdinal("PARA_TIPOARC_ID");
                                                                                                        int posPARA_TIPOARC_GRUPO = drd.GetOrdinal("PARA_TIPOARC_GRUPO");
                                                                                                        int posPARA_TIPOARC_DESC = drd.GetOrdinal("PARA_TIPOARC_DESC");
                                                                                                        int posPARA_TIPOARC_VALOR = drd.GetOrdinal("PARA_TIPOARC_VALOR");
                                                                                                        beParametro obeParametroTipoArc;
                                                                                                        while (drd.Read())
                                                                                                        {
                                                                                                            obeParametroTipoArc = new beParametro();
                                                                                                            obeParametroTipoArc.Parametroid = drd.GetInt16(posPARA_TIPOARC_ID);
                                                                                                            obeParametroTipoArc.Grupo = drd.GetString(posPARA_TIPOARC_GRUPO);
                                                                                                            obeParametroTipoArc.Descripcion = drd.GetString(posPARA_TIPOARC_DESC);
                                                                                                            obeParametroTipoArc.Valor = drd.GetString(posPARA_TIPOARC_VALOR);
                                                                                                            listaTipoArchivo.Add(obeParametroTipoArc);
                                                                                                        }
                                                                                                        obeGenerales.TipoArchivo = listaTipoArchivo;
                                                                                                        if (drd.NextResult())
                                                                                                        {
                                                                                                            // ESTADOS
                                                                                                            List<beEstado> lbeEstadoAdjunto = new List<beEstado>();
                                                                                                            int posESTADO_ADJUNTO_ID = drd.GetOrdinal("ESTADO_ADJUNTO_ID");
                                                                                                            int posESTADO_ADJUNTO_DESCRIPCION = drd.GetOrdinal("ESTADO_ADJUNTO_DESCRIPCION");
                                                                                                            int posESTADO_ADJUNTO_GRUPO = drd.GetOrdinal("ESTADO_ADJUNTO_GRUPO");
                                                                                                            beEstado obeEstadoAdjunto;
                                                                                                            while (drd.Read())
                                                                                                            {
                                                                                                                obeEstadoAdjunto = new beEstado();
                                                                                                                obeEstadoAdjunto.Estadoid = drd.GetInt16(posESTADO_ADJUNTO_ID);
                                                                                                                obeEstadoAdjunto.DescripcionCorta = drd.GetString(posESTADO_ADJUNTO_DESCRIPCION);
                                                                                                                obeEstadoAdjunto.Grupo = drd.GetString(posESTADO_ADJUNTO_GRUPO);
                                                                                                                lbeEstadoAdjunto.Add(obeEstadoAdjunto);
                                                                                                            }
                                                                                                            obeGenerales.ListaEstadosAdjunto = lbeEstadoAdjunto;
                                                                                                            if (drd.NextResult())
                                                                                                            {
                                                                                                                // ESTADOS
                                                                                                                List<beEstado> lbeEstadoSolicitud = new List<beEstado>();
                                                                                                                int posESTADO_SOL_ID = drd.GetOrdinal("ESTADO_SOL_ID");
                                                                                                                int posESTADO_SOL_DESCRIPCION = drd.GetOrdinal("ESTADO_SOL_DESCRIPCION");
                                                                                                                int posESTADO_SOL_GRUPO = drd.GetOrdinal("ESTADO_SOL_GRUPO");
                                                                                                                beEstado obeEstadoSolicitud;
                                                                                                                while (drd.Read())
                                                                                                                {
                                                                                                                    obeEstadoSolicitud = new beEstado();
                                                                                                                    obeEstadoSolicitud.Estadoid = drd.GetInt16(posESTADO_ADJUNTO_ID);
                                                                                                                    obeEstadoSolicitud.DescripcionCorta = drd.GetString(posESTADO_ADJUNTO_DESCRIPCION);
                                                                                                                    obeEstadoSolicitud.Grupo = drd.GetString(posESTADO_ADJUNTO_GRUPO);
                                                                                                                    lbeEstadoSolicitud.Add(obeEstadoSolicitud);
                                                                                                                }
                                                                                                                obeGenerales.ListaEstadosSolicitud = lbeEstadoSolicitud;
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
                                                                                                                        // ESTADOS
                                                                                                                        List<beEstado> lbeEstadoActaObs = new List<beEstado>();
                                                                                                                        int posESTADO_ACTAOBS_ID = drd.GetOrdinal("ESTADO_ACTAOBS_ID");
                                                                                                                        int posESTADO_ACTAOBS_DESCRIPCION = drd.GetOrdinal("ESTADO_ACTAOBS_DESCRIPCION");
                                                                                                                        int posESTADO_ACTAOBS_GRUPO = drd.GetOrdinal("ESTADO_ACTAOBS_GRUPO");
                                                                                                                        beEstado obeEstadoActaObs;
                                                                                                                        while (drd.Read())
                                                                                                                        {
                                                                                                                            obeEstadoActaObs = new beEstado();
                                                                                                                            obeEstadoActaObs.Estadoid = drd.GetInt16(posESTADO_ACTAOBS_ID);
                                                                                                                            obeEstadoActaObs.DescripcionCorta = drd.GetString(posESTADO_ACTAOBS_DESCRIPCION);
                                                                                                                            obeEstadoActaObs.Grupo = drd.GetString(posESTADO_ACTAOBS_GRUPO);
                                                                                                                            lbeEstadoActaObs.Add(obeEstadoActaObs);
                                                                                                                        }
                                                                                                                        obeGenerales.ListaEstadosActaObs = lbeEstadoActaObs;
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
                                                                                                                                                obeDocumentoIdentidadRegLinea.Tipodocumentoidentidadid = drd.GetInt16(posTipodocumentoidentidadid);
                                                                                                                                                obeDocumentoIdentidadRegLinea.DescripcionCorta = drd.GetString(posDescripcionCortaDocIdent);
                                                                                                                                                obeDocumentoIdentidadRegLinea.DescripcionLarga = drd.GetString(posDescripcionLargaDocIdent);
                                                                                                                                                obeDocumentoIdentidadRegLinea.PaisId = drd.GetInt16(posDOCIDENT_PAIS);
                                                                                                                                                ListaDocumentoIdentidadRegLinea.Add(obeDocumentoIdentidadRegLinea);
                                                                                                                                            }
                                                                                                                                            obeGenerales.ListaDocumentoIdentidadRegLinea = ListaDocumentoIdentidadRegLinea;
                                                                                                                                            if (drd.NextResult())
                                                                                                                                            {
                                                                                                                                                // estados
                                                                                                                                                List<beMensajeEstado> ListaMensajeEstado= new List<beMensajeEstado>();
                                                                                                                                                int posPARA_ESTADOID = drd.GetOrdinal("PARA_ESTADOID");
                                                                                                                                                int posPARA_DESCRIPCIONCORTA  = drd.GetOrdinal("PARA_DESCRIPCIONCORTA");
                                                                                                                                                int posPARA_MENSAJE = drd.GetOrdinal("PARA_MENSAJE");
                                                                                                                                                beMensajeEstado obeMensaje;
                                                                                                                                                while (drd.Read())
                                                                                                                                                {
                                                                                                                                                    obeMensaje = new beMensajeEstado();
                                                                                                                                                    obeMensaje.Estadoid = drd.GetInt16(posPARA_ESTADOID);
                                                                                                                                                    obeMensaje.EstadoDesc = drd.GetString(posPARA_DESCRIPCIONCORTA);
                                                                                                                                                    obeMensaje.Mensaje = drd.GetString(posPARA_MENSAJE);
                                                                                                                                                    ListaMensajeEstado.Add(obeMensaje);
                                                                                                                                                }
                                                                                                                                                obeGenerales.ListaMensajeEstado = ListaMensajeEstado;


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
