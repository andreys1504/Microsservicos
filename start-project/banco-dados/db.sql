CREATE DATABASE Pedidos_Microsservicosv2
GO

USE Pedidos_Microsservicosv2
GO


BEGIN TRANSACTION;
GO

CREATE TABLE [MessageInBroker]
(
	[MessageId] INT PRIMARY KEY IDENTITY(1,1)
	,[FullName] VARCHAR(1000) NOT NULL
	,[Name] VARCHAR(200) NOT NULL
	,[CurrentContext] VARCHAR(100) NOT NULL
	,[Body] NVARCHAR(MAX) NOT NULL
	,[Stored] DATETIMEOFFSET NOT NULL
	,[Processed] DATETIMEOFFSET NULL
	,[Num] INT NOT NULL
	,[IsEvent] BIT NOT NULL
	,[OriginalContext] VARCHAR(100) NULL
	,[MessageIdReference] INT NULL
	,[MessageInBroker] DATETIMEOFFSET NULL
	,[Queue] NVARCHAR(100) NULL
);
GO

COMMIT;
GO




CREATE DATABASE Pagamento_Microsservicosv2
GO

USE Pagamento_Microsservicosv2
GO


BEGIN TRANSACTION;
GO

CREATE TABLE [MessageInBroker]
(
	[MessageId] INT PRIMARY KEY IDENTITY(1,1)
	,[FullName] VARCHAR(1000) NOT NULL
	,[Name] VARCHAR(200) NOT NULL
	,[CurrentContext] VARCHAR(100) NOT NULL
	,[Body] NVARCHAR(MAX) NOT NULL
	,[Stored] DATETIMEOFFSET NOT NULL
	,[Processed] DATETIMEOFFSET NULL
	,[Num] INT NOT NULL
	,[IsEvent] BIT NOT NULL
	,[OriginalContext] VARCHAR(100) NULL
	,[MessageIdReference] INT NULL
	,[MessageInBroker] DATETIMEOFFSET NULL
	,[Queue] NVARCHAR(100) NULL
);
GO

COMMIT;
GO




CREATE DATABASE Catalogo_Microsservicosv2
GO

USE Catalogo_Microsservicosv2
GO


BEGIN TRANSACTION;
GO

CREATE TABLE [MessageInBroker]
(
	[MessageId] INT PRIMARY KEY IDENTITY(1,1)
	,[FullName] VARCHAR(1000) NOT NULL
	,[Name] VARCHAR(200) NOT NULL
	,[CurrentContext] VARCHAR(100) NOT NULL
	,[Body] NVARCHAR(MAX) NOT NULL
	,[Stored] DATETIMEOFFSET NOT NULL
	,[Processed] DATETIMEOFFSET NULL
	,[Num] INT NOT NULL
	,[IsEvent] BIT NOT NULL
	,[OriginalContext] VARCHAR(100) NULL
	,[MessageIdReference] INT NULL
	,[MessageInBroker] DATETIMEOFFSET NULL
	,[Queue] NVARCHAR(100) NULL
);
GO

COMMIT;
GO