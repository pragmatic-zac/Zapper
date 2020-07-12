CREATE TABLE [dbo].[AirlineAircraft]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [AircraftId] INT NOT NULL, 
    [AirlineId] INT NOT NULL, 
    [DateAcquired] DATETIME2 NOT NULL, 
    [Price] INT NULL,
    [TailNumber] VARCHAR(50) NOT NULL, 

    CONSTRAINT FK_AirlineAircraft FOREIGN KEY (AircraftId) REFERENCES Aircraft(Id),
    CONSTRAINT FK_AircraftAirline FOREIGN KEY (AirlineId) REFERENCES Airline(Id)
)
