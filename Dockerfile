FROM archlinux
ARG DISPLAY_IP
ENV DISPLAY_IP $DISPLAY_IP

ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENV ASPNETCORE_URLS=http://+:5000;http://+:5001
EXPOSE 5000
EXPOSE 5001

# Set up package manager and install required packages.
RUN pacman-key --init
RUN pacman -Syy
RUN pacman -Su --noconfirm
RUN pacman -S --noconfirm bash dotnet-sdk aspnet-runtime sudo xterm git curl dos2unix
SHELL ["/bin/bash", "-c"]

# Set up user environment.
RUN useradd user
RUN mkdir -p /home/user/.config
RUN mkdir -p /home/user/.local/bin
WORKDIR /home/user/
RUN usermod -G wheel user
RUN ["/bin/sh", "-c", "echo '%wheel ALL=(ALL:ALL) NOPASSWD: ALL' >> /etc/sudoers"]
COPY Container/*.sh /home/user/.local/bin
COPY Container/bash.rc /home/user/.bashrc
RUN ["/bin/sh", "-c", "echo 'export DISPLAY=$DISPLAY_IP:0.0' >> .bashrc"]
RUN chown --recursive user:user .
RUN dos2unix /home/user/.local/bin/*
RUN chmod +x /home/user/.local/bin/*
RUN pacman -R dos2unix --noconfirm
USER user
ENV PATH /home/user/.local/bin:/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin

# Set up NVM, install Gulp and PNPM.
RUN setup-nvm.sh

# Set up dev certificate.
RUN dev-cert.sh

# Build solution.
RUN mkdir Open-Source-Orchard-Core-Extensions
COPY --chown=user:user ./ Open-Source-Orchard-Core-Extensions
WORKDIR /home/user/Open-Source-Orchard-Core-Extensions/
RUN source ~/.bashrc; dotnet build
