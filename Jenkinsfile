pipeline {
    agent any

    stages {
        stage("Verfiy branch") {
            steps {
                echo "$GIT_BRANCH"
                pwsh(script: 'Write-Host "This happened"' )
            }            
        }
    }
}