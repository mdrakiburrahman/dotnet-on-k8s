# - - - - - - - - - - - - - - 
# Scaffolding the Service
# - - - - - - - - - - - - - - 

# New Webapi Project
# https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-6.0&tabs=visual-studio-code
dotnet new webapi -n PlatformService

# Remove weather nonsense
rm -rf ./PlatformService/WeatherForecast.cs
rm -rf ./PlatformService/Controllers/WeatherForecastController.cs

# Add dependencies
# https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-add-package
cd PlatformService
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection -v 8.1.1
# Work with Databases (SQL and In Memory)
dotnet add package Microsoft.EntityFrameworkCore -v 5.0.8
dotnet add package Microsoft.EntityFrameworkCore.Design -v 5.0.8 # Add for design time, not added for runtime
dotnet add package Microsoft.EntityFrameworkCore.InMemory -v 5.0.8
dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 5.0.8
# Note that Swashbuckle is already added by default - working with Swagger
# https://github.com/domaindrivendev/Swashbuckle.AspNetCore

# - - - - - - - - - - - - - - 
# Data Layer - Model
# - - - - - - - - - - - - - - 
cd /workspaces/dotnet-on-k8s/PlatformService/
mkdir Models
touch Models/Platform.cs # Internal model of the data
# Models = Internal
# DTO = External

# - - - - - - - - - - - - - - 
# Data
# - - - - - - - - - - - - - - 
cd /workspaces/dotnet-on-k8s/PlatformService/
mkdir Data
touch Data/AppDbContext.cs

# - - - - - - - - - - - - - - 
# Repository
# - - - - - - - - - - - - - - 
# We will use the "Interface Concrete Class" pattern:
#  - Interface: What signatures to implement
#  - Concrete Class: Implementation of the interface
# https://stackoverflow.com/questions/19789590/interface-abstract-class-concrete-class-pattern
# We're going to inject our repository through dependency injection via Startup Class for this one
cd /workspaces/dotnet-on-k8s/PlatformService/Data/
touch IPlatformRepo.cs # The "I" is for the interface