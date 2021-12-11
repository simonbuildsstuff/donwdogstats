namespace Secrets

module SecretHubSecrets =
    
    open SecretHub

    let resolveSecret = 
        let secretHubClient = new Client()
        secretHubClient.Resolve("secrethub://simonbuildsstuff/downdog/cookiecreds")

module AWSSecrets =
    
    open Amazon
    open Amazon.SecretsManager
    open Amazon.SecretsManager.Model
    open System.Threading

    let resolveSecret =  
        
        let awsSecretManagerClient = new AmazonSecretsManagerClient(RegionEndpoint.EUCentral1)
        
        let secretName = "/downdog/cred";
        let mutable secretValueRequest = GetSecretValueRequest()
        secretValueRequest.SecretId <- secretName

        let asyncSecrets = async {
            let! result = awsSecretManagerClient.GetSecretValueAsync(secretValueRequest, CancellationToken(false))
                        |> Async.AwaitTask  
            return result
        } 

        let secretResolved = Async.RunSynchronously(asyncSecrets)
        secretResolved.SecretString
        