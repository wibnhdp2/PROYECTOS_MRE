USE [BD_SGAC]
GO
/****** Object:  StoredProcedure [PN_REGISTRO].[USP_RE_REGISTROCIVIL_OBTENER_PARTICIPANTES]    Script Date: 21/04/2022 15:54:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


declare
	@reci_iActuacionDetalleId bigint = 190601


		DECLARE @COUNT INT
		SET @COUNT = ( select COUNT(acpa_iActuacionParticipanteId) from PN_REGISTRO.RE_ACTUACIONPARTICIPANTE where acpa_iActuacionDetalleId = @reci_iActuacionDetalleId and acpa_cEstado = 'A'
		AND acpa_sTipoParticipanteId IN (4812,4813,4814,4815,4816,4821,4824,4832,4833,4834,4835))

/*

			SELECT	AP.acpa_iActuacionParticipanteId iActuacionParticipanteId, p.pers_iPersonaId iPersonaId, ISNULL(P.pers_vApellidoPaterno,'') vApellidoPaterno
			,ISNULL(P.pers_vApellidoMaterno,'') vApellidoMaterno, ISNULL(P.pers_vNombres,'') vNombres, ap.acpa_sTipoParticipanteId sTipoParticipanteId
			,pa.para_vDescripcion vTipoParticipante, pei.peid_sDocumentoTipoId sDocumentoTipoId, DI.doid_vDescripcionCorta vDocumentoTipo
			,pei.peid_vDocumentoNumero vDocumentoNumero, di.doid_vDescripcionCorta + ' - ' + pei.peid_vDocumentoNumero vDocumentoCompleto
			,AP.acpa_sTipoDatoId sTipoDatoId
			,			-- Lo adicione para civil "Frans Simon".
			AP.acpa_sTipoVinculoId sTipoVinculoId,  	-- Lo adicione para civil "Frans Simon".
			P.pers_vApellidoPaterno + ' ' + P.pers_vApellidoMaterno + ',' + P.pers_vNombres vNombreCompleto, 	-- Lo adicione para civil "Frans Simon".
			ISNULL(p.pers_sNacionalidadId,0) pers_sNacionalidadId, ISNULL(p.pers_sGeneroId,0) pers_sGeneroId,
			CASE WHEN not ap.acpa_sTipoParticipanteId in (4812,4814,4815,4816,4821,4824,4832,4833,4834,4835) then  ISNULL(p.pers_cNacimientoLugar,'')  else null end pers_cNacimientoLugar
			, pers_dNacimientoFecha,
			p.pers_bFallecidoFlag,
			p.pers_cUbigeoDefuncion,
			p.pers_dFechaDefuncion,
			p.pers_sEstadoCivilId,
			
			case when  ap.acpa_sTipoParticipanteId in (4812,4813,4814,4815,4816,4821,4824,4832,4833,4834,4835) then RESI.resi_cResidenciaUbigeo    else null  end resi_cResidenciaUbigeo,
			case when  ap.acpa_sTipoParticipanteId in (4812,4813,4814,4815,4816,4821,4824,4832,4833,4834,4835) then RESI.resi_iResidenciaId else null  end resi_iResidenciaId,
			case when  ap.acpa_sTipoParticipanteId in (4812,4813,4814,4815,4816,4821,4824,4832,4833,4834,4835) then RESI.resi_vResidenciaDireccion else null  end resi_vResidenciaDireccion

		FROM PN_REGISTRO.RE_REGISTROCIVIL RM
		INNER JOIN PN_REGISTRO.RE_ACTUACIONPARTICIPANTE AP ON RM.reci_iActuacionDetalleId = AP.acpa_iActuacionDetalleId AND acpa_cEstado = 'A'
		LEFT JOIN PS_SISTEMA.SI_PARAMETRO Pa ON AP.acpa_sTipoParticipanteId = Pa.para_sParametroId
		LEFT JOIN PN_REGISTRO.RE_PERSONA P ON AP.acpa_iPersonaId = P.pers_iPersonaId 
		LEFT JOIN PN_REGISTRO.RE_PERSONAIDENTIFICACION PEI ON P.pers_iPersonaId = PEI.peid_iPersonaId AND PEI.peid_bActivoEnRune = 1
		
		LEFT JOIN PN_REGISTRO.RE_PERSONARESIDENCIA PERE ON PERE.pere_iPersonaId = P.pers_iPersonaId
		LEFT JOIN (
					SELECT
					top (@COUNT)
					TEMP01.iPersonaId,
					TEMP01.acpa_iActuacionDetalleId,
					MAX(TEMP01.dFecha) DFECHA,
					MAX(TEMP01.resi_iResidenciaId)resi_iResidenciaId
					FROM
					(
					*/

					SELECT	
							p.pers_iPersonaId iPersonaId,
							RESI.resi_iResidenciaId,
							AP.acpa_iActuacionDetalleId,
							CASE WHEN PERE.PERE_dFechaModificacion IS NULL THEN PERE.PERE_dFechaCreacion ELSE  PERE.PERE_dFechaModificacion END dFecha	
							FROM PN_REGISTRO.RE_REGISTROCIVIL RM
							INNER JOIN PN_REGISTRO.RE_ACTUACIONPARTICIPANTE AP ON RM.reci_iActuacionDetalleId = AP.acpa_iActuacionDetalleId AND acpa_cEstado = 'A'
							INNER JOIN PN_REGISTRO.RE_ACTUACIONDETALLE ACDE ON ACDE.acde_iActuacionDetalleId = AP.acpa_iActuacionDetalleId 
 							INNER JOIN PN_REGISTRO.RE_ACTUACION ACT ON ACT.actu_iActuacionId =  ACDE.acde_iActuacionId
							LEFT JOIN PS_SISTEMA.SI_PARAMETRO Pa ON AP.acpa_sTipoParticipanteId = Pa.para_sParametroId
							LEFT JOIN PN_REGISTRO.RE_PERSONA P ON AP.acpa_iPersonaId = P.pers_iPersonaId --AND ACT.actu_iPersonaRecurrenteId = P.pers_iPersonaId
							LEFT JOIN PN_REGISTRO.RE_PERSONAIDENTIFICACION PEI ON P.pers_iPersonaId = PEI.peid_iPersonaId AND PEI.peid_bActivoEnRune = 1
							LEFT JOIN PN_REGISTRO.RE_PERSONARESIDENCIA PERE ON PERE.pere_iPersonaId = P.pers_iPersonaId
							LEFT JOIN PN_REGISTRO.RE_RESIDENCIA RESI ON RESI.resi_iResidenciaId = PERE.pere_iResidenciaId AND RESI.resi_sResidenciaTipoId = 2252 
							LEFT JOIN SC_MAESTRO.MA_DOCUMENTO_IDENTIDAD DI ON PEI.peid_sDocumentoTipoId = DI.doid_sTipoDocumentoIdentidadId
						WHERE RM.reCi_iActuacionDetalleId = @reci_iActuacionDetalleId AND NOT RESI.resi_iResidenciaId IS NULL  

						/*
						GROUP BY 
						p.pers_iPersonaId,
						RESI.resi_iResidenciaId,
						AP.acpa_iActuacionDetalleId,
						CASE WHEN PERE.PERE_dFechaModificacion IS NULL THEN PERE.PERE_dFechaCreacion ELSE  PERE.PERE_dFechaModificacion END
						*/

						/*
						)TEMP01
						where TEMP01.iPersonaId in (select acpa_iPersonaId from PN_REGISTRO.RE_ACTUACIONPARTICIPANTE where acpa_iActuacionDetalleId = @reci_iActuacionDetalleId and acpa_cEstado = 'A'
						AND acpa_sTipoParticipanteId IN (4812,4813,4814,4815,4816,4821,4824,4832,4833,4834,4835))
						GROUP BY 
						TEMP01.iPersonaId,
						TEMP01.acpa_iActuacionDetalleId
						--TEMP01.resi_iResidenciaId
						order by 
						dFecha desc
						 
					)TEMP02 ON TEMP02.iPersonaId = P.pers_iPersonaId AND TEMP02.acpa_iActuacionDetalleId = AP.acpa_iActuacionDetalleId
					LEFT JOIN PN_REGISTRO.RE_RESIDENCIA RESI ON RESI.resi_iResidenciaId = TEMP02.resi_iResidenciaId  AND RESI.resi_sResidenciaTipoId = 2252 
		left JOIN SC_MAESTRO.MA_DOCUMENTO_IDENTIDAD DI ON PEI.peid_sDocumentoTipoId = DI.doid_sTipoDocumentoIdentidadId
	WHERE RM.reCi_iActuacionDetalleId = @reci_iActuacionDetalleId
	*/