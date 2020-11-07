CREATE TABLE [dbo].[Books]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(100) NOT NULL, 
    [Author] VARCHAR(100) NOT NULL, 
    [YearPublished] INT NOT NULL,

)
