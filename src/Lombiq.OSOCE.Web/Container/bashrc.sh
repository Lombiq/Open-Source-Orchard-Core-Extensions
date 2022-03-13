alias cls=clear
export XDG_CONFIG_HOME=/home/user/.config

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
