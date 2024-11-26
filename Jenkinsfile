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
                                            docker.build("ajovano/ea-front:${env.BUILD_NUMBER}", "-t ajovano/ea-front:latest .")
                                            sh 'docker push -a ajovano/ea-front'
                                        }
                                        sh 'docker rmi -f $(docker image ls --filter "reference=ajovano/ea-front" -q)'
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
                                            docker.build("ajovano/ea-back:${env.BUILD_NUMBER}", "-t ajovano/ea-back:latest .")
                                            sh 'docker push -a ajovano/ea-back'
                                        }
                                    }
                                }
                            }
                        }
                        stage("Back - NUnit - release") {
                            agent {
                                docker {
                                    image 'docker:cli'
                                    args '--privileged -u root -v /var/run/docker.sock:/var/run/docker.sock -v $PWD:/workspace'
                                }
                            }
                            steps {
                                dir('EvidencijaAparata.Tests') {
                                    script {
                                        docker.withRegistry('', 'docker-token') {
                                            docker.build("ajovano/ea-nunit:${env.BUILD_NUMBER}", "-t ajovano/ea-nunit:latest .")
                                            sh 'docker push -a ajovano/ea-nunit'
                                        }
                                        sh 'docker rmi -f $(docker image ls --filter "reference=ajovano/ea-back" -q)'
                                        sh 'docker rmi -f $(docker image ls --filter "reference=ajovano/ea-nunit" -q)'
                                    }
                                }
                            }
                        }
                        stage('Back - NUnit - deploy') {
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
                                        sh "helm upgrade --install ea-nunit charts/ea-nunit --set hostIP=${clusterIP}"
                                        def jobStatus = sh(script: "kubectl wait job ea-nunit --timeout=-1s --for=jsonpath='{.status.conditions[*].status}'=True -o jsonpath='{.status.conditions[*].type}'", returnStdout: true).trim()
                                        def line = sh(script: "kubectl logs jobs/ea-nunit | tail -n 1", returnStdout: true).trim()
                                        sh "helm uninstall ea-nunit"
                                        
                                        if (jobStatus == "Failed") {
                                            error "Doslo je do greske u pokretanju testova."
                                        }
                                        def failed = (line =~ /Failed:\s*(\d+)/)[0][1]
                                        
                                        if (failed.toInteger() != 0) {
                                            error "Neki testovi nisu prosli."
                                        }
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
        stage('e2e') {
            agent { 
                docker { 
                    image 'mcr.microsoft.com/playwright/dotnet:v1.49.0-noble' 
                    args '-u root -v $PWD:/workspace'
                } 
            }
            steps {
                dir('EvidencijaAparata.Playwright') {
                    sh 'dotnet test -- Playwright.ExpectTimeout=60000'
                }
            }
        }
    }
}