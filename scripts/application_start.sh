#!/usr/bin/env bash

source $(dirname $0)/common.sh

$MKDIR $RUN_PATH
$MKDIR $LOG_PATH

$CHOWN $WEB_USER:$WEB_USER $RUN_PATH
$CHOWN $WEB_USER:$WEB_USER $LOG_PATH

cd $APPLICATION_PATH$BACKEND_PATH_SUFFIX
sudo -u $WEB_USER SOCK=$RUN_PATH/backend.sock $PM2 start --name $APPLICATION_NAME-BACKEND app.js -o $LOG_PATH/backend.log -e $LOG_PATH/backend.log

cd $APPLICATION_PATH$FRONTEND_PATH_SUFFIX/production
sudo -u $WEB_USER SOCK=$RUN_PATH/frontend.sock $PM2 start --name $APPLICATION_NAME-FRONTEND --interpreter node_modules/@babel/node/bin/babel-node.js index.js -o $LOG_PATH/frontend.log -e $LOG_PATH/frontend.log

exit 0
