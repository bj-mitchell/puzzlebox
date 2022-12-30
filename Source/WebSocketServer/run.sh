#!/bin/bash

cd $(dirname $0)
BASEDIR=$(pwd)

./bin/Release/net7.0/WebSocketServer
