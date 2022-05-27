USE [BD_SGAC]
GO


SELECT actu_iActuacionId,* FROM PN_REGISTRO.RE_ACTUACION WITH(NOLOCK) 
WHERE actu_sOficinaConsularId = 30
AND actu_iPersonaRecurrenteId = 243836--723422


SELECT * FROM PN_REGISTRO.RE_ACTONOTARIAL WITH(NOLOCK) 
WHERE acno_iActuacionId = 191617 --191616
AND acno_iActoNotarialId = 122727


/*
AND acno_sTipoActoNotarialId = 8001
AND acno_sSubTipoActoNotarialId = 8050
AND acno_sOficinaConsularId = 30
AND acno_IFuncionarioAutorizadorId = 777
AND acno_bDigitalizadoFlag = 0
AND acno_sEstadoId = 98
AND acno_sAccionSubTipoActoNotarialId = 8421
AND acno_iActoNotarialId = 122726
*/

SELECT * FROM PN_REGISTRO.RE_ACTONOTARIALSEGUIMIENTO WITH(NOLOCK) 
WHERE anse_iActoNotarialId = 122727--122726
AND anse_sEstadoId = 98

SELECT * FROM PS_SISTEMA.SI_LIBRO WITH(NOLOCK)

--anse_iActoNotarialSeguimientoId = 112853
--anse_iActoNotarialSeguimientoId = 112854







