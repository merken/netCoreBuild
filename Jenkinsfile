/** Pipeline **/
node {
    ws('netcore') {
        try{
            stage("scm pull") {
				deleteDir()
				cloneRepo()
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
		sh(script: 'dotnet build Merken.NetCoreBuild.App.csproj', returnStdout: true)
	}
}

def dotnet_test(){
	dir('Merken.NetCoreBuild.Test') {
		sh(script: 'dotnet test Merken.NetCoreBuild.Test.csproj', returnStdout: true)
	}
}

def dotnet_publish(){
    dir('Merken.NetCoreBuild.App') {
        sh('echo publishing')
        sh(script: 'dotnet publish Merken.NetCoreBuild.App.csproj -o ./obj/Docker/publish', returnStdout: true)
        sh(script: 'cp Dockerfile ./obj/Docker/publish', returnStdout: true)
        sh('echo zipping')
        sh(script: 'tar zcf netcoreapp.tar.gz -C ./obj/Docker/publish .', returnStdout: true)
        sh('echo building')
        sh(script: 'curl -v -X POST -H "Content-Type:application/tar" --data-binary ''@netcoreapp.tar.gz'' --unix-socket /var/run/docker.sock http:/build?t=sample', returnStdout: true)
    }
}