/** Pipeline **/
node {
    ws('netcore') {
        try{
            stage("SCM Commit") {
				cloneRepo()
            }
            
            stage ("Tests") {

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
    bat([script: "git config --system core.longpaths true"])
    git branch: master, url: 'git@github.com:merken/netCoreBuild.git'
}