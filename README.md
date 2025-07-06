# TaskManager

This task management application is built using ASP.NET MVC and includes:

- Entity Framework Core - ORM for working with the database

- FluentMigrator - for versioning, managing and creating the database

- Microsoft SQL Server

The application is by default configured to create the database on a local SQL Server (Local MSSQL). - To use a different database, you can change the connection string in the `appsettings.json` file. 
Only SQL Server is supported at the moment.