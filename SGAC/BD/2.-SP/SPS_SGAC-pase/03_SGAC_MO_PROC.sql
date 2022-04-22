USE [BD_SGAC]
GO
/****** Object:  StoredProcedure [PN_REGISTRO].[USP_RE_ACTUACIONPARTICIPANTE_ADICIONAR]    Script Date: 22/03/2022 11:19:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--====================================================================================================
-- Nombre: PN_REGISTRO.USP_RE_ACTUACIONPARTICIPANTE_ADICIONAR
-- Descripción: ADICIONAR REGISTRO EN LA TABLA RE_ACTUACIONPARTICIPANTE
-- Fecha Creación:		 26/12/2014
-- Fecha Modificacion:	 16/05/2015
-- Usuario Modificacion: Héctor Vásquez
-- Descripción Parámetros:
-- Parámetro(s):
--	@acpa_iActuacionDetalleId		bigint			Id de detalle de actuación,	
--@acpa_iPersonaId					bigint			Id de persona,	
--@acpa_sTipoParticipanteId			smallint		Id tipo participante,
--@acpa_sTipoDatoId					smallint		Id de tipo de dato,
--@acpa_sTipoVinculoId				smallint		Id tipo vinculo,
--@acpa_sUsuarioCreacion			smallint		Usuario creación del registro,
--@acpa_vIPCreacion					varchar(50)		Ip de la PC donde se creó el registro,
--@acpa_sOficinaConsularId			smallint		Id de oficina consular,
--@acpa_vHostName					varchar(20)		Nombre de Hostname,
--@sTipoPersonaId					smallint		Id de tipo de persona,
--@sTipoDocumentoId					smallint		Id de tipo de documento,
--@vNumeroDocumento					varchar(20)		Numero de documento,	
--@sNacionalidadId					smallint		Id de nacionalidad,
--@vNombres							varchar(200)	Nombres del participante,
--@vPrimerApellido					varchar(100)	Apellido paterno del participante,
--@vSegundoApellido					varchar(100)	Apellido materno del participante,
--@vDireccion						varchar(100)	Direccion del participante,
--@cUbigeo							char(6)			Codigo de ubigeo,
--@ICentroPobladoId					int				Id de centro poblado,
--@pers_dNacimientoFecha			datetime		Fecha de nacimiento del particpante,
--@pers_sGeneroId					smallint		Id del genero del participante,
--@pers_cNacimientoLugar			char(6)			Lugar de nacimiento del particpante,
--@pers_sEstadoCivilId				int				Permite Guardar el Estado Civil Cuando sea Don o Doña en Matrimonio
--@acpa_iActuacionParticipanteId	bigint out		Retorna Id de actuación del particpante
--@acpa_iPersonaId					bigint out		Retorna Id de persona
-- Autor: Margarita Díaz
-- Version: 2.0
-- Cambios Importantes:
--Autor: Miguel Márquez Beltrán
--Fecha: 28/09/2016
--Objetivo: Si el participante existe obtener el acpa_iActuacionParticipanteId
--Autor: Miguel Márquez Beltrán
--Fecha: 06/10/2016
--Objetivo: Se realizo la busqueda del tipo y número de documento para otros documentos.
------------------------------------------------------------------------------------------------------
--FECHA			AUTOR		MOTIVO
--17/08/2018	MMARQUEZ	Actualizar la oficina consular del registro persona
--====================================================================================================
-- MODIFICADO POR: MIGUEL MÁRQUEZ BELTRÁN
-- FECHA DE MODIFICACIÓN: 14/11/2018
-- MOTIVO: ACTUALIZAR DATOS DE LA RESIDENCIA DE ACUERDO AL TIPO DE PARTICIPANTE
--====================================================================================================
-- MODIFICADO POR: MIGUEL MÁRQUEZ BELTRÁN
-- FECHA DE MODIFICACIÓN: 17/04/2019
-- MOTIVO: SE COMENTARON LAS LINEAS DEL 213 AL 230.
--====================================================================================================
-- MODIFICADO POR: MIGUEL MÁRQUEZ BELTRÁN
-- FECHA DE MODIFICACIÓN: 01/08/2019
-- MOTIVO: SE ADICIONO AL FILTRO DE BÚSQUEDA A: IGUAL TITULAR
--====================================================================================================
-- MODIFICADO POR: JONATAN SILVA CACHAY
-- FECHA DE MODIFICACIÓN: 26/03/2020
-- MOTIVO: SE CORRIGIO LA FORMA EN COMO SE OBTIENE LA DIRECCIÓN
--		   AL MOMENTO DE REGISTRAR UNA NUEVA PERSONA SE AGREGA QUE TAMBIEN ACTUALICE EL ESTADO CIVIL
--====================================================================================================
-- FECHA			AUTOR		MOTIVO
--====================================================================================================
-- 22/03/2022		MMARQUEZ	SE REGISTRA LA NACIONALIDAD LA PRIMERA VEZ TOMANDO COMO PARAMETRO EL @pers_spaisid.
--								CUANDO SEA PARTICIPANTE DE TIPO DE NACIMIENTO MADRE 
--								SE REGISTRARÁ LA DIRECCIÓN DE RESIDENCIA PARA EL TITULAR CON CUI.
--====================================================================================================

ALTER PROCEDURE [PN_REGISTRO].[USP_RE_ACTUACIONPARTICIPANTE_ADICIONAR]
@acpa_iActuacionDetalleId bigint,	
@acpa_sTipoParticipanteId smallint,
@acpa_sTipoDatoId smallint,
@acpa_sTipoVinculoId smallint,
@acpa_sUsuarioCreacion smallint,
@acpa_vIPCreacion varchar(50),
@acpa_sOficinaConsularId smallint,
@acpa_vHostName varchar(20),
@sTipoPersonaId smallint = null,
@sTipoDocumentoId smallint = null,
@vNumeroDocumento varchar(20) = null,
@sNacionalidadId smallint,
@vNombres varchar(200),
@vPrimerApellido varchar(100),
@vSegundoApellido varchar(100) = null,
@vDireccion varchar(100),
@cUbigeo char(6),
@ICentroPobladoId int,
-- 
@pers_dNacimientoFecha  datetime = null,
@pers_sGeneroId			smallint = null,
@pers_cNacimientoLugar  char(6)  = null,
--
@pers_bFallecidoFlag	bit = 0,
@pers_dFechaDefuncion	datetime = null,
@pers_cUbigeoDefuncion	char(6) = null,
--
@acpa_iResidenciaId bigint = null,
@pers_sEstadoCivilId int = null,

@acpa_iActuacionParticipanteId bigint out,
@acpa_iPersonaId			   bigint  out,
@pers_spaisid				   smallint = NULL
AS
BEGIN
	DECLARE @datFechaRegistro datetime;	
	DECLARE @sTipoDoc	smallint
	DECLARE  @resi_iResidenciaId	BIGINT

	SET NOCOUNT ON;
	BEGIN TRY
		----------------------------------------------------------------------------------------
		--Autor: Miguel Márquez Beltrán
		--Fecha: 06/10/2016
		--Objetivo: Se realizo la busqueda del tipo y número de documento para otros documentos.
		-----------------------------------------------------------------------------------------
	    SET @sTipoDoc = ISNULL((select doid_sTipoDocumentoIdentidadId from [SC_MAESTRO].[MA_DOCUMENTO_IDENTIDAD] (nolock)
								where upper(doid_vDescripcionCorta) = 'OTROS' and doid_cEstado='A'),0)
	    ----------------------------------------------------------------------------------------
		
		SET @datFechaRegistro = PS_ACCESORIOS.FN_OBTENER_FECHAACTUAL(@acpa_sOficinaConsularId);
		IF (@acpa_iPersonaId = 0)
		BEGIN	
			
			DECLARE @EXISTEPERSONA_SN BIT = 0

			DECLARE @STIPO_PARTICIPANTE_IGUAL_MADRE	  smallint
			DECLARE @STIPO_PARTICIPANTE_IGUAL_PADRE	  smallint
			DECLARE @STIPO_PARTICIPANTE_IGUAL_TITULAR smallint

			SET @STIPO_PARTICIPANTE_IGUAL_MADRE = (SELECT  PAR.para_sParametroId FROM [PS_SISTEMA].[SI_PARAMETRO] PAR (nolock)
													WHERE PAR.para_vDescripcion = 'IGUAL MADRE' AND PAR.para_cEstado = 'A')
			SET @STIPO_PARTICIPANTE_IGUAL_PADRE = (SELECT  PAR.para_sParametroId FROM [PS_SISTEMA].[SI_PARAMETRO] PAR (nolock)
													WHERE PAR.para_vDescripcion = 'IGUAL PADRE' AND PAR.para_cEstado = 'A')
			SET @STIPO_PARTICIPANTE_IGUAL_TITULAR = (SELECT  PAR.para_sParametroId FROM [PS_SISTEMA].[SI_PARAMETRO] PAR (nolock)
													WHERE PAR.para_vDescripcion = 'IGUAL TITULAR' AND PAR.para_cEstado = 'A')

			
			IF (@acpa_sTipoDatoId=@STIPO_PARTICIPANTE_IGUAL_MADRE OR @acpa_sTipoDatoId=@STIPO_PARTICIPANTE_IGUAL_PADRE OR  @acpa_sTipoDatoId=@STIPO_PARTICIPANTE_IGUAL_TITULAR)
			BEGIN
			  SET @EXISTEPERSONA_SN = IIF(NOT EXISTS(SELECT peid_iPersonaIdentificacionId FROM PN_REGISTRO.RE_PERSONAIDENTIFICACION (nolock) 				
													WHERE peid_vDocumentoNumero = @vNumeroDocumento AND peid_sDocumentoTipoId = @sTipoDocumentoId),0,1)
			END

			IF (@EXISTEPERSONA_SN = 0)
				BEGIN				

				EXEC PN_REGISTRO.USP_RE_PERSONA_PARTICIPANTE_ADICIONAR
					@sTipoPersonaId = @sTipoPersonaId,
					@sTipoDocumentoId = @sTipoDocumentoId,
					@vNumeroDocumento = @vNumeroDocumento,
					@sNacionalidadId = @sNacionalidadId,
					@vNombres = @vNombres,
					@vPrimerApellido = @vPrimerApellido,
					@vSegundoApellido = @vSegundoApellido,
					@vDireccion = @vDireccion,
					@cUbigeo = @cUbigeo,
					@ICentroPobladoId = @ICentroPobladoId,
					@vTelefono = '',
					@sUsuarioCreacion = @acpa_sUsuarioCreacion,
					@vIPCreacion = @acpa_vIPCreacion,
					@sOficinaConsularId = @acpa_sOficinaConsularId,
					@vHostName = @acpa_vHostName,
					@iPersonaId = @acpa_iPersonaId OUTPUT,
					@sGeneroId = @pers_sGeneroId,
					@spaisid = @pers_spaisid

				IF ((ISNULL(@acpa_sTipoVinculoId,0) <> 0) AND @acpa_sTipoParticipanteId IN (4812,4813,4832,4833,7962,7963,7964))
				BEGIN
					DECLARE @pefi_iPersonaFilacionId BIGINT = 0;

					EXEC PN_REGISTRO.USP_RE_PERSONAFILIACION_ADICIONAR
						@pefi_iPersonaId = @acpa_iPersonaId,
						@pefi_iFiliadoId = null,
						@pefi_sDocumentoTipoId	=  @sTipoPersonaId,
						@pefi_vNombreFiliacion = '',
						@pefi_vLugarNacimiento = '',
						@pefi_vFechaNacimiento = @pers_dNacimientoFecha,
						@pefi_sNacionalidad = @sNacionalidadId,
						@pefi_sTipoFilacionId = @acpa_sTipoVinculoId,
						@pefi_vNroDocumento = @vNumeroDocumento,
						@pefi_sOficinaConsularId = @acpa_sOficinaConsularId,
						@pefi_sUsuarioCreacion = @acpa_sUsuarioCreacion,
						@pefi_vIPCreacion = @acpa_vIPCreacion,
						@pefi_vHostName = @acpa_vHostName,
						@pefi_iPersonaFilacionId = @pefi_iPersonaFilacionId OUTPUT
				END	
				if(NOT @pers_sEstadoCivilId is null)
				begin
					UPDATE PN_REGISTRO.RE_PERSONA SET 
					pers_sEstadoCivilId = @pers_sEstadoCivilId
					WHERE pers_iPersonaId = @acpa_iPersonaId
				end
			END	
			ELSE
			BEGIN
				----------------------------------------------------------------------------------------
				--Autor: Miguel Márquez Beltrán
				--Fecha: 06/10/2016
				--Objetivo: Se realizo la busqueda del tipo y número de documento para otros documentos.
				-----------------------------------------------------------------------------------------
				IF (@sTipoDocumentoId = @sTipoDoc)
				BEGIN 
					SET @acpa_iPersonaId = ISNULL((SELECT top 1 peid_iPersonaId FROM PN_REGISTRO.RE_PERSONAIDENTIFICACION (nolock)
													WHERE peid_vDocumentoNumero = @vNumeroDocumento AND peid_sDocumentoTipoId not in ('1','2','3')),0)
					set @sTipoDocumentoId = ISNULL((SELECT top 1 peid_sDocumentoTipoId FROM PN_REGISTRO.RE_PERSONAIDENTIFICACION (nolock)
													WHERE peid_vDocumentoNumero = @vNumeroDocumento AND peid_sDocumentoTipoId not in ('1','2','3')),0)
				
					if not exists(SELECT top 1 peid_iPersonaId FROM PN_REGISTRO.RE_PERSONAIDENTIFICACION (nolock)
											WHERE peid_vDocumentoNumero = @vNumeroDocumento AND peid_sDocumentoTipoId not in ('1','2','3'))
					BEGIN
							SET @vNumeroDocumento = '-1'
							SET @acpa_iPersonaId = -1
					END
				END
				ELSE
				BEGIN
					SET @acpa_iPersonaId =	ISNULL((SELECT top 1 peid_iPersonaId FROM PN_REGISTRO.RE_PERSONAIDENTIFICACION (nolock) 
												WHERE peid_vDocumentoNumero = @vNumeroDocumento AND peid_sDocumentoTipoId = @sTipoDocumentoId),0)
				END
				-----------------------------------------------------------------------------------------
			END
		END
		ELSE
		BEGIN
			UPDATE [PN_REGISTRO].[RE_PERSONA]
			SET 
				[pers_sNacionalidadId]=@sNacionalidadId,	
				[pers_sUsuarioModificacion]=@acpa_sUsuarioCreacion,
				[pers_vIPModificacion]=@acpa_vIPCreacion,
				[pers_dFechaModificacion] = @datFechaRegistro,
				[pers_sOficinaConsularId] = @acpa_sOficinaConsularId
			WHERE pers_iPersonaId = @acpa_iPersonaId

			 		
			if(NOT @pers_sEstadoCivilId is null)
				begin
				
					UPDATE PN_REGISTRO.RE_PERSONA SET 
					pers_sEstadoCivilId = @pers_sEstadoCivilId
					WHERE pers_iPersonaId = @acpa_iPersonaId
				end
							

	--------------------------------------------------------------------------------------------------------------------------
	--TIPO DE ACTA: NACIMIENTO
	--PARTICIPANTE: 4812-> PADRE / 4813-> MADRE / 4814-> REGISTRADOR CIVIL / 4815-> DECLARANTE 1 / 4816-> DECLARANTE 2
	--------------------------------------------------------------------------------------------------------------------------
	--TIPO DE ACTA: MATRIMONIO
	--PARTICIPANTE: 4821-> CELEBRANTE / 4824-> REGISTRADOR CIVIL 
	--------------------------------------------------------------------------------------------------------------------------
	--TIPO DE ACTA: DEFUNCIÓN
	--PARTICIPANTE: 4832-> PADRE / 4833-> MADRE / 4834-> REGISTRADOR CIVIL / 4835-> DECLARANTE 
	--------------------------------------------------------------------------------------------------------------------------

		if (@acpa_sTipoParticipanteId = 4812 or @acpa_sTipoParticipanteId = 4813  or @acpa_sTipoParticipanteId = 4814 or @acpa_sTipoParticipanteId = 4815 or @acpa_sTipoParticipanteId = 4816 
		or @acpa_sTipoParticipanteId = 4821 or @acpa_sTipoParticipanteId = 4824 or @acpa_sTipoParticipanteId = 4832 or @acpa_sTipoParticipanteId = 4833 or @acpa_sTipoParticipanteId = 4834 
		or @acpa_sTipoParticipanteId = 4835)
		Begin
			IF (@cUbigeo <> NULL OR @cUbigeo <>'')
			BEGIN
				DECLARE @iResidenciaId INT
				
				if (not exists(select resi_iResidenciaId from PN_REGISTRO.RE_RESIDENCIA (nolock)
								where resi_iResidenciaId in (
										select pere_iResidenciaId from PN_REGISTRO.RE_PERSONARESIDENCIA (nolock) where pere_iPersonaId = @acpa_iPersonaId and pere_cEstado = 'A')
															and resi_sResidenciaTipoId = 2252 and resi_cResidenciaUbigeo = @cUbigeo AND resi_cEstado = 'A'))
				
				BEGIN
					INSERT INTO PN_REGISTRO.RE_RESIDENCIA (
						resi_sResidenciaTipoId, resi_vResidenciaDireccion, resi_vResidenciaTelefono,resi_cResidenciaUbigeo,
						resi_sUsuarioCreacion, resi_vIPCreacion, resi_dFechaCreacion, resi_cEstado)
					VALUES 
						(2252, @vDireccion,'', @cUbigeo, 
						@acpa_sUsuarioCreacion, @acpa_vIPCreacion, @datFechaRegistro,'A')

					SET @iResidenciaId = @@IDENTITY

					EXEC	[PN_REGISTRO].[USP_RE_PERSONARESIDENCIA_ADICIONAR]
						@pere_iPersonaId = @acpa_iPersonaId,
						@pere_iResidenciaId = @iResidenciaId,
						@pere_sOficinaConsularId = @acpa_sOficinaConsularId,
						@pere_sUsuarioCreacion = @acpa_sUsuarioCreacion,
						@pere_vIPCreacion = @acpa_vIPCreacion,
						@pere_vHostName = @acpa_vHostName

					
				END
				ELSE
				BEGIN					
					
					SET @resi_iResidenciaId = ISNULL((SELECT max(pere.pere_iResidenciaId) 
												FROM PN_REGISTRO.RE_PERSONARESIDENCIA pere (nolock)
												INNER JOIN PN_REGISTRO.RE_RESIDENCIA RESI (nolock) on pere.pere_iResidenciaId = RESI.resi_iResidenciaId
												WHERE pere.pere_iPersonaId = @acpa_iPersonaId and RESI.resi_sResidenciaTipoId = 2252 
												and pere.pere_cEstado = 'A' and RESI.resi_cEstado = 'A'),0)
					Select @resi_iResidenciaId
					IF (@resi_iResidenciaId > 0)
					BEGIN
						UPDATE PN_REGISTRO.RE_RESIDENCIA
						SET resi_vResidenciaDireccion = @vDireccion,
							resi_cResidenciaUbigeo = @cUbigeo
						WHERE resi_iResidenciaId = @resi_iResidenciaId
					END
				END
			END
		End

	---------------------------------------------------------------------
	-- Fecha: 22/03/2022
	--Motivo: CUANDO SEA PARTICIPANTE DE TIPO DE NACIMIENTO MADRE 
	--		  SE REGISTRARÁ LA DIRECCIÓN DE RESIDENCIA PARA EL TITULAR 

	---------------------------------------------------------------------
	if (@acpa_sTipoParticipanteId = 4813)
	BEGIN
		declare @iPersonaTitularId bigint
		set @iPersonaTitularId = isnull((Select acpa_iPersonaId From PN_REGISTRO.RE_ACTUACIONPARTICIPANTE (nolock)
									where acpa_iActuacionDetalleId = @acpa_iActuacionDetalleId		
									and acpa_sTipoParticipanteId = 4811 and acpa_cEstado='A'),0)
		declare @conCUI char(1)
		SET @conCUI = isnull((Select reci_cConCUI from PN_REGISTRO.RE_REGISTROCIVIL (nolock)
						where reci_iActuacionDetalleId = @acpa_iActuacionDetalleId),'N')

		if (@iPersonaTitularId > 0 and @conCUI = 'S')
		begin 
			 
			IF NOT EXISTS(SELECT resi_iResidenciaId from PN_REGISTRO.RE_RESIDENCIA (nolock)
																	where resi_iResidenciaId in (
																	select pere_iResidenciaId from PN_REGISTRO.RE_PERSONARESIDENCIA (nolock)
																	where pere_iPersonaId = @iPersonaTitularId and pere_cEstado = 'A')
																	and resi_sResidenciaTipoId = 2252 and resi_cResidenciaUbigeo = @cUbigeo AND resi_cEstado = 'A')
			begin
				INSERT INTO PN_REGISTRO.RE_RESIDENCIA (
						resi_sResidenciaTipoId, resi_vResidenciaDireccion, resi_vResidenciaTelefono,resi_cResidenciaUbigeo,
						resi_sUsuarioCreacion, resi_vIPCreacion, resi_dFechaCreacion, resi_cEstado)
					VALUES 
						(2252, @vDireccion,'', @cUbigeo, 
						@acpa_sUsuarioCreacion, @acpa_vIPCreacion, @datFechaRegistro,'A')

					SET @iResidenciaId = @@IDENTITY

					EXEC	[PN_REGISTRO].[USP_RE_PERSONARESIDENCIA_ADICIONAR]
						@pere_iPersonaId = @iPersonaTitularId,
						@pere_iResidenciaId = @iResidenciaId,
						@pere_sOficinaConsularId = @acpa_sOficinaConsularId,
						@pere_sUsuarioCreacion = @acpa_sUsuarioCreacion,
						@pere_vIPCreacion = @acpa_vIPCreacion,
						@pere_vHostName = @acpa_vHostName
			end
			else
			begin
					SET @resi_iResidenciaId = ISNULL((SELECT MAX(pere.pere_iResidenciaId) 
										FROM PN_REGISTRO.RE_PERSONARESIDENCIA pere (nolock)
										INNER JOIN PN_REGISTRO.RE_RESIDENCIA RESI (nolock) on pere.pere_iResidenciaId = RESI.resi_iResidenciaId
										WHERE pere.pere_iPersonaId = @iPersonaTitularId and RESI.resi_sResidenciaTipoId = 2252 
										and pere.pere_cEstado = 'A' and RESI.resi_cEstado = 'A'),0)
					IF (@resi_iResidenciaId > 0)
					BEGIN
						UPDATE PN_REGISTRO.RE_RESIDENCIA
						SET resi_vResidenciaDireccion = @vDireccion,
							resi_cResidenciaUbigeo = @cUbigeo
						WHERE resi_iResidenciaId = @resi_iResidenciaId
					END
				
			end
		end
	END

	--------------------------------------------------------------------------------
	--TIPO DE ACTA Y PARTICIPANTE:
	--4822-> MATRIMONIO - DON /	4823 -> MATRIMONIO - DOÑA / 4831 -> DEFUNCIÓN - TITULAR
	--------------------------------------------------------------------------------

		if (@acpa_sTipoParticipanteId = 4822 OR @acpa_sTipoParticipanteId = 4823 OR @acpa_sTipoParticipanteId = 4831)
		BEGIN
			IF (@cUbigeo <> NULL OR @cUbigeo <>'')
				BEGIN					
					UPDATE PN_REGISTRO.RE_PERSONA 
						SET [pers_cNacimientoLugar] = @cUbigeo					
					WHERE pers_iPersonaId = @acpa_iPersonaId					
				END
		END   
		
		END
				
		
		if (not exists(SELECT acpa_iActuacionParticipanteId FROM PN_REGISTRO.RE_ACTUACIONPARTICIPANTE (nolock) 
							WHERE acpa_iActuacionDetalleId = @acpa_iActuacionDetalleId
							AND acpa_sTipoParticipanteId = @acpa_sTipoParticipanteId AND acpa_cEstado = 'A'))				
		BEGIN
			INSERT INTO PN_REGISTRO.RE_ACTUACIONPARTICIPANTE
				   (acpa_iActuacionDetalleId, acpa_iPersonaId, acpa_sTipoParticipanteId,
					acpa_sTipoDatoId, acpa_sTipoVinculoId, acpa_sUsuarioCreacion, acpa_vIPCreacion,
					acpa_dFechaCreacion)
			 VALUES
				   (@acpa_iActuacionDetalleId, @acpa_iPersonaId, @acpa_sTipoParticipanteId,
					@acpa_sTipoDatoId, @acpa_sTipoVinculoId, @acpa_sUsuarioCreacion, @acpa_vIPCreacion,
					@datFechaRegistro)

			 SET @acpa_iActuacionParticipanteId = @@IDENTITY 

		------------------------------------------------------------------------------------------------------------------
		--TIPO DE ACTA Y PARTICIPANTE:
		--4811-> NACIMIENTO - TITULAR / 4822-> MATRIMONIO - DON / 4823-> MATRIMONIO - DOÑA / 4831-> DEFUNCIÓN - TITULAR
		------------------------------------------------------------------------------------------------------------------

			if (@acpa_sTipoParticipanteId = 4811  OR @acpa_sTipoParticipanteId=4822 OR @acpa_sTipoParticipanteId=4823  OR @acpa_sTipoParticipanteId= 4831)
			Begin
			
				DECLARE @dNacimientoFecha DATETIME = NULL
				DECLARE @sGeneroId INT = NULL
				DECLARE @cNacimientoLugar CHAR(6) = NULL
				DECLARE @EstadoCivilID int = null
				SELECT 
					@dNacimientoFecha = pers_dNacimientoFecha,	 
					@sGeneroId = pers_sGeneroId,
					@cNacimientoLugar = pers_cNacimientoLugar,
					@EstadoCivilID = pers_sEstadoCivilId
				FROM PN_REGISTRO.RE_PERSONA (nolock) WHERE pers_iPersonaId = @acpa_iPersonaId

				IF (@dNacimientoFecha IS NULL)
				BEGIN
					UPDATE PN_REGISTRO.RE_PERSONA SET 
						[pers_dNacimientoFecha] = @pers_dNacimientoFecha,
						[pers_sUsuarioModificacion]=@acpa_sUsuarioCreacion,
						[pers_vIPModificacion]=@acpa_vIPCreacion,
						[pers_dFechaModificacion] = @datFechaRegistro
					WHERE pers_iPersonaId = @acpa_iPersonaId
				END

				
				IF (@sGeneroId IS NULL)
				BEGIN
					UPDATE PN_REGISTRO.RE_PERSONA SET 
						[pers_sGeneroId]  = @pers_sGeneroId,
						[pers_sUsuarioModificacion]=@acpa_sUsuarioCreacion,
						[pers_vIPModificacion]=@acpa_vIPCreacion,
						[pers_dFechaModificacion] = @datFechaRegistro
					WHERE pers_iPersonaId = @acpa_iPersonaId
				END

				IF (@cNacimientoLugar IS NULL)
				BEGIN
					UPDATE PN_REGISTRO.RE_PERSONA SET 
						[pers_cNacimientoLugar] = @pers_cNacimientoLugar,
						[pers_sUsuarioModificacion]=@acpa_sUsuarioCreacion,
						[pers_vIPModificacion]=@acpa_vIPCreacion,
						[pers_dFechaModificacion] = @datFechaRegistro
					WHERE pers_iPersonaId = @acpa_iPersonaId
				END

				if(@EstadoCivilID is null)
				begin
				
					UPDATE PN_REGISTRO.RE_PERSONA SET 
						[pers_sEstadoCivilId] = @EstadoCivilID,
						[pers_sUsuarioModificacion]=@acpa_sUsuarioCreacion,
						[pers_vIPModificacion]=@acpa_vIPCreacion,
						[pers_dFechaModificacion] = @datFechaRegistro
					WHERE pers_iPersonaId = @acpa_iPersonaId
				end

			End

			
			if (@pers_bFallecidoFlag = 1)
			Begin
				UPDATE PN_REGISTRO.RE_PERSONA SET 
						[pers_bFallecidoFlag]  = @pers_bFallecidoFlag,
						[pers_dFechaDefuncion] = @pers_dFechaDefuncion,
						[pers_cUbigeoDefuncion] = @pers_cUbigeoDefuncion,
						[pers_sGeneroId]	= @pers_sGeneroId,
						[pers_sUsuarioModificacion]=@acpa_sUsuarioCreacion,
						[pers_vIPModificacion]=@acpa_vIPCreacion,
						[pers_dFechaModificacion] = @datFechaRegistro
					WHERE pers_iPersonaId = @acpa_iPersonaId
			End

			
			DECLARE @vCamposGenerales varchar(500);				
			SET @vCamposGenerales = '8,1055,1101,377'
			EXEC PS_SISTEMA.USP_SI_AUDITORIA_ADICIONAR
				@vCamposGenerales = @vCamposGenerales,
				@vNombreTabla = 'RE_ACTUACIONPARTICIPANTE',
				@vClavePrimaria = @acpa_iActuacionParticipanteId,
				@sOficinaConsularId = @acpa_sOficinaConsularId,			
				@audi_vComentario = 'Registro de Persona desde Actuación - Participantes (PERSONA, RESIDENCIA)',
				@audi_vHostName = @acpa_vHostName,
				@audi_sUsuarioCreacion = @acpa_sUsuarioCreacion,
				@audi_vIPCreacion = @acpa_vIPCreacion

			END
		ELSE
		BEGIN
		--------------------------------------------------------------------------------
		--Autor: Miguel Márquez Beltrán
		--Fecha: 28/09/2016
		--Objetivo: Si el participante existe obtener el acpa_iActuacionParticipanteId
		--------------------------------------------------------------------------------
		SET @acpa_iActuacionParticipanteId = ISNULL((SELECT TOP 1 acpa_iActuacionParticipanteId FROM PN_REGISTRO.RE_ACTUACIONPARTICIPANTE (nolock) 
														WHERE acpa_iActuacionDetalleId = @acpa_iActuacionDetalleId
																AND acpa_sTipoParticipanteId = @acpa_sTipoParticipanteId AND acpa_cEstado = 'A'),0)
		END
	END TRY
	BEGIN CATCH
		DECLARE @ErrorNumber INT = ERROR_NUMBER();
		DECLARE @ErrorMessage NVARCHAR(1000) = ERROR_MESSAGE() 
		RAISERROR('Error Number-%d : Error Message-%s', 16, 1, @ErrorNumber, @ErrorMessage)		
	END CATCH

	SET NOCOUNT OFF;
END


