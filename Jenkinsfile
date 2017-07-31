/** Pipeline **/
node {
    ws('netcore') {
        try{
            stage("SCM Commit") {
				deleteDir()
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
    checkout scm
}