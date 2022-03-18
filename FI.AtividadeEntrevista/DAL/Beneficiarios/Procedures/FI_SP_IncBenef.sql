﻿CREATE PROC FI_SP_IncBeneficiario
    @NOME          VARCHAR (100),
    @CPF           VARCHAR (14),
	@IDCLIENTE	   BIGINT
AS
BEGIN
	INSERT INTO BENEFICIARIOS (NOME, CPF, IDCLIENTE) 
	VALUES (@NOME, @CPF, @IDCLIENTE)

	SELECT SCOPE_IDENTITY()
END