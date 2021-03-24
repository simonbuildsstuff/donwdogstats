module Secrets

open SecretHub

let secretHubClient = new Client()

let resolveSecret secretPath = secretHubClient.Resolve(secretPath)
