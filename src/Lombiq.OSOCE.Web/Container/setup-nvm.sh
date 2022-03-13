#!/bin/bash

cd /home/user
source .bashrc
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.1/install.sh | bash

source .bashrc
nvm install 16

proxy-nvm-command node npm

npm install -g gulp pnpm
