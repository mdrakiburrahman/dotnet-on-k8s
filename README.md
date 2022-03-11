# dotnet-on-k8s
An end-to-end dotnet microservices architecture with k8s, SQL Server on Linux, RabbitMQ and gRPC.

> Built from scratch with: [link](https://www.youtube.com/watch?v=DgVjEo3OGBI)

## Solution Architecture

Overall solution Architecture
![Arch](_images/1.png)

### Service 1: Platform

![Platform Service](_images/4.png)

![Platform Service](_images/2.png)

### Service 2: Command Service

![Command Service](_images/5.png)
![Command Service](_images/3.png)

# Gotchas

If VSCode namespaces aren't found - we need to restart OmniSharp.

`CTRL + SHIFT + P` -> `Restart Omnisharp`. 