#!/usr/bin/env bash

eval $(docker-machine env uno-belfast-terry-01)

docker-compose down && \
docker-compose build && \
docker-compose up -d


