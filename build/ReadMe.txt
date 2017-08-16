#build the image
docker build . --no-cache -t netcorebuild:latest
#run the image
docker run -p 8080:8080 -d -v /var/run/docker.sock:/var/run/docker.sock --name netcorebuild netcorebuild:latest
#enter the image
docker exec -it netcorebuild bash
# Stop the running build container
docker container stop netcorebuild
# Delete all containers
docker rm $(docker ps -a -q)
# Delete all images
docker rmi $(docker images -q)

