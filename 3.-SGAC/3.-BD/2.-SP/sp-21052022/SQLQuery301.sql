	USE [BD_SGAC]
	GO
	DECLARE @ancu_iActoNotarialId bigint = 122727

	SELECT 
		ancu_vCuerpo       
	FROM 
		PN_REGISTRO.RE_ACTONOTARIALCUERPO
	WHERE 
		ancu_iActoNotarialId = @ancu_iActoNotarialId AND 
		ancu_cEstado = 'A'