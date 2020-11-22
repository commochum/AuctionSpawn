-- =======================================================
-- Inserts AuctionSpawn Database
-- Created by Karen T. Atas (kta)
-- 17-11-2020
-- ASP.Net MVC - C#
-- =======================================================
USE MASTER
GO

--Test Inserts

USE [AuctionSpawn_kta]
GO

INSERT INTO [dbo].[Auction]
           ([Description]
           ,[Date])
     VALUES
           ('AuctionTest1'
           ,GETDATE())
GO

INSERT INTO [dbo].[Item]
           ([AuctionID]
           ,[Title]
		   ,[Description]
           ,[StartPrice])
     VALUES
           ((SELECT IDENT_CURRENT('AUCTION'))
           ,'ItemTest1', 'This is a Test'
           ,1600.50)
GO

USE [AuctionSpawn_kta]
GO

INSERT INTO [dbo].[Auction]
           ([Description]
           ,[Date])
     VALUES
           ('AuctionTest-Quantity'
           ,GETDATE())
GO


INSERT INTO [dbo].[Item]
           ([AuctionID]
           ,[Title]
		   ,[Description]
           ,[StartPrice])
     VALUES
           ((SELECT IDENT_CURRENT('AUCTION'))
           ,'ItemTest-Q1', 'This is a Test'
           ,1600.50)
GO

INSERT INTO [dbo].[Item]
           ([AuctionID]
           ,[Title]
		   ,[Description]
           ,[StartPrice])
     VALUES
           ((SELECT IDENT_CURRENT('AUCTION'))
           ,'ItemTest-Q2', 'This is a Test'
           ,20)
GO

INSERT INTO [dbo].[Item]
           ([AuctionID]
           ,[Title]
		   ,[Description]
           ,[StartPrice])
     VALUES
           ((SELECT IDENT_CURRENT('AUCTION'))
           ,'ItemTest-Q3', 'This is a Test'
           ,89)
GO


INSERT INTO [dbo].[Item]
           ([AuctionID]
           ,[Title]
		   ,[Description]
           ,[StartPrice])
     VALUES
           ((SELECT IDENT_CURRENT('AUCTION'))
           ,'ItemTest-Q4', 'This is a Test'
           ,190)
GO

INSERT INTO [dbo].[Item]
           ([AuctionID]
           ,[Title]
		   ,[Description]
           ,[StartPrice])
     VALUES
           ((SELECT IDENT_CURRENT('AUCTION'))
           ,'ItemTest-Q5', 'This is a Test'
           ,60)
GO


INSERT INTO [dbo].[Item]
           ([AuctionID]
           ,[Title]
		   ,[Description]
           ,[StartPrice])
     VALUES
           ((SELECT IDENT_CURRENT('AUCTION'))
           ,'ItemTest-Q6', 'This is a Test'
           ,65)
GO

INSERT INTO [dbo].[Item]
           ([AuctionID]
           ,[Title]
		   ,[Description]
           ,[StartPrice])
     VALUES
           ((SELECT IDENT_CURRENT('AUCTION'))
           ,'ItemTest-Q7', 'This is a Test'
           ,79)
GO


INSERT INTO [dbo].[Item]
           ([AuctionID]
           ,[Title]
		   ,[Description]
           ,[StartPrice])
     VALUES
           ((SELECT IDENT_CURRENT('AUCTION'))
           ,'ItemTest-Q8', 'This is a Test'
           ,1300)
GO

INSERT INTO [dbo].[Item]
           ([AuctionID]
           ,[Title]
		   ,[Description]
           ,[StartPrice])
     VALUES
           ((SELECT IDENT_CURRENT('AUCTION'))
           ,'ItemTest-Q9', 'This is a Test'
           ,10000)

		   GO

Select * from Auction
Select * from Item

GO