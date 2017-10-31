# this is the jenkins version, to upgrade the base image, alter the argument below
ARG JENKINS_VERSION=2.60.3
FROM jenkins:$JENKINS_VERSION
LABEL name "netcorebuild"
MAINTAINER Maarten Merken

ARG NET_CORE_APP_NAME=netcoreapp
ARG JENKINS_CLI_URL=https://repo.jenkins-ci.org/public/org/jenkins-ci/main/cli/$JENKINS_VERSION/cli-$JENKINS_VERSION-jar-with-dependencies.jar
# get rid of admin password setup
ENV JAVA_OPTS="-Djenkins.install.runSetupWizard=false"
ENV JENKINS_HOME /var/jenkins_home

# networking
EXPOSE 8080

# elevation
USER root

# copy plugins to jenkins directory
COPY plugins.txt /usr/share/jenkins/plugins.txt
# create the jenkins job directory and copy the job configuration
RUN mkdir -p "$JENKINS_HOME"/jobs/"$NET_CORE_APP_NAME"
COPY "$NET_CORE_APP_NAME".xml "$JENKINS_HOME"/jobs/"$NET_CORE_APP_NAME"/config.xml

# setup netcore environment
ENV DOTNET_SDK_VERSION 2.0.0
ENV DOTNET_SDK_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-sdk-$DOTNET_SDK_VERSION-linux-x64.tar.gz
ENV DOTNET_SDK_DOWNLOAD_SHA E457F3A5685382F7F24851A2E76EDBE75B575948C8A7F43220F159BA29C329A5008BBE7220C18DFB31EAF0398FC72177B1948B65E19B34ED0D907EFB459CF4B0

# create temporary scripts folder
RUN mkdir -p /var/dockerscripts

# copy build scripts
COPY installsudo.sh /var/dockerscripts/
COPY installcurl.sh /var/dockerscripts/
COPY installscmpackages.sh /var/dockerscripts/
COPY installjenkinscli.sh /var/dockerscripts/
COPY installjenkinsplugins.sh /var/dockerscripts/
COPY installnetcoredependencies.sh /var/dockerscripts/
COPY installnetcoresdk.sh /var/dockerscripts/

# sudo is required for running command in elevation
RUN /bin/bash -c "source /var/dockerscripts/installsudo.sh"
# curl is required for downloading external packagese
RUN /bin/bash -c "source /var/dockerscripts/installcurl.sh"
# # scm packages are required for git/mercurial/svn support in jenkins
RUN /bin/bash -c "source /var/dockerscripts/installscmpackages.sh"
# the jenkins cli might come in handy at some point
RUN /bin/bash -c "source /var/dockerscripts/installjenkinscli.sh"
# install all required jenkins plugins, like pipeline support, etc
RUN /bin/bash -c "source /var/dockerscripts/installjenkinsplugins.sh"
# install the netcore cli dependencies
RUN /bin/bash -c "source /var/dockerscripts/installnetcoredependencies.sh"
# install the netcore sdk, to build, test, run and publish a netcore project
RUN /bin/bash -c "source /var/dockerscripts/installnetcoresdk.sh"

# cleanup of installation scripts
RUN rm -rf /var/lib/apt/lists/*
RUN rm -rf /var/dockerscripts