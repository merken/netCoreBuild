docker build . -t netcorebuild:latest
docker run -p 8080:8080 --rm --name netcorebuild netcorebuild:latest

wget http://localhost:8080/jnlpJars/jenkins-cli.jar
java -jar jenkins-cli.jar -s http://localhost:8080 who-am-i
java -jar jenkins-cli.jar -s http://localhost:8080 create-job netcorebuild < /usr/share/jenkins/ref/pipelineTemplate.xml

dotnet build /var/jenkins_home/workspace/netcorebuild@script/Merken.HereYouGo.ABuildServer/Merken.HereYouGo.ABuildServer.csproj
dotnet run /var/jenkins_home/workspace/netcorebuild@script/Merken.HereYouGo.ABuildServer/bin/debug/netcoreapp1.1/Merken.HereYouGo.ABuildServer.dll