#!/bin/bash
# buildpack-deps stretch:scm https://github.com/docker-library/buildpack-deps/blob/master/stretch/scm/Dockerfile
# procps is very common in build systems, and is a reasonably small package
apt-get update && apt-get install -y --no-install-recommends \
		bzr \
		git \
		mercurial \
		openssh-client \
		subversion \
		procps