USE [BD_SGAC]
GO

declare
@acpa_iActuacionDetalleId bigint = 190599

		declare @conCUI char(1)
		--SET @conCUI = isnull((
		--Select reci_cConCUI from PN_REGISTRO.RE_REGISTROCIVIL (nolock)
		--				where reci_iActuacionDetalleId = @acpa_iActuacionDetalleId
		--				),'N')

Select reci_cConCUI,* from PN_REGISTRO.RE_REGISTROCIVIL (nolock)
						where reci_iActuacionDetalleId = @acpa_iActuacionDetalleId AND reci_cEstado = 'A'