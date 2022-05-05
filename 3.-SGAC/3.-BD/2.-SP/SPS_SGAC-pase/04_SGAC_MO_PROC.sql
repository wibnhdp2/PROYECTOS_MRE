USE [BD_SGAC]
GO
/****** Object:  StoredProcedure [PN_REGISTRO].[USP_RE_ACTUACIONPARTICIPANTE_ACTUALIZAR]    Script Date: 22/03/2022 11:16:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	--====================================================================================================
	-- Nombre: PN_REGISTRO.USP_RE_ACTUACIONPARTICIPANTE_ACTUALIZAR
	-- Descripción: ADICIONAR REGISTRO EN LA TABLA RE_ACTUACIONPARTICIPANTE
	-- Fecha Creación: 26/12/2014
	-- Fecha Modificacion: 26/12/2014
	-- Descripción Parámetros:
	-- Parámetro(s):
		-- @acpa_iActuacionParticipanteId		bigint				Id de actuación relacionada a una participante,
		-- @acpa_iActuacionDetalleId			bigint				Id de detalle de actuación,	
		-- @acpa_iPersonaId						bigint				Id de persona,	
		-- @acpa_sTipoParticipanteId			smallint			Id de tipo de participante,
		-- @acpa_sTipoDatoId					smallint			Id de tipo de dato de un participante,
		-- @acpa_sTipoVinculoId					smallint			Id de tipo de vinculo de un participante,
		-- @acpa_sUsuarioModificacion			smallint			Usuario que modifica un registro,
		-- @acpa_vIPModificacion				varchar(50)			IP de la PC donde se modifica el registro,
		-- @acpa_sOficinaConsularId				smallint			Id de la oficina consular,
		-- @acpa_vHostName						varchar(20)			Nombre de Hostname
		-- @vDireccion							varchar(100) = null,Direccion del participante
		-- @cUbigeo								varchar(6) = null,	Ubigeo del participante
		-- @ICentroPobladoId					int = null			Id del centro poblando
		-- @sTipoDocumentoId					smallint			Identificador del tipo de identificacion de la persona. 
		-- @vNumeroDocumento					varchar(20) = null	Numero de la identificacion de la persona
		-- @pers_sNacionalidadID				smallint = null		Identificador de nacionalidad de la persona
		-- @pers_vNombres						varchar(100) = null Nombres de la persona
		-- @pers_vPrimerApellido				varchar(100) = null Apellido Paterno de la persona
		-- @pers_vSegundoApellido				varchar(100) = null Apellido Materno de la persona
		-- @sTipoPersonaId						SMALLINT = NULL		Identificador de tipo de Persona
		-- @pers_dNacimientoFecha				datetime = null		Fecha de nacimiento de la persona
		-- @pers_sGeneroId						smallint = null		Genero de la persona 
		-- @pers_cNacimientoLugar				char(6)  = null		Ubigeo de nacimiento de la persona
		-- @pers_sEstadoCivilId					int = null			Identificador del estado civil de la Persona
		-- @pers_dFechaDefuncion				datetime = null		Fecha de fallecido de la persona
	-- Autor: Sandro Guinet
	-- Version: 2.0
	-- Cambios Importantes:
	-- Autor del cambio: Miguel Márquez Beltrán
	-- Fecha del cambio: 13/09/2016
	-- Motivo del cambio:  Actualizar Nombres y Apellidos, Fecha de Nacimiento, Fecha de Defunción y Domicilio
	--====================================================================================================
	-- MODIFICADO POR: JONATAN SILVA CACHAY
	-- FECHA DE MODIFICACIÓN: 30/04/2018
	-- MOTIVO: SE EVITA QUE ACTUALICE LA FECHA DE NACIMIENTO Y EL LUGAR DE NACIMIENTO EN CASO LLEGUE NULL
	--====================================================================================================
	-- MODIFICADO POR: MIGUEL MÁRQUEZ BELTRÁN
	-- FECHA DE MODIFICACIÓN: 14/11/2018
	-- MOTIVO: ACTUALIZAR DATOS DE LA RESIDENCIA DE ACUERDO AL TIPO DE PARTICIPANTE
	--====================================================================================================
	-- MODIFICADO POR: JONATAN SILVA CACHAY
	-- FECHA DE MODIFICACIÓN: 01/04/2020
	-- MOTIVO: MUESTRA EL MAX DE DIRECCIÓN
	--====================================================================================================
	-- MODIFICADO POR: MIGUEL MÁRQUEZ BELTRÁN
	-- FECHA DE MODIFICACIÓN: 04/03/2021
	-- MOTIVO: REGISTRAR EL GENERO DE LA PERSONA.
--====================================================================================================
-- FECHA			AUTOR		MOTIVO
--====================================================================================================
-- 22/03/2022		MMARQUEZ	SE REGISTRA LA NACIONALIDAD LA PRIMERA VEZ TOMANDO COMO PARAMETRO EL @pers_spaisid.
--								CUANDO SEA PARTICIPANTE DE TIPO DE NACIMIENTO MADRE 
--								SE REGISTRARÁ LA DIRECCIÓN DE RESIDENCIA PARA EL TITULAR  CON CUI.
--====================================================================================================

	ALTER PROCEDURE [PN_REGISTRO].[USP_RE_ACTUACIONPARTICIPANTE_ACTUALIZAR]
	@acpa_iActuacionParticipanteId bigint,
	@acpa_iActuacionDetalleId bigint,	
	@acpa_iPersonaId bigint,	
	@acpa_sTipoParticipanteId smallint,
	@acpa_sTipoDatoId smallint,
	@acpa_sTipoVinculoId smallint,
	@acpa_sUsuarioModificacion smallint,
	@acpa_vIPModificacion varchar(50),
	@acpa_sOficinaConsularId smallint,
	@acpa_vHostName varchar(20),
	@vDireccion varchar(100) = null,
	@cUbigeo varchar(6) = null,
	@ICentroPobladoId int = null,
	@sTipoDocumentoId smallint = null,
	@vNumeroDocumento varchar(20) = null,
	@pers_sNacionalidadID smallint = null,
	@pers_vNombres varchar(100) = null,
	@pers_vPrimerApellido varchar(100) = null,
	@pers_vSegundoApellido varchar(100) = null,
	@sTipoPersonaId SMALLINT = NULL,
	@pers_dNacimientoFecha  datetime = null,
	@pers_sGeneroId			smallint = null,
	@pers_cNacimientoLugar  char(6)  = null,
	@pers_sEstadoCivilId int = null,
    @pers_dFechaDefuncion	datetime = null,
	@pers_spaisid			smallint = NULL
	AS
	BEGIN
		DECLARE @datFechaRegistro datetime;

		DECLARE @sNacionalidadID smallint=0;
		DECLARE @vNombres varchar(100);
		DECLARE @vPrimerApellido varchar(100);
		DECLARE @vSegundoApellido varchar(100);
		DECLARE  @resi_iResidenciaId	BIGINT

 
		SET @datFechaRegistro = PS_ACCESORIOS.FN_OBTENER_FECHAACTUAL(@acpa_sOficinaConsularId);

		SET NOCOUNT ON;

		BEGIN TRY
			

			IF (@acpa_iPersonaId = 0)
			BEGIN	

				IF (NOT EXISTS(SELECT peid_iPersonaIdentificacionId FROM PN_REGISTRO.RE_PERSONAIDENTIFICACION (nolock)
										WHERE peid_vDocumentoNumero = @vNumeroDocumento AND peid_sDocumentoTipoId = @sTipoDocumentoId))				
					BEGIN
					
					EXEC PN_REGISTRO.USP_RE_PERSONA_PARTICIPANTE_ADICIONAR
						@sTipoPersonaId = @sTipoPersonaId,
						@sTipoDocumentoId = @sTipoDocumentoId,
						@vNumeroDocumento = @vNumeroDocumento,
						@sNacionalidadId = @pers_sNacionalidadID,
						@vNombres = @pers_vNombres,
						@vPrimerApellido = @pers_vPrimerApellido,
						@vSegundoApellido = @pers_vSegundoApellido,
						@vDireccion = @vDireccion,
						@cUbigeo = @cUbigeo,
						@ICentroPobladoId = @ICentroPobladoId,
						@vTelefono = '',
						@sUsuarioCreacion = @acpa_sUsuarioModificacion,
						@vIPCreacion = @acpa_vIPModificacion,
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
									@pefi_vFechaNacimiento = NULL,
									@pefi_sNacionalidad = @sNacionalidadId,
									@pefi_sTipoFilacionId = @acpa_sTipoVinculoId,
									@pefi_vNroDocumento = @vNumeroDocumento,
									@pefi_sOficinaConsularId = @acpa_sOficinaConsularId,
									@pefi_sUsuarioCreacion = @acpa_sUsuarioModificacion,
									@pefi_vIPCreacion = @acpa_vIPModificacion,
									@pefi_vHostName = @acpa_vHostName,
									@pefi_iPersonaFilacionId = @pefi_iPersonaFilacionId OUTPUT
						END	

				  END	
				ELSE
				BEGIN
					SET @acpa_iPersonaId =	(SELECT top 1 peid_iPersonaId FROM PN_REGISTRO.RE_PERSONAIDENTIFICACION (nolock)
												WHERE peid_vDocumentoNumero = @vNumeroDocumento AND peid_sDocumentoTipoId = @sTipoDocumentoId)
					
					-------------------------------------------------------------
					-- Autor: Miguel Márquez Beltrán
					-- Fecha: 03/10/2016
					-- Objetivo: Actualizar Nombres, Domicilio
					--			 fecha de defunción, genero, lugar de nacimiento
					--			 ubigeo y estado civil.
					-------------------------------------------------------------
					UPDATE PN_REGISTRO.RE_PERSONA
					SET pers_vNombres = @pers_vNombres,
						pers_vApellidoPaterno	= @pers_vPrimerApellido,
						pers_vApellidoMaterno	= @pers_vSegundoApellido,
						pers_dNacimientoFecha	= ISNULL(@pers_dNacimientoFecha,pers_dNacimientoFecha),
						pers_dFechaDefuncion	= @pers_dFechaDefuncion,
						pers_sGeneroId			= ISNULL(@pers_sGeneroId,pers_sGeneroId),
						pers_cNacimientoLugar	= ISNULL(@pers_cNacimientoLugar,pers_cNacimientoLugar),
						pers_sEstadoCivilId		= @pers_sEstadoCivilId
					WHERE pers_iPersonaId = @acpa_iPersonaId

					

					SET @resi_iResidenciaId = ISNULL((SELECT MAX(pere.pere_iResidenciaId) 
											FROM PN_REGISTRO.RE_PERSONARESIDENCIA pere (nolock)
											INNER JOIN PN_REGISTRO.RE_RESIDENCIA RESI (nolock) on pere.pere_iResidenciaId = RESI.resi_iResidenciaId
											WHERE pere.pere_iPersonaId = @acpa_iPersonaId and RESI.resi_sResidenciaTipoId = 2252 
											and pere.pere_cEstado = 'A' and RESI.resi_cEstado = 'A'),0)
					IF (@resi_iResidenciaId > 0)
					BEGIN
						UPDATE PN_REGISTRO.RE_RESIDENCIA
						SET resi_vResidenciaDireccion = @vDireccion,
							resi_cResidenciaUbigeo = @cUbigeo
						WHERE resi_iResidenciaId = @resi_iResidenciaId
					END
					----------------------------------------------
				END

			END
			ELSE
			BEGIN
				-------------------------------------------------------------
				-- Autor: Miguel Márquez Beltrán
				-- Fecha: 03/10/2016
				-- Objetivo: Actualizar Nombres, Domicilio
				--			 fecha de defunción, genero, lugar de nacimiento
				--			 ubigeo y estado civil.
				-------------------------------------------------------------
				UPDATE PN_REGISTRO.RE_PERSONA
				SET pers_vNombres = @pers_vNombres,
					pers_vApellidoPaterno	= @pers_vPrimerApellido,
					pers_vApellidoMaterno	= @pers_vSegundoApellido,
					pers_dNacimientoFecha	= ISNULL(@pers_dNacimientoFecha,pers_dNacimientoFecha),
					pers_dFechaDefuncion	= @pers_dFechaDefuncion,
					pers_sGeneroId			= ISNULL(@pers_sGeneroId,pers_sGeneroId),
					pers_cNacimientoLugar	= ISNULL(@pers_cNacimientoLugar,pers_cNacimientoLugar),
					pers_sEstadoCivilId		= @pers_sEstadoCivilId
				WHERE pers_iPersonaId = @acpa_iPersonaId

			if (@acpa_sTipoParticipanteId = 4812 or @acpa_sTipoParticipanteId = 4813 or @acpa_sTipoParticipanteId = 4814 or @acpa_sTipoParticipanteId = 4815 or @acpa_sTipoParticipanteId = 4816 
					or @acpa_sTipoParticipanteId = 4821 or @acpa_sTipoParticipanteId = 4824 or @acpa_sTipoParticipanteId = 4832 or @acpa_sTipoParticipanteId = 4833 or @acpa_sTipoParticipanteId = 4834 
					or @acpa_sTipoParticipanteId = 4835)
				BEGIN

					SET @resi_iResidenciaId = ISNULL((SELECT MAX(pere.pere_iResidenciaId) 
												FROM PN_REGISTRO.RE_PERSONARESIDENCIA pere (nolock)
												INNER JOIN PN_REGISTRO.RE_RESIDENCIA RESI (nolock) on pere.pere_iResidenciaId = RESI.resi_iResidenciaId
												WHERE pere.pere_iPersonaId = @acpa_iPersonaId and RESI.resi_sResidenciaTipoId = 2252 
												and pere.pere_cEstado = 'A' and RESI.resi_cEstado = 'A'),0)
					IF (@resi_iResidenciaId > 0)
					BEGIN
						UPDATE PN_REGISTRO.RE_RESIDENCIA
						SET resi_vResidenciaDireccion = @vDireccion,
							resi_cResidenciaUbigeo = @cUbigeo
						WHERE resi_iResidenciaId = @resi_iResidenciaId
					END
					----------------------------------------------
				END
			END

	-------------------------------------------------
	--TIPO DE ACTA: NACIMIENTO
	--PARTICIPANTE: 4811-> TITULAR 
	-------------------------------------------------
	--TIPO DE ACTA: MATRIMONIO
	--PARTICIPANTE: 4822-> DON / 4823-> DOÑA
	-------------------------------------------------
	--TIPO DE ACTA: DEFUNCIÓN
	--PARTICIPANTE: 4831-> TITULAR
	-------------------------------------------------

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
					FROM PN_REGISTRO.RE_PERSONA (nolock) 
					WHERE pers_iPersonaId = @acpa_iPersonaId

					IF (@dNacimientoFecha IS NULL)
					BEGIN
						UPDATE PN_REGISTRO.RE_PERSONA SET 
							pers_dNacimientoFecha = ISNULL(@pers_dNacimientoFecha,pers_dNacimientoFecha)
						WHERE pers_iPersonaId = @acpa_iPersonaId
					END

				
					IF (@sGeneroId IS NULL)
					BEGIN
						UPDATE PN_REGISTRO.RE_PERSONA SET 
						pers_sGeneroId		  = ISNULL(@pers_sGeneroId,pers_sGeneroId)
						WHERE pers_iPersonaId = @acpa_iPersonaId
					END

					IF (@cNacimientoLugar IS NULL)
					BEGIN
						UPDATE PN_REGISTRO.RE_PERSONA SET 
						pers_cNacimientoLugar = ISNULL(@pers_cNacimientoLugar,pers_cNacimientoLugar)
						WHERE pers_iPersonaId = @acpa_iPersonaId
					END

					if(@EstadoCivilID is null)
					begin
						UPDATE PN_REGISTRO.RE_PERSONA SET 
						pers_sEstadoCivilId = @pers_sEstadoCivilId
						WHERE pers_iPersonaId = @acpa_iPersonaId
					end
				End

			UPDATE PN_REGISTRO.RE_ACTUACIONPARTICIPANTE
			   SET	
				acpa_iActuacionDetalleId = @acpa_iActuacionDetalleId,	
				acpa_iPersonaId = @acpa_iPersonaId,	
				acpa_sTipoParticipanteId = @acpa_sTipoParticipanteId,
				acpa_sTipoDatoId = @acpa_sTipoDatoId,
				acpa_sTipoVinculoId = @acpa_sTipoVinculoId,	  
				acpa_sUsuarioModificacion = @acpa_sUsuarioModificacion,
				acpa_vIPModificacion = @acpa_vIPModificacion,
				acpa_dFechaModificacion = @datFechaRegistro
			 WHERE 
				acpa_iActuacionParticipanteId = @acpa_iActuacionParticipanteId
			
			-- AUDITORIA
			DECLARE @vCamposGenerales varchar(500);				
			SET @vCamposGenerales = '8,1055,1101,393'
			EXEC PS_SISTEMA.USP_SI_AUDITORIA_ADICIONAR
				@vCamposGenerales = @vCamposGenerales,
				@vNombreTabla = 'RE_ACTUACIONPARTICIPANTE',
				@vClavePrimaria = @acpa_iActuacionParticipanteId,
				@sOficinaConsularId = @acpa_sOficinaConsularId,			
				@audi_vComentario = 'Actualización de Participantes',
				@audi_vHostName = @acpa_vHostName,
				@audi_sUsuarioCreacion = @acpa_sUsuarioModificacion,
				@audi_vIPCreacion = @acpa_vIPModificacion
			
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


			if (@acpa_sTipoParticipanteId = 4812 or @acpa_sTipoParticipanteId = 4813 or @acpa_sTipoParticipanteId = 4814 or @acpa_sTipoParticipanteId = 4815 or @acpa_sTipoParticipanteId = 4816 
			or @acpa_sTipoParticipanteId = 4821 or @acpa_sTipoParticipanteId = 4824 or @acpa_sTipoParticipanteId = 4832 or @acpa_sTipoParticipanteId = 4833 or @acpa_sTipoParticipanteId = 4834 
			or @acpa_sTipoParticipanteId = 4835)
				Begin

					
								select 
								@sNacionalidadID = pers_sNacionalidadId,
								@vNombres = pers_vNombres,
								@vPrimerApellido =  pers_vApellidoPaterno,
								@vSegundoApellido = pers_vApellidoMaterno 
								from PN_REGISTRO.RE_PERSONA (nolock)
								where pers_iPersonaId = @acpa_iPersonaId;

								if ISNULL(@sNacionalidadID,0)=0
								begin
								 update PN_REGISTRO.RE_PERSONA set pers_sNacionalidadId = @pers_sNacionalidadID where pers_iPersonaId = @acpa_iPersonaId
								end
							
								if LEN(LTRIM(ISNULL(@vNombres,'')))=0
								begin
								 update PN_REGISTRO.RE_PERSONA set pers_vNombres = @pers_vNombres where pers_iPersonaId = @acpa_iPersonaId
								end
							
								if LEN(LTRIM(ISNULL(@vPrimerApellido,'')))=0
								begin
								 update PN_REGISTRO.RE_PERSONA set pers_vApellidoPaterno = @pers_vPrimerApellido where pers_iPersonaId = @acpa_iPersonaId
								end
							
								if LEN(LTRIM(ISNULL(@vSegundoApellido,'')))=0
								begin
								 update PN_REGISTRO.RE_PERSONA set pers_vApellidoMaterno = @pers_vSegundoApellido where pers_iPersonaId = @acpa_iPersonaId
								end

								 -----------------------------------------------------------
								 DECLARE @EXISTE_PERSONA_IDENT_TIPO_DOC BIT = 0
								 
								 SET @EXISTE_PERSONA_IDENT_TIPO_DOC = IIF(NOT EXISTS(SELECT peid_iPersonaIdentificacionId 
																						FROM PN_REGISTRO.RE_PERSONAIDENTIFICACION (nolock) 
																						WHERE peid_iPersonaId = @acpa_iPersonaId or 
																						(peid_sDocumentoTipoId = @sTipoDocumentoId AND peid_vDocumentoNumero = @vNumeroDocumento)),0,1)  


								IF (@EXISTE_PERSONA_IDENT_TIPO_DOC = 0)
								BEGIN
										IF (ISNULL(@sTipoDocumentoId,0)<> 0 AND LEN(LTRIM(ISNULL(@vNumeroDocumento,'')))<>0 )
										BEGIN
											INSERT INTO PN_REGISTRO.RE_PERSONAIDENTIFICACION(
												peid_iPersonaId, peid_sDocumentoTipoId, peid_vDocumentoNumero, peid_bActivoEnRune,
												peid_sUsuarioCreacion, peid_vIPCreacion, peid_dFechaCreacion)
											VALUES
												(@acpa_iPersonaId, @sTipoDocumentoId, @vNumeroDocumento, 1,
												@acpa_sUsuarioModificacion, @acpa_vIPModificacion, @datFechaRegistro)
		
											INSERT INTO PN_REGISTRO.RE_REGISTROUNICO
											   (reun_iPersonaId, reun_sUsuarioCreacion, reun_vIPCreacion, reun_dFechaCreacion)
											 VALUES
												   (@acpa_iPersonaId, @acpa_sUsuarioModificacion, @acpa_vIPModificacion, @datFechaRegistro)		

										END
								END
						
		
					 
										IF (@cUbigeo <> NULL or @cUbigeo <> '')
										BEGIN
											DECLARE @iResidenciaId INT


											if (not exists(select resi_iResidenciaId from PN_REGISTRO.RE_RESIDENCIA (nolock)
															where resi_iResidenciaId in (
																			select pere_iResidenciaId from PN_REGISTRO.RE_PERSONARESIDENCIA (nolock)
																						where pere_iPersonaId = @acpa_iPersonaId and pere_cEstado = 'A')
																						and resi_sResidenciaTipoId = 2252 and resi_cResidenciaUbigeo = @cUbigeo AND resi_cEstado = 'A'))
											BEGIN
													  INSERT INTO PN_REGISTRO.RE_RESIDENCIA (
														resi_sResidenciaTipoId, resi_vResidenciaDireccion, resi_vResidenciaTelefono,resi_cResidenciaUbigeo,  
														resi_sUsuarioCreacion, resi_vIPCreacion, resi_dFechaCreacion,resi_cEstado)
													  VALUES 
														(2252, @vDireccion,'', @cUbigeo,  
														@acpa_sUsuarioModificacion, @acpa_vIPModificacion, @datFechaRegistro,'A')
												 
  													  SET @iResidenciaId = @@IDENTITY

													  EXEC	   [PN_REGISTRO].[USP_RE_PERSONARESIDENCIA_ADICIONAR]
									  							@pere_iPersonaId = @acpa_iPersonaId,
																@pere_iResidenciaId = @iResidenciaId,
																@pere_sOficinaConsularId = @acpa_sOficinaConsularId,
																@pere_sUsuarioCreacion = @acpa_sUsuarioModificacion,
																@pere_vIPCreacion = @acpa_vIPModificacion,
																@pere_vHostName = @acpa_vHostName
											END
											ELSE
											BEGIN
					
												SET @resi_iResidenciaId = ISNULL((SELECT MAX(pere.pere_iResidenciaId) 
																			FROM PN_REGISTRO.RE_PERSONARESIDENCIA pere (nolock)
																			INNER JOIN PN_REGISTRO.RE_RESIDENCIA RESI (nolock) on pere.pere_iResidenciaId = RESI.resi_iResidenciaId
																			WHERE pere.pere_iPersonaId = @acpa_iPersonaId and RESI.resi_sResidenciaTipoId = 2252 
																			and pere.pere_cEstado = 'A' and RESI.resi_cEstado = 'A'),0)
												IF (@resi_iResidenciaId > 0)
												BEGIN
													UPDATE PN_REGISTRO.RE_RESIDENCIA
													SET resi_vResidenciaDireccion = @vDireccion,
														resi_cResidenciaUbigeo = @cUbigeo
													WHERE resi_iResidenciaId = @resi_iResidenciaId
												END
											END
										---------------------------------------------------------------------
										-- Fecha: 22/03/2022
										--Motivo: CUANDO SEA PARTICIPANTE DE TIPO DE NACIMIENTO MADRE 
										--		  SE REGISTRARÁ LA DIRECCIÓN DE RESIDENCIA PARA EL TITULAR 

										---------------------------------------------------------------------

											if (@acpa_sTipoParticipanteId = 4813)
											begin
												declare @iPersonaTitularId bigint
												set @iPersonaTitularId = isnull((Select acpa_iPersonaId From PN_REGISTRO.RE_ACTUACIONPARTICIPANTE (nolock)
																			where acpa_iActuacionDetalleId = @acpa_iActuacionDetalleId		
																			and acpa_sTipoParticipanteId = 4811 and acpa_cEstado='A'),0)
												declare @conCUI char(1)
												SET @conCUI = isnull((Select reci_cConCUI from PN_REGISTRO.RE_REGISTROCIVIL (nolock)
																where reci_iActuacionDetalleId = @acpa_iActuacionDetalleId),'N')

												if (@iPersonaTitularId > 0 and @conCUI = 'S')
												begin

													IF (NOT EXISTS(select resi_iResidenciaId from PN_REGISTRO.RE_RESIDENCIA (nolock)
																								where resi_iResidenciaId in (
																								select pere_iResidenciaId from PN_REGISTRO.RE_PERSONARESIDENCIA (nolock)
																								where pere_iPersonaId = @iPersonaTitularId and pere_cEstado = 'A')
																								and resi_sResidenciaTipoId = 2252 and resi_cResidenciaUbigeo = @cUbigeo AND resi_cEstado = 'A'))

														begin
																INSERT INTO PN_REGISTRO.RE_RESIDENCIA (
																resi_sResidenciaTipoId, resi_vResidenciaDireccion, resi_vResidenciaTelefono,resi_cResidenciaUbigeo,  
																resi_sUsuarioCreacion, resi_vIPCreacion, resi_dFechaCreacion,resi_cEstado)
															  VALUES 
																(2252, @vDireccion,'', @cUbigeo,  
																@acpa_sUsuarioModificacion, @acpa_vIPModificacion, @datFechaRegistro,'A')
												 
  															  SET @iResidenciaId = @@IDENTITY

															  EXEC	   [PN_REGISTRO].[USP_RE_PERSONARESIDENCIA_ADICIONAR]
									  									@pere_iPersonaId = @iPersonaTitularId,
																		@pere_iResidenciaId = @iResidenciaId,
																		@pere_sOficinaConsularId = @acpa_sOficinaConsularId,
																		@pere_sUsuarioCreacion = @acpa_sUsuarioModificacion,
																		@pere_vIPCreacion = @acpa_vIPModificacion,
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
											end

										END
							 

 
				End
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

 		END TRY

		BEGIN CATCH
			DECLARE @ErrorNumber INT = ERROR_NUMBER();
			DECLARE @ErrorMessage NVARCHAR(1000) = ERROR_MESSAGE() 
			RAISERROR('Error Number-%d : Error Message-%s', 16, 1, @ErrorNumber, @ErrorMessage)		
		END CATCH

		SET NOCOUNT OFF;
	END

