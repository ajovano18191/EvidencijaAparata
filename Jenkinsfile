pipeline {

    agent none

    options { 
        skipDefaultCheckout() 
    }

    stages {
        stage('Build and release') {
            parallel {
                stage('Front') {
                    stages {
                        stage('Front - Checkout') {
                            agent any
                            steps {
                                checkout scm
                            }
                        }
                        stage('Front - Build') {
                            agent {
                                docker {
                                    image 'node:alpine'
                                    args '-u root -v $PWD:/workspace'
                                }
                            }
                            steps {
                                dir('evidencijaaparata.client') {
                                    sh '''
                                        npm i
                                        npm run build --source-map=false
                                    '''
                                }
                            }
                        }
                        stage('Front - Release') {
                            agent {
                                docker {
                                    image 'docker:cli'
                                    args '--privileged -u root -v /var/run/docker.sock:/var/run/docker.sock -v $PWD:/workspace'
                                }
                            }
                            steps {
                                dir('evidencijaaparata.client') {
                                    script {
                                        docker.withRegistry('', 'docker-token') {
                                            docker.build("ajovano/ea-front").push()
                                        }
                                        sh 'docker rmi ajovano/ea-front'
                                    }
                                }
                            }
                        }
                    }
                }
                stage('Back') {
                    stages {
                        stage('Back - Checkout') {
                            agent any
                            steps {
                                checkout scm
                            }
                        }
                        stage("Back - Build and release") {
                            agent {
                                docker {
                                    image 'docker:cli'
                                    args '--privileged -u root -v /var/run/docker.sock:/var/run/docker.sock -v $PWD:/workspace'
                                }
                            }
                            steps {
                                dir('EvidencijaAparata.Server') {
                                    script {
                                        docker.withRegistry('', 'docker-token') {
                                            docker.build("ajovano/ea-back").push()
                                        }
                                        sh 'docker rmi ajovano/ea-back'
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        stage('Redeploy') {
            agent {
                docker {
                    image 'dtzar/helm-kubectl'
                    args '-v $PWD:/workspace'
                }
            } 
            steps {
                script {
                    def clusterIP = "10.17.2.37"
                    withKubeConfig([credentialsId: 'dev-api-token', serverUrl: "https://${clusterIP}:16443/"]) {
                        sh "helm upgrade --install ea charts/ea --set hostIP=${clusterIP}"
                    }
                }
            }
        }
    }
}