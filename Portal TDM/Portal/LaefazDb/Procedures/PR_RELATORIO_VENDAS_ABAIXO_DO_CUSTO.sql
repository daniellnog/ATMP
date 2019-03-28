CREATE PROCEDURE [dbo].[PR_RELATORIO_VENDAS_ABAIXO_DO_CUSTO]
	@DISPLAYLENGTH INT,
	@DISPLAYSTART INT,
	@SORTCOL INT,
	@SORTDIR NVARCHAR(10),
	@SEARCH NVARCHAR(255) = NULL,
	@LISTARTODOS INT,
	@IDANALISE NVARCHAR(255)
AS
BEGIN
	DECLARE @FIRSTREC INT, @LASTREC INT
	SET @FIRSTREC = @DISPLAYSTART;
	SET @LASTREC = @DISPLAYSTART + @DISPLAYLENGTH;

	IF OBJECT_ID('tempdb..#RelatorioSaida') IS NOT NULL
		DROP TABLE #RelatorioSaida

	/*
		Passo 1: NF DE ENTRADA, DATA DE ENTRADA, CFOP ENTRADA, VALOR UNIT ENT
			1.	Recuperar na planilha de entrada todos os produtos unificados que possuem o valor CFOP (planilha CFOP planilha entradas).
			2.	Manter os que possuem o menor valor unitário (planilha de entrada).
			3.	Recuperar o que tiver menor data de entrada (planilha de entrada).
			4.	Descartar duplicidade de (valor unitário E data de entrada).
	*/

	DECLARE @RelatorioEntrada TABLE
	(
		IdProdutoPai			INT,
		MenorValorUnitario		DECIMAL(18,2),
		MenorDataEntrada		SMALLDATETIME,
		CFOPEntrada				INT,
		NFEntrada				NVARCHAR(MAX),
		SEF_NFE_ECF				VARCHAR(MAX)
	)

	INSERT INTO @RelatorioEntrada (IdProdutoPai, MenorValorUnitario, MenorDataEntrada, CFOPEntrada, NFEntrada, SEF_NFE_ECF)
	SELECT
		Prod.IdProdutoPai, Mov.ValorUnitario, Mov.DataEntrada, Mov.CFOP, Mov.NUM_NF, Mov.SEF_NFE_ECF
	FROM
		Produto Prod 
		INNER JOIN Movimentacao Mov ON Mov.IdProduto = Prod.Id AND Mov.Tipo = 'E'
	WHERE
			Prod.IdAnalise = @IDANALISE
		AND Prod.Ativo = 1 
		AND Prod.IdProdutoPai IS NOT NULL 
		AND Mov.CFOP IN (5403, 5405, 6102, 6104, 6106, 6108, 6112, 6114, 6115, 6117, 6119, 6120, 6123, 6403, 6404, 7102, 7106, 5102, 5104, 5106, 5112, 5114, 5115, 5117)

	DECLARE @RelatorioEntradaAgrupado TABLE
	(
		Ordem					INT,
		IdProdutoPai			INT,
		MenorValorUnitario		DECIMAL(18,2),
		MenorDataEntrada		SMALLDATETIME,
		CFOPEntrada				INT,
		NFEntrada				NVARCHAR(MAX),
		SEF_NFE					VARCHAR(MAX)
	)

	INSERT INTO @RelatorioEntradaAgrupado (Ordem, IdProdutoPai, MenorValorUnitario, MenorDataEntrada, CFOPEntrada, NFEntrada, SEF_NFE)
	SELECT
		RANK() OVER (PARTITION BY IdProdutoPai ORDER BY IdProdutoPai, MenorValorUnitario ASC, MenorDataEntrada ASC) ordem,
		IdProdutoPai, MenorValorUnitario, MenorDataEntrada, CFOPEntrada, NFEntrada, SEF_NFE_ECF
	FROM
		@RelatorioEntrada
	ORDER BY
		IdProdutoPai, MenorValorUnitario ASC, MenorDataEntrada ASC

	DELETE FROM @RelatorioEntradaAgrupado WHERE Ordem <> 1

	/*
		Passo 3: SEF / NFE / ECF, NUM NF/COO, DATA DE EMISSÃO, DESCRIÇÃO DA MERCADORIA, DESCRIÇÃO UNIFICADA, QUANTIDADE, UNIDADE, VALOR UNIT, VALOR TOTAL, CFOP
			1.	Recuperar na planilha de saída todos os produtos unificados que foram vendidos abaixo do valor
			2.	Considerar apenas os produtos unificados que possuem o valor CPFOP (planilha CFOP planilha saídas)
		Passo 4: DIFERENÇA VALOR UNIT
			•	(VALOR UNIT da planilha de saída, passo 3) - (VALOR UNIT MINIMO)
		Passo 5: BC DO ESTORNO
			•	DIFERENÇA VALOR UNIT x (QUANTIDADE da planilha de saída, passo 3)
		Passo 6: ICMS A ESTORNAR 
			•	BC DO ESTORNO x 0,17
	*/

	SELECT
		Mov.SEF_NFE_ECF SEF_NFE_ECF, 
		Mov.NUM_NF NUM_NF, 
		Mov.DataEntrada DataEmissao, 
		Prod.Descricao Descricao, 
		ProdUnificado.Descricao DescricaoUnificada, 
		Mov.Quantidade Quantidade, 
		Unid.Descricao DescricaoUnidade, 
		Mov.ValorUnitario ValorUnitario, 
		Mov.ValorTotal ValorTotal, 
		Mov.CFOP CFOP,
		ParteEntrada.NFEntrada NFEntrada,
		ParteEntrada.MenorDataEntrada DataEntradaMenor, 
		ParteEntrada.CFOPEntrada CFOPEntrada,
		ParteEntrada.SEF_NFE SEF_NFE,
		ParteEntrada.MenorValorUnitario ValorUnitarioMenor,  
		Mov.ValorUnitario - ParteEntrada.MenorValorUnitario ValorUnitarioDiferenca, 
		(Mov.ValorUnitario - ParteEntrada.MenorValorUnitario) * Mov.Quantidade BCEstorno,
		((Mov.ValorUnitario - ParteEntrada.MenorValorUnitario) * Mov.Quantidade) * 0.17 ICMSEstornar
	INTO 
		#RelatorioSaida
	FROM
		@RelatorioEntradaAgrupado ParteEntrada 
		INNER JOIN Produto Prod ON Prod.IdProdutoPai = ParteEntrada.IdProdutoPai
		INNER JOIN Movimentacao Mov ON Mov.IdProduto = Prod.Id AND Mov.Tipo = 'S' 
		INNER JOIN Produto ProdUnificado ON ProdUnificado.Id = Prod.IdProdutoPai
		INNER JOIN Unidade Unid ON Unid.Id = Prod.IdUnidade
	WHERE
		Prod.Ativo = 1
		AND Prod.IdProdutoPai IS NOT NULL 
		AND Prod.IdAnalise = @IdAnalise
		AND Mov.CFOP IN (5102, 5123, 5403, 5405, 6102, 6104, 6106, 6108, 6112, 6114, 6115, 6117, 6119, 6120, 6123, 6403, 6404, 7102, 7106) 
		AND Mov.ValorUnitario < ParteEntrada.MenorValorUnitario * 1.3;

	With CTE_RelatorioVenda as
	(
		Select 
			ROW_NUMBER() over 
			(
				order by 
					case when (@SortCol = 0 and @SortDir='asc') then SEF_NFE_ECF end asc,
					case when (@SortCol = 0 and @SortDir='desc') then SEF_NFE_ECF end desc,

					case when (@SortCol = 1 and @SortDir='asc') then NUM_NF end asc,
					case when (@SortCol = 1 and @SortDir='desc') then NUM_NF end desc,
					
					case when (@SortCol = 2 and @SortDir='asc') then DataEmissao end asc,
					case when (@SortCol = 2 and @SortDir='desc') then DataEmissao end desc,
					
					case when (@SortCol = 3 and @SortDir='asc') then Descricao end asc,
					case when (@SortCol = 3 and @SortDir='desc') then Descricao end desc,
					
					case when (@SortCol = 4 and @SortDir='asc') then DescricaoUnificada end asc,
					case when (@SortCol = 4 and @SortDir='desc') then DescricaoUnificada end desc,
					
					case when (@SortCol = 5 and @SortDir='asc') then Quantidade end asc,
					case when (@SortCol = 5 and @SortDir='desc') then Quantidade end desc,

					case when (@SortCol = 6 and @SortDir='asc') then DescricaoUnidade end asc,
					case when (@SortCol = 6 and @SortDir='desc') then DescricaoUnidade end desc,

					case when (@SortCol = 7 and @SortDir='asc') then ValorUnitario end asc,
					case when (@SortCol = 7 and @SortDir='desc') then ValorUnitario end desc,
					
					case when (@SortCol = 8 and @SortDir='asc') then ValorTotal end asc,
					case when (@SortCol = 8 and @SortDir='desc') then ValorTotal end desc,
					
					case when (@SortCol = 9 and @SortDir='asc') then CFOP end asc,
					case when (@SortCol = 9 and @SortDir='desc') then CFOP end desc,
					
					case when (@SortCol = 10 and @SortDir='asc') then SEF_NFE end asc,
					case when (@SortCol = 10 and @SortDir='desc') then SEF_NFE end desc,

					case when (@SortCol = 11 and @SortDir='asc') then NFEntrada end asc,
					case when (@SortCol = 11 and @SortDir='desc') then NFEntrada end desc,

					case when (@SortCol = 12 and @SortDir='asc') then DataEntradaMenor end asc,
					case when (@SortCol = 12 and @SortDir='desc') then DataEntradaMenor end desc,
					
					case when (@SortCol = 13 and @SortDir='asc') then CFOPEntrada end asc,
					case when (@SortCol = 13 and @SortDir='desc') then CFOPEntrada end desc,
					
					case when (@SortCol = 14 and @SortDir='asc') then ValorUnitarioMenor end asc,
					case when (@SortCol = 14 and @SortDir='desc') then ValorUnitarioMenor end desc,
					
					case when (@SortCol = 15 and @SortDir='asc') then ValorUnitarioDiferenca end asc,
					case when (@SortCol = 15 and @SortDir='desc') then ValorUnitarioDiferenca end desc,
					
					case when (@SortCol = 16 and @SortDir='asc') then BCEstorno end asc,
					case when (@SortCol = 16 and @SortDir='desc') then BCEstorno end desc,
					
					case when (@SortCol = 17 and @SortDir='asc') then ICMSEstornar end asc,
					case when (@SortCol = 17 and @SortDir='desc') then ICMSEstornar end desc
			) as RowNum,
		
			COUNT(*) over() as TotalCount,

			SEF_NFE_ECF,
			NUM_NF,
			DataEmissao,
			Descricao,
			DescricaoUnificada,
			Quantidade,
			DescricaoUnidade,
			ValorUnitario,
			ValorTotal,
			CFOP,
			SEF_NFE,
			NFEntrada,
			DataEntradaMenor,
			CFOPEntrada,
			ValorUnitarioMenor,
			ValorUnitarioDiferenca,
			BCEstorno,
			ICMSEstornar
		FROM
			#RelatorioSaida
		WHERE
			ROUND(ICMSEstornar,2) > 0.05 AND
			(@Search IS NULL 
			OR SEF_NFE_ECF like '%' + @Search + '%'
			OR NUM_NF like '%' + @Search + '%'
			OR DataEmissao like '%' + @Search + '%'
			OR Descricao like '%' + @Search + '%'
			OR Descricao like '%' + @Search + '%'
			OR Quantidade like '%' + @Search + '%'
			OR Descricao like '%' + @Search + '%'
			OR ValorUnitario like '%' + @Search + '%'
			OR ValorTotal like '%' + @Search + '%'
			OR CFOP like '%' + @Search + '%'
			OR SEF_NFE like '%' + @Search + '%'
			OR NFEntrada like '%' + @Search + '%'
			OR DataEntradaMenor like '%' + @Search + '%'
			OR CFOPEntrada like '%' + @Search + '%'
			OR ValorUnitarioMenor like '%' + @Search + '%'
			OR BCEstorno like '%' + @Search + '%'
			OR ICMSEstornar like '%' + @Search + '%')
	)
	Select 
		Rownum, 
		TotalCount, 
		SEF_NFE_ECF, 
		NUM_NF, 
		'' + CONVERT(NVARCHAR(MAX), DataEmissao,103) + '' AS DataEmissao, 
		Descricao, 
		DescricaoUnificada, 
		Quantidade, 
		DescricaoUnidade, 
		ValorUnitario, 
		ValorTotal, 
		CFOP,
		SEF_NFE,
		NFEntrada,
		'' + CONVERT(NVARCHAR(MAX), DataEntradaMenor,103) + '' AS DataEntradaMenor, 
		CFOPEntrada,
		ValorUnitarioMenor,  
		ValorUnitarioDiferenca, 
		BCEstorno,
		ICMSEstornar
	from 
		CTE_RelatorioVenda where @ListarTodos = 1 OR (@ListarTodos  = 0 AND RowNum > @FirstRec and RowNum <= @LastRec)
END
GO