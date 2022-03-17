# Linux Container for Lombiq's Open-Source Orchard Core Extensions

This is the readme for the [Docker container](../Dockerfile) that runs in userspace. It's based on [Arch Linux](https://archlinux.org/) so if you have that distibution you can consult it as a template for bare metal setup too.

# Setup

On a Windows host, first install [VcXsrv](https://sourceforge.net/projects/vcxsrv/). Once launched, open a `cmd` and type "ipconfig /all". Locate the "Hyper-V Virtual Ethernet Adapter" and set the IP as the value of the `DISPLAY_IP`. On a Linux host you don't need to install anything, use `ifconfig` to find you local IP.

Assuming that you are in the solution directory:
```shell
docker build --build-arg DISPLAY_IP=192.168.80.1 -t lombiq/osoce .
docker run --name osoce -it --rm -p 5000:5000 -p 5001:5001 lombiq/osoce
```

Verify that the X Display redirection works by typing `chromium`.
Chromium should launch in a new window.

Run the web project:
```shell
cd src/Lombiq.OSOCE.Web/bin/Debug/net6.0
./Lombiq.OSOCE.Web
```

Or execute the tests:
```shell
dotnet test --project src/Lombiq.OSOCE.Web/Lombiq.OSOCE.Web.csproj
```
