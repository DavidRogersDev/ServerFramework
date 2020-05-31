pipeline {
    agent any

    stages {
        stage("Verfiy branch") {
            steps {
                echo "$GIT_BRANCH"
                powershell(script: 'Write-Host "This happened"' )
            }            
        }
    }
}