# netCoreBuild

Hi,

This project started off as a hobby to see the possibilities of having a one-click miniature CI-environment for an aspnet core application.

Build the docker image like so :

```
docker build . -t netcorebuild:latest
```

Afterwards, run the container with port 8080 and 5000 exposed :

```
docker run -p 8080:8080 -p 5000:5000 --name netcorebuild netcorebuild:latest
```

You can create your own jenkins project by copying the build/pipelineTemplate.xml and replacing the tokens in [braces].
Don't forget to include this in the build/dockerfile.

![alt text](https://github.com/merken/netCoreBuild/blob/master/build/jenkins.png)

![alt text](https://github.com/merken/netCoreBuild/blob/master/build/netcoreapp.png)

Cheers,
Maarten