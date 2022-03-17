FROM archlinux AS build

# Set up package manager and install required packages.
RUN pacman-key --init
RUN pacman -Syy
RUN pacman -Su --noconfirm
RUN pacman -S --noconfirm dotnet-sdk npm nodejs

# Build solution.
RUN mkdir /source /app
WORKDIR /source
COPY ./ .
RUN mv Directory.Build.props props
RUN head props -n 1 > Directory.Build.props
RUN echo '<PropertyGroup><CopyRefAssembliesToPublishDirectory>true</CopyRefAssembliesToPublishDirectory></PropertyGroup>' >> Directory.Build.props
RUN tail props -n +2 >> Directory.Build.props
RUN dotnet restore
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENV ASPNETCORE_URLS=http://+:5000;http://+:5001
EXPOSE 5000
EXPOSE 5001

COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Lombiq.OSOCE.Web.dll"]
