FROM archlinux
ARG DISPLAY_IP
ARG CHECKOUT=dev
ENV DISPLAY_IP $DISPLAY_IP

# Set up package manager and install required packages.
RUN pacman-key --init
RUN pacman -Sy
RUN pacman -Su --noconfirm
RUN pacman -S --noconfirm bash dotnet-sdk dotnet-sdk-3.1 sudo chromium git curl
SHELL ["/bin/bash", "-c"]

# Set up user with sudo ability.
RUN useradd user
RUN mkdir -p /home/user/.config
WORKDIR /home/user/
RUN usermod -G wheel user
RUN ["/bin/sh", "-c", "echo '%wheel ALL=(ALL:ALL) NOPASSWD: ALL' >> /etc/sudoers"]
COPY setup-nvm.sh /home/user
COPY bashrc.sh /home/user/.bashrc
RUN ["/bin/sh", "-c", "echo 'export DISPLAY=$DISPLAY_IP:0.0' >> .bashrc"]
RUN chown --recursive user:user .
USER user

# Set up NVM, install Gulp and PNPM.
RUN bash setup-nvm.sh

# Check out the repository.
RUN git clone https://github.com/Lombiq/Open-Source-Orchard-Core-Extensions.git
WORKDIR /home/user/Open-Source-Orchard-Core-Extensions/
RUN git checkout $CHECKOUT
RUN git submodule update --init --recursive
RUN dotnet restore
RUN source ~/.bashrc; dotnet build
