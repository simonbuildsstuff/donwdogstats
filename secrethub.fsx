#r "nuget: SecretHub"

let shclient = new SecretHub.Client()

let secret = shclient.Resolve("secrethub://simonbuildsstuff/start/hello")

printfn "%s" secret
