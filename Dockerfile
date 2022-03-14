FROM archlinux
ARG DISPLAY_IP
ENV DISPLAY_IP $DISPLAY_IP

# Set up package manager and install required packages.
RUN pacman-key --init
RUN pacman -Sy
RUN pacman -Su --noconfirm
RUN pacman -S --noconfirm bash dotnet-sdk dotnet-sdk-3.1 sudo chromium git curl
SHELL ["/bin/bash", "-c"]

# Set up user environment.
RUN useradd user
RUN mkdir -p /home/user/.config
RUN mkdir -p /home/user/.local/bin
WORKDIR /home/user/
RUN usermod -G wheel user
RUN ["/bin/sh", "-c", "echo '%wheel ALL=(ALL:ALL) NOPASSWD: ALL' >> /etc/sudoers"]
COPY Container/setup-nvm.sh /home/user
COPY Container/bashrc.sh /home/user/.bashrc
RUN ["/bin/sh", "-c", "echo 'export DISPLAY=$DISPLAY_IP:0.0' >> .bashrc"]
RUN chown --recursive user:user .
USER user
ENV PATH /home/user/.local/bin:/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin

# Set up NVM, install Gulp and PNPM.
RUN bash setup-nvm.sh

# Build solution.
RUN mkdir Open-Source-Orchard-Core-Extensions
COPY --chown=user:user ./ Open-Source-Orchard-Core-Extensions
WORKDIR /home/user/Open-Source-Orchard-Core-Extensions/
RUN source ~/.bashrc; dotnet build
