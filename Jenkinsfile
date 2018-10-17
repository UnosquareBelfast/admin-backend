pipeline {
    agent {
        node {
            label 'master'
        }
    }
    environment {
        ECR_REPO = '575178181231.dkr.ecr.eu-west-1.amazonaws.com'
        ECR_VERSION = '0.0.0' // SEMVAR [MAJOR][MINOR][PATCH]
    }
    // parameters {
    //     string(name: 'PERSON', defaultValue: 'Mr Jenkins', description: 'Who should I say hello to?')
    //     text(name: 'BIOGRAPHY', defaultValue: '', description: 'Enter some information about the person')
    //     booleanParam(name: 'TOGGLE', defaultValue: true, description: 'Toggle this value')
    //     choice(name: 'CHOICE', choices: ['One', 'Two', 'Three'], description: 'Pick something')
    //     password(name: 'PASSWORD', defaultValue: 'SECRET', description: 'Enter a password')
    //     file(name: "FILE", description: "Choose a file to upload")
    // }
    // triggers {
    //     cron('40 17 * * *')
    // }
    stages {
        // stage('Get Repo') {
        //     steps {
        //         checkout([
        //             $class: 'GitSCM', 
        //             branches: [[name: 'ops/jenkins']], 
        //             userRemoteConfigs: [[url: 'https://github.com/UnosquareBelfast/admin-backend']]])
        //     }
        // }
        stage('Put Dockerfile into useable place'){
            steps {
                sh "cat ./docker/admin.core.Dockerfile > ./Dockerfile"
            }
        }
        stage('Docker build the image') {
            steps {
                sh "docker build -t ${env.ECR_REPO}/admincore:${env.ECR_VERSION}.${env.BUILD_ID} ."
                sh "docker tag ${env.ECR_REPO}/admincore:${env.ECR_VERSION}.${env.BUILD_ID} ${env.ECR_REPO}/admincore:latest"
                // script
                // {
                //     // // Build the docker image using a Dockerfile
                //     // // docker.build("${env.ECR_REPO}/admincore:${env.ECR_VERSION}.${env.BUILD_ID}","-f admin.core.Dockerfile ./docker")
                //     docker.build("${env.ECR_REPO}/admincore:${env.ECR_VERSION}.${env.BUILD_ID}")
                //     docker.build("${env.ECR_REPO}/admincore:latest")
                // }
            }
        }
        stage('Docker push repo') {
            steps {
                echo 'docker push unosquare.aws.repo/admincore:0.0.0'
                script {
                    docker.withRegistry('https://575178181231.dkr.ecr.eu-west-1.amazonaws.com','ecr:eu-west-1:uno-aws-global-creds')
                    {
                        docker.image("${env.ECR_REPO}/admincore:${env.ECR_VERSION}.${env.BUILD_ID}").push()
                        docker.image("${env.ECR_REPO}/admincore:latest").push()
                    }
                }
            }
        }
    }

    post {
        always {
            // Maybe cleanup -- 
            // sh "if [[ `docker images | grep 'dkr.ecr' | wc -l` -gt 2 ]]; then echo \"GT\"; fi"
            // sh "docker images | grep 'dkr.ecr' | sed -n '2p' | awk '{print $3}'"
            // docker rmi -f "${env.ECR_REPO}/admincore:${env.ECR_VERSION}.${env.BUILD_ID}"
            // docker rmi -f `docker images | grep 'ecr' | awk '{print $3}'`
            // docker rmi -f `docker images | grep '<none>' | awk '{print $3}'`
            echo "Done ${BUILD_ID - 1}"
        }
    }
}