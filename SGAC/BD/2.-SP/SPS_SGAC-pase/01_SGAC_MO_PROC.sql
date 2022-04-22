USE [BD_SGAC]
GO
/****** Object:  StoredProcedure [PN_REGISTRO].[USP_RE_PERSONA_ADICIONAR]    Script Date: 22/03/2022 10:08:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	--====================================================================================================
	-- Nombre: PN_REGISTRO.[USP_RE_PERSONA_ADICIONAR]
	-- Descripción: ACTUALIZA LAS TABLAS RE_PERSONA, RE_REGISTROUNICO, RE_ACTUACION Y RE_ACTUACIONDETALLE
	-- Fecha Creación: 14/07/2014
	-- Fecha Modificacion: 05/03/2015   -- Jose Caycho Garcia
	-- Descripción Parámetros:
	-- Parámetro(s):
		--@pers_sPersonaTipoId								Identificador del tipo de Persona	
		--@pers_vApellidoPaterno varchar(100)				Apellido Paterno de la persona,
		--@pers_vApellidoMaterno varchar(100)				Apellido Materno de la persona,
		--@pers_vNombres varchar(100)						Nombres de la persona,
		--@peid_sDocumentoTipoId smallint					Identificador del Tipo de Documento,
		--@peid_vDocumentoNumero varchar(20)				Numero de Documento,	
		--@pers_vCorreoElectronico varchar(150)				Correo electrónico de la persona,
		--@pers_dNacimientoFecha datetime					Fecha de nacimiento de la persona,
		--@pers_cNacimientoLugar char(6)					Ubigeo de nacimiento de la persona. Es FK de la tabla SI_UBICACIONGEOGRAFICA,
		--@pers_sGeneroId smallint							Genero de la persona. Es FK de la tabla SI_PARAMETRO Grupo=PERSONA-GENERO,
		--@pers_vObservaciones varchar(1000)				Observaciones acerca de la persona,	
		--@pers_sNacionalidadId smallint					Identificador de nacionalidad de la persona. Es FK de la tabla SI_PARAMETRO Grupo=PERSONA-NACIONALIDAD,
		--@pers_sEstadoCivilId smallint						Identificador del estado civil de la Persona. Es FK de la tabla MA_ESTADO_CIVIL,
		--@pers_sGradoInstruccionId smallint				Grado de instrucción de la Persona. Es FK de la tabla SI_PARAMETRO Grupo=PERSONA-GRADO INSTRUCCION,
		--@pers_sOcupacionId smallint						Ocupacion de la persona. Es FK de la tabla MA_OCUPACION,
		--@pers_sProfesionId smallint						Profesión de la persona. Es FK de la tabla MA_PROFESION,	
		--@pers_vApellidoCasada varchar(100)				Apellido de Casado(a) de la persona,
		--@reun_iRegistroUnicoId bigint						Identificador del registro Unico,
		--@reun_vEmergenciaNombre varchar(150)				Nombre del familiar o pariente cercano para una emergencia,
		--@reun_sEmergenciaRelacionId smallint				Identificador de la relacion de emergencia del pariente,
		--@reun_vEmergenciaDireccionLocal varchar(500)		Dirección del pariente/propio cercano en caso de Emergencia ,
		--@reun_vEmergenciaCodigoPostal varchar(10)			Código postal para casos de emergencia,
		--@reun_vEmergenciaTelefono varchar(10)				Número de teléfono en caso de emergencia,
		--@reun_vEmergenciaDireccionPeru varchar(500)		Dirección en caso de emergencia,
		--@reun_vEmergenciaCorreoElectronico varchar(500)	Correo Electrónico en caso de emergencia,
		--@reun_cViveExteriorDesde char(6)					Respuesta afirmativa o negativa si vive en el exterior,
		--@reun_bPiensaRetornarAlPeru bit					Respuesta afirmativa o negativa sip piensa retornar al Perú,
		--@reun_cCuandoRetornaAlPeru char(6)				Respuesta,
		--@reun_bAfiliadoSeguroSocial bit					Respuesta afirmativa o negativa se tiene seguros social,
		--@reun_bAfiliadoAFP bit							Respuesta afirmativa o negativa si esta afiliado a una AFP,
		--@reun_bAportaSeguroSocial bit						Respuesta afirmativa o negativa si aporta al seguro social,
		--@reun_bBeneficiadoExterior bit					Beneficiado exterior,
		--@reun_sOcupacionPeru smallint						Identificador de la ocupacion en el Perú,
		--@reun_sOcupacionExtranjero smallint				Identificador de la ocupación en el Extrangero,
		--@reun_vNombreConvenio varchar(400)				Nombre del Convenio,
		--@pers_sOficinaConsularId smallint					Identificador de la Oficina Consular,
		--@pers_bGenera58A bit								Estado de la persona si presenta la Actuación 58A,
		--@pers_sUsuarioModificacion smallint				Identificador del último usuario que modifica el registro,
		--@pers_vIPModificacion varchar(50)					IP de la última PC desde donde se modifica el registro,
		--@pers_vHostName varchar(20)						Nombre del Hostname
		--@pers_bFallecidoFlag bit							Flag que identifica si la persona es fallecida
		--@pers_iPersonaId bigint							Identificador de la Persona. Es auto incrementable (IDENTITY),
		--@pers_vSenasParticulares varchar(250)				Señas particulares de la persona
		--@pers_spaisid smallint							Identificador del País de Origen
		--@peid_vTipoDocumento								Descripción del Tipo de Documento Otros.
		--@peid_bValidacionReniec							indicador de validacion por reniec
	-- Autor: Sandro Guinet
	-- Version: 2.1
	-- Cambios Importantes:
	--  AUTOR		FECHA			MOTIVO
	--	02/10/2015	MDIAZ		Registro de Pago : Tipo de Pago, Tipo de Cambio bancario y Consular
	--	24/08/2016  MMARQUEZ	Consultar la moneda local desde la tabla de oficina consular
	--	05/11/2016	JTITO		VALIDACIÓN : SÓLO SE ACTUALIZARÁN LOS CORRELATIVOS DE LAS OFICINAS QUE TENGAN SGAC (BIOMÉTRICOS)
	--	22/12/2016	MMARQUEZ	Se incluyo el parametro: @pers_vSenasParticulares para su registro. 
	--	31/01/2017	MMARQUEZ	Asignar valor Nulo al parametro: @pers_dNacimientoFecha
	--	15/03/2017	MMARQUEZ	Adicionar el campo: pers_spaisid y peid_vTipoDocumento
	--	12/11/2018	JTITO		Se agrega validación del estado a la consulta de la tabla RE_PERSONAIDENTIFICACION
	--====================================================================================================
	-- MODIFICADO POR: JOnatan silva Cachay
	-- Fecha de modificación: 21/11/2017
	-- MOTIVO:	Se agrego un nuevo campo @actu_sCiudadItinerante
	--=====================================================
	-- MODIFICADO POR: JOnatan silva Cachay
	-- Fecha de modificación: 02/05/2018
	-- MOTIVO:	Se agrego un nuevo campo @pers_bPadresPeruanos
	--=====================================================
	-- MODIFICADO POR: JOnatan silva Cachay
	-- Fecha de modificación: 20/06/2018
	-- MOTIVO:	Se agrego un nuevo campo @pers_sOficinaConsularId para registrar en la tabla de persona
	--=====================================================
	-- MODIFICADO POR: JOnatan silva Cachay
	-- Fecha de modificación: 07/01/2019
	-- MOTIVO:	Se valida que se registre la persona y su documento de identidad solo si no existe en la BD
	--=====================================================
	-- MODIFICADO POR: JOnatan silva Cachay
	-- Fecha de modificación: 12/02/2019
	-- MOTIVO:	Se agrega nuevos campos
	--=====================================================
	-- MODIFICADO POR: JOnatan silva Cachay
	-- Fecha de modificación: 03/06/2019
	-- MOTIVO:	EN CASO SEA 58A SE INSERTA LA NORMA DE GRATUIDAD
	--=====================================================
	-- MODIFICADO POR: JOnatan silva Cachay
	-- Fecha de modificación: 23/08/2019
	-- MOTIVO:	EN CASO SEA SELECCIONE QUE ES UN REGISTRO DE PERUANO, SE INSERTA NACIONALIDAD PERUANO 
	--			EN LA TABLA DE NACIONALIDADES
	--=====================================================
   -- MODIFICADO POR: JOnatan silva Cachay
	-- Fecha de modificación: 19/02/2020
	-- MOTIVO:	SE HACE VALIDACIÓN PARA QUE SOLO AVANCE SI ES QUE REGISTRA PERSONA Y PERSONA IDENTIFICACIÓN
	--========================================================================================================
	--FECHA			USUARIO			MOTIVO
	--========================================================================================================
	--17/12/2021	MMARQUEZ		SE REGISTRA LA NACIONALIDAD LA PRIMERA VEZ Y SE COLOCAN LOS NOLOCK 
	--								Y SE REEMPLAZAN LOS IF COUNT POR IF EXISTS. 
	--11/01/2022	VPIPA			SE ADICIONA EL CAMPO peid_bValidacionReniec para indicar q el registro ha
	--								sido validado por RENIEC, ademas se borra "SET @datFechaRegistro = PS_ACCESORIOS.FN_OBTENER_FECHAACTUAL... por la doble seteo innesesario"
	--22/03/2022	mmarquez		SE REGISTRA LA NACIONALIDAD LA PRIMERA VEZ TOMANDO COMO PARAMETRO EL @pers_spaisid.
	--========================================================================================================

	ALTER PROCEDURE [PN_REGISTRO].[USP_RE_PERSONA_ADICIONAR]
	@pers_sPersonaTipoId smallint,
	@pers_vApellidoPaterno varchar(100),
	@pers_vApellidoMaterno varchar(100),
	@pers_vNombres varchar(100),
	@peid_sDocumentoTipoId smallint,
	@peid_vDocumentoNumero varchar(20),
	@pers_vCorreoElectronico varchar(150),
	@pers_dNacimientoFecha datetime = NULL,
	@pers_cNacimientoLugar char(6),
	@pers_sGeneroId smallint,
	@pers_vObservaciones varchar(1000),	
	@pers_sNacionalidadId smallint,
	@pers_sEstadoCivilId smallint,
	@pers_sGradoInstruccionId smallint,
	@pers_sOcupacionId smallint,
	@pers_sProfesionId smallint,	
	@pers_vApellidoCasada varchar(100),
	@reun_vEmergenciaNombre varchar(150),
	@reun_sEmergenciaRelacionId smallint,
	@reun_vEmergenciaDireccionLocal varchar(500),
	@reun_vEmergenciaCodigoPostal varchar(10),
	@reun_vEmergenciaTelefono varchar(10),
	@reun_vEmergenciaDireccionPeru varchar(500),
	@reun_vEmergenciaCorreoElectronico varchar(500),
	@reun_cViveExteriorDesde char(6),
	@reun_bPiensaRetornarAlPeru bit,
	@reun_cCuandoRetornaAlPeru char(6),
	@reun_bAfiliadoSeguroSocial bit,
	@reun_bAfiliadoAFP bit,
	@reun_bAportaSeguroSocial bit,
	@reun_bBeneficiadoExterior bit,
	@reun_sOcupacionPeru smallint,
	@reun_sOcupacionExtranjero smallint,
	@reun_vNombreConvenio varchar(400),
	@pers_sOficinaConsularId smallint,
	@pers_bGenera58A bit,
	@pers_sUsuarioCreacion smallint,
	@pers_vIPCreacion varchar(50),	
	@pers_vHostName varchar(20),
	@pers_bFallecidoFlag bit = 0,	
	@pers_iPersonaId bigint out,
	@pers_vSenasParticulares varchar(250)='',
	@pers_spaisid	smallint = NULL,
	@peid_vTipoDocumento	varchar(100) = '',
	@actu_sCiudadItinerante SmallInt = 0,   
	@pers_bPadresPeruanos bit = 0,
	@pers_vEstatura varchar(20) = null,
	@pers_sAnioEstudio	SmallInt  = null,
	@pers_cEstudioCompleto	Char(1)  = null,
	@pers_cDiscapacidad CHAR(1) = null,
	@pers_cInterdicto CHAR(1) = null,
	@pers_vNombreCurador VARCHAR(100) = null,
	@pers_cDonaOrganos CHAR(1) = null,
	@pers_sTipoDeclarante SMALLINT = null,
	@pers_sTipoTutorGuardador SMALLINT = null,
	@pers_bValidacionReniec BIT = 0
	AS
	BEGIN
	
		SET NOCOUNT ON

		DECLARE @actu_IActuacionId int
		DECLARE @intCorrelativoActuacionId int, @ICorrelativoActuacion int;
		DECLARE @intCorrelativoTarifarioId int, @ICorrelativoTarifario int;
		DECLARE @datFechaRegistro datetime;

		DECLARE @vCamposGenerales varchar(500);
		DECLARE @VCOMENTARIO VARCHAR(500);
		DECLARE @vID VARCHAR(50);
	
		SET @ICorrelativoActuacion = 0;
		SET @ICorrelativoTarifario = 0;	

		BEGIN TRY	

			SET @datFechaRegistro = PS_ACCESORIOS.FN_OBTENER_FECHAACTUAL(@pers_sOficinaConsularId);

			-----------------------------------------------------------
						
			IF NOT EXISTS(SELECT peid_iPersonaIdentificacionId FROM PN_REGISTRO.RE_PERSONAIDENTIFICACION (NOLOCK)
									WHERE  peid_sDocumentoTipoId = @peid_sDocumentoTipoId AND peid_vDocumentoNumero = @peid_vDocumentoNumero
										AND peid_cEstado = 'A')
			BEGIN

				INSERT INTO PN_REGISTRO.RE_PERSONA
						   (pers_sPersonaTipoId, pers_vApellidoPaterno, pers_vApellidoMaterno, pers_vNombres, 
							pers_vCorreoElectronico, pers_dNacimientoFecha, pers_cNacimientoLugar, pers_sGeneroId, pers_vObservaciones,
							pers_sNacionalidadId, pers_sEstadoCivilId, pers_sGradoInstruccionId, pers_sOcupacionId, pers_sProfesionId, pers_vApellidoCasada, 
							pers_sUsuarioCreacion, pers_vIPCreacion, pers_dFechaCreacion,pers_bFallecidoFlag, pers_vSenasParticulares, pers_spaisid,pers_bPadresPeruanos,pers_sOficinaConsularId,pers_vEstatura,
							pers_sAnioEstudio,pers_cEstudioCompleto,
							pers_cDiscapacidad,pers_cInterdicto,pers_vNombreCurador,pers_cDonaOrganos,pers_sTipoDeclarante,pers_sTipoTutorGuardador,pers_bValidacionReniec)
							VALUES (@pers_sPersonaTipoId, @pers_vApellidoPaterno, @pers_vApellidoMaterno, @pers_vNombres, 
							 @pers_vCorreoElectronico, @pers_dNacimientoFecha, @pers_cNacimientoLugar, @pers_sGeneroId, @pers_vObservaciones,
							 @pers_sNacionalidadId, @pers_sEstadoCivilId, @pers_sGradoInstruccionId, @pers_sOcupacionId, @pers_sProfesionId, @pers_vApellidoCasada,
							 @pers_sUsuarioCreacion, @pers_vIPCreacion, @datFechaRegistro,@pers_bFallecidoFlag, @pers_vSenasParticulares, @pers_spaisid,@pers_bPadresPeruanos,@pers_sOficinaConsularId,@pers_vEstatura,
							 @pers_sAnioEstudio,@pers_cEstudioCompleto,
							 @pers_cDiscapacidad,@pers_cInterdicto,@pers_vNombreCurador,@pers_cDonaOrganos,@pers_sTipoDeclarante,@pers_sTipoTutorGuardador,@pers_bValidacionReniec)

				SET @pers_iPersonaId= @@IDENTITY


            if(ISNULL(@pers_iPersonaId,0)>0)
            BEGIN
               INSERT INTO PN_REGISTRO.RE_PERSONAIDENTIFICACION
						   (peid_iPersonaId, peid_sDocumentoTipoId, peid_vDocumentoNumero,
							peid_dFecVcto, peid_dFecExpedicion, peid_dFecRenovacion, peid_vLugarRenovacion, peid_bActivoEnRune,
							peid_sUsuarioCreacion, peid_vIPCreacion, peid_dFechaCreacion, peid_vTipoDocumento)
					 VALUES
						   (@pers_iPersonaId, @peid_sDocumentoTipoId, @peid_vDocumentoNumero,
							NULL, NULL, NULL, NULL, 1,
							@pers_sUsuarioCreacion, @pers_vIPCreacion, @datFechaRegistro, @peid_vTipoDocumento)

               IF ISNULL(@@IDENTITY,0) <= 0
               BEGIN
                  SET @pers_iPersonaId = 0
                  return
               END
            END
            else
            BEGIN
               SET @pers_iPersonaId = 0
               return
            END
				INSERT INTO PN_REGISTRO.RE_REGISTROUNICO
				   (reun_iPersonaId, reun_vEmergenciaNombre, reun_sEmergenciaRelacionId,
					reun_vEmergenciaDireccionLocal, reun_vEmergenciaCodigoPostal, reun_vEmergenciaTelefono,
					reun_vEmergenciaDireccionPeru, reun_vEmergenciaCorreoElectronico, reun_cViveExteriorDesde, 
					reun_bPiensaRetornarAlPeru, reun_cCuandoRetornaAlPeru, reun_bAfiliadoSeguroSocial, reun_bAfiliadoAFP, 
					reun_bAportaSeguroSocial, reun_bBeneficiadoExterior, reun_sOcupacionPeru, reun_sOcupacionExtranjero, 
					reun_vNombreConvenio, reun_sUsuarioCreacion, reun_vIPCreacion, reun_dFechaCreacion)
				VALUES
				   (@pers_iPersonaId, @reun_vEmergenciaNombre, @reun_sEmergenciaRelacionId,
					@reun_vEmergenciaDireccionLocal, @reun_vEmergenciaCodigoPostal, @reun_vEmergenciaTelefono,
					@reun_vEmergenciaDireccionPeru,	@reun_vEmergenciaCorreoElectronico, @reun_cViveExteriorDesde, 
					@reun_bPiensaRetornarAlPeru, @reun_cCuandoRetornaAlPeru, @reun_bAfiliadoSeguroSocial, @reun_bAfiliadoAFP, 
					@reun_bAportaSeguroSocial, @reun_bBeneficiadoExterior, @reun_sOcupacionPeru, @reun_sOcupacionExtranjero, 
					@reun_vNombreConvenio, @pers_sUsuarioCreacion, @pers_vIPCreacion, @datFechaRegistro)


				/*REGISTRAR NACIONALIDAD*/
				-------------------------------------------------
				DECLARE @VNACIONALIDAD VARCHAR(100) = ''
					
				IF (ISNULL(@pers_spaisid,0) > 0)
					BEGIN
					   SET @VNACIONALIDAD = ISNULL((SELECT TOP 1 ISNULL([PAIS_VNACIONALIDAD],'') FROM [PS_SISTEMA].[SI_PAIS] P (nolock)
												WHERE P.PAIS_SPAISID = @pers_spaisid),'')
					   EXEC [PN_REGISTRO].[RE_PERSONANACIONALIDAD_ADICIONAR]  @pers_iPersonaId, @pers_spaisid, @VNACIONALIDAD,1, 
																			'A',  @pers_sUsuarioCreacion,  @pers_vIPCreacion
					END

				-------------------------------------
				-- AUDITORIA
				SET @vCamposGenerales = '7,1055,1101,326'
				EXEC PS_SISTEMA.USP_SI_AUDITORIA_ADICIONAR
					@vCamposGenerales = @vCamposGenerales,
					@vNombreTabla = 'RE_PERSONA',
					@vClavePrimaria = @pers_iPersonaId,
					@sOficinaConsularId = @pers_sOficinaConsularId,			
					@audi_vComentario = 'Registro RUNE (Persona, Identificación y Registro Unico)',
					@audi_vHostName = @pers_vHostName,
					@audi_sUsuarioCreacion = @pers_sUsuarioCreacion,
					@audi_vIPCreacion = @pers_vIPCreacion
				------------------------------------	
			END	
			ELSE -- SI EXISTE LA PERSONA
			BEGIN
				SELECT TOP 1 @pers_iPersonaId = peid_iPersonaId 
				FROM PN_REGISTRO.RE_PERSONAIDENTIFICACION (NOLOCK)
				WHERE  peid_sDocumentoTipoId = @peid_sDocumentoTipoId 
				AND peid_vDocumentoNumero = @peid_vDocumentoNumero
				AND peid_cEstado = 'A'
			END		

			/*
				JTITO:
				SE VALIDA LA INSERCIÓN/ACTUALIZACIÓN DE REGISTROS EN PN_REGISTRO.RE_CORRELATIVO
				PARA LAS OFICINAS CONFIGURADAS COMO ACTIVAS EN PS_SISTEMA.SI_PARAMETRO
			*/

			DECLARE @V_OFCO_SGAC	VARCHAR(1)

			SET @V_OFCO_SGAC = (CASE WHEN EXISTS(SELECT PARA_SPARAMETROID FROM PS_SISTEMA.SI_PARAMETRO (NOLOCK) WHERE PARA_VGRUPO = 'CONFIGURACIÓN-OFICINAS-ACTIVAS' 
														AND PARA_VVALOR = CONVERT(VARCHAR, @pers_sOficinaConsularId) AND PARA_CESTADO = 'A') THEN 'S' ELSE 'N' END)

			IF (@pers_bGenera58A = 1)
			BEGIN
				
			IF NOT EXISTS(SELECT actu_iPersonaRecurrenteId 
						FROM PN_REGISTRO.RE_ACTUACIONDETALLE AD (NOLOCK)
						INNER JOIN PN_REGISTRO.RE_ACTUACION A (NOLOCK) ON AD.acde_iActuacionId = A.actu_iActuacionId 
									AND actu_iPersonaRecurrenteId = @pers_iPersonaId
						WHERE AD.acde_sTarifarioId = 122 AND AD.acde_sEstadoId <> 1)
				BEGIN
				
					IF @V_OFCO_SGAC = 'S'
						BEGIN

							if EXISTS(select corr_sCorrelativoId from PN_REGISTRO.RE_CORRELATIVO (NOLOCK)
									where corr_sOficinaConsularId = @pers_sOficinaConsularId and corr_sPeriodo = year(@datFechaRegistro) and [corr_sTarifarioId] IS NULL)
							begin
								select 
									@intCorrelativoActuacionId = [corr_sCorrelativoId],
									@ICorrelativoActuacion = isnull([corr_ICorrelativo], 0) + 1
								from PN_REGISTRO.RE_CORRELATIVO  (NOLOCK)
								where [corr_sOficinaConsularId] = @pers_sOficinaConsularId and [corr_sPeriodo] = year (@datFechaRegistro) and [corr_sTarifarioId] IS NULL;
								
								UPDATE PN_REGISTRO.RE_CORRELATIVO 
									SET [corr_ICorrelativo] = @ICorrelativoActuacion,
										[corr_sUsuarioModificacion] = @pers_sUsuarioCreacion,
										[corr_vIPModificacion] =  @pers_vIPCreacion, 
										[corr_dFechaModificacion] = @datFechaRegistro
								WHERE [corr_sCorrelativoId] = @intCorrelativoActuacionId;

							end
							else		
							begin

								Set @ICorrelativoActuacion = 1;		
								INSERT INTO [PN_REGISTRO].[RE_CORRELATIVO] 
								(
									[corr_sOficinaConsularId],[corr_sPeriodo],[corr_sTarifarioId], 
									[corr_sUsuarioCreacion], [corr_vIPCreacion], [corr_dFechaCreacion],
									corr_ICorrelativo
								)
								VALUES 
								(
									@pers_sOficinaConsularId, year (@datFechaRegistro), NULL, 
									@pers_sUsuarioCreacion, @pers_vIPCreacion, @datFechaRegistro,
									@ICorrelativoActuacion
								);

								SET @intCorrelativoActuacionId = @@IDENTITY;

								SET @vID = CONVERT(VARCHAR(50),@intCorrelativoActuacionId);
								SET @vCamposGenerales = '7,1055,1101,390'
								SET @VCOMENTARIO = 'CORR ACT: 1' + ' - ' + 'Registro Correlativo Actuación DESDE Registro RUNE (Tarifa 58A)'
								EXEC PS_SISTEMA.USP_SI_AUDITORIA_ADICIONAR
									@vCamposGenerales = @vCamposGenerales,
									@vNombreTabla = 'RE_CORRELATIVO',
									@vClavePrimaria = @vID,
									@sOficinaConsularId = @pers_sOficinaConsularId,
									@audi_vComentario = @VCOMENTARIO,
									@audi_vHostName = @pers_vHostName,
									@audi_sUsuarioCreacion = @pers_sUsuarioCreacion,
									@audi_vIPCreacion = @pers_vIPCreacion
								---------------------------------------------------------
							 end


						IF EXISTS(SELECT corr_sCorrelativoId FROM PN_REGISTRO.RE_CORRELATIVO (NOLOCK)
								WHERE corr_sOficinaConsularId = @pers_sOficinaConsularId and corr_sPeriodo = year(@datFechaRegistro) 
								and corr_sTarifarioId = 122)

							begin
								select 
									@intCorrelativoTarifarioId = [corr_sCorrelativoId],
									@ICorrelativoTarifario = ISNULL([corr_ICorrelativo], 0) + 1 
								from PN_REGISTRO.RE_CORRELATIVO (NOLOCK)
								where [corr_sOficinaConsularId] = @pers_sOficinaConsularId and [corr_sPeriodo] = year (@datFechaRegistro) 
								and [corr_sTarifarioId] = 122; -- 58A
		
								UPDATE PN_REGISTRO.RE_CORRELATIVO 
									SET [corr_ICorrelativo] = @ICorrelativoTarifario,
										[corr_sUsuarioModificacion] = @pers_sUsuarioCreacion,
										[corr_vIPModificacion] =  @pers_vIPCreacion, 
										[corr_dFechaModificacion] = @datFechaRegistro
								WHERE [corr_sCorrelativoId] = @intCorrelativoTarifarioId;

								SET @vID = CONVERT(VARCHAR(50),@intCorrelativoTarifarioId);
								SET @vCamposGenerales = '7,1056,1101,390'
								SET @VCOMENTARIO = 'CORR TARIFA: ' + CONVERT(VARCHAR(20), @ICorrelativoTarifario) + ' - ' + 'Actualiza Correlativo Tarifario (Registro RUNE - 58A)';
								EXEC PS_SISTEMA.USP_SI_AUDITORIA_ADICIONAR
									@vCamposGenerales = @vCamposGenerales,
									@vNombreTabla = 'RE_CORRELATIVO',
									@vClavePrimaria = @vID,
									@sOficinaConsularId = @pers_sOficinaConsularId,			
									@audi_vComentario = @VCOMENTARIO,
									@audi_vHostName = @pers_vHostName,
									@audi_sUsuarioCreacion = @pers_sUsuarioCreacion,
									@audi_vIPCreacion = @pers_vIPCreacion
								---------------------------------------------------------
							end
							else
							begin
								Set @ICorrelativoTarifario = 1;
								INSERT INTO [PN_REGISTRO].[RE_CORRELATIVO] 
								(
									[corr_sOficinaConsularId],[corr_sPeriodo],[corr_sTarifarioId], 
									[corr_ICorrelativo],
									[corr_sUsuarioCreacion], [corr_vIpCreacion], [corr_dFechaCreacion]
								)
								VALUES 
								(
									@pers_sOficinaConsularId, year (@datFechaRegistro), 122, 
									@ICorrelativoTarifario,
									@pers_sUsuarioCreacion, @pers_vIPCreacion, @datFechaRegistro
								);

								SET @intCorrelativoTarifarioId = @@IDENTITY;

								------ AUDITORIA -----------------------------------
								SET @vID = CONVERT(VARCHAR(50),@intCorrelativoTarifarioId);
								SET @vCamposGenerales = '7,1055,1101,390'
								SET @VCOMENTARIO = 'CORR TARIFA: 1' + ' - ' + 'Actualiza Correlativo Tarifario (Registro RUNE - 58A)';
								EXEC PS_SISTEMA.USP_SI_AUDITORIA_ADICIONAR
									@vCamposGenerales = @vCamposGenerales,
									@vNombreTabla = 'RE_CORRELATIVO',
									@vClavePrimaria = @vID,
									@sOficinaConsularId = @pers_sOficinaConsularId,			
									@audi_vComentario = @VCOMENTARIO,
									@audi_vHostName = @pers_vHostName,
									@audi_sUsuarioCreacion = @pers_sUsuarioCreacion,
									@audi_vIPCreacion = @pers_vIPCreacion
								---------------------------------------------------------
							end
						END

					INSERT INTO PN_REGISTRO.RE_ACTUACION
						   (actu_sOficinaConsularId, actu_FCantidad, actu_iPersonaRecurrenteId, 
							actu_IFuncionarioId, actu_dFechaRegistro, actu_sEstado, 
							actu_sUsuarioCreacion, actu_vIPCreacion, actu_dFechaCreacion,actu_sCiudadItinerante)
					 VALUES (@pers_sOficinaConsularId, 1, @pers_iPersonaId, 
						   null, @datFechaRegistro, 5, 
						   @pers_sUsuarioCreacion, @pers_vIPCreacion, @datFechaRegistro,@actu_sCiudadItinerante)

					SET @actu_iActuacionId = @@IDENTITY -- toma el nuevo id generado

		
					INSERT INTO PN_REGISTRO.RE_ACTUACIONDETALLE
					(
						acde_iActuacionId, acde_sTarifarioId, acde_sItem,
						acde_dFechaRegistro, acde_bRequisitosFlag, 
						acde_ICorrelativoActuacion, acde_ICorrelativoTarifario, 
						acde_vNotas, acde_sEstadoId, 
						acde_sUsuarioCreacion,acde_vIPCreacion, acde_dFechaCreacion
					)
					 VALUES
					(
						@actu_iActuacionId, 122, 1,
						@datFechaRegistro, 0, 
						@ICorrelativoActuacion, @ICorrelativoTarifario, 
						'', 5, 
						@pers_sUsuarioCreacion, @pers_vIPCreacion, @datFechaRegistro
					)

					declare @pago_iActuacionDetalleId bigint
					SET @pago_iActuacionDetalleId = @@IDENTITY

					SET @vID = CONVERT(VARCHAR(50),@pago_iActuacionDetalleId);
					SET @vCamposGenerales = '7,1055,1101,322'
					EXEC PS_SISTEMA.USP_SI_AUDITORIA_ADICIONAR
						@vCamposGenerales = @vCamposGenerales,
						@vNombreTabla = 'RE_ACTUACIONDETALLE',
						@vClavePrimaria = @vID,
						@sOficinaConsularId = @pers_sOficinaConsularId,			
						@audi_vComentario = 'Registro de Actuación 58A desde RUNE',
						@audi_vHostName = @pers_vHostName,
						@audi_sUsuarioCreacion = @pers_sUsuarioCreacion,
						@audi_vIPCreacion = @pers_vIPCreacion
					----------------------------------------------------------------------
 
					DECLARE @pago_FTipCambioBancario FLOAT;
					DECLARE @pago_FTipCambioConsular FLOAT;

					SET @pago_FTipCambioBancario = PS_SISTEMA.FN_OBTENER_TIPOCAMBIO_BANCARIO(@pers_sOficinaConsularId);
					SET @pago_FTipCambioConsular = PS_SISTEMA.FN_OBTENER_TIPOCAMBIO_CONSULAR(@pers_sOficinaConsularId);
					----------------------------------------------------------
					-- Autor: Miguel Angel Márquez Beltrán
					-- Fecha: 24-08-2016
					-- Objetivo: Leer la moneda local 
					----------------------------------------------------------
					Declare @pago_sMonedaLocalId	smallint

					SET @pago_sMonedaLocalId = (Select top 1 ofco_sMonedaId From PS_SISTEMA.SI_OFICINACONSULAR (NOLOCK)
												Where ofco_sOficinaConsularId = @pers_sOficinaConsularId)											
					----------------------------------------------------------

					DECLARE @pago_vSustentoTipoPago VARCHAR(100) = 'DS 045-2003-RE TARIFA DE DERECHOS CONSULARES'
					DECLARE @sNORMA SMALLINT
					DECLARE @INTESTADOID	SMALLINT

					SET @INTESTADOID = (SELECT EST.esta_sEstadoId FROM [SC_MAESTRO].[MA_ESTADO] EST (NOLOCK)
										WHERE EST.esta_vGrupo = 'NORMA-ESTADO' AND EST.esta_vDescripcionCorta = 'VIGENTE')

					Select @sNORMA = ISNULL(NT.nota_sNormaId,0) 
					FROM PS_SISTEMA.SI_NORMA_TARIFARIO NT (nolock)
					INNER JOIN PS_SISTEMA.SI_NORMA N (NOLOCK) ON NT.nota_sNormaId = N.norm_sNormaId AND N.norm_sEstadoId = 210
					INNER JOIN PS_SISTEMA.SI_TARIFARIO TARIFA (NOLOCK) ON TARIFA.tari_sTarifarioId = NT.nota_sTarifarioId AND TARIFA.tari_sEstadoId = 33
					Where NT.nota_cEstado = 'A'
					And tari_sNumero = 1

					EXECUTE [PN_REGISTRO].[USP_RE_PAGO_ADICIONAR] '3509',@pago_iActuacionDetalleId,@datFechaRegistro,@pago_sMonedaLocalId,0,0,@pago_FTipCambioBancario,@pago_FTipCambioConsular,null,'',0,'',@pers_sOficinaConsularId,@pers_sUsuarioCreacion,@pers_vIPCreacion,@pers_vHostName,NULL,@pago_vSustentoTipoPago,@sNORMA
				END
			END			

		END TRY
		BEGIN CATCH
			SELECT ERROR_MESSAGE() AS ErrorMessage;
		END CATCH
	END

