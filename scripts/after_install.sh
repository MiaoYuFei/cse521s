#!/usr/bin/env bash

source $(dirname $0)/common.sh

chown -R $WEB_USER:$WEB_USER $APPLICATION_PATH
cd $APPLICATION_PATH$BACKEND_PATH_SUFFIX
sudo -u $WEB_USER npm install
cd $APPLICATION_PATH$FRONTEND_PATH_SUFFIX
sudo -u $WEB_USER npm install
sudo -u $WEB_USER npm run build
cd $APPLICATION_PATH$FRONTEND_PRODUCTION_PATH_SUFFIX
sudo -u $WEB_USER npm install

exit 0
