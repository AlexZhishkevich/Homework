UPDATE Catalog
SET Name = 'Devices'
WHERE ID =(SELECT ID FROM Catalog WHERE Id=1);