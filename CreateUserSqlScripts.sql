USE [master]
GO

--DROP LOGIN [skomja00]
--GO

-- Which database to use.
USE [Email]
GO

-- Delete existing user.
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'skomja00')
DROP USER [skomja00]
GO

-- Which database to use.
USE [master]
GO


-- Delete existing login.
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'skomja00')
DROP LOGIN [skomja00]
GO

-- Add new login.
CREATE LOGIN [skomja00] WITH PASSWORD=N'Sqls3rv3r$uck$', DEFAULT_DATABASE=[Email]
GO

-- Which database to use.
USE [Email]
GO

-- Add new user.
CREATE USER [skomja00] FOR LOGIN [skomja00] WITH DEFAULT_SCHEMA=[dbo]
GO

--CREATE ROLE db_executor;
--GRANT EXECUTE TO db_executor;


-- Add to database read / write roles
EXEC sp_addrolemember 'db_datareader', 'skomja00'
EXEC sp_addrolemember 'db_datawriter', 'skomja00'
EXEC sp_addrolemember 'db_executor', 'skomja00'
GO




