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

    sh 'whoami'
    def netcorebuildSericeExists = fileExists '/etc/systemd/system/netcorebuild.service'

    if (netcorebuildSericeExists) {
        sh(script: 'systemctl stop netcorebuild.service', returnStdout: true)
        sh(script: 'systemctl disable netcorebuild.service', returnStdout: true)
    } 

    dir('Merken.NetCoreBuild.App') {
        sh(script: 'dotnet publish Merken.NetCoreBuild.App.csproj -o /var/netcorebuild/', returnStdout: true)
    }

    dir('build') {
        sh(script: 'cp netcorebuild.service /etc/systemd/system/netcorebuild.service', returnStdout: true)
    }

    sh(script: 'systemctl enable netcorebuild.service', returnStdout: true)
    
    dir('build') {
        sh 'BUILD_ID=dontKillMe ./netcorebuild.sh'
    }
    //sh(script: 'systemctl start netcorebuild.service', returnStdout: true)
    //sh(script: 'sleep 120', returnStdout: true)
}