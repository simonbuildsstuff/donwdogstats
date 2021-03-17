#r "nuget: SecretHub"
#r "nuget: Http.fs, 5.4.0"
#r "nuget: FSharp.Data"
#r "nuget: NodaTime"

open Hopac
open HttpFs.Client
open FSharp.Data
open System.IO
open NodaTime

(*
    SecretHub: Get Cookie Credentials for DownDog
*)
let secretHubClient = new SecretHub.Client()

let secret =
    secretHubClient.Resolve("secrethub://simonbuildsstuff/downdog/cookiecreds")

(*
    Download DownDog History
*)
let body =
    Request.createUrl Post "https://www.downdogapp.com/json/history"
    |> Request.setHeader (
        ContentType
            (ContentType.parse "application/x-www-form-urlencoded")
                .Value
    )
    |> Request.setHeader (Custom("Cookie", secret))
    |> Request.responseAsString
    |> run


[<Literal>]
let fileName = "downdoghistory.json"

File.WriteAllText(fileName, body)

(*
    Get Typed Version of DownDog History with Typeprovider
*)
type DownDogHistory = JsonProvider<fileName, SampleIsList=true, ResolutionFolder=__SOURCE_DIRECTORY__>

let doc = DownDogHistory.GetSamples()

for item in doc do
    for innerItem in item.Items do
        printfn $"SequenceId: {innerItem.SequenceId}"

        printfn
            $"Timestamp: {
                              Instant
                                  .FromUnixTimeSeconds(int64 (floor innerItem.Timestamp.Seconds))
                                  .ToDateTimeUtc()
            }"
