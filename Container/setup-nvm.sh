#!/bin/bash

cd /home/user
source .bashrc
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.1/install.sh | bash

source .bashrc
nvm install 16

npm install -g gulp pnpm

function proxy-nvm-command() {
    for command in "$@"; do

    cat > ~/.local/bin/$command << DONE
#!/bin/bash

export NVM_DIR="$HOME/.config/nvm"
source "\$NVM_DIR/nvm.sh"
exec $command "\$@"
DONE

    chmod +x ~/.local/bin/$command

    done
}

proxy-nvm-command node npm
