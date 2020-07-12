CREATE TABLE [dbo].[Aircraft]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY (1,1), 
    [Manufacturer] VARCHAR(50) NOT NULL, 
    [Model] VARCHAR(50) NOT NULL, 
    [Engines] INT NOT NULL, 
    [IsJet] BIT NOT NULL, 
    [PrimaryUse] VARCHAR(50) NULL
)
