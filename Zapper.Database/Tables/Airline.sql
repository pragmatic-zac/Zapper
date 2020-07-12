CREATE TABLE [dbo].[Airline]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] VARCHAR(50) NOT NULL, 
    [DateFounded] DATETIME2 NOT NULL, 
    [TotalEmployees] INT NULL
)
