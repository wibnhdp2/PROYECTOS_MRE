USE [BD_SGAC]
GO

SELECT ancu_iActoNotarialId,
lower(ancu_vCuerpo) as ancu_vCuerpo       
FROM 
PN_REGISTRO.RE_ACTONOTARIALCUERPO
WHERE 
--ancu_iActoNotarialId = @ancu_iActoNotarialId AND 
ancu_cEstado = 'A'