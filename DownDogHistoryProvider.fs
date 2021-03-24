namespace DownDog

open FSharp.Data
open Hopac
open HttpFs.Client
open Secrets
open System.IO

module FileName =

    [<Literal>]
    let fileName = "downdoghistory.json"

module FileDownloader =

    let secret =
        resolveSecret "secrethub://simonbuildsstuff/downdog/cookiecreds"

    let body =
        Request.createUrl Post "https://www.downdogapp.com/json/history"
        |> Request.setHeader (ContentType(ContentType.create ("application", "x-www-form-urlencoded")))
        |> Request.setHeader (Custom("Cookie", secret))
        |> Request.responseAsString
        |> run


    let downloadFileToDisk =
        File.WriteAllText(FileName.fileName, body)

module JsonTypeProvider =

    type DownDogHistory = JsonProvider<FileName.fileName, SampleIsList=true, ResolutionFolder=__SOURCE_DIRECTORY__>

    let downDogHistory = DownDogHistory.GetSamples()
    let historyItems = downDogHistory.[0].Items
