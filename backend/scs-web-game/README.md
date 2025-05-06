# scs-web-game
# SQL-Express
# Install Express: https://www.microsoft.com/de-ch/sql-server/sql-server-downloads

# Backend 
# VS22
# .net 8

# Install Packages from NuGet Package Manager
#
# Microsoft.EntityFrameworkCore
# Microsoft.EntityFrameworkCore.Design
# Microsoft.EntityFrameworkCore.SqlServer
# Microsoft.EntityFrameworkCore.Tools
# 

# Connect backend to database. 
# Use Package Manager Console
# 
# Import dbo from sql into backend
# Scaffold-DbContext "Server=YourServer;Database=web-game; Integrated Security=true;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
#
# migrate dbo from backend to sql. 
# Add-Migration "MigrationTopic"
# Update-Database 

# Install Packages from NuGet Package Manager
# Logging
# Serilog.AspNetCore
# Serilog.Enrichers.Thread

# Install Packages from NuGet Package Manager
# Unit test 
# NUnit 
# NUnit3TestAdapter
#

# Rest Api
# Nswag studio v14.0.8
# in .net9 there ist no No compatibility with Nswag studio