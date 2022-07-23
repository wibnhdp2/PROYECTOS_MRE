USE [BD_CARDIP]

go

--======================================================================
--SISTEMA    :  CARDIP -administrador
--OBJETIVO    :  Adicion de campo observacion con la finalidad de grabar el motivo de la observacion del tramite de carnet  
--CREADO POR  :  VPIPA
--FECHA      :  29/06/2021
--======================================================================



IF NOT EXISTS (SELECT NAME
               FROM   sys.columns
               WHERE  object_id = Object_id(N'[SC_CARDIP].[CD_CARNE_IDENTIDAD]')
                      AND NAME = 'CAID_VOBSERVACION')
  BEGIN
      ALTER TABLE sc_cardip.cd_carne_identidad
        ADD caid_vobservacion VARCHAR(250) NULL

      EXEC sys.Sp_addextendedproperty
        @NAME=N'MS_DESCRIPTION',
        @VALUE=N'PARA GRABAR EL MOTIVO DE LA OBSERVACION DEL TRAMITE DE CARNET',
        @LEVEL0TYPE=N'SCHEMA',
        @LEVEL0NAME=N'SC_CARDIP',
        @LEVEL1TYPE=N'TABLE',
        @LEVEL1NAME=N'CD_CARNE_IDENTIDAD',
        @LEVEL2TYPE=N'COLUMN',
        @LEVEL2NAME=N'CAID_VOBSERVACION'
  END

go

--======================================================================
--SISTEMA    :  CARDIP -administrador
--OBJETIVO    :  adicion de Estado de observacion a la tabla de Estado, debido a la necesidad de  cambiar el estado a observado en el sistema Cardip 
--CREADO POR  :  VPIPA
--FECHA      :  29/06/2021  
--======================================================================
IF NOT EXISTS (SELECT esta_vgrupo
               FROM   sc_maestro.ma_estado WITH(NOLOCK)
               WHERE  esta_vgrupo = 'CARNE IDENTIDAD - ESTADO'
                      AND esta_vdescripcioncorta = 'OBSERVADO')
  BEGIN
      INSERT INTO sc_maestro.ma_estado
                  (esta_vdescripcioncorta,
                   esta_vdescripcionlarga,
                   esta_vgrupo,
                   esta_cestado,
                   esta_susuariocreacion,
                   esta_vipcreacion,
                   esta_dfechacreacion,
                   esta_susuariomodificacion,
                   esta_vipmodificacion,
                   esta_dfechamodificacion)
      VALUES     ( 'OBSERVADO',
                   'OBSERVADO',
                   'CARNE IDENTIDAD - ESTADO',
                   'A',
                   1,
                   '0.0.0.0',
                   Getdate(),
                   NULL,
                   NULL,
                   NULL)
  END

go

--======================================================================
--SISTEMA    :  CARDIP - REg-Linea
--OBJETIVO    :  adicion de Estado de atendido a la tabla de Estado, debido a la necesidad de  cambiar el estado a estado atendedido en el sistema Cardip en linea
--CREADO POR  :  VPIPA
--FECHA      :  29/06/2021  
--======================================================================
IF NOT EXISTS (SELECT esta_vdescripcioncorta
               FROM   sc_maestro.ma_estado WITH(NOLOCK)
               WHERE  esta_vgrupo = 'REG LINEA - ESTADO'
                      AND esta_vdescripcioncorta = 'ATENDIDO')
  BEGIN
      INSERT INTO sc_maestro.ma_estado
                  (esta_vdescripcioncorta,
                   esta_vdescripcionlarga,
                   esta_vgrupo,
                   esta_cestado,
                   esta_susuariocreacion,
                   esta_vipcreacion,
                   esta_dfechacreacion,
                   esta_susuariomodificacion,
                   esta_vipmodificacion,
                   esta_dfechamodificacion)
      VALUES     ( 'ATENDIDO',
                   'ATENDIDO',
                   'REG LINEA - ESTADO',
                   'A',
                   1,
                   '0.0.0.0',
                   Getdate(),
                   NULL,
                   NULL,
                   NULL)
  END

go

