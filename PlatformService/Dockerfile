# - - - - - - 
# BUILD IMAGE
# - - - - - - 
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
# Inside the container
WORKDIR /app 
# Restore dependencies from Nuget
COPY *.csproj ./ 
RUN dotnet restore
# Copy everything else
COPY . ./
# Build
RUN dotnet publish -c Release -o out

# - - - - - - 
# RUN IMAGE
# - - - - - - 
FROM mcr.microsoft.com/dotnet/aspnet:5.0
# Inside the container
WORKDIR /app
# Copy output from build env to run env
COPY --from=build-env /app/out .

# Entrypoint
ENTRYPOINT ["dotnet", "PlatformService.dll"]