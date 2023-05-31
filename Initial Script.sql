-- create database
Create DATABASE TaskManagement;

USE TaskManagement;
GO
-- create table
CREATE TABLE Task (
  	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](60) NOT NULL,
	[Status] [int] NOT NULL,
	PRIMARY KEY (Id)
);
