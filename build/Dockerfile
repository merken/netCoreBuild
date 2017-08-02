#https://dzone.com/articles/dockerizing-jenkins-2-setup-and-using-it-along-wit
ARG JENKINS_VERSION=2.60.2
#this is the base image we use to create our image from
FROM jenkins:$JENKINS_VERSION
LABEL name "netcorebuild"
#just info about who created this
MAINTAINER Maarten Merken

ARG NET_CORE_APP_NAME=netcorebuild
ARG JENKINS_CLI_URL=https://repo.jenkins-ci.org/public/org/jenkins-ci/main/cli/$JENKINS_VERSION/cli-$JENKINS_VERSION-jar-with-dependencies.jar
#get rid of admin password setup
ENV JAVA_OPTS="-Djenkins.install.runSetupWizard=false"
ENV JENKINS_HOME /var/jenkins_home
ENV ASPNETCORE_URLS http://+:5000

RUN echo "Jenkins version $JENKINS_VERSION"
RUN echo "App name $NET_CORE_APP_NAME"
RUN echo "Jenkins CLI $JENKINS_CLI_URL"
RUN echo "Jenkins home $JENKINS_HOME"
RUN echo "NETCORE URL $ASPNETCORE_URLS"

#networking
EXPOSE 5000

#elevation
USER root

#install sudo
RUN apt-get update && apt-get install -y --no-install-recommends \
		sudo 
	&& rm -rf /var/lib/apt/lists/*

#add jenkins user to sudo group
RUN adduser jenkins sudo

#download jenkins cli
RUN echo $JENKINS_CLI_URL
RUN wget $JENKINS_CLI_URL -O jenkins-cli.jar

#create the job directory and configuration
RUN mkdir -p "$JENKINS_HOME"/jobs/"$NET_CORE_APP_NAME"
COPY "$NET_CORE_APP_NAME".xml "$JENKINS_HOME"/jobs/"$NET_CORE_APP_NAME"/config.xml

#automatically installing required plugins
COPY plugins_.txt /usr/share/jenkins/plugins.txt 
RUN /usr/local/bin/install-plugins.sh $(cat /usr/share/jenkins/plugins.txt | tr '\n' ' ')

#install systemctl replacement to run services on stretch https://github.com/gdraheim/docker-systemctl-replacement
COPY systemctl.py /usr/bin/systemctl

#add jenkins to the root user group
#RUN usermod -a -G root jenkins
RUN apt-get update && apt-get install -y --no-install-recommends \
		ca-certificates \
		curl \
		wget \
	&& rm -rf /var/lib/apt/lists/*

#netcore stuff here
#buildpack-deps stretch:curl https://github.com/docker-library/buildpack-deps/blob/master/stretch/curl/Dockerfile
RUN apt-get update && apt-get install -y --no-install-recommends \
		ca-certificates \
		curl \
		wget \
	&& rm -rf /var/lib/apt/lists/*

RUN set -ex; \
	if ! command -v gpg > /dev/null; then \
		apt-get update; \
		apt-get install -y --no-install-recommends \
			gnupg2 \
			dirmngr \
		; \
		rm -rf /var/lib/apt/lists/*; \
	fi

#buildpack-deps stretch:scm https://github.com/docker-library/buildpack-deps/blob/master/stretch/scm/Dockerfile
# procps is very common in build systems, and is a reasonably small package
RUN apt-get update && apt-get install -y --no-install-recommends \
		bzr \
		git \
		mercurial \
		openssh-client \
		subversion \
		\
		procps \
	&& rm -rf /var/lib/apt/lists/*

# Install .NET Core SDK https://github.com/dotnet/dotnet-docker/blob/master/2.0/sdk/stretch/Dockerfile
# Install .NET CLI dependencies
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        libc6 \
        libcurl3 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu57 \
        liblttng-ust0 \
        libssl1.0.2 \
        libstdc++6 \
        libunwind8 \
        libuuid1 \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core SDK
ENV DOTNET_SDK_VERSION 2.0.0-preview2-006497
ENV DOTNET_SDK_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-x64.tar.gz
ENV DOTNET_SDK_DOWNLOAD_SHA 0664FE726EB07650D9C036B0E5E6D33AA8B10DB89DADA4A5E85FB5757FD1FAE1570AF0D526484014976761C829B1E55A70EF9966EECE877A56C2C426090896EB

RUN curl -SL $DOTNET_SDK_DOWNLOAD_URL --output dotnet.tar.gz \
    && echo "$DOTNET_SDK_DOWNLOAD_SHA dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

# Trigger the population of the local package cache
ENV NUGET_XMLDOC_MODE skip
RUN mkdir warmup \
    && cd warmup \
    && dotnet new \
    && cd .. \
    && rm -rf warmup \
    && rm -rf /tmp/NuGetScratch