IF NOT EXISTS (SELECT esta_vdescripcioncorta
               FROM   sc_maestro.ma_estado
               WHERE  esta_vgrupo = 'REG LINEA - ESTADO'
                      AND esta_vdescripcioncorta = 'SUBSANADO')
  BEGIN
      INSERT INTO sc_maestro.ma_estado
                  (esta_vdescripcioncorta,
                   esta_vdescripcionlarga,
                   esta_vgrupo,
                   esta_cestado,
                   esta_susuariocreacion,
                   esta_vipcreacion,
                   esta_dfechacreacion,
                   esta_susuariomodificacion,
                   esta_vipmodificacion,
                   esta_dfechamodificacion)
      VALUES     ( 'SUBSANADO',
                   'SUBSANADO',
                   'REG LINEA - ESTADO',
                   'A',
                   1,
                   '0.0.0.0',
                   Getdate(),
                   NULL,
                   NULL,
                   NULL)
  END

go

--======================================================================
--SISTEMA    :  CARDIP - REg-Linea
--OBJETIVO    :  Creacion de tabla CD_MENSAJE_ESTADO con la finalidad de configurar el mensaje por Estado. 
--          ejemplo para el estado de Oservado se configura un mensaje, esto se enviará automaticamente 
--          desde el sistema de cardip al observar la solicitud de carnet
--CREADO POR  :  VPIPA
--FECHA      :  29/06/2021
--Tabla      : CD_MENSAJE_ESTADO
--Columnas    :
--        MEES_SESTADOID : id del estado referenciado al tabla maestra  SC_MAESTRO.MA_ESTADO
--        MEES_VMENSAJE : CAMPO PARA GRABAR EL TEXTO DEL MENSAJE
--        MEES_CESTADO  : campo para habilitar o inabilitar el registro
--======================================================================
IF NOT EXISTS (SELECT xtype
               FROM   sysobjects
               WHERE  NAME = 'CD_MENSAJE_ESTADO'
                      AND xtype = 'U')
  BEGIN
      CREATE TABLE sc_cardip.cd_mensaje_estado
        (
           mees_sestadoid SMALLINT,
           mees_vmensaje  VARCHAR(250),
           mees_cestado   CHAR(1)
           CONSTRAINT [pk_MEES_SESTADOID] PRIMARY KEY CLUSTERED ( mees_sestadoid
           ),
           CONSTRAINT [FK_SE_CD_MENSAJE_ESTADO] FOREIGN KEY(mees_sestadoid)
           REFERENCES
           sc_maestro.ma_estado (esta_sestadoid)
        )
  END

go

IF NOT EXISTS(SELECT 1
              FROM   sys.extended_properties P
              WHERE  P.major_id = Object_id('SC_CARDIP.CD_MENSAJE_ESTADO'))
  BEGIN
      EXEC sys.Sp_addextendedproperty
        @NAME=N'MS_DESCRIPTION',
        @VALUE=
  N'ID DEL ESTADO REFERENCIADO AL TABLA MAESTRA  SC_MAESTRO.MA_ESTADO',
        @LEVEL0TYPE=N'SCHEMA',
        @LEVEL0NAME=N'SC_CARDIP',
        @LEVEL1TYPE=N'TABLE',
        @LEVEL1NAME=N'CD_MENSAJE_ESTADO',
        @LEVEL2TYPE=N'COLUMN',
        @LEVEL2NAME=N'MEES_SESTADOID'

      EXEC sys.Sp_addextendedproperty
        @NAME=N'MS_DESCRIPTION',
        @VALUE=N'CAMPO PARA GRABAR EL TEXTO DEL MENSAJE',
        @LEVEL0TYPE=N'SCHEMA',
        @LEVEL0NAME=N'SC_CARDIP',
        @LEVEL1TYPE=N'TABLE',
        @LEVEL1NAME=N'CD_MENSAJE_ESTADO',
        @LEVEL2TYPE=N'COLUMN',
        @LEVEL2NAME=N'MEES_VMENSAJE'

      EXEC sys.Sp_addextendedproperty
        @NAME=N'MS_DESCRIPTION',
        @VALUE=N'campo para habilitar o inabilitar el registro',
        @LEVEL0TYPE=N'SCHEMA',
        @LEVEL0NAME=N'SC_CARDIP',
        @LEVEL1TYPE=N'TABLE',
        @LEVEL1NAME=N'CD_MENSAJE_ESTADO',
        @LEVEL2TYPE=N'COLUMN',
        @LEVEL2NAME=N'MEES_CESTADO'
  END 