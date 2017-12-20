Ring Down Visual Console  Documentation
------------


Development
---

[.NET Core 2.0.3 SDK](https://download.microsoft.com/download/D/7/2/D725E47F-A4F1-4285-8935-A91AE2FCC06A/dotnet-sdk-2.0.3-win-x64.exe)


[Visual Studio 2017.4](https://www.visualstudio.com/)


[DotVVM Visual Studio Extension](https://marketplace.visualstudio.com/items?itemName=TomasHerceg.DotVVMforVisualStudio-17892) (not needed to compile, just to develop)


Notes:	

This is using EF Code First. To change the database, you simply need to change the RingDownConsoleDbContext, and associated model classes, then run:

	Add-Migration MigrationName
				
while targetting the RingDownConsole.Models project from the Package Manager Console, or the equivalent dotnet-ef commands from the developer console. Then run:
		
	Update-Database
	
		
This project uses the website Fontello to generate an optimized subset of Font Awesome, which can import the included config.js file in the wwwroot folder to regenerate it.
		
All of the included nuget packages and third party content are under open source software licenses
				
	


Deployment
---

[.NET Core 2.0.3 Web Hosting installer](https://download.microsoft.com/download/5/C/1/5C190037-632B-443D-842D-39085F02E1E8/DotNetCore.2.0.3-WindowsHosting.exe)


IIS by default, although it can run standalone

Windows by default, although it can run on linux and other environments.

SQL Server 2008 or higher by default, although it can use MySQL, MariaDB, or any other database server supported by Entity Framework Core


