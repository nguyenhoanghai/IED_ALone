CD /D C:\Program Files\Microsoft SQL Server\110\Tools\Binn
sqlcmd -U sa -P 123 -S .\GPROMSSQLSERVER -Q "EXEC sp_BackupDatabases @backupLocation='E:\GPRO DATA\backup files\', @backupType='F'"