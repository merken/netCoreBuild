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

            stage ("deploy app") {
				deploy_app()
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
		sh(script: 'dotnet publish Merken.NetCoreBuild.Test.csproj', returnStdout: true)
	}
}

def deploy_app(){
	dir('Merken.NetCoreBuild.App/bin/Debug/netcoreapp2.0/publish') {
		sh(script: 'dotnet Merken.NetCoreBuild.Test.dll', returnStdout: false)
	}
}