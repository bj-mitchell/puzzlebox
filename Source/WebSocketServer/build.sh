#!/bin/bash

cd $(dirname $0)
BASEDIR=$(pwd)

dotnet publish --configuration Release
