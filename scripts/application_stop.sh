#!/usr/bin/env bash

source $(dirname $0)/common.sh

sudo -u $WEB_USER $PM2 stop $APPLICATION_NAME-FRONTEND
sudo -u $WEB_USER $PM2 delete $APPLICATION_NAME-FRONTEND
sudo -u $WEB_USER $PM2 stop $APPLICATION_NAME-BACKEND
sudo -u $WEB_USER $PM2 delete $APPLICATION_NAME-BACKEND

$RM $PID_PATH

exit 0
