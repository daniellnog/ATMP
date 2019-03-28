



IF NOT EXISTS (SELECT * FROM TelaMapaCalor)
BEGIN
	INSERT INTO TelaMapaCalor (Descricao, Caminho) 
	SELECT 'DataPool', 'C:\TDM_Portal\PortalTDM\Assets\Images\TelaDatapool.png' 

	INSERT INTO TelaMapaCalor (Descricao, Caminho) 
	SELECT 'TestData', 'C:\TDM_Portal\PortalTDM\Assets\Images\TelaTestData.png' 
END




















