#!/usr/bin/env -S dotnet fsi
#r "nuget: AWSSDK.Core"
#r "nuget: AWSSDK.SecretsManager"

open Amazon
open Amazon.SecretsManager
open Amazon.SecretsManager.Model
open System.Threading
open System

let awsSecretManagerClient = new AmazonSecretsManagerClient(RegionEndpoint.EUCentral1)

let secretName = "/downdog/cred"
let mutable secretValueRequest = GetSecretValueRequest()
secretValueRequest.SecretId <- secretName

let asyncSecrets = async {
    let! result = awsSecretManagerClient.GetSecretValueAsync(secretValueRequest, CancellationToken(false)) 
                |> Async.AwaitTask  
    return result
    }

let secretString = (asyncSecrets |> Async.RunSynchronously).SecretString
printfn "%A" secretString
