#this is the base image we use to create our image from
FROM jenkins:2.60.1
#just info about who created this
MAINTAINER Maarten Merken
#get rid of admin password setup
ENV JAVA_OPTS="-Djenkins.install.runSetupWizard=false"
#automatically installing all plugins
COPY plugins.txt /usr/share/jenkins/ref/plugins.txt 
RUN /usr/local/bin/install-plugins.sh bouncycastle-api cloudbees-folder structs junit antisamy-markup-formatter pam-auth windows-slaves display-url-api mailer ldap token-macro external-monitor-job icon-shim matrix-auth script-security matrix-project build-timeout credentials workflow-step-api plain-credentials credentials-binding timestamper scm-api workflow-api workflow-support durable-task workflow-durable-task-step resource-disposer ws-cleanup pipeline-milestone-step jquery-detached ace-editor workflow-scm-step workflow-cps pipeline-input-step pipeline-stage-step workflow-job pipeline-graph-analysis pipeline-rest-api handlebars momentjs pipeline-stage-view pipeline-build-step pipeline-model-api pipeline-model-extensions ssh-credentials git-client git-server workflow-cps-global-lib branch-api workflow-multibranch authentication-tokens docker-commons docker-workflow pipeline-stage-tags-metadata pipeline-model-declarative-agent workflow-basic-steps pipeline-model-definition workflow-aggregator jackson2-api github-api git github github-branch-source pipeline-github-lib ssh-slaves javadoc maven-plugin dashboard-view build-name-setter ant run-condition conditional-buildstep parameterized-trigger jquery build-pipeline-plugin xunit

USER root
# .NET Core runtime deps https://github.com/dotnet/dotnet-docker/blob/master/2.0/runtime-deps/stretch/Dockerfile
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        ca-certificates \
        \
# .NET Core dependencies
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

# .NET Core runtime https://github.com/dotnet/dotnet-docker/blob/master/2.0/runtime/stretch/Dockerfile
RUN apt-get update \
	&& apt-get install -y --no-install-recommends \
    curl \
    && rm -rf /var/lib/apt/lists/*

# Install .NET Core
ENV DOTNET_VERSION 2.0.0-preview2-25407-01
ENV DOTNET_DOWNLOAD_URL https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-x64.tar.gz
ENV DOTNET_DOWNLOAD_SHA 72AA9ABAE8BC818EEB742AFFB943D6AF33B009F84696E6DE7C5316B8975FCC9B4303CB0B80652727BB1FFEE1A025E0C7A43CFE1771D11FE6DEE2D36B002A7EDC

RUN curl -SL $DOTNET_DOWNLOAD_URL --output dotnet.tar.gz \
    && echo "$DOTNET_DOWNLOAD_SHA dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet