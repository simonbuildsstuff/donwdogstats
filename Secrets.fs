module Secrets

let secretHubClient = new SecretHub.Client()

let resolveSecret secretPath = secretHubClient.Resolve(secretPath)
