﻿CREATE PROC FI_SP_DelBeneficiario
	@ID			   BIGINT,
	@IDCLIENTE	   BIGINT
AS
BEGIN
	IF (@ID > 0)
		BEGIN 
			DELETE FROM [BENEFICIARIOS]
			WHERE [ID] = @ID AND [IDCLIENTE] = @IDCLIENTE
		END
	ELSE
		BEGIN
			DELETE FROM [BENEFICIARIOS] 
			WHERE [IDCLIENTE] = @IDCLIENTE
		END 
END