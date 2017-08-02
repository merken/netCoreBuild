#build the image
docker build . -t netcorebuild:latest
#run the image
docker run -p 8080:8080 -p 5000:5000 --name netcorebuild netcorebuild:latest
#enter the image
docker exec -it netcorebuild bash

#setup the image
wget http://localhost:8080/jnlpJars/jenkins-cli.jar
java -jar jenkins-cli.jar -s http://localhost:8080 who-am-i
java -jar jenkins-cli.jar -s http://localhost:8080 create-job netcorebuild < /usr/share/jenkins/ref/pipelineTemplate.xml

#test the image
dotnet build /var/jenkins_home/workspace/netcorebuild@script/Merken.NetCoreBuild.App/Merken.NetCoreBuild.App.csproj
dotnet publish /var/jenkins_home/workspace/netcorebuild@script/Merken.NetCoreBuild.App/Merken.NetCoreBuild.App.csproj
dotnet /var/jenkins_home/workspace/netcorebuild@script/Merken.NetCoreBuild.App/bin/Debug/netcoreapp2.0/publish/Merken.NetCoreBuild.App.dll