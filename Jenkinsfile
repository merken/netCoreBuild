import groovy.json.JsonSlurper

VERSION_NUMBER = ""

/** Pipeline **/
node {
    ws('netcore') {
        try{
            stage("scm pull") {
				deleteDir()
				cloneRepo()
                VERSION_NUMBER = getVersionNumber()
                println "Use version $VERSION_NUMBER"
                currentBuild.displayName = "$VERSION_NUMBER"
            }

            stage ("dotnet build") {
				dotnet_build()
            }

            stage ("dotnet test") {
				dotnet_test()
            }

            stage ("dotnet publish") {
				dotnet_publish()
            }

            stage ("docker build") {
				docker_build()
            }

            stage ("docker run") {
				docker_run()
            }
        }
        catch (InterruptedException x) {
            currentBuild.result = 'ABORTED'
            throw x
        }
        catch (e) {
            currentBuild.result = 'FAILURE'
            throw e
        }
    }
}

def cloneRepo() {
    checkout scm
}

def dotnet_build(){
	dir('Merken.NetCoreBuild.App') {
		//sh(script: 'dotnet build Merken.NetCoreBuild.App.csproj', returnStdout: true)
	}
}

def dotnet_test(){
	dir('Merken.NetCoreBuild.Test') {
		//sh(script: 'dotnet test Merken.NetCoreBuild.Test.csproj', returnStdout: true)
	}
}

def dotnet_publish(){
    dir('Merken.NetCoreBuild.App') {
        sh(script: 'dotnet publish Merken.NetCoreBuild.App.csproj -o ./obj/Docker/publish', returnStdout: true)
        sh(script: 'cp Dockerfile ./obj/Docker/publish', returnStdout: true)
        sh(script: 'tar zcf netcoreapp.tar.gz -C ./obj/Docker/publish .', returnStdout: true)
    }
}

def docker_build(){
    dir('Merken.NetCoreBuild.App') {
        dockerApiRequest('containers/netcoreapp/stop', 'POST')
        dockerApiRequest('containers/prune', 'POST')
        dockerApiRequest('images/netcoreapp', 'DELETE')
        dockerApiRequest('build?t=netcoreapp:' + VERSION_NUMBER + '&nocache=1&rm=1', 'POST', 'tar','', '@netcoreapp.tar.gz', true)

        //sh(script: 'curl -v -X POST -H "Content-Type:application/json" --unix-socket /var/run/docker.sock http://0.0.0.0:2375/containers/netcoreapp/stop', returnStdout: true)
        //sh(script: 'curl -v -X POST -H "Content-Type:application/json" --unix-socket /var/run/docker.sock http://0.0.0.0:2375/containers/prune', returnStdout: true)
        //sh(script: 'curl -v -X DELETE -H "Content-Type:application/json" --unix-socket /var/run/docker.sock http://0.0.0.0:2375/images/netcoreapp', returnStdout: true)

        //sh(script: 'curl -v -X POST -H "Content-Type:application/x-tar" --data-binary @netcoreapp.tar.gz --dump-header - --no-buffer --unix-socket /var/run/docker.sock "http://0.0.0.0:2375/build?t=netcoreapp:' + VERSION_NUMBER + '&nocache=1&rm=1"', returnStdout: true)
    }
}

def docker_run(){
    dir('Merken.NetCoreBuild.App') {
        sh('echo \'{ "Image": "netcoreapp:' + VERSION_NUMBER + '", "ExposedPorts": { "5000/tcp" : {} }, "HostConfig": { "PortBindings": { "5000/tcp": [{ "HostPort": "5000" }] } } }\' > imageconf')

        def createResponse = dockerApiRequest('containers/create', 'POST', 'json', 'json', '@imageconf')
        def containerId = createResponse.Id
        println 'containers/' + containerId + '/rename?name=netcoreapp'

        dockerApiRequest('containers/' + containerId + '/rename?name=netcoreapp', 'POST')
        dockerApiRequest('containers/netcoreapp/start', 'POST')

        //def response = sh(script: 'curl -X POST -H "Content-Type:application/json" -H "Accept: application/json" --unix-socket /var/run/docker.sock -d @imageconf http://0.0.0.0:2375/containers/create', returnStdout: true)
        //def containerId = response.id;
        // sh "echo cosntainer id: $response"
        // def jsonSlurper = new JsonSlurper()
        // def json = jsonSlurper.parseText(response)
        // println json.toString()
        // def containerId = json.Id
        // sh "echo cosntffffainer id: $containerId"
        //sh(script: 'curl -v -X POST -H "Content-Type:application/json" -i -H "Accept: application/json" --unix-socket /var/run/docker.sock -d @imageconf http://0.0.0.0:2375/containers/create', returnStdout: true)
        //sh(script: 'curl -v -X POST -H "Content-Type:application/json" -i -H "Accept: application/json" --unix-socket /var/run/docker.sock http://0.0.0.0:2375/containers/netcoreapp/start', returnStdout: true)
    }
}

//Generates a version number
def getVersionNumber() {
    def out = sh(script: 'git rev-list --count HEAD', returnStdout: true)
    def array = out.split("\\r?\\n")
    def count = array[array.length - 1]

    def commitCount = count.trim()

    return commitCount;
}

def dockerApiRequest(request, method, contenttype = 'json', accept = '', data = '', isDataBinary = false){
    def requestBuilder = 'curl -v -X ' + method + ' --unix-socket /var/run/docker.sock "http://0.0.0.0:2375/' + request + '"'

    if(contenttype == 'json'){
        requestBuilder += ' -H "Content-Type:application/json"'
    }

    if(contenttype == 'tar'){
        requestBuilder += ' -H "Content-Type:application/x-tar"'
    }

    if(accept == 'json'){
        requestBuilder += ' -H "Accept: application/json"'
    }
    
    if(data.trim()){
        if(isDataBinary){
            requestBuilder += ' --data-binary ' + data + ' --dump-header - --no-buffer'
        }else{
            requestBuilder += ' -d ' + data
        }
    }

    def response = sh(script: requestBuilder, returnStdout:true)

    if(accept == 'json'){
        def jsonSlurper = new JsonSlurper()
        def json = jsonSlurper.parseText(response)
        return json;
    }

    return null;
}