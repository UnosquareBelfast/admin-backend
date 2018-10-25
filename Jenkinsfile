pipeline {
    agent {
        node {
            label 'master'
        }
    }
    environment {
    	// AWS_ACCESS_KEY_ID = credentials('AWS_ACCESS_KEY_ID')
		// AWS_SECRET_ACCESS_KEY = credentials('	AWS_SECRET_ACCESS_KEY')
		ECR_VERSION = '0.0.0' // SEMVAR [MAJOR][MINOR][PA
        ENV_TYPE = "prod" // [prod,dev,test]
    }

    stages {
        // stage('Get Repo') {
        //     steps {
        //         checkout([
        //             $class: 'GitSCM', 
        //             branches: [[name: 'ops/deploy']], 
        //             userRemoteConfigs: [[url: 'https://github.com/UnosquareBelfast/admin-backend']]])
        //     }
        // }
        
        stage('Set ENV Vars'){
            steps {
                withAWSParameterStore(credentialsId: 'uno-aws-global-creds', naming: 'relative', path: "/unosquare/project/internal/${env.ENV_TYPE}/", recursive: true, regionName: 'eu-west-1', namePrefixes:'') {
                    script {
                        env.AWS_RDS_POSTGRES_PORT = "${env.PARAM_RDS_RDSDATABASEPORT}"
                        env.AWS_RDS_POSTGRES_HOST = "${env.PARAM_RDS_RDSDATABASEENDPOINT}"
                        env.AWS_RDS_POSTGRES_DB = "${env.PARAM_RDS_RDSDATABASENAME}"
                        env.AWS_RDS_POSTGRES_USERNAME = "${env.PARAM_RDS_RDSDATABASEMASTERUSERNAME}"
                        env.AWS_RDS_POSTGRES_PWD = "${env.PARAM_RDS_RDSDATABASEMASTERUSERPWD}"
                        env.AWS_RDS_POSTGRES_PWD = "${env.PARAM_RDS_RDSDATABASEMASTERUSERPWD}"
                        
                        env.AWS_REPOSITORY_URI = "${env.PARAM_ECR_REPOSITORYURI}"
                        env.AWS_REPOSITORY_NAME = "${env.PARAM_ECR_REPONAME}"
                    }
                }
            }
        }
        stage('Setup props deploy file'){
            steps {
                sh "cat src/main/resources/application.properties.docker.aws.deploy.skel > src/main/resources/application.properties.docker"
                sh "sed -ie 's/{AWS_RDS_POSTGRES_HOST}/'\"${AWS_RDS_POSTGRES_HOST}\"'/' src/main/resources/application.properties.docker"
                sh "sed -ie 's/{AWS_RDS_POSTGRES_PORT}/'\"${AWS_RDS_POSTGRES_PORT}\"'/' src/main/resources/application.properties.docker"
                sh "sed -ie 's/{AWS_RDS_POSTGRES_DB}/'\"${AWS_RDS_POSTGRES_DB}\"'/' src/main/resources/application.properties.docker"
                sh "sed -ie 's/{AWS_RDS_POSTGRES_USERNAME}/'\"${AWS_RDS_POSTGRES_USERNAME}\"'/' src/main/resources/application.properties.docker"
                sh "sed -ie 's/{AWS_RDS_POSTGRES_PWD}/'\"${AWS_RDS_POSTGRES_PWD}\"'/' src/main/resources/application.properties.docker"
                sh "cat src/main/resources/application.properties.docker"
                // sh "exit 1"
            }
        }
        stage('Put Dockerfile into useable place'){
            steps {
                sh "cat ./docker/admin.core.Dockerfile > ./Dockerfile"
            }
        }
        stage('Docker build the image') {
            steps {
                sh "docker build -t ${env.AWS_REPOSITORY_URI}/${env.AWS_REPOSITORY_NAME}:${env.ECR_VERSION}.${env.BUILD_ID} ."
                sh "docker tag ${env.AWS_REPOSITORY_URI}/${env.AWS_REPOSITORY_NAME}:${env.ECR_VERSION}.${env.BUILD_ID} ${env.AWS_REPOSITORY_URI}/${env.AWS_REPOSITORY_NAME}:latest"
            }
        }
        stage('Docker push repo') {
            input {
                message "Should we deploy this docker container?"
            }
            steps {
                script {
                    docker.withRegistry("https://${env.AWS_REPOSITORY_URI}", 'ecr:eu-west-1:uno-aws-global-creds')
                    {
                        docker.image("${env.AWS_REPOSITORY_URI}/${env.AWS_REPOSITORY_NAME}:${env.ECR_VERSION}.${env.BUILD_ID}").push()
                        docker.image("${env.AWS_REPOSITORY_URI}/${env.AWS_REPOSITORY_NAME}:latest").push()
                    }
                }
            }
        }
    }

    post {
        always {
            echo "Done ${BUILD_ID}"
        }
    }
}