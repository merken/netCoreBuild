#https://dzone.com/articles/dockerizing-jenkins-2-setup-and-using-it-along-wit
#this is the base image we use to create our image from
FROM jenkins:2.60.1
#just info about who created this
MAINTAINER Maarten Merken
#get rid of admin password setup
ENV JAVA_OPTS="-Djenkins.install.runSetupWizard=false"
#automatically installing all plugins
COPY plugins.txt /usr/share/jenkins/ref/plugins.txt 
COPY pipelineTemplate.xml /usr/share/jenkins/ref/pipelineTemplate.xml
RUN /usr/local/bin/install-plugins.sh bouncycastle-api cloudbees-folder structs junit antisamy-markup-formatter pam-auth windows-slaves display-url-api mailer ldap token-macro external-monitor-job icon-shim matrix-auth script-security matrix-project build-timeout credentials workflow-step-api plain-credentials credentials-binding timestamper scm-api workflow-api workflow-support durable-task workflow-durable-task-step resource-disposer ws-cleanup pipeline-milestone-step jquery-detached ace-editor workflow-scm-step workflow-cps pipeline-input-step pipeline-stage-step workflow-job pipeline-graph-analysis pipeline-rest-api handlebars momentjs pipeline-stage-view pipeline-build-step pipeline-model-api pipeline-model-extensions ssh-credentials git-client git-server workflow-cps-global-lib branch-api workflow-multibranch authentication-tokens docker-commons docker-workflow pipeline-stage-tags-metadata pipeline-model-declarative-agent workflow-basic-steps pipeline-model-definition workflow-aggregator jackson2-api github-api git github github-branch-source pipeline-github-lib ssh-slaves javadoc maven-plugin dashboard-view build-name-setter ant run-condition conditional-buildstep parameterized-trigger jquery build-pipeline-plugin xunit

USER root
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