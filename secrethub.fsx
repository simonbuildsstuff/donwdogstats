#r "nuget: SecretHub"
#r "nuget: Http.fs, 5.4.0"
#r "nuget: FSharp.Data"

open Hopac
open HttpFs.Client

let shclient = new SecretHub.Client()

let secret = shclient.Resolve("secrethub://simonbuildsstuff/downdog/cookiecreds")


let body =
  Request.createUrl Post "https://www.downdogapp.com/json/history"
  |> Request.setHeader (ContentType (ContentType.parse "application/x-www-form-urlencoded").Value)
  |> Request.setHeader (Custom ("Cookie", secret))
  |> Request.responseAsString
  |> run

printfn "Here's the body: %s" body