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
						WHERE RM.reCi_iActuacionDetalleId = 190599 AND NOT RESI.resi_iResidenciaId IS NULL  
						GROUP BY 
						p.pers_iPersonaId,
						RESI.resi_iResidenciaId,
						AP.acpa_iActuacionDetalleId,
						CASE WHEN PERE.PERE_dFechaModificacion IS NULL THEN PERE.PERE_dFechaCreacion ELSE  PERE.PERE_dFechaModificacion END
						