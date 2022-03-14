# Linux Container for Lombiq's Open-Source Orchard Core Extensions

First install [VcXsrv](https://sourceforge.net/projects/vcxsrv/). Once launched, open a `cmd` and type "ipconfig /all". Locate the "Hyper-V Virtual Ethernet Adapter" and set the IP as the value of the `DISPLAY_IP`, like below:

```shell
cd path\to\Open-Source-Orchard-Core-Extensions
docker build --build-arg DISPLAY_IP=192.168.80.1 -t lombiq/osoce .
docker run --name osoce -it --rm -p 5000:5000 -p 5001:5001 osoce
```

Verify that the X Display redirection works:
```shell
chromium --no-sandbox
```
Chromium should launch in a new window.

Run the web project:
```shell
dotnet run --project src/Lombiq.OSOCE.Web/Lombiq.OSOCE.Web.csproj
```

Or execute the tests:
```shell
dotnet test --project src/Lombiq.OSOCE.Web/Lombiq.OSOCE.Web.csproj
```
