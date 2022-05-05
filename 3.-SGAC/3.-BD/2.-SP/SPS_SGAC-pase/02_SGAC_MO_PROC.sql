USE [BD_SGAC]
GO
/****** Object:  StoredProcedure [PN_REGISTRO].[USP_RE_PERSONA_PARTICIPANTE_ADICIONAR]    Script Date: 22/03/2022 10:26:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--====================================================================================================
-- Nombre: PN_REGISTRO.USP_RE_PERSONA_PARTICIPANTE_ADICIONAR
-- Descripción: ADICIONAR REGISTRO EN LA TABLA RE_ACTUACIONPARTICIPANTE
-- Fecha Creación:		07/02/2015
-- Fecha Modificacion:  06/03/2015 -- JOSE CAYCHO
-- Descripción Parámetros:
-- Parámetro(s):
	--@sTipoPersonaId		smallint	Id de tipo de persona,
	--@sTipoDocumentoId		smallint	Id de tipo de documento de persona,
	--@vNumeroDocumento		varchar(20)	Numnero de documento,
	--@sNacionalidadId		smallint	Id de nacionalidad,
	--@vNombres				varchar(200)	Nombres de la persona,
	--@vPrimerApellido		varchar(100)	Apellido paterno de la persona,
	--@vSegundoApellido		varchar(100)	Apellido materno de la persona,
	--@vDireccion			varchar(100)	Dirección de la persona,
	--@cUbigeo				char(6)	Codigo de ubigeo,
	--@ICentroPobladoId		int	Id de centro poblado,
	--@vTelefono			varchar(50)	Telefono de la persona,
	--@sUsuarioCreacion		smallint	Usuario que crea el registro,
	--@vIPCreacion			varchar(50)	IP la PC donde se crea el registro,
	--@sOficinaConsularId	smallint	Id de oficina consular,
	--@vHostName			varchar(20)	Nombre del hostname
	--@iPersonaId			bigint out	Variable que retorna el identificador de persona
	--@sGeneroId			smallint	Genero de la persona
-- Autor: Margarita Díaz
-- Version: 1.0
-- Cambios Importantes:
-- Autor del cambio: Miguel Márquez Beltrán
-- Fecha del cambio: 30/09/2016
-- Objetivo: Adicionar la creación del Registro único (PN_REGISTRO.RE_REGISTROUNICO)
--			junto a la creación de la persona.		
------------------------------------------------------------------------------------------------------
-- FECHA			AUTOR		MOTIVO	
-- 17/08/2018		MMARQUEZ	Actualizar la oficina consular en el registro de persona
-- 16/04/2019		MMARQUEZ	Validar el registro de RE_PERSONAIDENTIFICACION por tipo de documento y nro. de documento
-- 04/03/2021		MMARQUEZ	Se adiciono el parametro @sGeneroId para registrar el genero cuando es nueva persona.
-- 22/03/2022		MMARQUEZ	SE REGISTRA LA NACIONALIDAD LA PRIMERA VEZ TOMANDO COMO PARAMETRO EL @pers_spaisid.
--=========================================================================================================================
ALTER PROCEDURE [PN_REGISTRO].[USP_RE_PERSONA_PARTICIPANTE_ADICIONAR] --  1,1,'00000107',0,'JOSE','CAYCHO','GARCIA','AA AAA AA','150304',NULL,'',5,'::1',1,'KND-JCAYCHO',0, 2001
@sTipoPersonaId smallint,
@sTipoDocumentoId smallint = null,
@vNumeroDocumento varchar(20) = null,
@sNacionalidadId smallint,
@vNombres varchar(200),
@vPrimerApellido varchar(100),
@vSegundoApellido varchar(100),
@vDireccion varchar(100),
@cUbigeo char(6),
@ICentroPobladoId int,
@vTelefono varchar(50),
@sUsuarioCreacion smallint,
@vIPCreacion varchar(50),
@sOficinaConsularId smallint,
@vHostName varchar(20),
@iPersonaId bigint out,
@sGeneroId	smallint = null,
@spaisid	smallint = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @dFechaRegistro datetime;
	DECLARE @iResidenciaId bigint;
	BEGIN TRY

		SET @dFechaRegistro = PS_ACCESORIOS.FN_OBTENER_FECHAACTUAL(@sOficinaConsularId);

		-- PERSONA
		INSERT INTO PN_REGISTRO.RE_PERSONA (
			pers_vApellidoPaterno, pers_vApellidoMaterno, pers_vNombres, pers_sPersonaTipoId, pers_sNacionalidadId,
			pers_sUsuarioCreacion, pers_vIPCreacion, pers_dFechaCreacion, pers_sOficinaConsularId, pers_sGeneroId) 
		VALUES (
			@vPrimerApellido, @vSegundoApellido, @vNombres, @sTipoPersonaId, @sNacionalidadId,
			@sUsuarioCreacion, @vIPCreacion, @dFechaRegistro, @sOficinaConsularId, @sGeneroId);

		SET @iPersonaId = @@IDENTITY

		INSERT INTO PN_REGISTRO.RE_REGISTROUNICO
			(reun_iPersonaId, reun_sUsuarioCreacion, reun_vIPCreacion, reun_dFechaCreacion)
			VALUES
				(@iPersonaId, @sUsuarioCreacion, @vIPCreacion, @dFechaRegistro)		


		-------------------------------------------------
		DECLARE @VNACIONALIDAD VARCHAR(100) = ''
					
		IF (ISNULL(@spaisid,0) > 0)
			BEGIN
				SET @VNACIONALIDAD = ISNULL((SELECT TOP 1 ISNULL([PAIS_VNACIONALIDAD],'') FROM [PS_SISTEMA].[SI_PAIS] P (nolock)
										WHERE P.PAIS_SPAISID = @spaisid),'')
				EXEC [PN_REGISTRO].[RE_PERSONANACIONALIDAD_ADICIONAR]  @iPersonaId, @spaisid, @VNACIONALIDAD,1, 
																	'A',  @sUsuarioCreacion,  @vIPCreacion
			END

		 
		IF (@iPersonaId > 0 and (@vNumeroDocumento<> '' or not @vNumeroDocumento is null ) and not @sTipoDocumentoId is null ) 
		BEGIN

			IF (NOT EXISTS(SELECT peid_iPersonaIdentificacionId FROM PN_REGISTRO.RE_PERSONAIDENTIFICACION (nolock) WHERE peid_vDocumentoNumero = @vNumeroDocumento AND peid_sDocumentoTipoId = @sTipoDocumentoId))

			BEGIN
				INSERT INTO PN_REGISTRO.RE_PERSONAIDENTIFICACION(
					peid_iPersonaId, peid_sDocumentoTipoId, peid_vDocumentoNumero, peid_bActivoEnRune,
					peid_sUsuarioCreacion, peid_vIPCreacion, peid_dFechaCreacion)
				VALUES
					(@iPersonaId, @sTipoDocumentoId, @vNumeroDocumento, 1,
					@sUsuarioCreacion, @vIPCreacion, @dFechaRegistro)
			END
			ELSE
			BEGIN
				UPDATE PN_REGISTRO.RE_PERSONAIDENTIFICACION
				SET peid_iPersonaId = @iPersonaId,
				    peid_bActivoEnRune = 1, peid_sUsuarioModificacion = @sUsuarioCreacion,
					peid_vIPModificacion = @vIPCreacion, peid_dFechaModificacion = @dFechaRegistro
				WHERE peid_sDocumentoTipoId = @sTipoDocumentoId AND peid_vDocumentoNumero = @vNumeroDocumento
			END
			
		 
			IF (@cUbigeo <> NULL OR @cUbigeo <>'')
			BEGIN
		 
				INSERT INTO PN_REGISTRO.RE_RESIDENCIA (
					resi_sResidenciaTipoId, resi_vResidenciaDireccion, resi_vResidenciaTelefono,resi_cResidenciaUbigeo, resi_ICentroPobladoId, 
					resi_sUsuarioCreacion, resi_vIPCreacion, resi_dFechaCreacion)
				VALUES 
					(2252, @vDireccion, @vTelefono, @cUbigeo, @ICentroPobladoId, 
					@sUsuarioCreacion, @vIPCreacion, @dFechaRegistro)

				SET @iResidenciaId = @@IDENTITY

				EXEC	[PN_REGISTRO].[USP_RE_PERSONARESIDENCIA_ADICIONAR]
					@pere_iPersonaId = @iPersonaId,
					@pere_iResidenciaId = @iResidenciaId,
					@pere_sOficinaConsularId = @sOficinaConsularId,
					@pere_sUsuarioCreacion = @sUsuarioCreacion,
					@pere_vIPCreacion = @vIPCreacion,
					@pere_vHostName = @vHostName
			END
		END
		ELSE 
		BEGIN
				IF (@iPersonaId > 0 and   @sTipoDocumentoId is null ) 
				BEGIN
				IF (@cUbigeo <> NULL OR @cUbigeo <>'')
					BEGIN
		 
						INSERT INTO PN_REGISTRO.RE_RESIDENCIA (
							resi_sResidenciaTipoId, resi_vResidenciaDireccion, resi_vResidenciaTelefono,resi_cResidenciaUbigeo, resi_ICentroPobladoId, 
							resi_sUsuarioCreacion, resi_vIPCreacion, resi_dFechaCreacion)
						VALUES 
							(2252, @vDireccion, @vTelefono, @cUbigeo, @ICentroPobladoId, 
							@sUsuarioCreacion, @vIPCreacion, @dFechaRegistro)

						SET @iResidenciaId = @@IDENTITY

						EXEC	[PN_REGISTRO].[USP_RE_PERSONARESIDENCIA_ADICIONAR]
							@pere_iPersonaId = @iPersonaId,
							@pere_iResidenciaId = @iResidenciaId,
							@pere_sOficinaConsularId = @sOficinaConsularId,
							@pere_sUsuarioCreacion = @sUsuarioCreacion,
							@pere_vIPCreacion = @vIPCreacion,
							@pere_vHostName = @vHostName
					END
				END
		END

			-- AUDITORIA
		DECLARE @vCamposGenerales varchar(500);				
		SET @vCamposGenerales = '8,1055,1101,326'
		EXEC PS_SISTEMA.USP_SI_AUDITORIA_ADICIONAR
			@vCamposGenerales = @vCamposGenerales,
			@vNombreTabla = 'RE_PERSONA',
			@vClavePrimaria = @iPersonaId,
			@sOficinaConsularId = @sOficinaConsularId,			
			@audi_vComentario = 'Registro persona participante',
			@audi_vHostName = @vHostName,
			@audi_sUsuarioCreacion = @sUsuarioCreacion,
			@audi_vIPCreacion = @vIPCreacion

	END TRY
	BEGIN CATCH
		DECLARE @ErrorNumber INT = ERROR_NUMBER();
		DECLARE @ErrorMessage NVARCHAR(1000) = ERROR_MESSAGE() 
		RAISERROR('Error Number-%d : Error Message-%s', 16, 1, @ErrorNumber, @ErrorMessage)		
	END CATCH	

	SET NOCOUNT OFF;
END

