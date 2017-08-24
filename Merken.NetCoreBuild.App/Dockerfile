FROM microsoft/aspnetcore
# set up network
ENV ASPNETCORE_URLS http://+:5000
WORKDIR /app
EXPOSE 5000
COPY . /app

ENTRYPOINT ["dotnet", "Merken.NetCoreBuild.App.dll"]