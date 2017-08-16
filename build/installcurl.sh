#!/bin/bash
# buildpack-deps stretch:curl https://github.com/docker-library/buildpack-deps/blob/master/stretch/curl/Dockerfile
apt-get update && apt-get install -y --no-install-recommends \
		ca-certificates \
		curl \
		wget

set -ex; \
	if ! command -v gpg > /dev/null; then \
		apt-get update; \
		apt-get install -y --no-install-recommends \
			gnupg2 \
			dirmngr \
		;
	fi