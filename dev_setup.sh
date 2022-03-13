# ===========================
# Platform Service
# ===========================
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
# root ➜ /workspaces/dotnet-on-k8s/PlatformService (main ✗) $ dotnet run
# Building...
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

# - - - - - - - - - - - - - - 
# Docker
# - - - - - - - - - - - - - - 
cd /workspaces/dotnet-on-k8s/PlatformService
touch Dockerfile

# Build the image
docker build -t mdrrakiburrahman/platformservice .

# Run image in detached mode
docker run -p 8080:80 -d mdrrakiburrahman/platformservice

# Stop/Start
docker stop 3af6c8352406
docker start 3af6c8352406

# Login to docker with access token
docker login --username=mdrrakiburrahman --password=$DOCKERHUB_TOKEN

# Push to Docker Hub
docker push mdrrakiburrahman/platformservice

# - - - - - - - - - - - - - - 
# Kubernetes
# - - - - - - - - - - - - - -
cd /workspaces/dotnet-on-k8s
mkdir K8S
touch K8S/platforms-deply.yaml

# Create a deployment
kubectl apply -f K8S/platforms-depl.yaml

# ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~

# ===========================
# Commands Service
# ===========================
cd /workspaces/dotnet-on-k8s
dotnet new webapi -n CommandsService
cd /workspaces/dotnet-on-k8s/CommandsService

# Remove weather nonsense
rm -rf WeatherForecast.cs
rm -rf Controllers/WeatherForecastController.cs

# Add dependencies
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection -v 8.1.1
dotnet add package Microsoft.EntityFrameworkCore -v 5.0.8
dotnet add package Microsoft.EntityFrameworkCore.Design -v 5.0.8
dotnet add package Microsoft.EntityFrameworkCore.InMemory -v 5.0.8

# Test out
dotnet dev-certs https # Build a developer certificate
dotnet run

# - - - - - - - - - - - - - - 
# Controller
# - - - - - - - - - - - - - -
# Let's first get the service to talk to our PlatformService synchronously -  we will add Models etc later

# We will have 2 controllers:
# 1. PlatformsController
# 2. CommandsController

cd /workspaces/dotnet-on-k8s/CommandsService/Controllers
touch PlatformsController.cs

# Now let's create comms between Platform and Command - synchronous POST to start with
cd /workspaces/dotnet-on-k8s/PlatformService
mkdir SyncDataServices
mkdir SyncDataServices/Http # We will be using Http "Client Factory" that can handle best practices for Http stuff

cd SyncDataServices/Http
touch ICommandDataClient.cs
touch HttpCommandDataClient.cs

# Trust cert for Command and run both services
dotnet dev-certs https --clean
dotnet dev-certs https --trust

cd /workspaces/dotnet-on-k8s/CommandsService # Terminal 1
dotnet run

cd /workspaces/dotnet-on-k8s/PlatformService # Terminal 2
dotnet run

# - - - - - - - - - - - - - - 
# Dockerfile: Command Service
# - - - - - - - - - - - - - -
touch Dockerfile

# Build Command Service
cd /workspaces/dotnet-on-k8s/CommandsService
docker build -t mdrrakiburrahman/commandservice .
docker push mdrrakiburrahman/commandservice

# Test
docker run -p 8080:80 mdrrakiburrahman/commandservice

# - - - - - - - - - - - - - - - - - -
# Dockerfile: Platform Service Update
# - - - - - - - - - - - - - - - - - -
cd /workspaces/dotnet-on-k8s/PlatformService
touch appsettings.Production.json

# Rebuild Platform Service since we changed it
docker build -t mdrrakiburrahman/platformservice .
docker push mdrrakiburrahman/platformservice

# Deployment rolling update - forces image pull
kubectl rollout restart deployment platforms-depl
kubectl rollout restart deployment commands-depl

# - - - - - - - - - - - - - - - - - - - - - 
# NGINX Ingress Controller: AKA API Gateway
# - - - - - - - - - - - - - - - - - - - - - 
# microk8s enable ingress
# We already did it: https://microk8s.io/docs/addon-ingress
cd /workspaces/dotnet-on-k8s/K8S
touch ingress-svc.yaml

# Create ingress
kubectl apply -f ingress-svc.yaml
# ingress.networking.k8s.io/ingress-srv created
# service/ingress created

# Get the LoadBalancer IP that exposes the Nginx Ingress Controller Pod
kubectl get svc -n ingress
# NAME      TYPE           CLUSTER-IP       EXTERNAL-IP      PORT(S)                      AGE
# ingress   LoadBalancer   10.152.183.122   172.23.170.202   80:30530/TCP,443:31138/TCP   114s

# So basically:
# 0. We connect via DNS to LB Public IP
# 1. Service in nginx namespace connects to Nginx Controller
# 2. Nginx Controller handles the path based routing to ClusterIP services regardless of what namespace we put stuff in (it is a DaemonSet)
# 3. Routing rules go to Service, which goes to desired Pods

# Edit laptop host file: C:\Windows\System32\drivers\etc\hosts
# 172.23.170.202 acme.com

# Or container host file: /etc/hosts
echo '172.23.170.202 acme.com' >> /etc/hosts

# Test from in the VS Container
curl http://acme.com/api/platforms
# [{"id":1,"name":"Dot Net","publisher":"Microsoft","cost":"Free"},{"id":2,"name":"SQL Server Express","publisher":"Microsoft","cost":"Free"},{"id":3,"name":"Kubernetes","publisher":"Cloud Native Computing Foundation","cost":"Free"},{"id":4,"name":"Docker","publisher":"Docker","cost":"Free"},{"id":5,"name":"Docker","publisher":"Docker","cost":"Free"}]

# If we tail the logs of the Nginx Controller we see the requests as the flow in!

# - - - - - - -
# SQL on Linux
# - - - - - - -
cd /workspaces/dotnet-on-k8s/K8S

# Create SQL SA Secret
kubectl create secret generic mssql --from-literal=SA_PASSWORD=$SQL_PASSWORD

# Create SQL Pod
kubectl apply -f sql-depl.yaml

