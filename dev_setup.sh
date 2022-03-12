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
touch PlatformRepo.cs # The class implementing the interface
touch PrepDb.cs # Prepare some mock data and insert into our database

# Test out
dotnet dev-certs https # Build a developer certificate
dotnet build
dotnet run
root ➜ /workspaces/dotnet-on-k8s/PlatformService (main ✗) $ dotnet run
Building...
# --> Seeding data...
# warn: Microsoft.AspNetCore.Server.Kestrel[0]
#       Unable to bind to https://localhost:5001 on the IPv6 loopback interface: 'Cannot assign requested address'.
# warn: Microsoft.AspNetCore.Server.Kestrel[0]
#       Unable to bind to http://localhost:5000 on the IPv6 loopback interface: 'Cannot assign requested address'.
# info: Microsoft.Hosting.Lifetime[0]
#       Now listening on: https://localhost:5001
# info: Microsoft.Hosting.Lifetime[0]
#       Now listening on: http://localhost:5000
# info: Microsoft.Hosting.Lifetime[0]
#       Application started. Press Ctrl+C to shut down.
# info: Microsoft.Hosting.Lifetime[0]
#       Hosting environment: Development
# info: Microsoft.Hosting.Lifetime[0]
#       Content root path: /workspaces/dotnet-on-k8s/PlatformService

# So we have a running service now with some data - but we can't retrieve it yet.

# So we will create some DTOs now - Data Transfer Objects.

# - - - - - - - - - - - - - - 
# DTO - Data Transfer Object
# - - - - - - - - - - - - - - 
# Basically an external representation of our internal model/repository.
# We don't want to expose our internal model to the outside world.
# E.g. we might want to change our internal model in code anytime, but not break the contract with the external world.
cd /workspaces/dotnet-on-k8s/PlatformService
mkdir Dtos
touch Dtos/PlatformReadDto.cs
# The use case is Consumers will be reading from us - so we want to have a Read Dto for anyone reading from us
# The other is we have another Dto for a create operation, what data we expect to be passed in - will be slightly different.
touch Dtos/PlatformCreateDto.cs

# Now, we have our Dtos and our internal model/repo, but they don't know about each other!

# Enter 
# Automapper: Put it into Startup, performs the mapping
# Profile: Profile to map Dto to Model

# - - - - - - - - - - - - - - 
# Profile
# - - - - - - - - - - - - - - 
mkdir Profiles
touch Profiles/PlatformsProfile.cs

# - - - - - - - - - - - - - - 
# Controller
# - - - - - - - - - - - - - - 
touch Controllers/PlatformsController.cs