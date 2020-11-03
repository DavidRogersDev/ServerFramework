pipeline {
    agent any

    stages {
        stage("Verfiy branch") {
            steps {
                echo "$GIT_BRANCH"
                powershell(script: 'Write-Host "This happened"' )
            }            
        }
        stage('Restore PACKAGES') {
            steps {
                bat "dotnet restore ./KesselRun.Web.Api/KesselRun.Web.Api.csproj"
            }
        }
        stage('Clean') {
            steps {
                bat 'dotnet clean ./KesselRun.Web.Api/KesselRun.Web.Api.csproj'
            }
        }
        stage('Build') {
            steps {
                bat 'dotnet build ./KesselRun.Web.Api/KesselRun.Web.Api.csproj --configuration Release'
            }
        }
        stage('Pack') {
            steps {
                bat 'dotnet publish ./KesselRun.Web.Api/KesselRun.Web.Api.csproj -c Release -o publish'
            }
        }                    
    }
}