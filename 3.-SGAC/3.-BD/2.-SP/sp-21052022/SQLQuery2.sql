USE [BD_SGAC]
GO



SELECT [PS_ACCESORIOS].[FN_CONVERTIR_MAYUSCULAS_MINUSCULAS_MRE]('  PAMELA NOEMI DIAZ SALAS, IDENTIFICADA CON DOCUMENTO NACIONAL DE IDENTIDAD Nº 42071163 (CUATRO, DOS, CERO, SIETE, UNO, UNO, SEIS, TRES), DE NACIONALIDAD PERUANA, DE ESTADO CIVIL SOLTERA, DE OCUPACIÓN ABOGADO, CON DOMICILIO EN C/, CARDENAL CISNEROS Nº 52, 1º IZQ. SANTANDER, SANTANDER, ESPAÑA; A QUIEN PLENAMENTE IDENTIFICO Y QUIEN PROCEDE POR DERECHO PROPIO, DE LO QUE DOY FE')


/*
SELECT [PS_ACCESORIOS].[FN_PADLEFT]('1547865497841',23,'0')

DECLARE @cadenaIn VARCHAR(MAX) = ('  PAMELA NOEMI DÍAZ SALAS')
DECLARE @opcion INT = 5

DECLARE @contador INT
DECLARE @caracter CHAR(1)
DECLARE @cadenaOut VARCHAR(MAX) =''

--Quitar espacios inicio y final	@opcion = 0
--Convierte a Mayúscula				@opcion = 1
--Convierte a Minúscula				@opcion = 2
--Convierte a Capital				@opcion = 3
--Convierte a Capital 1er caracter	@opcion = 4
--Convierte texto al revés			@opcion = 5

SET @cadenaIn = RTRIM(LTRIM(@cadenaIn))

IF(@opcion = 1)
BEGIN
	 SET @cadenaOut  = UPPER(@cadenaIn)
END

IF(@opcion = 2)
BEGIN
	 SET @cadenaOut  = LOWER(@cadenaIn)
END

IF(@opcion = 3)
BEGIN 
	SET @contador = 1
	WHILE ( @contador <= LEN(@cadenaIn))
	BEGIN
		SET @caracter = SUBSTRING(@cadenaIn,@contador,1)
		IF ( @caracter = ' ') 
		BEGIN
			SET @contador +=1
			SET @cadenaOut  = @cadenaOut  +' '+(SUBSTRING(@cadenaIn,@contador,1))
			SET @contador +=1 
		END
		ELSE 
		BEGIN
			SET @cadenaOut  = @cadenaOut  + IIF(@contador!=1,LOWER(@caracter),@caracter)
			SET @contador  = @contador  + 1
		END
	END
END

IF(@opcion = 4)
BEGIN
	 SET @cadenaOut  = UPPER(LEFT(@cadenaIn,1)) + LOWER(RIGHT(@cadenaIn,LEN(@cadenaIn)-1))
END

IF(@opcion = 5)
BEGIN
	 SET @cadenaOut  = REVERSE(@cadenaIn)
END


PRINT  IIF((@opcion IS NULL) OR @opcion=0,@cadenaIn,@cadenaOut )


*/


