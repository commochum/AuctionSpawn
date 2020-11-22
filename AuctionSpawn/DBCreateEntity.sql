-- =======================================================
-- Create AuctionSpawn Database
-- Created by Karen T. Atas (kta)
-- 17-11-2020
-- ASP.Net MVC - C#
-- =======================================================
USE MASTER
GO

IF EXISTS ( SELECT *
			FROM master..sysdatabases
			WHERE name = N'AuctionSpawn_kta')
DROP DATABASE AuctionSpawn_kta;
GO

CREATE DATABASE AuctionSpawn_kta;
GO

SET DATEFORMAT dmy;
GO

USE AuctionSpawn_kta
GO

CREATE TABLE dbo.Auction(
ID					INT IDENTITY(1,1) NOT NULL,
Description			NVARCHAR(50) NOT NULL UNIQUE,
Date    			DATETIME NOT NULL
CONSTRAINT Auction_PK PRIMARY KEY (ID)
)
GO

CREATE TABLE dbo.Item(
ItemID			    INT IDENTITY(1,1) NOT NULL,
AuctionID			INT,
Title				NVARCHAR(50) NOT NULL UNIQUE,
Description			NVARCHAR(100),
StartPrice			INT
CONSTRAINT Item_PK PRIMARY KEY (ItemID, AuctionID),
CONSTRAINT Item_Auction_FK_Cascade FOREIGN KEY (AuctionID) REFERENCES Auction ON DELETE CASCADE
)
GO

ALTER Table Auction ADD ItemQuantity INT NOT NULL DEFAULT(10) 
GO