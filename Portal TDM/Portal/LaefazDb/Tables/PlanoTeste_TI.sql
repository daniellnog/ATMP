﻿CREATE TABLE [dbo].[PlanoTeste_TI](
    [ID] INT NOT NULL IDENTITY, 
	[SUBPROJETO] [nvarchar](32) NULL,
	[ENTREGA] [nvarchar](32) NULL,
	[PROJETO] [nvarchar](32) NOT NULL,
	[PLANO_TESTE] [nvarchar](256) NULL,
	[TESTE] [numeric](10, 0) NOT NULL,
	[CT] [numeric](10, 0) NOT NULL,
	[RELEASE] [nvarchar](32) NULL,
	[CICLO] [nvarchar](32) NULL,
	[FORNECEDOR] [nvarchar](32) NULL,
	[SISTEMA] [nvarchar](64) NULL,
	[NOME] [nvarchar](256) NULL,
	[COMPLEXIDADE] [nvarchar](32) NULL,
	[UAT] [nvarchar](32) NULL,
	[MASSA_TESTE] [nvarchar](32) NULL,
	[DT_PLANEJAMENTO] [datetime2](7) NULL,
	[DT_EXECUCAO] [datetime2](7) NULL,
	[STATUS_EXEC_CT] [nvarchar](32) NULL,
	[MOTIVO_CANCELAMENTO_CT] [nvarchar](64) NULL,
	[TESTADOR] [nvarchar](32) NULL,
	[QT_STEPS] [numeric](10, 0) NULL,
	[PASTA_LM] [nvarchar](256) NULL,
	[CT_AUTOMATIZADO] [nvarchar](32) NULL,
	[EXECUCAO_AUTOMATICA] [nvarchar](32) NULL,
	[MOTIVO_EXECUCAO_MANUAL] [nvarchar](256) NULL,
	[FABRICA_TESTE] [nvarchar](32) NULL,
	[ITERATIONS] [nvarchar](256) NULL,
	[VARIANTE] [nvarchar](256) NULL,
	[PRE_CONDICAO] [nvarchar](2000) NULL,
	[OBS] [nvarchar](256) NULL,
	[NRO_CENARIO] [numeric](10, 0) NULL,
	[NRO_CT] [numeric](10, 0) NULL,
	[EVIDENCIA_VALIDACAO_CLIENTE] [nvarchar](32) NULL,
	[EVIDENCIA_VALIDACAO_TECNICA] [nvarchar](32) NULL,
	[PLANO_VALIDACAO_CLIENTE] [nvarchar](32) NULL,
	[PLANO_VALIDACAO_TECNICA] [nvarchar](32) NULL,
	[CT_SUCESSOR] [nvarchar](512) NULL,
	[PRE_REQUISITO] [nvarchar](512) NULL, 
    CONSTRAINT [PK_CT] PRIMARY KEY ([ID])
) ON [PRIMARY]

GO

