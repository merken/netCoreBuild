#build the image
docker build . --no-cache -t netcorebuild:latest
#run the image
docker run -p 8080:8080 -v /var/run/docker.sock:/var/run/docker.sock --name netcorebuild netcorebuild:latest
#enter the image
docker exec -it netcorebuild bash