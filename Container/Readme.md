# Linux Container for Lombiq's Open-Source Orchard Core Extensions

This is the readme for the [Docker container](../Dockerfile). It uses [Arch Linux](https://archlinux.org/) to build and then the [ASP.NET Core Runtime](https://hub.docker.com/_/microsoft-dotnet-aspnet) to run.

# Usage

Assuming that you are in the solution directory:
```shell
docker build -t lombiq/osoce .
docker run --name osoce -it --rm -p 5000:5000 -p 5001:5001 lombiq/osoce
```
