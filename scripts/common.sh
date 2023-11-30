#!/usr/bin/env bash

APPLICATION_NAME="rfidreader"
APPLICATION_PATH="/var/www/$APPLICATION_NAME"

WEB_USER="www-data"

FRONTEND_PATH_SUFFIX="/frontend"
FRONTEND_PRODUCTION_PATH_SUFFIX="$FRONTEND_PATH_SUFFIX/production"
BACKEND_PATH_SUFFIX="/backend"

RUN_PATH="/var/run/$APPLICATION_NAME"
LOG_PATH="/var/log/$APPLICATION_NAME"

KILL="kill -9"
RM="rm -rf"
MKDIR="mkdir -p"
CHOWN="chown -R"
PM2="pm2"

$MKDIR /var/www/.pm2
$MKDIR /var/www/.npm
$CHOWN $WEB_USER:$WEB_USER /var/www/.pm2
$CHOWN $WEB_USER:$WEB_USER /var/www/.npm
