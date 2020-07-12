CREATE TABLE [dbo].[Employee]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY (1,1), 
    [Name] VARCHAR(100) NOT NULL, 
    [EmployerId] INT NOT NULL,

    CONSTRAINT FK_EmployeeAirline FOREIGN KEY (EmployerId) REFERENCES Airline(Id)
)
