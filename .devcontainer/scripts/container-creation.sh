#!/usr/bin/env bash

set -e

## build repository, so ready to work. 
dotnet build 

## Add .NET Dev Certs to environment to facilitate debugging
dotnet dev-certs https

exit