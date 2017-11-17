# netCoreBuild

Hi,

This project started off as a hobby to see the possibilities of having a one-click miniature CI-environment for an aspnet core application.

Build the docker image like so :

```
docker build build\. -t netcorebuild:latest
```

Afterwards, run the container with port 8080 exposed and the docker socket connected:

```
docker run -p 8080:8080 -d -v /var/run/docker.sock:/var/run/docker.sock --name netcorebuild netcorebuild:latest
```

After the build, a new docker image + container will be created on your docker host.

You can create your own jenkins project by copying the build/pipelineTemplate.xml and replacing the tokens in [braces].
Don't forget to include this in the build/dockerfile.

Keep in mind that all bash file line endings must be configured for LF (linux) and not CRLF.

![alt text](https://github.com/merken/netCoreBuild/blob/master/build/jenkins.png)

![alt text](https://github.com/merken/netCoreBuild/blob/master/build/netcoreapp.png)

Cheers,
Maarten